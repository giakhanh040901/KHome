using EPIC.DataAccess.Base;
using EPIC.DataAccess.Models;
using EPIC.IdentityEntities.DataEntities;
using EPIC.IdentityEntities.Dto.UsersPartner;
using EPIC.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.IdentityRepositories
{
    public class UsersPartnerEFRepository : BaseEFRepository<UsersPartner>
    {
        public UsersPartnerEFRepository(EpicSchemaDbContext dbContext) : base(dbContext, "SEQ_USERS_PARTNER")
        {
        }

        public PagingResult<UsersPartner> FindAll(FilterUsersManagerDto input, int? partnerId = null)
        {
            PagingResult<UsersPartner> result = new();
            var roleQuery = _dbSet.AsQueryable().Where(r => r.Deleted == YesNo.NO
                && (input.Keyword == null));

            if (partnerId != null)
            {
                roleQuery = roleQuery.Where(r => r.PartnerId == partnerId);
            }

            result.TotalItems = roleQuery.Count();
            if (input.PageSize != -1)
            {
                roleQuery = roleQuery
                    .Skip(input.Skip)
                    .Take(input.PageSize);
            }
            result.Items = roleQuery.ToList();
            return result;
        }
    }
}
