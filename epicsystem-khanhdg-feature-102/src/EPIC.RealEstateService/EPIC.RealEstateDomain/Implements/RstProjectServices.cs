using AutoMapper;
using DocumentFormat.OpenXml.Office2010.Excel;
using EPIC.CoreRepositories;
using EPIC.CoreSharedServices.Implements;
using EPIC.CoreSharedServices.Interfaces;
using EPIC.DataAccess.Base;
using EPIC.DataAccess.Models;
using EPIC.Entities;
using EPIC.Entities.Dto.Sale;
using EPIC.InvestEntities.DataEntities;
using EPIC.RealEstateDomain.Interfaces;
using EPIC.RealEstateEntities.DataEntities;
using EPIC.RealEstateEntities.Dto.RstApprove;
using EPIC.RealEstateEntities.Dto.RstOpenSell;
using EPIC.RealEstateEntities.Dto.RstOwner;
using EPIC.RealEstateEntities.Dto.RstProject;
using EPIC.RealEstateEntities.Dto.RstProjectExtend;
using EPIC.RealEstateEntities.Dto.RstProjectFile;
using EPIC.RealEstateEntities.Dto.RstProjectMedia;
using EPIC.RealEstateEntities.Dto.RstProjectMediaDetail;
using EPIC.RealEstateEntities.Dto.RstProjectInformationShare;
using EPIC.RealEstateEntities.Dto.RstProjectUtilityMedia;
using EPIC.RealEstateRepositories;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Identity;
using EPIC.Utils.ConstantVariables.RealEstate;
using EPIC.Utils.ConstantVariables.Shared;
using EPIC.Utils.DataUtils;
using Humanizer;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MySqlX.XDevAPI.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using EPIC.RealEstateEntities.Dto.RstProductItem;

namespace EPIC.RealEstateDomain.Implements
{
    public class RstProjectServices : IRstProjectServices
    {
        private readonly EpicSchemaDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<RstProjectServices> _logger;
        private readonly string _connectionString;
        private readonly IHttpContextAccessor _httpContext;
        private readonly ILogicInvestorTradingSharedServices _logicInvestorTradingSharedServices;
        private readonly RstProjectEFRepository _rstProjectEFRepository;
        private readonly RstProjectTypeEFRepository _rstProjectTypeEFRepository;
        private readonly RstProjectMediaEFRepository _rstProjectMediaEFRepository;
        private readonly RstProjectGuaranteeBankEFRepository _rstProjectGuaranteeBankRepository;
        private readonly RstProjectFileEFRepository _rstProjectFileEFRepository;
        private readonly RstOpenSellFileEFRepository _rstOpenSellFileEFRepository;
        private readonly RstProjectMediaDetailEFRepository _rstProjectMediaDetailEFRepository;
        private readonly TradingProviderEFRepository _tradingProviderEFRepository;
        private readonly BusinessCustomerEFRepository _businessCustomerEFRepository;
        private readonly RstApproveEFRepository _rstApproveEFRepository;
        private readonly RstOwnerEFRepository _rstOwnerEFRepository;
        private readonly SaleEFRepository _saleEFRepository;
        private readonly InvestorSaleEFRepository _investorSaleEFRepository;
        private readonly RstProjectExtendEFRepository _rstProjectExtendEFRepository;
        private readonly RstOpenSellEFRepository _rstOpenSellEFRepository;
        private readonly RstProductItemEFRepository _rstProductItemEFRepository;
        private readonly PartnerRepository _partnerRepository;
        private readonly RstProjectUtilityEFRepository _rstProjectUtilityEFRepository;
        private readonly RstProjectGuaranteeBankEFRepository _rstProjectGuaranteeBankEFRepository;
        private readonly RstProjectFavouriteEFRepository _rstProjectFavouriteEFRepository;
        private readonly RstOpenSellInterestEFRepository _rstOpenSellInterestEFRepository;
        private readonly RstProjectUtilityExtendEFRepository _rstProjectUtilityExtendEFRepository;

