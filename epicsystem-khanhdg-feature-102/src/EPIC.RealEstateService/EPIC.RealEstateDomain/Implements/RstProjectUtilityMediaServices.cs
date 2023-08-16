using AutoMapper;
using EPIC.DataAccess.Base;
using EPIC.DataAccess.Models;
using EPIC.Entities;
using EPIC.RealEstateDomain.Interfaces;
using EPIC.RealEstateEntities.DataEntities;
using EPIC.RealEstateRepositories;
using EPIC.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using EPIC.RealEstateEntities.Dto.RstProjectUtilityMedia;
using EPIC.RealEstateEntities.Dto.RstProjectUtilityExtend;
using EPIC.InvestEntities.DataEntities;

namespace EPIC.RealEstateDomain.Implements
{
    public class RstProjectUtilityMediaServices : IRstProjectUtilityMediaServices
    {
        private readonly EpicSchemaDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<RstProjectUtilityMediaServices> _logger;
        private readonly string _connectionString;
        private readonly IHttpContextAccessor _httpContext;
        private readonly RstProjectUtilityMediaEFRepository _rstProjectUtilityMediaEFRepository;
        private readonly RstProjectEFRepository _rstProjectEFRepository;

        public RstProjectUtilityMediaServices(EpicSchemaDbContext dbContext,
            DatabaseOptions databaseOptions,
            IMapper mapper,
            ILogger<RstProjectUtilityMediaServices> logger,
            IHttpContextAccessor httpContextAccessor)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
            _connectionString = databaseOptions.ConnectionString;
            _httpContext = httpContextAccessor;
            _rstProjectEFRepository = new RstProjectEFRepository(dbContext, logger);
            _rstProjectUtilityMediaEFRepository = new RstProjectUtilityMediaEFRepository(dbContext, logger);
        }

        /// <summary>
        /// Thêm mới hình ảnh minh họa
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public RstProjectUtilityMediaDto Add(CreateRstProjectUtilityMediaDto input)
        {
            _logger.LogInformation($"{nameof(Add)} : input = {JsonSerializer.Serialize(input)}");

            var partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            var projectFind = _rstProjectEFRepository.FindById(input.ProjectId, partnerId).ThrowIfNull(_dbContext, ErrorCode.RstProjectNotFound);
            var mediaInsert = _rstProjectUtilityMediaEFRepository.Add(_mapper.Map<RstProjectUtilityMedia>(input), username, partnerId);
            _dbContext.SaveChanges();
            return _mapper.Map<RstProjectUtilityMediaDto>(mediaInsert);
        }

        /// <summary>
        /// Thay đổi trạng thái hình ảnh minh họa
        /// </summary>
        /// <param name="id"></param>
        public void ChangeStatus(int id)
        {
            _logger.LogInformation($"{nameof(ChangeStatus)} : id = {id}");

            var partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
            var username = CommonUtils.GetCurrentUsername(_httpContext);

            var mediaUpdate = _rstProjectUtilityMediaEFRepository.Entity.FirstOrDefault(e => e.Id == id && e.PartnerId == partnerId && e.Deleted == YesNo.NO).ThrowIfNull(_dbContext, ErrorCode.RstProjectUtilityMediaNotFound);

            if (mediaUpdate.Status == Status.ACTIVE)
            {
                mediaUpdate.Status = Status.INACTIVE;
            }
            else
            {
                mediaUpdate.Status = Status.ACTIVE;
            }
            mediaUpdate.ModifiedBy = username;
            mediaUpdate.ModifiedDate = DateTime.Now;
            _dbContext.SaveChanges();
        }

        /// <summary>
        /// Xóa hình ảnh minh họa tiện ích
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int id)
        {
            _logger.LogInformation($"{nameof(Delete)} : id = {id}");

            var partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
            var username = CommonUtils.GetCurrentUsername(_httpContext);

            var mediaUpdate = _rstProjectUtilityMediaEFRepository.Entity.FirstOrDefault(e => e.Id == id && e.PartnerId == partnerId && e.Deleted == YesNo.NO).ThrowIfNull(_dbContext, ErrorCode.RstProjectUtilityMediaNotFound);

            mediaUpdate.Deleted = YesNo.YES;
            mediaUpdate.ModifiedBy = username;
            mediaUpdate.ModifiedDate = DateTime.Now;
            _dbContext.SaveChanges();
        }


        /// <summary>
        /// Cập nhật hình ảnh minh họa
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public RstProjectUtilityMediaDto Update(UpdateRstProjectUtilityMediaDto input)
        {
            _logger.LogInformation($"{nameof(Update)} : input = {JsonSerializer.Serialize(input)}");

            var partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
            var username = CommonUtils.GetCurrentUsername(_httpContext);

            var mediaUpdate = _rstProjectUtilityMediaEFRepository.FindById(input.Id, partnerId, input.ProjectId).ThrowIfNull(_dbContext, ErrorCode.RstProjectUtilityMediaNotFound);

            mediaUpdate.Title = input.Title;
            mediaUpdate.Url = input.Url;
            mediaUpdate.ModifiedBy = username;
            mediaUpdate.ModifiedDate = DateTime.Now;
            _dbContext.SaveChanges();
            return _mapper.Map<RstProjectUtilityMediaDto>(mediaUpdate);

        }

        /// <summary>
        /// Get all hình ảnh tiện tích
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public List<RstProjectUtilityMediaDto> GetAll(int projectId)
        {
            _logger.LogInformation($"{nameof(GetAll)} : projectId = {projectId}");

            var partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
            var username = CommonUtils.GetCurrentUsername(_httpContext);

            var mediaList = _rstProjectUtilityMediaEFRepository.GetAll(partnerId, projectId);

            return _mapper.Map<List<RstProjectUtilityMediaDto>>(mediaList);
        }

        /// <summary>
        /// get theo Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public RstProjectUtilityMediaDto GetById(int id)
        {
            _logger.LogInformation($"{nameof(GetAll)} :id = {id}");

            var partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
            var username = CommonUtils.GetCurrentUsername(_httpContext);

            var mediaList = _rstProjectUtilityMediaEFRepository.Entity.FirstOrDefault(e => e.PartnerId == partnerId && e.Id == id && e.Deleted == YesNo.NO).ThrowIfNull(_dbContext, ErrorCode.RstProjectUtilityMediaNotFound);

            return _mapper.Map<RstProjectUtilityMediaDto>(mediaList);
        }
    }
}
