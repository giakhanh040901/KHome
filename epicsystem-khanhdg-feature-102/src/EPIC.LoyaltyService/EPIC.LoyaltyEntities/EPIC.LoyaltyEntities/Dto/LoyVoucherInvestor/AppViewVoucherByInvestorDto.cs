using EPIC.Utils.ConstantVariables.Loyalty;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.LoyaltyEntities.Dto.LoyVoucherInvestor
{
    public class AppViewVoucherByInvestorDto
    {
        /// <summary>
        /// Id của Voucher và khách
        /// </summary>
        public int? Id { get; set; }

        /// <summary>
        /// Tên voucher
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Tên hiển thị trên ứng dụng
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// Ảnh voucher
        /// </summary>
        public string Avatar { get; set; }

        /// <summary>
        /// Loại voucher (C: Cứng; DT: Điện tử)
        /// <see cref="LoyVoucherTypes"/>
        /// </summary>
        public string VoucherType { get; set; }
        /// <summary>
        /// Link voucher
        /// </summary>
        public string LinkVoucher { get; set; }

        /// <summary>
        /// Giá trị voucher
        /// </summary>
        public decimal? Value { get; set; }

        /// <summary>
        /// Điểm quy đổi
        /// </summary>
        public int Point { get; set; }

        /// <summary>
        /// Ngày áp dụng
        /// </summary>
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// Ngày kết thúc
        /// </summary>
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// Ngày hết hạn
        /// </summary>
        public DateTime? ExpiredDate { get; set; }

        /// <summary>
        /// Ngày khách nhận voucher
        /// </summary>
        public DateTime? CreatedDate { get; set; }
    }
}