        public RstProjectServices(EpicSchemaDbContext dbContext,
            DatabaseOptions databaseOptions,
            IMapper mapper,
            ILogger<RstProjectServices> logger,
            IHttpContextAccessor httpContextAccessor,
            ILogicInvestorTradingSharedServices logicInvestorTradingSharedServices)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
            _connectionString = databaseOptions.ConnectionString;
            _httpContext = httpContextAccessor;
            _logicInvestorTradingSharedServices = logicInvestorTradingSharedServices;
            _rstProjectEFRepository = new RstProjectEFRepository(dbContext, logger);
            _rstProjectTypeEFRepository = new RstProjectTypeEFRepository(dbContext, logger);
            _rstProjectGuaranteeBankRepository = new RstProjectGuaranteeBankEFRepository(dbContext, logger);
            _tradingProviderEFRepository = new TradingProviderEFRepository(dbContext, logger);
            _rstProjectMediaEFRepository = new RstProjectMediaEFRepository(dbContext, logger);
            _businessCustomerEFRepository = new BusinessCustomerEFRepository(dbContext, logger);
            _rstProjectFileEFRepository = new RstProjectFileEFRepository(dbContext, logger);
            _rstOpenSellFileEFRepository = new RstOpenSellFileEFRepository(dbContext, logger);
            _rstProjectMediaDetailEFRepository = new RstProjectMediaDetailEFRepository(dbContext, logger);
            _rstApproveEFRepository = new RstApproveEFRepository(dbContext, logger);
            _rstOwnerEFRepository = new RstOwnerEFRepository(dbContext, logger);
            _saleEFRepository = new SaleEFRepository(dbContext, logger);
            _rstProjectFavouriteEFRepository = new RstProjectFavouriteEFRepository(dbContext, logger);
            _investorSaleEFRepository = new InvestorSaleEFRepository(dbContext, logger);
            _rstProjectExtendEFRepository = new RstProjectExtendEFRepository(dbContext, logger);
            _rstOpenSellEFRepository = new RstOpenSellEFRepository(dbContext, logger);
            _rstProductItemEFRepository = new RstProductItemEFRepository(dbContext, logger);
            _partnerRepository = new PartnerRepository(_connectionString, _logger);
            _rstProjectUtilityEFRepository = new RstProjectUtilityEFRepository(dbContext, logger);
            _rstProjectGuaranteeBankEFRepository = new RstProjectGuaranteeBankEFRepository(dbContext, logger);
            _rstOpenSellInterestEFRepository = new RstOpenSellInterestEFRepository(dbContext, logger);
            _rstProjectUtilityExtendEFRepository = new RstProjectUtilityExtendEFRepository(dbContext, logger);
        }

        #region CRUD
        /// <summary>
        /// Thêm mới dự án
        /// </summary>
        /// <param name="input"></param>
        public RstProjectDto Add(RstCreateProjectDto input)
        {
            var partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            _logger.LogInformation($"{nameof(Add)} : input = {JsonSerializer.Serialize(input)}, partnerId={partnerId}, username={username}");
            RstProject inputInsert = _mapper.Map<RstProject>(input);
            inputInsert.PartnerId = partnerId;
            inputInsert.CreatedBy = username;

            var transaction = _dbContext.Database.BeginTransaction();
            var ownerFind = _rstOwnerEFRepository.FindById(input.OwnerId).ThrowIfNull(_dbContext, ErrorCode.RstOwnerNotFound);
            var projectInsert = _rstProjectEFRepository.Add(inputInsert);
            _dbContext.SaveChanges();
            _rstProjectTypeEFRepository.UpdateProjectTypes(projectInsert.Id, RstProjectDetailTypes.ProductTypes, input.ProductTypes);
            _rstProjectTypeEFRepository.UpdateProjectTypes(projectInsert.Id, RstProjectDetailTypes.DistributionTypes, input.DistributionTypes);
            //_rstProjectGuaranteeBankRepository.UpdateGuaranteeBanks(projectInsert.Id, input.GuaranteeBanks);
            _rstProjectExtendEFRepository.UpdateProjectExtends(projectInsert.Id, _mapper.Map<List<RstProjectExtend>>(input.ProjectExtends), username);
            _dbContext.SaveChanges();
            transaction.Commit();
            return _mapper.Map<RstProjectDto>(projectInsert);
        }

        /// <summary>
        /// Cập nhật dự án
        /// </summary>
        /// <param name="input"></param>
        public RstProjectDto Update(RstUpdateProjectDto input)
        {
            var partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            _logger.LogInformation($"{nameof(Update)} : input = {JsonSerializer.Serialize(input)}, partnerId={partnerId}, username={username}");
            RstProject inputUpdate = _mapper.Map<RstProject>(input);
            inputUpdate.PartnerId = partnerId;
            inputUpdate.ModifiedBy = username;

            var transaction = _dbContext.Database.BeginTransaction();
            var ownerFind = _rstOwnerEFRepository.FindById(input.OwnerId).ThrowIfNull(_dbContext, ErrorCode.RstOwnerNotFound);
            _rstProjectEFRepository.Update(inputUpdate);
            _dbContext.SaveChanges();
            _rstProjectTypeEFRepository.UpdateProjectTypes(input.Id, RstProjectDetailTypes.ProductTypes, input.ProductTypes);
            _rstProjectTypeEFRepository.UpdateProjectTypes(input.Id, RstProjectDetailTypes.DistributionTypes, input.DistributionTypes);
            _rstProjectGuaranteeBankRepository.UpdateGuaranteeBanks(input.Id, input.GuaranteeBanks);
            _rstProjectExtendEFRepository.UpdateProjectExtends(input.Id, _mapper.Map<List<RstProjectExtend>>(input.ProjectExtends), username);
            _dbContext.SaveChanges();
            transaction.Commit();
            return _mapper.Map<RstProjectDto>(inputUpdate);
        }

