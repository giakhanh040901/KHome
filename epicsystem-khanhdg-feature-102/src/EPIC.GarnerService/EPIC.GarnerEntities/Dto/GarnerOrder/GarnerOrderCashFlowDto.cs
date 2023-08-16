using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.GarnerEntities.Dto.GarnerOrder
{
    /// <summary>
    /// Dòng tiền của hợp đồng
    /// </summary>
    public class GarnerOrderCashFlowDto
    {
        public List<GarnerOrderCashFlowExpectedDto> Expecteds { get; set; }
        public List<GarnerOrderCashFlowActualDto> Actuals { get; set; }
    }
}
