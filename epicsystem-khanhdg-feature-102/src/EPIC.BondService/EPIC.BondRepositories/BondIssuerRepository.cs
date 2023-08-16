using EPIC.BondEntities.DataEntities;
using EPIC.DataAccess;
using EPIC.DataAccess.Models;
using EPIC.Entities.DataEntities;
using EPIC.Entities.Dto.Issuer;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.BondRepositories
{
    public class BondIssuerRepository
    {
        private OracleHelper _oracleHelper;
        private ILogger _logger;
        private static string ADD_ISSUER_PROC = "PKG_ISSUER.PROC_EP_ISSUER_ADD";
        private static string UPDATE_ISSUER_PROC = "PKG_ISSUER.PROC_EP_ISSUER_UPDATE";
        private static string GET_ALL_ISSUER_PROC = "PKG_ISSUER.PROC_EP_ISSUER_GET_ALL";
        private static string GET_ISSUER_PROC = "PKG_ISSUER.PROC_EP_ISSUER_GET";
        private static string DELETE_ISSUER_PROC = "PKG_ISSUER.PROC_EP_ISSUER_DELETE";

        public BondIssuerRepository(string connectionString, ILogger logger)
        {
            _oracleHelper = new OracleHelper(connectionString, logger);
            _logger = logger;
        }
        public BondIssuer Add(BondIssuer entity)
        {
            _logger.LogInformation("Add Issuer");
            return _oracleHelper.ExecuteProcedureToFirst<BondIssuer>(
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
            _logger.LogInformation("Delete Issuer");
            var rslt = _oracleHelper.ExecuteProcedureNonQuery(DELETE_ISSUER_PROC, new
            {
                pv_ISSUER_ID = id
            });
            return rslt;
        }

        public List<BondIssuer> Filter(Func<Predicate<BondIssuer>, bool> expression)
        {
            throw new NotImplementedException();
        }

        public PagingResult<ViewIssuerDto> FindAll(int partnerId, int pageSize, int pageNumber, string keyword, string status)
        {
            var issuers = _oracleHelper.ExecuteProcedurePaging<ViewIssuerDto>(GET_ALL_ISSUER_PROC, new
            {
                pv_PARTNER_ID = partnerId,
                pageSize,
                pageNumber,
                keyword,
                pv_STATUS = status
            });
            return issuers;
        }

        public List<BondIssuer> GetAll()
        {
            throw new NotImplementedException();
        }

        public BondIssuer FindIssuerById(int id)
        {
            var result = _oracleHelper.ExecuteProcedureToFirst<BondIssuer>(GET_ISSUER_PROC, new
            {
                pv_ISSUER_ID = id,
            });
            return result;
        }

        public BondIssuer FindById(int id)
        {
            var issuer = _oracleHelper.ExecuteProcedureToFirst<BondIssuer>(GET_ISSUER_PROC, new
            {
                pv_ISSUER_ID = id,
            });
            return issuer;
        }

        public int Update(BondIssuer entity)
        {
            var result = _oracleHelper.ExecuteProcedureNonQuery(UPDATE_ISSUER_PROC, new
            {
                pv_ISSUER_ID = entity.Id,
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
    }
}
