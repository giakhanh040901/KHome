using EPIC.Utils.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.CoreEntities.Dto.ManagerInvestor
{
    public class CreateRequestEmailDto
    {
        private string _email { get; set; }

        public int InvestorId { get; set; }
        [Email]
        public string Email { get => _email; set => _email = value?.Trim(); }

        public bool IsTemp { get; set; }
    }
}
