using EPIC.RealEstateDomain.Implements;
using EPIC.RealEstateDomain.Interfaces;
using EPIC.RealEstateEntities.Dto.RstOrder;
using EPIC.RealEstateEntities.Dto.RstOrderSellingPolicy;
using EPIC.Shared.Filter;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Identity;
using EPIC.Utils.ConstantVariables.RealEstate;
using EPIC.Utils.Controllers;
using EPIC.WebAPIBase.FIlters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace EPIC.RealEstateAPI.Controllers
{
    [Authorize]
    [AuthorizeAdminUserTypeFilter]
    [Route("api/real-estate/order-selling-policy")]
    [ApiController]
    public class RstOrderSellingPolicyController : BaseController
    {
        private readonly IRstOrderSellingPolicyServices _rstOrderSellingPolicyServices;
        public RstOrderSellingPolicyController(ILogger<RstOrderController> logger,
            IRstOrderSellingPolicyServices rstOrderSellingPolicyServices)
        {
            _logger = logger;
            _rstOrderSellingPolicyServices = rstOrderSellingPolicyServices;
        }

        /// <summary>
        /// Tìm kiếm phân trang Chính sách ưu đãi BondOrder
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet("find-all")]
        [PermissionFilter(Permissions.RealStateMenuSoLenh_CSUuDai_DanhSach)]
        public APIResponse FindAll([FromQuery]FilterRstOrderSellingPolicyDto input)
        {
            try
            {
                var result = _rstOrderSellingPolicyServices.FindAll(input);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Thêm chính sách vào sổ lệnh
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("add")]
        public APIResponse Add([FromBody] CreateRstOrderSellingPolicyDto input)
        {
            try
            {
                _rstOrderSellingPolicyServices.Add(input);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }
    }
}
