using AutoMapper;
using EPIC.CoreRepositories;
using EPIC.DataAccess.Base;
using EPIC.Entities;
using EPIC.RealEstateDomain.Interfaces;
using EPIC.RealEstateEntities.DataEntities;
using EPIC.RealEstateEntities.Dto.RstRating;
using EPIC.RealEstateRepositories;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.RealEstate;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace EPIC.RealEstateDomain.Implements
{
    public class RstRatingServices : IRstRatingServices
    {
        private readonly EpicSchemaDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<RstRatingServices> _logger;
        private readonly string _connectionString;
        private readonly IHttpContextAccessor _httpContext;
        private readonly DefErrorEFRepository _defErrorEFRepository;
        private readonly RstOrderEFRepository _rstOrderEFRepository;
        private readonly CifCodeEFRepository _cifCodeEFRepository;
        private readonly RstRatingEFRepository _rstRatingEFRepository;
        private readonly RstOrderContractFileEFRepository _rstOrderContractFileEFRepository;
        private readonly InvestorEFRepository _investorEFRepository;

        public RstRatingServices(EpicSchemaDbContext dbContext,
            DatabaseOptions databaseOptions,
            IMapper mapper,
            ILogger<RstRatingServices> logger,
            IHttpContextAccessor httpContextAccessor)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
            _connectionString = databaseOptions.ConnectionString;
            _httpContext = httpContextAccessor;
            _defErrorEFRepository = new DefErrorEFRepository(dbContext);
            _rstOrderEFRepository = new RstOrderEFRepository(dbContext, logger);
            _cifCodeEFRepository = new CifCodeEFRepository(dbContext, logger);
            _rstRatingEFRepository = new RstRatingEFRepository(_dbContext, _logger);
            _rstOrderContractFileEFRepository = new RstOrderContractFileEFRepository(_dbContext, _logger);
            _investorEFRepository = new InvestorEFRepository(_dbContext, _logger);
        }

        /// <summary>
        /// Tìm hợp đồng mới nhất để đánh giá
        /// </summary>
        /// <returns></returns>
        public RstRatingDto FindLastOrder()
        {
            _logger.LogInformation($"{nameof(FindLastOrder)}");
            var result = new RstRatingDto();
            var investorId = CommonUtils.GetCurrentInvestorId(_httpContext);
            var cifCode = _cifCodeEFRepository.FindByInvestor(investorId);
            if (cifCode == null)
            {
                _defErrorEFRepository.ThrowException(ErrorCode.CoreCifCodeNotFound);
            }

            var order = _rstOrderEFRepository.EntityNoTracking.Where(o => o.CifCode == cifCode.CifCode && o.Status == RstOrderStatus.DA_COC && o.Deleted == YesNo.NO).OrderByDescending(o => o.Id).FirstOrDefault();
            if (order == null)
            {
                _defErrorEFRepository.ThrowException(ErrorCode.RstOrderNotFound);
            }
            string contractCode = null;
            var contractCodeGen = _rstOrderContractFileEFRepository.EntityNoTracking.Where(e => e.OrderId == order.Id && e.TradingProviderId == order.TradingProviderId && e.Deleted == YesNo.NO).Select(o => o.ContractCodeGen).Distinct();
            if (contractCodeGen.Count() == 1)
            {
                contractCode = contractCodeGen.First();
            }

            var rating = _rstRatingEFRepository.Entity.FirstOrDefault(o => o.OrderId == order.Id && o.Deleted == YesNo.NO);
            if (rating == null)
            {
                return new RstRatingDto()
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
        public void Add(CreateRstRatingDto input)
        {
            _logger.LogInformation($"{nameof(Add)}: input = {JsonSerializer.Serialize(input)}");
            var investorId = CommonUtils.GetCurrentInvestorId(_httpContext);
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            var contractCode = FindLastOrder();

            if (contractCode == null)
            {
                _defErrorEFRepository.ThrowException("Hợp đồng đã được đánh giá");
            }
            var investRatingFind = _rstRatingEFRepository.Entity.Where(o => o.OrderId == contractCode.OrderId && o.Deleted == YesNo.NO);
            if (!investRatingFind.Any())
            {
                var investRating = new RstRating()
                {
                    OrderId = contractCode.OrderId,
                    InvestorId = investorId,
                    Rate = input.Rate,
                    ProductExperience = input.ProductExperience,
                    Feedback = input.Feedback,
                    CreatedBy = username,
                    CreatedDate = DateTime.Now,
                };
                _rstRatingEFRepository.Add(investRating);
            }
            _dbContext.SaveChanges();
        }

        /// <summary>
        /// Xem danh sách ratting của dự án
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        public List<ViewRstRatingDto> ViewRstRating(int projectId)
        {
            _logger.LogInformation($"{nameof(ViewRstRating)}: projectId = {projectId}");
            var resutl = new List<ViewRstRatingDto>();

            var listRating = (from project in _dbContext.RstProjects
                              join productItem in _dbContext.RstProductItems on project.Id equals productItem.ProjectId
                              join order in _dbContext.RstOrders on productItem.Id equals order.ProductItemId
                              join rating in _dbContext.RstRatings on order.Id equals rating.OrderId
                              where order.Deleted == YesNo.NO && productItem.Deleted == YesNo.NO && project.Deleted == YesNo.NO 
                              && rating.Deleted == YesNo.NO && project.Id == projectId
                              select new { project.Id, rating.InvestorId, rating.Feedback, rating.CreatedDate }).OrderByDescending(o => o.CreatedDate);

            foreach (var item in listRating)
            {
                var viewRating = new ViewRstRatingDto();
                var investorFind = _investorEFRepository.FindById(item.InvestorId);
                viewRating.FaceImageUrl = investorFind.FaceImageUrl;
                viewRating.Name = investorFind.Name;
                viewRating.Feedback = item.Feedback;
                viewRating.CreatedDate = item.CreatedDate;
                resutl.Add(viewRating);
            }
            return resutl;
        }
    }
}
