using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.InvestEntities.Dto.Order
{
    public class UpdateInfoCustomerDto
    {
        public int OrderId { get; set; }
        public int? InvestorBankAccId { get; set; }
        public int? ContractAddressId { get; set; }
        public int? investorIdenId { get; set; }
    }
}
