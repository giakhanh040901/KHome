using EPIC.InvestEntities.DataEntities;
using EPIC.InvestEntities.Dto.InvestShared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.InvestSharedDomain.Interfaces
{
    public interface IInvestContractCodeServices
    {
        string GenOrderContractCodeDefault(GenInvestContractCodeDto input);
        string GetContractCode(InvOrder order, Policy policy, int configContractId);
    }
}
