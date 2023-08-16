using AutoMapper;
using DocumentFormat.OpenXml.Spreadsheet;
using EPIC.BondRepositories;
using EPIC.CoreDomain.Interfaces;
using EPIC.CoreEntities.Dto.ManagerInvestor;
using EPIC.CoreRepositories;
using EPIC.DataAccess.Models;
using EPIC.Entities;
using EPIC.Entities.DataEntities;
using EPIC.Entities.Dto.CoreApprove;
using EPIC.Entities.Dto.CoreHistoryUpdate;
//using EPIC.Entities.Dto.Investor;
using EPIC.Entities.Dto.ManagerInvestor;
using EPIC.Entities.Dto.Notification.ResetPassword;
using EPIC.Entities.Dto.RocketChat;
using EPIC.Entities.Dto.User;
using EPIC.FileDomain.Services;
using EPIC.IdentityEntities.DataEntities;
using EPIC.IdentityRepositories;
using EPIC.RocketchatDomain.Interfaces;
using EPIC.Notification.Services;
using EPIC.Utils;
using EPIC.Utils.ConfigModel;
using EPIC.Utils.ConstantVariables.Identity;
using EPIC.Utils.DataUtils;
using EPIC.Utils.Recognition.FPT;
using EPIC.Utils.Security;
using EPIC.Utils.SharedApiService;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.ServiceModel;
using System.Text.Json;
using System.Threading.Tasks;
using System.Transactions;
using EPIC.Entities.Dto.BusinessCustomer;
using EPIC.DataAccess.Base;
using EPIC.CoreSharedEntities.Dto.Investor;
using MySqlX.XDevAPI.Common;
using Humanizer;
using EPIC.LoyaltyRepositories;
using EPIC.Utils.Linq;
using DocumentFormat.OpenXml.Bibliography;
//using EPIC.CoreEntities.Dto.Investor;

namespace EPIC.CoreDomain.Implements
{
    public class ManagerInvestorServices : IManagerInvestorServices
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;
        private readonly IHttpContextAccessor _httpContext;
        private readonly OCRUtils _ocr;
        private readonly ManagerInvestorRepository _managerInvestorRepository;
        private readonly BusinessCustomerRepository _businessCustomerRepository;
        private readonly BusinessCustomerEFRepository _businessCustomerEFRepository;
        private readonly RecognitionApiConfiguration _recognitionApiConfig;
        private readonly ApproveRepository _approveRepository;
        private readonly InvestorRepository _investorRepository;
        private readonly InvestorEFRepository _investorEFRepository;
        private readonly UserRepository _userRepository;
        private readonly UsersEFRepository _usersEFRepository;
        private readonly SaleRepository _saleRepository;
        private readonly CoreHistoryUpdateRepository _coreHistoryUpdateRepository;
        private readonly LoyRankEFRepoistory _loyRankEFRepository;
        private readonly LoyPointInvestorEFRepoistory _loyPointInvestorEFRepository;
        private readonly IMapper _mapper;
        private readonly IFileServices _imageServices;
        private readonly SharedNotificationApiUtils _sharedEmailApiUtils;
        private readonly NotificationServices _sendEmailServices;
        private readonly IRocketChatServices _rocketChatServices;
        private readonly EpicSchemaDbContext _dbContext;

        public ManagerInvestorServices(
            ILogger<ManagerInvestorServices> logger,
            IConfiguration configuration,
            DatabaseOptions databaseOptions,
            SharedNotificationApiUtils sharedEmailApiUtils,
            EpicSchemaDbContext dbContext,
            IOptions<RecognitionApiConfiguration> recognitionApiConfiguration,
            IHttpContextAccessor httpContext,
            IFileServices imageServices,
            IMapper mapper,
            IRocketChatServices rocketChatServices,
            NotificationServices sendEmailServices)
        {
            _logger = logger;
            _configuration = configuration;
            _connectionString = databaseOptions.ConnectionString;
            _httpContext = httpContext;
            _dbContext = dbContext;
            _recognitionApiConfig = recognitionApiConfiguration.Value;
            _sharedEmailApiUtils = sharedEmailApiUtils;
            _managerInvestorRepository = new ManagerInvestorRepository(_connectionString, _logger, _httpContext);
            _businessCustomerRepository = new BusinessCustomerRepository(_connectionString, _logger);
            _approveRepository = new ApproveRepository(_connectionString, _logger);
            _investorRepository = new InvestorRepository(_connectionString, _logger);
            _investorEFRepository = new InvestorEFRepository(dbContext, _logger);
            _businessCustomerEFRepository = new BusinessCustomerEFRepository(dbContext, _logger);
            _usersEFRepository = new UsersEFRepository(dbContext, _logger);
            _userRepository = new UserRepository(_connectionString, _logger);
            _saleRepository = new SaleRepository(_connectionString, _logger);
            _coreHistoryUpdateRepository = new CoreHistoryUpdateRepository(_connectionString, _logger);
            _coreHistoryUpdateRepository = new CoreHistoryUpdateRepository(_connectionString, _logger);
            _loyRankEFRepository = new LoyRankEFRepoistory(dbContext, _logger);
            _loyPointInvestorEFRepository = new LoyPointInvestorEFRepoistory(dbContext, logger);
            _imageServices = imageServices;
            _mapper = mapper;
            _ocr = new OCRUtils(_recognitionApiConfig, _logger);
            _sendEmailServices = sendEmailServices;
            _rocketChatServices = rocketChatServices;
        }

        public List<InvestorDto> FindAllList(string keyword)
        {
            return _managerInvestorRepository.FindAllList(keyword);
        }

        /// <summary>
        /// Tao investor tam
        /// </summary>
        /// <param name="dto"></param>
        public InvestorTemporary CreateInvestorTemporary(CreateManagerInvestorEkycDto dto)
        {
            _ocr.CheckAge(dto.DateOfBirth);
            _ocr.CheckExp(dto.IdExpiredDate);
            string username = CommonUtils.GetCurrentUsername(_httpContext);
            int? tradingProviderId = null;

            try
            {
                tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            }
            catch (Exception)
            {
                tradingProviderId = null;
            }

            if (!string.IsNullOrEmpty(dto.RepresentativePhone))
            {
                dto.Phone = null;
                dto.Email = null;
            }

            var investorTemp = _managerInvestorRepository.CreateInvestorTemporary(dto, username, tradingProviderId);

            return investorTemp;
        }

        /// <summary>
        /// Detail Investor
        /// </summary>
        /// <param name="investorId"></param>
        /// <param name="investorGroupId"></param>
        /// <returns></returns>
        public ViewManagerInvestorTemporaryDto FindById(int investorId, bool isTemp, OptionFindDto options)
        {
            var investorDb = _managerInvestorRepository.FindById(investorId, isTemp);
            var result = _mapper.Map<ViewManagerInvestorTemporaryDto>(investorDb);

            //var test = _investorEFRepository.GetDefaultStock(investorId);

            if (options == null)
            {
                options = new OptionFindDto
                {
                    IsNeedDefaultIdentification = true,
                    IsNeedListBank = true,
                    IsNeedDefaultBank = true,
                    IsNeedDefaultAddress = true,
                };
            }

            AssignAddtionalValueToInvestor(result, isTemp, options);

            // Lấy thông tin người giới thiệu
            if (options.IsNeedReferralInvestor && !string.IsNullOrEmpty(result.ReferralCode))
            {
                int? tradingProviderId = null;
                var userType = CommonUtils.GetCurrentUserType(_httpContext);

                //if (new string[] {UserData.EPIC, UserData.ROOT_EPIC }.Contains(userType))
                //{
                //    tradingProviderId = null;
                //}
                //else
                //{
                //    try
                //    {
                //        tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
                //    }
                //    catch (Exception)
                //    {
                //        tradingProviderId = -1;
                //    }
                //}

                try
                {
                    var referralInvestor = _managerInvestorRepository.FindByReferralCodeSelf(result.ReferralCode, tradingProviderId);

                    if (referralInvestor != null)
                    {
                        result.ReferralInvestor = _mapper.Map<ViewManagerInvestorTemporaryDto>(referralInvestor);

                        AssignAddtionalValueToInvestor(result.ReferralInvestor, false, new OptionFindDto
                        {
                            IsNeedDefaultIdentification = true
                        });
                    }
                    else
                    {
                        var referralBusinessCustomer = _businessCustomerEFRepository.FindByReferralCodeSelf(result.ReferralCode);

                        if (tradingProviderId == null || _businessCustomerEFRepository.IsInTradingProvider(referralBusinessCustomer.BusinessCustomerId ?? 0, tradingProviderId ?? 0))
                        {
                            result.ReferralBusinessCustomer = _businessCustomerEFRepository.FindById(referralBusinessCustomer.BusinessCustomerId ?? 0);
                        }
                    }
                }
                catch (Exception)
                {

                }
            }

            return result;
        }

        /// <summary>
        /// Lấy investor theo group id
        /// </summary>
        /// <param name="investorGroupId"></param>
        /// <returns></returns>
        public ViewManagerInvestorTemporaryDto FindByGroupId(int investorGroupId)
        {
            var investorDb = _managerInvestorRepository.FindByInvestorGroupId(investorGroupId);
            var result = _mapper.Map<ViewManagerInvestorTemporaryDto>(investorDb);

            AssignAddtionalValueToInvestor(result, false, new OptionFindDto
            {
                IsNeedListAddress = true,
                IsNeedListBank = true,
            });
            return result;
        }

