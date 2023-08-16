using EPIC.RealEstateEntities.Dto.RstProductItem;
using EPIC.Utils.ConstantVariables.RealEstate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.RealEstateEntities.Dto.RstDistributionProductItem
{
    public class RstDistributionProductItemDto
    {
        public int Id { get; set; }

        public int ProductItemId { get; set; }
        /// <summary>
        /// Giá trị Lock căn
        /// </summary>
        public decimal LockPrice { get; set; }

        /// <summary>
        /// Giá trị cọc
        /// </summary>
        public decimal DepositPrice { get; set; }

        /// <summary>
        /// Trạng thái căn - Đối tác có khóa căn này ko cho đại lý bán không (A/D)
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// trạng thái của sản phẩm dự án
        /// 1: Khởi tạo (Đại lý chưa mở bán căn hộ này) 2: Giữ chỗ, 3: Khóa căn, 4: Đã cọc, 5: Đã bán, 7: đang bán mở bán (Đại lý mở bán căn hộ này) <br/>
        /// <see cref="RstProductItemStatus"/>
        /// </summary>
        public int ProductItemStatus { get; set; }

        /// <summary>
        /// Có phải là trạng thái dự án (Đã khóa căn, đã cọc, đã bán) là do đại lý giao dịch không
        /// </summary>
        public bool IsProductItemStatusOfTrading { get; set; }

        /// <summary>
        /// Ngày tạo phân phối cho căn hộ
        /// </summary>
        public DateTime? CreatedDate { get; set; }
        public RstProductItemDto ProductItem { get; set; }

        /// <summary>
        /// Tầng số
        /// </summary>
        public string ProductItemNoFloor { get; set; }

        /// <summary>
        /// Số tầng bao nhiêu
        /// </summary>
        public string ProductItemNumberFloor { get; set; }

        /// <summary>
        /// Kiểu phòng ngủ - số phòng (1: 1 phòng ngủ, 2: 2 phòng ngủ, 3: 3 phòng ngủ, 4: 4 phòng ngủ, 5: 5 phòng ngủ, 6: 6 phòng ngủ,
        /// 7: 7 phòng ngủ, 8: 8 phòng ngủ, 9: 1 phòng ngủ + 1, 10: 2 phòng ngủ + 1, 11: 3 phòng ngủ + 1, 12: 4 phòng ngủ + 1)
        /// <see cref="RstRoomTypes"/>
        /// </summary>
        public int? ProductItemRoomType { get; set; }

        /// <summary>
        /// Giá bán nhập giá hoặc không nếu nhập giá thì luồng xử lý là có đặt cọc,
        /// nếu không nhập giá thì luồng xử lý lúc giao dịch là liên hệ
        /// </summary>
        public decimal? ProductItemPrice { get; set; }

        /// <summary>
        /// Diện tích tính giá
        /// </summary>
        public decimal? ProductItemPriceArea { get; set; }

        /// <summary>
        /// Đơn giá
        /// </summary>
        public decimal? ProductItemUnitPrice { get; set; }
    }
}
