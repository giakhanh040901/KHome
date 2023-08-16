using EPIC.DataAccess.Models;
using EPIC.IdentityEntities.Dto.WhiteListIp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.IdentityDomain.Interfaces
{
    public interface IWhiteListIpServices
    {
        void CreateWhiteListIP(CreateWhiteListDto input);
        PagingResult<WhiteListIpDto> FindAllWhiteListIp(FilterWhiteListIpDto input);
        WhiteListIpDto GetById(int whiteListIpId);
        void UpdateWhiteListIP(int whiteListIpId, UpdateWhiteListIpDto input);
        void DeleteWhiteListIP(int whiteListIpId);
    }
}
