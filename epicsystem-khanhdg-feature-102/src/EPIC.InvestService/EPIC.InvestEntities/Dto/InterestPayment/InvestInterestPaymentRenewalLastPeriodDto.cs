using EPIC.InvestEntities.DataEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.InvestEntities.Dto.InterestPayment
{
    public class InvestInterestPaymentRenewalLastPeriodDto
    {
        /// <summary>
        /// Id của chi trả
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Tổng giá trị đầu tư sau tái tục mới
        /// </summary>
        public decimal InitTotalValueOrderNew { get; set; }

        /// <summary>
        ///  Phương thức tái tục sau khi duyệt
        /// </summary>
        public int SettlementMethod { get; set; }

        /// <summary>
        /// Số tiền chi trả
        /// </summary>
        public decimal InterestPaymentMoney { get; set; }

        /// <summary>
        /// Id kỳ hạn mới
        /// </summary>
        public int RenewalsPolicyDetailId { get; set; }
    }
}
