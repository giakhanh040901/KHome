using EPIC.Utils.ConstantVariables.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Utils.ConstantVariables.Garner
{
    /// <summary>
    /// Nhóm trạng thái của order trên App
    /// </summary>
    public static class GroupStatusOrders
    {
        /// <summary>
        ///  Nhóm màn lịch sử đang tích lũy
        /// </summary>
        public static readonly List<int> DANG_DAU_TU = new() { OrderStatus.DANG_DAU_TU };

        /// <summary>
        ///  Nhóm màn sổ lệnh
        ///  Trạng thái đang đầu tư thì check xem nếu có yêu cầu rút vốn thì đổ ra
        /// </summary>
        public static readonly List<int> SO_LENH = new() { OrderStatus.KHOI_TAO, OrderStatus.CHO_THANH_TOAN, OrderStatus.CHO_DUYET_HOP_DONG, OrderStatus.CHO_KY_HOP_DONG };

        /// <summary>
        ///  Nhóm màn sổ lệnh
        ///  Trạng thái đang đầu tư thì check xem nếu có yêu cầu rút vốn thì đổ ra
        /// </summary>
        public static readonly List<int> LICH_SU = new() { OrderStatus.TAT_TOAN };

        /// <summary>
        /// Thông tin nhóm sổ lệnh
        /// </summary>
        public static readonly Dictionary<int, List<int>> GroupStatus = new Dictionary<int, List<int>>()
        {
            { AppOrderGroupStatus.DANG_DAU_TU, DANG_DAU_TU },
            { AppOrderGroupStatus.SO_LENH, SO_LENH },
            { AppOrderGroupStatus.DA_TAT_TOAN, LICH_SU },
        };
    }
}
