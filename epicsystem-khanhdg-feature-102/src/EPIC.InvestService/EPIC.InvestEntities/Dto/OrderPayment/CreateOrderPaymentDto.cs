using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.InvestEntities.Dto.OrderPayment
{
    public class CreateOrderPaymentDto
    {
        public long OrderId { get; set; }
        public int TradingBankAccId { get; set; }

        [Required(ErrorMessage = "Ngày giao dịch không được bỏ trống")]
        public DateTime? TranDate { get; set; }
        public int? TranType { get; set; }
        public int? TranClassify { get; set; }
        public int? PaymentType { get; set; }
        public decimal? PaymentAmnount { get; set; }

        private string _description;
        public string Description
        {
            get => _description;
            set => _description = value?.Trim();
        }
    }
}
