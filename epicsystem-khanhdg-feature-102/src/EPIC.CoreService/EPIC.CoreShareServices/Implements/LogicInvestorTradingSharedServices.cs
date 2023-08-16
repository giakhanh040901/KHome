using AutoMapper;
using EPIC.CoreRepositories;
using EPIC.CoreSharedServices.Interfaces;
using EPIC.DataAccess.Base;
using EPIC.Entities;
using EPIC.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;

namespace EPIC.CoreSharedServices.Implements
{
    /// <summary>
    /// Logic bảng hàng
    /// </summary>
    public class LogicInvestorTradingSharedServices : ILogicInvestorTradingSharedServices
    {
        private readonly ILogger<LogicInvestorTradingSharedServices> _logger;
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;
        private readonly SaleRepository _saleRepository;
        private readonly IHttpContextAccessor _httpContext;
        private readonly EpicSchemaDbContext _dbContext;
        private readonly DepartmentRepository _departmentRepository;
        private readonly ManagerInvestorRepository _managerInvestorRepository;
        private readonly InvestorIdentificationRepository _investorIdentificationRepository;
        private readonly InvestorRepository _investorRepository;
        private readonly BusinessCustomerRepository _businessCustomerRepository;
        private readonly InvestorSaleRepository _investorSaleRepository;
        private readonly SaleEFRepository _saleEFRepository;
        private readonly IMapper _mapper;

        public LogicInvestorTradingSharedServices(
            ILogger<LogicInvestorTradingSharedServices> logger,
            IConfiguration configuration,
            DatabaseOptions databaseOptions,
            IHttpContextAccessor httpContext,
            EpicSchemaDbContext dbContext,
            IMapper mapper)
        {
            _logger = logger;
            _configuration = configuration;
            _connectionString = databaseOptions.ConnectionString;
            _httpContext = httpContext;
            _dbContext = dbContext;
            _saleRepository = new SaleRepository(_connectionString, _logger);
            _departmentRepository = new DepartmentRepository(_connectionString, _logger);
            _managerInvestorRepository = new ManagerInvestorRepository(_connectionString, _logger, _httpContext);
            _investorIdentificationRepository = new InvestorIdentificationRepository(_connectionString, _logger);
            _investorRepository = new InvestorRepository(_connectionString, _logger);
            _businessCustomerRepository = new BusinessCustomerRepository(_connectionString, _logger);
            _investorSaleRepository = new InvestorSaleRepository(_connectionString, _logger);
            _saleEFRepository = new SaleEFRepository(dbContext, logger);
            _mapper = mapper;
        }

        /// <summary>
        /// Lấy danh sách đại lý để lọc bảng hàng 
        /// </summary>
        public List<int> FindListTradingProviderForApp(int? investorId, bool isSaleView = false)
        {
            int? saleId = null;
            List<int> tradingProviderIds = new();

            if (!isSaleView) //không phải view của sale thì xem như khách cá nhân bt
            {
                var listTradingBySaleBusiness = _investorSaleRepository.BusinessSaleListTrading(investorId ?? 0)
                    .Select(o => o.TradingProviderId);
                tradingProviderIds.AddRange(listTradingBySaleBusiness);
            }
            else
            {
                //Kiểm tra xem investor có phải là saler hay không
                var checkSale = _saleRepository.AppCheckSaler(investorId ?? 0);
                if (checkSale.Status == CheckSaler.IsSaler)
                {
                    saleId = CommonUtils.GetCurrentSaleId(_httpContext);
                    //Lấy danh sách đại lý mà sale đang thuộc
                    var listTradingInSale = _saleRepository.AppListTradingProviderBySale(saleId ?? 0)
                        .Select(o => o.TradingProviderId);
                    tradingProviderIds.AddRange(listTradingInSale);
                    foreach (var item in listTradingInSale)
                    {
                        tradingProviderIds.AddRange(_saleEFRepository.FindTradingProviderBanHo(item));
                    }
                }
            }
            return tradingProviderIds;
        }

        /// <summary>
        /// Lấy danh sách đại lý để investor xem danh sách
        /// </summary>
        public List<int> FindListInvestorTradingProviderForApp(int? investorId)
        {
            List<int> tradingProviderIds = new();
            var listTradingBySaleBusiness = _investorSaleRepository.BusinessSaleListTrading(investorId ?? 0)
                    .Select(o => o.TradingProviderId);
            tradingProviderIds.AddRange(listTradingBySaleBusiness);
            return tradingProviderIds;
        }

        /// <summary>
        /// Lấy danh sách đại lý để sale xem danh sách
        /// </summary>
        public List<int> FindListSaleTradingProviderForApp(int? investorId)
        {
            int? saleId = null;
            List<int> tradingProviderIds = new();

            //Kiểm tra xem investor có phải là saler hay không
            var checkSale = _saleRepository.AppCheckSaler(investorId ?? 0);
            if (checkSale.Status == CheckSaler.IsSaler)
            {
                saleId = CommonUtils.GetCurrentSaleId(_httpContext);
                //Lấy danh sách đại lý mà sale đang thuộc
                var listTradingInSale = _saleRepository.AppListTradingProviderBySale(saleId ?? 0)
                    .Select(o => o.TradingProviderId);
                tradingProviderIds.AddRange(listTradingInSale);
            }
            return tradingProviderIds;
        }
    }
}
