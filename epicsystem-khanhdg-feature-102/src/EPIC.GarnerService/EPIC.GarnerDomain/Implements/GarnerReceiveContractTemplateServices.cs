using AutoMapper;
using DocumentFormat.OpenXml.Office2010.Excel;
using EPIC.DataAccess.Base;
using EPIC.DataAccess.Models;
using EPIC.Entities;
using EPIC.GarnerDomain.Interfaces;
using EPIC.GarnerEntities.DataEntities;
using EPIC.GarnerEntities.Dto.GarnerContractTemplateTemp;
using EPIC.GarnerEntities.Dto.GarnerReceiveContractTemp;
using EPIC.GarnerRepositories;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Garner;
using EPIC.Utils.ConstantVariables.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.GarnerDomain.Implements
{
    public class GarnerReceiveContractTemplateServices : IGarnerReceiveContractTemplateServices
    {

        private readonly EpicSchemaDbContext _dbContext;
        private readonly ILogger<GarnerReceiveContractTemplateServices> _logger;
        private readonly string _connectionString;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IMapper _mapper;

        private readonly GarnerReceiveContractTemplateEFRepository _garnerReceiverContractTempEFRepository;
        private readonly GarnerHistoryUpdateEFRepository _garnerHistoryUpdateEFRepository;

        public GarnerReceiveContractTemplateServices(EpicSchemaDbContext dbContext,
            DatabaseOptions databaseOptions,
            IMapper mapper,
            ILogger<GarnerReceiveContractTemplateServices> logger,
            IHttpContextAccessor httpContextAccessor)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
            _connectionString = databaseOptions.ConnectionString;
            _httpContext = httpContextAccessor;
            _garnerReceiverContractTempEFRepository = new GarnerReceiveContractTemplateEFRepository(dbContext, logger);
            _garnerHistoryUpdateEFRepository = new GarnerHistoryUpdateEFRepository(dbContext, logger);
        }

        /// <summary>
        /// Thêm hợp đồng mẫu
        /// </summary>
        /// <param name="input"></param>
        public void Add(CreateGarnerReceiveContractTempDto input)
        {
            _logger.LogInformation($"{nameof(GarnerReceiveContractTemplateEFRepository)}->{nameof(Add)}: input = {input}");

            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var username = CommonUtils.GetCurrentUsername(_httpContext);

            var receiveContractTempList = _garnerReceiverContractTempEFRepository.FindAllByDistributionId(input.DistributionId, tradingProviderId);
            
            var receiveContractTemp = new GarnerReceiveContractTemp()
            {
                Code = input.Code,
                Name = input.Name,
                DistributionId = input.DistributionId,
                FileUrl = input.FileUrl,
                Status = receiveContractTempList.Count() == 0 ? ContractTemplateStatus.ACTIVE : ContractTemplateStatus.DEACTIVE,
                TradingProviderId = tradingProviderId,
            };
            var result = _garnerReceiverContractTempEFRepository.Add(receiveContractTemp, username);

            //Lịch sử thay đổi
            _garnerHistoryUpdateEFRepository.Add(new GarnerHistoryUpdate()
            {
                UpdateTable = GarnerHistoryUpdateTables.GAN_RECEIVE_CONTRACT_TEMP,
                RealTableId = result.Id,
                Action = ActionTypes.THEM_MOI,
                Summary = GarnerHistoryUpdateSummary.SUMMARY_ADD_RECEIVE_CONTRACT
            }, username);

            _dbContext.SaveChanges();
        }

        /// <summary>
        /// Thay đổi status của mẫu giao nhận hợp đồng
        /// </summary>
        /// <param name="id"></param>
        public void ChangeStatus(int id)
        {
            _logger.LogInformation($"{nameof(GarnerReceiveContractTemplateEFRepository)}->{nameof(ChangeStatus)}: id = {id}");
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            var contractTemplate = _garnerReceiverContractTempEFRepository.FindById(id, tradingProviderId).ThrowIfNull<GarnerReceiveContractTemp>(_dbContext, ErrorCode.GarnerReceiveContractTemplateNotFound);

            var receiverContractTempList = _garnerReceiverContractTempEFRepository.FindAllByDistributionId(contractTemplate.DistributionId, tradingProviderId);

            var result = receiverContractTempList.Where(r => r.Id != id).ToList();

            foreach (var item in result)
            {
                item.Status = ContractTemplateStatus.DEACTIVE;
            }

            var status = ContractTemplateStatus.ACTIVE;

            //Lịch sử thay đổi
            _garnerHistoryUpdateEFRepository.Add(new GarnerHistoryUpdate()
            {
                UpdateTable = GarnerHistoryUpdateTables.GAN_RECEIVE_CONTRACT_TEMP,
                RealTableId = id,
                OldValue = StatusText.DEACTIVE,
                NewValue = StatusText.ACTIVE,
                FieldName = GarnerFieldName.UPDATE_STATUS,
                Action = ActionTypes.CAP_NHAT,
                Summary = GarnerHistoryUpdateSummary.SUMMARY_UPDATE_STATUS_RECEIVE_CONTRACT
            }, username);

            _garnerReceiverContractTempEFRepository.ChangeStatus(id, status, username);
            _dbContext.SaveChanges();
        }

        /// <summary>
        /// Cập nhập mẫu giao nhận hợp đồng 
        /// </summary>
        /// <param name="input"></param>
        public void Update(UpdateGarnerReceiveContractTempDto input)
        {
            _logger.LogInformation($"{nameof(GarnerReceiveContractTemplateEFRepository)}->{nameof(Update)}: input = {input}");
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var username = CommonUtils.GetCurrentUsername(_httpContext);

            var contractTemplate = _garnerReceiverContractTempEFRepository.FindById(input.Id, tradingProviderId).ThrowIfNull<GarnerReceiveContractTemp>(_dbContext, ErrorCode.GarnerReceiveContractTemplateNotFound);

            #region  Lịch sử thay đổi 
            //Mã hợp đồng giao nhận
            if (contractTemplate.Code != input.Code)
            {
                _garnerHistoryUpdateEFRepository.Add(new GarnerHistoryUpdate()
                {
                    UpdateTable = GarnerHistoryUpdateTables.GAN_RECEIVE_CONTRACT_TEMP,
                    RealTableId = contractTemplate.Id,
                    OldValue = contractTemplate.Code,
                    NewValue = input.Code,
                    FieldName = GarnerFieldName.UPDATE_CODE,
                    Action = ActionTypes.CAP_NHAT,
                    Summary = GarnerHistoryUpdateSummary.SUMMARY_UPDATE_CODE_RECEIVE_CONTRACT
                }, username);
            }

            //Tên hợp đồng giao nhận
            if (contractTemplate.Name != input.Name)
            {
                _garnerHistoryUpdateEFRepository.Add(new GarnerHistoryUpdate()
                {
                    UpdateTable = GarnerHistoryUpdateTables.GAN_RECEIVE_CONTRACT_TEMP,
                    RealTableId = contractTemplate.Id,
                    OldValue = contractTemplate.Name,
                    NewValue = input.Name,
                    FieldName = GarnerFieldName.UPDATE_NAME,
                    Action = ActionTypes.CAP_NHAT,
                    Summary = GarnerHistoryUpdateSummary.SUMMARY_UPDATE_NAME_RECEIVE_CONTRACT
                }, username);
            }

            //File hợp đồng giao nhận
            if (contractTemplate.FileUrl != input.FileUrl)
            {
                _garnerHistoryUpdateEFRepository.Add(new GarnerHistoryUpdate()
                {
                    UpdateTable = GarnerHistoryUpdateTables.GAN_RECEIVE_CONTRACT_TEMP,
                    RealTableId = contractTemplate.Id,
                    OldValue = contractTemplate.Name,
                    NewValue = input.Name,
                    FieldName = GarnerFieldName.UPDATE_FILE_URL_CONTRACT,
                    Action = ActionTypes.CAP_NHAT,
                    Summary = GarnerHistoryUpdateSummary.SUMMARY_UPDATE_FILE_RECEIVE_CONTRACT
                }, username);
            }
            #endregion

            contractTemplate.Code = input.Code;
            contractTemplate.Name = input.Name;
            contractTemplate.FileUrl = input.FileUrl;
            contractTemplate.ModifiedBy = username;
            _garnerReceiverContractTempEFRepository.Update(contractTemplate, username);


            
            _dbContext.SaveChanges();
        }

        /// <summary>
        /// Lấy thông tin của mẫu giao nhận hợp đồng
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public GarnerReceiveContractTemp FindById(int id)
        {
            _logger.LogInformation($"{nameof(GarnerReceiveContractTemplateEFRepository)}->{nameof(FindById)}: id = {id}");
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            return _garnerReceiverContractTempEFRepository.FindById(id, tradingProviderId);
        }

        /// <summary>
        /// Lấy mẫu giao nhận hợp đồng theo distribution id có phân trang
        /// </summary>
        /// <param name="distributionId"></param>
        /// <returns></returns>
        public PagingResult<GarnerReceiveContractTemp> FindByDistributionId(FilterGarnerReceiveContractTemplateDto input)
        {
            _logger.LogInformation($"{nameof(GarnerReceiveContractTemplateEFRepository)}->{nameof(FindByDistributionId)}: input = {input}");
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            return _garnerReceiverContractTempEFRepository.FindByDistributionId(input, tradingProviderId);
        }

        /// <summary>
        /// Xóa mẫu giao nhận giao nhận hợp đồng
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int id)
        {
            _logger.LogInformation($"{nameof(GarnerReceiveContractTemplateEFRepository)}->{nameof(Delete)}: id = {id}");
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var receiveContractTemplateFind = _garnerReceiverContractTempEFRepository.FindDetectiveById(id, tradingProviderId).ThrowIfNull<GarnerReceiveContractTemp>(_dbContext, ErrorCode.GarnerReceiveContractTemplateNotFound);
            _garnerReceiverContractTempEFRepository.Delete(id);

            //Lịch sử thay đổi
            _garnerHistoryUpdateEFRepository.Add(new GarnerHistoryUpdate()
            {
                UpdateTable = GarnerHistoryUpdateTables.GAN_RECEIVE_CONTRACT_TEMP,
                RealTableId = id,
                Action = ActionTypes.XOA,
                Summary = GarnerHistoryUpdateSummary.SUMMARY_DELETE_RECEIVE_CONTRACT
            }, username);

            _dbContext.SaveChanges();
        }
    }
}
