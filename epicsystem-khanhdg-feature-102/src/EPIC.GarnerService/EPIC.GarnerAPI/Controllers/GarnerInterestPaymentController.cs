using EPIC.GarnerDomain.Implements;
using EPIC.GarnerDomain.Interfaces;
using EPIC.GarnerEntities.Dto.GarnerInterestPayment;
using EPIC.GarnerEntities.Dto.GarnerProduct;
using EPIC.Shared.Filter;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Identity;
using EPIC.Utils.Controllers;
using EPIC.WebAPIBase.FIlters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace EPIC.GarnerAPI.Controllers
{
    [Authorize]
    [AuthorizeAdminUserTypeFilter]
    [Route("api/garner/interest-payment")]
    [ApiController]
    public class GarnerInterestPaymentController : BaseController
    {
        private readonly IGarnerInterestPaymentServices _garnerInterestPaymentServices;

        public GarnerInterestPaymentController(ILogger<GarnerInterestPaymentController> logger,
            IGarnerInterestPaymentServices garnerInterestPaymentServices)
        {
            _logger = logger;
            _garnerInterestPaymentServices=garnerInterestPaymentServices;
        }   

        /// <summary>
        /// Danh sách đến hạn chi trả
        /// </summary>
        [HttpGet("find-all-interest-payment")]
        [PermissionFilter(Permissions.GarnerHDPP_CTLC_DanhSach)]
        [ProducesResponseType(typeof(APIResponse<List<GarnerInterestPaymentByPolicyDto>>), (int)HttpStatusCode.OK)]
        public async Task<APIResponse> FindAllInterestPaymentPay([FromQuery] FilterGarnerInterestPaymentDto input)
        {
            try
            {
                var result = await _garnerInterestPaymentServices.FindAllInterestPaymentPay(input);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Danh sách đã lập chi trả
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet("find-all")]
        [PermissionFilter(Permissions.GarnerHDPP_CTLC_DanhSach)]
        [ProducesResponseType(typeof(APIResponse<List<GarnerInterestPaymentDto>>), (int)HttpStatusCode.OK)]
        public APIResponse FindAll([FromQuery] FilterGarnerInterestPaymentDto input)
        {
            try
            {
                var result = _garnerInterestPaymentServices.FindAll(input);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        [HttpGet("find/{id}")]
        [ProducesResponseType(typeof(APIResponse<GarnerInterestPaymentSetUpDto>), (int)HttpStatusCode.OK)]

        public APIResponse FindById(int id)
        {
            try
            {
                var result = _garnerInterestPaymentServices.FindById(id);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Thêm chi trả 
        /// </summary>
        [HttpPost("add")]
        [PermissionFilter(Permissions.GarnerHDPP_CTLC_LapDSChiTra)]
        public APIResponse Add([FromBody] List<CreateGarnerInterestPaymentDto> input)
        {
            try
            {
                var result = _garnerInterestPaymentServices.Add(input);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Chuẩn bị yêu cầu chi trả
        /// </summary>
        [HttpPost("prepare-approve")]
        [PermissionFilter(Permissions.GarnerHDPP_CTLC_DuyetChiTD)]
        public async Task<APIResponse> PrepareApproveRequestInterestPayment([FromBody] PrepareApproveRequestInterestPaymentDto input)
        {
            try
            {
                var result = await _garnerInterestPaymentServices.PrepareApproveRequestInterestPayment(input);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Duyệt yêu cầu chi trả
        /// </summary>
        [HttpPut("approve")]
        [PermissionFilter(Permissions.GarnerHDPP_CTLC_DuyetChiTC, Permissions.GarnerHDPP_CTLC_DuyetChiTD)]
        public async Task<APIResponse> ApproveInterestPaymentOrder([FromBody] InvestEntities.Dto.InterestPayment.ApproveInterestPaymentRenewalsOrderDto input)
        {
            try
            {
                await _garnerInterestPaymentServices.ApproveInterestPaymentOrder(input);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

    }
}
