using AutoMapper;
using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Office2010.ExcelAc;
using EPIC.CoreDomain.Interfaces;
using EPIC.CoreEntities.Dto.InvestorSearch;
using EPIC.CoreRepositories;
using EPIC.CoreSharedServices.Interfaces;
using EPIC.DataAccess.Base;
using EPIC.DataAccess.Models;
using EPIC.EventEntites.Dto.EvtEvent;
using EPIC.EventEntites.Entites;
using EPIC.GarnerEntities.DataEntities;
using EPIC.GarnerEntities.Dto.GarnerDistribution;
using EPIC.GarnerRepositories;
using EPIC.InvestEntities.DataEntities;
using EPIC.InvestRepositories;
using EPIC.RealEstateEntities.DataEntities;
using EPIC.RealEstateEntities.Dto.RstProject;
using EPIC.RealEstateRepositories;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Event;
using EPIC.Utils.ConstantVariables.Garner;
using EPIC.Utils.ConstantVariables.Payment;
using EPIC.Utils.ConstantVariables.RealEstate;
using EPIC.Utils.ConstantVariables.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text.Json;

namespace EPIC.CoreDomain.Implements
{
    public class InvestorSearchService : IInvestorSearchService
    {
        private readonly EpicSchemaDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<InvestorSearchService> _logger;
        private readonly IHttpContextAccessor _httpContext;
        private readonly ILogicInvestorTradingSharedServices _logicInvestorTradingSharedServices;

        private readonly DistributionEFRepository _distributionEFRepository;
        private readonly RstProjectEFRepository _rstProjectEFRepository;
        private readonly GarnerDistributionEFRepository _garnerDistributionEFRepository;
        private readonly InvestorSeachHistoryEFRepository _investorSeachHistoryEFRepository;

        public InvestorSearchService(
            EpicSchemaDbContext dbContext, 
            IMapper mapper, 
            ILogger<InvestorSearchService> logger,
            IHttpContextAccessor httpContext, 
            ILogicInvestorTradingSharedServices logicInvestorTradingSharedServices)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
            _httpContext = httpContext;
            _logicInvestorTradingSharedServices = logicInvestorTradingSharedServices;
            _rstProjectEFRepository = new RstProjectEFRepository(_dbContext, logger);
            _distributionEFRepository = new DistributionEFRepository(_dbContext, logger);
            _garnerDistributionEFRepository = new GarnerDistributionEFRepository(_dbContext, logger);
            _investorSeachHistoryEFRepository = new InvestorSeachHistoryEFRepository(_dbContext, _logger);
        }

