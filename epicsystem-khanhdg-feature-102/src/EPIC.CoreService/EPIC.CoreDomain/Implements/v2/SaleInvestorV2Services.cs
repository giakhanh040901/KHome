using EPIC.BondDomain.Implements;
using EPIC.CoreDomain.Interfaces;
using EPIC.CoreDomain.Interfaces.v2;
using EPIC.CoreEntities.Dto.SaleInvestor;
using EPIC.CoreRepositories;
using EPIC.CoreSharedEntities.Dto.Investor;
using EPIC.CoreSharedServices.Implements;
using EPIC.DataAccess.Base;
using EPIC.Entities.DataEntities;
using EPIC.Entities.Dto.ManagerInvestor;
using EPIC.Entities.Dto.SaleInvestor;
using EPIC.FileDomain.Services;
using EPIC.IdentityRepositories;
using EPIC.ImageAPI.Models;
using EPIC.Notification.Services;
using EPIC.Utils;
using EPIC.Utils.ConfigModel;
using EPIC.Utils.DataUtils;
using EPIC.Utils.Recognition.FPT;
using Hangfire;
using Humanizer;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using System.Transactions;

namespace EPIC.CoreDomain.Implements.v2
{
    public class SaleInvestorV2Services : ISaleInvestorV2Services
    {
        private readonly EpicSchemaDbContext _dbContext;
        private readonly ILogger<SaleInvestorV2Services> _logger;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IFileServices _fileServices;
        private readonly NotificationServices _sendEmailServices;
        private readonly IInvestorV2Services _investorV2Services;
        private readonly IInvestorServices _investorServices;
        private readonly IBackgroundJobClient _backgroundJobs;
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
        private readonly SaleEFRepository _saleEFRepository;

        public SaleInvestorV2Services(
            EpicSchemaDbContext dbContext,
            ILogger<SaleInvestorV2Services> logger,
            IHttpContextAccessor httpContext,
            IFileServices imageServices,
            IInvestorServices investorServices,
            IInvestorV2Services investorV2Services,
            IOptions<RecognitionApiConfiguration> recognitionApiConfiguration,
            NotificationServices sendEmailService,
            IBackgroundJobClient backgroundJobs
        )
        {
            _dbContext = dbContext;
            _logger = logger;
            _sendEmailServices = sendEmailService;
            _httpContext = httpContext;
            _fileServices = imageServices;
            _recognitionApiConfig = recognitionApiConfiguration;
            _backgroundJobs = backgroundJobs;
            _investorServices = investorServices;
            _investorV2Services = investorV2Services;
            _ocr = new OCRUtils(_recognitionApiConfig.Value, _logger);
            _investorRepository = new InvestorEFRepository(dbContext, logger);
            _sysVarRepository = new SysVarEFRepository(dbContext);
            _userRepository = new UsersEFRepository(dbContext, logger);
            _authOtpRepository = new AuthOtpEFRepository(dbContext, logger);
            _cifCodeRepository = new CifCodeEFRepository(dbContext, logger);
            _businessCustomerRepository = new BusinessCustomerEFRepository(dbContext, logger);
            _saleRepository = new SaleEFRepository(dbContext, logger);
            _investorSaleRepository = new InvestorSaleEFRepository(dbContext, logger);
            _saleEFRepository = new SaleEFRepository(dbContext, logger);
        }

        public async Task AddBank(CreateBankDto dto)
        {
            _logger.LogInformation($"{System.Reflection.MethodBase.GetCurrentMethod().Name}: {JsonSerializer.Serialize(dto)}");

            var investor = CheckInvestorTemp(dto.InvestorId);

            var listContactAddress = _investorRepository.GetListContactAddress(investor.InvestorId);
            if (listContactAddress?.Count() == 0)
            {
                _investorRepository.ThrowException(ErrorCode.InvestorContactAddressNotFound);
            }

            // Check trùng bank, stk
            var anySameBank = _investorRepository.AnyBankAccount(dto.BankId, dto.BankAccount);
            if (anySameBank)
            {
                _investorRepository.ThrowException(ErrorCode.InvestorBankIsRegistered);
            }

            string username = CommonUtils.GetCurrentUsername(_httpContext);

            using (var tran = _dbContext.Database.BeginTransaction())
            {
                _investorRepository.AddBankAccountWithDefault(new InvestorBankAccount
                {
                    BankId = dto.BankId,
                    InvestorId = dto.InvestorId,
                    InvestorGroupId = investor?.InvestorGroupId ?? 0,
                    IsDefault = YesNo.YES,
                    CreatedBy = username,
                    OwnerAccount = dto.OwnerAccount,
                    BankAccount = dto.BankAccount,
                });

                investor.Step = InvestorAppStep.DA_ADD_BANK;
                investor.FinalStepDate = DateTime.Now;

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

                _dbContext.SaveChanges();
                tran.Commit();
            }

            string phone = investor?.Phone;

            //Gửi email thông báo xác minh thành công
            await _sendEmailServices.SendEmailVerificationAccountSuccess(phone);
            await _sendEmailServices.SendNotifyEnterReferralWhenRegister(phone);
        }

