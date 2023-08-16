using EPIC.CoreDomain.Interfaces;
using EPIC.CoreEntities.Dto.Sale;
using EPIC.Utils;
using EPIC.Utils.Controllers;
using EPIC.Utils.Net.MimeTypes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace EPIC.CoreAPI.Controllers.AppController
{
    /// <summary>
    /// Hồ sơ cộng tác viên
    /// </summary>
    [Authorize]
    [Route("api/core/sale/profile")]
    [ApiController]
    public class AppSaleProfileController : BaseController
    {
        private readonly ISaleServices _saleServices;
        private readonly ISaleExportCollapContractServices _saleExportCollapContractServices;

        public AppSaleProfileController(ILogger<AppSaleProfileController> logger, ISaleServices saleServices, ISaleExportCollapContractServices saleExportCollapContractServices)
        {
            _logger = logger;
            _saleServices = saleServices;
            _saleExportCollapContractServices = saleExportCollapContractServices;
        }

        /// <summary>
        /// Hồ sơ của tôi thuộc đối tác đại lý sơ cấp nào
        /// </summary>
        /// <param name="tradingProviderId"></param>
        /// <returns></returns>
        [HttpGet("info/{tradingProviderId}")]
        public APIResponse Info([Range(1, int.MaxValue)] int tradingProviderId)
        {
            try
            {
                var result = _saleServices.AppSaleInfo(tradingProviderId);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Kiểm tra trạng thái của Sale
        /// </summary>
        /// <returns></returns>
        [HttpGet("check-sale")]
        public APIResponse AppCheckSaler()
        {
            try
            {
                var result = _saleServices.AppCheckSaler();
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Kiểm tra trạng thái Sale theo ĐLSC
        /// </summary>
        /// <param name="tradingProviderId"></param>
        /// <returns></returns>
        [HttpGet("check-status/{tradingProviderId}")]
        public APIResponse AppSaleCheckStatus([Range(1, int.MaxValue)] int tradingProviderId)
        {
            try
            {
                var result = _saleServices.AppSaleCheckStatusByTrading(tradingProviderId);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// lấy danh sách tài khoản ngân hàng
        /// </summary>
        /// <returns></returns>
        [HttpGet("bank/list")]
        public APIResponse ListBanks()
        {
            try
            {
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// chọn tài khoản thụ hưởng mặc định không phải trình duyệt
        /// </summary>
        /// <returns></returns>
        [HttpPost("bank/default/{bankAccId}")]
        public APIResponse DefaultBank([Range(1, int.MaxValue)] int bankAccId)
        {
            try
            {
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// danh sách các đại lý đang tham gia,
        /// dùng cho quản lý điều phối hoặc chỉ để xem thông tin
        /// theo sale
        /// </summary>
        /// <returns></returns>
        [HttpGet("trading-provider/list")]
        public APIResponse ListTradingProvider()
        {
            try
            {
                var result = _saleServices.AppListTradingProviderBySale();
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Danh sách đại lý sơ cấp của Sale bao gồm cả đại lý đang chờ ký duyệt và trạng thái của sale
        /// </summary>
        /// <returns></returns>
        [HttpGet("trading-provider/list-status")]
        public APIResponse AppListTradingProviderBySaleAndStatus()
        {
            try
            {
                var result = _saleServices.AppListTradingProviderBySaleAndStatus();
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Lấy danh sách hợp đồng cộng tác
        /// </summary>
        /// <returns></returns>
        [HttpGet("collaboration-contract")]
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
        /// Api cho khách hàng ký vào hợp đồng xác nhận làm sale
        /// </summary>
        /// <param name="saleTempId"></param>
        /// <param name="isSign"></param>
        /// <returns></returns>
        [HttpPut("sale-temp-sign")]
        public async Task<APIResponse> AppSaleTempSign([Range(1, int.MaxValue)] int saleTempId, bool isSign)
        {
            try
            {
                var result =  _saleServices.AppSaleTempSign(saleTempId, isSign);
                if (isSign == true)
                {
                    await _saleExportCollapContractServices.UpdateContractFileApp(result.SaleId, result.TradingProviderId);
                }
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// danh sách thông tin các sale con
        /// </summary>
        /// <param name="tradingProviderId"></param>
        /// <returns></returns>
        [HttpGet("manager-sale-child")]
        public APIResponse AppManagerSaleChild([Range(1, int.MaxValue)] int tradingProviderId)
        {
            try
            {
                var result = _saleServices.AppManagerSaleChild(tradingProviderId);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Xem thông tin sale con dành cho quản lý
        /// </summary>
        /// <param name="saleId"></param>
        /// <param name="tradingProviderId"></param>
        /// <returns></returns>
        [HttpGet("find-sale-child")]
        public APIResponse AppFindSaleChild([Range(1, int.MaxValue)] int saleId, [Range(1, int.MaxValue)] int tradingProviderId)
        {
            try
            {
                var result = _saleServices.AppFindSaleChild(saleId, tradingProviderId);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Lịch sử đăng ký của sale
        /// </summary>
        /// <returns></returns>
        [HttpGet("history-register")]
        public APIResponse AppHistoryRegister()
        {
            try
            {
                var result = _saleServices.AppHistoryRegister();
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// xuất file hợp đồng có chữ đã ký
        /// </summary>
        /// <param name="collabContractId">id file </param>
        /// <param name="tradingProviderId"></param>
        /// <returns></returns>
        [HttpGet("export-contract-signature")]
        public IActionResult ExportFileSignature([Range(1, int.MaxValue)] int collabContractId, int tradingProviderId)
        {
            try
            {
                var result = _saleExportCollapContractServices.AppExportContractSignature(collabContractId, tradingProviderId);
                return File(result.fileData, MimeTypeNames.ApplicationOctetStream, result.fileDownloadName);
            }
            catch (Exception ex)
            {
                return Ok(OkException(ex));
            }
        }

        /// <summary>
        /// Get danh sách file hợp đồng đã ký
        /// </summary>
        [HttpGet]
        [Route("contract/find-file-signature/{tradingProviderId}")]
        public APIResponse FindAllFileSignatureForApp([Range(1, int.MaxValue)] int tradingProviderId)
        {
            try
            {
                var contracts = _saleExportCollapContractServices.FindAllFileSignatureForApp(tradingProviderId);
                return new APIResponse(Utils.StatusCode.Success, contracts, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Bật tự động điều hướng
        /// </summary>
        /// <returns></returns>
        [HttpPut("change-auto-direction")]
        public APIResponse ChangeAutoDirection()
        {
            try
            {
                _saleServices.ChangeAutoDirection();
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }
    }
}
