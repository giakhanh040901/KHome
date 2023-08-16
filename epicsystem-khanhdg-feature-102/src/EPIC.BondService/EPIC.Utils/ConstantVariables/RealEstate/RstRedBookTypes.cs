using System.Collections.Generic;

namespace EPIC.Utils.ConstantVariables.RealEstate
{
    /// <summary>
    /// Trạng thái sổ đỏ
    /// </summary>
    public static class RstRedBookTypes
    {
        /// <summary>
        /// Có sổ đỏ
        /// </summary>
        public const int HasRedBook = 1;
        /// <summary>
        /// Sổ đỏ 50 năm
        /// </summary>
        public const int HasRedBook50Year = 2;
        /// <summary>
        /// Sổ lâu dài
        /// </summary>
        public const int HasRedBookLongTerm = 3;
        /// <summary>
        /// Chưa có sổ đỏ
        /// </summary>
        public const int NoRedBook = 4;

        public static string RedBookType(int redBookType)
        {
            return redBookType switch
            {
                HasRedBook => RstRedBookTypeText.HasRedBook,
                HasRedBook50Year => RstRedBookTypeText.HasRedBook50Year,
                HasRedBookLongTerm => RstRedBookTypeText.HasRedBookLongTerm,
                NoRedBook => RstRedBookTypeText.NoRedBook,
                _ => string.Empty
            };
        }
    }

    public class RstRedBookTypeText
    {
        public const string HasRedBook = "Có sổ đỏ";
        public const string HasRedBook50Year = "Sổ đỏ 50 năm";
        public const string HasRedBookLongTerm = "Sổ lâu dài";
        public const string NoRedBook = "Chưa có sổ đỏ";
        public static readonly List<string> All = new List<string>()
        {
            HasRedBook, HasRedBook50Year, HasRedBookLongTerm, NoRedBook
        };
    }
}
