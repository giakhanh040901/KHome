using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Entities.Dto.ContractData
{
    public class ContractDepositProviderDto
    {
        /// <summary>
        /// Tên đại lý lưu ký
        /// </summary>
        public string DepositName { get; set; }

        /// <summary>
        /// Tên viết đại lý lưu ký
        /// </summary>
        public string DepositShortName { get; set; }
    }
}
