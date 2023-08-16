using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Utils.ConstantVariables.RealEstate
{
    public class RstOrderPaymentsTranClassify
    {
        public const int THANH_TOAN_DAT_COC = 1;
        public static string TranClassify(int? tranClassify)
        {
            return tranClassify switch
            {
                THANH_TOAN_DAT_COC => "Đặt cọc",
                _ => string.Empty
            };
        }
    }
}
