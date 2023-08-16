using EPIC.Utils.ConstantVariables.Contract;
using EPIC.Utils.Validation;
using EPIC.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EPIC.Utils.ConstantVariables.Shared;

namespace EPIC.RealEstateEntities.Dto.RstContractTemplateTemp
{
    public class CreateRstContractTemplateTempDto
    {
        private string _name;
        [Required(ErrorMessage = "Tên mẫu hợp đồng mẫu không được để trống")]
        [StringLength(256, ErrorMessage = "Tên mẫu hợp đồng mẫu không được dài hơn 256 ký tự")]
        public string Name
        {
            get => _name;
            set => _name = value?.Trim();
        }

        /// <summary>
        /// Loai hop dong (1: ho so dat lenh, 2: ho so rut tien, 3: ho so tai tuc goc, 4: ho so rut tien app, 5: ho so tai tuc goc + loi nhuan)
        /// </summary>
        [IntegerRange(AllowableValues = new int[] { ContractTypes.RUT_TIEN, ContractTypes.DAT_LENH, ContractTypes.TAI_TUC_GOC, ContractTypes.RUT_TIEN_APP, ContractTypes.TAI_TUC_GOC_VA_LOI_NHUAN })]
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
