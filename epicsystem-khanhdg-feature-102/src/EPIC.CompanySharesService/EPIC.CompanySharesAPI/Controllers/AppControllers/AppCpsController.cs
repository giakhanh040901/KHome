using EPIC.CompanySharesDomain.Interfaces;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Identity;
using EPIC.Utils.Controllers;
using EPIC.WebAPIBase.FIlters;
using Hangfire;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.ComponentModel.DataAnnotations;

namespace EPIC.CompanySharesAPI.Controllers.AppControllers
{
    [Route("api/company-shares")]
    [ApiController]
    public class AppCpsController : BaseController
    {
        //private readonly ICpsInfoServices _cpsInfoServices;
        private readonly ICpsSecondaryServices _cpsSecondaryServices;
        private readonly IBackgroundJobClient _backgroundJobs;

        public AppCpsController(
            ILogger<AppCpsController> logger,
            //ICpsInfoServices cpsInfoServices,
            //IBondIssuerService issuerService,
            ICpsSecondaryServices cpsSecondaryServices,
            IBackgroundJobClient backgroundJobs)
        {
            _logger = logger;
            //_cpsInfoServices = cpsInfoServices;
            _cpsSecondaryServices = cpsSecondaryServices;
            _backgroundJobs = backgroundJobs;
        }

        #region lấy thông tin trái phiếu, chính sách
        /// <summary>
        /// Lấy danh sách sản phẩm, lấy danh sách trái phiếu, ds kỳ hạn,
        /// Nếu là khách hàng đăng nhập thì sẽ xem được bảng hàng cố định
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="orderByInterestDesc"></param>
        /// <returns></returns>
        [HttpGet("find")]
        public APIResponse FindAllCpsSecondary(string keyword, bool orderByInterestDesc)
        {
            try
            {
                var result = _cpsSecondaryServices.FindAllCpsSecondary(keyword?.Trim(), orderByInterestDesc);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Thông tin chi tiết cổ phần
        /// </summary>
        /// <param name="cpsSecondaryId"></param>
        /// <returns></returns>
        [HttpGet("find/{cpsSecondaryId}")]
        public APIResponse FindCpsById([Range(1, int.MaxValue, ErrorMessage = "cpsSecondaryId không được nhỏ hơn 1")] int cpsSecondaryId)
        {
            try
            {
                var result = _cpsSecondaryServices.FindCpsById(cpsSecondaryId);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        ///// <summary>
        ///// Get ưu đãi
        ///// </summary>
        //[HttpGet]
        //[Route("policy/get-promotion/{policyId}")]
        //public APIResponse GetPromotion([Range(1, int.MaxValue)] int policyId)
        //{
        //    try
        //    {
        //        var promotions = promotionInvestors.Where(p => p.InvestorId == 1 && p.Id == policyId).Select(p => p.Promotions);
        //        return new APIResponse(Utils.StatusCode.Success, promotions, 200, "Ok");
        //    }
        //    catch (Exception ex)
        //    {
        //        return OkException(ex);
        //    }
        //}

        /// <summary>
        /// Lấy chính sách (sản phẩm), tất cả chính sách còn mở, cho show app
        /// </summary>
        /// <param name="cpsSecondaryId"></param>
        /// <returns></returns>
        [Authorize]
        [AuthorizeUserTypeFilter(UserTypes.INVESTOR)]
        [HttpGet("policy/{cpsSecondaryId}")]
        public APIResponse FindAllPolicy([Range(1, int.MaxValue)] int cpsSecondaryId)
        {
            try
            {
                var result = _cpsSecondaryServices.FindAllListPolicy(cpsSecondaryId);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Get kỳ hạn, còn show app của chính sách đang chọn
        /// <param name="policyId"></param>
        /// <param name="totalValue"></param>
        /// </summary>
        [Authorize]
        [AuthorizeUserTypeFilter(UserTypes.INVESTOR)]
        [HttpGet("policy-detail/{policyId}")]
        public APIResponse FindAllPolicyDetail(int policyId, [Range(1, double.MaxValue)][Required] decimal? totalValue)
        {
            try
            {
                var result = _cpsSecondaryServices.FindAllListPolicyDetail(policyId, totalValue ?? 0);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }
        #endregion
    }
}
