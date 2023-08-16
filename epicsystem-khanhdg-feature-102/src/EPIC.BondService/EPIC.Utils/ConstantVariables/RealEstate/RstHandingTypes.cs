using DocumentFormat.OpenXml.Drawing.Charts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static EPIC.Utils.ConstantVariables.RealEstate.RstBuildingDensityTypes;

namespace EPIC.Utils.ConstantVariables.RealEstate
{
    /// <summary>
    /// Loại bàn giao
    /// </summary>
    public static class RstHandingTypes
    {
        /// <summary>
        /// Bàn giao thô
        /// </summary>
        public const int BanGiaoTho = 1;
        /// <summary>
        /// Nội thất cơ bản
        /// </summary>
        public const int NoiThatCoBan = 2;
        /// <summary>
        /// Nội thất liền tường
        /// </summary>
        public const int NoiThatLienTuong = 3;
        /// <summary>
        /// Nội thất cao cấp
        /// </summary>
        public const int NoiThatCaoCap = 4;
        /// <summary>
        /// Full nội thất
        /// </summary>
        public const int FullNoiThat = 5;

        public static string HandingType(int? handingType)
        {
            return handingType switch
            {
                BanGiaoTho => RstHandingTypeText.BanGiaoTho,
                NoiThatCoBan => RstHandingTypeText.NoiThatCoBan,
                NoiThatLienTuong => RstHandingTypeText.NoiThatLienTuong,
                NoiThatCaoCap => RstHandingTypeText.NoiThatCaoCap,
                FullNoiThat => RstHandingTypeText.FullNoiThat,
                _ => string.Empty
            };
        }
    }

    public static class RstHandingTypeText
    {
        public const string BanGiaoTho = "Bàn giao thô";
        public const string NoiThatCoBan = "Nội thất cơ bản";
        public const string NoiThatLienTuong = "Nội thất liền tường";
        public const string NoiThatCaoCap = "Nội thất cao cấp";
        public const string FullNoiThat = "Full nội thất";
        public static readonly List<string> All = new List<string>()
        {
            BanGiaoTho, NoiThatCoBan, NoiThatLienTuong, NoiThatCaoCap, FullNoiThat
        };
    }
}
