using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.RealEstateEntities.Dto.RstSellingPolicy
{
    public class RstSellingPolicyDto
    {
        /// <summary>
        /// Id chính sách mở bán
        /// </summary>
        public int? Id { get; set; }
        /// <summary>
        /// Id chính sách mở bán mẫu (ttrong cài đặt)
        /// </summary>
        public int SellingPolicyTempId { get; set; }
        /// <summary>
        /// Mã chính sách
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// Tên chính sách
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Loại hình đặt cọc
        /// </summary>
        public int? Source { get; set; }
        /// <summary>
        /// Giá trị quy đổi
        /// </summary>
        public decimal? ConversionValue { get; set; }
        /// <summary>
        /// Trạng thái
        /// </summary>
        public string Status { get; set; }
        public string IsSellingPolicySelected { get; set; }
        public int TradingProviderId { get; set; }
        public int OpenSellId { get; set; }
    }
}
