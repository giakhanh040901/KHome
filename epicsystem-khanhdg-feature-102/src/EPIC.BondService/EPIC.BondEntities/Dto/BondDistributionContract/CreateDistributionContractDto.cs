using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Entities.Dto.DistributionContract
{
    public class CreateDistributionContractDto
    {
        public int PartnerId { get; set; }

        [Required(ErrorMessage = "Đại lý sơ cấp không được bỏ trống")]
        public int TradingProviderId { get; set; }

        [Required(ErrorMessage = "Phát hành sơ cấp không được bỏ trống")]
        public int PrimaryId { get; set; }

        [Required(ErrorMessage = "Số lượng không được bỏ trống")]
        public long Quantity { get; set; }

        [Required(ErrorMessage = "Tổng số tiền không được bỏ trống")]
        public decimal TotalValue { get; set; }

        [Required(ErrorMessage = "Ngày mua không được bỏ trống")]
        public DateTime? DateBuy { get; set; }
    }
}
