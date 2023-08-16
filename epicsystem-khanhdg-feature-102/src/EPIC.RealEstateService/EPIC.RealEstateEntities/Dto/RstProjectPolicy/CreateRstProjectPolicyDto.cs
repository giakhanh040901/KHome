using EPIC.Utils;
using EPIC.Utils.ConstantVariables.RealEstate;
using EPIC.Utils.ConstantVariables.Shared;
using EPIC.Utils.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.RealEstateEntities.Dto.RstProjectPolicy
{
    public class CreateRstProjectPolicyDto
    {
        public int ProjectId { get; set; }

        private string _name;
        [Required(ErrorMessage = "Tên chính sách không được để trống")]
        [StringLength(256, ErrorMessage = "Tên chính sách không được dài hơn 256 ký tự")]
        public string Name 
        { 
            get => _name; 
            set => _name = value?.Trim(); 
        }

        private string _code;
        [Required(ErrorMessage = "Mã chính sách không được để trống")]
        [StringLength(256, ErrorMessage = "Mã chính sách không được dài hơn 256 ký tự")]
        public string Code 
        { 
            get => _code; 
            set => _code = value?.Trim(); 
        }

        private string _description;
        [StringLength(512, ErrorMessage = "Mô tả chính sách không được dài hơn 512 ký tự")]
        public string Description 
        { 
            get => _description; 
            set => _description = value?.Trim(); 
        }

        [Required(ErrorMessage = "Loại chính sách không được bỏ trống")]
        [IntegerRange(AllowableValues = new int[] { RstProjectPolicyTypes.ChinhSachQuaTang, RstProjectPolicyTypes.ChinhSachThanhToanSom, RstProjectPolicyTypes.ChinhSachMuaNhieu, RstProjectPolicyTypes.ChinhSachNgoaiGiao }, ErrorMessage = "Vui lòng chọn 1 trong các loại chính sách sau")]
        public int PolicyType { get; set; }

        [Required(ErrorMessage = "Loại hình đặt cọc không được bỏ trống")]
        [IntegerRange(AllowableValues = new int[] { SourceOrder.ONLINE, SourceOrder.OFFLINE, SourceOrder.ALL }, ErrorMessage = "Vui lòng chọn 1 trong các loại hình đặt cọc sau")]

        public int Source { get; set; }

        public decimal? ConversionValue { get; set; }
    }
}
