using EPIC.EntitiesBase.Dto;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.InvestEntities.Dto.Order
{
    public class FilterRenewalsRequestDto : PagingRequestBaseDto
    {

        [FromQuery(Name = "status")]
        public int? Status { get; set; }
        [FromQuery(Name = "source")]
        public int? Source { get; set; }

        [FromQuery(Name = "orderSource")]
        public int? OrderSource { get; set; }

        private string _phone;
        [FromQuery(Name = "phone")]
        public string Phone
        {
            get => _phone;
            set => _phone = value?.Trim();
        }

        private string _contractCode;
        [FromQuery(Name = "contractCode")]
        public string ContractCode
        {
            get => _contractCode;
            set => _contractCode = value?.Trim();
        }

        private string _cifCode;
        [FromQuery(Name = "cifCode")]
        public string CifCode
        {
            get => _cifCode;
            set => _cifCode = value?.Trim();
        }
        [FromQuery(Name = "orderer")]
        public int? Orderer { get; set; }
        [FromQuery(Name = "distributionId")]
        public int? DistributionId { get; set; }
        [FromQuery(Name = "policyDetailId")]
        public int? PolicyDetailId { get; set; }

        [FromQuery(Name = "policy")]
        public string Policy { get; set; }

        [FromQuery(Name = "policyId")]
        public int? PolicyId { get; set; }

        [FromQuery(Name = "contractCodeGen")]
        public string ContractCodeGen { get; set; }
        /// <summary>
        /// List id đại lý
        /// </summary>
        [FromQuery(Name = "tradingProviderIds")]
        public List<int> TradingProviderIds { get; set; }

        [FromQuery(Name = "settlementMethod")]
        public int? SettlementMethod { get; set; }
    }
}
