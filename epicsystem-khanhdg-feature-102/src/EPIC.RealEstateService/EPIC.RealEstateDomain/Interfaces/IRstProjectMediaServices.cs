using EPIC.DataAccess.Models;
using EPIC.RealEstateEntities.DataEntities;
using EPIC.RealEstateEntities.Dto.RstProjectMedia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.RealEstateDomain.Interfaces
{
    public interface IRstProjectMediaServices
    {
        List<RstProjectMedia> Add(CreateRstProjectMediasDto input);
        RstProjectMedia ChangeStatus(int id, string status);
        void Delete(int id);
        PagingResult<RstProjectMedia> FindAll(FilterRstProjectMediaDto input);
        RstProjectMedia FindById(int id);
        RstProjectMedia Update(UpdateRstProjectMediaDto input);
        List<RstProjectMedia> Find(int projectId, string location, string status);
        void UpdateSortOrder(RstProjectMediaSortDto input);
    }
}
