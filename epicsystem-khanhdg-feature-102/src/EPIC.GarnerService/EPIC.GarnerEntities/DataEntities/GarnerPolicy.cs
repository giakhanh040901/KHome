using EPIC.Utils.Attributes;
using EPIC.Utils;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EPIC.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using EPIC.Utils.ConstantVariables.Shared;
using Microsoft.EntityFrameworkCore;
using EPIC.Utils.DataUtils;
using EPIC.EntitiesBase.Interfaces.Policy;

namespace EPIC.GarnerEntities.DataEntities
{
    [Table("GAN_POLICY", Schema = DbSchemas.EPIC_GARNER)]
    [Comment("Chinh sach")]
    public class GarnerPolicy : IFullAudited, IProductPolicy, IInterestPeriod
    {
        public static string SEQ = $"SEQ_{nameof(GarnerPolicy).ToSnakeUpperCase()}";

        #region Id
        [Key]
        [ColumnSnackCase(nameof(Id))]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [ColumnSnackCase(nameof(TradingProviderId))]
        [Comment("Id dai ly")]
        public int TradingProviderId { get; set; }

        [ColumnSnackCase(nameof(DistributionId))]
        [Comment("Id san pham")]
        public int DistributionId { get; set; }

        [ColumnSnackCase(nameof(StartDate), TypeName = "DATE")]
        public DateTime? StartDate { get; set; }

        [ColumnSnackCase(nameof(EndDate), TypeName = "DATE")]
        public DateTime? EndDate { get; set; }

        [ColumnSnackCase(nameof(Description))]
        [MaxLength(2048)]
        public string Description { get; set; }

        [ColumnSnackCase(nameof(SortOrder))]
        public int SortOrder { get; set; }
        #endregion

        [Required]
        [MaxLength(100)]
        [ColumnSnackCase(nameof(Code))]
        [Comment("Chung: Ma chinh sach")]
        public string Code { get; set; }

        [Required]
        [MaxLength(256)]
        [ColumnSnackCase(nameof(Name))]
        [Comment("Chung: Ten chinh sach")]
        public string Name { get; set; }

        [ColumnSnackCase(nameof(MinMoney))]
        [Comment("Chung: So tien tich luy toi thieu (dau tu toi thieu) (nho hon hoac bang cua Project)")]
        public decimal MinMoney { get; set; }

        [ColumnSnackCase(nameof(MaxMoney))]
        [Comment("Chung: So tien tich luy toi da (dau tu toi da)")]
        public decimal? MaxMoney { get; set; }

        [ColumnSnackCase(nameof(MinInvestDay))]
        [Comment("Chung: So ngay tich luy toi thieu (Nho hon hoac bang so ngay nay cua Product) (So ngay dau tu toi thieu/So ngay nam giu toi thieu)")]
        public int MinInvestDay { get; set; }

        [ColumnSnackCase(nameof(IncomeTax))]
        [Comment("Chung: Thue loi nhuan")]
        public decimal IncomeTax { get; set; }

        [ColumnSnackCase(nameof(InvestorType), TypeName = "VARCHAR2")]
        [MaxLength(1)]
        [Comment("Chung: Loai nha dau tu (P: chuyen nghiep, A: tat ca)")]
        public string InvestorType { get; set; }

        [ColumnSnackCase(nameof(Classify))]
        [Comment("Chung: Phan loai chinh sach san pham (1: hop tac, 2: mua ban), mac dinh la hop tac")]
        public int Classify { get; set; }

        [ColumnSnackCase(nameof(CalculateType))]
        [Comment("Chung: Loai hinh tinh loi tuc (1: Net, 2: Gross)")]
        public int CalculateType { get; set; }

        [ColumnSnackCase(nameof(GarnerType))]
        [Comment("Chung: Loai hinh ky han (1: khong chon ky han (tich luy theo thoi gian), 2: chon ky han (ky han), 3(chua can): khong chon ky han theo so tien, 4(chua can): khong chon ky han theo ca thoi gian va so tien)")]
        public int GarnerType { get; set; }

