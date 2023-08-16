using AutoMapper;
using DocumentFormat.OpenXml.Office2010.Excel;
using EPIC.DataAccess.Base;
using EPIC.DataAccess.Models;
using EPIC.Entities;
using EPIC.Entities.DataEntities;
using EPIC.InvestEntities.DataEntities;
using EPIC.RealEstateDomain.Interfaces;
using EPIC.RealEstateEntities.DataEntities;
using EPIC.RealEstateEntities.Dto.RstDistributionPolicy;
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
    public class RstDistributionPolicyServices : IRstDistributionPolicyServices
    {
        private readonly EpicSchemaDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<RstDistributionPolicyServices> _logger;
        private readonly string _connectionString;
        private readonly IHttpContextAccessor _httpContext;
        private readonly RstDistributionPolicyEFRepository _rstProjectDisPolicyEFRepository;
        private readonly RstOpenSellEFRepository _rstOpenSellEFRepository;

        public RstDistributionPolicyServices(EpicSchemaDbContext dbContext,
            DatabaseOptions databaseOptions,
            IMapper mapper,
            ILogger<RstDistributionPolicyServices> logger,
            IHttpContextAccessor httpContextAccessor)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
            _connectionString = databaseOptions.ConnectionString;
            _httpContext = httpContextAccessor;
            _rstProjectDisPolicyEFRepository = new RstDistributionPolicyEFRepository(dbContext, logger);
            _rstOpenSellEFRepository = new RstOpenSellEFRepository(dbContext, logger);
        }

        /// <summary>
        /// Thêm chính sách phân phối
        /// </summary>
        public RstDistributionPolicy Add(CreateRstDistributionPolicyDto input)
        {
            _logger.LogInformation($"{nameof(Add)}: input = {JsonSerializer.Serialize(input)}");

            var username = CommonUtils.GetCurrentUsername(_httpContext);
            var partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);

            var policyQuery = _rstProjectDisPolicyEFRepository.Entity.Where(p => p.Deleted == YesNo.NO && p.DistributionId == input.DistributionId && p.PartnerId == partnerId);

            var insertInput = _mapper.Map<RstDistributionPolicy>(input);
            insertInput.PartnerId = partnerId;
            insertInput.CreatedBy = username;

            if (policyQuery.Any())
            {
                insertInput.Status = Status.INACTIVE;
            }

            var result = _rstProjectDisPolicyEFRepository.Add(insertInput);
            _dbContext.SaveChanges();
            return result;
        }

        /// <summary>
        /// đổi trạng thái kích hoạt cho sản phẩm phân phối
        /// </summary>
        /// <param name="id"></param>
        /// <param name="DistributionId"></param>
        public void ActiveDistributionPolicy(int id, int DistributionId)
        {
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            var partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);

            var policyQuery = _rstProjectDisPolicyEFRepository.Entity.Where(p => p.Deleted == YesNo.NO && p.DistributionId == DistributionId && p.PartnerId == partnerId);
            
            foreach(var item in policyQuery)
            {
                if(item.Id != id)
                {
                    item.Status = Status.INACTIVE;
                }
                else
                {
                    item.Status = Status.ACTIVE;
                }
            }
            _dbContext.SaveChanges();
        }

        /// <summary>
        /// Cập nhật chính sách phân phối
        /// </summary>
        public RstDistributionPolicy Update(UpdateRstDistributionPolicyDto input)
        {
            _logger.LogInformation($"{nameof(Update)}:  input = {JsonSerializer.Serialize(input)}");
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            var partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);

            var updateInput = _mapper.Map<RstDistributionPolicy>(input);
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
                         .ThrowIfNull<RstDistributionPolicy>(_dbContext, ErrorCode.RstProjectDistributionPolicyNotFound);
            policy.Deleted = YesNo.YES;
            policy.ModifiedBy = username;
            policy.ModifiedDate = DateTime.Now;
            _dbContext.SaveChanges();
        }

        /// <summary>
        /// Tìm kiếm chính sách
        /// </summary>
        public RstDistributionPolicy FindById(int id)
        {
            _logger.LogInformation($"{nameof(FindById)}: id = {id}");
            var partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
            var policy = _rstProjectDisPolicyEFRepository.FindById(id).ThrowIfNull<RstDistributionPolicy>(_dbContext, ErrorCode.RstProjectDistributionPolicyNotFound);
            return policy;
        }

        /// <summary>
        /// Danh sách chính sách
        /// </summary>
        public PagingResult<RstDistributionPolicyDto> FindAll(FilterRstDistributionPolicyDto input, int distributionId)
        {
            _logger.LogInformation($"{nameof(FindAll)}:  input = {JsonSerializer.Serialize(input)}");

            var usertype = CommonUtils.GetCurrentUserType(_httpContext);
            int? partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
            var result = _rstProjectDisPolicyEFRepository.FindAll(input, distributionId, partnerId);

            return new PagingResult<RstDistributionPolicyDto>
            {
                Items = _mapper.Map<List<RstDistributionPolicyDto>>(result.Items),
                TotalItems = result.TotalItems,
            };
        }

        /// <summary>
        /// Đổi trạng thái
        /// </summary>
        public RstDistributionPolicy ChangeStatus(int id)
        {
            _logger.LogInformation($"{nameof(ChangeStatus)}: id = {id}");

            var distributionPolicy = _rstProjectDisPolicyEFRepository.FindById(id).ThrowIfNull<RstDistributionPolicy>(_dbContext, ErrorCode.RstDistributionPolicyNotFound);
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

        public List<RstDistributionPolicyDto> GetAllPolicy(FilterDistrobutionPolicyDto input)
        {
            _logger.LogInformation($"{nameof(GetAllPolicy)}: input = {JsonSerializer.Serialize(input)}");

            var openSell = _rstOpenSellEFRepository.FindById(input.OpenSellId).ThrowIfNull(_dbContext, ErrorCode.RstOpenSellDetailNotFound);

            var distribution = _dbContext.RstDistributions.FirstOrDefault(e =>e.Id == openSell.DistributionId && e.Deleted == YesNo.NO)
                .ThrowIfNull(_dbContext, ErrorCode.RstDistributionNotFound);

            var distributionPolices = _rstProjectDisPolicyEFRepository.Entity.Where(e => e.DistributionId == openSell.DistributionId && e.Deleted == YesNo.NO && (input.Status == null || e.Status == input.Status));
            var result = _mapper.Map<List<RstDistributionPolicyDto>>(distributionPolices);
            return result;
        }
    }
}
