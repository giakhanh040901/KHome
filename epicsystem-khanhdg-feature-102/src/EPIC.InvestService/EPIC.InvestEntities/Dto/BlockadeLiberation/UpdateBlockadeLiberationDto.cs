using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.InvestEntities.Dto.BlockadeLiberation
{
    public class UpdateBlockadeLiberationDto : CreateBlockadeLiberationDto
    {
        public int? Id { get; set; }
        public string LiberationDescription { get; set; }
        public DateTime? LiberationDate { get; set; }
    }
}
