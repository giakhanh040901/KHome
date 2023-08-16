using AutoMapper;
using DocumentFormat.OpenXml.Office2010.Excel;
using EPIC.CoreRepositories;
using EPIC.DataAccess.Base;
using EPIC.DataAccess.Models;
using EPIC.Entities;
using EPIC.Entities.DataEntities;
using EPIC.FileDomain.Services;
using EPIC.ImageAPI.Models;
using EPIC.InvestDomain.Interfaces;
using EPIC.InvestEntities.DataEntities;
using EPIC.InvestEntities.Dto.ContractTemplate;
using EPIC.InvestEntities.Dto.ContractTemplateTemp;
using EPIC.InvestEntities.Dto.Distribution;
using EPIC.InvestEntities.Dto.InvConfigContractCode;
using EPIC.InvestRepositories;
using EPIC.RealEstateEntities.DataEntities;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Contract;
using EPIC.Utils.ConstantVariables.Identity;
using EPIC.Utils.ConstantVariables.Invest;
using EPIC.Utils.ConstantVariables.Shared;
using EPIC.Utils.DataUtils;
using EPIC.Utils.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text.Json;

namespace EPIC.InvestDomain.Implements
{
    public class ContractTemplateServices : IContractTemplateServices
    {
        private readonly ILogger<ContractTemplateServices> _logger;
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;
        private InvestOrderRepository _orderRepository;
        private InvestPolicyRepository _policyRepository;
        private readonly DistributionRepository _distributionRepository;
        private readonly CifCodeRepository _cifCodeRepository;
        private readonly OrderContractFileRepository _orderContractFileRepository;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IMapper _mapper;
        private readonly EpicSchemaDbContext _dbContext;
        private readonly DefErrorEFRepository _defErrorEFRepository;
        private readonly TradingProviderEFRepository _tradingProviderEFRepository;
        private readonly InvestContractTemplateEFRepository _investContractTemplateEFRepository;
        private readonly InvestContractTemplateTempEFRepository _investContractTemplateTempEFRepository;
        private readonly InvConfigContractCodeEFRepository _investConfigContractCodeEFRepository;
        private readonly IFileServices _fileServices;


        public ContractTemplateServices(ILogger<ContractTemplateServices> logger, 
            IConfiguration configuration, 
            DatabaseOptions databaseOptions, 
            IHttpContextAccessor httpContext, 
            IMapper mapper, 
            EpicSchemaDbContext dbContext,
            IFileServices fileServices)
        {
            _logger = logger;
            _configuration = configuration;
            _fileServices = fileServices;
            _connectionString = databaseOptions.ConnectionString;
            _distributionRepository = new DistributionRepository(_connectionString, _logger);
            _orderRepository = new InvestOrderRepository(_connectionString, _logger);
            _policyRepository = new InvestPolicyRepository(_connectionString, _logger);
            _cifCodeRepository = new CifCodeRepository(_connectionString, _logger);
            _orderContractFileRepository = new OrderContractFileRepository(_connectionString, _logger);
            _httpContext = httpContext;
            _mapper = mapper;
            _dbContext = dbContext;
            _defErrorEFRepository = new DefErrorEFRepository(dbContext);
            _tradingProviderEFRepository = new TradingProviderEFRepository(dbContext, logger);
            _investContractTemplateEFRepository = new InvestContractTemplateEFRepository(dbContext, logger);
            _investContractTemplateTempEFRepository = new InvestContractTemplateTempEFRepository(dbContext, logger);
            _investConfigContractCodeEFRepository = new InvConfigContractCodeEFRepository(dbContext, logger);
        }

        /// <summary>
        /// Thêm mẫu hợp đồng
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        /// <exception cref="FaultException"></exception>
        public void Add(CreateContractTemplateDto input)
        {
            _logger.LogInformation($"{nameof(Add)}: input = {JsonSerializer.Serialize(input)}");
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);

