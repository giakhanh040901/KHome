using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.BondEntities.Dto.InterestPayment
{
    public class InterestPaymentCreateListDto
    {
        public List<InvestInterestPaymentCreateDto> InterestPayments { get; set; }
    }

    public class InvestInterestPaymentCreateDto
    {
        public int OrderId { get; set; }
        public int PeriodIndex { get; set; }

        private string _cifCode { get; set; }
        public string CifCode
        {
            get => _cifCode;
            set => _cifCode = value?.Trim();
        }
        public decimal AmountMoney { get; set; }
        public int PolicyDetailId { get; set; }
        public DateTime? PayDate { get; set; }
        public string IsLastPeriod { get; set; }
    }
}
