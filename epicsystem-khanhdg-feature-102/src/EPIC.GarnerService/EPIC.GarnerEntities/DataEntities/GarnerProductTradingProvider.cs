using EPIC.Entities;
using EPIC.Utils;
using EPIC.Utils.Attributes;
using EPIC.Utils.ConstantVariables.Shared;
using EPIC.Utils.DataUtils;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EPIC.GarnerEntities.DataEntities
{
    [Table("GAN_PRODUCT_TRADING_PROVIDER", Schema = DbSchemas.EPIC_GARNER)]
    [Comment("Han muc cho tung dai ly")]
    public class GarnerProductTradingProvider : IFullAudited
    {
        public static string SEQ { get; } = $"SEQ_{(nameof(GarnerProductTradingProvider)).ToSnakeUpperCase()}";

        #region Id
        [Key]
        [ColumnSnackCase(nameof(Id))]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [Required]
        [ColumnSnackCase(nameof(PartnerId))]
        [Comment("Id doi tac")]
        public int PartnerId { get; set; }

        [Required]
        [ColumnSnackCase(nameof(ProductId))]
        [Comment("Id san pham")]
        public int ProductId { get; set; }

        [Required]
        [ColumnSnackCase(nameof(TradingProviderId))]
        [Comment("Id dai ly")]
        public int TradingProviderId { get; set; }
        #endregion

        [Required]
        [ColumnSnackCase(nameof(HasTotalInvestmentSub), TypeName = "VARCHAR2")]
        [MaxLength(1)]
        [Comment("Co cai han muc (Y, N), neu co cai thi phai nhap TotalInvestmentSub, neu khong cai thi cho ban toi da so luong con lai cua san pham tich luy (luu y so luong con lai cua cps va bond tinh bang so luong trai phie/so luong co phan)")]
        public string HasTotalInvestmentSub { get; set; }

        [Required]
        [ColumnSnackCase(nameof(IsProfitFromPartner), TypeName = "VARCHAR2")]
        [MaxLength(1)]
        [Comment("Co nhan loi nhuan tu to chuc phat hanh (Y, N)")]
        public string IsProfitFromPartner { get; set; }

        [ColumnSnackCase(nameof(TotalInvestmentSub))]
        [Comment("Han muc so tien cho tung dai ly")]
        public decimal? TotalInvestmentSub { get; set; }

        [ColumnSnackCase(nameof(Quantity))]
        [Comment("Han muc so luong cho tung dai ly, dung truong nay khi la tich luy Bond hoac Company Shares")]
        public long? Quantity { get; set; }

        [Required]
        [ColumnSnackCase(nameof(DistributionDate), TypeName = "DATE")]
        [Comment("Ngay phan phoi")]
        public DateTime DistributionDate { get; set; }

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
