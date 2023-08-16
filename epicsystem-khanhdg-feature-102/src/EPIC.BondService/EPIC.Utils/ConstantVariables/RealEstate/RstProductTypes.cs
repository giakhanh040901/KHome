using System.Collections.Generic;

namespace EPIC.Utils.ConstantVariables.RealEstate
{
    /// <summary>
    /// Loại hình căn/sản phẩm
    /// </summary>
    public static class RstProductTypes
    {
        /// <summary>
        /// Căn đơn
        /// </summary>
        public const int CanDon = 1;
        /// <summary>
        /// Căn ghép
        /// </summary>
        public const int CanGhep = 2;

        public static string RstProductType(int? productTypes)
        {
            return productTypes switch
            {
                CanDon => RstProductTypesText.CanDon,
                CanGhep => RstProductTypesText.CanGhep,
                _ => string.Empty
            };
        }
    }

    public class RstProductTypesText
    {
        public const string CanDon = "Căn đơn";
        public const string CanGhep = "Căn ghép";
        public static readonly List<string> All = new List<string>()
        {
             CanDon, CanGhep
        };
    }
}
