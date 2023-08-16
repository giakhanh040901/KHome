using EPIC.DataAccess.Base;
using EPIC.DataAccess.Models;
using EPIC.Entities.DataEntities;
using EPIC.LoyaltyEntities.DataEntities;
using EPIC.LoyaltyEntities.Dto.LoyHisAccumulatePoint;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Loyalty;
using EPIC.Utils.ConstantVariables.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace EPIC.LoyaltyRepositories
{
    public class LoyHisAccumulatePointEFRepository : BaseEFRepository<LoyHisAccumulatePoint>
    {
        public LoyHisAccumulatePointEFRepository(EpicSchemaDbContext dbContext, ILogger logger) : base(dbContext, logger, $"{DbSchemas.EPIC_LOYALTY}.{LoyHisAccumulatePoint.SEQ}")
        {
        }

        /// <summary>
        /// Thêm tích/tiêu điểm
        /// </summary>
        /// <param name="input"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        public LoyHisAccumulatePoint Add(LoyHisAccumulatePoint input, string username)
        {
            _logger.LogInformation($"{nameof(LoyHisAccumulatePoint)}->{nameof(Add)}: input = {JsonSerializer.Serialize(input)}, username = {username}");

            input.Id = (int)NextKey();
            input.CreatedBy = username;
            input.CreatedDate = DateTime.Now;
            input.Deleted = YesNo.NO;

            input.Status = input.Status ?? LoyHisAccumulatePointStatus.CREATED;

            input.Reason = input.Reason ?? LoyHisAccumulatePointReasons.KHAC;

            _dbSet.Add(input);

            return input;
        }

        /// <summary>
        /// Cập nhật
        /// </summary>
        /// <param name="input"></param>
        /// <param name="username"></param>
        public void Update(LoyHisAccumulatePoint entity, string username)
        {
            _logger.LogInformation($"{nameof(LoyHisAccumulatePoint)}->{nameof(Update)}: input = {JsonSerializer.Serialize(entity)}, username = {username}");

            entity.ModifiedBy = username;
            entity.ModifiedDate = DateTime.Now;
        }

        /// <summary>
        /// Cập nhật trạng thái đổi điểm
        /// </summary>
        /// <param name="id"></param>
        /// <param name="status"></param>
        /// <param name="username"></param>
        /// <param name="type"></param>
        public void UpdateConsumeStatus(UpdateHisAccumulateStatusDto dto, int status)
        {
            _logger.LogInformation($"{nameof(LoyHisAccumulatePoint)}->{nameof(Update)}: dto = {dto}");

            var entity = FindById(dto.Id).ThrowIfNull(_epicSchemaDbContext, ErrorCode.LoyVoucherHisAccumulatePointNotFound);

            entity.Status = status;
        }

        /// <summary>
        /// Xoá tích điểm/tiêu điểm theo id
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int id)
        {
            _logger.LogInformation($"{nameof(Delete)}: id = {id}");

            var data = FindById(id).ThrowIfNull(_epicSchemaDbContext, ErrorCode.LoyVoucherHisAccumulatePointNotFound);
            data.Deleted = YesNo.YES;
        }

        /// <summary>
        /// Lấy thông tin tích điểm/tiêu điểm theo id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public LoyHisAccumulatePoint FindById(int id)
        {
            _logger.LogInformation($"{nameof(LoyHisAccumulatePoint)}->{nameof(FindById)}: id = {id}");
            return _dbSet.FirstOrDefault(x => x.Id == id && x.Deleted == YesNo.NO);
        }

        /// <summary>
        /// Lấy thông tin tích điểm/tiêu điểm theo id no tracking
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public LoyHisAccumulatePoint FindByIdNoTracking(int id)
        {
            _logger.LogInformation($"{nameof(LoyHisAccumulatePoint)}->{nameof(FindById)}: id = {id}");
            return _dbSet.AsNoTracking().FirstOrDefault(x => x.Id == id && x.Deleted == YesNo.NO);
        }

        /// <summary>
        /// Lấy phân trang quản lý tích điểm
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public PagingResult<ViewFindAllHisAccumulatePointDto> FindAll(FindHisAccumulatePointDto dto, int? tradingProviderId)
        {
            _logger.LogInformation($"{nameof(ViewFindAllHisAccumulatePointDto)}->{nameof(FindAll)}: dto={JsonSerializer.Serialize(dto)}");

            var query = from his in _dbSet.AsNoTracking()
                        join inv in _epicSchemaDbContext.Investors.AsNoTracking() on his.InvestorId equals inv.InvestorId
                        join cif in _epicSchemaDbContext.CifCodes.AsNoTracking() on inv.InvestorId equals cif.InvestorId
                        from iden in _epicSchemaDbContext.InvestorIdentifications.AsNoTracking().Where(x => x.IsDefault == YesNo.YES && x.Deleted == YesNo.NO && x.InvestorId == inv.InvestorId)
                        where his.Deleted == YesNo.NO && inv.Deleted == YesNo.NO && cif.Deleted == YesNo.NO && (tradingProviderId == null || his.TradingProviderId == tradingProviderId) &&
                            (dto.Keyword == null || (inv.Phone.ToLower().Contains(dto.Keyword.ToLower())) || (iden.Fullname.ToLower().Contains(dto.Keyword.ToLower())) || (cif.CifCode.ToLower().Contains(dto.Keyword.ToLower()))) &&
                            (dto.PointType == null || dto.PointType == his.PointType)
                        orderby his.Id descending
                        select new ViewFindAllHisAccumulatePointDto
                        {
                            ApplyDate = his.ApplyDate,
                            CifCode = cif.CifCode,
                            Fullname = iden.Fullname,
                            CreatedBy = his.CreatedBy,
                            CreatedDate = his.CreatedDate,
                            Id = his.Id,
                            InvestorId = inv.InvestorId,
                            Point = his.Point,
                            PointType = his.PointType,
                            Reason = his.Reason,
                            Phone = inv.Phone,
                            Status = his.Status,
                            ExchangedPointStatus = his.ExchangedPointStatus,
                            LoyCurrentPoint = his.CurrentPoint,
                        };

            return new PagingResult<ViewFindAllHisAccumulatePointDto>
            {
                TotalItems = query.Count(),
                Items = query.Skip(dto.Skip).Take(dto.PageSize)
            };

        }

        /// <summary>
        /// Lấy phân trang yêu cầu đổi điểm
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public PagingResult<ViewFindAllHisAccumulatePointDto> FindAllRequestConsumePoint(FindRequestConsumePointDto dto, int? tradingProviderId)
        { 
            _logger.LogInformation($"{nameof(ViewFindAllHisAccumulatePointDto)}->{nameof(FindAll)}: dto={JsonSerializer.Serialize(dto)}");
            
            var query = (from his in _dbSet.AsNoTracking()
                        join inv in _epicSchemaDbContext.Investors.AsNoTracking() on his.InvestorId equals inv.InvestorId
                        join cif in _epicSchemaDbContext.CifCodes.AsNoTracking() on inv.InvestorId equals cif.InvestorId
                        from iden in _epicSchemaDbContext.InvestorIdentifications.AsNoTracking().Where(x => x.IsDefault == YesNo.YES && x.Deleted == YesNo.NO && x.InvestorId == inv.InvestorId).DefaultIfEmpty()
                        from logPending in _epicSchemaDbContext.LoyAccumulatePointStatusLogs.AsNoTracking().Where(x => x.Deleted == YesNo.NO && x.HisAccumulatePointId == his.Id && x.ExchangedPointStatus == LoyExchangePointStatus.PENDING).DefaultIfEmpty().Take(1)
                        from logDelivery in _epicSchemaDbContext.LoyAccumulatePointStatusLogs.AsNoTracking().Where(x => x.Deleted == YesNo.NO && x.HisAccumulatePointId == his.Id && x.ExchangedPointStatus == LoyExchangePointStatus.DELIVERY).DefaultIfEmpty().Take(1)
                        from logFinish in _epicSchemaDbContext.LoyAccumulatePointStatusLogs.AsNoTracking().Where(x => x.Deleted == YesNo.NO && x.HisAccumulatePointId == his.Id && x.ExchangedPointStatus == LoyExchangePointStatus.FINISHED).DefaultIfEmpty().Take(1)
                        from logCancel in _epicSchemaDbContext.LoyAccumulatePointStatusLogs.AsNoTracking().Where(x => x.Deleted == YesNo.NO && x.HisAccumulatePointId == his.Id && x.ExchangedPointStatus == LoyExchangePointStatus.CANCELED).DefaultIfEmpty().Take(1)
                         where his.Deleted == YesNo.NO && inv.Deleted == YesNo.NO && cif.Deleted == YesNo.NO && (tradingProviderId == null || his.TradingProviderId == tradingProviderId) &&
                            (dto.Keyword == null || (inv.Phone.ToLower().Contains(dto.Keyword.ToLower())) || (iden.Fullname.ToLower().Contains(dto.Keyword.ToLower())) || (cif.CifCode.ToLower().Contains(dto.Keyword.ToLower()))) &&
                            (his.PointType == LoyPointTypes.TIEU_DIEM) &&
                            (
                                (dto.Status == null) ||
                                (dto.Status == his.ExchangedPointStatus && dto.Date == null) ||
                                (dto.Status == his.ExchangedPointStatus && dto.Date.HasValue && (
                                    (
                                        dto.StatusDate == LoyFindRequestExchangePointStatusDates.BANG_NGAY && (
                                            (dto.Status == LoyExchangePointStatus.CREATED && his.CreatedDate.HasValue && his.CreatedDate.Value.Date == dto.Date.Value.Date) ||
                                            (dto.Status == LoyExchangePointStatus.PENDING && logPending.CreatedDate.HasValue && logPending.CreatedDate.Value.Date == dto.Date.Value.Date) || 
                                            (dto.Status == LoyExchangePointStatus.DELIVERY && logDelivery.CreatedDate.HasValue && logDelivery.CreatedDate.Value.Date == dto.Date.Value.Date) ||
                                            (dto.Status == LoyExchangePointStatus.FINISHED && logFinish.CreatedDate.HasValue && logFinish.CreatedDate.Value.Date == dto.Date.Value.Date) || 
                                            (dto.Status == LoyExchangePointStatus.CANCELED && logCancel.CreatedDate.HasValue && logCancel.CreatedDate.Value.Date == dto.Date.Value.Date)
                                        )
                                    ) ||
                                    (
                                        dto.StatusDate == LoyFindRequestExchangePointStatusDates.TAT_CA_NGAY && (
                                            (dto.Status == LoyExchangePointStatus.CREATED && his.CreatedDate.HasValue && his.CreatedDate.Value.Date <= dto.Date.Value.Date) ||
                                            (dto.Status == LoyExchangePointStatus.PENDING && logPending.CreatedDate.HasValue && logPending.CreatedDate.Value.Date <= dto.Date.Value.Date) ||
                                            (dto.Status == LoyExchangePointStatus.DELIVERY && logDelivery.CreatedDate.HasValue && logDelivery.CreatedDate.Value.Date <= dto.Date.Value.Date) ||
                                            (dto.Status == LoyExchangePointStatus.FINISHED && logFinish.CreatedDate.HasValue && logFinish.CreatedDate.Value.Date <= dto.Date.Value.Date) ||
                                            (dto.Status == LoyExchangePointStatus.CANCELED && logCancel.CreatedDate.HasValue && logCancel.CreatedDate.Value.Date <= dto.Date.Value.Date)
                                        )
                                    )
                                ))
                            )
                        orderby his.Id descending
                        select new ViewFindAllHisAccumulatePointDto
                        {
                            ApplyDate = his.ApplyDate,
                            CifCode = cif.CifCode,
                            Fullname = iden.Fullname,
                            CreatedBy = his.CreatedBy,
                            Id = his.Id,
                            InvestorId = inv.InvestorId,
                            Point = his.Point,
                            PointType = his.PointType,
                            Reason = his.Reason,
                            Phone = inv.Phone,
                            Status = his.Status,
                            ExchangedPointStatus = his.ExchangedPointStatus,
                            LoyCurrentPoint = his.CurrentPoint,
                            CreatedDate = his.CreatedDate,
                            DeliveryDate = logDelivery.CreatedDate,
                            PendingDate = logPending.CreatedDate,
                            FinishedDate = logFinish.CreatedDate,
                            CancelDate = logCancel.CreatedDate,
                        })
                        .Distinct().OrderByDescending(x => x.Id);

            return new PagingResult<ViewFindAllHisAccumulatePointDto>
            {
                TotalItems = query.Count(),
                Items = query.Skip(dto.Skip).Take(dto.PageSize)
            };

        }

        /// <summary>
        /// Phân trang lịch sử tích tiêu điểm theo investor id
        /// </summary>
        /// <param name="investorId"></param>
        /// <param name="dto"></param>
        /// <returns></returns>
        public PagingResult<ViewFindAllHisAccumulatePointDto> FindByInvestorIdPaging(int investorId, FindAccumulatePointByInvestorId dto, int? tradingProviderId)
        {
            _logger.LogInformation($"{nameof(ViewFindAllHisAccumulatePointDto)}->{nameof(FindByInvestorIdPaging)}: dto={JsonSerializer.Serialize(dto)}");
            var listStatus = new int?[] { LoyExchangePointStatus.FINISHED, LoyExchangePointStatus.PENDING, LoyExchangePointStatus.DELIVERY };

            var query = from his in _dbSet.AsNoTracking()
                        join inv in _epicSchemaDbContext.Investors.AsNoTracking() on his.InvestorId equals inv.InvestorId
                        where his.Deleted == YesNo.NO && inv.Deleted == YesNo.NO && his.InvestorId == investorId &&
                            (tradingProviderId == null || his.TradingProviderId == tradingProviderId) &&
                            his.Status == LoyHisAccumulatePointStatus.FINISHED &&
                            (dto.PointType == null || dto.PointType == his.PointType) &&
                            (dto.Reason == null || dto.Reason == his.Reason)
                        orderby his.Id descending
                        select new ViewFindAllHisAccumulatePointDto
                        {
                            ApplyDate = his.ApplyDate,
                            CreatedBy = his.CreatedBy,
                            CreatedDate = his.CreatedDate,
                            Id = his.Id,
                            InvestorId = inv.InvestorId,
                            Point = his.Point,
                            PointType = his.PointType,
                            Reason = his.Reason,
                            Status = his.Status,
                            ExchangedPointStatus = his.ExchangedPointStatus,
                        };

            return new PagingResult<ViewFindAllHisAccumulatePointDto>
            {
                TotalItems = query.Count(),
                Items = query.Skip(dto.Skip).Take(dto.PageSize)
            };
        }

        /// <summary>
        /// App lấy list lịch sử điểm thưởng theo investor
        /// </summary>
        /// <param name="investorId"></param>
        /// <returns></returns>
        public List<LoyHisAccumulatePoint> FindByInvestorId(int investorId, int? tradingProviderId)
        {
            _logger.LogInformation($"{nameof(LoyHisAccumulatePoint)}->{nameof(FindByInvestorIdPaging)}: investorId={investorId}; tradingProviderId={tradingProviderId}");

            var query = from his in _dbSet.AsNoTracking()
                        join inv in _epicSchemaDbContext.Investors.AsNoTracking() on his.InvestorId equals inv.InvestorId
                        where his.Deleted == YesNo.NO && inv.Deleted == YesNo.NO &&
                                inv.InvestorId == investorId && tradingProviderId == his.TradingProviderId
                        orderby his.Id descending
                        select his;

            return query.ToList();
        }
    }
}
