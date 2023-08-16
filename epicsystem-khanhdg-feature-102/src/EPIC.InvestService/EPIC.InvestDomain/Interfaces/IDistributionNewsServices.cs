using EPIC.DataAccess.Models;
using EPIC.InvestEntities.DataEntities;
using EPIC.InvestEntities.Dto.DistributionNews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.InvestDomain.Interfaces
{
    public interface IDistributionNewsServices
    {
        DistributionNews Add(CreateDistributionNewsDto body);
        int Delete(int id);
        int Update(UpdateDistributionNewsDto body);
        PagingResult<ViewDistributionNewsDto> FindAll(int pageSize, int pageNumber, string status, int distributionId);
        ViewDistributionNewsDto FindById(int id);
        int ChangeStatus(int id);
    }
}
