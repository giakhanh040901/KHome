using AutoMapper;
using EPIC.BondRepositories;
using EPIC.CoreDomain.Interfaces;
using EPIC.CoreRepositories;
using EPIC.DataAccess.Models;
using EPIC.Entities;
using EPIC.Entities.DataEntities;
using EPIC.Entities.Dto.CoreApprove;
using EPIC.Entities.Dto.User;
using EPIC.IdentityRepositories;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.CoreDomain.Implements
{
    public class ApproveServices : IApproveServices
    {
        private readonly ILogger<ApproveServices> _logger;
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;
        private readonly ApproveRepository _approveRepository;
        private readonly UserRepository _userRepository;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IMapper _mapper;

        public ApproveServices(
            ILogger<ApproveServices> logger,
            IConfiguration configuration,
            DatabaseOptions databaseOptions,
            IHttpContextAccessor httpContext,
            IMapper mapper
        )
        {
            _logger = logger;
            _configuration = configuration;
            _connectionString = databaseOptions.ConnectionString;
            _approveRepository = new ApproveRepository(_connectionString, _logger);
            _userRepository = new UserRepository(_connectionString, _logger);
            _httpContext = httpContext;
            _mapper = mapper;
        }

        public PagingResult<ViewCoreApproveDto> Find(GetApproveListDto dto)
        {
            int? tradingProviderId = null;
            int? partnerId = null;
            string userType = CommonUtils.GetCurrentUserType(_httpContext);

            if (new string[] {UserTypes.EPIC, UserTypes.ROOT_EPIC}.Contains(userType) && dto.TradingProviderId != null)
            {
                tradingProviderId = dto.TradingProviderId;
            }

            switch (dto.DataType)
            {
                case CoreApproveDataTypes.EP_PRODUCT_BOND_INFO:
                case CoreApproveDataTypes.EP_PRODUCT_BOND_PRIMARY:
                case CoreApproveDataTypes.DISTRIBUTION_CONTRACT_FILE:
                    if (userType == UserTypes.PARTNER || userType == UserTypes.ROOT_PARTNER)
                    {
                        partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
                    }
                    break;
                case CoreApproveDataTypes.EP_INVESTOR:
                case CoreApproveDataTypes.EP_BOND_SECONDARY:
                case CoreApproveDataTypes.EP_CORE_SALE:
                case CoreApproveDataTypes.EP_CORE_BUSINESS_CUSTOMER_BANK:
                case CoreApproveDataTypes.EP_RENEWALS_REQUEST:
                case CoreApproveDataType.INVESTOR_PROFESSIONAL:
                case CoreApproveDataType.INVESTOR_PHONE:
                case CoreApproveDataType.INVESTOR_EMAIL:
                    if (userType == UserTypes.TRADING_PROVIDER || userType == UserTypes.ROOT_TRADING_PROVIDER)
                    {
                        tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
                    }
                    break;
                case CoreApproveDataTypes.EP_CORE_BUSINESS_CUSTOMER:
                    if (userType == UserTypes.TRADING_PROVIDER || userType == UserTypes.ROOT_TRADING_PROVIDER)
                    {
                        tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
                    }
                    else if (userType == UserTypes.PARTNER || userType == UserTypes.ROOT_PARTNER)
                    {
                        partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
                    }
                    break;
                default:
                    throw new FaultException(new FaultReason($"Loại DataType không hợp lệ: {dto.DataType}"), new FaultCode(((int)ErrorCode.BadRequest).ToString()), "");
            }

            if (dto.DataType == CoreApproveDataTypes.EP_CORE_SALE)
            {
                tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            }
            var query = _approveRepository.GetAll(dto, tradingProviderId, partnerId, userType);

            var result = new PagingResult<ViewCoreApproveDto>
            {
                TotalItems = query.TotalItems,
            };
            result.Items = _mapper.Map<List<ViewCoreApproveDto>>(query.Items);
            if (result.TotalItems > 0)
            {
                if (result.Items != null)
                {
                    foreach (var approve in result.Items)
                    {
                        var appro = _userRepository.FindById(approve.UserApproveId ?? 0);
                        var userRequest = _userRepository.FindById(approve.UserRequestId ?? 0);
                        if (approve.UserRequestId != null)
                        {
                            approve.UserRequest = _mapper.Map<UserDto>(userRequest);
                        }
                        if (approve.UserApproveId != null)
                        {
                            approve.UserApprove = _mapper.Map<UserDto>(appro);
                        }
                    }
                }
            }
            return result;
        }
    }
}
