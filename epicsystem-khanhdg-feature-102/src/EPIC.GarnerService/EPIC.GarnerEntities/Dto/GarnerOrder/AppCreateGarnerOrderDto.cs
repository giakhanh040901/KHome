using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.GarnerEntities.Dto.GarnerOrder
{
    public class AppCreateGarnerOrderDto
    {
        /// <summary>
        /// Id của chính sách
        /// </summary>
        public int PolicyId { get; set; }

        /// <summary>
        /// Số tiền tích lũy
        /// </summary>
        public decimal TotalValue { get; set; }

        /// <summary>
        /// Ngân hàng thụ hưởng của nhà đầu tư
        /// </summary>
        public int InvestorBankAccId { get; set; }

        /// <summary>
        /// Địa chỉ của nhà đầu tư cá nhân
        /// </summary>
        public int? ContractAddressId { get; set; }

        private string _saleReferralCode;
        /// <summary>
        /// Mã tư vấn viên
        /// </summary>
        public string SaleReferralCode
        {
            get => _saleReferralCode;
            set => _saleReferralCode = value?.Trim();
        }

        private string _otp;
        /// <summary>
        /// OTP
        /// </summary>
        public string Otp
        {
            get => _otp;
            set => _otp = value?.Trim();
        }
    }
}
