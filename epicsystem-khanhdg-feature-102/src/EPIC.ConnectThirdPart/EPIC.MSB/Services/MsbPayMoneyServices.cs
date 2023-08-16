using EPIC.MSB.Configs;
using EPIC.MSB.ConstVariables;
using EPIC.MSB.Dto.Notification;
using EPIC.MSB.Dto.PayMoney;
using EPIC.Utils;
using EPIC.Utils.Net.MimeTypes;
using EPIC.Utils.Security;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.ServiceModel;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using EPIC.Utils.ConstantVariables.Shared;

namespace EPIC.MSB.Services
{
    /// <summary>
    /// Msb chi hộ 
    /// </summary>
    public class MsbPayMoneyServices : MsbServicesBase
    {
        //private static string CREATE_REQUEST = "/v1/api/msb/acq-hub/transfer-247/transfer-process/request";
        //private static string VALIDATE_REQUEST = "/v1/api/msb/acq-hub/transfer-247/transfer-process/validate";
        //private static string TRANSFER_PROCESS = "/v1/api/msb/acq-hub/transfer-247/transfer-process";
        //private static string TRANSFER_WITH_OTP = "/v1/api/msb/acq-hub/transfer-247/transfer-process/transfer-with-otp";
        //Product

        private static string CREATE_REQUEST = "/apis_acq_transfers/1.0.0/transfer-process/request"; // Tạo lô chi hộ theo lô

        private static string VALIDATE_REQUEST = "/apis_acq_transfers/1.0.0/transfer-process/validate"; // Validate lô chị hộ

        private static string TRANSFER_PROCESS = "/apis_acq_transfers/1.0.0/transfer-process"; // Thực hiện lô chi hộ

        private static string TRANSFER_WITH_OTP = "/apis_acq_transfers/1.0.0/transfer-process/transfer-with-otp"; //  Verify OTO lô chị hộ(nếu có)

        private static string INQUIRY_REQUEST = "/apis_acq_transfers/1.0.0/transfer-process/inquiry-batch"; // Inquiry lô chị hộ

        private static string TRANSFER_PROCESS_ONE_BY_ONE = "/apis_acq_transfers/1.0.0/transfer-process/transfer-one-by-one"; // Thực hiện lệnh chi hộ 1-1

        private static string TRANSFER_PROCESS_ONE_BY_ONE_WITH_OTP = "/apis_acq_transfers/1.0.0/transfer-process/transfer-one-by-one-with-otp"; // API verify OTP lệnh chi hộ 1-1(nếu có)

        private static string TRANSFER_PROCESS_INQUIRY_TRANSACTION = "/apis_acq_transfers/1.0.0/transfer-process/inquiry-transaction"; // inquiry giao dịch chi hộ 1-1, các giao dịch nhỏ trong lô:

        private static string TRANSFER_PROCESS_INQUIRY_ACCOUNT = "/apis_acq_transfers/1.0.0/transfer-process/inquiry-account"; // API inquiry tên tài khoản theo stk

        public MsbPayMoneyServices(ILogger<MsbPayMoneyServices> logger, IOptions<MsbConfiguration> config, IWebHostEnvironment env) : base(logger, config, env)
        {
            if (EnvironmentNames.DevelopEnv.Contains(env.EnvironmentName))
            {
                CREATE_REQUEST = "/v1/api/msb/acq-hub/transfer-247/transfer-process/request";
                VALIDATE_REQUEST = "/v1/api/msb/acq-hub/transfer-247/transfer-process/validate";
                TRANSFER_PROCESS = "/v1/api/msb/acq-hub/transfer-247/transfer-process";
                TRANSFER_WITH_OTP = "/v1/api/msb/acq-hub/transfer-247/transfer-process/transfer-with-otp";
                INQUIRY_REQUEST = "/v1/api/msb/acq-hub/transfer-247/transfer-process/inquiry-batch";
            }
        }

