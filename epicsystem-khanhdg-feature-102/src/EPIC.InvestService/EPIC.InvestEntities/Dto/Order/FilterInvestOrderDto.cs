using EPIC.EntitiesBase.Dto;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.InvestEntities.Dto.Order
{
    public class FilterInvestOrderDto : PagingRequestBaseDto
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

        [FromQuery(Name = "deliveryStatus")]
        public int? DeliveryStatus { get; set; }
        [FromQuery(Name = "status")]
        public int? Status { get; set; }
        [FromQuery(Name = "source")]
        public int? Source { get; set; }
        [FromQuery(Name = "saleOrderId")]
        public int? SaleOrderId { get; set; }

        [FromQuery(Name = "pendingDate")]
        public DateTime? PendingDate { get; set; }
        [FromQuery(Name = "deliveryDate")]
        public DateTime? DeliveryDate { get; set; }
        [FromQuery(Name = "receivedDate")]
        public DateTime? ReceivedDate { get; set; }
        [FromQuery(Name = "finishedDate")]
        public DateTime? FinishedDate { get; set; }
        [FromQuery(Name = "buyDate")]
        public DateTime? BuyDate { get; set; }

        private string _phone;
        [FromQuery(Name = "phone")]
        public string Phone
        {
            get => _phone;
            set => _phone = value?.Trim();
        }

        private string _idNo;
        [FromQuery(Name = "idNo")]
        public string IdNo
        {
            get => _idNo;
            set => _idNo = value?.Trim();
        }

        private string _taxCode;
        [FromQuery(Name = "taxCode")]
        public string TaxCode
        {
            get => _taxCode;
            set => _taxCode = value?.Trim();
        }

        [FromQuery(Name = "productType")]
        public int? ProductType { get; set; }

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
        [FromQuery(Name = "policyId")]
        public int? PolicyId { get; set; }
        [FromQuery(Name = "policyDetailId")]
        public int? PolicyDetailId { get; set; }
        public int? InvestHistoryStatus { get; set; }
        [FromQuery(Name = "settlementDate")]
        public DateTime? SettlementDate { get; set; }
        [FromQuery(Name = "projectId")]
        public int? ProjectId { get; set; }
        [FromQuery(Name = "methodInterest")]
        public int? MethodInterest { get; set; }
        [FromQuery(Name = "tradingProviderId")]
        public int? TradingProviderId { get; set; }

        /// <summary>
        /// List id đại lý
        /// </summary>
        [FromQuery(Name = "tradingProviderIds")]
        public List<int> TradingProviderIds { get; set; }
    }
}
