using EPIC.Utils;
using EPIC.Utils.Validation;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.CoreEntities.Dto.SaleInvestor
{
    public class GetInvestorBySaleDto
    {
        [FromQuery(Name = "status")]
        [StringRange(AllowableValues = new string[] { null, UserStatus.ACTIVE, UserStatus.DEACTIVE })]
        public string Status { get; set; }

        [FromQuery(Name = "tradingProviderId")]
        public int TradingProviderId { get; set; }

        [FromQuery(Name = "startDate")]
        public DateTime? StartDate { get; set; }

        [FromQuery(Name = "endDate")]
        public DateTime? EndDate { get; set; }

        [FromQuery(Name = "keyword")]
        public string Keyword { get; set; }
    }
}
