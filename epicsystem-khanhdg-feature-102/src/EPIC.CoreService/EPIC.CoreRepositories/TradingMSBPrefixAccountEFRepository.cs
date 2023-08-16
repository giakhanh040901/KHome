using EPIC.CoreEntities.DataEntities;
using EPIC.CoreEntities.Dto.TradingMSBPrefixAccount;
using EPIC.DataAccess.Base;
using EPIC.DataAccess.Models;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace EPIC.CoreRepositories
{
    public class TradingMSBPrefixAccountEFRepository : BaseEFRepository<TradingMsbPrefixAccount>
    {
        public TradingMSBPrefixAccountEFRepository(EpicSchemaDbContext dbContext, ILogger logger) : base(dbContext, logger, $"{DbSchemas.EPIC}.{TradingMsbPrefixAccount.SEQ}")
        {
        }

        public TradingMsbPrefixAccount FindById(int id, int? tradingProviderId = null)
        {
            _logger.LogInformation($"{nameof(TradingMSBPrefixAccountEFRepository)}->{nameof(FindById)}: Id = {id}, tradingProviderId = {tradingProviderId}");
            return _dbSet.FirstOrDefault(t => t.Id == id && (tradingProviderId == null || t.TradingProviderId == tradingProviderId) && t.Deleted == YesNo.NO);
        }

        public TradingMsbPrefixAccount FindByTradingBankId(int tradingBankAccId, int? tradingProviderId = null)
        {
            _logger.LogInformation($"{nameof(TradingMSBPrefixAccountEFRepository)}->{nameof(FindByTradingBankId)}: tradingBankAccId = {tradingBankAccId}, tradingProviderId = {tradingProviderId}");
            return _dbSet.FirstOrDefault(t => t.TradingBankAccountId == tradingBankAccId && (tradingProviderId == null || t.TradingProviderId == tradingProviderId) && t.Deleted == YesNo.NO);
        }

        public TradingMsbPrefixAccount Add(TradingMsbPrefixAccount input, int tradingProviderId, string username)
        {
            _logger.LogInformation($"{nameof(TradingMSBPrefixAccountEFRepository)}->{nameof(Add)}: input = {JsonSerializer.Serialize(input)}, tradingProviderId = {tradingProviderId}, username = {username}");
            var tradingMSBPrefixAccount = new TradingMsbPrefixAccount()
            {
                Id = (int)NextKey(),
                TradingProviderId = tradingProviderId,
                TradingBankAccountId = input.TradingBankAccountId,
                PrefixMsb = input.PrefixMsb,
                MId = input.MId,
                TId = input.TId,
                AccessCode = input.AccessCode,
                TIdWithoutOtp = input.TIdWithoutOtp,
                CreatedBy = username,
            };
            return _dbSet.Add(tradingMSBPrefixAccount).Entity;
        }

        public PagingResult<TradingMsbPrefixAccount> FindAll(FilterTradingMsbPrefixAccountDto input, int? tradingProviderId = null)
        {
            _logger.LogInformation($"{nameof(TradingMSBPrefixAccountEFRepository)}->{nameof(FindAll)}: input = {JsonSerializer.Serialize(input)}, tradingProviderId = {tradingProviderId}");
            PagingResult<TradingMsbPrefixAccount> result = new();
            var tradingMSBQuery = _dbSet.Where(t => t.Deleted == YesNo.NO 
                                        && (input.PrefixMsb == null || input.PrefixMsb == t.PrefixMsb));

            if (tradingProviderId != null)
            {
                tradingMSBQuery = tradingMSBQuery.Where(t => t.TradingProviderId == tradingProviderId);
            }
            if (input.PageSize != -1 && input.PageSize > 0)
            {
                tradingMSBQuery = tradingMSBQuery.Skip(input.Skip).Take(input.PageSize);
            }

            result.TotalItems = tradingMSBQuery.Count();
            result.Items = tradingMSBQuery.ToList();
            return result;
        }

        /// <summary>
        /// Lấy FirstOrDeFauld TradingMsbPrefixAccount
        /// </summary>
        /// <param name="prefixMsb"></param>
        /// <param name="tradingProviderId"></param>
        /// <returns></returns>
        public TradingMsbPrefixAccount FindOne(string prefixMsb, int tradingProviderId)
        {
            _logger.LogInformation($"{nameof(TradingMSBPrefixAccountEFRepository)}->{nameof(FindOne)}: prefixMsb = {prefixMsb}, tradingProviderId = {tradingProviderId}");

            return _dbSet.FirstOrDefault(t => t.TradingProviderId == tradingProviderId && t.PrefixMsb == prefixMsb && t.Deleted == YesNo.NO);
        }
    }
}
