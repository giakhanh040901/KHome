using EPIC.Entities;
using EPIC.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.BondEntities.DataEntities
{
    public class BondDistributionContractPayment : IFullAudited
    {
        [Column(Name = "ID")]
        public int Id { get; set; }

        [Column(Name = "DISTRIBUTION_CONTRACT_ID")]
        public int DistributionContractId { get; set; }

        [Column(Name = "TRANSACTION_TYPE")]
        public int TransactionType { get; set; }

        [Column(Name = "PAYMENT_TYPE")]
        public int PaymentType { get; set; }

        [Column(Name = "TOTAL_VALUE")]
        public decimal TotalValue { get; set; }

        [Column(Name = "DESCRIPTION")]
        public string Description { get; set; }

        [Column(Name = "APPROVE_BY")]
        public string ApproveBy { get; set; }

        [Column(Name = "APPROVE_DATE")]
        public DateTime? ApproveDate { get; set; }

        [Column(Name = "CANCEL_BY")]
        public string CancelBy { get; set; }

        [Column(Name = "CANCEL_DATE")]
        public DateTime? CancelDate { get; set; }

        public DateTime? TradingDate { get; set; }

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
