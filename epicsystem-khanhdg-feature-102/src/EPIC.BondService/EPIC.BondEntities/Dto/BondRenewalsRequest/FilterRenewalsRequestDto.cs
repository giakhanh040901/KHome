using EPIC.Entities.Dto.CoreApprove;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace EPIC.BondEntities.Dto.RenewalsRequest
{
    public class FilterRenewalsRequestDto : GetApproveListDto
    {
        [FromQuery(Name = "settlementMethod")]
        public int? SettlementMethod { get; set; }
    }
}
