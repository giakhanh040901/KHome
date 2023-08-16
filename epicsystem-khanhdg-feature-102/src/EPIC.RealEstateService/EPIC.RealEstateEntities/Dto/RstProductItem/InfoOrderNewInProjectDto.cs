using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.RealEstateEntities.Dto.RstProductItem
{
    /// <summary>
    /// Thông tin hợp đồng mới nhất được tạo trong dự án
    /// </summary>
    public class InfoOrderNewInProjectDto
    {
        /// <summary>
        /// Id dự án
        /// </summary>
        public int? ProjectId { get; set; }

        /// <summary>
        /// Id mở bán
        /// </summary>
        public int? OpenSellId { get; set; }

        /// <summary>
        /// Tên đại lý
        /// </summary>
        public string TradingProviderName { get; set; }

        /// <summary>
        /// Tên viết tắt (nếu có)
        /// </summary>
        public string AliasName { get; set; }

        /// <summary>
        /// Tên khách hàng
        /// </summary>
        public string FullName { get; set; }

        public string AvatarImageUrl { get; set; }
        /// <summary>
        /// Mã căn/mã sản phẩm
        /// </summary>
        public string ProductItemCode { get; set; }

        /// <summary>
        /// Trạng thái của hợp đồng
        /// </summary>
        public int OrderStatus { get; set; }

        /// <summary>
        /// Thời gian hết hạn giữ chỗ
        /// </summary>
        public DateTime? ExpTimeDeposit { get; set; }
    }
}
