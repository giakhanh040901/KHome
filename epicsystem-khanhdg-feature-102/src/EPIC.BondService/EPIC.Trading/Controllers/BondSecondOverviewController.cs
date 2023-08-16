using EPIC.BondDomain.Interfaces;
using EPIC.BondEntities.Dto.BondSecondary;
using EPIC.Utils;
using EPIC.Utils.Controllers;
using EPIC.WebAPIBase.FIlters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

namespace EPIC.BondAPI.Controllers
{
    [Authorize]
    [AuthorizeAdminUserTypeFilter]
    [Route("api/bond/product-bond-secondary")]
    [ApiController]
    public class BondSecondOverviewController : BaseController
    {
        private IBondInfoOverviewService _bondInfoOverviewServices;
        public BondSecondOverviewController(ILogger<BondSecondOverviewController> logger, IBondInfoOverviewService bondInfoOverviewServices)
        {
            _logger = logger;
            _bondInfoOverviewServices = bondInfoOverviewServices;
        }

        /// <summary>
        /// Thêm nội dung tổng quan của sản phẩm
        /// overview 1- MARKDOWN 2- HTML
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut("update-overview")]
        public APIResponse UpdateOverviewContent(UpdateBondInfoOverviewContentDto input)
        {
            try
            {
                _bondInfoOverviewServices.UpdateOverviewContent(input);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bondSecondaryid"></param>
        /// <returns></returns>
        [HttpGet("over-view-secondary-by-id")]
        public APIResponse FindOverViewById(int bondSecondaryid)
        {
            try
            {
                var result = _bondInfoOverviewServices.FindOverViewById(bondSecondaryid);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }
    }
}
