using EPIC.InvestDomain.Implements;
using EPIC.InvestDomain.Interfaces;
using EPIC.InvestEntities.Dto.ExportExcel;
using EPIC.InvestEntities.Dto.InterestPayment;
using EPIC.Shared.Filter;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Garner;
using EPIC.Utils.ConstantVariables.Identity;
using EPIC.Utils.ConstantVariables.Shared;
using EPIC.Utils.Controllers;
using EPIC.WebAPIBase.FIlters;
using EPIC.Utils.Net.MimeTypes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using static EPIC.Utils.ConstantVariables.Shared.ExcelReport;
using System.Threading.Tasks;

namespace EPIC.InvestAPI.Controllers
{
    [Authorize]
    [AuthorizeAdminUserTypeFilter]
    [Route("api/invest/export-excel-report")]
    [ApiController]
    public class ExportExcelReportController : BaseController
    {
        private readonly IExportExcelReportService _exportExcelReportService;
        public ExportExcelReportController(ILogger<ExportExcelReportController> logger, IExportExcelReportService exportExcelReportService)
        {
            _exportExcelReportService = exportExcelReportService;
            _logger = logger;
        }

        /// <summary>
        /// Báo cáo tổng hợp các mã BĐS
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        [HttpGet("export-invest-code-excel-report")]
        [PermissionFilter(Permissions.Invest_Menu_BaoCao)]
        public IActionResult GetAllInvestCode(DateTime? startDate, DateTime? endDate)
        {
            try
            {
                var result = _exportExcelReportService.FindAllInvestCode(startDate, endDate);
                return File(result.fileData, MimeTypeNames.ApplicationVndOpenxmlformatsOfficedocumentSpreadsheetmlSheet, "BaoCaoCacMaBDS.xlsx");
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
        [HttpGet("export-investment-excel-report")]
        [PermissionFilter(Permissions.Invest_Menu_BaoCao)]
        public IActionResult GetAllInvestment(DateTime? startDate, DateTime? endDate)
        {
            try
            {
                var result = _exportExcelReportService.ExportInvestmentReport(startDate, endDate);
                return File(result.fileData, MimeTypeNames.ApplicationVndOpenxmlformatsOfficedocumentSpreadsheetmlSheet, "BaoCaoTongHopCacKhoanDauTu.xlsx");
            }
            catch (Exception ex)
            {
                return Ok(OkException(ex));
            }
        }

        /// <summary>
        /// Báo cáo dự chi đến hạn
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        [HttpGet("due-expend-excel-report")]
        [PermissionFilter(Permissions.Invest_Menu_BaoCao)]
        public async Task<IActionResult> DueExpendExcelReport(DateTime? startDate, DateTime? endDate)
        {
            try
            {
                var result = await _exportExcelReportService.DueExpendExcelReport(startDate, endDate);
                //return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
                return File(result.fileData, MimeTypeNames.ApplicationVndOpenxmlformatsOfficedocumentSpreadsheetmlSheet, "BaoCaoDuChi.xlsx");
            }
            catch (Exception ex)
            {
                return Ok(OkException(ex));
            }
        }

        /// <summary>
        /// Báo cáo thực chi
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        [HttpGet("actual-expend-report")]
        [PermissionFilter(Permissions.Invest_Menu_BaoCao)]
        public IActionResult ActualExpend(DateTime? startDate, DateTime? endDate)
        {
            try
            {
                var result = _exportExcelReportService.ActualExpendReport(startDate, endDate);
                return File(result.fileData, MimeTypeNames.ApplicationVndOpenxmlformatsOfficedocumentSpreadsheetmlSheet, "BaoCaoThucChi.xlsx");
                //return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");

            }
            catch (Exception ex)
            {
                return Ok(OkException(ex));
            }
        }

        /// <summary>
        /// Báo cáo tổng các khoản thực chi trong một ngày
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        [HttpGet("actual-total-expend-in-day")]
        [PermissionFilter(Permissions.Invest_Menu_BaoCao)]
        public IActionResult ActualTotalExpendInDate(DateTime? startDate, DateTime? endDate)
        {
            try
            {
                var result = _exportExcelReportService.ActualTotalExpendInDay(startDate, endDate);
                return File(result.fileData, MimeTypeNames.ApplicationVndOpenxmlformatsOfficedocumentSpreadsheetmlSheet, "BaoCaoTongChiTraDauTu.xlsx");
            }
            catch (Exception ex)
            {
                return Ok(OkException(ex));
            }
        }

        /// <summary>
        /// Báo cáo tổng hợp vận hành
        /// </summary>
        [HttpGet("total-operation-report-list-day")]
        [PermissionFilter(Permissions.Invest_Menu_BaoCao)]
        public IActionResult TotalOperationReportDto(DateTime? startDate, DateTime? endDate)
        {
            try
            {
                var result = _exportExcelReportService.TotalOperationReport(startDate, endDate);
                //return File(result.fileData, MimeTypeNames.ApplicationVndOpenxmlformatsOfficedocumentSpreadsheetmlSheet, "BaoCaoTongChiTraDauTu.xlsx");
                return Ok(result);
            }
            catch (Exception ex)
            {
                return Ok(OkException(ex));
            }
        }

        /// <summary>
        /// Báo cáo tổng hợp các khoản đầu tư hvf
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        [HttpGet("investment-hvf-report")]
        [PermissionFilter(Permissions.Invest_Menu_BaoCao)]
        public IActionResult InvestmentHvfReport(DateTime? startDate, DateTime? endDate)
        {
            try
            {
                var result = _exportExcelReportService.ExportInvestmentHVFReport(startDate, endDate);
                return File(result.fileData, MimeTypeNames.ApplicationVndOpenxmlformatsOfficedocumentSpreadsheetmlSheet, "BaoCaoCacKhoanDauTuVH.xlsx");
            }
            catch (Exception ex)
            {
                return Ok(OkException(ex));
            }
        }

        /// <summary>
        /// Báo cáo tổng hợp các khoản đầu tư bán hộ
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        [HttpGet("sales-investment-report")]
        [PermissionFilter(Permissions.Invest_Menu_BaoCao)]
        public IActionResult SalesInvestmentReport(DateTime? startDate, DateTime? endDate)
        {
            try
            {
                var result = _exportExcelReportService.SalesInvestmentReport(startDate, endDate);
                return File(result.fileData, MimeTypeNames.ApplicationVndOpenxmlformatsOfficedocumentSpreadsheetmlSheet, "BaoCaoTongHopCacKhoanDauTuBanHo.xlsx");
            }
            catch (Exception ex)
            {
                return Ok(OkException(ex));
            }
        }

        /// <summary>
        /// Báo cáo sao kê tài khoản nhà đầu tư
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        [HttpGet("investment-account-statistical")]
        [PermissionFilter(Permissions.Invest_Menu_BaoCao)]
        public IActionResult InvestmentAccountStatisticalReport(DateTime? startDate, DateTime? endDate)
        {
            try
            {
                var result = _exportExcelReportService.InvestmentAccountStatisticalReport(startDate, endDate);
                return File(result.fileData, MimeTypeNames.ApplicationVndOpenxmlformatsOfficedocumentSpreadsheetmlSheet, "BaoCaoSaoKeTaiKhoanNDT.xlsx");
            }
            catch (Exception ex)
            {
                return Ok(OkException(ex));
            }
        }

        /// <summary>
        /// Báo cáo danh sách các lệnh đến hạn chi trả
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet("list-interest-payment-due")]
        [PermissionFilter(Permissions.Invest_Menu_BaoCao)]
        public async Task<IActionResult> InterestPaymentDue([FromQuery] InterestPaymentFilterDto input)
        {
            try
            {
                var result = await _exportExcelReportService.ListInterestPaymentDue(input, IsLastPeriod.NO);
                return File(result.fileData, MimeTypeNames.ApplicationVndOpenxmlformatsOfficedocumentSpreadsheetmlSheet, "BaoCaoChiTraLoiTucDenHanChiTra.xlsx");
            }
            catch (Exception ex)
            {
                return Ok(OkException(ex));
            }
        }

        /// <summary>
        /// Báo cáo danh sách các hợp đồng đến hạn chi trả
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet("list-last-interest-payment-due")]
        [PermissionFilter(Permissions.Invest_Menu_BaoCao)]
        public async Task<IActionResult> LastInterestPaymentDue([FromQuery] InterestPaymentFilterDto input)
        {
            try
            {
                var result = await _exportExcelReportService.ListInterestPaymentDue(input, IsLastPeriod.YES);
                return File(result.fileData, MimeTypeNames.ApplicationVndOpenxmlformatsOfficedocumentSpreadsheetmlSheet, "BaoCaoHopDongDaoHanDenHanChiTra.xlsx");
            }
            catch (Exception ex)
            {
                return Ok(OkException(ex));
            }
        }

