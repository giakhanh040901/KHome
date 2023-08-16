using AutoMapper;
using EPIC.DataAccess.Models;
using EPIC.Entities;
using EPIC.PaymentDomain.Interfaces;
using EPIC.PaymentEntities.DataEntities;
using EPIC.PaymentEntities.Dto;
using EPIC.PaymentEntities.Dto.Pvcb;
using EPIC.PaymentRepositories;
using EPIC.Pvcb.Configs;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Contract;
using EPIC.Utils.ConstantVariables.Core;
using EPIC.Utils.ConstantVariables.Identity;
using EPIC.Utils.Validation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace EPIC.PaymentDomain.Implements
{
    public class PvcbPaymentServices : IPvcbPaymentServices
    {
        private readonly ILogger _logger;
        private readonly string _connectionString;
        private readonly IMapper _mapper;
        private readonly IOptions<PvcbConfiguration> _config;
        private readonly PaymentPvcbCallbackRepository _paymentPVCBCallbackRepository;
        private readonly IHttpContextAccessor _httpContext;

        public PvcbPaymentServices(
            ILogger<PvcbPaymentServices> logger,
            DatabaseOptions databaseOptions,
            IMapper mapper,
            IOptions<PvcbConfiguration> config,
            IHttpContextAccessor httpContext)
        {
            _logger = logger;
            _httpContext = httpContext;
            _config = config;
            _connectionString = databaseOptions.ConnectionString;
            _mapper = mapper;
            _paymentPVCBCallbackRepository = new PaymentPvcbCallbackRepository(_connectionString, _logger);
        }

        public bool VerifyToken(string data, string signature)
        {
            RSACryptoServiceProvider rsaService = new RSACryptoServiceProvider();
            string publicPem = File.ReadAllText(_config.Value.PublicKey);
            // xử lý chuỗi file publicKey.pem
            Regex publicRSAKeyRegex = new Regex(@"-----(BEGIN|END) PUBLIC KEY-----[\W]*");
            var publicKeyPem = publicRSAKeyRegex.Replace(publicPem, "").Replace("\n", "").Replace("\r", "");
            //_logger.LogInformation($"Public Key Pem: {publicKeyPem}");
            try
            {
                byte[] rsaPublicKeyBytes = Convert.FromBase64String(publicKeyPem);
                rsaService.ImportSubjectPublicKeyInfo(rsaPublicKeyBytes, out _);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi cấu hình PublicKey");
                throw new JsonException("Lỗi cấu hình PublicKey");
            }
            // chuyển đổi kiểu dữ liệu của data và signature tương thích cho đầu vào hàm VerifyData
            UnicodeEncoding ByteConverter = new UnicodeEncoding();
            byte[] dataToEncrypt = ByteConverter.GetBytes(data);
            signature = Convert.ToBase64String(ByteConverter.GetBytes(signature));
            bool isVerify = false; // set giá trị mặc định
            try
            {
                isVerify = rsaService.VerifyData(dataToEncrypt, Convert.FromBase64String(signature), HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Chữ ký không hợp lệ");
                throw new JsonException("Chữ ký không hợp lệ");
            }
            if (isVerify == false)
            {
                throw new JsonException("Xác thực không thành công, chữ ký không chính xác");
            }
            return isVerify;
        }

        public PvcbCallbackDto PvcbCallbackAdd(string data, string token)
        {
            _logger.LogInformation($"Parameter values: data = {data}, token = {token}");
            CreatePvcbCallbackDto callBackData = null;
            try
            {
                callBackData = JsonSerializer.Deserialize<CreatePvcbCallbackDto>(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "không thể Deserialize data");
                throw new JsonException("không thể Deserialize data");
            }

            // Lưu data vào bảng EP_PVCB_PAYMENT_CALLBACK
            var entity = _paymentPVCBCallbackRepository.Add(new PvcbCallback
            {
                FtType = callBackData.FtType,
                Amount = callBackData.Amount,
                Balance = callBackData.Balance,
                SenderBankId = callBackData.SenderBankId,
                Description = callBackData.Description,
                TranId = callBackData.TranId,
                TranDate = callBackData.TranDate,
                Currency = callBackData.Currency,
                TranStatus = callBackData.TranStatus,
                ConAmount = callBackData.ConAmount,
                NumberOfBeneficiary = callBackData.NumberOfBeneficiary,
                Account = callBackData.Account,
                RequestIP = CommonUtils.GetCurrentRemoteIpAddress(_httpContext),
                Token = token
            });

            var desc = callBackData.Description;
            Regex rx = new Regex(RegexPatterns.ContractCode, RegexOptions.Compiled | RegexOptions.IgnoreCase);
            string contractCode = rx.Match(desc).Value;

            // Ép kiểu int để lấy ra Id
            int orderId = int.Parse(contractCode.Substring(2));

            // Xử lý thêm bank BondOrder
            var tradingProviderId = CommonUtils.GetCurrentTradingFilter(_httpContext, WhiteListIpTypes.TRADING_KEY);
            string code = contractCode.Substring(0, 2);
            if (code == ContractCodes.BOND)
            {
                try
                {
                    var bankInfo = _paymentPVCBCallbackRepository.CreateBondPayment(new CreateBankOrderPaymentDto
                    {
                        OrderId = orderId,
                        Amount = callBackData.Amount,
                        Account = callBackData.Account,
                        TranDate = callBackData.TranDate,
                        Description = callBackData.Description
                    }, tradingProviderId);
                }
                catch (Exception ex)
                {
                    string message = $"Mã đại lý sơ cấp của địa chỉ Ip không đúng - tradingProviderId {tradingProviderId}";
                    _logger.LogError(ex, message);
                    throw new JsonException(message);
                }
            }
            else if (code == ContractCodes.INVEST)
            {
                try
                {
                    var bankInfo = _paymentPVCBCallbackRepository.CreateInvestPayment(new CreateBankOrderPaymentDto
                    {
                        OrderId = orderId,
                        Amount = callBackData.Amount,
                        Account = callBackData.Account,
                        TranDate = callBackData.TranDate,
                        Description = callBackData.Description
                    }, tradingProviderId);
                }
                catch (Exception ex)
                {
                    string message = $"Tài khoản TradingProviderId {tradingProviderId} không đúng";
                    _logger.LogError(ex, message);
                    throw new JsonException(message);
                }
            }

            // check lại nếu orderId đã đổi trạng thái thành đang đầu tư thì xuất file hợp đồng
            //var orderData = _paymentPVCBCallbackRepository.FindOrderById(orderId);
            //var orderStatus = orderData.Status;

            return _mapper.Map<PvcbCallbackDto>(entity);
        }

        public PagingResult<CallbackDataDto> FindAll(int pageSize, int pageNumber, string keyword, string status)
        {
            var result = _paymentPVCBCallbackRepository.FindAll(pageSize, pageNumber, keyword, status);
            return result;
        }

        public string PvcbCallbackEncode(CreatePvcbCallbackDto input)
        {
            var data = new CreatePvcbCallbackDto
            {
                FtType = input.FtType,
                Amount = input.Amount,
                Balance = input.Balance,
                SenderBankId = input.SenderBankId,
                Description = input.Description,
                TranId = input.TranId,
                TranDate = input.TranDate,
                Currency = input.Currency,
                TranStatus = input.TranStatus,
                ConAmount = input.ConAmount,
                NumberOfBeneficiary = input.NumberOfBeneficiary,
                Account = input.Account,
            };

            string jsonString = JsonSerializer.Serialize(data);
            return jsonString;
        }
    }
}
