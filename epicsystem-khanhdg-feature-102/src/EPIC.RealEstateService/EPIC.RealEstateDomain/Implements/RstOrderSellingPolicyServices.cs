using AutoMapper;
using EPIC.DataAccess.Base;
using EPIC.DataAccess.Models;
using EPIC.Entities;
using EPIC.Entities.DataEntities;
using EPIC.EntitiesBase.Interfaces.Policy;
using EPIC.RealEstateDomain.Interfaces;
using EPIC.RealEstateEntities.DataEntities;
using EPIC.RealEstateEntities.Dto.RstOrderSellingPolicy;
using EPIC.RealEstateEntities.Dto.RstProductItemProjectPolicy;
using EPIC.RealEstateRepositories;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.RealEstate;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace EPIC.RealEstateDomain.Implements
{
    public class RstOrderSellingPolicyServices : IRstOrderSellingPolicyServices
    {
        private readonly EpicSchemaDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<RstProductItemServices> _logger;
        private readonly string _connectionString;
        private readonly IHttpContextAccessor _httpContext;
        private readonly DefErrorEFRepository _defErrorEFRepository;
        private readonly RstProductItemEFRepository _rstProductItemEFRepository;
        private readonly RstProductItemProjectPolicyEFRepository _rstProductItemProjectPolicyEFRepository;
        private readonly RstProjectPolicyEFRepository _rstProjectPolicyEFRepository;
        private readonly RstOpenSellDetailEFRepository _rstOpenSellDetailEFRepository;
        private readonly RstOpenSellEFRepository _rstOpenSellEFRepository;
        private readonly RstSellingPolicyEFRepository _rstSellingPolicyEFRepository;
        private readonly RstOrderEFRepository _rstOrderEFRepository;
        private readonly RstOrderSellingPolicyEFRepository _rstOrderSellingPolicyEFRepository;
        private readonly RstDistributionProductItemEFRepository _rstDistributionProductItemEFRepository;
        private readonly RstSellingPolicyTempEFRepository _rstSellingPolicyTempEFRepository;

        public RstOrderSellingPolicyServices(EpicSchemaDbContext dbContext,
            DatabaseOptions databaseOptions,
            IMapper mapper,
            ILogger<RstProductItemServices> logger,
            IHttpContextAccessor httpContextAccessor)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
            _connectionString = databaseOptions.ConnectionString;
            _httpContext = httpContextAccessor;
            _defErrorEFRepository = new DefErrorEFRepository(dbContext);
            _rstProductItemEFRepository = new RstProductItemEFRepository(dbContext, logger);
            _rstProductItemProjectPolicyEFRepository = new RstProductItemProjectPolicyEFRepository(dbContext, logger);
            _rstProjectPolicyEFRepository = new RstProjectPolicyEFRepository(dbContext, logger);
            _rstOpenSellDetailEFRepository = new RstOpenSellDetailEFRepository(dbContext, logger);
            _rstOpenSellEFRepository = new RstOpenSellEFRepository(dbContext, logger);
            _rstSellingPolicyEFRepository = new RstSellingPolicyEFRepository(dbContext, logger);
            _rstOrderEFRepository = new RstOrderEFRepository(dbContext, logger);
            _rstDistributionProductItemEFRepository = new RstDistributionProductItemEFRepository(dbContext, logger);
            _rstSellingPolicyTempEFRepository = new RstSellingPolicyTempEFRepository(dbContext, logger);
            _rstOrderSellingPolicyEFRepository = new RstOrderSellingPolicyEFRepository(dbContext, logger);
        }

        /// <summary>
        /// Tìm kiếm danh sách chính sách của sổ lệnh
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public PagingResult<RstOrderSellingPolicyDto> FindAll(FilterRstOrderSellingPolicyDto input)
        {
            _logger.LogInformation($"{nameof(FindAll)}: input = {JsonSerializer.Serialize(input)}");

            PagingResult<RstOrderSellingPolicyDto> result = new PagingResult<RstOrderSellingPolicyDto>();

            var resultItem = _rstOrderSellingPolicyEFRepository.FindPolicyByOrder(input);

            result.TotalItems = resultItem.Count();
            if (input.PageSize != -1)
            {
                resultItem = resultItem.Skip(input.Skip).Take(input.PageSize).ToList();
            }   

            result.Items = resultItem;
            return result;
        }

        /// <summary>
        /// Thêm chính sách ưu đãi của Đối tác và đại lý vào sổ lệnh
        /// </summary>
        /// <param name="input"></param>
        public void Add(CreateRstOrderSellingPolicyDto input)
        {
            var username = CommonUtils.GetCurrentUserType(_httpContext);
            _logger.LogInformation($"{nameof(Add)}: input = {JsonSerializer.Serialize(input)}, username = {username}");

            var orderFind = _rstOrderEFRepository.FindById(input.OrderId).ThrowIfNull(_dbContext, ErrorCode.RstOrderNotFound);
            
            var listOrderSellingPolicy = _rstOrderSellingPolicyEFRepository.FindPolicyByOrder(new FilterRstOrderSellingPolicyDto()
            {
                OrderId = input.OrderId
            });
            var listProjectPolicy = listOrderSellingPolicy.Where(e => e.ProjectPolicyId != null).Select(e => e.ProjectPolicyId.Value).ToList();  
            var listSellingPolicy = listOrderSellingPolicy.Where(e => e.SellingPolicyId != null).Select(e => e.SellingPolicyId.Value).ToList();

            //Lấy ra danh sách chính sách đã cài trong Db
            var getAllPolicy = _rstOrderSellingPolicyEFRepository.Entity.Where(e => e.OrderId == input.OrderId && e.Deleted == YesNo.NO);
            var getAllProjectPolicy = getAllPolicy.Where(e => e.ProductItemProjectPolicyId != null).Select(e => e.ProductItemProjectPolicyId.Value).ToList();
            var getAllSellingPolicy = getAllPolicy.Where(e => e.SellingPolicyId != null).Select(e => e.SellingPolicyId.Value).ToList();
            //Check đầu vào
            if (listProjectPolicy.Except(input.ProjectPolicyIds).Count() == listProjectPolicy.Count() && input.ProjectPolicyIds.Count() > 0)
            {
                _rstProductItemEFRepository.ThrowException(ErrorCode.RstProjectPolicyIsNotSelected);
            }

            if (listSellingPolicy.Except(input.SellingPolicyIds).Count() == listSellingPolicy.Count() && input.SellingPolicyIds.Count() > 0)
            {
                _rstProductItemEFRepository.ThrowException(ErrorCode.RstProjectPolicyIsNotSelected);
            }

            //Xóa những chính có trong db nhưng truyền vào không có
            var deleteProductItemPolicys = getAllProjectPolicy.Except(input.ProjectPolicyIds);
            foreach (var deleteItem in deleteProductItemPolicys)
            {
                var deleteProductItemPolicy = _rstOrderSellingPolicyEFRepository.Entity.FirstOrDefault(e => e.OrderId == input.OrderId && e.ProductItemProjectPolicyId == deleteItem && e.Deleted == YesNo.NO);
                deleteProductItemPolicy.Deleted = YesNo.YES;
                deleteProductItemPolicy.ModifiedDate = DateTime.Now;
                deleteProductItemPolicy.ModifiedBy = username;
            }

            var deleteSellingPolicys = getAllSellingPolicy.Except(input.SellingPolicyIds);
            foreach (var deleteItem in deleteSellingPolicys)
            {
                var deleteSellingPolicy = _rstOrderSellingPolicyEFRepository.Entity.FirstOrDefault(e => e.OrderId == input.OrderId && e.SellingPolicyId == deleteItem && e.Deleted == YesNo.NO);
                deleteSellingPolicy.Deleted = YesNo.YES;
                deleteSellingPolicy.ModifiedDate = DateTime.Now;
                deleteSellingPolicy.ModifiedBy = username;
            }

            //Thêm chính sách của đối tác vào sổ lệnh
            var insertProductItemPolicys = input.ProjectPolicyIds.Except(getAllProjectPolicy);
            foreach (var insertItem in insertProductItemPolicys)
            {
                var insertProductUtility = new RstOrderSellingPolicy()
                {
                    OrderId = input.OrderId,
                    ProductItemProjectPolicyId = insertItem
                };
                _rstOrderSellingPolicyEFRepository.Add(insertProductUtility, username);
            }
            //Thêm chính sách của đại lý vào sổ lệnh
            var insertSellingPolicys = input.SellingPolicyIds.Except(getAllSellingPolicy);
            foreach (var insertItem in insertSellingPolicys)
            {
                var insertProductUtility = new RstOrderSellingPolicy()
                {
                    OrderId = input.OrderId,
                    SellingPolicyId = insertItem
                };
                _rstOrderSellingPolicyEFRepository.Add(insertProductUtility, username);
            }
            _dbContext.SaveChanges();
        }
    }
}
