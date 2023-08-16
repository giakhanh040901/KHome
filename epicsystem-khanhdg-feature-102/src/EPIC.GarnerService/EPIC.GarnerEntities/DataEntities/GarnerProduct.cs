using EPIC.Entities;
using EPIC.Utils;
using EPIC.Utils.Attributes;
using EPIC.Utils.ConstantVariables.Shared;
using EPIC.Utils.DataUtils;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ColumnAttribute = System.ComponentModel.DataAnnotations.Schema.ColumnAttribute;

namespace EPIC.GarnerEntities.DataEntities
{
    [Table("GAN_PRODUCT", Schema = DbSchemas.EPIC_GARNER)]
    [Comment("San pham tich luy")]
    public class GarnerProduct : IFullAudited
    {
        public static string SEQ { get; } = $"SEQ_{(nameof(GarnerProduct)).ToSnakeUpperCase()}";

        #region Id, trạng thái
        [Key]
        [ColumnSnackCase(nameof(Id))]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [Required]
        [ColumnSnackCase(nameof(PartnerId))]
        [Comment("Id doi tac")]
        public int PartnerId { get; set; }

        [Required]
        [ColumnSnackCase(nameof(ProductType))]
        [Comment("Loai hinh tich luy (1: co phan, 2: co phieu, 3: trai phieu, 4: bat dong san)")]
        public int ProductType { get; set; }

        [ColumnSnackCase(nameof(Icon))]
        [MaxLength(512)]
        [Comment("Icon tren app")]
        public string Icon { get; set; }

        [Required]
        [ColumnSnackCase(nameof(IsCheck), TypeName = "VARCHAR2")]
        [MaxLength(1)]
        [Comment("Da kiem tra (Y, N)")]
        public string IsCheck { get; set; }

        [Required]
        [ColumnSnackCase(nameof(Status))]
        [Comment("Trang thai (1: Khoi tao, 2: Cho duyet, 3: Hoat dong, 4: Huy duyet, 5:Dong)")]
        public int Status { get; set; }
        #endregion

        #region thông tin chung
        [Required]
        [ColumnSnackCase(nameof(Code))]
        [MaxLength(256)]
        [Comment("Ma san pham")]
        public string Code { get; set; }

        [Required]
        [ColumnSnackCase(nameof(Name))]
        [MaxLength(256)]
        [Comment("Ten san pham")]
        public string Name { get; set; }

        [ColumnSnackCase(nameof(StartDate), TypeName = "DATE")]
        [Comment("Chung: Ngay bat dau dau tu/Ngay phat hanh (khi ban phan phoi chi duoc chay trong khoang nay)")]
        public DateTime? StartDate { get; set; }

        [ColumnSnackCase(nameof(EndDate), TypeName = "DATE")]
        [Comment("Chung: Ngay bat dau dau tu/Ngay dao han (khi ban phan phoi chi duoc chay trong khoang nay)")]
        public DateTime? EndDate { get; set; }

        [ColumnSnackCase(nameof(MaxInvestor))]
        [Comment("Chung: So khach hang dau tu toi da")]
        public int? MaxInvestor { get; set; }

        [ColumnSnackCase(nameof(MinInvestDay))]
        [Comment("Chung: So ngay dau tu toi thieu/So ngay nam giu toi thieu")]
        public int? MinInvestDay { get; set; }

        [ColumnSnackCase(nameof(CountType))]
        [Comment("Chung: Hinh thuc tinh lai cua to chuc phat hanh (1: tinh tu ngay phat hanh, 2: tinh tu ngay thanh toan)")]
        public int CountType { get; set; }

        [ColumnSnackCase(nameof(GuaranteeOrganization))]
        [MaxLength(512)]
        [Comment("Chung: To chuc bao lanh")]
        public string GuaranteeOrganization { get; set; }

        [ColumnSnackCase(nameof(IsPaymentGurantee), TypeName = "VARCHAR2")]
        [MaxLength(1)]
        [Comment("Chung: Co bao lanh thanh toan (Y, N)")]
        public string IsPaymentGurantee { get; set; }

