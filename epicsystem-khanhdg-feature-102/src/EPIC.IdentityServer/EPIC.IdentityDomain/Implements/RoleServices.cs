using AutoMapper;
using EPIC.DataAccess.Base;
using EPIC.DataAccess.Models;
using EPIC.Entities.DataEntities;
using EPIC.IdentityDomain.Interfaces;
using EPIC.IdentityEntities.DataEntities;
using EPIC.IdentityEntities.Dto.Permissions;
using EPIC.IdentityEntities.Dto.Roles;
using EPIC.IdentityRepositories;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Identity;
using EPIC.Utils.RolePermissions;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;

namespace EPIC.IdentityDomain.Implements
{
    public class RoleServices : IRoleServices
    {
        private readonly EpicSchemaDbContext _dbContext;
        private readonly RoleEFRepository _roleEFRepository;
        private readonly PartnerPermissionEFRepository _partnerPermissionEFRepository;
        private readonly TradingProviderPermissionEFRepository _tradingProviderPermissionEFRepository;
        private readonly RolePermissionEFRepository _rolePermissionEFRepository;
        private readonly UserRoleEFRepository _userRoleEFRepository;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public RoleServices(EpicSchemaDbContext dbContext,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _roleEFRepository = new RoleEFRepository(dbContext);
            _partnerPermissionEFRepository = new PartnerPermissionEFRepository(dbContext);
            _tradingProviderPermissionEFRepository = new TradingProviderPermissionEFRepository(dbContext);
            _rolePermissionEFRepository = new RolePermissionEFRepository(dbContext);
            _userRoleEFRepository = new UserRoleEFRepository(dbContext);
        }

        #region Cấu hình role mẫu
        public PagingResult<RoleDto> FindAll(FilterRoleDto input)
        {
            var result = new PagingResult<RoleDto>();
            var find = _roleEFRepository.FindAll(input);
            result.Items = _mapper.Map<IEnumerable<RoleDto>>(find.Items);
            result.TotalItems = find.TotalItems;
            return result;
        }

        public RoleDto FindByIdTemplate(int roleId)
        {
            var roleFind = _roleEFRepository.Entity.FirstOrDefault(r => r.Id == roleId && r.PartnerId == null);
            if (roleFind == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy thông tin role"), new FaultCode(((int)ErrorCode.NotFound).ToString()), "");
            }
            return _mapper.Map<RoleDto>(roleFind);
        }

        public RoleDto AddRoleTemplate(CreateRoleDto input)
        {
            var roleInsert = _roleEFRepository.Entity.Add(new Role
            {
                Id = (int)_roleEFRepository.NextKey(),
                Name = input.Name,
                CreatedBy = CommonUtils.GetCurrentUsername(_httpContextAccessor),
                RoleType = RoleTypes.Default,
                CreatedDate = System.DateTime.Now,
                Deleted = YesNo.NO
            });
            _dbContext.SaveChanges();
            return _mapper.Map<RoleDto>(roleInsert.Entity);
        }

        public RoleDto UpdateRoleTemplate(UpdateRoleDto input)
        {
            var roleFind = _roleEFRepository.Entity.FirstOrDefault(r => r.Id == input.Id && r.PartnerId == null);
            if (roleFind == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy thông tin role"), new FaultCode(((int)ErrorCode.NotFound).ToString()), "");
            }

            roleFind.Name = input.Name;
            _dbContext.SaveChanges();
            return _mapper.Map<RoleDto>(roleFind);
        }

        #endregion

        public RoleDto DeleteRole(int roleId)
        {
            var userType = CommonUtils.GetCurrentUserType(_httpContextAccessor);
            var username = CommonUtils.GetCurrentUsername(_httpContextAccessor);
            int? partnerId = null;
            int? tradingProviderId = null;
            if (userType == UserTypes.PARTNER || userType == UserTypes.ROOT_PARTNER)
            {
                partnerId = CommonUtils.GetCurrentPartnerId(_httpContextAccessor);
            }
            else if (userType == UserTypes.TRADING_PROVIDER || userType == UserTypes.ROOT_TRADING_PROVIDER)
            {
                tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContextAccessor);
            }

