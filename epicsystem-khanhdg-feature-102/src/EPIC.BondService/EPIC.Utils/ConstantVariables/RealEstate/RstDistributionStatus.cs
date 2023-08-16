using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Utils.ConstantVariables.RealEstate
{
    public static class RstDistributionStatus
    {
        public const int KHOI_TAO = 1;
        public const int CHO_DUYET = 2;
        /// <summary>
        /// ĐANG PHÂN PHỐI/ ĐANG BÁN
        /// </summary>
        public const int DANG_BAN = 3; 
        public const int TAM_DUNG = 4;
        public const int HET_HANG = 5;
        public const int HUY_DUYET = 6;

        /// <summary>
        /// Mở bán: Dừng không cho mở lại
        /// </summary>
        public const int DUNG_BAN = 7;
    }
}
