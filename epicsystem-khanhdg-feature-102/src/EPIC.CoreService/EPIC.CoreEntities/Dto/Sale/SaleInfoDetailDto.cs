using EPIC.Entities.Dto.BusinessCustomer;
using EPIC.Entities.Dto.Investor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.CoreEntities.Dto.Sale
{
    public class SaleInfoDetailDto
    {
        public InvestorDto Investor { get; set; }
        public BusinessCustomerDto BusinessCustomer { get; set; }
    }
}
