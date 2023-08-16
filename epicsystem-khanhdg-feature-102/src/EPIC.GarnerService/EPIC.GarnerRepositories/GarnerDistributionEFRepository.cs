using DocumentFormat.OpenXml.Wordprocessing;
using EPIC.DataAccess.Base;
using EPIC.DataAccess.Models;
using EPIC.Entities.DataEntities;
using EPIC.GarnerEntities.DataEntities;
using EPIC.GarnerEntities.Dto.GarnerDistribution;
using EPIC.GarnerEntities.Dto.GarnerProduct;
using EPIC.GarnerEntities.Dto.GarnerProductTradingProvider;
using EPIC.IdentityEntities.DataEntities;
using EPIC.InvestEntities.DataEntities;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Garner;
using EPIC.Utils.ConstantVariables.Identity;
using EPIC.Utils.ConstantVariables.Shared;
using EPIC.Utils.Linq;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.Arm;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace EPIC.GarnerRepositories
{
    public class GarnerDistributionEFRepository : BaseEFRepository<GarnerDistribution>
    {
        public GarnerDistributionEFRepository(EpicSchemaDbContext dbContext, ILogger logger) : base(dbContext, logger, $"{DbSchemas.EPIC_GARNER}.{GarnerDistribution.SEQ}")
        {
        }

        public GarnerDistribution Add(GarnerDistribution input, int tradingProviderId, string username)
        {
            _logger.LogInformation($"{nameof(Add)}: input = {JsonSerializer.Serialize(input)}, tradingProviderId = {tradingProviderId}, username = {username}");

            return _dbSet.Add(new GarnerDistribution
            {
                Id = (int)NextKey(),
                TradingProviderId = tradingProviderId,
                ProductId = input.ProductId,
                CloseCellDate = input.CloseCellDate,
                OpenCellDate = input.OpenCellDate,
                CreatedBy = username,
                Status = DistributionStatus.KHOI_TAO,
                IsDefault = YesNo.NO
            }).Entity;
        }

        public GarnerDistribution Update(GarnerDistribution input, int tradingProviderId, string username)
        {
            _logger.LogInformation($"{nameof(Update)}: input = {JsonSerializer.Serialize(input)}, tradingProviderId = {tradingProviderId}, username = {username}");
            var distributionFind = _dbSet.FirstOrDefault(d => d.Id == input.Id && d.TradingProviderId == tradingProviderId && d.Deleted == YesNo.NO);
            if (distributionFind != null)
            {
                distributionFind.OpenCellDate = input.OpenCellDate;
                distributionFind.CloseCellDate = input.CloseCellDate;
                distributionFind.Image = input.Image;
                if (input.IsShowApp != null)
                {
                    distributionFind.IsShowApp = input.IsShowApp;
                }
                distributionFind.ModifiedBy = username;
                distributionFind.ModifiedDate = DateTime.Now;
            }
            return distributionFind;
        }

        /// <summary>
        /// thay đổi IsClose của distribution
        /// </summary>
        /// <param name="id"></param>
        /// <param name="tradingProviderId"></param>
        /// <returns></returns>
        public GarnerDistribution DistributionClose(int id, int? tradingProviderId = null)
        {
            _logger.LogInformation($"{nameof(DistributionClose)}: Id = {id}, tradingProviderId = {tradingProviderId}");
            var distribution = _dbSet.FirstOrDefault(d => d.Id == id && (tradingProviderId == null || d.TradingProviderId == tradingProviderId) && d.Deleted == YesNo.NO);
            if (distribution.IsClose == YesNo.YES)
            {
                distribution.IsClose = YesNo.NO;
            }
            else if (distribution.IsClose == YesNo.NO)
            {
                distribution.IsClose = YesNo.YES;
            }
            return distribution;
        }

        public void Detele(int id, int tradingProviderId)
        {
            _logger.LogInformation($"{nameof(Detele)}: Id = {id}, TradingProviderId = {tradingProviderId}");

            var distributionFind = _dbSet.FirstOrDefault(d => d.Id == id && d.TradingProviderId == tradingProviderId && d.Deleted == YesNo.NO);
            if (distributionFind != null)
            {
                distributionFind.Deleted = YesNo.YES;
            }
        }

        public GarnerDistribution FindById(int id, int? tradingProviderId = null)
        {
            _logger.LogInformation($"{nameof(FindById)}: Id = {id}, tradingProviderId = {tradingProviderId}");

            return _dbSet.FirstOrDefault(d => d.Id == id && (tradingProviderId == null || d.TradingProviderId == tradingProviderId) && d.Deleted == YesNo.NO);
        }

        public IQueryable<GarnerDistribution> FindAllDistribution(GarnerDistributionFilterDto input, int? tradingProviderId = null)
        {
            _logger.LogInformation($"{nameof(FindAllDistribution)}: tradingProviderId = {tradingProviderId}");

            var query = _dbSet.Where(d => d.IsClose == YesNo.NO && d.Status == DistributionStatus.HOAT_DONG
                                && (d.OpenCellDate != null && d.OpenCellDate.Value.Date <= DateTime.Now.Date)
                                && (d.CloseCellDate != null && d.CloseCellDate.Value.Date >= DateTime.Now.Date)
                                && d.Deleted == YesNo.NO);

            if (tradingProviderId != null)
            {
                query = query.Where(o => o.TradingProviderId == tradingProviderId);
            }
            else
            {
                query = query.Where(o => input.TradingProviderIds != null && input.TradingProviderIds.Contains(o.TradingProviderId));
            }
            return query;
        }

        public bool CheckTradingHaveDistributionGarner(HashSet<int> tradingProviderIds)
        {
            _logger.LogInformation($"{nameof(CheckTradingHaveDistributionGarner)}: tradingProviderId = {tradingProviderIds}");

            return _dbSet.Where(d => tradingProviderIds.Contains(d.TradingProviderId)
                                && d.IsClose == YesNo.NO && d.Status == DistributionStatus.HOAT_DONG
                                && (d.OpenCellDate != null && d.OpenCellDate.Value.Date <= DateTime.Now.Date)
                                && (d.CloseCellDate != null && d.CloseCellDate.Value.Date >= DateTime.Now.Date)
                                && d.Deleted == YesNo.NO).Any();
        }

        public PagingResult<GarnerDistribution> FindAll(FilterGarnerDistributionDto input, int? tradingProviderId = null)
        {
            _logger.LogInformation($"{nameof(FindAll)}: input = {JsonSerializer.Serialize(input)}, tradingProviderId = {tradingProviderId}");

            PagingResult<GarnerDistribution> result = new();
            var garnerDistributionnQuery = from gd in _dbSet
                                           join gp in _epicSchemaDbContext.GarnerProducts
                                           on gd.ProductId equals gp.Id
                                           where (tradingProviderId == null || gd.TradingProviderId == tradingProviderId) && gp.Deleted == YesNo.NO && gd.Deleted == YesNo.NO
                                           && (input.Code == null || input.Code.ToLower().Contains(gp.Code.ToLower()))
                                           && (input.Name == null || input.Name.ToLower().Contains(gp.Name.ToLower()))
                                           && (input.Keyword == null || input.Keyword.ToLower().Contains(gp.Code.ToLower()) || input.Keyword.ToLower().Contains(gp.Name.ToLower()))
                                           && (input.Status == null || gd.Status == input.Status)
                                           && (input.IsClose == null || gd.IsClose == input.IsClose)
                                           orderby gp.Id descending
                                           select gd;

            garnerDistributionnQuery = garnerDistributionnQuery.OrderDynamic(input.Sort);
            result.TotalItems = garnerDistributionnQuery.Count();

            if (input.PageSize != -1)
            {
                garnerDistributionnQuery = garnerDistributionnQuery.Skip(input.Skip).Take(input.PageSize);
            }
            result.Items = garnerDistributionnQuery.ToList();
            return result;
        }

        public GarnerDistribution UpdateProductOverview(GarnerDistribution input, int tradingProviderId, string username)
        {
            _logger.LogInformation($"{nameof(UpdateProductOverview)}: input = {JsonSerializer.Serialize(input)}, tradingProviderId = {tradingProviderId}, username = {username}");

            var distributionFind = _dbSet.FirstOrDefault(d => d.Id == input.Id && d.TradingProviderId == tradingProviderId && d.Deleted == YesNo.NO);
            if (distributionFind != null)
            {
                distributionFind.ContentType = input.ContentType;
                distributionFind.OverviewContent = input.OverviewContent;
                distributionFind.OverviewImageUrl = input.OverviewImageUrl;
                distributionFind.ModifiedBy = username;
                distributionFind.ModifiedDate = DateTime.Now;
            }
            return distributionFind;
        }

        /// <summary>
        /// Lấy danh sách sản phẩm tích lũy, showApp
        /// </summary>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public IQueryable<AppGarnerDistributionDto> AppDistributionGetAll(string keyword)
        {
            _logger.LogInformation($"{nameof(AppDistributionGetAll)}: keyword = {keyword}");
            var result = (from distribution in _dbSet
                          join product in _epicSchemaDbContext.GarnerProducts on distribution.ProductId equals product.Id
                          join policy in _epicSchemaDbContext.GarnerPolicies on distribution.Id equals policy.DistributionId
                          join policyDetail in _epicSchemaDbContext.GarnerPolicyDetails on policy.Id equals policyDetail.PolicyId
                          join tradingProvider in _epicSchemaDbContext.TradingProviders on distribution.TradingProviderId equals tradingProvider.TradingProviderId
                          join businessCustomer in _epicSchemaDbContext.BusinessCustomers on tradingProvider.BusinessCustomerId equals businessCustomer.BusinessCustomerId
                          where (keyword == null || product.Name.ToLower().Contains(keyword.ToLower())
                              || policy.Code.ToLower().Contains(keyword.ToLower()) || policy.Name.ToLower().Contains(keyword.ToLower()))
                          && distribution.IsCheck == YesNo.YES && distribution.IsClose == YesNo.NO && distribution.Deleted == YesNo.NO && distribution.IsShowApp == YesNo.YES && distribution.Status == DistributionStatus.HOAT_DONG
                          && (distribution.OpenCellDate != null && distribution.OpenCellDate.Value.Date <= DateTime.Now.Date)
                          && (distribution.CloseCellDate != null && distribution.CloseCellDate.Value.Date >= DateTime.Now.Date)
                          && policy.Status == Status.ACTIVE && policy.Deleted == YesNo.NO && policy.IsShowApp == YesNo.YES
                          && policyDetail.IsShowApp == YesNo.YES && policyDetail.Status == Status.ACTIVE && policyDetail.Deleted == YesNo.NO
                          && tradingProvider.Deleted == YesNo.NO && businessCustomer.Deleted == YesNo.NO
                          group new { policyDetail } by new { distribution.Id, distribution.Image, product.ProductType, product.Name, product.Icon, PolicyId = policy.Id, TradingProviderName = businessCustomer.Name, tradingProvider.TradingProviderId } into g
                          select new AppGarnerDistributionDto
                          {
                              DistributionId = g.Key.Id,
                              ProductType = g.Key.ProductType,
                              ProductName = g.Key.Name,
                              PolicyId = g.Key.PolicyId,
                              Profit = g.Max(r => r.policyDetail.Profit),
                              Icon = g.Key.Icon,
                              TradingProviderName = g.Key.TradingProviderName,
                              TradingProviderId = g.Key.TradingProviderId,
                              Image = g.Key.Image,
                          });
            return result;
        }

        /// <summary>
        /// Lấy danh sách sản phẩm tích lũy
        /// </summary>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public IQueryable<AppGarnerDistributionDto> AppSearchDistribution(string keyword, List<int> tradingProviderIds)
        {
            _logger.LogInformation($"{nameof(AppDistributionGetAll)}: keyword = {keyword}");
            var policyDefault = new AppGarnerDistributionDto();
            var policyDefaultFind = new GarnerPolicy(); // Tìm chính sách mặc định được đại lý xét
            var distributionDefault = _epicSchemaDbContext.GarnerDistributions.FirstOrDefault(d => d.IsDefault == YesNo.YES && d.IsCheck == YesNo.YES && d.IsClose == YesNo.NO
                                                                    && d.Deleted == YesNo.NO && d.IsShowApp == YesNo.YES
                                                                    && d.Status == DistributionStatus.HOAT_DONG);

            // Tìm chính sách mặc định mà do đại lý cài cho sản phẩm phân phối
            if (distributionDefault != null)
            {
                policyDefaultFind = _epicSchemaDbContext.GarnerPolicies.FirstOrDefault(x => x.IsShowApp == YesNo.YES && x.Status == Utils.Status.ACTIVE && (x.TradingProviderId == distributionDefault.TradingProviderId && x.IsDefault == YesNo.YES) && x.Deleted == YesNo.NO);
            }
            
            var result = (from distribution in _dbSet
                          join product in _epicSchemaDbContext.GarnerProducts on distribution.ProductId equals product.Id
                          join policy in _epicSchemaDbContext.GarnerPolicies on distribution.Id equals policy.DistributionId
                          join policyDetail in _epicSchemaDbContext.GarnerPolicyDetails on policy.Id equals policyDetail.PolicyId
                          join tradingProvider in _epicSchemaDbContext.TradingProviders on distribution.TradingProviderId equals tradingProvider.TradingProviderId
                          join businessCustomer in _epicSchemaDbContext.BusinessCustomers on tradingProvider.BusinessCustomerId equals businessCustomer.BusinessCustomerId

                          where (keyword == null || product.Name.ToLower().Contains(keyword.ToLower())
                              || policy.Code.ToLower().Contains(keyword.ToLower()) || policy.Name.ToLower().Contains(keyword.ToLower()))
                          && (tradingProviderIds != null && tradingProviderIds.Contains(distribution.TradingProviderId))
                          && distribution.IsCheck == YesNo.YES && distribution.IsClose == YesNo.NO && distribution.Deleted == YesNo.NO && distribution.IsShowApp == YesNo.YES && distribution.Status == DistributionStatus.HOAT_DONG
                          && (distribution.OpenCellDate != null && distribution.OpenCellDate.Value.Date <= DateTime.Now.Date)
                          && (distribution.CloseCellDate != null && distribution.CloseCellDate.Value.Date >= DateTime.Now.Date)
                          && policy.Status == Status.ACTIVE && policy.Deleted == YesNo.NO && policy.IsShowApp == YesNo.YES
                          && policyDetail.IsShowApp == YesNo.YES && policyDetail.Status == Status.ACTIVE && policyDetail.Deleted == YesNo.NO
                          && tradingProvider.Deleted == YesNo.NO && businessCustomer.Deleted == YesNo.NO
                          group new { policyDetail } by new 
                          { 
                              distribution.Id, distribution.Image, product.ProductType, product.Name, 
                              product.Icon, PolicyId = policy.Id, PolicyName = policy.Name, policy.InterestType, policy.GarnerType,
                              policy.RepeatFixedDate, TradingProviderName = businessCustomer.Name, tradingProvider.TradingProviderId,
                              policy.CalculateType, policy.Classify, policy.IncomeTax, policy.InterestPeriodQuantity, policy.InterestPeriodType, 
                              policy.MinInvestDay, policy.MinMoney, policy.MinWithdraw, policy.OrderOfWithdrawal, policy.TransferAssetsFee, 
                              policy.WithdrawFee, policy.WithdrawFeeType, policy.MaxMoney, policy.MaxWithdraw, policy.InvestorType,
                          } into g
                          select new AppGarnerDistributionDto
                          {
                              DistributionId = g.Key.Id,
                              ProductType = g.Key.ProductType,
                              ProductName = g.Key.Name,
                              PolicyId = g.Key.PolicyId,
                              Profit = g.Max(r => r.policyDetail.Profit),
                              Icon = g.Key.Icon,
                              TradingProviderName = g.Key.TradingProviderName,
                              TradingProviderId = g.Key.TradingProviderId,
                              Image = g.Key.Image,
                              PolicyName = g.Key.PolicyName,
                              InterestTypeName = g.Key.GarnerType == GarnerPolicyTypes.KHONG_CHON_KY_HAN
                                        ? InterestTypes.InterestTypeNames(g.Key.InterestType, g.Key.RepeatFixedDate) : null,
                              IsDefault = YesNo.NO,
                              CalculateType = g.Key.CalculateType,
                              Classify = g.Key.Classify,
                              IncomeTax = g.Key.IncomeTax,
                              InterestPeriodQuantity = g.Key.InterestPeriodQuantity,
                              InterestPeriodType = g.Key.InterestPeriodType,
                              MinInvestDay = g.Key.MinInvestDay,
                              MinMoney = g.Key.MinMoney,
                              MinWithdraw = g.Key.MinWithdraw,
                              OrderOfWithdrawal = g.Key.OrderOfWithdrawal,
                              TransferAssetsFee = g.Key.TransferAssetsFee,
                              WithdrawFee = g.Key.WithdrawFee,
                              WithdrawFeeType = g.Key.WithdrawFeeType,
                              GarnerType = g.Key.GarnerType,
                              RepeatFixedDate = g.Key.RepeatFixedDate,
                              InterestType= g.Key.InterestType,
                              InvestorType= g.Key.InvestorType,
                              ProductTypeName = DictionaryNames.ProductTypeName(g.Key.ProductType)
                          });

            // Xét chính sách nổi bật
            // Nếu đại lý mặc định có trong danh sách đại lý lọc
            if (distributionDefault != null && result.Select(d => d.TradingProviderId).Contains(distributionDefault.TradingProviderId))
            {
                //Nếu chính sách mặc định của đại lý mặc định có trong danh sách bảng hàng
                if (policyDefaultFind != null && result.Select(d => d.PolicyId).Contains(policyDefaultFind.Id))
                {
                    policyDefault = result.FirstOrDefault(d => d.PolicyId == policyDefaultFind.Id);
                }
                // Nếu không thì lấy chính sách đầu tiên trong bảng hàng của đại lý mặc định
                else
                {
                    policyDefault = result.FirstOrDefault(d => d.TradingProviderId == distributionDefault.TradingProviderId);
                }
            }
            foreach (var item in result)
            {
                //Lấy chính sách mặc định nếu là khách hàng là sale hoặc có tư vấn viên mặc định
                if (policyDefault != null && policyDefault.PolicyId == item.PolicyId)
                {
                    item.IsDefault = YesNo.YES;
                }
            }
            return result;
        }
    }
}
