using EPIC.Utils.Attributes;
using EPIC.Utils.ConstantVariables.Shared;
using EPIC.Utils.DataUtils;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.PaymentEntities.DataEntities
{

    [Table("EP_MSB_NOTIFICATION_PAYMENT", Schema = DbSchemas.EPIC)]
    [Comment("Thong tin chi ho thanh cong Msb")]
    public class MsbNotificationPayment
    {
        public static string SEQ = $"SEQ_{nameof(MsbNotificationPayment).ToSnakeUpperCase()}";

        [Key]
        [ColumnSnackCase(nameof(Id))]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long Id { get; set; }

        [ColumnSnackCase(nameof(TransId))]
        public string TransId { get; set; }

        [ColumnSnackCase(nameof(TransDate))]
        public string TransDate { get; set; }

        [ColumnSnackCase(nameof(TId))]
        public string TId { get; set; }

        [ColumnSnackCase(nameof(MId))]
        public string MId { get; set; }

        [ColumnSnackCase(nameof(Note))]
        public string Note { get; set; }

        [ColumnSnackCase(nameof(SenderName))]
        public string SenderName { get; set; }

        [ColumnSnackCase(nameof(ReceiveName))]
        public string ReceiveName { get; set; }

        [ColumnSnackCase(nameof(SenderAccount))]
        public string SenderAccount { get; set; }

        [ColumnSnackCase(nameof(ReceiveAccount))]
        public string ReceiveAccount { get; set; }

        [ColumnSnackCase(nameof(ReceiveBank))]
        public string ReceiveBank { get; set; }

        [ColumnSnackCase(nameof(Amount))]
        public string Amount { get; set; }

        [ColumnSnackCase(nameof(Fee))]
        public string Fee { get; set; }

        [ColumnSnackCase(nameof(SecureHash))]
        public string SecureHash { get; set; }

        [ColumnSnackCase(nameof(NapasTransId))]
        public string NapasTransId { get; set; }

        [ColumnSnackCase(nameof(Rrn))]
        public string Rrn { get; set; }

        [ColumnSnackCase(nameof(Status))]
        [Comment("Trạng thái chuyển khoản 1: Thành công, 0: thất bại")]
        public string Status { get; set; }

        [ColumnSnackCase(nameof(HandleStatus))]
        [Comment("Trang thai xư ly: 1: THAT BAI, 2 THANH CONG")]
        public int HandleStatus { get; set; }

        [ColumnSnackCase(nameof(Exception))]
        [Comment("Ngoai le")]
        public string Exception { get; set; }

        [ColumnSnackCase(nameof(Ip), TypeName = "VARCHAR2")]
        public string Ip { get; set; }

        [ColumnSnackCase(nameof(CreatedDate), TypeName = "DATE")]
        public DateTime CreatedDate { get; set; }
    }
}
