using System.ComponentModel.DataAnnotations;

namespace EPIC.Entities.Dto.Department
{
    public class BaseDepartmentDto
    {
        private string _departmentName;
        [Required(ErrorMessage = "Tên phòng không được bỏ trống")]
        [MaxLength(512, ErrorMessage = "Tên phòng không dài quá 512 ký tự")]
        public string DepartmentName
        {
            get => _departmentName;
            set => _departmentName = value?.Trim();
        }

        private string _departmentAddress;
        [MaxLength(512, ErrorMessage = "Địa chỉ phòng không dài quá 512 ký tự")]
        public string DepartmentAddress
        {
            get => _departmentAddress;
            set => _departmentAddress = value?.Trim();
        }
    }
}
