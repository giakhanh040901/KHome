using DocumentFormat.OpenXml.Office2010.Excel;
using EPIC.DataAccess.Base;
using EPIC.DataAccess.Models;
using EPIC.LoyaltyEntities.DataEntities;
using EPIC.LoyaltyEntities.Dto.LoyPointInvestor;
using EPIC.LoyaltyEntities.Dto.LoyRank;
using EPIC.LoyaltyEntities.Dto.LoyVoucher;
using EPIC.LoyaltyEntities.Dto.LoyVoucherInvestor;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Identity;
using EPIC.Utils.ConstantVariables.Loyalty;
using EPIC.Utils.ConstantVariables.Shared;
using EPIC.Utils.Linq;
using Humanizer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.Json;

namespace EPIC.LoyaltyRepositories
{
    public class LoyPointInvestorEFRepoistory : BaseEFRepository<LoyPointInvestor>
    {
        public LoyPointInvestorEFRepoistory(EpicSchemaDbContext dbContext, ILogger logger) : base(dbContext, logger, $"{DbSchemas.EPIC_LOYALTY}.{LoyPointInvestor.SEQ}")
        {
            
        }

        /// <summary>
        /// Thêm điểm investor
        /// </summary>
        /// <param name="input"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        public LoyPointInvestor Add(LoyPointInvestor input, string username)
        {
            _logger.LogInformation($"{nameof(LoyRank)}->{nameof(Add)}: input = {JsonSerializer.Serialize(input)}, username = {username}");

            input.Id = (int)NextKey();
            input.CreatedBy = username;
            input.CreatedDate = DateTime.Now;
            input.Deleted = YesNo.NO;

            var result = _dbSet.Add(input).Entity;

            return result;
        }

        /// <summary>
        /// Lấy điểm theo khách và đại lý
        /// </summary>
        /// <param name="investorId"></param>
        /// <param name="tradingProviderId"></param>
        /// <returns></returns>
        public LoyPointInvestor Get(int? investorId, int? tradingProviderId)
        {
            _logger.LogInformation($"{nameof(LoyPointInvestor)}->{nameof(Get)}: investorId={investorId}, tradingProviderId={tradingProviderId}");

            var data = _dbSet.FirstOrDefault(x => x.InvestorId == investorId && (tradingProviderId == null || x.TradingProviderId == tradingProviderId) && x.Deleted == YesNo.NO);
            return data;
        }

        /// <summary>
        /// Lịch sử quy đổi điểm của nhà đầu tư/ Tab Danh sách ưu đãi
        /// </summary>
        public PagingResult<PointInvestorConversionHistoryDto> FindAllVoucherConversionHistory(FilterPointInvestorConversionHistoryDto dto, int? tradingProviderId)
        {
            _logger.LogInformation($"{nameof(PagingResult<VoucherConversionHistoryDto>)}->{nameof(FindAllVoucherConversionHistory)}: dto = {JsonSerializer.Serialize(dto)}, tradingProviderId = {tradingProviderId}");

            var query = (from voucher in _epicSchemaDbContext.LoyVouchers.AsNoTracking()
                         join conversionPointDetail in _epicSchemaDbContext.LoyConversionPointDetails on voucher.Id equals conversionPointDetail.VoucherId
                         join conversionPoint in _epicSchemaDbContext.LoyConversionPoints on conversionPointDetail.ConversionPointId equals conversionPoint.Id
                         let finishedDate = _epicSchemaDbContext.LoyConversionPointStatusLogs.Where(l => l.ConversionPointId == conversionPoint.Id && l.Status == LoyConversionPointStatus.FINISHED).OrderByDescending(x => x.Id).FirstOrDefault().CreatedDate
                         join investor in _epicSchemaDbContext.Investors on conversionPoint.InvestorId equals investor.InvestorId
                         where investor.InvestorId == dto.InvestorId && voucher.Deleted == YesNo.NO && conversionPointDetail.Deleted == YesNo.NO && conversionPoint.Deleted == YesNo.NO && investor.Deleted == YesNo.NO 
                             && (dto.Status == null || (dto.Status == conversionPoint.Status && voucher.ExpiredDate != null && voucher.ExpiredDate >= DateTime.Now)
                                || (dto.Status == LoyConversionPointStatus.CANCELED && LoyConversionPointStatus.CANCELED == conversionPoint.Status))
                             && (string.IsNullOrEmpty(dto.Keyword) || voucher.Name.Contains(dto.Keyword) || voucher.Code.Contains(dto.Keyword)) 
                             && (string.IsNullOrEmpty(dto.VoucherType) || voucher.VoucherType == dto.VoucherType) 
                             && (tradingProviderId == null || tradingProviderId == voucher.TradingProviderId)
                             && (dto.IsExpired == null || (dto.IsExpired.Value && voucher.ExpiredDate != null && voucher.ExpiredDate < DateTime.Now)
                                || (!dto.IsExpired.Value && voucher.ExpiredDate != null && voucher.ExpiredDate >= DateTime.Now))
                         orderby conversionPoint.Id
                         select new PointInvestorConversionHistoryDto
                         {
                             ConversionPointDetailQuantity = conversionPointDetail.Quantity,
                             ConversionPointFinishedDate = finishedDate,
                             ConversionPointDetailId = conversionPointDetail.Id,
                             VoucherId = voucher.Id,
                             Code = voucher.Code,
                             Name = voucher.Name,
                             VoucherType = voucher.VoucherType,
                             UseType = voucher.UseType,
                             Value = voucher.Value,
                             Point = voucher.Point,
                             PublishNum = voucher.PublishNum,
                             EndDate = voucher.EndDate,
                             ExchangeRoundNum = voucher.ExchangeRoundNum,
                             ExpiredDate = voucher.ExpiredDate,
                             StartDate = voucher.StartDate,
                             Unit = voucher.Unit,
                             ConversionPointStatus = conversionPoint.Status,
                             // Nếu không phải trạng thái hủy duyệt thì kiểm tra xem đã hết hạn hay chưa
                             IsExpired = conversionPoint.Status != LoyConversionPointStatus.CANCELED && voucher.ExpiredDate != null && voucher.ExpiredDate < DateTime.Now,
                         }).ToList().SelectMany(p => Enumerable.Repeat(p, p.ConversionPointDetailQuantity))
                         .OrderByDescending(p => p.ConversionPointDetailId);
            var count = query.Count();
            //query = query.OrderDynamic(dto.Sort);
            var result = new PagingResult<PointInvestorConversionHistoryDto>
            {
                TotalItems = count,
                Items = query.Skip(dto.Skip).Take(dto.PageSize)
            };
            return result;
        }

