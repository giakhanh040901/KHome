using EPIC.RealEstateEntities.Dto.RstProductItem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.RealEstateEntities.Dto.RstOpenSellDetail
{
    public class RstProductItemByOpenSellDetailDto : RstProductItemDto
    {
        /// <summary>
        /// Id của sản phẩm mở bán
        /// </summary>
        public int OpenSellId { get; set; }
    }
}
