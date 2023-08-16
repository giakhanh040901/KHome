using AutoMapper;
using EPIC.CompanySharesDomain.Interfaces;
using EPIC.CompanySharesEntities.DataEntities;
using EPIC.CompanySharesEntities.Dto.ContractTemplate;
using EPIC.CompanySharesRepositories;
using EPIC.CoreRepositories;
using EPIC.DataAccess.Models;
using EPIC.Entities;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Contract;
using EPIC.Utils.ConstantVariables.Invest;
using EPIC.Utils.DataUtils;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;

namespace EPIC.CompanySharesDomain.Implements
{
    public class ContractTemplateServices : IContractTemplateServices
    {
        private readonly ILogger<ContractTemplateServices> _logger;
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;
        private OrderRepository _orderRepository;
        private CpsPolicyRepository _cpsPolicyRepository;
        private readonly ContractTemplateRepository _contractTemplateRepository;
        private readonly CpsSecondaryRepository _cpsSecondaryRepository;
        private readonly CifCodeRepository _cifCodeRepository;
        private readonly OrderContractFileRepository _orderContractFileRepository;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IMapper _mapper;

        public ContractTemplateServices(ILogger<ContractTemplateServices> logger, IConfiguration configuration, DatabaseOptions databaseOptions, IHttpContextAccessor httpContext, IMapper mapper)
        {
            _logger = logger;
            _configuration = configuration;
            _connectionString = databaseOptions.ConnectionString;
            _contractTemplateRepository = new ContractTemplateRepository(_connectionString, _logger);
            _cpsSecondaryRepository = new CpsSecondaryRepository(_connectionString, _logger);
            _orderRepository = new OrderRepository(_connectionString, _logger);
            _cpsPolicyRepository = new CpsPolicyRepository(_connectionString, _logger);
            _cifCodeRepository = new CifCodeRepository(_connectionString, _logger);
            _orderContractFileRepository = new OrderContractFileRepository(_connectionString, _logger);
            _httpContext = httpContext;
            _mapper = mapper;
        }

        public ContractTemplate Add(CreateContractTemplateDto input)
        {
            var tradingProvider = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var username = CommonUtils.GetCurrentUsername(_httpContext);

            var contractTemplate = new ContractTemplate
            {
                Code = input.Code,
                Name = input.Name,
                TradingProviderId = tradingProvider,
                ContractTempUrl = input.ContractTempUrl,
                SecondaryId = input.SecondaryId,
                Type = input.Type,
                DisplayType = input.DisplayType,
                CreatedBy = username
            };
            return _contractTemplateRepository.Add(contractTemplate);
        }

        public PagingResult<ContractTemplate> FindAll(int pageSize, int pageNumber, string keyword, int secondaryId, string type)
        {
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            return _contractTemplateRepository.FindAll(pageSize, pageNumber, keyword, secondaryId, tradingProviderId, type);
        }

        public int Delete(int id)
        {
            return _contractTemplateRepository.Delete(id);
        }

