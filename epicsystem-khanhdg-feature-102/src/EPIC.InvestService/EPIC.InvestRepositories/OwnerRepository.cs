using EPIC.DataAccess;
using Microsoft.Extensions.Logging;
using EPIC.InvestEntities;
using EPIC.DataAccess.Models;
using EPIC.InvestEntities.DataEntities;
using EPIC.InvestEntities.Dto.Owner;

namespace EPIC.InvestRepositories
{
    public class OwnerRepository
    {
        private readonly OracleHelper _oracleHelper;
        private readonly ILogger _logger;
        private static string PROC_OWNER_ADD = "PKG_INV_OWNER.PROC_OWNER_ADD";
        private static string PROC_OWNER_UPDATE = "PKG_INV_OWNER.PROC_OWNER_UPDATE";
        private static string PROC_OWNER_GET_ALL = "PKG_INV_OWNER.PROC_OWNER_GET_ALL";
        private static string PROC_OWNER_GET = "PKG_INV_OWNER.PROC_OWNER_GET";
        private static string PROC_OWNER_DELETE = "PKG_INV_OWNER.PROC_OWNER_DELETE";

        public OwnerRepository(string connectionString, ILogger logger)
        {
            _oracleHelper = new OracleHelper(connectionString, logger);
            _logger = logger;
        }
        public void CloseConnection()
        {
            _oracleHelper.CloseConnection();
        }

        public Owner Add(Owner entity)
        {
            _logger.LogInformation("Add Issuer");
            return _oracleHelper.ExecuteProcedureToFirst<Owner>(PROC_OWNER_ADD, new
                {
                    pv_BUSINESS_CUSTOMER_ID = entity.BusinessCustomerId,
                    pv_PARTNER_ID = entity.PartnerId,
                    pv_BUSINESS_TURNOVER = entity.BusinessTurnover,
                    pv_BUSINESS_PROFIT = entity.BusinessProfit,
                    pv_ROA = entity.Roa,
                    pv_ROE = entity.Roe,
                    pv_IMAGE = entity.Image,
                    pv_WEBSITE = entity.Website,
                    pv_HOTLINE = entity.Hotline,
                    pv_FANPAGE = entity.Fanpage,
                    SESSION_USERNAME = entity.CreatedBy,
                });
        }

        public int Delete(int id, int partnerId)
        {
            _logger.LogInformation("Delete Owner");
            var rslt = _oracleHelper.ExecuteProcedureNonQuery(PROC_OWNER_DELETE, new
            {
                pv_ID = id,
                pv_PARTNER_ID = partnerId
            });
            return rslt;
        }

        public PagingResult<OwnerDto> FindAll(int partnerId, int pageSize, int pageNumber, string keyword, int? status)
        {
            var result = _oracleHelper.ExecuteProcedurePaging<OwnerDto>(PROC_OWNER_GET_ALL, new
            {
                pv_PARTNER_ID = partnerId,
                PAGE_SIZE = pageSize,
                PAGE_NUMBER = pageNumber,
                keyword = keyword,
                pv_STATUS = status
            });
            return result;
        }

        public ViewOwnerDto FindById(int id)
        {
            var result = _oracleHelper.ExecuteProcedureToFirst<ViewOwnerDto>(PROC_OWNER_GET, new
            {
                pv_ID = id,
            });
            return result;
        }

        public int Update(Owner entity)
        {
            var result = _oracleHelper.ExecuteProcedureNonQuery(PROC_OWNER_UPDATE, new
            {
                pv_ID = entity.Id,
                pv_PARTNER_ID = entity.PartnerId,
                pv_BUSINESS_TURNOVER = entity.BusinessTurnover,
                pv_BUSINESS_PROFIT = entity.BusinessProfit,
                pv_ROA = entity.Roa,
                pv_ROE = entity.Roe,
                pv_IMAGE = entity.Image,
                pv_WEBSITE = entity.Website,
                pv_HOTLINE = entity.Hotline,
                pv_FANPAGE = entity.Fanpage,
                SESSION_USERNAME = entity.ModifiedBy,
            });
            return result;
        }
    }
}
