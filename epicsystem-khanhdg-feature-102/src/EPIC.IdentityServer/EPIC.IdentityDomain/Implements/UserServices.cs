using AutoMapper;
using EPIC.CoreRepositories;
using EPIC.DataAccess.Base;
using EPIC.DataAccess.Models;
using EPIC.Entities;
using EPIC.Entities.DataEntities;
using EPIC.Entities.Dto.BondInvestorAccount;
using EPIC.Entities.Dto.RocketChat;
using EPIC.Entities.Dto.User;
using EPIC.IdentityDomain.Interfaces;
using EPIC.IdentityEntities.DataEntities;
using EPIC.IdentityEntities.Dto.UsersChat;
using EPIC.IdentityRepositories;
using EPIC.RealEstateEntities.DataEntities;
using EPIC.RealEstateEntities.Dto.RstConfigContractCode;
using EPIC.RealEstateEntities.Dto.RstConfigContractCodeDetail;
using EPIC.RealEstateRepositories;
using EPIC.RocketchatDomain.Interfaces;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Contract;
using EPIC.Utils.ConstantVariables.Core;
using EPIC.Utils.ConstantVariables.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.ServiceModel;
using System.Threading.Tasks;

namespace EPIC.IdentityDomain.Implements
{
    public class UserServices : IUserServices
    {
        private readonly IWebHostEnvironment _env;
        private readonly IMapper _mapper;
        private readonly EpicSchemaDbContext _dbContext;
        private readonly ILogger<UserServices> _logger;
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IRocketChatServices _rocketChatServices;
        private readonly UserRepository _userRepository;
        private readonly UsersEFRepository _usersEFRepository;
        private readonly UserRoleEFRepository _userRoleEFRepository;
        private readonly RolePermissionEFRepository _rolePermissionEFRepository;
        private readonly RoleEFRepository _roleEFRepository;
        private readonly PartnerPermissionEFRepository _partnerPermissionEFRepository;
        private readonly TradingProviderPermissionEFRepository _tradingProviderPermissionEFRepository;
        private readonly UsersTradingProviderEFRepository _usersTradingProviderEFRepository;
        private readonly UsersPartnerEFRepository _usersPartnerEFRepository;
        private readonly InvestorRepository _investorRepository;
        private readonly TradingProviderRepository _tradingProviderRepository;
        private readonly PartnerRepository _partnerRepository;
        private readonly BusinessCustomerRepository _businessCustomerRepository;
        private readonly ManagerInvestorRepository _managerInvestorRepository;
        private readonly SaleRepository _saleRepository;
        private readonly InvestorSaleRepository _investorSaleRepository;
        private readonly RstConfigContractCodeEFRepository _rstConfigContractCodeEFRepository;
        private readonly RstConfigContractCodeDetailEFRepository _rstConfigContractCodeDetailEFRepository;

        public UserServices(
            IWebHostEnvironment env,
            EpicSchemaDbContext dbContext,
            ILogger<UserServices> logger,
            IConfiguration configuration,
            DatabaseOptions databaseOptions,
            IHttpContextAccessor httpContext,
            IRocketChatServices rocketChatServices,
            IMapper mapper)
        {
            _env = env;
            _mapper = mapper;
            _dbContext = dbContext;
            _logger = logger;
            _configuration = configuration;
            _connectionString = databaseOptions.ConnectionString;
            _userRepository = new UserRepository(_connectionString, _logger);
            _usersEFRepository = new UsersEFRepository(dbContext, logger);
            _userRoleEFRepository = new UserRoleEFRepository(dbContext);
            _rolePermissionEFRepository = new RolePermissionEFRepository(dbContext);
            _roleEFRepository = new RoleEFRepository(dbContext);
            _partnerPermissionEFRepository = new PartnerPermissionEFRepository(dbContext);
            _tradingProviderPermissionEFRepository = new TradingProviderPermissionEFRepository(dbContext);
            _usersTradingProviderEFRepository = new UsersTradingProviderEFRepository(dbContext);
            _usersPartnerEFRepository = new UsersPartnerEFRepository(dbContext);
            _investorRepository = new InvestorRepository(_connectionString, _logger);
            _tradingProviderRepository = new TradingProviderRepository(_connectionString, _logger);
            _businessCustomerRepository = new BusinessCustomerRepository(_connectionString, _logger);
            _partnerRepository = new PartnerRepository(_connectionString, _logger);
            _httpContext = httpContext;
            _rocketChatServices = rocketChatServices;
            _managerInvestorRepository = new ManagerInvestorRepository(_connectionString, _logger, _httpContext);
            _saleRepository = new SaleRepository(_connectionString, _logger);
            _investorSaleRepository = new InvestorSaleRepository(_connectionString, _logger);
            _rstConfigContractCodeEFRepository = new RstConfigContractCodeEFRepository(dbContext, logger);
            _rstConfigContractCodeDetailEFRepository = new RstConfigContractCodeDetailEFRepository(dbContext, logger);
        }

