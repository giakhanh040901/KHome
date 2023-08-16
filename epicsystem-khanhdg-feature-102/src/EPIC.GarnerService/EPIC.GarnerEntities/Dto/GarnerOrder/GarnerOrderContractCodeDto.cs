using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.GarnerEntities.Dto.GarnerOrder
{
    /// <summary>
    /// Thông tin sinh mã hợp đồng thừ ConfigContractCode
    /// </summary>
    public class GarnerOrderContractCodeDto
    {
        public int ConfigContractCodeId { get; set; }

        /// <summary>
        /// Id order
        /// </summary>
        public long? OrderId { get; set; }

        /// <summary>
        /// Mã chính sách
        /// </summary>
        public string PolicyCode { get; set; }

        /// <summary>
        /// Mã sản phẩn
        /// </summary>
        public string ProductCode { get; set; }

        /// <summary>
        /// Ngày tạo
        /// </summary>
        public DateTime? Now { get; set; }
    }
}
