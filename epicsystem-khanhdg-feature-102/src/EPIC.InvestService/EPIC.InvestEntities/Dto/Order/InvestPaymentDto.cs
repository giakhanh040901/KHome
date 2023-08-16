using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.InvestEntities.Dto.Order
{
    public class InvestPaymentDateDto
    {
        public int OrderId { get; set; }
        public int PeriodIndex { get; set; }
        public DateTime PayDate { get; set; }
    }
}
