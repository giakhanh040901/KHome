using DocumentFormat.OpenXml.Bibliography;
using EPIC.DataAccess.Base;
using EPIC.DataAccess.Models;
using EPIC.LoyaltyEntities.DataEntities;
using EPIC.LoyaltyEntities.Dto.ConversionPoint;
using EPIC.LoyaltyEntities.Dto.LoyHisAccumulatePoint;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Loyalty;
using EPIC.Utils.ConstantVariables.Shared;
using EPIC.Utils.Linq;
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
    public class LoyConversionPointEFRepository : BaseEFRepository<LoyConversionPoint>
    {
        public LoyConversionPointEFRepository(EpicSchemaDbContext dbContext, ILogger logger) : base(dbContext, logger, $"{DbSchemas.EPIC_LOYALTY}.{LoyConversionPoint.SEQ}")
        {
        }

        public LoyConversionPoint Add(LoyConversionPoint entity)
        {
            _logger.LogInformation($"{nameof(LoyConversionPointEFRepository)}->{nameof(Add)}: input = {JsonSerializer.Serialize(entity)}");
            entity.Id = (int)NextKey();
            // Nếu loại cấp phát là đổi điểm thì Default là Yes
            entity.IsMinusPoint = (entity.AllocationType == LoyAllocationTypes.TANG_KHACH_HANG) ? entity.IsMinusPoint : YesNo.YES;
            entity.Status = LoyConversionPointStatus.CREATED;
            entity.CreatedDate = DateTime.Now;
            return _dbSet.Add(entity).Entity;
        }

        public LoyConversionPoint FindById(int id, int? tradingProviderId)
        {
            _logger.LogInformation($"{nameof(LoyConversionPointEFRepository)}->{nameof(FindById)}: id = {id}, tradingProviderId = {tradingProviderId}");
            var loyConversionPoint = _dbSet.FirstOrDefault(c => c.Id == id && (tradingProviderId == null || c.TradingProviderId == tradingProviderId) && c.Deleted == YesNo.NO);
            return loyConversionPoint;
        }

        /// <summary>
        /// Thêm chi tiết
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public LoyConversionPointDetail LoyConversionPointDetailAdd(LoyConversionPointDetail entity)
        {
            _logger.LogInformation($"{nameof(LoyConversionPointEFRepository)}->{nameof(LoyConversionPointDetailAdd)}: input = {JsonSerializer.Serialize(entity)}");
            entity.Id = (int)NextKey($"{DbSchemas.EPIC_LOYALTY}.{LoyConversionPointDetail.SEQ}");
            entity.CreatedDate = DateTime.Now;
            return _epicSchemaDbContext.LoyConversionPointDetails.Add(entity).Entity;
        }

        public PagingResult<FindAllLoyConversionPointDto> FindAll(FilterLoyConversionPointDto dto, int? tradingProviderId)
        {
            _logger.LogInformation($"{nameof(LoyConversionPointEFRepository)}->{nameof(FindAll)}: dto={JsonSerializer.Serialize(dto)}");

            var query = (from conversionPoint in _dbSet.AsNoTracking()
                         let pendingDate = _epicSchemaDbContext.LoyConversionPointStatusLogs.Where(l => l.ConversionPointId == conversionPoint.Id && l.Status == LoyConversionPointStatus.PENDING).OrderByDescending(x => x.Id).FirstOrDefault().CreatedDate
                         let deliveryDate = _epicSchemaDbContext.LoyConversionPointStatusLogs.Where(l => l.ConversionPointId == conversionPoint.Id && l.Status == LoyConversionPointStatus.DELIVERY).OrderByDescending(x => x.Id).FirstOrDefault().CreatedDate
                         let finishedDate = _epicSchemaDbContext.LoyConversionPointStatusLogs.Where(l => l.ConversionPointId == conversionPoint.Id && l.Status == LoyConversionPointStatus.FINISHED).OrderByDescending(x => x.Id).FirstOrDefault().CreatedDate
                         let canceledDate = _epicSchemaDbContext.LoyConversionPointStatusLogs.Where(l => l.ConversionPointId == conversionPoint.Id && l.Status == LoyConversionPointStatus.CANCELED).OrderByDescending(x => x.Id).FirstOrDefault().CreatedDate
                         join investor in _epicSchemaDbContext.Investors.AsNoTracking() on conversionPoint.InvestorId equals investor.InvestorId
                         join cifCode in _epicSchemaDbContext.CifCodes.AsNoTracking() on investor.InvestorId equals cifCode.InvestorId
                         from investorIden in _epicSchemaDbContext.InvestorIdentifications.AsNoTracking().Where(x => x.IsDefault == YesNo.YES && x.Deleted == YesNo.NO && x.InvestorId == investor.InvestorId).DefaultIfEmpty()
                         where conversionPoint.Deleted == YesNo.NO && investor.Deleted == YesNo.NO && cifCode.Deleted == YesNo.NO && (tradingProviderId == null || conversionPoint.TradingProviderId == tradingProviderId)
                         && (dto.Keyword == null || (investor.Phone.ToLower().Contains(dto.Keyword.ToLower())) || (investorIden.Fullname.ToLower().Contains(dto.Keyword.ToLower())) || (cifCode.CifCode.ToLower().Contains(dto.Keyword.ToLower())))
                         && (dto.Status == null || conversionPoint.Status == dto.Status)

                         orderby conversionPoint.Id descending
                         select new FindAllLoyConversionPointDto
                         {
                             Id = conversionPoint.Id,
                             InvestorId = investor.InvestorId,
                             CifCode = cifCode.CifCode,
                             Fullname = investorIden.Fullname,
                             Phone = investor.Phone,
                             RequestType = conversionPoint.RequestType,
                             AllocationType = conversionPoint.AllocationType,
                             Source = conversionPoint.Source,
                             Description = conversionPoint.Description,
                             RequestDate = conversionPoint.RequestDate,
                             LoyCurrentPoint = conversionPoint.CurrentPoint,
                             TotalConversionPoint = _epicSchemaDbContext.LoyConversionPointDetails.Where(d => d.ConversionPointId == conversionPoint.Id && d.Deleted == YesNo.NO)
                                                    .Sum(d => d.TotalConversionPoint),
                             Status = conversionPoint.Status,
                             DeliveryDate = deliveryDate,
                             PendingDate = pendingDate,
                             FinishedDate = finishedDate,
                             CanceledDate = canceledDate,
                         });
            query = query.OrderByDescending(x => x.Id);
            var totalItem = query.Count();
            query = query.OrderDynamic(dto.Sort);
            if (dto.PageSize != -1)
            {
                query = query.Skip(dto.Skip).Take(dto.PageSize);
            }
            return new PagingResult<FindAllLoyConversionPointDto>
            {
                TotalItems = totalItem,
                Items = query
            };
        }

        /// <summary>
        /// Lấy danh sách voucher cho nhà đầu tư
        /// </summary>
        /// <param name="investorId">Id nhà đầu tư</param>
        /// <param name="status">Lọc theo trạng thái</param>
        /// <param name="isExpired">Lọc các voucher hết hạn</param>
        /// <param name="take">Lấy vao nhiêu phần tử voucher</param>
        /// <param name="canUse">Lọc các voucher có thể sử dụng</param>
        /// <returns></returns>
        public IEnumerable<AppLoyConversionPointByInvestorDto> FindAllVoucherByInvestor(int tradingProviderId, int investorId, int? status, bool isExpired = false, int? take = null, bool canUse = false)
        {
            _logger.LogInformation($"{nameof(FindAllVoucherByInvestor)} : tradingProviderId = {tradingProviderId}, investorId = {investorId}, status = {status}, isExpired = {isExpired}, take = {take}, canUse = {canUse}");
            var result = (from conversionPointDetail in _epicSchemaDbContext.LoyConversionPointDetails
                          join conversionPoint in _epicSchemaDbContext.LoyConversionPoints on conversionPointDetail.ConversionPointId equals conversionPoint.Id
                          join voucher in _epicSchemaDbContext.LoyVouchers on conversionPointDetail.VoucherId equals voucher.Id
                          where conversionPointDetail.Deleted == YesNo.NO && voucher.Deleted == YesNo.NO && conversionPoint.Deleted == YesNo.NO
                          && investorId == conversionPoint.InvestorId && (status == null || conversionPoint.Status == status)
                          && (!isExpired || (voucher.ExpiredDate != null && voucher.ExpiredDate.Value.Date <= DateTime.Now.Date && conversionPoint.Status != LoyConversionPointStatus.CANCELED))
                          && (!canUse || (voucher.ExpiredDate != null && voucher.ExpiredDate.Value.Date >= DateTime.Now.Date))
                          && tradingProviderId == conversionPoint.TradingProviderId
                          select new
                          {
                              ConversionPointDetailQuantity = conversionPointDetail.Quantity,
                              ConversionPointDetailId = conversionPointDetail.Id,
                              ConversionPointStatus = conversionPoint.Status ?? 0,
                              VoucherId = voucher.Id,
                              Avatar = voucher.Avatar,
                              DisplayName = voucher.DisplayName,
                              VoucherType = voucher.VoucherType,
                              Name = voucher.Name,
                              LinkVoucher = voucher.LinkVoucher,
                              Point = voucher.Point,
                              StartDate = voucher.StartDate,
                              EndDate = voucher.EndDate,
                              ExpiredDate = voucher.ExpiredDate
                          }).ToList()
                         .SelectMany(p => Enumerable.Repeat(new AppLoyConversionPointByInvestorDto
                         {
                             ConversionPointDetailId = p.ConversionPointDetailId,
                             ConversionPointStatus = p.ConversionPointStatus,
                             VoucherId = p.VoucherId,
                             Avatar = p.Avatar,
                             DisplayName = p.DisplayName,
                             VoucherType = p.VoucherType,
                             Name = p.Name,
                             LinkVoucher = p.LinkVoucher,
                             Point = p.Point,
                             StartDate = p.StartDate,
                             EndDate = p.EndDate,
                             ExpiredDate = p.ExpiredDate
                         }, p.ConversionPointDetailQuantity));

            result = result.OrderBy(x => x.ExpiredDate).ThenByDescending(x => x.ConversionPointDetailId);

            // Lấy bao nhiêu phần tử
            if (take != null)
            {
                result = result.Take(take ?? 0);
            }
            return result;
        }
    }
}
