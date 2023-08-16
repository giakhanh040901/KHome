using AutoMapper;
using EPIC.CoreRepositories;
using EPIC.DataAccess.Base;
using EPIC.DataAccess.Models;
using EPIC.Entities;
using EPIC.GarnerDomain.Interfaces;
using EPIC.GarnerEntities.DataEntities;
using EPIC.GarnerEntities.Dto.GarnerContractTemplate;
using EPIC.GarnerEntities.Dto.GarnerContractTemplateApp;
using EPIC.GarnerEntities.Dto.GarnerContractTemplateTemp;
using EPIC.GarnerEntities.Dto.GarnerDistributionConfigContractCode;
using EPIC.GarnerEntities.Dto.GarnerDistributionConfigContractCodeDetail;
using EPIC.GarnerEntities.Dto.GarnerPolicy;
using EPIC.GarnerRepositories;
using EPIC.InvestEntities.DataEntities;
using EPIC.InvestRepositories;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Contract;
using EPIC.Utils.ConstantVariables.Garner;
using EPIC.Utils.ConstantVariables.Identity;
using EPIC.Utils.ConstantVariables.Shared;
using EPIC.Utils.DataUtils;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Text.Json;

namespace EPIC.GarnerDomain.Implements
{
    public class GarnerContractTemplateServices : IGarnerContractTemplateServices
    {
        private readonly EpicSchemaDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<GarnerPolicyTempServices> _logger;
        private readonly string _connectionString;
        private readonly IHttpContextAccessor _httpContext;
        private readonly TradingProviderEFRepository _tradingProviderEFRepository;
        private readonly DefErrorEFRepository _defErrorEFRepository;
        private readonly GarnerDistributionEFRepository _garnerDistributionEFRepository;
        private readonly GarnerPolicyEFRepository _garnerPolicyEFRepository;
        private readonly GarnerContractTemplateEFRepository _garnerContractTemplateEFRepository;
        private readonly GarnerOrderEFRepository _garnerOrderEFRepository;
        private readonly CifCodeEFRepository _cifCodeEFRepository;
        private readonly GarnerOrderContractFileEFRepository _garnerOrderContractFileEFRepository;
        private readonly GarnerContractTemplateTempEFRepository _garnerContractTemplateTempEFRepository;
        private readonly GarnerConfigContractCodeEFRepository _garnerConfigContractCodeEFRepository;
        private readonly GarnerConfigContractCodeDetailEFRepository _garnerConfigContractCodeDetailEFRepository;
        private readonly GarnerHistoryUpdateEFRepository _garnerHistoryUpdateEFRepository;

        public GarnerContractTemplateServices(EpicSchemaDbContext dbContext,
            DatabaseOptions databaseOptions,
            IMapper mapper,
            ILogger<GarnerPolicyTempServices> logger,
            IHttpContextAccessor httpContextAccessor)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
            _connectionString = databaseOptions.ConnectionString;
            _httpContext = httpContextAccessor;
            _defErrorEFRepository = new DefErrorEFRepository(dbContext);
            _tradingProviderEFRepository = new TradingProviderEFRepository(dbContext, logger);
            _garnerDistributionEFRepository = new GarnerDistributionEFRepository(dbContext, logger);
            _garnerContractTemplateEFRepository = new GarnerContractTemplateEFRepository(dbContext, logger);
            _garnerPolicyEFRepository = new GarnerPolicyEFRepository(dbContext, logger);
            _garnerOrderEFRepository = new GarnerOrderEFRepository(dbContext, logger);
            _cifCodeEFRepository = new CifCodeEFRepository(dbContext, logger);
            _garnerOrderContractFileEFRepository = new GarnerOrderContractFileEFRepository(dbContext, logger);
            _garnerContractTemplateTempEFRepository = new GarnerContractTemplateTempEFRepository(dbContext, logger);
            _garnerConfigContractCodeEFRepository = new GarnerConfigContractCodeEFRepository(dbContext, logger);
            _garnerConfigContractCodeDetailEFRepository = new GarnerConfigContractCodeDetailEFRepository(dbContext, logger);
            _garnerHistoryUpdateEFRepository = new GarnerHistoryUpdateEFRepository(dbContext, logger);
        }

