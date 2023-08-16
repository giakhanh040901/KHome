using AutoMapper.Execution;
using EPIC.DataAccess;
using EPIC.DataAccess.Models;
using EPIC.Entities;
using EPIC.Entities.DataEntities;
using EPIC.Entities.Dto.BondInvestorAccount;
using EPIC.Entities.Dto.RocketChat;
using EPIC.Entities.Dto.User;
using EPIC.IdentityEntities.DataEntities;
using EPIC.IdentityEntities.Dto.UsersChat;
using EPIC.IdentityEntities.Dto.UsersTradingProvider;
using EPIC.Utils;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.IdentityRepositories
{
    public class UserRepository
    {
        private readonly OracleHelper _oracleHelper;
        private readonly ILogger _logger;

        private const string PROC_USER_FIND_ALL = "PKG_USERS.PROC_USER_FIND_ALL";
        private const string PROC_USER_ADD = "PKG_USERS.PROC_USER_ADD";
        private const string PROC_USER_DELETE = "PKG_USERS.PROC_USER_DELETE";
        private const string PROC_USER_GET_BY_ID = "PKG_USERS.PROC_USER_GET_BY_ID";
        private const string PROC_USER_FIND_BY_INVESTOR_ID = "PKG_USERS.PROC_USER_FIND_BY_INVES_ID";
        private const string PROC_USER_UPDATE = "PKG_USERS.PROC_USER_UPDATE";
        private const string PROC_USER_GET_BY_USERNAME = "PKG_USERS.PROC_USER_GET_BY_USERNAME";
        private const string PROC_USER_ACTIVATE = "PKG_USERS.PROC_USER_ACTIVATE";
        private const string PROC_USER_CHANGE_PASSWORD = "PKG_USERS.PROC_USER_CHANGE_PASSWORD";
        private const string PROC_USER_TRADING_PROVIDER_ADD = "PKG_USERS_TRADING_PROVIDER.PROC_USER_TRADING_PROVIDER_ADD";
        private const string PROC_USER_CHANGE_PASSWORD_TEMP = "PKG_USERS.PROC_USER_CHANGE_PASSWORD_TEMP";
        private const string PROC_USER_GET_BY_TYPE_IS_INVESTOR = "PKG_USERS.PROC_USER_GET_BY_TYPE";
        private const string PROC_USER_ACCOUNT_INVESTOR = "PKG_USERS.PROC_USER_ACCOUNT_INVESTOR";
        private const string PROC_USER_GET_PARTNER_ALL = "PKG_USERS.PROC_USER_GET_PARTNER_ALL";
        private const string PROC_USER_GET_TRADING_ALL = "PKG_USERS.PROC_USER_GET_TRADING_ALL";
        private const string PROC_USER_GET_OTP_BY_ID = "PKG_USERS.PROC_USER_OTP_BY_ID";
        private const string PROC_USER_GET_OTP_BY_PHONE = "PKG_USERS.PROC_USER_OTP_BY_PHONE";
        private const string PROC_LOGIN = "PKG_USERS.PROC_LOGIN";
        private const string PROC_USER_GET_LIST_TRADINGS = "PKG_USERS.PROC_USER_GET_LIST_TRADINGS";
        private const string PROC_APP_CHAT_SAVE_ROOM_INFO = "PKG_USERS.PROC_APP_CHAT_SAVE_ROOM_INFO";
        private const string PROC_APP_CHAT_GET_ROOM_INFO = "PKG_USERS.PROC_APP_CHAT_GET_ROOM_INFO";

        #region fcm token
        private const string PROC_SAVE_FCM_TOKEN = "PKG_USERS.PROC_SAVE_FCM_TOKEN";
        private const string PROC_GET_FCM_TOKEN = "PKG_USERS.PROC_GET_FCM_TOKEN";
        private const string PROC_DELETE_FCM_TOKEN = "PKG_USERS.PROC_DELETE_FCM_TOKEN";
        #endregion


        public UserRepository(string connectionString, ILogger logger)
        {
            _oracleHelper = new OracleHelper(connectionString, logger);
            _logger = logger;
        }

        public UserDto Add(Users entity)
        {
            return _oracleHelper.ExecuteProcedureToFirst<UserDto>(
                PROC_USER_ADD,
                new
                {
                    pv_USERNAME = entity.UserName,
                    pv_DISPLAY_NAME = entity.DisplayName,
                    pv_PASSWORD = CommonUtils.CreateMD5(entity.Password),
                    pv_EMAIL = entity.Email,
                    pv_PARTNER_ID = entity.PartnerId,
                    pv_USER_TYPE = entity.UserType,
                    pv_TRADING_PROVIDER_ID = entity.TradingProviderId,
                    pv_CREATED_BY = entity.CreatedBy
                }
             );
        }

        public int Delete(int id, string deletedBy)
        {
            return _oracleHelper.ExecuteProcedureNonQuery(PROC_USER_DELETE, new { 
                pv_Id = id,
                pv_DELETED_BY = deletedBy
            });
        }

        public List<Users> Filter(Func<Predicate<Users>, bool> expression)
        {
            return new List<Users>();
        }

        public List<Users> GetAll()
        {
            string findAllSQL = "SELECT * FROM USERS WHERE IS_DELETED = 'N'";
            IEnumerable<Users> usersList = _oracleHelper.ExecuteCommandText<Users>(findAllSQL, null);
            if (usersList != null)
            {
                return usersList.ToList();
            }
            else
            {
                return new List<Users>();
            }
        }


        public Users FindById(int id)
        {
            return _oracleHelper.ExecuteProcedureToFirst<Users>(PROC_USER_GET_BY_ID, new 
            {
                pv_ID = id
            });
        }

        public int Update(Users entity)
        {
            return _oracleHelper.ExecuteProcedureNonQuery(
                PROC_USER_UPDATE,
                new
                {
                    pv_ID = entity.UserId,
                    pv_DISPLAY_NAME = entity.DisplayName,
                    pv_EMAIL = entity.Email,
                    pv_MODIFIED_BY = entity.ModifiedBy
                }
             );
        }

        public Users FindByUserName(string username)
        {
            return _oracleHelper.ExecuteProcedureToFirst<Users>(PROC_USER_GET_BY_USERNAME, new
            {
                pv_USERNAME = username
            });
        }

        public int Active(decimal id, bool isActive, string modifiedBy)
        {
            return _oracleHelper.ExecuteProcedureNonQuery(PROC_USER_ACTIVATE, new
            {
                pv_ID = id,
                pv_IS_ACTIVE = isActive,
                pv_MODIFIED_BY = modifiedBy
            });
        }

        public PagingResult<Users> FindAll(int pageSize, int pageNumber, string keyword)
        {
            var result = _oracleHelper.ExecuteProcedurePaging<Users>(PROC_USER_FIND_ALL, new
            {
                pv_PAGE_SIZE = pageSize,
                pv_PAGE_NUMBER = pageNumber,
                pv_KEY_WORD = keyword
            });
            return result;
        }

        public PagingResult<Users> GetAllByPartnerId(int pageSize, int pageNumber, string keyword, int partnerId)
        {
            var result = _oracleHelper.ExecuteProcedurePaging<Users>(PROC_USER_GET_PARTNER_ALL, new
            {
                pv_PARTNER_ID = partnerId,
                pv_PAGE_SIZE = pageSize,
                pv_PAGE_NUMBER = pageNumber,
                pv_KEY_WORD = keyword
            });
            return result;
        }

        public PagingResult<Users> GetAllByTradingProviderId(int pageSize, int pageNumber, string keyword, int tradingProviderId)
        {
            var result = _oracleHelper.ExecuteProcedurePaging<Users>(PROC_USER_GET_TRADING_ALL, new
            {
                pv_TRADING_PROVIDER_ID = tradingProviderId,
                pv_PAGE_SIZE = pageSize,
                pv_PAGE_NUMBER = pageNumber,
                pv_KEY_WORD = keyword
            });
            return result;
        }

        public PagingResult<UserIsInvestorDto> GetByType(FindBondInvestorAccountDto dto, int? tradingProviderId, int? partnerId)
        {
            var result = _oracleHelper.ExecuteProcedurePaging<UserIsInvestorDto>(PROC_USER_GET_BY_TYPE_IS_INVESTOR, new
            {
                pv_PAGE_SIZE = dto.PageSize,
                pv_PAGE_NUMBER = dto.PageNumber,
                pv_KEY_WORD = dto.Keyword,
                pv_FULL_NAME = dto.Fullname,
                pv_PHONE = dto.Phone,
                pv_EMAIL = dto.Email,
                pv_SEX = dto.Sex,
                pv_START_AGE = dto.StartAge,
                pv_END_AGE = dto.EndAge,
                pv_CIF_CODE = dto.CifCode,
                pv_STATUS = dto.Status,
                pv_CUSTOMER_TYPE = dto.CustomerType,
                pv_TRADING_PROVIDER_ID = tradingProviderId,
                pv_PARTNER_ID = partnerId
            });
            return result;
        }

        public PagingResult<UserIsInvestorDto> GetInvestorAccount(FindBondInvestorAccountDto dto, int? tradingProviderId, int? partnerId)
        {
            var result = _oracleHelper.ExecuteProcedurePaging<UserIsInvestorDto>(PROC_USER_ACCOUNT_INVESTOR, new
            {
                pv_PAGE_SIZE = dto.PageSize,
                pv_PAGE_NUMBER = dto.PageNumber,
                pv_KEY_WORD = dto.Keyword,
                pv_FULL_NAME = dto.Fullname,
                pv_PHONE = dto.Phone,
                pv_EMAIL = dto.Email,
                pv_SEX = dto.Sex,
                pv_START_AGE = dto.StartAge,
                pv_END_AGE = dto.EndAge,
                pv_CIF_CODE = dto.CifCode,
                pv_STATUS = dto.Status,
                pv_CUSTOMER_TYPE = dto.CustomerType,
                pv_TRADING_PROVIDER_ID = tradingProviderId,
                pv_PARTNER_ID = partnerId
            });
            return result;
        }

        public int Delete(int id)
        {
            throw new NotImplementedException();
        }

        public int ChangePassword(int userId, string oldPassword, string newPassword)
        {
            return _oracleHelper.ExecuteProcedureNonQuery(PROC_USER_CHANGE_PASSWORD, new
            {
                pv_USER_ID = userId,
                pv_OLD_PASSWORD = CommonUtils.CreateMD5(oldPassword),
                pv_NEW_PASSWORD = CommonUtils.CreateMD5(newPassword),
            });
        }

        public int ChangePasswordTemp(int userId, string password)
        {
            return _oracleHelper.ExecuteProcedureNonQuery(PROC_USER_CHANGE_PASSWORD_TEMP, new
            {
                pv_USER_ID = userId,
                pv_PASSWORD = CommonUtils.CreateMD5(password),
            });
        }

        public Users FindByInvestorId(int investorId)
        {
            return _oracleHelper.ExecuteProcedureToFirst<Users>(PROC_USER_FIND_BY_INVESTOR_ID, new
            {
                pv_INVESTOR_ID = investorId
            });
        }

        public Users AddTradingProviderUser(Users entity)
        {
            string addUserTradingProviderProc = PROC_USER_TRADING_PROVIDER_ADD;
            return _oracleHelper.ExecuteProcedureToFirst<Users>(
                addUserTradingProviderProc,
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

        /// <summary>
        /// Lưu fcm token
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="fcmtoken"></param>
        public void SaveFcmTokenByPhone(string phone, string fcmtoken)
        {
            _oracleHelper.ExecuteProcedureNonQuery(PROC_SAVE_FCM_TOKEN, new
            {
                pv_PHONE = phone,
                pv_FCM_TOKEN = fcmtoken
            }); 
        }

        /// <summary>
        /// Trả về fcm token
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public List<string> GetFcmToken(decimal userid)
        {
            var data = _oracleHelper.ExecuteProcedure<string>(PROC_GET_FCM_TOKEN, new
            {
                pv_USER_ID = userid
            });
            return data?.ToList();
        }

        /// <summary>
        /// Clear token and user
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="fcmToken"></param>
        public void DeleteFcmToken(int userid, string fcmToken)
        {
            _oracleHelper.ExecuteProcedureNonQuery(PROC_DELETE_FCM_TOKEN, new
            {
                pv_USER_ID = userid,
                pv_FCM_TOKEN = fcmToken
            });
        }

        public AuthOtp GetOtpByUserId(int userId)
        {
            return _oracleHelper.ExecuteProcedureToFirst<AuthOtp>(PROC_USER_GET_OTP_BY_ID, new
            {
                pv_ID = userId
            });
        }
        public AuthOtp GetOtpByPhone(string phone)
        {
            return _oracleHelper.ExecuteProcedureToFirst<AuthOtp>(PROC_USER_GET_OTP_BY_PHONE, new
            {
                pv_PHONE = phone
            });
        }


        /// <summary>
        /// Cập nhật giờ login cuối
        /// </summary>
        /// <param name="userId"></param>
        public void Login (decimal userId)
        {
            _oracleHelper.ExecuteProcedureNonQuery(PROC_LOGIN, new
            {
                pv_USER_ID = userId
            });
        }

        public IEnumerable<UsersTradingProviderDto> GetListTradingProviders(int userId)
        {
            return _oracleHelper.ExecuteProcedure<UsersTradingProviderDto>(PROC_USER_GET_LIST_TRADINGS, new
            {
                pv_USER_ID = userId,
            });
        }

        /// <summary>
        /// Lưu thông tin user chat từ app
        /// </summary>
        /// <param name="dto"></param>
        public void SaveUserChatRoomInfo(CreateUsersChatInfoDto dto)
        {
            _oracleHelper.ExecuteProcedureNonQuery(PROC_APP_CHAT_SAVE_ROOM_INFO, new
            {
                pv_USER_ID = dto.UserId,
                pv_ROOM_ID = dto.RoomId,
                pv_ROOM_TOKEN = dto.RoomToken,
                pv_ROOM_START_DATE = dto.RoomStartDate,
                pv_ROOM_END_DATE = dto.RoomEndDate,
                pv_AGENT_ID = dto.AgentId,
                pv_VISITOR_ID = dto.VisitorId,
                pv_VISITOR_TOKEN = dto.VisitorToken,
                pv_LAST_MESSAGE = dto.LastMessage,
            });
        }

        /// <summary>
        /// Lấy thông tin user chat từ app
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public PagingResult<UsersChatRoom> GetUserChatRoomInfo(FindUserChatRoomDto dto, int userId) 
        {
            var result = _oracleHelper.ExecuteProcedurePaging<UsersChatRoom>(PROC_APP_CHAT_GET_ROOM_INFO, new
            {
                PAGE_SIZE = dto.PageSize,
                PAGE_NUMBER = dto.PageNumber,
                pv_USER_ID = userId,
                pv_ROOM_ID = dto.RoomId,
                pv_VISITOR_ID = dto.VisitorId,
            });
            return result;
        }
    }
}
