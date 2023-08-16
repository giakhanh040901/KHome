using EPIC.DataAccess.Base;
using EPIC.DataAccess.Models;
using EPIC.Entities;
using EPIC.InvestDomain.Interfaces;
using EPIC.InvestEntities.DataEntities;
using EPIC.InvestEntities.Dto.ReceiveContractTemplate;
using EPIC.InvestRepositories;
using EPIC.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.InvestDomain.Implements
{
    public class ReceiveContractTemplateServices : IReceiveContractTemplateServices
    {
        //private readonly EpicSchemaDbContext _dbContext;
        private ILogger<ReceiveContractTemplate> _logger;
        private IConfiguration _configuration;
        private string _connectionString;
        private ReceiveContractTemplateRepository _receiveContractTemplateRepository;
        private IHttpContextAccessor _httpContext;

        public ReceiveContractTemplateServices(ILogger<ReceiveContractTemplate> logger, IConfiguration configuration, DatabaseOptions databaseOptions, IHttpContextAccessor httpContext)
        {
            _logger = logger;
            _configuration = configuration;
            _connectionString = databaseOptions.ConnectionString;
            _receiveContractTemplateRepository = new ReceiveContractTemplateRepository(_connectionString, _logger);
            _httpContext = httpContext;
        }

        public ReceiveContractTemplate Add(CreateReceiveContractTemplateDto input)
        {
            _logger.LogInformation($"{nameof(ReceiveContractTemplate)} => {nameof(Add)}: input = {input}");
            var tradingProvider = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var username = CommonUtils.GetCurrentUsername(_httpContext);

            var receiveContractTempList = _receiveContractTemplateRepository.GetAllByDistributionId(input.DistributionId, tradingProvider);

            var contractTemplate = new ReceiveContractTemplate
            {
                Code = input.Code,
                Name = input.Name,
                TradingProviderId = tradingProvider,
                FileUrl = input.FileUrl,
                DistributionId = input.DistributionId,
                CreatedBy = username,
                Status = receiveContractTempList.Count() == 0 ? ContractTemplateStatus.ACTIVE : ContractTemplateStatus.DEACTIVE,
            };
            return _receiveContractTemplateRepository.Add(contractTemplate);
        }

        public int Delete(int id)
        {
            _logger.LogInformation($"{nameof(ReceiveContractTemplate)} => {nameof(Delete)}: id = {id}");
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            return _receiveContractTemplateRepository.Delete(id);
        }

        public ReceiveContractTemplate FindById(int id)
        {
            _logger.LogInformation($"{nameof(ReceiveContractTemplate)} => {nameof(FindById)}: id = {id}");
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var result = _receiveContractTemplateRepository.FindById(id, tradingProviderId);
            return result;
        }

        public int Update(UpdateReceiveContractTemplateDto input)
        {
            _logger.LogInformation($"{nameof(ReceiveContractTemplate)} => {nameof(Update)}: input = {input}");
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            var contractTemplate = _receiveContractTemplateRepository.FindById(input.Id, tradingProviderId);
            contractTemplate.Code = input.Code;
            contractTemplate.Name = input.Name;
            contractTemplate.FileUrl = input.FileUrl;
            contractTemplate.ModifiedBy = username;
            return _receiveContractTemplateRepository.Update(contractTemplate);
        }

        public int ChangeStatus(int id)
        {
            _logger.LogInformation($"{nameof(ReceiveContractTemplate)} => {nameof(ChangeStatus)}: id = {id}");
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var contractTemplate = _receiveContractTemplateRepository.FindById(id, tradingProviderId);
            var receiverContractTempList = _receiveContractTemplateRepository.GetAllByDistributionId(contractTemplate.DistributionId, tradingProviderId);
            var result = receiverContractTempList.Where(r => r.Id != id).ToList();
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            foreach (var item in result)
            {
                item.Status = ContractTemplateStatus.DEACTIVE;
                _receiveContractTemplateRepository.UpdateStatus(item.Id, ContractTemplateStatus.DEACTIVE, username);
            }

            var status = ContractTemplateStatus.ACTIVE;
            return _receiveContractTemplateRepository.UpdateStatus(id, status, username);
        }

        public ReceiveContractTemplate FindByDistributionId(int distributionId)
        {
            _logger.LogInformation($"{nameof(ReceiveContractTemplate)} => {nameof(FindByDistributionId)}: distributionId = {distributionId}");
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            return _receiveContractTemplateRepository.FindAll(distributionId, tradingProviderId);
        }

        /// <summary>
        /// Tìm danh sách hợp đồng có phân trang
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageNumber"></param>
        /// <param name="keyword"></param>
        /// <param name="distributionId"></param>
        /// <returns></returns>
        public PagingResult<ReceiveContractTemplate> GetAll(int pageSize, int pageNumber, string keyword, int distributionId)
        {
            _logger.LogInformation($"{nameof(ReceiveContractTemplate)}->{nameof(GetAll)}: distributionId = {distributionId}");
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var receiveContractTemplateList = _receiveContractTemplateRepository.GetAll(tradingProviderId, pageSize, pageNumber, keyword, distributionId);
            var result = new PagingResult<ReceiveContractTemplate>();
            result.TotalItems = receiveContractTemplateList.TotalItems;
            //foreach (var blockadeLiberationFind in blockadeLiberationList.Items)
            //{
            //    try
            //    {
            //        var blockadeLiberationItem = _mapper.Map<BlockadeLiberationDto>(blockadeLiberationFind);
            //        var orderFind = _orderRepository.FindById(blockadeLiberationFind.Id, tradingProviderId);
            //        if (orderFind == null)
            //        {
            //            throw new FaultException(new FaultReason($"Không tìm thấy thông tin sổ lệnh"), new FaultCode(((int)ErrorCode.NotFound).ToString()), "");
            //        }
            //        blockadeLiberationItem.ContractCode = orderFind.ContractCode;
            //        blockadeLiberationItem.TotalValue = orderFind.TotalValue;
            //        items.Add(blockadeLiberationItem);
            //    }
            //    catch (Exception ex)
            //    {
            //        _logger.LogError(ex, $"Không tìm thấy thông tin sổ lệnh có id: {blockadeLiberationFind.Id}");
            //    }

            //}
            result.Items = receiveContractTemplateList.Items;
            return result;
        }
    }
}
