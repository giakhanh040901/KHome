using EPIC.DataAccess.Base;
using EPIC.DataAccess.Models;
using EPIC.Entities.DataEntities;
using EPIC.GarnerEntities.DataEntities;
using EPIC.GarnerEntities.Dto.GarnerPolicy;
using EPIC.GarnerEntities.Dto.GarnerProductPrice;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Shared;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace EPIC.GarnerRepositories
{
    public class GarnerProductPriceEFRepository : BaseEFRepository<GarnerProductPrice>
    {
        public GarnerProductPriceEFRepository(EpicSchemaDbContext dbContext, ILogger logger) : base(dbContext, logger, $"{DbSchemas.EPIC_GARNER}.{GarnerProductPrice.SEQ}")
        {
        }

        public GarnerProductPrice Add(GarnerProductPrice input)
        {
            _logger.LogInformation($"{nameof(Add)}: input = {JsonSerializer.Serialize(input)}");
            
            return _dbSet.Add(new GarnerProductPrice
            {
                Id = (int)NextKey(),
                TradingProviderId = input.TradingProviderId,
                DistributionId = input.DistributionId,
                PriceDate = input.PriceDate,
                Price = (decimal)input.Price,
                CreatedBy = input.CreatedBy,
                CreatedDate = DateTime.Now,
                Deleted = YesNo.NO
            }).Entity;
        }

        public void Delete(int ditributrionId, int? tradingProviderId = null)
        {
            _logger.LogInformation($"{nameof(Delete)}: input = {ditributrionId}; tradingProviderId = {tradingProviderId}");
            var productPrices = _dbSet.Where(p => p.DistributionId == ditributrionId && p.Deleted == YesNo.NO && (tradingProviderId == null || p.TradingProviderId == tradingProviderId));
            foreach (var item in productPrices)
            {
                item.Deleted = YesNo.YES;
            }
        }

        public PagingResult<GarnerProductPrice> FindAll(FilterGarnerProductPriceDto input, int? tradingProviderId = null)
        {
            _logger.LogInformation($"{nameof(FindAll)}: input = {JsonSerializer.Serialize(input)}, tradingproviderId = {tradingProviderId}");

            PagingResult<GarnerProductPrice> result = new();
            IQueryable<GarnerProductPrice> productPriceQuery = _dbSet.Where(p => p.DistributionId == input.DitributionId && (tradingProviderId == null || p.TradingProviderId == tradingProviderId) && p.Deleted == YesNo.NO)
                                            .OrderByDescending(p => p.PriceDate);
            result.TotalItems = productPriceQuery.Count();

            if (input.PageSize != -1)
            {
                productPriceQuery = productPriceQuery.Skip(input.Skip).Take(input.PageSize);
            }

            result.Items = productPriceQuery.ToList();
            return result;
        }

        public void Update(UpdateProductPriceDto input, string username, int? tradingProviderId = null)
        {
            _logger.LogInformation($"{nameof(Update)}: input = {JsonSerializer.Serialize(input)}, username = {username}, tradingproviderId = {tradingProviderId}");
            var productPrices = _dbSet.Where(p => p.Id == input.Id && p.DistributionId == input.DitributionId && p.Deleted == YesNo.NO 
                                        && (tradingProviderId == null || p.TradingProviderId == tradingProviderId));
            foreach (var item in productPrices)
            {
                item.Price = input.Price;
                item.ModifiedBy = username;
                item.ModifiedDate = DateTime.Now;
            }
        }

        public List<GarnerProductPrice> FindProductPrice(int id, DateTime priceDate)
        {
            _logger.LogInformation($"{nameof(FindProductPrice)}: id = {id}; priceDate = {priceDate}");
            return _dbSet.Where(p => p.Id == id && p.PriceDate == priceDate && p.Deleted == YesNo.NO).ToList();
        }
        
        public GarnerProductPrice FindProductPriceByDistributionAndPriceDate(int distributionId, DateTime priceDate, int? tradingProviderId = null)
        {
            _logger.LogInformation($"{nameof(FindProductPriceByDistributionAndPriceDate)}: distributionId = {distributionId}; priceDate = {priceDate}, tradingProviderId = {tradingProviderId}");
            return _dbSet.FirstOrDefault(p => p.DistributionId == distributionId && p.PriceDate.Date == priceDate.Date && (tradingProviderId == null || p.TradingProviderId == tradingProviderId) && p.Deleted == YesNo.NO);
        }
    }
}
