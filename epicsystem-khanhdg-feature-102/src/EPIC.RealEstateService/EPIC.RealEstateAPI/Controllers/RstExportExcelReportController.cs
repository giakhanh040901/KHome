using EPIC.RealEstateDomain.Interfaces;
using EPIC.Utils.Controllers;
using EPIC.WebAPIBase.FIlters;
using EPIC.Utils.Net.MimeTypes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using EPIC.Utils.ConstantVariables.Identity;
using EPIC.Shared.Filter;

namespace EPIC.RealEstateAPI.Controllers
{
    [Authorize]
    [AuthorizeAdminUserTypeFilter]
    [Route("api/real-estate/export-excel-report")]
    [ApiController]
    public class RstExportExcelReportController : BaseController
    {
        private readonly IRstExportExcelReportServices _rstExportExcelReportServices;
        public RstExportExcelReportController(ILogger<RstExportExcelReportController> logger, IRstExportExcelReportServices rstExportExcelReportServices)
        {
            _rstExportExcelReportServices = rstExportExcelReportServices;
            _logger = logger;
        }

        /// <summary>
        /// Báo cáo tổng quan bảng hàng dự án
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        [HttpGet("product-project-overview")]
        [PermissionFilter(Permissions.RealState_BaoCao_QuanTri_TQBangHangDuAn, Permissions.RealState_BaoCao_VanHanh_TQBangHangDuAn, Permissions.RealState_BaoCao_KinhDoanh_TQBangHangDuAn)]
        public async Task<IActionResult> ProductProjectOverview(DateTime? startDate, DateTime? endDate)
        {
            try
            {
                var result = await _rstExportExcelReportServices.ProductProjectOverview(startDate, endDate);
                return File(result.fileData, MimeTypeNames.ApplicationVndOpenxmlformatsOfficedocumentSpreadsheetmlSheet, "BaoCaoTongQuanBangHangDuAn.xlsx");
            }
            catch (Exception ex)
            {
                return Ok(OkException(ex));
            }
        }

        /// <summary>
        /// Báo cáo tổng hợp tiền về dự án
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        [HttpGet("synthetic-money-project")]
        [PermissionFilter(Permissions.RealState_BaoCao_QuanTri_TH_TienVeDA, Permissions.RealState_BaoCao_VanHanh_TH_TienVeDA, Permissions.RealState_BaoCao_KinhDoanh_TH_TienVeDA)]
        public async Task<IActionResult> SyntheticMoneyProject(DateTime? startDate, DateTime? endDate)
        {
            try
            {
                var result = await _rstExportExcelReportServices.SyntheticMoneyProject(startDate, endDate);
                return File(result.fileData, MimeTypeNames.ApplicationVndOpenxmlformatsOfficedocumentSpreadsheetmlSheet, "BaoCaoTongHopTienVeDuAn.xlsx");
            }
            catch (Exception ex)
            {
                return Ok(OkException(ex));
            }
        }

        /// <summary>
        /// Báo cáo tổng hợp các khoản giao dịch
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        [HttpGet("synthetic-trading")]
        [PermissionFilter(Permissions.RealState_BaoCao_QuanTri_TH_CacKhoanGD, Permissions.RealState_BaoCao_VanHanh_TH_CacKhoanGD, Permissions.RealState_BaoCao_KinhDoanh_TH_CacKhoanGD)]
        public async Task<IActionResult> SyntheticTrading(DateTime? startDate, DateTime? endDate)
        {
            try
            {
                var result = await _rstExportExcelReportServices.SyntheticTrading(startDate, endDate);
                return File(result.fileData, MimeTypeNames.ApplicationVndOpenxmlformatsOfficedocumentSpreadsheetmlSheet, "BaoCaoTongHopCacKhoanGiaoDich.xlsx");
            }
            catch (Exception ex)
            {
                return Ok(OkException(ex));
            }
        }
    }
}