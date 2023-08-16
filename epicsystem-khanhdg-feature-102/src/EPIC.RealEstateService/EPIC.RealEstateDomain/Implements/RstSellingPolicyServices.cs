using AutoMapper;
using EPIC.DataAccess.Base;
using EPIC.DataAccess.Models;
using EPIC.Entities;
using EPIC.RealEstateDomain.Interfaces;
using EPIC.RealEstateEntities.DataEntities;
using EPIC.RealEstateEntities.Dto.RstProductItemProjectPolicy;
using EPIC.RealEstateEntities.Dto.RstSellingPolicy;
using EPIC.RealEstateRepositories;
using EPIC.Utils;
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
    public class RstSellingPolicyServices : IRstSellingPolicyServices
    {
        private readonly EpicSchemaDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<RstSellingPolicyServices> _logger;
        private readonly string _connectionString;
        private readonly IHttpContextAccessor _httpContext;
        private readonly DefErrorEFRepository _defErrorEFRepository;
        private readonly RstSellingPolicyEFRepository _rstSellingPolicyEFRepository;
        private readonly RstSellingPolicyTempEFRepository _rstSellingPolicyTempEFRepository;
        private readonly RstOpenSellEFRepository _rstOpenSellEFRepository;

        public RstSellingPolicyServices(EpicSchemaDbContext dbContext,
            DatabaseOptions databaseOptions,
            IMapper mapper,
            ILogger<RstSellingPolicyServices> logger,
            IHttpContextAccessor httpContextAccessor)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
            _connectionString = databaseOptions.ConnectionString;
            _httpContext = httpContextAccessor;
            _defErrorEFRepository = new DefErrorEFRepository(dbContext);
            _rstSellingPolicyEFRepository = new RstSellingPolicyEFRepository(dbContext, logger);
            _rstSellingPolicyTempEFRepository = new RstSellingPolicyTempEFRepository(dbContext, logger);
            _rstOpenSellEFRepository = new RstOpenSellEFRepository(dbContext, logger);
        }

        public void AddSellingPolicy(CreateRstSellingPolicyDto input)
        {
            var usertype = CommonUtils.GetCurrentUserType(_httpContext);
            var username = CommonUtils.GetCurrentUserType(_httpContext);
            int tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            _logger.LogInformation($"{nameof(AddSellingPolicy)}: input = {JsonSerializer.Serialize(input)}, username = {username}, tradingProviderId = {tradingProviderId}");

            var productItemFind = _rstOpenSellEFRepository.FindById(input.OpenSellId).ThrowIfNull(_dbContext, ErrorCode.RstOpenSellNotFound);
            //Lấy ra danh sách chính sách mở bán của dự án cài đặt
            var listSellingPolicy = _rstSellingPolicyTempEFRepository.Entity.Where(e => e.TradingProviderId == tradingProviderId && e.Deleted == YesNo.NO).Select(e => e.Id).ToList();

            //Lấy ra danh sách chính sách mở bán của mở bán đó tỏng Db
            var getAllSellingPolicy = _rstSellingPolicyEFRepository.Entity.Where(e => e.TradingProviderId == tradingProviderId && e.OpenSellId == input.OpenSellId && e.Deleted == YesNo.NO).Select(e => e.SellingPolicyTempId).ToList();

            //Check đầu vàos
            if (listSellingPolicy.Except(input.SellingPolicies).Count() == listSellingPolicy.Count())
            {
                _rstSellingPolicyEFRepository.ThrowException(ErrorCode.RstSellingPolicyIsNotSelected);
            }

            //Xóa những chính sách có trong db nhưng truyền vào không có
            var deleteSellingPolicys = getAllSellingPolicy.Except(input.SellingPolicies);
            foreach (var deleteItem in deleteSellingPolicys)
            {
                var deleteSellingPolicy = _rstSellingPolicyEFRepository.Entity.FirstOrDefault(e => e.TradingProviderId == tradingProviderId && e.OpenSellId == input.OpenSellId && e.Deleted == YesNo.NO && e.SellingPolicyTempId == deleteItem);
                deleteSellingPolicy.Deleted = YesNo.YES;
                deleteSellingPolicy.ModifiedDate = DateTime.Now;
                deleteSellingPolicy.ModifiedBy = username;
            }

            //Thêm nhiều tiện ích vào căn hộ
            var insertSellingPolicys = input.SellingPolicies.Except(getAllSellingPolicy);
            foreach (var insertItem in insertSellingPolicys)
            {
                var insertSellingPolicy = new RstSellingPolicy()
                {
                    SellingPolicyTempId = insertItem,
                    OpenSellId = input.OpenSellId
                };
                _rstSellingPolicyEFRepository.Add(insertSellingPolicy, username, tradingProviderId);
            }
            _dbContext.SaveChanges();
        }

        public void ChangeStatusSellingPolicy(int id)
        {
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            _logger.LogInformation($"{nameof(ChangeStatusSellingPolicy)} : id = {id}, username = {username}, tradingProviderId = {tradingProviderId}");


            var sellingPolicyUpdate = _rstSellingPolicyEFRepository.Entity.FirstOrDefault(e => e.Id == id && e.TradingProviderId == tradingProviderId && e.Deleted == YesNo.NO).ThrowIfNull(_dbContext, ErrorCode.RstSellingPolicyNotFound);

            if (sellingPolicyUpdate.Status == Status.ACTIVE)
            {
                sellingPolicyUpdate.Status = Status.INACTIVE;
            }
            else
            {
                sellingPolicyUpdate.Status = Status.ACTIVE;
            }
            sellingPolicyUpdate.ModifiedBy = username;
            sellingPolicyUpdate.ModifiedDate = DateTime.Now;
            _dbContext.SaveChanges();
        }

        public PagingResult<RstSellingPolicyDto> FindAllSellingPolicy(FilterRstSellingPolicyDto input)
        {
            var usertype = CommonUtils.GetCurrentUserType(_httpContext);
            int? tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            _logger.LogInformation($"{nameof(FindAllSellingPolicy)}: input = {JsonSerializer.Serialize(input)}, userType = {usertype}, tradingProviderId = {tradingProviderId}");

            var result = new PagingResult<RstSellingPolicyDto>();
            var resultItems = new List<RstSellingPolicyDto>();

            var openSellFind = _rstOpenSellEFRepository.FindById(input.OpenSellId).ThrowIfNull(_dbContext, ErrorCode.RstOpenSellNotFound);

            //Lấy ra danh sách chính sách mở bán của dự án cài đặt
            var sellingPolicies = _rstSellingPolicyTempEFRepository.Entity;

            //Data Utility
            var sellingProjectpolicyQuery = _rstSellingPolicyEFRepository.Entity.Where(e => e.OpenSellId == input.OpenSellId && e.TradingProviderId == tradingProviderId && e.Deleted == YesNo.NO);

            var query = from policy in sellingPolicies
                        join sellingPolicy in sellingProjectpolicyQuery on policy.Id equals sellingPolicy.SellingPolicyTempId into listSellingPolicy
                        from itemPolicy in listSellingPolicy.DefaultIfEmpty()
                        where policy.TradingProviderId == tradingProviderId && policy.Deleted == YesNo.NO
                        && policy.Status == Status.ACTIVE
                        && (input.Keyword == null || policy.Name.Contains(input.Keyword) || policy.Code.Contains(input.Keyword))
                        && (input.Selected == null || itemPolicy != null)
                        && (input.Status == null || itemPolicy.Status == input.Status)
                        && (input.Source == null || policy.Source == input.Source)
                        select new
                        {
                            policy,
                            itemPolicy
                        };

            result.TotalItems = query.Count();
            if (input.PageSize != -1)
            {
                query = query.Skip(input.Skip).Take(input.PageSize);
            }

            foreach (var item in query)
            {
                //Lấy ra tên và nhóm tiện ích
                resultItems.Add(new RstSellingPolicyDto()
                {
                    Id = item.itemPolicy?.Id,
                    SellingPolicyTempId = item.policy.Id,
                    Code = item.policy.Code,
                    Name = item.policy.Name,
                    Source = item.policy.Source,
                    ConversionValue = item.policy.ConversionValue,
                    IsSellingPolicySelected = item.itemPolicy == null ? YesNo.NO : YesNo.YES,
                    Status = item.itemPolicy?.Status
                });
            }

            result.Items = resultItems;
            return result;
        }
    }
}
