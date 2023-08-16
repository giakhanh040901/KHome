using DocumentFormat.OpenXml.Drawing.Charts;
using System.Collections.Generic;
using static EPIC.Utils.ConstantVariables.RealEstate.RstBuildingDensityTypes;

namespace EPIC.Utils.ConstantVariables.RealEstate
{
    /// <summary>
    /// Hướng
    /// </summary>
    public static class RstDirections
    {
        public const int Dong = 1;
        public const int Tay = 2;
        public const int Nam = 3;
        public const int Bac = 4;
        public const int DongNam = 5;
        public const int DongBac = 6;
        public const int TayNam = 7;
        public const int TayBac = 8;
        public const int DongNamTayNam = 9;
        public const int DongNamDongBac = 10;
        public const int TayNamTayBac = 11;
        public const int DongNamTayBac = 12;
        public const int DongBacTayBac = 13;
        public const int DongBacTayNam = 14;

        public static string Directions(int? directions)
        {
            return directions switch
            {
                Dong => RstProductItemDirectionText.Dong,
                Tay => RstProductItemDirectionText.Tay,
                Nam => RstProductItemDirectionText.Nam,
                Bac => RstProductItemDirectionText.Bac,
                DongNam => RstProductItemDirectionText.DongNam,
                DongBac => RstProductItemDirectionText.DongBac,
                TayNam => RstProductItemDirectionText.TayNam,
                TayBac => RstProductItemDirectionText.TayBac,
                DongNamTayNam => RstProductItemDirectionText.DongNamTayNam,
                DongNamDongBac => RstProductItemDirectionText.DongNamDongBac,
                TayNamTayBac => RstProductItemDirectionText.TayNamTayBac,
                DongNamTayBac => RstProductItemDirectionText.DongNamTayBac,
                DongBacTayBac => RstProductItemDirectionText.DongBacTayBac,
                DongBacTayNam => RstProductItemDirectionText.DongBacTayNam,
                _ => string.Empty
            };
        }
    }
    public class RstProductItemDirectionText
    {
        public const string Dong = "Đông";
        public const string Tay = "Tây";
        public const string Nam = "Nam";
        public const string Bac = "Bắc";
        public const string DongNam = "Đông Nam";
        public const string DongBac = "Đông Bắc";
        public const string TayNam = "Tây Nam";
        public const string TayBac = "Tây Bắc";
        public const string DongNamTayNam = "Đông Nam + Tây Nam";
        public const string DongNamDongBac = "Đông Nam + Đông Bắc";
        public const string TayNamTayBac = "Tây Nam + Tây Bắc";
        public const string DongNamTayBac = "Đông Nam + Tây Bắc";
        public const string DongBacTayBac = "Đông Bắc + Tây Bắc";
        public const string DongBacTayNam = "Đông Bắc + Tây Nam";
        public static readonly List<string> All = new List<string>()
        {
            Dong, Tay, Nam, Bac, DongNam, DongBac,
            TayNam, TayBac, DongNamTayBac, DongNamDongBac, TayNamTayBac, DongNamTayBac, DongBacTayBac, DongBacTayNam
        };
    }
}
