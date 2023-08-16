using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.BondEntities.Dto.BondDashboard
{
    public class DashboardDanhSachTheoKyHanSP
    {
        public int InterestType { get; set; }
        public string PeriodType { get; set; }
        public int PeriodQuantity { get; set; }
        public decimal TotalValue { get; set; }
    }
}
