using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Entities.Dto.Sale
{
    public class ApproveSaleDto
    {
        public int SaleTempId { get; set; }
        public int ApproveID { get; set; }
        public string ApproveNote { get; set; }
    }
}
