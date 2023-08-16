using EPIC.DataAccess.Models;
using EPIC.LoyaltyDomain.Interfaces;
using EPIC.LoyaltyEntities.Dto.LoyPointInvestor;
using EPIC.LoyaltyEntities.Dto.LoyVoucher;
using EPIC.Utils;
using EPIC.Utils.Controllers;
using EPIC.WebAPIBase.FIlters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Threading.Tasks;

namespace EPIC.LoyaltyAPI.Controllers
{
    [Authorize]
    [AuthorizeAdminUserTypeFilter]
    [Route("api/loyalty/investor-point")]
    [ApiController]
    public class LoyPointInvestorController : BaseController
    {
        private readonly ILoyPointInvestorServices _loyPointInvestorServices;

        public LoyPointInvestorController(ILogger<LoyPointInvestorController> logger,
            ILoyPointInvestorServices loyPointInvestorServices)
        {
            _logger = logger;
            _loyPointInvestorServices = loyPointInvestorServices;
        }

        /// <summary>
        /// Lịch sử quy đổi điểm của nhà đầu tư/ Tab Danh sách ưu đãi
        /// </summary>
        [HttpGet("conversion-history")]
        [ProducesResponseType(typeof(APIResponse<PagingResult<PointInvestorConversionHistoryDto>>), (int)HttpStatusCode.OK)]
        public APIResponse FindAllVoucherConversionHistory([FromQuery] FilterPointInvestorConversionHistoryDto dto)
        {
            try
            {
                var result = _loyPointInvestorServices.FindAllVoucherConversionHistory(dto);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Xem chi tiết Tab Danh sách ưu đãi
        /// </summary>
        [HttpGet("conversion-history-get-by-id/{conversionPointDetailId}")]
        [ProducesResponseType(typeof(APIResponse<PointInvestorConversionHistoryInfoDto>), (int)HttpStatusCode.OK)]
        public APIResponse VoucherConversionHistoryGetById(int conversionPointDetailId)
        {
            try
            {
                var result = _loyPointInvestorServices.VoucherConversionHistoryGetById(conversionPointDetailId);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Tìm kiếm nhà đầu tư theo số điện thoại và điểm ranh của theo đại lý
        /// </summary>
        [HttpGet("find")]
        [ProducesResponseType(typeof(APIResponse<FindLoyPointInvestorDto>), (int)HttpStatusCode.OK)]
        public APIResponse FindInvestorByPhone(string phone, int? investorId)
        {
            try
            {
                var result = _loyPointInvestorServices.FindInvestorByPhone(phone, investorId);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

    }
}
