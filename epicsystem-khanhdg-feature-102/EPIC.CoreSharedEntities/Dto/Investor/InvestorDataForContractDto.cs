using EPIC.Entities.DataEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.CoreSharedEntities.Dto.Investor
{
    public class InvestorDataForContractDto
    {
        public Entities.DataEntities.Investor Investor { get; set; }
        public InvestorIdentification InvestorIdentification { get; set; }
        public InvestorBankAccount InvestorBankAccount { get; set; }
        public InvestorContactAddress InvestorContactAddress { get; set; }
    }
}
