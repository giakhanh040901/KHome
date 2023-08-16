using EPIC.DataAccess.Base;
using EPIC.DataAccess.Models;
using EPIC.Entities.DataEntities;
using EPIC.InvestEntities.DataEntities;
using EPIC.RealEstateEntities.DataEntities;
using EPIC.RealEstateEntities.Dto.RstDistribution;
using EPIC.RealEstateEntities.Dto.RstProject;
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
    public class RstDistributionEFRepository : BaseEFRepository<RstDistribution>
    {
        public RstDistributionEFRepository(EpicSchemaDbContext dbContext, ILogger logger) : base(dbContext, logger, $"{DbSchemas.EPIC_REAL_ESTATE}.{RstDistribution.SEQ}")
        {
        }
    
        public RstDistribution Add(RstDistribution input)
        {
            _logger.LogInformation($"{nameof(RstDistributionEFRepository)}->{nameof(Add)}: {JsonSerializer.Serialize(input)}");
            input.Id = (int)NextKey();
            input.CreatedDate = DateTime.Now;
            input.Deleted = YesNo.NO;
            return _dbSet.Add(input).Entity;
        }

        public void Update(RstDistribution input)
        {
            _logger.LogInformation($"{nameof(RstDistributionEFRepository)}->{nameof(Update)}: {JsonSerializer.Serialize(input)}");
            var distributionQuery = _dbSet.FirstOrDefault(d => d.Id == input.Id && d.PartnerId == input.PartnerId && d.Deleted == YesNo.NO)
                .ThrowIfNull(_epicSchemaDbContext, ErrorCode.RstDistributionNotFound);
            distributionQuery.DistributionType = input.DistributionType;
            distributionQuery.StartDate = input.StartDate;
            distributionQuery.EndDate = input.EndDate;
            distributionQuery.CreatedDate = DateTime.Now;
            distributionQuery.ModifiedBy = input.ModifiedBy;
        }
        
        public RstDistribution FindById(int id, int? partnerId = null)
        {
            _logger.LogInformation($"{nameof(RstDistributionEFRepository)}->{nameof(FindById)}: id = {id}, partnerId = {partnerId}");
            return _dbSet.FirstOrDefault(d => d.Id == id && (partnerId == null || d.PartnerId == partnerId) && d.Deleted == YesNo.NO);
        }

        //public PagingResult<RstDistribution> FindAll(FilterRstDistributionDto input, int? partnerId = null, int? tradingProviderId = null)
        //{
        //    _logger.LogInformation($"{nameof(RstDistributionEFRepository)}->{nameof(FindAll)}: input = {JsonSerializer.Serialize(input)},  partnerId = {partnerId}");

        //    PagingResult<RstDistribution> result = new();

        //    var distributionQuery = (from distribution in _dbSet
        //                             join project in _epicSchemaDbContext.RstProjects on distribution.ProjectId equals project.Id
        //                             where distribution.Deleted == YesNo.NO && project.Deleted == YesNo.NO
        //                             && (partnerId == null || (distribution.PartnerId == partnerId))
        //                             && (tradingProviderId == null || (distribution.TradingProviderId == tradingProviderId))
        //                             && (input.ProjectId == null || input.ProjectId == distribution.ProjectId)
        //                             && (input.TradingProviderId == null || input.TradingProviderId == distribution.TradingProviderId)
        //                             && (input.Status == null || input.Status == distribution.Status)
        //                             && (input.Keyword == null || project.Name.Contains(input.Keyword) || project.Name.Contains(input.Keyword))
        //                             select distribution);

        //    result.TotalItems = distributionQuery.Count();
        //    distributionQuery = distributionQuery.OrderDynamic(input.Sort);
        //    if (input.PageSize != -1)
        //    {
        //        distributionQuery = distributionQuery.Skip(input.Skip).Take(input.PageSize);
        //    }

        //    result.Items = distributionQuery;
        //    return result;
        //}

        public PagingResult<RstDistributionDto> FindAll(FilterRstDistributionDto input, int? partnerId = null, int? tradingProviderId = null)
        {
            _logger.LogInformation($"{nameof(RstDistributionEFRepository)}->{nameof(FindAll)}: input = {JsonSerializer.Serialize(input)},  partnerId = {partnerId}");

            PagingResult<RstDistributionDto> result = new();

            var distributionQuery = (from distribution in _dbSet
                                     join project in _epicSchemaDbContext.RstProjects on distribution.ProjectId equals project.Id
                                     where distribution.Deleted == YesNo.NO && project.Deleted == YesNo.NO
                                     && (partnerId == null || (distribution.PartnerId == partnerId))
                                     && (tradingProviderId == null || (distribution.TradingProviderId == tradingProviderId))
                                     && (input.ProjectId == null || input.ProjectId == distribution.ProjectId)
                                     && (input.TradingProviderId == null || input.TradingProviderId == distribution.TradingProviderId)
                                     && (input.Status == null || input.Status == distribution.Status)
                                     && (input.Keyword == null || project.Name.Contains(input.Keyword) || project.Name.Contains(input.Keyword))
                                     select new RstDistributionDto
                                     {
                                         Id = distribution.Id,
                                         ProjectId = distribution.ProjectId,
                                         CreatedBy = distribution.CreatedBy,
                                         CreatedDate = distribution.CreatedDate,
                                         StartDate = distribution.StartDate,
                                         EndDate = distribution.EndDate,
                                         Status = distribution.Status,
                                         Description = distribution.Description,
                                         TradingProviderId = distribution.TradingProviderId,
                                         Quantity = (from distributionProductItem in _epicSchemaDbContext.RstDistributionProductItems
                                                     join productItem in _epicSchemaDbContext.RstProductItems on distributionProductItem.ProductItemId equals productItem.Id
                                                     where distributionProductItem.DistributionId == distribution.Id && distributionProductItem.Deleted == YesNo.NO && productItem.Deleted == YesNo.NO
                                                     select distributionProductItem).Count(),
                                         QuantityDeposit = (from distributionItem in (from distributionProductItem in _epicSchemaDbContext.RstDistributionProductItems
                                                                                      join productItem in _epicSchemaDbContext.RstProductItems on distributionProductItem.ProductItemId equals productItem.Id
                                                                                      where distributionProductItem.DistributionId == distribution.Id && distributionProductItem.Deleted == YesNo.NO && productItem.Deleted == YesNo.NO
                                                                                      select distributionProductItem)
                                                            join openSellDetail in _epicSchemaDbContext.RstOpenSellDetails on distributionItem.Id equals openSellDetail.DistributionProductItemId
                                                            where openSellDetail.Deleted == YesNo.NO
                                                            select openSellDetail).Where(r => r.Status == RstProductItemStatus.DA_COC).Count(),
                                         QuantitySold = (from distributionItem in (from distributionProductItem in _epicSchemaDbContext.RstDistributionProductItems
                                                                                   join productItem in _epicSchemaDbContext.RstProductItems on distributionProductItem.ProductItemId equals productItem.Id
                                                                                   where distributionProductItem.DistributionId == distribution.Id && distributionProductItem.Deleted == YesNo.NO && productItem.Deleted == YesNo.NO
                                                                                   select distributionProductItem)
                                                         join openSellDetail in _epicSchemaDbContext.RstOpenSellDetails on distributionItem.Id equals openSellDetail.DistributionProductItemId
                                                         where openSellDetail.Deleted == YesNo.NO
                                                         select openSellDetail).Where(r => r.Status == RstProductItemStatus.DA_BAN).Count(),
                                     });

            result.TotalItems = distributionQuery.Count();
            distributionQuery = distributionQuery.OrderDynamic(input.Sort);
            if (input.PageSize != -1)
            {
                distributionQuery = distributionQuery.Skip(input.Skip).Take(input.PageSize);
            }

            result.Items = distributionQuery;
            return result;
        }
    }
}
