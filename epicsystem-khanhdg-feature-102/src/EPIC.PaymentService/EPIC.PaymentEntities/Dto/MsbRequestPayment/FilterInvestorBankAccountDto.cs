using EPIC.EntitiesBase.Dto;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.PaymentEntities.Dto.MsbRequestPayment
{
    public class FilterInvestorBankAccountDto : PagingRequestBaseDto
    {
        /// <summary>
        /// List id đại lý
        /// </summary>
        [FromQuery(Name = "investorIds")]
        public List<int> InvestorIds { get; set; }
    }
}
