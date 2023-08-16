using DocumentFormat.OpenXml.Office2021.DocumentTasks;
using EPIC.InvestDomain.Implements;
using EPIC.InvestDomain.Interfaces;
using EPIC.InvestEntities.Dto.Withdrawal;
using EPIC.Shared.Filter;
using EPIC.Notification.Services;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Identity;
using EPIC.Utils.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using EPIC.WebAPIBase.FIlters;
using DocumentFormat.OpenXml.Office2010.ExcelAc;
using System.Collections.Generic;
using DocumentFormat.OpenXml.Presentation;
using EPIC.GarnerEntities.Dto.GarnerWithdrawal;

namespace EPIC.InvestAPI.Controllers
{
    [Authorize]
    [AuthorizeAdminUserTypeFilter]
    [Route("api/invest/withdrawal")]
    [ApiController]
    public class WithdrawalController : BaseController
    {
        private readonly IWithdrawalServices _withdrawalSettlementServices;
        private readonly IConfiguration _configuration;
        private readonly NotificationServices _sendEmailServices;

        public WithdrawalController(
            ILogger<WithdrawalController> logger,
            IWithdrawalServices withdrawalSettlementServices,
            NotificationServices sendEmailServices,
            IConfiguration configuration)
        {
            _logger = logger;
            _withdrawalSettlementServices = withdrawalSettlementServices;
            _configuration = configuration;
            _sendEmailServices = sendEmailServices;
        }

        /// <summary>
        /// Lập Rút vốn 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("add")]
        [PermissionFilter(Permissions.InvestHDPP_HopDong_YeuCauRutVon)]
        public APIResponse WithdrawalAdd(CreateWithdrawalDto input)
        {
            try
            {
                _withdrawalSettlementServices.WithdrawalAdd(input);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Lấy danh sách lập rút vốn
        /// </summary>
        /// <returns></returns>
        [HttpGet("get-all")]
        [PermissionFilter(Permissions.InvestPDYCRV_DanhSach)]
        public APIResponse FindAll([FromQuery] InvestWithdrawalFilterDto input)
        {
            try
            {
                var result = _withdrawalSettlementServices.FindAll(input);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Duyệt yêu cầu rút vốn
        /// </summary>
        /// <returns></returns>
        [HttpPut("approve")]
        [PermissionFilter(Permissions.InvestPDYCRV_PheDuyetOrHuy)]
        [WhiteListIpFilter(WhiteListIpTypes.InvestDuyetChiTien)]
        public async Task<APIResponse> WithdrawalApprove(InvestApproveRequestWithdrawalDto input)
        {
            try
            {
                await _withdrawalSettlementServices.WithdrawalApprove(input);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Hủy duyệt yêu cầu rút vốn
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut("cancel/{id}")]
        [PermissionFilter(Permissions.InvestPDYCRV_PheDuyetOrHuy)]
        public APIResponse WithdrawalCancel(long id)
        {
            try
            {
                _withdrawalSettlementServices.WithdrawalCancel(id);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Phê duyệt rút vốn
        /// </summary>
        /// <returns></returns>
        [HttpPut("approve-withdrawal")]
        public async Task<APIResponse> ApproveRequestWithdrawal(InvestApproveRequestWithdrawalDto input)
        {
            try
            {
                await _withdrawalSettlementServices.ApproveRequestWithdrawal(input);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Chuẩn bị rút vốn
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut("prepare-approve")]
        public async Task<APIResponse> PrepareApproveRequestWithdrawal(PrepareApproveRequestWithdrawalDto input)
        {
            try
            {
                var result = await _withdrawalSettlementServices.PrepareApproveRequestWithdrawal(input);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Gửi lại thông báo rút vốn thành công (withdrawalType: 1),
        /// Thông báo tất toán trước hạn thành công (rút hết, withdrawalType: 2)
        /// </summary>
        /// <returns></returns>
        [HttpPut("resend-withdrawal-success")]
        public async Task<APIResponse> ResendNotifyInvestWithdrawalSuccess(int id, int withdrawalType )
        {
            try
            {
                await _withdrawalSettlementServices.ResendNotifyInvestWithdrawalSuccess(id, withdrawalType);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }
    }
 }
