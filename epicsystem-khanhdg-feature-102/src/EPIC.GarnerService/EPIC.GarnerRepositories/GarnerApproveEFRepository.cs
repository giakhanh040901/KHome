using EPIC.DataAccess.Base;
using EPIC.DataAccess.Models;
using EPIC.GarnerEntities.DataEntities;
using EPIC.GarnerEntities.Dto.GarnerApprove;
using EPIC.GarnerEntities.Dto.GarnerProduct;
using EPIC.Utils;
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
    public class GarnerApproveEFRepository : BaseEFRepository<GarnerApprove>
    {
        public GarnerApproveEFRepository(EpicSchemaDbContext dbContext, ILogger logger) : base(dbContext, logger, $"{DbSchemas.EPIC_GARNER}.{GarnerApprove.SEQ}")
        {
        }

        public GarnerApprove Request(GarnerApprove input)
        {
            _logger.LogInformation($"{System.Reflection.MethodBase.GetCurrentMethod().Name}: input = {JsonSerializer.Serialize(input)}"); 

            return _dbSet.Add(new GarnerApprove
            {
                Id = (int)NextKey(),
                UserRequestId = input.UserRequestId,
                UserApproveId = input.UserApproveId,
                RequestNote = input.RequestNote,
                ActionType = input.ActionType,
                DataType = input.DataType,
                ReferId = input.ReferId,
                Summary = input.Summary,
                TradingProviderId = input.TradingProviderId,
                Status = ApproveStatus.TRINH_DUYET,
                PartnerId = input.PartnerId,
                CreatedBy = input.CreatedBy,
                CreatedDate = input.CreatedDate,
                RequestDate = DateTime.Now,
            }).Entity;
        }

        public void Approve(GarnerApprove input)
        {
            _logger.LogInformation($"{System.Reflection.MethodBase.GetCurrentMethod().Name}: input = {JsonSerializer.Serialize(input)}");

            var garnerApproveFind = _dbSet.FirstOrDefault(a => a.Id == input.Id && a.Status == ApproveStatus.TRINH_DUYET && a.Deleted == YesNo.NO);
            if (garnerApproveFind != null)
            {
                garnerApproveFind.Status = ApproveStatus.DUYET;
                garnerApproveFind.ApproveDate = DateTime.Now;
                garnerApproveFind.ApproveNote = input.ApproveNote;
                garnerApproveFind.UserApproveId = input.UserApproveId;
            }    
        }

        public void Cancel(GarnerApprove input)
        {
            _logger.LogInformation($"{System.Reflection.MethodBase.GetCurrentMethod().Name}: input = {JsonSerializer.Serialize(input)}");

            var garnerApproveFind = _dbSet.FirstOrDefault(a => a.Id == input.Id && a.Status == ApproveStatus.TRINH_DUYET && a.Deleted == YesNo.NO);
            if (garnerApproveFind != null)
            {
                garnerApproveFind.Status = ApproveStatus.HUY_DUYET;
                garnerApproveFind.CancelDate = DateTime.Now;
                garnerApproveFind.CancelNote = input.CancelNote;
            }
        }

        /// <summary>
        /// Tìm kiếm bảng ghi duyệt theo Id của bảng muốn xét dataType
        /// </summary>
        /// <param name="id"></param>
        /// <param name="dataType"></param>
        /// <returns></returns>
        public GarnerApprove FindByIdOfDataType(int id, int dataType)
        {
            _logger.LogInformation($"{System.Reflection.MethodBase.GetCurrentMethod().Name}: Id = {id}, dataType = {dataType}");
            return _dbSet.FirstOrDefault(a => a.ReferId == id && a.DataType == dataType && a.Deleted == YesNo.NO);
        }

        public void Check(GarnerApprove input)
        {
            _logger.LogInformation($"{System.Reflection.MethodBase.GetCurrentMethod().Name}: input = {JsonSerializer.Serialize(input)}");
            var garnerApproveFind = _dbSet.FirstOrDefault(a => a.Id == input.Id && a.Status == ApproveStatus.DUYET && a.Deleted == YesNo.NO);
            if (garnerApproveFind != null)
            {
                garnerApproveFind.IsCheck = YesNo.YES;
                garnerApproveFind.UserCheckId = input.UserCheckId;
            }
        }

        public PagingResult<GarnerApprove> FindAll(FilterGarnerApproveDto input, int? partnerId = null, int? tradingProviderId = null)
        {
            _logger.LogInformation($"{System.Reflection.MethodBase.GetCurrentMethod().Name}: input = {JsonSerializer.Serialize(input)}, tradingProviderId = {tradingProviderId}, partnerId = {partnerId}");

            PagingResult<GarnerApprove> result = new();
            var garnerApproveQuery = _dbSet.Where(r => r.Deleted == YesNo.NO);

            if (partnerId != null)
            {
                garnerApproveQuery = garnerApproveQuery.Where(r => r.PartnerId == partnerId);
            }

            if (tradingProviderId != null)
            {
                garnerApproveQuery = garnerApproveQuery.Where(r => r.TradingProviderId == tradingProviderId);
            }

            garnerApproveQuery = garnerApproveQuery.Where(a => (input.UserApproveId == null || input.UserApproveId == a.UserApproveId)
                                                    && (input.UserRequestId == null || input.UserRequestId == a.UserRequestId)
                                                    && (input.DataType == null || input.DataType == a.DataType)
                                                    && (input.ActionType == null || input.ActionType == a.ActionType)
                                                    && (input.Status == null || input.Status == a.Status)
                                                    && (input.RequestDate == null || input.RequestDate.Value.Date == a.RequestDate.Value.Date)
                                                    && (input.ApproveDate == null || input.ApproveDate.Value.Date == a.ApproveDate.Value.Date)).OrderByDescending(a => a.RequestDate);
            if (input.Keyword != null)
            {
                garnerApproveQuery = garnerApproveQuery.Where(p => p.Summary.ToLower().Contains(input.Keyword.ToLower()));
            }

            result.TotalItems = garnerApproveQuery.Count();

            if (input.PageSize != -1)
            {
                garnerApproveQuery = garnerApproveQuery.Skip(input.Skip).Take(input.PageSize);
            }



            result.Items = garnerApproveQuery.ToList();
            return result;
        }
    }
}