        public List<Claim> GetClaims(int userId)
        {
            var user = _userRepository.FindById(userId);
            if (user == null)
            {
                _logger.LogError($"User with id = {userId} is not found or is deleted");
                throw new FaultException(new FaultReason($"Không tìm thấy user hoặc user đã bị xóa"),
                    new FaultCode(((int)ErrorCode.UserNotFound).ToString()), "");
            }
            var claims = new List<Claim>();

            string ipAddress = CommonUtils.GetCurrentRemoteIpAddress(_httpContext);
            claims.Add(new Claim(Utils.ConstantVariables.Shared.ClaimTypes.IpAddressLogin, ipAddress));

            if (user.UserType != null)
            {
                claims.Add(new Claim(Utils.ConstantVariables.Shared.ClaimTypes.UserType, user.UserType));
                if (user.UserType == UserTypes.INVESTOR)
                {
                    var investor = _investorRepository.GetByUserId(userId);
                    if (investor != null)
                    {
                        claims.Add(new Claim(Utils.ConstantVariables.Shared.ClaimTypes.InvestorId, investor.InvestorId.ToString()));
                        try
                        {
                            var sale = _saleRepository.FindSaleByInvestorId(investor.InvestorId);
                            if (sale != null)
                            {
                                claims.Add(new Claim(Utils.ConstantVariables.Shared.ClaimTypes.SaleId, sale.SaleId.ToString()));
                            }
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, $"User with id = {userId} error add claim sale_id");
                        }

                        //danh sách đại lý mà investor đó đang follow dựa theo chọn sale nào làm mặc định
                        var sales = _investorSaleRepository.GetListSaleByInvestorId(investor.InvestorId);
                        InvestorSale saleDefault = null;
                        saleDefault = sales.LastOrDefault(s => s.IsDefault == YesNo.YES);
                        if (saleDefault == null)
                        {
                            saleDefault = sales.OrderBy(s => s.Id).LastOrDefault();
                        }

                        if (saleDefault != null)
                        {
                            var listTrading = _saleRepository.AppListTradingProviderBySale(saleDefault.SaleId ?? 0);
                            if (listTrading != null)
                            {
                                //do một khách doanh nghiệp có thể có nhiều hơn một đại lý nên lấy đại lý đầu tiên
                                var businessCustomerGroupTrading = listTrading.GroupBy(t => t.BusinessCustomerId)
                                    .Where(t => t.Any());
                                List<int> tradingIds = new();
                                foreach (var businessCustomerGroup in businessCustomerGroupTrading)
                                {
                                    if (businessCustomerGroup.Key == 621 && _env.EnvironmentName.ToLower() == "production")
                                    {
                                        tradingIds.Add(173);
                                    }
                                    else
                                    {
                                        tradingIds.Add(businessCustomerGroup.OrderBy(b => b.Status).FirstOrDefault().TradingProviderId);
                                    }
                                }
                                foreach (var tradingId in tradingIds)
                                {
                                    claims.Add(new Claim(Utils.ConstantVariables.Shared.ClaimTypes.InvestorTradingIdDefaults, tradingId.ToString()));
                                }
                            }
                        }
                    }
                    else
                    {
                        _logger.LogError($"Investor not found with user id = {userId}");
                        throw new FaultException(new FaultReason($"Không tìm thấy thông tin nhà đầu tư theo user id = {userId}"),
                            new FaultCode(((int)ErrorCode.InvestorNotFound).ToString()), "");
                    }
                }
            }
            if (user.PartnerId != null)
            {
                claims.Add(new Claim(Utils.ConstantVariables.Shared.ClaimTypes.PartnerId, user.PartnerId.ToString()));
                var listTradings = _userRepository.GetListTradingProviders(userId);
                foreach (var item in listTradings)
                {
                    claims.Add(new Claim(Utils.ConstantVariables.Shared.ClaimTypes.TradingProviderIds, item.TradingProviderId.ToString()));
                }
            }
            if (user.TradingProviderId != null)
            {
                claims.Add(new Claim(Utils.ConstantVariables.Shared.ClaimTypes.TradingProviderId, user.TradingProviderId.ToString()));
            }
            if (user.UserName != null)
            {
                claims.Add(new Claim(Utils.ConstantVariables.Shared.ClaimTypes.Username, user.UserName));
            }
            if (user.DisplayName != null)
            {
                claims.Add(new Claim(Utils.ConstantVariables.Shared.ClaimTypes.DisplayName, user.DisplayName));
            }
            if (user.Email != null)
            {
                claims.Add(new Claim(Utils.ConstantVariables.Shared.ClaimTypes.Email, user.Email));
            }
            return claims;
        }

        //public async Task<string> GenerateAccessToken(int userId)
        //{
        //    var claims = GetClaims(userId);
        //    return await _identityServerTools.IssueJwtAsync(_configuration.GetValue<int>("IdentityServer:Default:AccessTokenLifetime"), claims);
        //}

        public Users FindById(int userId)
        {
            var result = _userRepository.FindById(userId);
            result.Password = null;
            return result;
        }

