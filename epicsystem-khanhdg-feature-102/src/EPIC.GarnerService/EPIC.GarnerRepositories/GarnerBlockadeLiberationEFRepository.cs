using EPIC.DataAccess.Base;
using EPIC.DataAccess.Models;
using EPIC.GarnerEntities.DataEntities;
using EPIC.GarnerEntities.Dto.GarnerBlockadeLiberation;
using EPIC.Utils.ConstantVariables.Garner;
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
    public class GarnerBlockadeLiberationEFRepository : BaseEFRepository<GarnerBlockadeLiberation>
    {
        public GarnerBlockadeLiberationEFRepository(EpicSchemaDbContext dbContext, ILogger logger) : base(dbContext, logger, $"{DbSchemas.EPIC_GARNER}.{GarnerBlockadeLiberation.SEQ}")
        {
        }

        /// <summary>
        /// Thêm phong toả
        /// </summary>
        /// <param name="input"></param>
        /// <param name="username"></param>
        /// <param name="tradingProviderId"></param>
        /// <returns></returns>
        public GarnerBlockadeLiberation Add(GarnerBlockadeLiberation input, string username, int tradingProviderId)
        {
            _logger.LogInformation($"{nameof(GarnerBlockadeLiberationEFRepository)}->{nameof(Add)}: input = {JsonSerializer.Serialize(input)}, username = {username}, tradingProviderId = {tradingProviderId}");
            return _dbSet.Add(new GarnerBlockadeLiberation()
            {
                Id = (int)NextKey(),
                Type = input.Type,
                BlockadeDescription = input.BlockadeDescription,
                BlockadeDate = input.BlockadeDate,
                OrderId = input.OrderId,
                Blockader = username,
                TradingProviderId = tradingProviderId,
                CreatedBy = username
            }).Entity;
        }


        public GarnerBlockadeLiberation Update(GarnerBlockadeLiberation input, string username, int? tradingProviderId = null)
        {
            _logger.LogInformation($"{nameof(GarnerBlockadeLiberationEFRepository)}->{nameof(Update)}: input = {JsonSerializer.Serialize(input)}, username = {username}, tradingProviderId = {tradingProviderId}");
            var blockadeLiberation = _dbSet.FirstOrDefault(b => b.Id == input.Id && (tradingProviderId == null || b.TradingProviderId == tradingProviderId));
            if (blockadeLiberation != null)
            {
                blockadeLiberation.Type = input.Type;
                blockadeLiberation.BlockadeDescription = input.BlockadeDescription;
                blockadeLiberation.BlockadeDate = input.BlockadeDate;
                blockadeLiberation.LiberationDescription = input.LiberationDescription;
                blockadeLiberation.LiberationDate = input.LiberationDate;
                blockadeLiberation.Liberator = username;
                blockadeLiberation.Status = BlockadeLiberationStatus.GIAI_TOA;
                blockadeLiberation.LiberationTime = DateTime.Now;
            }
            return blockadeLiberation;
        }

        public GarnerBlockadeLiberation FindById (int id, int? tradingProviderId = null)
        {
            _logger.LogInformation($"{nameof(GarnerBlockadeLiberationEFRepository)}->{nameof(FindById)}:, id = {id}, tradingProviderId = {tradingProviderId}");
            return _dbSet.FirstOrDefault(d => d.Id == id && (tradingProviderId == null || d.TradingProviderId == tradingProviderId));
        }

        public PagingResult<GarnerBlockadeLiberation> FindAllBlockadeLiberation(FilterGarnerBlockadeLiberationDto input, int? tradingProviderId = null)
        {
            _logger.LogInformation($"{nameof(GarnerBlockadeLiberationEFRepository)}->{nameof(FindAllBlockadeLiberation)}: input = {JsonSerializer.Serialize(input)}, tradingProviderId = {tradingProviderId}");
            PagingResult<GarnerBlockadeLiberation> result = new();
            IQueryable<GarnerBlockadeLiberation> blockadeLiberationQuery = _dbSet.Where(b => (tradingProviderId == null || b.TradingProviderId == tradingProviderId)).OrderByDescending(b => b.Id);

            if (input.Type == BlockadeLiberationTypes.OTHER)
            {
                blockadeLiberationQuery = blockadeLiberationQuery.Where(b => b.Type == BlockadeLiberationTypes.OTHER);
            }
            else if (input.Type == BlockadeLiberationTypes.PLEDGE)
            {
                blockadeLiberationQuery = blockadeLiberationQuery.Where(b => b.Type == BlockadeLiberationTypes.PLEDGE);
            }
            else if (input.Type == BlockadeLiberationTypes.ADVANCE_CAPITAL)
            {
                blockadeLiberationQuery = blockadeLiberationQuery.Where(b => b.Type == BlockadeLiberationTypes.ADVANCE_CAPITAL);
            }

            if (input.Status == BlockadeLiberationStatus.PHONG_TOA)
            {
                blockadeLiberationQuery = blockadeLiberationQuery.Where(b => b.Status == BlockadeLiberationStatus.PHONG_TOA);
            }
            else if (input.Status == BlockadeLiberationStatus.GIAI_TOA)
            {
                blockadeLiberationQuery = blockadeLiberationQuery.Where(b => b.Status == BlockadeLiberationStatus.GIAI_TOA);
            }

            result.TotalItems = blockadeLiberationQuery.Count();

            if (input.PageSize != -1)
            {
                blockadeLiberationQuery = blockadeLiberationQuery.Skip(input.Skip).Take(input.PageSize);
            }

            result.Items = blockadeLiberationQuery.ToList();
            return result;
        }

        public List<GarnerBlockadeLiberation> FindAll(int? tradingProviderId = null)
        {
            _logger.LogInformation($"{nameof(GarnerBlockadeLiberationEFRepository)}->{nameof(FindAll)}: tradingProviderId = {tradingProviderId}");

            var blockadeLiberations = _dbSet.Where(b => (tradingProviderId == null || b.TradingProviderId == tradingProviderId)).ToList();
            return blockadeLiberations;
        }
    }
}
