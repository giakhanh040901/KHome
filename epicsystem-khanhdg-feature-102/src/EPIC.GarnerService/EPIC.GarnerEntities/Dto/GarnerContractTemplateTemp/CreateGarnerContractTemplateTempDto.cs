using EPIC.Utils.ConstantVariables.Contract;
using EPIC.Utils.Validation;
using System;
using System.ComponentModel.DataAnnotations;

namespace EPIC.GarnerEntities.Dto.GarnerContractTemplateTemp
{
    public class CreateGarnerContractTemplateTempDto
    {
        private string _name;
        [Required(ErrorMessage = "Tên mẫu hợp đồng mẫu không được để trống")]
        [StringLength(256, ErrorMessage = "Tên mẫu hợp đồng mẫu không được dài hơn 256 ký tự")]
        public string Name
        {
            get => _name;
            set => _name = value?.Trim();
        }

        [IntegerRange(AllowableValues = new int[] { ContractTypes.RUT_TIEN, ContractTypes.DAT_LENH, ContractTypes.TAI_TUC_GOC, ContractTypes.RUT_TIEN_APP })]
        public int ContractType { get; set; }

        [IntegerRange(AllowableValues = new int[] { ContractSources.ONLINE, ContractSources.OFFLINE, ContractSources.ALL })]
        public int ContractSource { get; set; }

        private string _fileInvestor;
        [Required(ErrorMessage = "Đường dẫn file mẫu hợp đồng cá nhân không được để trống")]
        [StringLength(1024, ErrorMessage = "Đường dẫn file mẫu hợp đồng cá nhân không được dài hơn 1024 ký tự")]
        public string FileInvestor 
        {
            get => _fileInvestor;
            set => _fileInvestor = value?.Trim();
        }

        private string _fileBusinessCustomer;
        [Required(ErrorMessage = "Đường dẫn file mẫu hợp đồng doanh nghiệp không được để trống")]
        [StringLength(1024, ErrorMessage = "Đường dẫn file mẫu hợp đồng doanh nghiệp không được dài hơn 1024 ký tự")]
        public string FileBusinessCustomer
        {
            get => _fileBusinessCustomer;
            set => _fileBusinessCustomer = value?.Trim();
        }

        private string _description;
        public string Description 
        {
            get => _description; 
            set => _description = value?.Trim(); 
        }
    }
}
