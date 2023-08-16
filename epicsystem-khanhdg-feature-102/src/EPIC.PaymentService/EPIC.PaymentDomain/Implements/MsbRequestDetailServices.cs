using AutoMapper;
using EPIC.DataAccess.Base;
using EPIC.DataAccess.Models;
using EPIC.PaymentDomain.Interfaces;
using EPIC.PaymentEntities.DataEntities;
using EPIC.PaymentEntities.Dto.Msb;
using EPIC.PaymentEntities.Dto.MsbRequestPaymentDetail;
using EPIC.PaymentRepositories;
using EPIC.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace EPIC.PaymentDomain.Implements
{
    public class MsbRequestDetailServices : IMsbRequestDetailServices
    {
        private readonly EpicSchemaDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<MsbRequestDetailServices> _logger;
        private readonly IHttpContextAccessor _httpContext;
        private readonly MsbRequestPaymentDetailEFRepository _msbRequestPaymentDetailEFRepository;

        public MsbRequestDetailServices(EpicSchemaDbContext dbContext, IMapper mapper, ILogger<MsbRequestDetailServices> logger, IHttpContextAccessor httpContext)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
            _httpContext = httpContext;
            _msbRequestPaymentDetailEFRepository = new MsbRequestPaymentDetailEFRepository(_dbContext, _logger);
        }

        /// <summary>
        /// Thêm chi tiết chi hộ MSB
        /// </summary>
        /// <param name="input"></param>
        public void AddRequestDetail(CreateMsbRequestDetailDto input)
        {

            _logger.LogInformation($"{nameof(AddRequestDetail)}: input = {JsonSerializer.Serialize(input)}");
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var username = CommonUtils.GetCurrentUsername(_httpContext);

            var requestDetailInsert = _mapper.Map<MsbRequestPaymentDetail>(input);

            _msbRequestPaymentDetailEFRepository.Add(requestDetailInsert);
            _dbContext.SaveChanges();
        }

        /// <summary>
        /// Tìm theo Id chi tiết chi hộ
        /// </summary>
        /// <param name="requestDetailId"></param>
        /// <returns></returns>
        public MsbRequestDetailWithErrorDto FindById(long requestDetailId)
        {
            _logger.LogInformation($"{nameof(FindById)}: requestDetailId = {requestDetailId}");
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var username = CommonUtils.GetCurrentUsername(_httpContext);

            var getRequestDetail = _msbRequestPaymentDetailEFRepository.FindById(requestDetailId).ThrowIfNull(_dbContext, ErrorCode.NotFound);
            return _mapper.Map<MsbRequestDetailWithErrorDto>(getRequestDetail);
        }


        /// <summary>
        /// Update chi tiết chi hộ
        /// </summary>
        /// <param name="input"></param>
        public void UpdateRequestDetail(UpdateMsbRequestDetailDto input)
        {
            _logger.LogInformation($"{nameof(UpdateRequestDetail)}: input = {JsonSerializer.Serialize(input)}");
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var username = CommonUtils.GetCurrentUsername(_httpContext);

            var getRequestDetail = _msbRequestPaymentDetailEFRepository.FindById(input.Id).ThrowIfNull(_dbContext, ErrorCode.NotFound);

            var requestUpdate = _mapper.Map<MsbRequestPaymentDetail>(input);

            _msbRequestPaymentDetailEFRepository.Update(requestUpdate);
            _dbContext.SaveChanges();
        }

        /// <summary>
        /// Lấy danh sách lô chi
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public PagingResult<MsbCollectionPaymentDto> FindAllCollectionPayment(MsbCollectionPaymentFilterDto input)
        {
            _logger.LogInformation($"{nameof(FindAllCollectionPayment)}: input = {JsonSerializer.Serialize(input)}");
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var username = CommonUtils.GetCurrentUsername(_httpContext);

            var collectionPayments = _msbRequestPaymentDetailEFRepository.FindAllCollectPayment(input);

            return collectionPayments;
        }
    }
}
