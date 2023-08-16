using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.GarnerEntities.Dto.GarnerOrder
{
    public class AppCheckGarnerOrderDto : AppCreateGarnerOrderDto
    {
        /// <summary>
        /// Investor id của khách hàng được sale đặt lệnh hộ
        /// </summary>
        public int? InvestorId { get; set; }
    }
}
