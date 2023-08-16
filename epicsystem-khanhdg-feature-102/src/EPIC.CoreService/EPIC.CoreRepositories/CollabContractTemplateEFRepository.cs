using EPIC.CoreEntities.Dto.CollabContractTemp;
using EPIC.DataAccess.Base;
using EPIC.DataAccess.Models;
using EPIC.Entities.DataEntities;
using EPIC.Entities.Dto.CoreCollabContractTemp;
using EPIC.EntitiesBase.Dto;
using EPIC.Utils;
using EPIC.Utils.Linq;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.CoreRepositories
{
    public class CollabContractTemplateEFRepository : BaseEFRepository<CollabContractTemp>
    {

        public CollabContractTemplateEFRepository(EpicSchemaDbContext dbContext, ILogger logger) : base(dbContext, logger, CollabContractTemp.SEQ)
        {
        }

        public PagingResult<ViewCollabContractTempDto> FindAll(FilterCollabContractTempDto input)
        {
            var query = _epicSchemaDbContext.CollabContractTemps.Where(c => c.Deleted == YesNo.NO && (input.TradigProviderId == null || c.TradingProviderId == input.TradigProviderId)
                                                                       && (input.Type == null || c.Type == input.Type)
                                                                       && (input.Keyword == null || c.Title.ToLower().Contains(input.Keyword.ToLower())))
                                                                .Select(c => new ViewCollabContractTempDto
                                                                {
                                                                    Id = c.Id,
                                                                    TradingProviderId = c.TradingProviderId,
                                                                    Title = c.Title,
                                                                    Type = c.Type,
                                                                    FileUrl = c.FileUrl,
                                                                    Status = c.Status,
                                                                });
            var result = new PagingResult<ViewCollabContractTempDto>();
            result.TotalItems = query.Count();
            query = query.OrderDynamic(input.Sort);
            if (input.PageSize != -1)
            {
                query = query.Skip(input.Skip).Take(input.PageSize);
            }
            result.Items = query;
            return result;
        }
    }
}
