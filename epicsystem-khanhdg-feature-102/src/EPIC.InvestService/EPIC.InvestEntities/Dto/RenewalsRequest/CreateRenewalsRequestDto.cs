using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.InvestEntities.Dto.RenewalsRequest
{
    public class CreateRenewalsRequestDto
    {
        [Required(ErrorMessage ="Vui lòng chọn Id hợp đồng order")]
        public long OrderId { get; set; }

        [Required(ErrorMessage = "Phương thức tái tục không được bỏ trống")]
        public int SettlementMethod { get; set; }

        [Required(ErrorMessage = "Kỳ hạn tái tục không được bỏ trống")]
        public int RenewarsPolicyDetailId { get; set; }
        public string RequestNote { get; set; }
        public string Summary { get; set; }
    }
}
