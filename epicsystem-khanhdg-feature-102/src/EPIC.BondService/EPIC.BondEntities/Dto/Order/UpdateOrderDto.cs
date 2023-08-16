using EPIC.Utils;
using EPIC.Utils.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Entities.Dto.Order
{
    public class UpdateOrderDto : BaseOrderDto
    {
        public int OrderId { get; set; }
    }
}
