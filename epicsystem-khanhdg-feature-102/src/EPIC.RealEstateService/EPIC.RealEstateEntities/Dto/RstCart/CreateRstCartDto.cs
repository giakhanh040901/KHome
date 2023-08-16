using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.RealEstateEntities.Dto.RstCart
{
    public class CreateRstCartDto
    {
        /// <summary>
        /// Id sản phẩm của mở bán
        /// </summary>
        public List<int> OpenSellDetailId { get; set; }
    }
}
