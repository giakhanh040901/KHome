using EPIC.DataAccess.Models;
using EPIC.RealEstateEntities.DataEntities;
using EPIC.RealEstateEntities.Dto.RstSellingPolicy;

namespace EPIC.RealEstateDomain.Interfaces
{
    public interface IRstSellingPolicyTempServices
    {
        RstSellingPolicyTemp Add(CreateRstSellingPolicyTempDto input);
        RstSellingPolicyTemp Update(UpdateRstSellingPolicyTempDto input);
        void Delete(int id);
        ViewRstSellingPolicyTempDto FindById(int id);
        PagingResult<ViewRstSellingPolicyTempDto> FindAll(FilterRstSellingPolicyTempDto input);
        RstSellingPolicyTemp ChangeStatus(int id);
    }
}
