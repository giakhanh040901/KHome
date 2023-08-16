using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Entities.Dto.GuaranteeFile
{
    public class UpdateGuaranteeFileDto : CreateGuaranteeFileDto
    {
        public int GuaranteeFileId { get; set; }
    }
}
