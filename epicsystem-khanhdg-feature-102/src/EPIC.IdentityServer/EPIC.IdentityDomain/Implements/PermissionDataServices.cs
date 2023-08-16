using AutoMapper;
using EPIC.BondRepositories;
using EPIC.CoreRepositories;
using EPIC.DataAccess.Base;
using EPIC.DataAccess.Models;
using EPIC.Entities;
using EPIC.Entities.DataEntities;
using EPIC.Entities.Dto.BusinessCustomer;
using EPIC.Entities.Dto.Investor;
using EPIC.Entities.Dto.Partner;
using EPIC.Entities.Dto.User;
using EPIC.IdentityDomain.Interfaces;
using EPIC.IdentityEntities.DataEntities;
using EPIC.IdentityEntities.Dto.PartnerPermission;
using EPIC.IdentityEntities.Dto.Permissions;
using EPIC.IdentityEntities.Dto.Roles;
using EPIC.IdentityEntities.Dto.UsersPartner;
using EPIC.IdentityRepositories;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Identity;
using EPIC.Utils.RolePermissions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.IdentityDomain.Implements
{
    public class PermissionDataServices : IPermissionDataServices
    {
        private readonly EpicSchemaDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        private readonly string _connectionString;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly PartnerPermissionEFRepository _partnerPermissionRepository;
        private readonly RoleEFRepository _roleRepository;
        private readonly UsersPartnerEFRepository _usersPartnerRepository;
        private readonly UsersEFRepository _usersEFRepository;
        private readonly UsersTradingProviderEFRepository _usersTradingProviderEFRepository;
        private readonly UsersPartnerEFRepository _usersPartnerEFRepository;
        private readonly PartnerRepository _partnerRepository;
        private readonly BusinessCustomerRepository _businessCustomerRepository;
        private readonly TradingProviderRepository _tradingProviderRepository;
        private readonly UserRoleEFRepository _userRoleRepository;
        private readonly UserRepository _userRepository;
        private readonly InvestorIdentificationRepository _investorIdentificationRepository;
        private readonly InvestorRepository _investorRepository;

        //private readonly 

        public PermissionDataServices(EpicSchemaDbContext dbContext,
            DatabaseOptions databaseOptions,
            IMapper mapper,
            ILogger<PermissionDataServices> logger,
            IHttpContextAccessor httpContextAccessor)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
            _connectionString = databaseOptions.ConnectionString;
            _httpContextAccessor = httpContextAccessor;
            _partnerPermissionRepository = new PartnerPermissionEFRepository(dbContext);
            _roleRepository = new RoleEFRepository(dbContext);
            _userRoleRepository = new UserRoleEFRepository(dbContext);
            _usersPartnerRepository = new UsersPartnerEFRepository(dbContext);
            _usersEFRepository = new UsersEFRepository(dbContext, logger);
            _usersTradingProviderEFRepository = new UsersTradingProviderEFRepository(dbContext);
            _usersPartnerEFRepository = new UsersPartnerEFRepository(dbContext);
            _partnerRepository = new PartnerRepository(_connectionString, _logger);
            _businessCustomerRepository = new BusinessCustomerRepository(_connectionString, _logger);
            _tradingProviderRepository = new TradingProviderRepository(_connectionString, _logger);
            _userRepository = new UserRepository(_connectionString, _logger);
            _investorIdentificationRepository = new InvestorIdentificationRepository(_connectionString, _logger);
            _investorRepository = new InvestorRepository(_connectionString, _logger);
        }

        public UserRole AddRoleUser(CreateUserRoleDto input)
        {
            var userRoleQuery = _userRoleRepository.Entity.Where(p => p.UserId == input.UserId && p.Deleted == YesNo.NO);

            //remove những role không có trong danh sách đầu vào
            var removeList = userRoleQuery.Where(p => !input.RoleIds.Contains(p.RoleId)).ToList();
            foreach (var removeItem in removeList)
            {
                removeItem.Deleted = YesNo.YES;
            }
            //thêm
            foreach (var roleId in input.RoleIds)
            {
                var permissionPartner = userRoleQuery.FirstOrDefault(p => p.RoleId == roleId);
                if (permissionPartner == null)
                {
                    var roleUserInsert = _userRoleRepository.Entity.Add(new UserRole
                    {
                        Id = (int)_userRoleRepository.NextKey(),
                        CreatedBy = CommonUtils.GetCurrentUsername(_httpContextAccessor),
                        UserId = input.UserId,
                        RoleId = roleId
                    });
                }
            }
            _dbContext.SaveChanges();
            return null;
        }

        public UsersPartner AddUserPartner(CreateUsersPartnerDto input)
        {
            var usersPartnerQuery = _usersPartnerRepository.Entity.Where(p => p.PartnerId == CommonUtils.GetCurrentPartnerId(_httpContextAccessor) && p.Deleted == YesNo.NO);

            //remove những role không có trong danh sách đầu vào
            var removeList = usersPartnerQuery.Where(p => !input.UserIds.Contains(p.UserId)).ToList();
            foreach (var removeItem in removeList)
            {
                removeItem.Deleted = YesNo.YES;
            }
            //thêm
            foreach (var userId in input.UserIds)
            {
                var permissionPartner = usersPartnerQuery.FirstOrDefault(p => p.PartnerId == CommonUtils.GetCurrentPartnerId(_httpContextAccessor));
                if (permissionPartner == null)
                {
                    var roleUserInsert = _usersPartnerRepository.Entity.Add(new UsersPartner
                    {
                        Id = (int)_usersPartnerRepository.NextKey(),
                        CreatedBy = CommonUtils.GetCurrentUsername(_httpContextAccessor),
                        PartnerId = CommonUtils.GetCurrentPartnerId(_httpContextAccessor),
                        UserId = userId
                    });
                }
            }
            _dbContext.SaveChanges();
            return null;
        }

        /// <summary>
        /// Lấy danh sách tài khoản để phân quyền
        /// ROOT ADMIN lấy danh sách ROOT PARTNER mà nó tạo ra
        /// ROOT PARTNER lấy danh sách PARTNER thường và ROOT TRADING (trong bảng quan hệ EP_TRADING_PROVIDER_PARTNER)
        /// PARTNER thường lấy danh sách ROOT TRADING (trong bảng quan hệ EP_TRADING_PROVIDER_PARTNER)
        /// ROOT TRADING lấy danh sách đại lý sơ cấp thường của nó
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public PagingResult<UsersPartnerDto> FindAll(FilterUsersManagerDto input)
        {
            var result = new PagingResult<UsersPartnerDto>();
            var userType = CommonUtils.GetCurrentUserType(_httpContextAccessor);
            var userId = CommonUtils.GetCurrentUserId(_httpContextAccessor);
            var items = new List<UsersPartnerDto>();

            //Lấy tài khoản EPIC
            if (userType == UserTypes.ROOT_EPIC)
            {
                var userFind = _usersEFRepository.Entity.Where(u => u.UserType == UserTypes.EPIC).ToList();
                foreach (var item in userFind)
                {
                    var user = _mapper.Map<UsersPartnerDto>(item);
                    if (item.InvestorId != null)
                    {
                        var investorFind = _investorRepository.FindById((int)item.InvestorId);
                        if (investorFind != null)
                        {
                            var investor = _mapper.Map<InvestorDto>(investorFind);
                            user.Investor = investor;
                            var investorIdenDefaultFind = _investorIdentificationRepository.GetByInvestorId((int)item.InvestorId);

                            if (investorIdenDefaultFind != null)
                            {
                                user.Investor.InvestorIdentification = _mapper.Map<InvestorIdentificationDto>(investorIdenDefaultFind);
                            }
                        }
                    }
                    user.UserInfo = new MyInfoDto();
                    user.UserInfo = _mapper.Map<MyInfoDto>(item);
                    items.Add(user);
                }
            }

            if (userType == UserTypes.ROOT_EPIC || userType == UserTypes.EPIC)
            {
                //Lấy thông tin EPIC đang đăng nhập
                var userLoginNow = _usersEFRepository.Entity.FirstOrDefault(u => u.UserId == CommonUtils.GetCurrentUserId(_httpContextAccessor) && u.IsDeleted == YesNo.NO);
                if (userLoginNow != null)
                {
                    var user = _mapper.Map<UsersPartnerDto>(userLoginNow);
                    if (userLoginNow.InvestorId != null)
                    {
                        var investorFind = _investorRepository.FindById((int)userLoginNow.InvestorId);
                        if (investorFind != null)
                        {
                            var investor = _mapper.Map<InvestorDto>(investorFind);
                            user.Investor = investor;
                            var investorIdenDefaultFind = _investorIdentificationRepository.GetByInvestorId((int)userLoginNow.InvestorId);

                            if (investorIdenDefaultFind != null)
                            {
                                user.Investor.InvestorIdentification = _mapper.Map<InvestorIdentificationDto>(investorIdenDefaultFind);
                            }
                        }
                    }
                    user.UserInfo = new MyInfoDto();
                    user.UserInfo = _mapper.Map<MyInfoDto>(userLoginNow);
                    items.Add(user);
                }

                //Lấy tài khoản partner theo EPIC
                var userPartner = _partnerRepository.FindAll(-1, 0, null).Items;

                foreach (var item in userPartner)
                {
                    var userFind = _usersEFRepository.Entity.FirstOrDefault(u => u.PartnerId == item.PartnerId && u.UserType == UserTypes.ROOT_PARTNER);
                    if(userFind != null)
                    {
                        var user = _mapper.Map<UsersPartnerDto>(userFind);
                        if (userFind.InvestorId != null)
                        {
                            var investorFind = _investorRepository.FindById((int)userFind.InvestorId);
                            if (investorFind != null)
                            {
                                var investor = _mapper.Map<InvestorDto>(investorFind);
                                user.Investor = investor;
                                var investorIdenDefaultFind = _investorIdentificationRepository.GetByInvestorId((int)userFind.InvestorId);

                                if (investorIdenDefaultFind != null)
                                {
                                    user.Investor.InvestorIdentification = _mapper.Map<InvestorIdentificationDto>(investorIdenDefaultFind);
                                }
                            }
                        }    
                        user.PartnerId = (decimal)item.PartnerId;
                        var partnerFind = _partnerRepository.FindById((int)item.PartnerId);
                        if (partnerFind != null)
                        {
                            user.Partner = _mapper.Map<PartnerDto>(partnerFind);
                        }
                        var tradingProviderFind = _tradingProviderRepository.FindById((int)user.TradingProviderId);
                        if (tradingProviderFind != null)
                        {
                            var businessCustomer = _businessCustomerRepository.FindBusinessCustomerById(tradingProviderFind.BusinessCustomerId);
                            if (businessCustomer != null)
                            {
                                user.TradingProvider = new Entities.Dto.TradingProvider.TradingProviderDto();
                                user.TradingProvider.BusinessCustomer = _mapper.Map<BusinessCustomerDto>(businessCustomer);
                            }
                        }
                        user.UserInfo = new MyInfoDto();
                        user.UserInfo = _mapper.Map<MyInfoDto>(userFind);
                        items.Add(user);
                    }
                }
            }

            else if (userType == UserTypes.ROOT_PARTNER)
            {
                var partnerId = CommonUtils.GetCurrentPartnerId(_httpContextAccessor);
                //Lấy danh sách Partner thường
                var partnerUser = _usersEFRepository.Entity.Where(u => u.PartnerId == partnerId);
                foreach (var item in partnerUser)
                {
                    var user = new UsersPartnerDto();
                    if (item.InvestorId != null)
                    {
                        var investorFind = _investorRepository.FindById((int)item.InvestorId);
                        if (investorFind != null)
                        {
                            var investor = _mapper.Map<InvestorDto>(investorFind);
                            user.Investor = investor;
                            var investorIdenDefaultFind = _investorIdentificationRepository.GetByInvestorId((int)item.InvestorId);

                            if (investorIdenDefaultFind != null)
                            {
                                user.Investor.InvestorIdentification = _mapper.Map<InvestorIdentificationDto>(investorIdenDefaultFind);
                            }
                        }
                    }
                    var partnerFind = _partnerRepository.FindById((int)item.PartnerId);
                    if (partnerFind != null)
                    {
                        user.Partner = _mapper.Map<PartnerDto>(partnerFind);
                    }
                    user.UserId = item.UserId;
                    user.PartnerId = partnerId;
                    user.UserInfo = new MyInfoDto();
                    user.UserInfo = _mapper.Map<MyInfoDto>(item);
                    items.Add(user);
                }

                var tradingProviderFind = _tradingProviderRepository.FindAll(partnerId, - 1, 0, null, null);
                foreach (var item in tradingProviderFind.Items)
                {
                    var userTradingProviderFind = _usersEFRepository.Entity.Where(u => u.TradingProviderId == item.TradingProviderId && u.UserType == UserTypes.ROOT_TRADING_PROVIDER);
                    foreach (var userItem in userTradingProviderFind)
                    {
                        var user = _mapper.Map<UsersPartnerDto>(userItem);
                        user.TradingProviderId = (decimal)item.TradingProviderId;
                        if (userItem.InvestorId != null)
                        {
                            var investorFind = _investorRepository.FindById((int)userItem.InvestorId);
                            if (investorFind != null)
                            {
                                var investor = _mapper.Map<InvestorDto>(investorFind);
                                user.Investor = investor;
                                var investorIdenDefaultFind = _investorIdentificationRepository.GetByInvestorId((int)userItem.InvestorId);

                                if (investorIdenDefaultFind != null)
                                {
                                    user.Investor.InvestorIdentification = _mapper.Map<InvestorIdentificationDto>(investorIdenDefaultFind);
                                }
                            }
                        }
                        
                        var tradingProvider = _tradingProviderRepository.FindById((int)user.TradingProviderId);
                        if (tradingProvider != null)
                        {
                            var businessCustomer = _businessCustomerRepository.FindBusinessCustomerById(tradingProvider.BusinessCustomerId);
                            if (businessCustomer != null)
                            {
                                user.TradingProvider = new Entities.Dto.TradingProvider.TradingProviderDto();
                                user.TradingProvider.BusinessCustomer = _mapper.Map<BusinessCustomerDto>(businessCustomer);
                            }
                        }
                        user.UserInfo = new MyInfoDto();
                        user.UserInfo = _mapper.Map<MyInfoDto>(userItem);
                        items.Add(user);
                    }
                }
            }

            else if (userType == UserTypes.PARTNER)
            {
                var tradingProviderFind = _tradingProviderRepository.FindAll(CommonUtils.GetCurrentPartnerId(_httpContextAccessor), -1, 0, null, null);
                foreach (var item in tradingProviderFind.Items)
                {
                    var userFind = _usersEFRepository.Entity.Where(u => u.TradingProviderId == item.TradingProviderId && u.UserType == UserTypes.ROOT_TRADING_PROVIDER);
                    foreach (var userItem in userFind)
                    {
                        var user = _mapper.Map<UsersPartnerDto>(userItem);
                        if (userItem.InvestorId != null)
                        {
                            var investorFind = _investorRepository.FindById((int)userItem.InvestorId);
                            if (investorFind != null)
                            {
                                var investor = _mapper.Map<InvestorDto>(investorFind);
                                user.Investor = investor;
                                var investorIdenDefaultFind = _investorIdentificationRepository.GetByInvestorId((int)userItem.InvestorId);

                                if (investorIdenDefaultFind != null)
                                {
                                    user.Investor.InvestorIdentification = _mapper.Map<InvestorIdentificationDto>(investorIdenDefaultFind);
                                }
                            }
                        }
                        var tradingProvider = _tradingProviderRepository.FindById((int)user.TradingProviderId);
                        if (tradingProvider != null)
                        {
                            var businessCustomer = _businessCustomerRepository.FindBusinessCustomerById(tradingProvider.BusinessCustomerId);
                            if (businessCustomer != null)
                            {
                                user.TradingProvider = new Entities.Dto.TradingProvider.TradingProviderDto();
                                user.TradingProvider.BusinessCustomer = _mapper.Map<BusinessCustomerDto>(businessCustomer);

                            }
                        }
                        user.TradingProviderId = (decimal)userItem.TradingProviderId;
                        user.UserInfo = new MyInfoDto();
                        user.UserInfo = _mapper.Map<MyInfoDto>(userItem);
                        items.Add(user);
                    }
                }
            }

            else if (userType == UserTypes.ROOT_TRADING_PROVIDER)
            {
                var tradingProviderFind = _usersEFRepository.Entity.Where(u => u.TradingProviderId == CommonUtils.GetCurrentTradingProviderId(_httpContextAccessor));
                foreach (var item in tradingProviderFind)
                {
                    var user = new UsersPartnerDto();
                    if (item.InvestorId != null)
                    {
                        var investorFind = _investorRepository.FindById((int)item.InvestorId);
                        if (investorFind != null)
                        {
                            var investor = _mapper.Map<InvestorDto>(investorFind);
                            user.Investor = investor;
                            var investorIdenDefaultFind = _investorIdentificationRepository.GetByInvestorId((int)item.InvestorId);

                            if (investorIdenDefaultFind != null)
                            {
                                user.Investor.InvestorIdentification = _mapper.Map<InvestorIdentificationDto>(investorIdenDefaultFind);
                            }
                        }
                    }
                    var tradingProvider = _tradingProviderRepository.FindById((int)user.TradingProviderId);
                    if (tradingProvider != null)
                    {
                        var businessCustomer = _businessCustomerRepository.FindBusinessCustomerById(tradingProvider.BusinessCustomerId);
                        if (businessCustomer != null)
                        {
                            user.TradingProvider = new Entities.Dto.TradingProvider.TradingProviderDto();
                            user.TradingProvider.BusinessCustomer = _mapper.Map<BusinessCustomerDto>(businessCustomer);
                        }
                    }
                    user.TradingProviderId = (decimal)item.TradingProviderId;
                    user.UserId = item.UserId;
                    user.UserInfo = new MyInfoDto();
                    user.UserInfo = _mapper.Map<MyInfoDto>(item);
                    items.Add(user);
                }
            }

            var itemsQuery = items.AsQueryable();

            if (input.Keyword != null)
            {
                itemsQuery = itemsQuery.Where(p => p.UserInfo.UserName.Contains(input.Keyword));
            }
            
            if (input.Status != null && input.Status != Status.DA_XOA)
            {
                itemsQuery = itemsQuery.Where(u => u.UserInfo.Status == input.Status && u.UserInfo.IsDeleted == YesNo.NO);
            }  

            else if ((input.Status != null && input.Status == Status.DA_XOA))
            {
                itemsQuery = itemsQuery.Where(u => u.UserInfo.IsDeleted == YesNo.YES);
            }

            if (input.PageSize != -1)
            {
                itemsQuery = itemsQuery.Skip(input.Skip).Take(input.PageSize);
            }

            // Lọc theo keyword
            itemsQuery = itemsQuery.Where(o => input.Keyword == null || o.UserInfo.UserName.Contains(input.Keyword));

            result.Items = itemsQuery.OrderByDescending(o => o.UserId).ToList();
            result.TotalItems = items.Count();
            return result;
        }

        public List<int> FindRoleByUserId(int userId)
        {
            var roleFind = _userRoleRepository.Entity.Where(r => r.UserId == userId && r.Deleted == YesNo.NO && _dbContext.Roles.Any())
                .Select(x => x.RoleId);
            return roleFind.ToList();
        }

        public List<RoleDto> FindRoleByUserType()
        {
            var userType = CommonUtils.GetCurrentUserType(_httpContextAccessor);
            int? tradingProviderId = null;
            int? partnerId = null;
            if (userType == UserTypes.PARTNER || userType == UserTypes.ROOT_PARTNER)
            {
                partnerId = CommonUtils.GetCurrentPartnerId(_httpContextAccessor);
            }
            else if (userType == UserTypes.TRADING_PROVIDER || userType == UserTypes.ROOT_TRADING_PROVIDER)
            {
                tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContextAccessor);
            }
             
            var roleQuery = (from role in _dbContext.Roles
                             let roleDefault = _dbContext.RoleDefaultRelationships.FirstOrDefault(rd => rd.RoleId == role.Id && rd.Deleted == YesNo.NO && rd.Status == Status.ACTIVE
                                                && (partnerId == null || rd.PartnerId == partnerId) && (tradingProviderId == null || rd.TradingProviderId == tradingProviderId))
                             where role.Deleted == YesNo.NO && role.Status == Status.ACTIVE
                             && ((UserTypes.PARTNER_TYPES.Contains(userType) && ((role.PartnerId == partnerId && role.RoleType == RoleTypes.Partner)
                                    || (role.RoleType == RoleTypes.Default && roleDefault != null)))
                                || (UserTypes.TRADING_PROVIDER_TYPES.Contains(userType) && ((role.TradingProviderId == tradingProviderId && role.RoleType == RoleTypes.TradingProvider)
                                    || (role.RoleType == RoleTypes.Default && roleDefault != null)))
                                || (UserTypes.ROOT_ADMIN_TYPES.Contains(userType) && (userType == UserTypes.ROOT_EPIC && (role.RoleType == RoleTypes.Epic || role.RoleType == RoleTypes.Default))))
                             orderby role.Id descending
                             select new RoleDto
                             {
                                 Id = role.Id,
                                 Name = role.Name,
                                 PermissionInWeb = role.PermissionInWeb ?? 0,
                                 RoleType = role.RoleType,
                                 TradingProviderId = role.TradingProviderId,
                                 PartnerId = role.PartnerId,
                                 Status = roleDefault == null ? role.Status : roleDefault.Status,
                             }
                             );

            return roleQuery.ToList();
        }
    }
}