        public PagingResult<object> Search(FilterInvestorSearchDto input)
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
            _logger.LogInformation($"{nameof(Search)}: input = {JsonSerializer.Serialize(input)}, investorId = {investorId}");
            var listTradingProviderIds = _logicInvestorTradingSharedServices.FindListTradingProviderForApp(investorId);
            IEnumerable<InvestorSearchResultDto> queryInvest = _distributionEFRepository.FindAllAppQuery(input.Keyword, listTradingProviderIds)
                .Select(distribution => new InvestorSearchInvestDto
                {
                    DistributionId = distribution.DistributionId,
                    TradingProviderId = distribution.TradingProviderId,
                    ProjectId = distribution.ProjectId,
                    InvCode = distribution.InvCode,
                    TradingProviderName = distribution.TradingProviderName,
                    Profit = distribution.Profit,
                    Image = distribution.Image,
                    DistributionImage = distribution.DistributionImage,
                    MinPeriodQuantity = distribution.MinPeriodQuantity,
                    MaxPeriodQuantity = distribution.MaxPeriodQuantity,
                    MinPeriodType = distribution.MinPeriodType,
                    MaxPeriodType = distribution.MaxPeriodType,
                    SystemProductType = ProductTypes.INVEST
                });
            IEnumerable<InvestorSearchResultDto> queryGarner = _garnerDistributionEFRepository.AppSearchDistribution(input.Keyword, listTradingProviderIds)
                .Select(distribution => new InvestorSearchGarnerDto
                {
                    DistributionId = distribution.DistributionId,
                    ProductType = distribution.ProductType,
                    ProductName = distribution.ProductName,
                    PolicyId = distribution.PolicyId,
                    PolicyName = distribution.PolicyName,
                    Profit = distribution.Profit,
                    Icon = distribution.Icon,
                    TradingProviderName = distribution.TradingProviderName,
                    TradingProviderId = distribution.TradingProviderId,
                    Image = distribution.Image,
                    IsDefault = distribution.IsDefault,
                    InterestTypeName = distribution.InterestTypeName,
                    SystemProductType = ProductTypes.GARNER,
                    CalculateType = distribution.CalculateType,
                    Classify = distribution.Classify,
                    IncomeTax = distribution.IncomeTax,
                    InterestPeriodQuantity = distribution.InterestPeriodQuantity,
                    InterestPeriodType = distribution.InterestPeriodType,
                    MinInvestDay = distribution.MinInvestDay,
                    MinMoney = distribution.MinMoney,
                    MinWithdraw = distribution.MinWithdraw,
                    OrderOfWithdrawal = distribution.OrderOfWithdrawal,
                    TransferAssetsFee = distribution.TransferAssetsFee,
                    WithdrawFee = distribution.WithdrawFee,
                    WithdrawFeeType = distribution.WithdrawFeeType,
                    GarnerType = distribution.GarnerType,
                    RepeatFixedDate = distribution.RepeatFixedDate,
                    InterestType = distribution.InterestType,
                    InvestorType = distribution.InvestorType,
                    ProductTypeName = distribution.ProductTypeName
                });

            IEnumerable<InvestorSearchResultDto> queryRealEstate = _rstProjectEFRepository.AppFindOpenSellProject(new AppFindProjectDto { Keyword = input.Keyword }, listTradingProviderIds, investorId)
                .Select(o => new InvestorSearchRealEstateDto
                {
                    Id = o.Id,
                    TradingProviderId = o.TradingProviderId,
                    TradingProviderName = o.TradingProviderName,
                    Code = o.Code,
                    Name = o.Name,
                    MinSellingPrice = o.MinSellingPrice,
                    MaxSellingPrice = o.MaxSellingPrice,
                    ExpectedSellingPrice = o.ExpectedSellingPrice,
                    ProjectType = o.ProjectType,
                    RedBook = o.RedBook,
                    ProvinceCode = o.ProvinceCode,
                    ProvinceName = o.ProvinceName,
                    IsOutstanding = o.IsOutstanding,
                    LandArea = o.LandArea,
                    Latitude = o.Latitude,
                    Longitude = o.Longitude,
                    Address = o.Address,
                    OpenSellId = o.OpenSellId,
                    NumberOfUnit = o.NumberOfUnit,
                    ViewCount = o.ViewCount,
                    IsFavourite = o.IsFavourite,
                    CreatedDate = o.CreatedDate,
                    SystemProductType = ProductTypes.REAL_ESTATE
                });

