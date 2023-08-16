using EPIC.Entities.Dto.GuaranteeFile;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Entities.Dto.GuaranteeAsset
{
    public class CreateGuaranteeAssetDto : BaseGuaranteeAssetDto
    {
        public List<CreateGuaranteeFileDto> GuaranteeFiles { get; set; }
    }
}