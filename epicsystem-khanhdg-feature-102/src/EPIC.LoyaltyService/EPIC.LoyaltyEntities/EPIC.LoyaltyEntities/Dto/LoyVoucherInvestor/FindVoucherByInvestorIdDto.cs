using EPIC.EntitiesBase.Dto;
using EPIC.Utils.ConstantVariables.Loyalty;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.LoyaltyEntities.Dto.LoyVoucherInvestor
{
    public class FindVoucherByInvestorIdDto : PagingRequestBaseDto
    {
        [FromQuery(Name = "investorId")]
        public int InvestorId { get; set; }

        /// <summary>
        /// Loại voucher (C: Cứng; DT: Điện tử)
        /// </summary>
        [FromQuery(Name = "voucherType")]
        public string VoucherType { get; set; }

        /// <summary>
        /// Trạng thái của voucher
        /// Trạng thái (0: Khởi tạo; 1: Kích hoạt; 2: Hủy kích hoạt; 3: Đã xóa)<br/>
        /// <see cref="LoyVoucherInvestorStatus"/>
        /// </summary>
        [FromQuery(Name = "status")]
        public int? Status { get; set; }
    }
}
