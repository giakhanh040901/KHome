using DocumentFormat.OpenXml.Wordprocessing;
using EPIC.Entities;
using EPIC.Utils.Attributes;
using EPIC.Utils.ConstantVariables.RealEstate;
using EPIC.Utils.ConstantVariables.Shared;
using EPIC.Utils.DataUtils;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EPIC.RealEstateEntities.DataEntities
{
    [Table("RST_OPEN_SELL_FILE", Schema = DbSchemas.EPIC_REAL_ESTATE)]
    [Index(nameof(Deleted), nameof(TradingProviderId), nameof(OpenSellId), nameof(Status), IsUnique = false, Name = "IX_RST_OPEN_SELL_FILE")]
    public class RstOpenSellFile : IFullAudited
    {
        public static string SEQ { get; } = $"{DbConsts.SEQ}{nameof(RstOpenSellFile).ToSnakeUpperCase()}";

        [Key]
        [ColumnSnackCase(nameof(Id))]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [ColumnSnackCase(nameof(TradingProviderId))]
        public int TradingProviderId { get; set; }

        [ColumnSnackCase(nameof(OpenSellId))]
        public int OpenSellId { get; set; }

        [Required]
        [MaxLength(256)]
        [ColumnSnackCase(nameof(Name))]
        public string Name { get; set; }

        [Required]
        [MaxLength(512)]
        [ColumnSnackCase(nameof(Url), TypeName = "VARCHAR2")]
        public string Url { get; set; }

        /// <summary>
        /// Trạng thái (A: Active, D: Deactive)
        /// <see cref="Utils.Status"/>
        /// </summary>
        [MaxLength(1)]
        [ColumnSnackCase(nameof(Status), TypeName = "VARCHAR2")]
        public string Status { get; set; }

        /// <summary>
        /// Loại file <br/>
        /// <see cref="RstOpenSellFileTypes"/>
        /// </summary>
        [ColumnSnackCase(nameof(OpenSellFileType))]
        public int OpenSellFileType { get; set; }

        /// <summary>
        /// Thời gian hiệu lực
        /// </summary>
        [ColumnSnackCase(nameof(ValidTime))]
        public DateTime? ValidTime { get; set; }

        #region audit
        [ColumnSnackCase(nameof(CreatedDate), TypeName = "DATE")]
        public DateTime? CreatedDate { get; set; }

        [ColumnSnackCase(nameof(CreatedBy))]
        [MaxLength(50)]
        public string CreatedBy { get; set; }

        [ColumnSnackCase(nameof(ModifiedDate), TypeName = "DATE")]
        public DateTime? ModifiedDate { get; set; }

        [ColumnSnackCase(nameof(ModifiedBy))]
        [MaxLength(50)]
        public string ModifiedBy { get; set; }

        [ColumnSnackCase(nameof(Deleted), TypeName = "VARCHAR2")]
        [Required]
        [MaxLength(1)]
        public string Deleted { get; set; }
        #endregion
    }
}
