using AutoMapper;
using EPIC.DataAccess.Base;
using EPIC.DataAccess.Models;
using EPIC.Entities;
using EPIC.Entities.Dto.User;
using EPIC.GarnerRepositories;
using EPIC.Utils.ConstantVariables.Identity;
using EPIC.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using EPIC.GarnerEntities.Dto.GarnerApprove;
using EPIC.Utils.ConstantVariables.Garner;
using EPIC.IdentityRepositories;
using EPIC.GarnerDomain.Interfaces;
using System.Text.Json;
using DocumentFormat.OpenXml.Office2010.Excel;
using EPIC.CoreRepositories;
using EPIC.Entities.Dto.BusinessCustomer;

namespace EPIC.GarnerDomain.Implements
{
    public class GarnerApproveServices : IGarnerApproveServices
    {
        private readonly EpicSchemaDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<GarnerApproveServices> _logger;
        private readonly string _connectionString;
        private readonly IHttpContextAccessor _httpContext;
        private readonly GarnerProductEFRepository _garnerProductEFRepository;
        private readonly DefErrorEFRepository _defErrorEFRepository;
        private readonly GarnerProductTradingProviderEFRepository _garnerProductTradingProviderEFRepository;
        private readonly GarnerApproveEFRepository _garnerApproveEFRepository;
        private readonly BusinessCustomerEFRepository _businessCustomerEFRepository;
        private readonly UserRepository _userRepository;

        public GarnerApproveServices(EpicSchemaDbContext dbContext,
            DatabaseOptions databaseOptions,
            IMapper mapper,
            ILogger<GarnerApproveServices> logger,
            IHttpContextAccessor httpContextAccessor)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
            _connectionString = databaseOptions.ConnectionString;
            _httpContext = httpContextAccessor;
            _garnerProductEFRepository = new GarnerProductEFRepository(dbContext, logger);
            _defErrorEFRepository = new DefErrorEFRepository(dbContext);
            _garnerProductTradingProviderEFRepository = new GarnerProductTradingProviderEFRepository(dbContext, logger);
            _garnerApproveEFRepository = new GarnerApproveEFRepository(dbContext, logger);
            _businessCustomerEFRepository = new BusinessCustomerEFRepository(dbContext, logger);
            _userRepository = new UserRepository(_connectionString, logger);
        }

        public PagingResult<ViewGarnerApproveDto> FindAll(FilterGarnerApproveDto input)
        {
            _logger.LogInformation($"{System.Reflection.MethodBase.GetCurrentMethod().Name}: input = {JsonSerializer.Serialize(input)}");
            int? tradingProviderId = null;
            int? partnerId = null;
            string userType = CommonUtils.GetCurrentUserType(_httpContext);
            switch (input.DataType)
            {
                case GarnerApproveDataTypes.GAN_GARNER_PRODUCT:
                    if (userType != UserTypes.EPIC && userType != UserTypes.ROOT_EPIC)
                    {
                        partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
                    }
                    break;
                case GarnerApproveDataTypes.GAN_DISTRIBUTION:
                    if (userType != UserTypes.EPIC && userType != UserTypes.ROOT_EPIC)
                    {
                        tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
                    }
                    break;
                default:
                    throw new FaultException(new FaultReason($"Loại DataType không hợp lệ: {input.DataType}"), new FaultCode(((int)ErrorCode.BadRequest).ToString()), "");
            }

            var query = _garnerApproveEFRepository.FindAll(input, partnerId, tradingProviderId);

            var result = new PagingResult<ViewGarnerApproveDto>
            {
                TotalItems = query.TotalItems,
            };

            if (result.TotalItems > 0)
            {
                result.Items = _mapper.Map<List<ViewGarnerApproveDto>>(query.Items);
                if (result.Items != null)
                {
                    foreach (var approve in result.Items)
                    {
                        if (approve.UserRequestId != null)
                        {
                            var user = _userRepository.FindById(approve.UserRequestId ?? 0);
                            if (user != null)
                            {
                                approve.UserRequest = _mapper.Map<UserDto>(user);
                            }
                        }

                        if (approve.UserApproveId != null)
                        {
                            var user = _userRepository.FindById(approve.UserApproveId ?? 0);
                            if (user != null)
                            {
                                approve.UserApprove = _mapper.Map<UserDto>(user);
                            }
                        }
                        if (input.DataType == GarnerApproveDataTypes.GAN_GARNER_PRODUCT)
                        {
                            var productFind = _garnerProductEFRepository.FindById(approve.ReferId ?? 0);
                            if (productFind != null)
                            {
                                approve.CpsIssuer = _mapper.Map<BusinessCustomerDto>(_businessCustomerEFRepository.FindById(productFind.CpsIssuerId ?? 0));
                            }
                        }
                    }
                }
            }
            return result;
        }
    }
}
