using DocumentFormat.OpenXml.Bibliography;
using EPIC.DataAccess.Base;
using EPIC.Entities.DataEntities;
using EPIC.Entities.Dto.RocketChat;
using EPIC.IdentityEntities.DataEntities;
using EPIC.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Transactions;

namespace EPIC.IdentityRepositories
{
    public class AuthOtpEFRepository : BaseEFRepository<AuthOtp>
    {
        public AuthOtpEFRepository(EpicSchemaDbContext dbContext, ILogger logger) : base(dbContext, logger, "SEQ_AUTH_OTP")
        {
        }

        /// <summary>
        /// Set IsActive của otp là No theo user id
        /// </summary>
        /// <param name="value"></param>
        /// <param name="userId"></param>
        public void UpdateIsActiveByUserId(string value, int userId)
        {
            _logger.LogInformation($"{nameof(UpdateIsActiveByUserId)}: value = {value}; userId = {userId}");

            var otpList = _dbSet.Where(o => o.UserId == userId);
            if (value == YesNo.YES || value == YesNo.NO)
            {
                foreach (var otp in otpList)
                {
                    otp.IsActive = value;
                }
                //_dbContext.SaveChanges();
            }
        }

        /// <summary>
        /// Update IsActive của các otp về value theo phone
        /// </summary>
        /// <param name="value"></param>
        /// <param name="phone"></param>
        public void UpdateIsActiveByPhone(string value, string phone)
        {
            _logger.LogInformation($"{nameof(UpdateIsActiveByPhone)}: value = {value}; phone = {phone}");

            var otpList = _dbSet.Where(o => o.Phone == phone);
            if (value == YesNo.YES || value == YesNo.NO)
            {
                foreach (var otp in otpList)
                {
                    otp.IsActive = value;
                }
                //_dbContext.SaveChanges();
            }
        }

        /// <summary>
        /// Tạo mới otp
        /// </summary>
        /// <param name="otp"></param>
        public AuthOtp CreateOtp(AuthOtp otp)
        {
            _logger.LogInformation($"{nameof(CreateOtp)}: {(otp != null ? JsonSerializer.Serialize(otp) : "")}");

            var otpEntity = new AuthOtp
            {
                Id = (int)NextKey(),
                Phone = otp.Phone,
                OtpCode = otp.OtpCode,
                UserId = otp.UserId,
                CreatedTime = otp.CreatedTime,
                ExpiredTime = otp.ExpiredTime,
                IsActive = YesNo.YES
            };

            _dbSet.Add(otpEntity);
            //_dbContext.SaveChanges();

            return otpEntity;
        }

        /// <summary>
        /// Lấy otp active theo userId
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public AuthOtp FindActiveOtpByUserId(decimal userId)
        {
            var otp = _dbSet.FirstOrDefault(o => o.UserId == userId && o.IsActive == YesNo.YES);
            return otp;
        }

        /// <summary>
        /// Lấy otp active theo userId
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public AuthOtp FindActiveOtpByUserIdAndPhone(decimal userId, string phone)
        {
            var otp = _dbSet.FirstOrDefault(o => o.UserId == userId && o.Phone == phone && o.IsActive == YesNo.YES);
            return otp;
        }

        /// <summary>
        /// Lấy otp theo userid. Nếu chưa hết hạn thì update isactive = no. Còn các trường hợp khác thì báo lỗi. Sai quá 5 lần cái khóa luôn
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="otpCode"></param>
        public void CheckOtpByUserId(int userId, string otpCode, IHttpContextAccessor httpContextAccessor, string sessionKeys, int maxFailCount)
        {
            var otp = FindActiveOtpByUserId(userId);
            bool otpInvalid = false;

            if (otp == null)
            {
                otpInvalid = true;
            }

            var now = DateTime.Now;
            var defaultOtp = _epicSchemaDbContext.SysVars.FirstOrDefault(sv => sv.GrName == "AUTH" && sv.VarName == "OTP_DEFAULT")?.VarValue;
            if (otp.OtpCode == otpCode || otpCode == defaultOtp)
            {
                if (now > otp.ExpiredTime)
                {
                    ThrowException(ErrorCode.InvestorOTPExpire);
                }
                else
                {
                    httpContextAccessor.HttpContext.Session.Remove(sessionKeys);
                    otp.IsActive = YesNo.NO;

                    _dbContext.SaveChanges();
                }
            }
            else
            {            
                otpInvalid = true;
            }

            if (otpInvalid)
            {
                var invalidFailCount = httpContextAccessor.HttpContext.Session.GetInt32(sessionKeys) ?? 0;               

                if (invalidFailCount >= maxFailCount)
                {
                    var user = _epicSchemaDbContext.Users.FirstOrDefault(x => x.UserId == userId);
                    user.Status = UserStatus.DEACTIVE;

                    _dbContext.SaveChanges();
                    ThrowException(ErrorCode.UserDeactive);
                }

                invalidFailCount++;
                httpContextAccessor.HttpContext.Session.SetInt32(sessionKeys, invalidFailCount);

                ThrowException(ErrorCode.InvestorOTPInvalid);
            }
        }

    }
}
