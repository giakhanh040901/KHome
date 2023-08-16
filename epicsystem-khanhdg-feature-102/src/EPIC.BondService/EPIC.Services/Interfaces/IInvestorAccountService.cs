using EPIC.DataAccess.Models;
using EPIC.Entities.Dto.BondInvestorAccount;
using EPIC.Entities.Dto.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.BondDomain.Interfaces
{
    public interface IInvestorAccountService
    {
        PagingResult<UserIsInvestorDto> GetByType(FindBondInvestorAccountDto dto);
    }
}
