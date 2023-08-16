using EPIC.Utils.Attributes;
using EPIC.Utils.ConstantVariables.Shared;
using EPIC.Utils.DataUtils;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.PaymentEntities.DataEntities
{
    [Table("EP_MSB_REQUEST_PAYMENT_DETAIL", Schema = DbSchemas.EPIC)]
    [Comment("Thong tin chi tiet chi tra MSB")]
    public class MsbRequestPaymentDetail
    {
        public static string SEQ = $"SEQ_{nameof(MsbRequestPaymentDetail).ToSnakeUpperCase()}";

        /// <summary>
        /// Id giao dịch
        /// </summary>
        [Key]
        [ColumnSnackCase(nameof(Id))]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long Id { get; set; }

        [Required]
        [ColumnSnackCase(nameof(RequestId))]
        [Comment("Id của yêu cầu chi trả")]
        public long RequestId { get; set; }

        [Required]
        [ColumnSnackCase(nameof(DataType))]
        [Comment("Dữ liệu thuộc bảng nào (1 là bảng nào, 2 là bảng nào, ghi thêm vào đây)")]
        public int DataType { get; set; }

        [Required]
        [ColumnSnackCase(nameof(ReferId))]
        [Comment("ReferId đến dữ liệu trong bảng của DataType")]
        public long ReferId { get; set; }

        [Required]
        [ColumnSnackCase(nameof(AmountMoney))]
        [Comment("Số tiền")]
        public decimal AmountMoney { get; set; }

        [Required]
        [ColumnSnackCase(nameof(Status))]
        [Comment("Trạng thái của request (1. Khời tạo, 2. Thành công (Success), 3. Thất bại (Failed))")]
        public int Status { get; set; }

        [Required]
        [ColumnSnackCase(nameof(BankId))]
        [Comment("Id ngân hàng nhận")]
        public int BankId { get; set; }

        [Required]
        [ColumnSnackCase(nameof(OwnerAccount))]
        [Comment("tên chủ tài khoản ngân hàng nhận")]
        public string OwnerAccount { get; set; }

        [ColumnSnackCase(nameof(OwnerAccountNo))]
        [Comment("So tài khoản ngân hàng nhận")]
        public string OwnerAccountNo { get; set; }

        [ColumnSnackCase(nameof(Bin))]
        [Comment("tài khoản ngân hàng nhận")]
        public string Bin { get; set; }

        [ColumnSnackCase(nameof(Note))]
        [Comment("Nội dung chuyển")]
        public string Note { get; set; }

        [ColumnSnackCase(nameof(Exception))]
        [Comment("Thông báo của MSB")]
        public string Exception { get; set; }

        [ColumnSnackCase(nameof(TradingBankAccId))]
        [Comment("Tài khoản đại lý")]
        public int TradingBankAccId { get; set; }
    }
}
