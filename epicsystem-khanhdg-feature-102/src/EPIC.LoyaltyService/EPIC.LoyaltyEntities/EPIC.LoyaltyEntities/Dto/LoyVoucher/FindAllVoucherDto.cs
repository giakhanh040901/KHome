using EPIC.EntitiesBase.Dto;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.LoyaltyEntities.Dto.LoyVoucher
{
    /// <summary>
    /// Tìm kiếm list voucher ở cms
    /// </summary>
    public class FindAllVoucherDto : PagingRequestBaseDto
    {
        /// <summary>
        /// Trạng thái voucher
        /// </summary>
        [FromQuery(Name = "status")]
        public int? Status { get; set; }

        /// <summary>
        /// Ngày hết hạn
        /// </summary>
        [FromQuery(Name = "expiredDate")]
        public DateTime? ExpiredDate { get; set; }

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

        /// <summary>
        /// Hiển thị trên ứng dụng (rỗng; Y; N)
        /// </summary>
        [FromQuery(Name = "isShowApp")]
        public string IsShowApp { get; set; }
    }
}