        /// <summary>
        /// Tìm kiếm nhà đầu tư theo số điện thoại và điểm ranh của theo đại lý
        /// </summary>
        /// <param name="phone"></param>
        /// <param name="tradingProviderId"></param>
        /// <returns></returns>
        public List<FindLoyPointInvestorDto> FindPointInvestor(string phone, int? investorId, int? tradingProviderId)
        {
            _logger.LogInformation($"{nameof(FindPointInvestor)}: phone = {phone}, tradingProviderId = {tradingProviderId}");

            var investorQuery = from investor in _epicSchemaDbContext.Investors
                                join cifCode in _epicSchemaDbContext.CifCodes on investor.InvestorId equals cifCode.InvestorId
                                from iden in _epicSchemaDbContext.InvestorIdentifications.AsNoTracking().Where(x => x.InvestorId == investor.InvestorId && x.IsDefault == YesNo.YES && x.Deleted == YesNo.NO).DefaultIfEmpty()
                                from address in _epicSchemaDbContext.InvestorContactAddresses.AsNoTracking().Where(x => x.InvestorId == investor.InvestorId && x.IsDefault == YesNo.YES && x.Deleted == YesNo.NO).DefaultIfEmpty()
                                join pointInvestor in _epicSchemaDbContext.LoyPointInvestors.Where(l => l.TradingProviderId == tradingProviderId && l.Deleted == YesNo.NO) on investor.InvestorId equals pointInvestor.InvestorId into pointInvestors
                                from pointInvestor in pointInvestors.DefaultIfEmpty()
                                where investor.Deleted == YesNo.NO && cifCode.Deleted == YesNo.NO
                                && investor.Status == Status.ACTIVE && ((phone != null && (investor.Phone == phone || investor.RepresentativePhone == phone))
                                    || (investorId != null && investorId == investor.InvestorId))
                                select new FindLoyPointInvestorDto
                                {
                                    InvestorId = investor.InvestorId,
                                    Phone = investor.Phone,
                                    Email = investor.Email,
                                    CifCode = cifCode.CifCode,
                                    Fullname = iden.Fullname,
                                    Address = address.ContactAddress,
                                    RankName = _epicSchemaDbContext.LoyRanks.FirstOrDefault(x => x.PointStart <= pointInvestor.TotalPoint && pointInvestor.TotalPoint <= x.PointEnd && (tradingProviderId == null || tradingProviderId == x.TradingProviderId)
                                                 && x.Deleted == YesNo.NO && x.Status == LoyRankStatus.ACTIVE).Name,
                                    LoyCurrentPoint = pointInvestor.CurrentPoint,
                                    LoyTotalPoint = pointInvestor.TotalPoint,
                                };
            return investorQuery.ToList();
        }
    }
}
