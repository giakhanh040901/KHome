using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Utils.ConstantVariables.Core
{
    public static class FilterChartStatisticOrder
    {
        public const string WEEK = "W";
        public const string MONTH = "M";
        public const string YEAR = "Y";
        public static DateTime StartDate(string day, int number)
        {
            DateTime now = DateTime.Now;
            return day switch
            {
                WEEK => now.AddDays(-number * 7),
                MONTH => now.AddMonths(-number),
                YEAR => now.AddYears(-number),
                _ => now.AddDays(-6)
            };
        }
    }
}
