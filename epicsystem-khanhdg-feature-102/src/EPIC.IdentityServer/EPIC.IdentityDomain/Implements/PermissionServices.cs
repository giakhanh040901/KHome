using AutoMapper;
using DocumentFormat.OpenXml.Office2010.Excel;
using EPIC.BondRepositories;
using EPIC.CoreDomain.Implements;
using EPIC.CoreRepositories;
using EPIC.DataAccess.Base;
using EPIC.DataAccess.Models;
using EPIC.Entities;
using EPIC.Entities.DataEntities;
using EPIC.IdentityDomain.Interfaces;
using EPIC.IdentityEntities.DataEntities;
using EPIC.IdentityEntities.Dto.PartnerPermission;
using EPIC.IdentityEntities.Dto.Permissions;
using EPIC.IdentityEntities.Dto.Roles;
using EPIC.IdentityEntities.Dto.TradingProviderPermission;
using EPIC.IdentityRepositories;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Identity;
using EPIC.Utils.RolePermissions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Security;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.IdentityDomain.Implements
{
    public class PermissionServices : IPermissionServices
    {
        private readonly EpicSchemaDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly PartnerPermissionEFRepository _partnerPermissionEFRepository;
        private readonly RoleEFRepository _roleEFRepository;
        private readonly UserRoleEFRepository _userRoleEFRepository;
        private readonly RolePermissionEFRepository _rolePermissionEFRepository;
        private readonly TradingProviderPermissionEFRepository _tradingProviderPermissionEFRepository;
        private readonly UsersEFRepository _usersEFRepository;
        private readonly string _connectionString;
        private readonly PartnerRepository _partnerRepository;

        public PermissionServices(EpicSchemaDbContext dbContext,
            IMapper mapper,
            ILogger<PermissionServices> logger,
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration,
            DatabaseOptions databaseOptions)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
            _configuration = configuration;
            _connectionString = databaseOptions.ConnectionString;
            _partnerRepository = new PartnerRepository(_connectionString, _logger);
            _httpContextAccessor = httpContextAccessor;
            _partnerPermissionEFRepository = new PartnerPermissionEFRepository(dbContext);
            _roleEFRepository = new RoleEFRepository(dbContext);
            _userRoleEFRepository = new UserRoleEFRepository(dbContext);
            _rolePermissionEFRepository = new RolePermissionEFRepository(dbContext);
            _tradingProviderPermissionEFRepository = new TradingProviderPermissionEFRepository(dbContext);
            _usersEFRepository = new UsersEFRepository(dbContext, logger);
        }

        #region Cấu hình Permission tối đa cho Partner
        /// <summary>
        /// Lấy danh sách Permission tối đa của Partner
        /// </summary>
        /// <param name="permissionInWeb"></param>
        /// <param name="isGetWeb"></param>
        /// <returns></returns>
        public List<PartnerPermissionDto> FindAllListMaxPermission(int partnerId, int permissionInWeb = 0, bool isGetWeb = false)
        {
            var permissionQuery = _partnerPermissionEFRepository.Entity.Where(p => p.PartnerId == partnerId && p.Deleted == YesNo.NO);
            if (isGetWeb)
            {
                permissionQuery = permissionQuery.Where(p => p.PermissionType == PermissionTypes.Web);
            }
            else
            {
                //permission trong web truyền vào
                var permissionKeyInWeb = PermissionConfig.Configs.Where(c => c.Value.PermissionInWeb == permissionInWeb).Select(p => p.Key).ToList();
                permissionQuery = permissionQuery.Where(p => permissionKeyInWeb.Contains(p.PermissionKey));
            }
            return _mapper.Map<List<PartnerPermissionDto>>(permissionQuery.ToList());
        }

        /// <summary>
        /// Thêm PermissionInWeb cho Partner
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        /// <exception cref="FaultException"></exception>
        public PartnerPermissionDto CreateMaxWebPermission(CreateMaxWebPermissionDto input)
        {
            var partnerPermissionQuery = _partnerPermissionEFRepository.Entity
                .Where(p => p.PartnerId == input.PartnerId && p.PermissionType == PermissionTypes.Web
                    && p.Deleted == YesNo.NO);

            var removeList = partnerPermissionQuery.Where(p => !input.PermissionWebKeys.Contains(p.PermissionKey)
                                                    && p.PermissionType == PermissionTypes.Web
                                                    && p.Deleted == YesNo.NO).ToList();
            //Xóa các permission đang được sử dụng của Web bị remove
            foreach (var removeItem in removeList)
            {
                //Xóa permission trong bảng P_Partner_Permission đã được cấu hình từ trước
                var partnerPermissionsRemove = _partnerPermissionEFRepository.Entity.Where(r => r.PartnerId == input.PartnerId && r.PermissionInWeb == removeItem.PermissionInWeb);
                foreach (var item in partnerPermissionsRemove)
                {
                    _partnerPermissionEFRepository.Entity.Remove(item);
                }
                //Xóa Role đang được cấu hình bởi Web bị remove bảng P_ROLE
                var rolesRemove = _roleEFRepository.Entity.Where(r => r.PartnerId == input.PartnerId && r.PermissionInWeb == removeItem.PermissionInWeb);
                foreach (var roleItem in rolesRemove)
                {
                    _roleEFRepository.Entity.Remove(roleItem);

                    //Xóa Permission của Role bị remove bảng P_ROLE_PERMISSION
                    var rolePermissionRemove = _rolePermissionEFRepository.Entity.Where(r => r.RoleId == roleItem.Id);
                    foreach (var rolePermissionItem in rolePermissionRemove)
                    {
                        _rolePermissionEFRepository.Entity.Remove(rolePermissionItem);
                    }
                    //Xóa role được gán vào user bảng P_USER_ROLE
                    var userRoleRemove = _userRoleEFRepository.Entity.Where(r => r.RoleId == roleItem.Id);
                    foreach (var userRoleItem in userRoleRemove)
                    {
                        _userRoleEFRepository.Entity.Remove(userRoleItem);
                    }
                }
            }

            foreach (var permissionWeb in input.PermissionWebKeys)
            {
                var permissionValue = PermissionConfig.Configs[permissionWeb];
                var permission = partnerPermissionQuery.FirstOrDefault(p => p.PermissionKey == permissionWeb && p.PermissionType == PermissionTypes.Web);
                if (permission == null) //không thấy thì thêm
                {
                    if (permissionValue.PermissionType == PermissionTypes.Web)
                    {
                        _partnerPermissionEFRepository.Entity.Add(new PartnerPermission
                        {
                            Id = (int)_partnerPermissionEFRepository.NextKey(),
                            PartnerId = input.PartnerId,
                            PermissionKey = permissionWeb,
                            PermissionType = permissionValue.PermissionType,
                            PermissionInWeb = permissionValue.PermissionInWeb,
                            CreatedBy = CommonUtils.GetCurrentUsername(_httpContextAccessor),
                            CreatedDate = DateTime.Now,
                            Deleted = YesNo.NO
                        });
                    }
                }
            }
            _dbContext.SaveChanges();
            return null;
        }

        public void DeleteMaxWebPermission(int partnerId, string permissionWebKeys)
        {
            var permissionValue = PermissionConfig.Configs[permissionWebKeys];
            if (permissionValue.PermissionType != PermissionTypes.Web)
            {
                throw new FaultException(new FaultReason($"Permission có key: \"{permissionWebKeys}\" không phải là permission web"), new FaultCode(((int)ErrorCode.BadRequest).ToString()), "");
            }

            var partnerPermissionQuery = _partnerPermissionEFRepository.Entity
                .Where(p => p.PartnerId == partnerId
                    && p.Deleted == YesNo.NO
                    && p.PermissionInWeb == permissionValue.PermissionInWeb);
            var removeList = partnerPermissionQuery.ToList();
            foreach (var removeItem in removeList)
            {
                removeItem.Deleted = YesNo.YES;
            }
            _dbContext.SaveChanges();
        }

        /// <summary>
        /// Update List Permission Max cho partnerId
        /// </summary>
        /// <param name="permissionInWeb"></param>
        /// <param name="permissionKeys"></param>
        public void UpdateListMaxPermissionInWeb(UpdateMaxPermissionInWeb input, int partnerId)
        {
            var partnerPermissionQuery = _partnerPermissionEFRepository.Entity
                .Where(p => p.PartnerId == partnerId
                    && p.Deleted == YesNo.NO
                    && p.PermissionInWeb == input.PermissionInWeb);

            //remove những phần tử không nằm trong danh sách của web đang xét trừ permission có type là web
            var removeList = partnerPermissionQuery.Where(p => input.PermissionKeysRemove.Contains(p.PermissionKey)
                                                        && p.PermissionType != PermissionTypes.Web
                                                        && p.PermissionInWeb == input.PermissionInWeb
                                                        && p.Deleted == YesNo.NO).ToList();

            //Xóa các permission được remove
            foreach (var removeItem in removeList)
            {
                //Xóa permission trong bảng P_PARTNER_Permission 
                _partnerPermissionEFRepository.Entity.Remove(removeItem);

                //Tìm kiếm các role được tạo của đối tác này
                var rolesRemove = _roleEFRepository.Entity.Where(r => r.PartnerId == partnerId && r.PermissionInWeb == removeItem.PermissionInWeb);
                foreach (var roleItem in rolesRemove)
                {
                    //Xóa Permission bị remove mà đang có trong role
                    var rolePermissionRemove = _rolePermissionEFRepository.Entity.Where(r => r.RoleId == roleItem.Id && r.PermissionKey == removeItem.PermissionKey);
                    foreach (var rolePermissionItem in rolePermissionRemove)
                    {
                        _rolePermissionEFRepository.Entity.Remove(rolePermissionItem);
                    }
                }

                var tradingProviderFind = _partnerRepository.FindTradingProviderByPartner(partnerId);
                foreach (var tradingProviderItem in tradingProviderFind)
                {
                    //Lấy key trong permission tối đa của đại lý đang xét là key đang xóa
                    var permissionKeyInMaxPermission = _tradingProviderPermissionEFRepository.Entity.FirstOrDefault(r => r.TradingProviderId == tradingProviderItem.TradingProviderId && r.PermissionKey == removeItem.PermissionKey);
                    if (permissionKeyInMaxPermission != null)
                    {
                        _tradingProviderPermissionEFRepository.Entity.Remove(permissionKeyInMaxPermission);
                    }

                    //Tìm kiếm các role được tạo của đại lý này
                    var rolesTradingProviderRemove = _roleEFRepository.Entity.Where(r => r.TradingProviderId == tradingProviderItem.TradingProviderId && r.PermissionInWeb == removeItem.PermissionInWeb);
                    foreach (var roleItem in rolesTradingProviderRemove)
                    {
                        //Xóa Permission bị remove mà đang có trong role
                        var rolePermissionRemove = _rolePermissionEFRepository.Entity.Where(r => r.RoleId == roleItem.Id && r.PermissionKey == removeItem.PermissionKey);
                        foreach (var rolePermissionItem in rolePermissionRemove)
                        {
                            _rolePermissionEFRepository.Entity.Remove(rolePermissionItem);
                        }
                    }
                }
            }

            //thêm
            foreach (var permissionKey in input.PermissionKeys)
            {
                var permissionValue = PermissionConfig.Configs[permissionKey];
                var permission = partnerPermissionQuery.FirstOrDefault(p => p.PermissionKey == permissionKey && p.PermissionInWeb == input.PermissionInWeb);
                if (permission == null) //không thấy thì thêm
                {
                    if (permissionValue.PermissionInWeb == input.PermissionInWeb)
                    {
                        _partnerPermissionEFRepository.Entity.Add(new PartnerPermission
                        {
                            Id = (int)_partnerPermissionEFRepository.NextKey(),
                            PartnerId = partnerId,
                            PermissionKey = permissionKey,
                            PermissionType = permissionValue.PermissionType,
                            PermissionInWeb = permissionValue.PermissionInWeb,
                            CreatedBy = CommonUtils.GetCurrentUsername(_httpContextAccessor),
                            CreatedDate = DateTime.Now,
                            Deleted = YesNo.NO
                        });
                    }
                }
            }
            _dbContext.SaveChanges();
        }
        #endregion

        #region Cấu hình Permission tối đa cho ĐLSC

        /// <summary>
        /// Cấu hình Web tối đa cho ĐLSC
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public TradingProviderPermissionDto CreateMaxWebPermissionToTrading(CreateMaxWebPermissionToTradingDto input)
        {
            var tradingProviderPermissionQuery = _tradingProviderPermissionEFRepository.Entity
                .Where(p => p.TradingProviderId == input.TradingProviderId && p.PermissionType == PermissionTypes.Web);

            //Remove
            var removeList = tradingProviderPermissionQuery.Where(p => !input.PermissionWebKeys.Contains(p.PermissionKey)
                                                    && p.PermissionType == PermissionTypes.Web).ToList();

            foreach (var removeItem in removeList)
            {
                //Xóa permission trong bảng P_Partner_Permission đã được cấu hình từ trước
                var tradingPermissionsRemove = _tradingProviderPermissionEFRepository.Entity.Where(r => r.TradingProviderId == input.TradingProviderId && r.PermissionInWeb == removeItem.PermissionInWeb);
                foreach (var item in tradingPermissionsRemove)
                {
                    _tradingProviderPermissionEFRepository.Entity.Remove(item);
                }
                //Xóa Role đang được cấu hình bởi Web bị remove bảng P_ROLE
                var rolesRemove = _roleEFRepository.Entity.Where(r => r.TradingProviderId == input.TradingProviderId && r.PermissionInWeb == removeItem.PermissionInWeb);
                foreach (var roleItem in rolesRemove)
                {
                    _roleEFRepository.Entity.Remove(roleItem);

                    //Xóa Permission của Role bị remove bảng P_ROLE_PERMISSION
                    var rolePermissionRemove = _rolePermissionEFRepository.Entity.Where(r => r.RoleId == roleItem.Id);
                    foreach (var rolePermissionItem in rolePermissionRemove)
                    {
                        _rolePermissionEFRepository.Entity.Remove(rolePermissionItem);
                    }
                    //Xóa role được gán vào user bảng P_USER_ROLE
                    var userRoleRemove = _userRoleEFRepository.Entity.Where(r => r.RoleId == roleItem.Id);
                    foreach (var userRoleItem in userRoleRemove)
                    {
                        _userRoleEFRepository.Entity.Remove(userRoleItem);
                    }
                }
            }

            foreach (var permissionWeb in input.PermissionWebKeys)
            {
                var permissionValue = PermissionConfig.Configs[permissionWeb];
                var permission = tradingProviderPermissionQuery.FirstOrDefault(p => p.PermissionKey == permissionWeb && p.PermissionType == PermissionTypes.Web);
                if (permission == null) //không thấy thì thêm
                {
                    if (permissionValue.PermissionType == PermissionTypes.Web)
                    {
                        _tradingProviderPermissionEFRepository.Entity.Add(new TradingProviderPermission
                        {
                            Id = (int)_tradingProviderPermissionEFRepository.NextKey(),
                            TradingProviderId = input.TradingProviderId,
                            PermissionKey = permissionWeb,
                            PermissionType = permissionValue.PermissionType,
                            PermissionInWeb = permissionValue.PermissionInWeb,
                            CreatedBy = CommonUtils.GetCurrentUsername(_httpContextAccessor),
                            CreatedDate = DateTime.Now,
                        });
                    }
                }
            }
            _dbContext.SaveChanges();
            return null;
        }

        /// <summary>
        /// Cấu hình Permission tối đa cho mỗi Web của ĐLSC
        /// </summary>
        /// <param name="permissionInWeb"></param>
        /// <param name="permissionKeys"></param>
        /// <param name="tradingProviderId"></param>
        /// <exception cref="FaultException"></exception>
        public void UpdateListMaxPermissionToTradingInWeb(UpdateMaxPermissionInWeb input, int tradingProviderId)
        {
            var tradingProviderPermissionQuery = _tradingProviderPermissionEFRepository.Entity
                .Where(p => p.TradingProviderId == tradingProviderId && p.PermissionInWeb == input.PermissionInWeb);


            //remove những phần tử không nằm trong danh sách của web đang xét trừ permission có type là web
            var removeList = tradingProviderPermissionQuery.Where(p => input.PermissionKeysRemove.Contains(p.PermissionKey)
                                                        && p.PermissionType != PermissionTypes.Web
                                                        && p.PermissionInWeb == input.PermissionInWeb).ToList();
            foreach (var removeItem in removeList)
            {
                //Xóa permission trong bảng P_TRADING_Permission đã được cấu hình từ trước
                _tradingProviderPermissionEFRepository.Entity.Remove(removeItem);

                //Tìm role được cấu hình trong đại lý
                var rolesRemove = _roleEFRepository.Entity.Where(r => r.TradingProviderId == tradingProviderId && r.PermissionInWeb == removeItem.PermissionInWeb);
                foreach (var roleItem in rolesRemove)
                {
                    //Xóa Permission bị remove mà đang có trong role
                    var rolePermissionRemove = _rolePermissionEFRepository.Entity.Where(r => r.RoleId == roleItem.Id && r.PermissionKey == removeItem.PermissionKey);
                    foreach (var rolePermissionItem in rolePermissionRemove)
                    {
                        _rolePermissionEFRepository.Entity.Remove(rolePermissionItem);
                    }
                }
            }
            //thêm
            foreach (var permissionKey in input.PermissionKeys)
            {
                var permissionValue = PermissionConfig.Configs[permissionKey];
                var permission = tradingProviderPermissionQuery.FirstOrDefault(p => p.PermissionKey == permissionKey && p.PermissionInWeb == input.PermissionInWeb);
                if (permission == null) //không thấy thì thêm
                {
                    if (permissionValue.PermissionInWeb == input.PermissionInWeb)
                    {
                        _tradingProviderPermissionEFRepository.Entity.Add(new TradingProviderPermission
                        {
                            Id = (int)_tradingProviderPermissionEFRepository.NextKey(),
                            TradingProviderId = tradingProviderId,
                            PermissionKey = permissionKey,
                            PermissionType = permissionValue.PermissionType,
                            PermissionInWeb = permissionValue.PermissionInWeb,
                            CreatedBy = CommonUtils.GetCurrentUsername(_httpContextAccessor),
                            CreatedDate = DateTime.Now,
                        });
                    }
                }
            }
            _dbContext.SaveChanges();
        }

        public List<TradingProviderPermissionDto> FindAllListMaxPermissionInTrading(int tradingProviderId, int permissionInWeb = 0, bool isGetWeb = false)
        {
            var permissionQuery = _tradingProviderPermissionEFRepository.Entity.Where(p => p.TradingProviderId == tradingProviderId);
            if (isGetWeb)
            {
                permissionQuery = permissionQuery.Where(p => p.PermissionType == PermissionTypes.Web);
            }
            else
            {
                //permission trong web truyền vào
                var permissionKeyInWeb = PermissionConfig.Configs.Where(c => c.Value.PermissionInWeb == permissionInWeb).Select(p => p.Key).ToList();
                permissionQuery = permissionQuery.Where(p => permissionKeyInWeb.Contains(p.PermissionKey));
            }
            return _mapper.Map<List<TradingProviderPermissionDto>>(permissionQuery);
        }
        #endregion

        public List<string> GetPermission(int? permissionInWeb = null)
        {
            var userType = CommonUtils.GetCurrentUserType(_httpContextAccessor);
            int? partnerId = null;
            int? tradingProviderId = null;
            if (userType == UserTypes.ROOT_PARTNER || userType == UserTypes.PARTNER)
            {
                partnerId = CommonUtils.GetCurrentPartnerId(_httpContextAccessor);
            }
            else if (userType == UserTypes.ROOT_TRADING_PROVIDER || userType == UserTypes.TRADING_PROVIDER)
            {
                tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContextAccessor);
            }

            IEnumerable<string> results = Enumerable.Empty<string>();

            // Lấy danh sách quyền theo user được phân trong UserRole
            var userRolePermission = from role in _dbContext.Roles
                                     let roleDefault = _dbContext.RoleDefaultRelationships.FirstOrDefault(rd => role.RoleType == RoleTypes.Default && rd.RoleId == role.Id && rd.Deleted == YesNo.NO
                                                && (partnerId == null || rd.PartnerId == partnerId) && (tradingProviderId == null || rd.TradingProviderId == tradingProviderId))
                                     join userRole in _dbContext.UserRoles on role.Id equals userRole.RoleId
                                     join rolePermission in _dbContext.RolePermissions on role.Id equals rolePermission.RoleId
                                     where role.Deleted == YesNo.NO && role.Status == Status.ACTIVE && rolePermission.Deleted == YesNo.NO
                                     && (permissionInWeb == null || role.PermissionInWeb == permissionInWeb) && userRole.Deleted == YesNo.NO
                                     && userRole.UserId == CommonUtils.GetCurrentUserId(_httpContextAccessor) 
                                     && (roleDefault == null || roleDefault.Status == Status.ACTIVE)
                                     select rolePermission.PermissionKey;
            results = results.Union(userRolePermission);

            if (userType == UserTypes.ROOT_EPIC)
            {
                results = results.Union(PermissionConfig.Configs.Select(r => r.Key));
            }
            else if (userType == UserTypes.ROOT_PARTNER)
            {
                //Danh sách quyền tối đa
                var permission = _partnerPermissionEFRepository.Entity.Where(p => p.PartnerId == partnerId
                                    && (permissionInWeb == null || p.PermissionInWeb == permissionInWeb) && p.Deleted == YesNo.NO);
                results = results.Union(permission.Select(p => p.PermissionKey));

                // Lấy danh sách quyền mặc định
                var roleDefault = from role in _dbContext.Roles
                                  join roleDefaultRelationship in _dbContext.RoleDefaultRelationships on role.Id equals roleDefaultRelationship.RoleId
                                  join rolePermission in _dbContext.RolePermissions on role.Id equals rolePermission.RoleId
                                  where role.Deleted == YesNo.NO && role.Status == Status.ACTIVE && rolePermission.Deleted == YesNo.NO
                                  && (permissionInWeb == null || role.PermissionInWeb == permissionInWeb) && roleDefaultRelationship.Deleted == YesNo.NO
                                  && roleDefaultRelationship.PartnerId == partnerId && roleDefaultRelationship.Status == Status.ACTIVE
                                  select rolePermission.PermissionKey;
                results = results.Union(roleDefault);
            }
            else if (userType == UserTypes.ROOT_TRADING_PROVIDER)
            {
                var permission = _tradingProviderPermissionEFRepository.Entity.Where(p => p.TradingProviderId == tradingProviderId
                                    && (permissionInWeb == null || p.PermissionInWeb == permissionInWeb));
                results = results.Union(permission.Select(p => p.PermissionKey));

                // Lấy danh sách quyền mặc định
                var roleDefault = from role in _dbContext.Roles
                                  join roleDefaultRelationship in _dbContext.RoleDefaultRelationships on role.Id equals roleDefaultRelationship.RoleId
                                  join rolePermission in _dbContext.RolePermissions on role.Id equals rolePermission.RoleId
                                  where role.Deleted == YesNo.NO && role.Status == Status.ACTIVE && rolePermission.Deleted == YesNo.NO
                                  && (permissionInWeb == null || role.PermissionInWeb == permissionInWeb) && roleDefaultRelationship.Deleted == YesNo.NO
                                  && roleDefaultRelationship.TradingProviderId == tradingProviderId && roleDefaultRelationship.Status == Status.ACTIVE
                                  select rolePermission.PermissionKey;
                results = results.Union(roleDefault);
            }
            return results.ToList();
        }
    }
}
