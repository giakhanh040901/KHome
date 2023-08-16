using EPIC.DataAccess.Base;
using EPIC.DataAccess.Models;
using EPIC.InvestEntities.DataEntities;
using EPIC.RealEstateEntities.DataEntities;
using EPIC.RealEstateEntities.Dto.RstProjectStructure;
using EPIC.Utils;
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
    public class RstProjectStructureEFRepository : BaseEFRepository<RstProjectStructure>
    {
        public RstProjectStructureEFRepository(EpicSchemaDbContext dbContext, ILogger logger) : base(dbContext, logger, $"{DbSchemas.EPIC_REAL_ESTATE}.{RstProjectStructure.SEQ}")
        {
        }

        /// <summary>
        /// Thêm cấu trúc cấu thành dự án
        /// </summary>
        /// <param name="input"></param>
        /// <param name="username"></param>
        /// <param name="partnerId"></param>
        /// <returns></returns>
        public RstProjectStructure Add(RstProjectStructure input, string username, int partnerId)
        {
            _logger.LogInformation($"{nameof(RstProjectStructureEFRepository)}->{nameof(Add)}: input = {input}");
            return _dbSet.Add(new RstProjectStructure()
            {
                Id = (int)NextKey(),
                PartnerId = partnerId,
                ProjectId = input.ProjectId,
                Code = input.Code,
                Name = input.Name,
                ParentId = input.ParentId,
                BuildingDensityType = input.BuildingDensityType,
                CreatedBy = username
            }).Entity;
        }

        /// <summary>
        /// Update cấu trúc cấu thành dự án
        /// </summary>
        /// <param name="input"></param>
        /// <param name="partnerId"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        public RstProjectStructure Update(RstProjectStructure input, int partnerId, string username)
        {
            _logger.LogInformation($"{nameof(RstProjectStructureEFRepository)}->{nameof(Update)}: input = {input}, partnerId = {partnerId}, username = {username}");

            var projectStructure = _dbSet.FirstOrDefault(p => p.Id == input.Id && p.PartnerId == partnerId && p.Deleted == YesNo.NO);
            if (projectStructure != null)
            {
                projectStructure.Code = input.Code;
                projectStructure.Name = input.Name;
                projectStructure.ModifiedBy = username;
                projectStructure.ModifiedDate = DateTime.Now;
            }

            return projectStructure;
        }

        /// <summary>
        /// Tìm cấu trúc cấu thành dự án theo id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="partnerId"></param>
        /// <returns></returns>
        public RstProjectStructure FindById(int id, int? partnerId = null)
        {
            _logger.LogInformation($"{nameof(RstProjectStructureEFRepository)}->{nameof(FindById)}: id = {id}");
            var projectStructure = _dbSet.FirstOrDefault(p => p.Id == id && (partnerId == null || p.PartnerId == partnerId) && p.Deleted == YesNo.NO);
            return projectStructure;
        }

        /// <summary>
        /// Danh sách cấu trúc cấu thành dự án
        /// </summary>
        /// <param name="input"></param>
        /// <param name="partnerId"></param>
        /// <returns></returns>
        public PagingResult<RstProjectStructure> FindAll(FilterRstProjectStructureDto input, int? partnerId = null)
        {
            _logger.LogInformation($"{nameof(RstProjectStructureEFRepository)}->{nameof(FindAll)}: input = {input}");

            PagingResult<RstProjectStructure> result = new();
            IQueryable<RstProjectStructure> projectStructureQuery = _dbSet.Where(p => p.Deleted == YesNo.NO && (partnerId == null || p.PartnerId == partnerId));

            if (input.ProjectId != null)
            {
                projectStructureQuery = projectStructureQuery.Where(p => p.ProjectId == input.ProjectId);
            }

            result.TotalItems = projectStructureQuery.Count();

            projectStructureQuery = projectStructureQuery.OrderByDescending(p => p.Id);

            if (input.PageSize != -1)
            {
                projectStructureQuery = projectStructureQuery.Skip(input.Skip).Take(input.PageSize);
            }

            result.Items = projectStructureQuery.ToList();
            return result;
        }

        /// <summary>
        /// Tìm danh sách cấu trúc cấu thành dự án theo projectId
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="partnerId"></param>
        /// <returns></returns>
        public List<RstProjectStructure> FindByProjectId(int projectId, int? partnerId = null)
        {
            _logger.LogInformation($"{nameof(RstProjectStructureEFRepository)}->{nameof(FindByProjectId)}: projectId = {projectId}, partnerId = {partnerId}");

            var listStructure = _dbSet.Where(p => p.ProjectId == projectId && (partnerId == null || p.PartnerId == partnerId) && p.Deleted == YesNo.NO).OrderByDescending(p => p.Id).ToList();

            return listStructure;
        }

        /// <summary>
        /// Lấy danh sách các mật độ xây dựng con
        /// </summary>
        public IQueryable<RstProjectStructure> FindAllChildByProjectId(int projectId)
        {
            _logger.LogInformation($"{nameof(RstProjectStructureEFRepository)}->{nameof(FindByProjectId)}: projectId = {projectId}");

            var listStructure = _dbSet.Where(p => p.ProjectId == projectId && p.Deleted == YesNo.NO).OrderByDescending(p => p.Id);
            var structureWithoutRoot = _dbSet.Where(p => p.ProjectId == projectId && p.ParentId != null && p.Deleted == YesNo.NO);
            var result = listStructure.Where(s => !structureWithoutRoot.Any(l => l.ParentId == s.Id));
            return result;
        }
    }
}
