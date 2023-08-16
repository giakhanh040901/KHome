using EPIC.DataAccess.Base;
using EPIC.DataAccess.Models;
using EPIC.Entities.DataEntities;
using EPIC.InvestEntities.DataEntities;
using EPIC.RealEstateEntities.DataEntities;
using EPIC.RealEstateEntities.Dto.RstDistribution;
using EPIC.RealEstateEntities.Dto.RstOpenSell;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.RealEstate;
using EPIC.Utils.ConstantVariables.Shared;
using EPIC.Utils.Linq;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace EPIC.RealEstateRepositories
{
    public class RstOpenSellEFRepository : BaseEFRepository<RstOpenSell>
    {
        public RstOpenSellEFRepository(EpicSchemaDbContext dbContext, ILogger logger) : base(dbContext, logger, $"{DbSchemas.EPIC_REAL_ESTATE}.{RstOpenSell.SEQ}")
        {
        }

        public RstOpenSell Add(RstOpenSell input)
        {
            _logger.LogInformation($"{nameof(RstOpenSellEFRepository)}->{nameof(Add)}: {JsonSerializer.Serialize(input)}");

            input.Id = (int)NextKey();
            input.Status = RstDistributionStatus.KHOI_TAO;
            input.IsShowApp = YesNo.YES;
            input.CreatedDate = DateTime.Now;
            return _dbSet.Add(input).Entity;
        }

        public RstOpenSell Update(RstOpenSell input)
        {
            _logger.LogInformation($"{nameof(RstOpenSellEFRepository)}->{nameof(Update)}: {JsonSerializer.Serialize(input)}");
            var openSellQuery = _dbSet.FirstOrDefault(r => r.Id == input.Id && r.TradingProviderId == input.TradingProviderId && r.Deleted == YesNo.NO)
                .ThrowIfNull(_epicSchemaDbContext, ErrorCode.RstOpenSellNotFound);
            openSellQuery.StartDate = input.StartDate;
            openSellQuery.EndDate = input.EndDate;
            openSellQuery.ModifiedDate = DateTime.Now;
            openSellQuery.ModifiedBy = input.ModifiedBy;
            openSellQuery.KeepTime = input.KeepTime;
            openSellQuery.Hotline = input.Hotline;
            openSellQuery.FromType = input.FromType;
            openSellQuery.Description = input.Description;
            openSellQuery.IsRegisterSale = input.IsRegisterSale;
            return openSellQuery;
        }

        public RstOpenSell FindById(int id, int? tradingProviderId = null)
        {
            _logger.LogInformation($"{nameof(RstOpenSellEFRepository)}->{nameof(FindById)}: id = {id}");
            return _dbSet.FirstOrDefault(r => r.Id == id && (tradingProviderId == null || r.TradingProviderId == tradingProviderId) && r.Deleted == YesNo.NO);
        }

        //public PagingResult<RstOpenSell> FindAll(FilterRstOpenSellDto input, int? tradingProviderId)
        //{
        //    _logger.LogInformation($"{nameof(RstOpenSellEFRepository)}->{nameof(FindAll)}: input = {JsonSerializer.Serialize(input)},  tradingProviderId = {tradingProviderId}");

        //    PagingResult<RstOpenSell> result = new();

        //    var openSellQuery = (from openSell in _dbSet
        //                         join project in _epicSchemaDbContext.RstProjects on openSell.ProjectId equals project.Id
        //                         where openSell.Deleted == YesNo.NO && project.Deleted == YesNo.NO
        //                         && openSell.TradingProviderId == tradingProviderId
        //                         && (input.ProjectId == null || input.ProjectId == openSell.ProjectId)
        //                         && (input.OwnerId == null || input.OwnerId == project.OwnerId)
        //                         && (input.Status == null || input.Status == openSell.Status)
        //                         && (input.Keyword == null || project.Name.Contains(input.Keyword) || project.Code.Contains(input.Keyword))
        //                         select openSell);

        //    result.TotalItems = openSellQuery.Count();
        //    openSellQuery = openSellQuery.OrderByDescending(d => d.IsOutstanding).ThenByDescending(d => d.Id);
        //    openSellQuery = openSellQuery.OrderDynamic(input.Sort);
        //    if (input.PageSize != -1)
        //    {
        //        openSellQuery = openSellQuery.Skip(input.Skip).Take(input.PageSize);
        //    }

        //    result.Items = openSellQuery;
        //    return result;
        //}

        public PagingResult<RstOpenSellDto> FindAll(FilterRstOpenSellDto input, int? tradingProviderId)
        {
            _logger.LogInformation($"{nameof(RstOpenSellEFRepository)}->{nameof(FindAll)}: input = {JsonSerializer.Serialize(input)},  tradingProviderId = {tradingProviderId}");

            PagingResult<RstOpenSellDto> result = new();

            var openSellDetail = (from detail in _epicSchemaDbContext.RstOpenSellDetails
                                  join o in _dbSet on detail.OpenSellId equals o.Id
                                  where detail.Deleted == YesNo.NO && o.Deleted == YesNo.NO
                                  && (tradingProviderId == null || o.TradingProviderId == tradingProviderId)
                                  select detail);

            var openSellQuery = (from openSell in _dbSet
                                 join project in _epicSchemaDbContext.RstProjects on openSell.ProjectId equals project.Id
                                 where openSell.Deleted == YesNo.NO && project.Deleted == YesNo.NO
                                 && openSell.TradingProviderId == tradingProviderId
                                 && (input.ProjectId == null || input.ProjectId == openSell.ProjectId)
                                 && (input.OwnerId == null || input.OwnerId == project.OwnerId)
                                 && (input.Status == null || input.Status == openSell.Status)
                                 && (input.Keyword == null || project.Name.Contains(input.Keyword) || project.Code.Contains(input.Keyword))
                                 select new RstOpenSellDto
                                 {
                                     Id = openSell.Id,
                                     CreatedBy = openSell.CreatedBy,
                                     CreatedDate = openSell.CreatedDate,
                                     EndDate = openSell.EndDate,
                                     StartDate = openSell.StartDate,
                                     Status = openSell.Status,
                                     KeepTime = openSell.KeepTime,
                                     IsOutstanding = openSell.IsOutstanding,
                                     IsShowApp = openSell.IsShowApp,
                                     FromType = openSell.FromType,
                                     ProjectId = openSell.ProjectId,
                                     Quantity = openSellDetail.Where(o => o.OpenSellId == openSell.Id).Count(),
                                     QuantityDeposit = openSellDetail.Where(o => o.OpenSellId == openSell.Id && o.Status == RstProductItemStatus.DA_COC).Count(),
                                     QuantitySold = openSellDetail.Where(o => o.OpenSellId == openSell.Id && o.Status == RstProductItemStatus.DA_BAN).Count(),
                                 });

            result.TotalItems = openSellQuery.Count();
            openSellQuery = openSellQuery.OrderByDescending(d => d.IsOutstanding).ThenByDescending(d => d.Id);
            openSellQuery = openSellQuery.OrderDynamic(input.Sort);
            if (input.PageSize != -1)
            {
                openSellQuery = openSellQuery.Skip(input.Skip).Take(input.PageSize);
            }

            result.Items = openSellQuery;
            return result;
        }

        /// <summary>
        /// Thời gian giữ cọc theo mở bán/ Trường hợp có cài thời gian giữ cọc ở mở bán
        /// </summary>
        public DateTime? ExpTimeDeposit(int openSellId)
        {
            _logger.LogInformation($"{nameof(RstOpenSellEFRepository)}->{nameof(ExpTimeDeposit)}: openSellId ={openSellId}");
            var openSellQuery = _dbSet.FirstOrDefault(r => r.Id == openSellId && r.Deleted == YesNo.NO)
                .ThrowIfNull(_epicSchemaDbContext, ErrorCode.RstOpenSellNotFound);
            DateTime? expTimeDeposit = null;
            if (openSellQuery.KeepTime != null)
            {
                expTimeDeposit = DateTime.Now.AddSeconds(openSellQuery.KeepTime ?? 0);
            }
            return expTimeDeposit;
        }

        /// <summary>
        /// Kiểm tra xem đại lý có sản phẩm mở bán không
        /// </summary>
        /// <returns></returns>
        public bool CheckTradingHaveOpenSell(HashSet<int> tradingProviderIds)
        {
            _logger.LogInformation($"{nameof(CheckTradingHaveOpenSell)}: tradingProviderId = {tradingProviderIds}");

            return _dbSet.Where(d => tradingProviderIds.Contains(d.TradingProviderId)
                                && d.Status == RstDistributionStatus.DANG_BAN
                                && (d.StartDate.Date <= DateTime.Now.Date)
                                && (d.EndDate != null && d.EndDate.Value.Date >= DateTime.Now.Date)
                                && d.Deleted == YesNo.NO).Any();
        }
    }
}
