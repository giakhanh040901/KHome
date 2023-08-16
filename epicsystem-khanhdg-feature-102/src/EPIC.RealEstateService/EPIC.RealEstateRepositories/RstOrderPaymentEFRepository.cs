using EPIC.DataAccess.Base;
using EPIC.DataAccess.Models;
using EPIC.Entities.DataEntities;
using EPIC.RealEstateEntities.DataEntities;
using EPIC.RealEstateEntities.Dto.RstOrderPayment;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Core;
using EPIC.Utils.ConstantVariables.Shared;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace EPIC.RealEstateRepositories
{
    public class RstOrderPaymentEFRepository : BaseEFRepository<RstOrderPayment>
    {
        public RstOrderPaymentEFRepository(EpicSchemaDbContext dbContext, ILogger logger) : base(dbContext, logger, $"{DbSchemas.EPIC_REAL_ESTATE}.{RstOrderPayment.SEQ}")
        {
        }

        /// <summary>
        /// Thêm mới thanh toán
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="username"></param>
        /// <param name="tradingProviderId"></param>
        /// <returns></returns>
        public RstOrderPayment Add(RstOrderPayment entity, string username, int tradingProviderId)
        {
            _logger.LogInformation($"{nameof(RstOrderPaymentEFRepository)}->{nameof(Add)}: input = {JsonSerializer.Serialize(entity)}, username: {username}, tradingProviderId: {tradingProviderId}");
            entity.Id = (int)NextKey();
            entity.TradingProviderId = tradingProviderId;
            entity.CreatedBy = username;
            entity.CreatedDate = DateTime.Now;
            return _dbSet.Add(entity).Entity;
        }
        
        /// <summary>
        /// Cập nhật thanh toán
        /// </summary>
        /// <param name="input"></param>
        /// <param name="username"></param>
        /// <param name="tradingProviderId"></param>
        /// <returns></returns>
        public RstOrderPayment Update(RstOrderPayment input, string username, int tradingProviderId)
        {
            _logger.LogInformation($"{nameof(RstOrderPaymentEFRepository)}->{nameof(Update)}: input = {JsonSerializer.Serialize(input)}, username: {username}");
            var orderPayment = _dbSet.FirstOrDefault(p => p.Id == input.Id && p.TradingProviderId == tradingProviderId && p.Deleted == YesNo.NO);
            if (orderPayment != null)
            {
                orderPayment.TranDate = input.TranDate;
                orderPayment.TranType = input.TranType;
                orderPayment.TranClassify = input.TranClassify;
                orderPayment.PaymentType = input.PaymentType;
                orderPayment.PaymentAmount = input.PaymentAmount;
                orderPayment.Description = input.Description;
                orderPayment.ModifiedBy = username;
                orderPayment.ModifiedDate = DateTime.Now;
            }
            return orderPayment;
        }

        /// <summary>
        /// Tìm thanh toán theo id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="tradingProviderId"></param>
        /// <returns></returns>
        public RstOrderPayment FindById(int id, int? tradingProviderId = null)
        {
            _logger.LogInformation($"{nameof(RstOrderPaymentEFRepository)}->{nameof(FindById)}: Id= {id}, tradingProviderId={tradingProviderId} ");

            var result = _dbSet.FirstOrDefault(d => d.Id == id && (tradingProviderId == null || d.TradingProviderId == tradingProviderId) && d.Deleted == YesNo.NO);
            return result;
        }

        /// <summary>
        /// Tìm danh sách thanh toán phân trang
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public PagingResult<RstOrderPayment> FindAll(FilterRstOrderPaymentDto input, int? tradingProviderId = null)
        {
            _logger.LogInformation($"{nameof(RstOrderPaymentEFRepository)}->{nameof(FindAll)}: input = {JsonSerializer.Serialize(input)}");

            PagingResult<RstOrderPayment> result = new();
            var orderPaymentQuery = _dbSet.Where(op => op.OrderId == input.OrderId && op.Deleted == YesNo.NO
                                            && (input.Status == null || input.Status == op.Status));

            if (tradingProviderId != null)
            {
                orderPaymentQuery = orderPaymentQuery.Where(o => o.TradingProviderId == tradingProviderId);
            }
            else
            {
                orderPaymentQuery = orderPaymentQuery.Where(o => input.TradingProviderIds != null && input.TradingProviderIds.Contains(o.TradingProviderId));
            }
            result.TotalItems = orderPaymentQuery.Count();
            orderPaymentQuery = orderPaymentQuery.OrderByDescending(o => o.Id);

            if (input.PageSize != -1)
            {
                orderPaymentQuery = orderPaymentQuery.Skip(input.Skip).Take(input.PageSize);
            }
            result.Items = orderPaymentQuery.ToList();
            return result;
        }

        /// <summary>
        /// Lấy danh sách thanh toán không phân trang
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="status"></param>
        /// <param name="tranType"></param>
        /// <returns></returns>
        public List<RstOrderPayment> GetAll(long orderId, int? status = null, int? tranType = null)
        {
            _logger.LogInformation($"{nameof(RstOrderPaymentEFRepository)}->{nameof(FindAll)}: orderId = {orderId}, status = {status}, tranType = {tranType} ");

            var orderPaymentQuery = _dbSet.Where(op => op.OrderId == orderId && op.Deleted == YesNo.NO
                                            && (status == null || status == op.Status)
                                            && (tranType == null || op.TranType == tranType));
            return orderPaymentQuery.ToList();
        }

        public decimal SumPaymentDepositAmount(int orderId)
        {
            _logger.LogInformation($"{nameof(RstOrderPaymentEFRepository)}->{nameof(SumPaymentDepositAmount)}: orderId = {orderId}");
            return GetPaymentDepositAmountByOrder(orderId).Sum(p => p.PaymentAmount);
        }

        /// <summary>
        /// Lấy danh sách thanh toán cọc
        /// </summary>
        public IQueryable<RstOrderPayment> GetPaymentDepositAmountByOrder(int orderId)
        {
            _logger.LogInformation($"{nameof(RstOrderPaymentEFRepository)}->{nameof(GetPaymentDepositAmountByOrder)}: orderId = {orderId}");
            return _dbSet.Where(p => p.OrderId == orderId && p.Status == OrderPaymentStatus.DA_THANH_TOAN && p.TranClassify == TranClassifies.THANH_TOAN && p.TranType == TranTypes.THU && p.Deleted == YesNo.NO);
        }

        public RstOrderPayment GetFirstPayment(int orderId)
        {
            _logger.LogInformation($"{nameof(RstOrderPaymentEFRepository)}->{nameof(GetFirstPayment)}: orderId = {orderId}");

            List<int> orderPaymentStatus = new List<int>()
            {
                OrderPaymentStatus.NHAP,
                OrderPaymentStatus.DA_THANH_TOAN
            };
            return _dbSet.FirstOrDefault(op => op.OrderId == orderId && op.Deleted == YesNo.NO && op.TranType == TranTypes.THU
                                            && orderPaymentStatus.Contains(op.Status));
        }
    }
}
