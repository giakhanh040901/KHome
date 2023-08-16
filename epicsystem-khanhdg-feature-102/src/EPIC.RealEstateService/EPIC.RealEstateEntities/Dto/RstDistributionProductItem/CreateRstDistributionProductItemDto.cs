using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.RealEstateEntities.Dto.RstDistributionProductItem
{
    /// <summary>
    /// Thêm danh sách các căn cho phân phối
    /// </summary>
    public class CreateRstDistributionProductItemDto
    {
        public int DistributionId { get; set; }

        /// <summary>
        /// Id căn từ dự án
        /// </summary>
        public List<int> ProductItemIds { get; set; }
    }
}
