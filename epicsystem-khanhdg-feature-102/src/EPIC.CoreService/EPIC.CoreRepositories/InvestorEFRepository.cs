using AutoMapper;
using AutoMapper.Configuration.Annotations;
using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Drawing;
using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Wordprocessing;
using EPIC.CoreEntities.DataEntities;
using EPIC.CoreEntities.Dto.ExcelReport;
using EPIC.CoreEntities.Dto.ManagerInvestor;
using EPIC.CoreSharedEntities.Dto.Investor;
using EPIC.CoreSharedEntities.Dto.TradingProvider;
//using EPIC.CoreEntities.Dto.Investor;
using EPIC.DataAccess;
using EPIC.DataAccess.Base;
using EPIC.DataAccess.Models;
using EPIC.Entities.DataEntities;
using EPIC.Entities.Dto.ManagerInvestor;
using EPIC.Entities.Dto.RocketChat;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Identity;
using EPIC.Utils.ConstantVariables.Loyalty;
using EPIC.Utils.Linq;
using Humanizer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.Extensions.Logging;
using MySqlX.XDevAPI.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace EPIC.CoreRepositories
{
    public class InvestorEFRepository : BaseEFRepository<Investor>
    {
        private const string PROC_GET_ALL_INVESTORS = "EPIC.PKG_INVESTOR.PROC_GET_ALL_INVESTORS";
        private const string PROC_INVESTOR_FIND_ALL = "EPIC.PKG_INVESTOR.PROC_INVESTOR_FIND_ALL";
        private const string PROC_INVESTOR_FIND_BY_ID = "EPIC.PKG_INVESTOR.PROC_INVESTOR_FIND_BY_ID";
        private const string PROC_INVESTOR_CHECK_EMAIL_EXIST = "EPIC.PKG_INVESTOR.PROC_INVESTOR_CK_EMAIL_EXIST";
        private const string PROC_INVESTOR_CHECK_PHONE_EXIST = "EPIC.PKG_INVESTOR.PROC_INVESTOR_CK_PHONE_EXIST";
        private const string PROC_INVESTOR_IS_REGISTERED_OFFLINE = "EPIC.PKG_INVESTOR.PROC_INVESTOR_IS_REGIS_OFFLINE";
        private const string PROC_INVESTOR_CREATE_VERIFICATION_CODE = "EPIC.PKG_INVESTOR.PROC_INVESTOR_CREATE_VERIFY";
        private const string PROC_INVESTOR_VERIFY_CODE = "EPIC.PKG_INVESTOR.PROC_INVESTOR_VERIFY_CODE";
        private const string PROC_INVESTOR_REGISTER = "EPIC.PKG_INVESTOR.PROC_INVESTOR_REGISTER";
        private const string PROC_INVESTOR_VALIDATE_PASSWORD = "EPIC.PKG_INVESTOR.PROC_INVESTOR_VALID_PASSWORD";
        private const string PROC_INVESTOR_GET_BY_EMAIL_OR_PHONE = "EPIC.PKG_INVESTOR.PROC_INVESTOR_GET_BY_MAILPHONE";
        private const string PROC_INVESTOR_GET_BY_USERNAME = "EPIC.PKG_INVESTOR.PROC_INVESTOR_GET_BY_USERNAME";
        private const string PROC_INVESTOR_FORGOT_PASSWORD = "EPIC.PKG_INVESTOR.PROC_INVESTOR_FORGOT_PASSWORD";
        private const string PROC_INVESTOR_VERIFY_OTP_RESET_PASSWORD = "EPIC.PKG_INVESTOR.PROC_INVESTOR_VERIFY_RESET_P";
        private const string PROC_INVESTOR_GET_BY_USER_ID = "EPIC.PKG_INVESTOR.PROC_INVESTOR_GET_BY_USER_ID";
        private const string PROC_INVESTOR_RESET_PASSWORD = "EPIC.PKG_INVESTOR.PROC_INVESTOR_RESET_PASSWORD";
        private const string PROC_INVESTOR_CHANGE_PIN = "EPIC.PKG_INVESTOR.PROC_INVESTOR_CHANGE_PIN";
        private const string PROC_INVESTOR_VALIDATE_PIN = "EPIC.PKG_INVESTOR.PROC_INVESTOR_VALIDATE_PIN";
        private const string PROC_INVESTOR_UPDATE_EKYC_ID = "EPIC.PKG_INVESTOR.PROC_INVESTOR_UPDATE_EKYC_ID";
        private const string PROC_INVESTOR_EKYC_CONFIRM_INFO = "EPIC.PKG_INVESTOR.PROC_INVES_EKYC_CONFIRM_INFO";
        private const string PROC_INVESTOR_EKYC_FINISH = "EPIC.PKG_INVESTOR.PROC_INVESTOR_EKYC_FINISH";
        private const string PROC_INVESTOR_UPDATE = "EPIC.PKG_INVESTOR.PROC_INVESTOR_UPDATE";
        private const string PROC_INVESTOR_DELETE = "EPIC.PKG_INVESTOR.PROC_INVESTOR_DELETE";
        private const string PROC_MANAGER_INVESTOR_ACTIVATE = "EPIC.PKG_MANAGER_INVESTOR.PROC_MANAGER_INVESTOR_ACTIVATE";
        private const string PROC_MANAGER_INVESTOR_RESET_PASSWORD = "EPIC.PKG_MANAGER_INVESTOR.PROC_MANAGER_RESET_PASSWORD";
        private const string PROC_MANAGER_INVESTOR_DELETE = "EPIC.PKG_MANAGER_INVESTOR.PROC_MANAGER_INVESTOR_DELETE";
        private const string PROC_FINAL_STEP = "EPIC.PKG_INVESTOR.PROC_FINAL_STEP";
        private const string PROC_GENERATE_OTP = "EPIC.PKG_INVESTOR.PROC_GENERATE_OTP";
        private const string PROC_PHONE_GENERATE_OTP = "EPIC.PKG_INVESTOR.PROC_PHONE_GENERATE_OTP";
        private const string PROC_ADD_IDEN = "EPIC.PKG_INVESTOR.PROC_ADD_IDEN";
        private const string PROC_CONFIRM_IDEN = "EPIC.PKG_INVESTOR.PROC_CONFIRM_IDEN";

        private const string PROC_INVESTOR_CHECK_IS_PROF = "EPIC.PKG_INVESTOR.PROC_INVESTOR_CHECK_IS_PROF";
        private const string PROC_INVESTOR_GET_BY_VERIFY_EMAIL_CODE = "EPIC.PKG_INVESTOR.PROC_INV_GET_BY_VRF_EMAIL";

        private const string PROC_INVESTOR_CHECK_OTP = "EPIC.PKG_INVESTOR.PROC_INVESTOR_CHECK_OTP";
        private const string PROC_APP_DEACTIVE_ACCOUNT = "EPIC.PKG_INVESTOR.PROC_APP_DEACTIVE_ACCOUNT";

        private const string PROC_GET_ID_WHEN_REGISTER = "EPIC.PKG_INVESTOR.PROC_GET_ID_WHEN_REGISTER";
        private const string PROC_FACE_MATCH = "EPIC.PKG_INVESTOR.PROC_FACE_MATCH";
        private const string PROC_APP_GET_CHAT_TRADING = "EPIC.PKG_INVESTOR.PROC_APP_GET_CHAT_TRADING";
        private const string PROC_GET_LIST_CUSTOMER_EXCEL_REPORT = "EPIC.PKG_CORE_EXCEL_REPORT.PROC_LIST_CUSTOMER";
        private const string PROC_GET_LIST_CUSTOMER_ROOT_EXCEL_REPORT = "EPIC.PKG_CORE_EXCEL_REPORT.PROC_LIST_CUSTOMER_ROOT";

        private const string PROC_GET_LIST_USER_EXCEL_REPORT = "EPIC.PKG_CORE_EXCEL_REPORT.PROC_LIST_USER";


        #region CONTACT ADDRESS
        private const string PROC_ADD_CONTACT_ADDRESS = "EPIC.PKG_INVESTOR.PROC_ADD_CONTACT_ADDRESS";
        private const string PROC_UPDATE_CONTACT_ADDRESS = "EPIC.PKG_INVESTOR.PROC_UPDATE_CONTACT_ADDRESS";
        private const string PROC_SET_DEFAULT_ADDRESS = "EPIC.PKG_INVESTOR.PROC_SET_DEFAULT_ADDRESS";
        private const string PROC_GET_ONE_CONTACT_ADDRESS = "EPIC.PKG_INVESTOR_CONTACT_ADDRESS.PROC_GET";
        private const string PROC_GET_LIST_CONTACT_ADDRESS = "EPIC.PKG_INVESTOR_CONTACT_ADDRESS.PROC_GET_PAGING";
        private const string PROC_CONTRACT_ADDRESS_GET = "EPIC.PKG_INVESTOR_CONTACT_ADDRESS.PROC_CONTRACT_ADDRESS_GET";
        private const string PROC_CONTRACT_ADDRESS_DEFAULT = "EPIC.PKG_INVESTOR_CONTACT_ADDRESS.PROC_CONTRACT_ADDRESS_DEFAULT";
        #endregion

        #region BANK
        private const string PROC_ADD_BANK = "EPIC.PKG_INVESTOR.PROC_ADD_BANK";
        private const string PROC_APP_DELETE_BANK_ACC = "EPIC.PKG_INVESTOR.PROC_APP_DELETE_BANK_ACC";

        #endregion

        #region VERIFY EMAIL
        private const string PROC_INSERT_VERIFY_EMAIL_CODE = "EPIC.PKG_INVESTOR.PROC_INSERT_VERIFY_EMAIL_CODE";
        private const string PROC_CHECK_VERIFY_EMAIL_CODE = "EPIC.PKG_INVESTOR.PROC_CHECK_VERIFY_EMAIL_CODE";
        #endregion

        #region Mã giới thiệu - Sale
        private const string PROC_REGIS_REFERRAL_CODE = "EPIC.PKG_INVESTOR.PROC_REGIS_REFERRAL_CODE";
        private const string PROC_SET_DEFAULT_REF_CODE = "EPIC.PKG_INVESTOR.PROC_SET_DEFAULT_REF_CODE";
        private const string PROC_SCAN_REFERRAL_CODE_SALE = "EPIC.PKG_INVESTOR.PROC_SCAN_REFERRAL_CODE_SALE";
        private const string PROC_GET_LIST_REF_CODE = "EPIC.PKG_INVESTOR.PROC_GET_LIST_REF_CODE";
        private const string PROC_IS_SALE = "EPIC.PKG_INVESTOR.PROC_IS_SALE";
        private const string PROC_IS_REFERRAL_CODE_EXIST = "EPIC.PKG_INVESTOR.PROC_IS_REFERRAL_CODE_EXIST";
        private const string PROC_GET_DEPARMENTS_BY_INV_ID = "EPIC.PKG_INVESTOR.PROC_GET_DEPARMENTS_BY_INV_ID";
        #endregion

        private readonly EpicSchemaDbContext _epicDbContext;

        public InvestorEFRepository(EpicSchemaDbContext dbContext, ILogger logger) : base(dbContext, logger, Investor.SEQ)
        {
            _epicDbContext = dbContext;
        }

        /// <summary>
        /// Lấy investor theo user id
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public Investor GetByUserId(int userId)
        {
            _logger.LogInformation($"{System.Reflection.MethodBase.GetCurrentMethod().Name}: userId = {userId}");

            var user = _epicDbContext.Users.FirstOrDefault(u => u.UserId == userId && u.UserType == UserTypes.INVESTOR && u.IsDeleted == YesNo.NO && u.Status != InvestorStatus.LOCKED);

            if (user != null)
            {
                var investor = _epicDbContext.Investors.FirstOrDefault(i => i.InvestorId == user.InvestorId && i.Deleted == YesNo.NO);
                return investor;
            }

            return null;
        }

        /// <summary>
        /// Lấy theo số điện thoại (bất kể trạng thái)
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        public Investor GetByEmailOrPhone(string phone)
        {
            _logger.LogInformation($"{System.Reflection.MethodBase.GetCurrentMethod().Name}: phone = {phone}");

            var investor = _epicDbContext.Investors.FirstOrDefault(i => (i.Email.ToLower() == phone.ToLower() || i.Phone == phone) && i.Deleted == YesNo.NO && i.Status != InvestorStatus.LOCKED);
            return investor;
        }

        /// <summary>
        /// Get by phone và trạng thái truyền vào. Truyền status null thì lấy toàn bộ trạng thái
        /// </summary>
        /// <param name="phone"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public Investor GetByEmailOrPhone(string phone, List<string> status)
        {
            _logger.LogInformation($"{System.Reflection.MethodBase.GetCurrentMethod().Name}: phone = {phone}");

            if (status == null)
            {
                return _epicDbContext.Investors.FirstOrDefault(i => (i.Email.ToLower() == phone.ToLower() || i.Phone == phone) && i.Deleted == YesNo.NO);
            }
            return _epicDbContext.Investors.FirstOrDefault(i => (i.Email.ToLower() == phone.ToLower() || i.Phone == phone) && i.Deleted == YesNo.NO && status.Contains(i.Status));
        }

        /// <summary>
        /// Lấy theo email (bất kể trạng thái)
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        public Investor GetByEmail(string email)
        {
            _logger.LogInformation($"{System.Reflection.MethodBase.GetCurrentMethod().Name}: email = {email}");

            var investor = _epicDbContext.Investors.FirstOrDefault(i => (i.Email.ToLower() == email.ToLower()) && i.Deleted == YesNo.NO && i.Status != InvestorStatus.LOCKED);
            return investor;
        }

        /// <summary>
        /// Xem investor active chưa (Đã đăng ký xong chưa)
        /// </summary>
        /// <param name="investorId"></param>
        /// <returns></returns>
        public bool IsInvestorActive(int investorId)
        {
            _logger.LogInformation($"{System.Reflection.MethodBase.GetCurrentMethod().Name}: investorId = {investorId}");
            var investor = EntityNoTracking.FirstOrDefault(i => i.InvestorId == investorId && i.Deleted == YesNo.NO);

            return investor?.Status == InvestorStatus.ACTIVE;
        }

        /// <summary>
        /// Xem investor đã đăng ký đến step nào rồi
        /// </summary>
        /// <param name="investorId"></param>
        /// <returns></returns>
        public int? GetInvestorStep(int investorId)
        {
            _logger.LogInformation($"{System.Reflection.MethodBase.GetCurrentMethod().Name}: investorId = {investorId}");

            var investor = EntityNoTracking.FirstOrDefault(i => i.InvestorId == investorId && i.Deleted == YesNo.NO);

            return investor?.Step;
        }

        /// <summary>
        /// Check xem đã có investor thật nào dùng email này chưa
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public bool AnyInvestorHaveEmail(string email)
        {
            _logger.LogInformation($"{System.Reflection.MethodBase.GetCurrentMethod().Name}: email = {email}");
            return _dbSet.Any(i => i.Email.ToLower() == email.ToLower() && i.Deleted == YesNo.NO && i.Status != InvestorStatus.LOCKED);
        }

        /// <summary>
        /// Check xem đã có investor thật nào dùng phone này chưa
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        public bool AnyInvestorHavePhone(string phone)
        {
            _logger.LogInformation($"{System.Reflection.MethodBase.GetCurrentMethod().Name}: phone = {phone}");
            return _dbSet.Any(i => i.Phone == phone && i.Deleted == YesNo.NO && i.Status != InvestorStatus.LOCKED);
        }

        /// <summary>
        /// Đếm số investor thật theo phone (Đã qua được bước otp, tất cả status)
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        public int CountInvestorByPhone(string phone)
        {
            _logger.LogInformation($"{System.Reflection.MethodBase.GetCurrentMethod().Name}: phone = {phone}");

            return _epicDbContext.Investors.Count(i => i.Phone == phone && i.Deleted == YesNo.NO);
        }

        /// <summary>
        /// Lấy theo số điện thoại và email (bất kể trạng thái)
        /// </summary>
        /// <param name="phone"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        public Investor GetByEmailAndPhone(string phone, string email)
        {
            _logger.LogInformation($"{System.Reflection.MethodBase.GetCurrentMethod().Name}: phone = {phone}; email = {email}");

            var investor = _epicDbContext.Investors
                            .FirstOrDefault(i => (i.Email.ToLower() == email.ToLower() && i.Phone == phone) && i.Deleted == YesNo.NO && i.Status != InvestorStatus.LOCKED);
            return investor;
        }

        /// <summary>
        /// Lấy theo username
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public Investor GetByUsername(string username)
        {
            _logger.LogInformation($"{System.Reflection.MethodBase.GetCurrentMethod().Name}: username = {username}");

            var query = from user in _epicDbContext.Users
                        join i in _epicDbContext.Investors on user.InvestorId equals i.InvestorId
                        where user.UserName == username && user.IsDeleted == YesNo.NO && i.Deleted == YesNo.NO
                            && user.Status == Status.ACTIVE && i.Status == Status.ACTIVE && user.UserType == UserTypes.INVESTOR
                        select i;
            return query.FirstOrDefault();
        }

        /// <summary>
        /// Lấy theo username
        /// </summary>
        /// <param name="username"></param>
        /// <param name="tradingProviderId"></param>
        /// <returns></returns>
        public Investor GetByUsernameSkipStatus(string username, int? tradingProviderId)
        {
            _logger.LogInformation($"{System.Reflection.MethodBase.GetCurrentMethod().Name}: username = {username}");

            var query = (
                        from user in _epicDbContext.Users.AsNoTracking().Where(x => x.UserName == username && x.IsDeleted == YesNo.NO && x.UserType == UserTypes.INVESTOR)
                        from inv in _epicDbContext.Investors.AsNoTracking().Where(x => x.InvestorId == user.InvestorId && x.Deleted == YesNo.NO)
                        from investorTrading in _epicDbContext.InvestorTradingProviders.AsNoTracking().Where(x => x.InvestorId == inv.InvestorId && x.TradingProviderId == tradingProviderId && x.Deleted == YesNo.NO)
                        select inv
                     )
                     .Distinct();

            return query.FirstOrDefault();
        }

        /// <summary>
        /// lấy danh sách khách hàng cá nhân
        /// </summary>
        /// <param name="input"></param>
        /// <param name="tradingProviderId"></param>
        /// <returns></returns>
        public PagingResult<ViewManagerInvestorDto> FindAllInvestor(FindListInvestorDto input, string tradingProviderId, string userType, int? partnerId)
        {
            var query = from investor in _dbSet.AsNoTracking()
                        from cifCodes in _epicSchemaDbContext.CifCodes.Where(cc => investor.InvestorId == cc.InvestorId && cc.Deleted == YesNo.NO).DefaultIfEmpty()
                        from identification in _epicSchemaDbContext.InvestorIdentifications.Where(i => i.InvestorId == investor.InvestorId && i.Deleted == YesNo.NO)
                        .OrderByDescending(c => c.IsDefault).ThenBy(i => i.Id).Take(1).DefaultIfEmpty()
                        where investor.Deleted == YesNo.NO &&
                                        (investor.Status == Status.ACTIVE || investor.Status == Status.INACTIVE) &&
                                        (input.CifCode == null || cifCodes.CifCode == input.CifCode) &&
                                        (input.Phone == null || investor.Phone.Contains(input.Phone) || investor.RepresentativePhone.Contains(input.Phone)) &&
                                        (input.Email == null || investor.Email.ToLower().Contains(input.Email.ToLower())) &&
                                        (input.IsCheck == null || investor.IsCheck == input.IsCheck) &&
                                        (input.Status == null || investor.Status == input.Status) &&
                                        (input.IdNo == null || identification != null && identification.IdNo == input.IdNo) &&
                                        (input.Sex == null || identification != null &&  identification.Sex == input.Sex) &&
                                        (input.Fullname == null || identification != null && identification != null && identification.Fullname.ToLower().Contains(input.Fullname.ToLower())) &&
                                        (input.Nationality == null || identification != null && identification.Nationality.Contains(input.Nationality.ToLower())) &&
                                        (input.DateOfBirth == null || identification != null && identification.DateOfBirth.ToString() == input.DateOfBirth)
                        select new ViewManagerInvestorDto
                        {
                            Id = investor.InvestorId,
                            InvestorId = investor.InvestorId,
                            InvestorGroupId = investor.InvestorGroupId ?? 0,
                            Name = investor.Name,
                            Address = investor.Address,
                            ContactAddress = investor.ContactAddress,
                            Nationality = investor.Nationality,
                            Phone = investor.Phone,
                            Fax = investor.Fax,
                            Mobile = investor.Mobile,
                            CifCode = cifCodes.CifCode,
                            Email = investor.Email,
                            TaxCode = investor.TaxCode,
                            Status = investor.Status,
                            Isonline = investor.Isonline,
                            FaceImageUrl = investor.FaceImageUrl,
                            AccountStatus = investor.AccountStatus,
                            IsProf = investor.IsProf,
                            ProfFileUrl = investor.ProfFileUrl,
                            ProfDueDate = investor.ProfDueDate,
                            ProfStartDate = investor.ProfStartDate,
                            CreatedBy = investor.CreatedBy,
                            CreatedDate = investor.CreatedDate,
                            IsCheck = investor.IsCheck,
                            AvatarImageUrl = investor.AvatarImageUrl,
                            ReferralCodeSelf = investor.ReferralCodeSelf,
                            ReferralCode = investor.ReferralCode,
                            ReferralDate = investor.ReferralDate,
                            TradingProviderId = investor.TradingProviderId ?? 0,
                            Source = investor.Source,
                            RepresentativePhone = investor.RepresentativePhone,
                            RepresentativeEmail = investor.RepresentativeEmail,
                            FaceImageUrl1 = investor.FaceImageUrl1,
                            FaceImageUrl2 = investor.FaceImageUrl2,
                            FaceImageUrl3 = investor.FaceImageUrl3,
                            FaceImageUrl4 = investor.FaceImageUrl4,
                            FaceImageSimilarity1 = investor.FaceImageSimilarity1,
                            FaceImageSimilarity2 = investor.FaceImageSimilarity2,
                            FaceImageSimilarity3 = investor.FaceImageSimilarity3,
                            FaceImageSimilarity4 = investor.FaceImageSimilarity4,
                            LoyTotalPoint = investor.LoyTotalPoint,
                            LoyCurrentPoint = investor.LoyCurrentPoint,
                            DefaultIdentification = identification == null ? null : new ViewIdentificationDto
                            {
                                Id = identification.Id,
                                DateOfBirth = identification.DateOfBirth,
                                EkycIncorrectFields = identification.EkycIncorrectFields,
                                EkycInfoIsConfirmed = identification.EkycInfoIsConfirmed,
                                FaceImageUrl = identification.FaceImageUrl,
                                FaceVideoUrl = identification.FaceVideoUrl,
                                Fullname = identification.Fullname,
                                IdBackImageUrl = identification.IdBackImageUrl,
                                IdDate = identification.IdDate,
                                IdExpiredDate = identification.IdExpiredDate,
                                IdExtraImageUrl = identification.IdExtraImageUrl,
                                IdFrontImageUrl  = identification.IdFrontImageUrl,
                                IdIssuer = identification.IdIssuer,
                                IdNo = identification.IdNo,
                                IdType = identification.IdType,
                                InvestorGroupId = identification.InvestorGroupId,
                                InvestorId = identification.InvestorId,
                                IsDefault = identification.IsDefault,
                                IsVerifiedFace = identification.IsVerifiedFace,
                                IsVerifiedIdentification = identification.IsVerifiedIdentification,
                                Nationality = identification.Nationality,
                                PersonalIdentification = identification.PersonalIdentification,
                                PlaceOfOrigin = identification.PlaceOfOrigin,
                                PlaceOfResidence = identification.PlaceOfResidence,
                                Sex = identification.Sex,
                                Status = identification.Status,
                                StatusApproved = identification.StatusApproved,
                            }
                        };

            // lọc data khách hàng theo role 
            if (new[] { UserTypes.EPIC, UserTypes.SUPER_ADMIN, UserTypes.ROOT_EPIC }.Contains(userType))
            {
                query = query.Where(investors => input.TradingProviderId == null || _epicSchemaDbContext.InvestorTradingProviders
                                                                                                        .Any(itp => itp.Deleted == YesNo.NO &&
                                                                                                                    itp.TradingProviderId.ToString().Contains(input.TradingProviderId) &&
                                                                                                                    itp.InvestorId == investors.InvestorId));
            }
            else if (new[] { UserTypes.TRADING_PROVIDER, UserTypes.ROOT_TRADING_PROVIDER }.Contains(userType))
            {
                query = query.Where(investors => _epicSchemaDbContext.InvestorTradingProviders.Any(itp => itp.Deleted == YesNo.NO &&
                                                                                                          itp.TradingProviderId.ToString().Equals(input.TradingProviderId) &&
                                                                                                          itp.InvestorId == investors.InvestorId) ||
                                                 _epicSchemaDbContext.InvestorSales.Join(_epicSchemaDbContext.SaleTradingProviders, eis => eis.SaleId, est => est.SaleId, (eis, est) => new { eis, est })
                                                                                   .Any(joined => joined.eis.Deleted == YesNo.NO &&
                                                                                                  joined.est.Deleted == YesNo.NO &&
                                                                                                  joined.est.TradingProviderId.ToString().Contains(input.TradingProviderId) &&
                                                                                                  joined.eis.InvestorId == investors.InvestorId));
            }
            else if (new[] { UserTypes.PARTNER, UserTypes.ROOT_PARTNER }.Contains(userType))
            {
                query = query.Where(investors => _epicSchemaDbContext.InvestorTradingProviders.Any(itp => itp.Deleted == YesNo.NO && itp.InvestorId == investors.InvestorId
                                                   && _epicDbContext.TradingProviderPartners.Any(p => p.PartnerId == partnerId && p.TradingProviderId == itp.TradingProviderId && p.Deleted == YesNo.NO)));
            }

            var result = new PagingResult<ViewManagerInvestorDto>();
            result.TotalItems = query.Count();
            query = query.OrderDynamic(input.Sort);
            if (input.PageSize != -1)
            {
                query = query.Skip(input.Skip).Take(input.PageSize);
            }
            result.Items = query;
            return result;
        }

        public IQueryable<ViewManagerInvestorTemporaryDto> FindAllInvestorTemp(FindListInvestorDto input, string userType)
        {
            var query = from investorTemps in _epicSchemaDbContext.InvestorTemps
                        join investor in _epicSchemaDbContext.Investors on investorTemps.InvestorGroupId equals investor.InvestorGroupId into investors
                        from investor in investors.DefaultIfEmpty()
                        join cifCodes in _epicSchemaDbContext.CifCodes on investor.InvestorId equals cifCodes.InvestorId into investorCifCodes
                        from investorCifCode in investorCifCodes.DefaultIfEmpty()
                        join approves in _epicSchemaDbContext.CoreApproves on investorTemps.InvestorId equals approves.ReferIdTemp into approveInvestorTemps
                        from approveInvestorTemp in approveInvestorTemps.DefaultIfEmpty()
                        let defaultIndentification = _epicSchemaDbContext.InvestorIdentifications.Where(o => o.InvestorId == investor.InvestorId).OrderByDescending(o => o.IsDefault).FirstOrDefault()
                        where investorTemps.Deleted == YesNo.NO
                        && (approveInvestorTemp.DataType == null || approveInvestorTemp.DataType == 2) && (approveInvestorTemp.Status == null || (approveInvestorTemp.Status == 1 || approveInvestorTemp.Status == 3))
                        && (((userType == UserTypes.EPIC || userType == UserTypes.ROOT_EPIC || userType == UserTypes.SUPER_ADMIN)
                                         && input.TradingProviderId == null || (_epicSchemaDbContext.InvestorTradingProviders.Any(investorTrading => investorTrading.Deleted == YesNo.NO
                                              && investorTrading.InvestorId == investor.InvestorId
                                              && (investorTrading.TradingProviderId.ToString().Contains(input.TradingProviderId)
                                              || investorTemps.TradingProviderId.ToString().Contains(input.TradingProviderId))))
                                         )
                             ||
                            (userType == UserTypes.TRADING_PROVIDER || userType == UserTypes.ROOT_TRADING_PROVIDER)
                            && (investorTemps.TradingProviderId == null || (_epicSchemaDbContext.InvestorTradingProviders.Any(investorTrading => investorTrading.Deleted == YesNo.NO
                                && (input.TradingProviderId == null || investorTrading.TradingProviderId.ToString().Contains(input.TradingProviderId))
                                && investorTrading.InvestorId == investor.InvestorId)
                                || (from eis in _epicSchemaDbContext.InvestorSales
                                    join esctp in _epicSchemaDbContext.SaleTradingProviders on eis.SaleId equals esctp.SaleId
                                    where eis.InvestorId == investor.InvestorId && esctp.Deleted == YesNo.NO && eis.Deleted == YesNo.NO
                                    && (input.TradingProviderId == null || esctp.TradingProviderId.ToString().Contains(input.TradingProviderId))
                                    select eis.InvestorId).Any()
                                    || investorTemps.TradingProviderId.ToString().Contains(input.TradingProviderId))))

                        && (input.Phone == null || (investor.Phone != null && investor.Phone.Contains(input.Phone)) || (investor.RepresentativePhone != null && investor.RepresentativePhone.Contains(input.Phone))) &&
                        (input.Email == null || (investor.Email != null && investor.Email.ToLower().Contains(input.Email.ToLower()))) &&
                        (input.IsCheck == null || investor.IsCheck == input.IsCheck) &&
                        (input.Status == null || investor.Status == input.Status) &&
                        (input.IdNo == null || defaultIndentification!= null && defaultIndentification.IdNo == input.IdNo) &&
                        (input.Sex == null || defaultIndentification != null && defaultIndentification.Sex == input.Sex) &&
                        (input.Fullname == null || defaultIndentification != null && defaultIndentification.Fullname.ToLower().Contains(input.Fullname.ToLower())) &&
                        (input.Nationality == null || defaultIndentification != null && defaultIndentification.Nationality.ToLower().Contains(input.Nationality.ToLower())) &&
                        (input.DateOfBirth == null || defaultIndentification != null && defaultIndentification.DateOfBirth.ToString() == input.DateOfBirth) &&
                        (input.CifCode == null || investorCifCode.CifCode == input.CifCode)
                        select new ViewManagerInvestorTemporaryDto
                        {
                            Id = investorTemps.InvestorId,
                            InvestorId = investorTemps.InvestorId,
                            Name = investorTemps.Name,
                            Address = investorTemps.Address,
                            ContactAddress = investorTemps.ContactAddress,
                            Nationality = investorTemps.Nationality,
                            Phone = investorTemps.Phone,
                            Fax = investorTemps.Fax,
                            Mobile = investorTemps.Mobile,
                            Email = investorTemps.Email,
                            TaxCode = investorTemps.TaxCode,
                            Status = investorTemps.Status,
                            Source = investorTemps.Source,
                            ApproveId = approveInvestorTemp.ApproveID,
                            CifCode = investorCifCode.CifCode,
                            DefaultIdentification = defaultIndentification == null ? null : new ViewIdentificationDto()
                            {
                                Id = defaultIndentification.Id,
                                IdType = defaultIndentification.IdType,
                                IdNo = defaultIndentification.IdNo,
                                Fullname = defaultIndentification.Fullname,
                                DateOfBirth = defaultIndentification.DateOfBirth,
                                Nationality = defaultIndentification.Nationality,
                                PersonalIdentification = defaultIndentification.PersonalIdentification,
                                IdIssuer = defaultIndentification.IdIssuer,
                                IdDate = defaultIndentification.IdDate,
                                IdExpiredDate = defaultIndentification.IdExpiredDate,
                                PlaceOfOrigin = defaultIndentification.PlaceOfOrigin,
                                PlaceOfResidence = defaultIndentification.PlaceOfResidence,
                                IdFrontImageUrl = defaultIndentification.IdFrontImageUrl,
                                IdBackImageUrl = defaultIndentification.IdBackImageUrl,
                                IdExtraImageUrl = defaultIndentification.IdExtraImageUrl,
                                FaceImageUrl = defaultIndentification.FaceImageUrl,
                                FaceVideoUrl = defaultIndentification.FaceVideoUrl,
                                Status = defaultIndentification.Status,
                                Sex = defaultIndentification.Sex,
                                IsDefault = defaultIndentification.IsDefault,
                                IsVerifiedIdentification = defaultIndentification.IsVerifiedIdentification,
                                IsVerifiedFace = defaultIndentification.IsVerifiedFace,
                                StatusApproved = defaultIndentification.StatusApproved,
                                InvestorGroupId = defaultIndentification.InvestorGroupId,
                                InvestorId = defaultIndentification.InvestorId,
                                EkycIncorrectFields = defaultIndentification.EkycIncorrectFields,
                                EkycInfoIsConfirmed = defaultIndentification.EkycInfoIsConfirmed
                            },
                        };

            query = query.OrderDynamic(input.Sort);
            return query;
        }

        /// <summary>
        /// Lấy investor đang hoạt động theo investor id
        /// </summary>
        /// <param name="investorId"></param>
        /// <returns></returns>
        public Investor FindActiveInvestorById(int investorId)
        {
            _logger.LogInformation($"{System.Reflection.MethodBase.GetCurrentMethod().Name}: investorId = {investorId}");
            return _epicDbContext.Investors.FirstOrDefault(i => i.InvestorId == investorId && i.Deleted == YesNo.NO && i.Status == InvestorStatus.ACTIVE);
        }

        /// <summary>
        /// Lấy địa chỉ mặc định investor thật
        /// </summary>
        /// <param name="investorId"></param>
        /// <returns></returns>
        public InvestorContactAddress GetDefaultContactAddress(int investorId)
        {
            _logger.LogInformation($"{nameof(InvestorEFRepository)}->{nameof(GetDefaultContactAddress)}: investorId = {investorId}");
            var defaultAddress = _epicDbContext.InvestorContactAddresses
                                    .FirstOrDefault(c => c.InvestorId == investorId && c.Deleted == YesNo.NO && c.IsDefault == YesNo.YES);

            if (defaultAddress == null)
            {
                defaultAddress = GetListContactAddress(investorId)?.FirstOrDefault();
            }

            return defaultAddress;
        }

        /// <summary>
        /// Lấy list địa chỉ mặc định investor thật
        /// </summary>
        /// <param name="investorId"></param>
        /// <returns></returns>
        public IQueryable<InvestorContactAddress> GetListContactAddress(int investorId)
        {
            _logger.LogInformation($"{System.Reflection.MethodBase.GetCurrentMethod().Name}: investorId = {investorId}");
            var result = _epicDbContext.InvestorContactAddresses
                                    .Where(c => c.InvestorId == investorId && c.Deleted == YesNo.NO)
                                    .OrderByDescending(c => c.IsDefault).OrderByDescending(c => c.ContactAddressId);
            //.ToList();

            return result;
        }

        /// <summary>
        /// Thêm contact address vào bảng thật (từ app)
        /// </summary>
        /// <param name="entity"></param>
        public void AddContactAddressAutoDefault(InvestorContactAddress entity)
        {
            _logger.LogInformation($"{System.Reflection.MethodBase.GetCurrentMethod().Name}: {JsonSerializer.Serialize(entity)}");
            if (entity.IsDefault == YesNo.YES)
            {
                var listContactAddress = _epicDbContext.InvestorContactAddresses
                                                        .Where(x => x.InvestorId == entity.InvestorId && x.Deleted == YesNo.NO);

                foreach (var address in listContactAddress)
                {
                    address.IsDefault = YesNo.NO;
                }
            }

            do
            {
                entity.ContactAddressId = (int)NextKey(InvestorContactAddress.SEQ);
            } while (_epicDbContext.InvestorContactAddresses.AsNoTracking().Any(x => x.ContactAddressId == entity.ContactAddressId));

            entity.CreatedDate = DateTime.Now;
            entity.Deleted = YesNo.NO;

            _epicDbContext.InvestorContactAddresses.Add(entity);
        }

        /// <summary>
        /// Lấy list bank theo investor id
        /// </summary>
        /// <param name="investorId"></param>
        /// <returns></returns>
        public IQueryable<InvestorBankAccount> GetListBank(int investorId)
        {
            _logger.LogInformation($"{System.Reflection.MethodBase.GetCurrentMethod().Name}: investorId = {investorId}");
            var result = _epicDbContext.InvestorBankAccounts
                                    .Where(c => c.InvestorId == investorId && c.Deleted == YesNo.NO)
                                    .OrderByDescending(c => c.IsDefault).OrderByDescending(c => c.Id);

            return result;
        }

        /// <summary>
        /// Lấy bank theo investor bank id
        /// </summary>
        /// <param name="investorBankId"></param>
        /// <returns></returns>
        public InvestorBankAccount GetBankById(int investorBankId)
        {
            _logger.LogInformation($"{System.Reflection.MethodBase.GetCurrentMethod().Name}: investorBankId = {investorBankId}");
            var result = _epicDbContext.InvestorBankAccounts
                                    .FirstOrDefault(c => c.Id == investorBankId && c.Deleted == YesNo.NO);

            return result;
        }

        /// <summary>
        /// Lấy bank theo investor bank id, join với bảng tên ngân hàng
        /// </summary>
        /// <param name="investorBankId"></param>
        /// <returns></returns>
        public InvestorBankAccount FindBankById(int investorBankId)
        {
            _logger.LogInformation($"{System.Reflection.MethodBase.GetCurrentMethod().Name}: investorBankId = {investorBankId}");

            return (from investorBank in _epicDbContext.InvestorBankAccounts
                    join bank in _epicSchemaDbContext.CoreBanks on investorBank.BankId equals bank.BankId
                    where investorBank.Id == investorBankId && investorBank.Deleted == YesNo.NO
                    select new InvestorBankAccount()
                    {
                        Id = investorBankId,
                        InvestorId = investorBank.InvestorId,
                        OwnerAccount = investorBank.OwnerAccount,
                        BankAccount = investorBank.BankAccount,
                        CoreBankName = bank.BankName,
                        CoreFullBankName = bank.FullBankName,
                        CoreLogo = bank.Logo

                    }).FirstOrDefault();
        }

        /// <summary>
        /// Lấy bank theo investor bank id, join với bảng tên ngân hàng
        /// </summary>
        /// <param name="investorBankId"></param>
        /// <returns></returns>
        public List<InvestorBankAccount> FindListBank(int investorId)
        {
            _logger.LogInformation($"{System.Reflection.MethodBase.GetCurrentMethod().Name}: investorId = {investorId}");

            return (from investorBank in _epicDbContext.InvestorBankAccounts
                    join bank in _epicSchemaDbContext.CoreBanks on investorBank.BankId equals bank.BankId
                    where investorBank.InvestorId == investorId && investorBank.Deleted == YesNo.NO
                    select new InvestorBankAccount()
                    {
                        Id = investorBank.Id,
                        InvestorId = investorBank.InvestorId,
                        OwnerAccount = investorBank.OwnerAccount,
                        BankAccount = investorBank.BankAccount,
                        CoreBankName = bank.BankName,
                        CoreFullBankName = bank.FullBankName,
                        CoreLogo = bank.Logo

                    }).ToList();
        }

        /// <summary>
        /// Lấy địa chỉ ngân hàng theo investor id thật
        /// </summary>
        /// <param name="investorId"></param>
        /// <returns></returns>
        public InvestorBankAccount GetDefaultBankAccount(int investorId)
        {
            _logger.LogInformation($"{System.Reflection.MethodBase.GetCurrentMethod().Name}: investorId = {investorId}");
            var defaultBank = _epicDbContext.InvestorBankAccounts
                        .FirstOrDefault(c => c.InvestorId == investorId && c.Deleted == YesNo.NO && c.IsDefault == YesNo.YES);

            if (defaultBank == null)
            {
                defaultBank = GetListBank(investorId)?.FirstOrDefault();
            }

            return defaultBank;
        }

        /// <summary>
        /// Có tài khoản ngân hàng nào trùng không (ở bảng bank thật)
        /// </summary>
        /// <param name="bankId"></param>
        /// <param name="bankAccount"></param>
        /// <returns></returns>
        public bool AnySameBank(int bankId, string bankAccount)
        {
            _logger.LogInformation($"{System.Reflection.MethodBase.GetCurrentMethod().Name}: bankId = {bankId}; bankAccount = {bankAccount}");
            return _epicDbContext.InvestorBankAccounts.Any(b => b.BankId == bankId && b.BankAccount == bankAccount && b.Deleted == YesNo.NO);
        }

        /// <summary>
        /// Lấy list giấy tờ theo investor id
        /// </summary>
        /// <param name="investorId"></param>
        /// <returns></returns>
        public IQueryable<InvestorIdentification> GetListIdentification(int investorId)
        {
            _logger.LogInformation($"{System.Reflection.MethodBase.GetCurrentMethod().Name}: investorId = {investorId}");
            var result = _epicDbContext.InvestorIdentifications
                                    .Where(c => c.InvestorId == investorId && c.Deleted == YesNo.NO)
                                    .OrderByDescending(c => c.IsDefault).OrderByDescending(c => c.Id);

            return result;
        }

        /// <summary>
        /// Lấy list giấy tờ theo investor id temp
        /// </summary>
        /// <param name="investorId"></param>
        /// <returns></returns>
        public IQueryable<InvestorIdTemp> GetListIdentificationTemp(int investorId)
        {
            _logger.LogInformation($"{System.Reflection.MethodBase.GetCurrentMethod().Name}: investorId = {investorId}");
            var result = _epicDbContext.InvestorIdTemps
                                    .Where(c => c.InvestorId == investorId && c.Deleted == YesNo.NO)
                                    .OrderByDescending(c => c.IsDefault).OrderByDescending(c => c.Id);

            return result;
        }

        /// <summary>
        /// Lấy giấy tờ theo investor identification id
        /// </summary>
        /// <param name="investorIdentificationId"></param>
        /// <returns></returns>
        public InvestorIdentification GetIdentificationById(int investorIdentificationId)
        {
            _logger.LogInformation($"{System.Reflection.MethodBase.GetCurrentMethod().Name}: investorIdentificationId = {investorIdentificationId}");

            var result = _epicDbContext.InvestorIdentifications
                                    .FirstOrDefault(c => c.Id == investorIdentificationId && c.Deleted == YesNo.NO);
            return result;
        }

        /// <summary>
        /// Lấy giấy tờ temp theo investor id temp
        /// </summary>
        /// <param name="idenTempId"></param>
        /// <returns></returns>
        public InvestorIdTemp GetIdenTempById(int idenTempId)
        {
            _logger.LogInformation($"{System.Reflection.MethodBase.GetCurrentMethod().Name}: investorIdentificationId = {idenTempId}");

            var result = _epicDbContext.InvestorIdTemps
                                    .FirstOrDefault(c => c.Id == idenTempId && c.Deleted == YesNo.NO);
            return result;
        }

        /// <summary>
        /// Lấy giấy tờ mặc định theo investor id thật
        /// </summary>
        /// <param name="investorId"></param>
        /// <returns></returns>
        public InvestorIdentification GetDefaultIdentification(int investorId)
        {
            _logger.LogInformation($"{System.Reflection.MethodBase.GetCurrentMethod().Name}: investorId = {investorId}");

            var defaultIden = _epicDbContext.InvestorIdentifications
                        .FirstOrDefault(c => c.InvestorId == investorId && c.Deleted == YesNo.NO && c.IsDefault == YesNo.YES);

            if (defaultIden == null)
            {
                defaultIden = GetListIdentification(investorId)?.FirstOrDefault();
            }

            return defaultIden;
        }

        /// <summary>
        /// Lấy giấy tờ temp mặc định theo investor id temp
        /// </summary>
        /// <param name="investorId"></param>
        /// <returns></returns>
        public InvestorIdTemp GetDefaultIdentificationTemp(int investorId)
        {
            _logger.LogInformation($"{System.Reflection.MethodBase.GetCurrentMethod().Name}: investorId = {investorId}");

            var defaultIden = _epicDbContext.InvestorIdTemps
                        .FirstOrDefault(c => c.InvestorId == investorId && c.Deleted == YesNo.NO && c.IsDefault == YesNo.YES);

            if (defaultIden == null)
            {
                defaultIden = GetListIdentificationTemp(investorId)?.FirstOrDefault();
            }

            return defaultIden;
        }

        /// <summary>
        /// Lấy list chứng khoán theo investor id
        /// </summary>
        /// <param name="investorId"></param>
        /// <returns></returns>
        public IQueryable<InvestorStock> GetListStock(int investorId)
        {
            _logger.LogInformation($"{System.Reflection.MethodBase.GetCurrentMethod().Name}: investorId = {investorId}");

            var result = _epicDbContext.InvestorStocks
                                    .Where(c => c.InvestorId == investorId && c.Deleted == YesNo.NO)
                                    .OrderByDescending(c => c.IsDefault).OrderByDescending(c => c.Id);

            return result;
        }

        /// <summary>
        /// Lấy chứng khoán mặc định theo investor id thật
        /// </summary>
        /// <param name="investorId"></param>
        /// <returns></returns>
        public InvestorStock GetDefaultStock(int investorId)
        {
            _logger.LogInformation($"{System.Reflection.MethodBase.GetCurrentMethod().Name}: investorId = {investorId}");

            var result = _epicDbContext.InvestorStocks
                        .FirstOrDefault(c => c.InvestorId == investorId && c.Deleted == YesNo.NO && c.IsDefault == YesNo.YES);

            if (result == null)
            {
                result = GetListStock(investorId)?.FirstOrDefault();
            }

            return result;
        }

        public void FinishEKYC(string phone, string faceImageUrl)
        {
            _logger.LogInformation($"{System.Reflection.MethodBase.GetCurrentMethod().Name}: phone = {phone}; faceImageUrl = {faceImageUrl}");

            var converted = ObjectToParamAndQuery(PROC_INVESTOR_EKYC_FINISH, new
            {
                pv_PHONE = phone,
                pv_FACE_IMAGE_URL = faceImageUrl
            });

            _dbContext.Database.ExecuteSqlRaw(converted.SqlQuery, converted.Parameters);
        }

        /// <summary>
        /// Lấy investor theo id (bất kể trạng thái)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Investor FindById(int id)
        {
            _logger.LogInformation($"{System.Reflection.MethodBase.GetCurrentMethod().Name}: investorId = {id}");

            var result = _epicSchemaDbContext.Investors.FirstOrDefault(i => i.InvestorId == id && i.Deleted == YesNo.NO);
            return result;
        }

        /// <summary>
        /// Lấy investor tạm và đang có yêu cầu duyệt (trạng thái 1) theo phone
        /// </summary>
        /// <returns></returns>
        public InvestorTemp GetInvestorTempByPhone(string phone)
        {
            _logger.LogInformation($"{System.Reflection.MethodBase.GetCurrentMethod().Name}: phone = {phone}");

            var query = from it in _epicDbContext.InvestorTemps
                        from ca in _epicDbContext.CoreApproves.Where(x => x.ReferIdTemp == it.InvestorId).DefaultIfEmpty()
                        where it.Phone == phone && it.Deleted == YesNo.NO && (ca.Status == null || ca.Status == CoreApproveStatus.TRINH_DUYET)
                        select it;
            return query.FirstOrDefault();
        }

        /// <summary>
        /// Lấy investor tạm và đang có yêu cầu duyệt (trạng thái 1) theo phone
        /// </summary>
        /// <returns></returns>
        public InvestorTemp GetInvestorTempByEmail(string email)
        {
            _logger.LogInformation($"{System.Reflection.MethodBase.GetCurrentMethod().Name}: email = {email}");

            var query = from it in _epicDbContext.InvestorTemps
                        from ca in _epicDbContext.CoreApproves.Where(x => x.ReferIdTemp == it.InvestorId).DefaultIfEmpty()
                        where it.Email.ToLower() == email.ToLower() && it.Deleted == YesNo.NO && (ca.Status == null || ca.Status == CoreApproveStatus.TRINH_DUYET)
                        select it;
            return query.FirstOrDefault();
        }

        /// <summary>
        /// Tạo investor thật
        /// </summary>
        /// <param name="investor"></param>
        public void CreateInvestor(Investor investor, int refCodeSelfLen = 10)
        {
            _logger.LogInformation($"{System.Reflection.MethodBase.GetCurrentMethod().Name}: investor = {JsonSerializer.Serialize(investor)}; refCodeSelfLen = {refCodeSelfLen}");

            //var refCodeSelf = GenerateReferral(refCodeSelfLen);
            var refCodeSelf = investor.Phone;

            do
            {
                investor.InvestorId = (int)NextKey();
            } while (EntityNoTracking.Any(x => x.InvestorId == investor.InvestorId));

            do
            {
                investor.InvestorGroupId = (int)NextKey(Investor.SEQ_GROUP);
            } while (EntityNoTracking.Any(x => x.InvestorGroupId == investor.InvestorGroupId));

            investor.ReferralCodeSelf = refCodeSelf;
            investor.CreatedDate = DateTime.Now;
            investor.Deleted = YesNo.NO;

            investor.Step = InvestorAppStep.BAT_DAU;

            _dbSet.Add(investor);
        }

        /// <summary>
        /// Lấy investor theo mã giới thiệu (bất kể trạng thái)
        /// </summary>
        /// <param name="referralCodeSelf"></param>
        /// <returns></returns>
        public Investor GetByReferralCodeSelf(string referralCodeSelf)
        {
            _logger.LogInformation($"{System.Reflection.MethodBase.GetCurrentMethod().Name}: referralCodeSelf = {referralCodeSelf}");
            var result = _epicDbContext.Investors
                            .FirstOrDefault(i => i.ReferralCodeSelf == referralCodeSelf && i.Deleted == YesNo.NO && i.Status != InvestorStatus.LOCKED);
            return result;
        }

        /// <summary>
        /// Check xem investor đã quét mã giới thiệu này bao giờ chưa
        /// </summary>
        /// <param name="investorId"></param>
        /// <param name="referralCode"></param>
        /// <returns></returns>
        public bool IsReferralCodeInUse(int investorId, string referralCode)
        {
            _logger.LogInformation($"{System.Reflection.MethodBase.GetCurrentMethod().Name}: investorId = {investorId}; referralCode = {referralCode}");
            var useInInvestor = _epicDbContext.Investors
                                    .Any(i => i.InvestorId == investorId && i.ReferralCode == referralCode && i.Deleted == YesNo.NO && i.Status != InvestorStatus.TEMP && i.Status != InvestorStatus.LOCKED);
            var useInInvestorSale = _epicDbContext.InvestorSales
                                        .Any(iss => iss.InvestorId == investorId && iss.ReferralCode == referralCode && iss.Deleted == YesNo.NO);
            return useInInvestor || useInInvestorSale;
        }

        /// <summary>
        /// Check trùng ngân hàng và số tài khoản
        /// </summary>
        /// <param name="bankId"></param>
        /// <param name="bankAccount"></param>
        /// <returns></returns>
        public bool AnyBankAccount(int bankId, string bankAccount)
        {
            _logger.LogInformation($"{System.Reflection.MethodBase.GetCurrentMethod().Name}: bankId = {bankId}; bankAccount = {bankAccount}");

            return _epicDbContext.InvestorBankAccounts.Any(b => b.BankId == bankId && b.BankAccount == bankAccount && b.Deleted == YesNo.NO);
        }

        /// <summary>
        /// Tạo liên kết ngân hàng ở bảng thật. Đồng thời tự update giá trị isDefault cho phù hợp.
        /// </summary>
        /// <param name="entity"></param>
        public void AddBankAccountWithDefault(InvestorBankAccount entity)
        {
            _logger.LogInformation($"{System.Reflection.MethodBase.GetCurrentMethod().Name}: entity = {JsonSerializer.Serialize(entity)}");

            var isHaveDefaultAlready = _epicDbContext.InvestorBankAccounts
                                        .Any(b => b.InvestorId == entity.InvestorId && b.Deleted == YesNo.NO && b.IsDefault == YesNo.YES);
            if (isHaveDefaultAlready && entity.IsDefault == YesNo.YES)
            {
                // NEU LA DEFAULT => XOA HET DEFAULT TRUOC
                var listBankAccountOld = _epicDbContext.InvestorBankAccounts
                                            .Where(b => b.InvestorId == entity.InvestorId && b.Deleted == YesNo.NO && b.IsDefault == YesNo.YES);

                foreach (var oldBank in listBankAccountOld)
                {
                    oldBank.IsDefault = YesNo.NO;
                }
            }
            else
            {
                // NEU CHUA CO DEFAULT => SET DEFAULT
                entity.IsDefault = YesNo.YES;
            }

            entity.CreatedDate = DateTime.Now;
            entity.Deleted = YesNo.NO;

            do
            {
                entity.Id = (int)NextKey(InvestorBankAccount.SEQ);
            } while (_epicDbContext.InvestorBankAccounts.AsNoTracking().Any(x => x.Id == entity.Id));

            _epicDbContext.InvestorBankAccounts.Add(entity);
        }

        /// <summary>
        /// Kiểm tra xem có trùng số giấy tờ không (Khi khách hàng trên app thực hiện thêm giấy tờ)
        /// </summary>
        /// <param name="idNo"></param>
        /// <returns></returns>
        public bool AnyAppIdentitficationDuplicate(string idNo, int? investorGroupId)
        {
            _logger.LogInformation($"{System.Reflection.MethodBase.GetCurrentMethod().Name}: idNo = {idNo}; investorGroupId = {investorGroupId}");

            var isInIden = (
                                from ii in _epicDbContext.InvestorIdentifications
                                join i in _epicDbContext.Investors on new { investorId = ii.InvestorId, deleted = YesNo.NO } equals new { investorId = i.InvestorId, deleted = i.Deleted }
                                where ii.IdNo == idNo && ii.Deleted == YesNo.NO && ii.InvestorGroupId != investorGroupId
                                select ii
                            ).Any();
            var isInIdenTemp = _epicDbContext.InvestorIdTemps.Any(it => it.IdNo == idNo && it.Deleted == YesNo.NO && it.InvestorGroupId != investorGroupId);

            return isInIden || isInIdenTemp;
        }

        /// <summary>
        /// Lấy giấy tờ thật có trạng thái tạm theo số giấy tờ và investor id
        /// </summary>
        /// <param name="investorId"></param>
        /// <param name="idNo"></param>
        /// <returns></returns>
        public InvestorIdentification FindTempIdentificationByIdNo(int investorId, string idNo)
        {
            _logger.LogInformation($"{System.Reflection.MethodBase.GetCurrentMethod().Name}: investorId = {investorId}; idNo = {idNo}");

            return _epicDbContext.InvestorIdentifications
                        .FirstOrDefault(it => it.IdNo == idNo && it.Deleted == YesNo.NO && it.InvestorId == investorId && it.Status == InvestorStatus.TEMP);
        }

        /// <summary>
        /// Xóa mềm giấy tờ ở bảng thật theo investor id
        /// </summary>
        /// <param name="investorId"></param>
        public void SoftDeleteIdentification(int investorId)
        {
            _logger.LogInformation($"{System.Reflection.MethodBase.GetCurrentMethod().Name}: investorId = {investorId}");

            var idenList = _epicDbContext.InvestorIdentifications.Where(ii => ii.InvestorId == investorId && ii.Deleted == YesNo.NO);

            foreach (var iden in idenList)
            {
                iden.IsDefault = YesNo.NO;
                iden.Deleted = YesNo.YES;
            }
        }

        /// <summary>
        /// Check xem investor này có giấy tờ mặc định nào chưa
        /// </summary>
        /// <param name="investorId"></param>
        /// <returns></returns>
        public bool AnyDefaultIdentification(int investorId)
        {
            _logger.LogInformation($"{System.Reflection.MethodBase.GetCurrentMethod().Name}: investorId = {investorId}");

            return _epicDbContext.InvestorIdentifications
                        .Any(ii => ii.InvestorId == investorId && ii.Deleted == YesNo.NO && ii.IsDefault == YesNo.YES);
        }

        /// <summary>
        /// Khách hàng app thêm giấy tờ (bảng thật | luồng đký)
        /// </summary>
        /// <param name="entity"></param>
        public void AddIdentification(InvestorIdentification entity)
        {
            _logger.LogInformation($"{System.Reflection.MethodBase.GetCurrentMethod().Name}: entity = {JsonSerializer.Serialize(entity)}");

            do
            {
                entity.Id = (int)NextKey(InvestorIdentification.SEQ);
            } while (_epicDbContext.InvestorIdentifications.AsNoTracking().Any(x => x.Id == entity.Id));
            entity.Deleted = YesNo.NO;

            _epicDbContext.InvestorIdentifications.Add(entity);
        }

        /// <summary>
        /// Lấy investor chi tiết hơn chút theo investor id 
        /// </summary>
        /// <param name="investorId"></param>
        /// <param name="isTemp">Lấy từ bảng tạm hay bảng thật</param>
        /// <returns></returns>
        public InvestorTemporary GetInvestorById(int investorId, bool isTemp)
        {
            _logger.LogInformation($"{System.Reflection.MethodBase.GetCurrentMethod().Name}: investorId = {investorId}; isTemp = {isTemp}");

            if (isTemp)
            {
                var query = from a in _epicDbContext.InvestorTemps
                            from inv in _epicDbContext.Investors.Where(x => x.InvestorGroupId == a.InvestorGroupId && x.Status != InvestorStatus.LOCKED).DefaultIfEmpty()
                            from cc in _epicDbContext.CifCodes.Where(x => x.InvestorId == inv.InvestorId).DefaultIfEmpty()
                            where a.InvestorId == investorId && a.Deleted == YesNo.NO
                            select new InvestorTemporary
                            {
                                InvestorId = a.InvestorId,
                                InvestorGroupId = a.InvestorGroupId ?? 0,
                                ReferralCodeSelf = a.ReferralCodeSelf,
                                Phone = a.Phone,
                                Email = a.Email,
                                Deleted = a.Deleted,
                                CreatedBy = a.CreatedBy,
                                CreatedDate = a.CreatedDate,
                                InvestorStatus = InvestorStatus.TEMP,
                                CifCode = cc.CifCode,
                                IsUpdate = inv.InvestorId == 0 ? YesNo.NO : YesNo.YES
                            };
                return query.FirstOrDefault();
                //from cc in tmpCc.defa
            }

            var investor = from a in _dbSet
                           from cc in _epicDbContext.CifCodes.Where(x => x.InvestorId == a.InvestorId).DefaultIfEmpty()
                           where a.InvestorId == investorId && a.Deleted == YesNo.NO && a.Status != InvestorStatus.LOCKED
                           select new InvestorTemporary
                           {
                               InvestorId = a.InvestorId,
                               InvestorGroupId = a.InvestorGroupId ?? 0,
                               ReferralCodeSelf = a.ReferralCodeSelf,
                               ReferralCode = a.ReferralCode,
                               ReferralDate = a.ReferralDate,
                               TradingProviderId = a.TradingProviderId ?? 0,
                               AvatarImageUrl = a.AvatarImageUrl,
                               Phone = a.Phone,
                               Email = a.Email,
                               Deleted = a.Deleted,
                               CreatedBy = a.CreatedBy,
                               CreatedDate = a.CreatedDate,
                               InvestorStatus = a.Status,
                               CifCode = cc.CifCode,
                               ProfStatus = 1
                           };
            return investor.FirstOrDefault();
        }

        /// <summary>
        /// Luồng đăng ký, bước cuối, nếu khách confirm là chưa đúng thì tạo bản ghi temp
        /// </summary>
        /// <param name="phone"></param>
        public void FinalStep(int investorId)
        {
            _logger.LogInformation($"{System.Reflection.MethodBase.GetCurrentMethod().Name}: investorId = {investorId}");

            var converted = ObjectToParamAndQuery(PROC_FINAL_STEP, new
            {
                pv_INVESTOR_ID = investorId,
            });

            _dbContext.Database.ExecuteSqlRaw(converted.SqlQuery, converted.Parameters);
        }

        /// <summary>
        /// Lấy investor đăng ký trên app cho màn hình tài khoản chưa xác minh
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IQueryable<InvestorNoEkycDto> FindInvestorNoEkyc(FindInvestorNoEkycDto dto)
        {
            _logger.LogInformation($"{System.Reflection.MethodBase.GetCurrentMethod().Name}: entity = {JsonSerializer.Serialize(dto)}");

            var query = (from i in _epicDbContext.Investors
                         from inv in _epicDbContext.Investors.Where(x => x.ReferralCodeSelf == i.ReferralCode && x.Deleted == YesNo.NO && x.Status == InvestorStatus.ACTIVE).DefaultIfEmpty()
                         from saleInv in _epicDbContext.Sales.Where(x => x.InvestorId == inv.InvestorId && x.Deleted == YesNo.NO).DefaultIfEmpty()
                         from bus in _epicDbContext.BusinessCustomers.Where(x => x.ReferralCodeSelf == i.ReferralCode && x.Deleted == YesNo.NO).DefaultIfEmpty()
                         from saleBus in _epicDbContext.Sales.Where(x => x.BusinessCustomerId == bus.BusinessCustomerId && x.Deleted == YesNo.NO).DefaultIfEmpty()
                         from saleTradingInv in _epicDbContext.SaleTradingProviders.Where(x => x.SaleId == saleInv.SaleId && x.Deleted == YesNo.NO).DefaultIfEmpty()
                         from saleTradingBus in _epicDbContext.SaleTradingProviders.Where(x => x.SaleId == saleBus.SaleId && x.Deleted == YesNo.NO).DefaultIfEmpty()
                         from user in _epicDbContext.Users.Where(x => x.InvestorId == i.InvestorId && x.UserType == UserTypes.INVESTOR && x.IsDeleted == YesNo.NO)
                         where i.Deleted == YesNo.NO && i.Status != InvestorStatus.LOCKED
                             && user.Status != UserStatus.LOCKED
                             && (dto.TradingProviderId == null || ((dto.TradingProviderId == saleTradingBus.TradingProviderId || dto.TradingProviderId == saleTradingInv.TradingProviderId)))
                             && (dto.Keyword == null || (i.Phone == dto.Keyword || i.Email == dto.Keyword))
                             && (string.IsNullOrEmpty(dto.Step) || dto.Step.Contains(i.Step.ToString()))
                             && (string.IsNullOrEmpty(dto.Status) || dto.Status.Contains(user.Status))
                             && (string.IsNullOrEmpty(dto.Source) || dto.Source.Contains(i.Source.ToString()))
                         orderby i.InvestorId descending
                         select new InvestorNoEkycDto
                         {
                             CreatedBy = i.CreatedBy,
                             CreatedDate = i.CreatedDate,
                             Email = i.Email,
                             Phone = i.Phone,
                             InvestorId = i.InvestorId,
                             ReferralCode = i.ReferralCode,
                             Source = i.Source,
                             Status = user.Status,
                             InvestorStatus = i.Status,
                             //TradingProviderId = (saleTradingInv != null ? saleTradingInv.TradingProviderId : (saleTradingBus != null ? saleTradingBus.TradingProviderId : null)),
                             Step = i.Step,
                             UserId = user.UserId
                         }).Distinct();
            return query;
        }

        /// <summary>
        /// Lấy list user theo investor id
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IQueryable<ViewUserDto> FindListUserByInvestorId(FindUserByInvestorId dto)
        {
            _logger.LogInformation($"{System.Reflection.MethodBase.GetCurrentMethod().Name}: dto = {JsonSerializer.Serialize(dto)}");
            var investor = _epicDbContext.Investors.Where(x => x.InvestorId == dto.InvestorId && x.Deleted == YesNo.NO).FirstOrDefault();
            var query = (from u in _epicDbContext.Users
                         from i in _epicDbContext.Investors.Where(x => x.InvestorId == u.InvestorId && x.Deleted == YesNo.NO).DefaultIfEmpty()
                         from bank in _epicDbContext.InvestorBankAccounts.Where(x => x.InvestorId == u.InvestorId && x.Deleted == YesNo.NO).DefaultIfEmpty()
                         from iden in _epicDbContext.InvestorIdentifications.Where(x => x.InvestorId == u.InvestorId && x.Deleted == YesNo.NO).DefaultIfEmpty()
                         where (string.IsNullOrEmpty(dto.Keyword) || u.UserName.ToLower().Contains(dto.Keyword.ToLower()))
                             && u.IsDeleted == YesNo.NO
                             && u.InvestorId == dto.InvestorId
                         select new
                         {
                             user = u,
                             investor = i,
                             bank = bank.Id,
                             iden = iden.Id,
                         }).GroupBy(x => new
                         {
                             UserId = x.user.UserId,
                             UserName = x.user.UserName,
                             Status = x.user.Status,
                             DisplayName = x.user.DisplayName,
                             IsVerifiedEmail = x.user.IsVerifiedEmail,
                             LockedDate = x.user.LockedDate,
                             CreatedDate = x.user.CreatedDate,
                             LastLogin = x.user.LastLogin,
                             Phone = x.investor.Phone,
                             Email = x.investor.Email,
                             IsCheck = x.investor.IsCheck,
                             UserType = x.user.UserType,
                             IsHavePin = x.user.PinCode,
                             FaceImageUrl = x.investor.FaceImageUrl,
                         }).Select(x => new ViewUserDto
                         {
                             UserId = x.Key.UserId,
                             UserName = x.Key.UserName,
                             Status = x.Key.Status,
                             DisplayName = x.Key.DisplayName,
                             IsVerifiedEmail = x.Key.IsVerifiedEmail,
                             LockedDate = x.Key.LockedDate,
                             CreatedDate = x.Key.CreatedDate,
                             LastLogin = x.Key.LastLogin,
                             Phone = x.Key.Phone,
                             Email = x.Key.Email,
                             IsCheck = x.Key.IsCheck,
                             UserType = x.Key.UserType,
                             IsHavePin = string.IsNullOrEmpty(x.Key.IsHavePin) ? YesNo.NO : YesNo.YES,
                             IsHaveBank = x.Count(s => s.bank != 0) > 0 ? YesNo.YES : YesNo.NO,
                             IsEkyc = x.Count(s => s.iden != 0) > 0 ? YesNo.YES : YesNo.NO,
                             IsVerifiedFace = string.IsNullOrEmpty(x.Key.FaceImageUrl) ? YesNo.NO : YesNo.YES
                         });
            return query;
        }

        /// <summary>
        /// Sinh mã giới thiệu của investor
        /// </summary>
        /// <returns></returns>
        private string GenerateReferral(int len = 10)
        {
            _logger.LogInformation($"{System.Reflection.MethodBase.GetCurrentMethod().Name}: len = {len}");

            string result = "";
            Investor dup = new Investor();
            do
            {
                result = CommonUtils.RandomNumber(len);
                dup = GetByReferralCodeSelf(result);
            } while (dup != null);

            return result;
        }

        /// <summary>
        /// Get Data Investor cho hợp đồng
        /// </summary>
        /// <param name="investorId"></param>
        /// <param name="identificationId"></param>
        /// <param name="bankAccId"></param>
        public InvestorDataForContractDto GetDataInvestorForContract(int investorId, int identificationId, int bankAccId, int? contactAddressId = null)
        {
            _logger.LogInformation($"{System.Reflection.MethodBase.GetCurrentMethod().Name}: investorId = {investorId}; identificationId = {identificationId}; bankAccId = {bankAccId}");

            InvestorDataForContractDto investorDataForContractDto = new();
            //thông tin investor
            investorDataForContractDto.Investor = _epicDbContext.Investors.FirstOrDefault(i => i.InvestorId == investorId && i.Deleted == YesNo.NO && i.Status != InvestorStatus.LOCKED);
            investorDataForContractDto.InvestorBankAccount = (from investorBank in _epicDbContext.InvestorBankAccounts
                                                              join bank in _epicSchemaDbContext.CoreBanks on investorBank.BankId equals bank.BankId
                                                              where investorBank.Id == bankAccId && investorBank.Deleted == YesNo.NO
                                                              select new InvestorBankAccount()
                                                              {
                                                                  Id = investorBank.Id,
                                                                  InvestorId = investorBank.InvestorId,
                                                                  OwnerAccount = investorBank.OwnerAccount,
                                                                  BankAccount = investorBank.BankAccount,
                                                                  CoreBankName = bank.BankName,
                                                                  CoreFullBankName = bank.FullBankName,
                                                                  CoreLogo = bank.Logo
                                                              }).FirstOrDefault();
            investorDataForContractDto.InvestorIdentification = _epicDbContext.InvestorIdentifications.FirstOrDefault(ii => ii.Id == identificationId && ii.Deleted == YesNo.NO && ii.Status == Status.ACTIVE);
            if(contactAddressId != null)
            {
                investorDataForContractDto.InvestorContactAddress = _epicDbContext.InvestorContactAddresses.FirstOrDefault(ia => ia.ContactAddressId == contactAddressId && ia.Deleted == YesNo.NO);
            }
            return investorDataForContractDto;
        }

        /// <summary>
        /// Thêm vào bảng quan hệ nữa nhà đầu tư và đại lý
        /// </summary>
        /// <param name="investorId"></param>
        /// <param name="tradingProviderId"></param>
        /// <param name="username"></param>
        public void InsertInvestorTradingProvider(int investorId, int tradingProviderId, string username = null)
        {
            _logger.LogInformation($"{System.Reflection.MethodBase.GetCurrentMethod().Name}: investorId = {investorId}; tradingProviderId = {tradingProviderId}; username = {username}");
            var investorTradingProvider = _epicSchemaDbContext.InvestorTradingProviders.Where(i => i.TradingProviderId == tradingProviderId && i.InvestorId == investorId && i.Deleted == YesNo.NO);
            if (!investorTradingProvider.Any())
            {
                _epicSchemaDbContext.InvestorTradingProviders.Add(new CoreEntities.DataEntities.InvestorTradingProvider
                {
                    Id = (int)NextKey(InvestorTradingProvider.SEQ),
                    InvestorId = investorId,
                    TradingProviderId = tradingProviderId,
                    CreatedBy = username,
                    CreatedDate = DateTime.Now,
                    Deleted = YesNo.NO
                });
            }
        }

        public List<UserReportDto> FindUserNoEkyc(int? tradingProvider)
        {
            _logger.LogInformation($"{System.Reflection.MethodBase.GetCurrentMethod().Name} : tradingProvider = {tradingProvider}");

            var query = (from i in _epicDbContext.Investors
                         from inv in _epicDbContext.Investors.Where(x => x.ReferralCodeSelf == i.ReferralCode && x.Deleted == YesNo.NO && x.Status == InvestorStatus.ACTIVE).DefaultIfEmpty()
                         from saleInv in _epicDbContext.Sales.Where(x => x.InvestorId == inv.InvestorId && x.Deleted == YesNo.NO).DefaultIfEmpty()
                         from bus in _epicDbContext.BusinessCustomers.Where(x => x.ReferralCodeSelf == i.ReferralCode && x.Deleted == YesNo.NO).DefaultIfEmpty()
                         from saleBus in _epicDbContext.Sales.Where(x => x.BusinessCustomerId == bus.BusinessCustomerId && x.Deleted == YesNo.NO).DefaultIfEmpty()
                         from saleTradingInv in _epicDbContext.SaleTradingProviders.Where(x => x.SaleId == saleInv.SaleId && x.Deleted == YesNo.NO).DefaultIfEmpty()
                         from saleTradingBus in _epicDbContext.SaleTradingProviders.Where(x => x.SaleId == saleBus.SaleId && x.Deleted == YesNo.NO).DefaultIfEmpty()
                         from user in _epicDbContext.Users.Where(x => x.InvestorId == i.InvestorId && x.UserType == UserTypes.INVESTOR && x.IsDeleted == YesNo.NO)
                         where i.Deleted == YesNo.NO && i.Status != InvestorStatus.LOCKED
                             && (tradingProvider == null || ((tradingProvider == saleTradingBus.TradingProviderId || tradingProvider == saleTradingInv.TradingProviderId)))
                            && "1,2,3".Contains(i.Step.ToString())
                         orderby user.CreatedDate descending
                         select new UserReportDto
                         {
                             Username = user.UserName,
                             CreatedDate = user.CreatedDate,
                             ReferralCode = i.ReferralCode,
                             Step = i.Step ?? 0,
                             FinalStepDate = i.FinalStepDate,
                             Status = user.Status,
                         }).Distinct();

            return query.ToList();
        }

        /// <summary>
        /// Update ngày các step eKYC
        /// </summary>
        /// <param name="step"></param>
        /// <param name=""></param>
        public void UpdateEkycStepDate(int? step, int? investorId)
        {
            _logger.LogInformation($"{System.Reflection.MethodBase.GetCurrentMethod().Name}: investorId = {investorId}; step = {step}");
            var investorFind = _epicSchemaDbContext.Investors.FirstOrDefault(i => i.InvestorId == investorId);
            if (investorFind != null)
            {
                if (step == 1)
                {
                    investorFind.FirstStepDate = DateTime.Now;
                }
                else if (step == 2)
                {
                    investorFind.SecondStepDate = DateTime.Now;
                }
                else if (step == 3)
                {
                    investorFind.ThirdStepDate = DateTime.Now;
                }
            }
        }

        /// <summary>
        /// Cập nhật ảnh khi nhận diện
        /// </summary>
        /// <param name="investorId"></param>
        /// <param name="faceImageType">Loại ảnh nhận diện truyền lên</param>
        /// <param name="faceImageUrl">Đường dẫn ảnh</param>
        public void UpdateFaceMatchImage(int investorId, int faceImageType, string faceImageUrl, double? similarity)
        {
            _logger.LogInformation($"{System.Reflection.MethodBase.GetCurrentMethod().Name}: investorId = {investorId}; faceImageType = {faceImageType}; value = {faceImageUrl}");
            var investorFind = _epicSchemaDbContext.Investors.FirstOrDefault(i => i.InvestorId == investorId).ThrowIfNull(_epicSchemaDbContext, ErrorCode.InvestorNotFound);
            if (investorFind != null)
            {
                switch (faceImageType)
                {
                    case FaceMatchImageTypes.ANH_MAT_TRAI:
                        investorFind.FaceImageUrl1 = faceImageUrl;
                        investorFind.FaceImageSimilarity1 = similarity;
                        break;
                    case FaceMatchImageTypes.ANH_MAT_PHAI:
                        investorFind.FaceImageUrl2 = faceImageUrl;
                        investorFind.FaceImageSimilarity2 = similarity;
                        break;
                    case FaceMatchImageTypes.ANH_MAT_NHAY_MAT:
                        investorFind.FaceImageUrl3 = faceImageUrl;
                        investorFind.FaceImageSimilarity3 = similarity;
                        break;
                    case FaceMatchImageTypes.ANH_MAT_CUOI:
                        investorFind.FaceImageUrl4 = faceImageUrl;
                        investorFind.FaceImageSimilarity4 = similarity;
                        break;
                    default:
                        break;
                }
            }
        }

        /// <summary>
        /// Tạo investor temp
        /// </summary>
        /// <param name="entity"></param>
        public InvestorTemp CreateInvestorTemp(InvestorTemp entity)
        {
            entity.InvestorId = (int)NextKey(InvestorTemp.SEQ);
            entity.CreatedDate = DateTime.Now;
            entity.Deleted = YesNo.NO;

            _epicSchemaDbContext.InvestorTemps.Add(entity);

            return entity;
        }

        /// <summary>
        /// Thêm giấy tờ vào bảng tạm
        /// </summary>
        /// <param name="entity"></param>
        public void AddIdentificationTemp(InvestorIdTemp entity)
        {
            _logger.LogInformation($"{System.Reflection.MethodBase.GetCurrentMethod().Name}: entity = {JsonSerializer.Serialize(entity)}");

            do
            {
                entity.Id = (int)NextKey(InvestorIdTemp.SEQ);
            } while (_epicDbContext.InvestorIdentifications.AsNoTracking().Any(x => x.Id == entity.Id));
            entity.Deleted = YesNo.NO;

            _epicDbContext.InvestorIdTemps.Add(entity);
        }

        /// <summary>
        /// Copy bank thật sang bank temp
        /// </summary>
        /// <param name="mapper"></param>
        /// <param name="investorIdTemp"></param>
        /// <param name="investorGroupId"></param>
        public void MoveBankActualToTemp(IMapper mapper, int investorIdTemp, int investorGroupId)
        {
            _logger.LogInformation($"{System.Reflection.MethodBase.GetCurrentMethod().Name}: investorIdTemp = {investorIdTemp}; investorGroupId = {investorGroupId}");

            var listBank = _epicDbContext.InvestorBankAccounts.Where(x => x.InvestorGroupId == investorGroupId && x.Deleted == YesNo.NO);
            var listBankTemps = mapper.Map<List<InvestorBankAccountTemp>>(listBank);

            foreach (var item in listBankTemps)
            {
                item.ReferId = item.Id;
                item.Id = (int)NextKey(InvestorBankAccountTemp.SEQ);
                item.InvestorId = investorIdTemp;
            }

            _epicDbContext.InvestorBankAccountTemps.AddRange(listBankTemps);
        }

        /// <summary>
        /// Copy contact address thật sang temp
        /// </summary>
        /// <param name="mapper"></param>
        /// <param name="investorIdTemp"></param>
        /// <param name="investorGroupId"></param>
        public void MoveContactAddressActualToTemp(IMapper mapper, int investorIdTemp, int investorGroupId)
        {
            _logger.LogInformation($"{System.Reflection.MethodBase.GetCurrentMethod().Name}: investorIdTemp = {investorIdTemp}; investorGroupId = {investorGroupId}");

            var listAddress = _epicDbContext.InvestorContactAddresses.Where(x => x.InvestorGroupId == investorGroupId && x.Deleted == YesNo.NO);
            var listAddrrssTemp = mapper.Map<List<InvestorContactAddressTemp>>(listAddress);

            foreach (var item in listAddrrssTemp)
            {
                item.ReferId = item.ContactAddressId;
                item.ContactAddressId = (int)NextKey(InvestorContactAddressTemp.SEQ);
                item.InvestorId = investorIdTemp;
            }

            _epicDbContext.InvestorContactAddressTemps.AddRange(listAddrrssTemp);
        }

        /// <summary>
        /// Copy giấy tờ thật sang temp
        /// </summary>
        /// <param name="mapper"></param>
        /// <param name="investorIdTemp"></param>
        /// <param name="investorGroupId"></param>
        public void MoveIdentificationActualToTemp(IMapper mapper, int investorIdTemp, int investorGroupId)
        {
            _logger.LogInformation($"{System.Reflection.MethodBase.GetCurrentMethod().Name}: investorIdTemp = {investorIdTemp}; investorGroupId = {investorGroupId}");

            var listIden = _epicDbContext.InvestorIdentifications.Where(x => x.InvestorGroupId == investorGroupId && x.Deleted == YesNo.NO);
            var listIdenTemp = mapper.Map<List<InvestorIdTemp>>(listIden);

            foreach (var item in listIdenTemp)
            {
                item.ReferId = item.Id;
                item.Id = (int)NextKey(InvestorIdTemp.SEQ);
                item.InvestorId = investorIdTemp;
                item.IsVerifiedIdentification = YesNo.NO;
                item.Deleted = YesNo.NO;
            }

            _epicDbContext.InvestorIdTemps.AddRange(listIdenTemp);
        }

        /// <summary>
        /// Lấy list bank acc temp theo investor id temp (Cả xóa hoặc chưa xóa)
        /// </summary>
        /// <param name="investorId"></param>
        /// <returns></returns>
        public IEnumerable<InvestorBankAccount> GetListBankAccTemp(int investorId)
        {
            var query = from invBankAccTemp in _epicDbContext.InvestorBankAccountTemps
                        from bank in _epicDbContext.CoreBanks.Where(x => x.BankId == invBankAccTemp.BankId).DefaultIfEmpty()
                        where invBankAccTemp.InvestorId == investorId
                        select
                        new InvestorBankAccount
                        {
                            CreatedDate = invBankAccTemp.CreatedDate,
                            InvestorId = invBankAccTemp.InvestorId,
                            BankAccount = invBankAccTemp.BankAccount,
                            BankBranch = invBankAccTemp.BankBranch,
                            OwnerAccount = invBankAccTemp.OwnerAccount,
                            BankCode = invBankAccTemp.BankCode,
                            BankName = invBankAccTemp.BankName,
                            CoreFullBankName = bank.BankName,
                            BankId = invBankAccTemp.BankId,
                            Deleted = invBankAccTemp.Deleted,
                            CreatedBy = invBankAccTemp.CreatedBy,
                            Id = invBankAccTemp.Id,
                            ReferId = invBankAccTemp.ReferId,
                            IsDefault = invBankAccTemp.IsDefault,
                            InvestorGroupId = invBankAccTemp.InvestorGroupId
                        };

            return query;
        }

        /// <summary>
        /// Tìm thông tin giấy tờ khách hàng theo idNo
        /// </summary>
        /// <param name="idNo"></param>
        /// <returns></returns>
        public InvestorIdentification FindIdentificationByIdNo(string idNo)
        {
            _logger.LogInformation($"{nameof(InvestorEFRepository)}->{nameof(FindIdentificationByIdNo)}: idNo = {idNo}");
            var result = _epicSchemaDbContext.InvestorIdentifications.FirstOrDefault(o => o.IdNo == idNo);
            return result;
        }

        /// <summary>
        /// Tìm investor theo cifcode (mã khách hàng)
        /// </summary>
        /// <param name="cifcode"></param>
        /// <returns></returns>
        public Investor FindByCifCode(string cifcode)
        {
            _logger.LogInformation($"{nameof(Investor)}->{nameof(FindByCifCode)}: cifcode = {cifcode}");

            var query = from cif in _epicDbContext.CifCodes
                        join inv in _dbSet on cif.InvestorId equals inv.InvestorId
                        where cif.CifCode == cifcode && cif.Deleted == YesNo.NO && inv.Deleted == YesNo.NO
                        select inv;
            return query.FirstOrDefault();
        }

        /// <summary>
        /// Lấy list đại lý theo investor id
        /// </summary>
        /// <param name="investorId"></param>
        /// <returns></returns>
        public List<ViewTradingInfoDto> FindTradingByInvestorId(int investorId)
        {
            _logger.LogInformation($"{nameof(Investor)}->{nameof(FindTradingByInvestorId)}: investorId = {investorId}");

            var query = ((
                            from inv in _dbSet
                            join invTra in _epicDbContext.InvestorTradingProviders on inv.InvestorId equals invTra.InvestorId
                            join tra in _epicDbContext.TradingProviders on invTra.TradingProviderId equals tra.TradingProviderId
                            join bus in _epicDbContext.BusinessCustomers on tra.BusinessCustomerId equals bus.BusinessCustomerId
                            from point in _epicDbContext.LoyPointInvestors.AsNoTracking().Where(x => x.TotalPoint != null && x.InvestorId == investorId && x.TradingProviderId == tra.TradingProviderId && x.Deleted == YesNo.NO)
                            from rank in _epicDbContext.LoyRanks.AsNoTracking().Where(x => x.PointStart <= point.TotalPoint && point.TotalPoint <= x.PointEnd && x.Status == LoyRankStatus.ACTIVE && x.Deleted == YesNo.NO && x.TradingProviderId == point.TradingProviderId).DefaultIfEmpty()
                            where inv.InvestorId == investorId && inv.Deleted == YesNo.NO && invTra.Deleted == YesNo.NO && tra.Deleted == YesNo.NO && bus.Deleted == YesNo.NO
                            select new ViewTradingInfoDto
                            {
                                TradingProviderId = tra.TradingProviderId,
                                TradingProviderName = bus.Name,
                                Avatar = bus.AvatarImageUrl,
                                TotalPoint = point.TotalPoint,
                                CurrentPoint = point.CurrentPoint,
                                RankName = rank.Name,
                            }
                        )
                        .Union(
                            from inv in _dbSet
                            join invSale in _epicDbContext.InvestorSales on inv.InvestorId equals invSale.InvestorId
                            join saleTrading in _epicDbContext.SaleTradingProviders on invSale.SaleId equals saleTrading.SaleId
                            join tra in _epicDbContext.TradingProviders on saleTrading.TradingProviderId equals tra.TradingProviderId
                            join bus in _epicDbContext.BusinessCustomers on tra.BusinessCustomerId equals bus.BusinessCustomerId
                            where inv.InvestorId == investorId && inv.Deleted == YesNo.NO && invSale.Deleted == YesNo.NO && tra.Deleted == YesNo.NO && bus.Deleted == YesNo.NO
                            from point in _epicDbContext.LoyPointInvestors.AsNoTracking().Where(x => x.TotalPoint != null && x.InvestorId == investorId && x.TradingProviderId == tra.TradingProviderId && x.Deleted == YesNo.NO)
                            from rank in _epicDbContext.LoyRanks.AsNoTracking().Where(x => x.PointStart <= point.TotalPoint && point.TotalPoint <= x.PointEnd && x.Status == LoyRankStatus.ACTIVE && x.Deleted == YesNo.NO && x.TradingProviderId == point.TradingProviderId).DefaultIfEmpty()
                            select new ViewTradingInfoDto
                            {
                                TradingProviderId = tra.TradingProviderId,
                                TradingProviderName = bus.Name,
                                Avatar = bus.AvatarImageUrl,
                                TotalPoint = point.TotalPoint,
                                CurrentPoint = point.CurrentPoint,
                                RankName = rank.Name,
                            }
                        )).Distinct();
            return query.ToList();                                  
        }


        public static bool CheckInvestorTrading(EpicSchemaDbContext dbContext, int? tradingProviderId, Investor investor, string userStatus)
        {
            var result = (((new string[] { InvestorStatus.LOCKED, InvestorStatus.ACTIVE, InvestorStatus.DEACTIVE }.Contains(investor.Status)) && UserStatus.ACTIVE == userStatus
                            && (dbContext.InvestorTradingProviders.Any(investorTrading => investorTrading.Deleted == YesNo.NO
                            && tradingProviderId == investorTrading.TradingProviderId && investorTrading.InvestorId == investor.InvestorId)
                            || (from eis in dbContext.InvestorSales
                                join esctp in dbContext.SaleTradingProviders on eis.SaleId equals esctp.SaleId
                                where eis.InvestorId == investor.InvestorId && esctp.Deleted == YesNo.NO && eis.Deleted == YesNo.NO && tradingProviderId == esctp.TradingProviderId
                                select eis.InvestorId).Any()))
                        // Khách hàng chưa xác minh có nhập mã giới thiệu sale thuộc đại lý
                        || ((investor.Step == null || investor.Step == 1 || investor.Step == 2) && userStatus != null && userStatus == UserStatus.TEMP
                        // Nhập mã giới thiệu sale là Investor
                        && ((from inv in dbContext.Investors.Where(x => x.ReferralCodeSelf == investor.ReferralCode && x.Deleted == YesNo.NO && x.Status == InvestorStatus.ACTIVE).DefaultIfEmpty()
                                from saleInv in dbContext.Sales.Where(x => x.InvestorId == inv.InvestorId && x.Deleted == YesNo.NO).DefaultIfEmpty()
                                from saleTradingInv in dbContext.SaleTradingProviders.Where(x => x.SaleId == saleInv.SaleId && x.Deleted == YesNo.NO).DefaultIfEmpty()
                                where (tradingProviderId == null || tradingProviderId == saleTradingInv.TradingProviderId)
                                select saleTradingInv.TradingProviderId).Any()
                        // Nhập mã giới thiệu sale là BussinessCustomer
                        || (from bus in dbContext.BusinessCustomers.Where(x => x.ReferralCodeSelf == investor.ReferralCode && x.Deleted == YesNo.NO).DefaultIfEmpty()
                            from saleBus in dbContext.Sales.Where(x => x.BusinessCustomerId == bus.BusinessCustomerId && x.Deleted == YesNo.NO).DefaultIfEmpty()
                            from saleTradingBus in dbContext.SaleTradingProviders.Where(x => x.SaleId == saleBus.SaleId && x.Deleted == YesNo.NO).DefaultIfEmpty()
                            where (tradingProviderId == null || tradingProviderId == saleTradingBus.TradingProviderId)
                            select saleTradingBus.TradingProviderId).Any())));
            return result;
        }
    }
}
