using EPIC.Utils;
using EPIC.Utils.Attributes;
using EPIC.Utils.ConstantVariables.Contract;
using EPIC.Utils.ConstantVariables.RealEstate;
using EPIC.Utils.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.RealEstateEntities.Dto.RstSellingPolicy
{
    public class CreateRstSellingPolicyTempDto
    {
        private string _code;
        public string Code
        {
            get => _code;
            set => _code = value?.Trim();
        }

        private string _name;
        public string Name
        {
            get => _name;
            set => _name = value?.Trim();
        }

        [Required(ErrorMessage = "Loại hình áp dụng không được bỏ trống")]
        [IntegerRange(AllowableValues = new int[] { RstSalesPolicyType.GiaTriCoDinh, RstSalesPolicyType.GiaTriCanHo }, ErrorMessage = "Vui lòng chọn 1 trong các loại hình sau")]
        public int SellingPolicyType { get; set; }

        [Required(ErrorMessage = "Loại hình áp dụng không được bỏ trống")]
        [IntegerRange(AllowableValues = new int[] { ContractSources.ONLINE, ContractSources.OFFLINE, ContractSources.ALL }, ErrorMessage = "Vui lòng chọn 1 trong các loại hình sau")]
        public int Source { get; set; }

        public decimal ConversionValue { get; set; }
        public decimal? FromValue { get; set; }
        public decimal? ToValue { get; set; }

        private string _description;
        public string Description
        {
            get => _description;
            set => _description = value?.Trim();
        }

        private string _fileName;
        public string FileName
        {
            get => _fileName;
            set => _fileName = value?.Trim();
        }

        private string _fileUrl;
        public string FileUrl
        {
            get => _fileUrl;
            set => _fileUrl = value?.Trim();
        }
    }
}
