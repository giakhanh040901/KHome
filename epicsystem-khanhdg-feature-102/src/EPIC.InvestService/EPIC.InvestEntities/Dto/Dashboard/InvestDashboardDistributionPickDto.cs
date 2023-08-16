using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.InvestEntities.Dto.Dashboard
{
    /// <summary>
    /// Danh sách phân phối sản phẩm theo đại lý
    /// </summary>
    public class InvestDashboardDistributionPickDto
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
    }
}
