using EPIC.Utils.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.RealEstateEntities.Dto.RstOpenSell
{
    public class UpdateRstOpenSellDto : CreateRstOpenSellDto
    {
        public int Id { get; set; }

        /// <summary>
        /// Chức năng đăng ký làm cộng tác viên bán hàng.
        /// Khi bật lên thì App sẽ hiện chức năng đăng ký làm CTV bán hàng
        /// </summary>
        public bool IsRegisterSale { get; set; }
    }
}
