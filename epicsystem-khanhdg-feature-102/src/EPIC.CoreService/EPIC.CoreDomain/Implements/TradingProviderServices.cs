using AutoMapper;
using DocumentFormat.OpenXml.Office2010.Excel;
using EPIC.BondRepositories;
using EPIC.CoreDomain.Interfaces;
using EPIC.CoreRepositories;
using EPIC.DataAccess.Base;
using EPIC.DataAccess.Models;
using EPIC.Entities;
using EPIC.Entities.DataEntities;
using EPIC.Entities.Dto.BusinessCustomer;
using EPIC.Entities.Dto.DigitalSign;
using EPIC.Entities.Dto.RocketChat;
using EPIC.Entities.Dto.TradingProvider;
using EPIC.Entities.Dto.User;
using EPIC.IdentityEntities.DataEntities;
using EPIC.IdentityRepositories;
using EPIC.RocketchatDomain.Interfaces;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MySqlX.XDevAPI.Common;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

namespace EPIC.CoreDomain.Implements
{
    public class TradingProviderServices : ITradingProviderServices
    {
        private readonly ILogger<TradingProviderServices> _logger;
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;
        private readonly IRocketChatServices _rocketChatServices;
        private readonly TradingProviderRepository _tradingProviderRepository;
        private readonly TradingProviderUserRepository _tradingProviderUserRepository;
        private readonly BusinessCustomerRepository _businessCustomerRepository;
        private readonly TradingProviderEFRepository _tradingProviderEFRepository;
        private readonly BusinessCustomerEFRepository _businessCustomerEFRepository;
        private readonly BusinessCustomerBankEFRepository _businessCustomerBankEFRepository;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IMapper _mapper;
        private readonly EpicSchemaDbContext _dbContext;


        public TradingProviderServices(EpicSchemaDbContext dbContext,
            ILogger<TradingProviderServices> logger, IConfiguration configuration, DatabaseOptions databaseOptions, IHttpContextAccessor httpContext, IMapper mapper, IRocketChatServices rocketChatServices)
        {
            _dbContext = dbContext;
            _logger = logger;
            _configuration = configuration;
            _connectionString = databaseOptions.ConnectionString;
            _rocketChatServices = rocketChatServices;
            _tradingProviderRepository = new TradingProviderRepository(_connectionString, _logger);
            _tradingProviderUserRepository = new TradingProviderUserRepository(_connectionString, _logger);
            _businessCustomerRepository = new BusinessCustomerRepository(_connectionString, _logger);
            _httpContext = httpContext;
            _mapper = mapper;
            _tradingProviderEFRepository = new TradingProviderEFRepository(_dbContext, _logger);
            _businessCustomerEFRepository = new BusinessCustomerEFRepository(_dbContext, _logger);
            _businessCustomerBankEFRepository = new BusinessCustomerBankEFRepository(_dbContext, _logger);
        }

        public async Task<TradingProvider> Add(CreateTradingProviderDto input)
        {
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            var partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);

            using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                var tradingProvider = new TradingProvider
                {
                    BusinessCustomerId = input.BusinessCustomerId,
                    CreatedBy = username,
                    AliasName = input.AliasName,
                };

                var result = _tradingProviderRepository.Add(tradingProvider, partnerId);
                var businessCustomer = _businessCustomerRepository.FindBusinessCustomerById(result.BusinessCustomerId);

                await _rocketChatServices.LoginAdmin();
                await _rocketChatServices.CreateDepartment(new CreateDepartmentDto
                {
                    Department = new CreateDepartmentRocketchat
                    {
                        Name = _rocketChatServices.genChannelName(result, businessCustomer),
                        Email = businessCustomer.Email ?? $"DLSC-{tradingProvider.TradingProviderId}@epicpartner.vn",
                        Description = businessCustomer.Name,
                        Enabled = true,
                        ShowOnOfflineForm = true,
                        ShowOnRegistration = true,
                    },
                    Agents = new List<Agent>(),
                });

                transaction.Complete();

