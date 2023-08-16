using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.LoyaltyEntities.Dto.LoyHisAccumulatePoint
{
    public class AppConsumePointDto
    {
        [Required(ErrorMessage = "Điểm quy đổi không được để trống")]
        public int Point { get; set; }

        [Required(ErrorMessage = "Đại lý không được bỏ trống")]
        public int TradingProviderId { get; set; }
    }
}
