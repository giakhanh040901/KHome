using EPIC.Entities;
using EPIC.Utils.Attributes;
using EPIC.Utils.ConstantVariables.Shared;
using EPIC.Utils.DataUtils;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EPIC.GarnerEntities.DataEntities
{
    [Table("GAN_POLICY_DETAIL_TEMP", Schema = DbSchemas.EPIC_GARNER)]
    [Comment("ky han mau")]
    public class GarnerPolicyDetailTemp : IFullAudited
    {
        public static string SEQ = $"SEQ_{nameof(GarnerPolicyDetailTemp).ToSnakeUpperCase()}";

        #region Id
        [Key]
        [ColumnSnackCase(nameof(Id))]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [Required]
        [ColumnSnackCase(nameof(TradingProviderId))]
        [Comment("Id dai ly")]
        public int TradingProviderId { get; set; }

        [ColumnSnackCase(nameof(PolicyTempId))]
        [Comment("Id chinh sach mau")]
        public int PolicyTempId { get; set; }
        #endregion

        #region name, status
        [ColumnSnackCase(nameof(SortOrder))]
        [Comment("Chung: So thu tu")]
        public int SortOrder { get; set; }

        [Required]
        [MaxLength(256)]
        [ColumnSnackCase(nameof(Name))]
        [Comment("Chung: Ten")]
        public string Name { get; set; }

        [Required]
        [MaxLength(128)]
        [ColumnSnackCase(nameof(ShortName))]
        [Comment("Chung: Ten viet tat")]
        public string ShortName { get; set; }

        [Required]
        [MaxLength(1)]
        [ColumnSnackCase(nameof(Status), TypeName = "VARCHAR2")]
        [Comment("Chung: Trang thai (A, D)")]
        public string Status { get; set; }
        #endregion

        [ColumnSnackCase(nameof(Profit))]
        [Comment("Chung: Loi tuc % dung chung cho ca tich luy lan ky han")]
        public decimal Profit { get; set; }

        [ColumnSnackCase(nameof(InterestDays))]
        [Comment("Chung: So ngay tra loi nhuan chinh xac (kiem tra khong lon hon PeriodType * PeriodQuantity, voi Y la 366 * PeriodQuantity, M la 31 * PeriodQuantity, neu la D thi khong cho nhap truong nay)")]
        public int? InterestDays { get; set; }

        [ColumnSnackCase(nameof(PeriodQuantity))]
        [Comment("Chung: So ky dau tu (So ky dao han)")]
        public int PeriodQuantity { get; set; }

        [ColumnSnackCase(nameof(PeriodType), TypeName = "VARCHAR2")]
        [Comment("Chung: Don vi ky dau tu (Don vi ky dao han: Y, M, D)")]
        [MaxLength(1)]
        public string PeriodType { get; set; }

        #region loại kỳ hạn (với type của chính sách là loại kỳ hạn)
        [ColumnSnackCase(nameof(InterestType))]
        [Comment("Loai ky han: Kieu tra loi tuc (1: Dinh ky, 2: Cuoi ky)")]
        public int? InterestType { get; set; }

        [ColumnSnackCase(nameof(InterestPeriodQuantity))]
        [Comment("Loai ky han: So ky tra loi nhuan")]
        public int? InterestPeriodQuantity { get; set; }

        [ColumnSnackCase(nameof(InterestPeriodType), TypeName = "VARCHAR2")]
        [Comment("Loai ky han: Don vi ky tra loi nhuan (Y, M, D)")]
        [MaxLength(1)]
        public string InterestPeriodType { get; set; }

        [ColumnSnackCase(nameof(RepeatFixedDate))]
        [Comment("Loai ky han: Ngay tra co dinh hang thang cho loai ky han")]
        public int? RepeatFixedDate { get; set; }
        #endregion

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
