using EPIC.Utils.Validation;
using EPIC.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EPIC.Utils.ConstantVariables.CompanyShares;

namespace EPIC.CompanySharesEntities.Dto.CpsSecondary
{
    public class CreateCpsSecondaryDto
    {
        public int SecondaryId { get; set; }

        //[Required(ErrorMessage = "Số lượng không được bỏ trống")]		
        //[Range(1, int.MaxValue, ErrorMessage = "Số lượng phải lớn hơn 0")]
        //public int? Quantity { get; set; }

        [Required(ErrorMessage = "Ngày bắt đầu bán không được bỏ trống")]
        public DateTime? OpenCellDate { get; set; }

        [Required(ErrorMessage = "Ngày kết thúc bán không được bỏ trống")]
        public DateTime? CloseCellDate { get; set; }

        //[Required(ErrorMessage = "Danh sách chính sách không được bỏ trống")]	
        //[MinLength(1, ErrorMessage = "Danh sách chính sách không được bỏ trống")]
        public List<CreateCpsPolicyDto> Policies { get; set; }
        public List<int?> BusinessCustomerBankAcc { get; set; }

    }
}
