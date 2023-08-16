using EPIC.DataAccess.Base;
using EPIC.DataAccess.Models;
using EPIC.GarnerEntities.DataEntities;
using EPIC.GarnerEntities.Dto.GarnerDistribution;
using EPIC.GarnerEntities.Dto.GarnerPolicy;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Shared;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace EPIC.GarnerRepositories
{
    public class GarnerPolicyEFRepository : BaseEFRepository<GarnerPolicy>
    {
        public GarnerPolicyEFRepository(EpicSchemaDbContext dbContext, ILogger logger) : base(dbContext, logger, $"{DbSchemas.EPIC_GARNER}.{GarnerPolicy.SEQ}")
        {
        }

        /// <summary>
        /// Thêm chính sách vào phân phối sản phẩm
        /// </summary>
        /// <param name="input"></param>
        public GarnerPolicy Add(GarnerPolicy input, int tradingProviderId, string username)
        {
            _logger.LogInformation($"{nameof(Add)}: {JsonSerializer.Serialize(input)}, tradingProviderId = {tradingProviderId}, username = {username}");

            var policyInsert = new GarnerPolicy()
            {
                Id = (int)NextKey(),
                TradingProviderId = tradingProviderId,
                DistributionId = input.DistributionId,
                Code = input.Code,
                Name = input.Name,
                Status = Status.ACTIVE,
                MinMoney = input.MinMoney,
                MaxMoney = input.MaxMoney,
                MinInvestDay = input.MinInvestDay,
                IncomeTax = input.IncomeTax,
                InvestorType = input.InvestorType,
                InterestPeriodQuantity = input.InterestPeriodQuantity,
                InterestPeriodType  = input.InterestPeriodType,
                InterestType = input.InterestType,
                Classify = input.Classify,
                RepeatFixedDate = input.RepeatFixedDate,
                CalculateType = input.CalculateType,
                GarnerType = input.GarnerType,
                MinWithdraw = input.MinWithdraw,
                MaxWithdraw = input.MaxWithdraw,
                WithdrawFee = input.WithdrawFee,
                OrderOfWithdrawal = input.OrderOfWithdrawal,
                IsTransferAssets = input.IsTransferAssets,
                TransferAssetsFee = input.TransferAssetsFee,
                IsShowApp = input.IsShowApp,
                StartDate = input.StartDate,
                EndDate = input.EndDate,
                SortOrder = input.SortOrder,
                Description = input.Description,
                CreatedBy = username
            };
            return _dbSet.Add(policyInsert).Entity;
        }

        /// <summary>
        /// Update chính sách phân phối sản phẩm
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public GarnerPolicy Update(GarnerPolicy input, int tradingProviderId, string username)
        {
            _logger.LogInformation($"{nameof(Update)}: input = {JsonSerializer.Serialize(input)}, tradingProviderId = {tradingProviderId}, username = {username}");

            var getPolicy = _dbSet.FirstOrDefault(p => p.Id == input.Id && p.TradingProviderId == tradingProviderId && p.Deleted == YesNo.NO);
            if (getPolicy != null)
            {
                getPolicy.Code = input.Code;
                getPolicy.Name = input.Name;
                getPolicy.MinMoney = input.MinMoney;
                getPolicy.MaxMoney = input.MaxMoney;
                getPolicy.MinInvestDay = input.MinInvestDay;
                getPolicy.IncomeTax = input.IncomeTax;
                //getPolicy.Status = input.Status;
                getPolicy.InvestorType = input.InvestorType;
                getPolicy.Classify = input.Classify;
                getPolicy.CalculateType = input.CalculateType;
                getPolicy.GarnerType = input.GarnerType;
                getPolicy.MinWithdraw = input.MinWithdraw;
                getPolicy.MaxWithdraw = input.MaxWithdraw;
                getPolicy.WithdrawFee = input.WithdrawFee;
                getPolicy.WithdrawFeeType = input.WithdrawFeeType;
                getPolicy.OrderOfWithdrawal = input.OrderOfWithdrawal;
                getPolicy.IsTransferAssets = input.IsTransferAssets;
                getPolicy.TransferAssetsFee = input.TransferAssetsFee;
                getPolicy.InterestType = input.InterestType;
                getPolicy.InterestPeriodType = input.InterestPeriodType;
                getPolicy.InterestPeriodQuantity = input.InterestPeriodQuantity;
                getPolicy.RepeatFixedDate = input.RepeatFixedDate;
                getPolicy.IsShowApp = input.IsShowApp;
                getPolicy.StartDate = input.StartDate;
                getPolicy.EndDate = input.EndDate;
                getPolicy.Description = input.Description;
                getPolicy.SortOrder = input.SortOrder;
                getPolicy.ModifiedBy = username;
                getPolicy.ModifiedDate = DateTime.Now;
            }
            return getPolicy;
        }

        /// <summary>
        /// Lấy ra chính sách theo Id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="tradingProviderId"></param>
        /// <returns></returns>
        public GarnerPolicy FindById(int policyId, int? tradingProviderId = null)
        {
            _logger.LogInformation($"{nameof(FindById)}: policyId = {policyId}");

            return _dbSet.FirstOrDefault(x => x.Id == policyId && (tradingProviderId == null || x.TradingProviderId == tradingProviderId));
        }

        /// <summary>
        /// Tìm chính sách mặc định, nếu không truyền trading lấy mặc định do EPIC cài
        /// Nếu truyền đại lý lấy chính sách mặc định do đại lý cài
        /// </summary>
        /// <param name="tradingProviderId"></param>
        /// <returns></returns>
        public GarnerPolicy FindPolicyDefault(int? tradingProviderId = null)
        {
            _logger.LogInformation($"{nameof(FindPolicyDefault)}: tradingProviderId = {tradingProviderId}");

            return _dbSet.FirstOrDefault(x => x.IsShowApp == YesNo.YES && x.Status == Utils.Status.ACTIVE && ((tradingProviderId == null && x.IsDefaultEpic == YesNo.YES) || (x.TradingProviderId == tradingProviderId && x.IsDefault == YesNo.YES)) && x.Deleted == YesNo.NO);
        }

        public PagingResult<GarnerPolicy> FindAll(FilterPolicyDto input, int? tradingProviderId = null)
        {
            _logger.LogInformation($"{nameof(FindAll)}: input = {JsonSerializer.Serialize(input)}, tradingproviderId = {tradingProviderId}");

            PagingResult<GarnerPolicy> result = new();
            var policyQuery = _dbSet.Where(pt => (tradingProviderId == null || pt.TradingProviderId == tradingProviderId) && pt.Deleted == YesNo.NO && pt.Deleted == YesNo.NO
                                           && (input.Code == null || input.Code.ToLower().Contains(pt.Code.ToLower()))
                                           && (input.Name == null || input.Name.ToLower().Contains(pt.Name.ToLower())));
            //OrderByDescending(pt => pt.Id);

            if (input.PageSize != -1)
            {
                policyQuery = policyQuery.Skip(input.Skip).Take(input.PageSize);
            }

            result.TotalItems = policyQuery.Count();
            result.Items = policyQuery.ToList();
            return result;
        }

        public List<GarnerPolicy> GetAllPolicyByDistributionId(int distributionId, GarnerDistributionFilterDto input, int? tradingProviderId = null)
        {
            _logger.LogInformation($"{nameof(GetAllPolicyByDistributionId)}: distributionId = {distributionId}, tradingproviderId = {tradingProviderId}");

            var policyQuery = _dbSet.Where(gp => gp.DistributionId == distributionId && (input.Status == null || input.Status == gp.Status)&& gp.Deleted == YesNo.NO);
            if (tradingProviderId != null)
            {
                policyQuery = policyQuery.Where(o => o.TradingProviderId == tradingProviderId);
            }
            else
            {
                policyQuery = policyQuery.Where(o => input.TradingProviderIds != null && input.TradingProviderIds.Contains(o.TradingProviderId));
            }
            return policyQuery.OrderBy(o => o.Id).ToList();
        }

        public GarnerPolicy PolicyIsShowApp(int policyId, string isShowApp, int? tradingProviderId)
        {
            _logger.LogInformation($"{nameof(PolicyIsShowApp)}: policyId = {policyId}, isShowApp = {isShowApp}, tradingProviderId = {tradingProviderId}");
            var policyFind = _dbSet.FirstOrDefault(p => p.Id == policyId && p.TradingProviderId == tradingProviderId && p.Deleted == YesNo.NO);
            if (policyFind != null)
            {
                policyFind.IsShowApp = isShowApp;
            }
            return policyFind;
        }
    }
}
