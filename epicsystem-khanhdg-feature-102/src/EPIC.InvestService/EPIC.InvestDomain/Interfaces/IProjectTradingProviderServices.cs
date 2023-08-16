using EPIC.InvestEntities.Dto.ProjectTradingProvider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.InvestDomain.Interfaces
{
    public interface IProjectTradingProviderServices
    {
        List<ProjectTradingProviderDto> FindAll(int projectId);
        void UpdateProjectTrading(int projectId, List<CreateProjectTradingProviderDto> input);
    }
}
