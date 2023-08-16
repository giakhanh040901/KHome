using AutoMapper;
using EPIC.CoreRepositories;
using EPIC.DataAccess.Base;
using EPIC.DataAccess.Models;
using EPIC.Entities;
using EPIC.Entities.Dto.TradingProvider;
using EPIC.InvestDomain.Interfaces;
using EPIC.SharedDomain.Interfaces;
using EPIC.SharedEntities.Dto.OperationalInfo;
using EPIC.SharedRepositories;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Org.BouncyCastle.Asn1.Ocsp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.SharedDomain.Implements
{
    public class OperationalInfoServices : IOperationalInfoServices
    {
        private readonly ILogger<OperationalInfoServices> _logger;
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IMapper _mapper;
        private readonly TradingProviderRepository _tradingProviderRepository;
        private readonly SharedDataBCTRepository _sharedDataBCTRepository;

        public OperationalInfoServices(
            ILogger<OperationalInfoServices> logger,
            IConfiguration configuration,
            DatabaseOptions databaseOptions,
            IHttpContextAccessor httpContext,
            IMapper mapper)
        {
            _logger = logger;
            _configuration = configuration;
            _connectionString = databaseOptions.ConnectionString;
            _httpContext = httpContext;
            _mapper = mapper;
            _tradingProviderRepository = new TradingProviderRepository(_connectionString, _logger);
            _sharedDataBCTRepository = new SharedDataBCTRepository(_connectionString, _logger);
        }

        /// <summary>
        /// Lấy thông tin cho báo cáo của Bộ công thương
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public OperationalInfoDto GetInfoBCT(DateTime? startDate, DateTime? endDate)
        {
            string currentYear = DateTime.Now.Year.ToString();
            DateTime? beginYear = DateTime.Parse($"{currentYear}-01-01");
            if (startDate != null && startDate > beginYear)
            {
                beginYear = startDate;
            }
            // Thông tin chung
            int visitors = 0;
            int sellers = _sharedDataBCTRepository.GetSellers(startDate, endDate);
            int newSellers = _sharedDataBCTRepository.GetSellers(beginYear, endDate);
            int totalProducts = _sharedDataBCTRepository.GetTotalProducts(startDate, endDate);
            int newProducts = _sharedDataBCTRepository.GetTotalProducts(beginYear, endDate);
            // Các trường fix theo đại lý
            int transactions = _sharedDataBCTRepository.GetTransactions(beginYear, endDate);
            int successfulOrders = _sharedDataBCTRepository.GetSuccessfulOrders(beginYear, endDate);
            int failedOrders = _sharedDataBCTRepository.GetFailedOrders(beginYear, endDate);
            string transactionValue = _sharedDataBCTRepository.GetTotalTransactionValue(beginYear, endDate);
            long totalTransactionValue = 0;
            if (transactionValue != null)
            {
                totalTransactionValue = Convert.ToInt64(transactionValue);
            }

            var result = new OperationalInfoDto()
            {
                Visitors = visitors,
                Sellers = sellers,
                NewSellers = newSellers,
                TotalProducts = totalProducts,
                NewProducts = newProducts,
                Transactions = transactions,
                SuccessfulOrders = successfulOrders,
                FailedOrders = failedOrders,
                TotalTransactionValue = totalTransactionValue
            };
            return result;
        }

        /// <summary>
        /// Lấy danh sách để chọn đại lý
        /// </summary>
        /// <returns></returns>
        public PagingResult<TradingProviderDto> GetTradingList()
        {
            return _sharedDataBCTRepository.FindAllTrading();
        }

        /// <summary>
        /// Danh sách đại lý được tính trong báo cáo của Bộ công thương
        /// </summary>
        /// <returns></returns>
        public PagingResult<ReportTradingProviderDto> GetReportTradingList()
        {
            return _sharedDataBCTRepository.FindAllReportTrading();
        }

        /// <summary>
        /// Cập nhật danh sách các đại lý báo cáo
        /// </summary>
        /// <param name="input"></param>
        public void UpdateReportTradingList(int[] input)
        {
            var removeList = _sharedDataBCTRepository.FindAllReportTrading().Items
                                    .Where(t => !input.Contains(t.Id)).Select(t => t.Id).ToList();
            foreach (var id in removeList)
            {
                _sharedDataBCTRepository.RemoveReportTrading(id);
            }

            var reportTradingList = _sharedDataBCTRepository.FindAllReportTrading().Items.ToList();
            var reportIds = reportTradingList.Select(t => t.Id).ToList();
            reportIds.AddRange(removeList); // Nối với removeList để lọc ra list id cần thêm
            var addList = input.Except(reportIds).ToList();
            foreach (var tradingId in addList)
            {
                var data = _tradingProviderRepository.FindById(tradingId);
                _sharedDataBCTRepository.AddReportTrading(new ReportTradingProviderDto()
                {
                    Id = data.TradingProviderId,
                    BusinessCustomerId = data.BusinessCustomerId,
                    Status = data.Status,
                    Deleted = data.Deleted,
                    CreatedBy = data.CreatedBy,
                    CreatedDate = data.CreatedDate,
                    ModifiedBy = data.ModifiedBy,
                    ModifiedDate = data.ModifiedDate,
                });
            }
        }
    }
}