        /// <summary>
        /// Cập nhật Mô tả tổng quan dự án
        /// </summary>
        public void UpdateProjectOverviewContent(RstUpdateProjectOverviewContentDto input)
        {
            var partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            _logger.LogInformation($"{nameof(UpdateProjectOverviewContent)} : input = {JsonSerializer.Serialize(input)}, partnerId={partnerId}, username={username}");

            var projectFind = _rstProjectEFRepository.FindById(input.Id, partnerId).ThrowIfNull(_dbContext, ErrorCode.RstProjectNotFound);
            projectFind.OverviewContent = input.OverviewContent;
            projectFind.ContentType = input.ContentType;
            _dbContext.SaveChanges();
        }

        /// <summary>
        /// Tìm kiếm thông tin dự án
        /// </summary>
        public RstProjectDto FindById(int id)
        {
            var partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            _logger.LogInformation($"{nameof(FindById)} : id = {id}, partnerId={partnerId}, username={username}");
            RstProjectDto result = new();

            var projectFind = _rstProjectEFRepository.FindById(id, partnerId).ThrowIfNull(_dbContext, ErrorCode.RstProjectNotFound);
            var ownerFind = _rstOwnerEFRepository.FindById(projectFind.OwnerId).ThrowIfNull(_dbContext, ErrorCode.RstOwnerNotFound);
            var businessCustomer = _businessCustomerEFRepository.FindById(ownerFind.BusinessCustomerId);

            result = _mapper.Map<RstProjectDto>(projectFind);
            result.ProductTypes = _rstProjectTypeEFRepository.FindAllByProjectId(id, RstProjectDetailTypes.ProductTypes);
            result.DistributionTypes = _rstProjectTypeEFRepository.FindAllByProjectId(id, RstProjectDetailTypes.DistributionTypes);
            result.GuaranteeBanks = _rstProjectGuaranteeBankRepository.EntityNoTracking.Where(r => r.ProjectId == id).Select(b => b.BankId).ToList();
            // Lấy thêm thông tin mở rộng khác
            result.ProjectExtends = _mapper.Map<List<RstProjectExtendDto>>(_rstProjectExtendEFRepository.GetAll(id));
            // Thông tin chủ đầu tư
            ViewRstOwnerDto owner = _mapper.Map<ViewRstOwnerDto>(ownerFind);
            owner.BusinessCustomer = businessCustomer;
            result.Owner = owner;
            return result;
        }

        //public PagingResult<ViewRstProjectDto> FindAll(FilterRstProjectDto input)
        //{
        //    _logger.LogInformation($"{nameof(FindAll)}: input = {JsonSerializer.Serialize(input)}");

        //    int partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);

        //    var resultPaging = new PagingResult<ViewRstProjectDto>();
        //    var result = new List<ViewRstProjectDto>();
        //    var projectQuery = _rstProjectEFRepository.FindAll(input, partnerId);
        //    foreach (var item in projectQuery.Items)
        //    {
        //        var resultItem = new ViewRstProjectDto();

        //        resultItem = _mapper.Map<ViewRstProjectDto>(item);
        //        //Loại hình dự án
        //        resultItem.ProductTypes = _rstProjectTypeEFRepository.FindAllByProjectId(item.Id, RstProjectDetailTypes.ProductTypes);
        //        //Loại hình phân phối
        //        resultItem.DistributionTypes = _rstProjectTypeEFRepository.FindAllByProjectId(item.Id, RstProjectDetailTypes.DistributionTypes);
        //        //Ngân hàng đảm bảo
        //        resultItem.GuaranteeBanks = _rstProjectGuaranteeBankRepository.EntityNoTracking.Where(r => r.ProjectId == item.Id).Select(b => b.BankId).ToList();