            var transaction = _dbContext.Database.BeginTransaction();
            foreach (var policyIdItem in input.PolicyIds)
            {
                var policy = _policyRepository.FindPolicyById(policyIdItem, tradingProviderId).ThrowIfNull<Policy>(_dbContext, ErrorCode.InvestPolicyNotFound);
                if (input.ContractTemplateTempIds != null)
                {
                    foreach (var contractTemplateTempId in input.ContractTemplateTempIds)
                    {
                        var contractTemplateTemp = _investContractTemplateTempEFRepository.FindById(contractTemplateTempId, tradingProviderId).ThrowIfNull<InvestContractTemplateTemp>(_dbContext, ErrorCode.InvestContractTemplateNotFound);
                        var configContractCode = _investConfigContractCodeEFRepository.FindByConfigContractCodeId(input.ConfigContractId, tradingProviderId).ThrowIfNull<InvestConfigContractCode>(_dbContext, ErrorCode.InvestConfigContractCodeNotFound);
                        var contractTemplateInsert = _investContractTemplateEFRepository.Add(new InvestContractTemplate
                        {
                            PolicyId = policyIdItem,
                            DisplayType = input.DisplayType,
                            ConfigContractId = input.ConfigContractId,
                            ContractSource = input.ContractSource,
                            ContractTemplateTempId = contractTemplateTempId,
                            StartDate = input.StartDate,
                            TradingProviderId = tradingProviderId,
                            CreatedBy = username
                        });
                        _dbContext.SaveChanges();
                    }
                }
                else if (input.DistributionPolicyContractUploads != null)
                {
                    foreach (var item in input.DistributionPolicyContractUploads)
                    {
                        string fileUploadInvestorUrl = null;
                        if (item.InvestorFile != null)
                        {
                            fileUploadInvestorUrl = _fileServices.UploadFile(new UploadFileModel
                            {
                                File = item.InvestorFile,
                                Folder = FileFolder.Invest,
                            });
                        }
                        string fileUploadBusinessCustomerUrl = null;
                        if (item.BusinessCustomerFile != null)
                        {
                            fileUploadBusinessCustomerUrl = _fileServices.UploadFile(new UploadFileModel
                            {
                                File = item.BusinessCustomerFile,
                                Folder = FileFolder.Invest,
                            });
                        }

                        _dbContext.InvestContractTemplates.Add(new InvestContractTemplate
                        {
                            Id = (int)_investContractTemplateEFRepository.NextKey(),
                            PolicyId = policyIdItem,
                            DisplayType = input.DisplayType,
                            ConfigContractId = input.ConfigContractId,
                            ContractSource = input.ContractSource,
                            StartDate = input.StartDate,
                            TradingProviderId = tradingProviderId,
                            FileUploadName = item.DistributionPolicyUploadName,
                            FileUploadInvestorUrl = fileUploadInvestorUrl,
                            FileUploadBusinessCustomerUrl = fileUploadBusinessCustomerUrl,
                            ContractType = item.ContractType
                        });
                    }
                    _dbContext.SaveChanges();
                }
                else
                {
                    _investContractTemplateEFRepository.ThrowException(ErrorCode.InvestContractFileIsRequeired);
                }
            }
            transaction.Commit();
        }

        /// <summary>
        /// Tìm kiếm mẫu hợp đồng
        /// </summary>
        /// <param name="policyId"></param>
        /// <returns></returns>
        public List<ContractTemplateDto> FindAll(int policyId)
        {
            _logger.LogInformation($"{nameof(FindAll)}: policyId = {policyId}");

            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var contractTemplates = _investContractTemplateEFRepository.FindAll(policyId, tradingProviderId);
            return _mapper.Map<List<ContractTemplateDto>>(contractTemplates);
        }

        /// <summary>
        /// Xóa mẫu hợp đồng
        /// </summary>
        /// <param name="id"></param>
        /// <exception cref="FaultException"></exception>
        public void Delete(int id)
        {
            _logger.LogInformation($"{nameof(Delete)}: Id = {id}");

            //Lấy thông tin đối tác
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var contractTemplate = _investContractTemplateEFRepository.FindById(id).ThrowIfNull<InvestContractTemplate>(_dbContext, ErrorCode.InvestContractTemplateNotFound);
            contractTemplate.Deleted = YesNo.YES;
            contractTemplate.ModifiedBy = username;
            contractTemplate.ModifiedDate = DateTime.Now;
            _dbContext.SaveChanges();
        }

