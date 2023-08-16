using EPIC.Utils.ConstantVariables.Payment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Utils.ConstantVariables.RealEstate
{
    public class RstProductItemIsLock
    {
        public static string IsLock(string isLock)
        {
            return isLock switch
            {
                YesNo.YES => RstProductItemIsLockText.Khoa,
                YesNo.NO => RstProductItemIsLockText.MoKhoa,
                _ => string.Empty
            };
        }

        public class RstProductItemIsLockText
        {
            public const string Khoa = "Khóa"; 
            public const string MoKhoa = "Mở khóa";
            public static readonly List<string> All = new List<string>()
            {
                 Khoa, MoKhoa
            };
        }
    }
}
