using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Entities.Dto.ProductBond
{
    public class AppGuaranteeAssetDto
    {
        public int GuaranteeAssetId { get; set; }
        public string Code { get; set; }
        public decimal? AssetValue { get; set; }
        public string DescriptionAsset { get; set; }
        public List<AppGuaranteeFileDto> GuaranteeFiles { get; set; }
    }
}
