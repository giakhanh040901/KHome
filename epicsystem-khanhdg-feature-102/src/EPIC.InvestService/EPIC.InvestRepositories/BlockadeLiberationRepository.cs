using EPIC.DataAccess;
using EPIC.DataAccess.Models;
using EPIC.InvestEntities.DataEntities;
using EPIC.InvestEntities.Dto.BlockadeLiberation;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.InvestRepositories
{
    public class BlockadeLiberationRepository
    {
        private OracleHelper _oracleHelper;
        private ILogger _logger;
        private static string PROC_BLOCKADE_LIBERATION_ADD = "PKG_INV_BLOCKADE_LIBERATION.PROC_BLOCKADE_LIBERATION_ADD";
        private static string PROC_BLOCKADE_LIBERATION_UPDATE = "PKG_INV_BLOCKADE_LIBERATION.PROC_BLOCKADE_LIBERATION_UP";
        private static string PROC_BLOCKADE_LIBERATION_GET = "PKG_INV_BLOCKADE_LIBERATION.PROC_BLOCKADE_LIBERATION_GET";
        private static string PROC_BLOCKADE_LIBERATION_GET_ALL = "PKG_INV_BLOCKADE_LIBERATION.PROC_BLOCKADE_LIBERATION_ALL";

        public BlockadeLiberationRepository(string connectionString, ILogger logger)
        {
            _oracleHelper = new OracleHelper(connectionString, logger);
            _logger = logger;
        }

        public BlockadeLiberation Add(BlockadeLiberation entity)
        {
            var result = _oracleHelper.ExecuteProcedureToFirst<BlockadeLiberation>(PROC_BLOCKADE_LIBERATION_ADD, new
            {
                pv_TRADING_PROVIDER_ID = entity.TradingProviderId,
                pv_TYPE = entity.Type,
                pv_BLOCKADE_DESCRIPTION = entity.BlockadeDescription,
                pv_BLOCKADE_DATE = entity.BlockadeDate,
                pv_ORDER_ID = entity.OrderId,
                SESSION_USERNAME = entity.Blockader,
            }, false);
            return result;
        }
        public PagingResult<BlockadeLiberationDto> FindAll(int? tradingProviderId, FilterBlockadeLiberationDto input, List<int> tradingProviderChildIds)
        {
            var result = _oracleHelper.ExecuteProcedurePaging<BlockadeLiberationDto>(PROC_BLOCKADE_LIBERATION_GET_ALL, new
            {
                pv_TRADING_PROVIDER_ID = tradingProviderId,
                PAGE_SIZE = input.PageSize,
                PAGE_NUMBER = input.PageNumber,
                KEY_WORD = input.Keyword,
                pv_STATUS = input.Status,
                pv_TYPE = input.Type,
                pv_TRADING_PROVIDER_CHILD_IDS = tradingProviderChildIds != null ? string.Join(',', tradingProviderChildIds) : null,
            });
            return result;
        }

        public BlockadeLiberation FindById(int? id, int? tradingProviderId)
        {
            var result = _oracleHelper.ExecuteProcedureToFirst<BlockadeLiberation>(PROC_BLOCKADE_LIBERATION_GET, new
            {
                pv_TRADING_PROVIDER_ID = tradingProviderId,
                pv_ID = id,
            });
            return result;
        }

        public int Update(BlockadeLiberation entity)
        {
            var result = _oracleHelper.ExecuteProcedureNonQuery(PROC_BLOCKADE_LIBERATION_UPDATE, new
            {
                pv_TRADING_PROVIDER_ID = entity.TradingProviderId,
                pv_ID = entity.Id,
                pv_TYPE = entity.Type,
                pv_BLOCKADE_DESCRIPTION = entity.BlockadeDescription,
                pv_BLOCKADE_DATE = entity.BlockadeDate,
                pv_ORDER_ID = entity.OrderId,
                pv_LIBERATION_DESCRIPTION = entity.LiberationDescription,
                pv_LIBERATION_DATE = entity.LiberationDate,
                SESSION_USERNAME = entity.Liberator
            });
            return result;
        }
    }
}
