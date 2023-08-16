using EPIC.Entities.Dto.SaleAppStatistical;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Entities.Dto.Sale
{
    public class AppStatisticContractBySaleDto
    {
        public List<HopDongSaleAppDto> ContractOrders { get; set; }
        public List<AppStatisticChartContractBySaleDto> ChartContractOrders { get; set; }
    }
}
