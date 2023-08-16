using EPIC.Entities;
using EPIC.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.BondEntities.DataEntities
{
    public class BondDistributionContract : IFullAudited
    {
        [Column(Name = "ID")]
        public int Id { get; set; }

        [Column(Name = "PARTNER_ID")]
        public int PartnerId { get; set; }

        [Column(Name = "BOND_PRIMARY_ID")]
        public int BondPrimaryId { get; set; }

        [Column(Name = "TRADING_PROVIDER_ID")]
        public int TradingProviderId { get; set; }

        [Column(Name = "CONTRACT_CODE")]
        public string ContractCode { get; set; }

        [Column(Name = "QUANTITY")]
        public long Quantity { get; set; }

        [Column(Name = "TOTAL_VALUE")]
        public decimal TotalValue { get; set; }

        [Column(Name = "DATE_BUY")]
        public DateTime? DateBuy { get; set; }

        [Column(Name = "STATUS")]
        public int Status { get; set; }

        [Column(Name = "CREATED_DATE")]
        public DateTime? CreatedDate { get; set; }

        [Column(Name = "CREATED_BY")]
        public string CreatedBy { get; set; }

        [Column(Name = "MODIFIED_DATE")]
        public DateTime? ModifiedDate { get; set; }

        [Column(Name = "MODIFIED_BY")]
        public string ModifiedBy { get; set; }

        [Column(Name = "DELETED")]
        public string Deleted { get; set; }
    }

    public class DistributionContractFile : IFullAudited
    {
        [Column(Name = "FILE_ID")]
        public int Id { get; set; }

        [Column(Name = "DISTRIBUTION_CONTRACT_ID")]
        public int DistributionContractId { get; set; }

        [Column(Name = "TITLE")]
        public string Title { get; set; }

        [Column(Name = "FILE_URL")]
        public string FileUrl { get; set; }

        [Column(Name = "APPROVE_BY")]
        public string ApproveBy { get; set; }

        [Column(Name = "APPROVE_DATE")]
        public DateTime? ApproveDate { get; set; }

        [Column(Name = "CANCEL_BY")]
        public string CancelBy { get; set; }

        [Column(Name = "CANCEL_DATE")]
        public DateTime? CancelDate { get; set; }

        [Column(Name = "STATUS")]
        public string Status { get; set; }

        [Column(Name = "CREATED_DATE")]
        public DateTime? CreatedDate { get; set; }

        [Column(Name = "CREATED_BY")]
        public string CreatedBy { get; set; }

        [Column(Name = "MODIFIED_DATE")]
        public DateTime? ModifiedDate { get; set; }

        [Column(Name = "MODIFIED_BY")]
        public string ModifiedBy { get; set; }

        [Column(Name = "DELETED")]
        public string Deleted { get; set; }
    }
}
