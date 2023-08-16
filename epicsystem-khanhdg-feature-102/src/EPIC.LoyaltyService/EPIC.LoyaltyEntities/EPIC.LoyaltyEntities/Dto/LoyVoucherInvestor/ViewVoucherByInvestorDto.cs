using EPIC.Utils.ConstantVariables.Loyalty;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.LoyaltyEntities.Dto.LoyVoucherInvestor
{
    public class ViewVoucherByInvestorDto
    {
        /// <summary>
        /// Id của Voucher và khách
        /// </summary>
        public int? VoucherInvestorId { get; set; }

        /// <summary>
        /// Tên voucher
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Id của voucher
        /// </summary>
        public int VoucherId { get; set; }

        /// <summary>
        /// Loại voucher (C: Cứng; DT: Điện tử)
        /// <see cref="LoyVoucherTypes"/>
        /// </summary>
        public string VoucherType { get; set; }

        /// <summary>
        /// Giá trị voucher
        /// </summary>
        public decimal? Value { get; set; }

        /// <summary>
        /// Ngày áp dụng
        /// </summary>
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// Ngày hết hạn
        /// </summary>
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// Trạng thái của voucher
        /// Trạng thái (0: Khởi tạo; 1: Kích hoạt; 2: Hủy kích hoạt; 3: Đã xóa)<br/>
        /// <see cref="LoyVoucherInvestorStatus"/>
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// Ngày tạo voucher
        /// </summary>
        public DateTime? CreatedDate { get; set; }
        /// <summary>
        /// Người tạo
        /// </summary>
        public string CreatedBy { get; set; }
    }
}
