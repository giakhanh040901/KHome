using AutoMapper;
using EPIC.DataAccess.Base;
using EPIC.Entities;
using EPIC.FileDomain.Interfaces;
using EPIC.FileDomain.Services;
using EPIC.RealEstateDomain.Interfaces;
using EPIC.RealEstateEntities.DataEntities;
using EPIC.RealEstateEntities.Dto.RstProjectMedia;
using EPIC.RealEstateEntities.Dto.RstProjectMediaDetail;
using EPIC.RealEstateRepositories;
using EPIC.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace EPIC.RealEstateDomain.Implements
{
    public class RstProjectMediaDetailServices : IRstProjectMediaDetailServices
    {
        private readonly EpicSchemaDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<RstProjectMediaDetailServices> _logger;
        private readonly string _connectionString;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IFileExtensionServices _fileExtensionServices;
        private readonly IFileServices _fileServices;
        private readonly DefErrorEFRepository _defErrorEFRepository;
        private readonly RstProjectMediaDetailEFRepository _rstProjectMediaDetailEFRepository;
        private readonly RstProjectMediaEFRepository _rstProjectMediaEFRepository;

        public RstProjectMediaDetailServices(EpicSchemaDbContext dbContext,
            DatabaseOptions databaseOptions,
            IMapper mapper,
            ILogger<RstProjectMediaDetailServices> logger,
            IHttpContextAccessor httpContextAccessor,
            IFileExtensionServices fileExtensionServices,
            IFileServices fileServices)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
            _connectionString = databaseOptions.ConnectionString;
            _httpContext = httpContextAccessor;
            _fileExtensionServices = fileExtensionServices;
            _fileServices = fileServices;
            _defErrorEFRepository = new DefErrorEFRepository(dbContext);
            _rstProjectMediaDetailEFRepository = new RstProjectMediaDetailEFRepository(dbContext, logger);
            _rstProjectMediaEFRepository = new RstProjectMediaEFRepository(dbContext, logger);
        }

        /// <summary>
        /// Thêm nhóm hình ảnh
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public List<RstProjectMediaDetail> Add(CreateRstProjectMediaDetailsDto input)
        {
            _logger.LogInformation($"{nameof(Add)}: input = {JsonSerializer.Serialize(input)}");

            var username = CommonUtils.GetCurrentUsername(_httpContext);
            var partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);

            //var projectMedia = _rstProjectMediaEFRepository.FindById(input.ProjectMediaId).ThrowIfNull<RstProjectMedia>(_dbContext, ErrorCode.RstProjectMediaNotFound);

            var mediaList = new List<RstProjectMediaDetail>();
            var transaction = _dbContext.Database.BeginTransaction();

            var projectMedia = _rstProjectMediaEFRepository.Add(new RstProjectMedia()
            {
                GroupTitle = input.GroupTitle,
                ProjectId = input.ProjectId,
            }, username, partnerId);
            _dbContext.SaveChanges();

            foreach (var item in input.RstProjectMediaDetails)
            {
                var insert = _rstProjectMediaDetailEFRepository.Add(_mapper.Map<RstProjectMediaDetail>(item), username, partnerId);
                insert.ProjectMediaId = projectMedia.Id;
                var media = _fileExtensionServices.GetMediaExtensionFile(insert.UrlImage);
                insert.MediaType = media;
                insert.SortOrder = insert.Id;
                mediaList.Add(insert);
            }

            _dbContext.SaveChanges();
            transaction.Commit();
            return mediaList;
        }

        /// <summary>
        /// update thứ tự nhóm hình ảnh dự án
        /// </summary>
        /// <returns></returns>
        public void UpdateSortOrder(RstProjectMediaDetailSortDto input)
        {
            int partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
            _logger.LogInformation($"{nameof(UpdateSortOrder)}: input = {input}, partnerId = {partnerId}");

            var result = _rstProjectMediaDetailEFRepository.Find(input.ProjectId, partnerId);

            foreach (var item in result)
            {
                var sort = input.Sort.FirstOrDefault(o => o.ProjectMediaDetailId == item.Id);
                var projectMediaDetailFind = _rstProjectMediaDetailEFRepository.FindById(item.Id);
                if (sort != null)
                {
                    projectMediaDetailFind.SortOrder = sort.SortOrder;
                }
            }
            _dbContext.SaveChanges();
        }

        public List<RstProjectMediaDetail> AddListMediaDetail(AddRstProjectMediaDetailsDto input)
        {
            _logger.LogInformation($"{nameof(AddListMediaDetail)}:input = {JsonSerializer.Serialize(input)}");

            var username = CommonUtils.GetCurrentUsername(_httpContext);
            var partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);

            var mediaDetailList = new List<RstProjectMediaDetail>();

            foreach (var item in input.RstProjectMediaDetails)
            {
                var insert = _rstProjectMediaDetailEFRepository.Add(_mapper.Map<RstProjectMediaDetail>(item), username, partnerId);
                insert.ProjectMediaId = input.ProjectMediaId;
                var media = _fileExtensionServices.GetMediaExtensionFile(insert.UrlImage);
                insert.MediaType = media;
                mediaDetailList.Add(insert);
            }
            _dbContext.SaveChanges();
            return mediaDetailList;
        }

        /// <summary>
        /// Cập nhật nhóm hình ảnh
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public RstProjectMediaDetail Update(UpdateRstProjectMediaDetailDto input)
        {
            _logger.LogInformation($"{nameof(Update)}: input = {JsonSerializer.Serialize(input)}");
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            var partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
            var mediaDetailFind = _rstProjectMediaDetailEFRepository.Entity.FirstOrDefault(p => p.Id == input.Id && p.PartnerId == partnerId && p.Deleted == YesNo.NO).ThrowIfNull<RstProjectMediaDetail>(_dbContext, ErrorCode.RstProjectMediaDetailNotFound);
            var mediaFind = _rstProjectMediaEFRepository.FindById(mediaDetailFind.ProjectMediaId, partnerId);
            var updateMedia = _rstProjectMediaDetailEFRepository.Update(_mapper.Map<RstProjectMediaDetail>(input), partnerId, username);
            var media = _fileExtensionServices.GetMediaExtensionFile(updateMedia.UrlImage);
            updateMedia.MediaType = media;

            _dbContext.SaveChanges();
            return updateMedia;
        }

        /// <summary>
        /// Xoá nhóm hình ảnh
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int id)
        {
            _logger.LogInformation($"{nameof(Delete)}: id = {id}");

            //Lấy thông tin đối tác
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            var partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
            var media = _rstProjectMediaDetailEFRepository.Entity.FirstOrDefault(p => p.Id == id && p.PartnerId == partnerId && p.Deleted == YesNo.NO).ThrowIfNull<RstProjectMediaDetail>(_dbContext, ErrorCode.RstProjectMediaDetailNotFound);

            _rstProjectMediaDetailEFRepository.Delete(id, partnerId);
            _fileServices.DeleteFile(media.UrlImage);
            _dbContext.SaveChanges();
        }

        /// <summary>
        /// Tìm nhóm hình ảnh theo id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public RstProjectMediaDetail FindById(int id)
        {
            _logger.LogInformation($"{nameof(FindById)}: id = {id}");
            var partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
            var media = _rstProjectMediaDetailEFRepository.FindById(id, partnerId).ThrowIfNull<RstProjectMediaDetail>(_dbContext, ErrorCode.RstProjectMediaDetailNotFound);

            return media;
        }

        /// <summary>
        /// Đổi trạng thái nhóm hình ảnh
        /// </summary>
        /// <param name="id"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public RstProjectMediaDetail ChangeStatus(int id, string status)
        {
            _logger.LogInformation($"{nameof(ChangeStatus)}: id = {id}, status = {status}");

            var media = _rstProjectMediaDetailEFRepository.FindById(id).ThrowIfNull<RstProjectMediaDetail>(_dbContext, ErrorCode.RstProjectMediaDetailNotFound);

            var changeStatus = _rstProjectMediaDetailEFRepository.ChangeStatus(id, status);
            _dbContext.SaveChanges();
            return changeStatus;
        }

        /// <summary>
        /// Danh sách nhóm hình ảnh dự án order by theo tên nhóm
        /// </summary>
        /// <returns></returns>
        public List<RstProjectMediaDto> Find(int projectId, string status)
        {
            _logger.LogInformation($"{nameof(Find)}");
            var rstProjectMedia = new List<RstProjectMediaDto>();
            var usertype = CommonUtils.GetCurrentUserType(_httpContext);
            int partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
            var listRstProjectMedia = _rstProjectMediaEFRepository.Find(projectId, partnerId);
            var rstProjectMediaFind = _mapper.Map<List<RstProjectMediaDto>>(listRstProjectMedia);

            var listMediaDetail = _rstProjectMediaDetailEFRepository.Find(projectId, partnerId, status);
            foreach (var itemMedia in rstProjectMediaFind)
            {
                if (itemMedia.GroupTitle != null)
                {
                    var listRstProjectMediaDetail = new List<RstProjectMediaDetailDto>();
                    foreach (var item in listMediaDetail)
                    {
                        if (item.ProjectMediaId == itemMedia.Id && item.GroupTitle == itemMedia.GroupTitle)
                        {
                            listRstProjectMediaDetail.Add(item);
                        }
                    }
                    itemMedia.RstProjectMediaDetail = listRstProjectMediaDetail.OrderBy(o => o.SortOrder).ToList();
                    rstProjectMedia.Add(itemMedia);
                }
            }
            return rstProjectMedia;
        }
    }
}
