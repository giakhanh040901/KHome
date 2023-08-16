using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Utils.DataUtils
{
    public static class RandomNumberUtils
    {

        public static string RandomNumber(int length)
        {
            Random random = new Random();
            const string chars = "0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public static string RandomCharNumber(int length, string prefix = "")
        {
            Random random = new Random();
            const string chars = "0123456789ABCDEGIKLMNOPGRSTUVXY";
            return prefix + new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
