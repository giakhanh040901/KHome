using EPIC.BondDomain.Interfaces;
using EPIC.CoreDomain.Interfaces;
using EPIC.CoreEntities.Dto.Sale;
using EPIC.Entities.Dto.Sale;
using EPIC.Notification.Services;
using EPIC.Utils;
using EPIC.Utils.Controllers;
using EPIC.Utils.Net.MimeTypes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace EPIC.CoreAPI.Controllers.AppController
{
    /// <summary>
    /// Đăng ký sale
    /// </summary>
    [Authorize]
    [Route("api/core/sale/registration")]
    [ApiController]
    public class AppSaleRegistrationController : BaseController
    {
        private readonly IHttpContextAccessor _httpContext;
        private readonly ISaleServices _saleServices;
        private readonly ISaleExportCollapContractServices _saleExportCollapContractServices;
        private readonly IInvestorServices _investorServices;
        private readonly NotificationServices _sendEmailServices;

        public AppSaleRegistrationController(
            ILogger<AppSaleRegistrationController> logger,
            ISaleServices saleServices,
            ISaleExportCollapContractServices saleExportCollapContractServices,
            IInvestorServices investorServices,
            IHttpContextAccessor httpContext,
            NotificationServices sendEmailServices)
        {
            _logger = logger;
            _httpContext = httpContext;
            _saleServices = saleServices;
            _saleExportCollapContractServices = saleExportCollapContractServices;
            _investorServices = investorServices;
            _sendEmailServices = sendEmailServices;
        }

        #region đăng ký cộng tác viên
        /// <summary>
        /// Tìm quản lý xem có tồn tại không, mã giới thiệu có là sale quản lý trên cây phòng ban, 
        /// nếu sale hiện tại đã tham gia vào 1 đại lý thì sale quản lý không được ở đại lý đang tham gia (báo lỗi)
        /// </summary>
        /// <param name="referralCode"></param>
        /// <returns></returns>
        [HttpGet("find-manager/{referralCode}")]
        public APIResponse FindManager(string referralCode)
        {
            try
            {
                var result = _saleServices.FindAllListManager(referralCode?.Trim());
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// đăng ký lưu vào bảng tạm và trình duyệt (lưu bảng core approve với tài khoản người quản lý được chỉ định duyệt)
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("register")]
        public async Task<APIResponse> Register([FromBody] AppSaleRegisterDto input)
        {
            try
            {
                var result = await _saleServices.AppSaleRegister(input);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }
        #endregion

        #region điều hướng
        /// <summary>
        /// Danh sách sale đăng ký (bao gồm trạng thái), xem danh sách theo id sale quản lý đang đăng nhập
        /// </summary>
        /// <param name="tradingProviderId">id đại lý để lọc ra danh sách những sale đã được điều hướng đến đại lý này</param>
        /// <returns></returns>
        [HttpGet("list-sale-register")]
        public APIResponse ListRegister([Range(1, int.MaxValue)] int? tradingProviderId)
        {
            try
            {
                var result = _saleServices.AppListSaleRegister(tradingProviderId);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Quản lý điều hướng sale vào đại lý nào, vai trò là gì
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("direction-sale")]
        public APIResponse DirectionSale([FromBody] AppDirectionSaleDto input)
        {
            try
            {
                _saleServices.ManagerDirectionSale(input);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Danh sách đại lý sơ cấp theo quản lý sale ở trong phòng ban
        /// </summary>
        /// <returns></returns>
        [HttpGet("list-trading-provider")]
        public APIResponse AppListTradingProvider()
        {
            try
            {
                var result = _saleServices.AppListTradingProvider();
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }
        #endregion

        /// <summary>
        /// Lấy danh sách hợp đồng cộng tác
        /// </summary>
        /// <returns></returns>
        [HttpGet("get-collab-contract")]
        public APIResponse ListCollabContract()
        {
            try
            {
                var result = _saleServices.ListCollabContract();
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// export contract tạm
        /// </summary>
        /// <returns></returns>
        [HttpGet("export-contract")]
        public async Task<IActionResult> ExportContract([Range(1, int.MaxValue)] int collapContractTempId, [Range(1, int.MaxValue)] int saleTempId)
        {
            try
            {
                var result = await _saleExportCollapContractServices.ExportContractApp(collapContractTempId, saleTempId);
                return File(result.fileData, MimeTypeNames.ApplicationOctetStream, result.fileDownloadName);
            }
            catch (Exception ex)
            {
                return Ok(OkException(ex));
            }
        }

        /// <summary>
        /// Api cho khách hàng ký vào hợp đồng xác nhận làm sale
        /// </summary>
        /// <returns></returns>
        [HttpPut("sale-temp-sign")]
        public async Task<APIResponse> AppSaleTempSign([FromBody] AppSaleSignDto input)
        {
            try
            {
                _investorServices.CheckOtp(input.Otp);
                var result = new List<AppSaleTempSignDto>();
                foreach (var item in input.ListSign)
                {
                    var saleTemp = _saleServices.AppSaleTempSign(item.SaleTempId, item.IsSign);
                    if (saleTemp == null)
                    {
                        _logger.LogError($"Không tìm thấy thông tin sale ký với SaleTempId: {item.SaleTempId}");
                    }    
                    else
                    {
                        result.Add(saleTemp);
                        if (item.IsSign == true)
                        {
                            var listContract = await _saleExportCollapContractServices.UpdateContractFileApp(saleTemp.SaleId, saleTemp.TradingProviderId);
                            //thông báo đăng ký sale thành công
                            await _sendEmailServices.SendEmailSaleRegisterSuccess(saleTemp.SaleId, saleTemp.TradingProviderId, listContract);
                        }
                    } 
                }
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Huỷ ký hợp đồng
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPut("sale-temp-cancel")]
        public APIResponse AppSaleTempCancel([FromBody] AppSaleCancelDto input)
        {
            try
            {
                foreach (var item in input.ListCancel)
                {
                    var saleTemp = _saleServices.AppSaleTempSign(item.SaleTempId, false);
                }
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }
    }
}
