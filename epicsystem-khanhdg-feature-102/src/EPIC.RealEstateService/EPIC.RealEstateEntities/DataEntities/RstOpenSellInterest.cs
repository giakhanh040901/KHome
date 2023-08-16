using DocumentFormat.OpenXml.Wordprocessing;
using EPIC.Utils.Attributes;
using EPIC.Utils.ConstantVariables.Shared;
using EPIC.Utils.DataUtils;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.RealEstateEntities.DataEntities
{
    /// <summary>
    /// Số lượng người quan tâm cho
    /// </summary>
    [Table("RST_OPEN_SELL_INTEREST", Schema = DbSchemas.EPIC_REAL_ESTATE)]
    [Index(nameof(InvestorId), nameof(OpenSellDetailId), IsUnique = false, Name = "IX_RST_OPEN_SELL_INTEREST")]
    public class RstOpenSellInterest
    {
        public static string SEQ { get; } = $"{DbConsts.SEQ}{nameof(RstOpenSellInterest).ToSnakeUpperCase()}";

        [Key]
        [ColumnSnackCase(nameof(Id))]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        /// <summary>
        /// Id khách hàng cá nhân
        /// </summary>
        [ColumnSnackCase(nameof(InvestorId))]
        public int InvestorId { get; set; }

        /// <summary>
        /// Id của mở bán
        /// </summary>
        [ColumnSnackCase(nameof(OpenSellDetailId))]
        public int OpenSellDetailId { get; set; }

        /// <summary>
        /// Số lượng quan tâm
        /// </summary>
        [ColumnSnackCase(nameof(InterestCount))]
        public int InterestCount { get; set; }
    }
}
