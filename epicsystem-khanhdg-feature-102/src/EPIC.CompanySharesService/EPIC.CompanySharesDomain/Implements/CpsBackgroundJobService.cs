using EPIC.CompanySharesDomain.Interfaces;
using EPIC.CompanySharesRepositories;
using EPIC.Entities;
using EPIC.Entities.Dto.ContractData;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Hangfire;
using EPIC.Utils.ConstantVariables.Shared;
using EPIC.Utils.Filter;
using Hangfire;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.ServiceModel;
using System.Threading.Tasks;

namespace EPIC.CompanySharesDomain.Implements
{
    public class CpsBackgroundJobService
    {
        private readonly ILogger _logger;
        private readonly string _connectionString;

        private readonly IContractTemplateServices _contractTemplateServices;
        private readonly IContractDataServices _contractDataServices;

        private readonly OrderRepository _orderRepository;
        private readonly CpsPolicyRepository _policyRepository;
        private readonly OrderContractFileRepository _orderContractFileRepository;

        public CpsBackgroundJobService(
            ILogger<CpsBackgroundJobService> logger,
            DatabaseOptions databaseOptions,
            IContractTemplateServices contractTemplateServices,
            IContractDataServices contractDataServices)
        {
            _logger = logger;
            _connectionString = databaseOptions.ConnectionString;
            _contractTemplateServices = contractTemplateServices;
            _contractDataServices = contractDataServices;
            _orderRepository = new OrderRepository(_connectionString, _logger);
            _policyRepository = new CpsPolicyRepository(_connectionString, _logger);
            _orderContractFileRepository = new OrderContractFileRepository(_connectionString, _logger);
        }

        /// <summary>
        /// khi đặt lệnh bằng App thì sinh ra file chữ ký
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="tradingProviderId"></param>
        [AutomaticRetry(Attempts = 5)]
        [Queue(HangfireQueues.Shared)]
        [HangfireLogEverything]
        public async Task UpdateContractFileApp(int orderId, int tradingProviderId)
        {
            var order = _orderRepository.FindById(orderId, tradingProviderId);
            if (order == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy thông tin lệnh: {orderId}"), new FaultCode(((int)ErrorCode.CpsOrderNotFound).ToString()), "");
            }
            //Lấy ra danh sách hợp đồng
            var contractTemplate = _contractTemplateServices.FindAllByOrder(-1, 1, null, orderId, tradingProviderId);
            if (contractTemplate == null)
            {
                throw new FaultException($"Không tìm thấy danh sách hợp đồng mẫu: orderId = {orderId}, tradingProviderId = {tradingProviderId}");
            }

            List<Task> taskHandleFiles = new();
            List<string> filePathOldRemove = new();
            foreach (var contract in contractTemplate.Items)
            {
                SaveFileDto saveFile = null;
                try
                {
                    //Fill hợp đồng và lưu trữ
                    saveFile = await _contractDataServices.SaveContract(orderId, contract.Id, contract.IsSign);
                    taskHandleFiles.Add(saveFile.SaveFileTasks);
                    filePathOldRemove.Add(saveFile.FilePathToBeDeleted);

                    if (contract.OrderContractFileId != null)
                    {
                        var orderContractFile = _orderContractFileRepository.FindById(contract.OrderContractFileId ?? 0);
                        if (orderContractFile == null)
                        {
                            throw new FaultException(new FaultReason($"Không tìm thấy hợp đồng: {contract.OrderContractFileId}"), new FaultCode(((int)ErrorCode.BondOrderContractFileNotFound).ToString()), "");
                        }
                        //Lưu đường dẫn vào bảng Bond Secondary Contract
                        orderContractFile.FileSignatureUrl = saveFile.FileSignatureUrl;
                        orderContractFile.FileTempPdfUrl = saveFile.FileSignatureUrl;
                        orderContractFile.FileTempUrl = saveFile.FileTempUrl;
                        orderContractFile.PageSign = saveFile.PageSign;
                        _orderContractFileRepository.Update(orderContractFile);
                    }
                    else
                    {
                        //Lưu đường dẫn vào bảng Bond Secondary Contract
                        _orderContractFileRepository.Add(new CompanySharesEntities.DataEntities.OrderContractFile
                        {
                            ContractTempId = contract.Id,
                            FileTempUrl = saveFile.FileTempUrl,
                            FileSignatureUrl = saveFile.FileSignatureUrl,
                            FileTempPdfUrl = saveFile.FileSignatureUrl,
                            OrderId = orderId,
                            PageSign = saveFile.PageSign,
                            TradingProviderId = tradingProviderId,
                        });
                    }
                }
                catch (Exception ex) //file lỗi
                {
                    _logger.LogError(ex, $"Lỗi sinh file hợp đồng mobile app: orderId = {orderId}, tradingProviderId = {tradingProviderId}");
                }
            }

            //xử lý chuyển đổi file chạy tất cả cùng lúc
            await Task.WhenAll(taskHandleFiles);
            foreach (var itemFilePath in filePathOldRemove) //xử lý file cũ
            {
                var fileSignPathOld = itemFilePath.Replace(ContractFileExtensions.DOCX, ContractFileExtensions.SIGN_DOCX);
                if (File.Exists(fileSignPathOld)) //xóa file cũ có con dấu
                {
                    File.Delete(fileSignPathOld);
                }
            }
        }
    }
}
