using EPIC.DataAccess.Base;
using EPIC.DataAccess.Models;
using EPIC.Entities.DataEntities;
using EPIC.RealEstateEntities.DataEntities;
using EPIC.RealEstateEntities.Dto.RstProjectMedia;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.RealEstate;
using EPIC.Utils.ConstantVariables.Shared;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace EPIC.RealEstateRepositories
{
    public class RstProjectMediaEFRepository : BaseEFRepository<RstProjectMedia>
    {
        public RstProjectMediaEFRepository(EpicSchemaDbContext dbContext, ILogger logger) : base(dbContext, logger, $"{DbSchemas.EPIC_REAL_ESTATE}.{RstProjectMedia.SEQ}")
        {
        }

        /// <summary>
        /// Thêm hình ảnh dự án
        /// </summary>
        /// <param name="input"></param>
        /// <param name="username"></param>
        /// <param name="partnerId"></param>
        /// <returns></returns>
        public RstProjectMedia Add(RstProjectMedia input, string username, int partnerId)
        {
            _logger.LogInformation($"{nameof(RstProjectMediaEFRepository)}->{nameof(Add)}: input = {JsonSerializer.Serialize(input)}, username = {username}, partnerId = {partnerId}");
            return _dbSet.Add(new RstProjectMedia()
            {
                Id = (int)NextKey(),
                PartnerId = partnerId,
                ProjectId = input.ProjectId,
                GroupTitle = input.GroupTitle,
                UrlImage = input.UrlImage,
                UrlPath = input.UrlPath,
                Location = input.Location,
                CreatedBy = username
            }).Entity;
        }

        /// <summary>
        /// Cập nhật hình ảnh dự án 
        /// </summary>
        /// <param name="input"></param>
        /// <param name="partnerId"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        public RstProjectMedia Update(RstProjectMedia input, int partnerId, string username)
        {
            _logger.LogInformation($"{nameof(RstProjectMediaEFRepository)}->{nameof(Update)}: input = {JsonSerializer.Serialize(input)}, username = {username}, partnerId = {partnerId}");

            var media = _dbSet.FirstOrDefault(p => p.Id == input.Id && p.PartnerId == partnerId && p.Deleted == YesNo.NO);
            if (media != null)
            {
                media.GroupTitle = input.GroupTitle;
                media.UrlImage = input.UrlImage;
                media.Location = input.Location;
                media.ModifiedBy = username;
                media.ModifiedDate = DateTime.Now;
            }

            return media;
        }

        /// <summary>
        /// Tìm hình ảnh dự án theo id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="partnerId"></param>
        /// <returns></returns>
        public RstProjectMedia FindById(int id, int? partnerId = null)
        {
            _logger.LogInformation($"{nameof(RstProjectMediaEFRepository)}->{nameof(FindById)}: id = {id}, partnerId = {partnerId}");

            return _dbSet.FirstOrDefault(p => p.Id == id && (partnerId == null || p.PartnerId == partnerId) && p.Deleted == YesNo.NO);
        }

        /// <summary>
        /// Thay đổi trạng thái hình ảnh dự án 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="status"></param>
        /// <param name="partnerId"></param>
        /// <returns></returns>
        public RstProjectMedia ChangeStatus(int id, string status, int? partnerId = null)
        {
            _logger.LogInformation($"{nameof(RstProjectMediaEFRepository)}->{nameof(ChangeStatus)}: id = {id}, partnerId = {partnerId}");

            var media = _dbSet.FirstOrDefault(p => p.Id == id && (partnerId == null || p.PartnerId == partnerId) && p.Deleted == YesNo.NO);

            if (media != null)
            {
                media.Status = status;
            }

            return media;
        }

        /// <summary>
        /// Danh sách hình ảnh dự án có phân trang
        /// </summary>
        /// <param name="input"></param>
        /// <param name="partnerId"></param>
        /// <returns></returns>
        public PagingResult<RstProjectMedia> FindAll(FilterRstProjectMediaDto input, int? partnerId = null)
        {
            _logger.LogInformation($"{nameof(RstProjectMediaEFRepository)}->{nameof(FindAll)}: input = {input}, partnerId = {partnerId}");

            PagingResult<RstProjectMedia> result = new();
            IQueryable<RstProjectMedia> mediaQuery = _dbSet.Where(p => p.Deleted == YesNo.NO && (partnerId == null || p.PartnerId == partnerId));

            if (input.Status != null)
            {
                mediaQuery = mediaQuery.Where(p => p.Status == input.Status);
            }

            if (input.Location != null)
            {
                mediaQuery = mediaQuery.Where(p => p.Location == input.Location);
            }

            if (input.ProjectId != null)
            {
                mediaQuery = mediaQuery.Where(p => p.ProjectId == input.ProjectId);
            }

            if (input.MediaType != null)
            {
                mediaQuery = mediaQuery.Where(p => p.MediaType == input.MediaType);
            }

            result.TotalItems = mediaQuery.Count();

            mediaQuery = mediaQuery.OrderByDescending(p => p.Id);

            if (input.PageSize != -1)
            {
                mediaQuery = mediaQuery.Skip(input.Skip).Take(input.PageSize);
            }

            result.Items = mediaQuery.ToList();
            return result;
        }

        /// <summary>
        /// Danh sách hình ảnh dự án orderBy location
        /// </summary>
        /// <param name="partnerId"></param>
        /// <returns></returns>
        public IQueryable<RstProjectMedia> Find(int projectId, int? partnerId = null, string location = null, string status = null)
        {
            _logger.LogInformation($"{nameof(RstProjectMediaEFRepository)}->{nameof(Find)}: projectId = {projectId} partnerId = {partnerId}," +
                $" location = {location}, status = {status}");

            var mediaList = _dbSet.Where(o => o.ProjectId == projectId && o.Deleted == YesNo.NO 
            && (partnerId == null || o.PartnerId == partnerId)
            && (location == null || o.Location == location)
            && (status == null || o.Status == status));
            mediaList = mediaList.OrderByDescending(o => o.Location).ThenBy(o => o.SortOrder);
            return mediaList;
        }

        /// <summary>
        /// Lấy danh sách ảnh theo location
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="location"></param>
        /// <returns></returns>
        public List<RstProjectMedia> FindById(int projectId, string location)
        {
            _logger.LogInformation($"{nameof(RstProjectMediaEFRepository)}->{nameof(Find)}: projectId = {projectId}; location = {location}");
            var mediaList = _dbSet.Where(o => o.ProjectId == projectId && o.Deleted == YesNo.NO && o.Location == location).ToList();

            return mediaList;
        }

        /// <summary>
        /// Lấy url hình ảnh đơn của dự án
        /// </summary>
        /// <returns></returns>
        public RstProjectMedia GetByLocation(int projectId, string location)
        {
            _logger.LogInformation($"{nameof(RstProjectMediaEFRepository)}->{nameof(GetByLocation)}: projectId = {projectId}; location = {location}");
            return _dbSet.FirstOrDefault(o => o.ProjectId == projectId && o.Deleted == YesNo.NO && o.Location == location);
        }

        /// <summary>
        /// Lấy media banner quảng cáo theo id dự án
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        public List<RstProjectMedia> AppFindBannerQuangCaoById(int projectId)
        {
            _logger.LogInformation($"{nameof(RstProjectMediaEFRepository)}->{nameof(AppFindBannerQuangCaoById)}: projectId = {projectId}");
            return FindById(projectId, RstMediaLocations.BANNER_QUANG_CAO_DU_AN);
        }

        /// <summary>
        /// Xoá hình ảnh dự án 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="partnerId"></param>
        public void Delete(int id, int? partnerId = null)
        {
            _logger.LogInformation($"{nameof(RstProjectMediaEFRepository)}->{nameof(Delete)}: id = {id}, partnerId = {partnerId}");

            var media = _dbSet.FirstOrDefault(o => o.Id == id && (partnerId == null || o.PartnerId == partnerId) && o.Deleted == YesNo.NO);
            if (media != null)
            {
                _dbContext.Remove(media);
            }
        }
    }
}
