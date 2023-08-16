using EPIC.Utils;
using EPIC.Utils.Validation;
using System.ComponentModel.DataAnnotations;

namespace EPIC.Entities.Dto.Department
{
    public class MoveSaleDto
    {
        [Required(ErrorMessage = "Id saler không được bỏ trống")]
        public int? SaleId { get; set; }

        [Required(ErrorMessage = "Id phòng ban không được bỏ trống")]
        public int? DepartmentId { get; set; }

        [Required(ErrorMessage = "Loại saler không được bỏ trống")]
        [IntegerRange(AllowableValues = new int[] { SaleTypes.MANAGER, SaleTypes.EMPLOYEE, SaleTypes.COLLABORATOR })]
        public int? SaleType { get; set; }

        /// <summary>
        /// dùng cho trường hợp sale là cộng tác của một sale khác
        /// </summary>
        public int? SaleParentId { get; set; }
    }
}
