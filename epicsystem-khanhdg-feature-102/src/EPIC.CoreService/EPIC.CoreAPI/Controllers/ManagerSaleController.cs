using EPIC.CoreDomain.Interfaces;
using EPIC.Entities.Dto.Sale;
using EPIC.Shared.Filter;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Identity;
using EPIC.Utils.Controllers;
using EPIC.WebAPIBase.FIlters;
using EPIC.Utils.Net.MimeTypes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using EPIC.CoreEntities.Dto.Sale;

namespace EPIC.CoreAPI.Controllers
{
    /// <summary>
    /// quản lý sale
    /// </summary>
    [Authorize]
    [AuthorizeAdminUserTypeFilter]
    [Route("api/core/manager-sale")]
    [ApiController]
    public class ManagerSaleController : BaseController
    {
        private readonly ISaleServices _saleServices;
        private readonly ISaleExportCollapContractServices _saleExportCollapContractServices;
        private readonly IHttpContextAccessor _httpContext;
        public ManagerSaleController(
            ILogger<ManagerSaleController> logger,
            IHttpContextAccessor httpContext,
            ISaleServices saleServices,
            ISaleExportCollapContractServices saleExportCollapContractServices
        )
        {
            _logger = logger;
            _saleServices = saleServices;
            _saleExportCollapContractServices = saleExportCollapContractServices;
            _httpContext = httpContext;
        }

