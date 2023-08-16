using DocumentFormat.OpenXml.Bibliography;
using EPIC.BondDomain.Interfaces;
using EPIC.BondEntities.DataEntities;
using EPIC.BondRepositories;
using EPIC.Entities;
using EPIC.Entities.DataEntities;
using EPIC.Entities.Dto.ContractData;
using EPIC.Entities.Dto.RocketChat;
using EPIC.IdentityRepositories;
using EPIC.RocketchatDomain.Interfaces;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Hangfire;
using EPIC.Utils.ConstantVariables.Shared;
using EPIC.Utils.Filter;
using Hangfire;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.BondDomain.Implements
{
    public class BondBackgroundJobService
    {

        private readonly ILogger _logger;
        private readonly string _connectionString;

        public readonly BondOrderRepository _orderRepository;
        private readonly BondSecondaryContractRepository _bondSecondaryContractRepository;
        private readonly BondSecondaryRepository _productBondSecondaryRepository;
        private readonly UserRepository _userRepository;
        private readonly IBondContractTemplateService _contractTemplateServices;
        private readonly IBondContractDataService _contractDataServices;

        public BondBackgroundJobService(
            ILogger<BondBackgroundJobService> logger,
            IBondContractTemplateService contractTemplateServices,
            IBondContractDataService contractDataServices,
            DatabaseOptions databaseOptions)
        {
            _logger = logger;
            _connectionString = databaseOptions.ConnectionString;
            _orderRepository = new BondOrderRepository(_connectionString, _logger);
            _bondSecondaryContractRepository = new BondSecondaryContractRepository(_connectionString, _logger);
            _productBondSecondaryRepository = new BondSecondaryRepository(_connectionString, _logger);
            _userRepository = new UserRepository(_connectionString, _logger);
            _contractTemplateServices = contractTemplateServices;
            _contractDataServices = contractDataServices;

        }

        /// <summary>
        /// khi đặt lệnh bằng App thì sinh ra file chữ ký
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="filePath"></param>
        [AutomaticRetry(Attempts = 5)]
        [Queue(HangfireQueues.Shared)]
        [HangfireLogEverything]
        public async Task UpdateContractFileApp(int orderId, int tradingProviderId)
        {
            var order = _orderRepository.FindById(orderId, tradingProviderId);
            if (order == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy thông tin lệnh: {orderId}"), new FaultCode(((int)ErrorCode.BondOrderNotFound).ToString()), "");
            }
            var productBondPolicy = _productBondSecondaryRepository.FindPolicyById((int)order.PolicyId, tradingProviderId);
            if (productBondPolicy == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy chính sách: {order.PolicyId}"), new FaultCode(((int)ErrorCode.BondSecondaryNotFound).ToString()), "");
            }
            //Lấy ra danh sách hợp đồng
            var contractTemplate = _contractTemplateServices.FindAllByOrder(-1, 1, null, orderId, tradingProviderId);
            if (contractTemplate == null)
            {
                return;
            }
            List<string> filePathOldRemove = new();
            foreach (var contract in contractTemplate.Items)
            {
                SaveFileDto saveFile = null;
                try
                {
                    //Fill hợp đồng và lưu trữ
                    saveFile = await _contractDataServices.SaveContractPdfNotSign(orderId, contract.Id, (int)order.PolicyDetailId);
                    filePathOldRemove.Add(saveFile.FilePathToBeDeleted);
                    if (contract.SecondaryContractFileId != null)
                    {
                        var bondSecondaryContract = _bondSecondaryContractRepository.FindById(contract.SecondaryContractFileId ?? 0);
                        if (bondSecondaryContract == null)
                        {
                            throw new FaultException(new FaultReason($"Không tìm thấy hợp đồng: {contract.SecondaryContractFileId}"), new FaultCode(((int)ErrorCode.BondOrderContractFileNotFound).ToString()), "");
                        }
                        //Lưu đường dẫn vào bảng Bond Secondary Contract  
                        bondSecondaryContract.FileSignatureUrl = saveFile.FileSignatureUrl;
                        bondSecondaryContract.PageSign = saveFile.PageSign;
                        _bondSecondaryContractRepository.Update(bondSecondaryContract);
                    }
                    else
                    {
                        //Lưu đường dẫn vào bảng Bond Secondary Contract
                        var bondSecondaryContract = new BondSecondaryContract
                        {
                            ContractTempId = contract.Id,
                            FileSignatureUrl = saveFile.FileSignatureUrl,
                            OrderId = orderId,
                            TradingProviderId = tradingProviderId,
                            PageSign = saveFile.PageSign
                        };
                        _bondSecondaryContractRepository.Add(bondSecondaryContract);
                    }
                }
                catch (Exception ex) //file lỗi
                {
                    _logger.LogError(ex, $"Lỗi sinh file hợp đồng mobile app: orderId = {orderId}, tradingProviderId = {tradingProviderId}");
                }
                //xử lý chuyển đổi file chạy tất cả cùng lúc
                foreach (var itemFilePath in filePathOldRemove) //xử lý file cũ
                {
                    if (File.Exists(itemFilePath)) //xóa file cũ
                    {
                        File.Delete(itemFilePath);
                    }
                    var fileSignPathOld = itemFilePath.Replace(ContractFileExtensions.DOCX, ContractFileExtensions.SIGN_DOCX);
                    if (File.Exists(fileSignPathOld)) //xóa file cũ có con dấu
                    {
                        File.Delete(fileSignPathOld);
                    }
                }
            }
        }
    }
}
