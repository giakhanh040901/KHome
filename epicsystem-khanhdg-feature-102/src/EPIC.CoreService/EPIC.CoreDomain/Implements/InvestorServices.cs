using AutoMapper;
using EPIC.CoreDomain.Interfaces;
using EPIC.CoreRepositories;
using EPIC.CoreSharedEntities.Dto.Investor;
using EPIC.CoreSharedServices.Implements;
using EPIC.DataAccess.Base;
using EPIC.DataAccess.Models;
using EPIC.Entities;
using EPIC.Entities.DataEntities;
using EPIC.Entities.Dto.CoreApprove;
using EPIC.Entities.Dto.ManagerInvestor;
using EPIC.Entities.Dto.User;
using EPIC.FileDomain.Services;
using EPIC.GarnerRepositories;
using EPIC.IdentityRepositories;
using EPIC.InvestRepositories;
using EPIC.Notification.Services;
using EPIC.RealEstateRepositories;
using EPIC.RocketchatDomain.Interfaces;
using EPIC.Utils;
using EPIC.Utils.ConfigModel;
using EPIC.Utils.ConstantVariables.Core;
using EPIC.Utils.ConstantVariables.Identity;
using EPIC.Utils.ConstantVariables.RealEstate;
using EPIC.Utils.ConstantVariables.Shared;
using EPIC.Utils.DataUtils;
using EPIC.Utils.Recognition.FPT;
using EPIC.Utils.Security;
using EPIC.Utils.SharedApiService;
using EPIC.Utils.SharedApiService.Dto.SharedEmailApiDto;
using EPIC.Utils.SharedApiService.Dto.SharedEmailApiDto.NotifyVerifyEmailTemplate;
//using EPIC.CoreEntities.Dto.Investor;
using Hangfire;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Threading.Tasks;
using System.Transactions;

namespace EPIC.CoreDomain.Implements
{
    public class InvestorServices : IInvestorServices
    {
        private readonly EpicSchemaDbContext _dbContext;
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;
        private readonly IMapper _mapper;
        private readonly OCRUtils _ocr;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IFileServices _imageServices;
        private readonly IRocketChatServices _rocketChatServices;
        private readonly NotificationServices _sendEmailServices;
        private readonly InvestorRepository _investorRepository;
        private readonly CifCodeEFRepository _cifCodeEFRepository;
        private readonly InvestorBankAccountRepository _investorBankAccountRepository;
        private readonly ApproveRepository _approveRepository;
        private readonly UserRepository _userRepository;
        private readonly SaleRepository _saleRepository;
        private readonly IOptions<RecognitionApiConfiguration> _recognitionApiConfig;
        private readonly PartnerRepository _partnerRepository;
        private readonly ManagerInvestorRepository _managerInvestorRepository;
        private readonly SharedNotificationApiUtils _sharedEmailApiUtils;
        private readonly NotificationServices _sendEmailService;
        private readonly BackgroundJobIdentityService _backgroundJobIdentityService;
        private readonly IBackgroundJobClient _backgroundJobs;
        private readonly InvestorToDoEFRepository _investorToDoEFRepository;

        //Các thông tin liên quan 
        //Invest
        private readonly InvestOrderEFRepository _investOrderEFRepository;
        private readonly InvestOrderRepository _investOrderRepository;
        private readonly InvestPolicyRepository _investPolicyRepository;
        private readonly InvestOrderPaymentEFRepository _investOrderPaymentEFRepository;

        //Garner
        private readonly GarnerOrderEFRepository _garnerOrderEFRepository;
        private readonly GarnerOrderPaymentEFRepository _garnerOrderPaymentEFRepository;
        //RealEstate
        private readonly RstOrderEFRepository _rstOrderEFRepository;
        private readonly RstOrderPaymentEFRepository _rstOrderPaymentEFRepository;
        public InvestorServices(ILogger<InvestorServices> logger,
            EpicSchemaDbContext dbContext,
            IConfiguration configuration,
            DatabaseOptions databaseOptions,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor,
            IOptions<RecognitionApiConfiguration> recognitionApiConfiguration,
            SharedNotificationApiUtils sharedEmailApiUtils,
            IRocketChatServices rocketChatServices,
            IFileServices imageServices,
            NotificationServices sendEmailService,
            BackgroundJobIdentityService backgroundJobIdentityService,
            NotificationServices sendEmailServices,
            //Các serviecs
            IBackgroundJobClient backgroundJobs)
        {
            _dbContext = dbContext;
            _logger = logger;
            _configuration = configuration;
            _connectionString = databaseOptions.ConnectionString;
            _mapper = mapper;
            _imageServices = imageServices;
            _httpContextAccessor = httpContextAccessor;
            _recognitionApiConfig = recognitionApiConfiguration;
            _sharedEmailApiUtils = sharedEmailApiUtils;
            _rocketChatServices = rocketChatServices;
            _sendEmailServices = sendEmailServices;
            _investorRepository = new InvestorRepository(_connectionString, _logger);
            _investorBankAccountRepository = new InvestorBankAccountRepository(_connectionString, _logger);
            _approveRepository = new ApproveRepository(_connectionString, _logger);
            _userRepository = new UserRepository(_connectionString, _logger);
            _partnerRepository = new PartnerRepository(_connectionString, _logger);
            _managerInvestorRepository = new ManagerInvestorRepository(_connectionString, _logger, _httpContextAccessor);
            _ocr = new OCRUtils(_recognitionApiConfig.Value, _logger);
            _userRepository = new UserRepository(_connectionString, _logger);
            _saleRepository = new SaleRepository(_connectionString, _logger);
            _sendEmailService = sendEmailService;
            _backgroundJobIdentityService = backgroundJobIdentityService;
            _backgroundJobs = backgroundJobs;


            _cifCodeEFRepository = new CifCodeEFRepository(_dbContext, _logger);
            //Các repo liên quan
            //Invest
            _investorToDoEFRepository = new InvestorToDoEFRepository(_dbContext, _logger);
            _investOrderRepository = new InvestOrderRepository(_connectionString, _logger);
            _investOrderEFRepository = new InvestOrderEFRepository(_dbContext, _logger);
            _investPolicyRepository = new InvestPolicyRepository(_connectionString, _logger);
            _investOrderPaymentEFRepository = new InvestOrderPaymentEFRepository(_dbContext, _logger);
            //Garner
            _garnerOrderPaymentEFRepository = new GarnerOrderPaymentEFRepository(_dbContext, _logger);
            _garnerOrderEFRepository = new GarnerOrderEFRepository(_dbContext, _logger);

            //RealEstate
            _rstOrderEFRepository = new RstOrderEFRepository(_dbContext, _logger);
            _rstOrderPaymentEFRepository = new RstOrderPaymentEFRepository(_dbContext, _logger);
            //Các services liên quan
        }

