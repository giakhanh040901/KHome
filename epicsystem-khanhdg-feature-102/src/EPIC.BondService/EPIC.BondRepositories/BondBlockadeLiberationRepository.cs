using EPIC.BondEntities.DataEntities;
using EPIC.DataAccess;
using EPIC.DataAccess.Models;
using EPIC.Entities.DataEntities;
using EPIC.Entities.Dto.BlockadeLiberation;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.BondRepositories
{
    public class BondBlockadeLiberationRepository
    {
        private OracleHelper _oracleHelper;
        private ILogger _logger;
        private static string PROC_BLOCKADE_LIBERATION_ADD = "PKG_BOND_BLOCKADE_LIBERATION.PROC_BLOCKADE_LIBERATION_ADD";
        private static string PROC_BLOCKADE_LIBERATION_UPDATE = "PKG_BOND_BLOCKADE_LIBERATION.PROC_BLOCKADE_LIBERATION_UP";
        private static string PROC_BLOCKADE_LIBERATION_GET = "PKG_BOND_BLOCKADE_LIBERATION.PROC_BLOCKADE_LIBERATION_GET";
        private static string PROC_BLOCKADE_LIBERATION_GET_ALL = "PKG_BOND_BLOCKADE_LIBERATION.PROC_BLOCKADE_LIBERATION_ALL";

        public BondBlockadeLiberationRepository(string connectionString, ILogger logger)
        {
            _oracleHelper = new OracleHelper(connectionString, logger);
            _logger = logger;
        }

        public BondBlockadeLiberation Add(BondBlockadeLiberation entity)
        {
            var result = _oracleHelper.ExecuteProcedureToFirst<BondBlockadeLiberation>(PROC_BLOCKADE_LIBERATION_ADD, new
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
        public PagingResult<BlockadeLiberationDto> FindAll(int tradingProviderId, int pageSize, int pageNumber, string keyword, int? status, int? type)
        {
            var result = _oracleHelper.ExecuteProcedurePaging<BlockadeLiberationDto>(PROC_BLOCKADE_LIBERATION_GET_ALL, new
            {
                pv_TRADING_PROVIDER_ID = tradingProviderId,
                PAGE_SIZE = pageSize,
                PAGE_NUMBER = pageNumber,
                KEY_WORD = keyword,
                pv_STATUS = status,
                pv_TYPE = type
            });
            return result;
        }

        public BondBlockadeLiberation FindById(int? id, int? tradingProviderId)
        {
            var result = _oracleHelper.ExecuteProcedureToFirst<BondBlockadeLiberation>(PROC_BLOCKADE_LIBERATION_GET, new
            {
                pv_TRADING_PROVIDER_ID = tradingProviderId,
                pv_ID = id,
            });
            return result;
        }

        public int Update(BondBlockadeLiberation entity)
        {
            var result = _oracleHelper.ExecuteProcedureNonQuery(PROC_BLOCKADE_LIBERATION_UPDATE, new
            {
                pv_TRADING_PROVIDER_ID = entity.TradingProviderId,
                pv_ID = entity.Id,
                pv_TYPE = entity.Type,
                pv_BLOCKADE_DESCRIPTION = entity.BlockadeDescription,
                pv_BLOCKADE_DATE  = entity.BlockadeDate,
                pv_ORDER_ID = entity.OrderId,
                pv_LIBERATION_DESCRIPTION = entity.LiberationDescription,
                pv_LIBERATION_DATE = entity.LiberationDate,
                SESSION_USERNAME = entity.Liberator
            });
            return result;
        }

    }
}
