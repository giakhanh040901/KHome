using EPIC.DataAccess.Base;
using EPIC.DataAccess.Models;
using EPIC.GarnerEntities.DataEntities;
using EPIC.GarnerEntities.Dto.GarnerPolicyTemp;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Shared;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace EPIC.GarnerRepositories
{
    public class GarnerPolicyTempEFRepository : BaseEFRepository<GarnerPolicyTemp>
    {
        public GarnerPolicyTempEFRepository(EpicSchemaDbContext dbContext, ILogger logger) : base(dbContext, logger, $"{DbSchemas.EPIC_GARNER}.{GarnerPolicyTemp.SEQ}")
        {
        }

        /// <summary>
        /// Thêm chính sách mẫu
        /// </summary>
        /// <param name="input"></param>
        /// <param name="username"></param>
        /// <param name="tradingProviderId"></param>
        /// <returns></returns>
        public GarnerPolicyTemp Add(GarnerPolicyTemp input, string username, int tradingProviderId)
        {
            _logger.LogInformation($"{nameof(Add)}: input = {JsonSerializer.Serialize(input)}, username = {username}, tradingProviderId = {tradingProviderId}");

            return _dbSet.Add(new GarnerPolicyTemp
            {
                Id = (int)NextKey(),
                Name = input.Name,
                Code = input.Code,
                IsTransferAssets = input.IsTransferAssets,
                TransferAssetsFee = input.TransferAssetsFee,
                CalculateType = input.CalculateType,
                IncomeTax = input.IncomeTax,
                GarnerType = input.GarnerType,
                InvestorType = input.InvestorType,
                MinMoney = input.MinMoney,
                MaxMoney = input.MaxMoney,
                MinInvestDay = input.MinInvestDay,
                MinWithdraw = input.MinWithdraw,
                MaxWithdraw = input.MaxWithdraw,
                InterestPeriodQuantity = input.InterestPeriodQuantity,
                InterestPeriodType = input.InterestPeriodType,
                InterestType = input.InterestType,
                RepeatFixedDate = input.RepeatFixedDate,
                WithdrawFeeType = input.WithdrawFeeType,
                Classify = input.Classify,
                OrderOfWithdrawal = input.OrderOfWithdrawal,
                WithdrawFee = input.WithdrawFee,
                Description = input.Description,
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
        public GarnerPolicyTemp Update(GarnerPolicyTemp input, string username, int? tradingProviderId = null)
        {
            _logger.LogInformation($"{nameof(Update)}: input = {JsonSerializer.Serialize(input)}, username = {username}");
            var policyTemp = _dbSet.FirstOrDefault(p => p.Id == input.Id && (tradingProviderId == null || p.TradingProviderId == tradingProviderId) && p.Deleted == YesNo.NO);
            if (policyTemp != null)
            {
                policyTemp.Name = input.Name;
                policyTemp.Code = input.Code;
                policyTemp.IsTransferAssets = input.IsTransferAssets;
                policyTemp.TransferAssetsFee = input.TransferAssetsFee; 
                policyTemp.CalculateType = input.CalculateType;
                policyTemp.IncomeTax = input.IncomeTax;
                policyTemp.GarnerType = input.GarnerType;
                policyTemp.InvestorType = input.InvestorType;
                policyTemp.MinMoney = input.MinMoney;
                policyTemp.MaxMoney = input.MaxMoney;
                policyTemp.MinInvestDay = input.MinInvestDay;
                policyTemp.MinWithdraw = input.MinWithdraw;
                policyTemp.MaxWithdraw = input.MaxWithdraw;
                policyTemp.InterestPeriodQuantity = input.InterestPeriodQuantity;
                policyTemp.InterestPeriodType = input.InterestPeriodType;
                policyTemp.InterestType = input.InterestType;
                policyTemp.RepeatFixedDate = input.RepeatFixedDate;
                policyTemp.WithdrawFeeType = input.WithdrawFeeType;
                policyTemp.Classify = input.Classify;
                policyTemp.OrderOfWithdrawal = input.OrderOfWithdrawal;
                policyTemp.WithdrawFee = input.WithdrawFee;
                policyTemp.ModifiedBy = username;
                policyTemp.ModifiedDate = DateTime.Now;
            }
            return policyTemp;
        }

        public GarnerPolicyTemp FindById(int id, int? tradingProviderId = null)
        {
            _logger.LogInformation($"{nameof(FindById)}: Id = {id}, tradingproviderId = {tradingProviderId}");

            var policyTemp = _dbSet.FirstOrDefault(d => d.Id == id && (tradingProviderId == null || d.TradingProviderId == tradingProviderId) && d.Deleted == YesNo.NO);
            return policyTemp;
        }

        public PagingResult<GarnerPolicyTemp> FindAll(FilterPolicyTempDto input, int? tradingProviderId = null)
        {
            _logger.LogInformation($"{nameof(FindAll)}: input = {JsonSerializer.Serialize(input)}, tradingproviderId = {tradingProviderId}");

            PagingResult<GarnerPolicyTemp> result = new();
            var policyTempQuery = _dbSet.Where(pt => (tradingProviderId == null || pt.TradingProviderId == tradingProviderId) && pt.Deleted == YesNo.NO && pt.Deleted == YesNo.NO
                                           && (input.Code == null || input.Code.ToLower().Contains(pt.Code.ToLower()))
                                           && (input.Name == null || input.Name.ToLower().Contains(pt.Name.ToLower()))
                                           && (input.Status == null || pt.Status == input.Status))
                                        .OrderByDescending(e => e.Id)
                                        .AsQueryable();
            //OrderByDescending(pt => pt.Id);

            if (input.PageSize != -1)
            {
                policyTempQuery = policyTempQuery.Skip(input.Skip).Take(input.PageSize);
            }

            if (input.Keyword != null)
            {
                policyTempQuery = policyTempQuery.Where(p => p.Name.Contains(input.Keyword) || p.Code.Contains(input.Keyword));
            }

            result.TotalItems = policyTempQuery.Count();
            result.Items = policyTempQuery.ToList();
            return result;
        }

        public List<GarnerPolicyTemp> FindAllNoPermission(int? tradingProviderId = null, string status = null)
        {
            _logger.LogInformation($"{nameof(FindAllNoPermission)}, tradingproviderId = {tradingProviderId}");

            List<GarnerPolicyTemp> result = new();
            result = _dbSet.Where(pt => (tradingProviderId == null || pt.TradingProviderId == tradingProviderId) && (status == null || status == pt.Status) && pt.Deleted == YesNo.NO && pt.Deleted == YesNo.NO).ToList();
            //OrderByDescending(pt => pt.Id);
            return result;
        }
    }
}
