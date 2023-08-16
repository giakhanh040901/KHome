using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Entities.Dto.Sale
{
    public class CancelSaleDto
    {
        public int SaleTempId { get; set; }
        public string CancelNote { get; set; }
    }
}