            var roleFind = _roleEFRepository.Entity.FirstOrDefault(r => r.Id == roleId && (UserTypes.ROOT_ADMIN_TYPES.Contains(userType) || (r.RoleType == RoleTypes.Partner && r.PartnerId == partnerId)
                                || (r.RoleType == RoleTypes.TradingProvider && r.TradingProviderId == tradingProviderId)) && r.Deleted == YesNo.NO);
            if (roleFind == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy thông tin role"), new FaultCode(((int)ErrorCode.NotFound).ToString()), "");
            }
            // Nếu là mặc định
            if (roleFind.RoleType == RoleTypes.Default && !UserTypes.ROOT_ADMIN_TYPES.Contains(userType))
            {
                var roleDefaultFind = _dbContext.RoleDefaultRelationships.FirstOrDefault(r => r.RoleId == roleId && ((userType == UserTypes.ROOT_PARTNER && r.PartnerId == partnerId)
                                        || (userType == UserTypes.ROOT_TRADING_PROVIDER && r.TradingProviderId == tradingProviderId)) && r.Deleted == YesNo.NO);
                if (roleDefaultFind == null)
                {
                    throw new FaultException(new FaultReason($"Không tìm thấy thông tin role mặc định"), new FaultCode(((int)ErrorCode.NotFound).ToString()), "");
                }
                roleDefaultFind.Deleted = YesNo.YES;
                roleDefaultFind.ModifiedBy = username;
                roleDefaultFind.ModifiedDate = DateTime.Now;
            }
            else
            {
                roleFind.Deleted = YesNo.YES;
                roleFind.ModifiedBy = username;
                roleFind.ModifiedDate = DateTime.Now;
            }
            _dbContext.SaveChanges();
            return _mapper.Map<RoleDto>(roleFind);
        }

        /// <summary>
        /// Thay đổi trạng thái của role
        /// </summary>
        /// <param name="roleId"></param>
        /// <exception cref="FaultException"></exception>
        public void ChangeStatusRole(int roleId)
        {
            var userType = CommonUtils.GetCurrentUserType(_httpContextAccessor);
            var username = CommonUtils.GetCurrentUsername(_httpContextAccessor);
            int? partnerId = null;
            int? tradingProviderId = null;
            if (userType == UserTypes.PARTNER || userType == UserTypes.ROOT_PARTNER)
            {
                partnerId = CommonUtils.GetCurrentPartnerId(_httpContextAccessor);
            }
            else if (userType == UserTypes.TRADING_PROVIDER || userType == UserTypes.ROOT_TRADING_PROVIDER)
            {
                tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContextAccessor);
            }

