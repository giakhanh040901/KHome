using EPIC.Utils;
using EPIC.Utils.Validation;
using System.ComponentModel.DataAnnotations;

namespace EPIC.Entities.Dto.Sale
{
    public class CreateSaleTempDto
    {
        [Required(ErrorMessage = "Id khách hàng cá nhân không được bỏ trống")]
        public int? InvestorId { get; set; }

        [Required(ErrorMessage = "Id phòng ban không được bỏ trống")]
        public int? DepartmentId { get; set; }

        private string _employeeCode;
        [Required(AllowEmptyStrings = false, ErrorMessage = "Mã nhân viên không được bỏ trống")]
        public string EmployeeCode 
        { 
            get => _employeeCode; 
            set => _employeeCode = value?.Trim(); 
        }

        private string _area;
        [Required(AllowEmptyStrings = false, ErrorMessage = "Khu vực không được bỏ trống")]
        public string Area
        { 
            get => _area;
            set => _area = value?.Trim();
        }

        [Required(ErrorMessage = "Phân loại saler không được bỏ trống")]
        [IntegerRange(AllowableValues = new int[] { SaleTypes.EMPLOYEE, SaleTypes.COLLABORATOR })]
        public int? SaleType { get; set; }
        public int? SaleParentId { get; set; }
    }
}