        public bool ValidatePassword(string username, string password)
        {
            var user = _userRepository.FindByUserName(username);
            if (user == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy username: {username}"), new FaultCode(((int)ErrorCode.NotFound).ToString()), "");
            }
            return user.Password == CommonUtils.CreateMD5(password);
        }

        public Users GetByUserName(string username)
        {
            var result = _userRepository.FindByUserName(username);
            if (result != null)
            {
                result.Password = null;
            }
            return result;
        }

        public UserDto Create(CreateUserDto model)
        {
            var result = _userRepository.Add(new Users
            {
                DisplayName = model.DisplayName,
                UserName = model.UserName,
                Password = model.Password,
                Email = model.Email,
                CreatedBy = CommonUtils.GetCurrentUsername(_httpContext)
            });
            return result;
        }

        public int Delete(int userId)
        {
            return _userRepository.Delete(userId, CommonUtils.GetCurrentUsername(_httpContext));
        }

        public int Update(int id, UpdateUserDto model)
        {
            return _userRepository.Update(new Users
            {
                UserId = id,
                DisplayName = model.DisplayName,
                Email = model.Email,
                ModifiedBy = CommonUtils.GetCurrentUsername(_httpContext)
            });
        }

        public int ActiveUser(decimal userId, bool isActive)
        {
            return _userRepository.Active(userId, isActive, CommonUtils.GetCurrentUsername(_httpContext));
        }

        public PagingResult<Users> FindAll(int pageSize, int pageNumber, string keyword)
        {
            var result = _userRepository.FindAll(pageSize, pageNumber, keyword);
            foreach (var user in result.Items)
            {
                user.Password = null;
            }
            return result;
        }

        public PagingResult<Users> GetAllByPartnerId(int pageSize, int pageNumber, string keyword, int partnerId)
        {
            var result = _userRepository.GetAllByPartnerId(pageSize, pageNumber, keyword, partnerId);
            return result;
        }

        public PagingResult<Users> GetAllByTradingProvideId(int pageSize, int pageNumber, string keyword, int tradingProviderId)
        {
            var result = _userRepository.GetAllByTradingProviderId(pageSize, pageNumber, keyword, tradingProviderId);
            return result;
        }
        public Users FindByInvestorId(int investorId)
        {
            return _userRepository.FindByInvestorId(investorId);
        }

        public void ChangePassword(ChangePasswordDto input)
        {
            int userId = CommonUtils.GetCurrentUserId(_httpContext);
            _userRepository.ChangePassword(userId, input.OldPassword, input.NewPassword);
        }

        public UserDto CreateUserPartner(CreateUserPartnerDto model)
        {
            var result = _userRepository.Add(new Users
            {
                DisplayName = model.DisplayName,
                UserName = model.UserName,
                Password = model.Password,
                Email = model.Email,
                PartnerId = model.PartnerId,
                UserType = UserTypes.PARTNER,
                CreatedBy = CommonUtils.GetCurrentUsername(_httpContext)
            });
            return result;
        }

        /// <summary>
        /// Tạo tk rocketchat khi dlsc tạo user
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<UserDto> CreateUserTradingProvider(CreateUserTradingProviderDto model)
        {
            // tao user trong db
            var userModel = new Users
            {
                DisplayName = model.DisplayName,
                UserName = model.UserName,
                Password = model.Password,
                Email = model.Email,
                UserType = UserTypes.TRADING_PROVIDER,
                TradingProviderId = model.TradingProviderId,
                CreatedBy = CommonUtils.GetCurrentUsername(_httpContext)
            };
            var result = _userRepository.Add(userModel);

            // tao user rocketchat
            await _rocketChatServices.LoginAdmin();

            var rocketchatUser = await _rocketChatServices.CreateUser(new CreateRocketChatUserDto
            {
                email = userModel.Email,
                name = userModel.DisplayName,
                password = _rocketChatServices.genPasswordByUser(userModel),
                username = userModel.UserName,
                roles = new List<string> { "user" },
            });

            if (!rocketchatUser.success)
            {
                return result;
            }

            // tạo agent
            var agent = await _rocketChatServices.CreateAgent(new CreateAgentDto
            {
                Username = rocketchatUser.user.username
            });

            // get department
            var tradingProvider = _tradingProviderRepository.FindById(model.TradingProviderId);
            var businessCustomer = _businessCustomerRepository.FindBusinessCustomerById(tradingProvider.BusinessCustomerId);

            var departmentName = _rocketChatServices.genChannelName(tradingProvider, businessCustomer);

            var listDepartment = await _rocketChatServices.GetListDepartment(departmentName);
            DepartmentRocketchat department = new DepartmentRocketchat();

            if (listDepartment == null || listDepartment.Count < 1)
            {
                var resCreateDepartment = await _rocketChatServices.CreateDepartment(new CreateDepartmentDto
                {
                    Agents = new List<Agent>(),
                    Department = new CreateDepartmentRocketchat
                    {
                        Name = departmentName,
                        Email = businessCustomer.Email ?? $"DLSC-{tradingProvider.TradingProviderId}@epicpartner.vn",
                        Description = businessCustomer.Name,
                        Enabled = true,
                        ShowOnOfflineForm = true,
                        ShowOnRegistration = true,
                    },
                });

                if (resCreateDepartment.Success)
                {
                    department = resCreateDepartment?.Department;
                }
            }
            else
            {
                department = listDepartment[0];
            }

            if (!string.IsNullOrEmpty(department.Id))
            {
                // update agent vào department
                var upsert = new Upsert
                {
                    AgentId = agent.User.Id,
                    Username = agent.User.Username,
                };

                await _rocketChatServices.UpdateAgentDepartment(department.Id, new UpdateAgentDepartmentDto
                {
                    Remove = new List<Upsert>(),
                    Upsert = new List<Upsert>()
                    {
                        upsert
                    }
                });
            }

            return result;

        }

