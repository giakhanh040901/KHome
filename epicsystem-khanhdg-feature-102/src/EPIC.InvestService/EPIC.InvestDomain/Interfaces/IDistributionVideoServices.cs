using EPIC.DataAccess.Models;
using EPIC.InvestEntities.DataEntities;
using EPIC.InvestEntities.Dto.DistributionVideo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.InvestDomain.Interfaces
{
    public interface IDistributionVideoServices
    {
        DistributionVideo Add(CreateDistributionVideoDto body);
        int Delete(int id);
        int Update(UpdateDistributionVideoDto body);
        PagingResult<ViewDistributionVideoDto> FindAll(int pageSize, int pageNumber, string status, int distributionId);
        ViewDistributionVideoDto FindById(int id);
        int ChangeStatus(int id);
        int ChangeFeature(int id);
        ViewDistributionVideoDto FindNewVideo(int id);
    }
}
