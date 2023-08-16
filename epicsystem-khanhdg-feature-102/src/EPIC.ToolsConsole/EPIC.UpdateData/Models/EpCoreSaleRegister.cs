using System;
using System.Collections.Generic;

#nullable disable

namespace EPIC.UpdateData.Models
{
    public partial class EpCoreSaleRegister
    {
        public decimal Id { get; set; }
        public decimal? InvestorId { get; set; }
        public decimal? InvestorBankAccId { get; set; }
        public decimal? SaleManagerId { get; set; }
        public decimal? Status { get; set; }
        public string IpAddress { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
        public string Deleted { get; set; }
        public DateTime? CancelDate { get; set; }
        public DateTime? DirectionDate { get; set; }
    }
}
