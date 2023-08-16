using AutoMapper;
using EPIC.DataAccess.Base;
using EPIC.Entities;
using EPIC.FileDomain.Interfaces;
using EPIC.FileDomain.Services;
using EPIC.RealEstateDomain.Interfaces;
using EPIC.RealEstateEntities.DataEntities;
using EPIC.RealEstateEntities.Dto.RstProductItemMedia;
using EPIC.RealEstateEntities.Dto.RstProjectMedia;
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
    public class RstProductItemMediaServices : IRstProductItemMediaServices
    {
        private readonly EpicSchemaDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<RstProductItemMediaServices> _logger;
        private readonly string _connectionString;
        private readonly IHttpContextAccessor _httpContext;
        private readonly DefErrorEFRepository _defErrorEFRepository;
        private readonly RstProductItemMediaEFRepository _rstProductItemMediaEFRepository;
        private readonly RstProductItemMediaDetailEFRepository _rstProductItemMediaDetailEFRepository;
        private readonly IFileExtensionServices _fileExtensionServices;
        private readonly IFileServices _fileServices;

        public RstProductItemMediaServices(EpicSchemaDbContext dbContext,
            DatabaseOptions databaseOptions,
            IMapper mapper,
            ILogger<RstProductItemMediaServices> logger,
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
            _rstProductItemMediaEFRepository = new RstProductItemMediaEFRepository(dbContext, logger);
            _rstProductItemMediaDetailEFRepository = new RstProductItemMediaDetailEFRepository(dbContext, logger);
            _fileExtensionServices = fileExtensionServices;
            _fileServices = fileServices;
        }

        /// <summary>
        /// Thêm hình ảnh sản phẩm
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public List<RstProductItemMedia> Add(CreateRstProductItemMediasDto input)
        {
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            var partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
            _logger.LogInformation($"{nameof(Add)}: input = {JsonSerializer.Serialize(input)}, username = {username}, partnerId = {partnerId}");

            var mediaList = new List<RstProductItemMedia>();
            var transaction = _dbContext.Database.BeginTransaction();
            foreach (var item in input.RstProductItemMedias)
            {
                var insert = _rstProductItemMediaEFRepository.Add(_mapper.Map<RstProductItemMedia>(item), username, partnerId);
                var media = _fileExtensionServices.GetMediaExtensionFile(insert.UrlImage);
                insert.MediaType = media;
                insert.SortOrder = insert.Id;
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
        /// update thứ tự hình ảnh
        /// </summary>
        /// <returns></returns>
        public void UpdateSortOrder(RstProductItemMediaSortDto input)
        {
            int partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
            _logger.LogInformation($"{nameof(UpdateSortOrder)}: input = {input}, partnerId = {partnerId}");

            var result = _rstProductItemMediaEFRepository.Find(input.ProductItemId, partnerId, input.Location);

            foreach (var item in result)
            {
                var sort = input.Sort.FirstOrDefault(o => o.ProductItemMediaId == item.Id);
                if (sort != null)
                {
                    item.SortOrder = sort.SortOrder;
                }
            }
            _dbContext.SaveChanges();
        }

        /// <summary>
        /// Cập nhật hình ảnh sản phẩm 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public RstProductItemMedia Update(UpdateRstProductItemMediaDto input)
        {
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            var partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
            _logger.LogInformation($"{nameof(Update)}: input = {JsonSerializer.Serialize(input)}, username = {username}, partnerId = {partnerId}");

            var mediaFind = _rstProductItemMediaEFRepository
                .Entity.FirstOrDefault(p => p.Id == input.Id && p.PartnerId == partnerId && p.Deleted == YesNo.NO)
                .ThrowIfNull<RstProductItemMedia>(_dbContext, ErrorCode.RstProductItemMediaNotFound);
            var location = mediaFind.Location;
            if (input.GroupTitle == null)
            {
                mediaFind.UrlPath = input.UrlPath;
                mediaFind.Location = location;
                mediaFind.UrlImage = input.UrlImage;
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
        /// Xoá hình ảnh sản phẩm
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int id)
        {
            //Lấy thông tin đối tác
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            var partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
            _logger.LogInformation($"{nameof(Delete)}: id = {id}, username = {username}, partnerId = {partnerId}");

            var media = _rstProductItemMediaEFRepository.Entity.FirstOrDefault(p => p.Id == id && p.PartnerId == partnerId && p.Deleted == YesNo.NO)
                .ThrowIfNull<RstProductItemMedia>(_dbContext, ErrorCode.RstProductItemMediaNotFound);

            _rstProductItemMediaEFRepository.Delete(id, partnerId);
            // Kiểm tra file có được sử dụng bởi các media khác không
            var listUrlImageMedia = _rstProductItemMediaEFRepository.EntityNoTracking.Where(o => o.UrlImage == media.UrlImage && o.Deleted == YesNo.NO);
            if (!listUrlImageMedia.Any())
            {
                _fileServices.DeleteFile(media.UrlImage);
            }

            var mediaDetail = _rstProductItemMediaDetailEFRepository.FindByProjectMediaId(media.Id, partnerId);
            foreach (var item in mediaDetail)
            {
                _rstProductItemMediaDetailEFRepository.Delete(item.Id, partnerId);
                var listUrlImageMediaDetail = _rstProductItemMediaDetailEFRepository.EntityNoTracking.Where(o => o.UrlImage == item.UrlImage && o.Deleted == YesNo.NO);
                if (!listUrlImageMediaDetail.Any())
                {
                    _fileServices.DeleteFile(item.UrlImage);
                }
            }
            _dbContext.SaveChanges();
        }

        /// <summary>
        /// Tìm hình ảnh sản phẩm theo id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public RstProductItemMedia FindById(int id)
        {
            var partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
            _logger.LogInformation($"{nameof(FindById)}: id = {id}, partnerId = {partnerId}");
            var media = _rstProductItemMediaEFRepository.FindById(id, partnerId).ThrowIfNull<RstProductItemMedia>(_dbContext, ErrorCode.RstProductItemMediaNotFound);

            return media;
        }

        /// <summary>
        /// Thay đổi trạng thái hình ảnh sản phẩm
        /// </summary>
        /// <param name="id"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public RstProductItemMedia ChangeStatus(int id, string status)
        {
            var partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
            _logger.LogInformation($"{nameof(ChangeStatus)}: id = {id}, status = {status}, partnerId = {partnerId}");

            var media = _rstProductItemMediaEFRepository.FindById(id).ThrowIfNull<RstProductItemMedia>(_dbContext, ErrorCode.RstProductItemMediaNotFound);
            var changeStatus = _rstProductItemMediaEFRepository.ChangeStatus(id, status, partnerId);
            _dbContext.SaveChanges();
            return changeStatus;
        }

        /// <summary>
        /// Danh sách hình ảnh sản phẩm orderBy location
        /// </summary>
        /// <returns></returns>
        public List<RstProductItemMedia> Find(int productItemId, string location, string status)
        {
            int partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
            _logger.LogInformation($"{nameof(Find)}, partnerId = {partnerId}, location = {location}, status = {status}");

            var result = _rstProductItemMediaEFRepository.Find(productItemId, partnerId, location, status);
            return result;
        }
    }
}
