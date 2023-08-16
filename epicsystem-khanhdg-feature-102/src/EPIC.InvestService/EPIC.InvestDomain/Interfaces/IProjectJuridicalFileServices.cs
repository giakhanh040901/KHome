using EPIC.DataAccess.Models;
using EPIC.InvestEntities.DataEntities;
using EPIC.InvestEntities.Dto.ProjectJuridicalFile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.InvestDomain.Interfaces
{
    public interface IProjectJuridicalFileServices
    {
        ProjectJuridicalFile Add(CreateProjectJuridicalFileDto input);
        ProjectJuridicalFile FindById(int id);
        PagingResult<ProjectJuridicalFile> FindAll(int projectId, int pageSize, int pageNumber, string keyword);
        int Update(int id, UpdateProjectJuridicalFileDto input);
        int Delete(int id);
    }
}
