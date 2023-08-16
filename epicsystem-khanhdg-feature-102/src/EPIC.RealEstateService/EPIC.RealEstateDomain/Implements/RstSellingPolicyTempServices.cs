using AutoMapper;
using EPIC.DataAccess.Base;
using EPIC.DataAccess.Models;
using EPIC.Entities;
using EPIC.Entities.DataEntities;
using EPIC.RealEstateDomain.Interfaces;
using EPIC.RealEstateEntities.DataEntities;
using EPIC.RealEstateEntities.Dto.RstSellingPolicy;
using EPIC.RealEstateRepositories;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Identity;
using EPIC.Utils.ConstantVariables.RealEstate;
using EPIC.Utils.ConstantVariables.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace EPIC.RealEstateDomain.Implements
{
    public class RstSellingPolicyTempServices : IRstSellingPolicyTempServices
    {
        private readonly EpicSchemaDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<RstSellingPolicyTempServices> _logger;
        private readonly string _connectionString;
        private readonly IHttpContextAccessor _httpContext;
        private readonly DefErrorEFRepository _defErrorEFRepository;
        private readonly RstSellingPolicyTempEFRepository _rstSellingPolicyEFRepository;

        public RstSellingPolicyTempServices(EpicSchemaDbContext dbContext,
            DatabaseOptions databaseOptions,
            IMapper mapper,
            ILogger<RstSellingPolicyTempServices> logger,
            IHttpContextAccessor httpContextAccessor)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
            _connectionString = databaseOptions.ConnectionString;
            _httpContext = httpContextAccessor;
            _defErrorEFRepository = new DefErrorEFRepository(dbContext);
            _rstSellingPolicyEFRepository = new RstSellingPolicyTempEFRepository(dbContext, logger);
        }

        /// <summary>
        /// thêm mới chính sách bán hàng 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public RstSellingPolicyTemp Add(CreateRstSellingPolicyTempDto input)
        {

            var username = CommonUtils.GetCurrentUsername(_httpContext);
            int tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            _logger.LogInformation($"{nameof(Add)}: input = {JsonSerializer.Serialize(input)}, username = {username}, tradingProviderId = {tradingProviderId}");

            var insert = _rstSellingPolicyEFRepository.Add(input, username, tradingProviderId);
            _dbContext.SaveChanges();
            return insert;
        }

        /// <summary>
        /// Cập nhật chính sách bán hàng
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public RstSellingPolicyTemp Update(UpdateRstSellingPolicyTempDto input)
        {
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            int tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            _logger.LogInformation($"{nameof(Update)}: input = {JsonSerializer.Serialize(input)}, username = {username}, tradingProviderId = {tradingProviderId}");

            var salesPolicy = _rstSellingPolicyEFRepository.Entity.FirstOrDefault(p => p.Id == input.Id && p.TradingProviderId == tradingProviderId && p.Deleted == YesNo.NO).ThrowIfNull<RstSellingPolicyTemp>(_dbContext, ErrorCode.RstSellingPolicyNotFound);
            var updatesalesPolicy = _rstSellingPolicyEFRepository.Update(input, tradingProviderId, username);
            _dbContext.SaveChanges();
            return updatesalesPolicy;
        }

        /// <summary>
        /// Xóa chính sách bán hàng
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int id)
        {
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            int tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            _logger.LogInformation($"{nameof(Delete)}: id = {id}, username = {username}, tradingProviderId = {tradingProviderId}");

            var salesPolicy = _rstSellingPolicyEFRepository.Entity.FirstOrDefault(p => p.Id == id && p.TradingProviderId == tradingProviderId && p.Deleted == YesNo.NO).ThrowIfNull<RstSellingPolicyTemp>(_dbContext, ErrorCode.RstSellingPolicyNotFound);
            salesPolicy.Deleted = YesNo.YES;
            salesPolicy.ModifiedBy = username;
            salesPolicy.ModifiedDate = DateTime.Now;
            _dbContext.SaveChanges();
        }

        /// <summary>
        /// tìm kiếm chính sách bán hàng theo Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ViewRstSellingPolicyTempDto FindById(int id)
        {
            int tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            _logger.LogInformation($"{nameof(FindById)}: id={id}, tradingProviderId = {tradingProviderId}");

            var salesPolicy = _rstSellingPolicyEFRepository.Entity.FirstOrDefault(p => p.Id == id && p.TradingProviderId == tradingProviderId && p.Deleted == YesNo.NO).ThrowIfNull<RstSellingPolicyTemp>(_dbContext, ErrorCode.RstSellingPolicyNotFound);
            var result = _mapper.Map<ViewRstSellingPolicyTempDto>(salesPolicy);
            return result;
        }

        /// <summary>
        /// Tìm kiếm chính sách bán hàng có phân trang
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public PagingResult<ViewRstSellingPolicyTempDto> FindAll(FilterRstSellingPolicyTempDto input)
        {
            var usertype = CommonUtils.GetCurrentUserType(_httpContext);
            int? tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            _logger.LogInformation($"{nameof(FindAll)}: input = {JsonSerializer.Serialize(input)}, usertype = {usertype}, tradingProviderId = {tradingProviderId}");

            var result = new PagingResult<ViewRstSellingPolicyTempDto>();
            
            var salesPolicy = _rstSellingPolicyEFRepository.FindAll(input, tradingProviderId);
            result.Items = _mapper.Map<List<ViewRstSellingPolicyTempDto>>(salesPolicy.Items);
            result.TotalItems = salesPolicy.TotalItems;
            return result;
        }

        /// <summary>
        /// Đổi trạng thái
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public RstSellingPolicyTemp ChangeStatus(int id)
        {
            int? tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            _logger.LogInformation($"{nameof(ChangeStatus)}: id = {id}, tradingProviderId = {tradingProviderId}");

            var salesPolicy = _rstSellingPolicyEFRepository.FindById(id, tradingProviderId).ThrowIfNull<RstSellingPolicyTemp>(_dbContext, ErrorCode.RstSellingPolicyNotFound);
            var status = SaleStatus.ACTIVE;
            if (salesPolicy.Status == SaleStatus.ACTIVE)
            {
                status = SaleStatus.DEACTIVE;
            }
            else
            {
                status = SaleStatus.ACTIVE;
            }

            var changeStatus = _rstSellingPolicyEFRepository.ChangeStatus(id, status);
            _dbContext.SaveChanges();
            return changeStatus;
        }
    }
}
