using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.RealEstateEntities.Dto.RstExportExcel
{
    public class RstSyntheticTradingDto
    {
        /// <summary>
        /// Ngày giao dịch hợp đồng
        /// </summary>
        public DateTime? CreatedDate { get; set; }
        /// <summary>
        /// Mã hợp đồng
        /// </summary>
        public string ContractCode { get; set; }
        /// <summary>
        /// Mã dự án
        /// </summary>
        public string ProjectCode { get; set; }
        /// <summary>
        /// Loại mật độ xây dựng
        /// </summary>
        public int? BuildingDensityType { get; set; }
        /// <summary>
        /// tên phân khu/ tòa
        /// </summary>
        public string BuildingDensityName { get; set; }
        /// <summary>
        /// Mã khách hàng là cá nhân hoặc doanh nghiệp
        /// </summary>
        public string CifCode { get; set; }
        /// <summary>
        /// Mã giới thiệu của sale
        /// </summary>
        public string SaleReferralCode { get; set; }
        /// <summary>
        /// Loại hình giao dịch
        /// </summary>
        public int TranClassify { get; set; }
        /// <summary>
        /// Loại hình thanh toán
        /// </summary>
        public int PaymentType { get; set; }
        /// <summary>
        /// Loại căn hộ
        /// </summary>
        public int ClassifyType { get; set; }
        /// <summary>
        /// Tình trạng căn hộ
        /// </summary>
        public int ProductItemStatus { get; set; }
        /// <summary>
        /// Diện tích thông thủy
        /// </summary>
        public decimal CarpetArea { get; set; }
        /// <summary>
        /// Diện tích tim tường
        /// </summary>
        public decimal BuiltUpArea { get; set; }
        /// <summary>
        /// Giá bán căn hộ
        /// </summary>
        public decimal? Price { get; set; }
        /// <summary>
        /// TradingProvider của order
        /// </summary>
        public int TradingProviderId { get; set; }
        public decimal PaymentAmount { get; set; }
    }
}
