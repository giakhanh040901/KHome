using EPIC.Utils.Controllers;
using EPIC.Utils.Filter;
using EPIC.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using EPIC.LoyaltyDomain.Interfaces;
using EPIC.LoyaltyEntities.Dto.LoyVoucher;
using System.Net;
using EPIC.LoyaltyEntities.Dto.LoyVoucherInvestor;
using EPIC.DataAccess.Models;
using System.Threading.Tasks;
using EPIC.LoyaltyEntities.Dto.LoyHisAccumulatePoint;
using System.Collections.Generic;
using EPIC.Utils.ConstantVariables.Loyalty;
using DocumentFormat.OpenXml.Office2010.Excel;
using Humanizer;
using EPIC.LoyaltyEntities.Dto.LoyRank;
using EPIC.WebAPIBase.FIlters;

namespace EPIC.LoyaltyAPI.Controllers
{
    [Authorize]
    [AuthorizeAdminUserTypeFilter]
    [Route("api/loyalty/rank")]
    [ApiController]
    public class LoyRankController : BaseController
    {
        private readonly ILoyRankServices _loyRankServices;

        public LoyRankController(ILogger<LoyRankController> logger,
            ILoyRankServices loyRankServices)
        {
            _logger = logger;
            _loyRankServices = loyRankServices;
        }

        /// <summary>
        /// Tạo thêm cấu hình xếp hạng
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost("")]
        public APIResponse Add([FromBody] AddRankDto dto)
        {
            try
            {
                _loyRankServices.Add(dto);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Cập nhật cấu hình xếp hạng
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPut("")]
        public APIResponse Update([FromBody] UpdateRankDto dto)
        {
            try
            {
                _loyRankServices.Update(dto);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }


        /// <summary>
        /// Đổi trạng thái cấu hình xếp hạng (1: Khởi tạo; 2: Kích hoạt; 3: Hủy kích hoạt)
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPut("status")]
        public APIResponse UpdateStatusRank([FromBody] UpdateStatusDto dto)
        {
            try
            {
                _loyRankServices.UpdateStatus(dto);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Xóa cấu hình xếp hạng theo id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public APIResponse Delete(int id)
        {
            try
            {
                _loyRankServices.DeleteRank(id);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Ds phân trang cấu hình xếp hạng
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpGet("find")]
        [ProducesResponseType(typeof(APIResponse<PagingResult<ViewFindRankDto>>), (int)HttpStatusCode.OK)]
        public APIResponse FindAll([FromQuery] FindAllRankDto dto)
        {
            try
            {
                var result = _loyRankServices.FindAll(dto);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Chi tiết cấu hình xếp hạng
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(APIResponse<ViewFindRankDto>), (int)HttpStatusCode.OK)]
        public APIResponse FindById([FromRoute] int id)
        {
            try
            {
                var result = _loyRankServices.FindById(id);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

    }
}
