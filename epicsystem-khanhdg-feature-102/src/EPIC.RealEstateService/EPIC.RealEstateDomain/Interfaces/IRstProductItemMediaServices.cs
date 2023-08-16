using EPIC.RealEstateEntities.DataEntities;
using EPIC.RealEstateEntities.Dto.RstProductItemMedia;
using EPIC.RealEstateEntities.Dto.RstProjectMedia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.RealEstateDomain.Interfaces
{
    public interface IRstProductItemMediaServices
    {
        List<RstProductItemMedia> Add(CreateRstProductItemMediasDto input);
        RstProductItemMedia ChangeStatus(int id, string status);
        void Delete(int id);
        List<RstProductItemMedia> Find(int productItemId, string location, string status);
        RstProductItemMedia FindById(int id);
        RstProductItemMedia Update(UpdateRstProductItemMediaDto input);
        void UpdateSortOrder(RstProductItemMediaSortDto input);

    }
}
