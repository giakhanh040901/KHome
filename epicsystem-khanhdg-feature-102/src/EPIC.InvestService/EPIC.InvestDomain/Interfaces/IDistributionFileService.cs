using EPIC.DataAccess.Models;
using EPIC.InvestEntities.DataEntities;
using EPIC.InvestEntities.Dto.DistributionFile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.InvestDomain.Interfaces
{
    public interface IDistributionFileService
    {
        PagingResult<DistributionFileDto> FindAll(int distributionId, int pageSize, int pageNumber);
        DistributionFile Add(CreateDistributionFileDto input);
        void Update(UpdateDistributionFileDto input);
        DistributionFile FindById(int id);
        int Delete(int id);
    }
}
