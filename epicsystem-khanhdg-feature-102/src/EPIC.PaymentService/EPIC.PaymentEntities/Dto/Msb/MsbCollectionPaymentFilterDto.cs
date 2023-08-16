using DocumentFormat.OpenXml.Wordprocessing;
using EPIC.EntitiesBase.Dto;
using EPIC.Utils.ConstantVariables.Payment;
using EPIC.Utils.Validation;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.PaymentEntities.Dto.Msb
{
    public class MsbCollectionPaymentFilterDto : PagingRequestBaseDto
    {
        [FromQuery(Name = "status")]
        [IntegerRange(AllowableValues = new int[] { RequestPaymentStatus.KHOI_TAO, RequestPaymentStatus.SUCCESS, RequestPaymentStatus.FAILED})]
        public int? Status { get; set; }
        [FromQuery(Name = "productType")]
        public int? ProductType { get; set; }
        [FromQuery(Name = "createdDate")]
        public DateTime? CreatedDate { get; set; }
    }
}
