using EPIC.MSB.Configs;
using EPIC.MSB.ConstVariables;
using EPIC.MSB.Dto.Inquiry;
using EPIC.MSB.Dto.PayMoney;
using EPIC.Utils.ConstantVariables.Shared;
using EPIC.Utils.Security;
using Microsoft.AspNetCore.Hosting;
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

namespace EPIC.MSB.Services
{
    /// <summary>
    /// Truy vấn
    /// </summary>
    public class MsbInquiryServices : MsbServicesBase
    {
        public static string INQUIRY_ACCOUNT = "/apis_acq_transfers/1.0.0/transfer-process/inquiry-account";

        public MsbInquiryServices(ILogger<MsbInquiryServices> logger, IOptions<MsbConfiguration> config, IWebHostEnvironment env) : base(logger, config, env)
        {
            if (EnvironmentNames.DevelopEnv.Contains(env.EnvironmentName))
            {
                INQUIRY_ACCOUNT = "/v1/api/msb/acq-hub/transfer-247/transfer-process/inquiry-account";
            }
        }

        /// <summary>
        /// Truy vấn tên chủ tài khoản
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<string> InquiryAccount(InquiryAccountDto input)
        {
            _logger.LogInformation($"{nameof(InquiryAccount)}: input = {JsonSerializer.Serialize(input)}");
            var request = new RequestInquiryAccountDto()
            {
                TId = _config.Value.TId,
                MId = _config.Value.MId,
                AccountNo = input.BankAccount,
                BankCode = input.Bin,
            };
            request.SecureHash = CryptographyUtils.ComputeSha256Hash(_config.Value.AccessCode, request.TId, request.MId, request.AccountNo, request.BankCode).ToLower();
            HttpResponseMessage res = await RequestPostAsync(
                nameof(InquiryAccount),
                new(new Uri(_config.Value.BaseUrl), INQUIRY_ACCOUNT),
                JsonSerializer.Serialize(request), _config.Value.TimeOut);
            var resBody = await res.Content.ReadFromJsonAsync<ResponseRequestPayDto>();
            HandleResponse(resBody.Code, resBody.Description, false); //không show msb error code
            //trả về tên chủ tài khoản
            return resBody.Data;
        }
    }
}
