using DocumentFormat.OpenXml.Wordprocessing;
using EPIC.Entities;
using EPIC.Entities.DataEntities;
using EPIC.Utils.Attributes;
using EPIC.Utils.ConstantVariables.Media;
using EPIC.Utils.ConstantVariables.RealEstate;
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

namespace EPIC.EventEntites.Entites
{
    /// <summary>
    /// Hình ảnh Sự kiện
    /// </summary>
    [Table("EVT_EVENT_MEDIA", Schema = DbSchemas.EPIC_EVENT)]
    [Index(nameof(Deleted), nameof(TradingProviderId), nameof(EventId), nameof(Status), nameof(UrlImage), IsUnique = false, Name = "IX_EVT_EVENT_MEDIA")]
    public class EvtEventMedia : IFullAudited
    {
        public static string SEQ { get; } = $"{DbConsts.SEQ}{nameof(EvtEventMedia).ToSnakeUpperCase()}";

        [Key]
        [ColumnSnackCase(nameof(Id))]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [ColumnSnackCase(nameof(TradingProviderId))]
        public int TradingProviderId { get; set; }
        [ColumnSnackCase(nameof(EventId))]
        public int EventId { get; set; }
        public EvtEvent Event { get; set; }

        /// <summary>
        ///Tên nhóm (nhóm hình ảnh)
        /// </summary>
        [MaxLength(256)]
        [ColumnSnackCase(nameof(GroupTitle))]
        public string GroupTitle { get; set; }

        /// <summary>
        /// Đường dẫn file ảnh
        /// </summary>
        //[Required]
        [MaxLength(512)]
        [ColumnSnackCase(nameof(UrlImage), TypeName = "VARCHAR2")]
        public string UrlImage { get; set; }

        /// <summary>
        /// Đường dẫn điều hướng khi click ảnh
        /// </summary>
        [MaxLength(512)]
        [ColumnSnackCase(nameof(UrlPath), TypeName = "VARCHAR2")]
        public string UrlPath { get; set; }

        /// <summary>
        /// IMAGE: ảnh, VIDEO: video
        /// <see cref="MediaTypes"/>
        /// </summary>
        [MaxLength(256)]
        [ColumnSnackCase(nameof(MediaType), TypeName = "VARCHAR2")]
        public string MediaType { get; set; }

        /// <summary>
        /// Vị trí
        /// <see cref="RstMediaLocations"/>
        /// </summary>
        [MaxLength(256)]
        [ColumnSnackCase(nameof(Location), TypeName = "VARCHAR2")]
        public string Location { get; set; }

        [MaxLength(1)]
        [Required]
        [ColumnSnackCase(nameof(Status), TypeName = "VARCHAR2")]
        public string Status { get; set; }

        [ColumnSnackCase(nameof(SortOrder))]
        public int SortOrder { get; set; }
        public List<EvtEventMediaDetail> EventMediaDetails { get; set; } = new();

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
