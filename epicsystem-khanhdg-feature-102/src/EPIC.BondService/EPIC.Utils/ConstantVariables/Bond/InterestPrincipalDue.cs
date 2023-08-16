using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Utils.ConstantVariables.Bond
{
    public class InterestPrincipalDue
    {
        /// <summary>
        /// Ngày đáo hạn
        /// </summary>
        public DateTime? DueDate { get; set; }
        public int? BusinessCustomerId { get; set; }
        public int? InvestorId { get; set; }
        public long OrderId { get; set; }
        /// <summary>
        /// Mã hợp đồng
        /// </summary>
        public string ContractCode { get; set; }
        
        /// <summary>
        /// Số lượng trái phiếu
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// Mã khách hàng
        /// </summary>
        public string CifCode { get; set; }
        
        /// <summary>
        /// Mã trái phiếu
        /// </summary>
        public string BondCode { get; set; }

        /// <summary>
        /// Lãi suất, lợi tức
        /// </summary>
        public Decimal Profit { get; set; }

        /// <summary>
        /// Giá trị hợp đồng
        /// </summary>
        public Decimal TotalValue { get; set; }

        /// <summary>
        /// Loại kì hạn
        /// </summary>
        public string PeriodType { get; set; }

        /// <summary>
        /// Số lượng kì hạn
        /// </summary>
        public int PeriodQuantity { get; set; }

        /// <summary>
        /// Kỳ hạn
        /// </summary>
        public int InterestPeriod { get; set; }

        /// <summary>
        /// Phân loại trái phiếu
        /// </summary>
        public int Classify { get; set; }

        /// <summary>
        /// Thuế thu nhập cá nhân
        /// </summary>
        public Decimal IncomeTax { get; set; }
    }
}
