using EPIC.Entities;
using EPIC.Utils.Attributes;
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
    [Table("EP_CORE_BUSINESS_CUS_TRADING", Schema = DbSchemas.EPIC)]
    public class BusinessCustomerTrading
    {
        public static string SEQ { get; } = $"{DbSchemas.EPIC}.SEQ_BUSI_CUS_TRADING_PROVIDER";

        [Key]
        [ColumnSnackCase(nameof(Id))]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [ColumnSnackCase(nameof(BusinessCustomerId))]
        public int? BusinessCustomerId { get; set; }

        [ColumnSnackCase(nameof(TradingProviderId))]
        public int? TradingProviderId { get; set; }

        [ColumnSnackCase(nameof(CreateDate))]
        public DateTime? CreateDate { get; set; }

        [ColumnSnackCase(nameof(CreateBy))]
        public string CreateBy { get; set; }

        [ColumnSnackCase(nameof(Deleted))]
        public string Deleted { get; set; }
    }
}
