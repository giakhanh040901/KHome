using EPIC.DataAccess.Base;
using EPIC.DataAccess.Models;
using EPIC.InvestEntities.DataEntities;
using EPIC.PaymentEntities.DataEntities;
using EPIC.PaymentEntities.Dto.Msb;
using EPIC.PaymentSharedEntities.Dto;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Core;
using EPIC.Utils.ConstantVariables.Payment;
using EPIC.Utils.ConstantVariables.Shared;
using EPIC.Utils.Linq;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.Json;

namespace EPIC.PaymentRepositories
{
    public class MsbNotificationRepository : BaseEFRepository<MsbNotification>
    {
        public MsbNotificationRepository(EpicSchemaDbContext dbContext, ILogger logger) : base(dbContext, logger, $"{DbSchemas.EPIC}.{MsbNotification.SEQ}")
        {
        }

        public MsbNotification FindById(long id)
        {
            return _dbSet.FirstOrDefault(o => o.Id == id);
        }

        /// <summary>
        /// Thêm mới
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public MsbNotification Add(MsbNotification entity)
        {
            _logger.LogInformation($"{nameof(Add)}: input = {JsonSerializer.Serialize(entity)}");
            return _dbSet.Add(new MsbNotification
            {
                Id = (long)NextKey(),
                TranSeq = entity.TranSeq,
                VaCode = entity.VaCode,
                VaNumber = entity.VaNumber,
                FromAccountName = entity.FromAccountName,
                FromAccountNumber = entity.FromAccountNumber,
                ToAccountName = entity.ToAccountName,
                ToAccountNumber = entity.ToAccountNumber,
                TranAmount = entity.TranAmount,
                TranRemark = entity.TranRemark,
                TranDate = entity.TranDate,
                Signature = entity.Signature,
                Exception = entity.Exception,
                TransferDate = entity.TransferDate,
                Status = entity.Status,
                Ip = entity.Ip,
            }).Entity;
        }

        public void Update(MsbNotification entity)
        {
            _dbSet.Update(entity);
        }

        public MsbNotification FindByTranSeq(string tranSeq)
        {
            _logger.LogInformation($"{nameof(FindByTranSeq)}: tranSeq = {tranSeq}");
            return _dbSet.FirstOrDefault(x => x.TranSeq == tranSeq);
        }

        public PagingResult<MsbNotification> FindAll(MsbFilterPaymentDto input)
        {
            _logger.LogInformation($"{nameof(FindAll)}: input = {JsonSerializer.Serialize(input)}");

            PagingResult<MsbNotification> result = new();
            var notificationQuery = _dbSet.OrderByDescending(p => p.Id).AsQueryable();

            if (input.StartDate != null)
            {
                notificationQuery = notificationQuery.Where(r => r.CreatedDate >= input.StartDate);
            }

            if (input.EndDate != null)
            {
                notificationQuery = notificationQuery.Where(r => r.CreatedDate <= input.EndDate);
            }

            if (!string.IsNullOrEmpty(input.PrefixMsb))
            {
                notificationQuery = notificationQuery.Where(n => n.VaCode.StartsWith(input.PrefixMsb));
            }

            if (input.PageSize != -1)
            {
                notificationQuery = notificationQuery.Skip(input.Skip).Take(input.PageSize);
            }

            result.TotalItems = notificationQuery.Count();
            result.Items = notificationQuery.ToList();
            return result;
        }

