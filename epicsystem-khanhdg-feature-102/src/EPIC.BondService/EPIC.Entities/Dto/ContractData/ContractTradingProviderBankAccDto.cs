using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Entities.Dto.ContractData
{
    public class ContractTradingProviderBankAccDto
    {
        /// <summary>
        /// Tên chủ tài khoản
        /// </summary>
        public string BankAccName { get; set; }

        /// <summary>
        /// Số tài khoản
        /// </summary>
        public string BankAccNo { get; set; }

        /// <summary>
        /// Tên ngân hàng
        /// </summary>
        public string BankName { get; set; }
    }
}
