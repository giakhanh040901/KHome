using EPIC.BondDomain.Interfaces;
using EPIC.BondEntities.DataEntities;
using EPIC.BondRepositories;
using EPIC.DataAccess.Models;
using EPIC.Entities;
using EPIC.Entities.DataEntities;
using EPIC.Entities.Dto.ContractTemplate;
using EPIC.Utils;
using EPIC.Utils.DataUtils;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.BondDomain.Implements
{
    public class BondContractTemplateService : IBondContractTemplateService
    {
        private ILogger _logger;
        private IConfiguration _configuration;
        private string _connectionString;
        private BondContractTemplateRepository _contractTemplateRepository;
        private BondSecondaryRepository _productBondSecondaryRepository;
        private BondOrderRepository _orderRepository;
        private BondSecondaryContractRepository _bondSecondaryContractRepository;
        private IHttpContextAccessor _httpContext;

        public BondContractTemplateService(ILogger<BondContractTemplateService> logger, IConfiguration configuration, DatabaseOptions databaseOptions, IHttpContextAccessor httpContext)
        {
            _logger = logger;
            _configuration = configuration;
            _connectionString = databaseOptions.ConnectionString;
            _contractTemplateRepository = new BondContractTemplateRepository(_connectionString, _logger);
            _orderRepository = new BondOrderRepository(_connectionString, _logger);
            _productBondSecondaryRepository = new BondSecondaryRepository(_connectionString, _logger);
            _bondSecondaryContractRepository = new BondSecondaryContractRepository(_connectionString, _logger);
            _httpContext = httpContext;
        }
        public BondContractTemplate Add(CreateContractTemplateDto input)
        {
            var tradingProvider = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var username = CommonUtils.GetCurrentUsername(_httpContext);

            var contractTemplate = new BondContractTemplate
            {
                Code = input.Code,
                Name = input.Name,
                ContractType = input.ContractType,
                TradingProviderId = tradingProvider,
                ContractTempUrl = input.ContractTempUrl,
                Classify = input.Classify,
                SecondaryId = input.SecondaryId,
                Type = input.Type,
                CreatedBy = username
            };
            return _contractTemplateRepository.Add(contractTemplate);
        }

        public int Delete(int id)
        {
            return _contractTemplateRepository.Delete(id);
        }

        public PagingResult<BondContractTemplate> FindAll(int pageSize, int pageNumber, string keyword, int bondSecondaryId, int? classify, string type)
        {
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            return _contractTemplateRepository.FindAll(pageSize, pageNumber, keyword, bondSecondaryId, tradingProviderId, classify, type);
        }

        public BondContractTemplate FindById(int id)
        {
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var result = _contractTemplateRepository.FindById(id, tradingProviderId);
            return result;
        }

        public int Update(UpdateContractTemplateDto input)
        {
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            var contractTemplate = _contractTemplateRepository.FindById(input.Id, tradingProviderId);
            contractTemplate.Code = input.Code;
            contractTemplate.Name = input.Name;
            contractTemplate.ContractType = input.ContractType;
            contractTemplate.ContractTempUrl = input.ContractTempUrl;
            contractTemplate.ModifiedBy = username;
            contractTemplate.Classify = input.Classify;
            contractTemplate.Type = input.Type;
            return _contractTemplateRepository.Update(contractTemplate);
        }

        public int ChangeStatus(int id)
        {
            var contractTemplate = FindById(id);
            var status = ContractTemplateStatus.ACTIVE;
            if (contractTemplate.Status == ContractTemplateStatus.ACTIVE)
            {
                status = ContractTemplateStatus.DEACTIVE;
            }
            else
            {
                status = ContractTemplateStatus.ACTIVE;
            }
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            return _contractTemplateRepository.UpdateStatus(id, status, username);
        }

        public PagingResult<ViewContractTemplateByOrder> FindAllByOrder(int pageSize, int pageNumber, string keyword, int orderId, int? tradingProvider)
        {
            var tradingProviderId = tradingProvider ?? CommonUtils.GetCurrentTradingProviderId(_httpContext);

            var order = _orderRepository.FindById(orderId, tradingProviderId);
            if (order == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy sổ lệnh: {orderId}"), new FaultCode(((int)ErrorCode.NotFound).ToString()), "");
            }
            var productBondPolicy = _productBondSecondaryRepository.FindPolicyById((int)order.PolicyId, tradingProviderId);
            if (productBondPolicy == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy chính sách: {order.PolicyId}"), new FaultCode(((int)ErrorCode.NotFound).ToString()), "");
            }
            var contractTemplate = _contractTemplateRepository.FindAll(pageSize, pageNumber, keyword, (int)order.SecondaryId, tradingProviderId, (int)productBondPolicy.Classify, null);
            if (contractTemplate.TotalItems < 1)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy mẫu hợp đồng loại chính sách: { ContractDataUtils.GetNameClassify(productBondPolicy.Classify)}"), new FaultCode(((int)ErrorCode.NotFound).ToString()), "");
            }
            var result = new PagingResult<ViewContractTemplateByOrder>();
            var contractTemplatebyOrders = new List<ViewContractTemplateByOrder>();
            int totalItems = 0;
            foreach (var item in contractTemplate.Items)
            {
                var templateContract = new ViewContractTemplateByOrder();
                var orderContractFile = _bondSecondaryContractRepository.Find(orderId, tradingProviderId, item.Id);
                if (orderContractFile != null)
                {
                    //đã có file scan thì vẫn hiện mẫu hợp đồng dù đã xóa
                    templateContract.Name = item.Name;
                    templateContract.Code = item.Code;
                    templateContract.Classify = item.Classify;
                    templateContract.ContractType = item.ContractType;
                    templateContract.TradingProviderId = item.TradingProviderId;
                    templateContract.ContractTempUrl = item.ContractTempUrl;
                    templateContract.Id = item.Id;
                    templateContract.CreatedBy = item.CreatedBy;
                    templateContract.CreatedDate = item.CreatedDate;
                    templateContract.ModifiedBy = item.ModifiedBy;
                    templateContract.ModifiedDate = item.ModifiedDate;
                    templateContract.Deleted = item.Deleted;
                    templateContract.Status = item.Status;
                    templateContract.SecondaryContractFileId = orderContractFile.Id;
                    templateContract.FileTempUrl = orderContractFile.FileTempUrl;
                    templateContract.FileSignatureUrl = orderContractFile.FileSignatureUrl;
                    templateContract.FileScanUrl = orderContractFile.FileScanUrl;
                    templateContract.IsSign = orderContractFile.IsSign;

                }
                else
                {
                    //chưa có file scan nên phải check xem đã xóa và trạng thái
                    if (item.Deleted != YesNo.YES && item.Status != BondPolicyTemplate.DEACTIVE)
                    {
                        templateContract.Name = item.Name;
                        templateContract.Code = item.Code;
                        templateContract.Classify = item.Classify;
                        templateContract.ContractType = item.ContractType;
                        templateContract.TradingProviderId = item.TradingProviderId;
                        templateContract.ContractTempUrl = item.ContractTempUrl;
                        templateContract.Id = item.Id;
                        templateContract.CreatedBy = item.CreatedBy;
                        templateContract.CreatedDate = item.CreatedDate;
                        templateContract.ModifiedBy = item.ModifiedBy;
                        templateContract.ModifiedDate = item.ModifiedDate;
                        templateContract.Deleted = item.Deleted;
                        templateContract.Status = item.Status;
                    }
                }
                if (templateContract.Id > 0)
                {
                    contractTemplatebyOrders.Add(templateContract);
                    totalItems += 1;
                }

            }
            result.Items = contractTemplatebyOrders;
            result.TotalItems = totalItems;
            return result;
        }

        public PagingResult<BondContractTemplate> FindAllForApp(int policyDetailId)
        {
            var bondPolicyDetail = _productBondSecondaryRepository.FindPolicyDetailById(policyDetailId);
            if (bondPolicyDetail == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy thông tin kỳ hạn"), new FaultCode(((int)ErrorCode.BondPolicyDetailNotFound).ToString()), "");
            }

            var bondPolicy = _productBondSecondaryRepository.FindPolicyById(bondPolicyDetail.PolicyId, bondPolicyDetail.TradingProviderId);
            if (bondPolicy == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy thông tin chính sách"), new FaultCode(((int)ErrorCode.BondPolicyNotFound).ToString()), "");
            }

            return _contractTemplateRepository.FindAll(-1, 1, null, bondPolicy.SecondaryId, bondPolicyDetail.TradingProviderId, (int)bondPolicy.Classify, null);
        }

        public PagingResult<ViewOrderContractForApp> FindAllFileSignatureForApp(long orderId)
        {
            var order = _orderRepository.FindById(orderId, null);
            if (order == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy sổ lệnh: {orderId}"), new FaultCode(((int)ErrorCode.BondOrderNotFound).ToString()), "");
            }
            var contractFileSignatures = _bondSecondaryContractRepository.FindAll(orderId, order.TradingProviderId);
            var result = new PagingResult<ViewOrderContractForApp>();
            var contractFiles = new List<ViewOrderContractForApp>();
            result.TotalItems = contractFileSignatures.Count(c => c.FileSignatureUrl != null);
            foreach(var contract in contractFileSignatures)
            {
                if(contract.FileSignatureUrl != null)
                {
                    var fileSignatures = new ViewOrderContractForApp();
                    var template = _contractTemplateRepository.FindById(contract.ContractTempId, order.TradingProviderId);
                    if(template != null)
                    {
                        fileSignatures.ContractTemplateName = template.Name;
                        fileSignatures.ContractTemplateCode = template.Code;
                        fileSignatures.ContractTemplateId = template.Id;
                        fileSignatures.TradingProviderId = template.TradingProviderId;
                        fileSignatures.Id = contract.Id;
                        fileSignatures.IsSign = contract.IsSign;
                    }
                    contractFiles.Add(fileSignatures);
                }  
            }
            result.Items = contractFiles;
            return result;
        }
    }
}
