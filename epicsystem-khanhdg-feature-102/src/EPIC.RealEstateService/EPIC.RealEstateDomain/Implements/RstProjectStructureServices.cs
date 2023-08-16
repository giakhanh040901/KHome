using AutoMapper;
using EPIC.CoreRepositories;
using EPIC.DataAccess.Base;
using EPIC.DataAccess.Models;
using EPIC.Entities;
using EPIC.RealEstateDomain.Interfaces;
using EPIC.RealEstateEntities.DataEntities;
using EPIC.RealEstateEntities.Dto.RstProjectStructure;
using EPIC.RealEstateRepositories;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.RealEstate;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace EPIC.RealEstateDomain.Implements
{
    public class RstProjectStructureServices : IRstProjectStructureServices
    {
        private readonly EpicSchemaDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<RstProjectStructureServices> _logger;
        private readonly string _connectionString;
        private readonly IHttpContextAccessor _httpContext;
        private readonly DefErrorEFRepository _defErrorEFRepository;
        private readonly RstProjectStructureEFRepository _rstProjectStructureEFRepository;
        private readonly RstProjectEFRepository _rstProjectEFRepository;
        private readonly RstOwnerEFRepository _rstOwnerEFRepository;
        private readonly BusinessCustomerEFRepository _businessCustomerEFRepository;

        public RstProjectStructureServices(EpicSchemaDbContext dbContext,
            DatabaseOptions databaseOptions,
            IMapper mapper,
            ILogger<RstProjectStructureServices> logger,
            IHttpContextAccessor httpContextAccessor)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
            _connectionString = databaseOptions.ConnectionString;
            _httpContext = httpContextAccessor;
            _defErrorEFRepository = new DefErrorEFRepository(dbContext);
            _rstProjectStructureEFRepository = new RstProjectStructureEFRepository(dbContext, logger);
            _rstProjectEFRepository = new RstProjectEFRepository(dbContext, logger);
            _rstOwnerEFRepository = new RstOwnerEFRepository(dbContext, logger);
            _businessCustomerEFRepository = new BusinessCustomerEFRepository(dbContext, logger);
        }

        /// <summary>
        /// Thêm cấu trúc cấu thành dự án
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public RstProjectStructure Add(CreateRstProjectStructureDto input)
        {
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            var partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);

            _logger.LogInformation($"{nameof(Add)}: input = {JsonSerializer.Serialize(input)}, username = {username}, partnerId = {partnerId}");

            var insert = _rstProjectStructureEFRepository.Add(_mapper.Map<RstProjectStructure>(input), username, partnerId);

            var projectStructureFind = _rstProjectStructureEFRepository.Entity.FirstOrDefault(p => p.Id == input.ParentId && p.PartnerId == partnerId && p.Deleted == YesNo.NO);

            if (projectStructureFind != null)
            {
                insert.Level = projectStructureFind.Level + 1;
            }

            _dbContext.SaveChanges();
            return insert;
        }

        /// <summary>
        /// Cập nhật cấu trúc cấu thành dự án
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public RstProjectStructure Update(UpdateRstProjectStructureDto input)
        {
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            var partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);

            _logger.LogInformation($"{nameof(Update)}: input = {JsonSerializer.Serialize(input)}, username = {username}, partnerId = {partnerId}");

            var structure = _rstProjectStructureEFRepository.Entity.FirstOrDefault(p => p.Id == input.Id && p.PartnerId == partnerId && p.Deleted == YesNo.NO).ThrowIfNull<RstProjectStructure>(_dbContext, ErrorCode.RstProjectStructureNotFound);
            var parentUpdateStructure = _rstProjectStructureEFRepository.Entity.FirstOrDefault(p => p.Id == structure.ParentId && p.PartnerId == partnerId && p.Deleted == YesNo.NO);
            var updateStructure = _rstProjectStructureEFRepository.Update(_mapper.Map<RstProjectStructure>(input), partnerId, username);
            _dbContext.SaveChanges();

            return updateStructure;
        }

        /// <summary>
        /// Xoá cấu trúc cấu thành dự án
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int id)
        {
            //Lấy thông tin đối tác
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            var partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);

            _logger.LogInformation($"{nameof(Delete)}: id = {id}, username = {username}, partnerId = {partnerId}");

            var structure = _rstProjectStructureEFRepository.Entity.FirstOrDefault(p => p.Id == id && p.PartnerId == partnerId && p.Deleted == YesNo.NO).ThrowIfNull<RstProjectStructure>(_dbContext, ErrorCode.RstProjectStructureNotFound);
            var listChildStructure = _rstProjectStructureEFRepository.Entity.Where(p => p.ParentId == id && p.Deleted == YesNo.NO);

            foreach (var child in listChildStructure)
            {
                child.Deleted = YesNo.YES;
                child.ModifiedBy = username;
                child.ModifiedDate = DateTime.Now;
            }

            structure.Deleted = YesNo.YES;
            structure.ModifiedBy = username;
            structure.ModifiedDate = DateTime.Now;
            _dbContext.SaveChanges();
        }

        /// <summary>
        /// Tìm kiếm cấu trúc cấu thành dự án
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public RstProjectStructure FindById(int id)
        {
            var partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
            _logger.LogInformation($"{nameof(FindById)}: id = {id}, partnerId = {partnerId}");
            var structure = _rstProjectStructureEFRepository.FindById(id, partnerId).ThrowIfNull<RstProjectStructure>(_dbContext, ErrorCode.RstProjectStructureNotFound);

            return structure;
        }

        /// <summary>
        /// Danh sách cấu trúc cấu thành dự án
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public PagingResult<RstProjectStructure> FindAll(FilterRstProjectStructureDto input)
        {
            int partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);

            _logger.LogInformation($"{nameof(FindAll)}: input = {JsonSerializer.Serialize(input)}, partnerId = {partnerId}");

            var usertype = CommonUtils.GetCurrentUserType(_httpContext);
            var result = _rstProjectStructureEFRepository.FindAll(input, partnerId);

            return result;
        }

        public ViewRstProjectStructureDto FindByProjectId(int projectId)
        {
            int partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);

            _logger.LogInformation($"{nameof(FindByProjectId)}: projectId = {projectId}, partnerId = {partnerId}");

            var listStructure = _rstProjectStructureEFRepository.FindByProjectId(projectId, partnerId);
            var result = new ViewRstProjectStructureDto();
            var projectFind = _rstProjectEFRepository.FindById(projectId, partnerId);
            if (projectFind == null)
            {
                return result;
            }
            var ownerFind = _rstOwnerEFRepository.FindById(projectFind.OwnerId);
            if (ownerFind == null)
            {
                return result;
            }

            var businessCustomer = _businessCustomerEFRepository.FindById(ownerFind.BusinessCustomerId);
            if (businessCustomer == null)
            {
                return result;
            }

            result.AvatarImage = businessCustomer.AvatarImageUrl;
            result.ProjectName = projectFind.Name;
            result.ProjectStructures = _mapper.Map<List<RstProjectStructureDto>>(listStructure);
            return result;
        }

        /// <summary>
        /// <inheritdoc/>
        /// Danh sách mật độ xây dựng con
        /// </summary>
        /// <returns></returns>
        public List<RstProjectStructureChildDto> FindAllChild(int projectId)
        {
            _logger.LogInformation($"{nameof(FindAllChild)}: projectId = {projectId}");
            var listStructureQuery = _rstProjectStructureEFRepository.EntityNoTracking.Where(s => s.Deleted == YesNo.NO && s.ProjectId == projectId);
            var result = listStructureQuery.Where(o => !listStructureQuery.Any(s => s.ParentId == o.Id));
            return _mapper.Map<List<RstProjectStructureChildDto>>(result);
        }

        public List<RstProjectStructureChildDto> FindAllByLevel(int level, int projectId)
        {
            _logger.LogInformation($"{nameof(FindAllByLevel)}: level = {level}");
            var result = _rstProjectStructureEFRepository.Entity.Where(o => o.Level == level && o.ProjectId == projectId && o.Deleted == YesNo.NO).OrderByDescending(o => o.Id).ToList();
            return _mapper.Map<List<RstProjectStructureChildDto>>(result);
        }
    }
}
