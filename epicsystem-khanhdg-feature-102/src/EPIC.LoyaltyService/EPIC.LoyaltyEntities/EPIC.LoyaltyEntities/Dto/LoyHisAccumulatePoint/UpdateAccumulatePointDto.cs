using EPIC.Utils.Attributes;
using EPIC.Utils.ConstantVariables.Loyalty;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EPIC.Utils.Validation;

namespace EPIC.LoyaltyEntities.Dto.LoyHisAccumulatePoint
{
    public class UpdateAccumulatePointDto
    {
        private string _description;

        [Required(ErrorMessage = "Id không được bỏ trống")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Investor id ko được bỏ trống")]
        public int InvestorId { get; set; }

        /// <summary>
        /// Số điểm
        /// </summary>
        [Required(ErrorMessage = "Số điểm ko được bỏ trống")]
        public int Point { get; set; }

        /// <summary>
        /// Tích điểm hay tiêu điểm
        /// 1: Tích điểm, 2: Tiêu điểm<br/>
        /// <see cref="LoyPointTypes"/>
        /// </summary>
        [IntegerRange(AllowableValues = new int[] {LoyPointTypes.TIEU_DIEM, LoyPointTypes.TICH_DIEM})]
        public int PointType { get; set; }

        /// <summary>
        /// Lý do
        /// <see cref="LoyHisAccumulatePointReasons"/>
        /// </summary>
        public int? Reason { get; set; }

        /// <summary>
        /// Mô tả
        /// </summary>
        public string Description { get => _description; set => _description = value?.Trim(); }

        /// <summary>
        /// Ngày áp dụng thực tế
        /// </summary>
        public DateTime? ApplyDate { get; set; }

    }
}