        //        var productItemQuery = _rstProductItemEFRepository.Entity.Where(p => p.ProjectId == item.Id && p.Deleted == YesNo.NO);
        //        // Số lượng căn hộ đã được phân phối
        //        var distributionQuantity = (from productItem in _rstProductItemEFRepository.Entity
        //                                    join distributionProductItem in _dbContext.RstDistributionProductItems on productItem.Id equals distributionProductItem.ProductItemId
        //                                    join distribution in _dbContext.RstDistributions on distributionProductItem.DistributionId equals distribution.Id
        //                                    where productItem.Deleted == YesNo.NO && distributionProductItem.Deleted == YesNo.NO && distribution.Deleted == YesNo.NO
        //                                    && productItem.ProjectId == item.Id && distribution.Status != RstDistributionStatus.HUY_DUYET
        //                                    select new { productItem.Id }).Distinct().Count();

        //        resultItem.TotalQuantity = productItemQuery.Count();
        //        resultItem.SoldQuantity = productItemQuery.Count(r => r.Status == RstProductItemStatus.DA_BAN);
        //        resultItem.RemainingQuantity = resultItem.TotalQuantity - resultItem.SoldQuantity;
        //        resultItem.DistributionQuantity = distributionQuantity;

        //        // Thông tin chủ đầu tư
        //        var ownerFind = _rstOwnerEFRepository.FindById(item.OwnerId);
        //        if (ownerFind != null)
        //        {
        //            var businessCustomer = _businessCustomerEFRepository.FindById(ownerFind.BusinessCustomerId);
        //            var businessCustomerBanks = _businessCustomerEFRepository.GetListBankByBusinessCustomerId(ownerFind.BusinessCustomerId);
        //            if (businessCustomer != null)
        //            {
        //                ViewRstOwnerDto owner = _mapper.Map<ViewRstOwnerDto>(ownerFind);
        //                owner.BusinessCustomer = businessCustomer;
        //                resultItem.Owner = owner;
        //                resultItem.OwnerName = businessCustomer.Name;
        //                resultItem.Owner.BusinessCustomerBanks = businessCustomerBanks;
        //            }
        //        }
        //        result.Add(resultItem);
        //    }

        //    resultPaging.Items = result;
        //    resultPaging.TotalItems = projectQuery.TotalItems;
        //    return resultPaging;
        //}

        public PagingResult<ViewRstProjectDto> FindAll(FilterRstProjectDto input)
        {
            _logger.LogInformation($"{nameof(FindAll)}: input = {JsonSerializer.Serialize(input)}");

            int partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);

            var resultPaging = new PagingResult<ViewRstProjectDto>();
            var result = new List<ViewRstProjectDto>();
            var projectQuery = _rstProjectEFRepository.FindAll(input, partnerId);
            foreach (var item in projectQuery.Items)
            {
                var resultItem = new ViewRstProjectDto();

                resultItem = _mapper.Map<ViewRstProjectDto>(item);
                //Loại hình dự án
                resultItem.ProductTypes = _rstProjectTypeEFRepository.FindAllByProjectId(item.Id, RstProjectDetailTypes.ProductTypes);
                //Loại hình phân phối
                resultItem.DistributionTypes = _rstProjectTypeEFRepository.FindAllByProjectId(item.Id, RstProjectDetailTypes.DistributionTypes);
                //Ngân hàng đảm bảo
                resultItem.GuaranteeBanks = _rstProjectGuaranteeBankRepository.EntityNoTracking.Where(r => r.ProjectId == item.Id).Select(b => b.BankId).ToList();

                // Thông tin chủ đầu tư
                var ownerFind = _rstOwnerEFRepository.FindById(item.OwnerId);
                if (ownerFind != null)
                {
                    var businessCustomer = _businessCustomerEFRepository.FindById(ownerFind.BusinessCustomerId);
                    var businessCustomerBanks = _businessCustomerEFRepository.GetListBankByBusinessCustomerId(ownerFind.BusinessCustomerId);
                    if (businessCustomer != null)
                    {
                        ViewRstOwnerDto owner = _mapper.Map<ViewRstOwnerDto>(ownerFind);
                        owner.BusinessCustomer = businessCustomer;
                        resultItem.Owner = owner;
                        resultItem.OwnerName = businessCustomer.Name;
                        resultItem.Owner.BusinessCustomerBanks = businessCustomerBanks.ToList();
                    }
                }
                result.Add(resultItem);
            }