        public void AddContactAddress(CreateContactAddressDto dto)
        {
            _logger.LogInformation($"{System.Reflection.MethodBase.GetCurrentMethod().Name}: {JsonSerializer.Serialize(dto)}");

            var investor = CheckInvestorTemp(dto.InvestorId);

            var defaultIden = _investorRepository.GetDefaultIdentification(investor.InvestorId);
            if (defaultIden.EkycInfoIsConfirmed == YesNo.NO)
            {
                _investorRepository.ThrowException(ErrorCode.InvestorEKYCNotFound);
            }

            string username = CommonUtils.GetCurrentUsername(_httpContext);

            _investorRepository.AddContactAddressAutoDefault(new Entities.DataEntities.InvestorContactAddress
            {
                ContactAddress = dto.ContactAddress,
                ProvinceCode = dto.ProvinceCode,
                DistrictCode = dto.DistrictCode,
                WardCode = dto.WardCode,
                DetailAddress = dto.DetailAddress,
                IsDefault = dto.IsDefault,
                InvestorGroupId = investor.InvestorGroupId,
                InvestorId = investor.InvestorId,
                CreatedBy = username,
            });

            _dbContext.SaveChanges();

        }

        public void ConfirmAndUpdateEkyc(SaleInvestorConfirmUpdateDto dto)
        {
            _logger.LogInformation($"{System.Reflection.MethodBase.GetCurrentMethod().Name}: {JsonSerializer.Serialize(dto)}");

            var investor = CheckInvestorTemp(dto.InvestorId);
            var defaultIden = _investorRepository.GetDefaultIdentification(investor.InvestorId);

            using (var tran = _dbContext.Database.BeginTransaction())
            {
                investor.ThirdStepDate = DateTime.Now;
                defaultIden.Fullname = dto.Name;
                defaultIden.DateOfBirth = dto.BirthDate;
                defaultIden.Sex = dto.Sex;
                defaultIden.IdNo = dto.IdNo;
                defaultIden.IdDate = dto.IdDate;
                defaultIden.IdExpiredDate = dto.IdExpiredDate;
                defaultIden.IdIssuer = dto.IdIssuer;
                defaultIden.PlaceOfOrigin = dto.PlaceOfOrigin;
                defaultIden.PlaceOfResidence = dto.PlaceOfResidence;
                defaultIden.EkycInfoIsConfirmed = YesNo.YES;
                defaultIden.IsVerifiedIdentification = YesNo.YES;
                defaultIden.Nationality = dto.Nationality;
                defaultIden.Status = InvestorStatus.ACTIVE;
                investor.Step = InvestorAppStep.DA_EKYC;
                
                _dbContext.SaveChanges();
                tran.Commit();
            }
        }

        public IEnumerable<SaleInvestorInfoDto> GetAllListInvestor()
        {
            var saleId = CommonUtils.GetCurrentSaleId(_httpContext);

            var investorQuery = from investorSale in _dbContext.InvestorSales
                               join investor in _dbContext.Investors on investorSale.InvestorId equals investor.InvestorId
                               where investorSale.Deleted == YesNo.NO && investor.Deleted == YesNo.NO
                               && investor.Step > InvestorAppStep.DA_DANG_KY
                               && investorSale.SaleId == saleId && investor.Status != InvestorStatus.TEMP
                               && investorSale.Id == _dbContext.InvestorSales.FirstOrDefault(r => r.SaleId == saleId && r.InvestorId == investor.InvestorId).Id
                               select new
                               {
                                   investorSale.Id,
                                   investor.InvestorId,
                                   investor.Phone,
                                   investor.AvatarImageUrl,
                                   investorSale.CreatedDate,
                                   investor.Status,
                                   FullName = _dbContext.InvestorIdentifications
                                                .Where(ii => ii.InvestorId == investor.InvestorId && ii.Status != InvestorStatus.TEMP && ii.Deleted == YesNo.NO)
                                                .OrderByDescending(ii => ii.IsDefault)
                                                .ThenByDescending(ii => ii.Id)
                                                .Select(ii => ii.Fullname)
                                                .FirstOrDefault()
                               };

            return investorQuery.OrderByDescending(o => o.Id)
                .Select(o => new SaleInvestorInfoDto 
                {
                    InvestorId = o.InvestorId,
                    Phone = o.Phone,
                    FullName = o.FullName,
                    AvatarImageUrl = o.AvatarImageUrl,
                    Status = o.Status,
                    CreatedDate = o.CreatedDate
                });
        }

