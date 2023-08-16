using System;
using System.Collections.Generic;

#nullable disable

namespace EPIC.UpdateData.Models
{
    public partial class EpBondInterestPaymentDate
    {
        public decimal Id { get; set; }
        public decimal OrderId { get; set; }
        public decimal PeriodIndex { get; set; }
        public DateTime PayDate { get; set; }
        public DateTime? ClosePerDate { get; set; }
        public decimal TypeDate { get; set; }
    }
}
