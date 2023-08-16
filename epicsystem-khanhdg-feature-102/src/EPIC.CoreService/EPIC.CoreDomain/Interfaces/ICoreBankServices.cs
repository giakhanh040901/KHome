using EPIC.DataAccess.Models;
using EPIC.Entities.DataEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.CoreDomain.Interfaces
{
    public interface ICoreBankServices
    {
        public PagingResult<CoreBank> GetListBank(string keyword);
    }
}
