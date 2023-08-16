using EPIC.Utils.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.LoyaltyEntities.Dto.LoyRank
{
    public class ViewFindRankDto
    {
        public int Id { get; set; }

        /// <summary>
        /// Tên hạng
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Mô tả
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Điểm bắt đầu
        /// </summary>
        public int? PointStart { get; set; }

        /// <summary>
        /// Điểm kết thúc
        /// </summary>
        public int? PointEnd { get; set; }

        /// <summary>
        /// Trạng thái (1: Khởi tạo; 2: Kích hoạt; 3: Hủy kích hoạt)
        /// </summary>
        public int? Status { get; set; }

        /// <summary>
        /// Ngày kích hoạt
        /// </summary>
        public DateTime? ActiveDate { get; set; }

        /// <summary>
        /// Ngày hủy kích hoạt
        /// </summary>
        public DateTime? DeactiveDate { get; set; }

        /// <summary>
        /// Ngày tạo
        /// </summary>
        public DateTime? CreatedDate { get; set; }

        /// <summary>
        /// Người tạo
        /// </summary>
        public string CreatedBy { get; set; }

    }
}
