using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Entities.Dto.OrderContractFile
{
    public class UpdateOrderContractFileDto : CreateOrderContractFileDto
    {
        [Required(ErrorMessage = "Id Không được bỏ trống")]
        public int OrderContractFileId { get; set; }
    }
}
