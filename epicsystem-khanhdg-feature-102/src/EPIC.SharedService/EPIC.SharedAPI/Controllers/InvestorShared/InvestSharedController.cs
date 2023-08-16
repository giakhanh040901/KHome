using EPIC.SharedDomain.Interfaces;
using EPIC.Utils.Controllers;
using EPIC.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace EPIC.SharedAPI.Controllers.InvestorShared
{
    [Route("api/shared/investor-shared")]
    [ApiController]
    public class InvestSharedController : BaseController
    {
        private readonly IInvestorSharedServices _investorSharedServices;

        public InvestSharedController(ILogger<InvestSharedController> logger,
            IInvestorSharedServices investorSharedServices)
        {
            _logger = logger;
            _investorSharedServices = investorSharedServices;
        }

        ///// <summary>
        ///// Đổi trạng thái giao nhận hợp đồng từ đang giao thành nhận hợp đồng
        ///// </summary>
        //[HttpPut("delivery-status-recevired/{deliveryCode}")]
        //public async Task<APIResponse> ChangeDeliveryStatusRecevired(string deliveryCode)
        //{
        //    try
        //    {
        //        await _investorSharedServices.ChangeDeliveryStatusRecevired(deliveryCode);
        //        return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
        //    }
        //    catch (Exception ex)
        //    {
        //        return OkException(ex);
        //    }
        //}
    }
}
