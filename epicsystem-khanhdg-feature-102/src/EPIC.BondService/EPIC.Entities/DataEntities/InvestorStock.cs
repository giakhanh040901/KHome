using EPIC.Utils.ConstantVariables.Shared;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Entities.DataEntities
{
    [Table("EP_INVESTOR_STOCK", Schema = DbSchemas.EPIC)]
    public class InvestorStock : IFullAudited
    {
        [Key]
        [Column("ID")]
        public int Id { get; set; }
        [Column("INVESTOR_ID")]
        public int? InvestorId { get; set; }
        [Column("INVESTOR_GROUP_ID")]
        public int? InvestorGroupId { get; set; }
        [Column("IS_DEFAULT")]
        public string IsDefault { get; set; }
        [Column("SECURITY_COMPANY")]
        public int? SecurityCompany { get; set; }
        [Column("STOCK_TRADING_ACCOUNT")]
        public string StockTradingAccount { get; set; }
        [Column("REFER_ID")]
        public int? ReferId { get; set; }
        [Column("CREATED_DATE")]
        public DateTime? CreatedDate { get; set; }
        [Column("CREATED_BY")]
        public string CreatedBy { get; set; }
        [Column("MODIFIED_DATE")]
        public DateTime? ModifiedDate { get; set; }
        [Column("MODIFIED_BY")]
        public string ModifiedBy { get; set; }
        [Column("DELETED")]
        public string Deleted { get; set; }
    }
}
