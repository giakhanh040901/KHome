using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Utils
{
    /// <summary>
    /// Loại sale
    /// </summary>
    public static class SaleTypes
    {
        /// <summary>
        /// Là quản lý
        /// </summary>
        public const int MANAGER = 1;

        /// <summary>
        /// Là chuyên viên viên tư vấn
        /// </summary>
        public const int EMPLOYEE = 2;

        /// <summary>
        /// Cộng tác viên
        /// </summary>
        public const int COLLABORATOR = 3;

        /// <summary>
        /// Sale bán hộ
        /// </summary>
        public const int SALE_REPRESENTATIVE = 4;
    }
}
