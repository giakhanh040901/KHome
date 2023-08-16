using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Entities.Dto.ManagerInvestor
{
    public class FindByIdDto
    {
        [FromQuery(Name = "isNeedReferralInvestor")]
        public bool IsNeedReferralInvestor { get; set; }
    }
}
