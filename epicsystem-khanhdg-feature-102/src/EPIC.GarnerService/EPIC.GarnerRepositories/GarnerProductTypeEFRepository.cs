using EPIC.DataAccess.Base;
using EPIC.Entities.DataEntities;
using EPIC.GarnerEntities.DataEntities;
using EPIC.Utils.ConstantVariables.Shared;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace EPIC.GarnerRepositories
{
    public class GarnerProductTypeEFRepository : BaseEFRepository<GarnerProductType>
    {
        public GarnerProductTypeEFRepository(EpicSchemaDbContext dbContext, ILogger logger) : base(dbContext, logger, $"{DbSchemas.EPIC_GARNER}.{GarnerProductType.SEQ}")
        {
        }

        public List<GarnerProductType> FindAllByProductId(int productId)
        {
            _logger.LogInformation($"{System.Reflection.MethodBase.GetCurrentMethod().Name}: productId: {productId}");
            return _dbSet.Where(p => p.ProductId == productId).ToList();
        }

        public void UpdateProductType(int productId, List<int> input, int partnerId, string username)
        {
            _logger.LogInformation($"{System.Reflection.MethodBase.GetCurrentMethod().Name}: input = {JsonSerializer.Serialize(input)}, productId= {productId}, partnerId= {partnerId}, username = {username}");
            //Lấy danh sách loại sản phẩm đã có trong db
            var productTypeFind = FindAllByProductId(productId);

            //Xóa đi những đại lý không được truyền vào
            var productTypeRemove = productTypeFind.Where(p => !input.Contains(p.Type)).ToList();
            foreach (var productTypeItem in productTypeRemove)
            {
                _dbSet.Remove(productTypeItem);
            }

            foreach (var item in input)
            {
                //Nếu loại sản phẩm chưa có trong list thì thêm vào, nếu đã có thì giữ nguyên
                if( !productTypeFind.Select(p => p.Type).Contains(item))
                {
                    _dbSet.Add(new GarnerProductType
                    {
                        Id = (int)NextKey(),
                        ProductId = productId,
                        Type = item,
                        CreatedBy = username,
                        CreatedDate = DateTime.Now
                    });
                }    
            }
        }
    }
}
