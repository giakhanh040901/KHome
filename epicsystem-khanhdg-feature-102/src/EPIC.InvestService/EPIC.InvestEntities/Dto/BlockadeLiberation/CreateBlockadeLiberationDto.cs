using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.InvestEntities.Dto.BlockadeLiberation
{
    public class CreateBlockadeLiberationDto
    {
        public int? Type { get; set; }
        public string BlockadeDescription { get; set; }
        public DateTime? BlockadeDate { get; set; }
        public long OrderId { get; set; }
    }
}
