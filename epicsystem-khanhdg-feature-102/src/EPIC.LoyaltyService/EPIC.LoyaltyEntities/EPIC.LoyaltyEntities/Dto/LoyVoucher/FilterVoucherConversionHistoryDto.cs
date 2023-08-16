using DocumentFormat.OpenXml.Wordprocessing;
using EPIC.EntitiesBase.Dto;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.LoyaltyEntities.Dto.LoyVoucher
{
    public class FilterVoucherConversionHistoryDto: PagingRequestBaseDto
    {
        /// <summary>
        /// Trạng thái voucher
        /// </summary>
        [FromQuery(Name = "status")]
        public int? Status { get; set; }

        /// <summary>
        /// Ngày cấp phát
        /// </summary>
        [FromQuery(Name = "conversionPointFinishedDate")]
        public DateTime? ConversionPointFinishedDate { get; set; }

        /// <summary>
        /// Loại voucher
        /// </summary>
        [FromQuery(Name = "useType")]
        public string UseType { get; set; }

        /// <summary>
        /// Loại hình (C; DT)
        /// </summary>
        [FromQuery(Name = "voucherType")]
        public string VoucherType { get; set; }
    }
}
