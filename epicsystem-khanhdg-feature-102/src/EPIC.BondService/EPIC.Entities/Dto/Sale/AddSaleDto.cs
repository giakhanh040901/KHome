using EPIC.Utils;
using EPIC.Utils.Validation;
using System.ComponentModel.DataAnnotations;

namespace EPIC.Entities.Dto.Sale
{
    public class AddSaleDto
    {
        [RequiredWithOtherFields(ErrorMessage = "Khách hàng cá nhân không được bỏ trống", OtherFields = new string[] { "BusinessCustomerId" })]
        public int? InvestorId { get; set; }

        [RequiredWithOtherFields(ErrorMessage = "Khách hàng doanh nghiệp không được bỏ trống", OtherFields = new string[] { "InvestorId" })]
        public int? BusinessCustomerId { get; set; }
        private string _employeeCode { get; set; }
        /// <summary>
        /// Mã nhân viên
        /// </summary>
        [Required(ErrorMessage = "Mã nhân viên không được bỏ trống")]
        public string EmployeeCode
        {
            get => _employeeCode;
            set => _employeeCode = value?.Trim();
        }

        [Required(ErrorMessage = "Loại sale không được bỏ trống")]
        [IntegerRange(AllowableValues = new int[] { SaleTypes.MANAGER, SaleTypes.EMPLOYEE, SaleTypes.COLLABORATOR, SaleTypes.SALE_REPRESENTATIVE }, ErrorMessage = "Vui lòng chọn 1 trong các loại Sale sau")]
        public int? SaleType { get; set; }

        /// <summary>
        /// Nếu là cộng tác viên thì thêm vào trường id sale cha
        /// </summary>
        public int? SaleParentId { get; set; }

        /// <summary>
        /// Id phòng ban
        /// </summary>
        [Required(ErrorMessage = "phòng ban không được bỏ trống")]
        public int? DepartmentId { get; set; }

        [RequiredWithFieldsAttribute(ErrorMessage = "Tài khoản thụ hưởng của khách hàng cá nhân không được bỏ trống", OtherFields = new string[] { "InvestorId" })]
        public int? InvestorBankAccId { get; set; }

        [RequiredWithFieldsAttribute(ErrorMessage = "Tài khoản thụ hưởng của khách hàng doanh nghiệp không được bỏ trống", OtherFields = new string[] { "BusinessCustomerId" })]
        public int? BusinessCustomerBankAccId { get; set; }
    }
}
