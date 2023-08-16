using EPIC.CompanySharesDomain.Interfaces;
using EPIC.CompanySharesEntities.DataEntities;
using EPIC.CompanySharesEntities.Dto.OrderPayment;
using EPIC.CompanySharesRepositories;
using EPIC.DataAccess.Models;
using EPIC.Entities;
using EPIC.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.CompanySharesDomain.Implements
{
    public class CpsOrderPaymentServices: ICpsOrderPaymentServices
    {
        private readonly ILogger<CpsOrderPaymentServices> _logger;
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;
        private readonly IHttpContextAccessor _httpContext;
        private readonly CpsOrderPaymentRepository _orderPaymentRepository;
        public CpsOrderPaymentServices(
            ILogger<CpsOrderPaymentServices> logger,
            IConfiguration configuration,
            DatabaseOptions databaseOptions,
            IHttpContextAccessor httpContext)
        {
            _logger = logger;
            _configuration = configuration;
            _connectionString = databaseOptions.ConnectionString;
            _httpContext = httpContext;
            _orderPaymentRepository = new CpsOrderPaymentRepository(_connectionString, _logger);
        }

        public CpsOrderPayment AddPayment(CreateOrderPaymentDto input)
        {
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);

            return _orderPaymentRepository.Add(new CpsOrderPayment
            {
                TradingProviderId = tradingProviderId,
                OrderId = input.OrderId,
                TranDate = input.TranDate,
                TranType = input.TranType,
                TranClassify = input.TranClassify,
                PaymentType = input.PaymentType,
                PaymentAmnount = input.PaymentAmnount,
                Description = input.Description,
                CreatedBy = username
            });
        }

        public int ApprovePayment(int orderPaymentId, int status)
        {
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            return _orderPaymentRepository.ApprovePayment(orderPaymentId, status, tradingProviderId, username);
        }

        public int DeleteOrderPayment(int id)
        {
            return _orderPaymentRepository.Delete(id);
        }

        public PagingResult<CpsOrderPayment> FindAll(int orderId, int pageSize, int pageNumber, string keyword, int? status)
        {
            return _orderPaymentRepository.FindAll(orderId, pageSize, pageNumber, keyword, status);
        }
        public OrderPaymentDto FindPaymentById(int id)
        {
            return _orderPaymentRepository.FindById(id);
        }
    }
}
