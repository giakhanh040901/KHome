using EPIC.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.InvestEntities.Dto.OrderContractFile
{
    public class CreateOrderContractFileDto
    {
        [Required(ErrorMessage = "Id Sổ lệnh không được bỏ trống")]
        public int OrderId { get; set; }

        [Required(ErrorMessage = "Id Mẫu hợp đồng không được bỏ trống")]
        public int ContractTempId { get; set; }

        [Required(ErrorMessage = "Đường dẫn file scan không được bỏ trống")]
        public string FileScanUrl { get; set; }

        public string FileSignatureUrl { get; set; }

        public string FileTempUrl { get; set; }
    }
}