        /// <summary>
        /// Kiểu trả lợi tức lấy trong InterestType const
        /// </summary>
        [ColumnSnackCase(nameof(InterestType))]
        [Comment("GarnerType = khong chon ky han thi moi hien: Kieu tra loi tuc (1: dinh ky, 2: cuoi ky, 3: ngay co dinh(neu chon ngay co dinh thi moi hien nhap ngay tra co dinh), 4: ngay dau thang, 5: ngay cuoi thang), InterestType này không phai la bi lap lai cua InterestType trong policy detail ma la truong danh rieng cho tich luy khong chon ky han")]
        public int? InterestType { get; set; }

        [ColumnSnackCase(nameof(InterestPeriodQuantity))]
        [Comment("GarnerType = khong chon ky han va InterestType = dinh ky hoac cuoi ky thi moi hien: So ky tra loi nhuan")]
        public int? InterestPeriodQuantity { get; set; }

        [ColumnSnackCase(nameof(InterestPeriodType))]
        [MaxLength(1)]
        [Comment("GarnerType = khong chon ky han va InterestType = dinh ky hoac cuoi ky thi moi hien: Don vi ky tra loi nhuan (Y, M, D)")]
        public string InterestPeriodType { get; set; }

        /// <summary>
        /// Ngày trả cố định
        /// </summary>
        [ColumnSnackCase(nameof(RepeatFixedDate))]
        [Comment("GarnerType = khong chon ky han va InterestType = ngay co dinh thi moi hien va cho nhap: Ngay tra co dinh")]
        public int? RepeatFixedDate { get; set; }

        [ColumnSnackCase(nameof(MinWithdraw))]
        [Comment("Chung: So tien rut toi thieu (nho hon hoac bang cua Project)")]
        public decimal MinWithdraw { get; set; }

        [ColumnSnackCase(nameof(MaxWithdraw))]
        [Comment("Chung: So tien rut toi da")]
        public decimal? MaxWithdraw { get; set; }

        [ColumnSnackCase(nameof(WithdrawFee))]
        [Comment("Chung: Phi rut tien (so tien hoac theo nam)")]
        public decimal WithdrawFee { get; set; }

        [ColumnSnackCase(nameof(WithdrawFeeType))]
        [Comment("Chung: Loai phi rut von (1: so tien, 2: theo nam)")]
        public int WithdrawFeeType { get; set; }

        [ColumnSnackCase(nameof(OrderOfWithdrawal))]
        [Comment("Loai hinh linh hoat: Thu tu rut tien (1: moi nhat den cu nhat, 2: cu nhat den moi nhat, 3: gia tri gan nhat gia tri rut (uu tien gia tri HD nao to hon gia tri rut)))")]
        public int OrderOfWithdrawal { get; set; }

        [ColumnSnackCase(nameof(IsTransferAssets), TypeName = "VARCHAR2")]
        [MaxLength(1)]
        [Comment("Chung: Co chuyen doi tai san (Y, N)")]
        public string IsTransferAssets { get; set; }

        [ColumnSnackCase(nameof(TransferAssetsFee))]
        [Comment("Chung: phi chuyen doi tai san %")]
        public decimal TransferAssetsFee { get; set; }

        [Required]
        [MaxLength(1)]
        [ColumnSnackCase(nameof(IsShowApp), TypeName = "VARCHAR2")]
        [Comment("Chung: Co show app")]
        public string IsShowApp { get; set; }

        [Required]
        [MaxLength(1)]
        [ColumnSnackCase(nameof(Status), TypeName = "VARCHAR2")]
        [Comment("Chung: Trang thai (A, D)")]
        public string Status { get; set; }

        [Required]
        [ColumnSnackCase(nameof(IsDefault), TypeName = "VARCHAR2")]
        [MaxLength(1)]
        [Comment("Chung: Mac dinh cho dai ly (Y,N) mac dinh la N")]
        public string IsDefault { get; set; }

        [Required]
        [ColumnSnackCase(nameof(IsDefaultEpic), TypeName = "VARCHAR2")]
        [MaxLength(1)]
        [Comment("Chung: Mac dinh cho root (Y,N) mac dinh la N")]
        public string IsDefaultEpic { get; set; }

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
