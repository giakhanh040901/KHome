using EPIC.RealEstateEntities.Dto.RstRating;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.RealEstateDomain.Interfaces
{
    public interface IRstRatingServices
    {
        void Add(CreateRstRatingDto input);
        RstRatingDto FindLastOrder();
        List<ViewRstRatingDto> ViewRstRating(int projectId);
    }
}