        /// <summary>
        /// Tìm mẫu hợp đồng
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="FaultException"></exception>
        public ContractTemplateDto FindById(int id)
        {
            _logger.LogInformation($"{nameof(FindById)}: Id = {id}");

            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var contractTemplate = _investContractTemplateEFRepository.FindById(id, tradingProviderId).ThrowIfNull<InvestContractTemplate>(_dbContext, ErrorCode.InvestContractTemplateNotFound);
            return _mapper.Map<ContractTemplateDto>(contractTemplate);
        }

        /// <summary>
        /// Cập nhật mẫu hợp đồng
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        /// <exception cref="FaultException"></exception>
        public InvestContractTemplate Update(UpdateContractTemplateDto input)
        {
            _logger.LogInformation($"{nameof(Update)}: input = {JsonSerializer.Serialize(input)}");
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var contractTemplate = _dbContext.InvestContractTemplates.FirstOrDefault(ct => ct.Id == input.Id && ct.Deleted == YesNo.NO).ThrowIfNull(_dbContext, ErrorCode.InvestContractTemplateNotFound);
            contractTemplate.DisplayType = input.DisplayType;
            contractTemplate.ContractSource = input.ContractSource;
            contractTemplate.ConfigContractId = input.ConfigContractId;
            contractTemplate.StartDate = input.StartDate;
            if (input.ContractTemplateTempId != null)
            {
                contractTemplate.ContractTemplateTempId = input.ContractTemplateTempId.Value;
                contractTemplate.FileUploadName = null;
                contractTemplate.FileUploadInvestorUrl = null;
                contractTemplate.FileUploadInvestorUrl = null;
                contractTemplate.ContractType = null;
            }
            else if (input.FileUploadName != null)
            {
                contractTemplate.ContractTemplateTempId = null;
                contractTemplate.FileUploadName = input.FileUploadName;
                contractTemplate.ContractType = input.ContractType;
                if (input.FileUploadInvestorUrl != null)
                {
                    contractTemplate.FileUploadInvestorUrl = _fileServices.UploadFile(new UploadFileModel
                    {
                        File = input.FileUploadInvestorUrl,
                        Folder = FileFolder.Invest,
                    });
                }
               
                if (input.FileUploadBusinessCustomerUrl != null)
                {
                    contractTemplate.FileUploadInvestorUrl = _fileServices.UploadFile(new UploadFileModel
                    {
                        File = input.FileUploadBusinessCustomerUrl,
                        Folder = FileFolder.Invest,
                    });
                }
            }
            _dbContext.SaveChanges();
            return contractTemplate;
        }

        public PagingResult<ContractTemplateDto> FindAllContractTemplate(ContractTemplateTempFilterDto input)
        {
            _logger.LogInformation($"{nameof(FindAllContractTemplate)}");
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);

