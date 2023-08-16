using EPIC.Utils.Attributes;
using EPIC.Utils.ConstantVariables.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EPIC.Entities;
using EPIC.Utils.DataUtils;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Text.Json.Serialization;
using EPIC.Utils.ConstantVariables.Media;

namespace EPIC.EventEntites.Entites
{
    /// <summary>
    /// Hỉnh ảnh
    /// </summary>
    [Table("EVT_EVENT_MEDIA_DETAIL", Schema = DbSchemas.EPIC_EVENT)]
    [Index(nameof(Deleted), nameof(TradingProviderId), nameof(EventMediaId), nameof(Status), nameof(UrlImage), IsUnique = false, Name = "IX_EVT_EVENT_MEDIA_DETAIL")]
    public class EvtEventMediaDetail : IFullAudited
    {
        public static string SEQ { get; } = $"{DbConsts.SEQ}{nameof(EvtEventMediaDetail).ToSnakeUpperCase()}";

        [Key]
        [ColumnSnackCase(nameof(Id))]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [ColumnSnackCase(nameof(TradingProviderId))]
        public int TradingProviderId { get; set; }

        /// <summary>
        /// Id hình ảnh sự kiện
        /// </summary>
        [ColumnSnackCase(nameof(EventMediaId))]
        public int EventMediaId { get; set; }
        [JsonIgnore]
        public EvtEventMedia EventMedia { get; set; }

        /// <summary>
        /// Đường dẫn file ảnh
        /// </summary>
        [Required]
        [MaxLength(512)]
        [ColumnSnackCase(nameof(UrlImage), TypeName = "VARCHAR2")]
        public string UrlImage { get; set; }

        /// <summary>
        /// IMAGE: ảnh, VIDEO: video
        /// <see cref="MediaTypes"/>
        /// </summary>
        [MaxLength(256)]
        [ColumnSnackCase(nameof(MediaType), TypeName = "VARCHAR2")]
        public string MediaType { get; set; }

        [MaxLength(1)]
        [ColumnSnackCase(nameof(Status), TypeName = "VARCHAR2")]
        public string Status { get; set; }

        [ColumnSnackCase(nameof(SortOrder))]
        public int SortOrder { get; set; }

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
