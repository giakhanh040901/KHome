using AutoMapper;
using EPIC.DataAccess.Base;
using EPIC.DataAccess.Models;
using EPIC.Entities.Dto.User;
using EPIC.Entities;
using EPIC.GarnerEntities.Dto.GarnerApprove;
using EPIC.IdentityRepositories;
using EPIC.Utils.ConstantVariables.Garner;
using EPIC.Utils.ConstantVariables.Identity;
using EPIC.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using EPIC.RealEstateDomain.Interfaces;
using EPIC.RealEstateRepositories;
using System.Text.Json;
using EPIC.Utils.ConstantVariables.RealEstate;
using EPIC.RealEstateEntities.Dto.RstApprove;
using DocumentFormat.OpenXml.Office2010.Excel;
using EPIC.CoreRepositories;
using EPIC.RealEstateEntities.Dto.RstDistribution;
using EPIC.RealEstateEntities.Dto.RstProject;
using EPIC.RealEstateEntities.Dto.RstOpenSell;

namespace EPIC.RealEstateDomain.Implements
{
    public class RstApproveServices : IRstApproveServices
    {
        private readonly EpicSchemaDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<RstApproveServices> _logger;
        private readonly string _connectionString;
        private readonly IHttpContextAccessor _httpContext;
        private readonly RstProjectEFRepository _projectEFRepository;
        private readonly DefErrorEFRepository _defErrorEFRepository;
        private readonly RstApproveEFRepository _rstApproveEFRepository;
        private readonly UserRepository _userRepository;
        private readonly TradingProviderEFRepository _tradingProviderEFRepository;
        private readonly BusinessCustomerEFRepository _businessCustomerEFRepository;
        private readonly RstOwnerEFRepository _rstOwnerEFRepository;
        private readonly RstDistributionEFRepository _rstDistributionEFRepository;
        private readonly RstOpenSellDetailEFRepository _rstOpenSellDetailEFRepository;
        private readonly RstOpenSellEFRepository _rstOpenSellEFRepository;

        public RstApproveServices(EpicSchemaDbContext dbContext,
            DatabaseOptions databaseOptions,
            IMapper mapper,
            ILogger<RstApproveServices> logger,
            IHttpContextAccessor httpContextAccessor)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
            _connectionString = databaseOptions.ConnectionString;
            _httpContext = httpContextAccessor;
            _projectEFRepository = new RstProjectEFRepository(dbContext, logger);
            _defErrorEFRepository = new DefErrorEFRepository(dbContext);
            _rstApproveEFRepository = new RstApproveEFRepository(dbContext, logger);
            _userRepository = new UserRepository(_connectionString, logger);
            _tradingProviderEFRepository = new TradingProviderEFRepository(dbContext, logger);
            _businessCustomerEFRepository = new BusinessCustomerEFRepository(dbContext, logger);
            _rstOwnerEFRepository = new RstOwnerEFRepository(dbContext, logger);
            _rstDistributionEFRepository = new RstDistributionEFRepository(dbContext, logger);
            _rstOpenSellDetailEFRepository = new RstOpenSellDetailEFRepository(dbContext, logger);
            _rstOpenSellEFRepository = new RstOpenSellEFRepository(dbContext, logger);
        }

        public PagingResult<RstDataApproveDto> FindAll(FilterRstApproveDto input)
        {
            _logger.LogInformation($"{nameof(FindAll)}: input = {JsonSerializer.Serialize(input)}");
            int? tradingProviderId = null;
            int? partnerId = null;
            string userType = CommonUtils.GetCurrentUserType(_httpContext);
            switch (input.DataType)
            {
                case RstApproveDataTypes.RST_PROJECT:
                case RstApproveDataTypes.RST_DISTRIBUTION:
                    if (userType != UserTypes.EPIC && userType != UserTypes.ROOT_EPIC)
                    {
                        partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
                    }
                    break;
                case RstApproveDataTypes.RST_OPEN_SELL:
                    if (userType != UserTypes.EPIC && userType != UserTypes.ROOT_EPIC)
                    {
                        tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
                    }
                    break;
                default:
                    throw new FaultException(new FaultReason($"Loại DataType không hợp lệ: {input.DataType}"), new FaultCode(((int)ErrorCode.BadRequest).ToString()), "");
            }

            var query = _rstApproveEFRepository.FindAll(input, partnerId, tradingProviderId);

            var result = new PagingResult<RstDataApproveDto>
            {
                TotalItems = query.TotalItems,
            };

            if (result.TotalItems > 0)
            {
                result.Items = _mapper.Map<List<RstDataApproveDto>>(query.Items);
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
                        if (approve.DataType == RstApproveDataTypes.RST_PROJECT)
                        {
                            approve.Project = new();
                            // Thông tin dự án
                            var projectQuery = _projectEFRepository.FindById(approve.ReferId ?? 0);
                            approve.Project = _mapper.Map<RstProjectDto>(projectQuery);
                        }

                        if (approve.DataType == RstApproveDataTypes.RST_DISTRIBUTION)
                        {
                            approve.Distribution = new();
                            var distributionQuery = _rstDistributionEFRepository.FindById(approve.ReferId ?? 0);
                            if (distributionQuery != null)
                            {
                                // Thông tin dự án
                                var projectQuery = _projectEFRepository.FindById(distributionQuery.ProjectId);
                                // Thông tin của đại lý
                                var tradingProviderQuery = _tradingProviderEFRepository.GetById(distributionQuery.TradingProviderId);
                                approve.Distribution = _mapper.Map<RstDistributionDto>(distributionQuery);
                                approve.Distribution.Project = _mapper.Map<RstProjectDto>(projectQuery);
                                approve.Distribution.TradingProvider = tradingProviderQuery;
                            }
                        }
                        if (approve.DataType == RstApproveDataTypes.RST_OPEN_SELL)
                        {
                            approve.OpenSell = new();
                            var openSellQuery = _rstOpenSellEFRepository.FindById(approve.ReferId ?? 0);
                            if (openSellQuery != null)
                            {
                                // Thông tin dự án
                                var projectQuery = _projectEFRepository.FindById(openSellQuery.ProjectId);
                                approve.OpenSell = _mapper.Map<RstOpenSellDto>(openSellQuery);
                                approve.OpenSell.Project = _mapper.Map<RstProjectDto>(projectQuery);
                            }
                        }
                    }
                }
            }
            return result;
        }
    }
}
