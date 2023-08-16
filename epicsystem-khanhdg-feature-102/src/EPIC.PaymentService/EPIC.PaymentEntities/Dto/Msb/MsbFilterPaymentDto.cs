using EPIC.EntitiesBase.Dto;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.PaymentEntities.Dto.Msb
{
    public class MsbFilterPaymentDto : PagingRequestBaseDto
    {
        [FromQuery(Name = "startDate")]
        public DateTime? StartDate { get; set; }

        [FromQuery(Name = "endDate")]
        public DateTime? EndDate { get; set; }

        private string _prefixMsb;
        [FromQuery(Name = "prefixMsb")]
        public string PrefixMsb
        {
            get => _prefixMsb;
            set => _prefixMsb = value?.Trim();
        }
    }
}
