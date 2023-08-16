using EPIC.MSB.Configs;
using EPIC.MSB.ConstVariables;
using EPIC.MSB.Dto.CollectMoney;
using EPIC.MSB.Dto.Shared;
using EPIC.Utils;
using EPIC.Utils.Net.MimeTypes;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Security.Cryptography;
using System.ServiceModel;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using EPIC.Utils.ConstantVariables.Shared;

namespace EPIC.MSB.Services
{
    public class MsbServicesBase
    {
        protected readonly ILogger _logger;
        protected readonly IOptions<MsbConfiguration> _config;
        protected readonly IWebHostEnvironment _env;


        //private const string GET_ACCESS_TOKEN = "/v1/api/msb/acq-hub/payment-service/va-process/doAuth";
        private static string GET_ACCESS_TOKEN = "/msb_pay_va_auth/1.0.0";

        public MsbServicesBase(ILogger logger, IOptions<MsbConfiguration> config, IWebHostEnvironment env)
        {
            _logger = logger;
            _config = config;
            _env = env;
            if (EnvironmentNames.DevelopEnv.Contains(env.EnvironmentName))
            {
                GET_ACCESS_TOKEN = "/v1/api/msb/acq-hub/payment-service/va-process/doAuth";
            }
        }

        protected HttpClient CreateHttpClient()
        {
            HttpClient httpClient = new()
            {
                Timeout = TimeSpan.FromMilliseconds(_config.Value.TimeOut),
            };
            return httpClient;
        }

        /// <summary>
        /// Tạo request post, log lại nếu có lỗi xảy ra
        /// </summary>
        /// <param name="function">Tên hàm</param>
        /// <param name="path">đường dẫn</param>
        /// <param name="body">body</param>
        /// <param name="timeOut">thời gian time out</param>
        /// <returns></returns>
        protected async Task<HttpResponseMessage> RequestPostAsync(string function, Uri path, string body, double? timeOut = null)
        {
            HttpResponseMessage res = null;
            try
            {
                HttpClient client = CreateHttpClient();
                if (timeOut != null && timeOut > 0)
                {
                    client.Timeout = TimeSpan.FromMilliseconds(timeOut.Value);
                }
                res = await client.PostAsync(path, new StringContent(body, Encoding.UTF8, MimeTypeNames.ApplicationJson));
                string message = $"{function}: Path = {path}, RequestBody = {body}, ResponseCode: {res.StatusCode}," +
                    $" ResponseBody: {await res.Content.ReadAsStringAsync()}";
                if (res.StatusCode != HttpStatusCode.OK)
                {
                    _logger.LogError(message);
                    ThrowException($"Lỗi kết nối MSB: StatusCode = {res.StatusCode}");
                }
                else
                {
                    _logger.LogInformation(message);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"{function}: exception: {ex.Message}");
                if (ex.GetType() == typeof(TaskCanceledException))
                {
                    ThrowException("Lỗi kết nối MSB: Timeout khi kết nối đến MSB", ErrorCode.HttpRequestTimeOut);
                }
                ThrowException($"Lỗi kết nối MSB: {ex.GetType()} {ex.Message}");
            }
            return res;
        }

        protected void ThrowException()
        {
            throw new FaultException(new FaultReason($"Có lỗi xảy ra khi kết nối với MSB"),
                new FaultCode(((int)ErrorCode.HttpRequestException).ToString()), "");
        }

        protected void ThrowException(string message)
        {
            throw new FaultException(new FaultReason(message),
                new FaultCode(((int)ErrorCode.HttpRequestException).ToString()), "");
        }

        protected void ThrowException(string message, ErrorCode errorCode)
        {
            throw new FaultException(new FaultReason(message),
                new FaultCode(((int)errorCode).ToString()), "");
        }

