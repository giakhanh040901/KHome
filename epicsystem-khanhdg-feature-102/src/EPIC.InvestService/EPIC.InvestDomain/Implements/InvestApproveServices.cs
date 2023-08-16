using AutoMapper;
using EPIC.BondRepositories;
using EPIC.CoreRepositories;
using EPIC.CoreSharedEntities.Dto.Investor;
using EPIC.DataAccess.Base;
using EPIC.DataAccess.Models;
using EPIC.Entities;
using EPIC.Entities.Dto.BusinessCustomer;
using EPIC.Entities.Dto.User;
using EPIC.IdentityRepositories;
using EPIC.InvestDomain.Interfaces;
using EPIC.InvestEntities.DataEntities;
using EPIC.InvestEntities.Dto.Distribution;
using EPIC.InvestEntities.Dto.InvestApprove;
using EPIC.InvestRepositories;
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

namespace EPIC.InvestDomain.Implements
{
    public class InvestApproveServices : IInvestApproveServices
    {
        private readonly EpicSchemaDbContext _dbContext;
        private readonly ILogger<InvestApproveServices> _logger;
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;
        //Repo thường
        private readonly InvestApproveRepository _approveRepository;
        private readonly InvRenewalsRequestRepository _invRenewalsRequestRepository;
        private readonly InvestPolicyRepository _investPolicyRepository;
        private readonly InvestOrderRepository _investOrderRepository;
        private readonly InvestorRepository _investorRepository;
        private readonly UserRepository _userRepository;
        private readonly InvestorIdentificationRepository _investorIdentificationRepository;
        private readonly BusinessCustomerRepository _businessCustomerRepository;
        private readonly InvestPolicyRepository _policyRepository;
        private readonly ProjectRepository _projectRepository;
        private readonly DistributionRepository _distributionRepository;

        //EF REPO
        private readonly CifCodeEFRepository _cifCodeEFRepository;

        private readonly IHttpContextAccessor _httpContext;
        private readonly IMapper _mapper;

        public InvestApproveServices(EpicSchemaDbContext dbContext,
            ILogger<InvestApproveServices> logger,
            IConfiguration configuration,
            DatabaseOptions databaseOptions,
            IHttpContextAccessor httpContext,
            IMapper mapper
        )
        {
            _dbContext = dbContext;
            _logger = logger;
            _configuration = configuration;
            _connectionString = databaseOptions.ConnectionString;
            //REPO PROC
            _approveRepository = new InvestApproveRepository(_connectionString, _logger);
            _invRenewalsRequestRepository = new InvRenewalsRequestRepository(_connectionString, _logger);
            _investOrderRepository = new InvestOrderRepository(_connectionString, _logger);
            _investPolicyRepository = new InvestPolicyRepository(_connectionString, _logger);
            _userRepository = new UserRepository(_connectionString, _logger);
            _userRepository = new UserRepository(_connectionString, _logger);
            _investorRepository = new InvestorRepository(_connectionString, _logger);
            _investorIdentificationRepository = new InvestorIdentificationRepository(_connectionString, _logger);
            _businessCustomerRepository = new BusinessCustomerRepository(_connectionString, _logger);
            _policyRepository = new InvestPolicyRepository(_connectionString, logger);
            _projectRepository = new ProjectRepository(_connectionString, logger);
            _distributionRepository = new DistributionRepository(_connectionString, logger);
            //EF REPO
            _cifCodeEFRepository = new CifCodeEFRepository(_dbContext, _logger);
            _httpContext = httpContext;
            _mapper = mapper;
        }

        public PagingResult<ViewInvestApproveDto> Find(InvestApproveGetDto dto)
        {
            int? tradingProviderId = null;
            int? partnerId = null;
            string userType = CommonUtils.GetCurrentUserType(_httpContext);
            switch (dto.DataType)
            {
                case InvestApproveDataTypes.EP_INV_PROJECT:
                    if (userType != UserTypes.EPIC && userType != UserTypes.ROOT_EPIC)
                    {
                        partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
                    }    
                    break;
                case InvestApproveDataTypes.EP_INV_DISTRIBUTION:
                case InvestApproveDataTypes.EP_INV_RENEWALS_REQUEST:
                    if (userType != UserTypes.EPIC && userType != UserTypes.ROOT_EPIC)
                    {
                        tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
                    }
                    break;
                default:
                    throw new FaultException(new FaultReason($"Loại DataType không hợp lệ: {dto.DataType}"), new FaultCode(((int)ErrorCode.BadRequest).ToString()), "");
            }

            var query = _approveRepository.GetAll(dto, tradingProviderId, partnerId);

            var result = new PagingResult<ViewInvestApproveDto>
            {
                TotalItems = query.TotalItems,
            };

            if (result.TotalItems > 0)
            {
                result.Items = _mapper.Map<List<ViewInvestApproveDto>>(query.Items);
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

                        // Lấy tên sản phẩm đầu tư
                        if (approve.DataType == InvestApproveDataTypes.EP_INV_PROJECT)
                        {
                            var project = _projectRepository.FindById(approve.ReferId ?? 0);
                            if (project != null)
                            {
                                approve.InvestmentProduct = project.InvName;
                            }
                        }
                        else if (approve.DataType == InvestApproveDataTypes.EP_INV_DISTRIBUTION)
                        {
                            var distribution = _distributionRepository.FindById(approve.ReferId ?? 0);
                            if (distribution != null)
                            {
                                var project = _projectRepository.FindById(distribution.ProjectId);
                                if (project != null)
                                {
                                    approve.InvestmentProduct = project.InvName;
                                }
                            }
                        }
                        else if (approve.DataType == InvestApproveDataTypes.EP_INV_RENEWALS_REQUEST)
                        {
                            var renewalsRequest = _invRenewalsRequestRepository.GetById(approve.ReferId ?? 0);
                            if (renewalsRequest != null)
                            {
                                var order = _investOrderRepository.FindById(renewalsRequest.OrderId);
                                if (order != null)
                                {
                                    var project = _projectRepository.FindById(order.ProjectId);
                                    if (project != null)
                                    {
                                        approve.InvestmentProduct = project.InvName;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return result;
        }
    }
}
