using DocumentFormat.OpenXml.Wordprocessing;
using EPIC.Entities;
using EPIC.Utils.Attributes;
using EPIC.Utils.ConstantVariables.Loyalty;
using EPIC.Utils.ConstantVariables.Shared;
using EPIC.Utils.DataUtils;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EPIC.LoyaltyEntities.DataEntities
{
    /// <summary>
    /// Bảng chương trình trúng thưởng
    /// </summary>
    [Table("LOY_LUCKY_PROGRAM", Schema = DbSchemas.EPIC_LOYALTY)]
    [Index(nameof(Deleted), nameof(TradingProviderId), nameof(Status), IsUnique = false, Name = "IX_LOY_LUCKY_PROGRAM")]
    public class LoyLuckyProgram : IFullAudited
    {
        public static string SEQ { get; } = $"{DbConsts.SEQ}{nameof(LoyLuckyProgram).ToSnakeUpperCase()}";

        [Key]
        [ColumnSnackCase(nameof(Id))]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [ColumnSnackCase(nameof(TradingProviderId))]
        public int TradingProviderId { get; set; }

        /// <summary>
        /// Mã chương trình
        /// </summary>
        [Required]
        [ColumnSnackCase(nameof(Code), TypeName = "VARCHAR2")]
        [MaxLength(50)]
        public string Code { get; set; }

        /// <summary>
        /// Tên chương trình
        /// </summary>
        [Required]
        [ColumnSnackCase(nameof(Name))]
        [MaxLength(512)]
        public string Name { get; set; }

        /// <summary>
        /// Thời gian bắt đầu
        /// </summary>
        [ColumnSnackCase(nameof(StartDate), TypeName = "DATE")]
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Thời gian kết thúc
        /// </summary>
        [ColumnSnackCase(nameof(EndDate), TypeName = "DATE")]
        public DateTime EndDate { get; set; }

        /// <summary>
        /// Loại nội dung: MARKDOWN, HTML
        /// </summary>
        [ColumnSnackCase(nameof(DescriptionContentType), TypeName = "VARCHAR2")]
        [MaxLength(20)]
        public string DescriptionContentType { get; set; }

        /// <summary>
        /// Nội dung mô tả
        /// </summary>
        [ColumnSnackCase(nameof(DescriptionContent), TypeName = "CLOB")]
        public string DescriptionContent { get; set; }

        /// <summary>
        /// Ảnh đại diện chương trình
        /// </summary>
        [ColumnSnackCase(nameof(AvatarImageUrl), TypeName = "VARCHAR2")]
        [MaxLength(500)]
        public string AvatarImageUrl { get; set; }

        /// <summary>
        /// Cài đặt thời gian tham gia: null thì không cài
        /// 1. Tính theo mốc thời gian
        /// 2. Cài đặt thời gian tham gia
        /// </summary>
        [ColumnSnackCase(nameof(JoinTimeSetting))]
        public int? JoinTimeSetting { get; set; }

        /// <summary>
        /// Số lượt quay/ Khách hàng
        /// </summary>
        [ColumnSnackCase(nameof(NumberOfTurn))]
        public int? NumberOfTurn { get; set; }

        /// <summary>
        /// Thời gian bắt đầu được quay nếu cài JoinTimeSetting = 1
        /// Nếu JoinTimeSetting = 2 thì Thời gian bắt đầu được quay tính từ lúc khách quay
        /// </summary>
        [ColumnSnackCase(nameof(StartTurnDate), TypeName = "DATE")]
        public DateTime? StartTurnDate { get; set; }

        /// <summary>
        /// Reset lượt quay
        /// </summary>
        [ColumnSnackCase(nameof(ResetTurnQuantity))]
        public int? ResetTurnQuantity { get; set; }

        /// <summary>
        /// Reset lượt quay theo D, M, Y...
        /// <see cref="TimeTypes"/>
        /// </summary>
        [ColumnSnackCase(nameof(ResetTurnType))]
        public string ResetTurnType { get; set; }

        /// <summary>
        /// Số lượt tham gia tối đa của mỗi địa chỉ Ip
        /// </summary>
        [ColumnSnackCase(nameof(MaxNumberOfTurnByIp))]
        public int? MaxNumberOfTurnByIp { get; set; }

        /// <summary>
        /// Trạng thái
        /// <see cref="LoyLuckyProgramStatus"/>
        /// </summary>
        [ColumnSnackCase(nameof(Status))]
        public int Status { get; set; }

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

        public List<LoyLuckyScenario> LoyLuckyScenarios { get; } = new();
    }
}
