using EPIC.DataAccess.Base;
using EPIC.DataAccess.Models;
using EPIC.GarnerEntities.DataEntities;
using EPIC.GarnerEntities.Dto.GarnerApprove;
using EPIC.Utils.ConstantVariables.Shared;
using EPIC.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EPIC.RealEstateEntities.DataEntities;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using EPIC.RealEstateEntities.Dto.RstApprove;

namespace EPIC.RealEstateRepositories
{
    public class RstApproveEFRepository : BaseEFRepository<RstApprove>
    {
        public RstApproveEFRepository(EpicSchemaDbContext dbContext, ILogger logger) : base(dbContext, logger, $"{DbSchemas.EPIC_REAL_ESTATE}.{RstApprove.SEQ}")
        {
        }

        public RstApprove Request(RstApprove input)
        {
            _logger.LogInformation($"{nameof(RstApproveEFRepository)}-> {nameof(Request)}: input = {JsonSerializer.Serialize(input)}");
            input.Id = (int)NextKey();
            input.Status = ApproveStatus.TRINH_DUYET;
            input.RequestDate = DateTime.Now;
            return _dbSet.Add(input).Entity;
        }

        public void Approve(RstApprove input)
        {
            _logger.LogInformation($"{nameof(RstApproveEFRepository)}-> {nameof(Approve)}: input = {JsonSerializer.Serialize(input)}");

            var approveQuery = _dbSet.FirstOrDefault(a => a.Id == input.Id && a.Status == ApproveStatus.TRINH_DUYET && a.Deleted == YesNo.NO);
            if (approveQuery != null)
            {
                approveQuery.ApproveIp = input.ApproveIp;
                approveQuery.Status = ApproveStatus.DUYET;
                approveQuery.ApproveDate = DateTime.Now;
                approveQuery.ApproveNote = input.ApproveNote;
                approveQuery.UserApproveId = input.UserApproveId;
            }
        }

        public void Cancel(RstApprove input)
        {
            _logger.LogInformation($"{nameof(RstApproveEFRepository)}-> {nameof(Cancel)}: input = {JsonSerializer.Serialize(input)}");

            var approveQuery = _dbSet.FirstOrDefault(a => a.Id == input.Id && a.Status == ApproveStatus.TRINH_DUYET && a.Deleted == YesNo.NO);
            if (approveQuery != null)
            {
                approveQuery.Status = ApproveStatus.HUY_DUYET;
                approveQuery.CancelDate = DateTime.Now;
                approveQuery.CancelNote = input.CancelNote;
            }
        }

        /// <summary>
        /// Tìm kiếm bảng ghi duyệt theo Id của bảng muốn xét dataType
        /// </summary>
        public RstApprove FindByIdOfDataType(int id, int dataType)
        {
            _logger.LogInformation($"{nameof(RstApproveEFRepository)}-> {nameof(FindByIdOfDataType)}: Id = {id}, dataType = {dataType}");
            return _dbSet.FirstOrDefault(a => a.ReferId == id && a.DataType == dataType && a.Deleted == YesNo.NO);
        }

        public void Check(RstApprove input)
        {
            _logger.LogInformation($"{nameof(RstApproveEFRepository)}-> {nameof(Check)}: input = {JsonSerializer.Serialize(input)}");
            var garnerApproveFind = _dbSet.FirstOrDefault(a => a.Id == input.Id && a.Status == ApproveStatus.DUYET && a.Deleted == YesNo.NO);
            if (garnerApproveFind != null)
            {
                garnerApproveFind.IsCheck = YesNo.YES;
                garnerApproveFind.UserCheckId = input.UserCheckId;
            }
        }

        public PagingResult<RstApprove> FindAll(FilterRstApproveDto input, int? partnerId = null, int? tradingProviderId = null)
        {
            _logger.LogInformation($"{nameof(RstApproveEFRepository)}-> {nameof(FindAll)}: input = {JsonSerializer.Serialize(input)}, tradingProviderId = {tradingProviderId}, partnerId = {partnerId}");

            PagingResult<RstApprove> result = new();
            var approveQuery = _dbSet.Where(r => r.Deleted == YesNo.NO);

            if (partnerId != null)
            {
                approveQuery = approveQuery.Where(r => r.PartnerId == partnerId);
            }

            if (tradingProviderId != null)
            {
                approveQuery = approveQuery.Where(r => r.TradingProviderId == tradingProviderId);
            }

            approveQuery = approveQuery.Where(a => (input.UserApproveId == null || input.UserApproveId == a.UserApproveId)
                                                    && (input.UserRequestId == null || input.UserRequestId == a.UserRequestId)
                                                    && (input.DataType == null || input.DataType == a.DataType)
                                                    && (input.ActionType == null || input.ActionType == a.ActionType)
                                                    && (input.Status == null || input.Status == a.Status)
                                                    && (input.RequestDate == null || input.RequestDate.Value.Date == a.RequestDate.Value.Date)
                                                    && (input.ApproveDate == null || input.ApproveDate.Value.Date == a.ApproveDate.Value.Date)).OrderByDescending(a => a.RequestDate);
            if (input.Keyword != null)
            {
                approveQuery = approveQuery.Where(p => p.Summary.ToLower().Contains(input.Keyword.ToLower()));
            }

            result.TotalItems = approveQuery.Count();

            if (input.PageSize != -1)
            {
                approveQuery = approveQuery.Skip(input.Skip).Take(input.PageSize);
            }
            result.Items = approveQuery.ToList();
            return result;
        }
    }
}
