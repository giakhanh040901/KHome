using AutoMapper;
using EPIC.CoreDomain.Interfaces;
using EPIC.CoreDomain.Interfaces.v2;
using EPIC.CoreEntities.Dto.InvestorRegistorLog;
using EPIC.CoreEntities.Dto.ManagerInvestor;
//using EPIC.CoreEntities.Dto.Investor;
using EPIC.CoreRepositories;
using EPIC.CoreSharedEntities.Dto.Investor;
using EPIC.CoreSharedEntities.Dto.TradingProvider;
using EPIC.CoreSharedServices.Implements;
using EPIC.DataAccess.Base;
using EPIC.DataAccess.Models;
using EPIC.Entities.DataEntities;
using EPIC.Entities.Dto.Auth;
using EPIC.Entities.Dto.ManagerInvestor;
using EPIC.Entities.Dto.User;
using EPIC.FileDomain.Services;
using EPIC.IdentityEntities.DataEntities;
using EPIC.IdentityRepositories;
using EPIC.Notification.Services;
using EPIC.Utils;
using EPIC.Utils.ConfigModel;
using EPIC.Utils.ConstantVariables.Core;
using EPIC.Utils.ConstantVariables.Identity;
using EPIC.Utils.ConstantVariables.Shared;
using EPIC.Utils.ConstantVariables.Shared.Sysvar;
using EPIC.Utils.DataUtils;
using EPIC.Utils.Recognition.FPT;
using EPIC.Utils.Security;
using Hangfire;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text.Json;
using System.Threading.Tasks;
using System.Transactions;

namespace EPIC.CoreDomain.Implements.v2
{
    public class InvestorV2Services : IInvestorV2Services
    {
        private readonly EpicSchemaDbContext _dbContext;
        private readonly ILogger<InvestorV2Services> _logger;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IMapper _mapper;
        private readonly NotificationServices _sendEmailService;
        private readonly IFileServices _imageServices;
        private readonly IBackgroundJobClient _backgroundJobs;
        private readonly BackgroundJobIdentityService _backgroundJobIdentityService;
        private readonly IOptions<RecognitionApiConfiguration> _recognitionApiConfig;
        private readonly OCRUtils _ocr;
        private readonly InvestorEFRepository _investorRepository;
        private readonly SysVarEFRepository _sysVarRepository;
        private readonly UsersEFRepository _userRepository;
        private readonly AuthOtpEFRepository _authOtpRepository;
        private readonly CifCodeEFRepository _cifCodeRepository;
        private readonly BusinessCustomerEFRepository _businessCustomerRepository;
        private readonly SaleEFRepository _saleRepository;
        private readonly InvestorSaleEFRepository _investorSaleRepository;
        private readonly IInvestorRegisterLogServices _investorRegisterLogServices;
        private readonly InvestorRegisterLogEFRepository _investorRegisterLogEFRepository;

        public InvestorV2Services(
            EpicSchemaDbContext dbContext,
            ILogger<InvestorV2Services> logger,
            IHttpContextAccessor httpContext,
            IMapper mapper,
            IFileServices imageServices,
            IOptions<RecognitionApiConfiguration> recognitionApiConfiguration,
            NotificationServices sendEmailService,
            BackgroundJobIdentityService backgroundJobIdentityService,
            IBackgroundJobClient backgroundJobs,
            IInvestorRegisterLogServices investorRegisterLogServices
        )
        {
            _dbContext = dbContext;
            _logger = logger;
            _sendEmailService = sendEmailService;
            _httpContext = httpContext;
            _imageServices = imageServices;
            _recognitionApiConfig = recognitionApiConfiguration;
            _backgroundJobs = backgroundJobs;
            _mapper = mapper;
            _backgroundJobIdentityService = backgroundJobIdentityService;
            _ocr = new OCRUtils(_recognitionApiConfig.Value, _logger);
            _investorRepository = new InvestorEFRepository(dbContext, logger);
            _sysVarRepository = new SysVarEFRepository(dbContext);
            _userRepository = new UsersEFRepository(dbContext, logger);
            _authOtpRepository = new AuthOtpEFRepository(dbContext, logger);
            _cifCodeRepository = new CifCodeEFRepository(dbContext, logger);
            _businessCustomerRepository = new BusinessCustomerEFRepository(dbContext, logger);
            _saleRepository = new SaleEFRepository(dbContext, logger);
            _investorSaleRepository = new InvestorSaleEFRepository(dbContext, logger);
            _investorRegisterLogServices = investorRegisterLogServices;
            _investorRegisterLogEFRepository = new InvestorRegisterLogEFRepository(dbContext, logger);
        }
        public int ActivateManagerInvestor(int investorId, bool isActive)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Thêm liên kết ngân hàng
        /// </summary>
        /// <param name="dto"></param>
        public void AddBank(CreateBankDto dto)
        {
            _logger.LogInformation($"{System.Reflection.MethodBase.GetCurrentMethod().Name}: {JsonSerializer.Serialize(dto)}");

            var username = CommonUtils.GetCurrentUsername(_httpContext);
            // Check trùng bank, stk
            var anySameBank = _investorRepository.AnyBankAccount(dto.BankId, dto.BankAccount);
            if (anySameBank)
            {
                _investorRepository.ThrowException(ErrorCode.InvestorBankIsRegistered);
            }

            var investor = _investorRepository.FindById(dto.InvestorId);

            _investorRepository.AddBankAccountWithDefault(new InvestorBankAccount
            {
                BankId = dto.BankId,
                InvestorId = dto.InvestorId,
                InvestorGroupId = investor?.InvestorGroupId ?? 0,
                IsDefault = dto.IsDefault,
                CreatedBy = username,
                OwnerAccount = dto.OwnerAccount,
                BankAccount = dto.BankAccount,
            });

            _dbContext.SaveChanges();
        }

        /// <summary>
        /// Khách hàng đăng ký liên kết ngân hàng từ app ở luồng đăng ký
        /// </summary>
        /// <param name="input"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void AddBankAccount(BankAccountDto input)
        {
            _logger.LogInformation($"{System.Reflection.MethodBase.GetCurrentMethod().Name}: {JsonSerializer.Serialize(input)}");
            var investor = _investorRepository.GetByEmailOrPhone(input.Phone);
            if (investor == null)
            {
                _investorRepository.ThrowException(ErrorCode.UserNotFound);
            }

            var defaultIden = _investorRepository.GetDefaultIdentification(investor.InvestorId);
            var anySameBank = _investorRepository.AnySameBank(input.BankId, input.BankAccount);

            using (var tran = _dbContext.Database.BeginTransaction())
            {
                // nếu không trùng thì thêm mới
                if (!anySameBank)
                {
                    _investorRepository.AddBankAccountWithDefault(new InvestorBankAccount
                    {
                        InvestorId = investor.InvestorId,
                        InvestorGroupId = investor.InvestorGroupId ?? 0,
                        BankAccount = input.BankAccount,
                        BankId = input.BankId,
                        OwnerAccount = input.OwnerAccount,
                        IsDefault = YesNo.YES,
                    });
                }
                else
                {
                    _investorRepository.ThrowException(ErrorCode.InvestorBankIsRegistered);
                }

                // kích hoạt tài khoản đăng nhập
                var user = _userRepository.FindByInvestorId(investor.InvestorId);
                user.Status = UserStatus.ACTIVE;

                // kích hoạt investor cho đầu tư hay không
                investor.Status = defaultIden?.EkycInfoIsConfirmed == YesNo.YES ? InvestorStatus.ACTIVE : InvestorStatus.DEACTIVE;
                investor.Step = InvestorAppStep.DA_ADD_BANK;
                investor.FinalStepDate = DateTime.Now;

                // update giấy tờ về trạng thái active
                var listIden = _investorRepository.GetListIdentification(investor.InvestorId);
                foreach (var iden in listIden)
                {
                    iden.Status = InvestorStatus.ACTIVE;
                }
                _dbContext.SaveChanges();

                // chuyển thành đã xác nhận giấy tờ
                if (defaultIden.EkycInfoIsConfirmed == YesNo.YES)
                {
                    foreach (var iden in listIden)
                    {
                        iden.IsVerifiedIdentification = YesNo.YES;
                    }
                }
                else
                {
                    // EKYC KO ĐƯỢC CONFIRM THÌ TẠO BẢN ghi temp
                    _investorRepository.FinalStep(investor.InvestorId);
                }

                _dbContext.SaveChanges();

                _investorRegisterLogServices.Add(new CreateInvestorRegisterLogDto
                {
                    Type = InvestorRegisterLogTypes.SuccessfulBank,
                    Phone = input.Phone,
                });

                _investorRegisterLogServices.Add(new CreateInvestorRegisterLogDto
                {
                    Type = InvestorRegisterLogTypes.CompleteRegistration,
                    Phone = input.Phone,
                });
                _dbContext.SaveChanges();

                tran.Commit();
            }

            _backgroundJobs.Enqueue(() => _backgroundJobIdentityService.AppRegisterInvestorSuccess(investor));
        }

