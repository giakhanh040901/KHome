using DocumentFormat.OpenXml.Wordprocessing;
using EPIC.Entities.DataEntities;
using EPIC.Utils.ConstantVariables.Shared;
using EPIC.Utils;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EPIC.Utils.DataUtils;
using EPIC.Utils.Attributes;
using System.ComponentModel.DataAnnotations;
using EPIC.Entities;

namespace EPIC.CoreEntities.DataEntities
{
    [Table("EP_INVESTOR_SEARCH_HISTORY", Schema = DbSchemas.EPIC)]
    public class InvestorSearchHistory : IFullAudited
    {
        public static string SEQ { get; } = $"{DbConsts.SEQ}{nameof(InvestorSearchHistory).ToSnakeUpperCase()}";

        [Key]
        [ColumnSnackCase(nameof(Id))]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [ColumnSnackCase(nameof(InvestorId))]
        public int InvestorId { get; set; }

        [ColumnSnackCase(nameof(InvestDistributionId))]
        public int? InvestDistributionId { get; set; }

        [ColumnSnackCase(nameof(GarnerPolicyId))]
        public int? GarnerPolicyId { get; set; }

        [ColumnSnackCase(nameof(RstOpenSellId))]
        public int? RstOpenSellId { get; set; }

        [ColumnSnackCase(nameof(EventId))]
        public int? EventId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
        public string Deleted { get; set; }
    }
}