        /// <summary>
        /// Báo cáo danh sách hợp đồng đến hạn chi trả
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet("list-interest-payment-paid")]
        [PermissionFilter(Permissions.Invest_Menu_BaoCao)]
        public IActionResult InterestPaymentPaid([FromQuery] InterestPaymentFilterDto input)
        {
            try
            {
                var result = _exportExcelReportService.ListInterestPaymentPaid(input);
                var fileName = "";
                if (input.Status == PaymentTypeStatus.DA_LAP_CHUA_CHI_TRA)
                {
                    fileName = "BaocaoHopdongDaoHanDaLapChuaChiTra";
                }
                else if (input.Status == PaymentTypeStatus.DA_CHI_TRA)
                {
                    if (input.InterestPaymentStatus == InterestPaymentStatusType.CHI_TU_DONG)
                    {
                        fileName = "BaoCaoHopDongDaChiTraTuDong";
                    }
                    else if (input.InterestPaymentStatus == InterestPaymentStatusType.CHI_THU_CONG)
                    {
                        fileName = "BaoCaoHopDongDaChiTraThuCong";
                    }
                    else
                    {
                        fileName = "BaoCaoHopDongDaChiTra";
                    }
                }
                return File(result.fileData, MimeTypeNames.ApplicationVndOpenxmlformatsOfficedocumentSpreadsheetmlSheet, fileName);
            }
            catch (Exception ex)
            {
                return Ok(OkException(ex));
            }
        }
    }
}
