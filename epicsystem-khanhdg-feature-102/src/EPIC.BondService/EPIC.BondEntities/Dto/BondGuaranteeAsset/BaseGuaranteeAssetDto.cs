using EPIC.Entities.Dto.GuaranteeFile;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EPIC.Entities.Dto.GuaranteeAsset
{
    public class BaseGuaranteeAssetDto
    {
        public int ProductBondId { get; set; }

        private string _code;
        [Required(ErrorMessage = "Mã tài sản đảm bảo không được bỏ trống")]
        public string Code
        {
            get => _code;
            set => _code = value?.Trim();
        }

        [Required(ErrorMessage = "Giá trị tài sản không được bỏ trống")]
        public decimal AssetValue { get; set; }

        private string _descriptionAsset;
        [Required(ErrorMessage = "Mô tả tài sản đảm bảo không được bỏ trống")]
        public string DescriptionAsset
        {
            get => _descriptionAsset;
            set => _descriptionAsset = value?.Trim();
        }
    }
}
