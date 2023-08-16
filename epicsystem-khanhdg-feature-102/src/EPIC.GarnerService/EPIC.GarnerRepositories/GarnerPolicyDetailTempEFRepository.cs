using EPIC.DataAccess.Base;
using EPIC.DataAccess.Models;
using EPIC.GarnerEntities.DataEntities;
using EPIC.GarnerEntities.Dto.GarnerPolicyDetailTemp;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Shared;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace EPIC.GarnerRepositories
{
    public class GarnerPolicyDetailTempEFRepository : BaseEFRepository<GarnerPolicyDetailTemp>
    {
        public GarnerPolicyDetailTempEFRepository(EpicSchemaDbContext dbContext, ILogger logger) : base(dbContext, logger, $"{DbSchemas.EPIC_GARNER}.{GarnerPolicyDetailTemp.SEQ}")
        {
        }

        /// <summary>
        /// Thêm chính sách mẫu
        /// </summary>
        /// <param name="input"></param>
        /// <param name="username"></param>
        /// <param name="tradingProviderId"></param>
        /// <returns></returns>
        public GarnerPolicyDetailTemp Add(GarnerPolicyDetailTemp input, string username, int tradingProviderId)
        {
            _logger.LogInformation($"{nameof(Add)}: input = {JsonSerializer.Serialize(input)}, username = {username}, tradingProviderId = {tradingProviderId}");

            return _dbSet.Add(new GarnerPolicyDetailTemp
            {
                Id = (int)NextKey(),
                Name = input.Name,
                InterestDays = input.InterestDays,
                InterestType = input.InterestType,
                InterestPeriodQuantity = input.InterestPeriodQuantity,
                InterestPeriodType = input.InterestPeriodType,
                PeriodType = input.PeriodType,
                PeriodQuantity = input.PeriodQuantity,
                PolicyTempId = input.PolicyTempId,
                Profit = input.Profit,
                RepeatFixedDate = input.RepeatFixedDate,
                ShortName = input.ShortName,
                SortOrder = input.SortOrder,
                TradingProviderId = tradingProviderId,
                CreatedBy = username
            }).Entity;
        }

        /// <summary>
        /// Cập nhật chính sách mẫu
        /// </summary>
        /// <param name="input"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        public GarnerPolicyDetailTemp Update(GarnerPolicyDetailTemp input, string username)
        {
            _logger.LogInformation($"{nameof(Update)}: input = {JsonSerializer.Serialize(input)}, username = {username}");
            var policyDetailTemp = _dbSet.FirstOrDefault(p => p.Id == input.Id && p.TradingProviderId == input.TradingProviderId && p.Deleted == YesNo.NO);
            if (policyDetailTemp != null)
            {
                policyDetailTemp.Name = input.Name;
                policyDetailTemp.InterestDays = input.InterestDays;
                policyDetailTemp.InterestType = input.InterestType;
                policyDetailTemp.InterestPeriodQuantity = input.InterestPeriodQuantity;
                policyDetailTemp.InterestPeriodType = input.InterestPeriodType;
                policyDetailTemp.PeriodType = input.PeriodType;
                policyDetailTemp.PeriodQuantity = input.PeriodQuantity;
                policyDetailTemp.PolicyTempId = input.PolicyTempId;
                policyDetailTemp.Profit = input.Profit;
                policyDetailTemp.RepeatFixedDate = input.RepeatFixedDate;
                policyDetailTemp.ShortName = input.ShortName;
                policyDetailTemp.SortOrder = input.SortOrder;
                policyDetailTemp.ModifiedBy = username;
                policyDetailTemp.ModifiedDate = DateTime.Now;
            }
            return policyDetailTemp;
        }

        public GarnerPolicyDetailTemp FindById(int id, int? tradingProviderId = null)
        {
            _logger.LogInformation($"{nameof(FindById)}: Id = {id}, tradingproviderId = {tradingProviderId}");

            return _dbSet.FirstOrDefault(d => d.Id == id && (tradingProviderId == null || d.TradingProviderId == tradingProviderId) && d.Deleted == YesNo.NO);
        }

        public List<GarnerPolicyDetailTemp> FindAll(int policyTempId, int? tradingProviderId = null)
        {
            _logger.LogInformation($"{nameof(FindAll)}: policyTempId = {policyTempId}, tradingproviderId = {tradingProviderId}");
            var policyDetailTemps = _dbSet.Where(pt => (tradingProviderId == null || pt.TradingProviderId == tradingProviderId) && pt.Deleted == YesNo.NO 
                                           && pt.PolicyTempId == policyTempId).ToList();  
            return policyDetailTemps;
        }
    }
}
