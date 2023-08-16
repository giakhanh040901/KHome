using EPIC.Utils.ConstantVariables.Invest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.InvestEntities.Dto.InterestPayment
{
    public class FilterCalulationInterestPayment
    {
        // bool isAllSystem = false, int? methodInterest = null, List<int> tradingProviderChildIds = null, List<string> sort = null)
        public DateTime? NgayChiTra { get; set; }

        /// <summary>
        ///  Lọc ngày chính xác hay không
        /// </summary>
        public string IsExactDate { get; set; }

        /// <summary>
        /// Số điện thoại
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// Mã hợp đồng
        /// </summary>
        public string ContractCode { get; set; }

        /// <summary>
        /// Mã số thuế
        /// </summary>
        public string TaxCode { get; set; }

        /// <summary>
        /// Id dự án
        /// </summary>
        public int? ProjectId { get; set; }

        /// <summary>
        /// Hình thức chi trả lợi tức, đáo hạn
        /// <see cref="InvestMethodInterests"/>
        /// </summary>
        public int? MethodInterest { get; set; }

        /// <summary>
        /// Lọc là kỳ cuối (Y/N)
        /// </summary>
        public string IsLastPeriod { get; set; }

        /// <summary>
        /// Mã khách hàng
        /// </summary>
        public string CifCode { get; set; }

        /// <summary>
        /// Số giấy tờ của khach hàng
        /// </summary>
        public string IdNo { get; set; }

        public int? SettlementMethod { get; set; }
        public List<int> TradingProviderChildIds { get;set; }
    }
}
