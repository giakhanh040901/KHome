using EPIC.BondDomain.Interfaces;
using EPIC.CoreDomain.Implements;
using EPIC.CoreDomain.Interfaces;
using EPIC.CoreEntities.Dto.TradingFirstMessage;
using EPIC.Entities.DataEntities;
using EPIC.Entities.Dto.DigitalSign;
using EPIC.Entities.Dto.TradingProvider;
using EPIC.Entities.Dto.User;
using EPIC.Shared.Filter;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Identity;
using EPIC.Utils.Controllers;
using EPIC.WebAPIBase.FIlters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace EPIC.CoreAPI.Controllers
{
    [Authorize]
    [AuthorizeAdminUserTypeFilter]
    [ApiController]
    [Route("api/core/trading-provider")]
    public class TradingProviderController : BaseController
    {
        private readonly ITradingProviderServices _tradingProviderServices;
        private readonly ITradingFirstMessageServices _tradingFirstMessageServices;

        public TradingProviderController(ILogger<TradingProviderController> logger, ITradingProviderServices tradingProviderServices, ITradingFirstMessageServices tradingFirstMessageServices)
        {
            _logger = logger;
            _tradingProviderServices = tradingProviderServices;
            _tradingFirstMessageServices = tradingFirstMessageServices;
        }

        [HttpGet]
        [Route("find")]
        [PermissionFilter(Permissions.InvestDaiLy_DanhSach, Permissions.GarnerDaiLy_DanhSach, Permissions.BondCaiDat_DLSC_DanhSach, Permissions.RealStateDaiLy_DanhSach, Permissions.UserWeb, Permissions.CoreDaiLy_DanhSach)]
        public APIResponse GetAllTradingProvider(int pageNumber, int? pageSize, string keyword, int? status)
        {
            try
            {
                var tradingProvider = _tradingProviderServices.Find(pageSize ?? 100, pageNumber, keyword?.Trim(), status);
                return new APIResponse(Utils.StatusCode.Success, tradingProvider, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        [HttpGet]
        [Route("find-no-permission")]
        public APIResponse GetAllTradingProviderNoPermission(int pageNumber, int? pageSize, string keyword, int? status)
        {
            try
            {
                var tradingProvider = _tradingProviderServices.Find(pageSize ?? 100, pageNumber, keyword?.Trim(), status);
                return new APIResponse(Utils.StatusCode.Success, tradingProvider, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        [Route("find/{id}")]
        [HttpGet]
        public APIResponse GetTradingProvider(int id)
        {
            try
            {
                var result = _tradingProviderServices.FindById(id);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Lấy thông tin doanh nghiệp của đại lý đang đăng nhập
        /// </summary>
        /// <returns></returns>
        [HttpGet("get-info-current")]
        public APIResponse GetInfoTradingProviderCurrent()
        {
            try
            {
                var result = _tradingProviderServices.GetInfoTradingProviderCurrent();
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        [Route("find/tax-code/{taxCode}")]
        [HttpGet]
        public APIResponse GetTradingProviderTaxCode(string taxCode)
        {
            try
            {
                var result = _tradingProviderServices.FindByTaxCode(taxCode?.Trim());
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Tạo đlsc
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        [Route("add")] 
        [HttpPost]
        [PermissionFilter(Permissions.BondCaiDat_DLSC_ThemMoi, Permissions.CoreDaiLy_ThemMoi, Permissions.RealStateDaiLy_ThemMoi, Permissions.GarnerDaiLy_ThemMoi, Permissions.InvestDaiLy_ThemMoi)]
        public async Task<APIResponse> AddTradingProvider([FromBody] CreateTradingProviderDto body)
        {
            try
            {
                var result = await _tradingProviderServices.Add(body);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        [Route("delete/{id}")]
        [HttpDelete]
        [PermissionFilter(Permissions.BondCaiDat_DLSC_ThemMoi, Permissions.CoreDaiLy_ThemMoi, Permissions.RealStateDaiLy_ThemMoi, Permissions.GarnerDaiLy_ThemMoi, Permissions.InvestDaiLy_ThemMoi)]
        public APIResponse DeleteTradingProvider(int id)
        {
            try
            {
                var result = _tradingProviderServices.Delete(id);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        [HttpPost("user/create")]
        public APIResponse CreateUser([FromBody] CreateUserDto model)
        {
            try
            {
                _tradingProviderServices.CreateUser(model);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        [Authorize]
        [HttpPut("user/active/{userId}")]
        public APIResponse ActiveUser([FromRoute] int userId, bool isActive)
        {
            try
            {
                var result = _tradingProviderServices.ActiveUser(userId, isActive);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        [HttpPut("user/change-password")]
        public APIResponse ChangePasswordUser([FromBody] Entities.Dto.User.ChangePasswordDto input)
        {
            try
            {
                _tradingProviderServices.ChangePassword(input);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        [Authorize]
        [HttpGet("user/find-all-user")]
        public APIResponse FindAllUser(int pageSize, int pageNumber, string keyword)
        {
            try
            {
                var result = _tradingProviderServices.FindAll(pageSize, pageNumber, keyword);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        [Authorize]
        [HttpPut("user/update/{userId}")]
        public APIResponse UpdateUser([FromRoute] int userId, [FromBody] UpdateUserDto model)
        {
            try
            {
                var result = _tradingProviderServices.UpdateUser(userId, model);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        [Authorize]
        [HttpDelete("user/delete/{userId}")]
        public APIResponse DeleteUser(int userId)
        {
            try
            {
                var result = _tradingProviderServices.DeleteUser(userId);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Lấy danh sách ngân hàng theo DLSC đang đăng nhập
        /// </summary>
        /// <returns></returns>
        [HttpGet("list-bank")]
        public APIResponse FindBankByTrading()
        {
            try
            {
                var result = _tradingProviderServices.FindBankByTrading();
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Lấy thông tin chữ ký số
        /// </summary>
        /// <returns></returns>
        [HttpGet("get-digital-sign")]
        public APIResponse GetTradingDigitalSign()
        {
            try
            {
                var result = _tradingProviderServices.GetTradingDigitalSign();
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");

            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Cập nhập thông tin chữ ký số của khách hàng doanh nghiệp bảng chính
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut("update-digital-sign")]
        public APIResponse UpdateDigitalSign(DigitalSignDto input)
        {
            try
            {
                var result = _tradingProviderServices.UpdateTradingDigitalSign(input);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }
        /// <summary>
        /// Thay đổi trạng thái tradingProvider
        /// </summary>
        /// <param name="tradingProviderId"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        [HttpPut("change-status")]
        public APIResponse ChangeStatus(int tradingProviderId, int status)
        {
            try
            {
                var result = _tradingProviderServices.ChangeStatus(tradingProviderId, status);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Lưu tin nhắn rc theo đại lý
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("first-msg")]
        public APIResponse SaveTradingFirstMsg(SaveTradingFirstMessageDto input)
        {
            try
            {
                _tradingFirstMessageServices.SaveMessage(input);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Lấy tất cả danh sách cấu hình tin nhắn tự động
        /// </summary>
        /// <returns></returns>
        [HttpGet("first-msg")]
        public APIResponse FindAllTradingFirstMsg()
        {
            try
            {
                var data = _tradingFirstMessageServices.FindAll();
                return new APIResponse(Utils.StatusCode.Success, data, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Lấy cấu hình tin nhắn tự động theo đại lý
        /// </summary>
        /// <param name="tradingProviderId"></param>
        /// <returns></returns>
        [HttpGet("{tradingProviderId}/first-msg")]
        public APIResponse FindFirstMsgByTrading(int tradingProviderId)
        {
            try
            {
                var data = _tradingFirstMessageServices.FindByTrading(tradingProviderId);
                return new APIResponse(Utils.StatusCode.Success, data, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }
    }
}
