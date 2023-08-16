using EPIC.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.GarnerEntities.Dto.GarnerOrder
{
    public class CreateGarnerOrderDto
    {
        [Required(ErrorMessage = "Mã cif khách hàng không được bỏ trống")]
        public string CifCode { get; set; }

        [Required(ErrorMessage = "Id chính sách không được bỏ trống")]
        public int PolicyId { get; set; }

        //[Required(ErrorMessage = "Id kỳ hạn không được bỏ trống")]
        //public int? PolicyDetailId { get; set; }

        [Required(ErrorMessage = "Tổng số tiền không được bỏ trống")]
        public decimal TotalValue { get; set; }

        /// <summary>
        /// Tài khoản thụ hưởng
        /// </summary>
        public int? BusinessCustomerBankAccId { get; set; }

        public int? InvestorBankAccId { get; set; }

        /// <summary>
        /// Địa chỉ liên hệ, dùng cho nhà đầu tư cá nhân
        /// </summary>
        public int? ContractAddressId { get; set; }

        public string SaleReferralCode { get; set; }
    }
}
