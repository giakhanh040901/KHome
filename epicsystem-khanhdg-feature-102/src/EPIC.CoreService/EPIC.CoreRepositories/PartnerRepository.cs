using EPIC.DataAccess;
using EPIC.DataAccess.Models;
using EPIC.Entities.DataEntities;
using EPIC.Entities.Dto.TradingProvider;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.CoreRepositories
{
    public class PartnerRepository
    {
        private OracleHelper _oracleHelper;
        private ILogger _logger;
        private static string GET_ALL_PARTNER_PROC = "EPIC.PKG_PARTNER.PROC_PARTNER_GET_ALL";
        private static string DELETE_PARTNER_PROC = "EPIC.PKG_PARTNER.PROC_PARTNER_DELETE";
        private static string UPDATE_PARTNER_PROC = "EPIC.PKG_PARTNER.PROC_PARTNER_UPDATE";
        private static string ADD_PARTNER_PROC = "EPIC.PKG_PARTNER.PROC_PARTNER_ADD";

        #region find
        private static string FIND_BY_ID_PARTNER_PROC = "EPIC.PKG_PARTNER.PROC_PARTNER_GET";
        private static string PROC_GET_BY_INVESTOR_ID = "EPIC.PKG_PARTNER.PROC_GET_BY_INVESTOR_ID";
        private static string PROC_GET_BY_TRADING_PROVIDER_ID = "EPIC.PKG_PARTNER.PROC_GET_BY_TRADING_ID";
        private static string PROC_FIND_TRADING_BY_PARTNER = "EPIC.PKG_TRADING_PROVIDER.PROC_FIND_TRADING_BY_PARTNER";
        #endregion

        public PartnerRepository(string connectionString, ILogger logger)
        {
            _oracleHelper = new OracleHelper(connectionString, logger);
            _logger = logger;
        }

        public Partner Add(Partner entity)
        {
            _logger.LogInformation("Add Partner");
            return _oracleHelper.ExecuteProcedureToFirst<Partner>(ADD_PARTNER_PROC, new
            {
                pv_CODE = entity.Code,
                pv_NAME = entity.Name,
                pv_SHORT_NAME = entity.ShortName,
                pv_ADDRESS = entity.Address,
                pv_PHONE = entity.Phone,
                pv_MOBILE = entity.Mobile,
                pv_EMAIL = entity.Email,
                pv_TAX_CODE = entity.TaxCode,
                pv_LICENSE_DATE = entity.LicenseDate,
                pv_LICENSE_ISSUER = entity.LicenseIssuer,
                pv_CAPITAL = entity.Capital,
                pv_REP_NAME = entity.RepName,
                pv_REP_POSITION = entity.RepPosition,
                pv_TRADING_ADDRESS = entity.TradingAddress,
                pv_NATION = entity.Nation,
                pv_DECISION_NO = entity.DecisionNo,
                pv_DECISION_DATE = entity.DecisionDate,
                pv_NUMBER_MODIFIED = entity.NumberModified,
                pv_DATE_MODIFIED = entity.DateModified,
                SESSION_USERNAME = entity.CreatedBy,
            });
        }

        public int Delete(int id)
        {
            _logger.LogInformation($"Delete Partner: {id}");
            var result = _oracleHelper.ExecuteProcedureNonQuery(DELETE_PARTNER_PROC, new
            {
                pv_PARTNER_ID = id
            });
            return result;
        }

        public PagingResult<Partner> FindAll(int pageSize, int pageNumber, string keyword)
        {
            var partner = _oracleHelper.ExecuteProcedurePaging<Partner>(GET_ALL_PARTNER_PROC, new
            {
                PAGE_SIZE = pageSize,
                PAGE_NUMBER = pageNumber,
                KEY_WORD = keyword
            });
            return partner;
        }

        public Partner FindById(int id)
        {
            Partner partner = _oracleHelper.ExecuteProcedureToFirst<Partner>(FIND_BY_ID_PARTNER_PROC, new
            {
                pv_PARTNER_ID = id,
            });
            return partner;
        }

        public int Update(Partner entity)
        {
            var result = _oracleHelper.ExecuteProcedureNonQuery(UPDATE_PARTNER_PROC, new
            {
                pv_PARTNER_ID = entity.PartnerId,
                pv_CODE = entity.Code,
                pv_NAME = entity.Name,
                pv_SHORT_NAME = entity.ShortName,
                pv_ADDRESS = entity.Address,
                pv_PHONE = entity.Phone,
                pv_MOBILE = entity.Mobile,
                pv_EMAIL = entity.Email,
                pv_TAX_CODE = entity.TaxCode,
                pv_LICENSE_DATE = entity.LicenseDate,
                pv_LICENSE_ISSUER = entity.LicenseIssuer,
                pv_CAPITAL = entity.Capital,
                pv_REP_NAME = entity.RepName,
                pv_REP_POSITION = entity.RepPosition,
                pv_TRADING_ADDRESS = entity.TradingAddress,
                pv_NATION = entity.Nation,
                pv_DECISION_NO = entity.DecisionNo,
                pv_DECISION_DATE = entity.DecisionDate,
                pv_NUMBER_MODIFIED = entity.NumberModified,
                pv_DATE_MODIFIED = entity.DateModified,
                SESSION_USERNAME = entity.ModifiedBy,
            });
            return result;
        }

        /// <summary>
        /// Lấy partner theo investor id
        /// </summary>
        /// <param name="investorId"></param>
        /// <returns></returns>
        public Partner FindByInvestorId(int investorId)
        {
            return _oracleHelper.ExecuteProcedureToFirst<Partner>(PROC_GET_BY_INVESTOR_ID, new
            {
                pv_INVESTOR_ID = investorId,
            });
        }

        /// <summary>
        /// Lấy partner theo trading provider
        /// </summary>
        /// <param name="tradingProviderId"></param>
        /// <returns></returns>
        public Partner FindByTradingProviderId(int tradingProviderId)
        {
            return _oracleHelper.ExecuteProcedureToFirst<Partner>(PROC_GET_BY_TRADING_PROVIDER_ID, new
            {
                pv_TRADING_PROVIDER_ID = tradingProviderId,
            });
        }

        public List<TradingProviderDto> FindTradingProviderByPartner(int partnerId)
        {
            var result = _oracleHelper.ExecuteProcedure<TradingProviderDto>(PROC_FIND_TRADING_BY_PARTNER, new
            {
                pv_PARTNER_ID = partnerId
            }).ToList();
            return result;
        }
    }
 }
