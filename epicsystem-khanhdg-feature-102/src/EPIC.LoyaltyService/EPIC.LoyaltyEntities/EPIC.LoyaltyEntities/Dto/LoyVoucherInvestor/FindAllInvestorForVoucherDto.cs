using EPIC.EntitiesBase.Dto;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.LoyaltyEntities.Dto.LoyVoucherInvestor
{
    public class FindAllInvestorForVoucherDto : PagingRequestBaseDto
    {
        /// <summary>
        /// Giới tính
        /// </summary>
        [FromQuery(Name = "sex")]
        public string Sex { get; set; }

        /// <summary>
        /// Đã cấp voucher chưa
        /// </summary>
        [FromQuery(Name = "isAddedVoucher")]
        public bool? IsAddedVoucher { get; set; }

        /// <summary>
        /// Tài khoản đã được xác thực chưa
        /// </summary>
        [FromQuery(Name = "isCheckedInvestor")]
        public bool? IsCheckedInvestor { get; set; }

        /// <summary>
        /// Hạng (chưa có; để cho vui)
        /// </summary>
        [FromQuery(Name = "rank")]
        public int? Rank { get; set; }

    }
}