        public List<ViewUserFcmToken> GetUsersFcmTokens(List<decimal> listUserId)
        {
            List<ViewUserFcmToken> result = new List<ViewUserFcmToken>();

            foreach (var userid in listUserId)
            {
                var tmp = new ViewUserFcmToken
                {
                    UserId = userid,
                    FcmToken = new List<string>()
                };

                var listFcmToken = _userRepository.GetFcmToken(userid);
                tmp.FcmToken = listFcmToken;

                result.Add(tmp);
            }

            return result;
        }

        public void ClearFcmToken(string fcmToken)
        {
            var userid = CommonUtils.GetCurrentUserId(_httpContext);

            _userRepository.DeleteFcmToken(userid, fcmToken);
        }

        public PagingResult<UserIsInvestorForAppDto> GetByType(FindBondInvestorAccountDto dto)
        {
            int? partnerId = null;
            int? tradingProviderId = null;
            string userType = CommonUtils.GetCurrentUserType(_httpContext);
            if (userType == UserTypes.PARTNER)
            {
                partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
            }
            else if (userType == UserTypes.TRADING_PROVIDER || userType == UserTypes.ROOT_TRADING_PROVIDER)
            {
                tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            }
            else if (userType == UserTypes.EPIC || userType == UserTypes.SUPER_ADMIN || userType == UserTypes.ROOT_EPIC)
            {
            }
            else
            {
                throw new FaultException(new FaultReason($"UserType = {userType}"), new FaultCode(((int)ErrorCode.BadRequest).ToString()), "");
            }

            var result = new PagingResult<UserIsInvestorForAppDto>();
            var userFcmTokens = new List<UserIsInvestorForAppDto>();
            dto.Status = dto.Status ?? UserStatus.ACTIVE;
            var investorQuery = _userRepository.GetByType(dto, tradingProviderId, partnerId);
            foreach (var investor in investorQuery.Items)
            {
                var userFcmToken = new UserIsInvestorForAppDto()
                {
                    CifCode = investor.CifCode,
                    DisplayName = investor.DisplayName,
                    Email = investor.Email,
                    InvestorId = investor.InvestorId,
                    Name = investor.Name,
                    Phone = investor.Phone,
                    Sex = investor.Sex,
                };
                var user = _dbContext.Users.FirstOrDefault(u => u.InvestorId == investor.InvestorId && u.IsDeleted == YesNo.NO && u.UserType == UserTypes.INVESTOR);
                if (user != null)
                {
                    userFcmToken.UserId = user.UserId;
                    userFcmToken.UserName = user.UserName;
                    userFcmToken.FcmTokens = _userRepository.GetFcmToken(user.UserId);
                }

                userFcmTokens.Add(userFcmToken);
            }
            result.Items = userFcmTokens;
            result.TotalItems = investorQuery.TotalItems;
            return result;

        }

        public MyInfoDto GetMyInfo()
        {
            var userid = CommonUtils.GetCurrentUserId(_httpContext);
            var user = _userRepository.FindById(userid);

            var result = new MyInfoDto
            {
                UserName = user.UserName,
                DisplayName = user.DisplayName,
                Email = user.Email,
                UserType = user.UserType,
                IsTempPassword = user.IsTempPassword,
                UserAvatarImageUrl = user.AvatarImageUrl,
            };

            if (user.UserType == UserTypes.INVESTOR)
            {
                var investor = _investorRepository.FindById(CommonUtils.GetCurrentInvestorId(_httpContext));

                var identification = _managerInvestorRepository.GetDefaultIdentification(investor.InvestorId, false);

                result.AvatarImageUrl = investor?.AvatarImageUrl;
                result.DateOfBirth = identification?.DateOfBirth;
                result.DisplayName = identification?.Fullname;
                result.Email = investor.Email;
                result.Phone = investor.Phone;
            }
            else if (user.UserType == UserTypes.PARTNER || user.UserType == UserTypes.ROOT_PARTNER)
            {
                int partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
                var partner = _partnerRepository.FindById(partnerId);
                if (partner != null)
                {
                    result.Email = result.Email ?? partner.Email;
                    result.Phone = partner.Phone;
                }
                else
                {
                    _logger.LogError($"Không tìm thấy thông tin partner: partnerId = {partnerId}");
                }
            }
            else if (user.UserType == UserTypes.TRADING_PROVIDER || user.UserType == UserTypes.ROOT_TRADING_PROVIDER)
            {
                //var businessCustomer = _businessCustomerRepository.
                var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
                var tradingProvider = _tradingProviderRepository.FindById(tradingProviderId);
                var businessCustomer = _businessCustomerRepository.FindBusinessCustomerById(tradingProvider?.BusinessCustomerId ?? 0);

                if (businessCustomer != null)
                {
                    result.AvatarImageUrl = businessCustomer?.AvatarImageUrl;
                    result.Email = businessCustomer.Email;
                    result.Phone = businessCustomer.Phone;
                }
                else
                {
                    _logger.LogError($"Không tìm thấy thông tin bussinessCustomer: businessCustomerId = {tradingProvider?.BusinessCustomerId}");
                }
            }

            return result;
        }

