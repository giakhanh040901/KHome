using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Office2010.Excel;
using EPIC.DataAccess.Base;
using EPIC.InvestEntities.DataEntities;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Shared;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace EPIC.InvestRepositories
{
    public class InvestOrderContractFileEFRepository : BaseEFRepository<OrderContractFile>
    {
        public InvestOrderContractFileEFRepository(EpicSchemaDbContext dbContext, ILogger logger) : base(dbContext, logger, $"{DbSchemas.EPIC}.{OrderContractFile.SEQ}")
        {
        }

        public OrderContractFile FindById(int id)
        {
            _logger.LogInformation($"{nameof(FindById)}: id = {id}");
            var result = _epicSchemaDbContext.InvestOrderContractFile.FirstOrDefault(e => e.Id == id);
            return result;
        }

        public OrderContractFile Add(OrderContractFile input)
        {
            _logger.LogInformation($"{nameof(InvestOrderContractFileEFRepository)}->{nameof(Add)}: input = {JsonSerializer.Serialize(input)}");
            var orderQuery = _epicSchemaDbContext.InvOrders.FirstOrDefault(o => o.Id == input.OrderId && o.TradingProviderId == input.TradingProviderId && o.Deleted == YesNo.NO)
                .ThrowIfNull(_epicSchemaDbContext, ErrorCode.InvestOrderNotFound);
            if (orderQuery.DeliveryStatus == DeliveryStatus.RECEIVE)
            {
                orderQuery.DeliveryStatus = DeliveryStatus.COMPLETE;
                orderQuery.FinishedDate = DateTime.Now;
                orderQuery.FinishedDateModifiedBy = input.CreatedBy;
            }
            input.Id = (int)NextKey();
            input.CreatedDate = DateTime.Now;
            input.Deleted = YesNo.NO;
            return _dbSet.Add(input).Entity;
        }

        public void Update(OrderContractFile input)
        {
            _logger.LogInformation($"{nameof(InvestOrderContractFileEFRepository)}->{nameof(Update)}: input = {JsonSerializer.Serialize(input)}");
            var orderQuery = _epicSchemaDbContext.InvOrders.FirstOrDefault(o => o.Id == input.OrderId && o.TradingProviderId == input.TradingProviderId && o.Deleted == YesNo.NO)
                .ThrowIfNull(_epicSchemaDbContext, ErrorCode.InvestOrderNotFound);

            var orderContractFileQuery = _dbSet.FirstOrDefault(r => r.Id == input.Id && r.OrderId == input.OrderId && r.TradingProviderId == input.TradingProviderId && r.Deleted == YesNo.NO);
            if (orderContractFileQuery != null)
            {
                orderContractFileQuery.FileScanUrl = input.FileScanUrl;
                orderContractFileQuery.FileSignatureUrl = input.FileSignatureUrl;
                orderContractFileQuery.FileTempPdfUrl = input.FileTempPdfUrl;
                orderContractFileQuery.FileTempUrl = input.FileTempUrl;
                orderContractFileQuery.IsSign = input.IsSign;
                orderContractFileQuery.PageSign = input.PageSign;
                orderContractFileQuery.WithdrawalId = input.WithdrawalId;
                orderContractFileQuery.RenewalsId = input.RenewalsId;
                orderContractFileQuery.Times = input.Times;
                orderContractFileQuery.ContractCodeGen = input.ContractCodeGen;
                orderContractFileQuery.ModifiedBy = input.ModifiedBy;
                orderContractFileQuery.ModifiedDate = DateTime.Now;
            }
        }

    }
}
