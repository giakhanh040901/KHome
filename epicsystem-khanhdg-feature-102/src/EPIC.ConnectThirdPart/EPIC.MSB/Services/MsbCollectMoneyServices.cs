using EPIC.MSB.Configs;
using EPIC.MSB.Dto.Notification;
using EPIC.MSB.Dto.CollectMoney;
using EPIC.MSB.Dto.Shared;
using EPIC.Utils.Net.MimeTypes;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using EPIC.MSB.ConstVariables;
using DocumentFormat.OpenXml.Office2010.Excel;
using EPIC.Utils;
using System.ServiceModel;
using System.Net;
using EPIC.Utils.Security;
using Microsoft.AspNetCore.Hosting;
using EPIC.Utils.ConstantVariables.Shared;

namespace EPIC.MSB.Services
{
    /// <summary>
    /// Msb thu hộ
    /// </summary>
    public class MsbCollectMoneyServices : MsbServicesBase
    {
        //private const string CREATE_VIRTUAL_ACCOUNT = "/v1/api/msb/acq-hub/payment-service/va-process/createVA";
        //private const string UPDATE_VIRTUAL_ACCOUNT = "/v1/api/msb/acq-hub/payment-service/va-process/updateVA";
        private static string CREATE_QR = "/v1/api/msb/acq-hub/create/viet-qr-process/createQrCode";
        private static string CREATE_VIRTUAL_ACCOUNT = "/msb_pay_create_va/1.0.0";
        private static string UPDATE_VIRTUAL_ACCOUNT = "/msb_pay_updave_va/1.0.0";
        private static string HISTORY_TRANSACTION = "/msb_pay_va_find_transaction/1.0.0";
        public MsbCollectMoneyServices(ILogger<MsbCollectMoneyServices> logger, IOptions<MsbConfiguration> config, IWebHostEnvironment env) : base(logger, config, env)
        {
            if (EnvironmentNames.DevelopEnv.Contains(env.EnvironmentName))
            {
                CREATE_VIRTUAL_ACCOUNT = "/v1/api/msb/acq-hub/payment-service/va-process/createVA";
                UPDATE_VIRTUAL_ACCOUNT = "/v1/api/msb/acq-hub/payment-service/va-process/updateVA";
            }
        }

        /// <summary>
        /// Nếu có tài khoản msb thì tạo VA để chuyển khoản
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private async Task<ResultCreateVADto> CreateVirtualAccount(CreateVADto input)
        {
            _logger.LogInformation($"{nameof(CreateVirtualAccount)}: input = {JsonSerializer.Serialize(input)}");
            string accountNumber = $"{input.PrefixAccount}{input.OrderCode}";
            var item = new CreateVARowDto
            {
                AccountNumber = accountNumber,
                ReferenceNumber = input.OrderCode,
                Name = input.BankAccountName,
                PayType = MsbPayTypes.KhongGioiHanSoLan,
                Detail1 = input.Note,
                ExpiryDate = DateTime.Now.AddDays(365).ToString("dd'/'MM'/'yyyy HH:mm:ss"),
                Status = MsbVAStatus.Active,
            };
                        
            string json = JsonSerializer.Serialize(new RequestCreateVADto
            {
                TId = input.TId,
                MId = input.MId,
                ServiceCode = input.PrefixAccount,
                TokenKey = await GetAccessTokenAsync(input.TId, input.MId),
                Rows = new List<CreateVARowDto> { item }
            });
            StringContent content = new(json, Encoding.UTF8, MimeTypeNames.ApplicationJson);

            Uri path = new(new Uri(_config.Value.BaseUrl), CREATE_VIRTUAL_ACCOUNT);
            HttpResponseMessage res = null;
            try
            {
                res = await CreateHttpClient().PostAsync(path, content);
                string message = $"{nameof(CreateVirtualAccount)}: Path = {path}, RequestBody = {json}, ResponseCode: {res.StatusCode}, ResponseBody: {await res.Content.ReadAsStringAsync()}";
                if (res.StatusCode != HttpStatusCode.OK)
                {
                    _logger.LogError(message);
                    ThrowException();
                }
                else
                {
                    _logger.LogInformation(message);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"{nameof(CreateVirtualAccount)}: exception: {ex.Message}");
                if (ex.GetType() == typeof(TaskCanceledException))
                {
                    ThrowException("Timeout khi kết nối đến MSB", ErrorCode.HttpRequestTimeOut);
                }
                ThrowException();
            }

            var resBody = await res.Content.ReadFromJsonAsync<MsbResponse<ResponseDomainCreateVADto>>();
            if (resBody.ResponseMessage.RespCode != MsbErrorCodes.VACodeIsExists) // đã tồn tại tài khoản thì bỏ qua
            {
                HandleResponse(resBody.ResponseMessage.RespCode, resBody.ResponseMessage.RespDesc);
            }

            return new ResultCreateVADto
            {
                AccountNumber = accountNumber
            };
        }