        public void AddContactAddress(CreateContactAddressDto dto)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Thêm giấy tờ
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        /// <exception cref="FaultException"></exception>
        public async Task<EKYCOcrResultDto> AddIdentification(AddIdentificationDto input)
        {
            int investorId = CommonUtils.GetCurrentInvestorId(_httpContext);

            EKYCOcrResultDto resultOcr = new();

            string frontImageUrl = "";
            string backImageUrl = "";

            bool isSuccess = false;

            if (input.Type == CardTypesInput.CCCD || input.Type == CardTypesInput.CMND)
            {
                //front id
                OCRFrontIdDataNewType frontData = await _ocr.ReadFrontIdDataNewType(input.FrontImage);

                frontImageUrl = _imageServices.UploadFileID(new ImageAPI.Models.UploadFileModel
                {
                    File = input.FrontImage,
                    Folder = FileFolder.INVESTOR,
                });

                //back id
                OCRBackIdDataNewType backData = await _ocr.ReadBackIdDataNewType(input.BackImage);
                _ocr.CheckDifferenceImage(frontData, backData);

                resultOcr = new EKYCOcrResultDto
                {
                    IdNo = RecognitionUtils.GetValueStandard(frontData.Id),
                    Name = RecognitionUtils.GetValueStandard(frontData.Name),
                    Sex = OCRGenders.ConvertStandard(RecognitionUtils.GetValueStandard(frontData.Sex)),
                    DateOfBirth = DateTimeUtils.FromDateStrDD_MM_YYYY_ToDate(frontData.Dob),
                    IdIssueDate = DateTimeUtils.FromDateStrDD_MM_YYYY_ToDate(backData.IssueDate),
                    IdIssueExpDate = DateTimeUtils.FromDateStrDD_MM_YYYY_ToDate(frontData.Doe),
                    IdIssuer = RecognitionUtils.GetValueStandard(backData.IssueLoc),
                    PlaceOfOrigin = RecognitionUtils.GetValueStandard(frontData.Home),
                    PlaceOfResidence = RecognitionUtils.GetValueStandard(frontData.Address),
                    Nationality = RecognitionUtils.GetValueStandard(frontData.Nationality)
                };

                backImageUrl = _imageServices.UploadFileID(new ImageAPI.Models.UploadFileModel
                {
                    File = input.BackImage,
                    Folder = FileFolder.INVESTOR,
                });

                isSuccess = true;
            }
            else if (input.Type == CardTypesInput.PASSPORT)
            {
                OCRDataPassport passportData = await _ocr.ReadPassport(input.FrontImage);

                resultOcr = new EKYCOcrResultDto
                {
                    IdNo = RecognitionUtils.GetValueStandard(passportData.PassportNumber),
                    Name = RecognitionUtils.GetValueStandard(passportData.Name),
                    Sex = OCRGenders.ConvertStandard(passportData.Sex),
                    DateOfBirth = DateTimeUtils.FromDateStrDD_MM_YYYY_ToDate(passportData.Dob),
                    PlaceOfOrigin = RecognitionUtils.GetValueStandard(passportData.Pob),
                    PassportIdNumber = RecognitionUtils.GetValueStandard(passportData.PassportNumber),
                    IdIssueDate = DateTimeUtils.FromDateStrDD_MM_YYYY_ToDate(passportData.Doi),
                    IdIssueExpDate = DateTimeUtils.FromDateStrDD_MM_YYYY_ToDate(passportData.Doe),
                    Nationality = "",
                    IdIssuer = passportData.IdIssuer,
                };

                _ocr.CheckAge(resultOcr.DateOfBirth);
                _ocr.CheckExp(resultOcr.IdIssueExpDate);

                frontImageUrl = _imageServices.UploadFileID(new ImageAPI.Models.UploadFileModel
                {
                    File = input.FrontImage,
                    Folder = FileFolder.INVESTOR,
                });

                isSuccess = true;
            }

            if (isSuccess)
            {
                var investor = _investorRepository.GetInvestorById(investorId, false);

                if (investor == null)
                {
                    throw new FaultException(new FaultReason($"Nhà đầu tư không tồn tại: {investor.InvestorId}"), new FaultCode(((int)ErrorCode.InvestorNotFound).ToString()), "");
                }

                string username = CommonUtils.GetCurrentUsername(_httpContext);

                var dto = new CreateIdentificationTemporaryDto
                {
                    IdFrontImageUrl = frontImageUrl,
                    Fullname = resultOcr.Name,
                    IdBackImageUrl = backImageUrl,
                    IdNo = resultOcr.IdNo,
                    IdType = input.Type?.ToUpper(),
                    Sex = resultOcr.Sex,
                    DateOfBirth = resultOcr.DateOfBirth,
                    PlaceOfOrigin = resultOcr.PlaceOfOrigin,
                    PlaceOfResidence = resultOcr.PlaceOfResidence,
                    IdIssuer = resultOcr.IdIssuer,
                    IdExpiredDate = resultOcr.IdIssueExpDate,
                    IdDate = resultOcr.IdIssueDate,
                    Nationality = resultOcr.Nationality,
                    InvestorId = investorId,
                    InvestorGroupId = investor.InvestorGroupId,
                };

                // Xem có giấy tờ trùng số không
                var anySameIdno = _investorRepository.AnyAppIdentitficationDuplicate(resultOcr.IdNo, investor.InvestorGroupId);
                if (anySameIdno)
                {
                    _investorRepository.ThrowException(ErrorCode.InvestorIdNoExisted);
                }

                // Lấy giấy tờ mặc định của investor (nếu có)
                var oldIdentification = _investorRepository.FindTempIdentificationByIdNo(investorId, resultOcr.IdNo);

                var anyDefaultIdentification = _investorRepository.AnyDefaultIdentification(investorId);
                var isDefault = YesNo.NO;

                if (!anyDefaultIdentification)
                {
                    isDefault = YesNo.YES;
                }

                // cập nhật vào giấy tờ cũ
                if (oldIdentification != null)
                {
                    oldIdentification.Fullname = resultOcr.Name;
                    oldIdentification.IdFrontImageUrl = frontImageUrl;
                    oldIdentification.IdBackImageUrl = backImageUrl;
                    oldIdentification.IdType = input.Type?.ToUpper();
                    oldIdentification.Sex = resultOcr.Sex;
                    oldIdentification.DateOfBirth = resultOcr.DateOfBirth;
                    oldIdentification.PlaceOfOrigin = resultOcr.PlaceOfOrigin;
                    oldIdentification.PlaceOfResidence = resultOcr.PlaceOfResidence;
                    oldIdentification.IdIssuer = resultOcr.IdIssuer;
                    oldIdentification.IsDefault = isDefault;
                    oldIdentification.IdExpiredDate = resultOcr.IdIssueExpDate;
                    oldIdentification.Nationality = resultOcr.Nationality;

                    resultOcr.IdentificationId = oldIdentification.Id;
                }
                else
                {
                    // tạo giấy tờ mới
                    var newiden = new InvestorIdentification
                    {
                        IdFrontImageUrl = frontImageUrl,
                        Fullname = resultOcr.Name,
                        IdBackImageUrl = backImageUrl,
                        IdNo = resultOcr.IdNo,
                        IdType = input.Type?.ToUpper(),
                        Sex = resultOcr.Sex,
                        DateOfBirth = resultOcr.DateOfBirth,
                        PlaceOfOrigin = resultOcr.PlaceOfOrigin,
                        PlaceOfResidence = resultOcr.PlaceOfResidence,
                        IdIssuer = resultOcr.IdIssuer,
                        IdExpiredDate = resultOcr.IdIssueExpDate,
                        IdDate = resultOcr.IdIssueDate,
                        Nationality = resultOcr.Nationality,
                        InvestorId = investorId,
                        IsDefault = isDefault,
                        InvestorGroupId = investor.InvestorGroupId,
                    };

                    _investorRepository.AddIdentification(newiden);

                    resultOcr.IdentificationId = newiden.Id;
                }
                _dbContext.SaveChanges();
            }

            return resultOcr;
        }

        /// <summary>
        /// Lấy list trading theo đại lý
        /// </summary>
        /// <returns></returns>
        public List<ViewTradingInfoDto> GetListTrading()
        {
            var investorId = CommonUtils.GetCurrentInvestorId(_httpContext);

            var listTrading = _investorRepository.FindTradingByInvestorId(investorId);
            return listTrading;
        }

        public void AddTransactionAddess()
        {
            throw new NotImplementedException();
        }

        public void ChangePassword(ChangePasswordDto input)
        {
            throw new NotImplementedException();
        }

        public void ChangePasswordTemp(ChangePasswordTempDto input)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Thay đổi pin
        /// </summary>
        /// <param name="input"></param>
        /// <exception cref="FaultException"></exception>   
        public void ChangePin(ChangePinDto input)
        {
            var investorId = CommonUtils.GetCurrentInvestorId(_httpContext);
            var user = _userRepository.FindByInvestorId(investorId);

            if (input.OldPin != user.PinCode && !string.IsNullOrEmpty(user.PinCode))
            {
                throw new FaultException(new FaultReason(_investorRepository.GetDefError((int)ErrorCode.InvestorInvalidOldPin)), new FaultCode(((int)ErrorCode.InvestorInvalidOldPin).ToString()), "");
            }

            user.PinCode = CommonUtils.CreateMD5(input.NewPin);
            user.ModifiedDate = DateTime.Now;
            user.ModifiedBy = user.UserName;
            user.IsTempPin = YesNo.NO;

            _dbContext.SaveChanges();
        }

        public bool CheckEmailExist(CheckEmailExistDto model)
        {
            throw new NotImplementedException();
        }

        public void CheckOtp(string otp)
        {
            throw new NotImplementedException();
        }

        public bool CheckPhoneExist(CheckPhoneExistDto model)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Confirm ekyc với giấy tờ thêm sau khi đăng ký
        /// </summary>
        /// <param name="dto"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void ConfirmIdentificationEkyc(ConfirmIdentificationEkycDto dto)
        {
            _logger.LogInformation($"{System.Reflection.MethodBase.GetCurrentMethod().Name}: {JsonSerializer.Serialize(dto)}");

            var investorId = CommonUtils.GetCurrentInvestorId(_httpContext);

            var identification = _investorRepository.GetIdentificationById(dto.IdentificationId)
                                    .ThrowIfNull(_dbContext, ErrorCode.InvestorIdentificationNotFound);
            var listGender = new string[] { Genders.MALE, Genders.FEMALE };
            string sex = null;
            // Nếu ko phải cmnd và trường Sex có giá trị thì báo lỗi
            if (identification.IdType.ToLower() != IDTypes.CMND.ToLower() && listGender.Contains(dto.Sex))
            {
                _investorRepository.ThrowException(ErrorCode.InvestorEkycUpdateNotCmnd);
            }

            using (var tran = _dbContext.Database.BeginTransaction())
            {
                // Nếu là cmnd thì cập nhật Sex
                if (identification.IdType.ToLower() == IDTypes.CMND.ToLower())
                {
                    sex = dto.Sex;
                }

                // Check xem những trường bắt buộc khi ekyc đã có giá trị chưa
                dto.IsConfirmed = verifyRequiredFieldEkyc(identification, dto.IncorrectFields);
                string incorrectFieldString = string.Join(StringUtils.SEPARATOR, dto.IncorrectFields ?? new List<string>());

                if (dto.IsConfirmed)
                {
                    identification.EkycInfoIsConfirmed = YesNo.YES;
                    identification.EkycIncorrectFields = incorrectFieldString;
                    identification.Status = InvestorIdentificationStatus.ACTIVE;
                    identification.Sex = sex;

                    _dbContext.SaveChanges();
                }
                else
                {
                    var investor = _investorRepository.FindById(investorId);
                    var investorTemp = _investorRepository.GetInvestorTempByPhone(investor.Phone);

                    // UPDATE IDENTIFICATION
                    var currIden = _investorRepository.GetIdentificationById(dto.IdentificationId);
                    if (currIden != null)
                    {
                        currIden.EkycInfoIsConfirmed = dto.IsConfirmed ? YesNo.YES : YesNo.NO;
                        currIden.EkycIncorrectFields = incorrectFieldString;
                        currIden.Status = InvestorIdentificationStatus.ACTIVE;
                        currIden.Sex = sex;
                    }

                    // NẾU KHÔNG CÓ INVESTOR TEMP NÀO => TẠO CẢ INVESTOR TEMP, GIẤY TỜ TEMP, BANK TEMP, ADDRESS TEMP
                    if (investorTemp == null)
                    {
                        string username = CommonUtils.GetCurrentUsername(_httpContext);
                        // LAY DU LIEU O BANG THAT KET HOP VOI DU LIEU CAP NHAT => INSERT VAO BANG TEMP
                        var newInvestorTemp = _investorRepository.CreateInvestorTemp(new InvestorTemp
                        {
                            Phone = investor.Phone,
                            Fax = investor.Fax,
                            Email = investor.Email,
                            Mobile = investor.Mobile,
                            TaxCode = investor.TaxCode,
                            ProfDueDate = investor.ProfDueDate,
                            ProfFileUrl = investor.ProfFileUrl,
                            IsProf = investor.IsProf,
                            Status = investor.Status,
                            InvestorGroupId = investor.InvestorGroupId,
                            TradingProviderId = investor.TradingProviderId,
                            ReferralCodeSelf = investor.ReferralCodeSelf,
                            CreatedBy = username,
                        });
                        _dbContext.SaveChanges();

                        // COPY DU LIEU TU IDENTIFICATION THAT SANG IDENTIFICATION TEMP
                        _investorRepository.MoveIdentificationActualToTemp(_mapper, newInvestorTemp.InvestorId, newInvestorTemp.InvestorGroupId ?? 0);

                        // COPY BANK TU THAT SANG TEMP
                        _investorRepository.MoveBankActualToTemp(_mapper, newInvestorTemp.InvestorId, newInvestorTemp.InvestorGroupId ?? 0);

                        // COPY CONTACT ADDRESS THAT => TEMP
                        _investorRepository.MoveContactAddressActualToTemp(_mapper, newInvestorTemp.InvestorId, newInvestorTemp.InvestorGroupId ?? 0);

                        _dbContext.SaveChanges();
                    }
                    // NẾU CÓ RỒI => INSERT GIẤY TỜ NÀY TỪ THẬT SANG TEMP
                    else
                    {
                        _investorRepository.AddIdentificationTemp(new InvestorIdTemp
                        {
                            InvestorId = investorTemp.InvestorId,
                            IdType = identification.IdType,
                            IdNo = identification.IdNo,
                            Fullname = identification.Fullname,
                            DateOfBirth = identification.DateOfBirth,
                            Nationality = identification.Nationality,
                            PersonalIdentification = identification.PersonalIdentification,
                            IdIssuer = identification.IdIssuer,
                            IdDate = identification.IdDate,
                            IdExpiredDate = identification.IdExpiredDate,
                            PlaceOfOrigin = identification.PlaceOfOrigin,
                            PlaceOfResidence = identification.PlaceOfResidence,
                            IdFrontImageUrl = identification.IdFrontImageUrl,
                            IdBackImageUrl = identification.IdBackImageUrl,
                            IsVerifiedIdentification = YesNo.NO,
                            EkycInfoIsConfirmed = YesNo.NO,
                            EkycIncorrectFields = incorrectFieldString,
                            FaceVideoUrl = identification.FaceVideoUrl,
                            InvestorGroupId = identification.InvestorGroupId,
                            Status = identification.Status,
                            Sex = sex,
                            IsDefault = identification.IsDefault,
                            Deleted = identification.Deleted,
                        });

                        _dbContext.SaveChanges();
                    }
                }

                tran.Commit();
            }

        }

