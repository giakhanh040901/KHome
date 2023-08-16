using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.RealEstateEntities.Dto.RstExportExcel
{
    public class RstListSyntheticTradingDto
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
        /// Tên khách hàng
        /// </summary>
        public string Fullname { get; set; }
        /// <summary>
        /// Số giấy tờ
        /// </summary>
        public string IdNo { get; set; }
        /// <summary>
        /// Loại giấy tờ
        /// </summary>
        public string IdType { get; set; }
        /// <summary>
        /// Địa chỉ thường trú
        /// </summary>
        public string PlaceOfResidence { get; set; }
        /// <summary>
        /// Mã dự án
        /// </summary>
        public string ProjectCode { get; set; }
        /// <summary>
        /// Loại mật độ xây dựng
        /// </summary>
        public string BuildingDensityType { get; set; }
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
        public string TranClassify { get; set; }
        /// <summary>
        /// Loại hình thanh toán
        /// </summary>
        public string PaymentType { get; set; }
        /// <summary>
        /// Loại căn hộ
        /// </summary>
        public string ClassifyType { get; set; }
        /// <summary>
        /// Tình trạng căn hộ
        /// </summary>
        public string ProductItemStatus { get; set; }
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
        /// <summary>
        /// tên saler
        /// </summary>
        public string SalerName { get; set; }
        /// <summary>
        /// tên phòng ban
        /// </summary>
        public string SaleDepartmentName { get; set; }
        /// <summary>
        /// số tiền
        /// </summary>
        public decimal PaymentAmount { get; set; }
        /// <summary>
        /// % Khách hàng thanh toán
        /// </summary>
        public decimal? PayingCustomer { get; set; }
    }
}
