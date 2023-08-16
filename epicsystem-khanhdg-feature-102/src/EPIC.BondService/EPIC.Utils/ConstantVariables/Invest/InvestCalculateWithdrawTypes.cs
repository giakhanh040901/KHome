using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Utils.ConstantVariables.Invest
{
    /// <summary>
    /// Cách tính lợi tức rút vốn: 1: Kỳ hạn thấp hơn gần nhất, 2: Giá trị cố định
    /// </summary>
    public static class InvestCalculateWithdrawTypes
    {
        /// <summary>
        /// Tính toán lợi tức rút theo kỳ hạn gần nhất
        /// </summary>
        public const int KY_HAN_THAP_HON_GAN_NHAT = 1;

        /// <summary>
        /// Tính theo % lợi nhuận cố định
        /// </summary>
        public const int GIA_CO_DINH = 2;
    }
}
