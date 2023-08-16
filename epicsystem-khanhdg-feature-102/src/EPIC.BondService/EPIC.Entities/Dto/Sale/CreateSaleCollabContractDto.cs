using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Entities.Dto.Sale
{
    public class CreateSaleCollabContractDto
    {
        [Required(ErrorMessage = "Id Sale không được bỏ trống")]
        public int SaleId { get; set; }

        [Required(ErrorMessage = "Id Mẫu hợp đồng sale không được bỏ trống")]
        public int CollabContractTempId { get; set; }

        [Required(ErrorMessage = "Đường dẫn file không được bỏ trống")]
        public string FileScanUrl { get; set; }
    }
}
