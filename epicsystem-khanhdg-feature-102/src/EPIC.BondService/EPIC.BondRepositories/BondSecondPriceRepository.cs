using EPIC.BondEntities.DataEntities;
using EPIC.DataAccess;
using EPIC.DataAccess.Models;
using EPIC.Entities.DataEntities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace EPIC.BondRepositories
{
    public class BondSecondPriceRepository
    {
        private readonly OracleHelper _oracleHelper;
        private readonly ILogger _logger;
        private const string PROC_ADD_SECOND_PRICE = "PKG_PRODUCT_BOND_SECOND_PRICE.PROC_BOND_SECOND_PRICE_ADD";
        private const string PROC_SECOND_PRICE_ADD_MULTI = "PKG_PRODUCT_BOND_SECOND_PRICE.PROC_SECOND_PRICE_ADD_MULTI";
        private const string PROC_DELETE_SECOND_PRICE = "PKG_PRODUCT_BOND_SECOND_PRICE.PROC_BOND_SECOND_PRICE_DELETE";
        private const string PROC_GET_ALL_SECOND_PRICE = "PKG_PRODUCT_BOND_SECOND_PRICE.PROC_BOND_SECOND_PRICE_FIND";
        private const string PROC_BOND_SECOND_PRICE_FIND_BY_DATE = "PKG_PRODUCT_BOND_SECOND_PRICE.PROC_B_SECOND_P_FIND_BY_DATE";
        private const string PROC_BOND_SECOND_PRICE_UPDATE = "PKG_PRODUCT_BOND_SECOND_PRICE.PROC_BOND_SECOND_PRICE_UPDATE";

        public BondSecondPriceRepository(string connectionString, ILogger logger)
        {
            _oracleHelper = new OracleHelper(connectionString, logger);
            _logger = logger;
        }

        public void CloseConnection()
        {
            _oracleHelper.CloseConnection();
        }

        public void Add(BondSecondPrice entity)
        {
            _oracleHelper.ExecuteProcedureNonQuery(
            PROC_ADD_SECOND_PRICE, new
            {
                pv_TRADING_PROVIDER_ID = entity.TradingProviderId,
                pv_BOND_SECONDARY_ID = entity.SecondaryId,
                pv_PRICE_DATE = entity.PriceDate,
                pv_PRICE = entity.Price,
                SESSION_USERNAME = entity.CreatedBy
            }, false);
        }

        public int Delete(int bondSecondaryId, int tradingProviderId)
        {
            return _oracleHelper.ExecuteProcedureNonQuery(PROC_DELETE_SECOND_PRICE, new
            {
                pv_BOND_SECONDARY_ID = bondSecondaryId,
                pv_TRADING_PROVIDER_ID = tradingProviderId
            });
        }

        public BondSecondPrice FindByDate(int bondSecondaryId, DateTime priceDate, int? tradingProviderId = null)
        {
            return _oracleHelper.ExecuteProcedureToFirst<BondSecondPrice>(PROC_BOND_SECOND_PRICE_FIND_BY_DATE, new
            {
                pv_BOND_SECONDARY_ID = bondSecondaryId,
                pv_PRICE_DATE = priceDate,
                pv_TRADING_PROVIDER_ID = tradingProviderId
            });
        }

        public PagingResult<BondSecondPrice> FindAll(int pageSize, int pageNumber, int bondSecondaryId, int tradingProviderId)
        {
            return _oracleHelper.ExecuteProcedurePaging<BondSecondPrice>(PROC_GET_ALL_SECOND_PRICE, new
            {
                PAGE_SIZE = pageSize,
                PAGE_NUMBER = pageNumber,
                pv_BOND_SECONDARY_ID = bondSecondaryId,
                pv_TRADING_PROVIDER_ID = tradingProviderId
            });
        }

        public int Update(BondSecondPrice entity)
        {
            return _oracleHelper.ExecuteProcedureNonQuery(PROC_BOND_SECOND_PRICE_UPDATE, new
            {
                pv_PRICE_ID = entity.Id,
                pv_TRADING_PROVIDER_ID = entity.TradingProviderId,
                pv_BOND_SECONDARY_ID = entity.SecondaryId,
                pv_PRICE_DATE = entity.PriceDate,
                pv_PRICE = entity.Price,
                SESSION_USERNAME = entity.ModifiedBy,
            });
        }
    }
}

