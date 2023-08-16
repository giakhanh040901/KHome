using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.EventEntites.Dto.EvtOrder
{
    public class SingalREvtOrderDto
    {
        public int Id { get; set; }
        /// <summary>
        /// Id khách hàng
        /// </summary>
        public int InvestorId { get; set; }
        public int Status { get; set; }
        public DateTime? ExpiredTime { get; set; }
    }
}
