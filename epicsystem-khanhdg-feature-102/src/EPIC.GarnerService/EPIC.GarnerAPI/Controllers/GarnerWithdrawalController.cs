using EPIC.GarnerDomain.Interfaces;
using EPIC.Utils.ConstantVariables.Identity;
using EPIC.Utils;
using EPIC.Utils.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using EPIC.Shared.Filter;
using EPIC.GarnerEntities.Dto.Calendar;
using EPIC.GarnerEntities.Dto.GarnerWithdrawal;
using EPIC.Utils.ConstantVariables.Shared;
using EPIC.GarnerDomain.Implements;
using EPIC.GarnerEntities.Dto.GarnerProduct;
using EPIC.WebAPIBase.FIlters;
using System.Threading.Tasks;
using EPIC.GarnerEntities.Dto.GarnerOrder;
using System.Net;
using EPIC.PaymentEntities.Dto.MsbRequestPayment;
using System.Collections.Generic;

namespace EPIC.GarnerAPI.Controllers
{
    [Authorize]
    [AuthorizeAdminUserTypeFilter]
    [Route("api/garner/withdrawal")]
    [ApiController]
    public class GarnerWithdrawalController : BaseController
    {
        private readonly IGarnerWithdrawalServices _garnerWithdrawalServices;
        public GarnerWithdrawalController(IGarnerWithdrawalServices garnerWithdrawalServices)
        {
            _garnerWithdrawalServices = garnerWithdrawalServices;
        }

        /// <summary>
        /// lấy danh sách rút vốn
        /// </summary>
        /// <returns></returns>
        [HttpGet("find-all")]
        [PermissionFilter(Permissions.GarnerHDPP_XLRutTien_DanhSach)]
        public APIResponse FindAll([FromQuery] FilterGarnerWithdrawalDto input)
        {
            try
            {
                var result = _garnerWithdrawalServices.FindAll(input);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Yêu cầu rút vốn
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("request")]
        [PermissionFilter(Permissions.GarnerHDPP_HopDong_YeuCauRutVon)]
        public async Task<APIResponse> RequestWithdrawal([FromBody] GarnerWithdrawalRequestDto input)
        {
            try
            {
                var result = await _garnerWithdrawalServices.RequestWithdrawal(input.CifCode, input.PolicyId, input.Amount, input.WithdrawDate, SourceOrder.OFFLINE, input.BankAccountId);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Duyệt trạng thái ngân hàng sang thành công khi duyệt chi tiền
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpPut("approve-status-bank")]
        public APIResponse ApproveChangeStatusBank([FromBody] List<long> ids)
        {
            try
            {
                _garnerWithdrawalServices.ApproveChangeStatusBank(ids);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Chuẩn bị yêu cầu rút vốn
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("prepare-approve")]
        [PermissionFilter(Permissions.GarnerHDPP_XLRutTien_ChiTienTD)]
        [ProducesResponseType(typeof(APIResponse<MsbRequestPaymentWithErrorDto>), (int)HttpStatusCode.OK)]
        public async Task<APIResponse> PrepareApprove([FromBody] PrepareApproveRequestWithdrawalDto input)
        {
            try
            {
                var result = await _garnerWithdrawalServices.PrepareApproveRequestWithdrawal(input);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Duyệt/Huỷ yêu cầu rút
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut("approve")]
        [PermissionFilter(Permissions.GarnerHDPP_XLRutTien_ChiTienTC, Permissions.GarnerHDPP_XLRutTien_HuyYeuCau)]
        [WhiteListIpFilter(WhiteListIpTypes.GarnerDuyetChiTien)]
        public async Task<APIResponse> ApproveRequest([FromBody] GarnerApproveRequestWithdrawalDto input)
        {
            try
            {
                await _garnerWithdrawalServices.ApproveRequestWithdrawal(input);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }
    }
}
