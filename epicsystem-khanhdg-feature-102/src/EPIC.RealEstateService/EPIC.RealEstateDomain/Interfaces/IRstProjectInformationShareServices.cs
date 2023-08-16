using EPIC.DataAccess.Models;
using EPIC.RealEstateEntities.Dto.RstProjectInformationShare;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.RealEstateDomain.Interfaces
{
    public interface IRstProjectInformationShareServices
    {
        RstProjectInformationShareDto AddProjectShare(CreateRstProjectInformationShareDto input);
        void ChangStatus(int id);
        void Delete(int id);
        void UpdateProjectShare(UpdateRstProjectInformationShareDto input);
        PagingResult<RstProjectInformationShareDto> FindAll(FilterRstProjectInformationShareDto input);
        RstProjectInformationShareDto FindById(int id);
    }
}
