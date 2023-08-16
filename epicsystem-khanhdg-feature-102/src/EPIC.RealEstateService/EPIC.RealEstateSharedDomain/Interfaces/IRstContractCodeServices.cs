using EPIC.GarnerEntities.DataEntities;
using EPIC.GarnerEntities.Dto.GarnerShared;
using EPIC.RealEstateEntities.DataEntities;
using EPIC.RealEstateEntities.Dto.RstGenContractCode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.RealEstateSharedDomain.Interfaces
{
    public interface IRstContractCodeServices
    {
        string GenOrderContractCodeDefault(RstGenContractCodeDefaultDto input);
        string GetContractCode(RstGenContractCodeDto input);
    }
}
