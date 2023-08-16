using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.GarnerEntities.Dto.GarnerProductOverview
{
    public class GarnerProductOverviewDto
    {
        /// <summary>
        /// Id của Distribution Phân phối sản phẩm
        /// </summary>
        public int Id { get; set; }
        public string ContentType { get; set; }
        public string OverviewContent { get; set; }
        public string OverviewImageUrl { get; set; }
        public List<GarnerProductOverviewFileDto> ProductOverviewFiles { get; set; }
        public List<GarnerProductOverviewOrgDto> ProductOverviewOrgs { get; set; }
    }
}
