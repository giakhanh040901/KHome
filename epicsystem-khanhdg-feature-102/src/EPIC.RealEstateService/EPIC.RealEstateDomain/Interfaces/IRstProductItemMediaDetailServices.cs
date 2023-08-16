using EPIC.RealEstateEntities.DataEntities;
using EPIC.RealEstateEntities.Dto.RstProductItemMedia;
using EPIC.RealEstateEntities.Dto.RstProductItemMediaDetail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.RealEstateDomain.Interfaces
{
    public interface IRstProductItemMediaDetailServices
    {
        List<RstProductItemMediaDetail> Add(CreateRstProductItemMediaDetailsDto input);
        RstProductItemMediaDetail ChangeStatus(int id, string status);
        void Delete(int id);
        List<RstProductItemMediaDto> Find(int productItemId, string status);
        RstProductItemMediaDetail FindById(int id);
        RstProductItemMediaDetail Update(UpdateRstProductItemMediaDetailDto input);
        List<RstProductItemMediaDetail> AddListMediaDetail(AddRstProductItemMediaDetailsDto input);
        void UpdateSortOrder(RstProductItemMediaDetailSortDto input);
    }
}
