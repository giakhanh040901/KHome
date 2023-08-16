using AutoMapper;
using EPIC.DataAccess.Base;
using EPIC.DataAccess.Models;
using EPIC.Entities;
using EPIC.RealEstateDomain.Interfaces;
using EPIC.RealEstateEntities.DataEntities;
using EPIC.RealEstateEntities.Dto.RstDistributionPolicyTemp;
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
    public class RstDistributionPolicyTempServices : IRstDistributionPolicyTempServices
    {
        private readonly EpicSchemaDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<RstDistributionPolicyTempServices> _logger;
        private readonly string _connectionString;
        private readonly IHttpContextAccessor _httpContext;
        private readonly RstDistributionPolicyTempEFRepository _rstProjectDisPolicyEFRepository;

        public RstDistributionPolicyTempServices(EpicSchemaDbContext dbContext,
            DatabaseOptions databaseOptions,
            IMapper mapper,
            ILogger<RstDistributionPolicyTempServices> logger,
            IHttpContextAccessor httpContextAccessor)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
            _connectionString = databaseOptions.ConnectionString;
            _httpContext = httpContextAccessor;
            _rstProjectDisPolicyEFRepository = new RstDistributionPolicyTempEFRepository(dbContext, logger);
        }

        /// <summary>
        /// Thêm chính sách phân phối
        /// </summary>
        public RstDistributionPolicyTemp Add(CreateRstDistributionPolicyTempDto input)
        {
            _logger.LogInformation($"{nameof(Add)}: input = {JsonSerializer.Serialize(input)}");

            var username = CommonUtils.GetCurrentUsername(_httpContext);
            var partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
            var insertInput = _mapper.Map<RstDistributionPolicyTemp>(input);
            insertInput.PartnerId = partnerId;
            insertInput.CreatedBy = username;
            var result = _rstProjectDisPolicyEFRepository.Add(insertInput);
            _dbContext.SaveChanges();
            return result;
        }

        /// <summary>
        /// Cập nhật chính sách phân phối
        /// </summary>
        public RstDistributionPolicyTemp Update(UpdateRstDistributionPolicyTempDto input)
        {
            _logger.LogInformation($"{nameof(Update)}: input = {JsonSerializer.Serialize(input)}");
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            var partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);

            var updateInput = _mapper.Map<RstDistributionPolicyTemp>(input);
            updateInput.PartnerId = partnerId;
            updateInput.CreatedBy = username;

            var updatePolicy = _rstProjectDisPolicyEFRepository.Update(updateInput);
            _dbContext.SaveChanges();
            return updatePolicy;
        }

        /// <summary>
        /// Xoá chính sách
        /// </summary>
        public void Delete(int id)
        {
            _logger.LogInformation($"{nameof(Delete)}: id = {id}");

            //Lấy thông tin đối tác
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            var partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
            var policy = _rstProjectDisPolicyEFRepository.Entity.FirstOrDefault(p => p.Id == id && p.PartnerId == partnerId && p.Deleted == YesNo.NO)
                         .ThrowIfNull<RstDistributionPolicyTemp>(_dbContext, ErrorCode.RstProjectDistributionPolicyNotFound);
            policy.Deleted = YesNo.YES;
            policy.ModifiedBy = username;
            policy.ModifiedDate = DateTime.Now;
            _dbContext.SaveChanges();
        }

        /// <summary>
        /// Tìm kiếm chính sách
        /// </summary>
        public RstDistributionPolicyTemp FindById(int id)
        {
            _logger.LogInformation($"{nameof(FindById)}: id = {id}");
            var partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
            var policy = _rstProjectDisPolicyEFRepository.FindById(id).ThrowIfNull<RstDistributionPolicyTemp>(_dbContext, ErrorCode.RstProjectDistributionPolicyNotFound);
            return policy;
        }

        /// <summary>
        /// Danh sách chính sách
        /// </summary>
        public PagingResult<RstDistributionPolicyTempDto> FindAll(FilterRstDistributionPolicyTempDto input)
        {
            _logger.LogInformation($"{nameof(FindAll)}: input = {JsonSerializer.Serialize(input)}");

            var usertype = CommonUtils.GetCurrentUserType(_httpContext);
            int? partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
            var result = _rstProjectDisPolicyEFRepository.FindAll(input, partnerId);

            return new PagingResult<RstDistributionPolicyTempDto>
            {
                Items = _mapper.Map<List<RstDistributionPolicyTempDto>>(result.Items),
                TotalItems = result.TotalItems,
            };
        }

        /// <summary>
        /// Đổi trạng thái
        /// </summary>
        public RstDistributionPolicyTemp ChangeStatus(int id)
        {
            _logger.LogInformation($"{nameof(ChangeStatus)}: id = {id}");

            var distributionPolicy = _rstProjectDisPolicyEFRepository.FindById(id).ThrowIfNull<RstDistributionPolicyTemp>(_dbContext, ErrorCode.RstProjectDistributionPolicyNotFound);
            if (distributionPolicy.Status == Status.ACTIVE)
            {
                distributionPolicy.Status = Status.INACTIVE;
            }
            else
            {
                distributionPolicy.Status = Status.ACTIVE;
            }
            _dbContext.SaveChanges();
            return distributionPolicy;
        }
    }
}