        /// <summary>
        /// Thêm mẫu hợp đồng
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        /// <exception cref="FaultException"></exception>
        public void Add(CreateGarnerContractTemplateDto input)
        {
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            _logger.LogInformation($"{nameof(Add)}: input = {JsonSerializer.Serialize(input)} , username = {username}, tradingProviderId = {tradingProviderId}");

            var transaction = _dbContext.Database.BeginTransaction();
            foreach (var policyIdItem in input.PolicyIds)
            {
                var policy = _garnerPolicyEFRepository.FindById(policyIdItem).ThrowIfNull<GarnerPolicy>(_dbContext, ErrorCode.GarnerPolicyNotFound);
                var contractTemplateTemp = _garnerContractTemplateTempEFRepository.FindById(input.ContractTemplateTempId, tradingProviderId).ThrowIfNull<GarnerContractTemplateTemp>(_dbContext, ErrorCode.GarnerContractTemplateTempNotFound);
                var configContractCode = _garnerConfigContractCodeEFRepository.FindByConfigContractCodeId(input.ConfigContractId, tradingProviderId).ThrowIfNull<GarnerConfigContractCode>(_dbContext, ErrorCode.GarnerConfigContractCodeNotFound);
                var contractTemplateInsert = _garnerContractTemplateEFRepository.Add(new GarnerContractTemplate
                {
                    PolicyId = policyIdItem,
                    DisplayType = input.DisplayType,
                    ConfigContractId = input.ConfigContractId,
                    ContractSource = input.ContractSource,
                    ContractTemplateTempId = input.ContractTemplateTempId,
                    StartDate = input.StartDate,
                    TradingProviderId = tradingProviderId,
                    CreatedBy = username
                });

                //Lịch sử thay đổi
                _garnerHistoryUpdateEFRepository.Add(new GarnerHistoryUpdate()
                {
                    UpdateTable = GarnerHistoryUpdateTables.GAN_CONTRACT_TEMPLATE,
                    RealTableId = contractTemplateInsert.Id,
                    Action = ActionTypes.THEM_MOI,
                    Summary = GarnerHistoryUpdateSummary.SUMMARY_ADD_CONTRACT_TEMPLATE
                }, username);

                _dbContext.SaveChanges();
            }
            transaction.Commit();
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
            var contractTepmlate = _garnerContractTemplateEFRepository.FindById(id).ThrowIfNull<GarnerContractTemplate>(_dbContext, ErrorCode.GarnerContractTemplateNotFound);
            contractTepmlate.Deleted = YesNo.YES;
            contractTepmlate.ModifiedBy = username;
            contractTepmlate.ModifiedDate = DateTime.Now;

            //Lịch sử thay đổi
            _garnerHistoryUpdateEFRepository.Add(new GarnerHistoryUpdate()
            {
                UpdateTable = GarnerHistoryUpdateTables.GAN_CONTRACT_TEMPLATE,
                RealTableId = id,
                Action = ActionTypes.XOA,
                Summary = GarnerHistoryUpdateSummary.SUMMARY_DELETE_CONTRACT_TEMPLATE
            }, username);
            _dbContext.SaveChanges();
        }

        /// <summary>
        /// Tìm kiếm mẫu hợp đồng theo chính sách
        /// </summary>
        /// <param name="policyId"></param>
        /// <returns></returns>
        public List<GarnerContractTemplateDto> FindAll(int policyId)
        {
            _logger.LogInformation($"{nameof(FindAll)}: policyId = {policyId}");

            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var contractTemplates = _garnerContractTemplateEFRepository.FindAll(policyId, tradingProviderId);
            return _mapper.Map<List<GarnerContractTemplateDto>>(contractTemplates);
        }

