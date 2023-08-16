using EPIC.DataAccess.Base;
using EPIC.RealEstateEntities.DataEntities;
using EPIC.RealEstateEntities.Dto.RstProjectMediaDetail;
using EPIC.Utils;
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
    public class RstProjectMediaDetailEFRepository : BaseEFRepository<RstProjectMediaDetail>
    {
        public RstProjectMediaDetailEFRepository(EpicSchemaDbContext dbContext, ILogger logger) : base(dbContext, logger, $"{DbSchemas.EPIC_REAL_ESTATE}.{RstProjectMediaDetail.SEQ}")
        {
        }

        /// <summary>
        /// Thêm nhóm hình ảnh
        /// </summary>
        /// <param name="input"></param>
        /// <param name="username"></param>
        /// <param name="partnerId"></param>
        /// <returns></returns>
        public RstProjectMediaDetail Add(RstProjectMediaDetail input, string username, int partnerId)
        {
            _logger.LogInformation($"{nameof(RstProjectMediaDetailEFRepository)}->{nameof(Add)}: input = {JsonSerializer.Serialize(input)}, username = {username}, partnerId = {partnerId}");
            return _dbSet.Add(new RstProjectMediaDetail()
            {
                Id = (int)NextKey(),
                PartnerId = partnerId,
                UrlImage = input.UrlImage,
                ProjectMediaId = input.ProjectMediaId,
                CreatedBy = username
            }).Entity;
        }

        /// <summary>
        /// Cập nhật nhóm hình ảnh
        /// </summary>
        /// <param name="input"></param>
        /// <param name="partnerId"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        public RstProjectMediaDetail Update(RstProjectMediaDetail input, int partnerId, string username)
        {
            _logger.LogInformation($"{nameof(RstProjectMediaDetailEFRepository)}->{nameof(Update)}: input = {JsonSerializer.Serialize(input)}, username = {username}, partnerId = {partnerId}");

            var media = _dbSet.FirstOrDefault(p => p.Id == input.Id && p.PartnerId == partnerId && p.Deleted == YesNo.NO);
            if (media != null)
            {
                media.UrlImage = input.UrlImage;
                media.ModifiedBy = username;
                media.ModifiedDate = DateTime.Now;
            }

            return media;
        }

        /// <summary>
        /// Tìm nhóm hình ảnh theo id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="partnerId"></param>
        /// <returns></returns>
        public RstProjectMediaDetail FindById(int id, int? partnerId = null)
        {
            _logger.LogInformation($"{nameof(RstProjectMediaDetailEFRepository)}->{nameof(FindById)}: id = {id}, partnerId = {partnerId}");

            return _dbSet.FirstOrDefault(p => p.Id == id && (partnerId == null || p.PartnerId == partnerId) && p.Deleted == YesNo.NO);
        }

        /// <summary>
        /// Thay đổi trạng thái nhóm hình ảnh
        /// </summary>
        /// <param name="id"></param>
        /// <param name="status"></param>
        /// <param name="partnerId"></param>
        /// <returns></returns>
        public RstProjectMediaDetail ChangeStatus(int id, string status, int? partnerId = null)
        {
            _logger.LogInformation($"{nameof(RstProjectMediaDetailEFRepository)}->{nameof(ChangeStatus)}: id = {id}, partnerId = {partnerId}, status = {status}");

            var media = _dbSet.FirstOrDefault(p => p.Id == id && (partnerId == null || p.PartnerId == partnerId) && p.Deleted == YesNo.NO);

            if (media != null)
            {
                media.Status = status;
            }

            return media;
        }

        /// <summary>
        /// Tìm nhóm hình ảnh order by theo tên nhóm hình ảnh
        /// </summary>
        /// <param name="partnerId"></param>
        /// <returns></returns>
        public List<RstProjectMediaDetailDto> Find(int projectId, int? partnerId = null, string status = null)
        {
            _logger.LogInformation($"{nameof(RstProjectMediaDetailEFRepository)}->{nameof(Find)}: projectId = {projectId}, partnerId = {partnerId}" +
                $"status = {status}");

            //var mediaList = _dbSet.Where(o => o.Deleted == YesNo.NO && (partnerId == null || o.PartnerId == partnerId)).OrderByDescending(o => o.ProjectMediaId).ToList();

            var mediaList = from mediaDetail in _dbSet
                            join media in _epicSchemaDbContext.RstProjectMedias on mediaDetail.ProjectMediaId equals media.Id
                            where (mediaDetail.PartnerId == partnerId || partnerId == null) && mediaDetail.Deleted == YesNo.NO
                            && media.ProjectId == projectId && (status == null || mediaDetail.Status == status)
                            orderby media.GroupTitle descending
                            select new RstProjectMediaDetailDto()
                            {
                                Id = mediaDetail.Id,
                                PartnerId = mediaDetail.PartnerId,
                                GroupTitle = media.GroupTitle,
                                ProjectMediaId = mediaDetail.ProjectMediaId,
                                MediaType = mediaDetail.MediaType,
                                UrlImage = mediaDetail.UrlImage,
                                Status = mediaDetail.Status,
                                Location = media.Location,
                                SortOrder = mediaDetail.SortOrder,
                            };

            return mediaList.ToList();
        }

        /// <summary>
        /// Tìm nhóm hình ảnh theo ProjectMediaId
        /// </summary>
        /// <param name="partnerId"></param>
        /// <returns></returns>
        public List<RstProjectMediaDetailDto> FindByProjectMediaId(int projectMediaId, int? partnerId = null)
        {
            _logger.LogInformation($"{nameof(RstProjectMediaDetailEFRepository)}->{nameof(FindByProjectMediaId)}: projectMediaId = {projectMediaId}, partnerId = {partnerId}");

            //var mediaList = _dbSet.Where(o => o.Deleted == YesNo.NO && (partnerId == null || o.PartnerId == partnerId)).OrderByDescending(o => o.ProjectMediaId).ToList();

            var mediaList = from mediaDetail in _dbSet
                            join media in _epicSchemaDbContext.RstProjectMedias on mediaDetail.ProjectMediaId equals media.Id
                            where (mediaDetail.PartnerId == partnerId || partnerId == null) && mediaDetail.Deleted == YesNo.NO
                            && media.Id == projectMediaId
                            orderby media.GroupTitle descending
                            select new RstProjectMediaDetailDto()
                            {
                                Id = mediaDetail.Id,
                                PartnerId = mediaDetail.PartnerId,
                                GroupTitle = media.GroupTitle,
                                ProjectMediaId = mediaDetail.ProjectMediaId,
                                MediaType = mediaDetail.MediaType,
                                UrlImage = mediaDetail.UrlImage,
                                Status = mediaDetail.Status
                            };

            return mediaList.ToList();
        }

        /// <summary>
        /// Xoá hình ảnh trong nhóm 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="partnerId"></param>
        public void Delete(int id, int? partnerId = null)
        {
            _logger.LogInformation($"{nameof(RstProjectMediaEFRepository)}->{nameof(Find)}: id = {id}, partnerId = {partnerId}");

            var media = _dbSet.FirstOrDefault(o => o.Id == id && (partnerId == null || o.PartnerId == partnerId) && o.Deleted == YesNo.NO);
            if (media != null)
            {
                _dbContext.Remove(media);
            }
        }
    }
}
