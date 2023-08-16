using EPIC.BondDomain.Interfaces;
using EPIC.InvestDomain.Interfaces;
using EPIC.Shared.Filter;
using EPIC.Utils.ConstantVariables.Identity;
using EPIC.Utils.Controllers;
using EPIC.WebAPIBase.FIlters;
using EPIC.Utils.Net.MimeTypes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

namespace EPIC.BondAPI.Controllers
{
    [Authorize]
    [AuthorizeAdminUserTypeFilter]
    [Route("api/bond/export-excel-report")]
    [ApiController]
    public class BondExportExcelReportController : BaseController
    {
        private readonly IBondExportReportService _exportReportService;
        public BondExportExcelReportController(ILogger<BondExportExcelReportController> logger, IBondExportReportService exportReportService)
        {
            _exportReportService = exportReportService;
            _logger = logger;
        }

        [HttpGet("export-file")]
        public IActionResult GetAll(DateTime? startDate, DateTime? endDate)
        {
            try
            {
                var result = _exportReportService.ExportExcelBondPackages(startDate, endDate);
                return File(result.fileData, MimeTypeNames.ApplicationVndOpenxmlformatsOfficedocumentSpreadsheetmlSheet, "TongHopCacGoiTP.xlsx");
            }
            catch (Exception ex)
            {
                return Ok(OkException(ex));
            }
        }

        [HttpGet("export-bond-investment")]
        public IActionResult ExportBondInvestment(DateTime? startDate, DateTime? endDate)
        {
            try
            {
                var result = _exportReportService.ExportBondInvestment(startDate, endDate);
                return File(result.fileData, MimeTypeNames.ApplicationVndOpenxmlformatsOfficedocumentSpreadsheetmlSheet, "TongHopCacKhoanDauTuTP.xlsx");
            }
            catch (Exception ex)
            {
                return Ok(OkException(ex));
            }
        }

        [HttpGet("export-bond-due")]
        public IActionResult ExportInterestPrincipalDue(DateTime? startDate, DateTime? endDate)
        {
            try
            {
                var result = _exportReportService.ExportInterestPrincipalDue(startDate, endDate);
                return File(result.fileData, MimeTypeNames.ApplicationVndOpenxmlformatsOfficedocumentSpreadsheetmlSheet, "BaoCaoTinhGocLaiDenHan.xlsx");
            }
            catch (Exception ex)
            {
                return Ok(OkException(ex));
            }
        }
    }
}