        public ContractTemplate FindById(int id)
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
            contractTemplate.ContractTempUrl = input.ContractTempUrl;
            contractTemplate.ModifiedBy = username;
            contractTemplate.Type = input.Type;
            contractTemplate.DisplayType = input.DisplayType;
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
                throw new FaultException(new FaultReason($"Không tìm thấy sổ lệnh: {orderId}"), new FaultCode(((int)ErrorCode.CpsOrderNotFound).ToString()), "");
            }
            var policy = _cpsPolicyRepository.FindPolicyById(order.CpsPolicyId ?? 0, tradingProviderId);
            if (policy == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy chính sách: {order.CpsPolicyId ?? 0}"), new FaultCode(((int)ErrorCode.CpsPolicyNotFound).ToString()), "");
            }
            var cifCode = _cifCodeRepository.GetByCifCode(order.CifCode);
            if (cifCode == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy cif code: {cifCode}"), new FaultCode(((int)ErrorCode.CoreCifCodeNotFound).ToString()), "");
            }
            string contractTemplateType = null;
            if (cifCode.InvestorId != null)
            {
                contractTemplateType = SharedContractTemplateType.INVESTOR;
            }
            else
            {
                contractTemplateType = SharedContractTemplateType.BUSINESS_CUSTOMER;
            }
            var contractTemplate = _contractTemplateRepository.FindAll(pageSize, pageNumber, keyword, order.CpsSecondaryId ?? 0, tradingProviderId, contractTemplateType);
            if (contractTemplate.TotalItems < 1)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy mẫu hợp đồng dành cho nhà đầu tư là {ContractDataUtils.GetNameINVType(contractTemplateType)}"), new FaultCode(((int)ErrorCode.InvestContractTemplateNotFound).ToString()), "");
            }
            var result = new PagingResult<ViewContractTemplateByOrder>();
            var contractTemplatebyOrders = new List<ViewContractTemplateByOrder>();
            int totalItems = 0;
            foreach (var item in contractTemplate.Items)
            {
                var templateContract = new ViewContractTemplateByOrder();
                var orderContractFile = _orderContractFileRepository.Find(orderId, tradingProviderId, item.Id);
                if (orderContractFile != null)
                {
                    //đã có file scan thì vẫn hiện mẫu hợp đồng dù đã xóa
                    templateContract.Name = item.Name;
                    templateContract.Code = item.Code;
                    templateContract.Type = item.Type;
                    templateContract.DisplayType = item.DisplayType;
                    templateContract.TradingProviderId = item.TradingProviderId;
                    templateContract.ContractTempUrl = item.ContractTempUrl;
                    templateContract.Id = item.Id;
                    templateContract.CreatedBy = item.CreatedBy;
                    templateContract.CreatedDate = item.CreatedDate;
                    templateContract.ModifiedBy = item.ModifiedBy;
                    templateContract.ModifiedDate = item.ModifiedDate;
                    templateContract.Deleted = item.Deleted;
                    templateContract.Status = item.Status;
                    templateContract.OrderContractFileId = orderContractFile.Id;
                    templateContract.FileTempUrl = orderContractFile.FileTempUrl;
                    templateContract.FileSignatureUrl = orderContractFile.FileSignatureUrl;
                    templateContract.FileScanUrl = orderContractFile.FileScanUrl;
                    templateContract.IsSign = orderContractFile.IsSign;

                }
                else
                {
                    //chưa có file scan nên phải check xem đã xóa và trạng thái
                    if (item.Deleted != YesNo.YES && item.Status != ContractTemplateStatus.DEACTIVE)
                    {
                        templateContract.Name = item.Name;
                        templateContract.Code = item.Code;
                        templateContract.Type = item.Type;
                        templateContract.DisplayType = item.DisplayType;
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

        /// <summary>
        /// check file hợp đồng đảm bảo khi lấy danh sách hợp đồng cho order
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageNumber"></param>
        /// <param name="keyword"></param>
        /// <param name="orderId"></param>
        /// <param name="tradingProvider"></param>
        /// <returns></returns>
        public PagingResult<ViewContractTemplateByOrder> FindAllByOrderCheckDisplayType(int pageSize, int pageNumber, string keyword, int orderId, int? tradingProvider)
        {
            var tradingProviderId = tradingProvider ?? CommonUtils.GetCurrentTradingProviderId(_httpContext);

            var order = _orderRepository.FindById(orderId, tradingProviderId);
            if (order == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy sổ lệnh: {orderId}"), new FaultCode(((int)ErrorCode.InvestOrderNotFound).ToString()), "");
            }
            var policy = _cpsPolicyRepository.FindPolicyById(order.CpsPolicyId ?? 0, tradingProviderId);
            if (policy == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy chính sách: {order.CpsPolicyId ?? 0}"), new FaultCode(((int)ErrorCode.InvestPolicyNotFound).ToString()), "");
            }
            var cifCode = _cifCodeRepository.GetByCifCode(order.CifCode);
            if (cifCode == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy cif code: {cifCode}"), new FaultCode(((int)ErrorCode.CoreCifCodeNotFound).ToString()), "");
            }
            string contractTemplateType = null;
            if (cifCode.InvestorId != null)
            {
                contractTemplateType = SharedContractTemplateType.INVESTOR;
            }
            else
            {
                contractTemplateType = SharedContractTemplateType.BUSINESS_CUSTOMER;
            }
            var contractTemplate = _contractTemplateRepository.FindAll(pageSize, pageNumber, keyword, order.CpsSecondaryId ?? 0, tradingProviderId, contractTemplateType);
            if (order.Status != InvestOrderStatus.DANG_DAU_TU)
            {
                contractTemplate.Items = contractTemplate.Items.Where(c => c.DisplayType != ContractTemplateDisplayType.AFTER);
            }
            if (contractTemplate.TotalItems < 1)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy mẫu hợp đồng dành cho nhà đầu tư là {ContractDataUtils.GetNameINVType(contractTemplateType)}"), new FaultCode(((int)ErrorCode.InvestContractTemplateNotFound).ToString()), "");
            }
            var result = new PagingResult<ViewContractTemplateByOrder>();
            var contractTemplatebyOrders = new List<ViewContractTemplateByOrder>();
            int totalItems = 0;
            foreach (var item in contractTemplate.Items)
            {
                var templateContract = new ViewContractTemplateByOrder();
                /*var orderContractFile = _orderContractFileRepository.Find(orderId, tradingProviderId, item.Id);
                if (orderContractFile != null)
                {
                    //đã có file scan thì vẫn hiện mẫu hợp đồng dù đã xóa
                    templateContract.Name = item.Name;
                    templateContract.Code = item.Code;
                    templateContract.Type = item.Type;
                    templateContract.DisplayType = item.DisplayType;
                    templateContract.TradingProviderId = item.TradingProviderId;
                    templateContract.ContractTempUrl = item.ContractTempUrl;
                    templateContract.Id = item.Id;
                    templateContract.CreatedBy = item.CreatedBy;
                    templateContract.CreatedDate = item.CreatedDate;
                    templateContract.ModifiedBy = item.ModifiedBy;
                    templateContract.ModifiedDate = item.ModifiedDate;
                    templateContract.Deleted = item.Deleted;
                    templateContract.Status = item.Status;
                    templateContract.OrderContractFileId = orderContractFile.Id;
                    templateContract.FileTempUrl = orderContractFile.FileTempUrl;
                    templateContract.FileSignatureUrl = orderContractFile.FileSignatureUrl;
                    templateContract.FileScanUrl = orderContractFile.FileScanUrl;
                    templateContract.IsSign = orderContractFile.IsSign;

                }
                else
                {
                    //chưa có file scan nên phải check xem đã xóa và trạng thái
                    if (item.Deleted != YesNo.YES && item.Status != ContractTemplateStatus.DEACTIVE)
                    {
                        templateContract.Name = item.Name;
                        templateContract.Code = item.Code;
                        templateContract.Type = item.Type;
                        templateContract.DisplayType = item.DisplayType;
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
                }*/
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
        public PagingResult<ContractTemplateAppDto> FindAllForApp(int policyDetailId)
        {
            var result = new PagingResult<ContractTemplateAppDto>();
            var contractTemplates = new List<ContractTemplateAppDto>();
            var policyDetail = _cpsPolicyRepository.FindPolicyDetailById(policyDetailId);
            if (policyDetail == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy thông tin kỳ hạn"), new FaultCode(((int)ErrorCode.CpsPolicyDetailNotFound).ToString()), "");
            }

            var policy = _cpsPolicyRepository.FindPolicyById(policyDetail.PolicyId, policyDetail.TradingProviderId);
            if (policy == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy thông tin chính sách"), new FaultCode(((int)ErrorCode.CpsPolicyNotFound).ToString()), "");
            }

            var contractTemplate = _contractTemplateRepository.FindAll(-1, 1, null, policy.SecondaryId, policyDetail.TradingProviderId, SharedContractTemplateType.INVESTOR);
            foreach (var contract in contractTemplate.Items)
            {
                contractTemplates.Add(_mapper.Map<ContractTemplateAppDto>(contract));
            }
            result.Items = contractTemplates;
            result.TotalItems = contractTemplate.TotalItems;
            return result;
        }

        public PagingResult<ViewContractForApp> FindAllFileSignatureForApp(int orderId)
        {
            var order = _orderRepository.FindById(orderId, null);
            if (order == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy sổ lệnh: {orderId}"), new FaultCode(((int)ErrorCode.CpsOrderNotFound).ToString()), "");
            }
            var contractFileSignatures = _orderContractFileRepository.FindAll(orderId, order.TradingProviderId);
            var result = new PagingResult<ViewContractForApp>();
            var contractFiles = new List<ViewContractForApp>();
            result.TotalItems = contractFileSignatures.Count(c => c.FileSignatureUrl != null);
            foreach (var contract in contractFileSignatures)
            {
                if (contract.FileSignatureUrl != null)
                {
                    var fileSignatures = new ViewContractForApp();
                    var template = _contractTemplateRepository.FindById(contract.ContractTempId ?? 0, order.TradingProviderId);
                    if (template != null)
                    {
                        fileSignatures.Name = template.Name;
                        fileSignatures.Code = template.Code;
                        fileSignatures.ContractTempId = template.Id;
                        fileSignatures.TradingProviderId = template.TradingProviderId;
                        fileSignatures.OrderContractFileId = contract.Id;
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
