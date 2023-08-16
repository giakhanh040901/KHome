using AutoMapper;
using EPIC.CoreRepositories;
using EPIC.CoreSharedEntities.Dto.Investor;
using EPIC.Entities;
using EPIC.InvestDomain.Interfaces;
using EPIC.InvestRepositories;
using EPIC.SharedDomain.Interfaces;
using EPIC.SharedEntities.Dto.InvestorOrder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;

namespace EPIC.SharedDomain.Implements
{
    public class InvestorSharedTpsServices : IInvestorSharedTpsServices
    {
        private readonly ILogger<InvestorSharedTpsServices> _logger;
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;
        private readonly IHttpContextAccessor _httpContext;
        private readonly InvestOrderRepository _orderRepository;
        private readonly ProjectRepository _projectRepository;
        private readonly CifCodeRepository _cifCodeRepository;
        private readonly InvestorRepository _investorRepository;
        private readonly ManagerInvestorRepository _managerInvestorRepository;
        private readonly BusinessCustomerRepository _businessCustomerRepository;
        private readonly DistributionRepository _distributionRepository;
        private readonly InvestPolicyRepository _policyRepository;
        private readonly InvestRepositories.CalendarRepository _calendarRepository;
        private readonly InvestorIdentificationRepository _investorIdentificationRepository;
        private readonly IMapper _mapper;
        private readonly IInvestOrderShareServices _investOrderShareServices;

        public InvestorSharedTpsServices(
            ILogger<InvestorSharedTpsServices> logger,
            IConfiguration configuration,
            DatabaseOptions databaseOptions,
            IHttpContextAccessor httpContext,
            IInvestOrderShareServices investOrderShareServices,
            IMapper mapper)
        {
            _logger = logger;
            _configuration = configuration;
            _connectionString = databaseOptions.ConnectionString;
            _httpContext = httpContext;
            _projectRepository = new ProjectRepository(_connectionString, _logger);
            _orderRepository = new InvestOrderRepository(_connectionString, _logger);
            _cifCodeRepository = new CifCodeRepository(_connectionString, _logger);
            _investorRepository = new InvestorRepository(_connectionString, _logger);
            _businessCustomerRepository = new BusinessCustomerRepository(_connectionString, _logger);
            _calendarRepository = new InvestRepositories.CalendarRepository(_connectionString, _logger);
            _policyRepository = new InvestPolicyRepository(_connectionString, _logger);
            _managerInvestorRepository = new ManagerInvestorRepository(_connectionString, _logger, _httpContext);
            _investorIdentificationRepository = new InvestorIdentificationRepository(_connectionString, _logger);
            _distributionRepository = new DistributionRepository(_connectionString, _logger);
            _mapper = mapper;
            _investOrderShareServices = investOrderShareServices;
        }

        /// <summary>
        /// Lấy thông tin sổ lệnh của nhà đầu tư theo tài khoản chứng khoán
        /// </summary>
        /// <param name="securityCompany"></param>
        /// <param name="stockTradingAccount"></param>
        /// <returns></returns>
        public ListOrderInvestorByStockTradingAccountDto ListOrderInvestorByStockTradingAccount(int securityCompany, string stockTradingAccount, DateTime? startDate, DateTime? endDate)
        {
            ListOrderInvestorByStockTradingAccountDto result = new();
            result.InvestOrder = _investOrderShareServices.GetListInvestOrderByInvestor(securityCompany, stockTradingAccount, startDate, endDate);
            return result;
        }

        public SCInvestorIdentificationDto InvestorIdenByStockTradingAccount(int securityCompany, string stockTradingAccount)
        {
            var result = new SCInvestorIdentificationDto();
            var investorStockFind = _managerInvestorRepository.GetInvestorByStockTradingAccount(securityCompany, stockTradingAccount);
            if (investorStockFind != null)
            {
                var investorIdenDefaultFind = _investorIdentificationRepository.GetByInvestorId(investorStockFind.InvestorId ?? 0);
                result = _mapper.Map<SCInvestorIdentificationDto>(investorIdenDefaultFind);
                var investorFind = _managerInvestorRepository.FindById(investorStockFind.InvestorId ?? 0, false);
                if (investorFind != null)
                {
                    result.Phone = investorFind.Phone;
                    result.Email = investorFind.Email;
                }

                var investorContractAddressFind = _investorRepository.GetContactAddressDefault(investorStockFind.InvestorId ?? 0);
                if (investorContractAddressFind != null)
                {
                    result.ContractAddress = investorContractAddressFind.ContactAddress;
                }
            }
            return result;
        }
    }
}
