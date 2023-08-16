using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Entities.DataEntities
{
    public class CustomerHVFExcelReport
    {
        public DateTime? CreatedDate { get; set; }
        public string CifCode { get; set; }
        public string Status { get; set; }
        public int? InvestorId { get; set; }
        public int? BusinessCustomerId { get; set; }
        public string ReferralCode { get; set; }

        /// <summary>
        /// Số tài khoản chứng khoán
        /// </summary>
        public string StockTradingAccount { get; set; }

        /// <summary>
        /// Có phải nhà đầu tư chuyên nghiệp hay không
        /// </summary>
        public string IsProf { get; set; }

        /// <summary>
        /// Công ty chứng khoán
        /// </summary>
        public int SecurityCompany { get; set; }

        /// <summary>
        /// Nguồn tạo
        /// </summary>
        public int Source { get; set; }
        public decimal InvestmentValue { get; set; }
    }
}
