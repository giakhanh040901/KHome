using EPIC.DataAccess.Base;
using EPIC.DataAccess.Models;
using EPIC.MSB.ConstVariables;
using EPIC.PaymentEntities.DataEntities;
using EPIC.PaymentEntities.Dto.Msb;
using EPIC.PaymentEntities.Dto.MsbRequestPayment;
using EPIC.PaymentEntities.Dto.MsbRequestPaymentDetail;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Payment;
using EPIC.Utils.ConstantVariables.Shared;
using EPIC.Utils.Linq;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace EPIC.PaymentRepositories
{
    public class MsbRequestPaymentDetailEFRepository : BaseEFRepository<MsbRequestPaymentDetail>
    {
        public MsbRequestPaymentDetailEFRepository(EpicSchemaDbContext dbContext, ILogger logger) : base(dbContext, logger, $"{DbSchemas.EPIC}.{MsbRequestPaymentDetail.SEQ}")
        {
        }

        /// <summary>
        /// Thêm chi tiết request Yêu cầu chi hộ MSB
        /// </summary>
        /// <param name="input"></param>
        public MsbRequestPaymentDetail Add(MsbRequestPaymentDetail input)
        {
            _logger.LogInformation($"{nameof(MsbRequestPaymentDetailEFRepository)} -> {nameof(Add)}: input = {JsonSerializer.Serialize(input)}");
            return _dbSet.Add(input).Entity;
        }

        /// <summary>
        /// Cập nhật request Yêu cầu chi hộ MSB
        /// </summary>
        /// <param name="input"></param>
        public void Update(MsbRequestPaymentDetail input)
        {
            _logger.LogInformation($"{nameof(MsbRequestPaymentDetailEFRepository)} -> {nameof(Update)}: input = {JsonSerializer.Serialize(input)}");

            var requestDetail = _dbSet.FirstOrDefault(e => e.Id == input.Id);
            requestDetail.OwnerAccount = input.OwnerAccount;
            requestDetail.OwnerAccountNo = input.OwnerAccountNo;
            requestDetail.BankId = input.BankId;
            requestDetail.Exception = input.Exception;
            requestDetail.Status = input.Status;
            requestDetail.TradingBankAccId = input.TradingBankAccId;
        }

        /// <summary>
        /// Tìm kiếm theo request Id 
        /// </summary>
        /// <param name="requestDetailId"></param>
        public MsbRequestPaymentDetail FindById(long requestDetailId)
        {
            _logger.LogInformation($"{nameof(MsbRequestPaymentDetailEFRepository)} -> {nameof(FindById)}: requestDetailId = {requestDetailId}");

            var requestDetail = _dbSet.FirstOrDefault(e => e.Id == requestDetailId);
            return requestDetail;
        }

        /// <summary>
        /// Tìm kiếm tất cả request Id 
        /// </summary>
        /// <param name="input"></param>
        public PagingResult<ViewMsbRequestPaymentDto> FindAll(FilterMsbRequestDetailDto input)
        {
            _logger.LogInformation($"{nameof(MsbRequestPaymentDetailEFRepository)} -> {nameof(FindAll)}: input = {JsonSerializer.Serialize(input)}");
            PagingResult<ViewMsbRequestPaymentDto> result = new();
            var requestDetailQuery = (from requestDetail in _dbSet
                                      join request in _epicSchemaDbContext.MsbRequestPayment on requestDetail.RequestId equals request.Id
                                      join bank in _epicSchemaDbContext.CoreBanks on requestDetail.BankId equals bank.BankId
                                      join notificationPayment in _epicSchemaDbContext.MsbNotificationPayments
                                      on MsbPrefixRequestId.Prefix + requestDetail.Id equals notificationPayment.TransId into notifications
                                      from notify in notifications.DefaultIfEmpty()
                                      where (input.ProductType == null || request.ProductType == input.ProductType)
                                      && (input.Status == null || requestDetail.Status == input.Status)
                                      && (input.TradingProviderId == null || request.TradingProdiverId == input.TradingProviderId)
                                      && ((from invInterestPayment in _epicSchemaDbContext.InvestInterestPayments
                                           join invOrder in _epicSchemaDbContext.InvOrders on invInterestPayment.OrderId equals invOrder.Id
                                           where requestDetail.ReferId == invInterestPayment.Id && requestDetail.DataType == RequestPaymentDataTypes.EP_INV_INTEREST_PAYMENT
                                           && (invOrder.ContractCode == input.ContractCode || input.ContractCode == null) && invOrder.Deleted == YesNo.NO && invInterestPayment.Deleted == YesNo.NO
                                           select invOrder.ContractCode).Any()
                                       || (from invWithdrawal in _epicSchemaDbContext.InvestWithdrawals
                                           join invOrder in _epicSchemaDbContext.InvOrders on invWithdrawal.OrderId equals invOrder.Id
                                           where requestDetail.ReferId == invWithdrawal.Id && requestDetail.DataType == RequestPaymentDataTypes.EP_INV_WITHDRAWAL
                                           && (invOrder.ContractCode == input.ContractCode && input.ContractCode == null) && invOrder.Deleted == YesNo.NO && invWithdrawal.Deleted == YesNo.NO
                                           select invOrder.ContractCode).Any()
                                       || (from ganInterestPayment in _epicSchemaDbContext.GarnerInterestPayments
                                           join ganInterestPaymentDetail in _epicSchemaDbContext.GarnerInterestPaymentDetails
                                           on ganInterestPayment.Id equals ganInterestPaymentDetail.InterestPaymentId
                                           join ganOrder in _epicSchemaDbContext.GarnerOrders on ganInterestPaymentDetail.OrderId equals ganOrder.Id
                                           where requestDetail.ReferId == ganInterestPayment.Id && requestDetail.DataType == RequestPaymentDataTypes.GAN_INTEREST_PAYMENT
                                           && (ganOrder.ContractCode == input.ContractCode || input.ContractCode == null) && ganOrder.Deleted == YesNo.NO && ganInterestPayment.Deleted == YesNo.NO
                                           select ganOrder.ContractCode).Any()
                                       || (from ganWithdrawals in _epicSchemaDbContext.GarnerWithdrawals
                                           join ganWithdrawalDetail in _epicSchemaDbContext.GarnerWithdrawalDetails
                                           on ganWithdrawals.Id equals ganWithdrawalDetail.WithdrawalId
                                           join ganOrder in _epicSchemaDbContext.GarnerOrders on ganWithdrawalDetail.OrderId equals ganOrder.Id
                                           where requestDetail.ReferId == ganWithdrawals.Id && requestDetail.DataType == RequestPaymentDataTypes.GAN_WITHDRAWAL
                                           && (ganOrder.ContractCode == input.ContractCode || input.ContractCode == null) && ganOrder.Deleted == YesNo.NO && ganWithdrawals.Deleted == YesNo.NO
                                           select ganOrder.ContractCode).Any())
                                      select new ViewMsbRequestPaymentDto
                                      {
                                          Id = requestDetail.Id,
                                          RequestId = requestDetail.RequestId,
                                          ProductType = request.ProductType,
                                          RequestType = request.RequestType,
                                          DataType = requestDetail.DataType,
                                          ReferId = requestDetail.ReferId,
                                          Status = requestDetail.Status,
                                          AmountMoney = requestDetail.AmountMoney,
                                          BankId = requestDetail.BankId,
                                          BankName = bank.BankName,
                                          Exception = requestDetail.Exception,
                                          Bin = requestDetail.Bin,
                                          Note = requestDetail.Note,
                                          OwnerAccount = requestDetail.OwnerAccount,
                                          OwnerAccountNo = requestDetail.OwnerAccountNo,
                                          CreatedDate = request.CreatedDate,
                                          ResponseDate = notify.CreatedDate,
                                          TradingProviderId = request.TradingProdiverId,
                                      });

            requestDetailQuery = requestDetailQuery.OrderByDescending(e => e.Id);
            requestDetailQuery = requestDetailQuery.OrderDynamic(input.Sort);
            result.TotalItems = requestDetailQuery.Count();
            if (input.PageSize != -1)
            {
                requestDetailQuery = requestDetailQuery.Skip(input.Skip).Take(input.PageSize);
            }
            result.Items = requestDetailQuery.ToList();
            return result;
        }

        public PagingResult<MsbCollectionPaymentDto> FindAllCollectPayment(MsbCollectionPaymentFilterDto input)
        {
            _logger.LogInformation($"{nameof(MsbRequestPaymentDetailEFRepository)} -> {nameof(FindAllCollectPayment)}: input = {JsonSerializer.Serialize(input)}");

            PagingResult<MsbCollectionPaymentDto> result = new();
            var collectionPaymentQuery = (from requestDetail in _dbSet
                                          join request in _epicSchemaDbContext.MsbRequestPayment on requestDetail.RequestId equals request.Id
                                          join notificationPayment in _epicSchemaDbContext.MsbNotificationPayments on requestDetail.Id.ToString() equals notificationPayment.TransId.Replace(MsbPrefixRequestId.Prefix, "")
                                          where (input.ProductType == null || request.ProductType == input.ProductType)
                                          && (input.Status == null || requestDetail.Status == input.Status)
                                          select new MsbCollectionPaymentDto
                                          {
                                              RequestDetail = requestDetail,
                                              NotificationPayment = notificationPayment
                                          });

            collectionPaymentQuery = collectionPaymentQuery.OrderBy(e => e.RequestDetail.Id);

            result.TotalItems = collectionPaymentQuery.Count();
            if (input.PageSize != -1)
            {
                collectionPaymentQuery = collectionPaymentQuery.Skip(input.Skip).Take(input.PageSize);
            }
            result.Items = collectionPaymentQuery.ToList();
            return result;
        }
    }
}
