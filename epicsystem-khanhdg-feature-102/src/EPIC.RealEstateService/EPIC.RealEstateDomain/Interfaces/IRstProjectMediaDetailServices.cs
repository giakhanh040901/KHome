using EPIC.RealEstateEntities.DataEntities;
using EPIC.RealEstateEntities.Dto.RstProjectMedia;
using EPIC.RealEstateEntities.Dto.RstProjectMediaDetail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.RealEstateDomain.Interfaces
{
    public interface IRstProjectMediaDetailServices
    {
        List<RstProjectMediaDetail> Add(CreateRstProjectMediaDetailsDto input);
        List<RstProjectMediaDetail> AddListMediaDetail(AddRstProjectMediaDetailsDto input);
        RstProjectMediaDetail ChangeStatus(int id, string status);
        void Delete(int id);
        List<RstProjectMediaDto> Find(int projectId, string status);
        RstProjectMediaDetail FindById(int id);
        RstProjectMediaDetail Update(UpdateRstProjectMediaDetailDto input);
        void UpdateSortOrder(RstProjectMediaDetailSortDto input);
    }
}
