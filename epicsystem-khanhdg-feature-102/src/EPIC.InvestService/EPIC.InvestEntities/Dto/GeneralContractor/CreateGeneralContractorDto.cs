using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.InvestEntities.Dto.GeneralContractor
{
    public class CreateGeneralContractorDto
    {
        public int BusinessCustomerId { get; set; }
        public int PartnerId { get; set; }
    }
}
