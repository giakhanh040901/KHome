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
    public class AppDirectionSaleDto
    {
        [Required(ErrorMessage = "danh sách saler không được bỏ trống")]
        [MinLength(1)]
        public int[] SaleRegisterIds { get; set; }

        public bool IsCancel { get; set; }

        //[Required(ErrorMessage = "Đại lý sơ cấp không được bỏ trống")]
        [MinLength(1)]
        public int[] TradingProviders { get; set; }

        [IntegerRange(AllowableValues = new int[] { SaleTypes.EMPLOYEE, SaleTypes.COLLABORATOR })]
        public int? SaleType { get; set; }
    }
}
