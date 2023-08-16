using System.Collections.Generic;

namespace EPIC.Utils.ConstantVariables.RealEstate
{
    /// <summary>
    /// Loại mật độ xây dựng, thông thường có 2 cấp
    /// </summary>
    public static class RstBuildingDensityTypes
    {
        /// <summary>
        /// cấp 1
        /// </summary>
        public const int Toa = 1;
        /// <summary>
        /// cấp 1
        /// </summary>
        public const int PhanKhu = 2;
        /// <summary>
        /// cấp 1
        /// </summary>
        public const int ODat = 3;

        /// <summary>
        /// cấp 2 - áp dụng: Phân khu
        /// </summary>
        public const int Lo = 4;

        /// <summary>
        /// cấp 2 - áp dụng: Tầng
        /// </summary>
        public const int Tang = 5;

        public class RstBuildingDensityTypesText
        {
            public const string Toa = "Tòa";
            public const string PhanKhu = "Phân khu";
            public const string Odat = "Ô đất";
            public const string Lo = "Lô";
            public const string Tang = "Tầng";
            public static readonly List<string> All = new List<string>()
            {
                Toa, PhanKhu, Odat, Lo, Tang
            };
        }

        public static string BuildingDensityTypes(int? buildingDensityTypes)
        {
            return buildingDensityTypes switch
            {
                Toa => RstBuildingDensityTypesText.Toa,
                PhanKhu => RstBuildingDensityTypesText.PhanKhu,
                ODat => RstBuildingDensityTypesText.Odat,
                Lo => RstBuildingDensityTypesText.Lo,
                Tang => RstBuildingDensityTypesText.Tang,
                _ => string.Empty
            };
        }
    }
}
