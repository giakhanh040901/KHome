using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Entities.Dto.BusinessCustomer
{
    public class UpdateBusinessCustomerBankDto : CreateBusinessCustomerBankTempDto
    {
        public int BusinessCustomerBankId { get; set; }
    }
}