            IEnumerable<InvestorSearchResultDto> queryEvent = _dbContext.EvtEvents.Include(e => e.EventDetails).ThenInclude(ed => ed.Tickets)
                                                     .Include(e => e.EventMedias).ThenInclude(em => em.EventMediaDetails)
                                                     .Where(e => e.IsShowApp == YesNo.YES
                                                     && e.Deleted == YesNo.NO
                                                     && (e.IsCheck == YesNo.YES)
                                                     && (e.Status == EventStatus.DANG_MO_BAN)
                                                     && e.EventDetails.Where(e => e.Status == EventDetailStatus.KICH_HOAT && e.Deleted == YesNo.NO).Select(e => e.EndDate).Any(e => e >= DateTime.Now)
                                                     && (listTradingProviderIds.Count == 0 || listTradingProviderIds.Contains(e.TradingProviderId))
                                                     && (input.Keyword == null || e.Name.ToLower().Contains(input.Keyword.ToLower()))
                                                     && e.EventDetails.Any(e => e.EndDate >= DateTime.Now))
                                                    .Select(e => new
                                                    {
                                                        evt = e,
                                                        ticketPrice = e.EventDetails.Where(ed => ed.Deleted == YesNo.NO && ed.Status == EventDetailStatus.KICH_HOAT && ed.Tickets != null && ed.Tickets.Any(ti => ti.Deleted == YesNo.NO))
                                                                                    .SelectMany(ed => ed.Tickets.Where(ti => ti.Deleted == YesNo.NO).Select(ti => (decimal?)ti.Price))
                                                    })
                                                     .Select(e => new InvestorSearchEventDto
                                                     {
                                                         Id = e.evt.Id,
                                                         TradingProviderId = e.evt.TradingProviderId,
                                                         StartDate = e.evt.EventDetails.Where(ed => ed.Deleted == YesNo.NO && ed.Status == EventDetailStatus.KICH_HOAT && ed.StartDate != null).OrderBy(ed => ed.StartDate).Where(ed => ed.StartDate >= DateTime.Now).Select(e => e.StartDate).FirstOrDefault()
                                                                ?? e.evt.EventDetails.Where(ed => ed.Deleted == YesNo.NO && ed.Status == EventDetailStatus.KICH_HOAT && ed.StartDate != null).OrderBy(ed => ed.StartDate).Select(e => e.StartDate).LastOrDefault(),
                                                         EndDate = e.evt.EventDetails.Where(ed => ed.Deleted == YesNo.NO && ed.Status == EventDetailStatus.KICH_HOAT && ed.EndDate != null).OrderByDescending(ed => ed.EndDate).Where(ed => ed.EndDate >= DateTime.Now).Select(e => e.EndDate).FirstOrDefault(),
                                                         Name = e.evt.Name,
                                                         Organizator = e.evt.Organizator,
                                                         EventTypes = e.evt.EventTypes.Select(e => e.Type),
                                                         Location = e.evt.Location,
                                                         ProvinceCode = e.evt.ProvinceCode,
                                                         ProvinceName = e.evt.Province.Name,
                                                         Address = e.evt.Address,
                                                         Latitude = e.evt.Latitude,
                                                         Longitude = e.evt.Longitude,
                                                         BannerImageUrl = e.evt.EventMedias.Where(e => e.Location == EvtMediaLocation.BANNER_EVENT).Select(e => e.UrlImage).FirstOrDefault(),
                                                         AvatarImageUrl = e.evt.EventMedias.Where(e => e.Location == EvtMediaLocation.AVATAR_EVENT).Select(e => e.UrlImage).FirstOrDefault(),
                                                         MinTicketPrice = e.ticketPrice.Min() ?? 0,
                                                         MaxTicketPrice = e.ticketPrice.Max() ?? 0,
                                                         IsFree = e.evt.EventDetails.Where(ed => ed.Deleted == YesNo.NO && ed.Status == EventDetailStatus.KICH_HOAT).All(e => e.Tickets.All(e => e.IsFree)),
                                                         SystemProductType = ProductTypes.EVENT
                                                     });

            IEnumerable<InvestorSearchResultDto> query = queryInvest.Union(queryGarner).Union(queryRealEstate).Union(queryEvent);

