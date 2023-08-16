using EPIC.Utils;
using EPIC.Utils.Validation;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Entities.Dto.Investor
{
    public class AppGetListIdentificationDto
    {
        [StringRange(AllowableValues = new string[] {null, InvestorIdentificationStatus.TEMP, InvestorIdentificationStatus.ACTIVE})]
        [FromQuery(Name = "status")]
        public string Status { get; set; }
    }
}
