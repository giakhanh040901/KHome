using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.RealEstateEntities.Dto.RstOrder
{
    public class AppCreateRstOrderCheckDto : AppCreateRstOrderDto
    {
        public int? InvestorId { get; set; }
    }
}
