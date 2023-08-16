using DocumentFormat.OpenXml.Drawing.ChartDrawing;
using EPIC.DataAccess.Base;
using EPIC.Entities.DataEntities;
using EPIC.Entities.Dto.ManagerInvestor;
using EPIC.RealEstateEntities.DataEntities;
using EPIC.RealEstateEntities.Dto.RstOrder;
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
    public class RstOrderCoOwnerRepository : BaseEFRepository<RstOrderCoOwner>
    {
        public RstOrderCoOwnerRepository(EpicSchemaDbContext dbContext, ILogger logger) : base(dbContext, logger, $"{DbSchemas.EPIC_REAL_ESTATE}.{RstOrderCoOwner.SEQ}")
        {
        }

        public RstOrderCoOwner Add(RstOrderCoOwner entity)
        {
            _logger.LogInformation($"{nameof(RstOrderCoOwnerRepository)}->{nameof(Add)}: {JsonSerializer.Serialize(entity)}");
            entity.Id = (int)NextKey();
            return _dbSet.Add(entity).Entity;
        }
        
        public RstOrderCoOwner FindById(int id, int orderId)
        {
            _logger.LogInformation($"{nameof(RstOrderCoOwnerRepository)}->{nameof(FindById)}: id = {id}");
            var orderCoOwner = _dbSet.FirstOrDefault(o => o.Id == id && o.OrderId == orderId && o.Deleted == YesNo.NO);
            return orderCoOwner;
        }

        public RstOrderCoOwner Update(RstOrderCoOwner input)
        {
            _logger.LogInformation($"{nameof(RstOrderCoOwnerRepository)}->{nameof(Update)}: {JsonSerializer.Serialize(input)}");
            var orderCoOwnerFind = _dbSet.FirstOrDefault(o => o.Id == input.Id && o.Deleted == YesNo.NO).ThrowIfNull<RstOrderCoOwner>(_epicSchemaDbContext, ErrorCode.RstOrderCoOwnerNotFound);
            return _dbSet.Update(input).Entity;
        }

        /// <summary>
        /// Danh sách người đồng sở hữu của Lệnh
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public List<AppOrderCoOwnersDto> AppOrderCoOwners(int orderId)
        {
            _logger.LogInformation($"{nameof(RstOrderCoOwnerRepository)}->{nameof(AppOrderCoOwners)}: orderId = {orderId}");
            List<AppOrderCoOwnersDto> result = new List<AppOrderCoOwnersDto>();
            var orderCoOwnerQuery = _dbSet.Where(o => o.OrderId == orderId && o.Deleted == YesNo.NO);
            foreach (var item in orderCoOwnerQuery)
            {
                var identificationQuery = (from identification in _epicSchemaDbContext.InvestorIdentifications
                                           join investor in _epicSchemaDbContext.Investors on identification.InvestorId equals investor.InvestorId
                                           where identification.Id == item.InvestorIdenId
                                           where identification.Deleted == YesNo.NO
                                           && investor.Deleted == YesNo.NO
                                           select new AppOrderCoOwnersDto
                                           {
                                               Fullname = identification.Fullname,
                                               Address = investor.Address,
                                               Phone = investor.Phone,
                                               CreatedDate = item.CreatedDate
                                           }).FirstOrDefault();
                if (identificationQuery != null)
                {
                    result.Add(identificationQuery);
                }
                else if (item.Fullname != null)
                {
                    result.Add(new AppOrderCoOwnersDto()
                    {
                        Fullname = item.Fullname,
                        Address = item.Address,
                        Phone = item.Phone,
                        CreatedDate = item.CreatedDate
                    });
                }
            }
            return result;
        }
    }
}
