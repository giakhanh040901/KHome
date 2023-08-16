using AutoMapper;
using DocumentFormat.OpenXml.Vml.Office;
using EPIC.CoreRepositories;
using EPIC.DataAccess.Base;
using EPIC.DataAccess.Models;
using EPIC.Entities;
using EPIC.Entities.Dto.BusinessCustomer;
using EPIC.IdentityEntities.DataEntities;
using EPIC.RealEstateDomain.Interfaces;
using EPIC.RealEstateEntities.DataEntities;
using EPIC.RealEstateEntities.Dto.RstApprove;
using EPIC.RealEstateEntities.Dto.RstDistribution;
using EPIC.RealEstateEntities.Dto.RstDistributionProductItem;
using EPIC.RealEstateEntities.Dto.RstOwner;
using EPIC.RealEstateEntities.Dto.RstProductItem;
using EPIC.RealEstateEntities.Dto.RstProject;
using EPIC.RealEstateEntities.Dto.RstProjectStructure;
using EPIC.RealEstateRepositories;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Identity;
using EPIC.Utils.ConstantVariables.RealEstate;
using EPIC.Utils.ConstantVariables.Shared;
using EPIC.Utils.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace EPIC.RealEstateDomain.Implements
{
    public class RstDistributionServices : IRstDistributionServices
    {
        private readonly EpicSchemaDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<RstDistributionServices> _logger;
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
        private readonly RstOpenSellDetailEFRepository _rstOpenSellDetailEFRepository;
        private readonly RstOpenSellEFRepository _rstOpenSellEFRepository;
        private readonly RstHistoryUpdateEFRepository _rstHistoryUpdateEFRepository;
        private readonly RstDistributionBankEFRepository _rstDistributionBankEFRepository;
        private readonly RstProjectStructureEFRepository _rstProjectStructureEFRepository;

        public RstDistributionServices(EpicSchemaDbContext dbContext,
            DatabaseOptions databaseOptions,
            IMapper mapper,
            ILogger<RstDistributionServices> logger,
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
            _rstOpenSellDetailEFRepository = new RstOpenSellDetailEFRepository(dbContext, logger);
            _rstOpenSellEFRepository = new RstOpenSellEFRepository(dbContext, logger);
            _rstHistoryUpdateEFRepository = new RstHistoryUpdateEFRepository(dbContext, logger);
            _rstDistributionBankEFRepository = new RstDistributionBankEFRepository(dbContext, logger);
            _rstProjectStructureEFRepository = new RstProjectStructureEFRepository(dbContext, logger);
        }

        #region Đối tác - Phân phối cho đại lý
        /// <summary>
        /// Đối tác - Thêm mới phân phối cho đại lý
        /// </summary>
        public RstDistribution Add(CreateRstDistributionDto input)
        {
            var partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            _logger.LogInformation($"{nameof(Add)}: input = {JsonSerializer.Serialize(input)},  partnerId = {partnerId}, username = {username}");
            var projectFind = _rstProjectEFRepository.FindById(input.ProjectId, partnerId).ThrowIfNull(_dbContext, ErrorCode.RstProjectNotFound);
            var transaction = _dbContext.Database.BeginTransaction();
            var inputInsert = _mapper.Map<RstDistribution>(input);
            inputInsert.PartnerId = partnerId;
            inputInsert.CreatedBy = username;
            inputInsert = _rstDistributionEFRepository.Add(inputInsert);
            foreach (var partnerBankAccountItem in input.PartnerBankAccountIds)
            {
                _rstDistributionBankEFRepository.CheckPartnerBankAccount(input.ProjectId, partnerBankAccountItem);
                _rstDistributionBankEFRepository.Add(new RstDistributionBank
                {
                    DistributionId = inputInsert.Id,
                    PartnerBankAccountId = partnerBankAccountItem,
                    CreatedBy = username,
                });
            }
            _dbContext.SaveChanges();
            transaction.Commit();
            return inputInsert;
        }

        /// <summary>
        /// Đối tác - Cập nhật thông tin phân phối
        /// </summary>
        public void Update(UpdateRstDistributionDto input)
        {
            var partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            _logger.LogInformation($"{nameof(Update)}: input = {JsonSerializer.Serialize(input)},  partnerId = {partnerId}, username = {username}");
            var projectFind = _rstProjectEFRepository.FindById(input.ProjectId, partnerId).ThrowIfNull(_dbContext, ErrorCode.RstProjectNotFound);
            var transaction = _dbContext.Database.BeginTransaction();
            var inputUpdate = _mapper.Map<RstDistribution>(input);
            inputUpdate.PartnerId = partnerId;
            inputUpdate.ModifiedBy = username;
            _rstDistributionEFRepository.Update(inputUpdate);
            _rstDistributionBankEFRepository.UpdateDistributionBank(input.Id, input.PartnerBankAccountIds, username);
            _dbContext.SaveChanges();
            transaction.Commit();
        }

        /// <summary>
        /// Xem chi tiet phan phoi
        /// </summary>
        public RstDistributionDto FindById(int id)
        {
            RstDistributionDto result = new();
            _logger.LogInformation($"{nameof(FindById)}: id = {id}");
            var distributionQuery = _rstDistributionEFRepository.FindById(id);
            if (distributionQuery != null)
            {
                // Thông tin dự án
                var projectQuery = _rstProjectEFRepository.FindById(distributionQuery.ProjectId);
                var ownerQuery = _rstOwnerEFRepository.FindById(projectQuery.OwnerId);
                var businessCustomer = _businessCustomerEFRepository.FindById(ownerQuery.BusinessCustomerId);
                // Thông tin của đại lý
                var tradingProviderQuery = _tradingProviderEFRepository.GetById(distributionQuery.TradingProviderId);
                // Lấy ra các sản phẩm trong phân phối đã được đại lý mở bán
                var openSellDetailQuery = from distributionItem in _dbContext.RstDistributionProductItems
                                          join openSellDetail in _rstOpenSellDetailEFRepository.Entity on distributionItem.Id equals openSellDetail.DistributionProductItemId
                                          where openSellDetail.Deleted == YesNo.NO && distributionItem.Deleted == YesNo.NO
                                          && distributionItem.DistributionId == distributionQuery.Id
                                          select openSellDetail;
                result = _mapper.Map<RstDistributionDto>(distributionQuery);
                result.Quantity = _rstDistributionProductItemEFRepository.GetAllByDistribution(id).Count();
                result.QuantityDeposit = openSellDetailQuery.Where(r => r.Status == RstProductItemStatus.DA_COC).Count();
                result.QuantitySold = openSellDetailQuery.Where(r => r.Status == RstProductItemStatus.DA_BAN).Count();
                result.Project = _mapper.Map<RstProjectDto>(projectQuery);
                if (businessCustomer != null)
                {
                    var ownerFind = _mapper.Map<ViewRstOwnerDto>(ownerQuery);
                    result.Project.Owner = ownerFind;
                    var listBusinessCustomerBank = _businessCustomerEFRepository.GetListBankByBusinessCustomerId(businessCustomer.BusinessCustomerId ?? 0);
                    var businessCustomerBank = _mapper.Map<List<BusinessCustomerBankDto>>(listBusinessCustomerBank);
                    result.Project.Owner.BusinessCustomerBanks = businessCustomerBank;
                }
                result.TradingProvider = tradingProviderQuery;
                result.PartnerBankAccountIds = _rstDistributionBankEFRepository.GetAll(id).Select(r => r.PartnerBankAccountId).ToList();
            }
            return result;
        }

        /// <summary>
        /// Xem danh sách phân phối
        /// </summary>
        //public PagingResult<RstDistributionDto> FindAll(FilterRstDistributionDto input)
        //{
        //    _logger.LogInformation($"{nameof(FindAll)}: input = {JsonSerializer.Serialize(input)}");

        //    var usertype = CommonUtils.GetCurrentUserType(_httpContext);
        //    int? tradingProviderId = null;
        //    if (usertype == UserData.TRADING_PROVIDER || usertype == UserData.ROOT_TRADING_PROVIDER)
        //    {
        //        tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
        //    }
        //    int? partnerId = null;
        //    if (usertype == UserData.PARTNER || usertype == UserData.ROOT_PARTNER)
        //    {
        //        partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
        //    }

        //    List<RstDistributionDto> result = new();
        //    var distributionQuery = _rstDistributionEFRepository.FindAll(input, partnerId, tradingProviderId);

        //    foreach (var item in distributionQuery.Items)
        //    {
        //        // Thông tin dự án
        //        var projectQuery = _rstProjectEFRepository.FindById(item.ProjectId);

        //        // Thông tin của đại lý
        //        var tradingProviderQuery = _tradingProviderEFRepository.GetById(item.TradingProviderId);

        //        var distributionProductItems = _rstDistributionProductItemEFRepository.GetAllByDistribution(item.Id).OrderByDescending(o => o.Id);

        //        // Lấy ra các sản phẩm trong phân phối đã được đại lý mở bán
        //        var openSellDetailQuery = from distributionItem in distributionProductItems
        //                                  join openSellDetail in _rstOpenSellDetailEFRepository.Entity on distributionItem.Id equals openSellDetail.DistributionProductItemId
        //                                  where openSellDetail.Deleted == YesNo.NO
        //                                  select openSellDetail;

        //        // Tìm thông tin người duyệt sản phẩm
        //        var approveBy = (from approve in _dbContext.RstApproves
        //                         join user in _dbContext.Users on approve.UserApproveId equals user.UserId
        //                         where approve.Status == ApproveStatus.DUYET && approve.Deleted == YesNo.NO
        //                         && approve.DataType == RstApproveDataTypes.RST_DISTRIBUTION && approve.ReferId == item.Id
        //                         select user.UserName).FirstOrDefault();

        //        result.Add(new RstDistributionDto()
        //        {
        //            Id = item.Id,
        //            Quantity = distributionProductItems.Count(),
        //            QuantityDeposit = openSellDetailQuery.Where(r => r.Status == RstProductItemStatus.DA_COC).Count(),
        //            QuantitySold = openSellDetailQuery.Where(r => r.Status == RstProductItemStatus.DA_BAN).Count(),
        //            CreatedBy = item.CreatedBy,
        //            CreatedDate = item.CreatedDate,
        //            EndDate = item.EndDate,
        //            StartDate = item.StartDate,
        //            Status = item.Status,
        //            Description = item.Description,
        //            TradingProvider = tradingProviderQuery,
        //            ApproveBy = approveBy,
        //            Project = _mapper.Map<RstProjectDto>(projectQuery)
        //        });
        //    }
        //    return new PagingResult<RstDistributionDto>
        //    {
        //        Items = result,
        //        TotalItems = distributionQuery.TotalItems,
        //    };
        //}

        /// <summary>
        /// Xem danh sách phân phối
        /// </summary>
        public PagingResult<RstDistributionDto> FindAll(FilterRstDistributionDto input)
        {
            _logger.LogInformation($"{nameof(FindAll)}: input = {JsonSerializer.Serialize(input)}");

            var usertype = CommonUtils.GetCurrentUserType(_httpContext);
            int? tradingProviderId = null;
            if (usertype == UserTypes.TRADING_PROVIDER || usertype == UserTypes.ROOT_TRADING_PROVIDER)
            {
                tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            }
            int? partnerId = null;
            if (usertype == UserTypes.PARTNER || usertype == UserTypes.ROOT_PARTNER)
            {
                partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
            }

            List<RstDistributionDto> result = new();
            var distributionQuery = _rstDistributionEFRepository.FindAll(input, partnerId, tradingProviderId);

            foreach (var item in distributionQuery.Items)
            {
                var resultItem = new RstDistributionDto();
                resultItem = _mapper.Map<RstDistributionDto>(item);
                // Thông tin dự án
                var projectQuery = _rstProjectEFRepository.FindById(item.ProjectId);

                // Thông tin của đại lý
                var tradingProviderQuery = _tradingProviderEFRepository.GetById(item.TradingProviderId);

                var distributionProductItems = _rstDistributionProductItemEFRepository.GetAllByDistribution(item.Id).OrderByDescending(o => o.Id);

                // Lấy ra các sản phẩm trong phân phối đã được đại lý mở bán
                var openSellDetailQuery = from distributionItem in distributionProductItems
                                          join openSellDetail in _rstOpenSellDetailEFRepository.Entity on distributionItem.Id equals openSellDetail.DistributionProductItemId
                                          where openSellDetail.Deleted == YesNo.NO
                                          select openSellDetail;

                // Tìm thông tin người duyệt sản phẩm
                var approveBy = (from approve in _dbContext.RstApproves
                                 join user in _dbContext.Users on approve.UserApproveId equals user.UserId
                                 where approve.Status == ApproveStatus.DUYET && approve.Deleted == YesNo.NO
                                 && approve.DataType == RstApproveDataTypes.RST_DISTRIBUTION && approve.ReferId == item.Id
                                 select user.UserName).FirstOrDefault();

                resultItem.TradingProvider = tradingProviderQuery;
                resultItem.ApproveBy = approveBy;
                resultItem.Project = _mapper.Map<RstProjectDto>(projectQuery);

                result.Add(resultItem);
            }
            return new PagingResult<RstDistributionDto>
            {
                Items = result,
                TotalItems = distributionQuery.TotalItems,
            };
        }

        /// <summary>
        /// Tạm dừng phân phối (Đang phân phối - Tạm dừng)
        /// </summary>
        public void PauseDistribution(int id)
        {
            var partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            _logger.LogInformation($"{nameof(PauseDistribution)}: id = {id},  partnerId = {partnerId}, username = {username}");
            var transaction = _dbContext.Database.BeginTransaction();
            // Kiểm tra phân phối 
            var distributionQuery = _rstDistributionEFRepository.FindById(id, partnerId).ThrowIfNull(_dbContext, ErrorCode.RstDistributionNotFound);

            if (distributionQuery.Status == RstDistributionStatus.DANG_BAN)
            {
                // Kiểm tra xem có tồn tại sản phẩm mở bán của đại lý được phân phối đang cọc hay không
                // Nếu có không thể tạm dừng phân phối
                if (_rstDistributionProductItemEFRepository.CheckDistributionItem(id, null, new List<int> { RstProductItemStatus.DA_COC, RstProductItemStatus.GIU_CHO }).Any())
                {
                    _rstDistributionEFRepository.ThrowException(ErrorCode.RstDistributionCanNotPause);
                }
                distributionQuery.Status = RstDistributionStatus.TAM_DUNG;
            }
            else if (distributionQuery.Status == RstDistributionStatus.TAM_DUNG)
            {
                distributionQuery.Status = RstDistributionStatus.DANG_BAN;
                _rstHistoryUpdateEFRepository.Add(new RstHistoryUpdate(distributionQuery.Id, null, null, null, RstHistoryUpdateTables.RST_DISTRIBUTION, ActionTypes.CAP_NHAT, RstHistoryUpdateSummary.DISTRIBUTION, DateTime.Now, RstHistoryTypes.PhanPhoi), username);
            }
            distributionQuery.ModifiedBy = username;
            distributionQuery.ModifiedDate = DateTime.Now;
            _dbContext.SaveChanges();
            transaction.Commit();
        }

        /// <summary>
        /// Xóa phân phối
        /// </summary>
        public void DeleteDistribution(int id)
        {
            var partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            _logger.LogInformation($"{nameof(DeleteDistribution)}: id = {id},  partnerId = {partnerId}, username = {username}");
            var transaction = _dbContext.Database.BeginTransaction();
            // Kiểm tra phân phối 
            var distributionQuery = _rstDistributionEFRepository.FindById(id).ThrowIfNull(_dbContext, ErrorCode.RstDistributionNotFound);

            // Nếu có không thể xóa phân phối
            if (distributionQuery.Status != RstDistributionStatus.KHOI_TAO)
            {
                _rstDistributionEFRepository.ThrowException(ErrorCode.RstDistributionCanNotDelete);
            }
            distributionQuery.Deleted = YesNo.YES;
            distributionQuery.ModifiedBy = username;
            distributionQuery.ModifiedDate = DateTime.Now;
            _dbContext.SaveChanges();
            transaction.Commit();
        }

        #endregion

        #region Sản phẩm của phân phối
        /// <summary>
        /// Đối tác - Thêm sản phẩm cho phân phối 
        /// </summary>
        public void AddDistributionProductItem(CreateRstDistributionProductItemDto input)
        {
            var partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            _logger.LogInformation($"{nameof(AddDistributionProductItem)}: input = {JsonSerializer.Serialize(input.ProductItemIds)},  partnerId = {partnerId}, username = {username}");
            var distributionQuery = _rstDistributionEFRepository.FindById(input.DistributionId).ThrowIfNull(_dbContext, ErrorCode.RstDistributionNotFound);
            var transaction = _dbContext.Database.BeginTransaction();
            foreach (var productItem in input.ProductItemIds)
            {
                var checkProductItemExist = from project in _dbContext.RstProjects
                                            join projectItem in _dbContext.RstProductItems on project.Id equals projectItem.ProjectId
                                            join distribution in _dbContext.RstDistributions on project.Id equals distribution.ProjectId
                                            join distributionProductItem in _dbContext.RstDistributionProductItems on distribution.Id equals distributionProductItem.DistributionId
                                            where project.Deleted == YesNo.NO && projectItem.Deleted == YesNo.NO && distribution.Deleted == YesNo.NO && distributionProductItem.Deleted == YesNo.NO
                                            && distribution.Status != RstDistributionStatus.HUY_DUYET && distributionProductItem.ProductItemId == productItem
                                            && distribution.ProjectId == distributionQuery.ProjectId
                                            && distribution.TradingProviderId == distributionQuery.TradingProviderId
                                            select distributionProductItem;
                // Nếu căn hộ đã có trong phân phối nào đó của đại lý thì ko cho thêm tiếp
                if (checkProductItemExist.Any())
                {
                    _rstDistributionProductItemEFRepository.ThrowException(ErrorCode.RstProductItemExistInDistributionOfTradingProvider);
                }

                var productItemQuery = _rstProductItemEFRepository.FindById(productItem, partnerId).ThrowIfNull(_dbContext, ErrorCode.RstProductItemNotFound, productItem);
                _rstDistributionProductItemEFRepository.Add(new RstDistributionProductItem
                {
                    DistributionId = input.DistributionId,
                    ProductItemId = productItem,
                    CreatedBy = username,
                });
            }
            _dbContext.SaveChanges();
            transaction.Commit();
        }

        /// <summary>
        /// Xóa nhiều sản phẩm của phân phối
        /// </summary>
        public void DeleteDistributionProductItem(DeleteRstDistributionProductDto input)
        {
            var partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            _logger.LogInformation($"{nameof(DeleteDistributionProductItem)}: input = {JsonSerializer.Serialize(input.DistributionItemIds)},  partnerId = {partnerId}, username = {username}");
            var transaction = _dbContext.Database.BeginTransaction();
            var distribution = _rstDistributionEFRepository.FindById(input.Id, partnerId).ThrowIfNull(_dbContext, ErrorCode.RstDistributionNotFound, input.Id);
            foreach (var item in input.DistributionItemIds)
            {
                // Nếu sản phẩm được đại lý đã bán hoặc đã cọc thì không được xóa sản phẩm
                if (_rstDistributionProductItemEFRepository.CheckDistributionItem(input.Id, item, new List<int> { RstProductItemStatus.DA_COC, RstProductItemStatus.DA_BAN }).Any())
                {
                    _rstDistributionEFRepository.ThrowException(ErrorCode.RstDistributionItemCanNotDelete);
                }

                var distributionProductItem = _rstDistributionProductItemEFRepository.FindById(item).ThrowIfNull(_dbContext, ErrorCode.RstDistributionProductItemNotFound, item);
                distributionProductItem.Deleted = YesNo.YES;
                distributionProductItem.ModifiedBy = username;
                distributionProductItem.ModifiedDate = DateTime.Now;
            }
            _dbContext.SaveChanges();
            transaction.Commit();
        }

        /// <summary>
        /// Đối tác - Khóa sản phẩm của phân phối
        /// </summary>
        public void LockDistributionProductItem(LockRstDistributionProductItemDto input)
        {
            var partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            _logger.LogInformation($"{nameof(LockDistributionProductItem)}: distributionProductItemId = {input.Id},  partnerId = {partnerId}, username = {username}");
            var transaction = _dbContext.Database.BeginTransaction();
            var distributionProductItemQuery = _rstDistributionProductItemEFRepository.EntityNoTracking.FirstOrDefault(d => d.Id == input.Id && d.Deleted == YesNo.NO)
                                                .ThrowIfNull(_dbContext, ErrorCode.RstDistributionProductItemNotFound);
            var distributionAfterUpdate = _rstDistributionProductItemEFRepository.LockDistributionItem(input.Id, partnerId, username);
            if (distributionProductItemQuery.Status == Status.ACTIVE)
            {
                _rstHistoryUpdateEFRepository.Add(new RstHistoryUpdate
                {
                    RealTableId = input.Id,
                    OldValue = distributionProductItemQuery.Status,
                    NewValue = distributionAfterUpdate.Status,
                    FieldName = RstFieldName.UPDATE_STATUS,
                    UpdateTable = RstHistoryUpdateTables.RST_DISTRIBUTION_PRODUCT_ITEM,
                    Action = ActionTypes.CAP_NHAT,
                    ActionUpdateType = RstActionUpdateTypes.STATUS_LOCK_STRING,
                    UpdateReason = input.LockingReason,
                    Summary = input.Summary,
                    CreatedDate = DateTime.Now,
                }, username);
            }
            _dbContext.SaveChanges();
            transaction.Commit();
        }

        public PagingResult<RstDistributionProductItemDto> FindAllItemByDistribution(int distributionId, FilterRstDistributionProductItemDto input)
        {
            var usertype = CommonUtils.GetCurrentUserType(_httpContext);
            int? partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
            _logger.LogInformation($"{nameof(FindAllItemByDistribution)}: distributionId = {distributionId}, input = {JsonSerializer.Serialize(input)}, partnerId = {partnerId}");
            List<RstDistributionProductItemDto> result = new();
            var distributionQuery = _rstDistributionEFRepository.FindById(distributionId).ThrowIfNull(_dbContext, ErrorCode.RstDistributionNotFound, distributionId);
            var distributionProductItemQuery = _rstDistributionProductItemEFRepository.GetAllByDistribution(distributionId);

            decimal totalItems = distributionProductItemQuery.Count();
            distributionProductItemQuery = distributionProductItemQuery.OrderDynamic(input.Sort);
            if (input.PageSize != -1)
            {
                distributionProductItemQuery = distributionProductItemQuery.Skip(input.Skip).Take(input.PageSize);
            }

            foreach (var item in distributionProductItemQuery)
            {
                var resultItem = new RstDistributionProductItemDto();
                resultItem = _mapper.Map<RstDistributionProductItemDto>(item);
                bool isOfTrading = false;
                List<int> listStatusTrading = new() { RstProductItemStatus.DA_COC, RstProductItemStatus.DA_BAN, RstProductItemStatus.KHOA_CAN };
                var productItemQuery = _rstProductItemEFRepository.FindById(item.ProductItemId);
                var productItemPrice = _rstProductItemEFRepository.ProductItemPriceByDistribution(productItemQuery?.Price ?? 0, distributionId);
                var productItemStatus = productItemQuery.Status;

                var openSellDetailCheck = (from openSell in _dbContext.RstOpenSells
                                           join openSellDetail in _dbContext.RstOpenSellDetails on openSell.Id equals openSellDetail.OpenSellId
                                           where openSell.Deleted == YesNo.NO && openSellDetail.Deleted == YesNo.NO
                                           && openSell.Status != RstDistributionStatus.HUY_DUYET
                                           && openSellDetail.DistributionProductItemId == item.Id
                                           select openSellDetail);
                if (productItemStatus == RstProductItemStatus.KHOI_TAO)
                {
                    // Kiểm tra xem sản phẩm phân phối đã được đại lý cho mở bán hay chưa
                    productItemStatus = openSellDetailCheck.Any() ? RstProductItemStatus.LOGIC_DANG_MO_BAN : RstProductItemStatus.LOGIC_CHUA_MO_BAN;
                }
                else
                {
                    // Nếu hợp đồng đã được cọc thì kiểm tra xem có phải của nó hay không
                    isOfTrading = (listStatusTrading.Contains(productItemStatus) && openSellDetailCheck.Any(r => listStatusTrading.Contains(r.Status)));
                }

                resultItem.Status = item.Status;
                resultItem.LockPrice = productItemPrice.LockPrice;
                resultItem.DepositPrice = productItemPrice.DepositPrice;
                resultItem.ProductItemStatus = productItemStatus;
                resultItem.IsProductItemStatusOfTrading = isOfTrading;
                resultItem.ProductItem = _mapper.Map<RstProductItemDto>(productItemQuery);
                result.Add(resultItem);
                //result.Add(new RstDistributionProductItemDto
                //{
                //    Id = item.Id,
                //    Status = item.Status,
                //    LockPrice = productItemPrice.LockPrice,
                //    DepositPrice = productItemPrice.DepositPrice,
                //    ProductItemStatus = productItemStatus,
                //    IsProductItemStatusOfTrading = isOfTrading,
                //    ProductItem = _mapper.Map<RstProductItemDto>(productItemQuery)
                //});
            }
            //lọc theo trạng thái
            result = result.Where(r => input.Status == null || r.ProductItemStatus == input.Status).ToList();
            return new PagingResult<RstDistributionProductItemDto>
            {
                Items = result,
                TotalItems = totalItems,
            };
        }

        /// <summary>
        /// Xem chi tiet san pham cua phan phoi
        /// </summary>
        public RstDistributionProductItemDto FindDistributionItemById(int distributionProductItemId)
        {
            _logger.LogInformation($"{nameof(FindDistributionItemById)}: distributionProductItemId = {distributionProductItemId}");
            RstDistributionProductItemDto result = new();
            var distributionProductItemQuery = _rstDistributionProductItemEFRepository.FindById(distributionProductItemId).ThrowIfNull(_dbContext, ErrorCode.RstDistributionNotFound);
            if (distributionProductItemQuery != null)
            {
                var productItemQuery = _rstProductItemEFRepository.FindById(distributionProductItemQuery.ProductItemId);
                var productItemPrice = _rstProductItemEFRepository.ProductItemPriceByDistribution(productItemQuery.Price ?? 0, distributionProductItemQuery.DistributionId);
                result.DepositPrice = productItemPrice.DepositPrice;
                result.LockPrice = productItemPrice.LockPrice;
                result.Id = distributionProductItemQuery.Id;
                result.Status = distributionProductItemQuery.Status;
                result.ProductItem = _mapper.Map<RstProductItemDto>(productItemQuery);
            }
            return result;
        }

        /// <summary>
        /// Danh sách sản phẩm được phân phối theo dự án cho đại lý có thể thêm vào mở bán
        /// </summary>
        /// <returns></returns>
        public List<RstDistributionProductItemDto> GetAllProductItemByTrading(FilterRstDistributionProductItemByTradingDto input)
        {
            int tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            _logger.LogInformation($"{nameof(GetAllProductItemByTrading)}: input = {JsonSerializer.Serialize(input)}, tradingProviderId = {tradingProviderId}");
            List<RstDistributionProductItemDto> result = new();
            var distributionProductItemQuery = _rstDistributionProductItemEFRepository.GetAllProductItemByTrading(input, tradingProviderId);

            foreach (var item in distributionProductItemQuery)
            {
                var productItemQuery = _rstProductItemEFRepository.FindById(item.ProductItemId);
                var productItemPrice = _rstProductItemEFRepository.ProductItemPriceByDistribution(productItemQuery.Price ?? 0, item.DistributionId);
                var productItemDto = _mapper.Map<RstProductItemDto>(productItemQuery);
                productItemDto.ProjectStructure = _mapper.Map<RstProjectStructureDto>(_rstProjectStructureEFRepository.FindById(productItemQuery.BuildingDensityId ?? 0));

                result.Add(new RstDistributionProductItemDto
                {
                    Id = item.Id,
                    Status = item.Status,
                    LockPrice = productItemPrice.LockPrice,
                    DepositPrice = productItemPrice.DepositPrice,
                    ProductItem = productItemDto,
                    CreatedDate = item.CreatedDate
                });
            }
            return result;
        }

        public List<RstDistributionByTradingDto> GetAllByTrading()
        {
            int tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            _logger.LogInformation($"{nameof(GetAllByTrading)}: tradingProviderId = {tradingProviderId}");
            var distributionProductItemQuery = _dbContext.RstDistributions.Where(e => e.TradingProviderId == tradingProviderId && e.Deleted == YesNo.NO)
                .Select(distribution => new RstDistributionByTradingDto
                {
                    Id = distribution.Id,
                    StartDate = distribution.StartDate,
                    EndDate = distribution.EndDate,
                    Description = distribution.Description,
                    Project = new RstProjectDistributionDto
                    {
                        Id = distribution.Project.Id,
                        Name = distribution.Project.Name,
                        Code = distribution.Project.Code,
                    }
                }).OrderByDescending(e => e.StartDate);

            return distributionProductItemQuery.ToList();
        }
        #endregion

        #region Đối tác - Trình duyệt phân phối sản phẩm
        /// <summary>
        /// Yêu cầu trình duyệt
        /// </summary>
        public void Request(RstRequestDto input)
        {
            var partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
            var userId = CommonUtils.GetCurrentUserId(_httpContext);
            var username = CommonUtils.GetCurrentUsername(_httpContext);

            _logger.LogInformation($"{nameof(Request)}: input = {JsonSerializer.Serialize(input)}, partnerId = {partnerId}, username = {username}");

            var actionType = ActionTypes.THEM_MOI;

            //Tìm kiếm thông tin phân phối
            var distributionQuery = _rstDistributionEFRepository.FindById(input.Id).ThrowIfNull(_dbContext, ErrorCode.RstDistributionNotFound);

            // Status = 1 : trình duyệt với, Status = 4 trình duyệt lại dự án
            if (distributionQuery.Status == DistributionStatus.KHOI_TAO || distributionQuery.Status == DistributionStatus.HUY_DUYET)
            {
                //Nếu đã tồn tại bản ghi trước đó
                var findRequest = _rstApproveEFRepository.FindByIdOfDataType(input.Id, RstApproveDataTypes.RST_DISTRIBUTION);
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
                    DataType = RstApproveDataTypes.RST_DISTRIBUTION,
                    ReferId = input.Id,
                    Summary = input.Summary,
                    CreatedBy = username,
                    TradingProviderId = null,
                    PartnerId = partnerId
                });

                distributionQuery.Status = DistributionStatus.CHO_DUYET;
                request.DataStatus = DistributionStatus.CHO_DUYET;
            }
            _dbContext.SaveChanges();
        }

        /// <summary>
        /// Duyệt phân phối
        /// </summary>
        public void Approve(RstApproveDto input)
        {
            var userId = CommonUtils.GetCurrentUserId(_httpContext);
            _logger.LogInformation($"{nameof(Approve)}: input = {JsonSerializer.Serialize(input)}, userId = {userId}");
            //Tìm kiếm thông tin dự án
            var distributionQuery = _rstDistributionEFRepository.FindById(input.Id).ThrowIfNull(_dbContext, ErrorCode.RstDistributionNotFound);

            if (distributionQuery.Status == DistributionStatus.CHO_DUYET)
            {
                var findRequest = _rstApproveEFRepository.FindByIdOfDataType(input.Id, RstApproveDataTypes.RST_DISTRIBUTION).ThrowIfNull(_dbContext, ErrorCode.RstApproveNotFound);

                _rstApproveEFRepository.Approve(new RstApprove
                {
                    Id = findRequest.Id,
                    ApproveNote = input.ApproveNote,
                    UserApproveId = userId,
                    ApproveIp = CommonUtils.GetCurrentRemoteIpAddress(_httpContext)
                });

                distributionQuery.Status = DistributionStatus.HOAT_DONG;
            }
            _dbContext.SaveChanges();
        }

        /// <summary>
        /// Huy duyet phân phối
        /// </summary>
        public void Cancel(RstCancelDto input)
        {
            _logger.LogInformation($"{nameof(Cancel)}: input = {JsonSerializer.Serialize(input)}");

            //Tìm kiếm thông tin dự án
            var distributionQuery = _rstDistributionEFRepository.FindById(input.Id).ThrowIfNull(_dbContext, ErrorCode.RstDistributionNotFound);

            var findRequest = _rstApproveEFRepository.FindByIdOfDataType(input.Id, RstApproveDataTypes.RST_DISTRIBUTION).ThrowIfNull(_dbContext, ErrorCode.RstApproveNotFound);
            _rstApproveEFRepository.Cancel(new RstApprove
            {
                Id = findRequest.Id,
                CancelNote = input.CancelNote
            });
            //Cập nhật trạng thái cho phân phối
            distributionQuery.Status = RstDistributionStatus.HUY_DUYET;
            _dbContext.SaveChanges();
        }
        #endregion
    }
}
