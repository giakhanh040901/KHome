using EPIC.DataAccess.Base;
using EPIC.RealEstateEntities.DataEntities;
using EPIC.RealEstateEntities.Dto.RstProjectInformationShare;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Shared;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace EPIC.RealEstateRepositories
{
    public class RstProjectInformationShareDetailEFRepository : BaseEFRepository<RstProjectInformationShareDetail>
    {
        public RstProjectInformationShareDetailEFRepository(EpicSchemaDbContext dbContext, ILogger logger) : base(dbContext, logger, $"{DbSchemas.EPIC_REAL_ESTATE}.{RstProjectInformationShareDetail.SEQ}")
        {
        }

        public void Update(int projectShareId, int type, string username, List<UpdateRstProjectInfoShareDetailDto> input)
        {
            _logger.LogInformation($"{nameof(RstProjectInformationShareDetailEFRepository)}-> {nameof(Update)}: input = {JsonSerializer.Serialize(input)}");

            // Trường hợp null thì trả về mảng rỗng
            input = (input != null ? input : new List<UpdateRstProjectInfoShareDetailDto>());

            //Lấy danh sách theo loại hình sản phẩm
            var listFile = _dbSet.Where(t => t.ProjectShareId == projectShareId && t.Type == type && t.Deleted == YesNo.NO);

            //Xóa đi những loại không được truyền vào
            var fileRemove = listFile.Where(p => !input.Select(x => x.Id).Contains(p.Id));
            foreach (var item in fileRemove)
            {
                item.Deleted = YesNo.YES;
                item.ModifiedBy = username;
                item.ModifiedDate = DateTime.Now;
            }

            foreach (var item in input)
            {
                //Nếu là thêm mới thì thêm vào
                if (!listFile.Select(x => x.Id).Contains(item.Id ?? 0))
                {
                    _dbSet.Add(new RstProjectInformationShareDetail
                    {
                        Id = (int)NextKey(),
                        ProjectShareId = projectShareId,
                        Type = type,
                        Name = item.Name,
                        FileUrl = item.FileUrl,
                    });
                }
                else
                {
                    var fileQuery = _dbSet.FirstOrDefault(x => x.Id == item.Id);
                    if (fileQuery != null && (item.Name != fileQuery.Name || item.FileUrl != fileQuery.FileUrl))
                    {
                        fileQuery.Name = item.Name;
                        fileQuery.FileUrl = item.FileUrl;
                        fileQuery.ModifiedBy = username;
                        fileQuery.ModifiedDate = DateTime.Now;
                    }
                }
            }
        }
    }
}
