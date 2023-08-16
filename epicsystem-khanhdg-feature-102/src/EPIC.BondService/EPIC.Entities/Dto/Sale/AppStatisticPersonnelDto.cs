using EPIC.Entities.Dto.SaleAppStatistical;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Entities.Dto.Sale
{
    public class AppStatisticPersonnelDto
    {
        public List<SaleInfoAppDto> Personnels { get; set; }
        public List<AppStatisticChartPersonnelDto> StatisticChartPersonnels { get; set;}
    }
}
