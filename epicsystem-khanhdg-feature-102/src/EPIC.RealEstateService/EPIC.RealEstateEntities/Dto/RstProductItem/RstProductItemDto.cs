using EPIC.RealEstateEntities.Dto.RstProductItemExtend;
using EPIC.RealEstateEntities.Dto.RstProjectStructure;
using EPIC.Utils.ConstantVariables.RealEstate;
using System;
using System.Collections.Generic;

namespace EPIC.RealEstateEntities.Dto.RstProductItem
{
    public class RstProductItemDto
    {
        public int Id { get; set; }
        public int ProjectId { get; set; }

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
        /// Số tầng bao nhiêu
        /// </summary>
        public string NumberFloor { get; set; }

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

        /// <summary>
        /// Hướng ban công (1: Đông, 2: Tây, 3: Nam, 4: Bắc, 5: Đông Nam, 6: Đông Bắc, 7: Tây Nam, 8: Tây Bắc, 9: Đông Nam + Tây Bắc,
        /// 10: Đông Nam + Đông Bắc, 11: Tây Nam + Tây Bắc, 12: Đông Nam + Tây Bắc, 13: Đông Bắc + Tây Bắc, 14: Đông Bắc + Tây Nam)
        /// <see cref="RstDirections"/>
        /// </summary>
        public int BalconyDirection { get; set; }

        /// <summary>
        /// Vị trí căn/sản phẩm (1: Căn giữa, 2: Căn góc, 3: Cổng chính, 4: Toà riêng, 5: Căn thông tầng)
        /// <see cref="RstProductLocations"/>
        /// </summary>
        public int? ProductLocation { get; set; }

        /// <summary>
        /// Loại hình căn/sản phẩm (1: Căn đơn, 2: Căn ghép)
        /// <see cref="RstProductTypes"/>
        /// </summary>
        public int? ProductType { get; set; }

        /// <summary>
        /// Loại bàn giao (1: Bàn giao thô, 2: Nột thất cơ bản, 3: Nội thất liền tường, 4: Nội thất cao cấp)
        /// <see cref="RstHandingTypes"/>
        /// </summary>
        public int? HandingType { get; set; }

        /// <summary>
        /// Hướng view - mô tả
        /// </summary>
        public string ViewDescription { get; set; }

        /// <summary>
        /// Diện tích thông thuỷ
        /// </summary>
        public decimal CarpetArea { get; set; }

        /// <summary>
        /// Diện tích tim tường
        /// </summary>
        public decimal BuiltUpArea { get; set; }

        /// <summary>
        /// Diện tích đất
        /// </summary>
        public decimal LandArea { get; set; }

        /// <summary>
        /// Diện tích xây dựng
        /// </summary>
        public decimal ConstructionArea { get; set; }

        /// <summary>
        /// Giá bán nhập giá hoặc không nếu nhập giá thì luồng xử lý là có đặt cọc,
        /// nếu không nhập giá thì luồng xử lý lúc giao dịch là liên hệ
        /// </summary>
        public decimal? Price { get; set; }

        /// <summary>
        /// Loại nội dung: MARKDOWN, HTML
        /// </summary>
        public string MaterialContentType { get; set; }

        /// <summary>
        /// Nội dung mô tả vật liệu thi công
        /// </summary>
        public string MaterialContent { get; set; }

        /// <summary>
        /// Loại nội dung sơ đồ thiết kế: MARKDOWN, HTML
        /// </summary>
        public string DesignDiagramContentType { get; set; }

        /// <summary>
        /// Nội dung mô tả sơ đồ thiết kế
        /// </summary>
        public string DesignDiagramContent { get; set; }

        /// <summary>
        /// Nhập căn ghép
        /// </summary>
        public string CompoundRoom { get; set; }

        /// <summary>
        /// Tầng ghép
        /// </summary>
        public string CompoundFloor { get; set; }

        /// <summary>
        /// Trạng thái của sản phẩm (1: Khởi tạo (có thể là chưa mở bán hoặc đang mở bán) 2: Giữ chỗ, 3: Khóa căn, 4: Đã cọc, 5: Đã bán)
        /// 2 trạng thái 6: chưa mở bán và 7: đang bán tự  logic từ distribution<br/>
        /// <see cref="RstProductItemStatus"/>
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// Mật độ xây dựng
        /// </summary>
        public int? BuildingDensityId { get; set; }

        /// <summary>
        /// Phân loại sản phẩm (1: Căn hộ thông thường, 2: Căn hộ Studio, 3: Căn hộ Officetel, 4: Căn hộ Shophouse, 5: Căn hộ Penthouse, 6: Căn hộ Duplex,
        /// 7: Căn hộ Sky Villa, 8: Nhà ở nông thôn, 9: Biệt thự nhà ở, 10: Liền kề, 11: Chung cư thấp tầng, 12: Căn Shophouse, 13: Biệt thự nghỉ dưỡng, 14: Villa)
        /// </summary>
        public int ClassifyType { get; set; }

        /// <summary>
        /// Diện tích tính giá
        /// </summary>
        public decimal? PriceArea { get; set; }

        /// <summary>
        /// Đơn giá
        /// </summary>
        public decimal? UnitPrice { get; set; }

        /// <summary>
        /// Tầng số
        /// </summary>
        public string NoFloor { get; set; }

        public string IsLock { get; set; }

        /// <summary>
        /// Thời gian bàn giao
        /// </summary>
        public DateTime? HandoverTime { get; set; }

        /// <summary>
        /// danh sách tên đại lý
        /// </summary>
        public List<string> ListTradingProviderName { get; set; }

        /// <summary>
        /// Diện tích sàn xây dựng
        /// </summary>
        public decimal? FloorBuildingArea { get; set; }

        /// <summary>
        /// Số lượt xem của mở bán
        /// </summary>
        public int ViewCount { get; set; }

        /// <summary>
        /// Mật độ xây dựng
        /// </summary>
        public RstProjectStructureDto ProjectStructure { get; set; }

        /// <summary>
        /// Các thông tin khác của sản phẩm
        /// </summary>
        public List<RstProductItemExtendDto> ProductItemExtends { get; set; }
    }
}