using EPIC.DataAccess.Base;
using EPIC.DataAccess.Models;
using EPIC.RealEstateEntities.DataEntities;
using EPIC.RealEstateEntities.Dto.RstProjectFile;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.RealEstate;
using EPIC.Utils.ConstantVariables.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.RealEstateRepositories
{
    public class RstProjectFileEFRepository : BaseEFRepository<RstProjectFile>
    {
        public RstProjectFileEFRepository(EpicSchemaDbContext dbContext, ILogger logger) : base(dbContext, logger, $"{DbSchemas.EPIC_REAL_ESTATE}.{RstProjectFile.SEQ}")
        {
        }

        /// <summary>
        /// Thêm hồ sơ dự án
        /// </summary>
        /// <param name="input"></param>
        /// <param name="username"></param>
        /// <param name="partnerId"></param>
        /// <returns></returns>
        public RstProjectFile Add(RstProjectFile input, string username, int partnerId)
        {
            _logger.LogInformation($"{nameof(RstProjectFileEFRepository)}->{nameof(Add)}: input = {input}");
            return _dbSet.Add(new RstProjectFile()
            {
                Id = (int)NextKey(),
                PartnerId = partnerId,
                ProjectId = input.ProjectId,
                Name = input.Name,
                Url = input.Url,
                ProjectFileType = input.ProjectFileType,
                CreatedDate = DateTime.Now,
                CreatedBy = username
            }).Entity;
        }

        /// <summary>
        /// Cập nhật hồ sơ dự án
        /// </summary>
        /// <param name="input"></param>
        /// <param name="partnerId"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        public RstProjectFile Update(RstProjectFile input, int partnerId, string username)
        {
            _logger.LogInformation($"{nameof(RstProjectFileEFRepository)}->{nameof(Update)}: input = {input}");

            var juridicalFile = _dbSet.FirstOrDefault(p => p.Id == input.Id && p.PartnerId == partnerId && p.Deleted == YesNo.NO);
            if (juridicalFile != null)
            {
                juridicalFile.Name = input.Name;
                juridicalFile.Url = input.Url;
                juridicalFile.ModifiedBy = username;
                juridicalFile.ModifiedDate = DateTime.Now;
            }

            return juridicalFile;
        }

        /// <summary>
        /// Tìm kiếm hồ sơ dự án
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public RstProjectFile FindById(int id)
        {
            _logger.LogInformation($"{nameof(RstProjectFileEFRepository)}->{nameof(FindById)}: id = {id}");

            return _dbSet.FirstOrDefault(p => p.Id == id && p.Deleted == YesNo.NO);
        }

        /// <summary>
        /// Thay đổi trạng thái hồ sơ dự án từ đang hiển thị -> xoá 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public RstProjectFile ChangeStatus(int id, string status)
        {
            _logger.LogInformation($"{nameof(RstProjectFileEFRepository)}->{nameof(ChangeStatus)}: id = {id}");

            var juridicalFile = _dbSet.FirstOrDefault(p => p.Id == id && p.Deleted == YesNo.NO);

            if (juridicalFile != null)
            {
                juridicalFile.Status = status;
            }

            return juridicalFile;
        }

        /// <summary>
        /// Tìm kiếm danh sách hồ sơ dự án
        /// </summary>
        /// <param name="input"></param>
        /// <param name="partnerId"></param>
        /// <returns></returns>
        public PagingResult<RstProjectFile> FindAll(FilterRstProjectFileDto input, int? partnerId = null)
        {
            _logger.LogInformation($"{nameof(RstProjectFileEFRepository)}->{nameof(FindAll)}: input = {input}");

            PagingResult<RstProjectFile> result = new();
            IQueryable<RstProjectFile> juridicalFileQuery = _dbSet.Where(p => p.Deleted == YesNo.NO
                && (partnerId == null || p.PartnerId == partnerId)
                && (input.Name == null || p.Name.ToLower().Contains(input.Name.ToLower()))
                && (input.Status == null || p.Status == input.Status)
                && (input.ProjectFileType == null || p.ProjectFileType == input.ProjectFileType));

            if (input.ProjectId != null)
            {
                juridicalFileQuery = juridicalFileQuery.Where(p => p.ProjectId == input.ProjectId);
            }
            result.TotalItems = juridicalFileQuery.Count();

            juridicalFileQuery = juridicalFileQuery.OrderByDescending(p => p.Id);

            if (input.PageSize != -1)
            {
                juridicalFileQuery = juridicalFileQuery.Skip(input.Skip).Take(input.PageSize);
            }

            result.Items = juridicalFileQuery.ToList();
            return result;
        }

        /// <summary>
        /// App lấy hồ sơ pháp lý active
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="status"></param>
        /// <param name="type"><see cref="RstProjectFileTypes"/></param>
        /// <returns></returns>
        public List<RstProjectFile> AppFindByProjectId(int projectId, int type)
        {
            _logger.LogInformation($"{nameof(RstProjectFileEFRepository)}->{nameof(FindAll)}: projectId = {projectId}");

            return _dbSet.AsNoTracking().Where(x => x.ProjectId == projectId && x.ProjectFileType == type && x.Deleted == YesNo.NO).ToList();
        }
    }
}
