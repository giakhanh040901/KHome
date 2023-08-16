using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.MSB.ConstVariables
{
    /// <summary>
    /// Trạng thái của Request Payment Detail : 1. Khởi tạo, 2. Thành Công (Success), 3. Thất bại (Failed)
    /// </summary>
    public static class MsbRequestDetailStatus
    {
        public const int INIT = 1;
        public const int SUCCESS = 2;
        public const int FAIL = 3;
    }
}
