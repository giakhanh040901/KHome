using DocumentFormat.OpenXml.Wordprocessing;
using EPIC.EntitiesBase.Dto;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.InvestEntities.Dto.Withdrawal
{
    public class InvestWithdrawalFilterDto : PagingRequestBaseDto
    {
        [FromQuery(Name = "status")]
        public int? Status { get; set; }

        [FromQuery(Name = "source")]
        public int? Source { get; set; }

        [FromQuery(Name = "requestDate")]
        public DateTime? RequestDate { get; set; }

        [FromQuery(Name = "approveDate")]
        public DateTime? ApproveDate { get; set; }

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

        [FromQuery(Name = "distributionId")]
        public int? DistributionId { get; set; }

        private string _cifCode;
        [FromQuery(Name = "cifCode")]
        public string CifCode
        {
            get => _cifCode;
            set => _cifCode = value?.Trim();
        }
        [FromQuery(Name = "tradingProviderId")]
        public int? TradingProviderId { get; set; }
        /// <summary>
        /// List id đại lý
        /// </summary>
        [FromQuery(Name = "tradingProviderIds")]
        public List<int> TradingProviderIds { get; set; }

        [FromQuery(Name = "methodInterest")]
        public int? MethodInterest { get; set; }
    }
}
