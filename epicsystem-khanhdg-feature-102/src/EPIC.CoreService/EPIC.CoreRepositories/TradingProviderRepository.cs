using EPIC.BondRepositories;
using EPIC.DataAccess;
using EPIC.DataAccess.Models;
using EPIC.Entities.DataEntities;
using EPIC.Entities.Dto.BusinessCustomer;
using EPIC.Entities.Dto.TradingProvider;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.CoreRepositories
{
    public class TradingProviderRepository
    {
        private OracleHelper _oracleHelper;
        private ILogger _logger;
        private static string GET_ALL_TRADING_PROVIDER_PROC = "EPIC.PKG_TRADING_PROVIDER.PROC_TRADING_PROVIDER_FIND";
        private static string DELETE_TRADING_PROVIDER_PROC = "EPIC.PKG_TRADING_PROVIDER.PROC_TRADING_PROVIDER_DELETE";
        private static string FIND_BY_ID_TRADING_PROVIDER_PROC = "EPIC.PKG_TRADING_PROVIDER.PROC_TRADING_PROVIDER_GET";
        private static string UPDATE_TRADING_PROVIDER_PROC = "EPIC.PKG_TRADING_PROVIDER.PROC_TRADING_PROVIDER_UPDATE";
        private static string ADD_TRADING_PROVIDER_PROC = "EPIC.PKG_TRADING_PROVIDER.PROC_TRADING_PROVIDER_ADD";
        private static string PROC_TRADING_PROVIDER_TAX_CODE = "EPIC.PKG_TRADING_PROVIDER.PROC_TRADING_PROVIDER_TAX_CODE";
        private static string PROC_GET_LIST_BANK_BY_TRADING = "EPIC.PKG_TRADING_PROVIDER.PROC_GET_LIST_BANK_BY_TRADING";
        private static string PROC_GET_TRADING_DIGITAL_SIGN = "EPIC.PKG_TRADING_PROVIDER.PROC_GET_TRADING_DIGITAL_SIGN";
        private static string PROC_UPDATE_TRADING_DIGITAL_SIGN = "EPIC.PKG_TRADING_PROVIDER.PROC_UPDATE_TRADING_DIGITAL_SIGN";

        public TradingProviderRepository(string connectionString, ILogger logger)
        {
            _oracleHelper = new OracleHelper(connectionString, logger);
            _logger = logger;
        }

        public TradingProvider Add(TradingProvider entity,int PartnerId)
        {
            _logger.LogInformation("Add Trading Provider");
            return _oracleHelper.ExecuteProcedureToFirst<TradingProvider>(ADD_TRADING_PROVIDER_PROC, new
            {
                pv_BUSINESS_CUSTOMER_ID = entity.BusinessCustomerId,
                pv_PARTNER_ID = PartnerId,
                SESSION_USERNAME = entity.CreatedBy,
                pv_ALIAS_NAME = entity.AliasName
            });
        }

        public int Delete(int id)
        {
            _logger.LogInformation($"Delete Trading Provider: {id}");
            var result = _oracleHelper.ExecuteProcedureNonQuery(DELETE_TRADING_PROVIDER_PROC, new
            {
                pv_TRADING_PROVIDER_ID = id
            });
            return result;
        }

        public PagingResult<TradingProviderDto> FindAll(int? partnerId, int pageSize, int pageNumber, string keyword, int? status)
        {
            var tradingProvider = _oracleHelper.ExecuteProcedurePaging<TradingProviderDto>(GET_ALL_TRADING_PROVIDER_PROC, new 
            { 
                pv_PARTNER_ID = partnerId,
                PAGE_SIZE = pageSize, 
                PAGE_NUMBER = pageNumber,
                KEY_WORD = keyword,
                pv_STATUS = status
            });
            return tradingProvider; 
        }

        public TradingProvider FindById(int id)
        {
            TradingProvider tradingProvider = _oracleHelper.ExecuteProcedureToFirst<TradingProvider>(FIND_BY_ID_TRADING_PROVIDER_PROC, new
            {
                pv_TRADING_PROVIDER_ID = id,
            });
            return tradingProvider;
        }

        public List<ViewTradingProviderDto> FindByTaxCode(string taxCode)
        {
            var result = _oracleHelper.ExecuteProcedure<ViewTradingProviderDto>(PROC_TRADING_PROVIDER_TAX_CODE, new
            {
                pv_TAX_CODE = taxCode,
            }).ToList();
            return result;
        }

        public TradingProviderDto FindTradingProviderById(int id)
        {
            TradingProviderDto tradingProvider = _oracleHelper.ExecuteProcedureToFirst<TradingProviderDto>(FIND_BY_ID_TRADING_PROVIDER_PROC, new
            {
                pv_TRADING_PROVIDER_ID = id,
            });
            return tradingProvider;
        }

        public int Update(TradingProvider entity)
        {
            var result = _oracleHelper.ExecuteProcedureNonQuery(UPDATE_TRADING_PROVIDER_PROC, new
            {
                pv_TRADING_PROVIDER_ID = entity.TradingProviderId,
                SESSION_USERNAME = entity.ModifiedBy,
            });
            return result;
        }

        public List<BusinessCustomerBankDto> FindBankByTrading(int tradingProviderId, int? distributionId = null, int? type = null, int? bankId = null)
        {
            var result = _oracleHelper.ExecuteProcedure<BusinessCustomerBankDto>(PROC_GET_LIST_BANK_BY_TRADING, new
            {
                pv_TRADING_PROVIDER_ID = tradingProviderId,
                pv_DISTRIBUTION_ID = distributionId,
                pv_TYPE = type,
            }).Where(e => (bankId == null || e.BankId == bankId)).ToList();
            return result;
        }

        /// <summary>
        /// Lấy dữ liệu chữ ký số
        /// </summary>
        /// <param name="tradingProviderId"></param>
        /// <returns></returns>
        public DigitalSign GetDigitalSign(int? tradingProviderId)
        {
            _logger.LogInformation("Get DigitalSign");
            var result = _oracleHelper.ExecuteProcedureToFirst<DigitalSign>(PROC_GET_TRADING_DIGITAL_SIGN, new
            {
                pv_TRADING_PROVIDER_ID = tradingProviderId,
            });
            return result;
        }

        public DigitalSign UpdateDigitalSign(int? tradingProviderId, DigitalSign input)
        {
            _logger.LogInformation("Update DigitalSign");
            var result = _oracleHelper.ExecuteProcedureToFirst<DigitalSign>(PROC_UPDATE_TRADING_DIGITAL_SIGN, new
            {
                pv_TRADING_PROVIDER_ID = tradingProviderId,
                pv_SERVER = input.Server,
                pv_KEY = input.Key,
                pv_SECRET = input.Secret,
                pv_STAMP_IMAGE_URL = input.StampImageUrl,
                SESSION_USERNAME = input.ModifiedBy
            });
            return result;
        }
    }
}