            resultPaging.Items = result;
            resultPaging.TotalItems = projectQuery.TotalItems;
            return resultPaging;
        }

        /// <summary>
        /// Lấy danh sách dự án được phân phối
        /// </summary>
        public List<RstProjectDto> ProjectGetAll(List<int> tradingProviderIds)
        {
            //Thoong tin đại lý, đối tác
            var userType = CommonUtils.GetCurrentUserType(_httpContext);
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            int? tradingProviderId = null;
            int? partnerId = null;

            if (userType == UserTypes.PARTNER || userType == UserTypes.ROOT_PARTNER)
            {
                partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
            }
            else if (userType == UserTypes.TRADING_PROVIDER || userType == UserTypes.ROOT_TRADING_PROVIDER)
            {
                tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            }
            _logger.LogInformation($"{nameof(ProjectGetAll)} : tradingProviderId={tradingProviderId}, partnerId = {partnerId}, username={username}");

            var projectFind =  (from project in _dbContext.RstProjects
                                join distribution in _dbContext.RstDistributions on project.Id equals distribution.ProjectId
                                where project.Deleted == YesNo.NO && distribution.Deleted == YesNo.NO
                                && distribution.Status == RstDistributionStatus.DANG_BAN //&& project.Status == RstDistributionStatus.DANG_BAN 
                                && (partnerId == null || (distribution.PartnerId == partnerId && (tradingProviderIds.Count() == 0 || tradingProviderIds.Contains(distribution.TradingProviderId))))
                                && (tradingProviderId == null || distribution.TradingProviderId == tradingProviderId)
                                select project).ToList();

            return _mapper.Map<List<RstProjectDto>>(projectFind.Distinct());
        }

        /// <summary>
        /// Xóa dự án
        /// </summary>
        /// <param name="projectId"></param>
        public void Delete(int projectId)
        {
            var partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            _logger.LogInformation($"{nameof(Update)} : projectId = {projectId}, partnerId={partnerId}, username={username}");

            var projectFind = _rstProjectEFRepository.FindById(projectId, partnerId)
                .ThrowIfNull(_dbContext, ErrorCode.RstProjectNotFound);
            if (projectFind.Status != RstDistributionStatus.KHOI_TAO) 
            {
                _rstProjectEFRepository.ThrowException(ErrorCode.RstProjectCanNotDelete);
            }
            projectFind.Deleted = YesNo.YES;
            projectFind.ModifiedBy = username;
            projectFind.ModifiedDate = DateTime.Now;

            var productItem = _dbContext.RstProductItems.Where(p => p.ProjectId == projectId && p.Deleted == YesNo.NO);
            foreach (var item in productItem)
            {
                item.Deleted = YesNo.YES;
                item.ModifiedBy = username;
                item.ModifiedDate = DateTime.Now;
            }
            _dbContext.SaveChanges();
        }
        #endregion

        #region App
        /// <summary>
        /// App xem danh sách dự án
        /// </summary>
        /// <returns></returns>
        public List<AppViewListProjectDto> AppFindProjects(AppFindProjectDto dto)
        {
            int? investorId = null;
            try
            {
                investorId = CommonUtils.GetCurrentInvestorId(_httpContext);
            }
            catch
            {
                investorId = null;
            }

            _logger.LogInformation($"{nameof(AppFindProjects)}: dto = {JsonSerializer.Serialize(dto)}, investorId = {investorId}");

            var listTradingIds = _logicInvestorTradingSharedServices.FindListTradingProviderForApp(investorId, dto.IsSaleView);
            var listOpenProject = _rstProjectEFRepository.AppFindOpenSellProject(dto, listTradingIds.ToList(), investorId).ToList();
            foreach (var project in listOpenProject)
            {
                var projectQuery = _rstProjectEFRepository.FindById(project.Id);
                if (projectQuery == null)
                {
                    continue;
                }
                var media = _rstProjectMediaEFRepository.AppFindBannerQuangCaoById(project.Id);
                project.Media = _mapper.Map<List<AppFindProjectMediaDto>>(media);

                // Lấy thông tin hình ảnh của dự án
                project.ProjectLogoUrl = _rstProjectMediaEFRepository.GetByLocation(project.Id, RstMediaLocations.LOGO_DU_AN)?.UrlImage;
                project.ProjectAvatarUrl = _rstProjectMediaEFRepository.GetByLocation(project.Id, RstMediaLocations.ANH_DAI_DIEN_DU_AN)?.UrlImage;
                project.FavoriteCount = 100 + _rstProjectFavouriteEFRepository.EntityNoTracking.Where(x => x.OpenSellId == project.OpenSellId).Count();
                project.ViewCount = 1000 + project.ViewCount;

                var rating = from rate in _dbContext.RstRatings
                             join order in _dbContext.RstOrders on rate.OrderId equals order.Id
                             where order.OpenSellDetailId == project.OpenSellId && order.Deleted == YesNo.NO
                             select rate;

                project.RatingRate = rating.Any() ? decimal.Round(rating.Select(o => (decimal)o.Rate).Average(), 1) : 0;
                project.TotalReviewers = rating.Count();

                var ownerQuery = _rstOwnerEFRepository.AppFindById(projectQuery.OwnerId);
                project.OwnerName = ownerQuery?.OwnerName;
            }

            listOpenProject = listOpenProject
                              .OrderByDescending(x => _dbContext.RstProjectFavourites.FirstOrDefault(rp => rp.OpenSellId == x.OpenSellId && rp.InvestorId == investorId)?.CreatedDate).ToList();
            return listOpenProject;
        }

        /// <summary>
        /// Lấy các tham số để app tạo bộ lọc dự án
        /// </summary>
        /// <returns></returns>
        public AppGetParamsFindProjectDto AppGetParamsFindProjects(bool isSaleView)
        {
            int? investorId = null;
            try
            {
                investorId = CommonUtils.GetCurrentInvestorId(_httpContext);
            }
            catch
            {
                investorId = null;
            }
            _logger.LogInformation($"{nameof(AppGetParamsFindProjects)}: investorId = {investorId}");
            var listTradingIds = _logicInvestorTradingSharedServices.FindListTradingProviderForApp(investorId, isSaleView);
            var rslt = _rstProjectEFRepository.AppGetParamsFindProject(listTradingIds);
            return rslt;
        }

        /// <summary>
        /// App chi tiết dự án
        /// </summary>
        /// <param name="openSellId"></param>
        /// <returns></returns>
        public AppDetailProjectDto AppGetDetailProject(int openSellId)
        {
            _logger.LogInformation($"{nameof(UpdateProjectOverviewContent)} : openSellId = {openSellId}");
            // Lấy thông tin Tab tổng quan dự án
            var openSell = _rstOpenSellEFRepository.FindById(openSellId);
            var project = _rstProjectEFRepository.AppGetOverviewProject(openSell?.ProjectId ?? 0, openSell?.TradingProviderId ?? 0);
            project.TradingProviderId = openSell?.TradingProviderId;

            if (project != null)
            {
                // Lấy sản phẩm dự án
                var type = _rstProjectTypeEFRepository.FindAllByProjectId(openSell.ProjectId, RstProjectDetailTypes.ProductTypes);
                project.ProjectType = type;

                try
                {
                    var investorId = CommonUtils.GetCurrentInvestorId(_httpContext);
                    //Tăng giá trị view count(tổng lượt xem của mở bán lên 1 đơn vị)
                    var openSellInterest = new RstOpenSellInterest
                    {
                        OpenSellDetailId = openSellId,
                        InvestorId = investorId,
                    };
                    _rstOpenSellInterestEFRepository.Update(openSellInterest);
                }
                catch
                {

                }

                openSell.ViewCount++;
                _dbContext.SaveChanges();

                project.Hotline = openSell?.Hotline;
                project.IsRegisterSale = openSell.IsRegisterSale;

                // Nếu cho phép đăng ký làm cộng tác viên
                // Kiểm tra xem khách hàng đã là sale của đại lý hay chưa
                if (openSell.IsRegisterSale)
                {
                    try
                    {
                        var saleId = CommonUtils.GetCurrentSaleId(_httpContext);
                        // Nếu Khách hàng là sale của đại lý
                        if (_dbContext.SaleTradingProviders.Any(std => std.SaleId == saleId && std.TradingProviderId == openSell.TradingProviderId && std.Status == Status.ACTIVE 
                                && std.Deleted == YesNo.NO && _dbContext.Sales.Any(s => s.SaleId == std.SaleId && s.Deleted == YesNo.NO)))
                        {
                            project.IsRegisterSale = false;
                        }
                    }
                    catch 
                    {

                    }
                }    

                var utilities = from utility in RstProjectUtilityData.UtilityData
                                join projectUtility in _dbContext.RstProjectUtilities on utility.Id equals projectUtility.UtilityId
                                where projectUtility.Deleted == YesNo.NO && projectUtility.IsHighlight == YesNo.YES && projectUtility.ProjectId == openSell.ProjectId
                                select new AppViewUtilityDto
                                {
                                    Icon = utility.Icon,
                                    Name = utility.Name,
                                    Type = projectUtility.Type,
                                    IsHighlight = projectUtility.IsHighlight,
                                };
                project.Utilities = utilities.ToList();
                project.UtilityHightLight = utilities.Where(u => u.IsHighlight == YesNo.YES).ToList();
                var utilityMedia = _dbContext.RstProjectUtilityMedias.Where(m => m.ProjectId == openSell.ProjectId && m.Status == Status.ACTIVE && m.Deleted == YesNo.NO);
                project.UtilityMedia = _mapper.Map<List<AppRstProjectUtilityMediaDto>>(utilityMedia);
                project.GuaranteeBanks = _rstProjectGuaranteeBankEFRepository.GetAllProjectGuaranteeBank(openSell.ProjectId).ToList();
                project.ProjectExtends = _mapper.Map<List<AppRstProjectExtendDto>>(_rstProjectExtendEFRepository.GetAll(openSell.ProjectId));
                project.ProjectInformationShare = new();
                var projectShares = _dbContext.RstProjectInformationShares.Where(x => x.ProjectId == project.Id && x.Status == Status.ACTIVE && x.Deleted == YesNo.NO);
                foreach (var item in projectShares)
                {
                    var shareItem = _mapper.Map<AppRstProjectInformationShareDto>(item);
                    var projectShareDetail = _dbContext.RstProjectInformationShareDetails.Where(x => x.ProjectShareId == item.Id && x.Deleted == YesNo.NO);
                    shareItem.DocumentFiles = _mapper.Map<List<RstProjectInformationShareDetailDto>>(projectShareDetail.Where(x => x.Type == ProjectInformationShareFileTypes.Document));
                    shareItem.ImageFiles = _mapper.Map<List<RstProjectInformationShareDetailDto>>(projectShareDetail.Where(x => x.Type == ProjectInformationShareFileTypes.Image));
                    project.ProjectInformationShare.Add(shareItem);
                }
                // Lấy thông tin Tab chủ đầu tư
                var owner = _rstOwnerEFRepository.AppFindById(project.OwnerId ?? 0);

                // Danh sách các dự án của chủ đầu tư cũng do đại lý hiện tại mở bán
                var projectByOwner = _rstProjectEFRepository.AppFindListProject(project.OwnerId ?? 0, openSell.TradingProviderId, project.Id);

                if (owner != null)
                {
                    owner.ListProjects = projectByOwner;
                }

                // Lấy thông tin Tab Hồ sơ dự án
                var projectFile = _rstProjectFileEFRepository.AppFindByProjectId(openSell.ProjectId, RstProjectFileTypes.HoSoPhapLy);
                var sellingDocumentFiles = _rstProjectFileEFRepository.AppFindByProjectId(openSell.ProjectId, RstProjectFileTypes.TaiLieuBanHang);
                var distributionFile = _rstOpenSellFileEFRepository.AppFindActiveByProjectId(openSell.ProjectId, RstOpenSellFileTypes.TaiLieuPhanPhoi);
                var sellFile = _rstOpenSellFileEFRepository.AppFindActiveByProjectId(openSell.ProjectId, RstOpenSellFileTypes.ChuongTrinhBanHang);

                var files = new AppProjectFilesDto
                {
                    ProjectFiles = _mapper.Map<List<AppViewProjectFileDto>>(projectFile),
                    DistributionFiles = _mapper.Map<List<AppViewOpenSellFileDto>>(distributionFile),
                    OpenSellFiles = _mapper.Map<List<AppViewOpenSellFileDto>>(sellFile),
                    SellingDocumentFiles = _mapper.Map<List<AppViewProjectFileDto>>(sellingDocumentFiles),
                };

                // Lấy thông tin Hồ sơ thiết kế
                var listMediaLocation = new string[] { RstMediaLocations.CAN_HO_MAU_DU_AN, RstMediaLocations.ANH_MAT_BANG_DU_AN };
                var mediaDetails = _mapper.Map<List<AppViewProjectMediaDetailDto>>(_rstProjectMediaDetailEFRepository.Find(openSell.ProjectId)?.Where(x => listMediaLocation.Contains(x.Location)));

                return new AppDetailProjectDto
                {
                    OverviewProject = project,
                    Owner = owner,
                    Files = files,
                    GroupMediaDetail = mediaDetails,
                };
            }
            return null;
        }

        /// <summary>
        /// App lấy hết media theo dự án
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        public List<AppViewProjectMediaDto> AppGetAllMedia(int projectId)
        {
            _logger.LogInformation($"{nameof(UpdateProjectOverviewContent)} : projectId = {projectId}");

            var result = new List<AppViewProjectMediaDto>() { };
            var medias = _rstProjectMediaEFRepository.Find(projectId);
            foreach (var media in medias)
            {
                var tmpMedia = _mapper.Map<AppViewProjectMediaDto>(media);
                var tmpMediaDetail = _rstProjectMediaDetailEFRepository.FindByProjectMediaId(tmpMedia.Id);
                tmpMedia.Details = _mapper.Map<List<AppViewProjectMediaDetailDto>>(tmpMediaDetail);

                result.Add(tmpMedia);
            }
            return result;
        }

        #endregion

        #region Trình duyệt sản phẩm tích lũy
        /// <summary>
        /// Yêu cầu trình duyệt
        /// </summary>
        public void Request(RstRequestDto input)
        {
            _logger.LogInformation($"{nameof(Request)}: input = {JsonSerializer.Serialize(input)}");

            var partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
            var userId = CommonUtils.GetCurrentUserId(_httpContext);
            var username = CommonUtils.GetCurrentUsername(_httpContext);

            //Tìm kiếm thông tin dự án
            var projectQuery = _rstProjectEFRepository.FindById(input.Id).ThrowIfNull(_dbContext, ErrorCode.RstProjectNotFound);

            // Status = 1 : trình duyệt với, Status = 4 trình duyệt lại dự án
            if (projectQuery.Status == DistributionStatus.KHOI_TAO || projectQuery.Status == DistributionStatus.HUY_DUYET)
            {
                //Nếu đã tồn tại bản ghi trước đó
                var findRequest = _rstApproveEFRepository.FindByIdOfDataType(input.Id, RstApproveDataTypes.RST_PROJECT);

                var request = _rstApproveEFRepository.Request(new RstApprove
                {
                    UserRequestId = userId,
                    UserApproveId = input.UserApproveId,
                    RequestNote = input.RequestNote,
                    ActionType = findRequest != null ? ActionTypes.CAP_NHAT : ActionTypes.THEM_MOI,
                    DataType = RstApproveDataTypes.RST_PROJECT,
                    ReferId = input.Id,
                    Summary = input.Summary,
                    CreatedBy = username,
                    TradingProviderId = null,
                    PartnerId = partnerId
                });

                projectQuery.Status = DistributionStatus.CHO_DUYET;
                request.DataStatus = DistributionStatus.CHO_DUYET;
            }
            _dbContext.SaveChanges();
        }

        /// <summary>
        /// Duyệt dự án
        /// </summary>
        public void Approve(RstApproveDto input)
        {
            _logger.LogInformation($"{nameof(Approve)}: input = {JsonSerializer.Serialize(input)}");

            var userId = CommonUtils.GetCurrentUserId(_httpContext);
            //Tìm kiếm thông tin dự án
            var projectQuery = _rstProjectEFRepository.FindById(input.Id).ThrowIfNull(_dbContext, ErrorCode.RstProjectNotFound);

            if (projectQuery.Status == DistributionStatus.CHO_DUYET)
            {
                var findRequest = _rstApproveEFRepository.FindByIdOfDataType(input.Id, RstApproveDataTypes.RST_PROJECT).ThrowIfNull(_dbContext, ErrorCode.RstApproveNotFound);

                _rstApproveEFRepository.Approve(new RstApprove
                {
                    Id = findRequest.Id,
                    ApproveNote = input.ApproveNote,
                    UserApproveId = userId,
                    ApproveIp = CommonUtils.GetCurrentRemoteIpAddress(_httpContext)
                });

                projectQuery.Status = DistributionStatus.HOAT_DONG;
            }
            _dbContext.SaveChanges();
        }

        /// <summary>
        /// Huy duyet dự án
        /// </summary>
        public void Cancel(RstCancelDto input)
        {
            _logger.LogInformation($"{nameof(Cancel)}: input = {JsonSerializer.Serialize(input)}");

            //Tìm kiếm thông tin dự án
            var projectQuery = _rstProjectEFRepository.FindById(input.Id).ThrowIfNull(_dbContext, ErrorCode.RstProjectNotFound);

            var findRequest = _rstApproveEFRepository.FindByIdOfDataType(input.Id, RstApproveDataTypes.RST_PROJECT).ThrowIfNull(_dbContext, ErrorCode.RstApproveNotFound);
            _rstApproveEFRepository.Cancel(new RstApprove
            {
                Id = findRequest.Id,
                CancelNote = input.CancelNote
            });
            //Cập nhật trạng thái cho dự án
            projectQuery.Status = DistributionStatus.HUY_DUYET;
            _dbContext.SaveChanges();
        }
        #endregion
    }
}