        public void Login(decimal userId)
        {
            _userRepository.Login(userId);
        }

        /// <summary>
        /// Tạo tài khoản cho các cấp ở dưới UserData đang đăng nhập
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <exception cref="FaultException"></exception>
        public UserDto RootCreateUser(CreateUserByRootDto model)
        {
            var userType = CommonUtils.GetCurrentUserType(_httpContext);
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            var result = new Users();
            var data = new UserDto();

            // Kiểm tra xem username đã tồn tại trong hệ thống chưa
            if (_usersEFRepository.Entity.Any(u => u.UserName == model.UserName && u.IsDeleted == YesNo.NO))
            {
                _usersEFRepository.ThrowException(ErrorCode.UserUsernameDuplicated);
            }

            var insertUser = new Users
            {
                DisplayName = model.DisplayName,
                UserName = model.UserName,
                Password = CommonUtils.CreateMD5(model.Password),
                Email = model.Email,
                InvestorId = model.InvestorId,
                IsTempPassword = model.IsTempPassword,
                AvatarImageUrl = model.AvatarImageUrl,
                CreatedBy = username
            };
            var transaction = _dbContext.Database.BeginTransaction();
            //Nếu là ROOT_EPIC thì tạo tài khoản được cho EPIC thường
            if (userType == UserTypes.ROOT_EPIC)
            {
                if (model.PartnerId == null && model.TradingProviderId == null)
                {
                    insertUser.UserType = UserTypes.EPIC;
                    var user = _usersEFRepository.Add(insertUser);
                }
            }

            //Nếu là EPIC thì tạo tài khoản cho PARTNER_ROOT
            if (userType == UserTypes.EPIC || userType == UserTypes.ROOT_EPIC)
            {
                if (model.PartnerId != null)
                {
                    if (_usersEFRepository.Entity.Any(u => u.PartnerId == model.PartnerId && u.UserType == UserTypes.ROOT_PARTNER && u.IsDeleted == YesNo.NO))
                    {
                        throw new FaultException(new FaultReason($"Tài khoản đối tác Root đã tồn tại"), new FaultCode(((int)ErrorCode.UserUsernameDuplicated).ToString()), "");
                    }

                    insertUser.UserType = UserTypes.ROOT_PARTNER;
                    insertUser.PartnerId = model.PartnerId;
                    var user = _usersEFRepository.Add(insertUser);
                    _roleEFRepository.AddRoleDefault(model.PartnerId, null);

                    // Thêm mã hợp đồng RealEstate cho đối tác
                    AddConfigContractCodeDefault(username, null, model.PartnerId);

                    if (!_usersPartnerEFRepository.Entity.Any(r => r.UserId == CommonUtils.GetCurrentUserId(_httpContext)
                                                    && r.PartnerId == model.PartnerId && r.Deleted == YesNo.NO))
                    {
                        _usersPartnerEFRepository.Entity.Add(new UsersPartner
                        {
                            Id = (int)_usersPartnerEFRepository.NextKey(),
                            UserId = CommonUtils.GetCurrentUserId(_httpContext),
                            PartnerId = (int)(model.PartnerId ?? 0),
                            CreatedBy = CommonUtils.GetCurrentUsername(_httpContext)
                        });
                    }
                }
            }
            else if (userType == UserTypes.ROOT_PARTNER)
            {
                if (model.TradingProviderId == null)
                {
                    insertUser.UserType = UserTypes.PARTNER;
                    insertUser.PartnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
                    var user = _usersEFRepository.Add(insertUser);
                }
                else if (model.TradingProviderId != null)
                {
                    if (_usersEFRepository.Entity.Any(u => u.TradingProviderId == model.TradingProviderId && u.UserType == UserTypes.ROOT_TRADING_PROVIDER && u.IsDeleted == YesNo.NO))
                    {
                        throw new FaultException(new FaultReason($"Tài khoản đại lý Root đã tồn tại"), new FaultCode(((int)ErrorCode.UserUsernameDuplicated).ToString()), "");
                    }

                    insertUser.UserType = UserTypes.ROOT_TRADING_PROVIDER;
                    insertUser.TradingProviderId = model.TradingProviderId;
                    var user = _usersEFRepository.Add(insertUser);

                    // Thêm mã hợp đồng RealEstate cho đại lý
                    AddConfigContractCodeDefault(username, model.TradingProviderId, null);

                    _roleEFRepository.AddRoleDefault(null, model.TradingProviderId);
                    if (!_usersTradingProviderEFRepository.Entity.Any(r => r.UserId == CommonUtils.GetCurrentUserId(_httpContext)
                                                    && r.TradingProviderId == model.TradingProviderId && r.Deleted == YesNo.NO))
                    {
                        _usersTradingProviderEFRepository.Entity.Add(new UsersTradingProvider
                        {
                            Id = (int)_usersTradingProviderEFRepository.NextKey(),
                            UserId = CommonUtils.GetCurrentUserId(_httpContext),
                            TradingProviderId = model.TradingProviderId,
                            CreatedBy = CommonUtils.GetCurrentUsername(_httpContext)
                        });
                    }
                }
            }

            else if (userType == UserTypes.PARTNER)
            {
                if (_usersEFRepository.Entity.Any(u => u.TradingProviderId == model.TradingProviderId && u.UserType == UserTypes.ROOT_TRADING_PROVIDER && u.IsDeleted == YesNo.NO))
                {
                    throw new FaultException(new FaultReason($"Tài khoản đại lý Root đã tồn tại"), new FaultCode(((int)ErrorCode.UserUsernameDuplicated).ToString()), "");
                }

                insertUser.UserType = UserTypes.ROOT_TRADING_PROVIDER;
                insertUser.TradingProviderId = model.TradingProviderId;
                var user = _usersEFRepository.Add(insertUser);
                _roleEFRepository.AddRoleDefault(null, model.TradingProviderId);

                // Thêm mã hợp đồng RealEstate cho đại lý
                AddConfigContractCodeDefault(username, model.TradingProviderId, null);
                if (!_usersTradingProviderEFRepository.Entity.Any(r => r.UserId == CommonUtils.GetCurrentUserId(_httpContext)
                                                    && r.TradingProviderId == model.TradingProviderId && r.Deleted == YesNo.NO))
                {
                    _usersTradingProviderEFRepository.Entity.Add(new UsersTradingProvider
                    {
                        Id = (int)_usersTradingProviderEFRepository.NextKey(),
                        UserId = CommonUtils.GetCurrentUserId(_httpContext),
                        TradingProviderId = model.TradingProviderId,
                        CreatedBy = CommonUtils.GetCurrentUsername(_httpContext)
                    });
                }
            }

            else if (userType == UserTypes.ROOT_TRADING_PROVIDER)
            {
                insertUser.UserType = UserTypes.TRADING_PROVIDER;
                insertUser.TradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
                var user = _usersEFRepository.Add(insertUser);
            }
            _dbContext.SaveChanges();
            transaction.Commit();
            return data;
        }

