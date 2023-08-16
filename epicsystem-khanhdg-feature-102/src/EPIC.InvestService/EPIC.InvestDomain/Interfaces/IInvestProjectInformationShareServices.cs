using EPIC.DataAccess.Models;
using EPIC.InvestEntities.Dto.ProjectInformationShare;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.InvestDomain.Interfaces
{
    public interface IInvestProjectInformationShareServices
    {
        InvProjectInformationShareDto AddProjectInformationShare(CreateInvProjectInformationShareDto input);
        void ChangStatus(int id);
        void Delete(int id);
        PagingResult<InvProjectInformationShareDto> FindAll(FilterInvProjectInformationShareDto input);
        InvProjectInformationShareDto FindById(int id);
        void UpdateProjectInformationShare(UpdateInvProjectInformationShareDto input);
    }
}