            var result = new PagingResult<object>
            {
                Items = query.Skip(input.Skip).Take(input.PageSize).ToList(),
                TotalItems = query.Count()
            };
            return result;
        }

        public void AddHistorySearch(InvestorSearchHistoryCreateDto input)
        {
            var investorId = CommonUtils.GetCurrentInvestorId(_httpContext);
            _logger.LogInformation($"{nameof(InvestorServices)}->{nameof(AddHistorySearch)}: input = {JsonSerializer.Serialize(input)}");

            var checkExists = _dbContext.InvestorSearchHistories.FirstOrDefault(ivh => ivh.InvestDistributionId == input.InvestDistributionId
            && ivh.GarnerPolicyId == input.GarnerPolicyId && ivh.RstOpenSellId == input.RstOpenSellId && ivh.EventId == input.EventId && ivh.Deleted == YesNo.NO);

            if (checkExists != null)
            {
                checkExists.CreatedDate = DateTime.Now;
            } 
            else
            {
                var insert = new CoreEntities.DataEntities.InvestorSearchHistory()
                {
                    InvestorId = investorId,
                    InvestDistributionId = input.InvestDistributionId,
                    GarnerPolicyId = input.GarnerPolicyId,
                    RstOpenSellId = input.RstOpenSellId,
                    EventId = input.EventId,
                };
                insert.Id = (int)_investorSeachHistoryEFRepository.NextKey();
                _dbContext.InvestorSearchHistories.Add(insert);
            }
            _dbContext.SaveChanges();
        }

        public void DeleteHistorySearch()
        {
            var investorId = CommonUtils.GetCurrentInvestorId(_httpContext);
            _logger.LogInformation($"{nameof(InvestorServices)}->{nameof(DeleteHistorySearch)}: investorId = {investorId}");
            var deleteHistory = _dbContext.InvestorSearchHistories.Where(h => h.InvestorId == investorId);
            _dbContext.InvestorSearchHistories.RemoveRange(deleteHistory);
            _dbContext.SaveChanges();
        }

        public PagingResult<object> HistorySearch(FilterInvestorSearchDto input)
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
            _logger.LogInformation($"{nameof(Search)}: input = {JsonSerializer.Serialize(input)}, investorId = {investorId}");
            var listTradingProviderIds = _logicInvestorTradingSharedServices.FindListTradingProviderForApp(investorId);

            var listInvestDistributionId = new List<int>();
            var listGarnerPolicyId = new List<int>(); 
            var listRstOpenSellId = new List<int>();
            List<InvestorSearchResultDto> query = new List<InvestorSearchResultDto>(); 
            var historySearch = _dbContext.InvestorSearchHistories.Where(h => h.InvestorId == investorId).OrderByDescending(h => h.CreatedDate);
            foreach (var item in historySearch)
            {
                if (item.InvestDistributionId != null)
                {
                    var investDistribution = _distributionEFRepository.FindAllAppQuery(input.Keyword, listTradingProviderIds)
                    .Select(distribution => new InvestorSearchInvestDto
                    {
                        DistributionId = distribution.DistributionId,
                        TradingProviderId = distribution.TradingProviderId,
                        ProjectId = distribution.ProjectId,
                        InvCode = distribution.InvCode,
                        TradingProviderName = distribution.TradingProviderName,
                        Profit = distribution.Profit,
                        Image = distribution.Image,
                        DistributionImage = distribution.DistributionImage,
                        MinPeriodQuantity = distribution.MinPeriodQuantity,
                        MaxPeriodQuantity = distribution.MaxPeriodQuantity,
                        MinPeriodType = distribution.MinPeriodType,
                        MaxPeriodType = distribution.MaxPeriodType,
                        SystemProductType = ProductTypes.INVEST,
                    }).FirstOrDefault(e => e.DistributionId == item.InvestDistributionId);
                    if (investDistribution != null)
                    {
                        query.Add(investDistribution);
                    }
                }

                if (item.GarnerPolicyId != null)
                {
                    var garnerPolicy = _garnerDistributionEFRepository.AppSearchDistribution(input.Keyword, listTradingProviderIds)
                    .Select(distribution => new InvestorSearchGarnerDto
                    {
                        DistributionId = distribution.DistributionId,
                        ProductType = distribution.ProductType,
                        ProductName = distribution.ProductName,
                        PolicyId = distribution.PolicyId,
                        PolicyName = distribution.PolicyName,
                        Profit = distribution.Profit,
                        Icon = distribution.Icon,
                        TradingProviderName = distribution.TradingProviderName,
                        TradingProviderId = distribution.TradingProviderId,
                        Image = distribution.Image,
                        IsDefault = distribution.IsDefault,
                        InterestTypeName = distribution.InterestTypeName,
                        SystemProductType = ProductTypes.GARNER,
                        CalculateType = distribution.CalculateType,
                        Classify = distribution.Classify,
                        IncomeTax = distribution.IncomeTax,
                        InterestPeriodQuantity = distribution.InterestPeriodQuantity,
                        InterestPeriodType = distribution.InterestPeriodType,
                        MinInvestDay = distribution.MinInvestDay,
                        MinMoney = distribution.MinMoney,
                        MinWithdraw = distribution.MinWithdraw,
                        OrderOfWithdrawal = distribution.OrderOfWithdrawal,
                        TransferAssetsFee = distribution.TransferAssetsFee,
                        WithdrawFee = distribution.WithdrawFee,
                        WithdrawFeeType = distribution.WithdrawFeeType,
                        GarnerType = distribution.GarnerType,
                        RepeatFixedDate = distribution.RepeatFixedDate,
                        InterestType = distribution.InterestType,
                        InvestorType = distribution.InvestorType,
                        ProductTypeName = distribution.ProductTypeName,
                    }).FirstOrDefault(e => e.PolicyId == item.GarnerPolicyId);
                    if (garnerPolicy != null)
                    {
                        query.Add(garnerPolicy);
                    }
                }

                if (item.RstOpenSellId != null)
                {
                    var rstOpenSell = _rstProjectEFRepository.AppFindOpenSellProject(new AppFindProjectDto { Keyword = input.Keyword }, listTradingProviderIds, investorId)
                    .Select(o => new InvestorSearchRealEstateDto
                    {
                        Id = o.Id,
                        TradingProviderId = o.TradingProviderId,
                        TradingProviderName = o.TradingProviderName,
                        Code = o.Code,
                        Name = o.Name,
                        MinSellingPrice = o.MinSellingPrice,
                        MaxSellingPrice = o.MaxSellingPrice,
                        ExpectedSellingPrice = o.ExpectedSellingPrice,
                        ProjectType = o.ProjectType,
                        RedBook = o.RedBook,
                        ProvinceCode = o.ProvinceCode,
                        ProvinceName = o.ProvinceName,
                        IsOutstanding = o.IsOutstanding,
                        LandArea = o.LandArea,
                        Latitude = o.Latitude,
                        Longitude = o.Longitude,
                        Address = o.Address,
                        OpenSellId = o.OpenSellId,
                        NumberOfUnit = o.NumberOfUnit,
                        ViewCount = o.ViewCount,
                        IsFavourite = o.IsFavourite,
                        CreatedDate = o.CreatedDate,
                        SystemProductType = ProductTypes.REAL_ESTATE,
                    }).FirstOrDefault(e => e.OpenSellId == item.RstOpenSellId);
                    if (rstOpenSell != null)
                    {
                        query.Add(rstOpenSell);
                    }
                }

                if (item.EventId != null)
                {
                    var evtEvent = _dbContext.EvtEvents.Include(e => e.EventDetails).ThenInclude(ed => ed.Tickets)
                                                     .Include(e => e.EventMedias).ThenInclude(em => em.EventMediaDetails)
                                                     .Where(e => e.IsShowApp == YesNo.YES
                                                     && e.Deleted == YesNo.NO
                                                     && (e.IsCheck == YesNo.YES)
                                                     && (e.Status == EventStatus.DANG_MO_BAN)
                                                     && e.EventDetails.Where(e => e.Status == EventDetailStatus.KICH_HOAT && e.Deleted == YesNo.NO).Select(e => e.EndDate).Any(e => e >= DateTime.Now)
                                                     && (listTradingProviderIds.Count == 0 || listTradingProviderIds.Contains(e.TradingProviderId))
                                                     && (input.Keyword == null || e.Name.ToLower().Contains(input.Keyword.ToLower()))
                                                     && e.EventDetails.Any(e => e.EndDate >= DateTime.Now))
                                                    .Select(e => new
                                                    {
                                                        evt = e,
                                                        ticketPrice = e.EventDetails.Where(ed => ed.Deleted == YesNo.NO && ed.Status == EventDetailStatus.KICH_HOAT && ed.Tickets != null && ed.Tickets.Any(ti => ti.Deleted == YesNo.NO))
                                                                                    .SelectMany(ed => ed.Tickets.Where(ti => ti.Deleted == YesNo.NO).Select(ti => (decimal?)ti.Price))
                                                    })
                                                     .Select(e => new InvestorSearchEventDto
                                                     {
                                                         Id = e.evt.Id,
                                                         TradingProviderId = e.evt.TradingProviderId,
                                                         StartDate = e.evt.EventDetails.Where(ed => ed.Deleted == YesNo.NO && ed.Status == EventDetailStatus.KICH_HOAT && ed.StartDate != null).OrderBy(ed => ed.StartDate).Where(ed => ed.StartDate >= DateTime.Now).Select(e => e.StartDate).FirstOrDefault()
                                                                ?? e.evt.EventDetails.Where(ed => ed.Deleted == YesNo.NO && ed.Status == EventDetailStatus.KICH_HOAT && ed.StartDate != null).OrderBy(ed => ed.StartDate).Select(e => e.StartDate).LastOrDefault(),
                                                         EndDate = e.evt.EventDetails.Where(ed => ed.Deleted == YesNo.NO && ed.Status == EventDetailStatus.KICH_HOAT && ed.EndDate != null).OrderByDescending(ed => ed.EndDate).Where(ed => ed.EndDate >= DateTime.Now).Select(e => e.EndDate).FirstOrDefault(),
                                                         Name = e.evt.Name,
                                                         Organizator = e.evt.Organizator,
                                                         EventTypes = e.evt.EventTypes.Select(e => e.Type),
                                                         Location = e.evt.Location,
                                                         ProvinceCode = e.evt.ProvinceCode,
                                                         ProvinceName = e.evt.Province.Name,
                                                         Address = e.evt.Address,
                                                         Latitude = e.evt.Latitude,
                                                         Longitude = e.evt.Longitude,
                                                         BannerImageUrl = e.evt.EventMedias.Where(e => e.Location == EvtMediaLocation.BANNER_EVENT).Select(e => e.UrlImage).FirstOrDefault(),
                                                         AvatarImageUrl = e.evt.EventMedias.Where(e => e.Location == EvtMediaLocation.AVATAR_EVENT).Select(e => e.UrlImage).FirstOrDefault(),
                                                         MinTicketPrice = e.ticketPrice.Min() ?? 0,
                                                         MaxTicketPrice = e.ticketPrice.Max() ?? 0,
                                                         IsFree = e.evt.EventDetails.Where(ed => ed.Deleted == YesNo.NO && ed.Status == EventDetailStatus.KICH_HOAT).All(e => e.Tickets.All(e => e.IsFree)),
                                                         SystemProductType = ProductTypes.EVENT
                                                     }).FirstOrDefault(e => e.Id == item.EventId);
                    if (evtEvent != null)
                    {
                        query.Add(evtEvent);
                    }
                }
            }
            var result = new PagingResult<object>
            {
                Items = query.Skip(input.Skip).Take(input.PageSize).ToList(),
                TotalItems = query.Count()
            };
            return result;
        }
    }
}
