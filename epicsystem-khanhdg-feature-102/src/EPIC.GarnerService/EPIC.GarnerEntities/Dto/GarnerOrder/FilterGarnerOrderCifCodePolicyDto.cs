using EPIC.EntitiesBase.Dto;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.GarnerEntities.Dto.GarnerOrder
{
    public class FilterGarnerOrderCifCodePolicyDto : PagingRequestBaseDto
    {
        private string _name;
        [FromQuery(Name = "name")]
        public string Name
        {
            get => _name;
            set => _name = value?.Trim();
        }

        private string _code;
        [FromQuery(Name = "code")]
        public string Code
        {
            get => _code;
            set => _code = value?.Trim();
        }

        [FromQuery(Name = "status")]
        public int? Status { get; set; }
        [FromQuery(Name = "source")]
        public int? Source { get; set; }
        [FromQuery(Name = "saleOrderId")]
        public int? SaleOrderId { get; set; }

        [FromQuery(Name = "buyDate")]
        public DateTime? BuyDate { get; set; }

        [FromQuery(Name = "policyId")]
        public int PolicyId{ get; set; }

        private string _cifCode;
        [FromQuery(Name = "cifCode")]
        public string CifCode
        {
            get => _cifCode;
            set => _cifCode = value?.Trim();
        }
    }
}
