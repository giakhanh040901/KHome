using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.EventEntites.Dto.EvtOrder
{
    public class AppSaleCreateEvtOrderDto : AppCreateEvtOrderDto
    {
        /// <summary>
        /// Id khách hàng
        /// </summary>
        public int InvestorId { get; set; }
    }
}
