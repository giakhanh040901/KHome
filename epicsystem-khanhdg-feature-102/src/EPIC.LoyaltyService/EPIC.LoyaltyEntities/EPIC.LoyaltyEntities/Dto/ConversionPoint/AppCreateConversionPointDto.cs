using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.LoyaltyEntities.Dto.ConversionPoint
{
    public class AppCreateConversionPointDto
    {
        /// <summary>
        /// Id của Voucher
        /// </summary>
        public int VoucherId { get; set; }

        /// <summary>
        /// Số lượng đổi
        /// </summary>
        public int Quantity { get; set; }
    }
}