        public PagingResult<GarnerContractTemplateDto> FindAllContractTemplate(GarnerContractTemplateFilterDto input)
        {
            _logger.LogInformation($"{nameof(FindAllContractTemplate)}: input = {JsonSerializer.Serialize(input)}");
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var contractTemplates = _garnerContractTemplateEFRepository.FindAllContractTemplate(input, tradingProviderId);
            foreach (var item in contractTemplates.Items)
            {
                var policy = _garnerPolicyEFRepository.FindById(item.PolicyId, tradingProviderId);
                item.Policy = _mapper.Map<GarnerPolicyDto>(policy);

                var contractTemplateTemp = _garnerContractTemplateTempEFRepository.FindById(item.ContractTemplateTempId, tradingProviderId);
                item.ContractTemplateTemp = _mapper.Map<GarnerContractTemplateTempDto>(contractTemplateTemp);

                var configContractCode = _garnerConfigContractCodeEFRepository.FindByConfigContractCodeId(item.ConfigContractId, tradingProviderId);
                if (configContractCode != null)
                {
                    item.ConfigContractCode = new();
                    item.ConfigContractCode = _mapper.Map<GarnerConfigContractCodeDto>(configContractCode);

                    var configContractCodeDeital = _garnerConfigContractCodeDetailEFRepository.GetAllByConfigId(configContractCode.Id);
                    item.ConfigContractCode.ConfigContractCodeDetails = _mapper.Map<List<GarnerConfigContractCodeDetailDto>>(configContractCodeDeital);
                }    
            }
            return contractTemplates;
        }

        /// <summary>
        /// Lấy danh sách active
        /// </summary>
        /// <param name="distributionId"></param>
        /// <returns></returns>
        public List<GarnerContractTemplateDto> FindAllContractTemplateActive(int distributionId)
        {
            _logger.LogInformation($"{nameof(FindAllContractTemplateActive)}: distributionId : {distributionId}");
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var contractTemplates = _garnerContractTemplateEFRepository.FindAllContractTemplateActive(distributionId, tradingProviderId);
            foreach (var item in contractTemplates)
            {
                var policy = _garnerPolicyEFRepository.FindById(item.PolicyId, tradingProviderId);
                item.Policy = _mapper.Map<GarnerPolicyDto>(policy);

                var contractTemplateTemp = _garnerContractTemplateTempEFRepository.FindById(item.ContractTemplateTempId, tradingProviderId);
                item.ContractTemplateTemp = _mapper.Map<GarnerContractTemplateTempDto>(contractTemplateTemp);

                var configContractCode = _garnerConfigContractCodeEFRepository.FindByConfigContractCodeId(item.ConfigContractId, tradingProviderId);
                if (configContractCode != null)
                {
                    item.ConfigContractCode = new();
                    item.ConfigContractCode = _mapper.Map<GarnerConfigContractCodeDto>(configContractCode);

                    var configContractCodeDeital = _garnerConfigContractCodeDetailEFRepository.GetAllByConfigId(configContractCode.Id);
                    item.ConfigContractCode.ConfigContractCodeDetails = _mapper.Map<List<GarnerConfigContractCodeDetailDto>>(configContractCodeDeital);
                }
            }
            return contractTemplates;
        }

        /// <summary>
        /// Tìm mẫu hợp đồng
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="FaultException"></exception>
        public GarnerContractTemplateDto FindById(int id)
        {
            _logger.LogInformation($"{nameof(FindById)}: Id = {id}");

            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var contractTemplate = _garnerContractTemplateEFRepository.FindById(id, tradingProviderId).ThrowIfNull<GarnerContractTemplate>(_dbContext, ErrorCode.GarnerContractTemplateNotFound);
            return _mapper.Map<GarnerContractTemplateDto>(contractTemplate);
        }

        /// <summary>
        /// Cập nhật mẫu hợp đồng
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        /// <exception cref="FaultException"></exception>
        public GarnerContractTemplate Update(UpdateGarnerContractTemplateDto input)
        {
            _logger.LogInformation($"{nameof(Update)}: input = {JsonSerializer.Serialize(input)}");

            var username = CommonUtils.GetCurrentUsername(_httpContext);
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);

            var contractTemplate = _garnerContractTemplateEFRepository.FindById(input.Id, tradingProviderId).ThrowIfNull<GarnerContractTemplate>(_dbContext, ErrorCode.GarnerContractTemplateNotFound);
            var contractTemplateTemp = _garnerContractTemplateTempEFRepository.FindById(input.ContractTemplateTempId, tradingProviderId).ThrowIfNull<GarnerContractTemplateTemp>(_dbContext, ErrorCode.GarnerContractTemplateTempNotFound);
            var configContractCode = _garnerConfigContractCodeEFRepository.FindByConfigContractCodeId(input.ConfigContractId, tradingProviderId).ThrowIfNull<GarnerConfigContractCode>(_dbContext, ErrorCode.GarnerConfigContractCodeNotFound);

