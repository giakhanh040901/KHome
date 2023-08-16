using EPIC.InvestEntities.DataEntities;
using EPIC.InvestEntities.Dto.ProjectImage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.InvestDomain.Interfaces
{
    public interface IProjectImageServices
    {
        void Add(CreateProjectImageDto input);
        List<ProjectImage> FindByProjectId(int projectId);
        int Delete(int id);
    }
}
