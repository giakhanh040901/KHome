using System;

namespace EPIC.Utils.ConstantVariables.Shared
{
    /// <summary>
    /// Loại thời gian
    /// </summary>
    public class TimeTypes
    {
        public const string YEAR = "Y";
        public const string MONTH = "M";
        public const string DAY = "D";
        public const string WEEK = "W";
        public const string HOUR = "H";

        /// <summary>
        /// Tính thời gian kết thúc
        /// </summary>
        /// <returns></returns>
        public static DateTime CalculatorEndDate(DateTime time, string timerType, int number)
        {
            return timerType switch
            {
                HOUR => time.AddHours(number),
                DAY => time.AddDays(number),
                WEEK => time.AddDays(number * 7),
                MONTH => time.AddMonths(number),
                YEAR => time.AddYears(number),
                _ => time
            };
        }
    }
}
