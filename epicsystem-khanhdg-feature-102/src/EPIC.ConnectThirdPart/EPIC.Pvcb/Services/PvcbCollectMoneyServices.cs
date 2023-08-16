using EPIC.Pvcb.Configs;
using EPIC.Pvcb.Dto.CollectMoney;
using EPIC.Utils;
using EPIC.Utils.Net.MimeTypes;
using IdentityModel.Client;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Net;
using System.Net.Http;
using System.ServiceModel;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace EPIC.Pvcb.Services
{
    public class PvcbCollectMoneyServices
    {
        private readonly ILogger _logger;
        private readonly string _apiPvcbPayment;
        private readonly string _apiPvcb;
        private readonly PvcbClientCredential _pvcbClientCredential;

        private const string CONNECT_TOKEN = "auth/realms/pvcombank/protocol/openid-connect/token";
        private const string THONG_TIN_TAI_KHOAN = "payment/v1/fundtransfer/inquiryintrabank";

        public PvcbCollectMoneyServices(
            ILogger<PvcbCollectMoneyServices> logger,
            IOptions<PvcbConfiguration> config)
        {
            _logger = logger;
            _apiPvcbPayment = config.Value.ApiPvcb.Payment;
            _apiPvcb = config.Value.ApiPvcb.Pvcb;
            _pvcbClientCredential = config.Value.ApiPvcb.ClientCredential;
        }

        /// <summary>
        /// Lấy access token
        /// </summary>
        /// <returns></returns>
        private async Task<string> GetAccessTokenAsync()
        {
            HttpClient httpClient = new HttpClient();
            string path = $"{_apiPvcb}/{CONNECT_TOKEN}";
            var tokenResponse = await httpClient.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
            {
                Address = path,
                ClientId = _pvcbClientCredential.ClientId,
                ClientSecret = _pvcbClientCredential.ClientSecret
            });
            if (tokenResponse.IsError)
            {
                _logger.LogError($"Gọi api lấy access token pvcb thất bại: StatusCode = {tokenResponse.HttpStatusCode}, Message = {tokenResponse.Error}");
                throw new FaultException(new FaultReason($"Gọi api lấy access token pvcb thất bại"), new FaultCode(((int)ErrorCode.HttpRequestException).ToString()), "");
            }
            return tokenResponse.AccessToken;
        }

        /// <summary>
        /// Lấy tên chủ tài khoản theo số tài khoản và bankId cung cấp bởi PVCombank
        /// </summary>
        /// <param name="bankAccNo"></param>
        /// <param name="pvcbBankId"></param>
        /// <returns></returns>
        public async Task<string> GetOwnerBankAccNameAsync(string bankAccNo, string pvcbBankId)
        {
            string token = await GetAccessTokenAsync();
            HttpClient httpClient = new HttpClient();
            httpClient.SetBearerToken(token);
            string requestBody = JsonSerializer.Serialize(new
            {
                ftType = "ACC",
                numberOfBeneficiary = bankAccNo,
                amount = 0,
                description = "Lay thong tin tai khoan",
                bankId = pvcbBankId
            });
            var content = new StringContent(requestBody, Encoding.UTF8, MimeTypeNames.ApplicationJson);
            string path = $"{_apiPvcbPayment}/{THONG_TIN_TAI_KHOAN}";
            string bankAccName = null;
            try
            {
                HttpResponseMessage res = await httpClient.PostAsync(path, content);
                string responseBody = await res.Content.ReadAsStringAsync();
                if (res.StatusCode == HttpStatusCode.OK)
                {
                    var body = JsonSerializer.Deserialize<GetThongTinThanhToanDto>(responseBody);
                    bankAccName = body.NameOfBeneficiary;
                    _logger.LogInformation($"gọi api lấy tên chủ tài khoản ngân hàng thành công: Path = {path}, RequestBody = {requestBody}, StatusCode = {res.StatusCode}, ResponeBody = {responseBody}");
                }
                else
                {
                    _logger.LogError($"Không gọi được api lấy tên chủ tài khoản ngân hàng: Path = {path}, RequestBody = {requestBody}, StatusCode = {res.StatusCode}, ResponeBody = {responseBody}");
                    throw new FaultException(new FaultReason($"Gọi api bank thất bại"), new FaultCode(((int)ErrorCode.HttpRequestException).ToString()), "");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Không gọi được api lấy tên chủ tài khoản ngân hàng: Path = {path}, RequestBody = {requestBody}, RequestBody = {requestBody}, exception: {ex.Message}");
                throw new FaultException(new FaultReason($"Gọi api bank thất bại"), new FaultCode(((int)ErrorCode.HttpRequestException).ToString()), "");
            }
            return bankAccName;
        }
    }
}
