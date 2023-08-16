using AutoMapper;
using DocumentFormat.OpenXml.Office2010.CustomUI;
using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using EPIC.CoreDomain.Interfaces;
using EPIC.CoreEntities.DataEntities;
using EPIC.CoreEntities.Dto.BusinessCustomer;
using EPIC.CoreRepositories;
using EPIC.DataAccess.Base;
using EPIC.DataAccess.Models;
using EPIC.Entities;
using EPIC.Entities.DataEntities;
using EPIC.Entities.Dto.BusinessCustomer;
using EPIC.Entities.Dto.CoreApprove;
using EPIC.Entities.Dto.CoreHistoryUpdate;
using EPIC.Entities.Dto.DigitalSign;
using EPIC.Entities.Dto.User;
using EPIC.IdentityRepositories;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Identity;
using EPIC.Utils.DataUtils;
using EPIC.Utils.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Transactions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace EPIC.CoreDomain.Implements
{
    public class BusinessCustomerServices : IBusinessCustomerServices
    {
        private readonly ILogger<BusinessCustomerServices> _logger;
        private readonly IConfiguration _configuration;
        private readonly EpicSchemaDbContext _dbContext;
        private readonly string _connectionString;
        private readonly BusinessCustomerRepository _businessCustomerRepository;
        private readonly BusinessCustomerEFRepository _businessCustomerEFRepository;
        private readonly BusinessCustomerBankEFRepository _businessCustomerBankEFRepository;
        private readonly ApproveRepository _approveRepository;
        private readonly CoreHistoryUpdateRepository _coreHistoryUpdateRepository;
        private readonly UserRepository _userRepository;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IMapper _mapper;

        public BusinessCustomerServices(
            ILogger<BusinessCustomerServices> logger,
            EpicSchemaDbContext dbContext,
            IConfiguration configuration,
            DatabaseOptions databaseOptions,
            IHttpContextAccessor httpContext,
            IMapper mapper)
        {
            _logger = logger;
            _configuration = configuration;
            _dbContext = dbContext;
            _connectionString = databaseOptions.ConnectionString;
            _businessCustomerRepository = new BusinessCustomerRepository(_connectionString, _logger);
            _businessCustomerEFRepository = new BusinessCustomerEFRepository(_dbContext, _logger);
            _businessCustomerBankEFRepository = new BusinessCustomerBankEFRepository(_dbContext, _logger);
            _approveRepository = new ApproveRepository(_connectionString, _logger);
            _userRepository = new UserRepository(_connectionString, _logger);
            _coreHistoryUpdateRepository = new CoreHistoryUpdateRepository(_connectionString, _logger);
            _httpContext = httpContext;
            _mapper = mapper;
        }
        public BusinessCustomer FindBusinessCustomerByTaxCode(string taxCode)
        {
            return _businessCustomerRepository.FindBusinessCustomerByTaxCode(taxCode);
        }

        public BusinessCustomerTemp Add(CreateBusinessCustomerTempDto input)
        {
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            var userType = CommonUtils.GetCurrentUserType(_httpContext);
            int? tradingProviderId = null;
            int? partnerId = null;
            if (userType != UserTypes.EPIC || userType != UserTypes.ROOT_EPIC)
            {
                if (userType == UserTypes.TRADING_PROVIDER || userType == UserTypes.ROOT_TRADING_PROVIDER)
                {
                    tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
                }
                else if (userType == UserTypes.PARTNER || userType == UserTypes.ROOT_PARTNER)
                {
                    partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
                }
            }
            var business = new BusinessCustomerTemp()
            {
                Code = input.Code,
                Name = input.Name,
                ShortName = input.ShortName,
                Address = input.Address,
                TradingAddress = input.TradingAddress,
                Nation = input.Nation,
                Phone = input.Phone,
                Mobile = input.Mobile,
                Email = input.Email,
                TaxCode = input.TaxCode,
                BankAccNo = input.BankAccNo,
                BankAccName = input.BankAccName,
                BankId = input.BankId,
                BankBranchName = input.BankBranchName,
                LicenseDate = input.LicenseDate,
                LicenseIssuer = input.LicenseIssuer,
                Capital = input.Capital,
                RepName = input.RepName,
                RepPosition = input.RepPosition,
                DecisionNo = input.DecisionNo,
                DecisionDate = input.DecisionDate,
                NumberModified = input.NumberModified,
                DateModified = input.DateModified,
                CreatedBy = username,
                RepIdNo = input.RepIdNo,
                RepIdDate = input.RepIdDate,
                RepIdIssuer = input.RepIdIssuer,
                RepSex = input.RepSex,
                RepAddress = input.RepAddress,
                RepBirthDate = input.RepBirthDate,
                BusinessRegistrationImg = input.BusinessRegistrationImg,
                Fanpage = input.Fanpage,
                Website = input.Website,
                AvatarImageUrl = input.AvatarImageUrl,
                StampImageUrl = input.StampImageUrl,
                TradingProviderId = tradingProviderId,
                PartnerId = partnerId
            };
            return _businessCustomerRepository.Add(business);
        }

        /// <summary>
        /// Update dữ liệu ở bảng chính
        /// Nếu là đại lý sửa doanh nghiệp của chính mình thì Update trực tiếp
        /// Nếu không thì Khi Update sẽ tạo bản ghi Tạm để duyệt lên
        /// </summary>
        public BusinessCustomerTemp BusinessCustomerUpdate(CreateBusinessCustomerTempDto input)
        {
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            var userType = CommonUtils.GetCurrentUserType(_httpContext);
            int? tradingProviderId = null;
            int? partnerId = null;
            if (userType != UserTypes.EPIC || userType != UserTypes.ROOT_EPIC)
            {
                if (userType == UserTypes.TRADING_PROVIDER || userType == UserTypes.ROOT_TRADING_PROVIDER)
                {
                    tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);

                    // Kiểm tra xem doanh nghiệp đang sửa có phải là thông tin của đại lý hay không
                    var checkBusinessCustomer = _dbContext.TradingProviders.Any(t => t.TradingProviderId == tradingProviderId && t.BusinessCustomerId == input.BusinessCustomerId && t.Deleted == YesNo.NO);
                    if (checkBusinessCustomer)
                    {
                        // Update trực tiếp 
                        _businessCustomerEFRepository.Update(_mapper.Map<BusinessCustomer>(input));
                        _dbContext.SaveChanges();
                        return null;
                    }
                }
                else if (userType == UserTypes.PARTNER || userType == UserTypes.ROOT_PARTNER)
                {
                    partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
                }
            }

            var business = new BusinessCustomerTemp()
            {
                BusinessCustomerId = input.BusinessCustomerId,
                BusinessCustomerBankId = input.BusinessCustomerBankId,
                Code = input.Code,
                Name = input.Name,
                ShortName = input.ShortName,
                Address = input.Address,
                TradingAddress = input.TradingAddress,
                Nation = input.Nation,
                Phone = input.Phone,
                Mobile = input.Mobile,
                Email = input.Email,
                TaxCode = input.TaxCode,
                BankAccNo = input.BankAccNo,
                BankAccName = input.BankAccName,
                BankId = input.BankId,
                BankBranchName = input.BankBranchName,
                LicenseDate = input.LicenseDate,
                LicenseIssuer = input.LicenseIssuer,
                Capital = input.Capital,
                RepName = input.RepName,
                RepPosition = input.RepPosition,
                DecisionNo = input.DecisionNo,
                DecisionDate = input.DecisionDate,
                NumberModified = input.NumberModified,
                DateModified = input.DateModified,
                CreatedBy = username,
                RepIdNo = input.RepIdNo,
                RepIdDate = input.RepIdDate,
                RepIdIssuer = input.RepIdIssuer,
                RepSex = input.RepSex,
                RepAddress = input.RepAddress,
                RepBirthDate = input.RepBirthDate,
                BusinessRegistrationImg = input.BusinessRegistrationImg,
                Fanpage = input.Fanpage,
                Website = input.Website,
                Server = input.Server,
                Key = input.Key,
                Secret = input.Secret,
                AvatarImageUrl = input.AvatarImageUrl,
                StampImageUrl = input.StampImageUrl
            };
            return _businessCustomerRepository.BusinessCustomerUpdate(business, tradingProviderId, partnerId);
        }

        public PagingResult<BusinessCustomerTemp> FindAll(FilterBusinessCustomerTempDto input)
        {
            var userType = CommonUtils.GetCurrentUserType(_httpContext);
            int? tradingProviderId = userType == UserTypes.TRADING_PROVIDER || userType == UserTypes.ROOT_TRADING_PROVIDER
                                    ? CommonUtils.GetCurrentTradingProviderId(_httpContext) : null;
            int? partnerId = userType == UserTypes.PARTNER || userType == UserTypes.ROOT_PARTNER
                                    ? CommonUtils.GetCurrentPartnerId(_httpContext) : null;

            var result = _businessCustomerEFRepository.FindAllTemp(input, tradingProviderId, partnerId);
            return result;
        }

        public BusinessCustomerTempDto FindById(int id)
        {
            var result = _mapper.Map<BusinessCustomerTempDto>(_businessCustomerRepository.FindTempById(id));
            var businessCustomerFind = _businessCustomerRepository.FindBusinessCusBankDefault(id, IsTemp.YES);
            if (businessCustomerFind != null)
            {
                result.BusinessCustomerBank = _mapper.Map<BusinessCustomerBankDto>(businessCustomerFind);
            }
            return result;
        }

        public BusinessCustomerCheckUpdateDto BusinessCustomerCheckUpdate(int id)
        {
            var result = new BusinessCustomerCheckUpdateDto();
            var businessCustomerTemp = _businessCustomerRepository.FindTempById(id);
            var businessCustomerApprove = _approveRepository.GetOneByTemp(id, CoreApproveDataTypes.EP_CORE_BUSINESS_CUSTOMER);
            if (businessCustomerApprove != null)
            {
                var UserRequest = _userRepository.FindById(businessCustomerApprove.UserRequestId ?? 0);
                result.RequestDate = businessCustomerApprove.RequestDate;
                result.UserRequest = _mapper.Map<UserDto>(UserRequest);
            }
            result.BusinessCustomerTemp = _mapper.Map<BusinessCustomerTempDto>(businessCustomerTemp);
            var listBankTemp = _businessCustomerRepository.FindBankTempByBusinessCustomer(businessCustomerTemp?.BusinessCustomerTempId);

            result.BusinessCustomerTemp.BusinessCustomerBankTemps = _mapper.Map<List<BusinessCustomerBankTempDto>>(listBankTemp);

            if (businessCustomerTemp.BusinessCustomerId != null)
            {
                var businessCustomer = _businessCustomerRepository.FindBusinessCustomerById(businessCustomerTemp.BusinessCustomerId ?? 0);
                result.BusinessCustomer = _mapper.Map<BusinessCustomerDto>(businessCustomer);
                var listBank = _businessCustomerRepository.FindAllBusinessCusBank(businessCustomer.BusinessCustomerId ?? 0, -1, 0, null);
                result.BusinessCustomer.BusinessCustomerBanks = _mapper.Map<List<BusinessCustomerBankDto>>(listBank.Items);
            }
            return result;
        }

        /// <summary>
        /// Cập nhật data trong bảng tạm
        /// </summary>
        /// <param name="id"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        public BusinessCustomerTemp Update(int id, UpdateBusinessCustomerTempDto input)
        {
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            var userType = CommonUtils.GetCurrentUserType(_httpContext);
            int? tradingProviderId = null;
            int? partnerId = null;
            if (userType != UserTypes.EPIC || userType != UserTypes.ROOT_EPIC)
            {
                if (userType == UserTypes.TRADING_PROVIDER || userType == UserTypes.ROOT_TRADING_PROVIDER)
                {
                    tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
                }
                else if (userType == UserTypes.PARTNER || userType == UserTypes.ROOT_PARTNER)
                {
                    partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
                }
            }

            return _businessCustomerRepository.Update(new BusinessCustomerTemp
            {
                BusinessCustomerTempId = id,
                Code = input.Code,
                Name = input.Name,
                ShortName = input.ShortName,
                Address = input.Address,
                TradingAddress = input.TradingAddress,
                Nation = input.Nation,
                Phone = input.Phone,
                Mobile = input.Mobile,
                Email = input.Email,
                TaxCode = input.TaxCode,
                BankAccNo = input.BankAccNo,
                BankAccName = input.BankAccName,
                BankId = input.BankId,
                BankBranchName = input.BankBranchName,
                LicenseDate = input.LicenseDate,
                LicenseIssuer = input.LicenseIssuer,
                Capital = input.Capital,
                RepName = input.RepName,
                RepPosition = input.RepPosition,
                DecisionNo = input.DecisionNo,
                DecisionDate = input.DecisionDate,
                NumberModified = input.NumberModified,
                DateModified = input.DateModified,
                ModifiedBy = username,
                RepIdNo = input.RepIdNo,
                RepIdDate = input.RepIdDate,
                RepIdIssuer = input.RepIdIssuer,
                RepSex = input.RepSex,
                RepAddress = input.RepAddress,
                RepBirthDate = input.RepBirthDate,
                BusinessRegistrationImg = input.BusinessRegistrationImg,
                Fanpage = input.Fanpage,
                Website = input.Website,
                Server = input.Server,
                Key = input.Key,
                Secret = input.Secret,
                AvatarImageUrl = input.AvatarImageUrl,
                StampImageUrl = input.StampImageUrl
            }, tradingProviderId, partnerId);
        }

        public PagingResult<BusinessCustomerDto> FindAllBusinessCustomer(FilterBusinessCustomerDto input)
        {
            var userType = CommonUtils.GetCurrentUserType(_httpContext);
            int? tradingProviderId = userType == UserTypes.TRADING_PROVIDER || userType == UserTypes.ROOT_TRADING_PROVIDER
                            ? CommonUtils.GetCurrentTradingProviderId(_httpContext)
                            : null;
            int? partnerId = userType == UserTypes.PARTNER || userType == UserTypes.ROOT_PARTNER
                            ? CommonUtils.GetCurrentPartnerId(_httpContext)
                            : null;

            // lấy danh sách thông tin doanh nghiệp và bổ sung thông tin
            var businessCustomers = _businessCustomerEFRepository.GetAll(input, tradingProviderId, partnerId);
            var result = new PagingResult<BusinessCustomerDto>
            {
                TotalItems = businessCustomers.Count(),
            };
            var resultItem = new List<BusinessCustomerDto>();
            //Sort
            businessCustomers = businessCustomers.OrderDynamic(input.Sort);
            foreach (var item in businessCustomers)
            {
                item.Email = StringUtils.HideEmail(item.Email);
                item.CifCode = _dbContext.CifCodes.FirstOrDefault(cc => cc.BusinessCustomerId == item.BusinessCustomerId && cc.Deleted == YesNo.NO)?.CifCode;
                resultItem.Add(item);
            }

            result.Items = resultItem;
            if (input.PageSize != -1) 
            {
                result.Items = result.Items.Skip(input.Skip).Take(input.PageSize);
            }

            return result;
        }

        public PagingResult<BusinessCustomerDto> FindAllBusinessCustomerByTaxCode(int pageSize, int pageNumber, string keyword)
        {
            var businessCustomerList = _businessCustomerRepository.FindAllBusinessCustomerByTaxCode(pageSize, pageNumber, keyword);
            var result = new PagingResult<BusinessCustomerDto>();
            var items = new List<BusinessCustomerDto>();
            result.TotalItems = businessCustomerList.TotalItems;
            foreach (var businessCustomerFind in businessCustomerList.Items)
            {
                var businessCustomer = _mapper.Map<BusinessCustomerDto>(businessCustomerFind);
                if (businessCustomerFind.BusinessCustomerId != null)
                {
                    var listBank = _businessCustomerRepository.FindAllBusinessCusBank(businessCustomerFind.BusinessCustomerId ?? 0, -1, 0, null);
                    var businessCustomerBankList = new List<BusinessCustomerBankDto>();
                    foreach (var item in listBank.Items)
                    {
                        var businessCustomerBank = _mapper.Map<BusinessCustomerBankDto>(item);
                        businessCustomerBank.BusinessCustomerBankAccId = item.BusinessCustomerBankAccId;
                        businessCustomerBankList.Add(businessCustomerBank);
                    }
                    businessCustomer.BusinessCustomerBanks = businessCustomerBankList;
                }
                items.Add(businessCustomer);
            }
            result.Items = items;
            return result;
        }

        public BusinessCustomerDto FindBusinessCustomerById(int id)
        {
            var userType = CommonUtils.GetCurrentUserType(_httpContext);

            var result = _mapper.Map<BusinessCustomerDto>(_businessCustomerRepository.FindBusinessCustomerById(id));

            if (userType == UserTypes.TRADING_PROVIDER || userType == UserTypes.ROOT_TRADING_PROVIDER)
            {
                // Kiểm tra xem doanh nghiệp có phải của đại lý hay không
                var checkBusinessCustomer = _dbContext.TradingProviders.Any(t => t.TradingProviderId == CommonUtils.GetCurrentTradingProviderId(_httpContext)
                    && t.BusinessCustomerId == id && t.Deleted == YesNo.NO);
                if (checkBusinessCustomer)
                {
                    result.IsAccountLogin = true;
                }
            }
            var businessCustomerFind = _businessCustomerRepository.FindBusinessCusBankDefault(id, IsTemp.NO);
            if (businessCustomerFind != null)
            {
                result.BusinessCustomerBank = _mapper.Map<BusinessCustomerBankDto>(businessCustomerFind);
            }
            return result;
        }

        public PagingResult<BusinessCustomerBank> FindAllBusinessCusBank(int businessCustomerId, int pageSize, int pageNumber, string keyword)
        {

            return _businessCustomerRepository.FindAllBusinessCusBank(businessCustomerId, pageSize, pageNumber, keyword);
        }

        public BusinessCustomerBank FindBusinessCusBankById(int id)
        {
            return _businessCustomerRepository.FindBusinessCusBankById(id);
        }

        public BusinessCustomerBankTemp BusinessCustomerBankAdd(CreateBusinessCustomerBankTempDto input)
        {
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            var userType = CommonUtils.GetCurrentUserType(_httpContext);
            int? tradingProviderId = null;
            int? partnerId = null;
            if (userType != UserTypes.EPIC || userType != UserTypes.ROOT_EPIC)
            {
                if (userType == UserTypes.TRADING_PROVIDER || userType == UserTypes.ROOT_TRADING_PROVIDER)
                {
                    tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
                    // Kiểm tra xem doanh nghiệp đang sửa có phải là thông tin của đại lý hay không
                    var checkBusinessCustomer = _dbContext.TradingProviders.Any(t => t.TradingProviderId == tradingProviderId && t.BusinessCustomerId == input.BusinessCustomerId && t.Deleted == YesNo.NO);
                    if (checkBusinessCustomer && !input.IsTemp)
                    {
                        // Update trực tiếp 
                        _businessCustomerBankEFRepository.Add(new BusinessCustomerBank
                        {
                            BusinessCustomerId = input.BusinessCustomerId ?? 0,
                            BankId = input.BankId,
                            BankAccName = input.BankAccName,
                            BankAccNo = input.BankAccNo,
                            BankBranchName = input.BankBranchName,
                            BankName = input.BankName,
                            CreatedBy = username
                        });
                        _dbContext.SaveChanges();
                        return null;
                    }
                }
                else if (userType == UserTypes.PARTNER || userType == UserTypes.ROOT_PARTNER)
                {
                    partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
                }
            }

            var businessCustomers = _businessCustomerRepository.FindAllBusinessCusBankNoPage(input.BusinessCustomerId ?? 0);
            foreach (var businessCustomer in businessCustomers)
            {
                if (businessCustomer.BankAccNo == input.BankAccNo && businessCustomer.BankName == input.BankName)
                {
                    throw new FaultException(new FaultReason($"Số tài khoản đã tồn tại"), new FaultCode(((int)ErrorCode.CoreBankNoAndBankIsExists).ToString()), "");
                }
            }

            var bank = new BusinessCustomerBankTemp()
            {
                BusinessCustomerId = input.BusinessCustomerId,
                BankAccNo = input.BankAccNo,
                BankAccName = input.BankAccName,
                BankId = input.BankId,
                BankBranchName = input.BankBranchName,
                IsDefault = input.IsDefault,
                IsTemp = GetIsTemp(input.IsTemp),
                CreatedBy = username
            };
            return _businessCustomerRepository.BusinessCustomerBankAdd(bank, tradingProviderId, partnerId);
        }

        public BusinessCustomerBankTemp BusinessCustomerBankUpdate(int id, UpdateBusinessCustomerBankTempDto input)
        {
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            var userType = CommonUtils.GetCurrentUserType(_httpContext);
            int? tradingProviderId = null;
            int? partnerId = null;
            if (userType != UserTypes.EPIC || userType != UserTypes.ROOT_EPIC)
            {
                if (userType == UserTypes.TRADING_PROVIDER || userType == UserTypes.ROOT_TRADING_PROVIDER)
                {
                    tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
                    // Kiểm tra xem doanh nghiệp đang sửa có phải là thông tin của đại lý hay không
                    var checkBusinessCustomer = _dbContext.TradingProviders.Any(t => t.TradingProviderId == tradingProviderId && t.BusinessCustomerId == input.BusinessCustomerId && t.Deleted == YesNo.NO);
                    if (checkBusinessCustomer && !input.IsTemp)
                    {
                        // Update trực tiếp 
                        _businessCustomerBankEFRepository.Update(new BusinessCustomerBank
                        {
                            BusinessCustomerBankAccId = id,
                            BusinessCustomerId = input.BusinessCustomerId ?? 0,
                            BankId = input.BankId,
                            BankAccName = input.BankAccName,
                            BankAccNo = input.BankAccNo,
                            BankBranchName = input.BankBranchName,
                            BankName = input.BankName,
                            ModifiedBy = username
                        });
                        _dbContext.SaveChanges();
                        return null;
                    }
                }
                else if (userType == UserTypes.PARTNER || userType == UserTypes.ROOT_PARTNER)
                {
                    partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
                }
            }

            return _businessCustomerRepository.BusinessCustomerBankUpdate(new BusinessCustomerBankTemp
            {
                Id = id,
                BusinessCustomerId = input.BusinessCustomerId,
                BankAccName = input.BankAccName,
                BankAccNo = input.BankAccNo,
                BankId = input.BankId,
                BankBranchName = input.BankBranchName,
                ModifiedBy = username
            }, tradingProviderId, partnerId);
        }

        public int ActiveBusinessCustomerBank(int id, bool isActive)
        {
            return _businessCustomerRepository.BusinessCustomerActive(id, isActive);
        }

        public int BusinessCustomerDelete(int id)
        {
            return _businessCustomerRepository.BusinessCustomerDelete(id);
        }

        public int BusinessCustomerBankDelete(int id)
        {
            return _businessCustomerRepository.BusinessCustomerBankDelete(id);
        }

        /// <summary>
        /// Yêu cầu duyệt thêm mới thông tin doanh nghiệp từ bảng tạm 
        /// </summary>
        /// <param name="businessCustomerId"></param>
        /// <param name="dto"></param>
        public void Request(RequestBusinessCustomerDto input)
        {
            var userType = CommonUtils.GetCurrentUserType(_httpContext);
            int? tradingProviderId = null;
            int? partnerId = null;
            if (userType != UserTypes.EPIC || userType != UserTypes.ROOT_EPIC)
            {
                if (userType == UserTypes.TRADING_PROVIDER || userType == UserTypes.ROOT_TRADING_PROVIDER)
                {
                    tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
                }
                else if (userType == UserTypes.PARTNER || userType == UserTypes.ROOT_PARTNER)
                {
                    partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
                }
            }
            var userId = CommonUtils.GetCurrentUserId(_httpContext);
            using (var transaction = new TransactionScope())
            {
                var result = new CreateApproveRequestDto
                {
                    UserRequestId = userId,
                    UserApproveId = input.UserApproveId,
                    RequestNote = input.RequestNote,
                    ActionType = input.ActionType,
                    DataType = CoreApproveDataType.BUSINESS_CUSTOMER,
                    ReferIdTemp = input.Id,
                    Summary = input.Summary
                };
                //Nếu là hành động cập nhật thì lấy thêm Id thật 
                if (input.ActionType == ActionTypes.CAP_NHAT)
                {
                    var data = _businessCustomerRepository.FindTempById(input.Id);
                    if (data == null)
                    {
                        throw new FaultException(new FaultReason($"Không tìm thấy thông tin doanh nghiệp"), new FaultCode(((int)ErrorCode.CoreBusinessCustomerNotFound).ToString()), "");
                    }
                    result.ReferId = data.BusinessCustomerId ?? 0;
                }

                var approve = _approveRepository.CreateApproveRequest(result, tradingProviderId, partnerId);
                if (approve.ReferId != 0)
                {
                    var listHistoryUpdates = new List<CoreHistoryUpdate>();
                    #region check update investor
                    var businessCustomer = _businessCustomerRepository.FindBusinessCustomerById(approve.ReferId ?? 0);
                    var businessCustomerTemp = _businessCustomerRepository.FindTempById(approve.ReferIdTemp ?? 0);
                    var businessCustomerCheck = _mapper.Map<HistoryUpdateBusinessCustomerDto>(businessCustomer);
                    var businessCustomerTempCheck = _mapper.Map<HistoryUpdateBusinessCustomerDto>(businessCustomerTemp);
                    if (businessCustomerCheck != null && businessCustomerTempCheck != null)
                    {
                        var differences = CompareObject.DetailedCompare(businessCustomerCheck, businessCustomerTempCheck);
                        foreach (var dis in differences)
                        {
                            var historyUpdateDto = new CoreHistoryUpdate()
                            {
                                ApproveId = approve.ApproveID,
                                RealTableId = approve.ReferId ?? 0,
                                OldValue = dis.OldName?.ToString(),
                                NewValue = dis.NewValue?.ToString(),
                                FieldName = dis.FieldName,
                                CreatedBy = approve?.UserRequestId.ToString(),
                                UpdateTable = CoreHistoryUpdateTable.BUSINESS_CUSTOMER,
                            };
                            listHistoryUpdates.Add(historyUpdateDto);
                        }
                    }
                    #endregion

                    #region check update bank
                    var businessCustomerBankTemps = _businessCustomerRepository.FindBankTempByBusinessCustomer(approve.ReferId);
                    if (businessCustomerBankTemps != null)
                    {
                        foreach (var businessCustomerBankTemp in businessCustomerBankTemps)
                        {
                            if (businessCustomerBankTemp.BusinessCustomerBankAccId != null)
                            {
                                var businessCustomerBank = _businessCustomerRepository.FindBusinessCusBankById(businessCustomerBankTemp.BusinessCustomerBankAccId ?? 0);
                                if (businessCustomerBank != null)
                                {
                                    var bankCheck = _mapper.Map<HistoryUpdateBusinessCustomerBankDto>(businessCustomerBank);
                                    var bankTempCheck = _mapper.Map<HistoryUpdateBusinessCustomerBankDto>(businessCustomerBankTemp);
                                    if (bankCheck != null && bankTempCheck != null)
                                    {
                                        var differences = CompareObject.DetailedCompare(bankCheck, bankTempCheck);
                                        foreach (var dis in differences)
                                        {
                                            var historyUpdateDto = new CoreHistoryUpdate()
                                            {
                                                ApproveId = approve.ApproveID,
                                                RealTableId = businessCustomerBankTemp.BusinessCustomerBankAccId ?? 0,
                                                OldValue = dis.OldName?.ToString(),
                                                NewValue = dis.NewValue?.ToString(),
                                                FieldName = dis.FieldName,
                                                CreatedBy = approve?.UserRequestId.ToString(),
                                                UpdateTable = CoreHistoryUpdateTable.BUSINESS_CUSTOMER_BANK,
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

                //Chuyển trạng thái của bảng tạm sang trình duyệt
                _businessCustomerRepository.BusinessCustomerTempRequest(input.Id);
                transaction.Complete();
            }
            _businessCustomerRepository.CloseConnection();
        }

        /// <summary>
        /// Phê duyệt thông tin doanh nghiệp
        /// </summary>
        /// <param name="businessCustomerId"></param>
        /// <param name="input"></param>
        public int Approve(ApproveBusinessCustomerDto input)
        {
            var userId = CommonUtils.GetCurrentUserId(_httpContext);
            var businessCustomerId = 0;
            using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                var approve = _approveRepository.GetOneByTemp(input.Id, CoreApproveDataType.BUSINESS_CUSTOMER);

                if (approve == null)
                {
                    throw new FaultException(new FaultReason($"Không tìm thấy thông tin trình duyệt"), new FaultCode(((int)ErrorCode.CoreApproveNotFound).ToString()), "");
                }
                businessCustomerId = approve.ReferId ?? 0;

                //Nếu không truyền loại hành động thì mặc định là thêm mới
                if (approve.ActionType == null)
                {
                    approve.ActionType = ActionTypes.THEM_MOI;
                }

                //Nếu là thêm
                else if (approve.ActionType == ActionTypes.THEM_MOI)
                {
                    //Khi thêm mới từ bảng Temp, Repo sẽ thêm Id thật vào bảng CORE_APPROVE để  xác minh
                    var addApprove = _businessCustomerRepository.AddApprove(input.Id);
                    businessCustomerId = addApprove.ReferId ?? 0;
                }

                //Nếu là cập nhật
                else if (approve.ActionType == ActionTypes.CAP_NHAT)
                {
                    _businessCustomerRepository.UpdateApprove(approve.ReferId);
                }

                _approveRepository.ApproveRequestData(new ApproveRequestDto
                {
                    ReferId = approve.ReferId ?? 0,
                    ApproveID = approve.ApproveID,
                    ApproveNote = input.ApproveNote,
                    UserApproveId = userId
                });
                transaction.Complete();
            }
            _businessCustomerRepository.CloseConnection();
            return businessCustomerId;
        }

        /// <summary>
        /// EPIC Xac minh, truyền bằng Id thật
        /// </summary>
        /// <param name="input"></param>
        public void Check(CheckBusinessCustomerDto input)
        {
            var userid = CommonUtils.GetCurrentUserId(_httpContext);
            using (var transaction = new TransactionScope())
            {
                var approve = _approveRepository.GetOneByActual(input.Id, CoreApproveDataType.BUSINESS_CUSTOMER);
                if (approve == null)
                {
                    throw new FaultException(new FaultReason($"Không tìm thấy thông tin trình duyệt"), new FaultCode(((int)ErrorCode.CoreApproveNotFound).ToString()), "");
                }

                _approveRepository.CheckRequest(new CheckRequestDto
                {
                    ApproveID = approve.ApproveID,
                    UserCheckId = userid,
                });

                // Cập nhật là đã Xác Minh vào bảng Chính
                _businessCustomerRepository.Check(input.Id);

                transaction.Complete();
            }
            _businessCustomerRepository.CloseConnection();
        }

        /// <summary>
        /// Huy khi đang trình duyệt
        /// </summary>
        public void Cancel(CancelBusinessCustomerDto input)
        {
            var userid = CommonUtils.GetCurrentUserId(_httpContext);
            using (var transaction = new TransactionScope())
            {
                var approve = _approveRepository.GetOneByTemp(input.Id, CoreApproveDataType.BUSINESS_CUSTOMER);
                if (approve == null)
                {
                    throw new FaultException(new FaultReason($"Không tìm thấy thông tin trình duyệt"), new FaultCode(((int)ErrorCode.CoreApproveNotFound).ToString()), "");
                }

                _approveRepository.CancelRequest(new CancelRequestDto
                {
                    ApproveID = approve.ApproveID,
                    CancelNote = input.CancelNote,
                });

                _businessCustomerRepository.Cancel(input.Id);

                transaction.Complete();
            }
            _businessCustomerRepository.CloseConnection();
        }

        public int BankSetDefault(BusinessCustomerBankDefault input)
        {
            return _businessCustomerRepository.BankSetDefault(new BusinessCustomerBankDefault
            {
                BusinessCustomerBankAccId = input.BusinessCustomerBankAccId,
                BusinessCustomerId = input.BusinessCustomerId
            });
        }

        public int BankTempSetDefault(BusinessCustomerBankTempDefault input)
        {
            return _businessCustomerRepository.BankTempSetDefault(new BusinessCustomerBankTempDefault
            {
                Id = input.Id,
                BusinessCustomerTempId = input.BusinessCustomerTempId
            });
        }

        public int BusinessCustomerBankTempUpdate(int id, UpdateBusinessCustomerBankTempDto input)
        {
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            var userType = CommonUtils.GetCurrentUserType(_httpContext);
            int? tradingProviderId = null;
            int? partnerId = null;
            if (userType != UserTypes.EPIC || userType != UserTypes.ROOT_EPIC)
            {
                if (userType == UserTypes.TRADING_PROVIDER || userType == UserTypes.ROOT_TRADING_PROVIDER)
                {
                    tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
                }
                else if (userType == UserTypes.PARTNER || userType == UserTypes.ROOT_PARTNER)
                {
                    partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
                }
            }

            return _businessCustomerRepository.BusinessCustomerBankTempUpdate(new BusinessCustomerBankTemp
            {
                Id = id,
                BusinessCustomerTempId = input.BusinessCustomerTempId,
                BankAccName = input.BankAccName,
                BankAccNo = input.BankAccNo,
                BankId = input.BankId,
                BankBranchName = input.BankBranchName,
                ModifiedBy = username
            }, tradingProviderId, partnerId);
        }

        public BusinessCustomerBankTemp FindBusinessCustomerBankTempById(int id)
        {
            return _businessCustomerRepository.FindBusinessCustomerBankTempById(id);
        }

        public List<BusinessCustomerBankTempDto> FindBankTempByBusinessCustomer(int id)
        {
            var find = _businessCustomerRepository.FindBankTempByBusinessCustomer(id);
            return _mapper.Map<List<BusinessCustomerBankTempDto>>(find);
        }

        private int GetIsTemp(bool isTemp)
        {
            return isTemp ? 1 : 0;
        }

        public int UpdateDigitalSignTemp(int? businessCustomerTempId, DigitalSignDto input)
        {
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            var userType = CommonUtils.GetCurrentUserType(_httpContext);
            int? tradingProviderId = null;
            int? partnerId = null;
            if (userType != UserTypes.EPIC || userType != UserTypes.ROOT_EPIC)
            {
                if (userType == UserTypes.TRADING_PROVIDER || userType == UserTypes.ROOT_TRADING_PROVIDER)
                {
                    tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
                }
                else if (userType == UserTypes.PARTNER || userType == UserTypes.ROOT_PARTNER)
                {
                    partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
                }
            }

            return _businessCustomerRepository.UpdateDigitalSignTemp(businessCustomerTempId, tradingProviderId, partnerId, new DigitalSign
            {
                ModifiedBy = username,
                Server = input.Server,
                Key = input.Key,
                Secret = input.Secret,
                StampImageUrl = input.StampImageUrl,
            });
        }

        public DigitalSign UpdateDigitalSign(int? businessCustomerId, DigitalSignDto input)
        {
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            var userType = CommonUtils.GetCurrentUserType(_httpContext);
            int? tradingProviderId = null;
            int? partnerId = null;
            if (userType != UserTypes.EPIC || userType != UserTypes.ROOT_EPIC)
            {
                if (userType == UserTypes.TRADING_PROVIDER || userType == UserTypes.ROOT_TRADING_PROVIDER)
                {
                    tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
                }
                else if (userType == UserTypes.PARTNER || userType == UserTypes.ROOT_PARTNER)
                {
                    partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
                }
            }

            return _businessCustomerRepository.UpdateDigitalSign(businessCustomerId, tradingProviderId, partnerId, new DigitalSign
            {
                ModifiedBy = username,
                Server = input.Server,
                Key = input.Key,
                Secret = input.Secret,
                StampImageUrl = input.StampImageUrl,
            });
        }

        public DigitalSignDto GetDigitalSignTemp(int? businessCustomerTempId)
        {
            var digitalSignTempFind = _businessCustomerRepository.GetDigitalSignTemp(businessCustomerTempId);
            var result = new DigitalSignDto();
            if (digitalSignTempFind != null)
            {
                result.Key = digitalSignTempFind.Key;
                result.Secret = digitalSignTempFind.Secret;
                result.Server = digitalSignTempFind.Server;
                result.StampImageUrl = digitalSignTempFind.StampImageUrl;
            }

            return result;
        }

        public DigitalSignDto GetDigitalSign(int? businessCustomerId)
        {
            var digitalSignFind = _businessCustomerRepository.GetDigitalSign(businessCustomerId);
            var result = new DigitalSignDto();
            if (digitalSignFind != null)
            {

                result.Key = digitalSignFind.Key;
                result.Secret = digitalSignFind.Secret;
                result.Server = digitalSignFind.Server;
                result.StampImageUrl = digitalSignFind.StampImageUrl;
            }

            return result;
        }
    }
}
