using AutoMapper;
using EPIC.CoreRepositories;
using EPIC.DataAccess.Base;
using EPIC.Entities;
using EPIC.Entities.DataEntities;
using EPIC.GarnerDomain.Interfaces;
using EPIC.GarnerEntities.DataEntities;
using EPIC.GarnerEntities.Dto;
using EPIC.GarnerEntities.Dto.GarnerRating;
using EPIC.GarnerRepositories;
using EPIC.GarnerSharedDomain.Interfaces;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace EPIC.GarnerDomain.Implements
{
    public class GarnerRatingServices : IGarnerRatingServices
    {
        private readonly EpicSchemaDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<GarnerRatingServices> _logger;
        private readonly string _connectionString;
        private readonly IHttpContextAccessor _httpContext;
        private readonly DefErrorEFRepository _defErrorEFRepository;
        private readonly TradingProviderEFRepository _tradingProviderEFRepository;
        private readonly GarnerPolicyEFRepository _garnerPolicyEFRepository;
        private readonly GarnerOrderEFRepository _garnerOrderEFRepository;
        private readonly CifCodeEFRepository _cifCodeEFRepository;
        private readonly GarnerProductEFRepository _garnerProductEFRepository;
        private readonly IGarnerContractCodeServices _garnerContractCodeService;
        private readonly GarnerRatingEFRepository _garnerRatingEFRepository;
        private readonly GarnerOrderContractFileEFRepository _garnerOrderContractFileEFRepository;

        public GarnerRatingServices(EpicSchemaDbContext dbContext,
            DatabaseOptions databaseOptions,
            IMapper mapper,
            ILogger<GarnerRatingServices> logger,
            IHttpContextAccessor httpContextAccessor,
            IGarnerContractCodeServices garnerContractCodeServices)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
            _connectionString = databaseOptions.ConnectionString;
            _httpContext = httpContextAccessor;
            _defErrorEFRepository = new DefErrorEFRepository(dbContext);
            _tradingProviderEFRepository = new TradingProviderEFRepository(dbContext, logger);
            _garnerPolicyEFRepository = new GarnerPolicyEFRepository(dbContext, logger);
            _garnerOrderEFRepository = new GarnerOrderEFRepository(dbContext, logger);
            _cifCodeEFRepository = new CifCodeEFRepository(dbContext, logger);
            _garnerProductEFRepository = new GarnerProductEFRepository(_dbContext, _logger);
            _garnerContractCodeService = garnerContractCodeServices;
            _garnerRatingEFRepository = new GarnerRatingEFRepository(_dbContext, _logger);
            _garnerOrderContractFileEFRepository = new GarnerOrderContractFileEFRepository(_dbContext, _logger);
        }

        /// <summary>
        /// Tìm hợp đồng mới nhất để đánh giá
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public GarnerRatingDto FindLastOrder()
        {
            _logger.LogInformation($"{nameof(GarnerRatingServices)}->{nameof(FindLastOrder)}");

            var result = new GarnerRatingDto();
            var investorId = CommonUtils.GetCurrentInvestorId(_httpContext);
            var cifCode = _cifCodeEFRepository.FindByInvestor(investorId);
            if (cifCode == null)
            {
                _defErrorEFRepository.ThrowException(ErrorCode.CoreCifCodeNotFound);
            }
            var order = _garnerOrderEFRepository.FindByCifCode(cifCode.CifCode).Where(o => o.Status == OrderStatus.DANG_DAU_TU).OrderByDescending(o => o.Id).FirstOrDefault();
            if (order == null)
            {
                _defErrorEFRepository.ThrowException(ErrorCode.GarnerOrderNotFound);
            }
            var policy = _garnerPolicyEFRepository.FindById(order.PolicyId);
            if (policy == null)
            {
                _defErrorEFRepository.ThrowException(ErrorCode.GarnerPolicyNotFound);
            }
            var product = _garnerProductEFRepository.FindById(order.ProductId);
            if (product == null)
            {
                _defErrorEFRepository.ThrowException(ErrorCode.GarnerProductNotFound);
            }

            string contractCode = null;
            var contractCodeGen = _garnerOrderContractFileEFRepository.EntityNoTracking.Where(e => e.OrderId == order.Id && e.TradingProviderId == order.TradingProviderId && e.Deleted == YesNo.NO).Select(o => o.ContractCodeGen).Distinct();
            if (contractCodeGen.Count() == 1)
            {
                contractCode = contractCodeGen.First();
            }
            var rating = _garnerRatingEFRepository.Entity.FirstOrDefault(o => o.OrderId == order.Id);
            if (rating == null)
            {
                return new GarnerRatingDto()
                {
                    OrderId = order.Id,
                    ContractCode = contractCode ?? order?.ContractCode,
                };
            }

            return null;
        }

        /// <summary>
        /// Thêm đánh giá
        /// </summary>
        /// <param name="input"></param>
        public void Add(CreateGarnerRatingDto input)
        {
            _logger.LogInformation($"{nameof(GarnerRating)}->{nameof(Add)}: input = {JsonSerializer.Serialize(input)}");
            var investorId = CommonUtils.GetCurrentInvestorId(_httpContext);
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            var contractCode = FindLastOrder();

            if (contractCode == null)
            {
                _defErrorEFRepository.ThrowException("Hợp đồng đã được đánh giá");
            }

            var garnerRatingFind = _garnerRatingEFRepository.FindByOrderId(contractCode.OrderId);
            if (garnerRatingFind.Count() == 0)
            {
                var garnerRating = new GarnerRating()
                {
                    OrderId = contractCode.OrderId,
                    InvestorId = investorId,
                    Rate = input.Rate,
                    ProductExperience = input.ProductExperience,
                    Feedback = input.Feedback,
                    CreatedBy = username,
                    CreatedDate = DateTime.Now,
                };
                _garnerRatingEFRepository.Add(garnerRating);
            }
            _dbContext.SaveChanges();
        }
    }
}
