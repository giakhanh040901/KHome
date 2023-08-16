using EPIC.DataAccess.Base;
using EPIC.DataAccess.Models;
using EPIC.Entities.Dto.ManagerInvestor;
using EPIC.LoyaltyEntities.DataEntities;
using EPIC.LoyaltyEntities.Dto.LoyVoucher;
using EPIC.LoyaltyEntities.Dto.LoyVoucherInvestor;
using EPIC.RealEstateEntities.DataEntities;
using EPIC.Utils;
using EPIC.Utils.ConfigModel;
using EPIC.Utils.ConstantVariables.Identity;
using EPIC.Utils.ConstantVariables.Loyalty;
using EPIC.Utils.ConstantVariables.Shared;
using EPIC.Utils.Linq;
using EPIC.Utils.Security;
using Hangfire;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.VisualBasic;
using MySqlX.XDevAPI.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace EPIC.LoyaltyRepositories
{
    public class LoyVoucherEFRepository : BaseEFRepository<LoyVoucher>
    {
        private readonly LinkVoucherConfiguration _linkVoucherConfig;
        public LoyVoucherEFRepository(EpicSchemaDbContext dbContext, ILogger logger, LinkVoucherConfiguration config) : base(dbContext, logger, $"{DbSchemas.EPIC_LOYALTY}.{LoyVoucher.SEQ}")
        {
            _linkVoucherConfig = config;
        }

        /// <summary>
        /// Thêm mới voucher
        /// </summary>
        /// <param name="input"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        public LoyVoucher Add(LoyVoucher input, string username)
        {
            _logger.LogInformation($"{nameof(LoyVoucher)}->{nameof(Add)}: input = {JsonSerializer.Serialize(input)}, username = {username}");

            if (input.VoucherType != LoyVoucherTypes.DIEN_TU && string.IsNullOrEmpty(input.DescriptionContentType) && !string.IsNullOrEmpty(input.DescriptionContent))
            {
                _logger.LogError($"{nameof(LoyVoucher)}->{nameof(Add)}: Loại mô tả không được bỏ trống");
                ThrowException(ErrorCode.LoyVoucherDescriptionContentTypeRequired);
            }

            input.Id = (int)NextKey();
            input.CreatedBy = username;
            input.LinkVoucher = encryptLinkVoucher(input.LinkVoucher);
            input.Status = LoyVoucherStatus.KICH_HOAT;

            var result = _dbSet.Add(input).Entity;
            result.LinkVoucher = decryptLinkVoucher(result.LinkVoucher);

            return result;
        }

        /// <summary>
        /// Tìm voucher theo voucher id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public LoyVoucher FindById(int id)
        {
            _logger.LogInformation($"{nameof(LoyVoucher)}->{nameof(FindById)}: id = {id}");

            var voucher = _dbSet.FirstOrDefault(x => x.Id == id && x.Deleted == YesNo.NO);
            if (voucher != null)
            {
                voucher.LinkVoucher = decryptLinkVoucher(voucher.LinkVoucher);
            }
            return voucher;
        }

        /// <summary>
        /// Tìm voucher theo id yêu cầu tích điểm/tiêu điểm
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public LoyVoucher FindByHisAccumulatePointId(int id)
        {
            _logger.LogInformation($"{nameof(LoyVoucher)}->{nameof(FindByHisAccumulatePointId)}: id = {id}");

            var voucher = (from vi in _epicSchemaDbContext.LoyVoucherInvestors
                          join v in _dbSet on vi.VoucherId equals v.Id
                          where vi.HisAccumulatePointId == id && vi.Deleted == YesNo.NO && v.Deleted == YesNo.NO
                          select v).FirstOrDefault();

            if (voucher != null)
            {
                voucher.LinkVoucher = decryptLinkVoucher(voucher.LinkVoucher);
            }
            return voucher;
        }

        /// <summary>
        /// Tìm kiếm voucher (cms)
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="tradingProviderId"></param>
        /// <returns></returns>
        public PagingResult<ViewListVoucherDto> FindAll(FindAllVoucherDto dto, int? tradingProviderId, int? partnerId)
        {
            _logger.LogInformation($"{nameof(PagingResult<ViewListVoucherDto>)}->{nameof(FindAll)}: dto = {JsonSerializer.Serialize(dto)}, tradingProviderId = {tradingProviderId}, partnerId = {partnerId}");

            var query = from v in _epicSchemaDbContext.LoyVouchers.AsNoTracking()
                        where (string.IsNullOrEmpty(dto.Keyword) || v.Name.Contains(dto.Keyword)) &&
                            (string.IsNullOrEmpty(dto.VoucherType) || v.VoucherType == dto.VoucherType) &&
                            (string.IsNullOrEmpty(dto.UseType) || v.UseType == dto.UseType) &&
                            (string.IsNullOrEmpty(dto.IsShowApp) || v.IsShowApp == dto.IsShowApp) &&
                            (dto.ExpiredDate == null || v.ExpiredDate == dto.ExpiredDate) &&
                            (tradingProviderId == null || tradingProviderId == v.TradingProviderId) &&
                            (partnerId == null || partnerId == v.PartnerId) && v.Deleted == YesNo.NO &&
                            (dto.Status == null
                            || (dto.Status == v.Status && ((v.Status == LoyVoucherStatus.KICH_HOAT && (DateTime.Now.Date <= v.ExpiredDate.Value.Date || v.ExpiredDate == null)) || v.Status != LoyVoucherStatus.KICH_HOAT))
                            || (dto.Status == LoyVoucherStatus.HET_HAN && DateTime.Now.Date > v.ExpiredDate.Value.Date && v.Status == LoyVoucherStatus.KICH_HOAT))
                        orderby v.StartDate
                        select new ViewListVoucherDto
                        {
                            VoucherId = v.Id,
                            Code = v.Code,
                            Name = v.Name,
                            VoucherType = v.VoucherType,
                            UseType = v.UseType,
                            Value = v.Value,
                            Point = v.Point,
                            PublishNum = v.PublishNum,
                            DeliveryNum = 0,
                            IsShowApp = v.IsShowApp,
                            // Nếu quá hạn 
                            Status = v.Status,
                            IsExpiredDate = DateTime.Now.Date > v.ExpiredDate.Value.Date,
                            ExpiredDate = v.ExpiredDate,
                            CreatedBy = v.CreatedBy,
                            CreatedDate = v.CreatedDate,
                            EndDate = v.EndDate,
                            ExchangeRoundNum = v.ExchangeRoundNum,
                            IsHot = v.IsHot,
                            IsUseInLuckyDraw = v.IsUseInLuckyDraw,
                            LinkVoucher = v.LinkVoucher,
                            StartDate = v.StartDate,
                            DisplayName = v.DisplayName,
                            BatchEntryDate = v.BatchEntryDate,
                            Unit = v.Unit,
                            // Số lượng cấp phát(Lấy theo trạng thái hoàn thành trong Yêu cầu đổi điểm)
                            ConversionQuantiy = _epicSchemaDbContext.LoyConversionPointDetails.Where(d => d.VoucherId == v.Id && d.Deleted == YesNo.NO 
                                                    && _epicSchemaDbContext.LoyConversionPoints.Any(c => d.ConversionPointId == c.Id && c.TradingProviderId == v.TradingProviderId && c.Status == LoyConversionPointStatus.FINISHED)).Sum(d => d.Quantity)
                        };
            query = query.OrderByDescending(o => o.VoucherId);
            if (dto.Sort != null)
            {
                query = query.OrderDynamic(dto.Sort);
            }

            var result = new PagingResult<ViewListVoucherDto>
            {
                TotalItems = query.Count(),
                Items = query.Skip(dto.Skip).Take(dto.PageSize).ToList()
            };
            return result;

        }

        /// <summary>
        /// Lịch sử cấp phát: Nhân bản bản ghi theo số lượng trong conversionPointDetail
        /// </summary>
        public PagingResult<VoucherConversionHistoryDto> FindAllVoucherConversionHistory(FilterVoucherConversionHistoryDto dto, int? tradingProviderId)
        {
            _logger.LogInformation($"{nameof(PagingResult<VoucherConversionHistoryDto>)}->{nameof(FindAll)}: dto = {JsonSerializer.Serialize(dto)}, tradingProviderId = {tradingProviderId}");

            var query = (from voucher in _epicSchemaDbContext.LoyVouchers.AsNoTracking()
                         join conversionPointDetail in _epicSchemaDbContext.LoyConversionPointDetails on voucher.Id equals conversionPointDetail.VoucherId
                         join conversionPoint in _epicSchemaDbContext.LoyConversionPoints on conversionPointDetail.ConversionPointId equals conversionPoint.Id
                         let finishedDate = _epicSchemaDbContext.LoyConversionPointStatusLogs.Where(l => l.ConversionPointId == conversionPoint.Id && l.Status == LoyConversionPointStatus.FINISHED).OrderByDescending(x => x.Id).FirstOrDefault().CreatedDate
                         join investor in _epicSchemaDbContext.Investors on conversionPoint.InvestorId equals investor.InvestorId
                         where voucher.Deleted == YesNo.NO && conversionPointDetail.Deleted == YesNo.NO && conversionPoint.Deleted == YesNo.NO
                             && conversionPoint.Status == LoyConversionPointStatus.FINISHED && investor.Deleted == YesNo.NO
                             && (string.IsNullOrEmpty(dto.Keyword) || voucher.Name.Contains(dto.Keyword) || voucher.Code.Contains(dto.Keyword)) &&
                             (string.IsNullOrEmpty(dto.VoucherType) || voucher.VoucherType == dto.VoucherType) &&
                             (string.IsNullOrEmpty(dto.UseType) || voucher.UseType == dto.UseType) &&
                             (dto.ConversionPointFinishedDate == null || finishedDate.Value.Date == dto.ConversionPointFinishedDate.Value.Date) &&
                             (tradingProviderId == null || tradingProviderId == voucher.TradingProviderId)
                         orderby voucher.Id
                         select new VoucherConversionHistoryDto
                         {
                             Customer = investor.Phone,
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
                             Status = voucher.Status,
                             CreatedBy = voucher.CreatedBy,
                             EndDate = voucher.EndDate,
                             ExchangeRoundNum = voucher.ExchangeRoundNum,
                             ExpiredDate = voucher.ExpiredDate,
                             StartDate = voucher.StartDate,
                             Unit = voucher.Unit
                         }).ToList().SelectMany(p => Enumerable.Repeat(p, p.ConversionPointDetailQuantity));
            query = query.OrderByDescending(x => x.ConversionPointDetailId);
            var result = new PagingResult<VoucherConversionHistoryDto>
            {
                TotalItems = query.Count(),
                Items = query.Skip(dto.Skip).Take(dto.PageSize).ToList()
            };
            return result;
        }
        /// <summary>
        /// Get list voucher chưa cấp phát cho khách hàng, chưa đến ngày hết hạn và chưa xóa
        /// </summary>
        /// <returns></returns>
        public List<LoyVoucher> FindFreeVoucher(int? tradingProviderId, int? partnerId)
        {
            var now = DateTime.Now;
            _logger.LogInformation($"{nameof(FindFreeVoucher)}");

            var query = from voucher in _dbSet.AsNoTracking().Where(x => x.Deleted == YesNo.NO)
                        join z in (
                            (
                                from v in _dbSet.AsNoTracking().Where(x => x.Deleted == YesNo.NO)
                                from voucherInv in _epicSchemaDbContext.LoyVoucherInvestors.AsNoTracking().Where(x => x.Deleted == YesNo.NO && x.VoucherId == v.Id).DefaultIfEmpty()
                                select new
                                {
                                    Id = v.Id,
                                    VoucherInvId = voucherInv.VoucherId
                                }
                            ).GroupBy(x => new
                            {
                                Id = x.Id,
                            })
                            .Select(x => new
                            {
                                Id = x.Key.Id,
                                Count = x.Count(x => x.VoucherInvId != null)
                            })
                        ) on voucher.Id equals z.Id
                        where ((tradingProviderId == null && partnerId == null) || tradingProviderId == voucher.TradingProviderId || partnerId == voucher.PartnerId) &&
                               voucher.StartDate <= now && (voucher.EndDate == null || now <= voucher.EndDate) && z.Count == 0
                        orderby voucher.StartDate, voucher.Id descending
                        select voucher;

            return query.ToList();
        }

        /// <summary>
        /// Cập nhật voucher
        /// </summary>
        /// <param name="input"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        public LoyVoucher Update(LoyVoucher input, string username)
        {
            _logger.LogInformation($"{nameof(LoyVoucher)}->{nameof(Update)}: input = {JsonSerializer.Serialize(input)}, username = {username}");

            var voucher = _epicSchemaDbContext.LoyVouchers.FirstOrDefault(x => x.Id == input.Id && x.Deleted == YesNo.NO).ThrowIfNull(_epicSchemaDbContext, ErrorCode.LoyVoucherNotFound);
            voucher.ModifiedBy = username;
            voucher.ModifiedDate = DateTime.Now;
            voucher.Name = input.Name;
            voucher.Point = input.Point;
            voucher.Value = input.Value;
            voucher.Avatar = input.Avatar;
            voucher.BannerImageUrl = input.BannerImageUrl;
            voucher.StartDate = input.StartDate;
            voucher.EndDate = input.EndDate;
            voucher.DescriptionContent = input.DescriptionContent;
            voucher.DescriptionContentType = input.DescriptionContentType;
            voucher.LinkVoucher = encryptLinkVoucher(input.LinkVoucher);
            voucher.VoucherType = input.VoucherType;
            voucher.IsHot = input.IsHot == null ? YesNo.NO : input.IsHot;
            voucher.IsShowApp = input.IsShowApp == null ? YesNo.NO : input.IsShowApp;
            voucher.IsUseInLuckyDraw = input.IsUseInLuckyDraw == null ? YesNo.NO : input.IsUseInLuckyDraw;
            voucher.PublishNum = input.PublishNum;
            voucher.ExchangeRoundNum = input.ExchangeRoundNum;
            voucher.Code = input.Code;
            voucher.Unit = input.Unit;
            voucher.DisplayName = input.DisplayName;
            voucher.UseType = input.UseType;
            voucher.ExpiredDate = input.ExpiredDate;
            voucher.BatchEntryDate = input.BatchEntryDate;
            if (voucher.VoucherType != LoyVoucherTypes.DIEN_TU && string.IsNullOrEmpty(voucher.DescriptionContentType) && !string.IsNullOrEmpty(voucher.DescriptionContent))
            {
                _logger.LogError($"{nameof(LoyVoucher)}->{nameof(Update)}: Loại mô tả không được bỏ trống");
                ThrowException(ErrorCode.LoyVoucherDescriptionContentTypeRequired);
            }

            return voucher;
        }

        /// <summary>
        /// Cập nhật trạng thái voucher
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="username"></param>
        public void UpdateStatus(UpdateVoucherStatusDto dto, string username)
        {
            _logger.LogInformation($"{nameof(UpdateStatus)}: input = {JsonSerializer.Serialize(dto)}, username = {username}");

            var voucher = _epicSchemaDbContext.LoyVouchers.FirstOrDefault(x => x.Id == dto.Id && x.Deleted == YesNo.NO)
                .ThrowIfNull(_epicSchemaDbContext, ErrorCode.LoyVoucherNotFound);
            voucher.Status = dto.Status;
            if (voucher.Status == LoyVoucherStatus.HUY_KICH_HOAT)
            {
                voucher.IsShowApp = YesNo.NO;
            }
        }


        /// <summary>
        /// Xoá voucher khởi tạo (UPDATE TRẠNG THÁI)
        /// </summary>
        /// <param name="id"></param>
        public void DeleteById(int id, string username)
        {
            _logger.LogInformation($"{nameof(LoyVoucher)}->{nameof(DeleteById)}: id = {id}");
            bool any = _epicSchemaDbContext.LoyVoucherInvestors.AsNoTracking().Any(x => x.VoucherId == id && x.Deleted == YesNo.NO && x.Status == LoyVoucherStatus.KICH_HOAT);
  
            if (!any)
            {
                var voucher = _epicSchemaDbContext.LoyVouchers.FirstOrDefault(x => x.Id == id && x.Deleted == YesNo.NO).ThrowIfNull(_epicSchemaDbContext, ErrorCode.LoyVoucherNotFound);
                voucher.Status = LoyVoucherStatus.DA_XOA;
                voucher.IsShowApp= YesNo.NO;
                voucher.ModifiedBy = username;
                voucher.ModifiedDate = DateTime.Now;
            }
            else
            {
                _logger.LogError($"{nameof(DeleteById)}: Không xóa được voucher vì đã được gán cho khách");
                ThrowException(ErrorCode.LoyVoucherCanNotDeleteBecauseHaveVoucherInvestor);
            }
        }

        /// <summary>
        /// Mã hóa link voucher
        /// </summary>
        /// <param name="linkVoucher"></param>
        /// <returns></returns>
        private string encryptLinkVoucher(string linkVoucher)
        {
            try
            {
                return string.IsNullOrEmpty(linkVoucher) ? null : CryptographyUtils.EncryptString_Aes(linkVoucher, _linkVoucherConfig.Key, _linkVoucherConfig.Iv);
            }
            catch (Exception)
            {
                return linkVoucher;
            }
        }

        /// <summary>
        /// Giải mã link voucher
        /// </summary>
        /// <param name="linkVoucher"></param>
        /// <returns></returns>
        private string decryptLinkVoucher(string linkVoucher)
        {
            try
            {
                return string.IsNullOrEmpty(linkVoucher) ? null : CryptographyUtils.DecryptString_Aes(linkVoucher, _linkVoucherConfig.Key, _linkVoucherConfig.Iv);
            }
            catch (Exception)
            {
                return linkVoucher;
            }
        }

        /// <summary>
        /// Lấy danh sách 6 voucher nổi bật của đại lý
        /// </summary>
        public List<AppViewVoucherByInvestorDto> AppFindVoucherIsHot(int tradingProviderId)
        {
            _logger.LogInformation($"{nameof(AppFindVoucherIsHot)}: tradingProviderId = {tradingProviderId}");
            var now = DateTime.Now.Date;
            var query = (from v in _epicSchemaDbContext.LoyVouchers.AsNoTracking().Where(x => x.Deleted == YesNo.NO && x.TradingProviderId == tradingProviderId
                         && x.Status == LoyVoucherStatus.KICH_HOAT && x.IsShowApp == YesNo.YES && x.IsHot == YesNo.YES && x.StartDate.Date <= now 
                         && (x.EndDate == null || now <= x.EndDate.Value.Date) && (x.ExpiredDate == null || now <= x.ExpiredDate.Value.Date))
                         orderby v.Id descending
                         select new AppViewVoucherByInvestorDto
                         {
                             VoucherType = v.VoucherType,
                             Id = v.Id,
                             CreatedDate = v.CreatedDate,
                             EndDate = v.EndDate,
                             ExpiredDate = v.ExpiredDate,
                             LinkVoucher = v.LinkVoucher,
                             Name = v.Name,
                             Avatar = v.Avatar,
                             StartDate = v.StartDate,
                             Value = v.Value,
                             Point = v.Point,
                             DisplayName = v.DisplayName,
                         }).OrderBy(x => x.ExpiredDate).ThenByDescending(x => x.Id).Take(6).ToList();
            return query;
        }
    }
}