        public ResultAddInvestorDto RegisterInvestor(SaleRegisterInvestorDto dto)
        {
            _logger.LogInformation($"{System.Reflection.MethodBase.GetCurrentMethod().Name}: {JsonSerializer.Serialize(dto)}");
            
            var investorId = CommonUtils.GetCurrentInvestorId(_httpContext);
            var username = CommonUtils.GetCurrentUsername(_httpContext);

            string password = CommonUtils.CreateMD5("123456Aa@");

            var isActive = _investorRepository.IsInvestorActive(investorId);
            if (!isActive)
            {
                _investorRepository.ThrowException(ErrorCode.InvestorDeactive);
            }

            _investorV2Services.RegisterInvestor(new CoreSharedEntities.Dto.Investor.RegisterInvestorDto
            {
                Email = dto.Email,
                Phone = dto.Phone,
                ReferralCode = dto.ReferralCode,
                Password = password
            }, InvestorSource.SALE);

            var investor = _investorRepository.GetByEmailOrPhone(dto.Phone);

            return new ResultAddInvestorDto
            {
                InvestorId = investor?.InvestorId ?? 0
            };

        }

        public async Task<EKYCOcrResultDto> UpdateEkycIdentification(EkycSaleInvestorDto input)
        {
            _logger.LogInformation($"{System.Reflection.MethodBase.GetCurrentMethod().Name}: {JsonSerializer.Serialize(input)}");

            EKYCOcrResultDto resultOcr = new();

            string frontImageUrl = "";
            string backImageUrl = "";

            bool isSuccess = false;

            if (input.Type == CardTypesInput.CCCD || input.Type == CardTypesInput.CMND)
            {
                //front id
                OCRFrontIdDataNewType frontData = await _ocr.ReadFrontIdDataNewType(input.FrontImage);

                frontImageUrl = _fileServices.UploadFileID(new ImageAPI.Models.UploadFileModel
                {
                    File = input.FrontImage,
                    Folder = FileFolder.INVESTOR,
                });

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
                    Nationality = RecognitionUtils.GetValueStandard(frontData.Nationality)
                };

                _ocr.CheckAge(resultOcr.DateOfBirth);
                _ocr.CheckExp(resultOcr.IdIssueExpDate);

                backImageUrl = _fileServices.UploadFileID(new UploadFileModel
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

                frontImageUrl = _fileServices.UploadFileID(new UploadFileModel
                {
                    File = input.FrontImage,
                    Folder = FileFolder.INVESTOR,
                });

                isSuccess = true;
            }

            if (isSuccess)
            {
                var investor = _investorRepository.FindById(input.InvestorId);

                var invTempByPhone = _investorRepository.GetInvestorTempByPhone(investor?.Phone);
                if (invTempByPhone != null)
                {
                    _investorRepository.ThrowException(ErrorCode.InvestorPhoneExisted);
                }

                _investorV2Services.UpdateEkycId(investor.InvestorId, new UpdateEkycIdDto
                {
                    FrontImage = frontImageUrl,
                    Name = resultOcr.Name,
                    BackImage = backImageUrl,
                    IdNo = resultOcr.IdNo,
                    IdType = input.Type?.ToUpper(),
                    Sex = resultOcr.Sex,
                    DateOfBirth = resultOcr.DateOfBirth,
                    PlaceOfOrigin = resultOcr.PlaceOfOrigin,
                    PlaceOfResidence = resultOcr.PlaceOfResidence,
                    Issuer = resultOcr.IdIssuer,
                    IssueExpDate = resultOcr.IdIssueExpDate,
                    IssueDate = resultOcr.IdIssueDate,
                    Nationality = resultOcr.Nationality,
                }, true);

            }

            return resultOcr;
        }

        public string UploadAvatar(SaleInvestorUploadAvatarDto dto)
        {
            _logger.LogInformation($"{System.Reflection.MethodBase.GetCurrentMethod().Name}: {JsonSerializer.Serialize(dto)}");

            string fileUrl = _fileServices.UploadFileID(new UploadFileModel
            {
                Folder = FileFolder.INVESTOR,
                File = dto.Image
            });

            var investor = _investorRepository.FindById(dto.InvestorId);
            investor.AvatarImageUrl = fileUrl;

            _dbContext.SaveChanges();

            return fileUrl;
        }

        public void UploadProfFile(SaleInvestorUploadProfFileDto dto)
        {
            _logger.LogInformation($"{System.Reflection.MethodBase.GetCurrentMethod().Name}: {JsonSerializer.Serialize(dto)}");

            string username = CommonUtils.GetCurrentUsername(_httpContext);
            int userId = CommonUtils.GetCurrentUserId(_httpContext);

            _investorServices.UploadProfFileCommon(new UploadProfFileDto
            {
                ProfFile = dto.ProfFile
            }, dto.InvestorId, username, userId);
        }

        private Investor CheckInvestorTemp(int investorId)
        {
            _logger.LogInformation($"{System.Reflection.MethodBase.GetCurrentMethod().Name}: investorID = {investorId}");

            var investor = _investorRepository.FindById(investorId);
            if (investor == null)
            {
                _investorRepository.ThrowException(ErrorCode.InvestorNotFound);
            }

            if (investor.Status != InvestorStatus.TEMP)
            {
                _investorRepository.ThrowException(ErrorCode.InvestorStatusNotT);
            }

            return investor;
        }
    }
}
