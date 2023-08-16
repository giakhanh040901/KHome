using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Entities.Dto.AssetManager
{
    /// <summary>
    /// phân bổ tài sản
    /// </summary>
    public class DistributionAssetDto
    {
        /// <summary>
        /// Đầu tư
        /// </summary>
        public long Invest { get; set; }

        /// <summary>
        /// mua sắm
        /// </summary>
        public long Shopping { get; set; }

        /// <summary>
        /// Giao dịch bất động sản
        /// </summary>
        public long TransectionRealEstate { get; set; }

        /// <summary>
        /// Cho thuê bất động sản
        /// </summary>
        public long LeaseRealEstate { get; set; }
    }
}
