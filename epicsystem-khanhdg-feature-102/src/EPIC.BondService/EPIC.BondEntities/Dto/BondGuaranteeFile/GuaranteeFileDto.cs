using EPIC.Entities.Dto.DistributionContractPayment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Entities.Dto.GuaranteeFile
{
    public class GuaranteeFileDto
    {
        public int GuaranteeFileId { get; set; }
        public int GuaranteeAssetId { get; set; }
        public string Title { get; set; }
        public string FileUrl { get; set; }
    }
}