        public bool CheckEmailExist(CheckEmailExistDto model)
        {
            return _investorRepository.CheckEmailExist(model.Email);
        }

        public bool CheckPhoneExist(CheckPhoneExistDto model)
        {
            return _investorRepository.CheckPhoneExist(model.Phone);
        }

        public List<InvestorDto> Find()
        {
            return _mapper.Map<List<InvestorDto>>(_investorRepository.GetAll());
        }

        public bool IsRegisteredOffline(CheckRegisteredOfflineDto input)
        {
            return _investorRepository.IsRegisteredOffline(input.Email, input.Phone);
        }

        public void VerifyCode(VerificationCodeDto input)
        {
            _investorRepository.VerifyCode(input.Email, input.Phone, input.VerificationCode);
        }

        public void RegisterInventor(RegisterInvestorDto input)
        {
            _investorRepository.Register(input);
            if (!string.IsNullOrEmpty(input.FcmToken))
            {
                _userRepository.SaveFcmTokenByPhone(input.Phone, input.FcmToken);
            }
        }

        public bool ValidatePassword(string phone, string password, string fcmToken)
        {
            bool ok = false;

            _investorRepository.OpenConnection();

            using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                ok = _investorRepository.ValidatePassword(phone, password);

                if (ok && !string.IsNullOrEmpty(fcmToken))
                {
                    _userRepository.SaveFcmTokenByPhone(phone, fcmToken);
                }

                transaction.Complete();
            }
            _investorRepository.CloseConnection();

            return ok;
        }

        public Investor GetByEmailOrPhone(string phone)
        {
            return _investorRepository.GetByEmailOrPhone(phone);
        }

        public Investor GetByUsername(string username)
        {
            return _investorRepository.GetByUsername(username);
        }

        public async Task CreateVerificationCodeSendSmsAsync(InvestorEmailPhoneDto input)
        {
            var otp = _investorRepository.CreateVerificationCode(input);
            await _sendEmailService.SendSmsOtpAnonymous(otp.Otp, otp.Exp, input.Phone);
        }

        public async Task CreateVerificationCodeSendMailAsync(InvestorEmailPhoneDto input)
        {
            var otp = _investorRepository.CreateVerificationCode(input);
            await _sendEmailService.SendEmailOtpAnonymous(otp.Otp, otp.Exp, input.Email);
        }

        public async Task ForgotPasswordAsync(ForgotPasswordDto input)
        {
            var otp = _investorRepository.ForgotPassword(input.EmailOrPhone);
            var investor = _investorRepository.GetByEmailOrPhone(input.EmailOrPhone);
            if (input.SendType == ForgotPasswordType.SEND_MAIL)
            {
                await _sendEmailService.SendEmailOtpAnonymous(otp.Otp, otp.Exp, investor.Email);
            }
            else if (input.SendType == ForgotPasswordType.SEND_SMS)
            {
                await _sendEmailService.SendSmsOtpAnonymous(otp.Otp, otp.Exp, investor.Phone);
            }
        }

        public VerifyOtpResultDto VerifyOTPResetPass(VerifyOTPDto input)
        {
            string resetPasswordToken = GenerateCodes.ResetPasswordToken();
            int verifyOtpResult = _investorRepository.VerifyOTPResetPass(input.EmailOrPhone, input.OTP, resetPasswordToken);
            if (verifyOtpResult == TrueOrFalseNum.FALSE)
            {
                throw new FaultException(new FaultReason($"OTP không hợp lệ"), new FaultCode(((int)ErrorCode.InvestorOTPInvalid).ToString()), "");
            }
            else if (verifyOtpResult == TrueOrFalseNum.EXPIRE)
            {
                throw new FaultException(new FaultReason($"OTP hết hạn"), new FaultCode(((int)ErrorCode.InvestorOTPExpire).ToString()), "");
            }
            else
            {
                return new VerifyOtpResultDto()
                {
                    ResetPasswordToken = resetPasswordToken
                };
            }
        }

        public void ResetPassword(ResetPasswordDto input)
        {
            _investorRepository.ResetPassword(input.EmailOrPhone, input.ResetPasswordToken, input.Password);
        }

        public void ChangePassword(ChangePasswordDto input)
        {
            int userId = CommonUtils.GetCurrentUserId(_httpContextAccessor);
            _userRepository.ChangePassword(userId, input.OldPassword, input.NewPassword);
        }

        public void ChangePasswordTemp(ChangePasswordTempDto input)
        {
            int userId = CommonUtils.GetCurrentUserId(_httpContextAccessor);
            _userRepository.ChangePasswordTemp(userId, input.Password);
        }

        public void ChangePin(ChangePinDto input)
        {
            int investorId = CommonUtils.GetCurrentInvestorId(_httpContextAccessor);

            _investorRepository.ChangePin(investorId, input.OldPin, input.NewPin);
        }

        public bool ValidatePin(ValidatePinDto input)
        {
            int investorId = CommonUtils.GetCurrentInvestorId(_httpContextAccessor);
            return _investorRepository.ValidatePin(investorId, input.Pin);
        }

        public void AddBankAccount(BankAccountDto input)
        {
            _investorBankAccountRepository.Update(input.Phone, input.BankId, input.BankAccount, input.OwnerAccount);

            var investor = _investorRepository.GetByEmailOrPhone(input.Phone);

            _backgroundJobs.Enqueue(() => _backgroundJobIdentityService.AppRegisterInvestorSuccess(investor));
        }

