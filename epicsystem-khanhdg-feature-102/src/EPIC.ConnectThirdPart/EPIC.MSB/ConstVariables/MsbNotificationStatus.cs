using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.MSB.ConstVariables
{
    /// <summary>
    /// Trạng thái xử lý notification msb
    /// </summary>
    public class MsbNotificationStatus
    {
        /// <summary>
        /// Thất bại, hiện trên giao diện đang hiện là khởi tạo
        /// </summary>
        public const int FAIL = 1;
        /// <summary>
        /// Thành công
        /// </summary>
        public const int SUCCESS = 2;
        /// <summary>
        /// Bị lặp
        /// </summary>
        public const int DUPLICATE = 3;
    }
}
