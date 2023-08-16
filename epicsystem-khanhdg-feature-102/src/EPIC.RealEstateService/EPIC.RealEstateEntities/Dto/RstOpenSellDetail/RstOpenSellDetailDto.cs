using EPIC.RealEstateEntities.Dto.RstDistributionPolicy;
using EPIC.RealEstateEntities.Dto.RstProductItem;
using EPIC.RealEstateEntities.Dto.RstSellingPolicy;
using EPIC.Utils.Attributes;
using EPIC.Utils.ConstantVariables.RealEstate;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.RealEstateEntities.Dto.RstOpenSellDetail
{
    public class RstOpenSellDetailDto
    {
        /// <summary>
        /// Id của openSellDetail
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Id Phân phối mở bán
        /// </summary>
        public int OpenSellId { get; set; }

        /// <summary>
        /// Id căn từ DistributionProductItemId lần lên ProductItem
        /// </summary>
        public int DistributionProductItemId { get; set; }

        /// <summary>
        /// Có khóa căn hay không?
        /// </summary>
        public string IsLock { get; set; }

        /// <summary>
        /// Có hiện giá bán không? (Y/N) 
        /// Không hiện (N) thì có chọn liên hệ để xem thông tin giá
        /// </summary>
        public string IsShowPrice { get; set; }

        /// <summary>
        /// Loại liên hệ khi không hiện giá: 1: Hotline, 2: Khác
        /// </summary>
        public int? ContactType { get; set; }

        /// <summary>
        /// Số điện thoại liên hệ khi không hiện giá
        /// </summary>
        public string ContactPhone { get; set; }

        /// <summary>
        /// Trạng thái (1: Khởi tạo (có thể là chưa mở bán hoặc đang mở bán) 2: Giữ chỗ, 3: Khóa căn, 4: Đã cọc, 5: Đã bán)  <br/>
        /// <see cref="RstProductItemStatus"/>
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// Trạng thái của căn hộ
        /// </summary>
        public int ProductItemStatus { get; set; }

        /// <summary>
        /// Trạng thái của phân phối sản phẩm cho mở bán
        /// </summary>
        public string DistributionProductItemStatus { get; set; }

        /// <summary>
        /// Giá trị Lock căn
        /// </summary>
        public decimal LockPrice { get; set; }

        /// <summary>
        /// Giá trị cọc
        /// </summary>
        public decimal DepositPrice { get; set; }
        /// <summary>
        /// Thời gian giữ thanh toán cọc
        /// </summary>
        public int? KeepTime { get; set; }

        /// <summary>
        /// Bật tắt show App (Y/N)
        /// </summary>
        public string IsShowApp { get; set; }
        public string Deleted { get; set; }

        /// <summary>
        /// Diện tích tính giá căn hộ
        /// </summary>
        public decimal? ProductItemPriceArea { get; set; }

        /// <summary>
        /// Giá bán căn hộ
        /// </summary>
        public decimal? ProductItemPrice { get; set; }

        /// <summary>
        /// Thông tin phân phối con
        /// </summary>
        public RstProductItemDto ProductItem { get; set; }

        /// <summary>
        /// Thông tin chính sách phân phối từ phân phối sản phẩm do đối tác cài
        /// </summary>
        public RstDistributionPolicyDto DistributionPolicy { get; set; }

        /// <summary>
        /// Chính sách mở bán
        /// </summary>
        public List<RstSellingPolicyDto> SellingPolicy { get; set; }
    }
}
