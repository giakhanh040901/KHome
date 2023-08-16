using DocumentFormat.OpenXml.Bibliography;
using EPIC.DataAccess.Base;
using EPIC.DataAccess.Models;
using EPIC.Entities.DataEntities;
using EPIC.Entities.Dto.ManagerInvestor;
using EPIC.LoyaltyEntities.DataEntities;
using EPIC.LoyaltyEntities.Dto.LoyVoucherInvestor;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Identity;
using EPIC.Utils.ConstantVariables.Loyalty;
using EPIC.Utils.ConstantVariables.Shared;
using EPIC.Utils.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Org.BouncyCastle.Crypto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace EPIC.LoyaltyRepositories
{
    public class LoyVoucherInvestorEFRepository : BaseEFRepository<LoyVoucherInvestor>
    {
        public LoyVoucherInvestorEFRepository(EpicSchemaDbContext dbContext, ILogger logger) : base(dbContext, logger, $"{DbSchemas.EPIC_LOYALTY}.{LoyVoucherInvestor.SEQ}")
        {
        }

        /// <summary>
        /// Gán voucher cho khách
        /// </summary>
        /// <param name="input"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        public LoyVoucherInvestor Add(LoyVoucherInvestor input, string username)
        {
            _logger.LogInformation($"{nameof(LoyVoucherInvestor)}->{nameof(Add)}: input = {JsonSerializer.Serialize(input)}, username = {username}");

            input.Id = (int)NextKey();
            input.CreatedBy = username;
            input.Status = LoyVoucherInvestorStatus.KICH_HOAT;

            return _dbSet.Add(input).Entity;
        }

        /// <summary>
        /// Update trạng thái của voucher với khách
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="username"></param>
        public void UpdateStatus(UpdateVoucherInvestorStatusDto dto, string username)
        {
            _logger.LogInformation($"{nameof(UpdateStatus)}: dto = {JsonSerializer.Serialize(dto)}, username = {username}");

            var entity = _dbSet.FirstOrDefault(x => x.Id == dto.VoucherInvestorId && x.Deleted == YesNo.NO).ThrowIfNull<LoyVoucherInvestor>(_epicSchemaDbContext, ErrorCode.LoyVoucherNotFound);
            entity.Status = dto.Status;
        }

        /// <summary>
        /// Lấy danh sách khách và voucher
        /// Tạm thời partner vào là ko thấy. Trading hoặc root vào thì mới thấy được khách
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="tradingProviderId"></param>
        /// <returns></returns>
        public PagingResult<ViewInvestorVoucherDto> FindInvestorVoucherPaging(FindAllInvestorForVoucherDto dto, int? tradingProviderId, int? partnerId)
        {
            _logger.LogInformation($"{nameof(UpdateStatus)}: dto = {JsonSerializer.Serialize(dto)}, tradingProviderId = {tradingProviderId}, partnerId = {partnerId}");

            var query = (from investor in _epicSchemaDbContext.Investors
                         join cifcode in _epicSchemaDbContext.CifCodes on investor.InvestorId equals cifcode.InvestorId
                         from identification in _epicSchemaDbContext.InvestorIdentifications.Where(i => i.InvestorId == investor.InvestorId && i.Status == Status.ACTIVE && i.Deleted == YesNo.NO)
                                     .OrderByDescending(c => c.IsDefault).ThenByDescending(c => c.Id).Take(1).DefaultIfEmpty()
                         from point in _epicSchemaDbContext.LoyPointInvestors.AsNoTracking().Where(x => x.InvestorId == investor.InvestorId && tradingProviderId == x.TradingProviderId && x.Deleted == YesNo.NO).DefaultIfEmpty()
                         from rank in _epicSchemaDbContext.LoyRanks.AsNoTracking().Where(x => x.PointStart <= point.TotalPoint && point.TotalPoint <= x.PointEnd && (tradingProviderId == null || tradingProviderId == x.TradingProviderId) && x.Deleted == YesNo.NO && x.Status == LoyRankStatus.ACTIVE).DefaultIfEmpty()
                         from user in _epicSchemaDbContext.Users.Where(x => x.InvestorId == investor.InvestorId && x.UserType == UserTypes.INVESTOR && x.IsDeleted == YesNo.NO)//.DefaultIfEmpty()
                         where investor.Deleted == YesNo.NO && (string.IsNullOrEmpty(dto.Sex) || dto.Sex == identification.Sex)
                             && (string.IsNullOrEmpty(dto.Keyword) || identification.Fullname.ToLower().Contains(dto.Keyword.ToLower()) || investor.Phone.Contains(dto.Keyword) || investor.Email.ToLower().Contains(dto.Keyword.ToLower()) || cifcode.CifCode.ToLower().Contains(dto.Keyword.ToLower()))
                             // Kiểm tra khách hàng từ bảng quan hệ với Đại lý và Sale
                             && (((new string[] { InvestorStatus.LOCKED, InvestorStatus.ACTIVE, InvestorStatus.DEACTIVE }.Contains(investor.Status)) && UserStatus.ACTIVE == user.Status
                                    && (_epicSchemaDbContext.InvestorTradingProviders.Any(investorTrading => investorTrading.Deleted == YesNo.NO
                                    && tradingProviderId == investorTrading.TradingProviderId && investorTrading.InvestorId == investor.InvestorId)
                                    || (from eis in _epicSchemaDbContext.InvestorSales
                                        join esctp in _epicSchemaDbContext.SaleTradingProviders on eis.SaleId equals esctp.SaleId
                                        where eis.InvestorId == investor.InvestorId && esctp.Deleted == YesNo.NO && eis.Deleted == YesNo.NO && tradingProviderId == esctp.TradingProviderId
                                        select eis.InvestorId).Any()))
                             // Khách hàng chưa xác minh có nhập mã giới thiệu sale thuộc đại lý
                             || ((investor.Step == null || investor.Step == 1 || investor.Step == 2) && user != null && user.Status == UserStatus.TEMP
                                // Nhập mã giới thiệu sale là Investor
                                && ((from inv in _epicSchemaDbContext.Investors.Where(x => x.ReferralCodeSelf == investor.ReferralCode && x.Deleted == YesNo.NO && x.Status == InvestorStatus.ACTIVE).DefaultIfEmpty()
                                     from saleInv in _epicSchemaDbContext.Sales.Where(x => x.InvestorId == inv.InvestorId && x.Deleted == YesNo.NO).DefaultIfEmpty()
                                     from saleTradingInv in _epicSchemaDbContext.SaleTradingProviders.Where(x => x.SaleId == saleInv.SaleId && x.Deleted == YesNo.NO).DefaultIfEmpty()
                                     where (tradingProviderId == null || tradingProviderId == saleTradingInv.TradingProviderId)
                                     select saleTradingInv.TradingProviderId).Any()
                                // Nhập mã giới thiệu sale là BussinessCustomer
                                || (from bus in _epicSchemaDbContext.BusinessCustomers.Where(x => x.ReferralCodeSelf == investor.ReferralCode && x.Deleted == YesNo.NO).DefaultIfEmpty()
                                    from saleBus in _epicSchemaDbContext.Sales.Where(x => x.BusinessCustomerId == bus.BusinessCustomerId && x.Deleted == YesNo.NO).DefaultIfEmpty()
                                    from saleTradingBus in _epicSchemaDbContext.SaleTradingProviders.Where(x => x.SaleId == saleBus.SaleId && x.Deleted == YesNo.NO).DefaultIfEmpty()
                                    where (tradingProviderId == null || tradingProviderId == saleTradingBus.TradingProviderId)
                                    select saleTradingBus.TradingProviderId).Any())))
                         orderby investor.InvestorId descending
                         select new ViewInvestorVoucherDto
                         {
                             Email = investor.Email,
                             Phone = investor.Phone,
                             InvestorId = investor.InvestorId,
                             IsVerified = investor.Step >= InvestorAppStep.DA_EKYC,
                             CifCode = cifcode.CifCode,
                             RankId = rank.Id,
                             RankName = rank.Name,
                             UserId = user == null ? 0 : user.UserId,
                             Fullname = identification.Fullname,
                             Sex = identification.Sex,
                             LoyTotalPoint = point.TotalPoint ?? 0,
                             LoyCurrentPoint = point.CurrentPoint ?? 0,
                             Username = user.UserName,
                             // Số lượng Voucher đếm trong quy đổi voucher với trạng thái Hoàn thành
                             VoucherNum = _epicSchemaDbContext.LoyConversionPointDetails.Where(d => d.Deleted == YesNo.NO && _epicSchemaDbContext.LoyConversionPoints.Any(c => (tradingProviderId == null || tradingProviderId == c.TradingProviderId) && c.Status == LoyConversionPointStatus.FINISHED
                                              && d.ConversionPointId == c.Id && c.Deleted == YesNo.NO && c.InvestorId == investor.InvestorId)).Sum(d => d.Quantity)
                         })
                         .Where(x => (dto.IsAddedVoucher == null || ((dto.IsAddedVoucher ?? false) && x.VoucherNum > 0) || (!(dto.IsAddedVoucher ?? false) && x.VoucherNum == 0))
                                && (dto.IsCheckedInvestor == null || (dto.IsCheckedInvestor == true && x.IsVerified) || (dto.IsCheckedInvestor == false && !x.IsVerified))
                                && (dto.Rank == null || dto.Rank == x.RankId));

            query = query.OrderByDescending(x => x.VoucherNum).ThenByDescending(x => x.InvestorId);
            var totalItems = query.Count();
            query = query.OrderDynamic(dto.Sort);

            if (dto.PageSize != -1)
            {
                query = query.Skip(dto.Skip).Take(dto.PageSize);
            }

            return new PagingResult<ViewInvestorVoucherDto>
            {
                TotalItems = totalItems,
                Items = query
            };
        }

        /// <summary>
        /// Lấy list investor voucher theo voucher id
        /// </summary>
        /// <param name="voucherId"></param>
        /// <returns></returns>
        public List<LoyVoucherInvestor> FindInvestorVoucherByVoucherId(int voucherId)
        {
            var query = _epicSchemaDbContext.LoyVoucherInvestors.Where(x => x.VoucherId == voucherId && x.Deleted == YesNo.NO);
            return query.ToList();
        }

        /// <summary>
        /// Lấy list voucher theo investor id
        /// tradingProviderId và partnerId cùng null thì lấy hết (aka epic)
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="tradingProviderId"></param>
        /// <param name="partnerId"></param>
        /// <returns></returns>
        public PagingResult<ViewVoucherByInvestorDto> FindByInvestorIdPaging(FindVoucherByInvestorIdDto dto, int? tradingProviderId, int? partnerId)
        {
            _logger.LogInformation($"{nameof(FindByInvestorIdPaging)} : dto = {JsonSerializer.Serialize(dto)}, tradingProviderId = {tradingProviderId}, partnerId = {partnerId}");

            List<int> listTrading = null;

            if (partnerId != null)
            {
                //listTrading = (from tradingPartner in _epicSchemaDbContext.TradingProviderPartners.AsNoTracking().Where(x => x.PartnerId == partnerId && x.Deleted == YesNo.NO)
                //               from trading in _epicSchemaDbContext.TradingProviders.AsNoTracking().Where(x => x.TradingProviderId == tradingPartner.TradingProviderId && x.Deleted == YesNo.NO)
                //               select trading.TradingProviderId).ToList();
            }
            else if (tradingProviderId != null)
            {
                listTrading = new List<int> { tradingProviderId ?? -1 };
            }

            var query = (from vi in _epicSchemaDbContext.LoyVoucherInvestors.AsNoTracking().Where(x => x.InvestorId == dto.InvestorId && x.Deleted == YesNo.NO)
                         from v in _epicSchemaDbContext.LoyVouchers.AsNoTracking().Where(x => x.Id == vi.VoucherId && x.Deleted == YesNo.NO)
                         where (string.IsNullOrEmpty(dto.Keyword) || v.Name.ToLower().Contains(dto.Keyword.ToLower())) &&
                             (string.IsNullOrEmpty(dto.VoucherType) || v.VoucherType == dto.VoucherType) &&
                             (dto.Status == null || dto.Status == vi.Status) &&
                             (tradingProviderId == null || tradingProviderId == v.TradingProviderId) &&
                             (partnerId == null || partnerId == v.PartnerId)
                         select new ViewVoucherByInvestorDto
                         {
                             Status = vi.Status,
                             VoucherType = v.VoucherType,
                             VoucherInvestorId = vi.Id,
                             VoucherId = v.Id,
                             CreatedDate = v.CreatedDate,
                             EndDate = v.EndDate,
                             Name = v.Name,
                             StartDate = v.StartDate,
                             Value = v.Value,
                             CreatedBy = v.CreatedBy,
                         }
                        ).OrderByDescending(x => x.VoucherInvestorId);

            return new PagingResult<ViewVoucherByInvestorDto>
            {
                TotalItems = query.Count(),
                Items = query.Skip(dto.Skip).Take(dto.PageSize)
            };
        }

        /// <summary>
        /// Lấy list voucher active cho khách trên app
        /// </summary>
        /// <param name="investorId"></param>
        /// <returns></returns>
        public List<AppViewVoucherByInvestorDto> AppFindByInvestorId(int investorId, int? tradingProviderId, string useType)
        {
            _logger.LogInformation($"{nameof(AppFindByInvestorId)}: investorId = {investorId}");
            var now = DateTime.Now.Date;

            var query = (from v in _epicSchemaDbContext.LoyVouchers.AsNoTracking().Where(x => x.Deleted == YesNo.NO && x.TradingProviderId == tradingProviderId
                         && x.Status == LoyVoucherStatus.KICH_HOAT && x.StartDate.Date <= now && (x.EndDate == null || now <= x.EndDate.Value.Date)
                         && (x.ExpiredDate == null || now <= x.ExpiredDate.Value.Date) && (useType == null || useType == x.UseType) && x.IsShowApp == YesNo.YES)
                         orderby v.ExpiredDate, v.Id descending
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
                         }
                        ).ToList();
            return query;
        }

        /// <summary>
        /// Lấy list voucher hết hạn cho app
        /// </summary>
        /// <param name="investorId"></param>
        /// <returns></returns>
        public List<AppViewVoucherByInvestorDto> AppFindExpiredByInvestorId(int investorId)
        {
            _logger.LogInformation($"{nameof(AppFindExpiredByInvestorId)}: investorId = {investorId}");
            var now = DateTime.Now;

            var query = (from vi in _epicSchemaDbContext.LoyVoucherInvestors.AsNoTracking().Where(x => x.InvestorId == investorId && x.Deleted == YesNo.NO && !(new int[] { LoyVoucherStatus.DA_XOA, LoyVoucherInvestorStatus.HUY_KICH_HOAT }.Contains(x.Status)))
                         from v in _epicSchemaDbContext.LoyVouchers.AsNoTracking().Where(x => x.Id == vi.VoucherId && x.Deleted == YesNo.NO && now > x.EndDate)
                         select new AppViewVoucherByInvestorDto
                         {
                             VoucherType = v.VoucherType,
                             Id = vi.Id,
                             CreatedDate = vi.CreatedDate,
                             EndDate = v.EndDate,
                             LinkVoucher = v.LinkVoucher,
                             Name = v.Name,
                             DisplayName = v.DisplayName,
                             Point = v.Point,
                             Avatar = v.Avatar,
                             StartDate = v.StartDate,
                             Value = v.Value
                         }
                        ).ToList();

            return query;
        }

        /// <summary>
        /// Xóa mềm VoucherInvestor
        /// </summary>
        /// <param name="id"></param>
        public void DeletedById(int id)
        {
            _logger.LogInformation($"{nameof(DeletedById)}: investorId = {id}");

            var entity = _epicSchemaDbContext.LoyVoucherInvestors.FirstOrDefault(x => x.Id == id);
            if (entity != null)
            {
                entity.Deleted = YesNo.YES;
            }
        }

        public DataNotificationAddVoucherToInvestorDto GetDataToSendNotificationAddVoucherToInvestor(int voucherInvestorId)
        {
            _logger.LogInformation($"{nameof(GetDataToSendNotificationAddVoucherToInvestor)}: voucherInvestorId={voucherInvestorId}");

            var query = from vi in _epicSchemaDbContext.LoyVoucherInvestors.AsNoTracking().Where(x => x.Id == voucherInvestorId && x.Deleted == YesNo.NO)
                        from v in _epicSchemaDbContext.LoyVouchers.AsNoTracking().Where(x => x.Id == vi.VoucherId && x.Deleted == YesNo.NO)
                        from inv in _epicSchemaDbContext.Investors.AsNoTracking().Where(x => x.InvestorId == vi.InvestorId && x.Deleted == YesNo.NO)
                        from iden in _epicSchemaDbContext.InvestorIdentifications.AsNoTracking().Where(x => x.InvestorId == inv.InvestorId && x.IsDefault == YesNo.YES && x.Deleted == YesNo.NO).DefaultIfEmpty()
                        select new DataNotificationAddVoucherToInvestorDto
                        {
                            Fullname = iden.Fullname,
                            VoucherName = v.Name,
                            VoucherType = v.VoucherType,
                            StartDate = v.StartDate,
                            EndDate = v.EndDate,
                            InvestorId = inv.InvestorId,
                            TradingProviderId = v.TradingProviderId,
                        };

            return query.FirstOrDefault();
        }
    }
}
