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
    /// Vị trí căn/sản phẩm
    /// </summary>
    public static class RstProductLocations
    {
        /// <summary>
        /// Căn Giữa
        /// </summary>
        public const int CanGiua = 1;
        /// <summary>
        /// Căn góc
        /// </summary>
        public const int CanGoc = 2;
        /// <summary>
        /// Cổng chính
        /// </summary>
        public const int CongChinh = 3;
        /// <summary>
        /// Toà riêng
        /// </summary>
        public const int ToaRieng = 4;
        /// <summary>
        /// Căn thông tầng
        /// </summary>
        public const int CanThongTang = 5;

        public static string ProductLocation(int? productLocation)
        {
            return productLocation switch
            {
                CanGiua => RstProductLocationText.CanGiua,
                CanGoc => RstProductLocationText.CanGoc,
                CongChinh => RstProductLocationText.CongChinh,
                ToaRieng => RstProductLocationText.ToaRieng,
                CanThongTang => RstProductLocationText.CanThongTang,
                _ => string.Empty
            };
        }
    }

    public class RstProductLocationText
    {
        public const string CanGiua = "Căn giữa";
        public const string CanGoc = "Căn góc";
        public const string CongChinh = "Cổng chính";
        public const string ToaRieng = "Toà riêng";
        public const string CanThongTang = "Căn thông tầng";
        public static readonly List<string> All = new List<string>()
        {
            CanGiua, CanGoc, CongChinh, ToaRieng, CanThongTang
        };
    }
}
