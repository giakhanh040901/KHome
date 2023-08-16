using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.InvestEntities.Dto.Order
{
    public class SettlementMethodDto
    {
        /// <summary>
        /// Phương thức tất toán cuối kỳ
        /// </summary>
        [Range(1, int.MaxValue)]
        public int? SettlementMethod { get; set; }

        /// <summary>
        /// Loại kỳ hạn sau khi tái tục
        /// </summary>
        [Range(1, int.MaxValue)]
        public int? RenewalsPolicyDetailId { get; set; }
    }
}
