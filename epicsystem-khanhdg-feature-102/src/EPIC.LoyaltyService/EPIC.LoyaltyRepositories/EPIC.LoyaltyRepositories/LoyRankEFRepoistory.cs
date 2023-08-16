using DocumentFormat.OpenXml.Office2010.Excel;
using EPIC.DataAccess.Base;
using EPIC.DataAccess.Models;
using EPIC.LoyaltyEntities.DataEntities;
using EPIC.LoyaltyEntities.Dto.LoyRank;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Loyalty;
using EPIC.Utils.ConstantVariables.Shared;
using Humanizer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Text;
using System.Text.Json;

namespace EPIC.LoyaltyRepositories
{
    public class LoyRankEFRepoistory : BaseEFRepository<LoyRank>
    {
        public LoyRankEFRepoistory(EpicSchemaDbContext dbContext, ILogger logger) : base(dbContext, logger, $"{DbSchemas.EPIC_LOYALTY}.{LoyRank.SEQ}")
        {
            
        }

        /// <summary>
        /// Thêm cấu hình xếp hạng thành viên
        /// </summary>
        /// <param name="input"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        public LoyRank Add(LoyRank input, string username)
        {
            _logger.LogInformation($"{nameof(LoyRank)}->{nameof(Add)}: input = {JsonSerializer.Serialize(input)}, username = {username}");

            input.Id = (int)NextKey();
            input.CreatedBy = username;
            input.CreatedDate = DateTime.Now;
            input.Deleted = YesNo.NO;
            input.Status = LoyRankStatus.KHOI_TAO;

            var result = _dbSet.Add(input).Entity;

            return result;
        }

        /// <summary>
        /// Xóa theo id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="username"></param>
        public void Delete(int id, string username)
        {
            _logger.LogInformation($"{nameof(Delete)}: id = {id}; username={username}");

            var rank = FindById(id).ThrowIfNull(_epicSchemaDbContext, ErrorCode.LoyRankNotFound);

            rank.Deleted = YesNo.YES;
            rank.DeletedBy = username;
            rank.DeletedDate = DateTime.Now;
        }

        /// <summary>
        /// Tìm cấu hình xếp hạng theo id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public LoyRank FindById(int id)
        {
            _logger.LogInformation($"{nameof(LoyRank)}->{nameof(FindById)}: id = {id}");

            var query = _dbSet.FirstOrDefault(x => x.Deleted == YesNo.NO && x.Id == id);
            return query;
        }

        /// <summary>
        /// Check xem bắt đầu vs kết thúc có đè lên rank nào ko
        /// </summary>
        /// <param name="id"></param>
        /// <param name="pointStart"></param>
        /// <param name="pointEnd"></param>
        /// <param name="tradingProviderId"></param>
        /// <returns></returns>
        public bool AnyOverrideRank(int id, int? pointStart, int? pointEnd, int? tradingProviderId)
        {
            _logger.LogInformation($"{nameof(AnyOverrideRank)}: id={id}; pointStart={pointStart}; pointEnd={pointEnd}; tradingProviderId={tradingProviderId}");

            var anyo = _dbSet.AsNoTracking().Any(x => x.Id != id && x.TradingProviderId == tradingProviderId && x.Deleted == YesNo.NO && x.Status == LoyRankStatus.ACTIVE && ((x.PointStart <= pointStart && pointStart <= x.PointEnd) || (x.PointStart <= pointEnd && pointEnd <= x.PointEnd)));
            return anyo;
        }

        /// <summary>
        /// Lấy rank theo tổng điểm
        /// </summary>
        /// <param name="totalPoint"></param>
        /// <returns></returns>
        public LoyRank FindRankByTotalPoint(int? totalPoint, int? tradingProviderId)
        {
            return _dbSet.AsNoTracking().FirstOrDefault(x => x.Deleted == YesNo.NO && x.PointStart <= totalPoint && totalPoint <= x.PointEnd && x.TradingProviderId == tradingProviderId && x.Status == LoyRankStatus.ACTIVE);
        }

        /// <summary>
        /// Phân trang cấu hình xếp hạng
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public PagingResult<ViewFindRankDto> FindAll(FindAllRankDto dto, int? tradingProviderId)
        {
            _logger.LogInformation($"{nameof(FindAll)}: dto={JsonSerializer.Serialize(dto)}");

            var query = _dbSet.AsNoTracking().Where(x => x.Deleted == YesNo.NO && (tradingProviderId == null || x.TradingProviderId == tradingProviderId) &&
                            (dto.Keyword == null || x.Name.ToLower().Contains(dto.Keyword.ToLower()) || x.Description.ToLower().Contains(dto.Keyword.ToLower())) &&
                            (dto.Status == null || x.Status == dto.Status)
                        ).Select(x => new ViewFindRankDto
                        {
                            Id = x.Id,
                            ActiveDate = x.ActiveDate,
                            CreatedBy = x.CreatedBy,
                            CreatedDate = x.CreatedDate,
                            DeactiveDate = x.DeactiveDate,
                            Description = x.Description,
                            Name = x.Name,
                            PointEnd = x.PointEnd,
                            PointStart = x.PointStart,
                            Status = x.Status
                        }).OrderByDescending(x => x.Id);

            return new PagingResult<ViewFindRankDto>
            {
                TotalItems = query.Count(),
                Items = query.Skip(dto.Skip).Take(dto.PageSize).ToList()
            };
        }

        /// <summary>
        /// Lấy thông tin rank theo investor id
        /// </summary>
        /// <param name="investorId"></param>
        /// <returns></returns>
        public LoyRank FindByInvestorId(int investorId, int? tradingProviderId)
        {
            _logger.LogInformation($"{nameof(FindByInvestorId)}: investorId={investorId}; tradingProviderId={tradingProviderId}");

            var query = from invPoint in _epicSchemaDbContext.LoyPointInvestors.AsNoTracking()
                        from rank in _dbSet.AsNoTracking().Where(x => x.Deleted == YesNo.NO && x.Status == LoyRankStatus.ACTIVE && x.TradingProviderId == invPoint.TradingProviderId && x.PointStart <= invPoint.TotalPoint && invPoint.TotalPoint <= x.PointEnd)
                        where invPoint.Deleted == YesNo.NO && invPoint.InvestorId == investorId && invPoint.TradingProviderId == tradingProviderId
                        select rank;

            return query.FirstOrDefault();
        }
    }
}
