using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.RealEstateEntities.Dto.RstOrder
{
    public class AppRstOrderForSaleDto
    {
        /// <summary>
        /// Id của hợp đồng
        /// </summary>
        public long OrderId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string CifCode { get; set; }
        public string CustomerName { get; set; } //Tên khách hàng
        public string AvatarImageUrl { get; set; } //Ảnh đại diện
        public string ReferralCode { get; set; } // Mã giới thiệu của khách hàng
        public int Status { get; set; } //Trạng thái hiển thị

        /// <summary>
        /// trạng thái của hợp đồng
        /// </summary>
        public int OrderStatus { get; set; } //

        /// <summary>
        /// Ngày đặt cọc
        /// </summary>
        public DateTime? DepositDate { get; set; }

        /// <summary>
        /// Giá của căn hộ
        /// </summary>
        public decimal ProductItemPrice { get; set; } 

        /// <summary>
        /// Ngày phong tỏa
        /// </summary>
        public DateTime? BlockadeDate { get; set; }

        /// <summary>
        /// Ngày mua // CreatedDate
        /// </summary>
        public DateTime? BuyDate { get; set; }
        /// <summary>
        /// Mã hợp đồng
        /// </summary>
        public string ContractCode { get; set; }

        /// <summary>
        /// Loại hình dự án: 1 Bond: 2 Invest, 3, CompanyShares, 4: Garner, 5: Real Estate
        /// </summary>
        public int ProjectType { get; set; }
    }
}