        /// <summary>
        /// Tạo yêu cầu chi hộ
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private async Task<ResultCreateRequestPayDto> CreateRequest(MsbRequestPayMoneyDto input)
        {
            _logger.LogInformation($"{nameof(CreateRequest)}: input = {JsonSerializer.Serialize(input)}");
            var request = new RequestCreateRequestPayDto()
            {
                RequestId = MsbPrefixRequestId.Prefix + input.RequestId.ToString(),
                TId = input.TId,
                MId = input.MId,
                Data = input.Details.Select(detail => new RequestPayItem
                {
                    TransId = MsbPrefixRequestId.Prefix + detail.DetailId.ToString(),
                    TransDate = DateTime.Now.ToString("yyyyMMddHHmmss"),
                    Note = detail.Note,
                    ReceiveAccount = detail.BankAccount,
                    ReceiveName = detail.OwnerAccount,
                    ReceiveBank = detail.ReceiveBankBin,
                    Amount = detail.AmountMoney.ToString()
                }).ToList(),
            };
            request.SecureHash = CryptographyUtils.ComputeSha256Hash(input.AccessCode, request.RequestId,
                request.TId, request.MId, request.Data.Count.ToString()).ToLower();
            HttpResponseMessage res = await RequestPostAsync(
                nameof(CreateRequest),
                new(new Uri(_config.Value.BaseUrl), CREATE_REQUEST),
                JsonSerializer.Serialize(request), 60000);
            var resBody = await res.Content.ReadFromJsonAsync<ResponseRequestPayDto>();
            HandleResponse(resBody.Code, resBody.Description);
            return new()
            {
                Code = resBody.Code,
                Data = resBody.Data,
                Description = resBody.Description
            };
        }

        /// <summary>
        /// Kiểm tra yêu cầu chi hộ
        /// </summary>
        /// <returns></returns>
        private async Task<List<ResultValidatePayDto>> ValidateRequest(ValidateRequestDto input)
        {
            _logger.LogInformation($"{nameof(ValidateRequest)}: input = {JsonSerializer.Serialize(input)}");
            var request = new RequestValidatePayDto()
            {
                RequestId = MsbPrefixRequestId.Prefix + input.RequestId.ToString(),
                TId = input.TId,
                MId = input.MId,
            };
            request.SecureHash = CryptographyUtils.ComputeSha256Hash(input.AccessCode, request.RequestId, request.TId, request.MId).ToLower();
            HttpResponseMessage res = await RequestPostAsync(
                nameof(ValidateRequest),
                new(new Uri(_config.Value.BaseUrl), VALIDATE_REQUEST),
                JsonSerializer.Serialize(request), 60000); //không giới hạn thời gian timeout
            var resBody = await res.Content.ReadFromJsonAsync<ResponseRequestPayDto>();
            HandleResponse(resBody.Code, resBody.Description);
            List<ResultValidatePayDto> result = new();
            if (resBody.Code == MsbErrorCodes.ValidRequestError)
            {
                var detailErrors = JsonSerializer.Deserialize<Dictionary<string, string[]>>(resBody.Data);
                foreach (var item in detailErrors)
                {
                    result.Add(new ResultValidatePayDto
                    {
                        DetailId = long.Parse(item.Key.Replace(MsbPrefixRequestId.Prefix, "")),
                        ErrorMessage = string.Join("; ", item.Value)
                    });
                }
            }
            return result;
        }

        /// <summary>
        /// Thực thi yêu cầu chi hộ, nếu có yêu cầu thêm otp thì gọi thêm hàm thực thi yêu cầu chi hộ kèm otp,
        /// nếu otp hết hạn cũng gọi lại hàm này để thực thi lại
        /// Return về mã lỗi
        /// </summary>
        /// <param name="requestId"></param>
        /// <returns></returns>
        public async Task<bool> TransferProcess(long requestId, string TId, string MId, string accessCode)
        {
            _logger.LogInformation($"{nameof(TransferProcess)}: requestId = {requestId}");
            var request = new RequestValidatePayDto()
            {
                RequestId = MsbPrefixRequestId.Prefix + requestId.ToString(),
                TId = TId.Trim(),
                MId = MId.Trim(),
            };
            request.SecureHash = CryptographyUtils.ComputeSha256Hash(accessCode, request.RequestId, request.TId, request.MId).ToLower();
            HttpResponseMessage res = await RequestPostAsync(
                nameof(ValidateRequest),
                new(new Uri(_config.Value.BaseUrl), TRANSFER_PROCESS),
                JsonSerializer.Serialize(request), 60000);
            var resBody = await res.Content.ReadFromJsonAsync<ResponseRequestPayDto>();
            HandleResponse(resBody.Code, resBody.Description);
            //Nếu mã lỗi yêu cầu otp thì trả true
            return resBody.Code == MsbErrorCodes.SubmitRequestTransferSuccessfully;
        }