            var result = new PagingResult<ContractTemplateDto>();
            var contractTemplates = (from ct in _dbContext.InvestContractTemplates
                                     join pl in _dbContext.InvestPolicies on ct.PolicyId equals pl.Id
                                     join contractTemplateTemp in _dbContext.InvestContractTemplateTemps on ct.ContractTemplateTempId equals contractTemplateTemp.Id into contractTemplateTemps
                                     from contractTemplateTemp in contractTemplateTemps.DefaultIfEmpty()
                                     join configContractCode in _dbContext.InvestConfigContractCodes on ct.ConfigContractId equals configContractCode.Id into configContractCodes
                                     from configContractCode in configContractCodes.DefaultIfEmpty()
                                     where ct.TradingProviderId == tradingProviderId
                                     && ct.Deleted == YesNo.NO
                                     && pl.DistributionId == input.DistributionId
                                     && pl.Deleted == YesNo.NO
                                     && (ct.ContractTemplateTempId == null || (ct.ContractTemplateTempId != null && contractTemplateTemp.Deleted == YesNo.NO))
                                     && (input.PolicyId == null || ct.PolicyId == input.PolicyId)
                                     && (input.ContractSource == null || ct.ContractSource == input.ContractSource)
                                     && (input.Status == null || ct.Status == input.Status)
                                     && (input.Keyword == null || (ct.ContractTemplateTempId == null || (ct.ContractTemplateTempId != null && contractTemplateTemp.Name.ToLower().Contains(input.Keyword.ToLower()))))
                                     && (input.ContractType == null || (ct.ContractTemplateTempId == null || (ct.ContractTemplateTempId != null && contractTemplateTemp.ContractType == input.ContractType)))
                                     select new ContractTemplateDto
                                     {
                                         Id = ct.Id,
                                         PolicyId = ct.PolicyId,
                                         ConfigContractId = ct.ConfigContractId,
                                         ContractSource = ct.ContractSource,
                                         ContractTemplateTempId = ct.ContractTemplateTempId,
                                         DisplayType = ct.DisplayType,
                                         StartDate = ct.StartDate,
                                         Status = ct.Status,
                                         ConfigContractName = configContractCode.Name,
                                         PolicyName = pl.Name,
                                         ContractType = ct.ContractType,
                                         ContractTemplateName = ct.ContractTemplateTempId == null ? ct.FileUploadName : contractTemplateTemp.Name,
                                         FileUploadBusinessCustomerUrl = ct.FileUploadBusinessCustomerUrl,
                                         FileUploadInvestorUrl = ct.FileUploadInvestorUrl,
                                         FileUploadName = ct.FileUploadName,
                                     });
            result.TotalItems = contractTemplates.Count();
            contractTemplates = contractTemplates.OrderDynamic(input.Sort);
            if(input.PageSize != -1)
            {
                contractTemplates = contractTemplates.Skip(input.Skip).Take(input.PageSize);
            }
            result.Items = contractTemplates;
            return result;
        }

        /// <summary>
        /// danh sách acive
        /// </summary>
        /// <param name="distributionId"></param>
        /// <returns></returns>
        public List<ContractTemplateDto> FindAllContractTemplateActive(int distributionId)
        {
            _logger.LogInformation($"{nameof(FindAllContractTemplateActive)}");
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            //var contractTemplates = _investContractTemplateEFRepository.FindAllContractTemplateActive(distributionId, tradingProviderId);
            var contractTemplates = (from ct in _dbContext.InvestContractTemplates
                                     join pl in _dbContext.InvestPolicies on ct.PolicyId equals pl.Id
                                     join contractTemplateTemp in _dbContext.InvestContractTemplateTemps on ct.ContractTemplateTempId equals contractTemplateTemp.Id into contractTemplateTemps
                                     from contractTemplateTemp in contractTemplateTemps.DefaultIfEmpty()
                                     join configContractCode in _dbContext.InvestConfigContractCodes on ct.ConfigContractId equals configContractCode.Id into configContractCodes
                                     from configContractCode in configContractCodes.DefaultIfEmpty()
                                     where ct.TradingProviderId == tradingProviderId
                                     && ct.Deleted == YesNo.NO
                                     && pl.DistributionId == distributionId
                                     && pl.Deleted == YesNo.NO
                                     && (ct.ContractTemplateTempId == null || (ct.ContractTemplateTempId != null && contractTemplateTemp.Deleted == YesNo.NO))
                                     && ct.Status == Status.ACTIVE
                                     select new ContractTemplateDto
                                     {
                                         Id = ct.Id,
                                         PolicyId = ct.PolicyId,
                                         ConfigContractId = ct.ConfigContractId,
                                         ContractSource = ct.ContractSource,
                                         ContractTemplateTempId = ct.ContractTemplateTempId,
                                         DisplayType = ct.DisplayType,
                                         StartDate = ct.StartDate,
                                         Status = ct.Status,
                                         ConfigContractName = configContractCode.Name,
                                         PolicyName = pl.Name,
                                         ContractType = ct.ContractType,
                                         ContractTemplateName = ct.ContractTemplateTempId == null ? ct.FileUploadName : contractTemplateTemp.Name,
                                         FileUploadBusinessCustomerUrl = ct.FileUploadBusinessCustomerUrl,
                                         FileUploadInvestorUrl = ct.FileUploadInvestorUrl,
                                         FileUploadName = ct.FileUploadName,
                                     });
            return contractTemplates.ToList();
        }

        /// <summary>
        /// Đổi trạng thái mẫu hợp đồng
        /// </summary>
        public void ChangeStatus(int id)
        {
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            _logger.LogInformation($"{nameof(ChangeStatus)}: id={id}, tradingProviderId = {tradingProviderId}");
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            var contractTemplate = _investContractTemplateEFRepository.FindById(id, tradingProviderId).ThrowIfNull<InvestContractTemplate>(_dbContext, ErrorCode.InvestContractTemplateNotFound);
            if (contractTemplate.Status == ContractTemplateStatus.ACTIVE)
            {
                contractTemplate.Status = ContractTemplateStatus.DEACTIVE;
            }
            else
            {
                contractTemplate.Status = ContractTemplateStatus.ACTIVE;
            }
            contractTemplate.ModifiedBy = username;
            contractTemplate.ModifiedDate = DateTime.Now;
            _dbContext.SaveChanges();
        }

        /// <summary>
        /// Lấy danh sách mẫu và  file hợp đồng theo id lệnh
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageNumber"></param>
        /// <param name="keyword"></param>
        /// <param name="orderId"></param>
        /// <param name="tradingProvider"></param>
        /// <param name="contractType">đặt lệnh hay rút tiền</param>
        /// <returns></returns>
        public PagingResult<ViewContractTemplateByOrder> FindAllByOrder(int pageSize, int pageNumber, string keyword, long orderId, int? tradingProvider, int? contractType = null)
        {
            var tradingProviderId = tradingProvider ?? CommonUtils.GetCurrentTradingProviderId(_httpContext);

            var order = _orderRepository.FindById(orderId, tradingProviderId);
            if (order == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy sổ lệnh: {orderId}"), new FaultCode(((int)ErrorCode.InvestOrderNotFound).ToString()), "");
            }
            return FindAllByOrder(pageSize, pageNumber, keyword, order, tradingProvider, contractType);
        }

        /// <summary>
        /// Lấy danh sách mẫu và  file hợp đồng theo id lệnh
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageNumber"></param>
        /// <param name="keyword"></param>
        /// <param name="orderId"></param>
        /// <param name="tradingProvider"></param>
        /// <param name="contractType">đặt lệnh hay rút tiền</param>
        /// <returns></returns>
        public PagingResult<ViewContractTemplateByOrder> FindAllByOrder(int pageSize, int pageNumber, string keyword, InvOrder order, int? tradingProvider, int? contractType = null, string displayType = null)
        {
            var policy = _policyRepository.FindPolicyById(order.PolicyId, order.TradingProviderId);
            if (policy == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy chính sách: {order.PolicyId}"), new FaultCode(((int)ErrorCode.InvestPolicyNotFound).ToString()), "");
            }
            var cifCode = _cifCodeRepository.GetByCifCode(order.CifCode);
            if (cifCode == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy cif code: {cifCode}"), new FaultCode(((int)ErrorCode.CoreCifCodeNotFound).ToString()), "");
            }
            string contractTemplateType = SharedContractTemplateType.BUSINESS_CUSTOMER;
            if (cifCode.InvestorId != null)
            {
                contractTemplateType = SharedContractTemplateType.INVESTOR;
            }
            var contractTemplates = _investContractTemplateEFRepository.FindAllForUpdateContractFile(policy.Id, contractTemplateType, order.TradingProviderId, contractType, displayType, order.Source, Utils.Status.ACTIVE);
            if (contractTemplates.Count < 1)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy mẫu hợp đồng dành cho nhà đầu tư là {ContractDataUtils.GetNameINVType(contractTemplateType)}"), new FaultCode(((int)ErrorCode.InvestContractTemplateNotFound).ToString()), "");
            }
            var result = new PagingResult<ViewContractTemplateByOrder>();
            var contractTemplatebyOrders = new List<ViewContractTemplateByOrder>();
            int totalItems = 0;
            foreach (var item in contractTemplates)
            {
                var templateContract = new ViewContractTemplateByOrder();
                var orderContractFile = _orderContractFileRepository.Find(order.Id, order.TradingProviderId, item.Id);
                //đã có file scan thì vẫn hiện mẫu hợp đồng dù đã xóa
                templateContract.Name = item.Name;
                templateContract.DisplayType = item.DisplayType;
                templateContract.TradingProviderId = item.TradingProviderId;
                templateContract.ContractTempUrl = item.ContractTemplateUrl;
                templateContract.Id = item.Id;
                templateContract.ConfigContractId = item.ConfigContractId;
                templateContract.Status = item.Status;
                templateContract.OrderContractFileId = orderContractFile?.Id;
                templateContract.FileTempUrl = orderContractFile?.FileTempUrl;
                templateContract.FileSignatureUrl = orderContractFile?.FileSignatureUrl;
                templateContract.FileScanUrl = orderContractFile?.FileScanUrl;
                templateContract.IsSign = orderContractFile?.IsSign;
                templateContract.ContractType = item.ContractType;
                templateContract.ContractCode = orderContractFile?.ContractCode;
                contractTemplatebyOrders.Add(templateContract);
                totalItems += 1;
            }
            result.Items = contractTemplatebyOrders;
            result.TotalItems = totalItems;
            return result;
        }

        /// <summary>
        /// check file hợp đồng đảm bảo khi lấy danh sách hợp đồng cho order (list HSKH tại chi tiết sổ lệnh)
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageNumber"></param>
        /// <param name="keyword"></param>
        /// <param name="orderId"></param>
        /// <param name="tradingProvider"></param>
        /// <returns></returns>
        public PagingResult<ViewContractTemplateByOrder> FindAllByOrderCheckDisplayType(int pageSize, int pageNumber, string keyword, int orderId)
        {
            int? tradingProviderId = null;
            var userType = CommonUtils.GetCurrentUserType(_httpContext);
            int? partnerId = null;

            if (userType == UserTypes.TRADING_PROVIDER || userType == UserTypes.ROOT_TRADING_PROVIDER)
            {
                tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            }
            else if (userType == UserTypes.PARTNER || userType == UserTypes.ROOT_PARTNER)
            {
                partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
            }

            var result = new PagingResult<ViewContractTemplateByOrder>();
            var contractTemplatebyOrders = new List<ViewContractTemplateByOrder>();
            //Lấy hợp đồng của lệnh
            var order = _orderRepository.FindById(orderId, tradingProviderId, partnerId);
            if (order == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy sổ lệnh: {orderId}"), new FaultCode(((int)ErrorCode.InvestOrderNotFound).ToString()), "");
            }
            var policy = _policyRepository.FindPolicyById(order.PolicyId, tradingProviderId);
            if (policy == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy chính sách: {order.PolicyId}"), new FaultCode(((int)ErrorCode.InvestPolicyNotFound).ToString()), "");
            }
            var cifCode = _cifCodeRepository.GetByCifCode(order.CifCode);
            if (cifCode == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy cif code: {cifCode}"), new FaultCode(((int)ErrorCode.CoreCifCodeNotFound).ToString()), "");
            }
            string contractTemplateType = SharedContractTemplateType.BUSINESS_CUSTOMER;
            if (cifCode.InvestorId != null)
            {
                contractTemplateType = SharedContractTemplateType.INVESTOR;
            }
            var contractTemplates = _investContractTemplateEFRepository.FindAllForUpdateContractFile(policy.Id, contractTemplateType, tradingProviderId, null, null, order.Source, Status.ACTIVE);
            if (order.Status != InvestOrderStatus.DANG_DAU_TU)
            {
                contractTemplates = contractTemplates.Where(c => c.DisplayType != ContractTemplateDisplayType.AFTER).ToList();
            }
            if (contractTemplates.Count() < 1)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy mẫu hợp đồng dành cho nhà đầu tư là {ContractDataUtils.GetNameINVType(contractTemplateType)}"), new FaultCode(((int)ErrorCode.InvestContractTemplateNotFound).ToString()), "");
            }
            
            
            int totalItems = 0;
            foreach (var item in contractTemplates)
            {
                var orderContractFile = _dbContext.InvestOrderContractFile.FirstOrDefault(e => e.OrderId == order.Id && e.ContractTempId == item.Id && e.Deleted == YesNo.NO && e.TradingProviderId == tradingProviderId);
                if (orderContractFile != null)
                {
                    var note = string.Empty;
                    if (orderContractFile.WithdrawalId != null)
                    {
                        note = $"Hợp đồng rút tiền lần {orderContractFile.Times}";
                    }

                    if (orderContractFile.RenewalsId != null)
                    {
                        note = $"Hợp đồng tái tục lần {orderContractFile.Times}";
                    }

                    //đã có file scan thì vẫn hiện mẫu hợp đồng dù đã xóa
                    var templateContract = new ViewContractTemplateByOrder()
                    {
                        Name = item.Name,
                        DisplayType = item.DisplayType,
                        TradingProviderId = item.TradingProviderId,
                        ContractTempUrl = item.ContractTemplateUrl,
                        Id = item.Id,
                        ConfigContractId = item.ConfigContractId,
                        Status = item.Status,
                        OrderContractFileId = orderContractFile.Id,
                        FileScanUrl = orderContractFile.FileScanUrl,
                        FileSignatureUrl = orderContractFile.FileSignatureUrl,
                        FileTempUrl = orderContractFile.FileTempUrl,
                        IsSign = orderContractFile.IsSign,
                        WithdrawalId = orderContractFile.WithdrawalId,
                        RenewalsId = orderContractFile.RenewalsId,
                        Times = orderContractFile.Times,
                        Note = note
                    };
                    contractTemplatebyOrders.Add(templateContract);
                    totalItems += 1;
                }
            }
            //lệnh tái tục -> lấy thêm hợp đồng của lệnh gốc hoặc lệnh tái tục không giữ hợp đồng gốc gần nhất.
            if(order.RenewalsReferId != null && policy.RenewalsType == InvestRenewalsType.GIU_HOP_DONG_CU)
            {
                var fileContractRenewalsRefers = GetFileContractRenewalsRefer(order);
                contractTemplatebyOrders.AddRange(fileContractRenewalsRefers);
                totalItems += fileContractRenewalsRefers.Count();
            }

            result.Items = contractTemplatebyOrders;
            result.TotalItems = totalItems;
            return result;
        }

        /// <summary>
        /// Lấy danh sách hợp đồng của lệnh gốc hoặc lệnh tái tục không giữ hợp đồng gốc gần nhất 
        /// </summary>
        /// <param name="order"></param>
        /// <param name="policy"></param>
        /// <param name="displayType"></param>
        /// <returns></returns>
        public List<ViewContractTemplateByOrder> GetFileContractRenewalsRefer(InvOrder order)
        {
            var contractTemplatebyOrders = new List<ViewContractTemplateByOrder>();
            //Check xem lệnh giữ hợp đồng hay sinh mới
            if (order.RenewalsReferId != null)
            {
                var RenewalsRefer = _orderRepository.FindById(order.RenewalsReferId ?? 0);
                if (RenewalsRefer == null)
                {
                    throw new FaultException(new FaultReason($"Không tìm thấy sổ lệnh: {order.RenewalsReferId}"), new FaultCode(((int)ErrorCode.InvestOrderNotFound).ToString()), "");
                }
                var policyRenewalsRefer = _policyRepository.FindPolicyById(RenewalsRefer.PolicyId);
                if (policyRenewalsRefer == null)
                {
                    throw new FaultException(new FaultReason($"Không tìm thấy chính sách: {RenewalsRefer.PolicyId}"), new FaultCode(((int)ErrorCode.InvestPolicyNotFound).ToString()), "");
                }
                if (policyRenewalsRefer.RenewalsType == InvestRenewalsType.GIU_HOP_DONG_CU)
                {
                    //Đệ quy để tìm đến lệnh có chính sách tạo hợp đồng mới
                    return GetFileContractRenewalsRefer(RenewalsRefer);
                }
                else
                {
                    // trường hợp tìm được lệnh có chính sách tạo hợp đồng mới
                    var orderContractFile = FindAllByOrder(-1, 1, null, RenewalsRefer, RenewalsRefer.TradingProviderId, ContractTypes.DAT_LENH);
                    contractTemplatebyOrders = orderContractFile.Items.ToList();
                    return contractTemplatebyOrders;
                }
            }
            else
            {
                // trường hợp tìm đến lệnh gốc
                var orderContractFile = FindAllByOrder(-1, 1, null, order, order.TradingProviderId, ContractTypes.DAT_LENH);
                contractTemplatebyOrders = orderContractFile.Items.ToList();
                return contractTemplatebyOrders;
            }
        }

        public PagingResult<ContractTemplateAppDto> FindAllForApp(int policyDetailId)
        {
            var result = new PagingResult<ContractTemplateAppDto>();
            var policyDetail = _policyRepository.FindPolicyDetailById(policyDetailId);
            var policy = _policyRepository.FindPolicyById(policyDetail.PolicyId, policyDetail.TradingProviderId);
            if (policy == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy thông tin chính sách"), new FaultCode(((int)ErrorCode.InvestPolicyNotFound).ToString()), "");
            }

            var contractTemplates = from ct in _dbContext.InvestContractTemplates
                                    join ctt in _dbContext.InvestContractTemplateTemps on ct.ContractTemplateTempId equals ctt.Id into contracTempTemps
                                    from contractTemp in contracTempTemps.DefaultIfEmpty()
                                    where (ct.PolicyId == policy.Id && ct.Deleted == YesNo.NO && (ct.Status == Status.ACTIVE)
                                    && ((ct.ContractType == null && contractTemp.ContractType == ContractTypes.DAT_LENH) || (ct.ContractType != null && ct.ContractType == ContractTypes.DAT_LENH))
                                    && ((contractTemp.ContractSource == SourceOrder.ONLINE || contractTemp.ContractSource == ContractSources.ALL) || (ct.ContractSource == SourceOrder.ONLINE || ct.ContractSource == ContractSources.ALL)))
                                    select new ContractTemplateAppDto
                                    {
                                        PolicyId = ct.PolicyId,
                                        TradingProviderId = ct.TradingProviderId,
                                        ContractTemplateTempId = ct.ContractTemplateTempId,
                                        Name = ct.ContractTemplateTempId == null ? ct.FileUploadName : contractTemp.Name,
                                        StartDate = ct.StartDate,
                                        Id = ct.Id,
                                        Type = SharedContractTemplateType.INVESTOR,
                                    };
            result.Items = contractTemplates;
            return result;
        }

        public PagingResult<ViewContractForApp> FindAllFileSignatureForApp(int orderId)
        {
            var order = _orderRepository.FindById(orderId, null);
            if (order == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy sổ lệnh: {orderId}"), new FaultCode(((int)ErrorCode.InvestOrderNotFound).ToString()), "");
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
                    var template = _investContractTemplateEFRepository.FindByIdForUpdateContractFile(contract.ContractTempId ?? 0, SharedContractTemplateType.INVESTOR, order.TradingProviderId);
                    if (template != null)
                    {
                        fileSignatures.Name = template.Name;
                        fileSignatures.ContractTempId = template.Id;
                        fileSignatures.TradingProviderId = template.TradingProviderId;
                        fileSignatures.OrderContractFileId = contract.Id;
                        fileSignatures.PolicyId = template.PolicyId;
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
