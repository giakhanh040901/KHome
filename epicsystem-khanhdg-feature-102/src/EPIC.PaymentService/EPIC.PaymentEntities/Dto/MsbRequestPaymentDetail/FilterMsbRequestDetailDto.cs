using EPIC.EntitiesBase.Dto;
using Microsoft.AspNetCore.Mvc;
using System;

namespace EPIC.PaymentEntities.Dto.MsbRequestPaymentDetail
{
    public class FilterMsbRequestDetailDto : PagingRequestBaseDto
    {
        [FromQuery(Name = "status")]
        public int? Status { get; set; }
        [FromQuery(Name = "productType")]
        public int? ProductType { get; set; }
        [FromQuery(Name = "approveDate")]
        public DateTime? ApproveDate { get; set; }

        [FromQuery(Name = "contractCode")]
        public string ContractCode { get; set; }

        [FromQuery(Name = "tradingProviderId")]
        public int? TradingProviderId { get; set; }
    }
}