        /// <summary>
        /// Yêu cầu chi hộ, trả ra kết quả gọi api tạo yêu cầu chi hộ
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<ResultRequestPayDto> RequestPayMoney(MsbRequestPayMoneyDto input)
        {
            _logger.LogInformation($"{nameof(RequestPayMoney)}: input = {JsonSerializer.Serialize(input)}");
            var result = new ResultRequestPayDto();
            var resultCreate = await CreateRequest(input);
            var resultValidate = await ValidateRequest(new ValidateRequestDto()
            {
                RequestId = input.RequestId,
                TId = input.TId,
                MId = input.MId,
                AccessCode = input.AccessCode
            });
            result.ErrorDetails = resultValidate.Select(v => new ResultRequestPayDetailDto
            {
                DetailId = v.DetailId,
                ErrorMessage = v.ErrorMessage
            }).ToList();
            if (resultValidate.Count > 0)
                return result;
            //yêu cầu chi luôn để nhận otp
            result.IsSubmitOtp = await TransferProcess(input.RequestId, input.TId, input.MId, input.AccessCode);
            return result;
        }

        /// <summary>
        /// Thực thi yêu cầu chi hộ với otp, sau khi đã tạo yêu cầu chi hộ thành công
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task TransferProcessOtp(ProcessRequestPayDto input)
        {
            _logger.LogInformation($"{nameof(TransferProcessOtp)}: input = {JsonSerializer.Serialize(input)}");

            var request = new RequestProcessTransferDto()
            {
                RequestId = MsbPrefixRequestId.Prefix + input.RequestId.ToString(),
                TId = input.TId,
                MId = input.MId,
                Otp = input.Otp,
            };
            request.SecureHash = CryptographyUtils.ComputeSha256Hash(input.AccessCode, request.RequestId, request.TId, request.MId, request.Otp).ToLower();
            HttpResponseMessage res = await RequestPostAsync(
                nameof(TransferProcessOtp),
                new(new Uri(_config.Value.BaseUrl), TRANSFER_WITH_OTP),
                JsonSerializer.Serialize(request),
                60000);
            var resBody = await res.Content.ReadFromJsonAsync<ResponseRequestPayDto>();
            HandleResponse(resBody.Code, resBody.Description);
        }

        /// <summary>
        /// Xử lý nhận thông báo chi hộ SecureHash
        /// </summary>
        public void HandleNotificationPayment(ReceiveNotificationPaymentDto input, string accessCode)
        {
            _logger.LogInformation($"{nameof(HandleNotificationPayment)}: input = {JsonSerializer.Serialize(input)}");

            //kiểm tra SecureHash
            string hashSign = CryptographyUtils.ComputeSha256Hash(accessCode, input.TransId ?? "null", input.TransDate ?? "null",
                input.TId ?? "null", input.MId ?? "null", input.Note ?? "null", input.SenderName ?? "null", input.ReceiveName ?? "null",
                input.SenderAccount ?? "null", input.ReceiveAccount ?? "null", input.ReceiveBank ?? "null", input.Amount ?? "null",
                input.Fee ?? "null", input.Status ?? "null", input.NapasTransId ?? "null", input.Rrn ?? "null");
            if (input.SecureHash?.ToLower() != hashSign?.ToLower())
            {
                _logger.LogError($"SecureHash không hợp lệ: SecureHash phải là {hashSign}");
                //SecureHash không hợp lệ
                throw new FaultException(new FaultReason($"SecureHash không hợp lệ"), new FaultCode(((int)ErrorCode.BadRequest).ToString()), "");
            }
        }

        /// <summary>
        /// Truy vấn lô
        /// </summary>
        /// <param name="input"></param>
        public async Task<List<ResponseInquiryBatchDetailDto>> InquiryBatch(InquiryBatchDto input)
        {
            _logger.LogInformation($"{nameof(InquiryBatch)}: input = {JsonSerializer.Serialize(input)}");

            var request = new RequestInquiryBatchRequestDto()
            {
                RequestId = MsbPrefixRequestId.Prefix + input.RequestId.ToString(),
                TId = input.TId,
                MId = input.MId,
            };
            request.SecureHash = CryptographyUtils.ComputeSha256Hash(input.AccessCode, request.RequestId, request.TId, request.MId).ToLower();
            HttpResponseMessage res = await RequestPostAsync(
                nameof(InquiryBatch),
                new(new Uri(_config.Value.BaseUrl), INQUIRY_REQUEST),
                JsonSerializer.Serialize(request),
                60000);
            var resBody = await res.Content.ReadFromJsonAsync<ResponseInquiryBatchDto>();
            HandleResponse(resBody.Code, resBody.Description);
            var detailBatch = JsonSerializer.Deserialize<List<ResponseInquiryBatchDetailDto>>(resBody.Data);
            return detailBatch;
        }
    }
}
