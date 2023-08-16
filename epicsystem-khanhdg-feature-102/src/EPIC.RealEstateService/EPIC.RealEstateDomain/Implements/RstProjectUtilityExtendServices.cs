using AutoMapper;
using DocumentFormat.OpenXml.Office2010.ExcelAc;
using EPIC.DataAccess.Base;
using EPIC.Entities;
using EPIC.RealEstateDomain.Interfaces;
using EPIC.RealEstateEntities.DataEntities;
using EPIC.RealEstateEntities.Dto.RstProjectUtilityExtend;
using EPIC.RealEstateEntities.Dto.RstProjectUtilityMedia;
using EPIC.RealEstateRepositories;
using EPIC.Utils;
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
    public class RstProjectUtilityExtendServices : IRstProjectUtilityExtendServices
    {
        private readonly EpicSchemaDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<RstProjectUtilityExtendServices> _logger;
        private readonly string _connectionString;
        private readonly IHttpContextAccessor _httpContext;
        private readonly RstProjectUtilityExtendEFRepository _rstProjectUtilityExtendEFRepository;
        private readonly RstProjectEFRepository _rstProjectEFRepository;

        public RstProjectUtilityExtendServices(EpicSchemaDbContext dbContext,
            DatabaseOptions databaseOptions,
            IMapper mapper,
            ILogger<RstProjectUtilityExtendServices> logger,
            IHttpContextAccessor httpContextAccessor)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
            _connectionString = databaseOptions.ConnectionString;
            _httpContext = httpContextAccessor;
            _rstProjectUtilityExtendEFRepository = new RstProjectUtilityExtendEFRepository(dbContext, logger);
            _rstProjectEFRepository = new RstProjectEFRepository(dbContext, logger);
        }

        /// <summary>
        /// Thêm tiện tích khác
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public List<RstProjectUtilityExtendDto> Add(CreateRstProjectUtilityExtendDto input)
        {
            _logger.LogInformation($"{nameof(Add)} : input = {JsonSerializer.Serialize(input)}");

            var partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
            var username = CommonUtils.GetCurrentUsername(_httpContext);

            var result = new List<RstProjectUtilityExtendDto>();
            var projectFind = _rstProjectEFRepository.FindById(input.ProjectId, partnerId).ThrowIfNull(_dbContext, ErrorCode.RstProjectNotFound);

            foreach (var item in input.UtilityExtends)
            {
                var extendInsert = _rstProjectUtilityExtendEFRepository.Add(_mapper.Map<RstProjectUtilityExtend>(item), username, partnerId, input.ProjectId);
                result.Add(_mapper.Map<RstProjectUtilityExtendDto>(extendInsert));
            }
            _dbContext.SaveChanges();
            return result;
        }
        /// <summary>
        /// Xóa tiện ích khác
        /// </summary>
        /// <param name="id"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void Delete(int id)
        {
            _logger.LogInformation($"{nameof(Delete)} : id = {id}");

            var partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
            var username = CommonUtils.GetCurrentUsername(_httpContext);

            var extendUpdate = _rstProjectUtilityExtendEFRepository.Entity.FirstOrDefault(e => e.Id == id && e.PartnerId == partnerId && e.Deleted == YesNo.NO).ThrowIfNull(_dbContext, ErrorCode.RstProjectUtilityExtendNotFound);
            extendUpdate.Deleted = YesNo.YES;
            extendUpdate.ModifiedBy = username;
            extendUpdate.ModifiedDate = DateTime.Now;
            _dbContext.SaveChanges();
        }

        /// <summary>
        /// Cập nhật hình ảnh minh họa
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public void Update(UpdateRstProjectUtilityExtendDto input)
        {
            _logger.LogInformation($"{nameof(Update)} : input = {JsonSerializer.Serialize(input)}");

            var partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            
            var extendUpdate = _rstProjectUtilityExtendEFRepository.FindById(input.Id, partnerId, input.ProjectId).ThrowIfNull(_dbContext, ErrorCode.RstProjectUtilityExtendNotFound);

            extendUpdate.Title = input.Title;
            extendUpdate.IsHighlight = input.IsHighlight;
            extendUpdate.IconName = input.IconName;
            extendUpdate.ModifiedBy = username;
            extendUpdate.ModifiedDate = DateTime.Now;
            _dbContext.SaveChanges();
        }

        /// <summary>
        /// Thay đổi trạng thái tiện ích khác
        /// </summary>
        /// <param name="id"></param>
        public void ChangeStatus(int id)
        {
            _logger.LogInformation($"{nameof(ChangeStatus)} : id = {id}");

            var partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
            var username = CommonUtils.GetCurrentUsername(_httpContext);

            var extendUpdate = _rstProjectUtilityExtendEFRepository.Entity.FirstOrDefault(e => e.Id == id && e.PartnerId == partnerId && e.Deleted == YesNo.NO).ThrowIfNull(_dbContext, ErrorCode.RstProjectUtilityExtendNotFound);

            if (extendUpdate.Status == Status.ACTIVE)
            {
                extendUpdate.Status = Status.INACTIVE;
            }
            else
            {
                extendUpdate.Status = Status.ACTIVE;
            }
            extendUpdate.ModifiedBy = username;
            extendUpdate.ModifiedDate = DateTime.Now;
            _dbContext.SaveChanges();
        }

        /// <summary>
        /// Get all hình ảnh tiện tích
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public List<RstProjectUtilityExtendDto> GetAll(int projectId)
        {
            _logger.LogInformation($"{nameof(GetAll)} : projectId = {projectId}");

            var partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
            var username = CommonUtils.GetCurrentUsername(_httpContext);

            var extendList = _rstProjectUtilityExtendEFRepository.GetAll(partnerId, projectId);

            return _mapper.Map<List<RstProjectUtilityExtendDto>>(extendList);
        }

        /// <summary>
        /// get theo Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public RstProjectUtilityExtendDto GetById(int id)
        {
            _logger.LogInformation($"{nameof(GetAll)} :id = {id}");

            var partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
            var username = CommonUtils.GetCurrentUsername(_httpContext);

            var extendList = _rstProjectUtilityExtendEFRepository.Entity.FirstOrDefault(e => e.PartnerId == partnerId && e.Id == id && e.Deleted == YesNo.NO).ThrowIfNull(_dbContext, ErrorCode.RstProjectUtilityExtendNotFound);

            return _mapper.Map<RstProjectUtilityExtendDto>(extendList);
        }
    }
}
