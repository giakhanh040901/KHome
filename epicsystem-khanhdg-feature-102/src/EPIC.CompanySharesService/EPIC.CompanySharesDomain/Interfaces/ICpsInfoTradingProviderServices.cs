using EPIC.CompanySharesEntities.Dto.CompanySharesInfoTradingProvider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.CompanySharesDomain.Interfaces
{
    public interface ICpsInfoTradingProviderServices
    {
        List<CpsInfoTradingProviderDto> FindAll(int projectId);
        void UpdateProjectTrading(int projectId, List<CreateCpsInfoTradingProviderDto> input);
    }
}
