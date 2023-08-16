using EPIC.BondEntities.DataEntities;
using EPIC.DataAccess;
using EPIC.DataAccess.Models;
using EPIC.Entities.DataEntities;
using EPIC.Entities.Dto.DepositProvider;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.BondRepositories
{
    public class BondDepositProviderRepository
    {
        private OracleHelper _oracleHelper;
        private ILogger _logger;
        private static string ADD_DEPOSIT_PROVIDER_PROC = "PKG_DEPOSIT_PROVIDER.PROC_DEPOSIT_PROVIDER_ADD";
        private static string UPDATE_DEPOSIT_PROVIDER_PROC = "PKG_DEPOSIT_PROVIDER.PROC_DEPOSIT_PROVIDER_UPDATE";
        private static string DELETE_DEPOSIT_PROVIDER_PROC = "PKG_DEPOSIT_PROVIDER.PROC_DEPOSIT_PROVIDER_DELETE";
        private static string GET_ALL_DEPOSIT_PROVIDER_PROC = "PKG_DEPOSIT_PROVIDER.PROC_DEPOSIT_PROVIDER_GET_ALL";
        private static string GET_DEPOSIT_PROVIDER_PROC = "PKG_DEPOSIT_PROVIDER.PROC_DEPOSIT_PROVIDER_GET";
        private static string ACTIVE_DEPOSIT_PROVIDER_PROC = "PKG_DEPOSIT_PROVIDER.PROC_DEPOSIT_PROVIDER_ACTIVATE";
        public BondDepositProviderRepository(string connectionString, ILogger logger)
        {
            _oracleHelper = new OracleHelper(connectionString, logger);
            _logger = logger;
        }
        public BondDepositProvider Add(BondDepositProvider entity)
        {
            _logger.LogInformation("Add DepositProvider - SQL: {}", ADD_DEPOSIT_PROVIDER_PROC);
            return _oracleHelper.ExecuteProcedureToFirst<BondDepositProvider>(
                ADD_DEPOSIT_PROVIDER_PROC,
                new
                {
                    pv_BUSINESS_CUSTOMER_ID = entity.BusinessCustomerId,
                    pv_PARTNER_ID = entity.PartnerId,
                    SESSION_USERNAME = entity.CreatedBy
                }
             );
        }

        public int Delete(int id)
        {
            _logger.LogInformation("Delete DepositProvider - SQL: {}", DELETE_DEPOSIT_PROVIDER_PROC);
            var result = _oracleHelper.ExecuteProcedureNonQuery(DELETE_DEPOSIT_PROVIDER_PROC, new 
            {
                pv_DEPOSIT_PROVIDER_ID = id 
            });
            return result;
        }

        public List<BondDepositProvider> Filter(Func<Predicate<BondDepositProvider>, bool> expression)
        {
            return new List<BondDepositProvider>();
        }
        public PagingResult<DepositProviderDto> FindAll(int partnerId, int pageSize, int pageNumber, string keyword, string status)
        {
            _logger.LogInformation("FindAll DepositProvider - SQL: {}", GET_ALL_DEPOSIT_PROVIDER_PROC);
            var result = _oracleHelper.ExecuteProcedurePaging<DepositProviderDto>(GET_ALL_DEPOSIT_PROVIDER_PROC, new
            {
                pv_PARTNER_ID = partnerId,
                PAGE_SIZE = pageSize,
                PAGE_NUMBER = pageNumber,
                KEY_WORD = keyword,
                pv_STATUS = status
            });
            return result;
        }


        public BondDepositProvider FindById(int id)
        {
            _logger.LogInformation("FindById DepositProvider - SQL: {}", GET_DEPOSIT_PROVIDER_PROC);
            var result = _oracleHelper.ExecuteProcedureToFirst<BondDepositProvider>(GET_DEPOSIT_PROVIDER_PROC, new 
            {
                pv_DEPOSIT_PROVIDER_ID = id 
            });
            return result;
        }

        public DepositProviderDto FindDepositProviderById(int id)
        {
            _logger.LogInformation("FindById DepositProvider - SQL: {}", GET_DEPOSIT_PROVIDER_PROC);
            var result = _oracleHelper.ExecuteProcedureToFirst<DepositProviderDto>(GET_DEPOSIT_PROVIDER_PROC, new
            {
                pv_DEPOSIT_PROVIDER_ID = id
            });
            return result;
        }

        public int Active(int depositPoviderId, bool isActive)
        {
            return _oracleHelper.ExecuteProcedureNonQuery(ACTIVE_DEPOSIT_PROVIDER_PROC, new
            {
                pv_DEPOSIT_PROVIDER_ID = depositPoviderId,
                pv_STATUS = isActive,
            });
        }
        public int Update(BondDepositProvider entity)
        {
            _logger.LogInformation("Update DepositProvider - SQL: {}", UPDATE_DEPOSIT_PROVIDER_PROC);
            var result = _oracleHelper.ExecuteProcedureNonQuery(UPDATE_DEPOSIT_PROVIDER_PROC, new
            {
                pv_DEPOSIT_PROVIDER_ID = entity.Id,
                SESSION_USERNAME = entity.ModifiedBy,
            });
            return result;
        }
    }
}
