using AutoMapper;
using EPIC.DataAccess.Base;
using EPIC.Entities;
using EPIC.FileDomain.Services;
using EPIC.ImageAPI.Models;
using EPIC.LoyaltyDomain.Interfaces;
using EPIC.LoyaltyEntities.DataEntities;
using EPIC.LoyaltyEntities.Dto.LoyLuckyRotationInterface;
using EPIC.LoyaltyEntities.Dto.LoyLuckyScenario;
using EPIC.LoyaltyEntities.Dto.LoyLuckyScenarioDetail;
using EPIC.LoyaltyRepositories;
using EPIC.Notification.Services;
using EPIC.Utils;
using EPIC.Utils.ConfigModel;
using EPIC.Utils.ConstantVariables.Loyalty;
using EPIC.Utils.ConstantVariables.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EPIC.LoyaltyDomain.Implements
{
    public class LoyLuckyScenarioServices : ILoyLuckyScenarioServices
    {
        private readonly EpicSchemaDbContext _dbContext;
        private readonly IFileServices _fileServices;
        private readonly IMapper _mapper;
        private readonly ILogger<LoyLuckyScenarioServices> _logger;
        private readonly string _connectionString;
        private readonly IHttpContextAccessor _httpContext;
        private readonly LoyaltyNotificationServices _loyaltyNotificationServices;
        private readonly LoyLuckyScenarioEFRepository _loyLuckyScenarioEFRepository;

        public LoyLuckyScenarioServices(EpicSchemaDbContext dbContext,
            DatabaseOptions databaseOptions,
            IMapper mapper,
            IFileServices fileServices,
            ILogger<LoyLuckyScenarioServices> logger,
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
            _loyLuckyScenarioEFRepository = new LoyLuckyScenarioEFRepository(dbContext, logger);
        }

        /// <summary>
        /// Thêm mới kịch bản
        /// </summary>
        /// <param name="input"></param>
        public void AddLuckyScenario(CreateLoyLuckyScenarioDto input)
        {
            int tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            string username = CommonUtils.GetCurrentUsername(_httpContext);

            var transaction = _dbContext.Database.BeginTransaction();
            var checkLuckyProgram = _dbContext.LoyLuckyPrograms.Any(i => i.Id == input.LuckyProgramId && i.TradingProviderId == tradingProviderId && i.Deleted == YesNo.NO);
            if (!checkLuckyProgram)
            {
                _loyLuckyScenarioEFRepository.ThrowException(ErrorCode.LoyLuckyProgramNotFound);
            }

            // Mỗi chương trình chỉ có 1 loại kịch bản Vòng quay may mắn
            if (input.LuckyScenarioType == LoyLuckyScenarioTypes.VONG_QUAY_MAY_MAN && _dbContext.LoyLuckyScenarios.Any(l => l.LuckyProgramId == input.LuckyProgramId && l.Deleted == YesNo.NO
                && l.LuckyScenarioType == LoyLuckyScenarioTypes.VONG_QUAY_MAY_MAN))
            {
                _loyLuckyScenarioEFRepository.ThrowException(ErrorCode.LoyLuckyScenarioTypeLuckySpinExist);
            }

            string avatarImageUrl = null;
            if (input.AvatarImageUrl != null)
            {
                avatarImageUrl = _fileServices.UploadImage(new UploadFileModel
                {
                    File = input.AvatarImageUrl,
                    Folder = FileFolder.Loyalty,
                });
            }

            var luckyScenario = _dbContext.LoyLuckyScenarios.Add(new LoyLuckyScenario
            {
                Id = (int)_loyLuckyScenarioEFRepository.NextKey($"{DbSchemas.EPIC_LOYALTY}.{LoyLuckyScenario.SEQ}"),
                TradingProviderId = tradingProviderId,
                LuckyProgramId = input.LuckyProgramId,
                Name = input.Name,
                AvatarImageUrl = avatarImageUrl,
                LuckyScenarioType = input.LuckyScenarioType,
                Status = _dbContext.LoyLuckyScenarios.Any(x => x.Status == Status.ACTIVE && x.LuckyProgramId == input.LuckyProgramId && x.Deleted == YesNo.NO) ? Status.INACTIVE : Status.ACTIVE,
                PrizeQuantity = input.PrizeQuantity,
                CreatedBy = username
            }).Entity;
            _dbContext.SaveChanges();

            foreach (var item in input.LuckyScenarioDetails)
            {
                _dbContext.LoyLuckyScenarioDetails.Add(new LoyLuckyScenarioDetail
                {
                    Id = (int)_loyLuckyScenarioEFRepository.NextKey($"{DbSchemas.EPIC_LOYALTY}.{LoyLuckyScenarioDetail.SEQ}"),
                    LuckyScenarioId = luckyScenario.Id,
                    SortOrder = item.SortOrder,
                    VoucherId = item.VoucherId,
                    Name = item.VoucherId == null ? item.Name : null,
                    Probability = item.Probability,
                    Quantity = item.Quantity,
                    CreatedBy = username
                });
            }

            // Thêm mới vòng quay
            if (input.LuckyRotationInterface.Template != null && input.LuckyScenarioType == LoyLuckyScenarioTypes.VONG_QUAY_MAY_MAN)
            {
                AddLuckyRotationInterfaceCommon(input.LuckyRotationInterface, luckyScenario.Id);
            }
            _dbContext.SaveChanges();
            transaction.Commit();
        }

        /// <summary>
        /// Cập nhật vòng quay may mắn
        /// </summary>
        /// <param name="input"></param>
        /// <param name="luckyScenarioId"></param>
        public void UpdateLuckyRotationInterface(CreateLoyLuckyRotationInterfaceDto input, int luckyScenarioId)
        {
            int tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            string username = CommonUtils.GetCurrentUsername(_httpContext);

            var luckyScenario = _dbContext.LoyLuckyScenarios.FirstOrDefault(i => i.Id == luckyScenarioId && i.TradingProviderId == tradingProviderId && i.Deleted == YesNo.NO)
                .ThrowIfNull(_dbContext, ErrorCode.LoyLuckyScenarioNotFound);
            // Tìm kiếm xem đã có vòng quay hay chưa
            var luckyRotationInterface = _dbContext.LoyLuckyRotationInterfaces.FirstOrDefault(i => i.LuckyScenarioId == luckyScenarioId && i.Deleted == YesNo.NO);
            if (luckyRotationInterface == null)
            {
                AddLuckyRotationInterfaceCommon(input, luckyScenarioId);
            }
            else
            {
                luckyRotationInterface.Template = input.Template;
                luckyRotationInterface.ButtonColor = input.ButtonColor;
                luckyRotationInterface.ButtonHistory = input.ButtonHistory.GetValueOrDefault();
                luckyRotationInterface.ButtonPlay = input.ButtonPlay.GetValueOrDefault();
                luckyRotationInterface.ButtonRank = input.ButtonRank.GetValueOrDefault();
                luckyRotationInterface.IconHome = input.IconHome.GetValueOrDefault();
                luckyRotationInterface.ShowBanner = input.ShowBanner.GetValueOrDefault();
                luckyRotationInterface.WinText = input.WinText.GetValueOrDefault();
                luckyRotationInterface.WinTextColor = input.WinTextColor;
                luckyRotationInterface.WinTextBackgroundColor = input.WinTextBackgroundColor;
                luckyRotationInterface.ModifiedBy = username;
                luckyRotationInterface.ModifiedDate = DateTime.Now;

                if (input.Banner != null)
                {
                    _fileServices.DeleteFile(luckyRotationInterface.Banner);

                    luckyRotationInterface.Banner = _fileServices.UploadImage(new UploadFileModel
                    {
                        File = input.Banner,
                        Folder = FileFolder.Loyalty,
                    });
                }
                if (input.Background != null)
                {
                    _fileServices.DeleteFile(luckyRotationInterface.Background);
                    luckyRotationInterface.Background = _fileServices.UploadImage(new UploadFileModel
                    {
                        File = input.Background,
                        Folder = FileFolder.Loyalty,
                    });
                }
                if (input.IconHistory != null)
                {
                    _fileServices.DeleteFile(luckyRotationInterface.IconHistory);
                    luckyRotationInterface.IconHistory = _fileServices.UploadImage(new UploadFileModel
                    {
                        File = input.IconHistory,
                        Folder = FileFolder.Loyalty,
                    });
                }
                if (input.IconPlay != null)
                {
                    _fileServices.DeleteFile(luckyRotationInterface.IconPlay);
                    luckyRotationInterface.IconPlay = _fileServices.UploadImage(new UploadFileModel
                    {
                        File = input.IconPlay,
                        Folder = FileFolder.Loyalty,
                    });
                }
                if (input.IconRank != null)
                {
                    _fileServices.DeleteFile(luckyRotationInterface.IconRank);
                    luckyRotationInterface.IconRank = _fileServices.UploadImage(new UploadFileModel
                    {
                        File = input.IconRank,
                        Folder = FileFolder.Loyalty,
                    });
                }
                if (input.NeedleImage != null)
                {
                    _fileServices.DeleteFile(luckyRotationInterface.NeedleImage);
                    luckyRotationInterface.NeedleImage = _fileServices.UploadImage(new UploadFileModel
                    {
                        File = input.NeedleImage,
                        Folder = FileFolder.Loyalty,
                    });
                }
                if (input.RotationBackground != null)
                {
                    _fileServices.DeleteFile(luckyRotationInterface.RotationBackground);
                    luckyRotationInterface.RotationBackground = _fileServices.UploadImage(new UploadFileModel
                    {
                        File = input.RotationBackground,
                        Folder = FileFolder.Loyalty,
                    });
                }
                if (input.RotationImage != null)
                {
                    _fileServices.DeleteFile(luckyRotationInterface.RotationImage);
                    luckyRotationInterface.RotationImage = _fileServices.UploadImage(new UploadFileModel
                    {
                        File = input.RotationImage,
                        Folder = FileFolder.Loyalty,
                    });
                }
            }
            _dbContext.SaveChanges();
        }

        private void AddLuckyRotationInterfaceCommon(CreateLoyLuckyRotationInterfaceDto input, int luckyScenarioId)
        {
            string username = CommonUtils.GetCurrentUsername(_httpContext);

            string banner = null;
            string background = null;
            string iconHistory = null;
            string iconPlay = null;
            string iconRank = null;
            string needleImage = null;
            string ratationBackground = null;
            string ratationImage = null;
            if (input.Banner != null)
            {
                banner = _fileServices.UploadFileID(new UploadFileModel
                {
                    File = input.Banner,
                    Folder = FileFolder.Loyalty,
                });
            }
            if (input.Background != null)
            {
                background = _fileServices.UploadFileID(new UploadFileModel
                {
                    File = input.Background,
                    Folder = FileFolder.Loyalty,
                });
            }
            if (input.IconHistory != null)
            {
                iconHistory = _fileServices.UploadFileID(new UploadFileModel
                {
                    File = input.IconHistory,
                    Folder = FileFolder.Loyalty,
                });
            }
            if (input.IconPlay != null)
            {
                iconPlay = _fileServices.UploadFileID(new UploadFileModel
                {
                    File = input.IconPlay,
                    Folder = FileFolder.Loyalty,
                });
            }
            if (input.IconRank != null)
            {
                iconRank = _fileServices.UploadFileID(new UploadFileModel
                {
                    File = input.IconRank,
                    Folder = FileFolder.Loyalty,
                });
            }
            if (input.NeedleImage != null)
            {
                needleImage = _fileServices.UploadFileID(new UploadFileModel
                {
                    File = input.NeedleImage,
                    Folder = FileFolder.Loyalty,
                });
            }
            if (input.RotationBackground != null)
            {
                ratationBackground = _fileServices.UploadFileID(new UploadFileModel
                {
                    File = input.RotationBackground,
                    Folder = FileFolder.Loyalty,
                });
            }
            if (input.RotationImage != null)
            {
                ratationImage = _fileServices.UploadFileID(new UploadFileModel
                {
                    File = input.RotationImage,
                    Folder = FileFolder.Loyalty,
                });
            }
            var luckyRotationInterface = _dbContext.LoyLuckyRotationInterfaces.Add(new LoyLuckyRotationInterface
            {
                Id = (int)_loyLuckyScenarioEFRepository.NextKey($"{DbSchemas.EPIC_LOYALTY}.{LoyLuckyRotationInterface.SEQ}"),
                LuckyScenarioId = luckyScenarioId,
                CreatedBy = username,
                Template = input.Template,
                ButtonColor = input.ButtonColor,
                ButtonHistory = input.ButtonHistory.GetValueOrDefault(),
                ButtonPlay = input.ButtonPlay.GetValueOrDefault(),
                ButtonRank = input.ButtonRank.GetValueOrDefault(),
                IconHome = input.IconHome.GetValueOrDefault(),
                ShowBanner = input.ShowBanner.GetValueOrDefault(),
                WinText = input.WinText.GetValueOrDefault(),
                WinTextColor = input.WinTextColor,
                WinTextBackgroundColor = input.WinTextBackgroundColor,
                IconHistory = iconHistory,
                Banner = banner,
                Background = background,
                IconPlay = iconPlay,
                IconRank = iconRank,
                NeedleImage = needleImage,
                RotationBackground = ratationBackground,
                RotationImage = ratationImage
            });
        }

        /// <summary>
        /// Cùng 1 thời điểm chi có 1 kịch bản được active
        /// </summary>
        /// <param name="luckyScenarioId"></param>
        public void ChangeStatusLuckyScenario(int luckyScenarioId)
        {
            int tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            string username = CommonUtils.GetCurrentUsername(_httpContext);

            var luckyScenario = _dbContext.LoyLuckyScenarios.FirstOrDefault(i => i.Id == luckyScenarioId && i.Deleted == YesNo.NO)
                .ThrowIfNull(_dbContext, ErrorCode.LoyLuckyScenarioNotFound);
            luckyScenario.Status = luckyScenario.Status == Status.ACTIVE ? Status.INACTIVE : Status.ACTIVE;
            luckyScenario.ModifiedBy = username;
            luckyScenario.ModifiedDate = DateTime.Now;

            // Nếu chuyển sang active thì tìm đến kịch bảng active trước đó để đổi lại trạng thái
            var luckyScenarios = _dbContext.LoyLuckyScenarios.Where(x => x.LuckyProgramId == luckyScenario.LuckyProgramId && luckyScenario.Status == Status.ACTIVE
                                && x.Status == Status.ACTIVE && x.Deleted == YesNo.NO);
            foreach (var item in luckyScenarios)
            {
                item.Status = Status.INACTIVE;
                item.ModifiedBy = username;
                item.ModifiedDate = DateTime.Now;
            }
            _dbContext.SaveChanges();
        }

        /// <summary>
        /// Xóa kịch bản
        /// </summary>
        /// <param name="luckyScenarioId"></param>
        public void DeleteLuckyScenario(int luckyScenarioId)
        {
            int tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            string username = CommonUtils.GetCurrentUsername(_httpContext);

            var luckyScenario = _dbContext.LoyLuckyScenarios.FirstOrDefault(i => i.Id == luckyScenarioId && i.Deleted == YesNo.NO)
                .ThrowIfNull(_dbContext, ErrorCode.LoyLuckyScenarioNotFound);
            luckyScenario.Deleted = YesNo.YES;
            luckyScenario.ModifiedBy = username;
            luckyScenario.ModifiedDate = DateTime.Now;
            _fileServices.DeleteFile(luckyScenario.AvatarImageUrl);
            _dbContext.SaveChanges();
        }

        /// <summary>
        /// Danh sách kịch bản
        /// </summary>
        /// <param name="luckyProgramId"></param>
        /// <returns></returns>
        public List<ViewLoyLuckyScenarioDto> GetAllLuckyScenario(int luckyProgramId)
        {
            int tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);

            var result = from luckyScenario in _dbContext.LoyLuckyScenarios
                         let details = (from luckyScenarioDetail in _dbContext.LoyLuckyScenarioDetails
                                        join voucher in _dbContext.LoyVouchers on luckyScenarioDetail.VoucherId equals voucher.Id
                                        where luckyScenario.Id == luckyScenarioDetail.LuckyScenarioId && luckyScenarioDetail.VoucherId != null
                                        && luckyScenarioDetail.Deleted == YesNo.NO && voucher.Deleted == YesNo.NO
                                        select new { luckyScenarioDetail.Id, voucher.Value }).AsEnumerable()
                         where luckyProgramId == luckyScenario.LuckyProgramId && luckyScenario.Deleted == YesNo.NO
                         select new ViewLoyLuckyScenarioDto
                         {
                             Id = luckyScenario.Id,
                             Name = luckyScenario.Name,
                             Status = luckyScenario.Status,
                             LuckyScenarioType = luckyScenario.LuckyScenarioType,
                             CreatedBy = luckyScenario.CreatedBy,
                             CreatedDate = luckyScenario.CreatedDate,
                             GiftQuantity = details.Count(),
                             TotalValueGift = details.Sum(x => x.Value ?? 0)
                         };
            return result.ToList();
        }

        /// <summary>
        /// Xem chi tiết kịch bản
        /// </summary>
        /// <param name="luckyScenarioId"></param>
        /// <returns></returns>
        public LoyLuckyScenarioDto FindById(int luckyScenarioId)
        {
            var luckyScenario = _dbContext.LoyLuckyScenarios.FirstOrDefault(t => t.Id == luckyScenarioId && t.Deleted == YesNo.NO)
                .ThrowIfNull(_dbContext, ErrorCode.LoyLuckyScenarioNotFound);
            var result = _mapper.Map<LoyLuckyScenarioDto>(luckyScenario);
            result.LuckyScenarioDetails = _dbContext.LoyLuckyScenarioDetails.Where(x => x.LuckyScenarioId == luckyScenarioId && x.Deleted == YesNo.NO)
                                          .Select(x => new LoyLuckyScenarioDetailDto
                                          {
                                              Id = x.Id,
                                              Name = x.Name,
                                              LuckyScenarioId = x.Id,
                                              Probability = x.Probability,
                                              Quantity = x.Quantity,
                                              VoucherId = x.VoucherId,
                                              VoucherName = _dbContext.LoyVouchers.FirstOrDefault(v => v.Id == x.VoucherId && v.Deleted == YesNo.NO).Name
                                          }).ToList();
            var luckyRotationInterface = _dbContext.LoyLuckyRotationInterfaces.FirstOrDefault(x => x.LuckyScenarioId == luckyScenario.Id);
            result.LuckyRotationInterface = _mapper.Map<LoyLuckyRotationInterfaceDto>(luckyRotationInterface);
            return result;
        }

        /// <summary>
        /// Cập nhật kịch bản
        /// </summary>
        /// <param name="input"></param>
        public void UpdateLuckyScenario(UpdateLoyLuckyScenarioDto input)
        {
            int tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            string username = CommonUtils.GetCurrentUsername(_httpContext);

            var transaction = _dbContext.Database.BeginTransaction();
            var luckyScenario = _dbContext.LoyLuckyScenarios.FirstOrDefault(t => t.Id == input.Id && t.TradingProviderId == tradingProviderId && t.Deleted == YesNo.NO)
                .ThrowIfNull(_dbContext, ErrorCode.LoyLuckyScenarioNotFound);
            luckyScenario.Name = input.Name;
            luckyScenario.PrizeQuantity = input.PrizeQuantity;
            luckyScenario.ModifiedBy = username;
            luckyScenario.ModifiedDate = DateTime.Now;

            //Cập nhật Avatar
            if (input.AvatarImageUrl != null)
            {
                _fileServices.DeleteFile(luckyScenario.AvatarImageUrl);
                luckyScenario.AvatarImageUrl = _fileServices.UploadImage(new UploadFileModel
                {
                    File = input.AvatarImageUrl,
                    Folder = FileFolder.Loyalty,
                });
            }
            foreach (var item in input.LuckyScenarioDetails)
            {
                if (item.Id != 0)
                {
                    var luckyScenarioDetails = _dbContext.LoyLuckyScenarioDetails.FirstOrDefault(d => d.Id == item.Id && d.Deleted == YesNo.NO);
                    luckyScenarioDetails.SortOrder = item.SortOrder;
                    luckyScenarioDetails.Name = item.Name;
                    luckyScenarioDetails.VoucherId = item.VoucherId;
                    luckyScenarioDetails.Probability = item.Probability;
                    luckyScenarioDetails.Quantity = item.Quantity;
                }
                else
                {
                    _dbContext.LoyLuckyScenarioDetails.Add(new LoyLuckyScenarioDetail
                    {
                        Id = (int)_loyLuckyScenarioEFRepository.NextKey($"{DbSchemas.EPIC_LOYALTY}.{LoyLuckyScenarioDetail.SEQ}"),
                        LuckyScenarioId = luckyScenario.Id,
                        SortOrder = item.SortOrder,
                        VoucherId = item.VoucherId,
                        Name = item.VoucherId == null ? item.Name : null,
                        Probability = item.Probability,
                        Quantity = item.Quantity,
                        CreatedBy = username
                    });
                }
            }
            _dbContext.SaveChanges();
            transaction.Commit();
        }
    }
}
