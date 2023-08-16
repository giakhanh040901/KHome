using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Utils.ConstantVariables.Core
{
    /// <summary>
    /// Loại todo
    /// </summary>
    public static class InvestorToDoTypes
    {
        /// <summary>
        /// Khoản đầu tư invest đến hạn
        /// </summary>
        public const int INVEST_DEN_HAN = 1;
        /// <summary>
        /// Tài khoản chưa xác minh
        /// </summary>
        public const int TAI_KHOAN_CHUA_XAC_MINH = 2;
        /// <summary>
        /// Giao dịch dang dở invest
        /// </summary>
        public const int INVEST_GIAO_DICH_DANG_DO = 3;
        /// <summary>
        /// Giao dịch dang dở Garner
        /// </summary>
        public const int GARNER_GIAO_DICH_DANG_DO = 4;
        /// <summary>
        /// Giao dịch dang dở BĐS
        /// </summary>
        public const int RST_GIAO_DICH_DANG_DO = 5;
    }
}
