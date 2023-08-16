using EPIC.RealEstateEntities.Dto.RstHistoryUpdate;
using EPIC.RealEstateEntities.Dto.RstProductItem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.RealEstateEntities.Dto.RstDistributionProductItem
{
    /// <summary>
    /// Khóa sản phẩm của phân phối
    /// </summary>
    public class LockRstDistributionProductItemDto : RstUpdateStatusLockDtoBase
    {
        /// <summary>
        /// Id sản phẩm của phân phối
        /// </summary>
        public int Id { get; set; }
    }
}
