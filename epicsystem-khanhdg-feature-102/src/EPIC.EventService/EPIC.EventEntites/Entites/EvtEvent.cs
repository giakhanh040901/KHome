using DocumentFormat.OpenXml.Features;
using DocumentFormat.OpenXml.Office2010.Excel;
using EPIC.Entities;
using EPIC.Entities.DataEntities;
using EPIC.Utils.Attributes;
using EPIC.Utils.ConstantVariables.Shared;
using EPIC.Utils.DataUtils;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace EPIC.EventEntites.Entites
{
    /// <summary>
    /// Sự kiện
    /// </summary>
    [Table("EVT_EVENT", Schema = DbSchemas.EPIC_EVENT)]
    [Index(nameof(Deleted), nameof(Status), IsUnique = false, Name = "IX_EVT_EVENT")]

    public class EvtEvent : IFullAudited
    {
        public static string SEQ { get; } = $"{DbConsts.SEQ}{nameof(EvtEvent).ToSnakeUpperCase()}";

        [Key]
        [ColumnSnackCase(nameof(Id))]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }
        /// <summary>
        /// Tên sự kiện
        /// </summary>
        [ColumnSnackCase(nameof(Name))]
        public string Name { get; set; }
        /// <summary>
        /// Ban tổ chức
        /// </summary>
        [ColumnSnackCase(nameof(Organizator))]
        public string Organizator { get; set; }
        /// <summary>
        /// Địa điểm tổ chức
        /// </summary>
        [ColumnSnackCase(nameof(Location))]
        public string Location { get; set; }
        /// <summary>
        /// Thành phố
        /// </summary>
        [ColumnSnackCase(nameof(ProvinceCode))]
        public string ProvinceCode { get; set; }
        [JsonIgnore]
        public Province Province { get; set; }
        /// <summary>
        /// Địa chỉ
        /// </summary>
        [ColumnSnackCase(nameof(Address))]
        public string Address { get; set; }
        /// <summary>
        /// Vĩ độ
        /// </summary>
        [ColumnSnackCase(nameof(Latitude))]
        public string Latitude { get; set; }
        /// <summary>
        /// Kinh độ
        /// </summary>
        [ColumnSnackCase(nameof(Longitude))]
        public string Longitude { get; set; }
        /// <summary>
        /// Đối tượng xem
        /// </summary>
        [ColumnSnackCase(nameof(Viewing))]
        public int Viewing { get; set; }
        /// <summary>
        /// Cấu trúc mã hợp đồng
        /// </summary>
        [ColumnSnackCase(nameof(ConfigContractCodeId))]
        public int ConfigContractCodeId { get; set; }
        [ColumnSnackCase(nameof(TradingProviderId))]
        public int TradingProviderId { get; set; }
        /// <summary>
        /// Tài khoản nhận tiền
        /// </summary>
        [ColumnSnackCase(nameof(BackAccountId))]
        public int BackAccountId { get; set; }
        /// <summary>
        /// Website sự kiện
        /// </summary>
        [ColumnSnackCase(nameof(Website))]
        public string Website { get; set; }
        /// <summary>
        /// Faecbook sự kiện
        /// </summary>
        [ColumnSnackCase(nameof(Facebook))]
        public string Facebook { get; set; }
        [ColumnSnackCase(nameof(Phone))]
        public string Phone { get; set; }
        [ColumnSnackCase(nameof(Email))]
        public string Email { get; set; }
        [ColumnSnackCase(nameof(IsShowApp), TypeName = "VARCHAR2")]
        [MaxLength(1)]
        public string IsShowApp { get; set; }
        /// <summary>
        /// Da kiem tra (Y, N)
        /// </summary>
        [Required]
        [ColumnSnackCase(nameof(IsCheck), TypeName = "VARCHAR2")]
        [MaxLength(1)]
        public string IsCheck { get; set; }
        [ColumnSnackCase(nameof(IsHighlight))]
        public bool IsHighlight { get; set; }
        /// <summary>
        /// Cho phép khác hàng xuất vé bản cứng
        /// </summary>
        [ColumnSnackCase(nameof(CanExportTicket))]
        public bool CanExportTicket { get; set; }
        /// <summary>
        /// Cho phép khách hàng yêu cầu lấy hóa đơn
        /// </summary>
        [ColumnSnackCase(nameof(CanExportRequestRecipt))]
        public bool CanExportRequestRecipt { get; set; }
        /// <summary>
        /// Mô tả
        /// </summary>
        [ColumnSnackCase(nameof(Description))]
        public string Description { get; set; }
        [ColumnSnackCase(nameof(ContentType))]
        public string ContentType { get; set; }
        [ColumnSnackCase(nameof(OverviewContent))]
        public string OverviewContent { get; set; }
        [ColumnSnackCase(nameof(Status))]
        public int Status { get; set; }
        /// <summary>
        /// Chinh sách mua vé
        /// </summary>
        [ColumnSnackCase(nameof(TicketPurchasePolicy), TypeName = "VARCHAR2")]
        [MaxLength(1024)]
        public string TicketPurchasePolicy { get; set; }

        /// <summary>
        /// Id job back ground chạy kết thúc sự kiện
        /// </summary>
        [ColumnSnackCase(nameof(EventFinishedJobId), TypeName = "VARCHAR2")]
        [MaxLength(256)]
        public string EventFinishedJobId { get; set; }
        public List<EvtEventMedia> EventMedias { get; set; } = new();

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

        public List<EvtEventDetail> EventDetails { get; set; } = new();
        public List<EvtTicketTemplate> EvtTicketTemplates { get; set; } = new();
        public List<EvtEventType> EventTypes { get; set; } = new();
        public List<EvtInterestedPerson> InterestedPeople { get; set; } = new();
        public List<EvtSearchHistory> SearchHistorys { get; set; } = new();
        public List<EvtEventBankAccount> EvtEventBankAccounts { get; set; } = new();
        public List<EvtEventDescriptionMedia> EventDescriptionMedias { get; set; } = new();
        public List<EvtAdminEvent> AdminEvents { get; set; } = new();
        public List<EvtDeliveryTicketTemplate> DeliveryTicketTemplates { get; set; } = new();
    }
}
