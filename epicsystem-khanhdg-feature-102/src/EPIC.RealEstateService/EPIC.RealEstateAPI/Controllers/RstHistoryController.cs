using EPIC.RealEstateDomain.Interfaces;
using EPIC.RealEstateEntities.Dto.RstHistoryUpdate;
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

namespace EPIC.RealEstateAPI.Controllers
{
    [Authorize]
    [AuthorizeAdminUserTypeFilter]
    [Route("api/real-estate/history")]
    [ApiController]
    public class RstHistoryController : BaseController
    {
        private readonly IRstHistoryServices _rstHistoryServices;

        public RstHistoryController(ILogger<RstHistoryController> logger, IRstHistoryServices rstHistoryServices)
        {
            _logger = logger;
            _rstHistoryServices = rstHistoryServices;
        }

        /// <summary>
        /// Tìm kiếm lịch sử sổ lệnh
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet("order/find-all")]
        [PermissionFilter(Permissions.RealStateMenuSoLenh_LichSu_DanhSach)]
        public APIResponse FindAllHistoryOrder([FromQuery] FilterRstHistoryUpdateDto input)
        {
            try
            {
                var result = _rstHistoryServices.FindAllHistoryTable(input, new int[] { RstHistoryUpdateTables.RST_ORDER, RstHistoryUpdateTables.RST_ORDER_PAYMENT, RstHistoryUpdateTables.RST_ORDER_CO_OWNER });
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Tìm kiếm lịch sử căn hộ
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet("product-item/find-all")]
        [PermissionFilter(Permissions.RealStateProjectListDetail_LichSu_DanhSach)]
        public APIResponse FindAllHistoryProductItem([FromQuery] FilterRstHistoryUpdateDto input)
        {
            try
            {
                var result = _rstHistoryServices.FindAllHistoryTable(input, new int[] { RstHistoryUpdateTables.RST_PRODUCT_ITEM });
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Tìm kiếm lịch sử căn hộ mở bán
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet("open-sell-detail/find-all")]
        [PermissionFilter(Permissions.RealStateMoBan_DSSP_ChiTiet_LichSu_DanhSach)]
        public APIResponse FindAllHistoryOpenSellDetail([FromQuery] FilterRstHistoryUpdateDto input)
        {
            try
            {
                var result = _rstHistoryServices.FindAllHistoryTable(input, new int[] { RstHistoryUpdateTables.RST_OPEN_SELL_DETAIL });
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }
    }
}
