using EPIC.Utils.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.RealEstateEntities.Dto.RstDistribution
{
    /// <summary>
    /// Thêm mới phân phối
    /// </summary>
    public class CreateRstDistributionDto
    {
        /// <summary>
        /// Chọn dự án
        /// </summary>
        public int ProjectId { get; set; }
        /// <summary>
        /// Chọn đại lý phân phối
        /// </summary>
        public int TradingProviderId { get; set; }

        /// <summary>
        /// Loại phân phối 1: Phân phối không độc quyền, 2: Phân phối độc quyền
        /// </summary>
        public int? DistributionType { get; set; }

        /// <summary>
        /// Ngày phân phối
        /// </summary>
        [Required(ErrorMessage ="Ngày phân phối không được bỏ trống")]
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Ngày kết thúc phân phối
        /// </summary>
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// Danh sách ngân hàng của chủ đầu tư
        /// </summary>
        public List<int> PartnerBankAccountIds { get; set; }
    }
}
