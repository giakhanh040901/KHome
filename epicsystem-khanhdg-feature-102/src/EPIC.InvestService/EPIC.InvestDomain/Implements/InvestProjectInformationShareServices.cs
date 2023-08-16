using AutoMapper;
using EPIC.DataAccess.Base;
using EPIC.DataAccess.Models;
using EPIC.Entities;
using EPIC.InvestDomain.Interfaces;
using EPIC.InvestEntities.DataEntities;
using EPIC.InvestEntities.Dto.ProjectInformationShare;
using EPIC.InvestRepositories;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.RealEstate;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace EPIC.InvestDomain.Implements
{
    public class InvestProjectInformationShareServices : IInvestProjectInformationShareServices
    {
        private readonly EpicSchemaDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<InvestProjectInformationShareServices> _logger;
        private readonly string _connectionString;
        private readonly IHttpContextAccessor _httpContext;
        private readonly InvestProjectInformationShareDetailEFRepository _invProjectInforShareDetailEFRepository;
        private readonly InvestProjectInformationShareEFRepository _invProjectInforShareEFRepository;

        public InvestProjectInformationShareServices(EpicSchemaDbContext dbContext,
            DatabaseOptions databaseOptions,
            IMapper mapper,
            ILogger<InvestProjectInformationShareServices> logger,
            IHttpContextAccessor httpContextAccessor)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
            _connectionString = databaseOptions.ConnectionString;
            _httpContext = httpContextAccessor;
            _invProjectInforShareDetailEFRepository = new InvestProjectInformationShareDetailEFRepository(_dbContext, _logger);
            _invProjectInforShareEFRepository = new InvestProjectInformationShareEFRepository(_dbContext, _logger);
        }

        public InvProjectInformationShareDto AddProjectInformationShare(CreateInvProjectInformationShareDto input)
        {
            int partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
            string username = CommonUtils.GetCurrentUsername(_httpContext);
            _logger.LogInformation($"{nameof(AddProjectInformationShare)}: input = {JsonSerializer.Serialize(input)}, partnerId = {partnerId}, username = {username}");
            if (!_dbContext.InvestProjects.Any(p => p.Id == input.ProjectId && p.PartnerId == partnerId && p.Deleted == YesNo.NO))
            {
                _invProjectInforShareEFRepository.ThrowException(ErrorCode.InvestProjectNotFound);
            }

            var insertProjectShare = _invProjectInforShareEFRepository.Entity.Add(new InvestProjectInformationShare
            {
                Id = (int)_invProjectInforShareEFRepository.NextKey(),
                PartnerId = partnerId,
                Title = input.Title,
                ProjectId = input.ProjectId,
                ContentType = input.ContentType,
                OverviewContent = input.OverviewContent,
                Status = Status.TAM,
                CreatedBy = username,
            }).Entity;
            var result = _mapper.Map<InvProjectInformationShareDto>(insertProjectShare);
            // Thêm các file tài liệu
            foreach (var item in input.DocumentFiles)
            {
                _invProjectInforShareDetailEFRepository.Entity.Add(new InvestProjectInformationShareDetail
                {
                    Id = (int)_invProjectInforShareDetailEFRepository.NextKey(),
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
                _invProjectInforShareDetailEFRepository.Entity.Add(new InvestProjectInformationShareDetail
                {
                    Id = (int)_invProjectInforShareDetailEFRepository.NextKey(),
                    ProjectShareId = insertProjectShare.Id,
                    FileUrl = item.FileUrl,
                    Type = ProjectInformationShareFileTypes.Image,
                    CreatedBy = username,
                });
            }
            _dbContext.SaveChanges();
            return result;
        }

        public void UpdateProjectInformationShare(UpdateInvProjectInformationShareDto input)
        {
            int partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
            string username = CommonUtils.GetCurrentUsername(_httpContext);
            _logger.LogInformation($"{nameof(UpdateProjectInformationShare)}: input = {JsonSerializer.Serialize(input)}, partnerId = {partnerId}, username = {username}");
            var projectShare = _dbContext.InvestProjectInformationShares.FirstOrDefault(x => x.Id == input.Id && x.Deleted == YesNo.NO)
                .ThrowIfNull(_dbContext, ErrorCode.InvestProjectInformationShareNotFound);
            if (!_dbContext.InvestProjects.Any(p => p.Id == projectShare.ProjectId && p.PartnerId == partnerId && p.Deleted == YesNo.NO))
            {
                _invProjectInforShareEFRepository.ThrowException(ErrorCode.InvestProjectNotFound);
            }

            projectShare.Title = input.Title;
            projectShare.ContentType = input.ContentType;
            projectShare.OverviewContent = input.OverviewContent;
            projectShare.ModifiedBy = username;
            projectShare.ModifiedDate = DateTime.Now;

            _invProjectInforShareDetailEFRepository.Update(input.Id, ProjectInformationShareFileTypes.Document, username, input.DocumentFiles);
            _invProjectInforShareDetailEFRepository.Update(input.Id, ProjectInformationShareFileTypes.Image, username, input.ImageFiles);
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

            var projectShare = _dbContext.InvestProjectInformationShares.FirstOrDefault(x => x.Id == id && x.Deleted == YesNo.NO)
                .ThrowIfNull(_dbContext, ErrorCode.InvestProjectInformationShareNotFound);
            if (!_dbContext.InvestProjects.Any(p => p.Id == projectShare.ProjectId && p.PartnerId == partnerId && p.Deleted == YesNo.NO))
            {
                _invProjectInforShareEFRepository.ThrowException(ErrorCode.InvestProjectNotFound);
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

            var projectShare = _dbContext.InvestProjectInformationShares.FirstOrDefault(x => x.Id == id && x.Deleted == YesNo.NO)
                .ThrowIfNull(_dbContext, ErrorCode.InvestProjectInformationShareNotFound);
            if (!_dbContext.InvestProjects.Any(p => p.Id == projectShare.ProjectId && p.PartnerId == partnerId && p.Deleted == YesNo.NO))
            {
                _invProjectInforShareEFRepository.ThrowException(ErrorCode.InvestProjectNotFound);
            }
            projectShare.Deleted = YesNo.YES;
            projectShare.ModifiedBy = username;
            projectShare.ModifiedDate = DateTime.Now;
            var projectShareDetails = _dbContext.InvestProjectInformationShareDetails.Where(x => x.ProjectShareId == id && x.Deleted == YesNo.NO);
            foreach (var item in projectShareDetails)
            {
                item.Deleted = YesNo.YES;
                item.ModifiedBy = username;
                item.ModifiedDate = DateTime.Now;
            }
            _dbContext.SaveChanges();
        }

        public InvProjectInformationShareDto FindById(int id)
        {
            var projectShare = _dbContext.InvestProjectInformationShares.FirstOrDefault(x => x.Id == id && x.Deleted == YesNo.NO)
                .ThrowIfNull(_dbContext, ErrorCode.InvestProjectInformationShareNotFound);
            var result = _mapper.Map<InvProjectInformationShareDto>(projectShare);
            var projectShareDetail = _dbContext.InvestProjectInformationShareDetails.Where(x => x.ProjectShareId == projectShare.Id && x.Deleted == YesNo.NO);
            result.DocumentFiles = _mapper.Map<List<InvProjectInformationShareDetailDto>>(projectShareDetail.Where(x => x.Type == ProjectInformationShareFileTypes.Document));
            result.ImageFiles = _mapper.Map<List<InvProjectInformationShareDetailDto>>(projectShareDetail.Where(x => x.Type == ProjectInformationShareFileTypes.Image));
            return result;
        }

        public PagingResult<InvProjectInformationShareDto> FindAll(FilterInvProjectInformationShareDto input)
        {
            var projectShare = _dbContext.InvestProjectInformationShares.Where(x => x.ProjectId == input.ProjectId
                                        && (input.Keyword == null || x.Title.ToLower().Contains(input.Keyword.ToLower())) 
                                        && (input.Status == null || x.Status == input.Status) && x.Deleted == YesNo.NO);
            projectShare = projectShare.OrderByDescending(x => x.Id);
            int projectShareCount = projectShare.Count();
            if (input.PageSize != -1)
            {
                projectShare = projectShare.Skip(input.Skip).Take(input.PageSize);
            }
            return new PagingResult<InvProjectInformationShareDto>
            {
                Items = _mapper.Map<List<InvProjectInformationShareDto>>(projectShare),
                TotalItems = projectShareCount
            };
        }
    }
}
