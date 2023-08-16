using DocumentFormat.OpenXml.Wordprocessing;
using EPIC.Entities;
using EPIC.Entities.DataEntities;
using EPIC.EntitiesBase.Interfaces.Audit;
using EPIC.Utils;
using EPIC.Utils.Attributes;
using EPIC.Utils.ConstantVariables.Event;
using EPIC.Utils.ConstantVariables.Shared;
using EPIC.Utils.DataUtils;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace EPIC.EventEntites.Entites
{
    /// <summary>
    /// Sổ lệnh
    /// </summary>
    [Table("EVT_ORDER", Schema = DbSchemas.EPIC_EVENT)]
    [Index(nameof(Deleted), nameof(TradingProviderId), nameof(EventDetailId), nameof(InvestorId), nameof(InvestorIdenId), nameof(ContractAddressId), IsUnique = false, Name = "IX_EVT_ORDER")]
    public class EvtOrder : IFullAudited, IApproveAudit
    {
        public static string SEQ { get; } = $"{DbConsts.SEQ}{nameof(EvtOrder).ToSnakeUpperCase()}";

        [Key]
        [ColumnSnackCase(nameof(Id))]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }
        [ColumnSnackCase(nameof(TradingProviderId))]
        public int TradingProviderId { get; set; }
        public TradingProvider TradingProvider { get; set; }
        [ColumnSnackCase(nameof(KeepTicketJobId))]
        public string KeepTicketJobId { get; set; }
        [ColumnSnackCase(nameof(ContractCode))]
        public string ContractCode { get; set; }
        [ColumnSnackCase(nameof(ContractCodeGen))]
        public string ContractCodeGen { get; set; }
        [ColumnSnackCase(nameof(EventDetailId))]
        public int EventDetailId { get; set; }
        public EvtEventDetail EventDetail { get; set; }

        [ColumnSnackCase(nameof(DepartmentId))]
        public int? DepartmentId { get; set; }
        public Department Department { get; set; }

        [ColumnSnackCase(nameof(Source))]
        public int Source { get; set; }
        [ColumnSnackCase(nameof(ReferralSaleId))]
        public int? ReferralSaleId { get; set; }
        public Sale Sale { get; set; }
        [ColumnSnackCase(nameof(ContractAddressId))]
        public int? ContractAddressId { get; set; }
        public InvestorContactAddress ContractAddress { get; set; }
        [ColumnSnackCase(nameof(InvestorId))]
        public int InvestorId { get; set; }
        public Investor Investor { get; set; }
        [ColumnSnackCase(nameof(InvestorIdenId))]
        public int InvestorIdenId { get; set; }
        public InvestorIdentification InvestorIdentification { get; set; }
        /// <summary>
        /// Thời gian thanh toán còn lại
        /// </summary>
        [ColumnSnackCase(nameof(ExpiredTime))]
        public DateTime? ExpiredTime { get; set; }
        /// <summary>
        /// Nhận vé bản cứng (Yes/No)
        /// </summary>
        [ColumnSnackCase(nameof(IsReceiveHardTicket))]
        public bool IsReceiveHardTicket { get; set; }
        /// <summary>
        /// Yêu cầu nhận hóa đơn (Yes/No)
        /// </summary>
        [ColumnSnackCase(nameof(IsRequestReceiveRecipt))]
        public bool IsRequestReceiveRecipt { get; set; }
        /// <summary>
        /// <see cref="EvtOrderStatus"/>
        /// </summary>
        [ColumnSnackCase(nameof(Status))]
        public int Status { get; set; }
        [ColumnSnackCase(nameof(IpAddressCreated))]
        public string IpAddressCreated { get; set; }
        /// <summary>
        /// Free vé hay không
        /// </summary>
        [ColumnSnackCase(nameof(IsFree))]
        public bool IsFree { get; set; }
        /// <summary>
        /// Tạm khóa hay không
        /// </summary>
        [ColumnSnackCase(nameof(IsLock))]
        public bool IsLock { get; set; }

        [ColumnSnackCase(nameof(ApproveBy))]
        [MaxLength(50)]
        public string ApproveBy { get; set; }

        [ColumnSnackCase(nameof(ApproveDate), TypeName = "DATE")]
        public DateTime? ApproveDate { get; set; }
        /// <summary>
        /// <see cref="EventDeliveryStatus"/>
        /// Trạng thái giao nhận vé sự kiện: (1: Chờ xử lý, 2: Đang giao, 3: Hoàn thành)
        /// </summary>
        [ColumnSnackCase(nameof(DeliveryStatus))]
        public int? DeliveryStatus { get; set; }

        [ColumnSnackCase(nameof(DeliveryDate), TypeName = "DATE")]
        public DateTime? DeliveryDate { get; set; }

        [ColumnSnackCase(nameof(DeliveryDateModifiedBy))]
        [MaxLength(50)]
        public string DeliveryDateModifiedBy { get; set; }

        [ColumnSnackCase(nameof(PendingDate), TypeName = "DATE")]
        public DateTime? PendingDate { get; set; }

        [ColumnSnackCase(nameof(PendingDateModifiedBy))]
        [MaxLength(50)]
        public string PendingDateModifiedBy { get; set; }

        [ColumnSnackCase(nameof(FinishedDate), TypeName = "DATE")]
        public DateTime? FinishedDate { get; set; }

        [ColumnSnackCase(nameof(FinishedDateModifiedBy))]
        [MaxLength(50)]
        public string FinishedDateModifiedBy { get; set; }
        /// <summary>
        /// <see cref="EventDeliveryStatus"/>
        /// Trạng thái giao nhận hóa đơn: (1: Chờ xử lý, 2: Đang giao, 3: Hoàn thành)
        /// </summary>
        [ColumnSnackCase(nameof(DeliveryInvoiceStatus))]
        public int? DeliveryInvoiceStatus { get; set; }

        [ColumnSnackCase(nameof(DeliveryInvoiceDate), TypeName = "DATE")]
        public DateTime? DeliveryInvoiceDate { get; set; }

        [ColumnSnackCase(nameof(DeliveryInvoiceDateModifiedBy))]
        [MaxLength(50)]
        public string DeliveryInvoiceDateModifiedBy { get; set; }

        [ColumnSnackCase(nameof(PendingInvoiceDate), TypeName = "DATE")]
        public DateTime? PendingInvoiceDate { get; set; }

        [ColumnSnackCase(nameof(PendingInvoiceDateModifiedBy))]
        [MaxLength(50)]
        public string PendingInvoiceDateModifiedBy { get; set; }

        [ColumnSnackCase(nameof(FinishedInvoiceDate), TypeName = "DATE")]
        public DateTime? FinishedInvoiceDate { get; set; }

        [ColumnSnackCase(nameof(FinishedInvoiceDateModifiedBy))]
        [MaxLength(50)]
        public string FinishedInvoiceDateModifiedBy { get; set; }
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

        public List<EvtOrderDetail> OrderDetails { get; } = new();
        public List<EvtOrderPayment> OrderPayments { get; } = new();

        /// <summary>
        /// Job fill ticket
        /// </summary>
        [ColumnSnackCase(nameof(TicketJobId))]
        [MaxLength(256)]
        public string TicketJobId { get; set; }

        /// <summary>
        /// Job fill ticket
        /// </summary>
        [ColumnSnackCase(nameof(DeliveryCode))]
        [MaxLength(256)]
        public string DeliveryCode { get; set; }
    }
}
