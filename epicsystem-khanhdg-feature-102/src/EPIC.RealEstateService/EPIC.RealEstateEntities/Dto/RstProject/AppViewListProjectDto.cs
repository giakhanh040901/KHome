using EPIC.Utils.Attributes;
using EPIC.Utils.ConstantVariables.RealEstate;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EPIC.RealEstateEntities.Dto.RstProjectMedia;

namespace EPIC.RealEstateEntities.Dto.RstProject
{
    public class AppViewListProjectDto
    {
        /// <summary>
        /// Id dự án
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Trang thai (1: Khoi tao, 2: Cho duyet, 3: Hoat dong, 4: Huy duyet, 5:Dong)
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// Mã dự án
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// Tên dự án
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Tên chủ đầu tư
        /// </summary>
        public string OwnerName { get;set; }

        /// <summary>
        /// Loại hình dự án: 1: Đất đấu giá, 2: Đất BT, 3: Đất giao<br/>
        /// <see cref="RstProjectTypes"/>
        /// </summary>
        public int? ProjectType { get; set; }

        /// <summary>
        /// Trạng thái tiến độ dự án: 1: Đang xây dựng, 2: Đang bán, 3: Đã hết hàng, 4: Tạm dừng bán, 5: Sắp mở bán
        /// </summary>
        public int? ProjectStatus { get; set; }

        /// <summary>
        /// Giá bán tối thiểu. Nếu null thì hiển thị là Liên hệ.
        /// </summary>
        public decimal? MinSellingPrice { get; set; }

        /// <summary>
        /// Giá bán tối đa. Nếu null thì hiển thị là Liên hệ.
        /// </summary>
        public decimal? MaxSellingPrice { get; set; }

        /// <summary>
        /// Giá bán dự kiến
        /// </summary>
        public decimal? ExpectedSellingPrice { get; set; }

        /// <summary>
        /// Số căn
        /// </summary>
        public int? NumberOfUnit { get; set; }

        /// <summary>
        /// Mã tỉnh thành của dự án
        /// </summary>
        public string ProvinceCode { get; set; }

        /// <summary>
        /// Tên tỉnh thành của dự án
        /// </summary>
        public string ProvinceName { get; set; }

        /// <summary>
        /// Kinh độ
        /// </summary>
        [MaxLength(512)]
        public string Latitude { get; set; }

        /// <summary>
        /// Vĩ độ
        /// </summary>
        [MaxLength(512)]
        public string Longitude { get; set; }

        /// <summary>
        /// Địa chỉ dự án
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Diện tích đất
        /// </summary>
        public string LandArea { get; set; }

        /// <summary>
        /// Trạng thái sổ đỏ (1: có sổ đỏ, 2: chưa có sổ đỏ, 3: sổ đỏ 50 năm, 4: sổ lâu dài)
        /// </summary>
        public bool RedBook { get; set; }
        /// <summary>
        /// Id đại lý
        /// </summary>
        public int TradingProviderId { get; set; }
        /// <summary>
        /// Tên đại lý
        /// </summary>
        public string TradingProviderName { get; set; }
        /// <summary>
        /// Có nổi bật hay không
        /// </summary>
        public string IsOutstanding { get; set; }
        /// <summary>
        /// Id mở bán
        /// </summary>
        public int? OpenSellId { get; set; }
        /// <summary>
        /// Đường dẫn ảnh logo dự án
        /// </summary>
        public string ProjectLogoUrl { get; set; }
        /// <summary>
        /// Có phải dự án yêu thích hay không
        /// </summary>
        public bool IsFavourite { get; set; }
        /// <summary>
        /// Lượng xem quan tâm tới dự án
        /// </summary>
        public int ViewCount { get; set; }
        /// <summary>
        /// Tổng số người tham giá đánh giá
        /// </summary>
        public int TotalReviewers { get; set; }
        /// <summary>
        /// Tỉ lệ đánh giá
        /// </summary>
        public decimal RatingRate { get;set; }
        /// <summary>
        /// Số lượng yêu thích
        /// </summary>
        public int FavoriteCount { get; set; }
        /// <summary>
        /// Đường dẫn ảnh đại diện của dự án
        /// </summary>
        public string ProjectAvatarUrl { get; set; }

        /// <summary>
        /// Ngày tạo
        /// </summary>
        public DateTime? CreatedDate { get; set; }
        /// <summary>
        /// Danh sách media banner
        /// </summary>
        public List<AppFindProjectMediaDto> Media { get; set; }
    }
}
