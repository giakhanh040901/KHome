using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Utils.ConstantVariables.Garner
{
    /// <summary>
    /// Loại hình dự án
    /// </summary>
    public static class GarnerProductTypes
    {
        public const int CO_PHAN = 1;
        public const int CO_PHIEU = 2;
        public const int TRAI_PHIEU = 3;
        public const int BAT_DONG_SAN = 4;
    }

    public static class GarnerProductTypeNames
    {
        public const string CO_PHAN = "Cổ phần ưu đãi";
        public const string CO_PHIEU = "Cổ phiếu ưu đãi";
        public const string TRAI_PHIEU = "Trái phiếu ưu đãi";
        public const string BAT_DONG_SAN = "Bất động sản";
    }
}
