using AutoMapper;
using EPIC.DataAccess.Base;
using EPIC.DataAccess.Models;
using EPIC.Entities;
using EPIC.RealEstateDomain.Interfaces;
using EPIC.RealEstateEntities.DataEntities;
using EPIC.RealEstateEntities.Dto.RstProjectInformationShare;
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
    public class RstProjectInformationShareServices : IRstProjectInformationShareServices
    {
        private readonly EpicSchemaDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<RstProjectInformationShareServices> _logger;
        private readonly string _connectionString;
        private readonly IHttpContextAccessor _httpContext;
        private readonly RstProjectEFRepository _rstProjectEFRepository;
        private readonly RstProjectInformationShareDetailEFRepository _rstProjectShareDetailEFRepository;
        private readonly RstProjectInformationShareEFRepository _rstProjectShareEFRepository;

        public RstProjectInformationShareServices(EpicSchemaDbContext dbContext,
            DatabaseOptions databaseOptions,
            IMapper mapper,
            ILogger<RstProjectInformationShareServices> logger,
            IHttpContextAccessor httpContextAccessor)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
            _connectionString = databaseOptions.ConnectionString;
            _httpContext = httpContextAccessor;
            _rstProjectEFRepository = new RstProjectEFRepository(dbContext, logger);
            _rstProjectShareDetailEFRepository = new RstProjectInformationShareDetailEFRepository(_dbContext, _logger);
            _rstProjectShareEFRepository = new RstProjectInformationShareEFRepository(_dbContext, _logger);
        }

        public RstProjectInformationShareDto AddProjectShare(CreateRstProjectInformationShareDto input)
        {
            int partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
            string username = CommonUtils.GetCurrentUsername(_httpContext);
            _logger.LogInformation($"{nameof(AddProjectShare)}: input = {JsonSerializer.Serialize(input)}, partnerId = {partnerId}, username = {username}");
            if (!_dbContext.RstProjects.Any(p => p.Id == input.ProjectId && p.PartnerId == partnerId && p.Deleted == YesNo.NO))
            {
                _rstProjectShareEFRepository.ThrowException(ErrorCode.RstProjectNotFound);
            }

            var insertProjectShare = _rstProjectShareEFRepository.Entity.Add(new RstProjectInformationShare
            {
                Id = (int)_rstProjectShareEFRepository.NextKey(),
                PartnerId = partnerId,
                Title = input.Title,
                ProjectId = input.ProjectId,
                ContentType = input.ContentType,
                OverviewContent = input.OverviewContent,
                Status = Status.TAM,
                CreatedBy = username,
            }).Entity;
            var result = _mapper.Map<RstProjectInformationShareDto>(insertProjectShare);
            // Thêm các file tài liệu
            foreach (var item in input.DocumentFiles)
            {
                _rstProjectShareDetailEFRepository.Entity.Add(new RstProjectInformationShareDetail
                {
                    Id = (int)_rstProjectShareDetailEFRepository.NextKey(),
                    ProjectShareId = insertProjectShare.Id,
                    FileUrl = item.FileUrl,
                    Name = item.Name,
                    Type = ProjectInformationShareFileTypes.Document,
                    CreatedBy = username,
                });
            }

            // Thêm các file hình ảnh
            foreach (var item in input.ImageFiles)
            {
                _rstProjectShareDetailEFRepository.Entity.Add(new RstProjectInformationShareDetail
                {
                    Id = (int)_rstProjectShareDetailEFRepository.NextKey(),
                    ProjectShareId = insertProjectShare.Id,
                    FileUrl = item.FileUrl,
                    Type = ProjectInformationShareFileTypes.Image,
                    CreatedBy = username,
                });
            }
            _dbContext.SaveChanges();
            return result;
        }

        public void UpdateProjectShare(UpdateRstProjectInformationShareDto input)
        {
            int partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
            string username = CommonUtils.GetCurrentUsername(_httpContext);
            _logger.LogInformation($"{nameof(UpdateProjectShare)}: input = {JsonSerializer.Serialize(input)}, partnerId = {partnerId}, username = {username}");
            var projectShare = _dbContext.RstProjectInformationShares.FirstOrDefault(x => x.Id == input.Id && x.Deleted == YesNo.NO)
                .ThrowIfNull(_dbContext, ErrorCode.RstProjectInformationShareNotFound);
            if (!_dbContext.RstProjects.Any(p => p.Id == projectShare.ProjectId && p.PartnerId == partnerId && p.Deleted == YesNo.NO))
            {
                _rstProjectShareEFRepository.ThrowException(ErrorCode.RstProjectNotFound);
            }

            projectShare.Title = input.Title;
            projectShare.ContentType = input.ContentType;
            projectShare.OverviewContent = input.OverviewContent;
            projectShare.ModifiedBy = username;
            projectShare.ModifiedDate = DateTime.Now;

            _rstProjectShareDetailEFRepository.Update(input.Id, ProjectInformationShareFileTypes.Document, username, input.DocumentFiles);
            _rstProjectShareDetailEFRepository.Update(input.Id, ProjectInformationShareFileTypes.Image, username, input.ImageFiles);
            _dbContext.SaveChanges();
        }

        /// <summary>
        /// Thay đổi trạng thái 
        /// </summary>
        public void ChangStatus(int id)
        {
            int partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
            string username = CommonUtils.GetCurrentUsername(_httpContext);
            _logger.LogInformation($"{nameof(ChangStatus)}: id = {id}, partnerId = {partnerId}, username = {username}");

            var projectShare = _dbContext.RstProjectInformationShares.FirstOrDefault(x => x.Id == id && x.Deleted == YesNo.NO)
                .ThrowIfNull(_dbContext, ErrorCode.RstProjectInformationShareNotFound);
            if (!_dbContext.RstProjects.Any(p => p.Id == projectShare.ProjectId && p.PartnerId == partnerId && p.Deleted == YesNo.NO))
            {
                _rstProjectShareEFRepository.ThrowException(ErrorCode.RstProjectNotFound);
            }

            projectShare.Status = (projectShare.Status != Status.ACTIVE) ? Status.ACTIVE : Status.INACTIVE;
            projectShare.ModifiedBy = username;
            projectShare.ModifiedDate = DateTime.Now;
            _dbContext.SaveChanges();
        }

        /// <summary>
        /// Xóa chia sẻ dự án
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int id)
        {
            int partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
            string username = CommonUtils.GetCurrentUsername(_httpContext);
            _logger.LogInformation($"{nameof(Delete)}: id = {id}, partnerId = {partnerId}, username = {username}");

            var projectShare = _dbContext.RstProjectInformationShares.FirstOrDefault(x => x.Id == id && x.Deleted == YesNo.NO)
                .ThrowIfNull(_dbContext, ErrorCode.RstProjectInformationShareNotFound);
            if (!_dbContext.RstProjects.Any(p => p.Id == projectShare.ProjectId && p.PartnerId == partnerId && p.Deleted == YesNo.NO))
            {
                _rstProjectShareEFRepository.ThrowException(ErrorCode.RstProjectNotFound);
            }
            projectShare.Deleted = YesNo.YES;
            projectShare.ModifiedBy = username;
            projectShare.ModifiedDate = DateTime.Now;
            var projectShareDetails = _dbContext.RstProjectInformationShareDetails.Where(x => x.ProjectShareId == id && x.Deleted == YesNo.NO);
            foreach (var item in projectShareDetails)
            {
                item.Deleted = YesNo.YES;
                item.ModifiedBy = username;
                item.ModifiedDate = DateTime.Now;
            }
            _dbContext.SaveChanges();
        }

        public RstProjectInformationShareDto FindById(int id)
        {
            var projectShare = _dbContext.RstProjectInformationShares.FirstOrDefault(x => x.Id == id && x.Deleted == YesNo.NO)
                .ThrowIfNull(_dbContext, ErrorCode.RstProjectInformationShareNotFound);
            var result = _mapper.Map<RstProjectInformationShareDto>(projectShare);
            var projectShareDetail = _dbContext.RstProjectInformationShareDetails.Where(x => x.ProjectShareId == projectShare.Id && x.Deleted == YesNo.NO);
            result.DocumentFiles = _mapper.Map<List<RstProjectInformationShareDetailDto>>(projectShareDetail.Where(x => x.Type == ProjectInformationShareFileTypes.Document));
            result.ImageFiles = _mapper.Map<List<RstProjectInformationShareDetailDto>>(projectShareDetail.Where(x => x.Type == ProjectInformationShareFileTypes.Image));
            return result;
        }

        public PagingResult<RstProjectInformationShareDto> FindAll(FilterRstProjectInformationShareDto input)
        {
            var projectShare = _dbContext.RstProjectInformationShares.Where(x => x.ProjectId == input.ProjectId
                                        && (input.Keyword == null || x.Title.ToLower().Contains(input.Keyword.ToLower()))
                                        && (input.Status == null || x.Status == input.Status) && x.Deleted == YesNo.NO);
            projectShare = projectShare.OrderByDescending(x => x.Id);
            int projectShareCount = projectShare.Count();
            if (input.PageSize != -1)
            {
                projectShare = projectShare.Skip(input.Skip).Take(input.PageSize);
            }
            return new PagingResult<RstProjectInformationShareDto>
            {
                Items = _mapper.Map<List<RstProjectInformationShareDto>>(projectShare),
                TotalItems = projectShareCount
            };
        }
    }
}
