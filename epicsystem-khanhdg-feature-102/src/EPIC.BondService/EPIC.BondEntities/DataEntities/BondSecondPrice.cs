using EPIC.Entities;
using EPIC.Utils;
using System;

namespace EPIC.BondEntities.DataEntities
{
    public class BondSecondPrice : IFullAudited
    {
        [Column(Name = "ID")]
        public int Id { get; set; }

        [Column(Name = "TRADING_PROVIDER_ID")]
        public int TradingProviderId { get; set; }

        [Column(Name = "SECONDARY_ID")]
        public int SecondaryId { get; set; }

        [Column(Name = "PRICE_DATE")]
        public DateTime PriceDate { get; set; }

        [Column(Name = "PRICE")]
        public decimal Price { get; set; }

        [Column(Name = "CREATED_DATE")]
        public DateTime? CreatedDate { get; set; }

        [Column(Name = "CREATED_BY")]
        public string CreatedBy { get; set; }

        [Column(Name = "MODIFIED_BY")]
        public string ModifiedBy { get; set; }

        [Column(Name = "MODIFIED_DATE")]
        public DateTime? ModifiedDate { get; set; }

        [Column(Name = "DELETED")]
        public string Deleted { get; set; }

    }
}




