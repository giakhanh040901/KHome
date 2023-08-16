using EPIC.DataAccess.Models;
using EPIC.InvestEntities.Dto.InvestApprove;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.InvestDomain.Interfaces
{
    public interface IInvestApproveServices
    {
        public PagingResult<ViewInvestApproveDto> Find(InvestApproveGetDto dto);
    }
}
