using EPIC.DataAccess.Base;
using EPIC.DataAccess.Models;
using EPIC.GarnerEntities.DataEntities;
using EPIC.InvestEntities.DataEntities;
using EPIC.InvestEntities.Dto.InvConfigContractCode;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Shared;
using EPIC.Utils.Linq;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.InvestRepositories
{
    public class InvConfigContractCodeEFRepository : BaseEFRepository<InvestConfigContractCode>
    {
        public InvConfigContractCodeEFRepository(EpicSchemaDbContext dbContext, ILogger logger)
            : base(dbContext, logger, $"{DbSchemas.EPIC}.{InvestConfigContractCode.SEQ}")
        {
        }

        public InvestConfigContractCode Add(InvestConfigContractCode entity, int tradingProviderId, string username)
        {
            entity.Id = (int)NextKey();
            entity.TradingProviderId = tradingProviderId;
            entity.CreatedBy = username;
            return _dbSet.Add(entity).Entity;
        }

        public PagingResult<InvestConfigContractCode> GetAll(FilterInvConfigContractCodeDto input, int? tradingProviderId = null)
        {
            PagingResult<InvestConfigContractCode> result = new();
            IQueryable<InvestConfigContractCode> query = _dbSet.Where(e => (tradingProviderId == null || e.TradingProviderId == tradingProviderId) && e.Deleted == YesNo.NO).OrderByDescending(e => e.Id);
            result.TotalItems = query.Count();
            query = query.OrderDynamic(input.Sort);
            if (input.PageSize != -1)
            {
                query = query.Skip(input.Skip).Take(input.PageSize);
            }

            result.Items = query.ToList();
            return result;
        }

        public List<InvestConfigContractCode> GetAllStatusActive(int? tradingProviderId = null)
        {
            _logger.LogInformation($"{nameof(GetAllStatusActive)}: tradingproviderId = {tradingProviderId}");

            var result = _dbSet.Where(e => (tradingProviderId == null || e.TradingProviderId == tradingProviderId) && e.Status == Status.ACTIVE && e.Deleted == YesNo.NO);
            return result.ToList();
        }

        public InvestConfigContractCode FindById(int configContractCodeId, int? tradingProviderId = null)
        {
            var result = _dbSet.FirstOrDefault(e => e.Id == configContractCodeId && (tradingProviderId == null || e.TradingProviderId == tradingProviderId) && e.Deleted == YesNo.NO);
            return result;
        }

        public InvestConfigContractCode FindByConfigContractCodeId(int configContractCodeId, int? tradingProviderId = null)
        {
            var result = _dbSet.FirstOrDefault(e => e.Id == configContractCodeId &&
                                    (tradingProviderId == null || e.TradingProviderId == tradingProviderId) && e.Deleted == YesNo.NO);
            return result;
        }
    }
}