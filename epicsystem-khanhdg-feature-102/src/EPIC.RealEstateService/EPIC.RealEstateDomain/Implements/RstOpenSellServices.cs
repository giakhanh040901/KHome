using AutoMapper;
using EPIC.CoreRepositories;
using EPIC.DataAccess.Base;
using EPIC.DataAccess.Models;
using EPIC.Entities;
using EPIC.Entities.DataEntities;
using EPIC.Entities.Dto.BusinessCustomer;
using EPIC.RealEstateDomain.Interfaces;
using EPIC.RealEstateEntities.DataEntities;
using EPIC.RealEstateEntities.Dto.RstApprove;
using EPIC.RealEstateEntities.Dto.RstDistributionPolicy;
using EPIC.RealEstateEntities.Dto.RstOpenSell;
using EPIC.RealEstateEntities.Dto.RstOpenSellBank;
using EPIC.RealEstateEntities.Dto.RstOpenSellDetail;
using EPIC.RealEstateEntities.Dto.RstOwner;
using EPIC.RealEstateEntities.Dto.RstProductItem;
using EPIC.RealEstateEntities.Dto.RstProductItemExtend;
using EPIC.RealEstateEntities.Dto.RstProject;
using EPIC.RealEstateEntities.Dto.RstProjectStructure;
using EPIC.RealEstateEntities.Dto.RstSellingPolicy;
using EPIC.RealEstateRepositories;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Identity;
using EPIC.Utils.ConstantVariables.RealEstate;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace EPIC.RealEstateDomain.Implements
{
    public class RstOpenSellServices : IRstOpenSellServices
    {
        private readonly EpicSchemaDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<RstOpenSellServices> _logger;
        private readonly string _connectionString;
        private readonly IHttpContextAccessor _httpContext;
        private readonly RstProjectEFRepository _rstProjectEFRepository;
        private readonly RstProductItemEFRepository _rstProductItemEFRepository;
        private readonly RstProjectTypeEFRepository _rstProjectTypeEFRepository;
        private readonly RstProjectGuaranteeBankEFRepository _rstProjectGuaranteeBankRepository;
        private readonly TradingProviderEFRepository _tradingProviderEFRepository;
        private readonly BusinessCustomerEFRepository _businessCustomerEFRepository;
        private readonly RstApproveEFRepository _rstApproveEFRepository;
        private readonly RstOwnerEFRepository _rstOwnerEFRepository;
        private readonly RstDistributionEFRepository _rstDistributionEFRepository;
        private readonly RstDistributionProductItemEFRepository _rstDistributionProductItemEFRepository;
        private readonly RstOpenSellEFRepository _rstOpenSellEFRepository;
        private readonly RstOpenSellDetailEFRepository _rstOpenSellDetailEFRepository;
        private readonly BusinessCustomerBankEFRepository _businessCustomerBankEFRepository;
        private readonly RstSellingPolicyEFRepository _rstSellingPolicyEFRepository;
        private readonly RstSellingPolicyTempEFRepository _rstSellingPolicyTempEFRepository;
        private readonly RstProjectStructureEFRepository _rstProjectStructureEFRepository;
        private readonly RstProductItemExtendRepository _rstProductItemExtendRepository;
        private readonly RstOpenSellBankEFRepository _rstOpenSellBankEFRepository;
        private readonly RstDistributionBankEFRepository _rstDistributionBankEFRepository;
        private readonly RstDistributionPolicyEFRepository _rstDistributionPolicyEFRepository;
        private readonly RstHistoryUpdateEFRepository _rstHistoryUpdateEFRepository;
        private readonly SaleEFRepository _saleEFRepository;
        private readonly RstOrderEFRepository _rstOrderEFRepository;

        public RstOpenSellServices(EpicSchemaDbContext dbContext,
                 DatabaseOptions databaseOptions,
                 IMapper mapper,
                 ILogger<RstOpenSellServices> logger,
                 IHttpContextAccessor httpContextAccessor)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
            _connectionString = databaseOptions.ConnectionString;
            _httpContext = httpContextAccessor;
            _rstProjectEFRepository = new RstProjectEFRepository(dbContext, logger);
            _rstProductItemEFRepository = new RstProductItemEFRepository(dbContext, logger);
            _rstProjectTypeEFRepository = new RstProjectTypeEFRepository(dbContext, logger);
            _rstProjectGuaranteeBankRepository = new RstProjectGuaranteeBankEFRepository(dbContext, logger);
            _tradingProviderEFRepository = new TradingProviderEFRepository(dbContext, logger);
            _businessCustomerEFRepository = new BusinessCustomerEFRepository(dbContext, logger);
            _rstApproveEFRepository = new RstApproveEFRepository(dbContext, logger);
            _rstOwnerEFRepository = new RstOwnerEFRepository(dbContext, logger);
            _rstDistributionEFRepository = new RstDistributionEFRepository(dbContext, logger);
            _rstDistributionProductItemEFRepository = new RstDistributionProductItemEFRepository(dbContext, logger);
            _rstOpenSellEFRepository = new RstOpenSellEFRepository(dbContext, logger);
            _rstOpenSellDetailEFRepository = new RstOpenSellDetailEFRepository(dbContext, logger);
            _businessCustomerBankEFRepository = new BusinessCustomerBankEFRepository(dbContext, logger);
            _rstSellingPolicyEFRepository = new RstSellingPolicyEFRepository(dbContext, logger);
            _rstSellingPolicyTempEFRepository = new RstSellingPolicyTempEFRepository(dbContext, logger);
            _rstProjectStructureEFRepository = new RstProjectStructureEFRepository(dbContext, logger);
            _rstProductItemExtendRepository = new RstProductItemExtendRepository(dbContext, logger);
            _rstOpenSellBankEFRepository = new RstOpenSellBankEFRepository(dbContext, logger);
            _rstDistributionBankEFRepository = new RstDistributionBankEFRepository(dbContext, logger);
            _rstDistributionPolicyEFRepository = new RstDistributionPolicyEFRepository(dbContext, logger);
            _rstHistoryUpdateEFRepository = new RstHistoryUpdateEFRepository(dbContext, logger);
            _saleEFRepository = new SaleEFRepository(dbContext, logger);
            _rstOrderEFRepository = new RstOrderEFRepository(dbContext, logger);
        }

        #region Mở bán
        /// <summary>
        /// Thêm mở bán
        /// </summary>
        public RstOpenSell Add(CreateRstOpenSellDto input)
        {
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            _logger.LogInformation($"{nameof(Add)}: input = {JsonSerializer.Serialize(input)},  tradingProviderId = {tradingProviderId}, username = {username}");
            var inputInsert = _mapper.Map<RstOpenSell>(input);
            var projectFind = _rstProjectEFRepository.FindById(input.ProjectId).ThrowIfNull(_dbContext, ErrorCode.RstProjectNotFound);
            var rstDistribution = _rstDistributionEFRepository.FindById(input.DistributionId).ThrowIfNull(_dbContext, ErrorCode.RstDistributionNotFound);

            // Kiểm tra xem dự án được phân phối cho đại lý hay chưa
            if (!_rstProjectEFRepository.ProjectGetByTrading(tradingProviderId).Select(p => p.Id).Contains(input.ProjectId))
            {
                _rstProjectEFRepository.ThrowException(ErrorCode.RstProjectNotYetDistributionForTrading);
            }
            var transaction = _dbContext.Database.BeginTransaction();

            inputInsert.TradingProviderId = tradingProviderId;
            inputInsert.CreatedBy = username;
            var result = _rstOpenSellEFRepository.Add(inputInsert);

            _dbContext.SaveChanges();
            foreach (var item in input.OpenSellBanks)
            {
                if (item.TradingBankAccountId != null)
                {
                    // Kiểm tra xem Id tài khoản ngân hàng có thuộc đại lý không
                    var checkBankTrading = (from tradingProvider in _dbContext.TradingProviders
                                            join businessCustomerBank in _dbContext.BusinessCustomerBanks on tradingProvider.BusinessCustomerId equals businessCustomerBank.BusinessCustomerId
                                            where tradingProvider.Deleted == YesNo.NO && businessCustomerBank.Deleted == YesNo.NO
                                            && tradingProvider.TradingProviderId == tradingProviderId && businessCustomerBank.BusinessCustomerBankAccId == item.TradingBankAccountId
                                            select businessCustomerBank).FirstOrDefault().ThrowIfNull(_dbContext, ErrorCode.RstOpenSellBankTradingNotFound, item.TradingBankAccountId);
                }
                if (item.PartnerBankAccountId != null)
                {
                    _rstDistributionBankEFRepository.CheckPartnerBankAccount(input.ProjectId, item.PartnerBankAccountId ?? 0);
                }
                _rstOpenSellBankEFRepository.Add(new RstOpenSellBank
                {
                    OpenSellId = result.Id,
                    PartnerBankAccountId = item.PartnerBankAccountId,
                    TradingBankAccountId = item.TradingBankAccountId,
                    CreatedBy = username
                });
            }
            _dbContext.SaveChanges();
            transaction.Commit();
            return inputInsert;
        }

        /// <summary>
        /// Cập nhật mở bán
        /// </summary>
        public RstOpenSell Update(UpdateRstOpenSellDto input)
        {
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            _logger.LogInformation($"{nameof(Update)}: input = {JsonSerializer.Serialize(input)},  tradingProviderId = {tradingProviderId}, username = {username}");
            var inputUpdate = _mapper.Map<RstOpenSell>(input);
            var projectFind = _rstProjectEFRepository.FindById(input.ProjectId).ThrowIfNull(_dbContext, ErrorCode.RstProjectNotFound);
            inputUpdate.TradingProviderId = tradingProviderId;
            inputUpdate.ModifiedBy = username;
            _rstOpenSellEFRepository.Update(inputUpdate);
            _rstOpenSellBankEFRepository.UpdateOpenSellBank(input.Id, input.OpenSellBanks, username);
            _dbContext.SaveChanges();
            return inputUpdate;
        }

        /// <summary>
        /// Xem chi tiet mo ban
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public RstOpenSellDto FindById(int id)
        {
            var openSellQuery = _rstOpenSellEFRepository.FindById(id).ThrowIfNull(_dbContext, ErrorCode.RstOpenSellNotFound);
            RstOpenSellDto result = _mapper.Map<RstOpenSellDto>(openSellQuery);
            var projectQuery = _rstProjectEFRepository.FindById(openSellQuery.ProjectId).ThrowIfNull(_dbContext, ErrorCode.RstProjectNotFound);
            var ownerQuery = _rstOwnerEFRepository.FindById(projectQuery.OwnerId);
            var openSellDetail = _rstOpenSellDetailEFRepository.GetAllByOpenSell(id, null);
            var businessCustomer = _businessCustomerEFRepository.FindById(ownerQuery.BusinessCustomerId);

            result.Quantity = openSellDetail.Count();
            result.QuantityDeposit = openSellDetail.Count(r => r.Status == RstProductItemStatus.DA_COC);
            result.QuantitySold = openSellDetail.Count(r => r.Status == RstProductItemStatus.DA_BAN);
            result.QuantityHold = openSellDetail.Count(r => r.Status == RstProductItemStatus.GIU_CHO);
            result.Project = _mapper.Map<RstProjectDto>(projectQuery);
            result.Project.Owner = _mapper.Map<ViewRstOwnerDto>(ownerQuery);

            if (businessCustomer != null)
            {
                var businessCustomerName = businessCustomer?.Name;
                result.Project.Owner.BusinessCustomer = _mapper.Map<BusinessCustomerDto>(businessCustomer);
                var businessCustomerBank = _businessCustomerEFRepository.GetListBankByBusinessCustomerId(businessCustomer.BusinessCustomerId ?? 0);
                result.Project.Owner.BusinessCustomerBanks = _mapper.Map<List<BusinessCustomerBankDto>>(businessCustomerBank);
                result.Project.OwnerName = businessCustomerName;
                result.Project.Owner.OwnerName = businessCustomerName;
            }

            result.OpenSellBanks = new();
            var openSellBank = _rstOpenSellBankEFRepository.GetAll(id);
            foreach (var bankItem in openSellBank)
            {
                BusinessCustomerBankDto bankAccount = new();
                if (bankItem.TradingBankAccountId != null)
                {
                    bankAccount = _businessCustomerEFRepository.FindBankById(bankItem.TradingBankAccountId ?? 0)
                        .ThrowIfNull(_dbContext, ErrorCode.CoreBusinessCustomerBankNotFound, bankItem.TradingBankAccountId);
                }
                else
                {
                    bankAccount = _businessCustomerEFRepository.FindBankById(bankItem.PartnerBankAccountId ?? 0)
                        .ThrowIfNull(_dbContext, ErrorCode.CoreBusinessCustomerBankNotFound, bankItem.PartnerBankAccountId);
                }
                result.OpenSellBanks.Add(new RstOpenSellBankDto
                {
                    Id = bankItem.Id,
                    OpenSellId = bankItem.OpenSellId,
                    TradingBankAccountId = bankItem.TradingBankAccountId,
                    PartnerBankAccountId = bankItem.PartnerBankAccountId,
                    BankAccount = bankAccount
                });
            }
            return result;
        }

        /// <summary>
        /// Danh sách các mở bán của đại lý
        /// </summary>
        //public PagingResult<RstOpenSellDto> FindAll(FilterRstOpenSellDto input)
        //{
        //    _logger.LogInformation($"{nameof(FindAll)}: input = {JsonSerializer.Serialize(input)}");

        //    var usertype = CommonUtils.GetCurrentUserType(_httpContext);
        //    int? tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
        //    List<RstOpenSellDto> result = new();
        //    var openSellQuery = _rstOpenSellEFRepository.FindAll(input, tradingProviderId);

        //    foreach (var item in openSellQuery.Items)
        //    {
        //        var projectQuery = _rstProjectEFRepository.FindById(item.ProjectId);

        //        var openSellDetail = _rstOpenSellDetailEFRepository.GetAllByOpenSell(item.Id, tradingProviderId);
        //        result.Add(new RstOpenSellDto()
        //        {
        //            Id = item.Id,
        //            Quantity = openSellDetail.Count(),
        //            QuantityDeposit = openSellDetail.Where(d => d.Status == RstProductItemStatus.DA_COC).Count(),
        //            QuantitySold = openSellDetail.Where(d => d.Status == RstProductItemStatus.DA_BAN).Count(),
        //            CreatedBy = item.CreatedBy,
        //            CreatedDate = item.CreatedDate,
        //            EndDate = item.EndDate,
        //            StartDate = item.StartDate,
        //            Status = item.Status,
        //            KeepTime = item.KeepTime,
        //            IsOutstanding = item.IsOutstanding,
        //            IsShowApp = item.IsShowApp,
        //            FromType = item.FromType,
        //            Project = _mapper.Map<RstProjectDto>(projectQuery)
        //        });
        //    }
        //    return new PagingResult<RstOpenSellDto>
        //    {
        //        Items = result,
        //        TotalItems = openSellQuery.TotalItems,
        //    };
        //}

        public PagingResult<RstOpenSellDto> FindAll(FilterRstOpenSellDto input)
        {
            _logger.LogInformation($"{nameof(FindAll)}: input = {JsonSerializer.Serialize(input)}");

            var usertype = CommonUtils.GetCurrentUserType(_httpContext);
            int? tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            List<RstOpenSellDto> result = new();
            var openSellQuery = _rstOpenSellEFRepository.FindAll(input, tradingProviderId);

            foreach (var item in openSellQuery.Items)
            {
                var resultItem = new RstOpenSellDto();
                resultItem = _mapper.Map<RstOpenSellDto>(item);
                var projectQuery = _rstProjectEFRepository.FindById(item.ProjectId);
                resultItem.Project = _mapper.Map<RstProjectDto>(projectQuery);
                result.Add(resultItem);
            }
            return new PagingResult<RstOpenSellDto>
            {
                Items = result,
                TotalItems = openSellQuery.TotalItems,
            };
        }

        /// <summary>
        /// Xóa mở bán 
        /// </summary>
        public void DeleteOpenSell(int id)
        {
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            _logger.LogInformation($"{nameof(DeleteOpenSell)}: id = {id},  tradingProviderId = {tradingProviderId}, username = {username}");

            var transaction = _dbContext.Database.BeginTransaction();
            var openSell = _rstOpenSellEFRepository.FindById(id, tradingProviderId).ThrowIfNull(_dbContext, ErrorCode.RstOpenSellNotFound);

            // Nếu không phải khởi tạo hoặc hủy duyệt thì ko cho bán
            if (openSell.Status != RstDistributionStatus.KHOI_TAO)
            {
                _rstOpenSellEFRepository.ThrowException(ErrorCode.RstOpenSellCanNotDelete);
            }

            openSell.Deleted = YesNo.YES;
            openSell.ModifiedBy = username;
            openSell.ModifiedDate = DateTime.Now;

            var openSellDetail = _rstOpenSellDetailEFRepository.Entity.Where(r => r.OpenSellId == id && r.Deleted == YesNo.NO);
            foreach (var item in openSellDetail)
            {
                item.Deleted = YesNo.YES;
                item.ModifiedBy = username;
                item.ModifiedDate = DateTime.Now;
            }
            _dbContext.SaveChanges();
            transaction.Commit();
        }

        /// <summary>
        /// Tạm dừng mở bán (Tạm dừng - Đang bán)
        /// </summary>
        public void PauseOpenSell(int id)
        {
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            _logger.LogInformation($"{nameof(PauseOpenSell)}: id = {id},  tradingProviderId = {tradingProviderId}, username = {username}");
            var transaction = _dbContext.Database.BeginTransaction();
            var openSell = _rstOpenSellEFRepository.FindById(id, tradingProviderId).ThrowIfNull(_dbContext, ErrorCode.RstOpenSellNotFound);
            if (openSell.Status == RstDistributionStatus.DANG_BAN)
            {
                openSell.Status = RstDistributionStatus.TAM_DUNG;
            }
            else if (openSell.Status == RstDistributionStatus.TAM_DUNG)
            {
                openSell.Status = RstDistributionStatus.DANG_BAN;
            }
            openSell.ModifiedBy = username;
            openSell.ModifiedDate = DateTime.Now;
            _dbContext.SaveChanges();
            transaction.Commit();
        }

        /// <summary>
        /// Tắt showApp của mở bán
        /// </summary>
        /// <param name="id"></param>
        public void IsShowAppOpenSell(int id)
        {
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            _logger.LogInformation($"{nameof(IsShowAppOpenSell)}: id = {id},  tradingProviderId = {tradingProviderId}, username = {username}");
            var openSell = _rstOpenSellEFRepository.FindById(id, tradingProviderId).ThrowIfNull(_dbContext, ErrorCode.RstOpenSellNotFound);
            if (openSell.IsShowApp == YesNo.NO)
            {
                openSell.IsShowApp = YesNo.YES;
            }
            else if (openSell.IsShowApp == YesNo.YES)
            {
                openSell.IsShowApp = YesNo.NO;
            }
            _dbContext.SaveChanges();
        }

        /// <summary>
        /// Dừng mở bán
        /// </summary>
        /// <param name="id"></param>
        public void StopOpenSell(int id)
        {
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            _logger.LogInformation($"{nameof(StopOpenSell)}: id = {id},  tradingProviderId = {tradingProviderId}, username = {username}");
            var openSell = _rstOpenSellEFRepository.FindById(id, tradingProviderId).ThrowIfNull(_dbContext, ErrorCode.RstOpenSellNotFound);

            // Nếu mở bán đang bán thì được phép dừng bán
            // Các giao dịch đang giao dịch trước đó vẫn giao dịch bình thường
            if (openSell.Status == RstDistributionStatus.DANG_BAN)
            {
                openSell.Status = RstDistributionStatus.DUNG_BAN;
                // Lưu lại lịch sử người dừng bán
                _rstHistoryUpdateEFRepository.Add(new RstHistoryUpdate
                {
                    RealTableId = openSell.Id,
                    OldValue = RstDistributionStatus.DANG_BAN.ToString(),
                    NewValue = openSell.Status.ToString(),
                    FieldName = RstFieldName.UPDATE_STATUS,
                    UpdateTable = RstHistoryUpdateTables.RST_OPEN_SELL,
                    Action = ActionTypes.CAP_NHAT,
                    ActionUpdateType = RstActionUpdateTypes.STATUS_STOP,
                    Summary = "Dừng mở bán",
                    CreatedDate = DateTime.Now,
                }, username);
            }
            _dbContext.SaveChanges();
        }

        /// <summary>
        /// Thay đổi trạng thái nổi bật: Y/N
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public RstOpenSell ChangeOutstanding(int id)
        {
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            _logger.LogInformation($"{nameof(ChangeOutstanding)}: id = {id},  tradingProviderId = {tradingProviderId}, username = {username}");
            var openSell = _rstOpenSellEFRepository.FindById(id, tradingProviderId).ThrowIfNull(_dbContext, ErrorCode.RstOpenSellNotFound);
            var isOutstanding = YesNo.NO;
            if (openSell.IsOutstanding == isOutstanding)
            {
                openSell.IsOutstanding = YesNo.YES;
            }
            else
            {
                openSell.IsOutstanding = YesNo.NO;
            }
            openSell.ModifiedBy = username;
            openSell.ModifiedDate = DateTime.Now;
            _dbContext.SaveChanges();
            return openSell;
        }

        #endregion

        #region Sản phẩm của mở bán
        /// <summary>
        /// Thêm nhiều căn vào mở bán
        /// </summary>
        public void AddOpenSellDetail(CreateRstOpenSellDetailDto input)
        {
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            _logger.LogInformation($"{nameof(AddOpenSellDetail)}: input = {JsonSerializer.Serialize(input.DistributionProductItemIds)},  tradingProviderId = {tradingProviderId}, username = {username}");
            var openSellQuery = _rstOpenSellEFRepository.FindById(input.OpenSellId).ThrowIfNull(_dbContext, ErrorCode.RstOpenSellNotFound);

            var transaction = _dbContext.Database.BeginTransaction();
            foreach (var distributionItem in input.DistributionProductItemIds)
            {
                // Kiểm tra sản phẩm của phân phối
                var distributionProductItemQuery = _rstDistributionProductItemEFRepository.FindById(distributionItem).ThrowIfNull(_dbContext, ErrorCode.RstProductItemNotFound);

                //Kiểm tra xem sản phẩm đã có trong Mở bán này nay chưa
                if (_rstOpenSellDetailEFRepository.CheckAddOpenSellDetail(tradingProviderId, distributionItem))
                {
                    _rstOpenSellDetailEFRepository.ThrowException(ErrorCode.RstOpenSellDetailIsExistDistributionItem);
                }
                _rstOpenSellDetailEFRepository.Add(new RstOpenSellDetail
                {
                    OpenSellId = input.OpenSellId,
                    DistributionProductItemId = distributionItem,
                    CreatedBy = username,
                });
                _dbContext.SaveChanges();
            }
            transaction.Commit();
        }

        /// <summary>
        /// Xóa danh sách căn (Nếu có giao dịch trong đại lý về căn này thì không cho xóa)
        /// </summary>
        public void DeleteOpenSellDetail(List<int> openSellDetailIds)
        {
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            _logger.LogInformation($"{nameof(DeleteOpenSellDetail)}: input = {JsonSerializer.Serialize(openSellDetailIds)},  tradingProviderId = {tradingProviderId}, username = {username}");

            var transaction = _dbContext.Database.BeginTransaction();
            foreach (var item in openSellDetailIds)
            {
                var openSellDetail = _rstOpenSellDetailEFRepository.FindById(item, tradingProviderId).ThrowIfNull(_dbContext, ErrorCode.RstOpenSellDetailNotFound);
                // Kiểm tra xem có giao dịch mở bán hay không
                if (openSellDetail.Status != RstProductItemStatus.KHOI_TAO)
                {
                    _rstOpenSellDetailEFRepository.ThrowException(ErrorCode.RstOpenSellDetailExistTrading);
                }
                openSellDetail.Deleted = YesNo.YES;
                openSellDetail.ModifiedBy = username;
                openSellDetail.ModifiedDate = DateTime.Now;
            }
            _dbContext.SaveChanges();
            transaction.Commit();
        }

        /// <summary>
        /// Tắt showApp của mở bán
        /// </summary>
        /// <param name="id"></param>
        public void IsShowAppOpenSellDetail(int id)
        {
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            _logger.LogInformation($"{nameof(IsShowAppOpenSellDetail)}: id = {id},  tradingProviderId = {tradingProviderId}, username = {username}");
            var openSellDetail = _rstOpenSellDetailEFRepository.FindById(id, tradingProviderId).ThrowIfNull(_dbContext, ErrorCode.RstOpenSellDetailNotFound);
            if (openSellDetail.IsShowApp == YesNo.NO)
            {
                openSellDetail.IsShowApp = YesNo.YES;
                _rstHistoryUpdateEFRepository.Add(new RstHistoryUpdate(openSellDetail.Id, null, null, null, RstHistoryUpdateTables.RST_OPEN_SELL_DETAIL, ActionTypes.CAP_NHAT, "Bật showapp", DateTime.Now, RstHistoryTypes.BatShowApp), username);
            }
            else if (openSellDetail.IsShowApp == YesNo.YES)
            {
                openSellDetail.IsShowApp = YesNo.NO;
                _rstHistoryUpdateEFRepository.Add(new RstHistoryUpdate(openSellDetail.Id, null, null, null, RstHistoryUpdateTables.RST_OPEN_SELL_DETAIL, ActionTypes.CAP_NHAT, "Tắt showapp", DateTime.Now, RstHistoryTypes.TatShowApp), username);
            }
            _dbContext.SaveChanges();
        }

        /// <summary>
        /// Đại lý khóa căn
        /// </summary>
        /// <param name="input"></param>
        public void LockOpenSellDetail(LockRstOpenSellDetailDto input)
        {
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            _logger.LogInformation($"{nameof(LockOpenSellDetail)}: input = {JsonSerializer.Serialize(input)}, tradingProviderId = {tradingProviderId}, username = {username}");

            var transaction = _dbContext.Database.BeginTransaction();
            // Tìm kiếm thông tin căn
            var openSellDetailQuery = _rstOpenSellDetailEFRepository.FindById(input.Id).ThrowIfNull(_dbContext, ErrorCode.RstOpenSellDetailNotFound);
            var distributionItemQuery = _rstDistributionProductItemEFRepository.FindById(openSellDetailQuery.DistributionProductItemId).ThrowIfNull(_dbContext, ErrorCode.RstDistributionProductItemNotFound);
            var productItemQuery = _rstProductItemEFRepository.FindById(distributionItemQuery.ProductItemId).ThrowIfNull(_dbContext, ErrorCode.RstDistributionProductItemNotFound);

            // Nếu căn hộ, mở bán nằm trong trạng thái khởi tạo và không bị khóa ở phân phối thì Đại lý được phép khóa căn
            if (openSellDetailQuery.Status == RstProductItemStatus.KHOI_TAO && productItemQuery.Status == RstProductItemStatus.KHOI_TAO && distributionItemQuery.Status == Status.ACTIVE)
            {
                // Lưu lại lịch sử người mở khóa căn hộ
                _rstHistoryUpdateEFRepository.Add(new RstHistoryUpdate
                {
                    RealTableId = openSellDetailQuery.Id,
                    OldValue = RstProductItemIsLock.IsLock(openSellDetailQuery.IsLock),
                    NewValue = RstProductItemIsLock.IsLock(openSellDetailQuery.IsLock == YesNo.NO ? YesNo.YES : YesNo.NO),
                    FieldName = RstFieldName.UPDATE_STATUS,
                    UpdateTable = RstHistoryUpdateTables.RST_OPEN_SELL_DETAIL,
                    Action = ActionTypes.CAP_NHAT,
                    ActionUpdateType = RstActionUpdateTypes.LOCK,
                    UpdateReason = openSellDetailQuery.IsLock == YesNo.NO ? input.LockingReason : RstUpdateReasons.MO_KHOA,
                    Summary = openSellDetailQuery.IsLock == YesNo.NO ? "Khóa sản phẩm mở bán " + input.Summary : "Mở khóa sản phẩm mở bán",
                    CreatedDate = DateTime.Now,
                    Type = openSellDetailQuery.IsLock == YesNo.NO ? RstHistoryTypes.KhoaCan : RstHistoryTypes.MoCan,
                }, username);
                openSellDetailQuery.IsLock = openSellDetailQuery.IsLock == YesNo.NO ? YesNo.YES : YesNo.NO;
                _dbContext.SaveChanges();
                transaction.Commit();
            }
            else
            {
                _rstProductItemEFRepository.ThrowException(ErrorCode.RstProductItemCanNotLockCuzStatusTrading);
            }
        }

        /// <summary>
        /// Xem chi tiết căn hộ của mở bán
        /// </summary>
        public RstProductItemByOpenSellDetailDto ProductItemByOpenSellDetail(int openSellDetailId)
        {
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            _logger.LogInformation($"{nameof(ProductItemByOpenSellDetail)}: openSellDetailId = {openSellDetailId}, tradingProviderId = {tradingProviderId}");
            var openSellDetail = _rstOpenSellDetailEFRepository.FindById(openSellDetailId, tradingProviderId).ThrowIfNull(_dbContext, ErrorCode.RstOpenSellDetailNotFound);
            var productItem = _rstProductItemEFRepository.AppProductItemDetail(openSellDetailId).ThrowIfNull<RstProductItem>(_dbContext, ErrorCode.RstProductItemNotFound);
            var result = _mapper.Map<RstProductItemByOpenSellDetailDto>(productItem);
            result.OpenSellId = openSellDetail.OpenSellId;
            result.ProductItemExtends = _mapper.Map<List<RstProductItemExtendDto>>(_rstProductItemExtendRepository.GetAll(productItem?.Id ?? 0));
            var projectStructure = _rstProjectStructureEFRepository.FindById(productItem.BuildingDensityId ?? 0);
            result.ProjectStructure = _mapper.Map<RstProjectStructureDto>(projectStructure);
            return result;
        }

        /// <summary>
        /// Xem danh sách con
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        //public PagingResult<RstOpenSellDetailDto> FindAllOpenSellDetail(FilterRstOpenSellDetailDto input)
        //{
        //    _logger.LogInformation($"{nameof(FindAllOpenSellDetail)}: input = {JsonSerializer.Serialize(input)}");
        //    var usertype = CommonUtils.GetCurrentUserType(_httpContext);
        //    int? tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
        //    List<RstOpenSellDetailDto> result = new();
        //    var openSellQuery = _rstOpenSellEFRepository.FindById(input.OpenSellId).ThrowIfNull(_dbContext, ErrorCode.RstOpenSellNotFound);
        //    var openSellDetailQuery = _rstOpenSellDetailEFRepository.FindAll(input, tradingProviderId);
        //    List<RstSellingPolicyDto> sellingPolicyList = new();

        //    var sellingPolicies = _rstSellingPolicyEFRepository.FindAllByOpenSellId(input.OpenSellId);
        //    foreach (var sellingPolicy in sellingPolicies)
        //    {
        //        var sellingPolicyTemp = _rstSellingPolicyTempEFRepository.FindById(sellingPolicy.SellingPolicyTempId)
        //            .ThrowIfNull(_dbContext, ErrorCode.RstSellingPolicyNotFound);
        //        var sellingPolicyItem = new RstSellingPolicyDto()
        //        {
        //            Id = sellingPolicy.Id,
        //            SellingPolicyTempId = sellingPolicyTemp.Id,
        //            Name = sellingPolicyTemp?.Name,
        //            Code = sellingPolicyTemp?.Code,
        //            ConversionValue = sellingPolicyTemp?.ConversionValue,
        //            Status = sellingPolicyTemp?.Status,
        //            OpenSellId = sellingPolicy.OpenSellId,
        //        };
        //        sellingPolicyList.Add(sellingPolicyItem);
        //    }

        //    foreach (var item in openSellDetailQuery.Items)
        //    {
        //        RstOpenSellDetailDto resultItem = _mapper.Map<RstOpenSellDetailDto>(item);
        //        var distributionProductItemQuery = _rstDistributionProductItemEFRepository.FindById(item.DistributionProductItemId);
        //        if (distributionProductItemQuery == null)
        //        {
        //            continue;
        //        }
        //        var productItemQuery = _rstProductItemEFRepository.AppProductItemDetail(item.Id);
        //        if (productItemQuery == null)
        //        {
        //            continue;
        //        }
        //        var productItemPrice = _rstProductItemEFRepository.ProductItemPriceByDistribution(productItemQuery.Price ?? 0, distributionProductItemQuery?.DistributionId ?? 0);
        //        resultItem.DepositPrice = productItemPrice.DepositPrice;
        //        resultItem.LockPrice = productItemPrice.LockPrice;
        //        resultItem.ProductItem = _mapper.Map<RstProductItemDto>(productItemQuery);
        //        var projectStructure = _rstProjectStructureEFRepository.FindById((int)productItemQuery.BuildingDensityId);
        //        if (projectStructure != null)
        //        {
        //            resultItem.ProductItem.ProjectStructure = _mapper.Map<RstProjectStructureDto>(projectStructure);
        //        }
        //        resultItem.ProductItemStatus = productItemQuery.Status;
        //        resultItem.DistributionProductItemStatus = distributionProductItemQuery.Status;
        //        // Logic trạng thái mở bán hoặc chưa mở bán
        //        if (productItemQuery.Status == RstProductItemStatus.KHOI_TAO && (new List<int> { RstDistributionStatus.TAM_DUNG, RstDistributionStatus.DANG_BAN, RstDistributionStatus.HET_HANG }).Contains(openSellQuery.Status))
        //        {
        //            resultItem.ProductItemStatus = (openSellQuery.StartDate.Date <= DateTime.Now.Date) ? RstProductItemStatus.LOGIC_DANG_MO_BAN : RstProductItemStatus.LOGIC_CHUA_MO_BAN;
        //        }
        //        resultItem.SellingPolicy = sellingPolicyList;
        //        resultItem.KeepTime = openSellQuery.KeepTime;
        //        result.Add(resultItem);
        //    }
        //    return new PagingResult<RstOpenSellDetailDto>
        //    {
        //        Items = result,
        //        TotalItems = openSellDetailQuery.TotalItems,
        //    };
        //}

        public PagingResult<RstOpenSellDetailDto> FindAllOpenSellDetail(FilterRstOpenSellDetailDto input)
        {
            _logger.LogInformation($"{nameof(FindAllOpenSellDetail)}: input = {JsonSerializer.Serialize(input)}");
            var usertype = CommonUtils.GetCurrentUserType(_httpContext);
            int? tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            List<RstOpenSellDetailDto> result = new();
            var openSellQuery = _rstOpenSellEFRepository.FindById(input.OpenSellId).ThrowIfNull(_dbContext, ErrorCode.RstOpenSellNotFound);
            var openSellDetailQuery = _rstOpenSellDetailEFRepository.FindAll(input, tradingProviderId);
            List<RstSellingPolicyDto> sellingPolicyList = new();

            var sellingPolicies = _rstSellingPolicyEFRepository.FindAllByOpenSellId(input.OpenSellId);
            foreach (var sellingPolicy in sellingPolicies)
            {
                var sellingPolicyTemp = _rstSellingPolicyTempEFRepository.FindById(sellingPolicy.SellingPolicyTempId)
                    .ThrowIfNull(_dbContext, ErrorCode.RstSellingPolicyNotFound);
                var sellingPolicyItem = new RstSellingPolicyDto()
                {
                    Id = sellingPolicy.Id,
                    SellingPolicyTempId = sellingPolicyTemp.Id,
                    Name = sellingPolicyTemp?.Name,
                    Code = sellingPolicyTemp?.Code,
                    ConversionValue = sellingPolicyTemp?.ConversionValue,
                    Status = sellingPolicyTemp?.Status,
                    OpenSellId = sellingPolicy.OpenSellId,
                };
                sellingPolicyList.Add(sellingPolicyItem);
            }

            foreach (var item in openSellDetailQuery.Items)
            {
                RstOpenSellDetailDto resultItem = _mapper.Map<RstOpenSellDetailDto>(item);
                var distributionProductItemQuery = _rstDistributionProductItemEFRepository.FindById(item.DistributionProductItemId);
                if (distributionProductItemQuery == null)
                {
                    continue;
                }
                var productItemQuery = _rstProductItemEFRepository.AppProductItemDetail(item.Id);
                if (productItemQuery == null)
                {
                    continue;
                }
                var productItemPrice = _rstProductItemEFRepository.ProductItemPriceByDistribution(productItemQuery.Price ?? 0, distributionProductItemQuery?.DistributionId ?? 0);
                resultItem.DepositPrice = productItemPrice.DepositPrice;
                resultItem.LockPrice = productItemPrice.LockPrice;
                resultItem.ProductItem = _mapper.Map<RstProductItemDto>(productItemQuery);
                var projectStructure = _rstProjectStructureEFRepository.FindById((int)productItemQuery.BuildingDensityId);
                if (projectStructure != null)
                {
                    resultItem.ProductItem.ProjectStructure = _mapper.Map<RstProjectStructureDto>(projectStructure);
                }
                resultItem.ProductItemStatus = productItemQuery.Status;
                resultItem.DistributionProductItemStatus = distributionProductItemQuery.Status;
                // Logic trạng thái mở bán hoặc chưa mở bán
                if (productItemQuery.Status == RstProductItemStatus.KHOI_TAO && (new List<int> { RstDistributionStatus.TAM_DUNG, RstDistributionStatus.DANG_BAN, RstDistributionStatus.HET_HANG }).Contains(openSellQuery.Status))
                {
                    resultItem.ProductItemStatus = (openSellQuery.StartDate.Date <= DateTime.Now.Date) ? RstProductItemStatus.LOGIC_DANG_MO_BAN : RstProductItemStatus.LOGIC_CHUA_MO_BAN;
                }
                resultItem.SellingPolicy = sellingPolicyList;
                resultItem.KeepTime = openSellQuery.KeepTime;
                result.Add(resultItem);
            }
            return new PagingResult<RstOpenSellDetailDto>
            {
                Items = result,
                TotalItems = openSellDetailQuery.TotalItems,
            };
        }

        /// <summary>
        /// Thông tin hợp đồng mới nhất được tạo trong mở bán
        /// </summary>
        public InfoOrderNewInProjectDto InfoOrderNewInOpenSell(int openSellId)
        {
            var order = _dbContext.RstOrders.Select(o => new { o.Id, o.OpenSellDetailId, o.Status, o.Deleted })
                            .Where(o => o.Deleted == YesNo.NO && _dbContext.RstOpenSellDetails.Any(p => p.Id == o.OpenSellDetailId && p.Deleted == YesNo.NO && p.OpenSellId == openSellId))
                            .OrderByDescending(o => o.Id).FirstOrDefault();
            if (order == null)
            {
                return null;
            }
            var result = _rstOrderEFRepository.InfoOrderNewInProject(order.Id);
            return result;
        }

        /// <summary>
        /// Danh sách mở bán của đại lý có thể đặt lệnh
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public List<RstOpenSellDetailDto> GetAllOpenSellDetailForOrder(FilterRstOpenSellDetailForOrder input)
        {
            _logger.LogInformation($"{nameof(GetAllOpenSellDetailForOrder)}: input = {JsonSerializer.Serialize(input)}");
            var usertype = CommonUtils.GetCurrentUserType(_httpContext);
            int tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            List<RstOpenSellDetailDto> result = new();
            var openSellDetailQuery = _rstOpenSellDetailEFRepository.GetAllOpenSellDetailForOrder(input, tradingProviderId);
            foreach (var item in openSellDetailQuery)
            {
                RstOpenSellDetailDto resultItem = _mapper.Map<RstOpenSellDetailDto>(item);
                var distributionProductItemQuery = _rstDistributionProductItemEFRepository.FindById(item.DistributionProductItemId);
                if (distributionProductItemQuery == null)
                {
                    continue;
                }
                var productItemQuery = _rstProductItemEFRepository.AppProductItemDetail(item.Id);
                if (productItemQuery == null)
                {
                    continue;
                }
                var openSellQuery = _rstOpenSellEFRepository.FindById(item.OpenSellId);
                var productItemPrice = _rstProductItemEFRepository.ProductItemPriceByDistribution(productItemQuery.Price ?? 0, distributionProductItemQuery.DistributionId);
                resultItem.DepositPrice = productItemPrice.DepositPrice;
                resultItem.LockPrice = productItemPrice.LockPrice;
                var distributionPolicyQuery = _rstDistributionPolicyEFRepository.FindById(productItemPrice.DistributionPolicyId);
                if (distributionPolicyQuery == null)
                {
                    continue;
                }
                resultItem.DistributionPolicy = _mapper.Map<RstDistributionPolicyDto>(distributionPolicyQuery);
                resultItem.ProductItem = _mapper.Map<RstProductItemDto>(productItemQuery);
                var projectStructure = _rstProjectStructureEFRepository.FindById((int)productItemQuery.BuildingDensityId);
                if (projectStructure != null)
                {
                    resultItem.ProductItem.ProjectStructure = _mapper.Map<RstProjectStructureDto>(projectStructure);
                }
                resultItem.KeepTime = openSellQuery?.KeepTime;
                result.Add(resultItem);
            }
            return result;
        }

        /// <summary>
        /// Danh sách ngân hàng có thể phân phối cho mở bán của đại lý
        /// </summary>
        public List<BankAccountDtoForOpenSell> BankAccountCanDistributionOpenSell(int projectId, int? bankType)
        {
            List<BankAccountDtoForOpenSell> result = new();
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);

            var tradingProviderFind = _tradingProviderEFRepository.FindById(tradingProviderId).ThrowIfNull(_dbContext, ErrorCode.TradingProviderNotFound);
            var tradingBankAccount = _businessCustomerEFRepository.GetListBankByBusinessCustomerId(tradingProviderFind.BusinessCustomerId);
            foreach (var bankAccount in tradingBankAccount)
            {
                result.Add(new BankAccountDtoForOpenSell
                {
                    TradingBankAccountId = bankAccount.BusinessCustomerBankAccId,
                    BankAccount = bankAccount,
                });
            }

            var distributionBanks = from project in _dbContext.RstProjects
                                    join distribution in _dbContext.RstDistributions on project.Id equals distribution.ProjectId
                                    join distributionBank in _dbContext.RstDistributionBanks on distribution.Id equals distributionBank.DistributionId
                                    where distribution.TradingProviderId == tradingProviderId && distribution.ProjectId == projectId
                                    && project.Deleted == YesNo.NO && distribution.Deleted == YesNo.NO && distributionBank.Deleted == YesNo.NO
                                    select distributionBank;
            foreach (var item in distributionBanks)
            {
                BankAccountDtoForOpenSell resultItem = new()
                {
                    PartnerBankAccountId = item.PartnerBankAccountId,
                    BankAccount = _businessCustomerEFRepository.FindBankById(item.PartnerBankAccountId)
                };
                result.Add(resultItem);
            }
            result = result.Where(b => ((bankType == null || bankType == RstOpenSellBankTypes.All) || (bankType == RstOpenSellBankTypes.BankTrading && b.TradingBankAccountId != null)
                            || (bankType == RstOpenSellBankTypes.BankPartner && b.PartnerBankAccountId != null))).ToList();
            return result;
        }

        /// <summary>
        /// Tắt hiển thị giá bán của sản phẩm
        /// </summary>
        public void HideOpenSellDetail(RstOpenSellDetailHidePriceDto input)
        {
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            _logger.LogInformation($"{nameof(HideOpenSellDetail)}: input = {JsonSerializer.Serialize(input)},  tradingProviderId = {tradingProviderId}, username = {username}");

            var openSellDetail = _rstOpenSellDetailEFRepository.FindById(input.Id, tradingProviderId).ThrowIfNull(_dbContext, ErrorCode.RstOpenSellDetailNotFound);

            openSellDetail.IsShowPrice = YesNo.NO;
            openSellDetail.ContactType = input.ContractType;
            openSellDetail.ContactPhone = input.ContractPhone;
            _dbContext.SaveChanges();
        }

        /// <summary>
        /// Tắt hiển thị giá bán của sản phẩm
        /// </summary>
        public void ShowOpenSellDetail(int openSellDetailId)
        {
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            _logger.LogInformation($"{nameof(ShowOpenSellDetail)}: id = {openSellDetailId},  tradingProviderId = {tradingProviderId}, username = {username}");

            var openSellDetail = _rstOpenSellDetailEFRepository.FindById(openSellDetailId, tradingProviderId).ThrowIfNull(_dbContext, ErrorCode.RstOpenSellDetailNotFound);

            openSellDetail.IsShowPrice = YesNo.YES;
            openSellDetail.ContactType = null;
            openSellDetail.ContactPhone = null;
            _dbContext.SaveChanges();
        }
        #endregion

        #region Trình duyệt sản phẩm mở bán
        /// <summary>
        /// Yêu cầu trình duyệt
        /// </summary>
        public void Request(RstRequestDto input)
        {
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var userId = CommonUtils.GetCurrentUserId(_httpContext);
            var username = CommonUtils.GetCurrentUsername(_httpContext);

            _logger.LogInformation($"{nameof(Request)}: input = {JsonSerializer.Serialize(input)}, tradingProviderId = {tradingProviderId}, username = {username}");

            //Tìm kiếm thông tin mở bán
            var openSellQuery = _rstOpenSellEFRepository.FindById(input.Id).ThrowIfNull(_dbContext, ErrorCode.RstOpenSellNotFound);

            //Nếu đã tồn tại bản ghi trước đó
            var actionType = ActionTypes.THEM_MOI;
            var findRequest = _rstApproveEFRepository.FindByIdOfDataType(input.Id, RstApproveDataTypes.RST_OPEN_SELL);
            if (findRequest != null)
            {
                actionType = ActionTypes.CAP_NHAT;
            }

            var request = _rstApproveEFRepository.Request(new RstApprove
            {
                UserRequestId = userId,
                UserApproveId = input.UserApproveId,
                RequestNote = input.RequestNote,
                ActionType = actionType,
                DataType = RstApproveDataTypes.RST_OPEN_SELL,
                ReferId = input.Id,
                Summary = input.Summary,
                CreatedBy = username,
                TradingProviderId = tradingProviderId,
                PartnerId = null
            });

            openSellQuery.Status = RstDistributionStatus.CHO_DUYET;
            openSellQuery.Description = input.Summary;
            request.DataStatus = RstDistributionStatus.CHO_DUYET;
            _dbContext.SaveChanges();
        }

        /// <summary>
        /// Duyệt Mở bán
        /// </summary>
        public void Approve(RstApproveDto input)
        {
            var userId = CommonUtils.GetCurrentUserId(_httpContext);
            _logger.LogInformation($"{nameof(Approve)}: input = {JsonSerializer.Serialize(input)}, userId = {userId}");
            ///Tìm kiếm thông tin mở bán
            var openSellQuery = _rstOpenSellEFRepository.FindById(input.Id).ThrowIfNull(_dbContext, ErrorCode.RstOpenSellNotFound);

            // Kiểm tra xem mở bán có tài khoản nhận tiền chưa thì mới được phép duyệt
            if (!_rstOpenSellBankEFRepository.GetAll(input.Id).Any())
            {
                _rstOpenSellBankEFRepository.ThrowException(ErrorCode.RstOpenSellCanNotApproveOpenSellBankNotExist);
            };

            if (openSellQuery.Status == RstDistributionStatus.CHO_DUYET)
            {
                var findRequest = _rstApproveEFRepository.FindByIdOfDataType(input.Id, RstApproveDataTypes.RST_OPEN_SELL).ThrowIfNull(_dbContext, ErrorCode.RstApproveNotFound);

                _rstApproveEFRepository.Approve(new RstApprove
                {
                    Id = findRequest.Id,
                    ApproveNote = input.ApproveNote,
                    UserApproveId = userId,
                    ApproveIp = CommonUtils.GetCurrentRemoteIpAddress(_httpContext)
                });

                openSellQuery.Status = RstDistributionStatus.DANG_BAN;
            }
            _dbContext.SaveChanges();
        }

        /// <summary>
        /// Huy duyet mở bán
        /// </summary>
        public void Cancel(RstCancelDto input)
        {
            _logger.LogInformation($"{nameof(Cancel)}: input = {JsonSerializer.Serialize(input)}");

            //Tìm kiếm thông tin mở bán
            var openSellQuery = _rstOpenSellEFRepository.FindById(input.Id).ThrowIfNull(_dbContext, ErrorCode.RstOpenSellNotFound);

            var findRequest = _rstApproveEFRepository.FindByIdOfDataType(input.Id, RstApproveDataTypes.RST_OPEN_SELL).ThrowIfNull(_dbContext, ErrorCode.RstApproveNotFound);
            _rstApproveEFRepository.Cancel(new RstApprove
            {
                Id = findRequest.Id,
                CancelNote = input.CancelNote
            });
            //Cập nhật trạng thái cho mở bán
            openSellQuery.Status = RstDistributionStatus.HUY_DUYET;
            _dbContext.SaveChanges();
        }

        public IEnumerable<RstOpenSellByTradingDto> FindOpenSellBanHoByTrading()
        {
            var userType = CommonUtils.GetCurrentUserType(_httpContext);
            var (tradingProviderId, tradingBanHo) = userType == UserTypes.TRADING_PROVIDER || userType == UserTypes.ROOT_TRADING_PROVIDER
                                            ? (CommonUtils.GetCurrentTradingProviderId(_httpContext), _saleEFRepository.FindTradingProviderBanHo(CommonUtils.GetCurrentTradingProviderId(_httpContext)))
                                            : (0, new List<int>());

            var result = (from openSells in _dbContext.RstOpenSells
                          join projects in _dbContext.RstProjects on openSells.ProjectId equals projects.Id
                          where (userType == UserTypes.ROOT_EPIC ||
                                (tradingBanHo.Contains(openSells.TradingProviderId) || openSells.TradingProviderId == tradingProviderId))
                                && openSells.Status == RstDistributionStatus.DANG_BAN
                                && openSells.IsShowApp == YesNo.YES
                                && openSells.Deleted == YesNo.NO
                          select new RstOpenSellByTradingDto
                          {
                              Id = openSells.Id,
                              Code = projects.Code,
                              Name = projects.Name,
                              IsSalePartnership = tradingBanHo.Contains(openSells.TradingProviderId)
                          }).Distinct().AsEnumerable();
            return result;
        }
        #endregion
    }
}
