using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.RealEstateEntities.Dto.RstDashboard
{
    public class RstListProjectDashboardDto
    {
        /// <summary>
        /// tên dự án
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// id dự án
        /// </summary>
        public int ProjectId { get; set; }
    }
}
