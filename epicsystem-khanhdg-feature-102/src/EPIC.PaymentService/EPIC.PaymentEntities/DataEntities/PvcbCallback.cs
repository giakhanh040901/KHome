using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.PaymentEntities.DataEntities
{
    public class PvcbCallback
    {
        public decimal Id { get; set; }
        /// <summary>
        /// Loại giao dịch
        /// </summary>
        public string FtType { get; set; }
        /// <summary>
        /// Giá trị
        /// </summary>
        public decimal? Amount { get; set; }
        /// <summary>
        /// Số dư sau giao dịch 
        /// </summary>
        public decimal? Balance { get; set; }
        /// <summary>
        /// Mã ngân hàng gửi (Dùng bảng mã ngân hàng của PVCB)
        /// </summary>
        public string SenderBankId { get; set; }
        /// <summary>
        /// Nội dung
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Mã Giao dịch ở ngân hàng (FT)
        /// </summary>
        public string TranId { get; set; }
        /// <summary>
        /// Ngày giao dịch (YYYY-MM-DD HH:mm:ss)
        /// </summary>
        public DateTime? TranDate { get; set; }
        /// <summary>
        /// Tiền tệ
        /// </summary>
        public string Currency { get; set; }
        /// <summary>
        /// Trạng thái giao dịch 
        /// </summary>
        public string TranStatus { get; set; }
        /// <summary>
        /// Giá trị qui ra việt nam đồng
        /// </summary>
        public decimal? ConAmount { get; set; }
        /// <summary>
        /// Số thẻ nhận giao dịch, bằng 0 nếu là gd nhận vào tài khoản
        /// </summary>
        public string NumberOfBeneficiary { get; set; }
        /// <summary>
        /// Số tài khoản thụ hưởng
        /// </summary>
        public string Account { get; set; }
        /// <summary>
        /// Ngày tạo
        /// </summary>
        public DateTime? CreatedDate { get; set; }
        /// <summary>
        /// IP request
        /// </summary>
        public string RequestIP { get; set; }
        /// <summary>
        /// Trạng thái
        /// </summary>
        public decimal? Status { get; set; }
        /// <summary>
        /// Token pvcb gửi sang chưa biết để làm gì
        /// </summary>
        public string Token { get; set; }
    }
}
