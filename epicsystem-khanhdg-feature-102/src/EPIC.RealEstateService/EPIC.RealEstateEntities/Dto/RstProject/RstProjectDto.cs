using EPIC.RealEstateEntities.Dto.RstOwner;
using EPIC.RealEstateEntities.Dto.RstProjectExtend;
using EPIC.Utils.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.RealEstateEntities.Dto.RstProject
{
    /// <summary>
    /// Thông tin dự án
    /// </summary>
    public class RstProjectDto
    {
        /// <summary>
        /// Id dự án
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Chủ đầu tư
        /// </summary>
        public int OwnerId { get; set; }

        /// <summary>
        /// Mã dự án
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Tên dự án
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Trang thai (1: Khoi tao, 2: Cho duyet, 3: Hoat dong, 4: Huy duyet, 5:Dong)
        /// </summary>
        public int Status { get; set; }

        #region thông tin chung

        /// <summary>
        /// Tên tổng thầu
        /// </summary>
        public string ContractorName { get; set; }

        /// <summary>
        /// Liên kết giới thiệu Tổng thầu xây dựng
        /// </summary>
        public string ContractorLink { get; set; }

        /// <summary>
        /// Mô tả thông tin Tổng thầu xây dựng
        /// </summary>
        public string ContractorDescription { get; set; }

        /// <summary>
        /// Tên đơn vị vận hành
        /// </summary>
        public string OperatingUnit { get; set; }

        /// <summary>
        /// Liên kết giới thiệu đơn vị vận hành
        /// </summary>
        public string OperatingUnitLink { get; set; }

        /// <summary>
        /// Mô tả thông tin Đơn vị vận hành
        /// </summary>
        public string OperatingUnitDescription { get; set; }

        /// <summary>
        /// Liên kết website đến dự án  
        /// </summary>
        public string Website { get; set; }

        /// <summary>
        /// Điện thoại Hotline liên hệ dự án
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// Đường dẫn facebook
        /// </summary>
        public string FacebookLink { get; set; }
        /// <summary>
        /// Loại hình dự án: 1: Đất đấu giá, 2: Đất BT, 3: Đất giao
        /// </summary>
        public int? ProjectType { get; set; }

        /// <summary>
        /// (Sản phẩm dự án) Loại hình sản phẩm của dự án: 1: Chung cư, 2 Biệt thự...
        /// </summary>
        public List<int> ProductTypes { get; set; }

        /// <summary>
        /// Loại hình phân phối: 1: Chủ đầu tư phân phối, 2: Đại lý phân phối
        /// </summary>
        public List<int> DistributionTypes { get; set; }

        /// <summary>
        /// Ngân hàng đảm bảo
        /// </summary>
        public List<int> GuaranteeBanks { get; set; }
        #endregion

        #region Thông số dự án

        /// <summary>
        /// Trạng thái tiến độ dự án: 1: Đang xây dựng, 2: Đang bán, 3: Đã hết hàng, 4: Tạm dừng bán, 5: Sắp mở bán
        /// </summary>
        public int? ProjectStatus { get; set; }

        /// <summary>
        /// Diện tích đất
        /// </summary>
        public string LandArea { get; set; }

        /// <summary>
        /// Diện tích xây dựng
        /// </summary>
        public string ConstructionArea { get; set; }

        /// <summary>
        /// % Mật độ xây dựng
        /// </summary>
        public decimal? BuildingDensity { get; set; }

        /// <summary>
        /// Thửa đất số ...
        /// </summary>
        public string LandPlotNo { get; set; }

        /// <summary>
        /// Tờ bản đồ số ...
        /// </summary>
        public string MapSheetNo { get; set; }

        /// <summary>
        /// Ngày khởi công
        /// </summary>
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// Thời gian dự kiến hoàn thành
        /// </summary>
        public string ExpectedHandoverTime { get; set; }

        /// <summary>
        /// Tổng mức đầu tư
        /// </summary>
        public decimal? TotalInvestment { get; set; }

        /// <summary>
        /// Giá bán dự kiến
        /// </summary>
        public decimal? ExpectedSellingPrice { get; set; }

        /// <summary>
        /// Giá bán tối thiểu
        /// </summary>
        public decimal? MinSellingPrice { get; set; }

        /// <summary>
        /// Giá bán tối đa
        /// </summary>
        public decimal? MaxSellingPrice { get; set; }

        /// <summary>
        /// Số căn
        /// </summary>
        public int? NumberOfUnit { get; set; }

        /// <summary>
        /// Mã thành phố
        /// </summary>
        public string ProvinceCode { get; set; }

        /// <summary>
        /// Địa chỉ dự án
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Kinh độ
        /// </summary>
        public string Latitude { get; set; }

        /// <summary>
        /// Vĩ độ
        /// </summary>
        public string Longitude { get; set; }
        #endregion

        #region Mô tả thông tin dự án
        /// <summary>
        /// Loại nội dung tổng quan: MARKDOWN, HTML
        /// </summary>
        public string ContentType { get; set; }

        /// <summary>
        /// Nội dung tổng quan
        /// </summary>
        public string OverviewContent { get; set; }
        #endregion

        /// <summary>
        /// Ngày tạo
        /// </summary>
        public DateTime? CreatedDate { get; set; }

        /// <summary>
        /// Ngày tạo
        /// </summary>
        public string CreatedBy { get; set; }

        /// <summary>
        /// Tên chủ đầu tư
        /// </summary>
        public string OwnerName { get; set; }
        /// <summary>
        /// Thông tin chủ đầu tư
        /// </summary>
        public ViewRstOwnerDto Owner { get; set; }

        /// <summary>
        /// Thông tin khác của dự án
        /// </summary>
        public List<RstProjectExtendDto> ProjectExtends { get; set; }
    }
}
