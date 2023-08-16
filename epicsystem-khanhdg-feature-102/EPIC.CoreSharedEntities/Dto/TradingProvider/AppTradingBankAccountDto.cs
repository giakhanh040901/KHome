using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.CoreSharedEntities.Dto.TradingProvider
{
    public class AppTradingBankAccountDto
    {
        /// <summary>
        /// Id tài khoản đại lý
        /// </summary>
        public int BusinessCustomerBankAccId { get; set; }

        /// <summary>
        /// Mã Qr
        /// </summary>
        public string QrCode { get; set; }

        /// <summary>
        /// Mã ngân hàng Code
        /// </summary>
        public string BankCode { get; set; }

        /// <summary>
        /// Tên ngân hàng
        /// </summary>
        public string BankName { get; set; }

        /// <summary>
        /// Tên ngân hàng đầy đủ
        /// </summary>
        public string FullBankName { get; set; }

        /// <summary>
        /// Số tài khoản ngân hàng
        /// </summary>
        public string BankAccNo { get; set; }

        /// <summary>
        /// Chủ tài khoản
        /// </summary>
        public string BankAccName { get; set; }

        /// <summary>
        /// Logo
        /// </summary>
        public string Logo { get; set; }

        /// <summary>
        /// Số tiền giao dịch
        /// </summary>
        public decimal? TotalValue { get; set; }

        /// <summary>
        /// Nội dung giao dịch
        /// </summary>
        public string Note { get; set; }
        /// <summary>
        /// Avatar đại lý
        /// </summary>
        public string AvatarTradingImageUrl { get; set; }
    }
}
