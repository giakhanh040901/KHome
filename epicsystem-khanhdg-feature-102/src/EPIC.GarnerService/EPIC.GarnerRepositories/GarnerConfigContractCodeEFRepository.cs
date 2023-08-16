using EPIC.DataAccess.Base;
using EPIC.DataAccess.Models;
using EPIC.GarnerEntities.DataEntities;
using EPIC.GarnerEntities.Dto.GarnerDistributionConfigContractCode;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Shared;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.GarnerRepositories
{
    public class GarnerConfigContractCodeEFRepository : BaseEFRepository<GarnerConfigContractCode>
    {
        public GarnerConfigContractCodeEFRepository(EpicSchemaDbContext dbContext, ILogger logger) 
            : base(dbContext, logger, $"{DbSchemas.EPIC_GARNER}.{GarnerConfigContractCode.SEQ}")
        {
        }

        public GarnerConfigContractCode Add(GarnerConfigContractCode entity, int tradingProviderId, string username)
        {
            entity.Id = (int)NextKey();
            entity.TradingProviderId = tradingProviderId;
            entity.CreatedBy = username;
            return _dbSet.Add(entity).Entity;
        }

        public PagingResult<GarnerConfigContractCode> GetAll(FilterConfigContractCodeDto input, int? tradingProviderId = null)
        {
            PagingResult<GarnerConfigContractCode> result = new();
            IQueryable<GarnerConfigContractCode> query = _dbSet.Where(e => (tradingProviderId == null || e.TradingProviderId == tradingProviderId) && e.Deleted == YesNo.NO);

            // lọc theo trạng thái
            query = query.Where(e => input.Status == null || input.Status == e.Status);

            // lọc theo keyword
            query = query.Where(e => input.Keyword == null || e.Name.Contains(input.Keyword));

            result.TotalItems = query.Count();

            query = query.OrderByDescending(o => o.Id);

            if (input.PageSize != -1)
            {
                query = query.Skip(input.Skip).Take(input.PageSize);
            }

            result.Items = query.ToList();
            return result;
        }

        public List<GarnerConfigContractCode> GetAllStatusActive(int? tradingProviderId = null)
        {
            _logger.LogInformation($"{nameof(GetAllStatusActive)}: tradingproviderId = {tradingProviderId}");

            var result = _dbSet.Where(e => (tradingProviderId == null || e.TradingProviderId == tradingProviderId) && e.Status == Status.ACTIVE && e.Deleted == YesNo.NO);
            return result.ToList();
        }

        public GarnerConfigContractCode FindById(int configContractCodeId, int? tradingProviderId = null)
        {
            var result = _dbSet.FirstOrDefault(e => e.Id == configContractCodeId && 
                                    (tradingProviderId == null || e.TradingProviderId == tradingProviderId) && e.Deleted == YesNo.NO);
            return result;
        }

        /// <summary>
        /// Tìm cấu trúc mã không bao gồm loại khách hàng
        /// </summary>
        /// <param name="configContractCodeId"></param>
        /// <param name="tradingProviderId"></param>
        /// <returns></returns>
        public GarnerConfigContractCode FindByConfigContractCodeId(int configContractCodeId, int? tradingProviderId = null)
        {
            var result = _dbSet.FirstOrDefault(e => e.Id == configContractCodeId &&
                                    (tradingProviderId == null || e.TradingProviderId == tradingProviderId) && e.Deleted == YesNo.NO);
            return result;
        }
    }
}
