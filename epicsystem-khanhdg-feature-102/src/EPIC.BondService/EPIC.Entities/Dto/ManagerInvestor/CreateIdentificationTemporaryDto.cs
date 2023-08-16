using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Entities.Dto.ManagerInvestor
{
    public class CreateIdentificationTemporaryDto : CreateManagerInvestorEkycDto
    {
        [Required(ErrorMessage = "InvestorId không được để trống")]
        [Range(0, double.MaxValue, ErrorMessage = "InvestorId không hợp lệ")]
        public int InvestorId { get; set; }
        public int InvestorGroupId { get; set; }
        /// <summary>
        /// True => Tạm; Fales => Thật
        /// </summary>
        public bool IsTemp { get; set; }
    }
}
