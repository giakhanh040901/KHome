using EPIC.Entities.Dto.GuaranteeFile;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Entities.Dto.GuaranteeAsset
{
    public class UpdateGuaranteeAssetDto : BaseGuaranteeAssetDto
    {
        public int GuaranteeAssetId { get; set; }
        public int? Status { get; set; }
        public List<UpdateGuaranteeFileDto> GuaranteeFiles { get; set; }
    }
}
