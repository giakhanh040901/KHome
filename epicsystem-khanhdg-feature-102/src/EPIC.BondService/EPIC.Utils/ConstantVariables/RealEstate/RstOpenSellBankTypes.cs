using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Utils.ConstantVariables.RealEstate
{
    /// <summary>
    ///  Loại ngân hàng trong mở bán
    /// </summary>
    public static class RstOpenSellBankTypes
    { 
        /// <summary>
        /// Ngân hàng của đại lý
        /// </summary>
        public const int BankTrading = 1;

        /// <summary>
        /// Ngân hàng của đối tác
        /// </summary>
        public const int BankPartner = 2;

        /// <summary>
        /// lấy cả 2 loại đại lý và đối tác
        /// </summary>
        public const int All = 3;
    }
}