        /// <summary>
        /// Reset Password cho các cấp ở dưới UserData đang đăng nhập
        /// </summary>
        /// <param name="input"></param>
        /// <exception cref="FaultException"></exception>
        public void ResetPasswordByRoot(RootUpdatePasswordDto input)
        {
            List<Users> userData = ListUsersChild();
            //Kiểm tra xem User có thuộc trong danh sách không
            var checkUsers = userData.FirstOrDefault(r => r.UserId == input.UserId);
            if (checkUsers == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy thông tin tài khoản"), new FaultCode(((int)ErrorCode.UserNotFound).ToString()), "");
            }
            var updatePassword = _usersEFRepository.Entity.FirstOrDefault(r => r.UserId == checkUsers.UserId && r.IsDeleted == YesNo.NO);

            updatePassword.Password = CommonUtils.CreateMD5(input.NewPassword);
            updatePassword.IsTempPassword = input.IsTempPassword;
            _dbContext.SaveChanges();
        }

        /// <summary>
        /// Active/Deactive cho các cấp ở dưới UserData đang đăng nhập
        /// </summary>
        /// <exception cref="FaultException"></exception>
        public void ActiveUsersByRoot(int userId)
        {
            List<Users> userData = ListUsersChild();
            //Kiểm tra xem User có thuộc trong danh sách không
            var checkUsers = userData.FirstOrDefault(r => r.UserId == userId);
            if (checkUsers == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy thông tin tài khoản"), new FaultCode(((int)ErrorCode.UserNotFound).ToString()), "");
            }
            var findUser = _usersEFRepository.Entity.FirstOrDefault(r => r.UserId == checkUsers.UserId && r.IsDeleted == YesNo.NO);
            if (findUser.Status == Status.ACTIVE)
            {
                findUser.Status = Status.INACTIVE;
            }
            else if (findUser.Status == Status.INACTIVE)
            {
                findUser.Status = Status.ACTIVE;
            }
            findUser.ModifiedBy = CommonUtils.GetCurrentUsername(_httpContext);
            findUser.ModifiedDate = DateTime.Now;
            _dbContext.SaveChanges();
        }

