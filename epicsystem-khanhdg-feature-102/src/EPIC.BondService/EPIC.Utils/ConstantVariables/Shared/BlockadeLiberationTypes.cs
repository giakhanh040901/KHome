using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Utils.ConstantVariables.Shared
{
    /// <summary>
    /// Loại phong toả
    /// </summary>
    public static class BlockadeLiberationTypes
    {
        /// <summary>
        /// 1: Khác, 2: Cầm cố khoản vay, 3: Ứng vốn
        /// </summary>
        public const int OTHER = 1;
        public const int PLEDGE = 2;
        public const int ADVANCE_CAPITAL = 3;
    }
}
