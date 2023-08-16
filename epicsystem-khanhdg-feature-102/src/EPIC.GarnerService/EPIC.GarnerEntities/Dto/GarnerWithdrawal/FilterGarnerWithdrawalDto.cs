using DocumentFormat.OpenXml.Wordprocessing;
using EPIC.EntitiesBase.Dto;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.GarnerEntities.Dto.GarnerWithdrawal
{
    public class FilterGarnerWithdrawalDto : PagingRequestBaseDto
    {
        [FromQuery(Name = "status")]
        public List<int> Status { get; set; }

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

        /// <summary>
        /// Ngày yêu cầu rút tiền
        /// </summary>
        [FromQuery(Name = "withdrawalDate")]
        public DateTime? WithdrawalDate { get; set; }

        /// <summary>
        /// Ngày duyệt
        /// </summary>
        [FromQuery(Name = "approveDate")]
        public DateTime? ApproveDate { get; set; }
        [FromQuery(Name = "tradingProviderIds")]
        public List<int> TradingProviderIds { get; set; }
    }
}
