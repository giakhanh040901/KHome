using EPIC.DataAccess.Models;
using EPIC.Entities.DataEntities;
using EPIC.Entities.Dto.CoreProductNews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.CoreDomain.Interfaces
{
    public interface ICoreProductNewsServices
    {
        CoreProductNews Add(CreateCoreProductNewsDto body);
        int Delete(int id);
        int Update(UpdateCoreProductNewsDto body);
        PagingResult<ViewCoreProductNewsDto> FindAll(int pageSize, int pageNumber, string status, int? location);
        PagingResult<ViewCoreProductNewsDto> AppFindAll(int pageSize, int pageNumber, string status, int? location);
        ViewCoreProductNewsDto FindById(int id);
        int ChangeStatus(int id);
        int ChangeFeature(int id);
    }
}
