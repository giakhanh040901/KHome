using EPIC.LoyaltyDomain.Interfaces;
using EPIC.Utils.Controllers;
using EPIC.Utils.Net.MimeTypes;
using EPIC.WebAPIBase.FIlters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

namespace EPIC.LoyaltyAPI.Controllers
{
    [Authorize]
    [AuthorizeAdminUserTypeFilter]
    [Route("api/loyalty/export-excel-report")]
    [ApiController]
    public class LoyExportExcelReportController : BaseController
    {
        private readonly ILoyExportExcelReportServices _loyExportExcelReportServices;
        private readonly ILogger<LoyExportExcelReportController> _logger;

        public LoyExportExcelReportController(ILogger<LoyExportExcelReportController> logger,
            ILoyExportExcelReportServices loyExportExcelReportServices)
        {
            _loyExportExcelReportServices = loyExportExcelReportServices;
            _logger = logger;
        }

        /// <summary>
        /// Báo cáo danh sách yêu cầu đổi voucher
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        [HttpGet("report-conversion-point")]
        public IActionResult ReportConversionPoint(DateTime? startDate, DateTime? endDate)
        {
            try
            {
                var result = _loyExportExcelReportServices.ReportConversionPoint(startDate, endDate);
                return File(result.fileData, MimeTypeNames.ApplicationVndOpenxmlformatsOfficedocumentSpreadsheetmlSheet, "BaoCaoDanhSachYeuCauDoiVoucher.xlsx");
            }
            catch (Exception ex)
            {
                return Ok(OkException(ex));
            }
        }

        /// <summary>
        /// Báo cáo xuất nhập tồn voucher
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        [HttpGet("report-voucher")]
        public IActionResult ReportVoucher(DateTime? startDate, DateTime? endDate)
        {
            try
            {
                var result = _loyExportExcelReportServices.ReportVoucher(startDate, endDate);
                return File(result.fileData, MimeTypeNames.ApplicationVndOpenxmlformatsOfficedocumentSpreadsheetmlSheet, "BaoCaoNhapXuatTonVoucher.xlsx");
            }
            catch (Exception ex)
            {
                return Ok(OkException(ex));
            }
        }
    }
}