        /// <summary>
        /// Gửi email luồng đăng ký
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task CreateVerificationCodeSendMailAsync(InvestorEmailPhoneDto input)
        {
            _logger.LogInformation($"{System.Reflection.MethodBase.GetCurrentMethod().Name}: {JsonSerializer.Serialize(input)}");
            var otp = new OtpDto();
            using (var tran = _dbContext.Database.BeginTransaction())
            {
                otp = CreateVerifyCode(input.Phone, input.Email);

                _investorRegisterLogServices.Add(new CreateInvestorRegisterLogDto
                {
                    Phone = input.Phone,
                    Type = InvestorRegisterLogTypes.OtpSent
                });
                _dbContext.SaveChanges();
                tran.Commit();

            }

            if (!string.IsNullOrEmpty(otp.Otp))
            {
                await _sendEmailService.SendEmailOtpAnonymous(otp.Otp, otp.Exp, input.Email);
            }
        }

        /// <summary>
        /// Gửi otp sms luồng đký
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task CreateVerificationCodeSendSmsAsync(InvestorEmailPhoneDto input)
        {
            _logger.LogInformation($"{System.Reflection.MethodBase.GetCurrentMethod().Name}: {JsonSerializer.Serialize(input)}");
            var otp = new OtpDto();
            using (var tran = _dbContext.Database.BeginTransaction())
            {
                otp = CreateVerifyCode(input.Phone, input.Email);

                _investorRegisterLogServices.Add(new CreateInvestorRegisterLogDto
                {
                    Phone = input.Phone,
                    Type = InvestorRegisterLogTypes.OtpSent
                });
                _dbContext.SaveChanges();
                tran.Commit();
            }

            if (!string.IsNullOrEmpty(otp.Otp))
            {
                await _sendEmailService.SendSmsOtpAnonymous(otp.Otp, otp.Exp, input.Phone);
            }
        }

        public void DeactiveMyUserAccount(AppDeactiveMyUserAccountDto dto)
        {
            throw new NotImplementedException();
        }

        public void DeleteBankAccount(AppDeleteBankDto dto)
        {
            throw new NotImplementedException();
        }

        public int DeteleInvestor(int id)
        {
            throw new NotImplementedException();
        }

        public int DeteleManagerInvestor(int id)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Xác nhận ekyc
        /// </summary>
        /// <param name="input"></param>
        /// <exception cref="FaultException"></exception>
        public void EkycConfirmInfo(EKYCConfirmInfoDto input)
        {
            _logger.LogInformation($"{System.Reflection.MethodBase.GetCurrentMethod().Name}: {JsonSerializer.Serialize(input)}");

            var investor = _investorRepository.GetByEmailOrPhone(input.Phone);
            if (investor == null)
            {
                throw new FaultException(new FaultReason(_investorRepository.GetDefError((int)ErrorCode.UserNotFound)), new FaultCode(((int)ErrorCode.UserNotFound).ToString()), "");
            }

            var defaultIden = _investorRepository.GetDefaultIdentification(investor.InvestorId);
            var listGender = new string[] { Genders.MALE, Genders.FEMALE };

            // Nếu ko phải cmnd và trường Sex có giá trị thì báo lỗi
            if (defaultIden.IdType.ToLower() != IDTypes.CMND.ToLower() && listGender.Contains(input.Sex))
            {
                _investorRepository.ThrowException(ErrorCode.InvestorEkycUpdateNotCmnd);
            }

            // Nếu là cmnd thì lấy trường Sex truyền lên để update
            if (defaultIden.IdType.ToLower() == IDTypes.CMND.ToLower())
            {
                defaultIden.Sex = input.Sex;
            }

            // Check xem những trường bắt buộc khi ekyc đã có giá trị chưa
            input.IsConfirmed = verifyRequiredFieldEkyc(defaultIden, input.IncorrectFields);
            string incorrectFieldString = string.Join(StringUtils.SEPARATOR, input.IncorrectFields ?? new List<string>());

            using (var tran = _dbContext.Database.BeginTransaction())
            {
                // UPDATE
                defaultIden.EkycInfoIsConfirmed = input.IsConfirmed ? YesNo.YES : YesNo.NO;
                defaultIden.EkycIncorrectFields = incorrectFieldString;
                defaultIden.Status = input.IsConfirmed ? InvestorStatus.ACTIVE : InvestorStatus.TEMP;

                _investorRegisterLogServices.Add(new CreateInvestorRegisterLogDto
                {
                    Phone = input.Phone,
                    Type = InvestorRegisterLogTypes.SuccessfulIdentification
                });
                _dbContext.SaveChanges();
                tran.Commit();
            }

        }

        /// <summary>
        /// So khớp mặt khi đăng ký
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<EKYCFaceMatchResultDto> EkycFaceMatchAsync(EKYCFaceMatchDto input)
        {
            _logger.LogInformation($"Nhận diện khi đăng ký");
            _logger.LogInformation($"{System.Reflection.MethodBase.GetCurrentMethod().Name}: {JsonSerializer.Serialize(input)}");

            FaceMatchData faceMatchData = await _ocr.FaceRecognition(input.IdCardImage, input.FaceImage);

            if (_ocr.CheckFaceSimilarity(faceMatchData.Similarity))
            {
                string faceImageUrl = _imageServices.UploadFileID(new ImageAPI.Models.UploadFileModel
                {
                    File = input.FaceImage,
                    Folder = FileFolder.INVESTOR,
                });

                var investor = _investorRepository.GetByEmailOrPhone(input.Phone);
                if (investor == null)
                {
                    _investorRepository.ThrowException(ErrorCode.UserNotFound);
                }

                var defaultIden = _investorRepository.GetDefaultIdentification(investor.InvestorId);
                if (defaultIden == null)
                {
                    _investorRepository.ThrowException(ErrorCode.InvestorEKYCNotFound);
                }

                using (var tran = _dbContext.Database.BeginTransaction())
                {
                    _investorRepository.UpdateEkycStepDate(InvestorAppStep.DA_EKYC, investor.InvestorId);
                    // cập nhật ảnh nhận diện
                    investor.EkycStatus = defaultIden.EkycInfoIsConfirmed == YesNo.YES ? EKYCStatus.APPROVED_IDENTIFIER : EKYCStatus.IDENTIFIED;
                    investor.FaceImageUrl = faceImageUrl;
                    investor.Step = InvestorAppStep.DA_EKYC;
                    investor.FaceImageSimilarity = faceMatchData?.Similarity;

                    // cập nhật trường đã nhận diện mặt
                    defaultIden.IsVerifiedFace = YesNo.YES;

                    _investorRegisterLogServices.Add(new CreateInvestorRegisterLogDto
                    {
                        Type = InvestorRegisterLogTypes.SuccessfulEkyc,
                        Phone = input.Phone
                    });

                    _dbContext.SaveChanges();
                    tran.Commit();
                }
            }
            else
            {
                _investorRepository.ThrowException(ErrorCode.InvestorFaceRecognitionNotMatch);
            }

            var result = new EKYCFaceMatchResultDto
            {
                IsMatch = faceMatchData.IsMatch,
                Similarity = faceMatchData.Similarity,
                IsBothImgIDCard = faceMatchData.IsBothImgIDCard
            };
            _logger.LogInformation($"{System.Reflection.MethodBase.GetCurrentMethod().Name} RESULT: {JsonSerializer.Serialize(result)}");

            return result;
        }

        /// <summary>
        /// Khách hàng quét mặt sau khi đăng ký
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<EKYCFaceMatchResultDto> EkycFaceMatchLoggedInAsync(FaceRecognitionLoggedInDto input)
        {
            var investorId = CommonUtils.GetCurrentInvestorId(_httpContext);

            var defaultIdentification = _investorRepository.GetDefaultIdentification(investorId);
            if (defaultIdentification == null)
            {
                return null;
            }

            var frontImage = _imageServices.GenFileFromPath(defaultIdentification.IdFrontImageUrl);
            FaceMatchData faceMatchData = await _ocr.FaceRecognition(frontImage, input.FaceImage);
            if (_ocr.CheckFaceSimilarity(faceMatchData.Similarity))
            {
                string faceImageUrl = _imageServices.UploadFile(new ImageAPI.Models.UploadFileModel
                {
                    File = input.FaceImage,
                    Folder = FileFolder.INVESTOR,
                });

                var investor = _investorRepository.FindById(investorId);

                string ekycStatus = defaultIdentification.EkycInfoIsConfirmed == YesNo.YES ? EKYCStatus.APPROVED_IDENTIFIER : EKYCStatus.IDENTIFIED;

                using (var tran = _dbContext.Database.BeginTransaction())
                {
                    investor.FaceImageSimilarity = faceMatchData.Similarity;
                    investor.FaceImageUrl = faceImageUrl;
                    investor.EkycStatus = ekycStatus;

                    defaultIdentification.IsVerifiedFace = YesNo.YES;

                    _dbContext.SaveChanges();

                    tran.Commit();
                }
            }
            else
            {
                _investorRepository.ThrowException(ErrorCode.InvestorFaceRecognitionNotMatch);
            }

            return new EKYCFaceMatchResultDto
            {
                IsMatch = faceMatchData.IsMatch,
                Similarity = faceMatchData.Similarity,
                IsBothImgIDCard = faceMatchData.IsBothImgIDCard
            };
        }