        /// <summary>
        /// xử lý nếu có lỗi xảy ra ném ra lỗi 
        /// </summary>
        public void HandleResponse(string code, string description, bool isShowMsbError = true)
        {
            // Môi trường Test thì bỏ qua check lỗi
            if (EnvironmentNames.DevelopEnv.Contains(_env.EnvironmentName))
            {
                return;
            }
            string msbError = $"MSB Error ({code}): ";
            if (!isShowMsbError)
            {
                msbError = "";
            }
            if (code == MsbErrorCodes.DuplicateRequestId)
            {
                ThrowException($"{msbError}Id yêu cầu sang MSB bị lặp lại", ErrorCode.HttpRequestThirdPartDomainError);
            }
            //những code xảy ra lỗi nhưng vẫn cho pass qua để xử lý tiếp
            else if (code == MsbErrorCodes.ValidRequestError || code == MsbErrorCodes.SubmitRequestTransferSuccessfully)
            {
                return;
            }
            else if (code == MsbErrorCodes.RequestDataIsNotCorrectFormat)
            {
                ThrowException($"{msbError}Request Data không đúng định dạng", ErrorCode.HttpRequestThirdPartDomainError);
            }
            else if (code == MsbErrorCodes.MerchentIsNotExists)
            {
                ThrowException($"{msbError}Thông tin merchant không tồn tại", ErrorCode.HttpRequestThirdPartDomainError);
            }
            else if (code == MsbErrorCodes.ChecksumFail)
            {
                ThrowException($"{msbError}Checksum không chính xác", ErrorCode.HttpRequestThirdPartDomainError);
            }
            else if (code == MsbErrorCodes.RequestNotFound)
            {
                ThrowException($"{msbError}Không tìm thấy id yêu cầu do chưa qua được bước kiểm tra thông tin", ErrorCode.HttpRequestThirdPartDomainError);
            }
            else if (code == MsbErrorCodes.DuplicateRequestId)
            {
                ThrowException($"{msbError}Trùng id giao dịch khi gọi api yêu cầu chi hộ MSB", ErrorCode.HttpRequestThirdPartDomainError);
            }
            else if (code == MsbErrorCodes.ValidRequestError)
            {
                ThrowException($"{msbError}Lỗi valid request khi gọi api validate", ErrorCode.HttpRequestThirdPartDomainError);
            }
            else if (code == MsbErrorCodes.MerchantIsNotConfigured)
            {
                ThrowException($"{msbError}Merchant chưa được cấu hình dịch vụ chi hộ", ErrorCode.HttpRequestThirdPartDomainError);
            }
            else if (code == MsbErrorCodes.AmountExceedsLimit)
            {
                ThrowException($"{msbError}Số dư không đủ để thực hiện giao dịch", ErrorCode.HttpRequestThirdPartDomainError);
            }
            else if (code == MsbErrorCodes.DuplicateTransId)
            {
                ThrowException($"{msbError}Trùng id giao dịch trong lô", ErrorCode.HttpRequestThirdPartDomainError);
            }
            else if (code == MsbErrorCodes.InvalidTransactionNumber)
            {
                ThrowException($"{msbError}Số giao dịch không hợp lệ", ErrorCode.HttpRequestThirdPartDomainError);
            }
            else if (code == MsbErrorCodes.SubmitRequestTransferSuccessfully)
            {
                ThrowException($"{msbError}Submit thành công yêu cầu xác thực thêm otp", ErrorCode.HttpRequestThirdPartDomainError);
            }
            else if (code == MsbErrorCodes.ExpOtp)
            {
                ThrowException($"{msbError}Otp hết hạn", ErrorCode.HttpRequestThirdPartDomainError);
            }
            else if (code == MsbErrorCodes.OTPTooManyTimesAllowed)
            {
                ThrowException($"{msbError}Nhâp OTP quá số lần cho phép (5 lần)", ErrorCode.HttpRequestThirdPartDomainError);
            }
            else if (code == MsbErrorCodes.ErrorOtp)
            {
                ThrowException($"{msbError}Otp không chính xác", ErrorCode.HttpRequestThirdPartDomainError);
            }
            else if (code == MsbErrorCodes.AccountIsNotExitsOrBlocked)
            {
                ThrowException($"{msbError}Số tài khoản không tồn tại hoặc đã bị khóa", ErrorCode.InvestorBankAccNotFound);
            }
            else if (code == MsbErrorCodes.InvalidSpendingAccountNumber)
            {
                ThrowException($"{msbError}Số tài khoản chi không hợp lệ", ErrorCode.HttpRequestThirdPartDomainError);
            }
            else if (code == MsbErrorCodes.InternalServerError)
            {
                ThrowException($"{msbError}Lỗi xử lý phía MSB", ErrorCode.HttpRequestThirdPartDomainError);
            }
            else if (code == MsbErrorCodes.VACodeIsExists)
            {
                ThrowException($"{msbError}VA code đã tồn tại", ErrorCode.HttpRequestThirdPartDomainError);
            }
            else if (code != MsbErrorCodes.Success)
            {
                ThrowException($"MSB Error ({code}): {description}", ErrorCode.HttpRequestThirdPartDomainError);
            }
        }

        protected async Task<string> GetAccessTokenAsync(string TId, string MId)
        {
            _logger.LogInformation($"{nameof(GetAccessTokenAsync)}:");
            HttpClient httpClient = new()
            {
                Timeout = TimeSpan.FromMilliseconds(_config.Value.TimeOut),
            };
            string json = JsonSerializer.Serialize(new RequestMsbIdDto
            {
                TId = TId,
                MId = MId
            });
            StringContent content = new(json, Encoding.UTF8, MimeTypeNames.ApplicationJson);

            Uri path = new(new Uri(_config.Value.BaseUrl), GET_ACCESS_TOKEN);
            HttpResponseMessage res = null;
            try
            {
                res = await httpClient.PostAsync(path, content);
                string message = $"{nameof(GetAccessTokenAsync)}: Path = {path}, RequestBody = {json}, ResponseCode: {res.StatusCode}, ResponseBody: {await res.Content.ReadAsStringAsync()}";
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
                _logger.LogError($"{nameof(GetAccessTokenAsync)}: Path = {path}, RequestBody = {json}, exception: {ex.Message}");
                ThrowException();
            }
            var resBody = await res.Content.ReadFromJsonAsync<MsbResponse<ResponseDomainLoginDto>>();
            HandleResponse(resBody.ResponseMessage.RespCode, resBody.ResponseMessage.RespDesc);
            string accessToken = resBody.ResponseDomain?.AccessToken;

            if (accessToken == null)
            {
                _logger.LogError($"{nameof(GetAccessTokenAsync)}: Path = {path}, RequestBody = {json}, exception: accessToken == null");
                ThrowException();
            }
            return accessToken;
        }
    }
}
