using EPIC.CompanySharesEntities.DataEntities;
using EPIC.DataAccess;
using EPIC.DataAccess.Models;
using EPIC.Utils.ConstantVariables.Shared;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.CompanySharesRepositories
{
    public class CpsSecondaryPriceRepository
    {
        private readonly OracleHelper _oracleHelper;
        private readonly ILogger _logger;
        private const string schema = DbSchemas.EPIC_COMPANY_SHARES + ".";

        private const string PROC_ADD_SECOND_PRICE = schema + "PKG_CPS_SECONDARY_PRICE.PROC_SECONDARY_PRICE_ADD";
        private const string PROC_SECOND_PRICE_ADD_MULTI = schema + "PKG_CPS_SECONDARY_PRICE.PROC_SECOND_PRICE_ADD_MULTI";
        private const string PROC_DELETE_SECOND_PRICE = schema + "PKG_CPS_SECONDARY_PRICE.PROC_SECONDARY_PRICE_DELETE";
        private const string PROC_GET_ALL_SECOND_PRICE = schema + "PKG_CPS_SECONDARY_PRICE.PROC_SECONDARY_PRICE_FIND";
        private const string PROC_CPS_SECOND_PRICE_FIND_BY_DATE = schema + "PKG_CPS_SECONDARY_PRICE.PROC_SECONDARY_P_FIND_BY_DATE";
        private const string PROC_CPS_SECOND_PRICE_UPDATE = schema + "PKG_CPS_SECONDARY_PRICE.PROC_SECONDARY_PRICE_UDPATE";

        public CpsSecondaryPriceRepository(string connectionString, ILogger logger)
        {
            _oracleHelper = new OracleHelper(connectionString, logger);
            _logger = logger;
        }

        public void CloseConnection()
        {
            _oracleHelper.CloseConnection();
        }

        public void Add(CpsSecondaryPrice entity)
        {
            _oracleHelper.ExecuteProcedureNonQuery(
            PROC_ADD_SECOND_PRICE, new
            {
                pv_TRADING_PROVIDER_ID = entity.TradingProviderId,
                pv_SECONDARY_ID = entity.SecondaryId,
                pv_PRICE_DATE = entity.PriceDate,
                pv_PRICE = entity.Price,
                SESSION_USERNAME = entity.CreatedBy
            }, false);
        }

        public int Delete(int bondSecondaryId, int tradingProviderId)
        {
            return _oracleHelper.ExecuteProcedureNonQuery(PROC_DELETE_SECOND_PRICE, new
            {
                pv_SECONDARY_ID = bondSecondaryId,
                pv_TRADING_PROVIDER_ID = tradingProviderId
            });
        }

        public CpsSecondaryPrice FindByDate(int bondSecondaryId, DateTime priceDate, int? tradingProviderId = null)
        {
            return _oracleHelper.ExecuteProcedureToFirst<CpsSecondaryPrice>(PROC_CPS_SECOND_PRICE_FIND_BY_DATE, new
            {
                pv_SECONDARY_ID = bondSecondaryId,
                pv_PRICE_DATE = priceDate,
                pv_TRADING_PROVIDER_ID = tradingProviderId
            });
        }

        public PagingResult<CpsSecondaryPrice> FindAll(int pageSize, int pageNumber, int bondSecondaryId, int tradingProviderId)
        {
            return _oracleHelper.ExecuteProcedurePaging<CpsSecondaryPrice>(PROC_GET_ALL_SECOND_PRICE, new
            {
                PAGE_SIZE = pageSize,
                PAGE_NUMBER = pageNumber,
                pv_SECONDARY_ID = bondSecondaryId,
                pv_TRADING_PROVIDER_ID = tradingProviderId
            });
        }

        public int Update(CpsSecondaryPrice entity)
        {
            return _oracleHelper.ExecuteProcedureNonQuery(PROC_CPS_SECOND_PRICE_UPDATE, new
            {
                pv_PRICE_ID = entity.Id,
                pv_TRADING_PROVIDER_ID = entity.TradingProviderId,
                pv_SECONDARY_ID = entity.SecondaryId,
                pv_PRICE_DATE = entity.PriceDate,
                pv_PRICE = entity.Price,
                SESSION_USERNAME = entity.ModifiedBy,
            });
        }
    }
}
