using AutoMapper;
using EPIC.CoreRepositories;
using EPIC.DataAccess.Base;
using EPIC.DataAccess.Models;
using EPIC.Entities;
using EPIC.FileDomain.Services;
using EPIC.ImageAPI.Models;
using EPIC.LoyaltyDomain.Interfaces;
using EPIC.LoyaltyEntities.DataEntities;
using EPIC.LoyaltyEntities.Dto.LoyHistoryUpdate;
using EPIC.LoyaltyEntities.Dto.LoyLuckyProgram;
using EPIC.LoyaltyEntities.Dto.LoyLuckyProgramInvestor;
using EPIC.LoyaltyEntities.Dto.LoyLuckyRotationInterface;
using EPIC.LoyaltyEntities.Dto.LoyLuckyScenario;
using EPIC.LoyaltyEntities.Dto.LoyLuckyScenarioDetail;
using EPIC.LoyaltyRepositories;
using EPIC.Notification.Services;
using EPIC.Utils;
using EPIC.Utils.ConfigModel;
using EPIC.Utils.ConstantVariables.Identity;
using EPIC.Utils.ConstantVariables.Loyalty;
using EPIC.Utils.ConstantVariables.Shared;
using EPIC.Utils.DataUtils;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace EPIC.LoyaltyDomain.Implements
{
    public class LoyLuckyProgramServices : ILoyLuckyProgramServices
    {
        private readonly EpicSchemaDbContext _dbContext;
        private readonly IFileServices _fileServices;
        private readonly IMapper _mapper;
        private readonly ILogger<LoyLuckyProgramServices> _logger;
        private readonly string _connectionString;
        private readonly IHttpContextAccessor _httpContext;
        private readonly LoyaltyNotificationServices _loyaltyNotificationServices;
        private readonly LoyLuckyProgramRepository _loyLuckyProgramRepository;
        private readonly LoyLuckyScenarioEFRepository _loyLuckyScenarioEFRepository;
        private readonly LoyHistoryUpdateEFRepository _loyHistoryUpdateEFRepository;
        private readonly InvestorEFRepository _investorEFRepository;

        public LoyLuckyProgramServices(EpicSchemaDbContext dbContext,
            DatabaseOptions databaseOptions,
            IMapper mapper,
            IFileServices fileServices,
            ILogger<LoyLuckyProgramServices> logger,
            IHttpContextAccessor httpContextAccessor,
            IOptions<LinkVoucherConfiguration> linkVoucherConfiguration,
            LoyaltyNotificationServices loyaltyNotificationServices
        )
        {
            _dbContext = dbContext;
            _fileServices = fileServices;
            _mapper = mapper;
            _logger = logger;
            _connectionString = databaseOptions.ConnectionString;
            _httpContext = httpContextAccessor; ;
            _loyaltyNotificationServices = loyaltyNotificationServices;
            _loyLuckyProgramRepository = new LoyLuckyProgramRepository(dbContext, logger);
            _loyLuckyScenarioEFRepository = new LoyLuckyScenarioEFRepository(dbContext, logger);
            _loyHistoryUpdateEFRepository = new LoyHistoryUpdateEFRepository(dbContext, logger);
            _investorEFRepository = new InvestorEFRepository(dbContext, logger);
        }

        /// <summary>
        /// Thêm chương trình
        /// </summary>
        /// <param name="input"></param>
        public LoyLuckyProgramDto Add(CreateLuckyProgramDto input)
        {
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var username = CommonUtils.GetCurrentUsername(_httpContext);

            var transaction = _dbContext.Database.BeginTransaction();
            string avatarImageUrl = null;
            if (input.AvatarImageUrl != null)
            {
                avatarImageUrl = _fileServices.UploadImage(new UploadFileModel
                {
                    File = input.AvatarImageUrl,
                    Folder = FileFolder.Loyalty,
                });
            }

            var luckyProgram = _dbContext.LoyLuckyPrograms.Add(new LoyLuckyProgram
            {
                Id = (int)_loyLuckyProgramRepository.NextKey(),
                TradingProviderId = tradingProviderId,
                Code = input.Code,
                Name = input.Name,
                StartDate = input.StartDate,
                EndDate = input.EndDate,
                AvatarImageUrl = avatarImageUrl,
                DescriptionContent = input.DescriptionContent,
                DescriptionContentType = input.DescriptionContentType,
                JoinTimeSetting = input.JoinTimeSetting,
                NumberOfTurn = input.NumberOfTurn,
                StartTurnDate = input.StartTurnDate,
                ResetTurnQuantity = input.ResetTurnQuantity,
                ResetTurnType = input.ResetTurnType,
                MaxNumberOfTurnByIp = input.MaxNumberOfTurnByIp,
                Status = LoyLuckyProgramStatus.KICH_HOAT,
                CreatedBy = username
            }).Entity;
            _dbContext.SaveChanges();

            foreach (var item in input.LuckyScenarios)
            {
                string luckyScenarioAvatar = null;
                if (item.AvatarImageUrl != null)
                {
                    luckyScenarioAvatar = _fileServices.UploadImage(new UploadFileModel
                    {
                        File = item.AvatarImageUrl,
                        Folder = FileFolder.Loyalty,
                    });
                }

                var luckyScenarioItem = _dbContext.LoyLuckyScenarios.Add(new LoyLuckyScenario
                {
                    Id = (int)_loyLuckyScenarioEFRepository.NextKey(),
                    TradingProviderId = tradingProviderId,
                    LuckyProgramId = luckyProgram.Id,
                    LuckyScenarioType = item.LuckyScenarioType,
                    AvatarImageUrl = luckyScenarioAvatar,
                    PrizeQuantity = item.PrizeQuantity,
                    Name = item.Name,
                    CreatedBy = username,
                }).Entity;
                _dbContext.SaveChanges();

                foreach (var detailItem in item.LuckyScenarioDetails)
                {
                    _dbContext.LoyLuckyScenarioDetails.Add(new LoyLuckyScenarioDetail
                    {
                        Id = (int)_loyLuckyScenarioEFRepository.NextKey($"{DbSchemas.EPIC_LOYALTY}.{LoyLuckyScenarioDetail.SEQ}"),
                        LuckyScenarioId = luckyScenarioItem.Id,
                        SortOrder = detailItem.SortOrder,
                        VoucherId = detailItem.VoucherId,
                        Name = detailItem.VoucherId == null ? detailItem.Name : null,
                        Probability = detailItem.Probability,
                        Quantity = detailItem.Quantity,
                        CreatedBy = username
                    });
                }

                // Nếu có Giao diện vòng quay may mắn
                if (item.LuckyRotationInterface != null)
                {
                    string banner = null;
                    string background = null;
                    string iconHistory = null;
                    string iconPlay = null;
                    string iconRank = null;
                    string needleImage = null;
                    string rotationBackground = null;
                    string rotationImage = null;
                    if (item.LuckyRotationInterface.Banner != null)
                    {
                        banner = _fileServices.UploadImage(new UploadFileModel
                        {
                            File = item.LuckyRotationInterface.Banner,
                            Folder = FileFolder.Loyalty,
                        });
                    }
                    if (item.LuckyRotationInterface.Background != null)
                    {
                        background = _fileServices.UploadImage(new UploadFileModel
                        {
                            File = item.LuckyRotationInterface.Background,
                            Folder = FileFolder.Loyalty,
                        });
                    }
                    if (item.LuckyRotationInterface.IconHistory != null)
                    {
                        iconHistory = _fileServices.UploadImage(new UploadFileModel
                        {
                            File = item.LuckyRotationInterface.IconHistory,
                            Folder = FileFolder.Loyalty,
                        });
                    }
                    if (item.LuckyRotationInterface.IconPlay != null)
                    {
                        iconPlay = _fileServices.UploadImage(new UploadFileModel
                        {
                            File = item.LuckyRotationInterface.IconPlay,
                            Folder = FileFolder.Loyalty,
                        });
                    }
                    if (item.LuckyRotationInterface.IconRank != null)
                    {
                        iconRank = _fileServices.UploadImage(new UploadFileModel
                        {
                            File = item.LuckyRotationInterface.IconRank,
                            Folder = FileFolder.Loyalty,
                        });
                    }
                    if (item.LuckyRotationInterface.NeedleImage != null)
                    {
                        needleImage = _fileServices.UploadImage(new UploadFileModel
                        {
                            File = item.LuckyRotationInterface.NeedleImage,
                            Folder = FileFolder.Loyalty,
                        });
                    }
                    if (item.LuckyRotationInterface.RotationBackground != null)
                    {
                        rotationBackground = _fileServices.UploadImage(new UploadFileModel
                        {
                            File = item.LuckyRotationInterface.RotationBackground,
                            Folder = FileFolder.Loyalty,
                        });
                    }
                    if (item.LuckyRotationInterface.RotationImage != null)
                    {
                        rotationImage = _fileServices.UploadImage(new UploadFileModel
                        {
                            File = item.LuckyRotationInterface.RotationImage,
                            Folder = FileFolder.Loyalty,
                        });
                    }
                    var luckyRotationInterface = _dbContext.LoyLuckyRotationInterfaces.Add(new LoyLuckyRotationInterface
                    {
                        Id = (int)_loyLuckyScenarioEFRepository.NextKey($"{DbSchemas.EPIC_LOYALTY}.{LoyLuckyRotationInterface.SEQ}"),
                        LuckyScenarioId = luckyScenarioItem.Id,
                        CreatedBy = username,
                        Template = item.LuckyRotationInterface.Template,
                        ButtonColor = item.LuckyRotationInterface.ButtonColor,
                        ButtonHistory = item.LuckyRotationInterface.ButtonHistory.GetValueOrDefault(),
                        ButtonPlay = item.LuckyRotationInterface.ButtonPlay.GetValueOrDefault(),
                        ButtonRank = item.LuckyRotationInterface.ButtonRank.GetValueOrDefault(),
                        IconHome = item.LuckyRotationInterface.IconHome.GetValueOrDefault(),
                        ShowBanner = item.LuckyRotationInterface.ShowBanner.GetValueOrDefault(),
                        WinText = item.LuckyRotationInterface.WinText.GetValueOrDefault(),
                        WinTextColor = item.LuckyRotationInterface.WinTextColor,
                        WinTextBackgroundColor = item.LuckyRotationInterface.WinTextBackgroundColor,
                        IconHistory = iconHistory,
                        Banner = banner,
                        Background = background,
                        IconPlay = iconPlay,
                        IconRank = iconRank,
                        NeedleImage = needleImage,
                        RotationBackground = rotationBackground,
                        RotationImage = rotationImage
                    });
                }
                _dbContext.SaveChanges();
            }
            transaction.Commit();
            return _mapper.Map<LoyLuckyProgramDto>(luckyProgram);
        }

        /// <summary>
        /// Xem danh sách chương trình
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public PagingResult<ViewLoyLuckyProgramDto> FindAllLuckyProgram(FilterLoyLuckyProgramDto input)
        {
            int tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var result = from luckyProgram in _dbContext.LoyLuckyPrograms.Include(l => l.LoyLuckyScenarios)
                         let giftQuantity = _dbContext.LoyLuckyScenarioDetails.Where(sd => _dbContext.LoyLuckyScenarios.Any(s => s.Id == sd.LuckyScenarioId && s.LuckyProgramId == luckyProgram.Id && s.Deleted == YesNo.NO)
                                             && sd.Deleted == YesNo.NO && sd.VoucherId != null).Sum(x => x.Quantity) ?? 0
                         // Số lượng quà đã trúng
                         let giftWinQuantity = _dbContext.LoyLuckyProgramInvestorDetails.Where(id => _dbContext.LoyLuckyProgramInvestors.Any(pi => pi.Id == id.LuckyProgramInvestorId && pi.LuckyProgramId == luckyProgram.Id && pi.Deleted == YesNo.NO)
                                             && _dbContext.LoyLuckyScenarioDetails.Any(sd => sd.Id == id.LuckyScenarioDetailId && sd.Deleted == YesNo.NO)
                                             && id.Deleted == YesNo.NO && id.VoucherId != null).Count()
                         where luckyProgram.Deleted == YesNo.NO && luckyProgram.TradingProviderId == tradingProviderId
                            && (input.Status == null || (input.Status == luckyProgram.Status && luckyProgram.EndDate > DateTime.Now))
                            && (input.IsExpried == null || input.IsExpried.Value && luckyProgram.EndDate < DateTime.Now)
                            && (input.Keyword == null || input.Keyword.Contains(luckyProgram.Name) || input.Keyword.Contains(luckyProgram.Code))
                            && (input.StartDate == null || input.StartDate.Value.Date == luckyProgram.StartDate.Date)
                            && (input.EndDate == null || input.EndDate.Value.Date == luckyProgram.EndDate.Date)
                         select new ViewLoyLuckyProgramDto
                         {
                             Id = luckyProgram.Id,
                             Code = luckyProgram.Code,
                             Name = luckyProgram.Name,
                             StartDate = luckyProgram.StartDate,
                             EndDate = luckyProgram.EndDate,
                             CreatedBy = luckyProgram.CreatedBy,
                             Status = luckyProgram.Status,
                             IsExpired = luckyProgram.EndDate < DateTime.Now,
                             NumberLuckyScenario = luckyProgram.LoyLuckyScenarios.Where(x => x.Deleted == YesNo.NO).Count(),
                             GiftQuantity = giftQuantity,
                             GiftQuantityRemain = giftQuantity - giftWinQuantity
                         };

            result = result.OrderByDescending(x => x.Id);
            int totalItem = result.Count();

            return new PagingResult<ViewLoyLuckyProgramDto>
            {
                TotalItems = totalItem,
                Items = result.Skip(input.Skip).Take(input.PageSize)
            };
        }

        /// <summary>
        /// Xem chi tiết
        /// </summary>
        /// <param name="luckyProgramId"></param>
        /// <returns></returns>
        public LoyLuckyProgramDto FindById(int luckyProgramId)
        {
            int tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var luckyProgram = _dbContext.LoyLuckyPrograms.FirstOrDefault(l => l.Id == luckyProgramId && l.TradingProviderId == tradingProviderId && l.Deleted == YesNo.NO)
                .ThrowIfNull(_dbContext, ErrorCode.LoyLuckyProgramNotFound);
            var result = _mapper.Map<LoyLuckyProgramDto>(luckyProgram);

            var historyUpdate = _dbContext.LoyHistoryUpdates.Where(h => h.RealTableId == luckyProgramId && h.UpdateTable == LoyHistoryUpdateTables.LOY_LUCKY_PROGRAM);
            result.HistoryUpdates = _mapper.Map<List<LoyHistoryUpdateDto>>(historyUpdate);
            return result;
        }

        /// <summary>
        /// Xóa chương trình
        /// </summary>
        /// <param name="luckyProgramId"></param>
        public void Delete(int luckyProgramId)
        {
            int tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            var luckyProgram = _dbContext.LoyLuckyPrograms.FirstOrDefault(l => l.Id == luckyProgramId && l.TradingProviderId == tradingProviderId && l.Deleted == YesNo.NO)
                .ThrowIfNull(_dbContext, ErrorCode.LoyLuckyProgramNotFound);
            luckyProgram.Deleted = YesNo.YES;
            luckyProgram.ModifiedBy = username;
            luckyProgram.ModifiedDate = DateTime.Now;
            _dbContext.SaveChanges();
        }

        /// <summary>
        /// Cập nhật chương trình
        /// </summary>
        /// <param name="input"></param>
        public void Update(UpdateLoyLuckyProgramDto input)
        {
            int tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            var luckyProgram = _dbContext.LoyLuckyPrograms.FirstOrDefault(l => l.Id == input.Id && l.TradingProviderId == tradingProviderId && l.Deleted == YesNo.NO)
                .ThrowIfNull(_dbContext, ErrorCode.LoyLuckyProgramNotFound);
            if (luckyProgram.Name != input.Name)
            {
                _loyHistoryUpdateEFRepository.Add(new LoyHistoryUpdate(luckyProgram.Id, luckyProgram.Name, input.Name, LoyFieldName.UPDATE_LOY_LUCKY_PROGRAM_NAME, LoyHistoryUpdateTables.LOY_LUCKY_PROGRAM,
                    ActionTypes.CAP_NHAT, "Tiêu đề chương trình"), username);
            }
            if (luckyProgram.NumberOfTurn != input.NumberOfTurn)
            {
                _loyHistoryUpdateEFRepository.Add(new LoyHistoryUpdate(luckyProgram.Id, luckyProgram.NumberOfTurn?.ToString(), input.NumberOfTurn?.ToString(), LoyFieldName.UPDATE_LOY_LUCKY_PROGRAM_NUMBER_OF_TURN,
                    LoyHistoryUpdateTables.LOY_LUCKY_PROGRAM, ActionTypes.CAP_NHAT, "Lượt tham gia - Vòng quay may mắn"), username);
            }
            if (input.AvatarImageUrl != null)
            {
                _fileServices.DeleteFile(luckyProgram.AvatarImageUrl);
                luckyProgram.AvatarImageUrl = _fileServices.UploadImage(new UploadFileModel
                {
                    File = input.AvatarImageUrl,
                    Folder = FileFolder.Loyalty,
                });
            }
            luckyProgram.StartDate = input.StartDate;
            luckyProgram.EndDate = input.EndDate;
            luckyProgram.Name = input.Name;
            luckyProgram.Code = input.Code;
            luckyProgram.DescriptionContent = input.DescriptionContent;
            luckyProgram.DescriptionContentType = input.DescriptionContentType;
            luckyProgram.JoinTimeSetting = input.JoinTimeSetting;
            luckyProgram.NumberOfTurn = input.NumberOfTurn;
            luckyProgram.MaxNumberOfTurnByIp = input.MaxNumberOfTurnByIp;
            luckyProgram.StartTurnDate = input.JoinTimeSetting == LoyJoinTimeSettingTypes.THEO_MOC_THOI_GIAN ? input.StartTurnDate : null;
            luckyProgram.ResetTurnQuantity = input.ResetTurnQuantity;
            luckyProgram.ResetTurnType = input.ResetTurnType;
            _dbContext.SaveChanges();
        }

        /// <summary>
        /// Cập nhật cài đặt thời gian tham gia chương trình
        /// </summary>
        /// <param name="input"></param>
        public void UpdateSetting(UpdateLuckyLoyProgramSettingDto input)
        {
            int tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            var luckyProgram = _dbContext.LoyLuckyPrograms.FirstOrDefault(l => l.Id == input.Id && l.TradingProviderId == tradingProviderId && l.Deleted == YesNo.NO)
                .ThrowIfNull(_dbContext, ErrorCode.LoyLuckyProgramNotFound);
            luckyProgram.JoinTimeSetting = input.JoinTimeSetting;
            luckyProgram.NumberOfTurn = input.NumberOfTurn;
            luckyProgram.MaxNumberOfTurnByIp = input.MaxNumberOfTurnByIp;
            luckyProgram.StartTurnDate = input.JoinTimeSetting == LoyJoinTimeSettingTypes.THEO_MOC_THOI_GIAN ? input.StartTurnDate : null;
            luckyProgram.ResetTurnQuantity = input.ResetTurnQuantity;
            luckyProgram.ResetTurnType = input.ResetTurnType;
            _dbContext.SaveChanges();
        }
        /// <summary>
        /// Cập nhật trạng thái
        /// </summary>
        /// <param name="luckyProgramId"></param>
        public void ChangeStatus(int luckyProgramId)
        {
            int tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            var luckyProgram = _dbContext.LoyLuckyPrograms.FirstOrDefault(l => l.Id == luckyProgramId && l.TradingProviderId == tradingProviderId && l.Deleted == YesNo.NO)
                .ThrowIfNull(_dbContext, ErrorCode.LoyLuckyProgramNotFound);
            luckyProgram.Status = luckyProgram.Status == LoyLuckyProgramStatus.KICH_HOAT ? LoyLuckyProgramStatus.TAM_DUNG : LoyLuckyProgramStatus.KICH_HOAT;
            luckyProgram.ModifiedBy = username;
            luckyProgram.ModifiedDate = DateTime.Now;
            _dbContext.SaveChanges();
        }

        /// <summary>
        /// Xem danh sách lịch sử tham gia
        /// </summary>
        public PagingResult<LoyLuckyProgramHistoryDto> LuckyProgramHistory(FilterLoyLuckyProgramHistoryDto input)
        {
            int tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var resultQuery = from luckyProgramInvestorDetail in _dbContext.LoyLuckyProgramInvestorDetails
                              join luckyScenarioDetail in _dbContext.LoyLuckyScenarioDetails on luckyProgramInvestorDetail.LuckyScenarioDetailId equals luckyScenarioDetail.Id
                              join luckyProgramInvestor in _dbContext.LoyLuckyProgramInvestors on luckyProgramInvestorDetail.LuckyProgramInvestorId equals luckyProgramInvestor.Id
                              join investor in _dbContext.Investors on luckyProgramInvestor.InvestorId equals investor.InvestorId
                              join cifcode in _dbContext.CifCodes on investor.InvestorId equals cifcode.InvestorId
                              from identification in _dbContext.InvestorIdentifications.Where(i => i.InvestorId == investor.InvestorId && i.Status == Status.ACTIVE && i.Deleted == YesNo.NO)
                                .OrderByDescending(c => c.IsDefault).ThenByDescending(c => c.Id).Take(1).DefaultIfEmpty()
                              join luckyProgram in _dbContext.LoyLuckyPrograms on luckyProgramInvestor.LuckyProgramId equals luckyProgram.Id
                              where luckyProgramInvestorDetail.Deleted == YesNo.NO && luckyProgramInvestor.Deleted == YesNo.NO && luckyProgram.Deleted == YesNo.NO && luckyScenarioDetail.Deleted == YesNo.NO
                              && luckyProgram.TradingProviderId == tradingProviderId
                              && (input.IsPrize == null || (input.IsPrize.Value && luckyProgramInvestorDetail.VoucherId != null) || (!input.IsPrize.Value && luckyProgramInvestorDetail.VoucherId == null))
                              && (input.CreatedDate == null || input.CreatedDate.Value.Date == luckyProgramInvestorDetail.CreatedDate.Value.Date)
                              && (input.LuckyProgramId == null || input.LuckyProgramId == luckyProgram.Id)
                              select new LoyLuckyProgramHistoryDto
                              {
                                  Id = luckyProgramInvestorDetail.Id,
                                  CreatedDate = luckyProgramInvestorDetail.CreatedDate,
                                  LuckyProgramName = luckyProgram.Name,
                                  CifCode = cifcode.CifCode,
                                  CustomerName = identification.Fullname,
                                  Phone = investor.Phone,
                                  IsPrize = luckyProgramInvestorDetail.VoucherId != null,
                                  GiftName = luckyProgramInvestorDetail.VoucherId == null ? luckyScenarioDetail.Name
                                                 : _dbContext.LoyVouchers.FirstOrDefault(v => v.Id == luckyProgramInvestorDetail.VoucherId && v.Deleted == YesNo.NO).Name
                              };
            resultQuery = resultQuery.OrderByDescending(x => x.Id);
            int totalItems = resultQuery.Count();
            resultQuery = resultQuery.Skip(input.Skip).Take(input.PageSize);

            var result = resultQuery.ToList();
            foreach (var item in result)
            {
                item.Phone = StringUtils.HidePhone(item.Phone);
            }
            return new PagingResult<LoyLuckyProgramHistoryDto>
            {
                TotalItems = totalItems,
                Items = result
            };
        }

        /// <summary>
        /// Xem danh sách lịch sử trúng thưởng
        /// </summary>
        public PagingResult<LoyLuckyProgramPrizeHistoryDto> LuckyProgramPrizeHistory(FilterLoyLuckyProgramPrizeHistoryDto input)
        {
            int tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var resultQuery = from luckyProgramInvestorDetail in _dbContext.LoyLuckyProgramInvestorDetails
                              join luckyScenarioDetail in _dbContext.LoyLuckyScenarioDetails on luckyProgramInvestorDetail.LuckyScenarioDetailId equals luckyScenarioDetail.Id
                              join luckyProgramInvestor in _dbContext.LoyLuckyProgramInvestors on luckyProgramInvestorDetail.LuckyProgramInvestorId equals luckyProgramInvestor.Id
                              join investor in _dbContext.Investors on luckyProgramInvestor.InvestorId equals investor.InvestorId
                              join cifcode in _dbContext.CifCodes on investor.InvestorId equals cifcode.InvestorId
                              from identification in _dbContext.InvestorIdentifications.Where(i => i.InvestorId == investor.InvestorId && i.Status == Status.ACTIVE && i.Deleted == YesNo.NO)
                                .OrderByDescending(c => c.IsDefault).ThenByDescending(c => c.Id).Take(1).DefaultIfEmpty()
                              join luckyProgram in _dbContext.LoyLuckyPrograms on luckyProgramInvestor.LuckyProgramId equals luckyProgram.Id
                              from point in _dbContext.LoyPointInvestors.Where(x => x.InvestorId == investor.InvestorId && tradingProviderId == x.TradingProviderId && x.Deleted == YesNo.NO).DefaultIfEmpty()
                              from rank in _dbContext.LoyRanks.Where(x => x.PointStart <= point.TotalPoint && point.TotalPoint <= x.PointEnd && tradingProviderId == x.TradingProviderId && x.Deleted == YesNo.NO && x.Status == LoyRankStatus.ACTIVE).DefaultIfEmpty()
                              where luckyProgramInvestorDetail.Deleted == YesNo.NO && luckyProgramInvestor.Deleted == YesNo.NO && luckyProgram.Deleted == YesNo.NO && luckyScenarioDetail.Deleted == YesNo.NO
                              && luckyProgram.TradingProviderId == tradingProviderId && luckyProgramInvestorDetail.VoucherId != null
                              && (input.RankId == null || input.RankId == rank.Id)
                              && (input.CreatedDate == null || input.CreatedDate.Value.Date == luckyProgramInvestorDetail.CreatedDate.Value.Date)
                              && (input.LuckyProgramId == null || input.LuckyProgramId == luckyProgram.Id)
                              select new LoyLuckyProgramPrizeHistoryDto
                              {
                                  Id = luckyProgramInvestorDetail.Id,
                                  CreatedDate = luckyProgramInvestorDetail.CreatedDate,
                                  LuckyProgramName = luckyProgram.Name,
                                  CifCode = cifcode.CifCode,
                                  CustomerName = identification.Fullname,
                                  Phone = investor.Phone,
                                  RankName = rank.Name,
                                  GiftName = luckyProgramInvestorDetail.VoucherId == null ? luckyScenarioDetail.Name
                                                 : _dbContext.LoyVouchers.FirstOrDefault(v => v.Id == luckyProgramInvestorDetail.VoucherId && v.Deleted == YesNo.NO).Name
                              };
            resultQuery = resultQuery.OrderByDescending(x => x.Id);
            int totalItems = resultQuery.Count();
            resultQuery = resultQuery.Skip(input.Skip).Take(input.PageSize);

            var result = resultQuery.ToList();
            foreach (var item in result)
            {
                item.Phone = StringUtils.HidePhone(item.Phone);
            }
            return new PagingResult<LoyLuckyProgramPrizeHistoryDto>
            {
                TotalItems = totalItems,
                Items = result
            };
        }

        #region Danh sách khách hàng tham gia
        /// <summary>
        /// Danh sách nhà đầu tư của đại lý
        /// </summary>
        public PagingResult<LoyInvestorOfTradingDto> GetAllInvestorByTrading(FilterLoyInvestorOfTradingDto input)
        {
            int tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            _logger.LogInformation($"{nameof(GetAllInvestorByTrading)}: input = {JsonSerializer.Serialize(input)}, tradingProviderId = {tradingProviderId}");
            var query = (from investor in _dbContext.Investors
                         let isSelected = _dbContext.LoyLuckyProgramInvestors.Any(i => input.LuckyProgramId != null && i.LuckyProgramId == input.LuckyProgramId && i.InvestorId == investor.InvestorId && i.Deleted == YesNo.NO)
                         join cifcode in _dbContext.CifCodes on investor.InvestorId equals cifcode.InvestorId
                         let orderInvest = _dbContext.InvOrders.Any(o => o.CifCode == cifcode.CifCode && o.TradingProviderId == tradingProviderId && o.Deleted == YesNo.NO)
                         let orderGarner = _dbContext.GarnerOrders.Any(o => o.CifCode == cifcode.CifCode && o.TradingProviderId == tradingProviderId && o.Deleted == YesNo.NO)
                         let orderRealEstate = _dbContext.RstOrders.Any(o => o.CifCode == cifcode.CifCode && o.TradingProviderId == tradingProviderId && o.Deleted == YesNo.NO)
                         from identification in _dbContext.InvestorIdentifications.Where(i => i.InvestorId == investor.InvestorId && i.Status == Status.ACTIVE && i.Deleted == YesNo.NO)
                                     .OrderByDescending(c => c.IsDefault).ThenByDescending(c => c.Id).Take(1).DefaultIfEmpty()
                         from point in _dbContext.LoyPointInvestors.AsNoTracking().Where(x => x.InvestorId == investor.InvestorId && tradingProviderId == x.TradingProviderId && x.Deleted == YesNo.NO).DefaultIfEmpty()
                         from rank in _dbContext.LoyRanks.AsNoTracking().Where(x => x.PointStart <= point.TotalPoint && point.TotalPoint <= x.PointEnd && tradingProviderId == x.TradingProviderId && x.Deleted == YesNo.NO && x.Status == LoyRankStatus.ACTIVE).DefaultIfEmpty()
                         from user in _dbContext.Users.Where(x => x.InvestorId == investor.InvestorId && x.UserType == UserTypes.INVESTOR && x.IsDeleted == YesNo.NO).DefaultIfEmpty()
                         where investor.Deleted == YesNo.NO && (string.IsNullOrEmpty(input.Sex) || input.Sex == identification.Sex)
                             && (string.IsNullOrEmpty(input.Keyword) || identification.Fullname.ToLower().Contains(input.Keyword.ToLower()) || investor.Phone.Contains(input.Keyword) || investor.Email.ToLower().Contains(input.Keyword.ToLower()) || cifcode.CifCode.ToLower().Contains(input.Keyword.ToLower()))
                             && (input.RankId == null || input.RankId == rank.Id)
                             && (input.IsSelected == null || input.IsSelected.Value == isSelected)
                             // Kiểm tra khách hàng từ bảng quan hệ với Đại lý và Sale
                             && (((input.CustomerType == null || (input.CustomerType == LoyLuckyProgramCustomerType.KHACH_DA_DAU_TU && (orderInvest || orderGarner || orderGarner))
                                    || (input.CustomerType == LoyLuckyProgramCustomerType.KHACH_CHUA_DAU_TU && (!orderInvest && !orderGarner && !orderGarner)))
                                && ((new string[] { InvestorStatus.LOCKED, InvestorStatus.ACTIVE, InvestorStatus.DEACTIVE }.Contains(investor.Status)) && UserStatus.ACTIVE == user.Status
                                        && ((_dbContext.InvestorTradingProviders.Any(investorTrading => investorTrading.Deleted == YesNo.NO
                                        && tradingProviderId == investorTrading.TradingProviderId && investorTrading.InvestorId == investor.InvestorId)
                                        || (from eis in _dbContext.InvestorSales
                                            join esctp in _dbContext.SaleTradingProviders on eis.SaleId equals esctp.SaleId
                                            where eis.InvestorId == investor.InvestorId && esctp.Deleted == YesNo.NO && eis.Deleted == YesNo.NO && tradingProviderId == esctp.TradingProviderId
                                            select eis.InvestorId).Any()))
                                    // Khách hàng chưa xác minh có nhập mã giới thiệu sale thuộc đại lý
                                    || ((investor.Step == null || investor.Step == 1 || investor.Step == 2) && user != null && user.Status == UserStatus.TEMP
                                        // Nhập mã giới thiệu sale là Investor
                                        && ((from inv in _dbContext.Investors.Where(x => x.ReferralCodeSelf == investor.ReferralCode && x.Deleted == YesNo.NO && x.Status == InvestorStatus.ACTIVE).DefaultIfEmpty()
                                             from saleInv in _dbContext.Sales.Where(x => x.InvestorId == inv.InvestorId && x.Deleted == YesNo.NO).DefaultIfEmpty()
                                             from saleTradingInv in _dbContext.SaleTradingProviders.Where(x => x.SaleId == saleInv.SaleId && x.Deleted == YesNo.NO).DefaultIfEmpty()
                                             where tradingProviderId == saleTradingInv.TradingProviderId
                                             select saleTradingInv.TradingProviderId).Any()
                                    // Nhập mã giới thiệu sale là BussinessCustomer
                                    || (from bus in _dbContext.BusinessCustomers.Where(x => x.ReferralCodeSelf == investor.ReferralCode && x.Deleted == YesNo.NO).DefaultIfEmpty()
                                        from saleBus in _dbContext.Sales.Where(x => x.BusinessCustomerId == bus.BusinessCustomerId && x.Deleted == YesNo.NO).DefaultIfEmpty()
                                        from saleTradingBus in _dbContext.SaleTradingProviders.Where(x => x.SaleId == saleBus.SaleId && x.Deleted == YesNo.NO).DefaultIfEmpty()
                                        where tradingProviderId == saleTradingBus.TradingProviderId
                                        select saleTradingBus.TradingProviderId).Any()))))
                                //Lọc tư vấn viên
                                || (input.CustomerType == LoyLuckyProgramCustomerType.TU_VAN_VIEN && _dbContext.Sales.Any(s => s.InvestorId == investor.InvestorId && s.Deleted == YesNo.NO
                                    && _dbContext.SaleTradingProviders.Any(st => st.SaleId == s.SaleId && st.TradingProviderId == tradingProviderId && st.Status == Status.ACTIVE && st.Deleted == YesNo.NO)))
                               )
                         orderby investor.InvestorId descending
                         select new LoyInvestorOfTradingDto
                         {
                             Email = investor.Email,
                             Phone = investor.Phone,
                             InvestorId = investor.InvestorId,
                             CifCode = cifcode.CifCode,
                             RankId = rank.Id,
                             RankName = rank.Name,
                             UserId = user == null ? 0 : user.UserId,
                             Fullname = identification.Fullname,
                             Sex = identification.Sex,
                             Username = user.UserName,
                             IsSelected = isSelected
                         });

            query = query.OrderByDescending(x => x.IsSelected).ThenByDescending(x => x.InvestorId);
            var totalItems = query.Count();
            if (input.PageSize != -1)
            {
                query = query.Skip(input.Skip).Take(input.PageSize);
            }

            return new PagingResult<LoyInvestorOfTradingDto>
            {
                TotalItems = totalItems,
                Items = query
            };
        }

        /// <summary>
        /// Thêm khách hàng tham gia
        /// </summary>
        /// <param name="input"></param>
        public void AddLuckyProgramInvestor(CreateLuckyProgramInvestorDto input)
        {
            int tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var username = CommonUtils.GetCurrentUsername(_httpContext);

            if (!_dbContext.LoyLuckyPrograms.Any(i => i.Id == input.LuckyProgramId && i.TradingProviderId == tradingProviderId && i.Deleted == YesNo.NO))
            {
                _loyLuckyProgramRepository.ThrowException(ErrorCode.LoyLuckyProgramNotFound);
            }

            foreach (var item in input.InvestorIds)
            {
                if (_dbContext.LoyLuckyProgramInvestors.Any(i => i.InvestorId == item && i.LuckyProgramId == input.LuckyProgramId && i.Deleted == YesNo.NO))
                {
                    continue;
                }
                _dbContext.LoyLuckyProgramInvestors.Add(new LoyLuckyProgramInvestor
                {
                    Id = (int)_loyLuckyProgramRepository.NextKey($"{DbSchemas.EPIC_LOYALTY}.{LoyLuckyProgramInvestor.SEQ}"),
                    TradingProviderId = tradingProviderId,
                    LuckyProgramId = input.LuckyProgramId,
                    InvestorId = item,
                    CreatedBy = username,
                });
            }
            _dbContext.SaveChanges();
        }

        /// <summary>
        /// Xóa khách hàng tham gia/ Khi khách hàng chưa tham gia 
        /// </summary>
        /// <param name="luckyProgramInvestorId"></param>
        public void DeleteLuckyProgramInvestor(int luckyProgramInvestorId)
        {
            int tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var luckyProgramInvestor = _dbContext.LoyLuckyProgramInvestors.FirstOrDefault(i => i.Id == luckyProgramInvestorId && i.TradingProviderId == tradingProviderId && i.Deleted == YesNo.NO)
                                        .ThrowIfNull(_dbContext, ErrorCode.LoyLuckyProgramInvestorNotFound);
            if (_dbContext.LoyLuckyProgramInvestorDetails.Any(i => i.LuckyProgramInvestorId == luckyProgramInvestor.Id && i.Deleted == YesNo.NO))
            {
                _loyLuckyProgramRepository.ThrowException(ErrorCode.LoyLuckyProgramInvestorCanNotDelCuzIsJoin);
            }
            luckyProgramInvestor.Deleted = YesNo.YES;
            _dbContext.SaveChanges();
        }

        /// <summary>
        /// Xem danh sách khách hàng tham gia của chương trình
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public PagingResult<LoyLuckyProgramInvestorDto> FindAllLuckyProgramInvestor(FilterLoyLuckyProgramInvestorDto input)
        {
            int tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var result = (from luckyProgramInvestor in _dbContext.LoyLuckyProgramInvestors
                          let isJoin = _dbContext.LoyLuckyProgramInvestorDetails.Any(i => i.LuckyProgramInvestorId == luckyProgramInvestor.Id && i.Deleted == YesNo.NO)
                          join investor in _dbContext.Investors on luckyProgramInvestor.InvestorId equals investor.InvestorId
                          join cifcode in _dbContext.CifCodes on investor.InvestorId equals cifcode.InvestorId
                          from identification in _dbContext.InvestorIdentifications.Where(i => i.InvestorId == investor.InvestorId && i.Status == Status.ACTIVE && i.Deleted == YesNo.NO)
                            .OrderByDescending(c => c.IsDefault).ThenByDescending(c => c.Id).Take(1).DefaultIfEmpty()
                          from point in _dbContext.LoyPointInvestors.AsNoTracking().Where(x => x.InvestorId == investor.InvestorId && tradingProviderId == x.TradingProviderId && x.Deleted == YesNo.NO).DefaultIfEmpty()
                          from rank in _dbContext.LoyRanks.AsNoTracking().Where(x => x.PointStart <= point.TotalPoint && point.TotalPoint <= x.PointEnd && tradingProviderId == x.TradingProviderId && x.Deleted == YesNo.NO && x.Status == LoyRankStatus.ACTIVE).DefaultIfEmpty()
                          where investor.Deleted == YesNo.NO && luckyProgramInvestor.Deleted == YesNo.NO
                          && luckyProgramInvestor.TradingProviderId == tradingProviderId && luckyProgramInvestor.LuckyProgramId == input.LuckyProgramId
                          && (input.IsJoin == null || input.IsJoin.Value == isJoin)
                          && (string.IsNullOrEmpty(input.Keyword) || identification.Fullname.ToLower().Contains(input.Keyword.ToLower()) || cifcode.CifCode.Contains(input.Keyword))
                          orderby investor.InvestorId descending
                          select new LoyLuckyProgramInvestorDto
                          {
                              Id = luckyProgramInvestor.Id,
                              Phone = investor.Phone,
                              InvestorId = investor.InvestorId,
                              CifCode = cifcode.CifCode,
                              Fullname = identification.Fullname,
                              IdNo = identification.IdNo,
                              RankName = rank.Name,
                              TotalPoint = point.TotalPoint ?? 0,
                              // Số lượng Voucher đếm trong quy đổi voucher với trạng thái Hoàn thành
                              VoucherNum = _dbContext.LoyConversionPointDetails.Where(d => d.Deleted == YesNo.NO && _dbContext.LoyConversionPoints.Any(c => tradingProviderId == c.TradingProviderId && c.Status == LoyConversionPointStatus.FINISHED
                                               && d.ConversionPointId == c.Id && c.Deleted == YesNo.NO && c.InvestorId == investor.InvestorId)).Sum(d => d.Quantity)
                                           + _dbContext.LoyLuckyProgramInvestorDetails.Count(x => x.LuckyProgramInvestorId == luckyProgramInvestor.Id && x.VoucherId != null && x.Deleted == YesNo.NO),
                              IsJoin = isJoin,
                          });
            return new PagingResult<LoyLuckyProgramInvestorDto>
            {
                TotalItems = result.Count(),
                Items = result.Skip(input.Skip).Take(input.PageSize)
            };
        }
        #endregion

        #region App
        /// <summary>
        /// APP - Random giải thưởng cho nhà đầu tư
        /// </summary>
        /// <param name="luckyScenarioId"></param>
        public AppLoyRandomPrize AppRandomPrizeByInvestor(int luckyScenarioId)
        {
            int investorId = CommonUtils.GetCurrentInvestorId(_httpContext);
            var username = CommonUtils.GetCurrentUsername(_httpContext);

            var luckyScenario = _dbContext.LoyLuckyScenarios.FirstOrDefault(i => i.Id == luckyScenarioId && i.Status == Status.ACTIVE && i.Deleted == YesNo.NO)
                .ThrowIfNull(_dbContext, ErrorCode.LoyLuckyScenarioNotFound);

            var luckyProgramInvestor = _dbContext.LoyLuckyProgramInvestors.FirstOrDefault(i => i.InvestorId == investorId && i.LuckyProgramId == luckyScenario.LuckyProgramId && i.Deleted == YesNo.NO)
                .ThrowIfNull(_dbContext, ErrorCode.LoyLuckyProgramInvestorNotFound);

            var luckyProgram = _dbContext.LoyLuckyPrograms.FirstOrDefault(i => i.Id == luckyScenario.LuckyProgramId && i.Deleted == YesNo.NO)
                .ThrowIfNull(_dbContext, ErrorCode.LoyLuckyProgramNotFound);

            // Kiểm tra số lượt tham gia của nhà đầu tư
            if (luckyProgram.NumberOfTurn != null && luckyProgram.NumberOfTurn <= _dbContext.LoyLuckyProgramInvestorDetails.Where(i => i.LuckyProgramInvestorId == luckyProgramInvestor.Id && i.Deleted == YesNo.NO).Count())
            {
                // Đã hết lượt quay
                _loyLuckyProgramRepository.ThrowException(ErrorCode.LoyLuckyProgramInvestorExceedNumberOfTurn);
            }

            if (luckyProgram.JoinTimeSetting == LoyJoinTimeSettingTypes.THEO_MOC_THOI_GIAN)
            {
                // Nếu chưa đến thời gian bắt đầu lượt quay
                if (luckyProgram.StartTurnDate != null && luckyProgram.StartTurnDate > DateTime.Now)
                {
                    // Chưa đến thời gian bắt đầu quay
                    _loyLuckyProgramRepository.ThrowException(ErrorCode.LoyLuckyProgramInvestorNotYetStartTurn);
                }
                if (luckyProgram.StartTurnDate != null && luckyProgram.ResetTurnType != null && luckyProgram.ResetTurnQuantity != null
                    && DateTime.Now > TimeTypes.CalculatorEndDate(luckyProgram.StartTurnDate.Value, luckyProgram.ResetTurnType, luckyProgram.ResetTurnQuantity.Value))
                {
                    _loyLuckyProgramRepository.ThrowException(ErrorCode.LoyLuckyProgramInvestorTimeUpTurn);
                }
            }
            else if (luckyProgram.JoinTimeSetting == LoyJoinTimeSettingTypes.THEO_MOC_THOI_GIAN)
            {
                // Tìm lượt quay lần đầu của chương trình
                var firstTurnDate = _dbContext.LoyLuckyProgramInvestorDetails.Where(i => i.LuckyProgramInvestorId == luckyProgramInvestor.Id && i.Deleted == YesNo.NO).OrderBy(x => x.Id).FirstOrDefault();

                // Kiểm tra lượt reset lượt
                if (firstTurnDate != null && firstTurnDate.CreatedDate != null && luckyProgram.ResetTurnType != null && luckyProgram.ResetTurnQuantity != null
                    && DateTime.Now > TimeTypes.CalculatorEndDate(luckyProgram.StartTurnDate.Value, luckyProgram.ResetTurnType, luckyProgram.ResetTurnQuantity.Value))
                {
                    _loyLuckyProgramRepository.ThrowException(ErrorCode.LoyLuckyProgramInvestorTimeUpTurn);
                }
            }

            var sumProbability = _dbContext.LoyLuckyScenarioDetails.Where(i => i.LuckyScenarioId == luckyScenario.Id && i.Deleted == YesNo.NO).Sum(x => x.Probability ?? 0);
            if (sumProbability > 100)
            {
                _loyLuckyProgramRepository.ThrowException(ErrorCode.LoyLuckyScenarioDetailSumProbabilityInvaid);
            }

            // Define the list of probabilities and corresponding outcomes
            var luckyScenarioDetails = (from luckyScenarioDetail in _dbContext.LoyLuckyScenarioDetails
                                        let count = _dbContext.LoyLuckyScenarioDetails.Where(i => i.LuckyScenarioId == luckyScenario.Id && i.VoucherId == null && i.Deleted == YesNo.NO).Count()
                                        where luckyScenarioDetail.LuckyScenarioId == luckyScenario.Id && luckyScenarioDetail.Deleted == YesNo.NO
                                        orderby luckyScenarioDetail.SortOrder ascending
                                        select new
                                        {
                                            luckyScenarioDetail.Id,
                                            luckyScenarioDetail.VoucherId,
                                            Name = luckyScenarioDetail.VoucherId != null ? _dbContext.LoyVouchers.FirstOrDefault(v => v.Id == luckyScenarioDetail.VoucherId && v.Deleted == YesNo.NO).Name
                                                     : luckyScenarioDetail.Name,
                                            LuckyScenarioId = luckyScenarioDetail.LuckyScenarioId,
                                            Probability = luckyScenarioDetail.Probability != null ? luckyScenarioDetail.Probability / 100
                                                           : (count > 0) ? ((100 - sumProbability) / count) / 100 : 0,
                                        }).ToList();

            // Create a new instance of the Random class
            Random random = new Random();

            // Generate a random number between 0 and 1 (exclusive)
            double randomNumber = random.NextDouble();

            // Calculate the cumulative probabilities
            List<double> cumulativeProbabilities = new List<double>();
            double cumulativeProbability = 0;
            foreach (var probability in luckyScenarioDetails)
            {
                cumulativeProbability += probability.Probability.Value;
                cumulativeProbabilities.Add(cumulativeProbability);
            }

            // Find the first index where the random number is less than the cumulative probability
            int selectedOutcomeIndex = cumulativeProbabilities.FindIndex(p => randomNumber < p);

            // Get the corresponding outcome based on the index
            var selectedPrize = luckyScenarioDetails[selectedOutcomeIndex];

            _dbContext.LoyLuckyProgramInvestorDetails.Add(new LoyLuckyProgramInvestorDetail
            {
                Id = (int)_loyLuckyProgramRepository.NextKey($"{DbSchemas.EPIC_LOYALTY}.{LoyLuckyProgramInvestorDetail.SEQ}"),
                TradingProviderId = luckyProgramInvestor.TradingProviderId,
                LuckyScenarioDetailId = selectedPrize.Id,
                LuckyScenarioId = selectedPrize.LuckyScenarioId,
                LuckyProgramInvestorId = luckyProgramInvestor.Id,
                VoucherId = selectedPrize.VoucherId,
                CreatedBy = username,
            });
            _dbContext.SaveChanges();
            return new AppLoyRandomPrize
            {
                Id = selectedPrize.Id,
                Name = selectedPrize.Name,
            };
        }

        /// <summary>
        /// APP - Danh sách chương trình được tham gia của nhà đầu tư theo Đại lý
        /// </summary>
        public IEnumerable<AppLoyLuckyProgramByInvestorDto> AppGetAllLuckyProgram(int tradingProviderId)
        {
            int investorId = CommonUtils.GetCurrentInvestorId(_httpContext);
            var result = from luckyProgramInvestor in _dbContext.LoyLuckyProgramInvestors
                         join luckyProgram in _dbContext.LoyLuckyPrograms on luckyProgramInvestor.LuckyProgramId equals luckyProgram.Id
                         where luckyProgramInvestor.Deleted == YesNo.NO && luckyProgram.Deleted == YesNo.NO
                         && _dbContext.LoyLuckyScenarios.Any(l => l.LuckyProgramId == luckyProgram.Id && l.Status == Status.ACTIVE && l.Deleted == YesNo.NO)
                         && luckyProgram.TradingProviderId == tradingProviderId && luckyProgram.Status == LoyLuckyProgramStatus.KICH_HOAT
                         && luckyProgramInvestor.InvestorId == investorId
                         select new AppLoyLuckyProgramByInvestorDto
                         {
                             Id = luckyProgram.Id,
                             Name = luckyProgram.Name,
                             Code = luckyProgram.Code,
                             AvatarImageUrl = luckyProgram.AvatarImageUrl,
                             StartDate = luckyProgram.StartDate,
                             EndDate = luckyProgram.EndDate,
                         };
            return result;
        }

        /// <summary>
        /// Lấy kịch bản vòng quay may mắn theo đại lý
        /// Tại 1 thời điểm chí có 1 vòng quay may mắn được hoạt động
        /// </summary>
        /// <param name="tradingProviderId"></param>
        /// <returns></returns>
        public AppLoyLuckyScenarioDto AppLoyLuckyScenarioRotationProgram(int tradingProviderId)
        {
            int investorId = CommonUtils.GetCurrentInvestorId(_httpContext);

            // Lấy chương trình có kịch bản vòng quay may mắn đang hoạt động Active
            var result = (from luckyProgram in _dbContext.LoyLuckyPrograms
                          join luckyProgramInvestor in _dbContext.LoyLuckyProgramInvestors on luckyProgram.Id equals luckyProgramInvestor.LuckyProgramId
                          from luckyProgramScenario in _dbContext.LoyLuckyScenarios.Where(x => x.LuckyProgramId == luckyProgram.Id && x.LuckyScenarioType == LoyLuckyScenarioTypes.VONG_QUAY_MAY_MAN && x.Status == Status.ACTIVE && x.Deleted == YesNo.NO).Take(1)
                          where luckyProgram.Deleted == YesNo.NO && luckyProgramInvestor.Deleted == YesNo.NO && luckyProgram.TradingProviderId == tradingProviderId
                          && luckyProgramInvestor.InvestorId == investorId && luckyProgram.StartDate <= DateTime.Now.Date && luckyProgram.EndDate > DateTime.Now
                          && luckyProgram.Status == LoyLuckyProgramStatus.KICH_HOAT
                          select new AppLoyLuckyScenarioDto
                          {
                              Id = luckyProgram.Id,
                              LuckyScenarioId = luckyProgramScenario.Id,
                              Name = luckyProgram.Name,
                              Code = luckyProgram.Code,
                              AvatarImageUrl = luckyProgram.AvatarImageUrl,
                              DescriptionContent = luckyProgram.DescriptionContent,
                              DescriptionContentType = luckyProgram.DescriptionContentType,
                              EndDate = luckyProgram.EndDate,
                              StartDate = luckyProgram.StartDate,
                              LuckyScenarioAvatarImageUrl = luckyProgramScenario.AvatarImageUrl,
                              PrizeQuantity = luckyProgramScenario.PrizeQuantity,
                              LuckyScenarioType = luckyProgramScenario.LuckyScenarioType,
                              LuckyScenarioName = luckyProgramScenario.Name
                          }).FirstOrDefault();
            if (result == null)
            {

            }
            // Lấy ra danh sách kịch bản
            result.LuckyScenarioDetails = _dbContext.LoyLuckyScenarioDetails.Where(x => x.LuckyScenarioId == result.LuckyScenarioId && x.Deleted == YesNo.NO)
                                          .Select(x => new LoyLuckyScenarioDetailDto
                                          {
                                              Id = x.Id,
                                              Name = x.Name,
                                              LuckyScenarioId = x.Id,
                                              Probability = x.Probability,
                                              Quantity = x.Quantity,
                                              VoucherId = x.VoucherId,
                                              VoucherName = _dbContext.LoyVouchers.FirstOrDefault(v => v.Id == x.VoucherId && v.Deleted == YesNo.NO).Name
                                          });

            // Thông tin vòng quay may mắn
            var luckyRotationInterface = _dbContext.LoyLuckyRotationInterfaces.FirstOrDefault(x => x.LuckyScenarioId == result.LuckyScenarioId);
            result.LuckyRotationInterface = _mapper.Map<LoyLuckyRotationInterfaceDto>(luckyRotationInterface);

            // Lịch sử chương trình may mắn
            result.LuckyProgramInvestorDetails = (from luckyProgramInvestor in _dbContext.LoyLuckyProgramInvestors
                                                  join luckyProgramInvestorDetail in _dbContext.LoyLuckyProgramInvestorDetails on luckyProgramInvestor.Id equals luckyProgramInvestorDetail.LuckyProgramInvestorId
                                                  join luckyScenarioDetail in _dbContext.LoyLuckyScenarioDetails on luckyProgramInvestorDetail.LuckyScenarioDetailId equals luckyScenarioDetail.Id
                                                  join voucher in _dbContext.LoyVouchers on luckyProgramInvestorDetail.VoucherId equals voucher.Id into vouchers
                                                  from voucher in vouchers.DefaultIfEmpty()
                                                  where luckyProgramInvestorDetail.Deleted == YesNo.NO
                                                  && luckyProgramInvestorDetail.LuckyScenarioId == result.LuckyScenarioId && luckyProgramInvestor.InvestorId == investorId
                                                  orderby luckyProgramInvestorDetail.Id descending
                                                  select new AppLoyLuckyProgramInvestorDetailDto
                                                  {
                                                      Id = luckyProgramInvestorDetail.Id,
                                                      CreatedDate = luckyProgramInvestorDetail.CreatedDate,
                                                      Name = luckyProgramInvestorDetail.VoucherId != null ? voucher.Name : luckyScenarioDetail.Name,
                                                      VoucherAvatar = luckyProgramInvestorDetail.VoucherId != null ? voucher.Avatar : null,

                                                  });
            return result;
        }
        #endregion
    }
}