                return result;
            }

        }

        public int Delete(int id)
        {
            return _tradingProviderRepository.Delete(id);
        }

        public PagingResult<TradingProviderDto> Find(int pageSize, int pageNumber, string keyword, int? status)
        {
            int? partnerId = -1;
            string usertype = CommonUtils.GetCurrentUserType(_httpContext);
            if (new string[] {UserTypes.PARTNER, UserTypes.ROOT_PARTNER}.Contains(usertype))
            {
                partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
            } 
            else if (new string[] { UserTypes.EPIC, UserTypes.ROOT_EPIC }.Contains(usertype))
            {
                partnerId = null;
            }

           var tradingProviderList = _tradingProviderRepository.FindAll(partnerId, pageSize, pageNumber, keyword, status);

            var result = new PagingResult<TradingProviderDto>();
            var items = new List<TradingProviderDto>();
            result.TotalItems = tradingProviderList.TotalItems;
            foreach (var tradingFind in tradingProviderList.Items)
            {
                var bondTrading = new TradingProviderDto()
                {
                    TradingProviderId = tradingFind.TradingProviderId,
                    BusinessCustomerId = tradingFind.BusinessCustomerId,
                };

                var tradingProviderFind = _tradingProviderEFRepository.EntityNoTracking.FirstOrDefault(o => o.TradingProviderId == tradingFind.TradingProviderId);
                if (tradingProviderFind != null)
                {
                    bondTrading.Status = tradingProviderFind.Status;
                }
                

                //var businessCustomer = _businessCustomerRepository.FindBusinessCustomerById(tradingFind.BusinessCustomerId ?? 0);
                var businessCustomer = _businessCustomerEFRepository.FindById(tradingFind.BusinessCustomerId ?? 0);
                if (businessCustomer != null)
                {
                    bondTrading.Name = businessCustomer.Name;
                    bondTrading.Code = businessCustomer.Code;
                    bondTrading.AliasName = tradingFind.AliasName;
                    bondTrading.BusinessCustomer = businessCustomer;

                    //var listBank = _businessCustomerRepository.FindAllBusinessCusBank(businessCustomer.BusinessCustomerId ?? 0, -1, 0, null);
                    var listBank = _businessCustomerEFRepository.GetListBankByBusinessCustomerId(businessCustomer.BusinessCustomerId ?? 0);
                    bondTrading.BusinessCustomer.BusinessCustomerBanks = listBank.ToList();
                }
                items.Add(bondTrading);
            }
            result.Items = items;
            return result;
        }

        public TradingProviderDto FindById(int id)
        {
            var tradingProvider = _tradingProviderRepository.FindById(id);
            var result = new TradingProviderDto()
            {
                TradingProviderId = tradingProvider.TradingProviderId,
                BusinessCustomerId = tradingProvider.BusinessCustomerId,
                AliasName = tradingProvider.AliasName
            };

            var businessCustomer = _businessCustomerRepository.FindBusinessCustomerById(tradingProvider.BusinessCustomerId);
            if (businessCustomer != null)
            {
                result.Name = businessCustomer.Name;
                result.Code = businessCustomer.Code;
                result.BusinessCustomer = _mapper.Map<BusinessCustomerDto>(businessCustomer);
                var businessCustomerBank = _businessCustomerRepository.FindBusinessCusBankDefault(businessCustomer.BusinessCustomerId ?? 0, IsTemp.NO);
                if (businessCustomerBank != null)
                {
                    result.BusinessCustomer.BusinessCustomerBank = _mapper.Map<BusinessCustomerBankDto>(businessCustomerBank);
                }
                var listBank = _businessCustomerRepository.FindAllBusinessCusBank(businessCustomer.BusinessCustomerId ?? 0, -1, 0, null);
                result.BusinessCustomer.BusinessCustomerBanks = _mapper.Map<List<BusinessCustomerBankDto>>(listBank.Items);
            }
            return result;
        }

        /// <summary>
        /// Lấy thông tin doanh nghiệp của đại lý đang đăng nhập
        /// </summary>
        /// <returns></returns>
        public BusinessCustomerDto GetInfoTradingProviderCurrent()
        {
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var tradingProviderFind = _dbContext.TradingProviders.FirstOrDefault(x => x.TradingProviderId == tradingProviderId && x.Deleted == YesNo.NO)
                .ThrowIfNull(_dbContext, ErrorCode.TradingProviderNotFound);
            var result = _businessCustomerEFRepository.FindById(tradingProviderFind.BusinessCustomerId);
            if (result != null)
            {
                result.BusinessCustomerBank = _businessCustomerEFRepository.GetListBankByBusinessCustomerId(tradingProviderFind.BusinessCustomerId)?.FirstOrDefault();
            }
            return result;
        }
        public int ActiveUser(int userId, bool isActive)
        {
            int tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            return _tradingProviderUserRepository.Active(userId, isActive,
                CommonUtils.GetCurrentUsername(_httpContext), tradingProviderId);
        }

        public void ChangePassword(ChangePasswordDto input)
        {
            int tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            int userId = CommonUtils.GetCurrentUserId(_httpContext);
            _tradingProviderUserRepository.ChangePassword(userId, input.OldPassword,
                input.NewPassword, tradingProviderId);
        }

        public Users CreateUser(CreateUserDto model)
        {
            int tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var result = _tradingProviderUserRepository.Add(new Users
            {
                TradingProviderId = tradingProviderId,
                DisplayName = model.DisplayName,
                UserName = model.UserName,
                Password = model.Password,
                Email = model.Email,
                CreatedBy = CommonUtils.GetCurrentUsername(_httpContext)
            });
            result.Password = null;
            return result;
        }

        public PagingResult<Users> FindAll(int pageSize, int pageNumber, string keyword)
        {
            var result = _tradingProviderUserRepository.FindAll(pageSize, pageNumber, keyword);
            foreach (var user in result.Items)
            {
                user.Password = null;
            }
            return result;
        }

        public int DeleteUser(int userId)
        {
            int tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            return _tradingProviderUserRepository.Delete(userId, CommonUtils.GetCurrentUsername(_httpContext), tradingProviderId);
        }

        public int UpdateUser(int id, UpdateUserDto model)
        {
            int tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            return _tradingProviderUserRepository.Update(new Users
            {
                TradingProviderId = tradingProviderId,
                UserId = id,
                DisplayName = model.DisplayName,
                Email = model.Email,
                ModifiedBy = CommonUtils.GetCurrentUsername(_httpContext)
            });
        }

        public List<ViewTradingProviderDto> FindByTaxCode(string taxCode)
        {
            return _tradingProviderRepository.FindByTaxCode(taxCode);
        }

        public List<BusinessCustomerBankDto> FindBankByTrading(int? distributionId = null, int? bankId = null)
        {
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            return _tradingProviderRepository.FindBankByTrading(tradingProviderId, distributionId, bankId);
        }

        public List<BusinessCustomerBankDto> FindBankByTradingInvest()
        {
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            return _tradingProviderEFRepository.FindBankByTradingInvest(tradingProviderId);
        }

        public DigitalSignDto GetTradingDigitalSign()
        {
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var digitalSignFind = _tradingProviderRepository.GetDigitalSign(tradingProviderId);
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

        public DigitalSign UpdateTradingDigitalSign(DigitalSignDto input)
        {
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);

            return _tradingProviderRepository.UpdateDigitalSign(tradingProviderId, new DigitalSign
            {
                ModifiedBy = username,
                Server = input.Server,
                Key = input.Key,
                Secret = input.Secret,
                StampImageUrl = input.StampImageUrl,
            });
        }

        /// <summary>
        /// Thay đổi trạng thái tradingProvider
        /// </summary>
        /// <param name="tradingProviderId"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public TradingProvider ChangeStatus(int tradingProviderId, int status)
        {
            var userType = CommonUtils.GetCurrentUserType(_httpContext);
            _logger.LogInformation($"{nameof(ChangeStatus)}: tradingProviderId = {tradingProviderId}, status = {status}, userType = {userType}");
            var tradingProviderFind = _tradingProviderEFRepository.EntityNoTracking.FirstOrDefault(o => o.TradingProviderId == tradingProviderId && o.Deleted == YesNo.NO)
                                    .ThrowIfNull(_dbContext, ErrorCode.TradingProviderNotFound);
            var result = new TradingProvider();
            if (userType == UserTypes.ROOT_EPIC || userType == UserTypes.ROOT_PARTNER || userType == UserTypes.PARTNER)
            {
                result = _tradingProviderEFRepository.ChangeStatus(tradingProviderId, status);
            }
            _dbContext.SaveChanges();
            return result;
        }
    }
}
