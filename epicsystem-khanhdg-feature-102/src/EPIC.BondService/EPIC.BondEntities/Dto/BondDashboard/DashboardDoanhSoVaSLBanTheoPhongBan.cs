using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.BondEntities.Dto.BondDashboard
{
    public class DashboardDoanhSoVaSLBanTheoPhongBan
    {
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public decimal? CountSale { get; set; }
        public decimal? TotalValue { get; set; }
    }
}
