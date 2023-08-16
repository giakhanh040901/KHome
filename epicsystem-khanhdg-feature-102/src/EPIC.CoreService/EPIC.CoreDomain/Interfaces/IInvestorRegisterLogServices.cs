using EPIC.CoreEntities.DataEntities;
using EPIC.CoreEntities.Dto.InvestorRegistorLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.CoreDomain.Interfaces
{
    public interface IInvestorRegisterLogServices
    {
        InvestorRegisterLog Add(CreateInvestorRegisterLogDto input);
    }
}
