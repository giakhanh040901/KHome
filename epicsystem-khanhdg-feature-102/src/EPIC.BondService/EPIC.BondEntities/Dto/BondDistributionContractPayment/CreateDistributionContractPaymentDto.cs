using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Entities.Dto.DistributionContractPayment
{
    public class CreateDistributionContractPaymentDto
    {

        [Required(ErrorMessage = "Hợp đồng phân phối không được bỏ trống")]
        public int DistributionContractId { get; set; }

        public int TransactionType { get; set; }

        public int PaymentType { get; set; }

        [Required(ErrorMessage = "Tổng số tiền không được bỏ trống")]
        public decimal TotalValue { get; set; }

        private string _description;
        [StringLength(500, ErrorMessage = "Mô tả không được dài hơn 500 ký tự")]
        public string Description
        {
            get => _description;
            set => _description = value?.Trim();
        }
        public DateTime? TradingDate { get; set; }
    }
}
