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
    public class RstProductItemExtendRepository : BaseEFRepository<RstProductItemExtend>
    {
        public RstProductItemExtendRepository(EpicSchemaDbContext dbContext, ILogger logger) : base(dbContext, logger, $"{DbSchemas.EPIC_REAL_ESTATE}.{RstProductItemExtend.SEQ}")
        {
        }

        public void UpdateProductItemExtends(int productItemId, List<RstProductItemExtend> extends, string username)
        {
            _logger.LogInformation($"{nameof(RstProductItemExtendRepository)}-> {nameof(UpdateProductItemExtends)}: projectId = {productItemId}, extends = {JsonSerializer.Serialize(extends)}, username ={username}");

            //Lấy danh sách theo thông tin khác đã có theo dự án
            var extendQuery = _dbSet.Where(t => t.ProductItemId == productItemId && t.Deleted == YesNo.NO);

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
                    _dbSet.Add(new RstProductItemExtend
                    {
                        Id = (int)NextKey(),
                        ProductItemId = productItemId,
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
        public RstProductItemExtend Add(RstProductItemExtend input)
        {
            _logger.LogInformation($"{nameof(RstProductItemExtendRepository)}->{nameof(Add)}: input = {JsonSerializer.Serialize(input)}");
            input.Id = (int)NextKey();
            input.CreatedDate = DateTime.Now;
            return _dbSet.Add(input).Entity;
        }

        /// <summary>
        /// Find by Id
        /// </summary>
        public RstProductItemExtend FindById(int id)
        {
            _logger.LogInformation($"{nameof(RstProductItemExtendRepository)}->{nameof(FindById)}: id = {id}");
            return _dbSet.FirstOrDefault(e => e.Id == id && e.Deleted == YesNo.NO);
        }

        /// <summary>
        /// Get All
        /// </summary>
        public List<RstProductItemExtend> GetAll(int productItemId)
        {
            _logger.LogInformation($"{nameof(RstProductItemExtendRepository)}->{nameof(GetAll)}: productItemId = {productItemId}");
            var result = _dbSet.Where(e => e.ProductItemId == productItemId && e.Deleted == YesNo.NO).OrderByDescending(e => e.Id);
            return result.ToList();
        }
    }
}
