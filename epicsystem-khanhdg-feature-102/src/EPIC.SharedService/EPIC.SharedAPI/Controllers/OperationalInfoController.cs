using EPIC.Shared.Filter;
using EPIC.SharedDomain.Interfaces;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Shared;
using EPIC.Utils.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

namespace EPIC.SharedAPI.Controllers
{
    [Authorize(Policy = AuthPolicies.SharedDataBoCongThuongPolicy)]
    [Route("api/shared/operational-info")]
    [ApiController]
    public class OperationalInfoController : BaseController
    {
        private readonly IOperationalInfoServices _operationalInfoServices;

        public OperationalInfoController(ILogger<OperationalInfoController> logger, IOperationalInfoServices operationalInfoServices)
        {
            _logger = logger;
            _operationalInfoServices = operationalInfoServices;
        }

        /// <summary>
        /// Lấy thông tin cho báo cáo của Bộ công thương
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        [ClientFilter(ClientIdentityServer.ClientSharedDataBoCongThuong)]
        [HttpPost("bo-cong-thuong")]
        public APIResponse GetInfoBCT(DateTime? startDate, DateTime? endDate)
        {
            try
            {
                var result = _operationalInfoServices.GetInfoBCT(startDate, endDate);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Lấy danh sách Trading trong hệ thống để chọn đại lý FakeData
        /// </summary>
        /// <returns></returns>
        [HttpGet("get-all")]
        public APIResponse GetAllTrading()
        {
            try
            {
                var result = _operationalInfoServices.GetTradingList();
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Danh sách đại lý được tính trong báo cáo của Bộ công thương
        /// </summary>
        /// <returns></returns>
        [HttpGet("get-all-report")]
        public APIResponse GetAllReportTrading()
        {
            try
            {
                var result = _operationalInfoServices.GetReportTradingList();
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Cập nhật danh sách các đại lý báo cáo
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut("update")]
        public APIResponse UpdateReportTradingList(int[] input)
        {
            try
            {
                _operationalInfoServices.UpdateReportTradingList(input);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }
    }
}
