using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Utils.ConstantVariables.RealEstate
{
    public static class RstCopyFromTypes
    {
        public const string Yes = "Có copy";
        public const string No = "Không copy";
        public static readonly List<string> All = new List<string>()
        {
            Yes, No
        };

        public static string YesNoCheck(string yesNo, string message)
        {
            return yesNo switch
            {
                "Có copy" => Yes,
                "Không copy" => No,
                "" => string.Empty,
                _ => throw new Exception(message + $" giá trị: {yesNo}"),
            };
        }
    }
}
