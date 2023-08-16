using EPIC.DataAccess;
using EPIC.DataAccess.Models;
using EPIC.Entities;
using EPIC.IdentityEntities.DataEntities;
using EPIC.Utils;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.IdentityRepositories
{
    public class TradingProviderUserRepository
    {
        private readonly OracleHelper _oracleHelper;
        private readonly ILogger _logger;

        private const string PROC_TRADING_PROVIDER_USER_FIND_ALL = "PKG_USERS.PROC_TP_USER_FIND_ALL";
        private const string PROC_TRADING_PROVIDER_USER_ADD = "PKG_USERS.PROC_TRADING_PROVIDER_USER_ADD";
        private const string PROC_TRADING_PROVIDER_USER_ACTIVATE = "PKG_USERS.PROC_TP_USER_ACTIVATE";
        private const string PROC_TRADING_PROVIDER_USER_CHANGE_PASSWORD = "PKG_USERS.PROC_TP_USER_CHANGE_PASSWORD";
        private const string PROC_TRADING_PROVIDER_USER_UPDATE = "PKG_USERS.PROC_TP_USER_UPDATE";
        private const string PROC_TRADING_PROVIDER_USER_DELETE = "PKG_USERS.PROC_TP_USER_DELETE";

        public TradingProviderUserRepository(string connectionString, ILogger logger)
        {
            _oracleHelper = new OracleHelper(connectionString, logger);
            _logger = logger;
        }

        public Users Add(Users entity)
        {
            string addTradingProviderUserProc = PROC_TRADING_PROVIDER_USER_ADD;
            return _oracleHelper.ExecuteProcedureToFirst<Users>(
                addTradingProviderUserProc,
                new
                {
                    pv_TRADING_PROVIDER_ID = entity.TradingProviderId,
                    pv_USERNAME = entity.UserName,
                    pv_DISPLAY_NAME = entity.DisplayName,
                    pv_PASSWORD = CommonUtils.CreateMD5(entity.Password),
                    pv_EMAIL = entity.Email,
                    pv_CREATED_BY = entity.CreatedBy
                }
             );
        }

        public int Active(int id, bool isActive, string modifiedBy, int tradingProviderId)
        {
            return _oracleHelper.ExecuteProcedureNonQuery(PROC_TRADING_PROVIDER_USER_ACTIVATE, new
            {
                pv_ID = id,
                pv_IS_ACTIVE = isActive,
                pv_MODIFIED_BY = modifiedBy,
                pv_TRADING_PROVIDER_ID = tradingProviderId,
            });
        }

        public int ChangePassword(int userId, string oldPassword, string newPassword, int tradingProviderId)
        {
            return _oracleHelper.ExecuteProcedureNonQuery(PROC_TRADING_PROVIDER_USER_CHANGE_PASSWORD, new
            {
                pv_USER_ID = userId,
                pv_OLD_PASSWORD = CommonUtils.CreateMD5(oldPassword),
                pv_NEW_PASSWORD = CommonUtils.CreateMD5(newPassword),
                pv_TRADING_PROVIDER_ID = tradingProviderId
            });
        }

        public int Delete(int id, string deletedBy, int tradingProviderId)
        {
            return _oracleHelper.ExecuteProcedureNonQuery(PROC_TRADING_PROVIDER_USER_DELETE, new
            {
                pv_TRADING_PROVIDER_ID = tradingProviderId,
                pv_Id = id,
                pv_DELETED_BY = deletedBy
            });
        }

        public List<Users> Filter(Func<Predicate<Users>, bool> expression)
        {
            throw new NotImplementedException();
        }

        public PagingResult<Users> FindAll(int pageSize, int pageNumber, string keyword)
        {
            var result = _oracleHelper.ExecuteProcedurePaging<Users>(PROC_TRADING_PROVIDER_USER_FIND_ALL, new
            {
                pv_PAGE_SIZE = pageSize,
                pv_PAGE_NUMBER = pageNumber,
                pv_KEY_WORD = keyword
            });
            return result;
        }

        public Users FindById(int id)
        {
            throw new NotImplementedException();
        }

        public List<Users> GetAll()
        {
            throw new NotImplementedException();
        }

        public int Update(Users entity)
        {
            return _oracleHelper.ExecuteProcedureNonQuery(
                PROC_TRADING_PROVIDER_USER_UPDATE,
                new
                {
                    pv_TRADING_PROVIDER_ID = entity.TradingProviderId,
                    pv_ID = entity.UserId,
                    pv_DISPLAY_NAME = entity.DisplayName,
                    pv_EMAIL = entity.Email,
                    pv_MODIFIED_BY = entity.ModifiedBy
                }
             );
        }

        public int Delete(int id)
        {
            throw new NotImplementedException();
        }
    }
}
