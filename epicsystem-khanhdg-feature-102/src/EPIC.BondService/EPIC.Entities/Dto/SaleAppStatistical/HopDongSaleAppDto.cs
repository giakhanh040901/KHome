using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Entities.Dto.SaleAppStatistical
{
    public class HopDongSaleAppDto
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
        public int OrderStatus { get; set; } //trạng thái của hợp đồng
        public DateTime? InvestDate { get; set; } //ngày đầu tư
        public Decimal? TotalValue { get; set; } //giá trị đầu tư
        public Decimal? InitTotalValue { get; set; } //giá trị đầu tư ban đầu
        public DateTime? BlockadeDate { get; set; }  //ngày phong toả
        public DateTime? BuyDate { get; set; } //ngày đặt lệnh
        public DateTime? LiberationDate { get; set; } //ngày giải toả
        public DateTime? SettlementDate { get; set; } //Ngày tất toán
        public DateTime? PaymentFullDate { get; set; } // Ngày thanh toán đủ
        public DateTime? DueDate { get; set; } // Ngày đáo hạn

        /// <summary>
        /// Real Estate Ngày đặt cọc
        /// </summary>
        public DateTime? DepositDate { get; set; }

        /// <summary>
        /// Real Estate Giá của căn hộ
        /// </summary>
        public decimal ProductItemPrice { get; set; }

        /// <summary>
        /// Mã hợp đồngs
        /// </summary>
        public string ContractCode { get; set; }

        /// <summary>
        /// Loại hình dự án: 1 Bond: 2 Invest, 3, CompanyShares, 4: Garner, 5: Real Estate
        /// </summary>
        public int ProjectType { get; set; }
    }
}
