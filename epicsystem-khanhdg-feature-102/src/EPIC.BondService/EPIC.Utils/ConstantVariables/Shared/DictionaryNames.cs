using EPIC.Utils.ConstantVariables.Garner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Utils.ConstantVariables.Shared
{
    /// <summary>
    /// Thông tin chi tiết của hằng số
    /// </summary>
    public class DictionaryNames
    {
        /// <summary>
        /// Thông tin ngày tháng năm
        /// </summary>
        /// <param name="periodUnit"></param>
        /// <returns></returns>
        public static string PeriodUnitNameFind(string periodUnit)
        {
            var listPeriod = new List<string>()
            {
                PeriodUnit.YEAR,
                PeriodUnit.MONTH,
                PeriodUnit.DAY,
                PeriodUnit.QUARTER
            };

            if(!listPeriod.Contains(periodUnit))
            {
                return null;
            }
            var productTypeName = new Dictionary<string, string>()
            {
                { PeriodUnit.YEAR, PeriodUnitName.YEAR},
                { PeriodUnit.MONTH, PeriodUnitName.MONTH },
                { PeriodUnit.QUARTER, PeriodUnitName.QUARTER },
                { PeriodUnit.DAY, PeriodUnitName.DAY },
            };
            return productTypeName[periodUnit];
        }

        /// <summary>
        /// Loại hình sản phẩm tích lũy
        /// </summary>
        /// <param name="productType"></param>
        /// <returns></returns>
        public static string ProductTypeName(int productType)
        {
            var listPeriod = new List<int>()
            {
                GarnerProductTypes.CO_PHAN,
                GarnerProductTypes.CO_PHIEU,
                GarnerProductTypes.TRAI_PHIEU,
                GarnerProductTypes.BAT_DONG_SAN
            };

            if (!listPeriod.Contains(productType))
            {
                return null;
            }
            var productTypeName = new Dictionary<int, string>()
            {
                { GarnerProductTypes.CO_PHAN, GarnerProductTypeNames.CO_PHAN },
                { GarnerProductTypes.CO_PHIEU, GarnerProductTypeNames.CO_PHIEU },
                { GarnerProductTypes.TRAI_PHIEU, GarnerProductTypeNames.TRAI_PHIEU },
                { GarnerProductTypes.BAT_DONG_SAN, GarnerProductTypeNames.BAT_DONG_SAN },
            };
            return productTypeName[productType];
        }
    }
}
