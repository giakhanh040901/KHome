using EPIC.RealEstateEntities.Dto.RstHistoryUpdate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.RealEstateEntities.Dto.RstOpenSellDetail
{
    public class LockRstOpenSellDetailDto : RstUpdateStatusLockDtoBase
    {
        /// <summary>
        /// Id của sản phẩm mở bán
        /// </summary>
        public int Id { get; set; }
    }
}
