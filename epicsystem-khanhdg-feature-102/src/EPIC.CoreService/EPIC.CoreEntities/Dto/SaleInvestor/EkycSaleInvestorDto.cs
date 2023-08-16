using EPIC.Entities.Dto.ManagerInvestor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Entities.Dto.SaleInvestor
{
    public class EkycSaleInvestorDto : EkycManagerInvestorDto
    {
        public int InvestorId { get; set; }
    }
}
