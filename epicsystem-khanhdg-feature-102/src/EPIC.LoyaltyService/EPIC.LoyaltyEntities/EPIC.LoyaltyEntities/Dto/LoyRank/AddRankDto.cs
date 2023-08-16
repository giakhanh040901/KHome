using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.LoyaltyEntities.Dto.LoyRank
{
    public class AddRankDto
    {
        private string _name;
        private string _description;

        /// <summary>
        /// Tên hạng
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = "Tên hạng không được bỏ trống")]
        public string Name { get => _name; set => _name = value?.Trim(); }

        /// <summary>
        /// Mô tả
        /// </summary>
        public string Description { get => _description; set => _description = value?.Trim(); }

        /// <summary>
        /// Điểm bắt đầu
        /// </summary>
        [Required(ErrorMessage = "Số điểm bắt đầu không được bỏ trống")]
        public int? PointStart { get; set; }

        /// <summary>
        /// Điểm kết thúc
        /// </summary>
        [Required(ErrorMessage = "Số điểm kết thúc không được bỏ trống")]
        public int? PointEnd { get; set; }

        /// <summary>
        /// Ngày kích hoạt
        /// </summary>
        public DateTime? ActiveDate { get; set; }

        /// <summary>
        /// Ngày hủy kích hoạt
        /// </summary>
        public DateTime? DeactiveDate { get; set; }
    }
}
