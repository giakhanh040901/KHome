using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.InvestEntities.Dto.Order
{
    public class AppCheckOrderDto
    {
        /// <summary>
        /// Kỳ hạn
        /// </summary>
        [Required(ErrorMessage = "Kỳ hạn không được bỏ trống")]
        [Range(1, int.MaxValue)]
        public int? PolicyDetailId { get; set; }

        /// <summary>
        /// Khuyến mãi
        /// </summary>
        [Range(1, int.MaxValue)]
        public int? PromotionId { get; set; }

        /// <summary>
        /// Số tiền đầu tư
        /// </summary>
        [Required(ErrorMessage = "Số tiền đầu tư không được bỏ trống")]
        [Range(1, double.MaxValue)]
        public decimal? TotalValue { get; set; }

        /// <summary>
        /// Tài khoản thụ hưởng
        /// </summary>
        [Required(ErrorMessage = "Tài khoản thụ hưởng không được bỏ trống")]
        [Range(1, int.MaxValue)]
        public int? BankAccId { get; set; }

        /// <summary>
        /// Id cccd
        /// </summary>
        [Required(ErrorMessage = "Giấy tờ không được bỏ trống")]
        [Range(1, int.MaxValue)]
        public int? IdentificationId { get; set; }

        /// <summary>
        /// Có nhận hợp đồng
        /// </summary>
        public bool IsReceiveContract { get; set; }

        /// <summary>
        /// id địa chỉ giao dịch
        /// </summary>
        [Range(1, int.MaxValue)]
        public int? TranAddess { get; set; }

        /// <summary>
        /// Mã giới thiệu
        /// </summary>
        //[Required(AllowEmptyStrings = false, ErrorMessage = "Mã giới thiệu không được bỏ trống")]
        public string ReferralCode { get; set; }
    }
}
