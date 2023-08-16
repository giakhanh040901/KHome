using System;
using System.Collections.Generic;

#nullable disable

namespace EPIC.UpdateData.Models
{
    public partial class EpInvOwner
    {
        public decimal Id { get; set; }
        public decimal? BusinessCustomerId { get; set; }
        public bool? Status { get; set; }
        public string Deleted { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public decimal? PartnerId { get; set; }
        public decimal? BusinessTurnover { get; set; }
        public decimal? BusinessProfit { get; set; }
        public decimal? Roa { get; set; }
        public decimal? Roe { get; set; }
        public string Image { get; set; }
        public string Website { get; set; }
        public string Hotline { get; set; }
        public string Fanpage { get; set; }
    }
}
