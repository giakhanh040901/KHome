using EPIC.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.CoreEntities.Dto.Sale
{
    public class AppSaleCancelItemDto
    {
        [Range(1, int.MaxValue)]
        public int SaleTempId { get; set; }
    }

    public class AppSaleCancelDto
    {
        [Required(ErrorMessage = "Danh sách huỷ ký không được để trống")]
        [MinLength(1, ErrorMessage = "Chọn tối thiểu một đại lý")]
        public List<AppSaleCancelItemDto> ListCancel { get; set; }
    }
}
