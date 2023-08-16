using AutoMapper;
using EPIC.DataAccess.Base;
using EPIC.DataAccess.Models;
using EPIC.Entities;
using EPIC.FileDomain.Interfaces;
using EPIC.FileDomain.Services;
using EPIC.RealEstateDomain.Interfaces;
using EPIC.RealEstateEntities.DataEntities;
using EPIC.RealEstateEntities.Dto.RstProjectMedia;
using EPIC.RealEstateRepositories;
using EPIC.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace EPIC.RealEstateDomain.Implements
{
    public class RstProjectMediaServices : IRstProjectMediaServices
    {
        private readonly EpicSchemaDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<RstProjectMediaServices> _logger;
        private readonly string _connectionString;
        private readonly IHttpContextAccessor _httpContext;
        private readonly DefErrorEFRepository _defErrorEFRepository;
        private readonly RstProjectMediaEFRepository _rstProjectMediaEFRepository;
        private readonly RstProjectMediaDetailEFRepository _rstProjectMediaDetailEFRepository;
        private readonly IFileExtensionServices _fileExtensionServices;
        private readonly IFileServices _fileServices;

        public RstProjectMediaServices(EpicSchemaDbContext dbContext,
            DatabaseOptions databaseOptions,
            IMapper mapper,
            ILogger<RstProjectMediaServices> logger,
            IHttpContextAccessor httpContextAccessor,
            IFileExtensionServices fileExtensionServices,
            IFileServices fileServices)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
            _connectionString = databaseOptions.ConnectionString;
            _httpContext = httpContextAccessor;
            _defErrorEFRepository = new DefErrorEFRepository(dbContext);
            _rstProjectMediaEFRepository = new RstProjectMediaEFRepository(dbContext, logger);
            _rstProjectMediaDetailEFRepository = new RstProjectMediaDetailEFRepository(dbContext, logger);
            _fileExtensionServices = fileExtensionServices;
            _fileServices = fileServices;
        }

        /// <summary>
        /// Thêm hình ảnh dự án
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public List<RstProjectMedia> Add(CreateRstProjectMediasDto input)
        {
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            var partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
            _logger.LogInformation($"{nameof(Add)}: input = {JsonSerializer.Serialize(input)}, username = {username}, partnerId = {partnerId}");

            var mediaList = new List<RstProjectMedia>();
            var transaction = _dbContext.Database.BeginTransaction();
            foreach (var item in input.RstProjectMedias)
            {
                var insert = _rstProjectMediaEFRepository.Add(_mapper.Map<RstProjectMedia>(item), username, partnerId);
                if (insert.UrlImage != null)
                {
                    var media = _fileExtensionServices.GetMediaExtensionFile(insert.UrlImage);
                    insert.MediaType = media;
                    insert.SortOrder = insert.Id;
                }
                mediaList.Add(insert);
            }

            foreach (var item in mediaList)
            {
                item.Location = input.Location;
            }

            _dbContext.SaveChanges();
            transaction.Commit();
            return mediaList;
        }

        /// <summary>
        /// Cập nhật hình ảnh dự án
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public RstProjectMedia Update(UpdateRstProjectMediaDto input)
        {
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            var partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
            _logger.LogInformation($"{nameof(Update)}: input = {JsonSerializer.Serialize(input)}, username = {username}, partnerId = {partnerId}");

            var mediaFind = _rstProjectMediaEFRepository.Entity
                .FirstOrDefault(p => p.Id == input.Id && p.PartnerId == partnerId && p.Deleted == YesNo.NO)
                .ThrowIfNull<RstProjectMedia>(_dbContext, ErrorCode.RstProjectMediaNotFound);
            var location = mediaFind.Location;
            if (input.GroupTitle == null)
            {
                mediaFind.UrlPath = input.UrlPath;
                mediaFind.UrlImage = input.UrlImage;
                mediaFind.Location = location;
                var media = _fileExtensionServices.GetMediaExtensionFile(mediaFind.UrlImage);
                mediaFind.MediaType = media;
            }
            else
            {
                mediaFind.GroupTitle = input.GroupTitle;
            }
            mediaFind.ModifiedBy = username;
            mediaFind.ModifiedDate = DateTime.Now;
            _dbContext.SaveChanges();
            return mediaFind;
        }

        /// <summary>
        /// Xoá hình ảnh dự án
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int id)
        {
            //Lấy thông tin đối tác
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            var partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
            _logger.LogInformation($"{nameof(Delete)}: id = {id}, username = {username}, partnerId = {partnerId}");

            var media = _rstProjectMediaEFRepository.Entity.FirstOrDefault(p => p.Id == id && p.PartnerId == partnerId && p.Deleted == YesNo.NO).ThrowIfNull<RstProjectMedia>(_dbContext, ErrorCode.RstProjectMediaNotFound);

            _rstProjectMediaEFRepository.Delete(id, partnerId);
            var listUrlImageMedia = _rstProjectMediaEFRepository.EntityNoTracking.Where(o => o.UrlImage == media.UrlImage && o.Deleted == YesNo.NO);
            if (!listUrlImageMedia.Any())
            {
                _fileServices.DeleteFile(media.UrlImage);
            }

            var mediaDetail = _rstProjectMediaDetailEFRepository.FindByProjectMediaId(media.Id, partnerId);
            foreach (var item in mediaDetail)
            {
                _rstProjectMediaDetailEFRepository.Delete(item.Id, partnerId);
                var listUrlImageMediaDetail = _rstProjectMediaDetailEFRepository.EntityNoTracking.Where(o => o.UrlImage == item.UrlImage && o.Deleted == YesNo.NO);
                if (!listUrlImageMediaDetail.Any())
                {
                    _fileServices.DeleteFile(item.UrlImage);
                }
            }
            _dbContext.SaveChanges();
        }

        /// <summary>
        /// Tìm hình ảnh dự án
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public RstProjectMedia FindById(int id)
        {
            var partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
            _logger.LogInformation($"{nameof(FindById)}: id = {id}, partnerId = {partnerId}");

            var media = _rstProjectMediaEFRepository.FindById(id, partnerId).ThrowIfNull<RstProjectMedia>(_dbContext, ErrorCode.RstProjectMediaNotFound);
            return media;
        }

        /// <summary>
        /// Danh sách hình ảnh dự án
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public PagingResult<RstProjectMedia> FindAll(FilterRstProjectMediaDto input)
        {
            int partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
            _logger.LogInformation($"{nameof(FindAll)}: input = {JsonSerializer.Serialize(input)}, partnerId = {partnerId}");

            var result = _rstProjectMediaEFRepository.FindAll(input, partnerId);
            return result;
        }

        /// <summary>
        /// Thay đổi trạng thái hình ảnh dự án
        /// </summary>
        /// <param name="id"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public RstProjectMedia ChangeStatus(int id, string status)
        {
            int partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
            _logger.LogInformation($"{nameof(ChangeStatus)}: id = {id}, status = {status}, partnerId = {partnerId}");

            var media = _rstProjectMediaEFRepository.FindById(id).ThrowIfNull<RstProjectMedia>(_dbContext, ErrorCode.RstProjectMediaNotFound);
            var changeStatus = _rstProjectMediaEFRepository.ChangeStatus(id, status, partnerId);
            _dbContext.SaveChanges();
            return changeStatus;
        }

        /// <summary>
        /// Danh sách hình ảnh dự án orderBy location
        /// </summary>
        /// <returns></returns>
        public List<RstProjectMedia> Find(int projectId, string location, string status)
        {
            int partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
            _logger.LogInformation($"{nameof(Find)}: projectId = {projectId}, partnerId = {partnerId}, " +
                $" location = {location}, status = {status}");

            var result = _rstProjectMediaEFRepository.Find(projectId, partnerId, location, status);
            return result.ToList();
        }

        /// <summary>
        /// update thứ tự hình ảnh dự án
        /// </summary>
        /// <returns></returns>
        public void UpdateSortOrder(RstProjectMediaSortDto input)
        {
            int partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
            _logger.LogInformation($"{nameof(UpdateSortOrder)}: input = {input}, partnerId = {partnerId}");

            var result = _rstProjectMediaEFRepository.Find(input.ProjectId, partnerId, input.Location);

            foreach (var item in result)
            {
                var sort = input.Sort.FirstOrDefault(o => o.ProjectMediaId == item.Id);
                if (sort != null)
                {
                    item.SortOrder = sort.SortOrder;
                }
            }
            _dbContext.SaveChanges();
        }
    }
}
