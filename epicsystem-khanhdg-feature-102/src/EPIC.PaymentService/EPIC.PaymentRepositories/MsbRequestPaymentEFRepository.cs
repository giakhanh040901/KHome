using EPIC.DataAccess.Base;
using EPIC.DataAccess.Models;
using EPIC.PaymentEntities.DataEntities;
using EPIC.PaymentEntities.Dto.MsbRequestPayment;
using EPIC.Utils.ConstantVariables.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace EPIC.PaymentRepositories
{
    public class MsbRequestPaymentEFRepository : BaseEFRepository<MsbRequestPayment>
    {
        public MsbRequestPaymentEFRepository(EpicSchemaDbContext dbContext, ILogger logger) : base(dbContext, logger, $"{DbSchemas.EPIC}.{MsbRequestPayment.SEQ}")
        {
        }

        /// <summary>
        /// Thêm request Yêu cầu chi hộ MSB
        /// </summary>
        /// <param name="input"></param>
        /// <param name="username"></param>
        /// <param name="tradingProviderId"></param>
        public MsbRequestPayment Add(MsbRequestPayment input)
        {
            _logger.LogInformation($"{nameof(Add)}: input = {JsonSerializer.Serialize(input)}");

            input.CreatedDate = DateTime.Now;
            return _dbSet.Add(input).Entity;
        }

        /// <summary>
        /// Cập nhật request Yêu cầu chi hộ MSB
        /// </summary>
        /// <param name="input"></param>
        /// <param name="username"></param>
        /// <param name="tradingProviderId"></param>
        public MsbRequestPayment Udpate(MsbRequestPayment input, int? tradingProviderId = null)
        {
            _logger.LogInformation($"{nameof(Udpate)}: input = {JsonSerializer.Serialize(input)}, tradingProviderId = {tradingProviderId}");

            var request = _dbSet.FirstOrDefault(e => e.Id == input.Id && (tradingProviderId == null || e.TradingProdiverId == tradingProviderId));
            request.ProductType = input.ProductType;
            request.RequestType = input.RequestType;
            return request;
        }

        /// <summary>
        /// Tìm kiếm theo request Id 
        /// </summary>
        /// <param name="input"></param>
        /// <param name="tradingProviderId"></param>
        public MsbRequestPayment FindById(long requestId, int? tradingProviderId = null)
        {
            _logger.LogInformation($"{nameof(FindById)}: requestId = {requestId}, tradingProviderId = {tradingProviderId}");

            var request = _dbSet.FirstOrDefault(e => e.Id == requestId && (tradingProviderId == null || e.TradingProdiverId == tradingProviderId));
            return request;
        }

        /// <summary>
        /// Tìm kiếm tất cả request Id 
        /// </summary>
        /// <param name="tradingProviderId"></param>
        public PagingResult<MsbRequestPayment> FindAll(FilterMsbRequestPaymentDto input, int? tradingProviderId = null)
        {
            _logger.LogInformation($"{nameof(FindById)}: input = {JsonSerializer.Serialize(input)}, tradingProviderId = {tradingProviderId}");

            PagingResult<MsbRequestPayment> result = new();
            var requestPayment = _dbSet.OrderByDescending(p => p.Id)
                .Where(r => (tradingProviderId == null || r.TradingProdiverId == tradingProviderId));

            if (input.PageSize != -1)
            {
                requestPayment = requestPayment.Skip(input.Skip).Take(input.PageSize);
            }

            result.TotalItems = requestPayment.Count();
            result.Items = requestPayment.ToList();
            return result;
        }
    }
}
