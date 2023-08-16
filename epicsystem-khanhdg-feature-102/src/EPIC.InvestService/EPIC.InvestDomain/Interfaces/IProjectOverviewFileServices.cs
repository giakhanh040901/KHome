using EPIC.DataAccess.Models;
using EPIC.InvestEntities.DataEntities;
using EPIC.InvestEntities.Dto.ProjectOverViewFile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.InvestDomain.Interfaces
{
    public interface IProjectOverviewFileServices
    {
        ProjectOverviewFile Add(CreateProjectOverviewFileDto input);
        int Delete(int id);
        PagingResult<ProjectOverviewFile> FindAll(int distributionId, int pageSize, int pageNumber, int? status);
        ProjectOverviewFile FindById(int id);
        int Update(UpdateProjectOverviewFileDto input);
    }
}
