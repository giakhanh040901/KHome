using EPIC.Utils.ConstantVariables.Loyalty;
using EPIC.Utils.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.LoyaltyEntities.Dto.LoyVoucherInvestor
{
    public class UpdateVoucherInvestorStatusDto
    {
        /// <summary>
        /// Id của VoucherInvestor
        /// </summary>
        public int VoucherInvestorId { get; set; }
        /// <summary>
        /// Trạng thái của voucher với khách
        /// (1: Kích hoạt; 2: Hủy kích hoạt; 3: Đã xóa)
        /// <see cref="LoyVoucherInvestorStatus"/>
        /// </summary>
        [IntegerRange(AllowableValues = new int[] {LoyVoucherInvestorStatus.DA_XOA, LoyVoucherInvestorStatus.HUY_KICH_HOAT, LoyVoucherInvestorStatus.KICH_HOAT})]
        public int Status { get; set; }
    }
}
