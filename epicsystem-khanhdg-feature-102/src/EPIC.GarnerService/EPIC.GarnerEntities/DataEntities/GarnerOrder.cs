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
using EPIC.Entities.DataEntities;
using EPIC.Entities.Dto.ManagerInvestor;
using System.Text.Json.Serialization;

namespace EPIC.GarnerEntities.DataEntities
{
    [Table("GAN_ORDER", Schema = DbSchemas.EPIC_GARNER)]
    [Comment("So lenh")]
    public class GarnerOrder : IFullAudited
    {
        public static string SEQ = $"SEQ_{nameof(GarnerOrder).ToSnakeUpperCase()}";

        [Key]
        [ColumnSnackCase(nameof(Id))]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long Id { get; set; }

        [ColumnSnackCase(nameof(TradingProviderId))]
        public int TradingProviderId { get; set; }

        [ColumnSnackCase(nameof(CifCode), TypeName = "VARCHAR2")]
        [MaxLength(50)]
        [Comment("Ma cif khach hang ca nhan lan khach hang doanh nghiep")]
        public string CifCode { get; set; }
        [JsonIgnore]
        public CifCodes CifCodes { get; set; }

        [ColumnSnackCase(nameof(DepartmentId))]
        [Comment("Phong giao dich quan ly")]
        public int? DepartmentId { get; set; }

        [ColumnSnackCase(nameof(ProductId))]
        [Comment("San pham tich luy")]
        public int ProductId { get; set; }
        public GarnerProduct Product { get; set; }

        [ColumnSnackCase(nameof(DistributionId))]
        [Comment("Ban phan phoi")]
        public int DistributionId { get; set; }
        public GarnerDistribution Distribution { get; set; }

        [ColumnSnackCase(nameof(PolicyId))]
        [Comment("Chinh sach")]
        public int PolicyId { get; set; }
        public GarnerPolicy Policy { get; set; }

        [ColumnSnackCase(nameof(PolicyDetailId))]
        [Comment("Ky han co the null neu nhu loai tich luy la tich luy theo thoi gian hoac so tien")]
        public int? PolicyDetailId { get; set; }
        public GarnerPolicyDetail PolicyDetail { get; set; }

        [ColumnSnackCase(nameof(TotalValue), TypeName = "DECIMAL(18, 2)")]
        [Comment("Tong tien dau tu hien tai")]
        public decimal TotalValue { get; set; }

        [ColumnSnackCase(nameof(InitTotalValue), TypeName = "DECIMAL(18, 2)")]
        [Comment("Tong tien dau tu ban dau")]
        public decimal InitTotalValue { get; set; }

        [ColumnSnackCase(nameof(BuyDate), TypeName = "DATE")]
        [Comment("Ngay dat lenh")]
        public DateTime BuyDate { get; set; }

        [ColumnSnackCase(nameof(Status))]
        [Comment("Trang thai 1 khoi tao 2 cho thanh toan Khi co 1 ban ghi thanh toan hoac dau tu online 3 cho ky hop dong khi dat tren app vao tt nay luon sau khi nhap pin otp thi sang cho thanh toan 4 cho duyet hop dong khi co 1 ban ghi file hop dong day len 5 dang dau tu 6 phong toa 7 Giai toa 8 tat toan")]
        public int Status { get; set; }

        [ColumnSnackCase(nameof(Source))]
        [Comment("Nguon dat lenh 1 online 2 offline")]
        public int Source { get; set; }

        [ColumnSnackCase(nameof(ContractCode), TypeName = "VARCHAR2")]
        [MaxLength(100)]
        [Comment("EG 8 so 000 kem id")]
        public string ContractCode { get; set; }

        [ColumnSnackCase(nameof(TradingBankAccId))]
        [Comment("Id tai khoan thu huong cua dai ly so cap dlsc dang chon o phan phoi san pham")]
        public int TradingBankAccId { get; set; }

        [ColumnSnackCase(nameof(BusinessCustomerBankAccId))]
        [Comment("Id tai khoan thu huong cua khach hang doanh nghiep")]
        public int? BusinessCustomerBankAccId { get; set; }

        [ColumnSnackCase(nameof(PaymentFullDate), TypeName = "DATE")]
        [Comment("Ngay thanh toan du neu truong nay null thi tinh toan loi tuc o date time now khi thanh toan khong du hoac thua thi la NULL")]
        public DateTime? PaymentFullDate { get; set; }

        [ColumnSnackCase(nameof(InvestorBankAccId))]
        [Comment("Tai khoan thu huong cua khach hang")]
        public int? InvestorBankAccId { get; set; }

        [ColumnSnackCase(nameof(SaleReferralCode), TypeName = "VARCHAR2")]
        [MaxLength(50)]
        [Comment("Ma gioi thieu nhan vien tu van")]
        public string SaleReferralCode { get; set; }

        [ColumnSnackCase(nameof(DeliveryStatus))]
        [Comment("Trang thai giao nhan hop dong")]
        public int? DeliveryStatus { get; set; }

