using EPIC.Utils.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Entities.Dto.ManagerInvestor
{
    public class ResetUserPasswordManagerInvestorDto
    {
        public int UserId { get; set; }
        public int InvestorId { get; set; }
    }
}
