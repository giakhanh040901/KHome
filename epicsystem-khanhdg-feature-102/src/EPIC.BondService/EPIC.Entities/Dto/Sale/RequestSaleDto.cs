using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Entities.Dto.Sale
{
    public class RequestSaleDto
    {
        public int SaleTempId { get; set; }
        public int ActionType { get; set; }
        public string RequestNote { get; set; }
        public int? UserApproveId { get; set; }
        public string Summary { get; set; }
    }
}