        /// <summary>
        /// Xóa tài khoản cấp dưới
        /// </summary>
        public void DeleteUsersByRoot(int userId)
        {
            List<Users> userData = ListUsersChild();
            //Kiểm tra xem User có thuộc trong danh sách không
            var checkUsers = userData.FirstOrDefault(r => r.UserId == userId);
            if (checkUsers == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy thông tin tài khoản"), new FaultCode(((int)ErrorCode.UserNotFound).ToString()), "");
            }
            var findUser = _usersEFRepository.Entity.FirstOrDefault(r => r.UserId == checkUsers.UserId && r.IsDeleted == YesNo.NO);
            findUser.IsDeleted = YesNo.YES;
            findUser.DeletedBy = CommonUtils.GetCurrentUsername(_httpContext);
            findUser.DeletedDate = DateTime.Now;
            _dbContext.SaveChanges();
        }

        public List<Users> ListUsersChild()
        {
            var userData = new List<Users>();
            var userType = CommonUtils.GetCurrentUserType(_httpContext);

            if (userType == UserTypes.ROOT_EPIC)
            {
                //Lấy tài khoản EPIC
                var userFind = _usersEFRepository.Entity.Where(u => u.UserType == UserTypes.EPIC && u.IsDeleted == YesNo.NO);
                userData.AddRange(userFind);
            }

            if (userType == UserTypes.EPIC || userType == UserTypes.ROOT_EPIC)
            {
                //Lấy tài khoản partner theo EPIC
                var userPartner = _partnerRepository.FindAll(-1, 0, null).Items;
                foreach (var item in userPartner)
                {
                    var userFind = _usersEFRepository.Entity.FirstOrDefault(u => u.PartnerId == item.PartnerId && u.UserType == UserTypes.ROOT_PARTNER && u.IsDeleted == YesNo.NO);
                    if (userFind != null)
                    {
                        userData.Add(userFind);
                    }
                }
            }

            else if (userType == UserTypes.ROOT_PARTNER)
            {
                var partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
                //Lấy danh sách Partner thường
                var partnerUser = _userRepository.GetAllByPartnerId(-1, 0, null, partnerId);
                userData.AddRange(partnerUser.Items);

                var tradingProviderFind = _tradingProviderRepository.FindAll(partnerId, -1, 0, null, null);
                foreach (var item in tradingProviderFind.Items)
                {
                    var userTradingProviderFind = _usersEFRepository.Entity.Where(u => u.TradingProviderId == item.TradingProviderId && u.UserType == UserTypes.ROOT_TRADING_PROVIDER && u.IsDeleted == YesNo.NO);
                    userData.AddRange(userTradingProviderFind);
                }
            }

            else if (userType == UserTypes.PARTNER)
            {
                var tradingProviderFind = _tradingProviderRepository.FindAll(CommonUtils.GetCurrentPartnerId(_httpContext), -1, 0, null, null);
                foreach (var item in tradingProviderFind.Items)
                {
                    var userFind = _usersEFRepository.Entity.Where(u => u.TradingProviderId == item.TradingProviderId && u.UserType == UserTypes.ROOT_TRADING_PROVIDER && u.IsDeleted == YesNo.NO);
                    userData.AddRange(userFind);
                }
            }

            else if (userType == UserTypes.ROOT_TRADING_PROVIDER)
            {
                var tradingProviderFind = _userRepository.GetAllByTradingProviderId(-1, 0, null, CommonUtils.GetCurrentTradingProviderId(_httpContext));
                userData.AddRange(tradingProviderFind.Items);
            }
            return userData;
        }

        /// <summary>
        /// Thay đổi mật khẩu của người dùng đang đăng nhập khi đang ở trong trạng thái mật khẩu tạm: IsTempPassword = Y
        /// </summary>
        /// <param name="input"></param>
        /// <exception cref="FaultException"></exception>
        public void ChangePasswordTempByUser(UpdatePasswordUserDto input)
        {
            var userId = CommonUtils.GetCurrentUserId(_httpContext);
            var userFind = _usersEFRepository.Entity.FirstOrDefault(u => u.UserId == userId && u.IsDeleted == YesNo.NO);
            if (userFind == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy thông tin tài khoản của người dùng"), new FaultCode(((int)ErrorCode.UserNotFound).ToString()), "");
            }
            else if (userFind.IsTempPassword == YesNo.NO)
            {
                throw new FaultException(new FaultReason($"Mật khẩu của bạn không nằm trong trạng thái tạm không thể sửa đổi với chức năng này"), new FaultCode(((int)ErrorCode.UserIsNotIsTempPassword).ToString()), "");
            }
            //Khi đang ở trạng thái mật khẩu tạm
            userFind.Password = CommonUtils.CreateMD5(input.NewPassword);
            userFind.IsTempPassword = YesNo.NO;
            _dbContext.SaveChanges();
        }

        public void SaveUserChatRoomInfo(CreateUsersChatInfoDto dto)
        {
            var userId = CommonUtils.GetCurrentUserId(_httpContext);
            dto.UserId = userId;

            _userRepository.SaveUserChatRoomInfo(dto);
        }

