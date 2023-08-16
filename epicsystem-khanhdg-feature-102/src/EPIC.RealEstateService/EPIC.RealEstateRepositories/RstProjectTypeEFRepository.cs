using EPIC.DataAccess.Base;
using EPIC.GarnerEntities.DataEntities;
using EPIC.GarnerEntities.Dto.GarnerProductOverview;
using EPIC.RealEstateEntities.DataEntities;
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
    public class RstProjectTypeEFRepository : BaseEFRepository<RstProjectType>
    {
        public RstProjectTypeEFRepository(EpicSchemaDbContext dbContext, ILogger logger) : base(dbContext, logger, $"{DbSchemas.EPIC_REAL_ESTATE}.{RstProjectType.SEQ}")
        {
        }

        public List<int> FindAllByProjectId(int projectId, int projectType)
        {
            return _dbSet.Where(t => t.ProjectId == projectId && t.ProjectType == projectType).Select(r => r.Type).ToList();
        }

        public void UpdateProjectTypes(int projectId, int projectType, List<int> types)
        {
            _logger.LogInformation($"{nameof(RstProjectTypeEFRepository)}-> {nameof(UpdateProjectTypes)}: input = {JsonSerializer.Serialize(types)}");

            // Trường hợp null thì trả về mảng rỗng
            types = (types != null ? types : new List<int>());

            //Lấy danh sách theo loại hình sản phẩm
            var listTypes = _dbSet.Where(t => t.ProjectId == projectId && t.ProjectType == projectType);

            //Xóa đi những loại không được truyền vào
            var typesRemove = listTypes.Where(p => !types.Contains(p.Type));
            foreach (var typeItem in typesRemove)
            {
                _dbSet.Remove(typeItem);
            }

            foreach (var typeItem in types)
            {
                //Nếu là thêm mới thì thêm vào
                if (!listTypes.Select(r => r.Type).Contains(typeItem))
                {
                    _dbSet.Add(new RstProjectType
                    {
                        Id = (int)NextKey(),
                        ProjectId = projectId,
                        Type = typeItem,
                        ProjectType = projectType
                    });
                }
            }
        }
    }
}