            var roleFind = _roleEFRepository.Entity.FirstOrDefault(r => r.Id == roleId && (UserTypes.ROOT_ADMIN_TYPES.Contains(userType) || r.RoleType == RoleTypes.Default || (r.RoleType == RoleTypes.Partner && r.PartnerId == partnerId)
                                || (r.RoleType == RoleTypes.TradingProvider && r.TradingProviderId == tradingProviderId)) && r.Deleted == YesNo.NO);
            if (roleFind == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy thông tin role"), new FaultCode(((int)ErrorCode.NotFound).ToString()), "");
            }
            // Nếu là mặc định
            if (roleFind.RoleType == RoleTypes.Default && !UserTypes.ROOT_ADMIN_TYPES.Contains(userType))
            {
                var roleDefaultFind = _dbContext.RoleDefaultRelationships.FirstOrDefault(r => r.RoleId == roleId && ((userType == UserTypes.ROOT_PARTNER && r.PartnerId == partnerId)
                                        || (userType == UserTypes.ROOT_TRADING_PROVIDER && r.TradingProviderId == tradingProviderId)) && r.Deleted == YesNo.NO);
                if (roleDefaultFind == null)
                {
                    throw new FaultException(new FaultReason($"Không tìm thấy thông tin role mặc định"), new FaultCode(((int)ErrorCode.NotFound).ToString()), "");
                }
                roleDefaultFind.Status = roleDefaultFind.Status == Status.ACTIVE ? Status.INACTIVE : Status.ACTIVE;
                roleDefaultFind.ModifiedBy = username;
                roleDefaultFind.ModifiedDate = DateTime.Now;
            }
            else
            {
                roleFind.Status = roleFind.Status == Status.ACTIVE ? Status.INACTIVE : Status.ACTIVE;
                roleFind.ModifiedBy = username;
                roleFind.ModifiedDate = DateTime.Now;
            }
            _dbContext.SaveChanges();
        }

        /// <summary>
        /// Xem thông tin quyền
        /// </summary>
        public RolePermissionInfoDto FindRoleById(int roleId)
        {
            var userType = CommonUtils.GetCurrentUserType(_httpContextAccessor);
            var roleFind = _roleEFRepository.Entity.FirstOrDefault(r => r.Id == roleId && (UserTypes.ADMIN_TYPES.Contains(userType)
                                || (UserTypes.PARTNER_TYPES.Contains(userType) && r.PartnerId == CommonUtils.GetCurrentPartnerId(_httpContextAccessor))
                                || (UserTypes.TRADING_PROVIDER_TYPES.Contains(userType) && r.TradingProviderId == CommonUtils.GetCurrentTradingProviderId(_httpContextAccessor))));
            if (roleFind == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy thông tin role"), new FaultCode(((int)ErrorCode.NotFound).ToString()), "");
            }
            var result = _mapper.Map<RolePermissionInfoDto>(roleFind);
            var permissionFind = _rolePermissionEFRepository.Entity.Where(p => p.RoleId == roleId && p.Deleted == YesNo.NO);
            result.PermissionKeys = _mapper.Map<List<RolePermissionDto>>(permissionFind);
            return result;
        }

        #region Cấu hình Role cho Epic

        public PagingResult<RoleDto> FindAllByEpic(FilterRoleDto input)
        {
            var userType = CommonUtils.GetCurrentUserType(_httpContextAccessor);
            var result = new PagingResult<RoleDto>();
            var find = _roleEFRepository.FindAll(input, null, null, userType);
            result.Items = _mapper.Map<IEnumerable<RoleDto>>(find.Items);
            foreach (var item in result.Items)
            {
                var totalUse = _userRoleEFRepository.TotalUse(item.Id);
                item.TotalUse = totalUse;
            }
            result.TotalItems = find.TotalItems;
            return result;
        }

        public RoleDto AddRole(CreateRolePermissionDto input)
        {
            var userType = CommonUtils.GetCurrentUserType(_httpContextAccessor);
            var username = CommonUtils.GetCurrentUsername(_httpContextAccessor);
            var transaction = _dbContext.Database.BeginTransaction();
            int? tradingProviderId = null;
            int? partnerId = null;
            if (UserTypes.ROOT_ADMIN_TYPES.Contains(userType))
            {
                input.RoleType = input.RoleType ?? RoleTypes.Epic;
            }
            else if (UserTypes.PARTNER_TYPES.Contains(userType))
            {
                partnerId = CommonUtils.GetCurrentPartnerId(_httpContextAccessor);
                input.RoleType = RoleTypes.Partner;
            }
            else if (UserTypes.TRADING_PROVIDER_TYPES.Contains(userType))
            {
                tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContextAccessor);
                input.RoleType = RoleTypes.TradingProvider;
            }
            else
            {
                throw new FaultException(new FaultReason($"Loại tài khoản không hợp lệ"), new FaultCode(((int)ErrorCode.NotFound).ToString()), "");
            }

            var roleInsert = _roleEFRepository.Entity.Add(new Role
            {
                Id = (int)_roleEFRepository.NextKey(),
                CreatedBy = username,
                PartnerId = partnerId,
                TradingProviderId = tradingProviderId,
                RoleType = input.RoleType ?? RoleTypes.Epic,
                Name = input.Name,
                Status = Status.ACTIVE,
                PermissionInWeb = input.PermissionInWeb,
                CreatedDate = System.DateTime.Now,
                Deleted = YesNo.NO,
            });

            //thêm
            foreach (var permissionKey in input.PermissionKeys)
            {
                var permissionValue = PermissionConfig.Configs[permissionKey];

                // Nếu là đối tác thì kiểm tra xem có trong quyền tối đa của đối tác 
                if (UserTypes.PARTNER_TYPES.Contains(userType) && !_dbContext.PartnerPermissions.Any(p => p.PermissionKey == permissionKey && p.Deleted == YesNo.NO
                    && p.PartnerId == CommonUtils.GetCurrentPartnerId(_httpContextAccessor)))
                {
                    continue;
                }
                // Nếu là đại lý thì kiểm tra xem có trong quyền tối đa của đại lý
                else if (UserTypes.TRADING_PROVIDER_TYPES.Contains(userType) && !_dbContext.TradingProviderPermissions.Any(p => p.PermissionKey == permissionKey
                        && p.TradingProviderId == CommonUtils.GetCurrentTradingProviderId(_httpContextAccessor)))
                {
                    continue;
                }

                _rolePermissionEFRepository.Entity.Add(new RolePermission
                {
                    Id = (int)_rolePermissionEFRepository.NextKey(),
                    RoleId = roleInsert.Entity.Id,
                    PermissionKey = permissionKey,
                    PermissionType = permissionValue.PermissionType,
                    CreatedBy = username,
                    CreatedDate = System.DateTime.Now,
                    Deleted = YesNo.NO
                });
            }
            _dbContext.SaveChanges();

            // Nếu là role mặc định thì thêm vào cho các người dùng khác
            if (input.RoleType == RoleTypes.Default)
            {
                var userTradingProvider = _dbContext.Users.Where(u => u.IsDeleted == YesNo.NO && u.TradingProviderId != null).Select(u => u.TradingProviderId).Distinct();
                foreach (var item in userTradingProvider)
                {
                    _dbContext.RoleDefaultRelationships.Add(new RoleDefaultRelationship
                    {
                        Id = (int)_roleEFRepository.NextKey(RoleDefaultRelationship.SEQ),
                        TradingProviderId = (int)item,
                        CreatedBy= username,
                        CreatedDate = DateTime.Now,
                        RoleId = roleInsert.Entity.Id,
                        Status = Status.ACTIVE,
                        Deleted = YesNo.NO
                    });
                }

                var userPartner = _dbContext.Users.Where(u => u.IsDeleted == YesNo.NO && u.PartnerId != null).Select(u => u.PartnerId).Distinct();
                foreach (var item in userPartner)
                {
                    _dbContext.RoleDefaultRelationships.Add(new RoleDefaultRelationship
                    {
                        Id = (int)_roleEFRepository.NextKey(RoleDefaultRelationship.SEQ),
                        PartnerId = (int)item,
                        CreatedBy = username,
                        CreatedDate = DateTime.Now,
                        RoleId = roleInsert.Entity.Id,
                        Status = Status.ACTIVE,
                        Deleted = YesNo.NO
                    });
                }
            }
            _dbContext.SaveChanges();

            transaction.Commit();
            return _mapper.Map<RoleDto>(roleInsert.Entity);
        }

        /// <summary>
        /// Cấu hình permission cho role 
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="input"></param>
        /// <exception cref="FaultException"></exception>
        public void UpdatePermissionInRole(int roleId, CreateRolePermissionDto input)
        {
            var userType = CommonUtils.GetCurrentUserType(_httpContextAccessor);
            var username = CommonUtils.GetCurrentUsername(_httpContextAccessor);
            var transaction = _dbContext.Database.BeginTransaction();
            var role = _roleEFRepository.Entity.FirstOrDefault(r => r.Id == roleId  && r.Deleted == YesNo.NO
                        && ((UserTypes.ROOT_ADMIN_TYPES.Contains(userType) && (r.RoleType == RoleTypes.Epic || r.RoleType == RoleTypes.Default))
                            || (UserTypes.PARTNER_TYPES.Contains(userType) && r.RoleType == RoleTypes.Partner && r.PartnerId == CommonUtils.GetCurrentPartnerId(_httpContextAccessor))
                            || (UserTypes.TRADING_PROVIDER_TYPES.Contains(userType) && r.RoleType == RoleTypes.TradingProvider && r.TradingProviderId == CommonUtils.GetCurrentTradingProviderId(_httpContextAccessor))));
            if (role == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy thông tin role"), new FaultCode(((int)ErrorCode.NotFound).ToString()), "");
            }
            role.Name = input.Name;
            role.ModifiedDate = DateTime.Now;
            role.ModifiedBy = username;

            var rolePermissionQuery = _rolePermissionEFRepository.Entity.Where(p => p.RoleId == roleId && p.Deleted == YesNo.NO);

            //xóa những Permission không có trong List
            var removeList = rolePermissionQuery.Where(p => p.RoleId == roleId && input.PermissionKeysRemove.Contains(p.PermissionKey));
            foreach (var removeItem in removeList)
            {
                _rolePermissionEFRepository.Entity.Remove(removeItem);
            }

            //thêm
            foreach (var permissionKey in input.PermissionKeys)
            {
                var permissionValue = PermissionConfig.Configs[permissionKey];

                // Nếu là đối tác thì kiểm tra xem có trong quyền tối đa của đối tác 
                if (UserTypes.PARTNER_TYPES.Contains(userType) && !_dbContext.PartnerPermissions.Any(p => p.PermissionKey == permissionKey && p.Deleted == YesNo.NO
                    && p.PartnerId == CommonUtils.GetCurrentPartnerId(_httpContextAccessor)))
                {
                    continue;
                }
                // Nếu là đại lý thì kiểm tra xem có trong quyền tối đa của đại lý
                else if (UserTypes.TRADING_PROVIDER_TYPES.Contains(userType) && !_dbContext.TradingProviderPermissions.Any(p => p.PermissionKey == permissionKey
                        && p.TradingProviderId == CommonUtils.GetCurrentTradingProviderId(_httpContextAccessor)))
                {
                    continue;
                }

                // Nếu đã tồn tại rồi thì bỏ qua
                if (rolePermissionQuery.Any(p => p.PermissionKey == permissionKey))
                {
                    continue;
                }
                else
                {
                    _rolePermissionEFRepository.Entity.Add(new RolePermission
                    {
                        Id = (int)_rolePermissionEFRepository.NextKey(),
                        RoleId = roleId,
                        PermissionKey = permissionKey,
                        PermissionType = permissionValue.PermissionType,
                        CreatedBy = username,
                        CreatedDate = DateTime.Now,
                        Deleted = YesNo.NO
                    });
                }
            }
            _dbContext.SaveChanges();
            transaction.Commit();
        }
        #endregion

        #region Cấu hình Role cho đối tác
        public PagingResult<RoleDto> FindAllPartner(FilterRoleDto input)
        {
            var result = new PagingResult<RoleDto>();
            var partnerId = CommonUtils.GetCurrentPartnerId(_httpContextAccessor);
            var find = _roleEFRepository.FindAll(input, partnerId);
            result.Items = _mapper.Map<IEnumerable<RoleDto>>(find.Items);
            foreach (var item in result.Items)
            {
                var totalUse = _userRoleEFRepository.TotalUse(item.Id);
                if (item.RoleType == RoleTypes.Default)
                {
                    totalUse = _dbContext.UserRoles.Where(r => r.Deleted == YesNo.NO && r.RoleId ==  item.Id && _dbContext.Users.Any(u => u.UserId == r.UserId && u.IsDeleted == YesNo.NO
                                && u.PartnerId == partnerId)).Count();
                }
                item.TotalUse = totalUse;
            }
            result.TotalItems = find.TotalItems;
            return result;
        }

        #endregion

        #region Tạo role cho ĐLSC
        /// <summary>
        /// Lấy danh sách được cấu hình web cho đại lý sơ cấp
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public PagingResult<RoleDto> FindAllRoleInWebTrading(FilterRoleDto input)
        {
            var result = new PagingResult<RoleDto>();
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContextAccessor);
            var find = _roleEFRepository.FindAll(input, null, tradingProviderId);
            result.Items = _mapper.Map<IEnumerable<RoleDto>>(find.Items);
            foreach (var item in result.Items)
            {
                var totalUse = _userRoleEFRepository.TotalUse(item.Id);
                if (item.RoleType == RoleTypes.Default)
                {
                    totalUse = _dbContext.UserRoles.Where(r => r.Deleted == YesNo.NO && r.RoleId == item.Id && _dbContext.Users.Any(u => u.UserId == r.UserId && u.IsDeleted == YesNo.NO
                                && u.TradingProviderId == tradingProviderId)).Count();
                }
                item.TotalUse = totalUse;
            }
            result.TotalItems = find.TotalItems;
            return result;
        }
        #endregion
    }
}
