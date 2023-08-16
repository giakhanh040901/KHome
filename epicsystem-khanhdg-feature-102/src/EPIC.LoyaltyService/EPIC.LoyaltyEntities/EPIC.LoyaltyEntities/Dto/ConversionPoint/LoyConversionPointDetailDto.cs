using EPIC.LoyaltyEntities.Dto.LoyVoucher;
using EPIC.Utils.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.LoyaltyEntities.Dto.ConversionPoint
{
    public class LoyConversionPointDetailDto
    {
        public int Id { get; set; }
        public int VoucherId { get; set; }

        /// <summary>
        /// Thông tin chi tiết của Voucher
        /// </summary>
        public ViewVoucherDto VoucherInfo { get; set; }

        /// <summary>
        /// Điểm quy đổi theo Voucher
        /// </summary>
        public int Point { get; set; }

        /// <summary>
        /// Số lượng Voucher yêu cầu
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// Tổng điểm đổi
        /// </summary>
        public int TotalConversionPoint { get; set; }
    }
}
