using EPIC.GarnerEntities.DataEntities;
using EPIC.GarnerEntities.Dto.GarnerShared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.GarnerSharedDomain.Interfaces
{
    public interface IGarnerContractCodeServices
    {
        string GenOrderContractCodeDefault(GenGarnerContractCodeDto input);
        string GetContractCode(GarnerOrder order, GarnerProduct product, GarnerPolicy policy, int configContractId);
    }
}
