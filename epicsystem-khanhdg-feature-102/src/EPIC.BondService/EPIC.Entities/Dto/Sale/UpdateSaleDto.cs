using EPIC.Utils;
using EPIC.Utils.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Entities.Dto.Sale
{
    public class UpdateSaleDto
    {
        public int SaleId { get; set; }

        [RequiredWithOtherFields(ErrorMessage = "Khách hàng cá nhân không được bỏ trống", OtherFields = new string[] { "BusinessCustomerId" })]
        public int? InvestorId { get; set; }

        [RequiredWithOtherFields(ErrorMessage = "Khách hàng doanh nghiệp không được bỏ trống", OtherFields = new string[] { "InvestorId" })]
        public int? BusinessCustomerId { get; set; }

        [Required(ErrorMessage = "Id phòng ban không được bỏ trống")]
        public int? DepartmentId { get; set; }

        private string _employeeCode;
        [Required(AllowEmptyStrings = false, ErrorMessage = "Mã nhân viên không được bỏ trống")]
        public string EmployeeCode
        {
            get => _employeeCode;
            set => _employeeCode = value?.Trim();
        }

        [Required(ErrorMessage = "Phân loại saler không được bỏ trống")]
        [IntegerRange(ErrorMessage = "Vui lòng chọn 1 trong các loại Sale sau", AllowableValues = new int[] { SaleTypes.MANAGER,  SaleTypes.EMPLOYEE, SaleTypes.COLLABORATOR, SaleTypes.SALE_REPRESENTATIVE})]
        public int? SaleType { get; set; }
        public int? SaleParentId { get; set; }

        [RequiredWithFieldsAttribute(ErrorMessage = "Tài khoản thụ hưởng của khách hàng cá nhân không được bỏ trống", OtherFields = new string[] { "InvestorId" })]
        public int? InvestorBankAccId { get; set; }

        [RequiredWithFieldsAttribute(ErrorMessage = "Tài khoản thụ hưởng của khách hàng doanh nghiệp không được bỏ trống", OtherFields = new string[] { "BusinessCustomerId" })]
        public int? BusinessCustomerBankAccId { get; set; }
    }
}
