using EPIC.Entities.Dto.ContractData;
using System;
using System.Threading.Tasks;

namespace EPIC.RealEstateDomain.Interfaces
{
    public interface IRstExportExcelReportServices
    {
        Task<ExportResultDto> ProductProjectOverview(DateTime? startDate, DateTime? endDate);
        Task<ExportResultDto> SyntheticMoneyProject(DateTime? startDate, DateTime? endDate);
        Task<ExportResultDto> SyntheticTrading(DateTime? startDate, DateTime? endDate);
    }
}