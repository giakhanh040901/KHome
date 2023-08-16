using EPIC.DataAccess.Base;
using EPIC.DataAccess.Models;
using EPIC.IdentityEntities.DataEntities;
using EPIC.IdentityEntities.Dto.Roles;
using EPIC.Utils.ConstantVariables.Identity;
using EPIC.Utils;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EPIC.IdentityEntities.Dto.WhiteListIp;
using EPIC.Entities.DataEntities;

namespace EPIC.IdentityRepositories
{
    public class WhiteListIpEFRepository : BaseEFRepository<WhiteListIp>
    {
        public WhiteListIpEFRepository(DbContext dbContext) : base(dbContext, "SEQ_WHITE_LIST_IP")
        {
        }

        public PagingResult<WhiteListIp> FindAll(FilterWhiteListIpDto input, int? tradingProviderId = null)
        {
            PagingResult<WhiteListIp> result = new();
            var whiteListIpQuery = _dbSet.AsQueryable().Where(r => r.Deleted == YesNo.NO
                && ((tradingProviderId == null && (input.TradingProviderId == null || r.TradingProviderId == input.TradingProviderId))|| r.TradingProviderId == tradingProviderId)
                && (input.Keyword == null || r.Name != null && r.Name.Contains(input.Keyword)));

            if (input.PageSize != -1)
            {
                whiteListIpQuery = whiteListIpQuery
                    .Skip(input.Skip)
                    .Take(input.PageSize);
            }
            result.Items = whiteListIpQuery.ToList();
            result.TotalItems = whiteListIpQuery.Count();
            return result;
        }
    }
}
