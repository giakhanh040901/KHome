using System.ComponentModel.DataAnnotations;

namespace EPIC.Entities.Dto.Sale
{
    public class UpdateSaleTempDto
    {
        [Required(ErrorMessage = "Id Sale tạm không được bỏ trống")]
        public int SaleTempId { get; set; }
        
        [Required(ErrorMessage = "Id phòng ban không được bỏ trống")]
        public int? DepartmentId { get; set; }
        public int? ParentId { get; set; }

        private string _employeeCode;
        [Required(ErrorMessage = "Mã nhân viên không được bỏ trống")]
        public string EmployeeCode
        {
            get => _employeeCode;
            set => _employeeCode = value?.Trim();
        }
    }
}