        /// <summary>
        /// Khách hàng Ekyc ocr ảnh giấy tờ khi đăng ký
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<EKYCOcrResultDto> EkycOCRAsync(EKYCOcrDto input)
        {
            _logger.LogInformation($"{System.Reflection.MethodBase.GetCurrentMethod().Name}: {JsonSerializer.Serialize(input)}");
            int investorId = _investorRepository.GetByEmailOrPhone(input.Phone)?.InvestorId ?? 0;
            EKYCOcrResultDto resultOcr = new();

            string frontImageUrl = "";
            string backImageUrl = "";

            if (input.Type == CardTypesInput.CCCD || input.Type == CardTypesInput.CMND)
            {
                _ocr.CheckBackImage(input.BackImage);

                //front id
                OCRFrontIdDataNewType frontData = await _ocr.ReadFrontIdDataNewType(input.FrontImage);

                string idType = null;
                string typeNew = frontData.TypeNew.ToLower();
                if (typeNew.Contains("cmnd"))
                {
                    idType = IDTypes.CMND;
                }
                else if (typeNew.Contains("cccd"))
                {
                    idType = IDTypes.CCCD;
                }

                _ocr.CheckIdType(input.Type.ToLower(), idType.ToLower());
                _ocr.CheckUploadTypeAndIdType(input.Type, frontData);

                //back id
                OCRBackIdDataNewType backData = await _ocr.ReadBackIdDataNewType(input.BackImage);

                _ocr.CheckDifferenceImage(frontData, backData);
                var issueDate = _ocr.ConvertStringIssudeDateToDateTime(backData.IssueDate);

                resultOcr = new EKYCOcrResultDto
                {
                    IdNo = RecognitionUtils.GetValueStandard(frontData.Id),
                    Name = RecognitionUtils.GetValueStandard(frontData.Name),
                    Sex = OCRGenders.ConvertStandard(RecognitionUtils.GetValueStandard(frontData.Sex)),
                    DateOfBirth = DateTimeUtils.FromDateStrDD_MM_YYYY_ToDate(frontData.Dob),
                    IdIssueDate = issueDate,
                    IdIssueExpDate = _ocr.ProccessExpDate(frontData.Doe, issueDate, input.Type.ToUpper()),
                    IdIssuer = RecognitionUtils.GetValueStandard(backData.IssueLoc),
                    PlaceOfOrigin = RecognitionUtils.GetValueStandard(frontData.Home),
                    PlaceOfResidence = RecognitionUtils.GetValueStandard(frontData.Address),
                    Nationality = OCRNationality.ConvertStandard(frontData.Nationality)
                };

                _ocr.CheckAge(resultOcr.DateOfBirth);
                _ocr.CheckExp(resultOcr.IdIssueExpDate);

                frontImageUrl = _imageServices.UploadFileID(new ImageAPI.Models.UploadFileModel
                {
                    File = input.FrontImage,
                    Folder = FileFolder.INVESTOR,
                });

                backImageUrl = _imageServices.UploadFileID(new ImageAPI.Models.UploadFileModel
                {
                    File = input.BackImage,
                    Folder = FileFolder.INVESTOR,
                });

                UpdateEkycId(investorId, new UpdateEkycIdDto
                {
                    Phone = input.Phone,
                    Name = resultOcr.Name,
                    DateOfBirth = resultOcr.DateOfBirth,
                    Sex = resultOcr.Sex,
                    IdNo = resultOcr.IdNo,
                    IssueDate = resultOcr.IdIssueDate,
                    IssueExpDate = resultOcr.IdIssueExpDate,
                    Issuer = resultOcr.IdIssuer,
                    PlaceOfOrigin = resultOcr.PlaceOfOrigin,
                    PlaceOfResidence = resultOcr.PlaceOfResidence,
                    Nationality = resultOcr.Nationality,
                    IdType = idType,
                    FrontImage = frontImageUrl,
                    BackImage = backImageUrl
                });

                _dbContext.SaveChanges();
            }
            else if (input.Type == CardTypesInput.PASSPORT)
            {
                OCRDataPassport passportData = await _ocr.ReadPassport(input.FrontImage);

                _ocr.CheckIdType(input.Type.ToLower(), IDTypes.PASSPORT.ToLower());

                resultOcr = new EKYCOcrResultDto
                {
                    IdNo = RecognitionUtils.GetValueStandard(passportData.PassportNumber),
                    Name = RecognitionUtils.GetValueStandard(passportData.Name),
                    Sex = OCRGenders.ConvertStandard(passportData.Sex),
                    DateOfBirth = DateTimeUtils.FromDateStrDD_MM_YYYY_ToDate(passportData.Dob),
                    PlaceOfOrigin = RecognitionUtils.GetValueStandard(passportData.Pob),
                    PassportIdNumber = RecognitionUtils.GetValueStandard(passportData.PassportNumber),
                    IdIssueDate = DateTimeUtils.FromDateStrDD_MM_YYYY_ToDate(passportData.Doi),
                    IdIssueExpDate = DateTimeUtils.FromDateStrDD_MM_YYYY_ToDate(passportData.Doe),
                    Nationality = OCRNationality.ConvertStandard(passportData.Nationality),
                    IdIssuer = passportData.IdIssuer,
                };

                _ocr.CheckAge(resultOcr.DateOfBirth);
                _ocr.CheckExp(resultOcr.IdIssueExpDate);

                frontImageUrl = _imageServices.UploadFileID(new ImageAPI.Models.UploadFileModel
                {
                    File = input.FrontImage,
                    Folder = FileFolder.INVESTOR,
                });

                UpdateEkycId(investorId, new UpdateEkycIdDto
                {
                    Phone = input.Phone,
                    Name = resultOcr.Name,
                    DateOfBirth = resultOcr.DateOfBirth,
                    Sex = resultOcr.Sex,
                    IdNo = resultOcr.IdNo,
                    IssueDate = resultOcr.IdIssueDate,
                    IssueExpDate = resultOcr.IdIssueExpDate,
                    Issuer = resultOcr.IdIssuer,
                    PlaceOfOrigin = resultOcr.PlaceOfOrigin,
                    PlaceOfResidence = resultOcr.PlaceOfResidence,
                    Nationality = resultOcr.Nationality,
                    IdType = IDTypes.PASSPORT,
                    FrontImage = frontImageUrl,
                    BackImage = backImageUrl
                });
                _dbContext.SaveChanges();
            }
            return resultOcr;
        }

        /// <summary>
        /// OCR ảnh giấy tờ cá nhân
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<EKYCOcrResultDto> CheckInfoOCRAsync(AddIdentificationDto input)
        {
            _logger.LogInformation($"{nameof(CheckInfoOCRAsync)}: {JsonSerializer.Serialize(input)}");

            EKYCOcrResultDto resultOcr = new();
            if (input.Type == CardTypesInput.CCCD || input.Type == CardTypesInput.CMND)
            {
                _ocr.CheckBackImage(input.BackImage);

                //front id
                OCRFrontIdDataNewType frontData = await _ocr.ReadFrontIdDataNewType(input.FrontImage);

                string idType = null;
                string typeNew = frontData.TypeNew.ToLower();
                if (typeNew.Contains("cmnd"))
                {
                    idType = IDTypes.CMND;
                }
                else if (typeNew.Contains("cccd"))
                {
                    idType = IDTypes.CCCD;
                }

                _ocr.CheckIdType(input.Type.ToLower(), idType.ToLower());
                _ocr.CheckUploadTypeAndIdType(input.Type, frontData);

                //back id
                OCRBackIdDataNewType backData = await _ocr.ReadBackIdDataNewType(input.BackImage);

                _ocr.CheckDifferenceImage(frontData, backData);
                var issueDate = _ocr.ConvertStringIssudeDateToDateTime(backData.IssueDate);

                resultOcr = new EKYCOcrResultDto
                {
                    IdNo = RecognitionUtils.GetValueStandard(frontData.Id),
                    Name = RecognitionUtils.GetValueStandard(frontData.Name),
                    Sex = OCRGenders.ConvertStandard(RecognitionUtils.GetValueStandard(frontData.Sex)),
                    DateOfBirth = DateTimeUtils.FromDateStrDD_MM_YYYY_ToDate(frontData.Dob),
                    IdIssueDate = issueDate,
                    IdIssueExpDate = _ocr.ProccessExpDate(frontData.Doe, issueDate, input.Type.ToUpper()),
                    IdIssuer = RecognitionUtils.GetValueStandard(backData.IssueLoc),
                    PlaceOfOrigin = RecognitionUtils.GetValueStandard(frontData.Home),
                    PlaceOfResidence = RecognitionUtils.GetValueStandard(frontData.Address),
                    Nationality = OCRNationality.ConvertStandard(frontData.Nationality)
                };
                _ocr.CheckExp(resultOcr.IdIssueExpDate);
            }
            else if (input.Type == CardTypesInput.PASSPORT)
            {
                OCRDataPassport passportData = await _ocr.ReadPassport(input.FrontImage);

                _ocr.CheckIdType(input.Type.ToLower(), IDTypes.PASSPORT.ToLower());

                resultOcr = new EKYCOcrResultDto
                {
                    IdNo = RecognitionUtils.GetValueStandard(passportData.PassportNumber),
                    Name = RecognitionUtils.GetValueStandard(passportData.Name),
                    Sex = OCRGenders.ConvertStandard(passportData.Sex),
                    DateOfBirth = DateTimeUtils.FromDateStrDD_MM_YYYY_ToDate(passportData.Dob),
                    PlaceOfOrigin = RecognitionUtils.GetValueStandard(passportData.Pob),
                    PassportIdNumber = RecognitionUtils.GetValueStandard(passportData.PassportNumber),
                    IdIssueDate = DateTimeUtils.FromDateStrDD_MM_YYYY_ToDate(passportData.Doi),
                    IdIssueExpDate = DateTimeUtils.FromDateStrDD_MM_YYYY_ToDate(passportData.Doe),
                    Nationality = OCRNationality.ConvertStandard(passportData.Nationality),
                    IdIssuer = passportData.IdIssuer,
                };
                _ocr.CheckExp(resultOcr.IdIssueExpDate);
            }
            return resultOcr;
        }

        public List<InvestorDto> Find()
        {
            throw new NotImplementedException();
        }

        public PagingResult<InvestorDto> FindAll(int pageSize, int pageNumber, string keyword)
        {
            throw new NotImplementedException();
        }

        public Investor FindById(int id)
        {
            throw new NotImplementedException();
        }