        /// <summary>
        /// cập nhật VA
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private async Task<ResultUpdateVADto> UpdateVirtualAccount(string accountNumber, CreateVARowDto itemVA, string TId, string MId, string serviceCode)
        {
            _logger.LogInformation($"{nameof(UpdateVirtualAccount)}: accountNumber = {accountNumber}; itemVA = {JsonSerializer.Serialize(itemVA)}");

            string json = JsonSerializer.Serialize(new RequestCreateVADto
            {
                TId = TId,
                MId = MId,
                ServiceCode = serviceCode,
                TokenKey = await GetAccessTokenAsync(TId, MId),
                Rows = new List<CreateVARowDto> { itemVA }
            });
            StringContent content = new(json, Encoding.UTF8, MimeTypeNames.ApplicationJson);

            Uri path = new(new Uri(_config.Value.BaseUrl), UPDATE_VIRTUAL_ACCOUNT);
            HttpResponseMessage res = null;
            try
            {
                res = await CreateHttpClient().PostAsync(path, content);
                string message = $"{nameof(UpdateVirtualAccount)}: Path = {path}, RequestBody = {json}, ResponseCode: {res.StatusCode}, ResponseBody: {await res.Content.ReadAsStringAsync()}";
                if (res.StatusCode != HttpStatusCode.OK)
                {
                    _logger.LogError(message);
                    ThrowException();
                }
                else
                {
                    _logger.LogInformation(message);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"{nameof(UpdateVirtualAccount)}: exception: {ex.Message}");
                if (ex.GetType() == typeof(TaskCanceledException))
                {
                    ThrowException("Timeout khi kết nối đến MSB", ErrorCode.HttpRequestTimeOut);
                }
                ThrowException();
            }
            var resBody = await res.Content.ReadFromJsonAsync<MsbResponse<ResponseDomainCreateVADto>>();
            HandleResponse(resBody.ResponseMessage.RespCode, resBody.ResponseMessage.RespDesc);

            return new ResultUpdateVADto
            {
                AccountNumber = accountNumber
            };
        }

        /// <summary>
        /// Xử lý nhận thông báo khi có thay đổi số dư vào tài khoản VA
        /// </summary>
        public ResultNotificationDto HandleNotification(ReceiveNotificationDto input)
        {
            _logger.LogInformation($"{nameof(HandleNotification)}: input = {JsonSerializer.Serialize(input)}");

            string orderCode = null;
            try
            {
                orderCode = input.VaNumber?.Substring(input.VaNumber.LastIndexOf("E"));
            }
            catch
            {
                throw new FaultException(new FaultReason($"Không lấy được orderCode trong VA Number"), new FaultCode(((int)ErrorCode.BadRequest).ToString()), "");
            }

            // convert DateTime Msb
            string msbDateTime = null;
            if (input.TranDate.Length == 12)
            {
                string msbTranDate = input.TranDate;
                string year = msbTranDate.Substring(0, 2);
                string month = msbTranDate.Substring(2, 2);
                string day = msbTranDate.Substring(4, 2);
                string hour = msbTranDate.Substring(6, 2);
                string minute = msbTranDate.Substring(8, 2);
                string second = msbTranDate.Substring(10, 2);

                msbDateTime = $"20{year}-{month}-{day} {hour}:{minute}:{second}";
            }

            if (!DateTime.TryParse(msbDateTime, out DateTime tranDate))
            {
                throw new Exception($"Không parse được TranDate = {input.TranDate}");
            }

            if (!decimal.TryParse(input.TranAmount, out decimal tranAmount))
            {
                throw new Exception($"Không parse được TranAmount = {input.TranAmount}");
            }

            var result = new ResultNotificationDto
            {
                PrefixAccount = input.VaNumber?.Replace(orderCode, ""),
                OrderCode = orderCode,
                TranDate = tranDate,
                TranAmount = tranAmount
            };
            return result;
        }

