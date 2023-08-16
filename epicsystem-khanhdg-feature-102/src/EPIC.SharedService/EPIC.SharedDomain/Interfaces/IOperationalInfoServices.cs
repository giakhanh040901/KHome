using EPIC.DataAccess.Models;
using EPIC.Entities.Dto.TradingProvider;
using EPIC.SharedEntities.Dto.InvestorOrder;
using EPIC.SharedEntities.Dto.OperationalInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.SharedDomain.Interfaces
{
    public interface IOperationalInfoServices
    {
        OperationalInfoDto GetInfoBCT(DateTime? startDate, DateTime? endDate);
        PagingResult<TradingProviderDto> GetTradingList();
        PagingResult<ReportTradingProviderDto> GetReportTradingList();
        void UpdateReportTradingList(int[] input);
    }
}