            //Lịch sử thay đổi
            #region Lịch sử thay đổi
            //Kiểu hợp đồng
            if (contractTemplate.ContractSource != input.ContractSource)
            {
                string oldValue = contractTemplate.ContractSource switch
                {
                    SourceOrder.ONLINE => oldValue = SourceOrderText.ONLINE,
                    SourceOrder.OFFLINE => oldValue = SourceOrderText.OFFLINE,
                    SourceOrder.ALL => oldValue = SourceOrderText.ALL,
                    _ => "",
                };
                string newValue = contractTemplate.ContractSource switch
                {
                    SourceOrder.ONLINE => newValue = SourceOrderText.ONLINE,
                    SourceOrder.OFFLINE => newValue = SourceOrderText.OFFLINE,
                    SourceOrder.ALL => newValue = SourceOrderText.ALL,
                    _ => "",
                };

                _garnerHistoryUpdateEFRepository.Add(new GarnerHistoryUpdate()
                {
                    UpdateTable = GarnerHistoryUpdateTables.GAN_CONTRACT_TEMPLATE,
                    RealTableId = contractTemplate.Id,
                    Action = ActionTypes.CAP_NHAT,
                    OldValue = oldValue,
                    NewValue = newValue,
                    FieldName = GarnerFieldName.UPDATE_CONTRACT_SOURCE,
                    Summary = GarnerHistoryUpdateSummary.SUMMARY_UPDATE_CONTRACT_SOURCE
                }, username);
            }

            //Hợp đồng
            if (contractTemplate.ContractTemplateTempId != input.ContractTemplateTempId)
            {
                var oldContractTemplateTemp = _garnerContractTemplateTempEFRepository.FindById(contractTemplate.ContractTemplateTempId, tradingProviderId).ThrowIfNull<GarnerContractTemplateTemp>(_dbContext, ErrorCode.GarnerContractTemplateTempNotFound);

                _garnerHistoryUpdateEFRepository.Add(new GarnerHistoryUpdate()
                {
                    UpdateTable = GarnerHistoryUpdateTables.GAN_CONTRACT_TEMPLATE,
                    RealTableId = contractTemplate.Id,
                    Action = ActionTypes.CAP_NHAT,
                    OldValue = oldContractTemplateTemp.Name,
                    NewValue = contractTemplateTemp.Name,
                    FieldName = GarnerFieldName.UPDATE_CONTRACT_TEMPLATE_TEMP_ID,
                    Summary = GarnerHistoryUpdateSummary.SUMMARY_UPDATE_CONTRACT_TEMPLATE_TEMP
                }, username);
            }

            //Chính sách
            if (contractTemplate.PolicyId != input.PolicyId)
            {
                var oldPolicy = _garnerPolicyEFRepository.FindById(contractTemplate.PolicyId, tradingProviderId).ThrowIfNull(_dbContext, ErrorCode.GarnerPolicyNotFound);
                var newPolicy = _garnerPolicyEFRepository.FindById(input.PolicyId, tradingProviderId).ThrowIfNull(_dbContext, ErrorCode.GarnerPolicyNotFound);

                _garnerHistoryUpdateEFRepository.Add(new GarnerHistoryUpdate()
                {
                    UpdateTable = GarnerHistoryUpdateTables.GAN_CONTRACT_TEMPLATE,
                    RealTableId = contractTemplate.Id,
                    Action = ActionTypes.CAP_NHAT,
                    OldValue = oldPolicy.Name,
                    NewValue = oldPolicy.Name,
                    FieldName = GarnerFieldName.UPDATE_POLICY_ID,
                    Summary = GarnerHistoryUpdateSummary.SUMMARY_UPDATE_POLICY_ID
                }, username);
            }