        [ColumnSnackCase(nameof(IsCollateral), TypeName = "VARCHAR2")]
        [MaxLength(1)]
        [Comment("Chung: Co tai san dam bao (Y, N)")]
        public string IsCollateral { get; set; }
        #endregion

        #region tích luỹ cổ phần
        [ColumnSnackCase(nameof(CpsIssuerId))]
        [Comment("CPS: Id to chuc phat hanh")]
        public int? CpsIssuerId { get; set; }

        [ColumnSnackCase(nameof(CpsDepositProviderId))]
        [Comment("CPS: Id dai ly luu ky")]
        public int? CpsDepositProviderId { get; set; }

        [ColumnSnackCase(nameof(CpsPeriod))]
        [Comment("CPS: So ky han")]
        public int? CpsPeriod { get; set; }

        [ColumnSnackCase(nameof(CpsPeriodUnit))]
        [Comment("CPS: Don vi ky han Y M D")]
        public string CpsPeriodUnit { get; set; }

        [ColumnSnackCase(nameof(CpsInterestRate))]
        [Comment("CPS: Co tuc % nam")]
        public decimal? CpsInterestRate { get; set; }

        [ColumnSnackCase(nameof(CpsInterestRateType))]
        [Comment("CPS: Kieu tra co tuc (1: dinh ky, 2: cuoi ky)")]
        public int? CpsInterestRateType { get; set; }

        [ColumnSnackCase(nameof(CpsInterestPeriod))]
        [Comment("CPS: So ky tra co tuc")]
        public int? CpsInterestPeriod { get; set; }

        [ColumnSnackCase(nameof(CpsInterestPeriodUnit))]
        [Comment("CPS: Don vi ky han Y M D")]
        public string CpsInterestPeriodUnit { get; set; }

        [ColumnSnackCase(nameof(CpsNumberClosePer))]
        [Comment("CPS: So ngay chot quyen")]
        public int? CpsNumberClosePer { get; set; }

        [ColumnSnackCase(nameof(CpsParValue))]
        [Comment("CPS: Menh gia")]
        public decimal? CpsParValue { get; set; }

        [ColumnSnackCase(nameof(CpsQuantity))]
        [Comment("CPS: So luong co phan")]
        public long? CpsQuantity { get; set; }

        [ColumnSnackCase(nameof(CpsIsListing), TypeName = "VARCHAR2")]
        [MaxLength(1)]
        [Comment("CPS: Co niem yet (Y, N)")]
        public string CpsIsListing { get; set; }

        [Column("CPS_IS_ALLOW_SBD", TypeName = "VARCHAR2")]
        [MaxLength(1)]
        [Comment("CPS: Co ban lai truoc han khong sell before expiration date")]
        public string CpsIsAllowSBD { get; set; }
        #endregion

        #region tích luỹ invest
        [ColumnSnackCase(nameof(InvOwnerId))]
        [Comment("Invest: Chu dau tu")]
        public int? InvOwnerId { get; set; }

        [ColumnSnackCase(nameof(InvGeneralContractorId))]
        [Comment("Invest: Tong thau thi cong")]
        public int? InvGeneralContractorId { get; set; }

        [ColumnSnackCase(nameof(InvTotalInvestmentDisplay))]
        [Comment("Invest: Tong muc dau tu")]
        public decimal? InvTotalInvestmentDisplay { get; set; }

        [ColumnSnackCase(nameof(InvTotalInvestment))]
        [Comment("Invest: Han muc dau tu")]
        public decimal? InvTotalInvestment { get; set; }

        [ColumnSnackCase(nameof(InvArea))]
        [MaxLength(128)]
        [Comment("Invest: Dien tich")]
        public string InvArea { get; set; }

        [ColumnSnackCase(nameof(InvLocationDescription))]
        [MaxLength(512)]
        [Comment("Invest: Mo ta vi tri")]
        public string InvLocationDescription { get; set; }

        [ColumnSnackCase(nameof(InvLatitude))]
        [MaxLength(128)]
        [Comment("Invest: Kinh do")]
        public string InvLatitude { get; set; }

        [ColumnSnackCase(nameof(InvLongitude))]
        [MaxLength(128)]
        [Comment("Invest: Vi do")]
        public string InvLongitude { get; set; }
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
