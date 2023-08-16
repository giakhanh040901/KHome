using EPIC.CoreSharedEntities.Dto.Investor;
using EPIC.Entities.Dto.BusinessCustomer;
using EPIC.GarnerEntities.Dto.GarnerPolicy;
using EPIC.GarnerEntities.Dto.GarnerProduct;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.GarnerEntities.Dto.GarnerInterestPayment
{
    public class GarnerInterestPaymentByPolicyDto
    {
        public int PolicyId { get; set; }

        public string CifCode { get; set; }
        public DateTime PayDate { get; set; }

        /// <summary>
        /// Kỳ chi trả
        /// </summary>
        public int PeriodIndex { get; set; }

        /// <summary>
        /// Số tiền thực nhận
        /// </summary>
        public decimal AmountMoney { get; set; }

        /// <summary>
        /// Tổng giá trị đầu tư
        /// </summary>
        public decimal AllTotalValue { get; set; }

        /// <summary>
        /// Tổng thuế TNCN
        /// </summary>
        public decimal Tax { get; set; }
        public List<GarnerInterestPaymentSetUpDto> Details { get; set; }

        public GarnerPolicyDto Policy { get; set; }
        public GarnerProductDto Product { get; set; }
        public BusinessCustomerDto BusinessCustomer { get; set; }
        public InvestorDto Investor { get; set; }
    }
}
