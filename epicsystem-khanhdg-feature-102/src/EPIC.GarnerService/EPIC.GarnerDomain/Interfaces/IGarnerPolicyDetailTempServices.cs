using EPIC.GarnerEntities.DataEntities;
using EPIC.GarnerEntities.Dto.GarnerPolicyDetailTemp;
using System.Collections.Generic;

namespace EPIC.GarnerDomain.Interfaces
{
    public interface IGarnerPolicyDetailTempServices
    {
        GarnerPolicyDetailTemp Add(CreateGarnerPolicyDetailTempDto input);
        void Delete(int id);
        List<GarnerPolicyDetailTempDto> FindAll(int policyTempId);
        GarnerPolicyDetailTempDto FindById(int id);
        GarnerPolicyDetailTemp Update(UpdateGarnerPolicyDetailTempDto input);
    }
}
