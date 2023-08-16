using EPIC.Entities.Dto.DistributionContractPayment;
using EPIC.Entities.Dto.GuaranteeFile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Entities.Dto.GuaranteeAsset
{
    public class GuaranteeAssetDto
    {
        public int GuaranteeAssetId { get; set; }
        public int ProductBondId { get; set; }
        public string Code { get; set; }
        public decimal? AssetValue { get; set; }
        public string DescriptionAsset { get; set; }
        public int? Status { get; set; }

        public List<GuaranteeFileDto> GuaranteeFiles { get; set; }
    }
}
