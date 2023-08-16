using EPIC.DataAccess.Base;
using EPIC.RealEstateEntities.DataEntities;
using EPIC.Utils.ConstantVariables.Shared;
using EPIC.Utils;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

namespace EPIC.RealEstateRepositories
{
    public class RstProjectExtendEFRepository : BaseEFRepository<RstProjectExtend>
    {
        public RstProjectExtendEFRepository(EpicSchemaDbContext dbContext, ILogger logger) : base(dbContext, logger, $"{DbSchemas.EPIC_REAL_ESTATE}.{RstProjectExtend.SEQ}")
        {
        }

        /// <summary>
        /// Thêm thông tin mở rộng
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="extends"></param>
        /// <param name="username"></param>
        public void UpdateProjectExtends(int projectId, List<RstProjectExtend> extends, string username)
        {
            _logger.LogInformation($"{nameof(RstProjectExtendEFRepository)}-> {nameof(UpdateProjectExtends)}: projectId = {projectId}, extends = {JsonSerializer.Serialize(extends)}, username ={username}");

            //Lấy danh sách theo thông tin khác đã có theo dự án
            var extendQuery = _dbSet.Where(t => t.ProjectId == projectId && t.Deleted == YesNo.NO);

            //Xóa đi những thông tin không được truyền vào
            var extendRemove = extendQuery.Where(p => !extends.Select(e => e.Id).Contains(p.Id));
            foreach (var extendItem in extendRemove)
            {
                extendItem.Deleted = YesNo.YES;
                extendItem.ModifiedBy = username;
                extendItem.ModifiedDate = DateTime.Now;
            }

            foreach (var extendItem in extends)
            {
                //Nếu là thêm mới thì thêm vào
                if (!extendQuery.Select(r => r.Id).Contains(extendItem.Id))
                {
                    _dbSet.Add(new RstProjectExtend
                    {
                        Id = (int)NextKey(),
                        ProjectId = projectId,
                        Title = extendItem.Title,
                        IconName = extendItem.IconName,
                        Description = extendItem.Description,
                        CreatedDate = DateTime.Now,
                        CreatedBy = username,
                    });
                }
                else
                {
                    var extendFind = _dbSet.FirstOrDefault(e => e.Id == extendItem.Id && e.Deleted == YesNo.NO);
                    if (extendFind != null)
                    {
                        extendFind.Title = extendItem.Title;
                        extendFind.Description = extendItem.Description;
                        extendFind.IconName = extendItem.IconName;
                    }
                }
            }
        }

        /// <summary>
        /// Add
        /// </summary>
        public RstProjectExtend Add(RstProjectExtend input)
        {
            _logger.LogInformation($"{nameof(RstProjectExtendEFRepository)}->{nameof(Add)}: input = {JsonSerializer.Serialize(input)}");
            input.Id = (int)NextKey();
            input.CreatedDate = DateTime.Now;
            return _dbSet.Add(input).Entity;
        }

        /// <summary>
        /// Find by Id
        /// </summary>
        public RstProjectExtend FindById(int id, int partnerId, int projectId)
        {
            _logger.LogInformation($"{nameof(RstProjectExtendEFRepository)}->{nameof(FindById)}: id = {id}, parnerId = {partnerId}");
            return _dbSet.FirstOrDefault(e => e.Id == id && e.ProjectId == projectId && e.Deleted == YesNo.NO);
        }

        /// <summary>
        /// Get All
        /// </summary>
        public List<RstProjectExtend> GetAll(int projectId)
        {
            _logger.LogInformation($"{nameof(RstProjectExtendEFRepository)}->{nameof(GetAll)}: projectId = {projectId}");
            var result = _dbSet.Where(e => e.ProjectId == projectId && e.Deleted == YesNo.NO).OrderByDescending(e => e.Id);
            return result.ToList();
        }
    }
}