        public async Task<EKYCOcrResultDto> EkycOCRAsync(EKYCOcrDto input)
        {
            //int investorId = CommonUtils.GetCurrentInvestorId(_httpContextAccessor);
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
                    //if (typeNew.Contains("chip"))
                    //{
                    //    idType = IDTypes.CCCD_CHIP;
                    //}
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
                    IdIssueExpDate = _ocr.ProccessExpDate(frontData.Doe, issueDate, idType),
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

                _investorRepository.UpdateEkycId(input.Phone, resultOcr.Name, resultOcr.DateOfBirth, resultOcr.Sex, resultOcr.IdNo,
                    resultOcr.IdIssueDate, resultOcr.IdIssueExpDate, resultOcr.IdIssuer, resultOcr.PlaceOfOrigin, resultOcr.PlaceOfResidence,
                    resultOcr.Nationality, idType, frontImageUrl, backImageUrl);
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
                    IdIssuer = passportData.IdIssuer,
                    Nationality = OCRNationality.ConvertStandard(passportData.Nationality)
                };

                _ocr.CheckAge(resultOcr.DateOfBirth);
                _ocr.CheckExp(resultOcr.IdIssueExpDate);

                frontImageUrl = _imageServices.UploadFileID(new ImageAPI.Models.UploadFileModel
                {
                    File = input.FrontImage,
                    Folder = FileFolder.INVESTOR,
                });