        /// <summary>
        /// Xử lý kiểm tra chữ ký signature
        /// </summary>
        /// <param name="input"></param>
        /// <exception cref="FaultException"></exception>
        public void HandleNotificationSignature(ReceiveNotificationDto input, string accessCode)
        {
            _logger.LogInformation($"{nameof(HandleNotificationSignature)}: input = {JsonSerializer.Serialize(input)}");

            //kiểm tra signature
            string hashSign = CryptographyUtils.ComputeSha256Hash(accessCode, input.TranSeq, input.TranDate, input.VaNumber, input.TranAmount, input.FromAccountNumber, input.ToAccountNumber);
            if (input.Signature?.ToLower() != hashSign?.ToLower())
            {
                _logger.LogError($"Chữ ký không hợp lệ: Signature phải là {hashSign}");
                //chữ ký không hợp lệ
                throw new FaultException(new FaultReason($"Chữ ký không hợp lệ"), new FaultCode(((int)ErrorCode.BadRequest).ToString()), "");
            }
        }

        /// <summary>
        /// Tạo Viet qr
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        /// <exception cref="FaultException"></exception>
        public async Task<ResultCreateVietQrDto> CreateVietQr(CreateVietQrDto input)
        {
            _logger.LogInformation($"{nameof(CreateVietQr)}: input = {JsonSerializer.Serialize(input)}");
            RequestCreateVietQrDto request = new()
            {
                TId = input.TId,
                MId = input.MId,
                Amount = input.AmountMoney.ToString(),
                Account = input.BankAccount,
                Remark = input.Note,
                AccountName = input.OwnerAccount
            };
            request.SecureHash = CryptographyUtils.ComputeSha256Hash(input.AccessCode, request.MId, request.TId, request.Amount, request.Account, request.Remark, request.AccountName)?.ToLower();

            string json = JsonSerializer.Serialize(request);
            StringContent content = new(json, Encoding.UTF8, MimeTypeNames.ApplicationJson);

            Uri path = new(new Uri(_config.Value.BaseUrl), CREATE_QR);
            HttpResponseMessage res = null;
            try
            {
                res = await CreateHttpClient().PostAsync(path, content);
                string message = $"{nameof(CreateVietQr)}: Path = {path}, RequestBody = {json}, ResponseCode: {res.StatusCode}, ResponseBody: {await res.Content.ReadAsStringAsync()}";
                if (res.StatusCode != HttpStatusCode.OK)
                {
                    _logger.LogError(message);
                    ThrowException();
                }
                else
                {
                    _logger.LogInformation(message);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"{nameof(CreateVietQr)}: exception: {ex.Message}");
                if (ex.GetType() == typeof(TaskCanceledException))
                {
                    ThrowException("Timeout khi kết nối đến MSB", ErrorCode.HttpRequestTimeOut);
                }
                ThrowException();
            }

            string strResBody = await res.Content.ReadAsStringAsync();
            var resBody = JsonSerializer.Deserialize<ResponseDomainCreateQrDto>(strResBody);
            if (resBody.Code != MsbErrorCodes.Success)
            {
                ThrowException();
            }
            //kiểm tra check sum
            string checksum = $"{resBody.Code}|{resBody.Message}|{resBody.Data}|{input.AccessCode}";
            string hashChecksum = CryptographyUtils.ComputeMD5(checksum);
            
            string test = CryptographyUtils.ComputeMD5($"{resBody.Code}|{resBody.Message}|{resBody.Data}|{input.AccessCode}");
            
            if (resBody.Checksum?.ToLower() != hashChecksum.ToLower())
            {
                throw new FaultException(new FaultReason($"Checksum không hợp lệ"), new FaultCode(((int)ErrorCode.BadRequest).ToString()), "");
            }

            return new ResultCreateVietQrDto
            {
                QrCode = resBody.Data
            };
        }

        /// <summary>
        /// Tạo yêu cầu thu hộ, trả về mã Qr
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<ResultRequestCollectMoneyDto> RequestCollectMoney(RequestCollectMoneyDto input)
        {
            _logger.LogInformation($"{nameof(RequestCollectMoney)}: input = {JsonSerializer.Serialize(input)}");
            var resultCreateVA = await CreateVirtualAccount(new CreateVADto
            {
                TId = input.TId,
                MId = input.MId,
                PrefixAccount = input.PrefixAccount,
                OrderCode = input.OrderCode,
                BankAccountName = input.OwnerAccount,
                Note = input.Note,
            });
            //var resultCreateVietQr = await CreateVietQr(new CreateVietQrDto
            //{
            //    BankAccount = resultCreateVA.AccountNumber,
            //    OwnerAccount = input.OwnerAccount,
            //    AmountMoney = input.AmountMoney,
            //    Note = input.Note
            //});
            return new ResultRequestCollectMoneyDto
            {
                AccountNumber = resultCreateVA.AccountNumber,
                //QrCode = resultCreateVietQr.QrCode
            };
        }
    }
}
