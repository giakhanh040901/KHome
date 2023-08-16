using DocumentFormat.OpenXml.Wordprocessing;
using EPIC.Utils.Attributes;
using EPIC.Utils.ConstantVariables.RealEstate;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.RealEstateEntities.Dto.RstProductItem
{
    public class AppGetAllProductItemDto
    {
        /// <summary>
        /// Id của mở bán
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Id căn hộ
        /// </summary>
        public int ProductItemId { get; set; }

        /// <summary>
        /// Trạng thái sổ đỏ (1: có sổ đỏ, 2: sổ đỏ 50 năm, 3: sổ lâu dài, 4: chưa có sổ đỏ)<br/>
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
        public string NoFloor { get; set; }

        /// <summary>
        /// Kiểu phòng ngủ - số phòng (1: 1 phòng ngủ, 2: 2 phòng ngủ, 3: 3 phòng ngủ, 4: 4 phòng ngủ, 5: 5 phòng ngủ, 6: 6 phòng ngủ, 
        /// 7: 7 phòng ngủ, 8: 8 phòng ngủ, 9: 1 phòng ngủ + 1, 10: 2 phòng ngủ + 1, 11: 3 phòng ngủ + 1, 12: 4 phòng ngủ + 1)
        /// <see cref="RstRoomTypes"/>
        /// </summary>
        public int? RoomType { get; set; }

        /// <summary>
        /// Hướng cửa(1: Đông, 2: Tây, 3: Nam, 4: Bắc, 5: Đông Nam, 6: Đông Bắc, 7: Tây Nam, 8: Tây Bắc, 9: Đông Nam + Tây Bắc,
        /// 10: Đông Nam + Đông Bắc, 11: Tây Nam + Tây Bắc, 12: Đông Nam + Tây Bắc, 13: Đông Bắc + Tây Bắc, 14: Đông Bắc + Tây Nam)<br/>
        /// <see cref="RstDirections"/>
        /// </summary>
        public int? DoorDirection { get; set; }

        /// <summary>
        /// Diện tích tính giá
        /// </summary>
        public decimal? PriceArea { get; set; }

        /// <summary>
        /// Giá bán nhập giá hoặc không nếu nhập giá thì luồng xử lý là có đặt cọc,
        /// nếu không nhập giá thì luồng xử lý lúc giao dịch là liên hệ
        /// </summary>
        public decimal? Price { get; set; }

        /// <summary>
        /// Mật độ
        /// </summary>
        public int? BuildingDensityId { get; set; }

        /// <summary>
        /// Loại mật độ: 1:Tòa, 2: Khu, 3: Ô đất, 4: Lô, 5: Tầng
        /// </summary>
        public int? BuildingDensityType { get; set; }

        /// <summary>
        /// Tên đơn vị mật độ 
        /// </summary>
        public string BuildingDensityName { get; set; }

        /// <summary>
        /// Trạng thái của sản phẩm (1: Khởi tạo (có thể là chưa mở bán hoặc đang mở bán) 2: Giữ chỗ, 3: Khóa căn, 4: Đã cọc, 5: Đã bán) 
        /// 6: Chưa mở bán, 7: Đang mở bán
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// Diện tích sàn xây dựng
        /// </summary>
        public decimal? FloorBuildingArea { get; set; }

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
        /// Thời gian hết hạn giữ chỗ
        /// </summary>
        public DateTime? ExpTimeHold { get; set; }

        /// <summary>
        /// Đang giữ chỗ và có phát sinh thanh toán của hợp đồng đấy
        /// </summary>
        public string HavePaymentOrder { get; set; }
    }
}
