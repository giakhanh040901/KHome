using EPIC.Entities.Dto.BusinessCustomer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.CoreSharedEntities.Dto.BusinessCustomer
{
    public class BusinessCustomerForContractDto
    {
        public BusinessCustomerDto BusinessCustomer { get; set; }
        public BusinessCustomerBankDto BusinessCustomerBank { get; set; }
    }
}
