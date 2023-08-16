using EPIC.Utils.Attributes;
using EPIC.Utils.ConstantVariables.Loyalty;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.LoyaltyEntities.Dto.LoyHisAccumulatePoint
{
    public class ViewHisAccumulatePointStatusLogDto
    {
        public int Id { get; set; }
        //public int HisAccumulatePointId { get; set; }

        /// <summary>
        /// Ghi chú
        /// </summary>
        public string Note { get; set; }

        /// <summary>
        /// Trạng thái (1: Chờ duyệt; 2: Đang giao; 3: Đã nhận; 4: Hoàn thành)
        /// <see cref="LoyHisAccumulatePointStatus"/>
        /// </summary>
        public int? Status { get; set; }
        public int? ExchangedPointStatus { get; set; }

        /// <summary>
        /// Nguồn (1: on; 2: off)
        /// </summary>
        public int? Source { get; set; }

        public DateTime? CreatedDate { get; set; }

        public string CreatedBy { get; set; }

    }
}
