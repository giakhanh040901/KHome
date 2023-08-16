
using EPIC.Utils.ConstantVariables.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.CoreEntities.DataEntities
{
    [Table("EP_INVESTOR_TRADING_PROVIDER", Schema = DbSchemas.EPIC)]
    public class InvestorTradingProvider
    {
        public static string SEQ { get; } = $"SEQ_INVESTOR_TRADING_PROVIDER";
        [Key]
        [Column("ID")]
        public int Id { get; set; }

        [Column("INVESTOR_ID")]
        public int InvestorId { get; set; }

        [Column("TRADING_PROVIDER_ID")]
        public int TradingProviderId { get; set; }  

        [Column("CREATE_DATE")]
        public DateTime CreatedDate { get; set; }

        [Column("CREATE_BY")]
        public string CreatedBy { get; set; }

        [Column("DELETED")]
        public string Deleted { get; set; }

    }
}
