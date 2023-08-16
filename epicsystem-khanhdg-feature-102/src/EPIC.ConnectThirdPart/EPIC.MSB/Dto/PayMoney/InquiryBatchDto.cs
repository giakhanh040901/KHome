using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.MSB.Dto.PayMoney
{
    public class InquiryBatchDto
    {
        public long RequestId { get; set; }
        public string TId { get; set; }
        public string MId { get; set; }
        public string AccessCode { get; set; }
    }
}
