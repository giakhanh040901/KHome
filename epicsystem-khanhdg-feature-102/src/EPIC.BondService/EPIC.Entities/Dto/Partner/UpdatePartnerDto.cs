using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Entities.Dto.Partner
{
    public class UpdatePartnerDto : CreatePartnerDto
    {
        public int PartnerId { get; set; }
    }
}
