using EPIC.Utils;
using EPIC.Utils.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Entities.Dto.ContractTemplate
{
    public class CreateContractTemplateDto
    {
        private string _code;
        [Required(ErrorMessage = "Mã hợp đồng không được bỏ trống")]
        [StringLength(50, ErrorMessage = "Mã hợp đồng không được dài hơn 50 ký tự")]
        public string Code
        {
            get => _code;
            set => _code = value?.Trim();
        }

        private string _name;
        [Required(ErrorMessage = "Tên hợp đồng không được bỏ trống")]
        [StringLength(256, ErrorMessage = "Tên hợp đồng không được dài hơn 256 ký tự")]
        public string Name
        {
            get => _name;
            set => _name = value?.Trim();
        }
       
        [Required(ErrorMessage = "Phân loại hợp đồng không được bỏ trống")]
        public int Classify {get ;set;}

        [Required(ErrorMessage = "Loại hợp đồng không được bỏ trống")]
        public int ContractType { get; set; }
        private string _contractTempUrl { get; set; }
        [StringLength(1024, ErrorMessage = "Đường dẫn file mẫu repx không được dài hơn 1 ký tự")]
        public string ContractTempUrl
        {
            get => _contractTempUrl;
            set => _contractTempUrl = value?.Trim();
        }

        public int SecondaryId { get; set; }

        public string Type { get; set; }
    }
}
