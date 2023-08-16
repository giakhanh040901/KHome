using EPIC.CompanySharesEntities.Dto.CpsSecondary;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.CompanySharesEntities.Dto
{
    public class CreateCpsPolicyDto : CreateCpsPolicyBaseDto
    {

        [Required(ErrorMessage = "Danh sách kỳ hạn không được bỏ trống")]
        [MinLength(1, ErrorMessage = "Danh sách kỳ hạn không được bỏ trống")]
        public List<CreateCpsPolicyDetailDto> Details { get; set; }
    }
}
