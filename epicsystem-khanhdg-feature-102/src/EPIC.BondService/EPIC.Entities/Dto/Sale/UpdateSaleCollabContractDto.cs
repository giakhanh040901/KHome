using EPIC.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Entities.Dto.Sale
{
    public class UpdateSaleCollabContractDto : CreateSaleCollabContractDto
    {
        [Required(ErrorMessage = "Id Không được bỏ trống")]
        public int CollabContractId { get; set; }
    }
}
