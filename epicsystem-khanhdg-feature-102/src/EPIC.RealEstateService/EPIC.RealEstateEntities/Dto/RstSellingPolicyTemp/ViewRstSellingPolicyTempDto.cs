using EPIC.Utils.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.RealEstateEntities.Dto.RstSellingPolicy
{
    public class ViewRstSellingPolicyTempDto
    {
        public int Id { get; set; }

        /// <summary>
        /// Mã chính sách
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Tên chính sách
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Loại hình áp dụng: 1 giá trị cố định, 2 giá trị căn hộ 
        /// </summary>
        public int? SellingPolicyType { get; set; }

        /// <summary>
        /// Loại hình áp dụng: 1 online, 2 offline, 3 all
        /// </summary>
        public int Source { get; set; }

        /// <summary>
        /// Giá trị quy ra tiền
        /// </summary>
        public decimal ConversionValue { get; set; }

        /// <summary>
        /// Giá trị từ
        /// </summary>
        public decimal FromValue { get; set; }

        /// <summary>
        /// Đến giá trị
        /// </summary>
        public decimal ToValue { get; set; }

        /// <summary>
        /// Trạng thái: A kích hoạt, D hủy kích hoạt
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Mô tả chính sách
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Tên file
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// Đường dẫn file
        /// </summary>
        public string FileUrl { get; set; }

        public DateTime? CreatedDate { get; set; }
    }
}