        public async Task ForgotPasswordAsync(ForgotPasswordDto input)
        {
            var investor = _investorRepository.GetByEmailOrPhone(input.EmailOrPhone);
            if (investor == null)
            {
                throw new FaultException(new FaultReason(_investorRepository.GetDefError((int)ErrorCode.UserNotFound)), new FaultCode(((int)ErrorCode.UserNotFound).ToString()), "");
            }

            if (investor.Status == InvestorStatus.TEMP)
            {
                throw new FaultException(new FaultReason(_investorRepository.GetDefError((int)ErrorCode.InvestorEmalOrPhoneNotRegis)), new FaultCode(((int)ErrorCode.InvestorEmalOrPhoneNotRegis).ToString()), "");
            }

            // Lấy khoảng thời gian hết hạn otp và Độ dài chuỗi otp
            var otpExpRange = _sysVarRepository.GetVarByName("EXPIRE", "OTP_EXP")?.VarValue;
            var otpLen = _sysVarRepository.GetVarByName("AUTH", "OTP_LENGTH")?.VarValue;

            string otpCode = "";

            if (otpLen != null)
            {
                // Sinh mã otp
                otpCode = CommonUtils.RandomNumber(Convert.ToInt32(otpLen));
            }

            // Lấy và check user
            var user = _userRepository.FindByInvestorId(investor.InvestorId);
            if (user?.Status == UserStatus.DEACTIVE)
            {
                throw new FaultException(new FaultReason(_investorRepository.GetDefError((int)ErrorCode.InvestorDeactive)), new FaultCode(((int)ErrorCode.InvestorDeactive).ToString()), "");
            }

            if (user?.Status == UserStatus.LOCKED)
            {
                throw new FaultException(new FaultReason(_investorRepository.GetDefError((int)ErrorCode.UserIsLocked)), new FaultCode(((int)ErrorCode.UserIsLocked).ToString()), "");
            }

            using (var transacton = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                // Cập nhật những otp cũ thành deactive
                _authOtpRepository.UpdateIsActiveByUserId(YesNo.NO, Convert.ToInt32(user.UserId));

                // Tạo otp mới
                var otpExp = DateTime.Now.AddSeconds(Convert.ToInt32(otpExpRange));
                _authOtpRepository.CreateOtp(new AuthOtp
                {
                    Phone = investor.Phone,
                    CreatedTime = DateTime.Now,
                    ExpiredTime = otpExp,
                    OtpCode = otpCode,
                    UserId = user?.UserId ?? 0,
                });

                _dbContext.SaveChanges();
                transacton.Complete();

                // gửi thông báo sms/email
                if (input.SendType == ForgotPasswordType.SEND_MAIL)
                {
                    await _sendEmailService.SendEmailOtpAnonymous(otpCode, otpExp, investor.Email);
                }
                else if (input.SendType == ForgotPasswordType.SEND_SMS)
                {
                    await _sendEmailService.SendSmsOtpAnonymous(otpCode, otpExp, investor.Phone);
                }
            }

        }

        public void GenerateOtpMail()
        {
            var investorId = CommonUtils.GetCurrentInvestorId(_httpContext);
            GenerateOtp(investorId);
        }

        public void GenerateOtpSms()
        {
            int investorId = CommonUtils.GetCurrentInvestorId(_httpContext);
            GenerateOtp(investorId);
        }

        public void GenerateOtpSmsByPhone(string phone)
        {
            GenerateOtpByPhone(phone);
        }

        public Investor GetByEmailOrPhone(string phone)
        {
            throw new NotImplementedException();
        }

        public Investor GetByUsername(string username)
        {
            throw new NotImplementedException();
        }

        public InvestorIdentification GetDefaultIdentification()
        {
            throw new NotImplementedException();
        }

        public List<InvestorBankAccount> GetListBank()
        {
            throw new NotImplementedException();
        }

        public List<InvestorContactAddress> GetListContactAddress(string keyword)
        {
            throw new NotImplementedException();
        }

        public List<InvestorIdentification> GetListIdentification(AppGetListIdentificationDto dto)
        {
            throw new NotImplementedException();
        }

        public List<IdDto> GetListIdInfo()
        {
            throw new NotImplementedException();
        }

        public List<InvestorSale> GetListReferralCodeSale()
        {
            throw new NotImplementedException();
        }

        public List<TransactionAddessDto> GetListTransactionAddress()
        {
            throw new NotImplementedException();
        }

        public InvestorMyInfoDto GetMyInfo()
        {
            throw new NotImplementedException();
        }

        public AppShortSalerInfoDto GetShortSalerInfoByReferralCodeSelf(string referralCode)
        {
            throw new NotImplementedException();
        }

        public bool isReferralCodeExist(string referralCode)
        {
            throw new NotImplementedException();
        }

        public bool IsRegisteredOffline(CheckRegisteredOfflineDto input)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// App đăng ký tài khoản investor
        /// </summary>
        /// <param name="input"></param>
        /// <exception cref="FaultException"></exception>
        public void RegisterInvestor(RegisterInvestorDto input, int source = InvestorSource.ONLINE)
        {
            _logger.LogInformation($"{System.Reflection.MethodBase.GetCurrentMethod().Name}: {JsonSerializer.Serialize(input)}; source: {source}");
            string ipAddress = CommonUtils.GetCurrentRemoteIpAddress(_httpContext);
            bool createNewUser = false;
            bool updateEmail = false;
            bool insertRefCode = false;
            // Kiểm tra investor có tồn tại ko (theo phone) trong bảng tạm
            // Nếu có phone trong bảng tạm rồi thì báo sđt đã được đký
            var investorTemp = _investorRepository.GetInvestorTempByPhone(input.Phone);

            if (investorTemp != null)
            {
                throw new FaultException(new FaultReason(_investorRepository.GetDefError((int)ErrorCode.InvestorPhoneExisted)), new FaultCode(((int)ErrorCode.InvestorPhoneExisted).ToString()), "");
            }

            // Kiểm tra email có trong investor tạm chưa
            // Nếu có email trong bảng tạm rồi thì báo sđt đã được đký
            investorTemp = _investorRepository.GetInvestorTempByEmail(input.Email);

            if (investorTemp != null)
            {
                _investorRepository.ThrowException(ErrorCode.InvestorEmailExisted);
            }

            // Kiểm tra investor có tồn tại ko (theo phone) trong bảng thật
            // Nếu nhiều hơn 1, thì báo sđt đã được đký
            var count = _investorRepository.CountInvestorByPhone(input.Phone);
            if (count > 1)
            {
                _investorRepository.ThrowException(ErrorCode.InvestorPhoneExisted);
            }
            else
            {
                // đã đăng ký quá step 2 (đã nhập đc otp) thì báo trùng
                var investorCheck = _investorRepository.GetByEmailOrPhone(input.Phone, null);

                if (investorCheck != null)
                {
                    bool isDaDangKy = new string[] { InvestorStatus.LOCKED, InvestorStatus.DEACTIVE }.Contains(investorCheck.Status) || investorCheck?.Step >= InvestorAppStep.DA_DANG_KY;
                    if (isDaDangKy)
                    {
                        _investorRepository.ThrowException(ErrorCode.InvestorPhoneExisted);
                    }
                }


                // ok thì tạo invesor + user mới
                if (count == 0)
                {
                    createNewUser = true;
                }
            }


            var investor = _investorRepository.GetByEmailOrPhone(input.Phone);


            var tmpInvestor = _investorRepository.GetByEmailAndPhone(input.Phone, input.Email);
            if (tmpInvestor != null)
            {
                investor = tmpInvestor;
                insertRefCode = true;
            }
            else if (count == 1 && tmpInvestor == null)
            {
                updateEmail = true;
            }

            if (investor?.Status == InvestorStatus.ACTIVE)
            {
                throw new FaultException(new FaultReason(_investorRepository.GetDefError((int)ErrorCode.InvestorPhoneExisted)), new FaultCode(((int)ErrorCode.InvestorPhoneExisted).ToString()), "");
            }

            if (createNewUser)
            {
                // Kiểm tra xem có tài khoản nào dùng email này nữa ko
                var another = _investorRepository.GetByEmail(input.Email);
                if (another != null)
                {
                    throw new FaultException(new FaultReason(_investorRepository.GetDefError((int)ErrorCode.InvestorEmailExisted)), new FaultCode(((int)ErrorCode.InvestorEmailExisted).ToString()), "");
                }

                // Lấy tham số cấu hình
                var refLen = Convert.ToInt32(_sysVarRepository.GetVarByName("AUTH", "REFERRAL_LENGTH")?.VarValue);
                var cifCodeLen = Convert.ToInt32(_sysVarRepository.GetVarByName("AUTH", "CIF_CODE_LENGTH")?.VarValue);

                using (var tran = _dbContext.Database.BeginTransaction())
                {
                    // Tạo investor mới
                    var newInvestor = new Investor
                    {
                        Phone = input.Phone,
                        Email = input.Email,
                        RegisterSource = "ON",
                        Status = InvestorStatus.TEMP,
                        Source = source,
                        IpAddress = ipAddress
                    };
                    _investorRepository.CreateInvestor(newInvestor, refLen);

                    // tạo tài khoản ở trạng thái tạm (T) sau khi xác thực mới mở
                    _userRepository.CreateUser(new Users
                    {
                        UserName = input.Phone,
                        UserType = UserTypes.INVESTOR,
                        Status = UserStatus.TEMP,
                        Email = input.Email,
                        Password = CommonUtils.CreateMD5(input.Password),
                        InvestorId = newInvestor.InvestorId,
                    });

                    // tạo cif code
                    _cifCodeRepository.CreateCifCodeByInvestorId(newInvestor.InvestorId, cifCodeLen);
                    _dbContext.SaveChanges();

                    // insert ma gioi thieu
                    if (!string.IsNullOrEmpty(input.ReferralCode))
                    {
                        RegisReferralCode(newInvestor.InvestorId, input.ReferralCode);
                    }

                    _dbContext.SaveChanges();
                    tran.Commit();
                }
            }
            else if (updateEmail)
            {
                // Kiểm tra xem có tài khoản nào dùng email này nữa ko
                var another = _investorRepository.GetByEmail(input.Email);
                if (another != null)
                {
                    throw new FaultException(new FaultReason(_investorRepository.GetDefError((int)ErrorCode.InvestorEmailExisted)), new FaultCode(((int)ErrorCode.InvestorEmailExisted).ToString()), "");
                }

                using (var tran = _dbContext.Database.BeginTransaction())
                {
                    // Cập nhật email
                    investor.Email = input.Email;
                    investor.Step = InvestorAppStep.BAT_DAU;

                    // Cập nhật mã giới thiệu
                    if (!string.IsNullOrEmpty(input.ReferralCode))
                    {
                        RegisReferralCode(investor.InvestorId, input.ReferralCode);
                    }

                    _dbContext.SaveChanges();
                    tran.Commit();
                }
            }
            else if (insertRefCode)
            {
                using (var tran = _dbContext.Database.BeginTransaction())
                {
                    investor.Step = InvestorAppStep.BAT_DAU;

                    // insert ma gioi thieu
                    if (!string.IsNullOrEmpty(input.ReferralCode))
                    {
                        RegisReferralCode(investor.InvestorId, input.ReferralCode);
                    }

                    _dbContext.SaveChanges();
                    tran.Commit();
                }
            }
            else
            {
                investor.Step = InvestorAppStep.BAT_DAU;

                _dbContext.SaveChanges();
            }

        }

