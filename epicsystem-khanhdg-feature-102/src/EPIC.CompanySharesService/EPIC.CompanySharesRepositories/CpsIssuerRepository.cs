using EPIC.CompanySharesEntities.DataEntities;
using EPIC.CompanySharesEntities.Dto.Issuer;
using EPIC.DataAccess;
using EPIC.DataAccess.Models;
using EPIC.Utils.ConstantVariables.Shared;
using Microsoft.Extensions.Logging;

namespace EPIC.CompanySharesRepositories
{
    public class CpsIssuerRepository
    {
        private OracleHelper _oracleHelper;
        private ILogger _logger;
        private static string schemas = DbSchemas.EPIC_COMPANY_SHARES + ".";
        private static string ADD_ISSUER_PROC = schemas + "PKG_CPS_ISSUER.PROC_CPS_ISSUER_ADD";
        private static string UPDATE_ISSUER_PROC = schemas + "PKG_CPS_ISSUER.PROC_CPS_ISSUER_UPDATE";
        private static string GET_ALL_ISSUER_PROC = schemas + "PKG_CPS_ISSUER.PROC_CPS_ISSUER_GET_ALL";
        private static string GET_ISSUER_PROC = schemas + "PKG_CPS_ISSUER.PROC_CPS_ISSUER_GET";
        private static string DELETE_ISSUER_PROC = schemas + "PKG_CPS_ISSUER.PROC_CPS_ISSUER_DELETE";

        public CpsIssuerRepository(string connectionString, ILogger logger)
        {
            _oracleHelper = new OracleHelper(connectionString, logger);
            _logger = logger;
        }

        public CpsIssuer Add(CpsIssuer entity)
        {
            return _oracleHelper.ExecuteProcedureToFirst<CpsIssuer>(
                ADD_ISSUER_PROC,
                new
                {
                    pv_BUSINESS_CUSTOMER_ID = entity.BusinessCustomerId,
                    pv_PARTNER_ID = entity.PartnerId,
                    pv_BUSINESS_TURNOVER = entity.BusinessTurnover,
                    pv_BUSINESS_PROFIT = entity.BusinessProfit,
                    pv_ROA = entity.ROA,
                    pv_ROE = entity.ROE,
                    pv_IMAGE = entity.Image,
                    SESSION_USERNAME = entity.CreatedBy,
                });
        }

        public int Delete(int id)
        {
            var rslt = _oracleHelper.ExecuteProcedureNonQuery(DELETE_ISSUER_PROC, new
            {
                pv_ID = id
            });
            return rslt;
        }

        public int Update(CpsIssuer entity)
        {
            var result = _oracleHelper.ExecuteProcedureNonQuery(UPDATE_ISSUER_PROC, new
            {
                pv_ID = entity.Id,
                pv_PARTNER_ID = entity.PartnerId,
                pv_BUSINESS_TURNOVER = entity.BusinessTurnover,
                pv_BUSINESS_PROFIT = entity.BusinessProfit,
                pv_ROA = entity.ROA,
                pv_ROE = entity.ROE,
                pv_IMAGE = entity.Image,
                SESSION_USERNAME = entity.ModifiedBy,
            });
            return result;
        }

        public CpsIssuer FindIssuerById(int id)
        {
            var result = _oracleHelper.ExecuteProcedureToFirst<CpsIssuer>(GET_ISSUER_PROC, new
            {
                pv_ID = id,
            });
            return result;
        }

        public PagingResult<ViewCpsIssuerDto> FindAll(int partnerId, int pageSize, int pageNumber, string keyword, string status)
        {
            var issuers = _oracleHelper.ExecuteProcedurePaging<ViewCpsIssuerDto>(GET_ALL_ISSUER_PROC, new
            {
                pv_PARTNER_ID = partnerId,
                pv_PAGESIZE = pageSize,
                pv_PAGENUMBER = pageNumber,
                pv_KEYWORD = keyword,
                pv_STATUS = status
            });
            return issuers;
        }
    }
}
