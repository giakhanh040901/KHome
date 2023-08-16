using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Utils.ConstantVariables.Core
{
    /// <summary>
    /// Trạng thái todo
    /// </summary>
    public static class InvestorTodoStatus
    {
        /// <summary>
        /// Khởi tạo
        /// </summary>
        public const int INIT = 1;
        /// <summary>
        /// Đã xem
        /// </summary>
        public const int SEEN = 2;
    }

    public static class InvestorAccountSource 
    {
        public const int APP = 1;
        public const int CMS = 2;
        public const int SALER = 3;
    }

}
