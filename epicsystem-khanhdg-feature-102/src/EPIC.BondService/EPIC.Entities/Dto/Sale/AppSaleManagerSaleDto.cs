using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Entities.Dto.Sale
{
    public class AppSaleManagerSaleDto
    {
        public int SaleId { get; set; }
        public string FullName { get; set; }
        public string Status { get; set; }
        public string AvatarImageUrl { get; set; }
        /// <summary>
        /// Số dư
        /// </summary>
        public decimal Balance { get; set; }
        /// <summary>
        /// Doanh thu
        /// </summary>
        public decimal Revenue { get; set; }
    }
}
