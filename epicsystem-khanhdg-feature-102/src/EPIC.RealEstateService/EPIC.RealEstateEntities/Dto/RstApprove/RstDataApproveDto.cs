using EPIC.Entities.Dto.User;
using EPIC.EntitiesBase.Dto.ModuleApprove;
using EPIC.RealEstateEntities.Dto.RstDistribution;
using EPIC.RealEstateEntities.Dto.RstOpenSell;
using EPIC.RealEstateEntities.Dto.RstOpenSellDetail;
using EPIC.RealEstateEntities.Dto.RstProject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.RealEstateEntities.Dto.RstApprove
{
    public class RstDataApproveDto : ModuleDataApproveBaseDto
    {
        public UserDto UserRequest { get; set; }
        public UserDto UserApprove { get; set; }

        /// <summary>
        /// Thông tin dự án
        /// </summary>
        public RstProjectDto Project { get; set; }

        /// <summary>
        /// Thông tin phân phối sản phẩm
        /// </summary>
        public RstDistributionDto Distribution { get; set; }

        /// <summary>
        /// Thông tin mở bán
        /// </summary>
        public RstOpenSellDto OpenSell { get; set; }
    }
}
