using EPIC.Utils.Attributes;
using EPIC.Utils.ConstantVariables.Loyalty;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.LoyaltyEntities.Dto.ConversionPoint
{
    public class LoyConversionPointStatusLogDto
    {
        public int Id { get; set; }

        /// <summary>
        /// Ghi chú
        /// </summary>
        public string Note { get; set; }

        /// <summary>
        /// Trạng thái đổi điểm : 1. Khởi tạo, 2. Tiếp nhận Y/C, 3. Đang giao, 4.Hoàn thành, 5.Hủy yêu cầu
        /// <see cref="LoyHisAccumulatePointStatus"/>
        /// </summary>
        public int? Status { get; set; }

        /// <summary>
        /// Nguồn
        /// </summary>
        public int? Source { get; set; }

        /// <summary>
        /// Thời gian thực hiện
        /// </summary>
        public DateTime? CreatedDate { get; set; }

        /// <summary>
        /// Người thực hiện
        /// </summary>
        public string CreatedBy { get; set; }
    }
}
