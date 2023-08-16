using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Utils.ConstantVariables.RealEstate
{
    public class RstProductItemPolicyStatus
    {
        public static string ProductItemPolicyStatus(string status)
        {
            return status switch
            {
                Status.INACTIVE => RstProductItemPolicyStatusText.Khoa,
                Status.ACTIVE => RstProductItemPolicyStatusText.KichHoat,
                _ => string.Empty
            };
        }

        public class RstProductItemPolicyStatusText
        {
            public const string Khoa = "Khóa";
            public const string KichHoat = "Kích hoạt";
            public static readonly List<string> All = new List<string>()
            {
                 Khoa, KichHoat
            };
        }
    }
}
