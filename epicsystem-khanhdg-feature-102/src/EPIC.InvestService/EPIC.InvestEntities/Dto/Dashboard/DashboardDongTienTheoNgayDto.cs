using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.InvestEntities.Dto.Dashboard
{
    public class DashboardDongTienTheoNgayDto
    {
        public DateTime? Date { get; set; }
        public decimal? TotalValue { get; set; }
        public decimal? TotalValueOut { get; set; }
    }
}
