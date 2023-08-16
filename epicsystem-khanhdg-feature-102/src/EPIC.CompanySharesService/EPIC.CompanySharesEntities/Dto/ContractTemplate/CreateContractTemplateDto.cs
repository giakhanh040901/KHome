using EPIC.Utils;
using EPIC.Utils.Validation;
using System.ComponentModel.DataAnnotations;

namespace EPIC.CompanySharesEntities.Dto.ContractTemplate
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


        private string _contractTempUrl { get; set; }
        [StringLength(1024, ErrorMessage = "Đường dẫn file mẫu repx không được dài hơn 1 ký tự")]
        public string ContractTempUrl
        {
            get => _contractTempUrl;
            set => _contractTempUrl = value?.Trim();
        }

        public string Type { get; set; }
        public string DisplayType { get; set; }

        public int SecondaryId { get; set; }
    }
}