        /// <summary>
        /// Reset mật khẩu với refresh token
        /// </summary>
        /// <param name="input"></param>
        /// <exception cref="FaultException"></exception>
        public void ResetPassword(ResetPasswordDto input)
        {
            var investor = _investorRepository.GetByEmailOrPhone(input.EmailOrPhone);
            if (investor == null)
            {
                throw new FaultException(new FaultReason(_investorRepository.GetDefError((int)ErrorCode.InvestorEmalOrPhoneNotRegis)), new FaultCode(((int)ErrorCode.InvestorEmalOrPhoneNotRegis).ToString()), "");
            }

            var user = _userRepository.FindByInvestorId(investor.InvestorId);
            if (user == null)
            {
                throw new FaultException(new FaultReason(_investorRepository.GetDefError((int)ErrorCode.UserNotFound)), new FaultCode(((int)ErrorCode.UserNotFound).ToString()), "");
            }

            var now = DateTime.Now;
            if (input.ResetPasswordToken == user.ResetPasswordToken)
            {
                // Refresh password token hết hạn
                if (now > user.ResetPasswordTokenExp)
                {
                    throw new FaultException(new FaultReason(_investorRepository.GetDefError((int)ErrorCode.InvestorResetPasswordTokenExpire)), new FaultCode(((int)ErrorCode.InvestorResetPasswordTokenExpire).ToString()), "");
                }
                else if (input.Password == user.Password)
                {
                    throw new FaultException(new FaultReason(_investorRepository.GetDefError((int)ErrorCode.UserNewPasswordSameOldPassword)), new FaultCode(((int)ErrorCode.UserNewPasswordSameOldPassword).ToString()), "");
                }
                else
                {
                    user.Password = input.Password;
                    user.IsTempPassword = YesNo.NO;

                    // cho reset pass token hết hạn
                    user.ResetPasswordTokenExp = now;

                    _dbContext.SaveChanges();
                }
            }
            else
            {
                throw new FaultException(new FaultReason(_investorRepository.GetDefError((int)ErrorCode.InvestorResetPasswordTokenInvalid)), new FaultCode(((int)ErrorCode.InvestorResetPasswordTokenInvalid).ToString()), "");
            }
        }

        public void ResetPassword(ResetPasswordManagerInvestorDto input)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Khách hàng quét mã giới thiệu lần đầu
        /// </summary>
        /// <param name="dto"></param>
        /// <exception cref="FaultException"></exception>
        public void ScanReferralCodeFirstTime(ScanReferralCodeFirstTimeDto dto)
        {
            int investorId = CommonUtils.GetCurrentInvestorId(_httpContext);

            using (var tran = _dbContext.Database.BeginTransaction())
            {
                RegisReferralCode(investorId, dto.ReferralCode);

                _dbContext.SaveChanges();
                tran.Commit();
            }

        }

        public void ScanReferralCodeSale(ScanReferralCodeSaleDto dto)
        {
            throw new NotImplementedException();
        }

        public Task<string> SendVerifyEmail()
        {
            throw new NotImplementedException();
        }

        public void SetBankDefault(int investorBankId)
        {
            throw new NotImplementedException();
        }

        public void SetDefaultContactAddress(SetDefaultContactAddressDto dto)
        {
            throw new NotImplementedException();
        }

        public void SetDefaultIdentification(int identificationId)
        {
            throw new NotImplementedException();
        }

        public void SetDefaultReferralCodeSale(decimal id)
        {
            throw new NotImplementedException();
        }

        public void UpdateContactAddress(UpdateContactAddressDto dto)
        {
            throw new NotImplementedException();
        }

        public int UpdateInvestor(int id, UpdateInvestorDto input)
        {
            throw new NotImplementedException();
        }

        public void UploadAvatar(AppUploadAvatarDto dto)
        {
            throw new NotImplementedException();
        }

        public void UploadProfFile(UploadProfFileDto dto)
        {
            throw new NotImplementedException();
        }

        public void UploadProfFileCommon(UploadProfFileDto dto, int investorId, string username, int userId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Validate password aka Investor đăng nhập trên app
        /// </summary>
        /// <param name="phone"></param>
        /// <param name="password"></param>
        /// <param name="fcmToken"></param>
        /// <returns></returns>
        public bool ValidatePassword(string phone, string password, string fcmToken)
        {
            bool ok = false;

            using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                var investor = _investorRepository.GetByEmailOrPhone(phone);
                var user = _userRepository.FindByInvestorId(investor.InvestorId);

                if (user.Password == CommonUtils.CreateMD5(password))
                {
                    ok = true;

                    if (user.IsFirstTime == YesNo.YES)
                    {
                        user.IsFirstTime = YesNo.NO;
                    }
                }

                if (ok && !string.IsNullOrEmpty(fcmToken))
                {
                    _userRepository.SaveFcmTokenByPhone(phone, fcmToken);
                }

                _dbContext.SaveChanges();
                transaction.Complete();
            }
            return ok;
        }

        /// <summary>
        /// Validate pin. Sai quá số lần quy định trong sysvar thì bị khóa
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public bool ValidatePin(ValidatePinDto input)
        {
            var userId = CommonUtils.GetCurrentUserId(_httpContext);
            var user = _userRepository.FindById(userId);

            if (user == null)
            {
                _userRepository.ThrowException(ErrorCode.UserNotFound);
            }

            if (user.PinCode != input.Pin)
            {
                var maxPinInvalidCount = Convert.ToInt32(_sysVarRepository.GetVarByName("AUTH", "PIN_INVALID_COUNT")?.VarValue);

                var pinInvalidCount = _httpContext.HttpContext.Session.GetInt32(SessionKeys.PIN_VALIDATE_FAIL_COUNT);

                pinInvalidCount++;
                _httpContext.HttpContext.Session.SetInt32(SessionKeys.PIN_VALIDATE_FAIL_COUNT, pinInvalidCount ?? 0);

                if (pinInvalidCount >= maxPinInvalidCount)
                {
                    user.Status = UserStatus.DEACTIVE;
                    _dbContext.SaveChanges();
                    _userRepository.ThrowException(ErrorCode.UserDeactive);
                }
                else
                {
                    _userRepository.ThrowException(ErrorCode.InvestorInvalidPin);
                }
            }
            else
            {
                _httpContext.HttpContext.Session.Remove(SessionKeys.PIN_VALIDATE_FAIL_COUNT);
            }

            return user.PinCode == input.Pin;
        }

        /// <summary>
        /// Luồng đăng ký. Xác thực otp
        /// </summary>
        /// <param name="input"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void VerifyCode(VerificationCodeDto input)
        {
            _logger.LogInformation($"{System.Reflection.MethodBase.GetCurrentMethod().Name}: {JsonSerializer.Serialize(input)}");

            var investor = _investorRepository.GetByEmailOrPhone(input.Phone);
            if (investor == null)
            {
                _investorRepository.ThrowException(ErrorCode.UserNotFound);
            }

            var user = _userRepository.FindByInvestorId(investor.InvestorId);
            if (user == null)
            {
                _investorRepository.ThrowException(ErrorCode.UserNotFound);
            }

            using (var tran = _dbContext.Database.BeginTransaction())
            {
                // chuyển mã xác thực otp về không active
                investor.Step = InvestorAppStep.DA_DANG_KY;
                var maxOtpFailCount = _sysVarRepository.GetInvValueByName("AUTH", "OTP_INVALID_COUNT");

                try
                {
                    _authOtpRepository.CheckOtpByUserId((int)user.UserId, input.VerificationCode, _httpContext, SessionKeys.OTP_FAIL_COUNT, maxOtpFailCount);
                    _investorRepository.UpdateEkycStepDate(InvestorAppStep.DA_DANG_KY, investor.InvestorId);
                }
                catch (Exception)
                {
                    tran.Commit();
                    throw;
                }

                // add investor sale nếu là mã giới thiệu của sale
                var sale = _saleRepository.FindSaleByReferralCode(investor.ReferralCode);
                if (sale != null)
                {
                    _investorSaleRepository.CreateInvestorSale(new InvestorSale
                    {
                        InvestorId = investor.InvestorId,
                        SaleId = sale.SaleId,
                        ReferralCode = investor.ReferralCode,
                        IsDefault = YesNo.YES,
                        Deleted = YesNo.NO,
                    });
                }

                _investorRegisterLogServices.Add(new CreateInvestorRegisterLogDto
                {
                    Phone = input.Phone,
                    Type = InvestorRegisterLogTypes.SuccessfulOtp
                });

                _dbContext.SaveChanges();
                tran.Commit();
            }
        }

        public void VerifyEmail(string verifyCode)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Xác nhận otp reset password
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        /// <exception cref="FaultException"></exception>
        public VerifyOtpResultDto VerifyOTPResetPass(VerifyOTPDto input)
        {
            var investor = _investorRepository.GetByEmailOrPhone(input.EmailOrPhone);

            if (investor == null)
            {
                throw new FaultException(new FaultReason(_investorRepository.GetDefError((int)ErrorCode.UserNotFound)), new FaultCode(((int)ErrorCode.UserNotFound).ToString()), "");
            }

            var resetPassExpRange = _sysVarRepository.GetVarByName("EXPIRE", "RESET_PASSWORD_TOKEN_EXP")?.VarValue;

            var user = _userRepository.FindByInvestorId(investor.InvestorId);

            // Lấy otp active theo user id. Ko có thì là hết hạn
            var otp = _authOtpRepository.FindActiveOtpByUserId(user.UserId);
            if (otp == null)
            {
                throw new FaultException(new FaultReason(_investorRepository.GetDefError((int)ErrorCode.InvestorOTPExpire)), new FaultCode(((int)ErrorCode.InvestorOTPExpire).ToString()), "");
            }

            using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                // Sinh token refresh password
                string resetPasswordToken = GenerateCodes.ResetPasswordToken();
                user.ResetPasswordToken = resetPasswordToken;
                user.ResetPasswordTokenExp = DateTime.Now.AddSeconds(Convert.ToInt32(resetPassExpRange));

                var maxOtpFailCount = _sysVarRepository.GetInvValueByName("AUTH", "OTP_INVALID_COUNT");

                try
                {
                    _authOtpRepository.CheckOtpByUserId((int)user.UserId, input.OTP, _httpContext, SessionKeys.OTP_FAIL_COUNT, maxOtpFailCount);
                }
                catch (Exception)
                {
                    transaction.Complete();
                    throw;
                }

                _dbContext.SaveChanges();
                transaction.Complete();

                return new VerifyOtpResultDto
                {
                    ResetPasswordToken = resetPasswordToken
                };
            }
        }

        /// <summary>
        /// Lấy step của investor. Nếu truyền phone = null thì lấy investor id từ token
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        public int? GetInvestorStep(string phone)
        {
            int? investorId = null;
            if (string.IsNullOrEmpty(phone))
            {
                investorId = CommonUtils.GetCurrentInvestorId(_httpContext);
            }
            else
            {
                investorId = _investorRepository.GetByEmailOrPhone(phone)?.InvestorId;
            }

            return _investorRepository.GetInvestorStep(investorId ?? 0);
        }

