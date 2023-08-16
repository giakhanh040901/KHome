using EPIC.InvestDomain.Interfaces;
using EPIC.InvestEntities.Dto.InterestPayment;
using EPIC.Shared.Filter;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Identity;
using EPIC.Utils.Controllers;
using EPIC.WebAPIBase.FIlters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EPIC.InvestAPI.Controllers
{
    [Authorize]
    [AuthorizeAdminUserTypeFilter]
    [Route("api/invest/interest-payment")]
    [ApiController]
    public class InterestPaymentController : BaseController
    {
        private readonly IInterestPaymentServices _interestPaymentServices;
        private readonly IConfiguration _configuration;

        public InterestPaymentController(
            ILogger<InterestPaymentController> logger,
            IInterestPaymentServices interestPaymentServices,
            IConfiguration configuration)
        {
            _logger = logger;
            _interestPaymentServices = interestPaymentServices;
            _configuration = configuration;
        }

        /// <summary>
        /// Lập chi trả
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("add")]
        [PermissionFilter(Permissions.InvestHopDong_HopDongDaoHan_LapDSChiTra)]
        public APIResponse InterestPaymentAdd(InterestPaymentCreateListDto input)
        {
            try
            {
                var result = _interestPaymentServices.InterestPaymentAdd(input);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }
        /// <summary>
        /// Danh sách chi trả phân trang
        /// isLastPeriod : Y = kỳ cuối, N = kỳ bình thường
        /// </summary>
        /// <returns></returns>
        [HttpGet("find")]
        [PermissionFilter(Permissions.InvestHopDong_HopDongDaoHan_DanhSach)]
        public APIResponse FindAll([FromQuery] InterestPaymentFilterDto input)
        {
            try
            {
                var result = _interestPaymentServices.FindAll(input);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Danh sách chi trả theo id 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("find/{id}")]
        [HttpGet]
        [PermissionFilter(Permissions.InvestHopDong_HopDongDaoHan_DanhSach)]
        public APIResponse FindById(int id)
        {
            try
            {
                var result = _interestPaymentServices.FindById(id);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Chuyển trạng thái từ đã lập chưa chi trả sang đã chi trả, chi trả từng kỳ
        /// </summary>
        /// <returns></returns>
        [HttpPut("change-established-to-paid-status")]
        [PermissionFilter(Permissions.InvestHopDong_HopDongDaoHan_DuyetChiThuCong)]
        public async Task<APIResponse> ChangeEstablishedWithOutPayingToPaidStatus(List<int> ids)
        {
            try
            {
                await _interestPaymentServices.ApproveInterestPayment(ids);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// hủy yêu cầu tái tục 
        /// </summary>
        /// <param name="interestPaymentId"></param>
        /// <returns></returns>
        [HttpPut("cancel-renewal-request")]
        public APIResponse CancelRenewalRequest(List<int> interestPaymentId)
        {
            try
            {
                _interestPaymentServices.CancelRenewalRequest(interestPaymentId);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Chi trả cuối kỳ tái tục vốn
        /// </summary>
        /// <returns></returns>
        [HttpPut("payment-last-period")]
        [PermissionFilter(Permissions.InvestHopDong_HopDongDaoHan_DuyetTaiTuc)]
        public async Task<APIResponse> PaymentLastPeriod(List<int> ids)
        {
            try
            {
                await _interestPaymentServices.ApprovePaymentLastPeriod(ids);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Gửi lại thông báo chi trả thành công
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut("resend-notify-interest-payment-success")]
        public async Task<APIResponse> ResendNotifyInvestInterestPaymentSuccess(int id)
        {
            try
            {
                await _interestPaymentServices.ResendNotifyInvestInterestPaymentSuccess(id);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Gửi yêu cầu cbi chi tiền
        /// </summary>
        /// <returns></returns>
        [HttpPut("prepare-approve")]
        public async Task<APIResponse> PrepareApproveRequestInterestPayment(PrepareApproveRequestInterestPaymentDto input)
        {
            try
            {
                var result = await _interestPaymentServices.PrepareApproveRequestInterestPayment(input);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Chi trả duyệt tái tục (Tự động Status = 2, thủ công Status = 4)
        /// </summary>
        /// <returns></returns>
        [HttpPut("approve-payment-renewal")]
        public async Task<APIResponse> ApproveInterestPaymentOrderRenewal(ApproveInterestPaymentRenewalsOrderDto input)
        {
            try
            {
                await _interestPaymentServices.ApproveInterestPaymentOrder(input, true);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Chi trả từng kỳ, cuối kỳ (Tự động Status = 2, thủ công Status = 4)
        /// </summary>
        /// <returns></returns>
        [HttpPut("approve-payment")]
        public async Task<APIResponse> ApproveInterestPaymentOrder(ApproveInterestPaymentRenewalsOrderDto input)
        {
            try
            {
                await _interestPaymentServices.ApproveInterestPaymentOrder(input, false);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Gửi lại thông báo TÁI TỤC thành công
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut("resend-notify-renewals-success")]
        public async Task<APIResponse> ResendNotifyInvestRenewalsSuccess(int id)
        {
            try
            {
                await _interestPaymentServices.ResendNotifyInvestRenewalsSuccess(id);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }
    }
}