            //Cấu trúc mã
            if (contractTemplate.ConfigContractId != input.ConfigContractId)
            {
                var oldConfigContractCode = _garnerConfigContractCodeEFRepository.FindByConfigContractCodeId(contractTemplate.ConfigContractId, tradingProviderId).ThrowIfNull<GarnerConfigContractCode>(_dbContext, ErrorCode.GarnerConfigContractCodeNotFound);

                _garnerHistoryUpdateEFRepository.Add(new GarnerHistoryUpdate()
                {
                    UpdateTable = GarnerHistoryUpdateTables.GAN_CONTRACT_TEMPLATE,
                    RealTableId = contractTemplate.Id,
                    Action = ActionTypes.CAP_NHAT,
                    OldValue = oldConfigContractCode.Name,
                    NewValue = configContractCode.Name,
                    FieldName = GarnerFieldName.UPDATE_CONFIG_CONTRACT_ID,
                    Summary = GarnerHistoryUpdateSummary.SUMMARY_UPDATE_CONFIG_CONTRACT_CODE
                }, username);
            }

            //Loại hiển thị
            if (contractTemplate.DisplayType != input.DisplayType)
            {

                string oldValue = contractTemplate.DisplayType switch
                {
                    ContractTemplateDisplayType.AFTER => oldValue = ContractTemplateDisplayTypeText.AFTER,
                    ContractTemplateDisplayType.BEFORE => oldValue = ContractTemplateDisplayTypeText.BEFORE,
                    _ => "",
                };

                string newValue = input.DisplayType switch
                {
                    ContractTemplateDisplayType.AFTER => newValue = ContractTemplateDisplayTypeText.AFTER,
                    ContractTemplateDisplayType.BEFORE => newValue = ContractTemplateDisplayTypeText.BEFORE,
                    _ => "",
                };

                _garnerHistoryUpdateEFRepository.Add(new GarnerHistoryUpdate()
                {
                    UpdateTable = GarnerHistoryUpdateTables.GAN_CONTRACT_TEMPLATE,
                    RealTableId = contractTemplate.Id,
                    Action = ActionTypes.CAP_NHAT,
                    OldValue = oldValue,
                    NewValue = newValue,
                    FieldName = GarnerFieldName.UPDATE_DISPLAY_TYPE,
                    Summary = GarnerHistoryUpdateSummary.SUMMARY_UPDATE_DISPLAY_TYPE
                }, username);
            }

            //Ngày hiệu lực
            if (contractTemplate.StartDate != input.StartDate)
            {
                
                _garnerHistoryUpdateEFRepository.Add(new GarnerHistoryUpdate()
                {
                    UpdateTable = GarnerHistoryUpdateTables.GAN_CONTRACT_TEMPLATE,
                    RealTableId = contractTemplate.Id,
                    Action = ActionTypes.CAP_NHAT,
                    OldValue = contractTemplate.StartDate.ToString(),
                    NewValue = input.StartDate.ToString(),
                    FieldName = GarnerFieldName.UPDATE_START_DATE,
                    Summary = GarnerHistoryUpdateSummary.SUMMARY_UPDATE_CONTRACT_TEMPLATE_START_DATE
                }, username);
            }
            #endregion

            var updateContractTepmlate = _garnerContractTemplateEFRepository.Update(_mapper.Map<GarnerContractTemplate>(input), tradingProviderId, username);

