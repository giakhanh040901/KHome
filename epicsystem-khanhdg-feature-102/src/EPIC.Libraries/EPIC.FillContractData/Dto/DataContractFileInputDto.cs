using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.FillContractData.Dto
{
    /// <summary>
    /// Input class cho hàm xuất file word xem tạm trên app
    /// </summary>
    public class ContractFileInputCustomerDtoBase : IdInfoInvestorDto
    {
        /// <summary>
        /// Id khách hàng cá nhân
        /// </summary>
        public int? InvestorId { get; set; }
        /// <summary>
        /// Id khách hàng doanh nghiệp
        /// </summary>
        public int? BusinessCustomerId { get; set; }
        /// <summary>
        /// Id đại lý
        /// </summary>
        public int TradingProviderId { get; set; }
        /// <summary>
        /// id tài khoản thụ hưởng của đại lý trong bán theo kỳ hạn
        /// </summary>
        public int TradingBankAccId { get; set; }
        /// <summary>
        /// Ký hay chưa (Y:YES, N:NO)
        /// </summary>
        public bool IsSign { get; set; } = false;
        public int? ConfigContractCodeId { get; set; }
        public string ContractCode { get; set; }
    }
}
