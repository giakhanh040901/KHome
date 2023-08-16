using DocumentFormat.OpenXml.Wordprocessing;
using EPIC.CoreDomain.Interfaces;
using EPIC.Utils.Controllers;
using EPIC.WebAPIBase.FIlters;
using EPIC.Utils.Net.MimeTypes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

namespace EPIC.CoreAPI.Controllers
{
    [Authorize]
    [AuthorizeAdminUserTypeFilter]
    [Route("api/core/export-excel-report")]
    [ApiController]
    public class ExportExcelReportController : BaseController
    {
        private readonly IExportExcelReportServices _exportReportService;
        public ExportExcelReportController(ILogger<ExportExcelReportController> logger, IExportExcelReportServices exportReportService)
        {
            _exportReportService = exportReportService;
            _logger = logger;
        }

        [HttpGet("list-saler-report")]
        public IActionResult GetSalerListExcelReport(DateTime? startDate, DateTime? endDate)
        {
            try
            {
                var result = _exportReportService.SalerListExcelReport(startDate, endDate);
                return File(result.fileData, MimeTypeNames.ApplicationVndOpenxmlformatsOfficedocumentSpreadsheetmlSheet, "BaoCaoDanhSachSaler.xlsx");
            }
            catch (Exception ex)
            {
                return Ok(OkException(ex));
            }
        }

        [HttpGet("list-customer-report")]
        public IActionResult GetCustomerListExcelReport(DateTime? startDate, DateTime? endDate)
        {
            try
            {
                var result = _exportReportService.CustomerListExcelReport(startDate, endDate);
                return File(result.fileData, MimeTypeNames.ApplicationVndOpenxmlformatsOfficedocumentSpreadsheetmlSheet, "BaoCaoDanhSachKhachHang.xlsx");
            }
            catch (Exception ex)
            {
                return Ok(OkException(ex));
            }
        }

        [HttpGet("list-customer-root-report")]
        public IActionResult GetCustomerListRootExcelReport(DateTime? startDate, DateTime? endDate)
        {
            try
            {
                var result = _exportReportService.CustomerRootListExcelReport(startDate, endDate);
                return File(result.fileData, MimeTypeNames.ApplicationVndOpenxmlformatsOfficedocumentSpreadsheetmlSheet, "BaoCaoDanhSachKhachHangRoot.xlsx");
            }
            catch (Exception ex)
            {
                return Ok(OkException(ex));
            }
        }

        [HttpGet("list-user-report")]
        public IActionResult GetUserExcelReport(DateTime? startDate, DateTime? endDate)
        {
            try
            {
                var result = _exportReportService.UserExcelReport(startDate, endDate);
                return File(result.fileData, MimeTypeNames.ApplicationVndOpenxmlformatsOfficedocumentSpreadsheetmlSheet, "BaoCaoDanhSachNguoiDung.xlsx");
            }
            catch (Exception ex)
            {
                return Ok(OkException(ex));
            }
        }

        /// <summary>
        /// Báo cáo danh sách khách hàng HVF
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        [HttpGet("list-customer-hvf-report")]
        public IActionResult GetListCustomerHVSExcelReport(DateTime? startDate, DateTime? endDate)
        {
            try
            {
                var result = _exportReportService.ListCustomerHVF(startDate, endDate);
                return File(result.fileData, MimeTypeNames.ApplicationVndOpenxmlformatsOfficedocumentSpreadsheetmlSheet, "BaoCaoDanhSachKhachHangHVF.xlsx");
            }
            catch (Exception ex)
            {
                return Ok(OkException(ex));
            }
        }
        
        /// <summary>
        /// Báo cáo thay đổi thông tin khách hàng
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        [HttpGet("list-customer-info-change-report")]
        public IActionResult GetListCustomerInfoChangeReport(DateTime? startDate, DateTime? endDate)
        {
            try
            {
                var result = _exportReportService.CustomerInfoChangeReport(startDate, endDate);
                return File(result.fileData, MimeTypeNames.ApplicationVndOpenxmlformatsOfficedocumentSpreadsheetmlSheet, "BaoCaoThayDoiThongTinKhachHang.xlsx");
            }
            catch (Exception ex)
            {
                return Ok(OkException(ex));
            }
        }

        /// <summary>
        /// Báo cáo thay đổi thông tin khách hàng root
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        [HttpGet("list-customer-info-change-root-report")]
        public IActionResult GetListCustomerInfoRootChangeReport(DateTime? startDate, DateTime? endDate)
        {
            try
            {
                var result = _exportReportService.CustomerInfoChangeRootReport(startDate, endDate);
                return File(result.fileData, MimeTypeNames.ApplicationVndOpenxmlformatsOfficedocumentSpreadsheetmlSheet, "BaoCaoThayDoiThongTinKhachHangRoot.xlsx");
            }
            catch (Exception ex)
            {
                return Ok(OkException(ex));
            }
        }

        [HttpGet("list-user-no-ekyc-report")]
        public IActionResult GetListUserNoEkyc(DateTime? startDate, DateTime? endDate)
        {
            try
            {
                var result = _exportReportService.UserNoEkycExcelReport(startDate, endDate);
                return File(result.fileData, MimeTypeNames.ApplicationVndOpenxmlformatsOfficedocumentSpreadsheetmlSheet, "BaoCaoNguoiDungChuaEkyc.xlsx");
            }
            catch (Exception ex)
            {
                return Ok(OkException(ex));
            }
        }

    }
}