        /// <summary>
        /// Tìm investor, gán vào phòng ban, trả ra sale temp, thêm trên trang quản trị
        /// </summary>
        /// <returns></returns>
        [HttpPost("add")]
        [PermissionFilter(Permissions.CoreDuyetSale_ThemMoi)]
        public APIResponse AddSale(AddSaleDto input)
        {
            try
            {
                var result = _saleServices.AddSaleTemp(input);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// danh sách sale temp,
        /// keyword tìm theo tên, idNo, phone, email, mã giới thiệu
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet("find-all-temp")]
        [PermissionFilter(Permissions.CoreDuyetSale_DanhSach)]
        public APIResponse FindAllTemp([FromQuery] FilterSaleTempDto input)
        {
            try
            {
                var result = _saleServices.FindAllSaleTemp(input);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// lấy lịch sử chỉnh sửa sale theo saleId phân trang
        /// </summary>
        /// <param name="saleId"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageNumber"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        [HttpGet("find-all-history-sale")]
        public APIResponse FindAllHistorySale(int saleId ,int pageSize, int pageNumber, string keyword)
        {
            try
            {
                var result = _saleServices.FindAllHistorySale(saleId, pageSize, pageNumber, keyword?.Trim());
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Xóa sale Temp
        /// </summary>
        /// <param name="saleTempId"></param>
        /// <returns></returns>
        [HttpDelete("delete-temp/{saleTempId}")]
        public APIResponse SaleTempDelete(int saleTempId)
        {
            try
            {
                var result = _saleServices.DeleteSaleTemp(saleTempId);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// cập nhật thông tin sale không chuyển phòng bao gồm loại sale, nếu đang là cộng tác viên chuyển thành nhân viên hoặc quản lý thì bỏ SALE_PARENT_ID, 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut("sale-temp-update")]
        public APIResponse SaleTempUpdate(UpdateSaleTempDto input)
        {
            try
            {
                var result = _saleServices.UpdateSaleTemp(input);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// cập nhật thông tin sale khi tạo trên Cms không chuyển phòng bao gồm loại sale, nếu đang là cộng tác viên chuyển thành nhân viên hoặc quản lý thì bỏ SALE_PARENT_ID, 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut("sale-temp-update-cms")]
        [PermissionFilter(Permissions.CoreDuyetSale_CapNhat)]
        public APIResponse SaleTempUpdateCms(UpdateSaleTempCmsDto input)
        {
            try
            {
                var result = _saleServices.UpdateSaleTempCms(input);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        [HttpPut("sale-update")]
        [PermissionFilter(Permissions.CoreSaleActive_CapNhat)]
        public APIResponse UpdateSale(UpdateSaleDto input)
        {
            try
            {
                var result = _saleServices.Update(input);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        //danh sách sale cộng tác viên của sale truyền id vào

        /// <summary>
        /// Trình duyệt sale temp
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("request")]
        [PermissionFilter(Permissions.CoreDuyetSale_TrinhDuyet)]
        public APIResponse AddSaleRequest([FromBody] RequestSaleDto input)
        {
            try
            {
                _saleServices.AddRequestSale(input);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Duyệt sale
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut("approve")]
        [PermissionFilter(Permissions.CoreQLPD_Sale_PheDuyetOrHuy)]
        public APIResponse ApproveSale([FromBody] ApproveSaleDto input)
        {
            try
            {
                _saleServices.ApproveSale(input);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Hủy duyệt trong trạng thái trình duyệt
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut("cancel")]
        [PermissionFilter(Permissions.CoreQLPD_Sale_PheDuyetOrHuy)]
        public APIResponse CancelSale([FromBody] CancelSaleDto input)
        {
            try
            {
                _saleServices.CancelSale(input);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Lấy danh sách sale 
        /// Sử dụng cho lọc với danh sách sale trong phòng ban
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet("find-all-list-sale")]
        public APIResponse FindAllListSale([FromQuery] FilterSaleSto input)
        {
            try
            {
                var result = _saleServices.FindAllSale(input);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// danh sách sale bảng thật,
        /// keyword tìm theo tên, idNo, phone, email, mã giới thiệu,
        /// nếu truyền departmentId 
        /// </summary>
        /// <returns></returns>
        [HttpGet("find-all")]
        [PermissionFilter(Permissions.CoreSaleActive_DanhSach)]
        public APIResponse FindAll([FromQuery] FilterSaleSto input)
        {
            try
            {
                var result = _saleServices.FindAllSale(input);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// kích hoạt, khóa sale
        /// </summary>
        /// <param name="saleId"></param>
        /// <returns></returns>
        [HttpPut("active/{saleId}")]
        [PermissionFilter(Permissions.CoreSaleActive_KichHoat)]
        public APIResponse ActiveSale(int saleId)
        {
            try
            {
                var result = _saleServices.Active(saleId);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Tìm kiếm quản lý sale theo ĐLSC đang đăng nhập
        /// </summary>
        /// <param name="referralCode"></param>
        /// <returns></returns>
        [HttpGet("find-manager/{referralCode}")]
        public APIResponse FindAllListManagerTrading(string referralCode)
        {
            try
            {
                var result = _saleServices.FindAllListManagerTrading(referralCode);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Lấy chi tiết SaleTemp
        /// </summary>
        /// <param name="saleTempId"></param>
        /// <returns></returns>
        [HttpGet("find-temp/{saleTempId}")]
        [PermissionFilter(Permissions.CoreDuyetSale_ThongTinSale, Permissions.CoreQLPD_Sale_ThongTinChiTiet)]
        public APIResponse FindSaleTemp(int saleTempId)
        {
            try
            {
                var result = _saleServices.FindSaleTemp(saleTempId);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Lấy chi tiết Sale
        /// </summary>
        /// <param name="saleId"></param>
        /// <returns></returns>
        [HttpGet("find/{saleId}")]
        [PermissionFilter(Permissions.CoreSaleActive_ThongTinSale)]
        public APIResponse SaleFindById(int saleId)
        {
            try
            {
                var result = _saleServices.SaleFindById(saleId);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        [HttpDelete("delete/{saleId}")]
        public APIResponse Delete(int saleId)
        {
            try
            {
                var result = _saleServices.Delete(saleId);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Danh sách sale đăng ký trên app, xem bởi EPIC
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet("find-all-register")]
        [PermissionFilter(Permissions.CoreSaleApp_DanhSach)]
        public APIResponse FindAllSaleRegister([FromQuery] FilterSaleRegisterDto input)
        {
            try
            {
                var result = _saleServices.FindAllSaleRegister(input);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// tìm kiếm saler theo mã giới thiệu thuộc đlsc
        /// lỗi nếu: 1. không thấy mã gt đó là sale, 2. mã gt sale không thuộc đại lý, 3. mã gt sale không còn hoạt động
        /// </summary>
        /// <returns></returns>
        [HttpGet("find-by-trading-provider")]
        public APIResponse FindByTradingProvider(string referralCode, string phone)
        {
            try
            {
                var result = _saleServices.FindSaleByReferralCode(referralCode?.Trim(), phone?.Trim());
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Update (file lại data) hợp đồng
        /// </summary>
        /// <param name="saleId"></param>
        /// <returns></returns>
        [Route("update-contract-file")]
        [HttpPut]
        [PermissionFilter(Permissions.CoreSaleActive_HDCT_UpdateFile)]
        public APIResponse UpdateContractFile(int saleId)
        {
            try
            {
                _saleExportCollapContractServices.UpdateContractFile(saleId);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Success");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Ký hợp đồng
        /// </summary>
        /// <param name="saleId"></param>
        /// <returns></returns>
        [Route("sign-contract-file")]
        [HttpPut]
        [PermissionFilter(Permissions.CoreSaleActive_HDCT_Sign)]
        public APIResponse SignContractFile(int saleId)
        {
            try
            {
                _saleExportCollapContractServices.UpdateContractFileSignPdf(saleId);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Success");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Tải file lưu trữ
        /// </summary>
        /// <param name="collabContractId"></param>
        /// <returns></returns>
        [HttpGet("collab-contract/export-file-scan")]
        public IActionResult ExportFileScan(int collabContractId)
        {
            try
            {
                var result = _saleExportCollapContractServices.ExportContractScan(collabContractId);
                return File(result.fileData, MimeTypeNames.ApplicationOctetStream, result.fileDownloadName);
            }
            catch (Exception ex)
            {
                return Ok(OkException(ex));
            }
        }

        /// <summary>
        /// Tải file mẫu
        /// </summary>
        /// <param name="collabContractId"></param>
        /// <returns></returns>
        [HttpGet("collab-contract/export-file-temp")]
        //[PermissionFilter(Permissions.CoreSaleActive_HDCT_Download)]
        public IActionResult ExportFileTemp(int collabContractId)
        {
            try
            {
                var result = _saleExportCollapContractServices.ExportContractTemp(collabContractId);
                return File(result.fileData, MimeTypeNames.ApplicationOctetStream, result.fileDownloadName);
            }
            catch (Exception ex)
            {
                return Ok(OkException(ex));
            }
        }

        /// <summary>
        /// Tải file đã ký
        /// </summary>
        /// <param name="collabContractId"></param>
        /// <returns></returns>
        [HttpGet("collab-contract/export-file-signature")]
        //[PermissionFilter(Permissions.CoreSaleActive_HDCT_Download_Sign)]
        public IActionResult ExportFileSignature(int collabContractId)
        {
            try
            {
                var result = _saleExportCollapContractServices.ExportContractSignature(collabContractId);
                return File(result.fileData, MimeTypeNames.ApplicationOctetStream, result.fileDownloadName);
            }
            catch (Exception ex)
            {
                return Ok(OkException(ex));
            }
        }

        /// <summary>
        /// Sửa file scan hợp đồng
        /// </summary>
        /// <returns></returns>
        [HttpPut("collab-contract/update-scan-contract")]
        public APIResponse SecondaryContractScanUpdate([FromBody] UpdateSaleCollabContractDto input)
        {
            try
            {
                var result = _saleExportCollapContractServices.UpdateCollabContractFileScan(input);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Thêm file scan hợp đồng
        /// </summary>
        /// <returns></returns>
        [HttpPost("collab-contract/add-scan-contract")]
        //[PermissionFilter(Permissions.CoreSaleActive_HDCT_UploadFile)]
        public APIResponse SecondaryContractScanAdd([FromBody] CreateSaleCollabContractDto input)
        {
            try
            {
                var result = _saleExportCollapContractServices.CreateCollabContractFileScan(input);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Root điều hướng sale vào đại lý nào, vai trò là gì
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("root-direction-sale")]
        [PermissionFilter(Permissions.CoreSaleApp_DieuHuong)]
        public APIResponse RootDirectionSale([FromBody] AppDirectionSaleDto input)
        {
            try
            {
                _saleServices.RootDirectionSale(input);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        ///  Lấy danh sách đại lý của sale quản lý đang thuộc
        ///  Root dùng để điều hướng
        /// </summary>
        /// <param name="managerSaleId"></param>
        /// <returns></returns>
        [HttpGet("trading-provider-by-manager-sale")]
        public APIResponse AppListTradingProviderByManagerSale(int managerSaleId)
        {
            try
            {
                var result = _saleServices.AppListTradingProviderByManagerSale(managerSaleId);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

 
        /// <summary>
        /// export contract receive
        /// </summary>
        /// <returns></returns>
        [HttpGet("file-template-word")]
        public IActionResult ExportContractWordTest([Range(1, int.MaxValue)] int tradingProviderId, [Range(1, int.MaxValue)] int contractTemplateId)
        {
            try
            {
                var result = _saleExportCollapContractServices.ExportContractWordTest(tradingProviderId, contractTemplateId);
                return File(result.fileData, MimeTypeNames.ApplicationOctetStream, result.fileDownloadName);
            }
            catch (Exception ex)
            {
                return Ok(OkException(ex));
            }
        }

        /// <summary>
        /// export contract receive
        /// </summary>
        /// <returns></returns>
        [HttpGet("file-template-pdf")]
        public async Task<IActionResult> ExportContractPdfTest([Range(1, int.MaxValue)] int tradingProviderId, [Range(1, int.MaxValue)] int contractTemplateId)
        {
            try
            {
                var result = await _saleExportCollapContractServices.ExportContractPdfTest(tradingProviderId, contractTemplateId);
                return File(result.fileData, MimeTypeNames.ApplicationOctetStream, result.fileDownloadName);
            }
            catch (Exception ex)
            {
                return Ok(OkException(ex));
            }
        }
    }
}
