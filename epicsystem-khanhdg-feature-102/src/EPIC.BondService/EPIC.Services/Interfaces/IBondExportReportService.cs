using EPIC.Entities.Dto.ContractData;
using EPIC.Entities.Dto.ExportReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.BondDomain.Interfaces
{
    public interface IBondExportReportService
    {
        ExportResultDto ExportExcelBondPackages(DateTime? startDate, DateTime? endDate);
        ExportResultDto ExportBondInvestment(DateTime? startDate, DateTime? endDate);
        ExportResultDto ExportInterestPrincipalDue(DateTime? startDate, DateTime? endDate);
    }
}
