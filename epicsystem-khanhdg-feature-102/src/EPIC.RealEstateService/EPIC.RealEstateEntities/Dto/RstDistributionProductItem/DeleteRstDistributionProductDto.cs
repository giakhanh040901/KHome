using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.RealEstateEntities.Dto.RstDistributionProductItem
{
    public class DeleteRstDistributionProductDto
    {
        /// <summary>
        /// Id phân phối
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Danh sách sản phẩm của phân phối cần xóa
        /// </summary>
        public List<int> DistributionItemIds { get; set; }
    }
}
