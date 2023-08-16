using EPIC.Entities.Dto.User;
using System;

namespace EPIC.Entities.Dto.ManagerInvestor
{
    public class DiffRequestInvestorDto
    {
        public ViewManagerInvestorTemporaryDto Investor { get; set; }
        public ViewManagerInvestorTemporaryDto InvestorTemp { get; set; }
        public DateTime? RequestDate { get; set; }
        public UserDto UserRequest { get; set; }
    }
}
