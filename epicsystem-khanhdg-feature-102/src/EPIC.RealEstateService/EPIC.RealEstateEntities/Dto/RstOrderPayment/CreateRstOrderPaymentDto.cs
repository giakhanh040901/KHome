using EPIC.Utils.Validation;
using EPIC.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.RealEstateEntities.Dto.RstOrderPayment
{
    public class CreateRstOrderPaymentDto
    {
        public int OrderId { get; set; }

        /// <summary>
        /// Tài khoản ngân hàng của đối tác
        /// </summary>
        public int? PartnerBankAccountId { get; set; }

        /// <summary>
        /// Tài khoản ngân hàng của đại lý
        /// </summary>
        public int? TradingBankAccountId { get; set; }

        public DateTime? TranDate { get; set; }
        public int TranType { get; set; }
        public int TranClassify { get; set; }
        public int PaymentType { get; set; }
        public decimal? PaymentAmount { get; set; }

        private string _description;
        public string Description
        {
            get => _description;
            set => _description = value?.Trim();
        }
    }
}
