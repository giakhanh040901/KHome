using EPIC.EntitiesBase.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.GarnerEntities.Dto.GarnerOrderPayment
{
    public class FilterOrderPaymentDto : PagingRequestBaseDto
    {
        public int Status { get; set; }
        [System.Text.Json.Serialization.JsonIgnore]
        public long OrderId { get; set; }   
    }
}
