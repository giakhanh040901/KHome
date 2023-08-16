using EPIC.DataAccess.Base;
using EPIC.GarnerEntities.DataEntities;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace EPIC.GarnerRepositories
{
    public class GarnerOrderContractFileEFRepository : BaseEFRepository<GarnerOrderContractFile>
    {
        public GarnerOrderContractFileEFRepository(DbContext dbContext, ILogger logger) : base(dbContext, logger, $"{DbSchemas.EPIC_GARNER}.{GarnerOrderContractFile.SEQ}")
        {
        }

        /// <summary>
        /// Thêm hồ sơ khách hàng
        /// </summary>
        /// <param name="input"></param>
        /// <param name="username"></param>
        /// <param name="tradingProviderId"></param>
        /// <returns></returns>
        public GarnerOrderContractFile Add(GarnerOrderContractFile input, string username, int tradingProviderId)
        {
            _logger.LogInformation($"{System.Reflection.MethodBase.GetCurrentMethod().Name}: input = {JsonSerializer.Serialize(input)}, tradingProviderId = {tradingProviderId}, username = {username}");

            return _dbSet.Add(new GarnerOrderContractFile
            {
                Id = (int)NextKey(),
                TradingProviderId = tradingProviderId,
                OrderId = input.OrderId,
                ContractTempId = input.ContractTempId,
                FileTempUrl = input.FileTempUrl,
                FileScanUrl = input.FileScanUrl,
                FileSignatureUrl = input.FileSignatureUrl,
                FileTempPdfUrl = input.FileTempPdfUrl,
                PageSign = input.PageSign,
                CreatedBy = username,
                CreatedDate = DateTime.Now,
                WithdrawalId = input.WithdrawalId,
                ContractCodeGen = input.ContractCodeGen
            }).Entity;
        }

        /// <summary>
        /// Update hồ sơ khách hàng
        /// </summary>
        /// <param name="input"></param>
        /// <param name="username"></param>
        /// <param name="tradingProviderId"></param>
        /// <returns></returns>
        public GarnerOrderContractFile Update(GarnerOrderContractFile input, string username, int tradingProviderId)
        {
            _logger.LogInformation($"{System.Reflection.MethodBase.GetCurrentMethod().Name}: input = {JsonSerializer.Serialize(input)}, tradingProviderId = {tradingProviderId}, username = {username}");

            var orderContractFile = _dbSet.FirstOrDefault(d => d.Id == input.Id && d.TradingProviderId == tradingProviderId && d.Deleted == YesNo.NO);
            if (orderContractFile != null)
            {
                orderContractFile.FileTempUrl = input.FileTempUrl;
                orderContractFile.FileTempPdfUrl = input.FileTempPdfUrl;
                orderContractFile.FileSignatureUrl = input.FileSignatureUrl;
                orderContractFile.FileScanUrl = input.FileScanUrl;
                orderContractFile.IsSign = input.IsSign;
                orderContractFile.PageSign = input.PageSign;
                orderContractFile.ModifiedBy = username;
                orderContractFile.ModifiedDate = DateTime.Now;
                orderContractFile.WithdrawalId = input.WithdrawalId;
                orderContractFile.FileSignatureStampUrl = input.FileSignatureStampUrl;
            }
            return orderContractFile;
        }
        
        /// <summary>
        /// Lấy danh sách hồ sơ khách hàng 
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="tradingProviderId"></param>
        /// <returns></returns>
        public List<GarnerOrderContractFile> FindAll(long orderId, int tradingProviderId)
        {
            _logger.LogInformation($"{System.Reflection.MethodBase.GetCurrentMethod().Name}: orderId = {orderId}, tradingProviderId = {tradingProviderId}");

            var result = from odf in _dbSet
                         where odf.OrderId == orderId && odf.TradingProviderId == tradingProviderId
                         && odf.Deleted == YesNo.NO
                         orderby odf.CreatedDate descending
                         select odf;
            return result.ToList();
        }

        public void DeleteContractFileByOrderId(long orderId)
        {
            _logger.LogInformation($"{nameof(DeleteContractFileByOrderId)}: orderId = {orderId}");
            var orderContractFiles = _dbSet.Where(d => d.OrderId == orderId && d.Deleted == YesNo.NO);
            foreach(var orderContractFile in orderContractFiles)
            {
                orderContractFile.Deleted = YesNo.YES;
                _dbSet.Update(orderContractFile);
            }
            
        }

        /// <summary>
        /// Lấy Hồ sơ khách hàng theo id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="tradingProviderId"></param>
        /// <returns></returns>
        public GarnerOrderContractFile FindById(long id, int tradingProviderId)
        {
            _logger.LogInformation($"{System.Reflection.MethodBase.GetCurrentMethod().Name}: Id = {id}, tradingProviderId = {tradingProviderId}");

            var result = _dbSet.FirstOrDefault(e => e.Id == id && e.TradingProviderId == tradingProviderId && e.Deleted == YesNo.NO);
            return result;
        }

        /// <summary>
        /// Lấy Hợp đồng đặt lệnh, rút tiền khách hàng theo id lệnh, id mẫu hợp đồng (khi rút tiền sẽ sinh file nên 1 mẫu có nhiều file hợp đồng)
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="tradingProviderId"></param>
        /// <param name="contractTemplateId"></param>
        /// <returns></returns>
        public List<GarnerOrderContractFile> FindByContractTemplateAndOrder(long orderId, int contractTemplateId, int? tradingProviderId = null)
        {
            _logger.LogInformation($"{System.Reflection.MethodBase.GetCurrentMethod().Name}: orderId = {orderId}, tradingProviderId = {tradingProviderId}, contractTemplateId = {contractTemplateId}");
            var result = _dbSet.Where(e => e.OrderId == orderId && (tradingProviderId == null || e.TradingProviderId == tradingProviderId) && e.ContractTempId == contractTemplateId && e.Deleted == YesNo.NO).ToList();
            return result;
        }

        /// <summary>
        /// Chỉ Lấy Hợp đồng đặt lệnh khách hàng theo id lệnh, id mẫu hợp đồng (khi đặt lệnh chỉ sinh ra 1 file mỗi mẫu hợp đồng)
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="tradingProviderId"></param>
        /// <param name="contractTemplateId"></param>
        /// <returns></returns>
        public GarnerOrderContractFile FindByContractTemplateAndOrderAdd(long orderId, int contractTemplateId, int? tradingProviderId = null)
        {
            _logger.LogInformation($"{System.Reflection.MethodBase.GetCurrentMethod().Name}: orderId = {orderId}, tradingProviderId = {tradingProviderId}, contractTemplateId = {contractTemplateId}");
            var result = _dbSet.FirstOrDefault(e => e.OrderId == orderId && (tradingProviderId == null || e.TradingProviderId == tradingProviderId) && e.ContractTempId == contractTemplateId && e.Deleted == YesNo.NO);
            return result;
        }

        public GarnerOrderContractFile FindByContractTemplateAndOrderWithdrawal(long orderId, int contractTemplateId, long withdrawalId, int? tradingProviderId = null)
        {
            _logger.LogInformation($"{System.Reflection.MethodBase.GetCurrentMethod().Name}: orderId = {orderId}, tradingProviderId = {tradingProviderId}, contractTemplateId = {contractTemplateId}");
            var result = _dbSet.FirstOrDefault(e => e.OrderId == orderId && (tradingProviderId == null || e.TradingProviderId == tradingProviderId) && e.ContractTempId == contractTemplateId && e.WithdrawalId == withdrawalId && e.Deleted == YesNo.NO);
            return result;
        }
    }
}