                _investorRepository.UpdateEkycId(input.Phone, resultOcr.Name, resultOcr.DateOfBirth, resultOcr.Sex, resultOcr.IdNo,
                    resultOcr.IdIssueDate, resultOcr.IdIssueExpDate, resultOcr.IdIssuer, resultOcr.PlaceOfOrigin, resultOcr.PlaceOfResidence,
                    resultOcr.Nationality, IDTypes.PASSPORT, frontImageUrl, backImageUrl);
            }
            return resultOcr;
        }

        public void EkycConfirmInfo(EKYCConfirmInfoDto input)
        {
            //int investorId = CommonUtils.GetCurrentInvestorId(_httpContextAccessor);
            string incorrectFieldString = string.Join(StringUtils.SEPARATOR, input.IncorrectFields ?? new List<string>());
            _logger.LogInformation($"EkycConfirmInfo: phone : {input.Phone}, confirm giấy tờ với isConfirm : {input.IsConfirmed}, danh sách các trường bị sai {incorrectFieldString}, giới tính: {input.Sex}");

            _investorRepository.EkycConfirmInfo(input.Phone, input.IsConfirmed, incorrectFieldString, input.Sex);
        }

        /// <summary>
        /// Nhận diện
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<EKYCFaceMatchResultDto> EkycFaceMatchAsync(EKYCFaceMatchDto input)
        {
            _logger.LogInformation($"Nhận diện khi đăng ký");
            FaceMatchData faceMatchData = await _ocr.FaceRecognition(input.IdCardImage, input.FaceImage);

            if (_ocr.CheckFaceSimilarity(faceMatchData.Similarity))
            {
                string faceImageUrl = _imageServices.UploadFileID(new ImageAPI.Models.UploadFileModel
                {
                    File = input.FaceImage,
                    Folder = FileFolder.INVESTOR,
                });

                _investorRepository.FinishEKYC(input.Phone, faceImageUrl);
            }
            else
            {
                throw new FaultException(new FaultReason("Xác thực khuôn mặt không khớp"), new FaultCode(((int)ErrorCode.InvestorFaceRecognitionNotMatch).ToString()), "");
            }

            return new EKYCFaceMatchResultDto
            {
                IsMatch = faceMatchData.IsMatch,
                Similarity = faceMatchData.Similarity,
                IsBothImgIDCard = faceMatchData.IsBothImgIDCard
            };
        }

        /// <summary>
        /// Nhận diện gương mặt. So khớp với ảnh của giấy tờ mặc định
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<EKYCFaceMatchResultDto> EkycFaceMatchLoggedInAsync(FaceRecognitionLoggedInDto input)
        {
            var investorId = CommonUtils.GetCurrentInvestorId(_httpContextAccessor);

            var defaultIdentification = _managerInvestorRepository.GetDefaultIdentification(investorId, false);
            if (defaultIdentification == null)
            {
                return null;
            }

            var baseDir = _configuration["FilePath"];
            _logger.LogInformation($"Nhận diện sau khi login");
            _logger.LogInformation($"IdFrontImageUrl = {defaultIdentification.IdFrontImageUrl}");
            _logger.LogInformation($"baseDir = {defaultIdentification.IdFrontImageUrl}");
            var frontImageInfo = FileUtils.GetPhysicalPath(defaultIdentification.IdFrontImageUrl, baseDir);
            if (!File.Exists(frontImageInfo.FullPath))
            {
                throw new FaultException(new FaultReason($"Ảnh mặt trước không tồn tại."), new FaultCode(((int)ErrorCode.FileExtensionNoAllow).ToString()), "");
            }
            var bytes = File.ReadAllBytes(frontImageInfo.FullPath);

            MemoryStream memoryStream = new(bytes.ToArray());
            FaceMatchData faceMatchData = await _ocr.FaceRecognition(new FormFile(memoryStream, 0, memoryStream.Length, frontImageInfo.FileName, frontImageInfo.FileName), input.FaceImage);

            if (_ocr.CheckFaceSimilarity(faceMatchData.Similarity))
            {
                string faceImageUrl = _imageServices.UploadFile(new ImageAPI.Models.UploadFileModel
                {
                    File = input.FaceImage,
                    Folder = frontImageInfo.Folder,
                });

                _investorRepository.FaceMatchLoggedIn(investorId, faceImageUrl);
            }
            else
            {
                throw new FaultException(new FaultReason("Xác thực khuôn mặt không khớp"), new FaultCode(((int)ErrorCode.InvestorFaceRecognitionNotMatch).ToString()), "");
            }

            return new EKYCFaceMatchResultDto
            {
                IsMatch = faceMatchData.IsMatch,
                Similarity = faceMatchData.Similarity,
                IsBothImgIDCard = faceMatchData.IsBothImgIDCard
            };
        }

        public PagingResult<InvestorDto> FindAll(int pageSize, int pageNumber, string keyword)
        {
            return _investorRepository.FindAll(pageSize, pageNumber, keyword);
        }

        public int UpdateInvestor(int id, UpdateInvestorDto input)
        {
            return _investorRepository.Update(new Investor
            {
                InvestorId = id,
                Name = input.Name,
                Address = input.Address,
                ContactAddress = input.ContactAddress,
                Nationality = input.Nationality,
            });
        }

        public Investor FindById(int id)
        {
            var investor = _investorRepository.FindById(id);
            return investor;
        }

        public int DeteleInvestor(int id)
        {
            return _investorRepository.Delete(id);
        }

        public void ResetPassword(ResetPasswordManagerInvestorDto input)
        {
            _investorRepository.ManagerResetPassword(input.EmailOrPhone, input.Password);
        }

        public int DeteleManagerInvestor(int id)
        {
            return _investorRepository.DeleteManagerInvestor(id);
        }

        public int ActivateManagerInvestor(int investorId, bool isActive)
        {
            return _investorRepository.ActivateManagerInvestor(investorId, isActive, CommonUtils.GetCurrentUsername(_httpContextAccessor));
        }

        public List<IdDto> GetListIdInfo()
        {
            var investorId = CommonUtils.GetCurrentInvestorId(_httpContextAccessor);
            var investorIdFind = _managerInvestorRepository.GetListIdentification(investorId, null, false);
            return _mapper.Map<List<IdDto>>(investorIdFind);
        }

        public List<TransactionAddessDto> GetListTransactionAddress()
        {
            var investorId = CommonUtils.GetCurrentInvestorId(_httpContextAccessor);
            var investorIdFind = _investorRepository.GetListContactAddress(null, investorId);
            var result = _mapper.Map<List<TransactionAddessDto>>(investorIdFind);
            return result;
        }

        public void AddTransactionAddess()
        {
            throw new NotImplementedException();
        }

        public void GenerateOtpMail()
        {
            int investorId = CommonUtils.GetCurrentInvestorId(_httpContextAccessor);
            _investorRepository.GenerateOtp(investorId);

        }

        public void GenerateOtpSms()
        {
            int investorId = CommonUtils.GetCurrentInvestorId(_httpContextAccessor);
            _investorRepository.GenerateOtp(investorId);
        }

        public void GenerateOtpSmsByPhone(string phone)
        {
            _investorRepository.GenerateOtpByPhone(phone);
        }

        public List<ViewInvestorContactAddressDto> GetListContactAddress(string keyword)
        {
            int investorId = CommonUtils.GetCurrentInvestorId(_httpContextAccessor);
            var contractAddresses = from contractAddress in _dbContext.InvestorContactAddresses
                                    let provinceName = _dbContext.Provinces.FirstOrDefault(p => p.Code == contractAddress.ProvinceCode).Name
                                    let districtName = _dbContext.Districts.FirstOrDefault(p => p.Code == contractAddress.DistrictCode).Name
                                    let wardName = _dbContext.Wards.FirstOrDefault(p => p.Code == contractAddress.WardCode).Name
                                    where contractAddress.InvestorId == investorId && contractAddress.Deleted == YesNo.NO
                                    orderby contractAddress.IsDefault descending, contractAddress.ContactAddressId descending
                                    select new ViewInvestorContactAddressDto
                                    {
                                        ContactAddressId = contractAddress.ContactAddressId,
                                        InvestorId = investorId,
                                        ContactAddress = contractAddress.ContactAddress,
                                        ProvinceCode = contractAddress.ProvinceCode,
                                        DetailAddress = contractAddress.DetailAddress,
                                        DistrictCode = contractAddress.DistrictCode,
                                        WardCode = contractAddress.WardCode,
                                        IsDefault = contractAddress.IsDefault,
                                        InvestorGroupId = contractAddress.InvestorGroupId,
                                        ProvinceName = provinceName,
                                        DistrictName = districtName,
                                        WardName = wardName
                                    };

            return contractAddresses.ToList();
        }

        public void AddContactAddress(CreateContactAddressDto dto)
        {
            string username = CommonUtils.GetCurrentUsername(_httpContextAccessor);
            dto.InvestorId = CommonUtils.GetCurrentInvestorId(_httpContextAccessor);

            _investorRepository.CreateContactAddress(dto, username);
        }

        public async Task UpdateContactAddress(UpdateContactAddressDto dto)
        {
            string username = CommonUtils.GetCurrentUsername(_httpContextAccessor);
            dto.InvestorId = CommonUtils.GetCurrentInvestorId(_httpContextAccessor);
            var investorContactAddress = _dbContext.InvestorContactAddresses.FirstOrDefault(x => x.ContactAddressId == dto.ContactAddressId 
                                            && x.InvestorId == dto.InvestorId && x.Deleted == YesNo.NO).ThrowIfNull(_dbContext, ErrorCode.InvestorContactAddressNotFound);
            _investorRepository.UpdateContactAddress(dto, username);

            if (investorContactAddress.IsDefault == YesNo.NO && dto.IsDefault == YesNo.YES)
            {
                await _sendEmailServices.SendEmailSetContactAddressDefaultSuccess(dto.ContactAddressId);
            }
        }

        public void SetDefaultContactAddress(SetDefaultContactAddressDto dto)
        {
            string username = CommonUtils.GetCurrentUsername(_httpContextAccessor);
            dto.InvestorId = CommonUtils.GetCurrentInvestorId(_httpContextAccessor);
            _investorRepository.SetDefaultContactAddress(dto, username);
        }

        public void SetDefaultIdentification(int identificationId)
        {
            string username = CommonUtils.GetCurrentUsername(_httpContextAccessor);
            _managerInvestorRepository.SetDefaultIdentification(new SetDefaultIdentificationDto
            {
                IsTemp = false,
                IdentificationId = identificationId
            }, username);
        }

        public List<InvestorIdentification> GetListIdentification(AppGetListIdentificationDto dto)
        {
            int investorId = CommonUtils.GetCurrentInvestorId(_httpContextAccessor);

            var listIdenDb = _managerInvestorRepository.GetListIdentification(investorId, null, false);

            if (string.IsNullOrEmpty(dto.Status))
            {
                return listIdenDb?.ToList();
            }

            return listIdenDb?.Where(iden => iden.Status == dto.Status)?.ToList();
        }

        public InvestorIdentification GetDefaultIdentification()
        {
            int investorId = CommonUtils.GetCurrentInvestorId(_httpContextAccessor);

            return _managerInvestorRepository.GetDefaultIdentification(investorId, false);
        }

        public async Task<EKYCOcrResultDto> AddIdentification(AddIdentificationDto input)
        {
            int investorId = CommonUtils.GetCurrentInvestorId(_httpContextAccessor);

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
                var investor = _managerInvestorRepository.FindById(investorId, false);

                if (investor == null)
                {
                    throw new FaultException(new FaultReason($"Nhà đầu tư không tồn tại: {investor.InvestorId}"), new FaultCode(((int)ErrorCode.InvestorNotFound).ToString()), "");
                }

                string username = CommonUtils.GetCurrentUsername(_httpContextAccessor);

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

                var identificationId = _investorRepository.CreateIdentification(dto, username);
                resultOcr.IdentificationId = identificationId;
            }

            return resultOcr;

        }

        public void UploadProfFile(UploadProfFileDto dto)
        {
            int investorId = CommonUtils.GetCurrentInvestorId(_httpContextAccessor);
            int userId = CommonUtils.GetCurrentUserId(_httpContextAccessor);
            string username = CommonUtils.GetCurrentUsername(_httpContextAccessor);

            UploadProfFileCommon(dto, investorId, username, userId);
        }

        public InvestorMyInfoDto GetMyInfo()
        {
            int investorId = CommonUtils.GetCurrentInvestorId(_httpContextAccessor);
            int userId = CommonUtils.GetCurrentUserId(_httpContextAccessor);

            var info = _managerInvestorRepository.FindById(investorId, false);

            if (info != null)
            {
                var user = _userRepository.FindById(userId);
                var result = _mapper.Map<InvestorMyInfoDto>(info);
                result.CurrentStep = info.Step;
                result.IsVerifiedFace = string.IsNullOrEmpty(info.FaceImageUrl) ? YesNo.NO : YesNo.YES;

                var identifications = _managerInvestorRepository.GetListIdentification(investorId, info.InvestorGroupId, false);

                var identification = identifications.FirstOrDefault(i => i.IsVerifiedIdentification == YesNo.YES);
                if (identification != null)
                {
                    result.IsVerified = identification.IsVerifiedIdentification;
                    result.EkycInfoIsConfirmed = identification.EkycInfoIsConfirmed;
                    result.Name = identification.Fullname;
                    result.Sex = identification.Sex;
                    result.BirthDate = identification.DateOfBirth;
                }

                var identificationDefault = _managerInvestorRepository.GetDefaultIdentification(investorId, false);
                if (identificationDefault != null)
                {
                    result.IsVerified = identificationDefault.IsVerifiedIdentification;
                    result.EkycInfoIsConfirmed = identificationDefault.EkycInfoIsConfirmed;
                    result.Name = identificationDefault.Fullname;
                    result.Sex = identificationDefault.Sex;
                    result.BirthDate = identificationDefault.DateOfBirth;
                }

                if (user != null)
                {
                    result.IsVerifiedEmail = user.IsVerifiedEmail;
                }

                result.IsScannedReferralCode = string.IsNullOrEmpty(info.ReferralCode) ? YesNo.NO : YesNo.YES;

                var coreSaleFind = _saleRepository.FindSaleByInvestorId(investorId);
                if (coreSaleFind != null)
                {
                    result.AutoDirectional = coreSaleFind.AutoDirectional;
                    var checkSaleType = _saleRepository.SaleCheckSaleTypeNotCollaborator(coreSaleFind.SaleId);
                    if (checkSaleType != null)
                    {
                        result.IsDirectioner = YesNo.YES;
                    }
                    else
                    {
                        result.IsDirectioner = YesNo.NO;
                    }
                }
                result.IsAdmin = _dbContext.EvtAdminEvents.Where(u => u.InvestorId == investorId).Any();
                return result;
            }
            return null;
        }

        public List<InvestorBankAccount> GetListBank()
        {
            int investorId = CommonUtils.GetCurrentInvestorId(_httpContextAccessor);

            var investor = _managerInvestorRepository.FindById(investorId, false);

            return _managerInvestorRepository.GetListBank(investorId, investor.InvestorGroupId, false)?.ToList();
        }

        public void AddBank(CreateBankDto dto)
        {
            dto.InvestorId = CommonUtils.GetCurrentInvestorId(_httpContextAccessor);
            string username = CommonUtils.GetCurrentUsername(_httpContextAccessor);

            _investorRepository.AddBank(dto, username);
        }

        public void SetBankDefault(int investorBankId)
        {
            string username = CommonUtils.GetCurrentUsername(_httpContextAccessor);

            _managerInvestorRepository.SetDefaultBank(new SetDefaultBankDto
            {
                InvestorBankId = investorBankId,
                IsTemp = false
            }, username);
        }

        public async Task<string> SendVerifyEmail()
        {
            var investorId = CommonUtils.GetCurrentInvestorId(_httpContextAccessor);
            var configLinkVerifyEmail = _configuration.GetSection("LinkVerifyEmail").Value;

            var investor = _managerInvestorRepository.FindById(investorId, false);
            var investorDefaultIdentification = _managerInvestorRepository.GetDefaultIdentification(investorId, false);

            string verifyEmailCode = _investorRepository.GenVerifyEmailCode(investorId);
            string linkVerify = $"{configLinkVerifyEmail}/{verifyEmailCode}";

            var partner = _partnerRepository.FindByInvestorId(investorId);

            await _sharedEmailApiUtils.NotifyVerifyEmailTemplateDto(new NotifyVerifyEmailTemplateContent
            {
                CustomerName = investorDefaultIdentification.Fullname,
                LinkVerify = linkVerify
            },
            new Receiver
            {
                Email = new EmailNotifi
                {
                    Address = investor.Email,
                    Title = "Thông báo xác minh địa chỉ email"
                }
            },
            new ParamsChooseTemplate
            {
                TradingProviderId = null
            });

            return linkVerify;
        }

        /// <summary>
        /// Xác thực email
        /// </summary>
        /// <param name="verifyCode"></param>
        public void VerifyEmail(string verifyCode)
        {
            _investorRepository.VerifyEmail(verifyCode);
        }

        /// <summary>
        /// Xác nhận thông tin ekyc giấy tờ
        /// </summary>
        /// <param name="dto"></param>
        public void ConfirmIdentificationEkyc(ConfirmIdentificationEkycDto dto)
        {
            var investorId = CommonUtils.GetCurrentInvestorId(_httpContextAccessor);
            string incorrectFieldString = string.Join(StringUtils.SEPARATOR, dto.IncorrectFields ?? new List<string>());

            _investorRepository.ConfirmIdentificationEkyc(dto, incorrectFieldString, investorId);
        }

        /// <summary>
        /// Cập nhật avatar
        /// </summary>
        /// <param name="dto"></param>
        public void UploadAvatar(AppUploadAvatarDto dto)
        {
            var investorId = CommonUtils.GetCurrentInvestorId(_httpContextAccessor);
            var username = CommonUtils.GetCurrentUsername(_httpContextAccessor);

            string filePath = _imageServices.UploadFile(new ImageAPI.Models.UploadFileModel
            {
                Folder = FileFolder.INVESTOR,
                File = dto.Avatar
            });

            _managerInvestorRepository.UploadAvatar(new UploadAvatarDto
            {
                Avatar = dto.Avatar,
                InvestorId = investorId,
                IsTemp = false
            }, filePath, username);
        }

        /// <summary>
        /// App quét mã giới thiệu lần đầu
        /// </summary>
        /// <param name="dto"></param>
        public void ScanReferralCodeFirstTime(ScanReferralCodeFirstTimeDto dto)
        {
            int investorId = CommonUtils.GetCurrentInvestorId(_httpContextAccessor);

            _investorRepository.ScanReferralCodeFirstTime(dto, investorId);
        }

        /// <summary>
        /// Lấy ra ds mã giới thiệu sale
        /// </summary>
        /// <returns></returns>
        public List<SaleInfoByInvestorDto> GetListReferralCodeSale()
        {
            int investorId = CommonUtils.GetCurrentInvestorId(_httpContextAccessor);
            return _investorRepository.GetListReferralCodeSaleByInvestorId(investorId, null, null, true);
        }

        /// <summary>
        /// Chọn mã giới thiệu mặc định
        /// </summary>
        /// <param name="id"></param>
        public void SetDefaultReferralCodeSale(decimal id)
        {
            _investorRepository.SetDefaultReferralCodeSale(id);
        }

        /// <summary>
        /// Quét mã giới thiệu của sale
        /// </summary>
        /// <param name="dto"></param>
        public void ScanReferralCodeSale(ScanReferralCodeSaleDto dto)
        {
            int investorId = CommonUtils.GetCurrentInvestorId(_httpContextAccessor);

            _investorRepository.ScanReferralCodeSale(dto, investorId);
        }

        /// <summary>
        /// Lấy thông tin investor/sale theo mã giới thiệu
        /// </summary>
        /// <param name="referralCode"></param>
        /// <returns></returns>
        public AppShortSalerInfoDto GetShortSalerInfoByReferralCodeSelf(string referralCode)
        {
            var sale = _saleRepository.SaleGetInfoByReferralCode(referralCode);

            if (sale != null)
            {
                var result = new AppShortSalerInfoDto
                {
                    FullName = sale.Fullname,
                    AvatarImageUrl = sale.AvatarImageUrl,
                    Email = sale.Email,
                    Phone = sale.Phone,
                    IsSale = sale.SaleId > 0
                };

                var listDepartment = _saleRepository.GetListDepartmentBySaleId(sale.SaleId);
                if (listDepartment != null)
                {
                    result.ListDepartment = _mapper.Map<List<AppDeparmentDto>>(listDepartment);
                }

                return result;
            }
            else
            {
                var investor = _managerInvestorRepository.FindByReferralCodeSelf(referralCode);

                if (investor != null)
                {
                    var identification = _managerInvestorRepository.GetDefaultIdentification(investor.InvestorId, false);
                    var result = _mapper.Map<AppShortSalerInfoDto>(investor);
                    var isSale = _investorRepository.IsSaleById(investor.InvestorId);
                    var listDepartment = _investorRepository.GetListDepartmentByInvestorId(investor.InvestorId);

                    result.FullName = identification?.Fullname;
                    result.IsSale = isSale;

                    if (listDepartment != null)
                    {
                        result.ListDepartment = _mapper.Map<List<AppDeparmentDto>>(listDepartment);
                    }

                    return result;
                }
            }

            return null;
        }

        public void CheckOtp(string otp)
        {
            int userId = CommonUtils.GetCurrentUserId(_httpContextAccessor);
            _investorRepository.CheckOtp(userId, otp);
        }

        /// <summary>
        /// Check mã giới thiệu có tồn tại ko
        /// </summary>
        /// <param name="referralCode"></param>
        /// <returns></returns>
        public bool isReferralCodeExist(string referralCode)
        {
            return _investorRepository.isReferralCodeExist(referralCode);
        }

        public void DeactiveMyUserAccount(AppDeactiveMyUserAccountDto dto)
        {
            var userId = CommonUtils.GetCurrentUserId(_httpContextAccessor);
            dto.PinCode = CommonUtils.CreateMD5(dto.PinCode);

            _investorRepository.DeactiveMyUserAccount(dto, userId);
        }

        /// <summary>
        /// Xoá bank account
        /// </summary>
        /// <param name="dto"></param>
        public void DeleteBankAccount(AppDeleteBankDto dto)
        {
            _investorRepository.DeleteBankAccount(dto);
        }

        /// <summary>
        /// Upload file nhà đầu tư chuyên nghiệp dùng chung
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="investorId">investorId được upload</param>
        /// <param name="username">username của người thao tác upload</param>
        /// <param name="userId">userId của người thao tác upload</param>
        public void UploadProfFileCommon(UploadProfFileDto dto, int investorId, string username, int userId)
        {
            var listFilePaths = new List<string>();
            var listFileName = new List<string>();
            var listFileType = new List<string>();

            foreach (var file in dto.ProfFile)
            {
                string path = _imageServices.UploadFile(new ImageAPI.Models.UploadFileModel
                {
                    File = file,
                    Folder = FileFolder.PROFESSIONAL
                });

                listFilePaths.Add(path);
                listFileName.Add(file.FileName);
                listFileType.Add(Path.GetExtension(file.FileName));
            }

            _approveRepository.OpenConnection();

            using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                // Tạo nhà đầu tư cn tạm
                var investorTemp = _managerInvestorRepository.UploadProfFile(investorId, username, listFilePaths, listFileType, listFileName);

                string summary = $"Nhà đầu tư có id: {investorId}";
                var listIdenDb = _managerInvestorRepository.GetListIdentification(investorId, null, false);
                var idenDefault = listIdenDb?.FirstOrDefault(i => i.IsDefault == YesNo.YES);
                if (idenDefault == null)
                {
                    idenDefault = listIdenDb?.FirstOrDefault();
                }
                if (idenDefault != null)
                {
                    summary = $"Nhà đầu tư: {idenDefault.Fullname} - {idenDefault.IdNo}";
                }

                // Tạo yêu cầu trình duyệt
                var approve = new CreateApproveRequestDto
                {
                    ActionType = ActionTypes.CAP_NHAT,
                    DataType = CoreApproveDataType.INVESTOR_PROFESSIONAL,
                    RequestNote = summary,
                    UserApproveId = null,
                    UserRequestId = userId,
                    ReferIdTemp = investorTemp.InvestorId,
                    ReferId = investorId,
                    Summary = summary,
                };

                _approveRepository.CreateApproveRequest(approve);

                transaction.Complete();
            }

            _approveRepository.CloseConnection();
        }

        public List<InvestorTodoDto> FindAllTodo()
        {
            _logger.LogInformation($"{nameof(FindAllTodo)}:");

            return _mapper.Map<List<InvestorTodoDto>>(_investorToDoEFRepository.FindAllToDo());
        }

        public List<InvestorTodoDto> GetAllTodo()
        {
            _logger.LogInformation($"{nameof(FindAllTodo)}:");
            DateTime now = DateTime.Now.Date;
            var result = new List<InvestorTodoDto>();
            var resultDueDate = new List<InvestorTodoDto>();
            var resultTransaction = new List<InvestorUnfinishedTransactionDto>();
            var investorId = CommonUtils.GetCurrentInvestorId(_httpContextAccessor);
            var cifcode = _cifCodeEFRepository.FindByInvestor(investorId).ThrowIfNull(_dbContext, ErrorCode.CoreCifCodeNotFound);

            #region Tài khoản chưa xác minh
            var investor = _investorRepository.FindById(investorId);
            if (investor.Step < InvestorAppStep.DA_EKYC)
            {
                result.Add(new InvestorTodoDto()
                {
                    InvestorId = investorId,
                    Type = InvestorToDoTypes.TAI_KHOAN_CHUA_XAC_MINH
                });
            }
            #endregion

            #region Khoản đầu tư sắp đến hạn
            //Invest 
            // Lấy toàn bộ danh sách hợp đồng active của cả hệ thống
            var listOrder = _investOrderEFRepository.Entity.Where(o => o.Status == OrderStatus.DANG_DAU_TU && o.CifCode == cifcode.CifCode && o.Deleted == YesNo.NO);
            foreach (var order in listOrder)
            {
                if (order.InvestDate != null)
                {
                    var policy = _investPolicyRepository.FindPolicyById(order.PolicyId);
                    if (policy == null)
                    {
                        continue;
                    }
                    //Lấy kỳ hạn
                    var policyDetailFind = _investPolicyRepository.FindPolicyDetailById(order.PolicyDetailId, order.TradingProviderId, false);
                    if (policyDetailFind == null)
                    {
                        continue;
                    }
                    var distribution = _dbContext.InvestDistributions.FirstOrDefault(r => r.Id == policy.DistributionId && r.Deleted == YesNo.NO);
                    if (distribution == null) continue;
                    //Lấy ngày bắt đầu tính lãi
                    var ngayDauKy = order.InvestDate.Value.Date;

                    //Tính ngày đáo hạn
                    var ngayDaoHan = _investOrderRepository.CalculateDueDate(policyDetailFind, ngayDauKy, distribution.CloseCellDate, false);
                    //Khoảng đến hạn 3 < ngày đến hạn < 7
                    if ((ngayDaoHan.Date - DateTime.Now.Date).Days > policy.ExpirationRenewals && (ngayDaoHan.Date - DateTime.Now.Date).Days <= policy.RemindRenewals)
                    {
                        var resultItem = new InvestorTodoDto()
                        {
                            InvestorId = investorId,
                            Type = InvestorToDoTypes.INVEST_DEN_HAN,
                            OrderId = order.Id,
                        };
                        result.Add(resultItem);
                    }
                }
            }
            #endregion

            #region Giao dịch dang dở
            //Invest
            var orderInvests = _investOrderEFRepository.EntityNoTracking.Where(o => o.CifCode == cifcode.CifCode
                                        && (o.Status == OrderStatus.KHOI_TAO || o.Status == OrderStatus.CHO_THANH_TOAN || o.Status == OrderStatus.CHO_KY_HOP_DONG)
                                        && o.Deleted == YesNo.NO)
                .Select(x => new
                {
                    Order = x,
                    TotalPayment = _dbContext.OrderPayments.AsNoTracking().Where(p => p.OrderId == x.Id && p.TranType == TranTypes.THU
                                        && p.TranClassify == TranClassifies.THANH_TOAN && p.PaymentType == PaymentTypes.CHUYEN_KHOAN
                                        && p.Status == OrderPaymentStatus.DA_THANH_TOAN && p.Deleted == YesNo.NO).Sum(x => x.PaymentAmnount),
                });
            foreach (var order in orderInvests)
            {
                resultTransaction.Add(new InvestorUnfinishedTransactionDto()
                {
                    OrderId = order.Order.Id,
                    Type = InvestorToDoTypes.INVEST_GIAO_DICH_DANG_DO,
                    CreatedDate = order.Order.CreatedDate,
                    TotalPayment = order.TotalPayment,
                });
            }

            //Garner
            var orderGarners = _garnerOrderEFRepository.EntityNoTracking.Where(o => o.CifCode == cifcode.CifCode && (o.Status == OrderStatus.KHOI_TAO || o.Status == OrderStatus.CHO_THANH_TOAN || o.Status == OrderStatus.CHO_KY_HOP_DONG) && o.Deleted == YesNo.NO)
                .Select(x => new
                {
                    Order = x,
                    TotalPayment = _dbContext.RstOrderPayments.AsNoTracking().Where(p => p.OrderId == x.Id && p.TranType == TranTypes.THU
                                        && p.TranClassify == TranClassifies.THANH_TOAN && p.PaymentType == PaymentTypes.CHUYEN_KHOAN
                                        && p.Status == OrderPaymentStatus.DA_THANH_TOAN && p.Deleted == YesNo.NO).Sum(x => x.PaymentAmount),
                });
            foreach (var order in orderGarners)
            {
                resultTransaction.Add(new InvestorUnfinishedTransactionDto()
                {
                    OrderId = order.Order.Id,
                    Type = InvestorToDoTypes.GARNER_GIAO_DICH_DANG_DO,
                    CreatedDate = order.Order.CreatedDate,
                    TotalPayment = order.TotalPayment,
                });
            }

            //Realestate
            var orderRealestatesQuery = _rstOrderEFRepository.EntityNoTracking.Where(o => o.CifCode == cifcode.CifCode && (o.Status == RstOrderStatus.KHOI_TAO || o.Status == RstOrderStatus.CHO_THANH_TOAN_COC || o.Status == RstOrderStatus.CHO_KY_HOP_DONG) && o.Deleted == YesNo.NO);
            //Trường hợp không có thời gian đặt cọc
            var orderRealestates = orderRealestatesQuery.Where(o => o.ExpTimeDeposit == null)
                .Select(x => new
                {
                    Order = x,
                    TotalPayment = _dbContext.RstOrderPayments.AsNoTracking().Where(p => p.OrderId == x.Id && p.TranType == TranTypes.THU
                                        && p.TranClassify == TranClassifies.THANH_TOAN && p.PaymentType == PaymentTypes.CHUYEN_KHOAN
                                        && p.Status == OrderPaymentStatus.DA_THANH_TOAN && p.Deleted == YesNo.NO).Sum(x => x.PaymentAmount),
                });

            foreach (var order in orderRealestates)
            {
                resultTransaction.Add(new InvestorUnfinishedTransactionDto()
                {
                    OrderId = order.Order.Id,
                    Type = InvestorToDoTypes.RST_GIAO_DICH_DANG_DO,
                    CreatedDate = order.Order.CreatedDate,
                    TotalPayment = order.TotalPayment
                });
            }

            //Trường hợp có thời gian đặt cọc
            var orderDeposits = orderRealestatesQuery.Where(e => e.ExpTimeDeposit != null && e.ExpTimeDeposit >= DateTime.Now);
            foreach (var order in orderDeposits)
            {
                resultTransaction.Add(new InvestorUnfinishedTransactionDto()
                {
                    OrderId = order.Id,
                    Type = InvestorToDoTypes.RST_GIAO_DICH_DANG_DO,
                    CreatedDate = order.CreatedDate,
                    TotalPayment = 0,
                });
            }
            var resultItemTransaction = resultTransaction.OrderByDescending(r => r.TotalPayment).ThenByDescending(r => r.CreatedDate).FirstOrDefault(e => e.Type == InvestorToDoTypes.INVEST_GIAO_DICH_DANG_DO || e.Type == InvestorToDoTypes.GARNER_GIAO_DICH_DANG_DO || e.Type == InvestorToDoTypes.RST_GIAO_DICH_DANG_DO);
            if (resultItemTransaction != null)
            {
                result.Add(new InvestorTodoDto
                {
                    InvestorId = investorId,
                    OrderId = resultItemTransaction.OrderId,
                    Type = resultItemTransaction.Type,
                });
            }
            #endregion
            return result;
        }

        public void UpdateStatusToDo(int toDoId)
        {
            _logger.LogInformation($"{nameof(UpdateStatusToDo)}: toDoId = {toDoId}");

            var investorToDo = _investorToDoEFRepository.FindToDoById(toDoId).ThrowIfNull(_dbContext, ErrorCode.InvestorToDoNotFound);
            investorToDo.Status = InvestorTodoStatus.SEEN; // Chuyển trạng thái sang đã xem
            _dbContext.SaveChanges();
        }

        public void UpdateStatusListToDo(List<int> toDoIds)
        {
            _logger.LogInformation($"{nameof(InvestorServices)}->{nameof(UpdateStatusListToDo)}: toDoIds = {toDoIds}");

            foreach (var toDoId in toDoIds)
            {
                var investorToDo = _investorToDoEFRepository.FindToDoById(toDoId).ThrowIfNull(_dbContext, ErrorCode.InvestorToDoNotFound);
                investorToDo.Status = InvestorTodoStatus.SEEN; // Chuyển trạng thái sang đã xem
            }
            _dbContext.SaveChanges();
        }

        public List<int> FindAllTradingProviderIds()
        {
            int investorId = CommonUtils.GetCurrentInvestorId(_httpContextAccessor);
            return _dbContext.InvestorTradingProviders.Where(o => o.InvestorId == investorId)
                .Select(o => o.TradingProviderId)
                .ToList();
        }
    }
}
