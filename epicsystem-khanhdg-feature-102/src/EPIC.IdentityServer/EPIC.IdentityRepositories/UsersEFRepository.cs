using EPIC.DataAccess;
using EPIC.DataAccess.Base;
using EPIC.Entities.DataEntities;
using EPIC.Entities.Dto.RocketChat;
using EPIC.IdentityEntities.DataEntities;
using EPIC.IdentityEntities.EntityFramework;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace EPIC.IdentityRepositories
{
    public class UsersEFRepository : BaseEFRepository<Users>
    {
        #region fcm token
        private const string PROC_SAVE_FCM_TOKEN = "PKG_USERS.PROC_SAVE_FCM_TOKEN";
        private const string PROC_GET_FCM_TOKEN = "PKG_USERS.PROC_GET_FCM_TOKEN";
        private const string PROC_DELETE_FCM_TOKEN = "PKG_USERS.PROC_DELETE_FCM_TOKEN";
        #endregion

        public UsersEFRepository(EpicSchemaDbContext dbContext, ILogger logger) : base(dbContext, logger, Users.SEQ)
        {
        }

        public Users Add(Users input)
        {
            _logger.LogInformation($"{nameof(UsersEFRepository)}->{nameof(Add)}: investorId = {JsonSerializer.Serialize(input)}");
            input.UserId = (int)NextKey();
            input.Status = Status.ACTIVE;
            input.IsDeleted = YesNo.NO;
            input.IsFirstTime = YesNo.NO;
            input.IsVerifiedEmail = YesNo.NO;
            input.CreatedDate = DateTime.Now;
            return _dbSet.Add(input).Entity;
        }

        /// <summary>
        /// Lấy theo investor id
        /// </summary>
        /// <param name="investorId"></param>
        /// <returns></returns>
        public Users FindByInvestorId(int investorId)
        {
            _logger.LogInformation($"{nameof(UsersEFRepository)}->{nameof(FindByInvestorId)}: investorId = {investorId}");

            var user = _dbSet
                        .FirstOrDefault(u => u.InvestorId == investorId && u.IsDeleted == YesNo.NO && u.UserType == UserTypes.INVESTOR && u.Status != UserStatus.LOCKED);
            return user;
        }

        /// <summary>
        /// Tìm username theo investor id
        /// </summary>
        /// <param name="investorId"></param>
        /// <returns></returns>
        public string FindUserNameByInvestorId(int investorId)
        {
            _logger.LogInformation($"{nameof(UsersEFRepository)}->{nameof(FindUserNameByInvestorId)}: investorId = {investorId}");
            var user = _dbSet
                        .FirstOrDefault(u => u.InvestorId == investorId && u.IsDeleted == YesNo.NO && u.UserType == UserTypes.INVESTOR);
            string username = null;
            if (user != null)
            {
                username = user.UserName;
            }
            return username;
        }

        /// <summary>
        /// Lấy theo user id
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public Users FindById(int userId)
        {
            _logger.LogInformation($"{nameof(UsersEFRepository)}->{nameof(FindById)}: userId = {userId}");

            var user = _dbSet.FirstOrDefault(u => u.UserId == userId && u.IsDeleted == YesNo.NO && u.Status != UserStatus.LOCKED);
            return user;
        }

        /// <summary>
        /// Lưu fcm token 
        /// </summary>
        /// <param name="phone"></param>
        /// <param name="fcmtoken"></param>
        public void SaveFcmTokenByPhone(string phone, string fcmtoken)
        {
            _logger.LogInformation($"{nameof(UsersEFRepository)}->{nameof(SaveFcmTokenByPhone)}: phone = {phone}, fcmtoken = {fcmtoken}");

            var converted = ObjectToParamAndQuery(PROC_SAVE_FCM_TOKEN, new {
                pv_PHONE = phone,
                pv_FCM_TOKEN = fcmtoken
            });

            _dbContext.Database.ExecuteSqlRaw(converted.SqlQuery, converted.Parameters);
        }

        /// <summary>
        /// Lấy fcm token bằng userId
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<string> GetFcmTokenByUserId(int userId)
        {
            _logger.LogInformation($"{nameof(UsersEFRepository)}->{nameof(GetFcmTokenByUserId)}: userId = {userId}");

            var data = _epicSchemaDbContext.UsersFcmTokens
                            .AsNoTracking()
                            .Where(uf => uf.UserId == userId && uf.Deleted == YesNo.NO)
                            .Select(x => x.FcmToken).ToList();

            return data;
        }

        /// <summary>
        /// Tạo user
        /// </summary>
        /// <param name="user"></param>
        public void CreateUser(Users user)
        {
            _logger.LogInformation($"{nameof(UsersEFRepository)}->{nameof(CreateUser)}: user = {user}");

            _dbSet.Add(new Users
            {
                UserId = (int)NextKey(),
                UserName = user.UserName,
                UserType = user.UserType,
                Status = user.Status,
                Email = user.Email,
                Password = user.Password,
                InvestorId = user.InvestorId,
                IsFirstTime = YesNo.YES,
                IsDeleted = YesNo.NO,
                CreatedDate = DateTime.Now,
            });
        }

        /// <summary>
        /// Xóa mềm user theo user id
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="deletedBy"></param>
        public void DeleteUserByUserId(int userId, string deletedBy = null)
        {
            _logger.LogInformation($"{nameof(UsersEFRepository)}->{nameof(DeleteUserByUserId)}: userId = {userId}, deletedBy = {deletedBy}");

            var user = _dbSet.FirstOrDefault(u => u.UserId == userId && u.IsDeleted == YesNo.NO).ThrowIfNull(_epicSchemaDbContext, ErrorCode.UserNotFound);

            user.IsDeleted = YesNo.YES;
            user.DeletedBy = deletedBy;
            user.DeletedDate = DateTime.Now;
        }

        public void UpdateDisplayNameUser(Users input, string username)
        {
            _logger.LogInformation($"{nameof(UsersEFRepository)}->{nameof(UpdateDisplayNameUser)}: input = {input}, username = {username}");

            var user = _dbSet.FirstOrDefault(u => u.UserId == input.UserId && u.IsDeleted == YesNo.NO).ThrowIfNull(_epicSchemaDbContext, ErrorCode.UserNotFound);

            user.DisplayName = input.DisplayName;
            user.ModifiedBy = username;
            user.ModifiedDate = DateTime.Now;
        }
    }
}
