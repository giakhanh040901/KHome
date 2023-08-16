using DocumentFormat.OpenXml.Wordprocessing;
using EPIC.EntitiesBase.Dto;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.LoyaltyEntities.Dto.LoyPointInvestor
{
    /// <summary>
    /// Bộ lọc Tab danh sách ưu đãi của khách hàng
    /// </summary>
    public class FilterPointInvestorConversionHistoryDto : PagingRequestBaseDto
    {

        [FromQuery(Name = "investorId")]
        public int InvestorId { get; set; }

        /// <summary>
        /// Trạng thái voucher
        /// </summary>
        [FromQuery(Name = "status")]
        public int? Status { get; set; }

        /// <summary>
        /// Loại hình (C; DT)
        /// </summary>
        [FromQuery(Name = "voucherType")]
        public string VoucherType { get; set; }

        /// <summary>
        /// Lấy voucher hết hạn
        /// </summary>
        [FromQuery(Name = "isExpired")]
        public bool? IsExpired { get; set; }
    }
}
