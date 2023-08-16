using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Entities.Dto.Investor
{
    /// <summary>
    /// Địa chỉ giao dịch
    /// </summary>
    public class TransactionAddessDto
    {

        public int ContactAddressId { get; set; }
        /// <summary>
        /// địa chỉ
        /// </summary>
        public string ContactAddress { get; set; }
        /// <summary>
        /// là mặc định hay không
        /// </summary>
        public string IsDefault { get; set; }
    }
}