        public PagingResult<ViewUsersChatInfoDto> GetUserChatRoomInfo(FindUserChatRoomDto dto)
        {
            var userId = CommonUtils.GetCurrentUserId(_httpContext);

            var query = _userRepository.GetUserChatRoomInfo(dto, userId);

            var result = new PagingResult<ViewUsersChatInfoDto>()
            {
                TotalItems = query.TotalItems
            };

            if (result.TotalItems > 0)
            {
                var items = _mapper.Map<List<ViewUsersChatInfoDto>>(query.Items);
                result.Items = items;
            }

            return result;
        }

        /// <summary>
        /// Xoá mềm user theo user id
        /// </summary>
        /// <param name="userId"></param>
        public void DeleteByUserId(int userId)
        {
            string username = CommonUtils.GetCurrentUsername(_httpContext);

            _usersEFRepository.DeleteUserByUserId(userId, username);
            _dbContext.SaveChanges();
        }

        /// <summary>
        /// Update tên hiển thị
        /// </summary>
        /// <param name="input"></param>
        public void UpdateDisplayNameUser(UpdateUserDto input)
        {
            _logger.LogInformation($"{System.Reflection.MethodBase.GetCurrentMethod().Name}: input = {input}");
            var username = CommonUtils.GetCurrentUsername(_httpContext);

            var user = _usersEFRepository.FindById(input.UserId).ThrowIfNull<Users>(_dbContext, ErrorCode.UserNotFound);
            if (user != null)
            {
                user.DisplayName = input.DisplayName;
            }
            _usersEFRepository.UpdateDisplayNameUser(user, username);
            _dbContext.SaveChanges();
        }

        /// <summary>
        /// Cập nhật ảnh đại diện
        /// </summary>
        /// <param name="avatarImageUrl"></param>
        public void UpdateAvatarUser(string avatarImageUrl)
        {
            _logger.LogInformation($"{nameof(UpdateAvatarUser)}: avatarImageUrl = {avatarImageUrl}");
            var userId = CommonUtils.GetCurrentUserId(_httpContext);
            var user = _usersEFRepository.FindById(userId)
                .ThrowIfNull(_dbContext, ErrorCode.UserNotFound);
            user.AvatarImageUrl = avatarImageUrl;
            _dbContext.SaveChanges();
        }

        /// <summary>
        /// Đổ ra thông tin quyền riêng tư và cá nhân hoá
        /// </summary>
        /// <returns></returns>
        public PrivacyInfoDto GetPrivacyInfo()
        {
            var userId = CommonUtils.GetCurrentUserId(_httpContext);
            var userFind = _dbContext.Users.FirstOrDefault(o => o.UserId == userId)
                .ThrowIfNull(_dbContext, ErrorCode.UserNotFound);

            var result = new PrivacyInfoDto()
            {
                LastDevice = userFind.LastDevice,
                LastLogin = userFind.LastLogin,
                AvatarImageUrl = userFind.AvatarImageUrl,
            };
            return result;
        }

        #region Sinh thêm các thông tin
        /// <summary>
        /// Thêm cấu trúc mã hợp đồng
        /// </summary>
        /// <param name="username"></param>
        /// <param name="tradingProviderId"></param>
        /// <param name="partnerId"></param>
        private void AddConfigContractCodeDefault(string username, int? tradingProviderId, int? partnerId)
        {
            var checkConfigContractCode = _dbContext.RstConfigContractCodes.Any(c => c.Deleted == YesNo.NO && ((tradingProviderId != null && c.TradingProviderId == tradingProviderId)
                                            || (partnerId != null && c.PartnerId == partnerId)));
            // Nếu đã tồn tại cấu trúc mã HĐ thì bỏ qua
            if (checkConfigContractCode)
            {
                return;
            }

            CreateRstConfigContractCodeDto rstConfigContractCode = new()
            {
                Name = "Cấu trúc mã mặc định",
                Description = "Cấu trúc mã mặc định",
                ConfigContractCodeDetails = new()
                {
                    new CreateRstConfigContractCodeDetailDto ()
                    {
                        SortOrder = 0,
                        Key = ConfigContractCode.FIX_TEXT,
                        Value = ContractCodes.REAL_ESTATE
                    },
                    new CreateRstConfigContractCodeDetailDto ()
                    {
                        SortOrder = 1,
                        Key = ConfigContractCode.ORDER_ID_PREFIX_0
                    }
                }
            };

            //Add cấu hình contractCode
            var insertConfig = _mapper.Map<RstConfigContractCode>(rstConfigContractCode);

            var resultConfig = _rstConfigContractCodeEFRepository.Add(insertConfig, username, partnerId, tradingProviderId);
            //Add Config detail
            foreach (var item in rstConfigContractCode.ConfigContractCodeDetails)
            {
                var insertConfigDetail = _mapper.Map<RstConfigContractCodeDetail>(item);
                insertConfigDetail.ConfigContractCodeId = resultConfig.Id;
                var detailAdd = _rstConfigContractCodeDetailEFRepository.Add(insertConfigDetail);
            }
            _dbContext.SaveChanges();

        }
        #endregion
    }
}
