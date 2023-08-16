using EPIC.RealEstateEntities.Dto.RstProductItemExtend;
using EPIC.RealEstateEntities.Dto.RstProjectExtend;
using EPIC.RealEstateEntities.Dto.RstProjectInformationShare;
using EPIC.RealEstateEntities.Dto.RstProjectUtilityMedia;
using EPIC.Utils.Attributes;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.RealEstateEntities.Dto.RstProject
{
    public class AppOverviewProjectDto
    {
        /// <summary>
        /// Id dự án
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Id chủ đầu tư
        /// </summary>
        public int? OwnerId { get; set; }
        /// <summary>
        /// Tên chủ đầu tư
        /// </summary>
        public string OwnerName { get; set; }
        /// <summary>
        /// Phân phối
        /// </summary>
        public string TradingProviderName { get; set; }

        /// <summary>
        /// Mã giới thiệu của đại lý
        /// </summary>
        public string TradingReferralCode { get; set; }

        /// <summary>
        /// Id đlsc
        /// </summary>
        public int? TradingProviderId { get; set; }

        /// <summary>
        /// Tên tổng thầu xây dựng
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
        /// Ngân hàng đảm bảo
        /// </summary>
        public string GuaranteeBankName { get; set; }
        /// <summary>
        /// Sản phẩm dự án: 1: Chung cư, 2 Biệt thự, 3: Liền kề, 4 Khách sạn...
        /// </summary>
        public List<int> ProjectType { get; set; }
        /// <summary>
        /// Tổng mức đầu tư
        /// </summary>
        public decimal? TotalInvestment { get; set; }
        /// <summary>
        /// Tổng diện tích - Diện tích đất
        /// </summary>
        public string LandArea { get; set; }
        /// <summary>
        /// % Mật độ xây dựng
        /// </summary>
        public decimal? BuildingDensity { get; set; }

        /// <summary>
        /// Giá bán tối thiểu
        /// </summary>
        public decimal? MinSellingPrice { get; set; }

        /// <summary>
        /// Giá bán tối đa
        /// </summary>
        public decimal? MaxSellingPrice { get; set; }

        /// <summary>
        /// Giá bán dự kiến (đơn giá)
        /// </summary>
        public decimal? ExpectedSellingPrice { get; set; }

        /// <summary>
        /// Ngày khởi công
        /// </summary>
        public DateTime? StartDate { get; set; }
        /// <summary>
        /// Dự kiến bàn giao - Thời gian dự kiến hoàn thành
        /// </summary>
        public string ExpectedHandoverTime { get; set; }
        /// <summary>
        /// Kinh độ (Vị trí)
        /// </summary>
        public string Latitude { get; set; }

        /// <summary>
        /// Vĩ độ (Vị trí)
        /// </summary>
        public string Longitude { get; set; }
        /// <summary>
        /// Địa chỉ
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Loại nội dung tổng quan: MARKDOWN, HTML
        /// </summary>
        public string ContentType { get; set; }

        /// <summary>
        /// Mô tả - Nội dung tổng quan
        /// </summary>
        public string OverviewContent { get; set; }
        /// <summary>
        /// Tiện ích nổi bật
        /// </summary>
        public List<AppViewUtilityDto> UtilityHightLight { get; set; }

        /// <summary>
        /// Danh sách Tiện ích
        /// </summary>
        public List<AppViewUtilityDto> Utilities { get; set; }

        /// <summary>
        /// Danh sách hình ảnh của tiện ích
        /// </summary>
        public List<AppRstProjectUtilityMediaDto> UtilityMedia { get; set; }

        /// <summary>
        /// Website của dự án
        /// </summary>
        public string Website { get; set; }
        /// <summary>
        /// Hotline dự án
        /// </summary>
        public string ProjectHotline { get; set; }

        /// <summary>
        /// Hotline liên hệ đến đại lý
        /// </summary>
        public string Hotline { get; set; }

        /// <summary>
        /// Website của đại lý
        /// </summary>
        public string WebsiteTradingProvider { get; set; }

        /// <summary>
        /// Đường dẫn facebook của dự án
        /// </summary>
        public string FacebookLink { get; set; }

        /// <summary>
        /// Fanpage của đại lý
        /// </summary>
        public string Fanpage { get; set; }

        /// <summary>
        /// Chức năng đăng ký làm cộng tác viên bán hàng.
        /// Khi bật lên: true thì App sẽ hiện chức năng đăng ký làm CTV bán hàng
        /// </summary>
        public bool IsRegisterSale { get; set; }

        /// <summary>
        /// Danh sách ngân hàng đảm bảo
        /// </summary>
        public List<RstProjectGuaranteeBankDto> GuaranteeBanks { get; set; }

        /// <summary>
        /// Thông tin khác của dự án
        /// </summary>
        public List<AppRstProjectExtendDto> ProjectExtends { get; set; }

        /// <summary>
        /// Danh sách thông tin chia sẻ dự án
        /// </summary>
        public List<AppRstProjectInformationShareDto> ProjectInformationShare { get; set; }
    }
}
