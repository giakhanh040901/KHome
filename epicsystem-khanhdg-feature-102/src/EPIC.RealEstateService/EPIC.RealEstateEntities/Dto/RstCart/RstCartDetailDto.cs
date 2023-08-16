using EPIC.RealEstateEntities.Dto.RstSellingPolicy;
using EPIC.Utils.ConstantVariables.RealEstate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.RealEstateEntities.Dto.RstCart
{
    public class RstCartDetailDto
    {
        public int Id { get; set; }

        /// <summary>
        /// Id Đại lý mở bán
        /// </summary>
        public int TradingProviderId { get; set; }

        /// <summary>
        /// Id của sản phẩm mở bán
        /// </summary>
        public int OpenSellDetailId { get; set; }

        /// <summary>
        /// Id phân phối sản phẩm
        /// </summary>
        public int DistributionId { get; set; }

        /// <summary>
        /// Id mở bán
        /// </summary>
        public int OpenSellId { get; set; }

        /// <summary>
        /// Id dự án
        /// </summary>
        public int ProjectId { get; set; }

        /// <summary>
        /// Id căn hộ
        /// </summary>
        public int ProductItemId { get; set; }
        /// <summary>
        /// Mã dự án
        /// </summary>
        public string ProjectCode { get; set; }

        /// <summary>
        /// Tên dự án
        /// </summary>
        public string ProjectName { get; set; }

        /// <summary>
        /// Mã căn/mã sản phẩm
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Số căn/tên
        /// </summary>
        public string Name { get; set; }

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
        /// Diện tích tính giá
        /// </summary>
        public decimal? PriceArea { get; set; }

        /// <summary>
        /// Giá
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// Trạng thái của căn hộ
        /// </summary>
        public int ProductItemStatus { get; set; }

        /// <summary>
        /// Trạng thái của căn hộ trong mở bán
        /// </summary>
        public int OpenSellDetailStatus { get; set; }

        /// <summary>
        /// Thời gian giữ chỗ
        /// </summary>
        public int? KeepTime { get; set; }

        /// <summary>
        /// Hotline liên hệ của đại lý qua mở bán
        /// </summary>
        public string Hotline { get; set; }

        /// <summary>
        /// OpenSellDetail Có hiện giá bán không? (Y/N) 
        /// </summary>
        public string IsShowPrice { get; set; }

        /// <summary>
        /// OpenSellDetail Có khóa bán không? (Y/N) 
        /// </summary>
        public string IsLock { get; set; }

        /// <summary>
        /// OpenSellDetail Loại liên hệ khi không hiện giá: 1: Hotline, 2: Khác
        /// </summary>
        public int? ContactType { get; set; }

        /// <summary>
        /// OpenSellDetail Số điện thoại liên hệ khi không hiện giá
        /// </summary>
        public string ContactPhone { get; set; }
    }
}
