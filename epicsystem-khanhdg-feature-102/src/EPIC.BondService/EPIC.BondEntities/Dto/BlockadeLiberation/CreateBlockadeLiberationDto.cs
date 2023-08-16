using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Entities.Dto.BlockadeLiberation
{
    public class CreateBlockadeLiberationDto
    {
        public int Type { get; set; }
        public string BlockadeDescription { get; set; }
        public DateTime? BlockadeDate { get; set; }
        public int OrderId { get; set; }
    }
}
