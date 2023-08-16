using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Entities.Dto.DistributionContract
{
    public class UpdateDistributionContractDto
    {
        public int PartnerId { get; set; }
        public decimal DistributionContractId {get; set;}
        [Required(ErrorMessage = "Số lượng không được bỏ trống")]
        public long Quantity { get; set; }

        [Required(ErrorMessage = "Tổng số tiền không được bỏ trống")]
        public decimal TotalValue { get; set; }

        [Required(ErrorMessage = "Ngày mua không được bỏ trống")]
        public DateTime? DateBuy { get; set; }
    }
}
