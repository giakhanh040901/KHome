using EPIC.Utils.ConstantVariables.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Utils.ConstantVariables.Invest
{
    public static class InvestGroupStatusOrder
    {
        /// <summary>
        ///  Nhóm màn group status = null
        /// </summary>
        public static readonly List<int> DEFAULT = new() { OrderStatus.KHOI_TAO, OrderStatus.CHO_THANH_TOAN, OrderStatus.TAT_TOAN };
        /// <summary>
        ///  Nhóm màn hợp đồng
        /// </summary>
        public static readonly List<int> DANG_DAU_TU = new() { OrderStatus.DANG_DAU_TU };

        /// <summary>
        /// Nhóm màn xử lý hợp đồng
        /// </summary>
        public static readonly List<int> XU_LY_HOP_DONG = new() { OrderStatus.CHO_DUYET_HOP_DONG };

        /// <summary>
        ///  Nhóm màn sổ lệnh
        /// </summary>
        public static readonly List<int> SO_LENH = new() { OrderStatus.KHOI_TAO, OrderStatus.CHO_THANH_TOAN, OrderStatus.CHO_KY_HOP_DONG};

        /// <summary>
        ///  Nhóm màn sổ lệnh
        /// </summary>
        public static readonly List<int> PHONG_TOA = new() { OrderStatus.PHONG_TOA, OrderStatus.GIAI_TOA };
    }
}
