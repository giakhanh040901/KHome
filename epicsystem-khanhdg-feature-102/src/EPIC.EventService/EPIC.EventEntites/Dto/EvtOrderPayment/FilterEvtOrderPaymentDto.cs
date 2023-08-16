using DocumentFormat.OpenXml.Wordprocessing;
using EPIC.EntitiesBase.Dto;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.EventEntites.Dto.EvtOrderPayment
{
    public class FilterEvtOrderPaymentDto : PagingRequestBaseDto
    {
        /// <summary>
        /// Id sổ lệnh
        /// </summary>
        [FromQuery(Name = "orderId")]
        public int OrderId { get; set; }

        /// <summary>
        /// Trạng thái thanh toán (1: Khởi tạo, 2: Đã thanh toán (phe duyet), 3: Huỷ thanh toán (huy duyet))
        /// </summary>
        [FromQuery(Name = "status")]
        public int? Status { get; set; }
    }
}