            _dbContext.SaveChanges();
            return updateContractTepmlate;
        }

        /// <summary>
        /// Đổi trạng thái mẫu hợp đồng
        /// </summary>
        public void ChangeStatus(int id)
        {
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            _logger.LogInformation($"{nameof(ChangeStatus)}: id={id}, tradingProviderId = {tradingProviderId}");
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            var contractTemplate = _garnerContractTemplateEFRepository.FindById(id, tradingProviderId).ThrowIfNull<GarnerContractTemplate>(_dbContext, ErrorCode.GarnerContractTemplateNotFound);

            string oldValue= contractTemplate.Status;

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

            string newValue = contractTemplate.Status;

            //Lịch sử thay đổi
            _garnerHistoryUpdateEFRepository.Add(new GarnerHistoryUpdate()
            {
                UpdateTable = GarnerHistoryUpdateTables.GAN_CONTRACT_TEMPLATE,
                RealTableId = id,
                OldValue = oldValue,
                NewValue = newValue,
                FieldName = GarnerFieldName.UPDATE_STATUS,
                Action = ActionTypes.CAP_NHAT,
                Summary = GarnerHistoryUpdateSummary.SUMMARY_UPDATE_CONTRACT_TEMPLATE_STATUS
            }, username);

            _dbContext.SaveChanges();
        }

        /// <summary>
        /// Find All Contract Template cho App
        /// </summary>
        /// <param name="policyId"></param>
        /// <returns></returns>
        /// <exception cref="FaultException"></exception>
        public List<GarnerContractTemplateAppDto> FindAllForApp(int policyId, int contractType)
        {
            _logger.LogInformation($"{nameof(FindAllForApp)}: policyId = {policyId}, contractType = {contractType}");

            List<GarnerContractTemplateAppDto> contractTemplates = new();
            var policy = _garnerPolicyEFRepository.FindById(policyId).ThrowIfNull(_dbContext, ErrorCode.GarnerPolicyNotFound);

            var contractTemplate = _garnerContractTemplateEFRepository.FindAllForUpdateContractFile(policyId, SharedContractTemplateType.INVESTOR, null, contractType, null, SourceOrder.ONLINE, Status.ACTIVE);

            return _mapper.Map<List<GarnerContractTemplateAppDto>>(contractTemplate);
        }



        #region Xử lý liên quan đến hợp đồng, đặt lệnh

        /// <summary>
        /// Tìm kiếm tất cả mẫu hợp đồng, kèm hợp đồng theo id lệnh (dùng để ký điện tử)
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="tradingProviderId"></param>
        /// <returns></returns>
        public List<ViewContractTemplateByOrder> FindAllByOrder(long orderId, int? tradingProviderId = null)
        {
            _logger.LogInformation($"{nameof(FindAllByOrder)}: orderId = {orderId}, tradingProviderId = {tradingProviderId}");

            tradingProviderId = tradingProviderId ?? CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var order = _garnerOrderEFRepository.FindById(orderId).ThrowIfNull(_dbContext, ErrorCode.GarnerOrderNotFound);
            var policy = _garnerPolicyEFRepository.FindById(order.PolicyId, tradingProviderId).ThrowIfNull(_dbContext, ErrorCode.GarnerPolicyNotFound);
            var cifCode = _cifCodeEFRepository.FindByCifCode(order.CifCode).ThrowIfNull(_dbContext, ErrorCode.CoreCifCodeNotFound);
            string contractTemplateType = SharedContractTemplateType.BUSINESS_CUSTOMER;
            if (cifCode.InvestorId != null)
            {
                contractTemplateType = SharedContractTemplateType.INVESTOR;
            }
            var contractTemplates = _garnerContractTemplateEFRepository.FindAllForUpdateContractFile(order.PolicyId, contractTemplateType, tradingProviderId, null, null, order.Source, Status.ACTIVE);
            if (contractTemplates.Count < 1)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy mẫu hợp đồng"), new FaultCode(((int)ErrorCode.GarnerContractTemplateNotFound).ToString()), "");
            }
            var result = new List<ViewContractTemplateByOrder>();
            foreach (var contractTemplate in contractTemplates)
            {
                var orderContractFiles = _garnerOrderContractFileEFRepository.FindByContractTemplateAndOrder(orderId, contractTemplate.Id, tradingProviderId);
                if (orderContractFiles.Count > 0)
                {
                    foreach (var orderContractFile in orderContractFiles)
                    {
                        var templateContract = new ViewContractTemplateByOrder();
                        //đã có file scan thì vẫn hiện mẫu hợp đồng dù đã xóa
                        templateContract.Name = contractTemplate.Name;
                        templateContract.ContractType = contractTemplate.ContractType;
                        templateContract.ContractTempUrl = contractTemplate.ContractTemplateUrl;
                        templateContract.DisplayType = contractTemplate.DisplayType;
                        templateContract.TradingProviderId = contractTemplate.TradingProviderId;
                        templateContract.Id = contractTemplate.Id;
                        templateContract.Status = contractTemplate.Status;
                        templateContract.OrderContractFileId = orderContractFile.Id;
                        templateContract.FileTempUrl = orderContractFile.FileTempUrl;
                        templateContract.FileSignatureUrl = orderContractFile.FileSignatureUrl;
                        templateContract.FileScanUrl = orderContractFile.FileScanUrl;
                        templateContract.IsSign = orderContractFile.IsSign;
                        templateContract.WithdrawalId = orderContractFile.WithdrawalId;
                        result.Add(templateContract);
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// Tìm mẫu hợp đồng, kèm hợp đồng theo id lệnh khi đặt lệnh
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="contractType">Hợp đồng rút tiền hay đặt lệnh</param>
        /// <param name="tradingProviderId"></param>
        /// <returns></returns>
        public List<ViewContractTemplateByOrder> FindAllByOrderAdd(long orderId, int? contractType = null, int? tradingProviderId = null)
        {
            _logger.LogInformation($"{System.Reflection.MethodBase.GetCurrentMethod().Name}: orderId = {orderId}, contractType = {contractType}, tradingProviderId = {tradingProviderId}");

            tradingProviderId = tradingProviderId ?? CommonUtils.GetCurrentTradingProviderId(_httpContext);

            var order = _garnerOrderEFRepository.FindById(orderId).ThrowIfNull(_dbContext, ErrorCode.GarnerOrderNotFound);
            var cifCode = _cifCodeEFRepository.FindByCifCode(order.CifCode).ThrowIfNull(_dbContext, ErrorCode.CoreCifCodeNotFound);
            string contractTemplateType = SharedContractTemplateType.BUSINESS_CUSTOMER;
            if (cifCode.InvestorId != null)
            {
                contractTemplateType = SharedContractTemplateType.INVESTOR;
            }
            var contractTemplates = _garnerContractTemplateEFRepository.FindAllForUpdateContractFile(order.PolicyId, contractTemplateType, tradingProviderId, contractType, null, order.Source, Status.ACTIVE);
            if (contractTemplates.Count < 1)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy mẫu hợp đồng"), new FaultCode(((int)ErrorCode.GarnerContractTemplateNotFound).ToString()), "");
            }
            var result = new List<ViewContractTemplateByOrder>();
            foreach (var contractTemplate in contractTemplates)
            {
                var templateContract = new ViewContractTemplateByOrder();
                var orderContractFile = _garnerOrderContractFileEFRepository.FindByContractTemplateAndOrderAdd(orderId, contractTemplate.Id, tradingProviderId);
                if (orderContractFile != null)
                {
                    //đã có file scan thì vẫn hiện mẫu hợp đồng dù đã xóa
                    templateContract.Name = contractTemplate.Name;
                    templateContract.DisplayType = contractTemplate.DisplayType;
                    templateContract.TradingProviderId = contractTemplate.TradingProviderId;
                    templateContract.Id = contractTemplate.Id;
                    templateContract.Status = contractTemplate.Status;
                    templateContract.OrderContractFileId = orderContractFile.Id;
                    templateContract.FileTempUrl = orderContractFile.FileTempUrl;
                    templateContract.FileSignatureUrl = orderContractFile.FileSignatureUrl;
                    templateContract.FileScanUrl = orderContractFile.FileScanUrl;
                    templateContract.IsSign = orderContractFile.IsSign;
                    result.Add(templateContract);
                } 

            }
            return result;
        }

        /// <summary>
        /// Tìm kiếm mẫu hợp đồng Theo Id lệnh (List dành cho View sHSKH sổ lệnh)
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="tradingProviderId"></param>
        /// <returns></returns>
        public List<ViewContractTemplateByOrder> FindAllByOrderCheckDisplayType(long orderId)
        {
            int? tradingProviderId = null;
            var userType = CommonUtils.GetCurrentUserType(_httpContext);
            int? partnerId = null;

            if (userType == UserTypes.TRADING_PROVIDER && userType == UserTypes.ROOT_TRADING_PROVIDER)
            {
                tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            }
            else if (userType == UserTypes.PARTNER || userType == UserTypes.ROOT_PARTNER)
            {
                partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
            }
            _logger.LogInformation($"{nameof(FindAllByOrderCheckDisplayType)}: orderId = {orderId}, tradingProviderId = {tradingProviderId}");


            //tradingProviderId = tradingProviderId ?? CommonUtils.GetCurrentTradingProviderId(_httpContext);

            var order = _garnerOrderEFRepository.FindById(orderId, tradingProviderId, partnerId).ThrowIfNull(_dbContext, ErrorCode.GarnerOrderNotFound);
            var policy = _garnerPolicyEFRepository.FindById(order.PolicyId, tradingProviderId).ThrowIfNull(_dbContext, ErrorCode.GarnerPolicyNotFound);
            var cifCode = _cifCodeEFRepository.FindByCifCode(order.CifCode).ThrowIfNull(_dbContext, ErrorCode.CoreCifCodeNotFound);
            string contractTemplateType = SharedContractTemplateType.BUSINESS_CUSTOMER;
            if (cifCode.InvestorId != null)
            {
                contractTemplateType = SharedContractTemplateType.INVESTOR;
            }
            string displayType = null;
            if (order.Status != OrderStatus.DANG_DAU_TU)
            {
                displayType = DisplayType.TRUOC_KHI_DUYET;
            }
            var contractTemplates = _garnerContractTemplateEFRepository.FindAllForUpdateContractFile(order.PolicyId, contractTemplateType,tradingProviderId, null, displayType, order.Source);

            if (contractTemplates.Count < 1)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy mẫu hợp đồng"), new FaultCode(((int)ErrorCode.GarnerContractTemplateNotFound).ToString()), "");
            }
            var result = new List<ViewContractTemplateByOrder>();
            foreach (var contractTemplate in contractTemplates)
            {
                var orderContractFiles = _garnerOrderContractFileEFRepository.FindByContractTemplateAndOrder(orderId, contractTemplate.Id, tradingProviderId);
                if (orderContractFiles?.Count != 0)
                {
                    foreach (var orderContractFile in orderContractFiles)
                    {
                        var templateContract = new ViewContractTemplateByOrder();
                        //đã có file hợp đồng thì vẫn hiện mẫu hợp đồng dù đã xóa
                        templateContract.Name = contractTemplate.Name;
                        templateContract.ContractType = contractTemplate.ContractType;
                        templateContract.DisplayType = contractTemplate.DisplayType;
                        templateContract.TradingProviderId = contractTemplate.TradingProviderId;
                        templateContract.Id = contractTemplate.Id;
                        templateContract.Status = contractTemplate.Status;
                        templateContract.OrderContractFileId = orderContractFile.Id;
                        templateContract.FileTempUrl = orderContractFile.FileTempUrl;
                        templateContract.FileSignatureUrl = orderContractFile.FileSignatureUrl;
                        templateContract.FileScanUrl = orderContractFile.FileScanUrl;
                        templateContract.IsSign = orderContractFile.IsSign;
                        templateContract.WithdrawalId = orderContractFile.WithdrawalId;              
                        result.Add(templateContract);
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// Lấy danh sách file hợp đồng đã ký cho app
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public List<ViewContractForApp> FindAllFileSignature(long orderId)
        {
            _logger.LogInformation($"{nameof(FindAllFileSignature)}: orderId = {orderId}");
            var order = _garnerOrderEFRepository.FindById(orderId).ThrowIfNull(_dbContext, ErrorCode.GarnerOrderNotFound);
            var orderContractFiles = _garnerOrderContractFileEFRepository.FindAll(orderId, order.TradingProviderId);
            List<ViewContractForApp> result = new();
            foreach (var contract in orderContractFiles)
            {
                if (contract.FileSignatureUrl != null)
                {
                    var fileSignatures = new ViewContractForApp();
                    var template = _garnerContractTemplateEFRepository.FindByIdForUpdateContractFile(contract.ContractTempId, SharedContractTemplateType.INVESTOR, order.TradingProviderId);
                    if (template != null)
                    {
                        fileSignatures.Name = template.Name;
                        fileSignatures.ContractTempId = template.Id;
                        fileSignatures.TradingProviderId = template.TradingProviderId;
                        fileSignatures.OrderContractFileId = contract.Id;
                        fileSignatures.IsSign = contract.IsSign;
                        result.Add(fileSignatures);
                    } 
                }
            }
            return result;
        }
        #endregion

    }
}
