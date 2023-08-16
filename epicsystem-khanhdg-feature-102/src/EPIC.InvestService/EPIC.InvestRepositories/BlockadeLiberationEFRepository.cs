using EPIC.DataAccess.Base;
using EPIC.DataAccess.Models;
using EPIC.InvestEntities.DataEntities;
using EPIC.InvestEntities.Dto.BlockadeLiberation;
using EPIC.InvestEntities.Dto.Order;
using EPIC.Utils;
using EPIC.Utils.Attributes;
using EPIC.Utils.ConstantVariables.Invest;
using EPIC.Utils.ConstantVariables.Shared;
using EPIC.Utils.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.InvestRepositories
{
    public class BlockadeLiberationEFRepository : BaseEFRepository<BlockadeLiberation>
    {
        public BlockadeLiberationEFRepository(EpicSchemaDbContext dbContext, ILogger logger) : base(dbContext, logger, $"{DbSchemas.EPIC}.{BlockadeLiberation.SEQ}")
        {
        }

        public PagingResult<BlockadeLiberation> FindAll(FilterBlockadeLiberationDto input)
        {
            var result = new PagingResult<BlockadeLiberation>();

            var query = _epicSchemaDbContext.InvestBlockadeLiberations
                    .Include(bl => bl.Order)
                    .Where(bl => bl.Status != null
                    && (input.Keyword == null || (_epicSchemaDbContext.InvestOrderContractFile.Where(e => e.OrderId == bl.OrderId && e.ContractCodeGen.Contains(input.Keyword)).Any()))
                    && (input.Status == null || input.Status == bl.Status)
                    && (input.Type == null || input.Type == bl.Type)
                    ).Select(bl => new BlockadeLiberation()
                    {
                        Id = bl.Id,
                        Type = bl.Type,
                        BlockadeDescription = bl.BlockadeDescription,
                        BlockadeDate = bl.BlockadeDate,
                        OrderId = bl.OrderId,
                        Order = bl.Order,
                        Blockader = bl.Blockader,
                        BlockadeTime = bl.BlockadeTime,
                        LiberationDescription = bl.LiberationDescription,
                        LiberationDate = bl.LiberationDate,
                        Liberator = bl.Liberator,
                        LiberationTime = bl.LiberationTime,
                        Status = bl.Status,
                        TradingProviderId = bl.TradingProviderId,
                        CreatedBy = bl.CreatedBy,
                        ContractCodeGen = _epicSchemaDbContext.InvestOrderContractFile.Where(e => e.OrderId == bl.OrderId && e.Deleted == YesNo.NO).Select(e => e.ContractCodeGen).Distinct().Count() == 1 ? _epicSchemaDbContext.InvestOrderContractFile.Where(e => e.OrderId == bl.OrderId && e.Deleted == YesNo.NO).Select(e => e.ContractCodeGen).Distinct().First() : bl.Order.ContractCode,
                    });

            //Phân trang
            
            query = query.OrderDynamic(input.Sort);

            //Phân trang
            if (input.PageSize != -1)
            {
                query = query.Skip(input.Skip).Take(input.PageSize);
            }

            result.Items = query.ToList();
            return result;
        }
    }
}
