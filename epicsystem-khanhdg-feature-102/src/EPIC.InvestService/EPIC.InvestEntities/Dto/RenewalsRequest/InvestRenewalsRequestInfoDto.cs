using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.InvestEntities.Dto.RenewalsRequest
{
    public class InvestRenewalsRequestInfoDto
    {
        public int? PolicyId { get; set; }
        public int? PolicyDetailId { get; set; }
        public string IsRenewal { get; set; }
    }
}
