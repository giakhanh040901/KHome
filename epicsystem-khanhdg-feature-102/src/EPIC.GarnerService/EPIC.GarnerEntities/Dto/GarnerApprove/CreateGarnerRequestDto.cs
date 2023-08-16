using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.GarnerEntities.Dto.GarnerApprove
{
    public class CreateGarnerRequestDto
    {
        public int Id { get; set; }
        public int? UserApproveId { get; set; }
        public string RequestNote { get; set; }
        public string Summary { get; set; }
    }
}