        public PagingResult<ViewMsbCollectionPaymentDto> FindAllCollectionPayment(MsbCollectionPaymentFilterDto input, int? tradingProviderId = null)
        {
            _logger.LogInformation($"{nameof(FindAllCollectionPayment)}: input = {JsonSerializer.Serialize(input)}");

            PagingResult<ViewMsbCollectionPaymentDto> result = new();

            var productTypeDict = new Dictionary<int, string>()
            {
                {ProductTypes.GARNER, ContractCodes.GARNER},
                {ProductTypes.INVEST, ContractCodes.INVEST},
                {ProductTypes.REAL_ESTATE, ContractCodes.REAL_ESTATE},
                {ProductTypes.EVENT, ContractCodes.EVENT},
            };

            var notificationQuery = from notiMsb in  _dbSet.AsQueryable()
                                    let dateTimeOffset = DateTimeOffset.ParseExact(notiMsb.TranDate, "yyMMddHHmmss", CultureInfo.InvariantCulture)
                                    let contractCode = (notiMsb.ProjectType == ContractCodes.INVEST) ? (_epicSchemaDbContext.InvOrders.FirstOrDefault(x => x.Id == notiMsb.ReferId).ContractCode)
                                          : ((notiMsb.ProjectType == ContractCodes.GARNER) ? (_epicSchemaDbContext.GarnerOrders.FirstOrDefault(x => x.Id == notiMsb.ReferId).ContractCode)
                                              : (_epicSchemaDbContext.RstOrders.FirstOrDefault(x => x.Id == notiMsb.ReferId).ContractCode))
                                    let genContractCode = (notiMsb.ProjectType == ContractCodes.INVEST) ? (_epicSchemaDbContext.InvestOrderContractFile.FirstOrDefault(x => x.OrderId == notiMsb.ReferId && x.ContractCodeGen != null && x.Deleted == YesNo.NO).ContractCodeGen ?? contractCode)
                                          : ((notiMsb.ProjectType == ContractCodes.GARNER) ? (_epicSchemaDbContext.GarnerOrderContractFiles.FirstOrDefault(x => x.OrderId == notiMsb.ReferId && x.Deleted == YesNo.NO).ContractCodeGen ?? contractCode)
                                              : (_epicSchemaDbContext.RstOrderContractFiles.FirstOrDefault(x => x.OrderId == notiMsb.ReferId && x.Deleted == YesNo.NO).ContractCodeGen ?? contractCode))
                                    where (input.Status == null || notiMsb.Status == input.Status) && (tradingProviderId == null || notiMsb.TradingProviderId == tradingProviderId)
                                    && (input.ProductType == null || notiMsb.VaNumber.Contains(productTypeDict[input.ProductType.Value])) // lọc theo sản phẩm
                                    && (input.CreatedDate == null || notiMsb.CreatedDate.Date == input.CreatedDate.Value.Date)
                                    && (input.Keyword == null || notiMsb.VaCode.Contains(input.Keyword) || notiMsb.VaNumber.Contains(input.Keyword)
                                          || (genContractCode != null && genContractCode.Contains(input.Keyword)) || (contractCode != null && contractCode.Contains(input.Keyword)))
                                    select new ViewMsbCollectionPaymentDto
                                    {
                                        CreatedDate = notiMsb.CreatedDate,
                                        Exception = notiMsb.Exception,
                                        Id = notiMsb.Id,
                                        FromAccountName = notiMsb.FromAccountName,
                                        FromAccountNumber = notiMsb.FromAccountNumber,
                                        Status = notiMsb.Status,
                                        ToAccountName = notiMsb.ToAccountName,
                                        ToAccountNumber = notiMsb.ToAccountNumber,
                                        TranAmount = notiMsb.TranAmount,
                                        TranDate = notiMsb.TranDate,
                                        TranRemark = notiMsb.TranRemark,
                                        VaNumber = notiMsb.VaNumber,
                                        VaCode = notiMsb.VaCode,
                                        TransferDate = notiMsb.TransferDate,
                                        TranSeq = notiMsb.TranSeq,
                                        GenContractCode = genContractCode,
                                        ReferId = notiMsb.ReferId,
                                        ProjectType = notiMsb.ProjectType,
                                    };
            
            notificationQuery = notificationQuery.OrderByDescending(n => n.Id);
            result.TotalItems = notificationQuery.Count();
            notificationQuery = notificationQuery.OrderDynamic(input.Sort);

            if (input.PageSize != -1)
            {
                notificationQuery = notificationQuery.Skip(input.Skip).Take(input.PageSize);
            }

            result.Items = notificationQuery.ToList();
            return result;
        }
    }
}