        [ColumnSnackCase(nameof(IpAddressCreated), TypeName = "VARCHAR2")]
        [MaxLength(50)]
        [Comment("Ip dat lenh")]
        public string IpAddressCreated { get; set; }

        [ColumnSnackCase(nameof(InvestorIdenId))]
        [Comment("Giay to cua nha dau tu ca nhan")]
        public int? InvestorIdenId { get; set; }
        [JsonIgnore]
        public InvestorIdentification InvestorIdentification { get; set; }

        [ColumnSnackCase(nameof(ContractAddressId))]
        [Comment("Dia chi cua nha dau tu ca nhan")]
        public int? ContractAddressId { get; set; }

        [ColumnSnackCase(nameof(DeliveryCode), TypeName = "VARCHAR2")]
        [MaxLength(50)]
        [Comment("Code giao nhan hop dong")]
        public string DeliveryCode { get; set; }

        [ColumnSnackCase(nameof(ActiveDate), TypeName = "DATE")]
        [Comment("Ngay hop dong Active Ngay so lenh Active")]
        public DateTime? ActiveDate { get; set; }

        [ColumnSnackCase(nameof(InvestDate), TypeName = "DATE")]
        [Comment("Ngay dau tu (ngay nay cho lua chon) de tinh tien")]
        public DateTime? InvestDate { get; set; }

        [ColumnSnackCase(nameof(SettlementDate), TypeName = "DATE")]
        [Comment("Ngay tat toan (ngay rut het tien trong lenh)")]
        public DateTime? SettlementDate { get; set; }

        [ColumnSnackCase(nameof(SaleOrderId))]
        [Comment("Id sale dat lenh ho cho khach")]
        public int? SaleOrderId { get; set; }

        [ColumnSnackCase(nameof(PendingDate), TypeName = "DATE")]
        [Comment("Ngay thay doi trang thai giao nhan hop dong sang cho xu ly")]
        public DateTime? PendingDate { get; set; }

        [ColumnSnackCase(nameof(DeliveryDate), TypeName = "DATE")]
        [Comment("Ngay thay doi trang thai giao nhan hop dong dang giao hop dong")]
        public DateTime? DeliveryDate { get; set; }

        [ColumnSnackCase(nameof(ReceivedDate), TypeName = "DATE")]
        [Comment("Ngay thay doi trang thai giao nhan hop dong sang da nhan")]
        public DateTime? ReceivedDate { get; set; }

        [ColumnSnackCase(nameof(FinishedDate), TypeName = "DATE")]
        [Comment("Ngay thay doi trang thai giao nhan hop dong sang hoan thanh")]
        public DateTime? FinishedDate { get; set; }

        [ColumnSnackCase(nameof(PendingDateModifiedBy), TypeName = "VARCHAR2")]
        [MaxLength(50)]
        public string PendingDateModifiedBy { get; set; }

        [ColumnSnackCase(nameof(DeliveryDateModifiedBy), TypeName = "VARCHAR2")]
        [MaxLength(50)]
        public string DeliveryDateModifiedBy { get; set; }

        [ColumnSnackCase(nameof(ReceivedDateModifiedBy), TypeName = "VARCHAR2")]
        [MaxLength(50)]
        public string ReceivedDateModifiedBy { get; set; }

        [ColumnSnackCase(nameof(FinishedDateModifiedBy), TypeName = "VARCHAR2")]
        [MaxLength(50)]
        public string FinishedDateModifiedBy { get; set; }

        [ColumnSnackCase(nameof(DepartmentIdSub))]
        [Comment("Phong ban dai ly ban ho")]
        public int? DepartmentIdSub { get; set; }

        [ColumnSnackCase(nameof(SaleReferralCodeSub), TypeName = "VARCHAR2")]
        [MaxLength(50)]
        [Comment("Ma gioi thieu saler cua ly ban ho")]
        public string SaleReferralCodeSub { get; set; }

        [ColumnSnackCase(nameof(RenewalsPolicyId))]
        [Comment("Chinh sach cai dat khi tai tuc")]
        public int? RenewalsPolicyId { get; set; }

        [ColumnSnackCase(nameof(RenewalsPolicyDetailId))]
        [Comment("Ky han cai dat khi tai tuc")]
        public int? RenewalsPolicyDetailId { get; set; }

        [ColumnSnackCase(nameof(SettlementMethod))]
        [Comment("Phuong thuc tat toan 1 tat toan khong tai tuc 2 nhan loi nhuan va tai tuc goc 3 tai tuc goc va loi nhuan")]
        public int? SettlementMethod { get; set; }

        [ColumnSnackCase(nameof(ApproveBy))]
        [Comment("Nguoi duyet hop dong")]
        public string ApproveBy { get; set; }

        [ColumnSnackCase(nameof(ApproveDate))]
        [Comment("Ngay duyet hop dong")]
        public DateTime? ApproveDate { get; set; }

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
        [DefaultValue(YesNo.NO)]
        [MaxLength(1)]
        public string Deleted { get; set; }
        #endregion
        [JsonIgnore]
        public List<GarnerOrderContractFile> OrderContracFiles { get; set; }
        [NotMapped]
        public string CustomerName { get; set; }
    }
}
