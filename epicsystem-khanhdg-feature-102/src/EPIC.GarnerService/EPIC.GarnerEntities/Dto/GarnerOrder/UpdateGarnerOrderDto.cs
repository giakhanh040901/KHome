using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.GarnerEntities.Dto.GarnerOrder
{
    public class UpdateGarnerOrderDto : CreateGarnerOrderDto
    {
        public long Id { get; set; }
    }
}
