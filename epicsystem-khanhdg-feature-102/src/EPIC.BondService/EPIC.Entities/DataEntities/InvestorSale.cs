using EPIC.Utils.ConstantVariables.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Entities.DataEntities
{
    [Table("EP_INVESTOR_SALE", Schema = DbSchemas.EPIC)]
    public class InvestorSale
    {
        public static string SEQ { get; } = $"{DbSchemas.EPIC}.SEQ_INVESTOR_SALE";

        [Key]
        [Column("ID")]
        public int Id { get; set; }

        [Column("INVESTOR_ID")]
        public int InvestorId { get; set; }

        [Column("SALE_ID")]
        public int? SaleId { get; set; }

        [Column("REFERRAL_CODE")]
        public string ReferralCode { get; set; }

        [Column("IS_DEFAULT")]
        public string IsDefault { get; set; }

        [Column("CREATED_BY")]
        public string CreatedBy { get; set; }

        [Column("CREATED_DATE")]
        public DateTime CreatedDate { get; set; }

        [Column("DELETED")]
        public string Deleted { get; set; }

        [NotMapped]
        public string AvatarImageUrl { get; set; }

        [NotMapped]
        public string Fullname { get; set; }

        [NotMapped]
        public int? InvestorIdOfSale { get; set; }

        [NotMapped]
        public int? BusinessCustomerIdOfSale { get; set; }

    }
}
