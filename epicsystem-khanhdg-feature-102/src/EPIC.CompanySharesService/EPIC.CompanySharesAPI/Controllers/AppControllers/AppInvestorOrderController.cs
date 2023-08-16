using EPIC.CompanySharesDomain.Interfaces;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Identity;
using EPIC.Utils.Controllers;
using EPIC.WebAPIBase.FIlters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using System;
using EPIC.CompanySharesEntities.Dto.Order;
using EPIC.CompanySharesEntities.Dto.SaleInvestor;

namespace EPIC.CompanySharesAPI.Controllers.AppControllers
{
    /// <summary>
    /// Nhà đầu tư cá nhân đặt lệnh
    /// </summary>
    [Authorize]
    [AuthorizeUserTypeFilter(UserTypes.INVESTOR)]
    [Route("api/company-shares/investor-order")]
    [ApiController]
    public class AppInvestorOrderController : BaseController
    {
        private readonly IOrderServices _orderServices;
        private readonly ICpsSharedServices _cpsSharedService;
        private readonly IConfiguration _configuration;

        public AppInvestorOrderController(
            IOrderServices orderServices,
            ICpsSharedServices cpsSharedService,
            IConfiguration configuration
        )
        {
            _orderServices = orderServices;
            _cpsSharedService = cpsSharedService;
            _configuration = configuration;
        }

        #region lưu lệnh trên app, dòng tiền
        /// <summary>
        /// Kiểm tra lệnh
        /// </summary>
        /// <returns></returns>
        [Route("order/check")]
        [HttpPost]
        public APIResponse CheckOrder([FromBody] CheckOrderAppDto input)
        {
            try
            {
                _orderServices.CheckOrder(input);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        ///// <summary>
        ///// Get mô tả dòng tiền
        ///// </summary>
        //[HttpGet]
        //[Route("cash-flow")]
        //public APIResponse GetCashFlow([Range(1, double.MaxValue)] decimal totalValue, [Range(1, int.MaxValue)] int policyDetailId)
        //{
        //    try
        //    {
        //        var result = _cpsSharedService.GetCashFlow(totalValue, policyDetailId);
        //        return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
        //    }
        //    catch (Exception ex)
        //    {
        //        return OkException(ex);
        //    }
        //}

        ///// <summary>
        ///// Get danh sách hợp đồng mẫu
        ///// </summary>
        //[HttpGet]
        //[Route("contract/find")]
        //public APIResponse GetContract([Range(1, int.MaxValue)] int policyDetailId)
        //{
        //    try
        //    {
        //        var contracts = _contractTemplateServices.FindAllForApp(policyDetailId);
        //        return new APIResponse(Utils.StatusCode.Success, contracts, 200, "Ok");
        //    }
        //    catch (Exception ex)
        //    {
        //        return OkException(ex);
        //    }
        //}

        /// <summary>
        /// Get danh sách tài khoản thụ hưởng
        /// </summary>
        [HttpGet("bank-account/find")]
        public APIResponse GetBankAccount()
        {
            try
            {
                var result = _orderServices.FindAllListBank();
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Get ngân hàng hỗ trợ
        /// </summary>
        /// <returns></returns>
        [HttpGet("find-support-bank")]
        public APIResponse GetSupportBank(string keyword)
        {
            try
            {
                var result = _orderServices.GetListBankSupport(keyword);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Check mã OTP, lưu order, đồng thời sinh file đã ký nếu không có file hoặc file không tồn tại vẫn cho lưu lệnh thành công như bình thường
        /// </summary>
        /// <returns></returns>
        [HttpPost("order/add")]
        public APIResponse OrderAdd([FromBody] CreateOrderAppDto input)
        {
            try
            {
                var result = _orderServices.InvestorAdd(input);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Sale đặt lệnh hộ investor
        /// </summary>
        /// <returns></returns>
        [HttpPost("order/sale-add")]
        public APIResponse SaleOrderAdd([FromBody] SaleInvestorAddOrderDto input)
        {
            try
            {
                var result = _orderServices.SaleAddInvestorOrder(input);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Check sale trước khi đặt lệnh. Nếu thỏa mãn các điều kiện thì trả về thông tin của sale
        /// </summary>
        /// <param name="referralCode"></param>
        /// <param name="secondaryId"></param>
        /// <returns></returns>
        [HttpGet("order/add/check/sale/{referralCode}/secondary/{secondaryId}")]
        public APIResponse CheckSalerBeforeAddOrder(string referralCode, int secondaryId)
        {
            try
            {
                _logger.LogInformation("MGT => ", referralCode);
                _logger.LogInformation("MSP => ", secondaryId);
                var result = _orderServices.CheckSaleBeforeAddOrder(referralCode, secondaryId);
                return new APIResponse(result == null ? Utils.StatusCode.Error : Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Tìm kiếm sale theo mã giới thiệu đặt lệnh
        /// </summary>
        /// <param name="referralCode"></param>
        /// <param name="policyDetailId"></param>
        /// <returns></returns>
        [HttpGet("order/find-referral-code")]
        public APIResponse AppSaleOrderFindReferralCode(string referralCode, [Range(1, int.MaxValue)] int policyDetailId)
        {
            try
            {
                var result = _orderServices.AppSaleOrderFindReferralCode(referralCode?.Trim(), policyDetailId);
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
