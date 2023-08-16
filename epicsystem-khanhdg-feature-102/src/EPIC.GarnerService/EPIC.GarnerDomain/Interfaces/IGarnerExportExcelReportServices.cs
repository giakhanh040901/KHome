using EPIC.Entities.Dto.ContractData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.GarnerDomain.Interfaces
{
    public interface IGarnerExportExcelReportServices
    {
        /// <summary>
        /// Báo cáo tổng hợp các khoản đầu tư garner
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        Task<ExportResultDto> ExcelGarnerTotalInvestmentAsync(DateTime? startDate, DateTime? endDate);

        /// <summary>
        /// Báo cáo quản trị tổng hợp 
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        Task<ExportResultDto> ExcelGarnerListAdministrationAsync(DateTime? startDate, DateTime? endDate);

        /// <summary>
        /// Báo cáo tổng chi trả đầu tư
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        ExportResultDto TotalInvestmentPayment(DateTime? startDate, DateTime? endDate);

        /// <summary>
        /// Báo cáo các sản phẩm tích lũy
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        ExportResultDto ExcelListGarnerProduct(DateTime? startDate, DateTime? endDate);

        /// <summary>
        /// Báo cáo thực chi
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        Task<ExportResultDto> ExcelGarnerActualPaymentAsync(DateTime? startDate, DateTime? endDate);

        /// <summary>
        /// Báo cáo tổng hợp các khoản đầu tư bán hộ
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        Task<ExportResultDto> ExcelGarnerSaleInvestmentAsync(DateTime? startDate, DateTime? endDate);
    }
}
