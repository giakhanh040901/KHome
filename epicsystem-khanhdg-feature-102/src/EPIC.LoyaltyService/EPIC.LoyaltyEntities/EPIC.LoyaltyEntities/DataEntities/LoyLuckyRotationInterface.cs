using DocumentFormat.OpenXml.Wordprocessing;
using EPIC.Entities;
using EPIC.Utils.Attributes;
using EPIC.Utils.ConstantVariables.Shared;
using EPIC.Utils.DataUtils;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EPIC.LoyaltyEntities.DataEntities
{
    /// <summary>
    /// Giao diện vòng quay may mắn
    /// </summary>
    [Table("LOY_LUCKY_ROTATION_INTERFACE", Schema = DbSchemas.EPIC_LOYALTY)]
    [Index(nameof(Deleted), nameof(LuckyScenarioId), IsUnique = false, Name = "IX_LOY_LUCKY_ROTATION_INTERFACE")]
    public class LoyLuckyRotationInterface : IFullAudited
    {
        public static string SEQ { get; } = $"{DbConsts.SEQ}{nameof(LoyLuckyRotationInterface).ToSnakeUpperCase()}";

        [Key]
        [ColumnSnackCase(nameof(Id))]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        /// <summary>
        /// Id kịch bản
        /// </summary>
        [ColumnSnackCase(nameof(LuckyScenarioId))]
        public int LuckyScenarioId { get; set; }

        /// <summary>
        /// Mẫu vòng quay
        /// </summary>
        [ColumnSnackCase(nameof(Template))]
        [MaxLength(50)]
        public string Template { get; set; }

        /// <summary>
        /// Nút chơi game
        /// </summary>
        [ColumnSnackCase(nameof(ButtonPlay))]
        public bool ButtonPlay { get; set; }

        /// <summary>
        /// Nút lịch sử
        /// </summary>
        [ColumnSnackCase(nameof(ButtonHistory))]
        public bool ButtonHistory { get; set; }

        /// <summary>
        /// Nút xếp hạng
        /// </summary>
        [ColumnSnackCase(nameof(ButtonRank))]
        public bool ButtonRank { get; set; }

        /// <summary>
        /// Mã màu các nút button Chơi game, lịch sử, xếp hạng
        /// </summary>
        [ColumnSnackCase(nameof(ButtonColor))]
        [MaxLength(20)]
        public string ButtonColor { get; set; }

        /// <summary>
        /// Hiện icon Home hay ko
        /// </summary>
        [ColumnSnackCase(nameof(IconHome))]
        public bool IconHome { get; set; }

        /// <summary>
        /// Hiện thông báo trúng thưởng hay không
        /// </summary>
        [ColumnSnackCase(nameof(WinText))]
        public bool WinText { get; set; }

        /// <summary>
        /// Hiện banner hay ko
        /// </summary>
        [ColumnSnackCase(nameof(ShowBanner))]
        public bool ShowBanner { get; set; }

        /// <summary>
        /// ảnh icon nút Chơi game
        /// </summary>
        [MaxLength(1024)]
        [ColumnSnackCase(nameof(IconPlay))]
        public string IconPlay { get; set; }

        /// <summary>
        /// ảnh icon nút lịch sử chơi
        /// </summary>
        [MaxLength(1024)]
        [ColumnSnackCase(nameof(IconHistory))]
        public string IconHistory { get; set; }

        /// <summary>
        /// ảnh icon nút xếp hạng
        /// </summary>
        [MaxLength(1024)]
        [ColumnSnackCase(nameof(IconRank))]
        public string IconRank { get; set; }

        /// <summary>
        /// mã màu chữ thông báo trúng thưởng
        /// </summary>
        [MaxLength(20)]
        [ColumnSnackCase(nameof(WinTextColor))]
        public string WinTextColor { get; set; }

        /// <summary>
        /// mã màu nền thông báo trúng thưởng
        /// </summary>
        [MaxLength(1024)]
        [ColumnSnackCase(nameof(WinTextBackgroundColor))]
        public string WinTextBackgroundColor { get; set; }

        /// <summary>
        /// Ảnh banner
        /// </summary>
        [MaxLength(1024)]
        [ColumnSnackCase(nameof(Banner))]
        public string Banner { get; set; }

        /// <summary>
        /// ảnh vòng quay
        /// </summary>
        [MaxLength(1024)]
        [ColumnSnackCase(nameof(RotationImage))]
        public string RotationImage { get; set; }

        /// <summary>
        /// ảnh nền vòng quay
        /// </summary>
        [MaxLength(1024)]
        [ColumnSnackCase(nameof(RotationBackground))]
        public string RotationBackground { get; set; }

        /// <summary>
        /// Ảnh kim quay
        /// </summary>
        [MaxLength(1024)]
        [ColumnSnackCase(nameof(NeedleImage))]
        public string NeedleImage { get; set; }

        /// <summary>
        /// Hình nền
        /// </summary>
        [MaxLength(1024)]
        [ColumnSnackCase(nameof(Background))]
        public string Background { get; set; }

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
