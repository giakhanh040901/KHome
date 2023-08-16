using EPIC.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.CompanySharesEntities.DataEntities
{
    public class InterestPaymentDate
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int PeriodIndex { get; set; }
        public DateTime PayDate { get; set; }
        public DateTime? ClosePerDate { get; set; }
        public int TypeDate { get; set; }
    }
}
