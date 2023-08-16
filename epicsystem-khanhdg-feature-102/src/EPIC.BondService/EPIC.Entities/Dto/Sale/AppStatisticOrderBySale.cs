using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Entities.Dto.Sale
{
    public class AppStatisticOrderBySale
    {
        /// <summary>
        /// Id của hợp đồng
        /// </summary>
        public long OrderId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string CifCode { get; set; }

        /// <summary>
        /// Trạng thái hợp đồng
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// Ngày đặt cọc RealEstate
        /// </summary>
        public DateTime? DepositDate { get; set; }

        public decimal TotalValue { get; set; }

        public decimal InitTotalValue { get; set; }
        /// <summary>
        /// Ngày đầu tư Invest/ Garner
        /// </summary>
        public DateTime? InvestDate { get; set; }

        /// <summary>
        /// Giá của căn hộ
        /// </summary>
        public decimal ProductItemPrice { get; set; }

        /// <summary>
        /// Ngày tạo
        /// </summary>
        public DateTime? CreatedDate { get; set; }

        /// <summary>
        /// Loại hình dự án: 1 Bond: 2 Invest, 3, CompanyShares, 4: Garner, 5: Real Estate
        /// </summary>
        public int ProjectType { get; set; }
    }
}
