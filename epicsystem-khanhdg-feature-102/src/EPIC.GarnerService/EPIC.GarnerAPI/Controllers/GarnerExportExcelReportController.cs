using EPIC.GarnerDomain.Interfaces;
using EPIC.InvestDomain.Interfaces;
using EPIC.Shared.Filter;
using EPIC.Utils.ConstantVariables.Identity;
using EPIC.Utils.Controllers;
using EPIC.WebAPIBase.FIlters;
using EPIC.Utils.Net.MimeTypes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace EPIC.GarnerAPI.Controllers
{
    [Authorize]
    [AuthorizeAdminUserTypeFilter]
    [Route("api/garner/export-excel-report")]
    [ApiController]
    public class GarnerExportExcelReportController : BaseController
    {
        private readonly IGarnerExportExcelReportServices _garnerExportExcelReportServices;
        public GarnerExportExcelReportController(ILogger<GarnerExportExcelReportController> logger, IGarnerExportExcelReportServices garnerExportExcelReportServices)
        {
            _garnerExportExcelReportServices = garnerExportExcelReportServices;
            _logger = logger;
        }

        /// <summary>
        /// Báo cáo tổng tri trả đầu tư
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        [HttpGet("export-total-investment-payment")]
        public IActionResult TotalInvestmentPayment(DateTime? startDate, DateTime? endDate)
        {
            try
            {
                var result = _garnerExportExcelReportServices.TotalInvestmentPayment(startDate, endDate);
                return File(result.fileData, MimeTypeNames.ApplicationVndOpenxmlformatsOfficedocumentSpreadsheetmlSheet, "BaoCaoTongChiTraDauTu.xlsx");
            }
            catch (Exception ex)
            {
                return Ok(OkException(ex));
            }
        }

        /// <summary>
        /// Báo cáo tổng hợp các khoản đầu tư
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        [HttpGet("export-list-total-investment")]
        public async Task<IActionResult> ListTotalInvestment(DateTime? startDate, DateTime? endDate)
        {
            try
            {
                var result = await _garnerExportExcelReportServices.ExcelGarnerTotalInvestmentAsync(startDate, endDate);
                return File(result.fileData, MimeTypeNames.ApplicationVndOpenxmlformatsOfficedocumentSpreadsheetmlSheet, "BaoCaoTongHopCacKhoanDauTu.xlsx");
            }
            catch (Exception ex)
            {
                return Ok(OkException(ex));
            }
        }

        /// <summary>
        /// Báo cáo quản trị tổng hợp
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        [HttpGet("export-garner-list-administration")]
        public async Task<IActionResult> ExcelGarnerListAdministrationAsync(DateTime? startDate, DateTime? endDate)
        {
            try
            {
                var result = await _garnerExportExcelReportServices.ExcelGarnerListAdministrationAsync(startDate, endDate);
                return File(result.fileData, MimeTypeNames.ApplicationVndOpenxmlformatsOfficedocumentSpreadsheetmlSheet, "BaoCaoQuantriTongHop.xlsx");
            }
            catch (Exception ex)
            {
                return Ok(OkException(ex));
            }
        }

        /// <summary>
        /// Báo cáo các sản phẩm tích lũy
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        [HttpGet("excel-list-garner-product")]
        public IActionResult ExcelListGarnerProduct(DateTime? startDate, DateTime? endDate)
        {
            try
            {
                var result = _garnerExportExcelReportServices.ExcelListGarnerProduct(startDate, endDate);
                return File(result.fileData, MimeTypeNames.ApplicationVndOpenxmlformatsOfficedocumentSpreadsheetmlSheet, "BaoCaoCacSanPhamTichLuy.xlsx");
            }
            catch (Exception ex)
            {
                return Ok(OkException(ex));
            }
        }

        [HttpGet("excel-list-actual-payment")]
        public async Task<IActionResult> ExcelListActualPayment(DateTime? startDate, DateTime? endDate)
        {
            try
            {
                var result = await _garnerExportExcelReportServices.ExcelGarnerActualPaymentAsync(startDate, endDate);
                return File(result.fileData, MimeTypeNames.ApplicationVndOpenxmlformatsOfficedocumentSpreadsheetmlSheet, "BaoCaoTichLuyThucChi.xlsx");
            }
            catch (Exception ex)
            {
                return Ok(OkException(ex));
            }
        }

        /// <summary>
        /// Báo cáo các khoản đầu tư bán hộ
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        [HttpGet("excel-list-sale-investment")]
        public async Task<IActionResult> ExcelListSaleInvestment(DateTime? startDate, DateTime? endDate)
        {
            try
            {
                var result = await _garnerExportExcelReportServices.ExcelGarnerSaleInvestmentAsync(startDate, endDate);
                return File(result.fileData, MimeTypeNames.ApplicationVndOpenxmlformatsOfficedocumentSpreadsheetmlSheet, "BaoCaoCacKhoanDauTuBanHo.xlsx");
            }
            catch (Exception ex)
            {
                return Ok(OkException(ex));
            }
        }
    }
}
