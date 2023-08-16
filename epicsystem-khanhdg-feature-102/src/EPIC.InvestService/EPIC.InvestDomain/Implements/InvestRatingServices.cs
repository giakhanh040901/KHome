using AutoMapper;
using EPIC.CoreRepositories;
using EPIC.DataAccess.Base;
using EPIC.Entities;
using EPIC.InvestDomain.Interfaces;
using EPIC.InvestEntities.DataEntities;
using EPIC.InvestEntities.Dto.InvestRating;
using EPIC.InvestRepositories;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace EPIC.InvestDomain.Implements
{
    public class InvestRatingServices : IInvestRatingServices
    {
        private readonly EpicSchemaDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<InvestRatingServices> _logger;
        private readonly string _connectionString;
        private readonly IHttpContextAccessor _httpContext;
        private readonly DefErrorEFRepository _defErrorEFRepository;
        private readonly InvestOrderEFRepository _investOrderEFRepository;
        private readonly CifCodeEFRepository _cifCodeEFRepository;
        private readonly InvestRatingEFRepository _investRatingEFRepository;
        private readonly InvestOrderContractFileEFRepository _investOrderContractFileEFRepository;

        public InvestRatingServices(EpicSchemaDbContext dbContext,
            DatabaseOptions databaseOptions,
            IMapper mapper,
            ILogger<InvestRatingServices> logger,
            IHttpContextAccessor httpContextAccessor)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
            _connectionString = databaseOptions.ConnectionString;
            _httpContext = httpContextAccessor;
            _defErrorEFRepository = new DefErrorEFRepository(dbContext);
            _investOrderEFRepository = new InvestOrderEFRepository(dbContext, logger);
            _cifCodeEFRepository = new CifCodeEFRepository(dbContext, logger);
            _investRatingEFRepository = new InvestRatingEFRepository(_dbContext, _logger);
            _investOrderContractFileEFRepository = new InvestOrderContractFileEFRepository(_dbContext, _logger);
        }

        public InvestRatingDto FindLastOrder()
        {
            _logger.LogInformation($"{nameof(FindLastOrder)}");
            var result = new InvestRatingDto();
            var investorId = CommonUtils.GetCurrentInvestorId(_httpContext);
            var cifCode = _cifCodeEFRepository.FindByInvestor(investorId);
            if (cifCode == null)
            {
                _defErrorEFRepository.ThrowException(ErrorCode.CoreCifCodeNotFound);
            }
            
            var order = _investOrderEFRepository.EntityNoTracking.Where(o => o.CifCode == cifCode.CifCode && o.Status == OrderStatus.DANG_DAU_TU && o.Deleted == YesNo.NO).OrderByDescending(o => o.Id).FirstOrDefault();
            if (order == null)
            {
                _defErrorEFRepository.ThrowException(ErrorCode.InvestOrderNotFound);
            }
            string contractCode = null;
            var contractCodeGen = _investOrderContractFileEFRepository.EntityNoTracking.Where(e => e.OrderId == order.Id && e.TradingProviderId == order.TradingProviderId && e.Deleted == YesNo.NO).Select(o => o.ContractCodeGen).Distinct();
            if (contractCodeGen.Count() == 1)
            {
                contractCode = contractCodeGen.First();
            }

            var rating = _investRatingEFRepository.Entity.FirstOrDefault(o => o.OrderId == order.Id && o.Deleted == YesNo.NO);
            if (rating == null)
            {
                return new InvestRatingDto()
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
        public void Add(CreateInvestRatingDto input)
        {
            _logger.LogInformation($"{nameof(Add)}: input = {JsonSerializer.Serialize(input)}");
            var investorId = CommonUtils.GetCurrentInvestorId(_httpContext);
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            var contractCode = FindLastOrder();

            if (contractCode == null)
            {
                _defErrorEFRepository.ThrowException("Hợp đồng đã được đánh giá");
            }
            var investRatingFind = _investRatingEFRepository.Entity.Where(o => o.OrderId == contractCode.OrderId && o.Deleted == YesNo.NO);
            if (investRatingFind.Count() == 0)
            {
                var investRating = new InvestRating()
                {
                    OrderId = contractCode.OrderId,
                    InvestorId = investorId,
                    Rate = input.Rate,
                    ProductExperience = input.ProductExperience,
                    Feedback = input.Feedback,
                    CreatedBy = username,
                    CreatedDate = DateTime.Now,
                };
                _investRatingEFRepository.Add(investRating);
            }
            _dbContext.SaveChanges();
        }
    }
}
