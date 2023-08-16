using EPIC.Utils.Validation;
using EPIC.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EPIC.Entities.Dto.BusinessCustomer;

namespace EPIC.RealEstateEntities.Dto.RstOpenSellBank
{
    public class RstOpenSellBankDto
    {
        public int Id { get; set; }
        public int OpenSellId { get; set; }

        /// <summary>
        /// Tài khoản ngân hàng của đối tác
        /// </summary>
        public int? PartnerBankAccountId { get; set; }

        /// <summary>
        /// Tài khoản ngân hàng của đại lý
        /// </summary>
        public int? TradingBankAccountId { get; set; }

        /// <summary>
        /// Thông tin ngân hàng của đại lý
        /// </summary>
        public BusinessCustomerBankDto BankAccount { get; set; }
    }
}
