using EPIC.DataAccess.Models;
using EPIC.Entities.Dto.ContractData;
using EPIC.Entities.Dto.ExportReport;
using EPIC.InvestEntities.DataEntities;
using EPIC.InvestEntities.Dto.ExportExcel;
using EPIC.InvestEntities.Dto.InterestPayment;
using EPIC.InvestEntities.Dto.InvestShared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.InvestDomain.Interfaces
{
    public interface IExportExcelReportService
    {
        ExportResultDto FindAllInvestCode(DateTime? startDate, DateTime? endDate);
        ExportResultDto ExportInvestmentReport(DateTime? startDate, DateTime? endDate);
        Task<ExportResultDto> DueExpendExcelReport(DateTime? startDate, DateTime? endDate);
        RutVonDto RutVon(long orderId, decimal soTienRut, DateTime ngayRut);
        ExportResultDto ActualExpendReport(DateTime? startDate, DateTime? endDate);
        /// <summary>
        /// Báo cáo tổng tiền chi trả đầu tư tất cả hợp đồng trong từng ngày
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        ExportResultDto ActualTotalExpendInDay(DateTime? startDate, DateTime? endDate);
        List<TotalOperationReportDto> TotalOperationReport(DateTime? startDate, DateTime? endDate);
        /// <summary>
        /// Báo cáo tổng hợp các khoản đầu tư HVF
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        ExportResultDto ExportInvestmentHVFReport(DateTime? startDate, DateTime? endDate);
        /// <summary>
        /// Báo cáo tổng hợp các khoản đầu tư bán hộ
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        ExportResultDto SalesInvestmentReport(DateTime? startDate, DateTime? endDate);
        /// <summary>
        /// Báo cáo sao kê tài khoản nhà đầu tư
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        ExportResultDto InvestmentAccountStatisticalReport(DateTime? startDate, DateTime? endDate);
        /// <summary>
        /// Báo cáo tổng hợp danh sách lệnh đến hạn chi trả
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        Task<ExportResultDto> ListInterestPaymentDue(InterestPaymentFilterDto input, bool isLastPeriod);

        ExportResultDto ListInterestPaymentPaid(InterestPaymentFilterDto input);
    }
}
