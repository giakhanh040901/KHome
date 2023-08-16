using EPIC.Utils.ConstantVariables.Loyalty;
using EPIC.Utils.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.LoyaltyEntities.Dto.LoyVoucher
{
    public class UpdateVoucherStatusDto
    {
        /// <summary>
        /// Id của Voucher
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Trạng thái của voucher với khách
        /// (2: Kích hoạt; 3: Hủy kích hoạt; 4: Đã xóa)
        /// <see cref="LoyVoucherStatus"/>
        /// </summary>
        [IntegerRange(AllowableValues = new int[] { LoyVoucherStatus.DA_XOA, LoyVoucherStatus.HUY_KICH_HOAT, LoyVoucherStatus.KICH_HOAT })]
        public int Status { get; set; }
    }
}