        public void UpdateEkycId(int investorId, UpdateEkycIdDto dto, bool saveChanges = false)
        {
            _logger.LogInformation($"{System.Reflection.MethodBase.GetCurrentMethod().Name}: investorId = {investorId}; dto = {JsonSerializer.Serialize(dto)}; saveChanges = {saveChanges}");

            var investor = _investorRepository.FindById(investorId);
            if (investor == null)
            {
                _investorRepository.ThrowException(ErrorCode.UserNotFound);
            }
            //investor.Step = InvestorAppStep.DA_EKYC;

            // Xem có giấy tờ trùng số không
            var anySameIdno = _investorRepository.AnyAppIdentitficationDuplicate(dto.IdNo, investor.InvestorGroupId);
            if (anySameIdno)
            {
                _investorRepository.ThrowException(ErrorCode.InvestorIdNoExisted);
            }

            // Lấy giấy tờ mặc định của investor (nếu có)
            var oldIdentification = _investorRepository.FindTempIdentificationByIdNo(investorId, dto.IdNo);
            string status = InvestorStatus.TEMP;

            if (oldIdentification != null)
            {
                // có rồi thì update
                oldIdentification.Status = status;
                oldIdentification.IdNo = dto.IdNo;
                oldIdentification.Fullname = dto.Name;
                oldIdentification.DateOfBirth = dto.DateOfBirth;
                oldIdentification.Nationality = dto.Nationality;
                oldIdentification.Sex = dto.Sex;
                oldIdentification.IdIssuer = dto.Issuer;
                oldIdentification.IdDate = dto.IssueDate;
                oldIdentification.IdExpiredDate = dto.IssueExpDate;
                oldIdentification.PlaceOfOrigin = dto.PlaceOfOrigin;
                oldIdentification.PlaceOfResidence = dto.PlaceOfResidence;
                oldIdentification.IdFrontImageUrl = dto.FrontImage;
                oldIdentification.IdBackImageUrl = dto.BackImage;

                if (saveChanges)
                {
                    _dbContext.SaveChanges();
                }
            }
            else
            {
                using (var tran = _dbContext.Database.BeginTransaction())
                {
                    // chưa có thì bỏ default ở những giấy tờ trước và xóa các giấy tờ trc đi
                    _investorRepository.SoftDeleteIdentification(investorId);

                    // thêm mới giấy tờ và lấy giấy tờ này làm mặc định
                    _investorRepository.AddIdentification(new InvestorIdentification
                    {
                        InvestorId = investorId,
                        InvestorGroupId = investor.InvestorGroupId ?? 0,
                        IdType = dto.IdType,
                        IdNo = dto.IdNo,
                        DateOfBirth = dto.DateOfBirth,
                        Fullname = dto.Name,
                        Nationality = dto.Nationality,
                        Sex = dto.Sex,
                        IsDefault = YesNo.YES,
                        IdIssuer = dto.Issuer,
                        IdDate = dto.IssueDate,
                        IdExpiredDate = dto.IssueExpDate,
                        PlaceOfOrigin = dto.PlaceOfOrigin,
                        PlaceOfResidence = dto.PlaceOfResidence,
                        IdFrontImageUrl = dto.FrontImage,
                        IdBackImageUrl = dto.BackImage
                    });

                    if (saveChanges)
                    {
                        _dbContext.SaveChanges();
                        tran.Commit();
                    }
                }

            }
        }

        /// <summary>
        /// Cập nhật ảnh khi nhận diện
        /// </summary>
        /// <param name="dto"></param>
        public async Task UploadFaceImage(SaveFaceMatchImageDto dto)
        {
            var investorId = _investorRepository.GetByEmailOrPhone(dto.Phone)
                                .ThrowIfNull(_dbContext, ErrorCode.InvestorNotFound)
                                ?.InvestorId;
            _logger.LogInformation($"{System.Reflection.MethodBase.GetCurrentMethod().Name}: dto = {JsonSerializer.Serialize(dto)};");

            var identification = _investorRepository.GetDefaultIdentification(investorId ?? 0);
            var frontImage = _imageServices.GenFileFromPath(identification.IdFrontImageUrl);

            var res = await _ocr.FaceRecognition(frontImage, dto.Image);
            string filePath = _imageServices.UploadImage(new ImageAPI.Models.UploadFileModel
            {
                Folder = FileFolder.INVESTOR,
                File = dto.Image
            });

            var investorRegisterLog = _investorRegisterLogEFRepository.EntityNoTracking.FirstOrDefault(o => o.Phone == dto.Phone
                                    && o.Type == InvestorRegisterLogTypes.StartEkyc);
            if (investorRegisterLog == null)
            {
                _investorRegisterLogServices.Add(new CreateInvestorRegisterLogDto
                {
                    Phone = dto.Phone,
                    Type = InvestorRegisterLogTypes.StartEkyc
                });
            }

            _investorRepository.UpdateFaceMatchImage(investorId ?? 0, dto.ImageType, filePath, res?.Similarity);
            _dbContext.SaveChanges();
        }

        /// <summary>
        /// Đăng ký ngay (DashboardCore)
        /// </summary>
        public void RegisterNow()
        {
            _investorRegisterLogServices.Add(new CreateInvestorRegisterLogDto
            {
                Phone = null,
                Type = InvestorRegisterLogTypes.RegisterNow
            });
        }
        public void UpdateReferralCode(int investorId, string referralCode)
        {

            // lấy status của investor(để xem là đang đăng ký từ app hay đã đký xong rồi)
            var investor = _investorRepository.FindById(investorId)
                .ThrowIfNull(_dbContext, ErrorCode.InvestorNotFound);

            // check mã giới thiệu có tồn tại trong bảng investor ko. Loại ra mã giới thiệu của chính investor này.
            // check mã giới thiệu có tồn tại trong bảng business customer ko
            var investorCheck = _investorRepository.GetByReferralCodeSelf(referralCode);
            var bcCheck = _businessCustomerRepository.FindByReferralCodeSelf(referralCode);

            if ((investorCheck == null || investorCheck.InvestorId == investor.InvestorId) && bcCheck == null)
            {
                _investorRepository.ThrowException(ErrorCode.InvestorRefCodeNotFound);
            }

            // check investor đã có mã giới thiệu chưa
            if (investor.ReferralCode != null)
            {
                _investorRepository.ThrowException(ErrorCode.InvestorAlreadyUseReferralCode);
            }

            // check mã giới thiệu có phải của sale ko
            // lấy ra sale id theo mã giới thiệu
            var sale = _saleRepository.GetSaleByReferralCodeSelf(referralCode);

            // check xem mã giới thiệu đã được investor sử dụng chưa
            var referralCodeUsed = _investorRepository.IsReferralCodeInUse(investor.InvestorId, referralCode);
            if (referralCodeUsed)
            {
                _investorRepository.ThrowException(ErrorCode.InvestorReferralCodeIsUsed);
            }

            // cập nhật vào investor
            investor.ReferralCode = referralCode;
            investor.ReferralDate = DateTime.Now;

            // cập nhật vào investor sale
            if (sale != null && investor.Status != InvestorStatus.TEMP)
            {
                // Set giá trị của trường mặc định
                bool isInvestorScanSaleReferralCode = _investorSaleRepository.IsInvestorScanReferralCode(investorId);

                // insert
                _investorSaleRepository.CreateInvestorSale(new InvestorSale
                {
                    InvestorId = investorId,
                    SaleId = sale.SaleId,
                    ReferralCode = referralCode,
                    IsDefault = isInvestorScanSaleReferralCode ? YesNo.NO : YesNo.YES
                });
            }
            _dbContext.SaveChanges();
        }

        #region private
        private string GetDefaultOtp()
        {
            var otp = _sysVarRepository.GetVarByName("AUTH", "OTP_DEFAULT");

            return otp?.VarValue;
        }

        /// <summary>
        /// Sinh otp theo investor id
        /// </summary>
        /// <param name="investorId"></param>
        /// <returns></returns>
        /// <exception cref="FaultException"></exception>
        private string GenerateOtp(int investorId)
        {
            var otpExpRange = Convert.ToInt32(_sysVarRepository.GetVarByName("EXPIRE", "OTP_EXP")?.VarValue);
            var otpLen = Convert.ToInt32(_sysVarRepository.GetVarByName("AUTH", "OTP_LENGTH")?.VarValue);

            var otpCode = CommonUtils.RandomNumber(otpLen);

            var investor = _investorRepository.FindById(investorId);
            if (investor == null)
            {
                throw new FaultException(new FaultReason(_investorRepository.GetDefError((int)ErrorCode.UserNotFound)), new FaultCode(((int)ErrorCode.UserNotFound).ToString()), "");
            }

            var user = _userRepository.FindByInvestorId(investorId);
            if (user == null)
            {
                throw new FaultException(new FaultReason(_investorRepository.GetDefError((int)ErrorCode.UserNotFound)), new FaultCode(((int)ErrorCode.UserNotFound).ToString()), "");
            }

            if (user.Status == UserStatus.DEACTIVE)
            {
                throw new FaultException(new FaultReason(_investorRepository.GetDefError((int)ErrorCode.InvestorDeactive)), new FaultCode(((int)ErrorCode.InvestorDeactive).ToString()), "");
            }

            if (user.Status == UserStatus.LOCKED)
            {
                throw new FaultException(new FaultReason(_investorRepository.GetDefError((int)ErrorCode.UserIsLocked)), new FaultCode(((int)ErrorCode.UserIsLocked).ToString()), "");
            }

            var now = DateTime.Now;
            var expTime = now.AddSeconds(otpExpRange);

            using (var tran = _dbContext.Database.BeginTransaction())
            {
                _authOtpRepository.UpdateIsActiveByUserId(YesNo.NO, Convert.ToInt32(user.UserId));

                _authOtpRepository.CreateOtp(new AuthOtp
                {
                    Phone = investor.Phone,
                    OtpCode = otpCode,
                    UserId = user?.UserId ?? 0,
                    CreatedTime = now,
                    ExpiredTime = expTime,
                    IsActive = YesNo.YES,
                });
                _dbContext.SaveChanges();
                tran.Commit();
            }

            return otpCode;
        }

        private OtpDto CreateVerifyCode(string phone, string email)
        {
            var verifyExpRange = Convert.ToInt32(_sysVarRepository.GetVarByName("EXPIRE", "VERIFY_EXP")?.VarValue);
            var otpLen = Convert.ToInt32(_sysVarRepository.GetVarByName("AUTH", "OTP_LENGTH")?.VarValue);

            // check khách có email chưa
            var investorHaveEmail = _investorRepository.AnyInvestorHaveEmail(email);
            if (!investorHaveEmail)
            {
                _investorRepository.ThrowException(ErrorCode.InvestorCreateUserNotEmail);
            }

            // check khách có phone chưa
            var investorHavePhone = _investorRepository.AnyInvestorHavePhone(phone);
            if (!investorHavePhone)
            {
                _investorRepository.ThrowException(ErrorCode.InvestorCreateUserNotPhone);
            }

            var otpCode = CommonUtils.RandomNumber(otpLen);

            var investor = _investorRepository.GetByEmailAndPhone(phone, email);
            var user = _userRepository.FindByInvestorId(investor.InvestorId)
                .ThrowIfNull(_dbContext, ErrorCode.UserNotFound);

            _authOtpRepository.UpdateIsActiveByUserId(YesNo.NO, (int)user.UserId);

            var now = DateTime.Now;
            var expDate = now.AddSeconds(verifyExpRange);

            _authOtpRepository.CreateOtp(new AuthOtp
            {
                CreatedTime = now,
                ExpiredTime = expDate,
                OtpCode = otpCode,
                IsActive = YesNo.YES,
                Phone = phone,
                UserId = user.UserId
            });

            return new OtpDto { Exp = expDate, Otp = otpCode };
        }

