using EPIC.Utils.Validation;
using EPIC.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.CompanySharesEntities.Dto.Order
{
    public class BaseOrderDto
    {
        [Required(ErrorMessage = "Mã cif khách hàng không được bỏ trống")]
        public string CifCode { get; set; }

        [Required(ErrorMessage = "Id bán theo kỳ hạn (cổ phần) không được bỏ trống")]
        public int? SecondaryId { get; set; }

        [Required(ErrorMessage = "Id chính sách không được bỏ trống")]
        public int? PolicyId { get; set; }

        [Required(ErrorMessage = "Id kỳ hạn không được bỏ trống")]
        public int? PolicyDetailId { get; set; }

        [Required(ErrorMessage = "Tổng số tiền không được bỏ trống")]
        public decimal? TotalValue { get; set; }

        //[Required(ErrorMessage = "Ngày mua không được bỏ trống")]
        //public DateTime? BuyDate { get; set; }

        /// <summary>
        /// Tài khoản thụ hưởng
        /// </summary>
        [Required(ErrorMessage = "Tài khoản thụ hưởng của khách hàng không được bỏ trống")]
        public int? InvestorBankAccId { get; set; }

        [Required(ErrorMessage = "Có trả lãi hay không không được bỏ trống")]
        [StringRange(AllowableValues = new string[] { YesNo.YES, YesNo.NO })]
        public string IsInterest { get; set; }
        public string SaleReferralCode { get; set; }
        public int? ContractAddressId { get; set; }
    }

    public class CreateOrderDto : BaseOrderDto
    {
    }
}
