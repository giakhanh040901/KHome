using EPIC.Utils.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.CoreEntities.Dto.ManagerInvestor
{
    public class CreateRequestPhoneDto
    {
        private string _phone { get; set; }

        public int InvestorId { get; set; }

        [PhoneEpic]
        public string Phone { get => _phone; set => _phone = value?.Trim(); }

        public bool IsTemp { get; set; }
    }
}
