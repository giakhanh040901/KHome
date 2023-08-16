using EPIC.DataAccess.Base;
using EPIC.DataAccess.Models;
using EPIC.Entities.DataEntities;
using EPIC.InvestEntities.DataEntities;
using EPIC.InvestEntities.Dto.Distribution;
using EPIC.InvestEntities.Dto.InvestProject;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Shared;
using EPIC.Utils.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace EPIC.InvestRepositories
{
    public class DistributionEFRepository : BaseEFRepository<Distribution>
    {
        public DistributionEFRepository(EpicSchemaDbContext dbContext, ILogger logger) : base(dbContext, logger, $"{DbSchemas.EPIC}.{Distribution.SEQ}")
        {
        }

        public IQueryable<ViewDistributionDto> FindAll(FilterInvestDistributionDto input) 
        {
            _logger.LogInformation($"{nameof(DistributionEFRepository)}->{nameof(FindAll)}: input = {JsonSerializer.Serialize(input)}");

            var orderQuery = _epicSchemaDbContext.InvOrders.Include(order => order.Distribution).ThenInclude(d => d.Project).Where(order => (order.Status == OrderStatus.CHO_DUYET_HOP_DONG || order.Status == OrderStatus.DANG_DAU_TU)
                                                                                                     && order.Deleted == YesNo.NO && order.Distribution.Deleted == YesNo.NO);

            var query = _epicSchemaDbContext.InvestDistributions.Include(d => d.Project).ThenInclude(p => p.ProjectTradingProviders)
                                                                .Include(d => d.Project).ThenInclude(p => p.Owner).ThenInclude(o => o.BusinessCustomer)
                                                                .Include(d => d.Project).ThenInclude(p => p.GeneralContractor).ThenInclude(gc => gc.BusinessCustomer)
                                                                .Include(d => d.Policies)
                                                                .Include(d => d.PolicyDetails)
                                                                .Include(d => d.Orders)
                                                                .Where(d => d.Deleted == YesNo.NO
                                                                    && ((input.TradingProviderId == null) || (d.TradingProviderId == input.TradingProviderId && d.Project.ProjectTradingProviders.Where(ptd => ptd.TradingProviderId == input.TradingProviderId && ptd.Deleted == YesNo.NO).Any()))
                                                                    && ((input.Keyword == null) || (d.Project.InvName != null && d.Project.InvName.ToLower().Contains(input.Keyword.ToLower())) || (d.Project.InvCode != null && d.Project.InvCode.ToLower().Contains(input.Keyword.ToLower())))
                                                                    && (input.Status == null || d.Status == input.Status)
                                                                    && (input.IsClose == null || d.IsClose == input.IsClose))
                                                                .Select(d => new ViewDistributionDto
                                                                {
                                                                    Id = d.Id,
                                                                    ProjectId = d.ProjectId,
                                                                    TradingProviderId = d.TradingProviderId,
                                                                    BusinessCustomerBankAccId = d.BusinessCustomerBankAccId,
                                                                    Status = d.Status,
                                                                    OpenCellDate = d.OpenCellDate,
                                                                    CloseCellDate = d.CloseCellDate,
                                                                    ContentType = d.ContentType,
                                                                    OverviewContent = d.OverviewContent,
                                                                    OverviewImageUrl = d.OverviewImageUrl,
                                                                    IsClose = d.IsClose,
                                                                    IsCheck = d.IsCheck,
                                                                    IsShowApp = d.IsShowApp,
                                                                    InvName = d.Project.InvName,
                                                                    Project = new InvestEntities.Dto.Project.ProjectDto
                                                                    {
                                                                        Id = d.Project.Id,
                                                                        PartnerId = d.Project.PartnerId,
                                                                        OwnerId = d.Project.OwnerId,
                                                                        GeneralContractorId = d.Project.GeneralContractorId,
                                                                        InvCode = d.Project.InvCode,
                                                                        InvName = d.Project.InvName,
                                                                        Content = d.Project.Content,
                                                                        TotalInvestment = d.Project.TotalInvestment,
                                                                        TotalInvestmentDisplay = d.Project.TotalInvestmentDisplay,
                                                                    },
                                                                    HanMucToiDa = d.Project.ProjectTradingProviders.FirstOrDefault() == null ? 0 
                                                                            : (d.Project.ProjectTradingProviders.FirstOrDefault().TotalInvestmentSub != null
                                                                                ? (d.Project.ProjectTradingProviders.FirstOrDefault().TotalInvestmentSub - orderQuery.Where(order => order.DistributionId == d.Id).Sum(order => order.TotalValue))
                                                                                : (d.Project.TotalInvestment - orderQuery.Where(o => o.Distribution.ProjectId == d.ProjectId).Sum(order => order.TotalValue))),
                                                                    IsInvested = orderQuery.Where(o => o.Distribution.ProjectId == d.ProjectId).Sum(order => order.TotalValue)
                                                                });
            return query;
        }


        private decimal MaxTotalInvestment(int distributionId, int? tradingProviderId = null)
        {
            var distributionFind = _epicSchemaDbContext.InvestDistributions.FirstOrDefault(d => d.Id == distributionId && d.Deleted == YesNo.NO);
            if (distributionFind == null)
            {
                return 0;
            }
            var projectFind = _epicSchemaDbContext.InvestProjects.FirstOrDefault(p => p.Id == distributionFind.ProjectId && p.Deleted == YesNo.NO);
            if (projectFind == null)
            {
                return 0;
            }
            var totalInvestment = (from project in _epicSchemaDbContext.InvestProjects
                                   join projectTrading in _epicSchemaDbContext.InvestProjectTradingProviders on project.Id equals projectTrading.ProjectId
                                   where project.Deleted == YesNo.NO && projectTrading.Deleted == YesNo.NO
                                   && project.Id == projectFind.Id && projectTrading.Deleted == YesNo.NO
                                   select project.TotalInvestment).FirstOrDefault();

            var totalInvestmentSub = (from project in _epicSchemaDbContext.InvestProjects
                                      join projectTrading in _epicSchemaDbContext.InvestProjectTradingProviders on project.Id equals projectTrading.ProjectId
                                      where project.Deleted == YesNo.NO && projectTrading.Deleted == YesNo.NO
                                      && project.Id == projectFind.Id && projectTrading.Deleted == YesNo.NO
                                      select projectTrading.TotalInvestmentSub).FirstOrDefault();

            if (totalInvestment == null)
            {
                totalInvestment = totalInvestmentSub ?? 0;
            }
            //Lấy tổng giá trị đang đầu tư từ sổ lệnh của riêng PHÂN PHỐI ĐẦU TƯ DISTRIBUTION
            var totalValue = _epicSchemaDbContext.InvOrders.Where(order => (order.TradingProviderId == tradingProviderId)
                                                                  && (order.DistributionId == distributionFind.Id)
                                                                  && (order.Status == OrderStatus.CHO_DUYET_HOP_DONG || order.Status == OrderStatus.DANG_DAU_TU)
                                                                  && order.Deleted == YesNo.NO).Sum(order => order.TotalValue);
            return totalInvestment.Value - totalValue;
        }

        private decimal SumMoney(int distributionId, int? tradingProviderId = null)
        {
            var distributionFind = _epicSchemaDbContext.InvestDistributions.FirstOrDefault(d => d.Id == distributionId && d.Deleted == YesNo.NO);
            if (distributionFind == null)
            {
                return 0;
            }
            var projectFind = _epicSchemaDbContext.InvestProjects.FirstOrDefault(p => p.Id == distributionFind.ProjectId && p.Deleted == YesNo.NO);
            if (projectFind == null)
            {
                return 0;
            }
            var listDistributionId = _epicSchemaDbContext.InvestDistributions.Where(d => d.ProjectId == distributionFind.ProjectId && d.Deleted == YesNo.NO).Select(d => d.Id);
            var totalValue = _epicSchemaDbContext.InvOrders.Where(order => (tradingProviderId == null || order.TradingProviderId == tradingProviderId)
                                                                 && listDistributionId.Contains(order.DistributionId)
                                                                 && (order.Status == OrderStatus.CHO_DUYET_HOP_DONG || order.Status == OrderStatus.DANG_DAU_TU)
                                                                 && order.Deleted == YesNo.NO).Sum(order => order.TotalValue);
            return totalValue;
        }

        /// <summary>
        /// Tìm kiếm danh sách phân phối cho app
        /// </summary>
        /// <returns></returns>
        public IQueryable<ProjectDistributionDto> FindAllAppQuery(string keyword, List<int> tradingProviderIds)
        {
            _logger.LogInformation($"{nameof(DistributionEFRepository)}->{nameof(FindAllAppQuery)}: keyword = {keyword}, tradingProviderIds = {JsonSerializer.Serialize(tradingProviderIds)}");
            return _epicSchemaDbContext.InvestDistributions
                .Include(d => d.Project)
                .Include(d => d.Policies.Where(p => p.Deleted == YesNo.NO && p.IsShowApp == YesNo.YES && p.Status == Status.ACTIVE))
                .Include(d => d.PolicyDetails.Where(pd => pd.Deleted == YesNo.NO && pd.IsShowApp == YesNo.YES && pd.Status == Status.ACTIVE))
                .Include(d => d.TradingProvider).ThenInclude(t => t.BusinessCustomer)
                .AsSplitQuery()
                .Where(d => d.Deleted == YesNo.NO
                    && d.IsShowApp == YesNo.YES && d.IsCheck == YesNo.YES && d.IsClose == YesNo.NO
                    && (d.OpenCellDate == null || d.OpenCellDate <= DateTime.Now)
                    && (d.CloseCellDate == null || d.CloseCellDate >= DateTime.Now)
                    && d.Project.Deleted == YesNo.NO
                    && (tradingProviderIds != null || tradingProviderIds.Contains(d.TradingProviderId))
                    && (keyword == null
                        || (d.Project.InvCode != null && d.Project.InvCode.ToLower().Contains(keyword.ToLower()))
                        || (d.Project.InvName != null && d.Project.InvName.Contains(keyword.ToLower()))))
                .Select(distribution => new ProjectDistributionDto
                {
                    DistributionId = distribution.Id,
                    TradingProviderId = distribution.TradingProviderId,
                    ProjectId = distribution.ProjectId,
                    InvCode = distribution.Project.InvCode,
                    TradingProviderName = distribution.TradingProvider.BusinessCustomer.Name,
                    Profit = distribution.PolicyDetails.Max(pd => pd.Profit),
                    Image = distribution.Project.Image,
                    DistributionImage = distribution.Image,
                    MinPeriodQuantity = distribution.PolicyDetails.Any() ? distribution.PolicyDetails.Min(pd => pd.PeriodQuantity) : null,
                    MaxPeriodQuantity = distribution.PolicyDetails.Any() ? distribution.PolicyDetails.Max(pd => pd.PeriodQuantity) : null,
                    MinPeriodType = distribution.PolicyDetails.Any() ? distribution.PolicyDetails.Min(pd => pd.PeriodType) : null,
                    MaxPeriodType = distribution.PolicyDetails.Any() ? distribution.PolicyDetails.Max(pd => pd.PeriodType) : null,
                });
        }
    }
}
