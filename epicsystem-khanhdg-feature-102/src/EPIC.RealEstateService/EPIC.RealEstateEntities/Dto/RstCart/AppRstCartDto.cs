using EPIC.RealEstateEntities.Dto.RstSellingPolicy;
using EPIC.Utils.ConstantVariables.RealEstate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.RealEstateEntities.Dto.RstCart
{
    public class AppRstCartDto
    {
        /// <summary>
        /// Id của giỏ hàng Item
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Id của sản phẩm mở bán
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
        public decimal? Price { get; set; }

        /// <summary>
        /// Giá lock căn
        /// </summary>
        public decimal LockPrice { get; set; }

        /// <summary>
        /// Gía đặt cọc
        /// </summary>
        public decimal DepositPrice { get; set; }

        /// <summary>
        /// Đường dẫn ảnh đại diện
        /// </summary>
        public string UrlImage { get; set; }

        /// <summary>
        /// Trạng thái của căn hộ
        /// </summary>
        public int ProductItemStatus { get; set; }

        /// <summary>
        /// Thời gian giữ chỗ từ mở bán
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
        /// Chính sách ưu đãi
        /// </summary>
        public List<AppRstSellingPolicyDto> Policys { get; set; }
    }
}
