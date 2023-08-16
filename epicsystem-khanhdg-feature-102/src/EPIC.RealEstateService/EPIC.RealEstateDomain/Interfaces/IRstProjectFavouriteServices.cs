using EPIC.RealEstateEntities.Dto.RstProjectFavourite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.RealEstateDomain.Interfaces
{
    public interface IRstProjectFavouriteServices
    {
        RstProjectFavouriteDto AppAddProjectFavourite(CreateRstProjectFavouriteDto input);
        void AppDeleteProjectFavourite(int openSellId);
    }
}
