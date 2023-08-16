using EPIC.DataAccess.Base;
using EPIC.DataAccess.Models;
using EPIC.InvestEntities.DataEntities;
using EPIC.InvestEntities.Dto.PolicyTemp;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Invest;
using EPIC.Utils.ConstantVariables.Shared;
using EPIC.Utils.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.InvestRepositories
{
    public class InvestPolicyTempEFRepository : BaseEFRepository<PolicyTemp>
    {
        public InvestPolicyTempEFRepository(EpicSchemaDbContext dbContext, ILogger logger) : base(dbContext, logger, $"{DbSchemas.EPIC}.{Distribution.SEQ}")
        {
        }

        public PagingResult<PolicyTemp> FindAll(FilterPolicyTempDto input)
        {
            var result = new PagingResult<PolicyTemp>();
            var query = _epicSchemaDbContext.InvestPolicyTemps.Include(policy => policy.PolicyDetailTemps)
                                                                .Where(policy => policy.Deleted == YesNo.NO
                                                                              && ((input.Keyword == null) || (policy.Code.ToLower().Contains(input.Keyword.ToLower()))
                                                                                || (policy.Name.ToLower().Contains(input.Keyword.ToLower())))
                                                                              && (input.Status == null || policy.Status.ToLower() == input.Status.ToLower())
                                                                              && (input.Classify == null || policy.Classify == input.Classify)
                                                                              && (input.Type == null || policy.Type == input.Type)
                                                                              && (input.TradingProviderId == null || (policy.TradingProviderId == input.TradingProviderId || policy.TradingProviderId == null)))
                                                                .OrderByDescending(policy => policy.Id)
                                                                .Select(policy => policy);
            result.TotalItems = query.Count();
            //Sort
            query = query.OrderDynamic(input.Sort);

            //Phân trang
            if (input.PageSize != -1)
            {
                query = query.Skip(input.Skip).Take(input.PageSize);
            }

            result.Items = query.ToList();
            return result;
        }
    }
}
