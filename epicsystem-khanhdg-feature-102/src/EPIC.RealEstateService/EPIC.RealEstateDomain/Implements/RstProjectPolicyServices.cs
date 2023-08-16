using AutoMapper;
using EPIC.DataAccess.Base;
using EPIC.DataAccess.Models;
using EPIC.Entities;
using EPIC.RealEstateDomain.Interfaces;
using EPIC.RealEstateEntities.DataEntities;
using EPIC.RealEstateEntities.Dto.RstProjectPolicy;
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
    public class RstProjectPolicyServices : IRstProjectPolicyServices
    {
        private readonly EpicSchemaDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<RstProjectPolicyServices> _logger;
        private readonly string _connectionString;
        private readonly IHttpContextAccessor _httpContext;
        private readonly DefErrorEFRepository _defErrorEFRepository;
        private readonly RstProjectPolicyEFRepository _realEstateProjectPolicyEFRepository;

        public RstProjectPolicyServices(EpicSchemaDbContext dbContext,
            DatabaseOptions databaseOptions,
            IMapper mapper,
            ILogger<RstProjectPolicyServices> logger,
            IHttpContextAccessor httpContextAccessor)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
            _connectionString = databaseOptions.ConnectionString;
            _httpContext = httpContextAccessor;
            _defErrorEFRepository = new DefErrorEFRepository(dbContext);
            _realEstateProjectPolicyEFRepository = new RstProjectPolicyEFRepository(dbContext, logger);
        }

        /// <summary>
        /// Thêm chính sách ưu đãi chủ đầu tư
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public RstProjectPolicy Add(CreateRstProjectPolicyDto input)
        {
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            var partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
            _logger.LogInformation($"{nameof(Add)}: input = {JsonSerializer.Serialize(input)}, username = {username}, partnerId = {partnerId}");

            var insert = _realEstateProjectPolicyEFRepository.Add(_mapper.Map<RstProjectPolicy>(input), username, partnerId);
            _dbContext.SaveChanges();
            return insert;
        }

        /// <summary>
        /// Cập nhật chính sách ưu đãi chủ đầu tư
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public RstProjectPolicy Update(UpdateRstProjectPolicyDto input)
        {
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            var partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
            _logger.LogInformation($"{nameof(Update)}: input = {JsonSerializer.Serialize(input)}, username = {username}, partnerId = {partnerId}");

            var policy = _realEstateProjectPolicyEFRepository.Entity.FirstOrDefault(p => p.Id == input.Id && p.PartnerId == partnerId && p.Deleted == YesNo.NO).ThrowIfNull<RstProjectPolicy>(_dbContext, ErrorCode.RstProjectPolicyNotFound);
            var updatePolicy = _realEstateProjectPolicyEFRepository.Update(_mapper.Map<RstProjectPolicy>(input), partnerId, username);
            _dbContext.SaveChanges();
            return updatePolicy;
        }

        /// <summary>
        /// Xoá chính sách ưu đãi chủ đầu tư
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int id)
        {
            //Lấy thông tin đối tác
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            var partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
            _logger.LogInformation($"{nameof(Delete)}: id = {id}, username = {username}, partnerId = {partnerId}");

            var policy = _realEstateProjectPolicyEFRepository.Entity.FirstOrDefault(p => p.Id == id && p.PartnerId == partnerId && p.Deleted == YesNo.NO).ThrowIfNull<RstProjectPolicy>(_dbContext, ErrorCode.RstProjectPolicyNotFound);

            policy.Deleted = YesNo.YES;
            policy.ModifiedBy = username;
            policy.ModifiedDate = DateTime.Now;
            _dbContext.SaveChanges();
        }

        /// <summary>
        /// Tìm kiếm chính sách ưu đãi chủ đầu tư
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public RstProjectPolicy FindById(int id)
        {
            var partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
            _logger.LogInformation($"{nameof(FindById)}: id = {id}, partnerId = {partnerId}");

            var policy = _realEstateProjectPolicyEFRepository.FindById(id).ThrowIfNull<RstProjectPolicy>(_dbContext, ErrorCode.RstProjectPolicyNotFound);

            return policy;
        }

        /// <summary>
        /// Danh sách chính sách ưu đãi chủ đầu tư
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public PagingResult<RstProjectPolicy> FindAll(FilterRstProjectPolicyDto input)
        {
            int partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
            _logger.LogInformation($"{nameof(FindAll)}: input = {JsonSerializer.Serialize(input)}, partnerId = {partnerId}");

            var result = _realEstateProjectPolicyEFRepository.FindAll(input, partnerId);

            return result;
        }

        /// <summary>
        /// Đổi trạng thái chính sách ưu đãi chủ đầu tư
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public RstProjectPolicy ChangeStatus(int id)
        {
            _logger.LogInformation($"{nameof(ChangeStatus)}: id = {id}");

            var policy = _realEstateProjectPolicyEFRepository.FindById(id).ThrowIfNull<RstProjectPolicy>(_dbContext, ErrorCode.RstProjectPolicyNotFound);
            var status = Status.ACTIVE;
            if (policy.Status == Status.ACTIVE)
            {
                status = Status.INACTIVE;
            }
            else
            {
                status = Status.ACTIVE;
            }

            var changeStatus = _realEstateProjectPolicyEFRepository.ChangeStatus(id, status);
            _dbContext.SaveChanges();
            return changeStatus;
        }
    }
}
