using EPIC.DataAccess.Base;
using EPIC.GarnerEntities.DataEntities;
using EPIC.RealEstateEntities.DataEntities;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace EPIC.RealEstateRepositories
{
    public class RstOrderContractFileEFRepository : BaseEFRepository<RstOrderContractFile>
    {
        public RstOrderContractFileEFRepository(DbContext dbContext, ILogger logger) : base(dbContext, logger, $"{DbSchemas.EPIC_REAL_ESTATE}.{RstOrderContractFile.SEQ}")
        {
        }

        /// <summary>
        /// Thêm hồ sơ khách hàng
        /// </summary>
        /// <param name="input"></param>
        /// <param name="username"></param>
        /// <param name="tradingProviderId"></param>
        /// <returns></returns>
        public RstOrderContractFile Add(RstOrderContractFile input, string username, int tradingProviderId)
        {
            _logger.LogInformation($"{nameof(Add)}: input = {JsonSerializer.Serialize(input)}, tradingProviderId = {tradingProviderId}, username = {username}");

            return _dbSet.Add(new RstOrderContractFile
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
                ContractCodeGen = input.ContractCodeGen,
            }).Entity;
        }

        /// <summary>
        /// Update hồ sơ khách hàng
        /// </summary>
        /// <param name="input"></param>
        /// <param name="username"></param>
        /// <param name="tradingProviderId"></param>
        /// <returns></returns>
        public RstOrderContractFile Update(RstOrderContractFile input, string username, int tradingProviderId)
        {
            _logger.LogInformation($"{nameof(Update)}: input = {JsonSerializer.Serialize(input)}, tradingProviderId = {tradingProviderId}, username = {username}");

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
            }
            return orderContractFile;
        }

        /// <summary>
        /// Lấy danh sách hồ sơ khách hàng 
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="tradingProviderId"></param>
        /// <returns></returns>
        public List<RstOrderContractFile> FindAll(long orderId, int? tradingProviderId = null)
        {
            _logger.LogInformation($"{nameof(FindAll)}: orderId = {orderId}, tradingProviderId = {tradingProviderId}");
            var result = from odf in _dbSet
                         where odf.OrderId == orderId && (tradingProviderId == null || odf.TradingProviderId == tradingProviderId)
                         && odf.Deleted == YesNo.NO
                         orderby odf.CreatedDate descending
                         select odf;
            return result.ToList();
        }

        public void DeleteContractFileByOrderId(long orderId)
        {
            _logger.LogInformation($"{nameof(DeleteContractFileByOrderId)}: orderId = {orderId}");
            var orderContractFiles = _dbSet.Where(d => d.OrderId == orderId && d.Deleted == YesNo.NO);
            foreach (var orderContractFile in orderContractFiles)
            {
                orderContractFile.Deleted = YesNo.YES;
                _dbSet.Update(orderContractFile);
            }

        }

        /// <summary>
        /// Lấy hợp đồng sổ lệnh theo id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="tradingProviderId"></param>
        /// <returns></returns>
        public RstOrderContractFile FindById(long id, int? tradingProviderId = null)
        {
            _logger.LogInformation($"{nameof(FindById)}: Id = {id}, tradingProviderId = {tradingProviderId}");

            var result = _dbSet.FirstOrDefault(e => e.Id == id && (tradingProviderId == null || e.TradingProviderId == tradingProviderId) && e.Deleted == YesNo.NO);
            return result;
        }

        /// <summary>
        /// Lấy hợp đồng sổ lệnh theo id của order
        /// </summary>
        /// <param name="id"></param>
        /// <param name="tradingProviderId"></param>
        /// <returns></returns>
        public List<RstOrderContractFile> FindByOrderAndContractTemplate(int orderId, int contractTemplateTempId, int? tradingProviderId = null)
        {
            _logger.LogInformation($"{nameof(FindByOrderAndContractTemplate)}: orderId = {orderId}, contractTemplateTempId = {contractTemplateTempId}, tradingProviderId = {tradingProviderId}");
            
            var result = _dbSet.Where(e => e.OrderId == orderId && e.ContractTempId == contractTemplateTempId &&(tradingProviderId == null || e.TradingProviderId == tradingProviderId) && e.Deleted == YesNo.NO);
           
            return result.ToList();
        }
    }
}
