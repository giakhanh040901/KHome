using EPIC.Entities.Dto.BusinessCustomer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Entities.Dto.DepositProvider
{
    public class DepositProviderDto
    {
        public int? DepositProviderId { get; set; }

        public int? BusinessCustomerId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public BusinessCustomerDto BusinessCustomer { get; set; }
    }
}
