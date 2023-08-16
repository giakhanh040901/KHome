using AutoMapper;
using EPIC.BondDomain.Interfaces;
using EPIC.BondRepositories;
using EPIC.CoreRepositories;
using EPIC.DataAccess.Base;
using EPIC.Entities;
using EPIC.FileEntities.Settings;
using EPIC.IdentityRepositories;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.BondDomain.Implements
{
    public class BondJobsService : IBondJobsService
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;
        private readonly IHttpContextAccessor _httpContext;
        private readonly BondOrderRepository _orderRepository;
        private readonly BondOrderPaymentRepository _orderPaymentRepository;
        private readonly BusinessCustomerRepository _businessCustomerRepository;
        private readonly BondInfoRepository _productBondInfoRepository;
        private readonly BondPrimaryRepository _productBondPrimaryRepository;
        private readonly BondSecondaryRepository _productBondSecondaryRepository;
        private readonly CifCodeRepository _cifCodeRepository;
        private readonly InvestorRepository _investorRepository;
        private readonly BondSecondPriceRepository _productBondSecondPriceRepository;
        private readonly BondCalendarRepository _calendarRepository;
        private readonly ManagerInvestorRepository _managerInvestorRepository;
        private readonly BankRepository _bankRepository;
        private readonly BondContractTemplateRepository _contractTemplateRepository;
        private readonly BondSecondaryContractRepository _bondSecondaryContractRepository;
        private readonly InvestorIdentificationRepository _investorIdentificationRepository;
        private readonly IMapper _mapper;
        private readonly IBondSharedService _bondSharedService;
        private readonly IOptions<FileConfig> _fileConfig;
        private readonly UserRepository _userRepository;
        private readonly SysVarRepository _sysVarRepository;

        public BondJobsService(
            ILogger<BondJobsService> logger,
            IConfiguration configuration,
            DatabaseOptions databaseOptions,
            IHttpContextAccessor httpContext,
            IMapper mapper,
            IBondSharedService bondSharedService,
            IBondContractDataService contractDataServices,
            IBondContractTemplateService contractTemplateServices,
            IOptions<FileConfig> fileConfig)
        {
            _logger = logger;
            _configuration = configuration;
            _connectionString = databaseOptions.ConnectionString;
            _httpContext = httpContext;
            _orderRepository = new BondOrderRepository(_connectionString, _logger);
            _orderPaymentRepository = new BondOrderPaymentRepository(_connectionString, _logger);
            _businessCustomerRepository = new BusinessCustomerRepository(_connectionString, _logger);
            _productBondInfoRepository = new BondInfoRepository(_connectionString, _logger);
            _cifCodeRepository = new CifCodeRepository(_connectionString, _logger);
            _investorRepository = new InvestorRepository(_connectionString, _logger);
            _productBondPrimaryRepository = new BondPrimaryRepository(_connectionString, _logger);
            _productBondSecondaryRepository = new BondSecondaryRepository(_connectionString, _logger);
            _productBondSecondPriceRepository = new BondSecondPriceRepository(_connectionString, _logger);
            _calendarRepository = new BondCalendarRepository(_connectionString, _logger);
            _managerInvestorRepository = new ManagerInvestorRepository(_connectionString, _logger, _httpContext);
            _bankRepository = new BankRepository(_connectionString, _logger);
            _contractTemplateRepository = new BondContractTemplateRepository(_connectionString, _logger);
            _bondSecondaryContractRepository = new BondSecondaryContractRepository(_connectionString, _logger);
            _investorIdentificationRepository = new InvestorIdentificationRepository(_connectionString, _logger);
            _mapper = mapper;
            _bondSharedService = bondSharedService;
            _fileConfig = fileConfig;
            _userRepository = new UserRepository(_connectionString, _logger);
            _sysVarRepository = new SysVarRepository(_connectionString, _logger);
        }

        public void ScanOrder(int tradingProviderId)
        {
            throw new NotImplementedException();
        }
    }
}