        /// <summary>
        /// Trinh duyet
        /// </summary>
        /// <param name="dto"></param>
        public void CreateApproveRequest(RequestApproveDto dto)
        {
            using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                int userid = CommonUtils.GetCurrentUserId(_httpContext);
                int? tradingProviderId = null;
                var userType = CommonUtils.GetCurrentUserType(_httpContext);
                if (userType != UserTypes.EPIC && userType != UserTypes.ROOT_EPIC)
                {
                    tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
                }

                var approve = new CreateApproveRequestDto
                {
                    ActionType = dto.Action,
                    DataType = CoreApproveDataType.INVESTOR,
                    RequestNote = dto.Notice,
                    UserApproveId = dto.UserApproveId,
                    UserRequestId = userid,
                    ReferIdTemp = dto.InvestorId,
                    Summary = dto.Summary,
                    ApproveRequestFileUrl = dto.ApproveRequestFileUrl,
                };

                _managerInvestorRepository.CheckPhoneAndEmailTemp(dto.InvestorId);

                // Trình duyệt trong trường hợp sửa. Sẽ có id thật để lưu
                if (dto.Action == ActionTypes.CAP_NHAT)
                {
                    var investorActual = _managerInvestorRepository.GetActualInvestorByTemp(dto.InvestorId);
                    approve.ReferId = investorActual.InvestorId;
                }

                var coreApprove = _approveRepository.CreateApproveRequest(approve, tradingProviderId);
                if (approve.ReferId != 0)
                {
                    var listHistoryUpdates = new List<CoreHistoryUpdate>();
                    #region check update investor
                    var investor = _managerInvestorRepository.FindById(approve.ReferId, false);
                    var investorTemp = _managerInvestorRepository.FindById(approve.ReferIdTemp, true);
                    var investorCheck = _mapper.Map<HistoryUpdateInvestorDto>(investor);
                    var investorTempCheck = _mapper.Map<HistoryUpdateInvestorDto>(investorTemp);
                    if (investorCheck != null && investorTempCheck != null)
                    {
                        var differences = CompareObject.DetailedCompare(investorCheck, investorTempCheck);
                        foreach (var dis in differences)
                        {
                            var historyUpdateDto = new CoreHistoryUpdate()
                            {
                                ApproveId = coreApprove.ApproveID,
                                RealTableId = approve.ReferId,
                                OldValue = dis.OldName?.ToString(),
                                NewValue = dis.NewValue?.ToString(),
                                FieldName = dis.FieldName,
                                CreatedBy = approve?.UserRequestId.ToString(),
                                UpdateTable = CoreHistoryUpdateTable.INVESTOR,
                            };
                            listHistoryUpdates.Add(historyUpdateDto);
                        }
                    }
                    #endregion
                    #region check update identification
                    var identificationTemps = _managerInvestorRepository.GetListIdentification(dto.InvestorId, dto.InvestorGroupId, true);
                    if (identificationTemps != null)
                    {
                        foreach (var idenTemp in identificationTemps)
                        {
                            if (idenTemp.ReferId != null)
                            {
                                var iden = _managerInvestorRepository.GetIdentificationById(idenTemp.ReferId ?? 0);
                                if (iden != null)
                                {
                                    var idenCheck = _mapper.Map<HistoryUpdateIdentificationsDto>(iden);
                                    var idenTempCheck = _mapper.Map<HistoryUpdateIdentificationsDto>(idenTemp);
                                    if (idenCheck != null && idenTempCheck != null)
                                    {
                                        var differences = CompareObject.DetailedCompare(idenCheck, idenTempCheck);
                                        foreach (var dis in differences)
                                        {
                                            var historyUpdateDto = new CoreHistoryUpdate()
                                            {
                                                ApproveId = coreApprove.ApproveID,
                                                RealTableId = idenTemp.ReferId ?? 0,
                                                OldValue = dis.OldName?.ToString(),
                                                NewValue = dis.NewValue?.ToString(),
                                                FieldName = dis.FieldName,
                                                CreatedBy = approve?.UserRequestId.ToString(),
                                                UpdateTable = CoreHistoryUpdateTable.INVESTOR_IDENTIFICATION,
                                            };
                                            listHistoryUpdates.Add(historyUpdateDto);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    #endregion

                    #region check update bank
                    //var idenBankTemps = _managerInvestorRepository.GetListBank(dto.InvestorId, dto.InvestorGroupId, true);
                    var idenBankTemps = _investorEFRepository.GetListBankAccTemp(dto.InvestorId)?.ToList();
                    if (idenBankTemps != null)
                    {
                        foreach (var bankTemp in idenBankTemps)
                        {
                            if (bankTemp.ReferId != null)
                            {
                                var bank = _managerInvestorRepository.GetBankById(bankTemp.ReferId ?? 0);
                                if (bank != null)
                                {
                                    var bankCheck = _mapper.Map<HistoryUpdateBankDto>(bank);
                                    var bankTempCheck = _mapper.Map<HistoryUpdateBankDto>(bankTemp);
                                    if (bankCheck != null && bankTempCheck != null)
                                    {
                                        var differences = CompareObject.DetailedCompare(bankCheck, bankTempCheck);
                                        foreach (var dis in differences)
                                        {
                                            var historyUpdateDto = new CoreHistoryUpdate()
                                            {
                                                ApproveId = coreApprove.ApproveID,
                                                RealTableId = bankTemp.ReferId ?? 0,
                                                OldValue = dis.OldName?.ToString(),
                                                NewValue = dis.NewValue?.ToString(),
                                                FieldName = dis.FieldName,
                                                CreatedBy = approve?.UserRequestId.ToString(),
                                                UpdateTable = CoreHistoryUpdateTable.INVESTOR_BANK,
                                            };
                                            listHistoryUpdates.Add(historyUpdateDto);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    #endregion

                    #region check update address
                    var ContactAddressTemps = _managerInvestorRepository.GetListContactAddress(-1, 0, "", dto.InvestorId, true);
                    if (ContactAddressTemps != null)
                    {
                        foreach (var contactAddressTemp in ContactAddressTemps.Items)
                        {
                            if (contactAddressTemp.ReferId != null)
                            {
                                var contactAddress = _managerInvestorRepository.GetContactdAddressById(contactAddressTemp.ReferId ?? 0);
                                if (contactAddress != null)
                                {
                                    var contactAddressCheck = _mapper.Map<HistoryUpdateContactAddressDto>(contactAddress);
                                    var contactAddressTempCheck = _mapper.Map<HistoryUpdateContactAddressDto>(contactAddressTemp);
                                    if (contactAddressCheck != null && contactAddressTempCheck != null)
                                    {
                                        var differences = CompareObject.DetailedCompare(contactAddressCheck, contactAddressTempCheck);
                                        foreach (var dis in differences)
                                        {
                                            var historyUpdateDto = new CoreHistoryUpdate()
                                            {
                                                ApproveId = coreApprove.ApproveID,
                                                RealTableId = contactAddressTemp.ReferId ?? 0,
                                                OldValue = dis.OldName?.ToString(),
                                                NewValue = dis.NewValue?.ToString(),
                                                FieldName = dis.FieldName,
                                                CreatedBy = approve?.UserRequestId.ToString(),
                                                UpdateTable = CoreHistoryUpdateTable.INVESTOR_CONTACT_ADDRESS,
                                            };
                                            listHistoryUpdates.Add(historyUpdateDto);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    #endregion

                    #region chứng khoán
                    var stockTemps = _managerInvestorRepository.GetListStockNoPaging(new FindInvestorStockDto { InvestorId = dto.InvestorId, InvestorGroupId = dto.InvestorGroupId, IsTemp = true });
                    if (stockTemps != null)
                    {
                        foreach (var stockTemp in stockTemps)
                        {
                            if (stockTemp.ReferId != null)
                            {
                                var stock = _managerInvestorRepository.GetStockByStockId(stockTemp.ReferId ?? 0, false);
                                if (stock != null)
                                {
                                    var stockCheck = _mapper.Map<HistoryUpdateStockDto>(stock);
                                    var stockTempCheck = _mapper.Map<HistoryUpdateStockDto>(stockTemp);
                                    if (stockCheck != null && stockTempCheck != null)
                                    {
                                        var differences = CompareObject.DetailedCompare(stockCheck, stockTempCheck);
                                        foreach (var dis in differences)
                                        {
                                            var historyUpdateDto = new CoreHistoryUpdate()
                                            {
                                                ApproveId = coreApprove.ApproveID,
                                                RealTableId = stockTemp.ReferId ?? 0,
                                                OldValue = dis.OldName?.ToString(),
                                                NewValue = dis.NewValue?.ToString(),
                                                FieldName = dis.FieldName,
                                                CreatedBy = approve?.UserRequestId.ToString(),
                                                UpdateTable = CoreHistoryUpdateTable.INVESTOR_STOCK,
                                            };
                                            listHistoryUpdates.Add(historyUpdateDto);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    #endregion

                    foreach (var historyUpdate in listHistoryUpdates)
                    {
                        _coreHistoryUpdateRepository.Add(historyUpdate);
                    }
                }
                transaction.Complete();
            }
            _managerInvestorRepository.CloseConnection();
        }

        /// <summary>
        /// Tạo user
        /// </summary>
        /// <param name="dto"></param>
        public void CreateUser(CreateUserByInvestorDto dto)
        {
            string username = CommonUtils.GetCurrentUsername(_httpContext);
            dto.Password = CommonUtils.CreateMD5(dto.Password);

            _managerInvestorRepository.CreateUser(dto, username);

        }

        /// <summary>
        /// Duyet
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task Approve(ApproveManagerInvestorDto input)
        {
            var userId = CommonUtils.GetCurrentUserId(_httpContext);

            using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                // XU LY INVESTOR
                var approve = _approveRepository.GetOneByTemp(input.InvestorId, CoreApproveDataType.INVESTOR);
                var investorActual = new Investor();

                if (approve.ActionType == ActionTypes.THEM_MOI)
                {
                    string plainPassword = GenerateCodes.GetRandomPassword();
                    string password = CommonUtils.CreateMD5(plainPassword);
                    investorActual = _managerInvestorRepository.ApproveAddInvestor(input, password);
                }
                else if (approve.ActionType == ActionTypes.CAP_NHAT)
                {
                    investorActual = _managerInvestorRepository.ApproveUpdateInvestor(input);
                }

                // XU LY BANG APPROVE
                var approveRequest = new ApproveRequestDto
                {
                    ApproveID = approve.ApproveID,
                    ApproveNote = input.Notice,
                    UserApproveId = userId
                };

                // Duyệt lúc thêm mới phải lưu cả id thật. Vì khi trình duyệt, ko có id thật để lưu.
                if (approve.ActionType == ActionTypes.THEM_MOI)
                {
                    approveRequest.ReferId = investorActual.InvestorId;

                    var user = _userRepository.FindByInvestorId(investorActual.InvestorId);
                    if (user != null)
                    {
                        // tạo user rocketchat
                        await _rocketChatServices.CreateRocketchatUserForInvestor(new CreateRocketChatUserDto
                        {
                            email = investorActual.Email,
                            name = user.DisplayName ?? user.UserName,
                            password = _rocketChatServices.genPasswordByUser(user),
                            username = user.UserName,
                            roles = new List<string> { "user" },
                        });
                    }
                }

                _approveRepository.ApproveRequestData(approveRequest);

                // Commit
                transaction.Complete();
            }

            _managerInvestorRepository.CloseConnection();
        }

        /// <summary>
        /// Xac minh
        /// </summary>
        /// <param name="input"></param>
        public void Check(ApproveManagerInvestorDto input)
        {
            var userid = CommonUtils.GetCurrentUserId(_httpContext);
            var approve = _approveRepository.GetOneByActual(input.InvestorId, CoreApproveDataType.INVESTOR);

            _managerInvestorRepository.EpicCheck(input.InvestorId, approve?.ApproveID ?? 0, userid);
        }

        /// <summary>
        /// Huy
        /// </summary>
        public void Cancel(CancelRequestManagerInvestorDto input)
        {
            var approve = _approveRepository.GetOneByTemp(input.InvestorIdTemp, CoreApproveDataType.INVESTOR);
            var userId = CommonUtils.GetCurrentUserId(_httpContext);

            _approveRepository.OpenConnection();

            using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                _managerInvestorRepository.CancelRequestInvestor(input);

                _approveRepository.CancelRequest(new CancelRequestDto
                {
                    ApproveID = approve.ApproveID,
                    CancelNote = input.Notice,
                    UserApproveId = userId
                });

                transaction.Complete();
            }

            _approveRepository.CloseConnection();

        }

        /// <summary>
        /// Ekyc thong tin giay to tuy than
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<EKYCOcrResultDto> EkycOCRAsync(EkycManagerInvestorDto input)
        {
            EKYCOcrResultDto resultOcr = new();

            string nationality = "Việt Nam";

            if (input.Type == CardTypesInput.CCCD || input.Type == CardTypesInput.CMND)
            {
                //front id
                OCRFrontIdDataNewType frontData = await _ocr.ReadFrontIdDataNewType(input.FrontImage);

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
                    Nationality = nationality
                };

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
                    Nationality = nationality,
                    IdIssuer = passportData.IdIssuer,
                };

                _ocr.CheckAge(resultOcr.DateOfBirth);
                _ocr.CheckExp(resultOcr.IdIssueExpDate);
            }
            return resultOcr;
        }

        /// <summary>
        /// Tao giay to temporary
        /// </summary>
        /// <param name="dto"></param>
        public int CreateIdentificationTemporary(CreateIdentificationTemporaryDto dto)
        {
            string username = CommonUtils.GetCurrentUsername(_httpContext);

            return _managerInvestorRepository.CreateIdentificationTemporary(dto, username);
        }

        /// <summary>
        /// Thay thế giấy tờ
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public int ReplaceIdentification(ReplaceIdentificationDto dto)
        {
            string username = CommonUtils.GetCurrentUsername(_httpContext);

            return _managerInvestorRepository.ReplaceIdentification(dto, username);
        }

        /// <summary>
        /// Cập nhật liên kết ngân hàng
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public int UpdateInvestorBankAcc(UpdateInvestorBankAccDto dto)
        {
            string username = CommonUtils.GetCurrentUsername(_httpContext);

            return _managerInvestorRepository.UpdateInvestorBankAcc(dto, username);
        }

        /// <summary>
        /// Lấy list investor temp để trình duyệt
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageNumber"></param>
        /// <param name="isNoPaging"></param>
        /// <param name="keyword"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public PagingResult<ViewManagerInvestorTemporaryDto> GetListToRequest(FindListInvestorDto dto)
        {
            var usertype = CommonUtils.GetCurrentUserType(_httpContext);

            if (usertype == UserTypes.TRADING_PROVIDER || usertype == UserTypes.ROOT_TRADING_PROVIDER)
            {
                var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
                dto.TradingProviderId = tradingProviderId.ToString();
            }

            var query = _managerInvestorRepository.GetListToRequest(dto, usertype);

            PagingResult<ViewManagerInvestorTemporaryDto> result = new PagingResult<ViewManagerInvestorTemporaryDto>
            {
                TotalItems = query.TotalItems,
                Items = new List<ViewManagerInvestorTemporaryDto>() { }
            };

            var items = new List<ViewManagerInvestorTemporaryDto>() { };

            if (result.TotalItems > 0)
            {
                var listIds = query.Items.Select(x => $"{x.InvestorId}");

                var listDefaultIdentification = _managerInvestorRepository.GetListDefaultIdentification(listIds, true);

                foreach (var investor in query.Items)
                {
                    var tmpInvestor = _mapper.Map<ViewManagerInvestorTemporaryDto>(investor);

                    tmpInvestor.Phone = StringUtils.HidePhone(tmpInvestor.Phone);
                    tmpInvestor.Email = StringUtils.HideEmail(tmpInvestor.Email);

                    var defaultIden = listDefaultIdentification?.FirstOrDefault(x => x.InvestorId == investor.InvestorId);

                    tmpInvestor.DefaultIdentification = _mapper.Map<ViewIdentificationDto>(defaultIden);

                    AssignAdditionalValueToInvestorInListTemp(tmpInvestor);

                    items.Add(tmpInvestor);
                }
            }

            result.Items = items;

            return result;
        }

        public PagingResult<ViewManagerInvestorTemporaryDto> GetListToRequests(FindListInvestorDto dto)
        {
            var usertype = CommonUtils.GetCurrentUserType(_httpContext);

            if (usertype == UserTypes.TRADING_PROVIDER || usertype == UserTypes.ROOT_TRADING_PROVIDER)
            {
                var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
                dto.TradingProviderId = tradingProviderId.ToString();
            }

            var query = _investorEFRepository.FindAllInvestorTemp(dto, usertype);

            PagingResult<ViewManagerInvestorTemporaryDto> result = new PagingResult<ViewManagerInvestorTemporaryDto>();
            result.TotalItems = query.Count();

            if (dto.PageSize != -1)
            {
                query = query.Skip(dto.Skip).Take(dto.PageSize);
            }
            var resultItem = new List<ViewManagerInvestorTemporaryDto>();
            if (result.TotalItems > 0)
            {
                foreach (var investor in query)
                {
                    var item = new ViewManagerInvestorTemporaryDto();
                    var tmpInvestor = investor;
                    tmpInvestor.Phone = StringUtils.HidePhone(tmpInvestor.Phone);
                    tmpInvestor.Email = StringUtils.HideEmail(tmpInvestor.Email);
                    AssignAdditionalValueToInvestorInListTemp(tmpInvestor);
                    item = tmpInvestor;
                    resultItem.Add(item);
                }
            }
            result.Items = resultItem;
            return result;
        }

        /// <summary>
        /// Lấy list investor thật
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageNumber"></param>
        /// <param name="keyword"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public PagingResult<ViewManagerInvestorDto> GetListInvestor(FindListInvestorDto input)
        {
            int? partnerId = null;
            var usertype = CommonUtils.GetCurrentUserType(_httpContext);
            if (usertype == UserTypes.TRADING_PROVIDER || usertype == UserTypes.ROOT_TRADING_PROVIDER)
            {
                var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
                input.TradingProviderId = tradingProviderId.ToString();
            }
            else if (new[] { UserTypes.PARTNER, UserTypes.ROOT_PARTNER }.Contains(usertype))
            {
                partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
            }
            var result = new PagingResult<ViewManagerInvestorDto>();
            var resultItems = new  List<ViewManagerInvestorDto>();
            var query = _investorEFRepository.FindAllInvestor(input, input.TradingProviderId, usertype, partnerId);
            foreach (var item in query.Items)
            {
                item.Email = StringUtils.HideEmail(item.Email);
                item.Phone = StringUtils.HidePhone(item.Phone);
                var approve = _approveRepository.GetOneByActual(item.InvestorId, CoreApproveDataType.INVESTOR);
                if (approve != null)
                {
                    item.Approve = approve;
                }
                else
                {
                    item.Approve = new CoreApprove { Status = CoreApproveStatus.DUYET };
                }
                item.ProfStatus = _managerInvestorRepository.GetIsProf(item.InvestorId);
                var resultItem = item;
                resultItems.Add(resultItem);
            }
            result.TotalItems = query.TotalItems;
            result.Items = resultItems;
            return result;
        }

        /// <summary>
        /// Filter investor thật bắt buộc nhập keyword
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageNumber"></param>
        /// <param name="keyword"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public PagingResult<ViewManagerInvestorDto> FilterInvestor(FilterManagerInvestorDto dto)
        {
            PagingResult<ViewManagerInvestorDto> result = new PagingResult<ViewManagerInvestorDto>
            {
                TotalItems = 0,
                Items = new List<ViewManagerInvestorDto>() { }
            };

            var query = _managerInvestorRepository.FilterInvestor(dto);

            result.TotalItems = query.TotalItems;
            var items = new List<ViewManagerInvestorDto>() { };

            if (result.TotalItems > 0)
            {
                foreach (var investor in query.Items)
                {
                    var tmpInvestor = _mapper.Map<ViewManagerInvestorDto>(investor);

                    AssignValueToInvestor(tmpInvestor, false, new OptionFindDto
                    {
                        IsNeedListAddress = true,
                        IsNeedDefaultBank = true,
                        IsNeedListBank = true,
                        IsNeedDefaultIdentification = true,
                        IsNeedListIdentification = true,
                    });

                    items.Add(tmpInvestor);
                }

                result.Items = items;
                result.TotalItems = items.Count;
            }

            return result;
        }

        /// <summary>
        /// Lấy list users theo investor id
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageNumber"></param>
        /// <param name="keyword"></param>
        /// <param name="investorId"></param>
        /// <returns></returns>
        public PagingResult<ViewUserDto> GetListUsers(FindUserByInvestorId dto)
        {
            var data = _investorEFRepository.FindListUserByInvestorId(dto);
            return data.ToPaging(dto.PageSize, dto.PageNumber);
        }

        /// <summary>
        /// Lấy list ngân hàng phân trang
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageNumber"></param>
        /// <param name="keyword"></param>
        /// <param name="investorId"></param>
        /// <param name="isTemp"></param>
        /// <returns></returns>
        public PagingResult<InvestorBankAccount> GetBankPaging(int pageSize, int pageNumber, string keyword, int investorId, bool isTemp)
        {
            return _managerInvestorRepository.GetListBankPaging(pageSize, pageNumber, keyword, investorId, isTemp);
        }

        /// <summary>
        /// Cập nhật investor
        /// </summary>
        /// <param name="dto"></param>
        public int UpdateInvestor(UpdateManagerInvestorDto dto)
        {
            var investor = _managerInvestorRepository.FindById(dto.InvestorId, dto.IsTemp);
            if (investor.RepresentativePhone != null)
            {
                investor.Phone = null;
                investor.Email = null;
            }
            else
            {
                investor.RepresentativePhone = null;
                investor.RepresentativeEmail = null;
            }

            return _managerInvestorRepository.UpdateInvestor(dto);
        }

        /// <summary>
        /// Cập nhật thông tin chứng khoán
        /// </summary>
        /// <param name="dto"></param>
        public void UpdateInvestorCongTyChungKhoan(UpdateCongTyChungKhoanDto dto)
        {
            var investor = FindById(dto.InvestorId, dto.IsTemp, new OptionFindDto
            {
                IsNeedDefaultIdentification = true,
                IsNeedListBank = true,
                IsNeedDefaultBank = true,
            });

            if (investor != null)
            {
                var investorUpdate = _mapper.Map<UpdateManagerInvestorDto>(investor);

                investorUpdate.DefaultIdentification = _mapper.Map<UpdateIdentificationDto>(investor.DefaultIdentification);
                investorUpdate.DefaultBank = _mapper.Map<UpdateDefaultBankDto>(investor.DefaultBank);

                investorUpdate.IsTemp = dto.IsTemp;
                investorUpdate.SecurityCompany = dto.SecurityCompany;
                investorUpdate.StockTradingAccount = dto.StockTradingAccount;

                UpdateInvestor(investorUpdate);
            }
        }

        /// <summary>
        /// Upload anh | Dong thoi check face match
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<EKYCFaceMatchResultDto> UploadFaceImageAsync(UploadFaceImageDto input)
        {
            var investor = FindById(input.InvestorId, input.IsTemp, null);
            var baseDir = _configuration["FilePath"];

            var frontImageInfo = FileUtils.GetPhysicalPath(investor.DefaultIdentification.IdFrontImageUrl, baseDir);
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

                _managerInvestorRepository.UploadFaceImage(input, faceImageUrl, input.IsTemp);
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
        /// Tạo bank
        /// </summary>
        /// <param name="dto"></param>
        public int CreateBankTemporary(CreateInvestorBankTempDto dto)
        {
            string username = CommonUtils.GetCurrentUsername(_httpContext);

            return _managerInvestorRepository.CreateInvestorBankTemporary(dto, username);
        }

        /// <summary>
        /// Chọn bank mặc định
        /// </summary>
        /// <param name="dto"></param>
        public void SetDefaultBank(SetDefaultBankDto dto)
        {
            string username = CommonUtils.GetCurrentUsername(_httpContext);
            _managerInvestorRepository.SetDefaultBank(dto, username);
        }

        /// <summary>
        /// Chọn giấy tờ mặc định
        /// </summary>
        /// <param name="dto"></param>
        public void SetDefaultIdentification(SetDefaultIdentificationDto dto)
        {
            string username = CommonUtils.GetCurrentUsername(_httpContext);
            _managerInvestorRepository.SetDefaultIdentification(dto, username);
        }

        /// <summary>
        /// Đổi trang thái user
        /// </summary>
        /// <param name="dto"></param>
        public void ChangeUserStatus(ChangeUserStatusDto dto)
        {
            var username = CommonUtils.GetCurrentUsername(_httpContext);

            _managerInvestorRepository.ChangeUserStatus(dto, username);
        }

        /// <summary>
        /// Reset password user
        /// </summary>
        /// <param name="dto"></param>
        public string ResetUserPassword(ResetUserPasswordManagerInvestorDto dto)
        {
            var username = CommonUtils.GetCurrentUsername(_httpContext);

            string plainPassword = GenerateCodes.GetRandomPassword();
            string password = CommonUtils.CreateMD5(plainPassword);

            _logger.LogDebug($"Password mới: {plainPassword}");

            _managerInvestorRepository.ResetUserPassword(dto, password, username);

            return plainPassword;
        }

        /// <summary>
        /// Reset pin
        /// </summary>
        /// <param name="dto"></param>
        public string ResetPin(ResetPinDto dto)
        {
            var username = CommonUtils.GetCurrentUsername(_httpContext);

            string plainPin = GenerateCodes.GetRandomPIN();
            string pin = CommonUtils.CreateMD5(plainPin);

            _managerInvestorRepository.ResetPin(dto, pin, username);

            return plainPin;
        }

        /// <summary>
        /// List dc lien he
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageNumber"></param>
        /// <param name="keyword"></param>
        /// <param name="investorId"></param>
        /// <returns></returns>
        public PagingResult<InvestorContactAddress> GetListContactAddress(int? pageSize, int? pageNumber, string keyword, int investorId, bool isTemp)
        {
            return _managerInvestorRepository.GetListContactAddress(pageSize, pageNumber, keyword, investorId, isTemp);
        }

        /// <summary>
        /// Them dc lien he
        /// </summary>
        /// <param name="dto"></param>
        public int AddContactAddress(CreateManagerInvestorContactAddressDto dto)
        {
            string username = CommonUtils.GetCurrentUsername(_httpContext);

            return _managerInvestorRepository.CreateContactAddress(dto, username);
        }

        /// <summary>
        /// Cap nhat dc lien he
        /// </summary>
        /// <param name="dto"></param>
        public void UpdateContactAddress(UpdateContactAddressDto dto)
        {
            string username = CommonUtils.GetCurrentUsername(_httpContext);

            _managerInvestorRepository.UpdateContactAddress(dto, username);
        }

        /// <summary>
        /// Set dc lien he mac dinh
        /// </summary>
        /// <param name="dto"></param>
        public void SetDefaultContactAddress(SetDefaultManagerInvestorContactAddressDto dto)
        {
            string username = CommonUtils.GetCurrentUsername(_httpContext);

            _managerInvestorRepository.SetDefaultContactAddress(dto, username);
        }

        public PagingResult<InvestorTemporary> GetListRequestProf(int? pageSize, int? pageNumber, string keyword, int? status)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Duyệt nhà đầu tư chuyên nghiệp
        /// </summary>
        /// <param name="dto"></param>
        public void ApproveProf(ApproveProfDto dto)
        {
            string username = CommonUtils.GetCurrentUsername(_httpContext);
            _approveRepository.OpenConnection();

            using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                _managerInvestorRepository.ApproveProf(dto, username);

                _approveRepository.ApproveRequestData(new ApproveRequestDto
                {
                    ApproveID = dto.ApproveId,
                    ApproveNote = dto.ApproveNote,
                    ReferId = dto.InvestorId
                });

                transaction.Complete();
            }

            _approveRepository.CloseConnection();

            string userType = CommonUtils.GetCurrentUserType(_httpContext);
            int? tradingProviderId = null;
            if (userType == UserTypes.TRADING_PROVIDER || userType == UserTypes.ROOT_TRADING_PROVIDER)
            {
                tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            }
            _sendEmailServices.SendEmailInvestorProf(dto.InvestorId, tradingProviderId);
        }

        /// <summary>
        /// Huỷ duyệt nhà đầu tư chuyên nghiệp
        /// </summary>
        /// <param name="dto"></param>
        public void CancelRequestProf(CancelRequestInvestorProfDto dto)
        {
            var approve = _approveRepository.GetOneByTemp(dto.InvestorTempId, CoreApproveDataType.INVESTOR_PROFESSIONAL);

            _approveRepository.OpenConnection();
            using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                _approveRepository.CancelRequest(new CancelRequestDto
                {
                    ApproveID = approve.ApproveID,
                    CancelNote = dto.Notice,
                });

                _managerInvestorRepository.CancelRequestProf(dto);

                transaction.Complete();
            }

            _approveRepository.CloseConnection();
        }

        /// <summary>
        /// Lấy danh sách yc duyệt nhà đầu tư chuyên nghiệp
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageNumber"></param>
        /// <param name="keyword"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public void InvestorCheckProf()
        {
            var investorId = CommonUtils.GetCurrentInvestorId(_httpContext);
            _investorRepository.InvestorCheckProf(investorId);
        }

        /// <summary>
        /// Lấy danh sách người giới thiệu
        /// </summary>
        /// <param name="investorId"></param>
        /// <returns></returns>
        public List<ViewManagerInvestorDto> GetIntroduceUsers(int investorId)
        {
            List<ViewManagerInvestorDto> result = null;
            var list = _managerInvestorRepository.GetIntroduceUsers(investorId);

            if (list != null)
            {
                result = _mapper.Map<List<ViewManagerInvestorDto>>(list);

                foreach (var investor in result)
                {
                    var defaultIdentification = _managerInvestorRepository.GetDefaultIdentification(investorId, false);
                    investor.DefaultIdentification = _mapper.Map<ViewIdentificationDto>(defaultIdentification);
                }
            }

            return result;
        }

        /// <summary>
        /// Upload avatar
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public int UploadAvatar(UploadAvatarDto dto)
        {
            string username = CommonUtils.GetCurrentUsername(_httpContext);

            string filePath = _imageServices.UploadFile(new ImageAPI.Models.UploadFileModel
            {
                Folder = FileFolder.INVESTOR,
                File = dto.Avatar
            });

            var data = _managerInvestorRepository.UploadAvatar(dto, filePath, username);

            return data;
        }

        /// <summary>
        /// Lấy ra lịch sử thay đổi investor
        /// </summary>
        /// <param name="approveId"></param>
        /// <returns></returns>
        public ViewInvestorUpdateHistoryDto GetDiff(int approveId)
        {
            var listDiff = _coreHistoryUpdateRepository.GetByApproveId(approveId)?.ToList();

            var result = new ViewInvestorUpdateHistoryDto
            {
                Investor = new Dictionary<string, HistoryDto>(),
                ListIdentification = new List<ObjectIdentificationDto>(),
                ListBank = new List<ObjectBankDto>(),
                ListContactAddress = new List<ObjectContactAddressDto>(),
                ListStock = new List<ObjectStockDto>(),
            };
            if (listDiff != null)
            {
                var approve = _approveRepository.GetByApproveId(approveId);
                // Thay đổi của bảng investor
                var listDiffInvestor = listDiff.Where(d => d.UpdateTable == CoreHistoryUpdateTable.INVESTOR);

                foreach (var diff in listDiffInvestor)
                {
                    result.Investor.Add(diff.FieldName, new HistoryDto
                    {
                        NewValue = diff.NewValue,
                        OldValue = diff.OldValue
                    });
                }

                if (approve != null)
                {
                    var investorTempId = approve.ReferIdTemp;

                    // Thay đổi của bảng identification
                    var listIdentification = _managerInvestorRepository.GetListIdentification(investorTempId ?? 0, null, true);

                    if (listIdentification != null)
                    {
                        foreach (var identification in listIdentification)
                        {
                            if (identification.ReferId == null)
                            {
                                result.ListIdentification.Add(new ObjectIdentificationDto
                                {
                                    NewObject = _mapper.Map<ViewIdentificationDto>(identification)
                                });
                            }
                            else
                            {
                                var listDiffIden = listDiff.Where(d => d.RealTableId == identification.ReferId);

                                if (listDiffIden != null)
                                {
                                    var tmpHistory = new ObjectIdentificationDto
                                    {
                                        Id = identification.ReferId ?? 0,
                                        History = new Dictionary<string, HistoryDto>(),
                                    };

                                    foreach (var diff in listDiffIden)
                                    {
                                        tmpHistory.History.Add(diff.FieldName, new HistoryDto
                                        {
                                            NewValue = diff.NewValue,
                                            OldValue = diff.OldValue,
                                        });
                                    }

                                    if (tmpHistory.History.Count > 0)
                                    {
                                        result.ListIdentification.Add(tmpHistory);
                                    }
                                }
                            }
                        }
                    }

                    // Thay đổi của bảng bank
                    //var listBank = _managerInvestorRepository.GetListBank(investorTempId ?? 0, null, true);
                    var listBank = _investorEFRepository.GetListBankAccTemp(investorTempId ?? 0);

                    if (listBank != null)
                    {
                        foreach (var bank in listBank)
                        {
                            if (bank.ReferId == null)
                            {
                                result.ListBank.Add(new ObjectBankDto
                                {
                                    NewObject = _mapper.Map<ViewInvestorBankAccountDto>(bank)
                                });
                            }
                            else
                            {
                                var listDiffBank = listDiff.Where(d => d.RealTableId == bank.ReferId);

                                if (listDiffBank != null)
                                {
                                    var tmpHistory = new ObjectBankDto
                                    {
                                        Id = bank.ReferId ?? 0,
                                        History = new Dictionary<string, HistoryDto>(),
                                    };

                                    foreach (var diff in listDiffBank)
                                    {
                                        tmpHistory.History.Add(diff.FieldName, new HistoryDto
                                        {
                                            NewValue = diff.NewValue,
                                            OldValue = diff.OldValue,
                                        });
                                    }

                                    bool isDeleted = listDiffBank.Any(x => x.FieldName == "Deleted");
                                    if (isDeleted)
                                    {
                                        tmpHistory.DeletedObject = _mapper.Map<ViewInvestorBankAccountDto>(bank);
                                    }

                                    if (tmpHistory.History.Count > 0)
                                    {
                                        result.ListBank.Add(tmpHistory);
                                    }
                                }
                            }
                        }
                    }

                    // Thay đổi của bảng địa chỉ

                    var listContactAddress = _managerInvestorRepository.GetListContactAddressNoPaging(investorTempId ?? 0, true);

                    if (listContactAddress != null)
                    {
                        foreach (var contactAddress in listContactAddress)
                        {
                            if (contactAddress.ReferId == null)
                            {
                                result.ListContactAddress.Add(new ObjectContactAddressDto
                                {
                                    NewObject = _mapper.Map<ViewInvestorContactAddressDto>(contactAddress)
                                });
                            }
                            else
                            {
                                var listDiffContactAddress = listDiff.Where(d => d.RealTableId == contactAddress.ReferId);

                                if (listDiffContactAddress != null)
                                {
                                    var tmpHistory = new ObjectContactAddressDto
                                    {
                                        Id = contactAddress.ReferId ?? 0,
                                        History = new Dictionary<string, HistoryDto>(),
                                    };

                                    foreach (var diff in listDiffContactAddress)
                                    {
                                        tmpHistory.History.Add(diff.FieldName, new HistoryDto
                                        {
                                            NewValue = diff.NewValue,
                                            OldValue = diff.OldValue,
                                        });
                                    }

                                    if (tmpHistory.History.Count > 0)
                                    {
                                        result.ListContactAddress.Add(tmpHistory);
                                    }
                                }
                            }
                        }
                    }

                    // Thay đổi của bảng stock

                    var listStock = _managerInvestorRepository.GetListStockNoPaging(new FindInvestorStockDto { IsTemp = true, InvestorId = investorTempId ?? 0, InvestorGroupId = null });

                    if (listStock != null)
                    {
                        foreach (var stockTemp in listStock)
                        {
                            if (stockTemp.ReferId == null)
                            {
                                result.ListStock.Add(new ObjectStockDto
                                {
                                    NewObject = _mapper.Map<ViewInvestorStockDto>(stockTemp)
                                });
                            }
                            else
                            {
                                var listDiffStock = listDiff.Where(d => d.RealTableId == stockTemp.ReferId);

                                if (listDiffStock != null)
                                {
                                    var tmpHistory = new ObjectStockDto
                                    {
                                        Id = stockTemp.ReferId ?? 0,
                                        History = new Dictionary<string, HistoryDto>(),
                                    };

                                    foreach (var diff in listDiffStock)
                                    {
                                        tmpHistory.History.Add(diff.FieldName, new HistoryDto
                                        {
                                            NewValue = diff.NewValue,
                                            OldValue = diff.OldValue,
                                        });
                                    }

                                    if (tmpHistory.History.Count > 0)
                                    {
                                        result.ListStock.Add(tmpHistory);
                                    }
                                }
                            }
                        }
                    }

                    // Lấy user request + user approve
                    var userRequest = _userRepository.FindById(approve.UserRequestId ?? 0);
                    var userApprove = _userRepository.FindById(approve.UserApproveId ?? 0);

                    if (userRequest != null)
                    {
                        result.UserRequest = _mapper.Map<UserDto>(userRequest);
                    }

                    if (userApprove != null)
                    {
                        result.UserApprove = _mapper.Map<UserDto>(userApprove);
                    }
                    result.approveDto = _mapper.Map<ViewCoreApproveDto>(approve);
                }
            }

            return result;
        }

        /// <summary>
        /// Get list file chứng minh ndt cn
        /// </summary>
        /// <param name="investorId"></param>
        /// <returns></returns>
        public List<InvestorProfFile> GetListProfFile(int investorTempId)
        {
            return _managerInvestorRepository.GetProfFile(investorTempId);
        }

        private void AssignAddtionalValueToInvestor(ViewManagerInvestorTemporaryDto investor, bool isTemp, OptionFindDto options)
        {
            if (investor != null)
            {
                // muốn lấy trạng thái duyệt
                if (options.IsNeedApproveStatus)
                {
                    var approve = isTemp ? _approveRepository.GetOneByTemp(investor.InvestorId, CoreApproveDataType.INVESTOR) : _approveRepository.GetOneByActual(investor.InvestorId, CoreApproveDataType.INVESTOR);
                    if (approve != null)
                    {
                        investor.Approve = approve;
                        investor.StatusEditInvestor = approve.Status;
                    }
                    else
                    {
                        investor.Approve = new CoreApprove { Status = CoreApproveStatus.KHOI_TAO };
                        investor.StatusEditInvestor = CoreApproveStatus.KHOI_TAO;
                    }

                    if (!isTemp)
                    {
                        var approvePhone = _approveRepository.GetOneByActual(investor.InvestorId, CoreApproveDataType.INVESTOR_PHONE);
                        if (approvePhone != null)
                        {
                            investor.StatusEditPhone = approvePhone.Status;
                        }
                        var approveEmail = _approveRepository.GetOneByActual(investor.InvestorId, CoreApproveDataType.INVESTOR_EMAIL);
                        if (approveEmail != null)
                        {
                            investor.StatusEditEmail = approveEmail.Status;
                        }
                        var investorCheckUpdate = (from investorTemp in _dbContext.InvestorTemps
                                                   let statusApprove = _dbContext.CoreApproves.FirstOrDefault(p => p.DataType == CoreApproveDataType.INVESTOR && p.ReferIdTemp == investorTemp.InvestorId)
                                                   where investorTemp.InvestorGroupId == investor.InvestorGroupId && investorTemp.Deleted == YesNo.NO
                                                   && !_dbContext.CoreApproves.Any(p => (p.DataType == CoreApproveDataType.INVESTOR_EMAIL || p.DataType == CoreApproveDataType.INVESTOR_PHONE) && p.ReferIdTemp == investorTemp.InvestorId)
                                                   orderby investorTemp.InvestorId
                                                   orderby investorTemp.InvestorId descending
                                                   select new
                                                   {
                                                       InvestorTempId = investorTemp.InvestorId,
                                                       StatusApprove = statusApprove.Status
                                                   }).FirstOrDefault();

                        // Nếu có bản ghi Temp nhưng chưa trình duyệt thì Status = 1
                        investor.StatusEditInvestor = investorCheckUpdate != null ? (investorCheckUpdate.StatusApprove != null ? investorCheckUpdate.StatusApprove : CoreApproveStatus.TRINH_DUYET) : CoreApproveStatus.KHOI_TAO;
                    }
                }

                // muốn lấy danh sách identification
                if (options.IsNeedListIdentification)
                {
                    var listIdentification = _managerInvestorRepository.GetListIdentification(investor.InvestorId, investor.InvestorGroupId, isTemp)?.Where(item => new string[] { InvestorIdentificationStatus.ACTIVE }.Contains(item.Status));
                    investor.ListIdentification = _mapper.Map<List<ViewIdentificationDto>>(listIdentification);
                }

                // muốn lấy giấy tờ mặc định
                if (options.IsNeedDefaultIdentification)
                {
                    if (investor.ListIdentification != null)
                    {
                        investor.DefaultIdentification = investor.ListIdentification.FirstOrDefault(identification => identification.IsDefault == YesNo.YES);
                        if (investor.DefaultIdentification == null)
                        {
                            investor.DefaultIdentification = investor.ListIdentification.FirstOrDefault();
                        }
                    }
                    else
                    {
                        var defaultIden = _managerInvestorRepository.GetDefaultIdentification(investor.InvestorId, isTemp);
                        investor.DefaultIdentification = _mapper.Map<ViewIdentificationDto>(defaultIden);
                    }
                }

                // muốn lấy ds ngân hàng
                if (options.IsNeedListBank)
                {
                    investor.ListBank = _managerInvestorRepository.GetListBank(investor.InvestorId, investor.InvestorGroupId, isTemp)?.ToList();
                }

                // muốn lấy ngân hàng mặc định
                if (options.IsNeedDefaultBank)
                {
                    if (investor.ListBank != null)
                    {
                        investor.DefaultBank = investor.ListBank.FirstOrDefault(bank => bank.IsDefault == YesNo.YES);
                        if (investor.DefaultBank == null)
                        {
                            investor.DefaultBank = investor.ListBank.FirstOrDefault();
                        }
                    }
                }

                // muốn lấy danh sách địa chỉ
                if (options.IsNeedListAddress)
                {
                    var listContactAddress = _managerInvestorRepository.GetListContactAddress(-1, 0, "", investor.InvestorId, isTemp);
                    investor.ListContactAddress = listContactAddress.Items?.ToList();
                }

                if (options.IsNeedDefaultAddress)
                {
                    investor.DefaultContactAddress = _managerInvestorRepository.GetDefaultContactAddress(investor.InvestorId, isTemp);
                }

                if (options.IsNeedListStock)
                {
                    var listStock = _managerInvestorRepository.GetListStockNoPaging(new FindInvestorStockDto { InvestorGroupId = investor.InvestorGroupId, InvestorId = investor.InvestorId, IsTemp = isTemp });
                    investor.ListStock = _mapper.Map<List<ViewInvestorStockDto>>(listStock?.ToList());
                }

                if (options.IsNeedDefaultStock)
                {
                    var stock = _managerInvestorRepository.GetDefaultStock(investor.InvestorId, isTemp);
                    investor.DefaultStock = _mapper.Map<ViewInvestorStockDto>(stock);
                }

                // Muốn lấy điểm loyalty
                if (options.IsNeedLoyalty && !isTemp)
                {
                    var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
                    var invPoint = _loyPointInvestorEFRepository.Get(investor.InvestorId, tradingProviderId);

                    investor.LoyTotalPoint = invPoint?.TotalPoint;
                    investor.LoyCurrentPoint = invPoint?.CurrentPoint;
                    investor.LoyConsumePoint = invPoint?.TotalPoint - invPoint?.CurrentPoint;
                    var rank = _loyRankEFRepository.FindRankByTotalPoint(invPoint?.TotalPoint ?? 0, tradingProviderId);
                    if (rank != null)
                    {
                        investor.RankId = rank.Id;
                        investor.RankName = rank.Name;
                    }
                }

            }
        }

        /// <summary>
        /// Gán giá trị thêm cho investor temp để trả ra ở cms
        /// </summary>
        /// <param name="tmpInvestor"></param>
        private void AssignAdditionalValueToInvestorInListTemp(ViewManagerInvestorTemporaryDto tmpInvestor)
        {
            var approve = _approveRepository.GetByApproveId(tmpInvestor.ApproveId ?? 0);
            if (tmpInvestor.ApproveId != null)
            {
                tmpInvestor.Approve = approve;
            }
            else
            {
                tmpInvestor.Approve = new CoreApprove { Status = CoreApproveStatus.KHOI_TAO };
            }

        }

        /// <summary>
        /// Gán giá trị cho investor thật để trả ra ở cms
        /// </summary>
        /// <param name="investor"></param>
        /// <param name="isTemp"></param>
        /// <param name="isNeedBank"></param>
        /// <param name="isNeedContactAddress"></param>
        public void AssignValueToInvestor(ViewManagerInvestorDto investor, bool isTemp, OptionFindDto options)
        {
            var approve = _approveRepository.GetOneByActual(investor.InvestorId, CoreApproveDataType.INVESTOR);
            if (approve != null)
            {
                investor.Approve = approve;
            }
            else
            {
                investor.Approve = new CoreApprove { Status = CoreApproveStatus.DUYET };
            }

            if (options.IsNeedListIdentification)
            {
                var listIdenitifcation = _managerInvestorRepository.GetListIdentification(investor.InvestorId, investor.InvestorGroupId, false)?.Where(item => item.Status == InvestorIdentificationStatus.ACTIVE);
                investor.ListIdentification = _mapper.Map<List<ViewIdentificationDto>>(listIdenitifcation.ToList());
            }

            if (options.IsNeedDefaultIdentification)
            {
                if (investor.ListIdentification != null)
                {
                    investor.DefaultIdentification = investor.ListIdentification.FirstOrDefault(identification => identification.IsDefault == YesNo.YES);
                    if (investor.DefaultIdentification == null)
                    {
                        investor.DefaultIdentification = investor.ListIdentification.FirstOrDefault();
                    }
                }
                else
                {
                    var defaultIden = _managerInvestorRepository.GetDefaultIdentification(investor.InvestorId, isTemp);
                    if (defaultIden != null)
                    {
                        investor.DefaultIdentification = _mapper.Map<ViewIdentificationDto>(defaultIden);
                    }
                }
            }


            if (options.IsNeedListBank)
            {
                investor.ListBank = _managerInvestorRepository.GetListBank(investor.InvestorId, investor.InvestorGroupId, false)?.ToList();
            }

            if (options.IsNeedDefaultBank)
            {
                if (investor.ListBank != null)
                {
                    investor.DefaultBank = investor.ListBank.FirstOrDefault(bank => bank.IsDefault == YesNo.YES);
                }
            }

            if (options.IsNeedListAddress)
            {
                var listContactAddressQuery = _managerInvestorRepository.GetListContactAddress(-1, 0, null, investor.InvestorId, isTemp);

                investor.ListContactAddress = listContactAddressQuery?.Items?.ToList();

            }

            if (options.IsNeedDefaultAddress)
            {
                var defaultAddress = _managerInvestorRepository.GetDefaultContactAddress(investor.InvestorId, isTemp);
                investor.DefaultContactAddress = defaultAddress;
            }

            if (options.IsNeedListStock)
            {
                var listStockQuery = _managerInvestorRepository.GetListStockNoPaging(new FindInvestorStockDto
                {
                    InvestorGroupId = investor.InvestorGroupId,
                    InvestorId = investor.InvestorId,
                    IsTemp = false,
                })?.ToList();

                investor.ListStock = _mapper.Map<List<ViewInvestorStockDto>>(listStockQuery);
            }

            if (options.IsNeedDefaultStock)
            {
                var defaultStock = _managerInvestorRepository.GetDefaultStock(investor.InvestorId, isTemp);
                investor.DefaultStock = _mapper.Map<ViewInvestorStockDto>(defaultStock);
            }

        }

        /// <summary>
        /// Lấy thông tin của sale mà investor đã quét
        /// </summary>
        /// <param name="investorId"></param>
        /// <returns></returns>
        public List<SaleManagerInvestorDto> GetListSale(int investorId)
        {
            string userType = CommonUtils.GetCurrentUserType(_httpContext);
            int? tradingProviderId = -1;
            if (new string[] { UserTypes.TRADING_PROVIDER, UserTypes.ROOT_TRADING_PROVIDER }.Contains(userType))
            {
                try
                {
                    tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
                }
                catch (Exception)
                {
                    tradingProviderId = -1;
                }
            }
            else if (new string[] { UserTypes.ROOT_EPIC, UserTypes.EPIC }.Contains(userType))
            {
                tradingProviderId = null;
            }

            var listInvestorSale = _investorRepository.GetListReferralCodeSaleByInvestorId(investorId, tradingProviderId, userType);

            var result = new List<SaleManagerInvestorDto>();
            var options = new OptionFindDto()
            {
                IsNeedDefaultIdentification = true
            };

            foreach (var investorSale in listInvestorSale)
            {
                var saleInfo = _saleRepository.FindSaleById(investorSale.SaleId ?? 0, tradingProviderId);

                if (investorSale.InvestorIdOfSale != null)
                {
                    // nếu sale là investor
                    var investorDb = _managerInvestorRepository.FindById(investorSale.InvestorIdOfSale ?? 0, false);
                    if (investorDb != null)
                    {
                        var tmp = _mapper.Map<SaleManagerInvestorDto>(investorDb);

                        AssignAddtionalValueToInvestor(tmp, false, options);

                        tmp.SaleId = investorSale.SaleId;

                        if (saleInfo != null)
                        {
                            tmp.Sale = saleInfo;
                        }

                        tmp.InvestorSale = investorSale;

                        result.Add(tmp);
                    }
                }
                else
                {
                    // nếu sale là business customer 
                    var businessCustomer = _businessCustomerRepository.FindBusinessCustomerById(investorSale.BusinessCustomerIdOfSale ?? 0);

                    if (businessCustomer != null)
                    {
                        var tmp = new SaleManagerInvestorDto
                        {
                            Sale = saleInfo,
                            InvestorSale = investorSale,
                            SaleId = investorSale?.SaleId,
                            BusinessCustomer = _mapper.Map<BusinessCustomerDto>(businessCustomer),
                        };

                        result.Add(tmp);

                    }
                }

            }

            return result;
        }

        /// <summary>
        /// Đăng ký tư vấn viên
        /// </summary>
        /// <param name="dto"></param>
        public void AddSale(AddSaleManagerInvestorDto dto)
        {
            _investorRepository.ScanReferralCodeSale(new ScanReferralCodeSaleDto
            {
                ReferralCode = dto.ReferralCode
            }, dto.InvestorId);
        }

        /// <summary>
        /// Chọn sale mặc định
        /// </summary>
        /// <param name="dto"></param>
        public void SetDefaultSale(UpdateSaleIsDefaultDto dto)
        {
            _investorRepository.SetDefaultReferralCodeSale(dto.InvestorSaleId);
        }

        /// <summary>
        /// Tạo yêu cầu duyệt email
        /// </summary>
        /// <param name="dto"></param>
        public void CreateRequestEmail(CreateRequestEmailDto dto)
        {
            var userId = CommonUtils.GetCurrentUserId(_httpContext);
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            int? tradingProviderId;
            try
            {
                tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            }
            catch (Exception)
            {
                tradingProviderId = null;
            }

            _approveRepository.OpenConnection();

            using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                // Tạo nhà đầu tư cn tạm
                var investorTemp = _managerInvestorRepository.CreateRequestEmail(dto, username);
                var investor = _managerInvestorRepository.FindByInvestorGroupId(investorTemp.InvestorGroupId);

                string summary = $"Nhà đầu tư có id: {investor.InvestorId}";
                var listIdenDb = _managerInvestorRepository.GetListIdentification(investor.InvestorId, null, false);
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
                    DataType = CoreApproveDataType.INVESTOR_EMAIL,
                    RequestNote = summary,
                    UserApproveId = 0,
                    UserRequestId = userId,
                    ReferIdTemp = investorTemp.InvestorId,
                    ReferId = investor.InvestorId,
                    Summary = summary,
                };

                var coreApprove = _approveRepository.CreateApproveRequest(approve, tradingProviderId);

                if (approve.ReferId != 0)
                {
                    var historyUpdateDto = new CoreHistoryUpdate()
                    {
                        ApproveId = coreApprove.ApproveID,
                        RealTableId = approve.ReferId,
                        OldValue = investor.Email,
                        NewValue = investorTemp.Email,
                        FieldName = "Email",
                        CreatedBy = approve?.UserRequestId.ToString(),
                        UpdateTable = CoreHistoryUpdateTable.INVESTOR,
                    };
                    _coreHistoryUpdateRepository.Add(historyUpdateDto);
                }

                transaction.Complete();
            }

            _approveRepository.CloseConnection();
        }

        /// <summary>
        /// Tạo yêu cầu duyệt phone
        /// </summary>
        /// <param name="dto"></param>
        public void CreateRequestPhone(CreateRequestPhoneDto dto)
        {
            var userId = CommonUtils.GetCurrentUserId(_httpContext);
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            int? tradingProviderId;
            try
            {
                tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            }
            catch (Exception)
            {
                tradingProviderId = null;
            }

            _approveRepository.OpenConnection();

            using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                var investorTemp = _managerInvestorRepository.CreateRequestPhone(dto, username);
                var investor = _managerInvestorRepository.FindByInvestorGroupId(investorTemp.InvestorGroupId);

                string summary = $"Nhà đầu tư có id: {investor.InvestorId}";
                var listIdenDb = _managerInvestorRepository.GetListIdentification(investor.InvestorId, null, false);
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
                    DataType = CoreApproveDataType.INVESTOR_PHONE,
                    RequestNote = summary,
                    UserApproveId = 0,
                    UserRequestId = userId,
                    ReferIdTemp = investorTemp.InvestorId,
                    ReferId = investor.InvestorId,
                    Summary = summary,
                };

                var coreApprove = _approveRepository.CreateApproveRequest(approve, tradingProviderId);

                if (approve.ReferId != 0)
                {
                    var historyUpdateDto = new CoreHistoryUpdate()
                    {
                        ApproveId = coreApprove.ApproveID,
                        RealTableId = approve.ReferId,
                        OldValue = investor.Phone,
                        NewValue = investorTemp.Phone,
                        FieldName = "Phone",
                        CreatedBy = approve?.UserRequestId.ToString(),
                        UpdateTable = CoreHistoryUpdateTable.INVESTOR,
                    };
                    _coreHistoryUpdateRepository.Add(historyUpdateDto);
                }

                transaction.Complete();
            }

            _approveRepository.CloseConnection();
        }

        /// <summary>
        /// Duyệt email
        /// </summary>
        /// <param name="dto"></param>
        public void ApproveEmail(ApproveEmailDto dto)
        {
            string username = CommonUtils.GetCurrentUsername(_httpContext);
            int userId = CommonUtils.GetCurrentUserId(_httpContext);
            _approveRepository.OpenConnection();

            using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                _managerInvestorRepository.ApproveEmail(dto, username);
                var approve = _approveRepository.GetOneByTemp(dto.InvestorIdTemp, CoreApproveDataType.INVESTOR_EMAIL);

                _approveRepository.ApproveRequestData(new ApproveRequestDto
                {
                    ApproveID = approve.ApproveID,
                    ApproveNote = dto.Note,
                    ReferId = approve.ReferId,
                    UserApproveId = userId,
                });

                transaction.Complete();
            }

            _approveRepository.CloseConnection();
        }

        /// <summary>
        /// Duyệt phone
        /// </summary>
        /// <param name="dto"></param>
        public void ApprovePhone(ApprovePhoneDto dto)
        {
            string username = CommonUtils.GetCurrentUsername(_httpContext);
            int userId = CommonUtils.GetCurrentUserId(_httpContext);
            _approveRepository.OpenConnection();

            using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                _managerInvestorRepository.ApprovePhone(dto, username);
                var approve = _approveRepository.GetOneByTemp(dto.InvestorIdTemp, CoreApproveDataType.INVESTOR_PHONE);

                _approveRepository.ApproveRequestData(new ApproveRequestDto
                {
                    ApproveID = approve.ApproveID,
                    ApproveNote = dto.Note,
                    ReferId = approve.ReferId,
                    UserApproveId = userId,
                });

                transaction.Complete();
            }

            _approveRepository.CloseConnection();
        }

        /// <summary>
        /// Huỷ duyệt mail
        /// </summary>
        /// <param name="dto"></param>
        public void CancelRequestEmail(CancelRequestNotInvestor dto)
        {
            var approve = _approveRepository.GetOneByTemp(dto.InvestorIdTemp, CoreApproveDataType.INVESTOR_EMAIL);

            _approveRepository.CancelRequest(new CancelRequestDto
            {
                ApproveID = approve.ApproveID,
                CancelNote = dto.Notice,
            });
        }

        /// <summary>
        /// Huỷ duyệt phone
        /// </summary>
        /// <param name="dto"></param>
        public void CancelRequestPhone(CancelRequestNotInvestor dto)
        {
            var approve = _approveRepository.GetOneByTemp(dto.InvestorIdTemp, CoreApproveDataType.INVESTOR_PHONE);

            _approveRepository.CancelRequest(new CancelRequestDto
            {
                ApproveID = approve.ApproveID,
                CancelNote = dto.Notice,
            });
        }

        /// <summary>
        /// Lấy danh sách công ty chứng khoán
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public List<InvestorStock> GetListStockNoPaging(FindInvestorStockDto dto)
        {
            var data = _managerInvestorRepository.GetListStockNoPaging(dto);

            return data?.ToList();
        }

        /// <summary>
        /// Tạo công ty chứng khoán
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public int CreateInvestorStock(CreateInvestorStockDto dto)
        {
            string username = CommonUtils.GetCurrentUsername(_httpContext);

            var result = _managerInvestorRepository.CreateInvestorStock(dto, username);

            return result;
        }

        /// <summary>
        /// Chọn công ty chứng khoán mặc định
        /// </summary>
        /// <param name="dto"></param>
        public void SetDefaultStock(SetDefaultInvestorStockDto dto)
        {
            string username = CommonUtils.GetCurrentUsername(_httpContext);

            _managerInvestorRepository.SetDefaultStock(dto, username);
        }

        /// <summary>
        /// Lấy investor đăng ký trên app cho màn hình tài khoản chưa xác minh
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public PagingResult<InvestorNoEkycDto> FindAppNoEkycInvestor(FindInvestorNoEkycDto dto)
        {
            _logger.LogInformation($"{System.Reflection.MethodBase.GetCurrentMethod().Name}: {JsonSerializer.Serialize(dto)}");

            string usertype = CommonUtils.GetCurrentUserType(_httpContext);

            if (new string[] { UserTypes.TRADING_PROVIDER, UserTypes.ROOT_TRADING_PROVIDER }.Contains(usertype))
            {
                int? tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
                dto.TradingProviderId = tradingProviderId;
            }
            else if (!(new string[] { UserTypes.EPIC, UserTypes.ROOT_EPIC }.Contains(usertype)))
            {
                dto.TradingProviderId = -1;
            }

            var data = _investorEFRepository.FindInvestorNoEkyc(dto);
            //Sắp xếp từ mới đến cũ theo ngày CreatedDate
            data = data.OrderByDescending(e => e.CreatedDate);
            data = data.OrderDynamic(dto.Sort);
            var rslt = data.ToPaging(dto.PageSize, dto.PageNumber);

            var listItems = new List<InvestorNoEkycDto>();
            foreach (var item in rslt.Items)
            {
                InvestorNoEkycDto tmp = item;

                tmp.PlainPhone = item.Phone;
                tmp.PlainEmail = item.Email;

                tmp.Phone = StringUtils.HidePhone(item.Phone);
                tmp.Email = StringUtils.HideEmail(item.Email);

                listItems.Add(tmp);
            }

            rslt.Items = listItems;
            return rslt;
        }

        /// <summary>
        /// Thực hiện ekyc cho khách hàng chưa qua bước ekyc trên app
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task UpdateAppInvestorIdentification(UpdateAppInvestorIdentificationDto dto)
        {
            _logger.LogInformation($"{System.Reflection.MethodBase.GetCurrentMethod().Name}: {JsonSerializer.Serialize(dto)}");

            var investor = _investorEFRepository.FindById(dto.InvestorId);
            if (investor == null)
            {
                _investorEFRepository.ThrowException(ErrorCode.UserNotFound);
            }
            //investor.Step = InvestorAppStep.DA_EKYC;

            // Xem có giấy tờ trùng số không
            var anySameIdno = _investorEFRepository.AnyAppIdentitficationDuplicate(dto.IdNo, investor.InvestorGroupId);
            if (anySameIdno)
            {
                _investorEFRepository.ThrowException(ErrorCode.InvestorIdNoExisted);
            }

            // Lấy giấy tờ mặc định của investor (nếu có)
            var oldIdentification = _investorEFRepository.FindTempIdentificationByIdNo(dto.InvestorId, dto.IdNo);
            string status = InvestorStatus.TEMP;

            using (var tran = _dbContext.Database.BeginTransaction())
            {
                if (oldIdentification != null)
                {
                    // có rồi thì update
                    oldIdentification.Status = status;
                    oldIdentification.IdNo = dto.IdNo;
                    oldIdentification.Fullname = dto.Fullname;
                    oldIdentification.DateOfBirth = dto.DateOfBirth;
                    oldIdentification.Nationality = dto.Nationality;
                    oldIdentification.Sex = dto.Sex;
                    oldIdentification.IdIssuer = dto.IdIssuer;
                    oldIdentification.IdDate = dto.IdDate;
                    oldIdentification.IdExpiredDate = dto.IdExpiredDate;
                    oldIdentification.PlaceOfOrigin = dto.PlaceOfOrigin;
                    oldIdentification.PlaceOfResidence = dto.PlaceOfResidence;
                    oldIdentification.IdFrontImageUrl = dto.IdFrontImageUrl;
                    oldIdentification.IdBackImageUrl = dto.IdBackImageUrl;
                    oldIdentification.EkycInfoIsConfirmed = YesNo.YES;
                    oldIdentification.Status = InvestorStatus.ACTIVE;
                    oldIdentification.IsVerifiedIdentification = YesNo.YES;

                    _dbContext.SaveChanges();
                }
                else
                {
                    // chưa có thì bỏ default ở những giấy tờ trước
                    _investorEFRepository.SoftDeleteIdentification(dto.InvestorId);

                    // thêm mới giấy tờ và lấy giấy tờ này làm mặc định
                    _investorEFRepository.AddIdentification(new InvestorIdentification
                    {
                        InvestorId = dto.InvestorId,
                        InvestorGroupId = investor.InvestorGroupId ?? 0,
                        IdType = dto.IdType,
                        IdNo = dto.IdNo,
                        Fullname = dto.Fullname,
                        Nationality = dto.Nationality,
                        Sex = dto.Sex,
                        DateOfBirth = dto.DateOfBirth,
                        IsDefault = YesNo.YES,
                        IdIssuer = dto.IdIssuer,
                        IdDate = dto.IdDate,
                        IdExpiredDate = dto.IdExpiredDate,
                        PlaceOfOrigin = dto.PlaceOfOrigin,
                        PlaceOfResidence = dto.PlaceOfResidence,
                        IdFrontImageUrl = dto.IdFrontImageUrl,
                        IdBackImageUrl = dto.IdBackImageUrl,
                        EkycInfoIsConfirmed = YesNo.YES,
                        Status = InvestorStatus.ACTIVE,
                        IsVerifiedIdentification = YesNo.YES,
                    });
                }

                // bank
                var anySameBank = _investorEFRepository.AnySameBank(dto.BankId, dto.BankAccount);
                if (!anySameBank)
                {
                    _investorEFRepository.AddBankAccountWithDefault(new InvestorBankAccount
                    {
                        InvestorId = investor.InvestorId,
                        InvestorGroupId = investor.InvestorGroupId ?? 0,
                        BankAccount = dto.BankAccount,
                        BankId = dto.BankId,
                        OwnerAccount = dto.OwnerAccount,
                        IsDefault = YesNo.YES,
                    });
                }
                else
                {
                    _investorEFRepository.ThrowException(ErrorCode.InvestorBankIsRegistered);
                }

                // kích hoạt tài khoản đăng nhập
                var user = _usersEFRepository.FindByInvestorId(dto.InvestorId);
                user.Status = UserStatus.ACTIVE;

                // kích hoạt investor cho đầu tư hay không
                investor.Status = InvestorStatus.ACTIVE;

                // cập nhật step 4 và ngày mà investor đăng ký xong
                investor.Step = InvestorAppStep.DA_ADD_BANK;
                investor.FinalStepDate = DateTime.Now;

                if (investor.ThirdStepDate == null)
                {
                    investor.ThirdStepDate = investor.FinalStepDate;
                }

                if (investor.SecondStepDate == null)
                {
                    investor.SecondStepDate = investor.FinalStepDate;
                }

                if (investor.FirstStepDate == null)
                {
                    investor.FirstStepDate = investor.FinalStepDate;
                }

                _dbContext.SaveChanges();
                tran.Commit();
            }
            await _sendEmailServices.SendEmailVerificationAccountSuccess(investor.Phone);
        }

        public int DeleteBankAccount(DeleteBankAccountDto dto)
        {
            string username = CommonUtils.GetCurrentUsername(_httpContext);
            return _managerInvestorRepository.DeleteBankAccount(dto, username);
        }

        /// <summary>
        /// Xóa Investor theo Id hoặc phone
        /// </summary>
        /// <param name="phone"></param>
        public void DeletedInvestor(int? investorId, string phone)
        {
            // Nếu không tìm được theo Id thì tìm theo Sđt
            var investorQuery = _dbContext.Investors.FirstOrDefault(i => investorId != null && i.InvestorId == investorId);
            if (investorQuery == null)
            {
                investorQuery = _dbContext.Investors.FirstOrDefault(i => i.Phone == phone)
                    .ThrowIfNull(_dbContext, ErrorCode.InvestorPhoneNotFound);
            }
            var cifCodeQuery = _dbContext.CifCodes.FirstOrDefault(c => c.InvestorId == investorQuery.InvestorId)
                .ThrowIfNull(_dbContext, ErrorCode.CoreCifCodeNotFound);

            if (_dbContext.InvOrders.Any(c => c.CifCode == cifCodeQuery.CifCode))
            {
                _investorEFRepository.ThrowException(ErrorCode.InvestorCanNotDelExistOrderInvest);
            }
            if (_dbContext.InvOrders.Any(c => c.SaleReferralCode == investorQuery.ReferralCodeSelf || c.SaleReferralCodeSub == investorQuery.ReferralCodeSelf))
            {
                _investorEFRepository.ThrowException(ErrorCode.InvestorCanNotDelExistReferraCodeInOrderInv);
            }
            if (_dbContext.GarnerOrders.Any(c => c.CifCode == cifCodeQuery.CifCode))
            {
                _investorEFRepository.ThrowException(ErrorCode.InvestorCanNotDelExistOrderGarner);
            }
            if (_dbContext.GarnerOrders.Any(c => c.SaleReferralCode == investorQuery.ReferralCodeSelf || c.SaleReferralCodeSub == investorQuery.ReferralCodeSelf))
            {
                _investorEFRepository.ThrowException(ErrorCode.InvestorCanNotDelExistReferraCodeInOrderGan);
            }
            if (_dbContext.RstOrders.Any(c => c.CifCode == cifCodeQuery.CifCode))
            {
                _investorEFRepository.ThrowException(ErrorCode.InvestorCanNotDelExistOrderRealEstate);
            }
            if (_dbContext.RstOrders.Any(c => c.SaleReferralCode == investorQuery.ReferralCodeSelf || c.SaleReferralCodeSub == investorQuery.ReferralCodeSelf))
            {
                _investorEFRepository.ThrowException(ErrorCode.InvestorCanNotDelExistReferraCodeInOrderRst);
            }
            _dbContext.Remove(investorQuery);
            _dbContext.Remove(cifCodeQuery);

            var investorTempQuery = _dbContext.InvestorTemps.FirstOrDefault(i => i.InvestorGroupId == investorQuery.InvestorGroupId);
            if (investorTempQuery != null)
            {
                _dbContext.RemoveRange(_dbContext.InvestorBankAccountTemps.Where(i => i.InvestorId == investorTempQuery.InvestorId));
                _dbContext.RemoveRange(_dbContext.InvestorContactAddressTemps.Where(i => i.InvestorId == investorTempQuery.InvestorId));
                _dbContext.RemoveRange(_dbContext.InvestorIdTemps.Where(i => i.InvestorId == investorTempQuery.InvestorId));
                _dbContext.RemoveRange(_dbContext.InvestorTemps.Where(i => i.InvestorId == investorTempQuery.InvestorId));
            }
            _dbContext.RemoveRange(_dbContext.InvestorBankAccounts.Where(i => i.InvestorId == investorQuery.InvestorId));
            _dbContext.RemoveRange(_dbContext.InvestorContactAddresses.Where(i => i.InvestorId == investorQuery.InvestorId));
            _dbContext.RemoveRange(_dbContext.InvestorIdentifications.Where(i => i.InvestorId == investorQuery.InvestorId));
            _dbContext.RemoveRange(_dbContext.InvestorSales.Where(i => i.InvestorId == investorQuery.InvestorId));
            _dbContext.RemoveRange(_dbContext.InvestorTradingProviders.Where(i => i.InvestorId == investorQuery.InvestorId));

            var userInvestorQuery = _dbContext.Users.FirstOrDefault(u => u.InvestorId == investorQuery.InvestorId && u.UserType == UserTypes.INVESTOR);
            if (userInvestorQuery != null)
            {
                _dbContext.Remove(userInvestorQuery);
                _dbContext.RemoveRange(_dbContext.UsersChatRooms.Where(i => i.UserId == userInvestorQuery.UserId));
                _dbContext.RemoveRange(_dbContext.AuthOtps.Where(i => i.UserId == userInvestorQuery.UserId));
            }
            var saleQuery = _dbContext.Sales.Where(s => s.InvestorId == investorQuery.InvestorId);
            foreach (var sale in saleQuery)
            {
                _dbContext.RemoveRange(_dbContext.SaleTradingProviders.Where(i => i.SaleId == sale.SaleId));
                _dbContext.RemoveRange(_dbContext.DepartmentSales.Where(i => i.SaleId == sale.SaleId));
                _dbContext.RemoveRange(_dbContext.InvestorSales.Where(i => i.SaleId == sale.SaleId));
                _dbContext.RemoveRange(_dbContext.SaleCollabContracts.Where(i => i.SaleId == sale.SaleId));
            }
            _dbContext.RemoveRange(_dbContext.SaleTemps.Where(i => i.InvestorId == investorQuery.InvestorId));
            _dbContext.RemoveRange(_dbContext.Sales.Where(i => i.InvestorId == investorQuery.InvestorId));

            _dbContext.SaveChanges();
        }
    }
}