        /// <summary>
        /// Sinh otp theo sđt
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        private string GenerateOtpByPhone(string phone)
        {
            var userId = CommonUtils.GetCurrentUserId(_httpContext);
            var otpExpRange = Convert.ToInt32(_sysVarRepository.GetVarByName("EXPIRE", "OTP_EXP")?.VarValue);
            var otpLen = Convert.ToInt32(_sysVarRepository.GetVarByName("AUTH", "OTP_LENGTH")?.VarValue);

            var otpCode = CommonUtils.RandomNumber(otpLen);

            var now = DateTime.Now;
            var expTime = now.AddSeconds(otpExpRange);

            using (var tran = _dbContext.Database.BeginTransaction())
            {
                _authOtpRepository.UpdateIsActiveByPhone(YesNo.NO, phone);

                _authOtpRepository.CreateOtp(new AuthOtp
                {
                    Phone = phone,
                    OtpCode = otpCode,
                    UserId = userId,
                    CreatedTime = now,
                    ExpiredTime = expTime,
                    IsActive = YesNo.YES,
                });

                tran.Commit();
            }

            return otpCode;
        }

        /// <summary>
        /// App khách hàng quét mã giới thiệu lần đầu
        /// </summary>
        /// <param name="investorId"></param>
        /// <param name="referralCode"></param>
        /// <exception cref="FaultException"></exception>
        private void RegisReferralCode(int? investorId, string referralCode)
        {
            if (investorId == null)
            {
                investorId = CommonUtils.GetCurrentInvestorId(_httpContext);

            }

            // lấy status của investor(để xem là đang đăng ký từ app hay đã đký xong rồi)
            var investor = _investorRepository.FindById(investorId ?? 0);
            if (investor == null)
            {
                throw new FaultException(new FaultReason(_investorRepository.GetDefError((int)ErrorCode.InvestorNotFound)), new FaultCode(((int)ErrorCode.InvestorNotFound).ToString()), "");
            }

            // check mã giới thiệu có tồn tại trong bảng investor ko. Loại ra mã giới thiệu của chính investor này.
            // check mã giới thiệu có tồn tại trong bảng business customer ko
            var investorCheck = _investorRepository.GetByReferralCodeSelf(referralCode);
            var bcCheck = _businessCustomerRepository.FindByReferralCodeSelf(referralCode);

            if ((investorCheck == null || investorCheck.InvestorId == investor.InvestorId) && bcCheck == null)
            {
                throw new FaultException(new FaultReason(_investorRepository.GetDefError((int)ErrorCode.InvestorRefCodeNotFound)), new FaultCode(((int)ErrorCode.InvestorRefCodeNotFound).ToString()), "");
            }

            // check investor đã có mã giới thiệu chưa
            if (investor.Status != InvestorStatus.TEMP && string.IsNullOrEmpty(investor.ReferralCode))
            {
                throw new FaultException(new FaultReason(_investorRepository.GetDefError((int)ErrorCode.InvestorAlreadyUseReferralCode)), new FaultCode(((int)ErrorCode.InvestorAlreadyUseReferralCode).ToString()), "");
            }

            // check mã giới thiệu có phải của sale ko
            // lấy ra sale id theo mã giới thiệu
            var sale = _saleRepository.GetSaleByReferralCodeSelf(referralCode);

            // check xem mã giới thiệu đã được investor sử dụng chưa
            var referralCodeUsed = _investorRepository.IsReferralCodeInUse(investor.InvestorId, referralCode);

            if (referralCodeUsed)
            {
                _investorRepository.ThrowException(ErrorCode.InvestorReferralCodeIsUsed);
            }


            // cập nhật vào investor
            investor.ReferralCode = referralCode;
            investor.ReferralDate = DateTime.Now;

            // cập nhật vào investor sale
            if (sale != null && investor.Status != InvestorStatus.TEMP)
            {
                // Set giá trị của trường mặc định
                bool isInvestorScanSaleReferralCode = _investorSaleRepository.IsInvestorScanReferralCode(investorId ?? 0);

                // insert
                _investorSaleRepository.CreateInvestorSale(new InvestorSale
                {
                    InvestorId = investorId ?? 0,
                    SaleId = sale.SaleId,
                    ReferralCode = referralCode,
                    IsDefault = isInvestorScanSaleReferralCode ? YesNo.NO : YesNo.YES
                });
            }
            _dbContext.SaveChanges();
        }

        private bool verifyRequiredFieldEkyc(InvestorIdentification identification, List<string> incorrectFields)
        {
            bool isConfirmed = true;
            // Check xem những trường bắt buộc khi ekyc đã có giá trị chưa
            var checkDefaultIden = _mapper.Map<EkycRequiredFieldsDto>(identification);
            foreach (var property in checkDefaultIden.GetType().GetProperties())
            {
                var val = property.GetValue(checkDefaultIden, null);
                bool allowCheck = identification.IdType?.ToUpper() != IDTypes.PASSPORT.ToUpper() || (identification.IdType?.ToUpper() == IDTypes.PASSPORT.ToUpper() && property.Name != "PlaceOfResidence");

                if (((val is string && string.IsNullOrEmpty(val?.ToString())) || val == null) && allowCheck)
                {
                    incorrectFields.Add(EkycFields.Dict[property.Name]);
                    isConfirmed = false;
                }
            }

            return isConfirmed;
        }

        public void CheckTooManyRequestIpAddress(string phone)
        {
            var ipAddress = CommonUtils.GetCurrentRemoteIpAddress(_httpContext);
            DateTime now = DateTime.Now;

            //Khoảng thời gian đăng ký là 1 tiếng cho 1 requestIp
            var sysVar = _sysVarRepository.GetVarByName(GrNames.AUTH, "TIME_ACCEPT_REQUEST_IP");
            //Nếu trong vòng 1 tiếng trởi lại mà có 1 investor đăng ký bởi ip này thì chặn
            var timeAcceptRequestIp = int.Parse(sysVar?.VarValue ?? "3600");
            var investor = _dbContext.Investors.Where(i => i.IpAddress == ipAddress && (i.CreatedDate != null && i.CreatedDate.Value.AddSeconds(timeAcceptRequestIp) > now) && i.Phone != phone);
            if (investor.Any())
            {
                throw new FaultException(new FaultReason(_investorRepository.GetDefError((int)ErrorCode.InvestorTooManyRequestIp)), new FaultCode(((int)ErrorCode.InvestorTooManyRequestIp).ToString()), "");
            }
        }

        public IEnumerable<AppInvestorContactListDto> GetAllContactList(AppFilterContactListDto input)
        {
            var investorId = CommonUtils.GetCurrentInvestorId(_httpContext);
            _logger.LogInformation($"{nameof(GetAllContactList)}: investorId = {investorId}");

            var investorInfo = _dbContext.Investors.FirstOrDefault(i => i.InvestorId == investorId && i.Deleted == YesNo.NO).ThrowIfNull(_dbContext, ErrorCode.InvestorNotFound);
            var result = new List<AppInvestorContactListDto>();

            var businessCustomerContact = from investorTrading in _dbContext.InvestorTradingProviders
                                          join tradingProvider in _dbContext.TradingProviders on investorTrading.TradingProviderId equals tradingProvider.TradingProviderId
                                          join businessCustomer in _dbContext.BusinessCustomers on tradingProvider.BusinessCustomerId equals businessCustomer.BusinessCustomerId
                                          where investorTrading.InvestorId == investorId && investorTrading.Deleted == YesNo.NO
                                          && tradingProvider.Deleted == YesNo.NO && businessCustomer.Status == BusinessCustomerStatus.HOAT_DONG
                                          && (input.Keyword == null || businessCustomer.Phone.Contains(input.Keyword) || businessCustomer.Name.ToLower().Contains(input.Keyword.ToLower()) || businessCustomer.ShortName.ToLower().Contains(input.Keyword.ToLower()))
                                          select new AppInvestorContactListDto
                                          {
                                              TradingProviderId = investorTrading.TradingProviderId,
                                              AvatarImageUrl = businessCustomer.AvatarImageUrl,
                                              FullName = businessCustomer.Name,
                                              Phone = businessCustomer.Phone,
                                              ShortName = businessCustomer.ShortName
                                          };
            var investorSaleContact = from investorSale in _dbContext.InvestorSales
                                      join sale in _dbContext.Sales on investorSale.SaleId equals sale.SaleId
                                      join investor in _dbContext.Investors on sale.InvestorId equals investor.InvestorId
                                      from identification in _dbContext.InvestorIdentifications.Where(i => i.InvestorId == investor.InvestorId && i.Deleted == YesNo.NO)
                                        .OrderByDescending(c => c.IsDefault).ThenBy(i => i.Id).Take(1).DefaultIfEmpty()
                                      where investorSale.InvestorId == investorId
                                      && investorSale.Deleted == YesNo.NO
                                      && sale.Deleted == YesNo.NO
                                      && investor.Deleted == YesNo.NO && investor.Status == InvestorStatus.ACTIVE
                                      && (input.Keyword == null || investor.Phone.Contains(input.Keyword) || identification.Fullname.ToLower().Contains(input.Keyword.ToLower()))
                                      select new AppInvestorContactListDto
                                      {
                                          InvestorId = investorSale.InvestorId,
                                          FullName = identification.Fullname,
                                          AvatarImageUrl = investor.AvatarImageUrl,
                                          Phone = investor.Phone
                                      };

            var investorContact = from investor in _dbContext.Investors
                                  from identification in _dbContext.InvestorIdentifications.Where(i => i.InvestorId == investor.InvestorId && i.Deleted == YesNo.NO)
                                    .OrderByDescending(c => c.IsDefault).ThenBy(i => i.Id).Take(1).DefaultIfEmpty()
                                  where investor.ReferralCode == investorInfo.ReferralCodeSelf
                                  && investor.Deleted == YesNo.NO && investor.Status == InvestorStatus.ACTIVE
                                  && (input.Keyword == null || investor.Phone.Contains(input.Keyword) || identification.Fullname.ToLower().Contains(input.Keyword.ToLower()))
                                  select new AppInvestorContactListDto
                                  {
                                      InvestorId = investor.InvestorId,
                                      FullName = identification.Fullname,
                                      AvatarImageUrl = investor.AvatarImageUrl,
                                      Phone = investor.Phone
                                  };

            result.AddRange(businessCustomerContact);
            result.AddRange(investorSaleContact);
            result.AddRange(investorContact);
            return result;
        }
        #endregion
    }
}
