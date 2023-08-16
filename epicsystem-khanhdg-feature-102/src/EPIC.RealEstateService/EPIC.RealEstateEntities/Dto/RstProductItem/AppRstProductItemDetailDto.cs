using EPIC.RealEstateEntities.Dto.RstOpenSellContractTemplate;
using EPIC.RealEstateEntities.Dto.RstProductItemExtend;
using EPIC.RealEstateEntities.Dto.RstProductItemProjectUtility;
using EPIC.RealEstateEntities.Dto.RstSellingPolicy;
using EPIC.Utils.Attributes;
using EPIC.Utils.ConstantVariables.RealEstate;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.RealEstateEntities.Dto.RstProductItem
{
    public class AppRstProductItemDetailDto
    {
        /// <summary>
        /// Dùng ở xem chi tiết mở bán : Id chi tiết mở bán/ 
        /// Xem chi tiết hợp đồng: Id hợp đồng
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Id của căn
        /// </summary>
        public int ProductItemId { get; set; }

        public int ProjectId { get; set; }
        /// <summary>
        /// ID đại lý
        /// </summary>
        public int TradingProviderId { get; set; }
        /// <summary>
        /// Id tài khoản của đại lý
        /// </summary>
        public int TradingBankAccId { get; set; }

        /// <summary>
        /// Trạng thái sổ đỏ (1: có sổ đỏ, 2: sổ đỏ 50 năm, 3: sổ lâu dài, 4: chưa có sổ đỏ)
        /// <see cref="RstRedBookTypes"/>
        /// </summary>
        public int RedBookType { get; set; }

        /// <summary>
        /// Mã căn/mã sản phẩm
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Số căn/tên
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// ID mở bán
        /// </summary>
        public int OpenSellId { get; set; }
        /// <summary>
        /// ID chi tiết mở bán
        /// </summary>
        public int OpenSellDetailId { get; set; }
        /// <summary>
        /// Mã dự án
        /// </summary>
        public string ProjectCode { get; set; }

        /// <summary>
        /// Tên dự án
        /// </summary>
        public string ProjectName { get; set; }

        #region     1
        /// <summary>
        /// Giá bán 
        /// </summary>
        public decimal? Price { get; set; }

        /// <summary>
        /// Diện tích tính giá
        /// </summary>
        public decimal? PriceArea { get; set; }

        /// <summary>
        /// Kiểu phòng ngủ - số phòng (1: 1 phòng ngủ, 2: 2 phòng ngủ, 3: 3 phòng ngủ, 4: 4 phòng ngủ, 5: 5 phòng ngủ, 6: 6 phòng ngủ, 
        /// 7: 7 phòng ngủ, 8: 8 phòng ngủ, 9: 1 phòng ngủ + 1, 10: 2 phòng ngủ + 1, 11: 3 phòng ngủ + 1, 12: 4 phòng ngủ + 1)
        /// <see cref="RstRoomTypes"/>
        /// </summary>
        public int? RoomType { get; set; }

        /// <summary>
        /// Hướng cửa (1: Đông, 2: Tây, 3: Nam, 4: Bắc, 5: Đông Nam, 6: Đông Bắc, 7: Tây Nam, 8: Tây Bắc, 9: Đông Nam + Tây Bắc, 
        /// 10: Đông Nam + Đông Bắc, 11: Tây Nam + Tây Bắc, 12: Đông Nam + Tây Bắc, 13: Đông Bắc + Tây Bắc, 14: Đông Bắc + Tây Nam)
        /// <see cref="RstDirections"/>
        /// </summary>
        public int? DoorDirection { get; set; }

        #endregion

        #region Thông tin căn 
        /// <summary>
        /// Mật độ xây dựng
        /// </summary>
        public int? BuildingDensityId { get; set; }

        /// <summary>
        /// Tên mật độ xây dựng
        /// </summary>
        public string BuildingDensityName { get; set; }

        /// <summary>
        /// Phân loại sản phẩm 
        /// (1: Căn hộ thông thường, 2: Căn hộ Studio, 3: Căn hộ Officetel, 4: Căn hộ Shophouse, 5: Căn hộ Penthouse, 6: Căn hộ Duplex, 
        /// 7: Căn hộ Sky Villa, 8: Nhà ở nông thôn, 9: Biệt thự nhà ở, 10: Liền kề, 11: Chung cư thấp tầng, 12: Căn Shophouse, 13: Biệt thự nghỉ dưỡng, 14: Villa)
        /// </summary>
        public int ClassifyType { get; set; }

        /// <summary>
        /// Số tầng bao nhiêu
        /// </summary>
        public string NumberFloor { get; set; }

        /// <summary>
        /// Vị trí căn/sản phẩm (1: Căn giữa, 2: Căn góc, 3: Cổng chính, 4: Toà riêng, 5: Căn thông tầng)
        /// <see cref="RstProductLocations"/>
        /// </summary>
        public int? ProductLocation { get; set; }

        /// <summary>
        /// Diện tích thông thuỷ
        /// </summary>
        public decimal CarpetArea { get; set; }

        /// <summary>
        /// Diện tích tim tường
        /// </summary>
        public decimal BuiltUpArea { get; set; }

        /// <summary>
        /// Hướng view - mô tả
        /// </summary>
        public string ViewDescription { get; set; }

        /// <summary>
        /// Hướng ban công (1: Đông, 2: Tây, 3: Nam, 4: Bắc, 5: Đông Nam, 6: Đông Bắc, 7: Tây Nam, 8: Tây Bắc, 9: Đông Nam + Tây Bắc, 
        /// 10: Đông Nam + Đông Bắc, 11: Tây Nam + Tây Bắc, 12: Đông Nam + Tây Bắc, 13: Đông Bắc + Tây Bắc, 14: Đông Bắc + Tây Nam)
        /// <see cref="RstDirections"/>
        /// </summary>
        public int BalconyDirection { get; set; }

        /// <summary>
        /// Loại bàn giao (1: Bàn giao thô, 2: Nột thất cơ bản, 3: Nội thất liền tường, 4: Nội thất cao cấp)
        /// <see cref="RstHandingTypes"/>
        /// </summary>
        public int? HandingType { get; set; }

        /// <summary>
        /// Thời gian bàn giao
        /// </summary>
        public DateTime? HandoverTime { get; set; }

        /// <summary>
        /// Loại hình căn/sản phẩm (1: Căn đơn, 2: Căn ghép)
        /// <see cref="RstProductTypes"/>
        /// </summary>
        public int? ProductType { get; set; }

        /// <summary>
        /// Diện tích đất
        /// </summary>
        public decimal LandArea { get; set; }

        /// <summary>
        /// Diện tích xây dựng
        /// </summary>
        public decimal ConstructionArea { get; set; }

        /// <summary>
        /// Đơn giá
        /// </summary>
        public decimal? UnitPrice { get; set; }

        /// <summary>
        /// Tầng số
        /// </summary>
        public string NoFloor { get; set; }
        #endregion

        #region Nội dụng vật liệu
        /// <summary>
        /// Loại nội dung: MARKDOWN, HTML
        /// </summary>
        public string MaterialContentType { get; set; }

        /// <summary>
        /// Nội dung mô tả vật liệu thi công
        /// </summary>
        public string MaterialContent { get; set; }
        #endregion

        #region Nội dung sơ đồ thiết kế
        /// <summary>
        /// Loại nội dung sơ đồ thiết kế: MARKDOWN, HTML
        /// </summary>
        public string DesignDiagramContentType { get; set; }

        /// <summary>
        /// Nội dung mô tả sơ đồ thiết kế
        /// </summary>
        public string DesignDiagramContent { get; set; }
        #endregion

        /// <summary>
        /// Nhập căn ghép
        /// </summary>
        public string CompoundRoom { get; set; }

        /// <summary>
        /// Tầng ghép
        /// </summary>
        public string CompoundFloor { get; set; }

        /// <summary>
        /// Giá lock căn
        /// </summary>
        public decimal LockPrice { get; set; }

        /// <summary>
        /// Gía đặt cọc
        /// </summary>
        public decimal DepositPrice { get; set; }

        /// <summary>
        /// Trạng thái của sản phẩm (1: Khởi tạo (có thể là chưa mở bán hoặc đang mở bán) 2: Giữ chỗ, 3: Khóa căn, 4: Đã cọc, 5: Đã bán) 
        /// 2 trạng thái 6: chưa mở bán và 7: đang bán tự  logic từ distribution<br/>
        /// <see cref="RstProductItemStatus"/>
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// Đường dẫn ảnh đại diện
        /// </summary>
        public string UrlImage { get; set; }

        /// <summary>
        /// Thời gian giữ chỗ (s)
        /// </summary>
        public int? KeepTime { get; set; }

        /// <summary>
        /// OpenSellDetail Có hiện giá bán không? (Y/N) 
        /// </summary>
        public string IsShowPrice { get; set; }

        /// <summary>
        /// OpenSellDetail Loại liên hệ khi không hiện giá: 1: Hotline, 2: Khác
        /// </summary>
        public int? ContactType { get; set; }

        /// <summary>
        /// OpenSellDetail Số điện thoại liên hệ khi không hiện giá
        /// </summary>
        public string ContactPhone { get; set; }

        /// <summary>
        /// Tổng số người tham gia tích lũy (hợp đồng active)
        /// </summary>
        public int TotalParticipants { get; set; }

        /// <summary>
        /// Tỷ lệ đánh giá
        /// </summary>
        public decimal RatingRate { get; set; }

        /// <summary>
        /// Tổng số người tham giá đánh giá
        /// </summary>
        public int TotalReviewers { get; set; }

        #region Các thông tin liên quan
        /// <summary>
        /// Mẫu hợp đồng đặt cọc 
        /// </summary>
        public List<AppRstOpenSellContractTemplateDto> DepositContracts { get; set; }

        /// <summary>
        /// Chính sách ưu đã của mở bán và chính sách ưu đã mà chủ đầu tư cài cho căn hộ 
        /// </summary>
        public List<AppRstSellingPolicyDto> Policys { get; set; }

        /// <summary>
        /// Danh sách tiện ích
        /// </summary>
        public List<AppRstProductItemUtilityDto> Utilities { get; set; }

        /// <summary>
        /// Danh sách thông tin khác của căn hộ
        /// </summary>
        public List<AppRstProductItemExtendDto> ProductItemExtends { get; set; }
        #endregion
    }
}
