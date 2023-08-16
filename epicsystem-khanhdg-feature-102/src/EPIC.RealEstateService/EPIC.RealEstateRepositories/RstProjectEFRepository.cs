using EPIC.DataAccess.Base;
using EPIC.DataAccess.Models;
using EPIC.InvestEntities.DataEntities;
using EPIC.RealEstateEntities.DataEntities;
using EPIC.RealEstateEntities.Dto.RstProject;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.RealEstate;
using EPIC.Utils.ConstantVariables.Shared;
using EPIC.Utils.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace EPIC.RealEstateRepositories
{
    public class RstProjectEFRepository : BaseEFRepository<RstProject>
    {
        public RstProjectEFRepository(EpicSchemaDbContext dbContext, ILogger logger) : base(dbContext, logger, $"{DbSchemas.EPIC_REAL_ESTATE}.{RstProject.SEQ}")
        {
        }

        public RstProject Add(RstProject input)
        {
            _logger.LogInformation($"{nameof(RstProjectEFRepository)}-> {nameof(Add)}: input = {JsonSerializer.Serialize(input)}");
            input.Id = (int)NextKey();
            input.CreatedDate = DateTime.Now;
            input.IsCheck = YesNo.NO;
            input.Status = DistributionStatus.KHOI_TAO;
            return _dbSet.Add(input).Entity;
        }

        public RstProject Update(RstProject input)
        {
            _logger.LogInformation($"{nameof(RstProjectEFRepository)}-> {nameof(Update)}: input = {JsonSerializer.Serialize(input)}");
            var projectFind = _dbSet.FirstOrDefault(p => p.Id == input.Id && p.PartnerId == input.PartnerId && p.Deleted == YesNo.NO).ThrowIfNull(_epicSchemaDbContext, ErrorCode.RstProjectNotFound);
            projectFind.OwnerId = input.OwnerId;
            projectFind.Code = input.Code;
            projectFind.Name = input.Name;
            projectFind.ContractorName = input.ContractorName;
            projectFind.ContractorLink = input.ContractorLink;
            projectFind.ContractorDescription = input.ContractorDescription;
            projectFind.OperatingUnit = input.OperatingUnit;
            projectFind.OperatingUnitLink = input.OperatingUnitLink;
            projectFind.OperatingUnitDescription = input.OperatingUnitDescription;
            projectFind.Website = input.Website;
            projectFind.Phone = input.Phone;
            projectFind.FacebookLink = input.FacebookLink;
            projectFind.ProjectType = input.ProjectType;
            projectFind.ProjectStatus = input.ProjectStatus;
            projectFind.LandArea = input.LandArea;
            projectFind.ConstructionArea = input.ConstructionArea;
            projectFind.BuildingDensity = input.BuildingDensity;
            projectFind.LandPlotNo = input.LandPlotNo;
            projectFind.MapSheetNo = input.MapSheetNo;
            projectFind.StartDate = input.StartDate;
            projectFind.ExpectedHandoverTime = input.ExpectedHandoverTime;
            projectFind.TotalInvestment = input.TotalInvestment;
            projectFind.ExpectedSellingPrice = input.ExpectedSellingPrice;
            projectFind.MinSellingPrice = input.MinSellingPrice;
            projectFind.MaxSellingPrice = input.MaxSellingPrice;
            projectFind.NumberOfUnit = input.NumberOfUnit;
            projectFind.ProvinceCode = input.ProvinceCode;
            projectFind.Address = input.Address;
            projectFind.Latitude = input.Latitude;
            projectFind.Longitude = input.Longitude;
            projectFind.ModifiedDate = DateTime.Now;
            projectFind.ModifiedBy = input.ModifiedBy;
            return projectFind;
        }

        /// <summary>
        /// Cập nhật nội dung tổng quan của 
        /// </summary>
        /// <param name="input"></param>
        /// <param name="partnerId"></param>
        /// <returns></returns>
        public RstProject FindById(int id, int? partnerId = null)
        {
            _logger.LogInformation($"{nameof(RstProjectEFRepository)}-> {nameof(FindById)}: id = {id}");
            var projectFind = _dbSet.FirstOrDefault(p => p.Id == id && (partnerId == null || p.PartnerId == partnerId) && p.Deleted == YesNo.NO);
            return projectFind;
        }

        /// <summary>
        /// Lấy ra danh sách dự án mà đại lý được phân phối
        /// </summary>
        public List<RstProject> ProjectGetByTrading(int tradingProviderId)
        {
            _logger.LogInformation($"{nameof(RstProjectEFRepository)}-> {nameof(ProjectGetByTrading)}: tradingProviderId = {tradingProviderId}");
            var result = (from project in _dbSet
                          join distribution in _epicSchemaDbContext.RstDistributions on project.Id equals distribution.ProjectId
                          where project.Deleted == YesNo.NO && distribution.Deleted == YesNo.NO
                          && distribution.Status == RstDistributionStatus.DANG_BAN //&& project.Status == RstDistributionStatus.DANG_BAN 
                          && distribution.TradingProviderId == tradingProviderId
                          select project).ToList();
            return result.Distinct().ToList();
        }

        //public PagingResult<RstProject> FindAll(FilterRstProjectDto input, int? partnerId = null)
        //{
        //    _logger.LogInformation($"{nameof(RstProjectEFRepository)}->{nameof(FindAll)}: input = {JsonSerializer.Serialize(input)},  partnerId = {partnerId}");

        //    PagingResult<RstProject> result = new();
        //    var projectQuery = _dbSet.Where(r => r.Deleted == YesNo.NO
        //                && (input.Keyword == null || r.Code.Contains(input.Keyword) || r.Name.Contains(input.Keyword))
        //                && (input.Code == null || r.Code.Contains(input.Code))
        //                && (input.Name == null || r.Name.Contains(input.Name))
        //                && (input.Status == null || input.Status == r.Status)
        //                && (input.OwnerId == null || input.OwnerId == r.OwnerId)
        //                && (input.ProjectType == null || input.ProjectType == r.ProjectType));
        //    if (partnerId != null)
        //    {
        //        projectQuery = projectQuery.Where(r => r.PartnerId == partnerId);
        //    }

        //    if (input.PageSize != -1)
        //    {
        //        projectQuery = projectQuery.Skip(input.Skip).Take(input.PageSize);
        //    }

        //    result.TotalItems = projectQuery.Count();
        //    projectQuery = projectQuery.OrderDynamic(input.Sort);
        //    result.Items = projectQuery.ToList();
        //    return result;
        //}

        public PagingResult<ViewRstProjectDto> FindAll(FilterRstProjectDto input, int? partnerId = null)
        {
            _logger.LogInformation($"{nameof(RstProjectEFRepository)}->{nameof(FindAll)}: input = {JsonSerializer.Serialize(input)},  partnerId = {partnerId}");

            PagingResult<ViewRstProjectDto> result = new();
            var productItem = _epicSchemaDbContext.RstProductItems;

            var projectQuery = from project in _dbSet
                               where project.Deleted == YesNo.NO
                               && (input.Keyword == null || project.Code.Contains(input.Keyword) || project.Name.Contains(input.Keyword))
                               && (input.Code == null || project.Code.Contains(input.Code))
                               && (input.Name == null || project.Name.Contains(input.Name))
                               && (input.Status == null || project.Status == input.Status)
                               && (input.OwnerId == null || project.OwnerId == input.OwnerId)
                               && (input.ProjectType == null || input.ProjectType == project.ProjectType)
                               && (project.PartnerId == partnerId)
                               select new ViewRstProjectDto
                               {
                                   Id = project.Id,
                                   Address = project.Address,
                                   BuildingDensity = project.BuildingDensity,
                                   Code = project.Code,
                                   ConstructionArea = project.ConstructionArea,
                                   ContentType = project.ContentType,
                                   ContractorDescription = project.ContractorDescription,
                                   ContractorLink = project.ContractorLink,
                                   ContractorName = project.ContractorName,
                                   ExpectedHandoverTime = project.ExpectedHandoverTime,
                                   ExpectedSellingPrice = project.ExpectedSellingPrice,
                                   FacebookLink = project.FacebookLink,
                                   LandArea = project.LandArea,
                                   LandPlotNo = project.LandPlotNo,
                                   Latitude = project.Latitude,
                                   Longitude = project.Longitude,
                                   MapSheetNo = project.MapSheetNo,
                                   MaxSellingPrice = project.MaxSellingPrice,
                                   MinSellingPrice = project.MinSellingPrice,
                                   Name = project.Name,
                                   NumberOfUnit = project.NumberOfUnit,
                                   OperatingUnit = project.OperatingUnit,
                                   OperatingUnitDescription = project.OperatingUnitDescription,
                                   OperatingUnitLink = project.OperatingUnitLink,
                                   OverviewContent = project.OverviewContent,
                                   OwnerId = project.OwnerId,
                                   Phone = project.Phone,
                                   ProjectStatus = project.ProjectStatus,
                                   ProjectType = project.ProjectType,
                                   ProvinceCode = project.ProvinceCode,
                                   StartDate = project.StartDate,
                                   Website = project.Website,
                                   Status = project.Status,
                                   CreatedDate = project.CreatedDate,
                                   CreatedBy = project.CreatedBy,
                                   TotalInvestment = project.TotalInvestment,
                                   DistributionQuantity = (from productItem in _epicSchemaDbContext.RstProductItems
                                                           join distributionProductItem in _epicSchemaDbContext.RstDistributionProductItems on productItem.Id equals distributionProductItem.ProductItemId
                                                           join distribution in _epicSchemaDbContext.RstDistributions on distributionProductItem.DistributionId equals distribution.Id
                                                           where productItem.Deleted == YesNo.NO && distributionProductItem.Deleted == YesNo.NO && distribution.Deleted == YesNo.NO
                                                           && productItem.ProjectId == project.Id && distribution.Status != RstDistributionStatus.HUY_DUYET
                                                           select new { productItem.Id }).Distinct().Count(),
                                   TotalQuantity = productItem.Where(p => p.ProjectId == project.Id && p.Deleted == YesNo.NO).Count(),
                                   SoldQuantity = productItem.Where(p => p.ProjectId == project.Id && p.Deleted == YesNo.NO).Count(r => r.Status == RstProductItemStatus.DA_BAN),
                                   RemainingQuantity = productItem.Where(p => p.ProjectId == project.Id && p.Deleted == YesNo.NO).Count()
                                                        - productItem.Where(p => p.ProjectId == project.Id && p.Deleted == YesNo.NO)
                                                        .Count(r => r.Status == RstProductItemStatus.DA_BAN),
                               };

            result.TotalItems = projectQuery.Count();

            if (input.ProductTypes != null)
            {
                projectQuery = projectQuery.Where(e => _epicSchemaDbContext.RstProjectTypes.Where(p => p.ProjectType == e.ProjectType && p.ProjectId == e.Id && input.ProductTypes.Contains(p.Type)).Any());
            }
            projectQuery = projectQuery.OrderDynamic(input.Sort);

            if (input.PageSize != -1)
            {
                projectQuery = projectQuery.Skip(input.Skip).Take(input.PageSize);
            }

            result.Items = projectQuery;
            return result;
        }

        /// <summary>
        /// App Hiển thị dự án
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IQueryable<AppViewListProjectDto> AppFindOpenSellProject(AppFindProjectDto dto, List<int> listTradingIds, int? investorId = null)
        {
            _logger.LogInformation($"{nameof(RstProjectEFRepository)}->{nameof(AppFindOpenSellProject)}: input = {JsonSerializer.Serialize(dto)}; listTradingIds = {JsonSerializer.Serialize(listTradingIds)}");

            var haveRedBookStatus = new int[] { RstRedBookTypes.HasRedBook, RstRedBookTypes.HasRedBook50Year };
            var projectFavourite = _epicSchemaDbContext.RstProjectFavourites.Where(pf => pf.InvestorId == investorId);

            var query = (from op in _epicSchemaDbContext.RstOpenSells.Where(x => (listTradingIds.Count == 0 || listTradingIds.Contains(x.TradingProviderId)) && x.Deleted == YesNo.NO && x.IsShowApp == YesNo.YES)
                         from opd in _epicSchemaDbContext.RstOpenSellDetails.Where(x => x.OpenSellId == op.Id && x.Deleted == YesNo.NO)
                         from dpi in _epicSchemaDbContext.RstDistributionProductItems.Where(x => x.Id == opd.DistributionProductItemId && x.Deleted == YesNo.NO)
                         from dt in _epicSchemaDbContext.RstDistributions.Where(x => x.Id == dpi.DistributionId && x.Deleted == YesNo.NO)
                         from pi in _epicSchemaDbContext.RstProductItems.Where(x => x.Id == dpi.ProductItemId && x.Deleted == YesNo.NO)
                         from p in _epicSchemaDbContext.RstProjects.Where(x => x.Id == pi.ProjectId && x.Deleted == YesNo.NO)
                         from trading in _epicSchemaDbContext.TradingProviders.Where(x => x.TradingProviderId == op.TradingProviderId && x.Deleted == YesNo.NO).DefaultIfEmpty()
                         from bus in _epicSchemaDbContext.BusinessCustomers.Where(x => x.BusinessCustomerId == trading.BusinessCustomerId && x.Deleted == YesNo.NO).DefaultIfEmpty()
                         from province in _epicSchemaDbContext.Provinces.Where(x => x.Code == p.ProvinceCode).DefaultIfEmpty()
                         where op.Status == RstDistributionStatus.DANG_BAN && dt.Status == RstDistributionStatus.DANG_BAN &&
                            (dto.ProjectType == null || dto.ProjectType.Contains(p.ProjectType.ToString())) &&
                            (dto.ProductType == null || _epicSchemaDbContext.RstProjectTypes.Any(r => r.ProjectId == p.Id && r.ProjectType == RstProjectDetailTypes.ProductTypes && dto.ProductType.Contains(r.Type.ToString()))) &&
                            (dto.ProvinceCode == null || dto.ProvinceCode.Contains(p.ProvinceCode)) &&
                            (dto.RedBook == null || dto.RedBook.Contains(pi.RedBookType.ToString())) &&
                            (dto.MinSellingPrice == null || (p.MinSellingPrice >= dto.MinSellingPrice && dto.MinSellingPrice <= p.MaxSellingPrice)) &&
                            (dto.MaxSellingPrice == null || (p.MinSellingPrice <= dto.MaxSellingPrice)) &&
                            (dto.MinSellingPrice == null || dto.MaxSellingPrice == null || dto.MinSellingPrice <= dto.MaxSellingPrice) &&
                            (string.IsNullOrEmpty(dto.Keyword) || p.Name.ToLower().Contains(dto.Keyword.ToLower()) || p.Code.ToLower().Contains(dto.Keyword.ToLower())) &&
                            (string.IsNullOrEmpty(dto.IsOutstanding) || op.IsOutstanding == dto.IsOutstanding) 
                         select new
                         {
                             p,
                             openSellId = op.Id,
                             isOutstanding = op.IsOutstanding,
                             opd,
                             bus,
                             provinceName = province != null ? province.Name : null,
                             tradingId = trading.TradingProviderId,
                             viewCount = op.ViewCount,
                             redBook = _epicSchemaDbContext.RstProductItems.Any(x => haveRedBookStatus.Contains(x.RedBookType) && x.Deleted == YesNo.NO),
                             createdDate = op.CreatedDate
                         })
                         .GroupBy(x => new
                         {
                             x.openSellId,
                             x.tradingId,
                             tradingName = x.bus.Name,
                             x.p.Id,
                             x.p.Code,
                             x.p.Name,
                             x.p.MaxSellingPrice,
                             x.p.MinSellingPrice,
                             x.p.ExpectedSellingPrice,
                             x.p.LandArea,
                             x.p.Latitude,
                             x.p.Longitude,
                             x.p.Address,
                             x.redBook,
                             x.p.ProjectType,
                             x.provinceName,
                             x.p.ProvinceCode,
                             x.isOutstanding,
                             x.viewCount,
                             x.createdDate
                         })
                         .Select(x => new AppViewListProjectDto
                         {
                             Id = x.Key.Id,
                             TradingProviderId = x.Key.tradingId,
                             TradingProviderName = x.Key.tradingName,
                             Code = x.Key.Code,
                             Name = x.Key.Name,
                             MinSellingPrice = x.Key.MinSellingPrice,
                             MaxSellingPrice = x.Key.MaxSellingPrice,
                             ExpectedSellingPrice = x.Key.ExpectedSellingPrice,
                             ProjectType = x.Key.ProjectType,
                             RedBook = x.Key.redBook,
                             ProvinceCode = x.Key.ProvinceCode,
                             ProvinceName = x.Key.provinceName,
                             IsOutstanding = x.Key.isOutstanding,
                             LandArea = x.Key.LandArea,
                             Latitude = x.Key.Latitude,
                             Longitude = x.Key.Longitude,
                             Address = x.Key.Address,
                             OpenSellId = x.Key.openSellId,
                             NumberOfUnit = x.Count(),
                             ViewCount = x.Key.viewCount,
                             IsFavourite = projectFavourite.Where(e => e.OpenSellId == x.Key.openSellId).Any(),
                             CreatedDate = x.Key.createdDate,
                         })
                         .Where(e => dto.IsFavourite == null || dto.IsFavourite == e.IsFavourite)
                         .OrderByDescending(o => o.IsOutstanding).ThenByDescending(o => o.OpenSellId);
            return query;
        }

        /// <summary>
        /// App lấy tham số để client tạo bộ lọc
        /// </summary>
        /// <param name="listTradingIds"></param>
        /// <returns></returns>
        public AppGetParamsFindProjectDto AppGetParamsFindProject(List<int> listTradingIds)
        {
            _logger.LogInformation($"{nameof(RstProjectEFRepository)}->{nameof(AppFindOpenSellProject)}: listTradingIds = {JsonSerializer.Serialize(listTradingIds)}");

            var haveRedBookStatus = new int[] { RstRedBookTypes.HasRedBook, RstRedBookTypes.HasRedBook50Year };

            var query = (from op in _epicSchemaDbContext.RstOpenSells.Where(x => (listTradingIds.Count == 0 || listTradingIds.Contains(x.TradingProviderId)) && x.Deleted == YesNo.NO)
                         from opd in _epicSchemaDbContext.RstOpenSellDetails.Where(x => x.OpenSellId == op.Id && x.Deleted == YesNo.NO)
                         from dpi in _epicSchemaDbContext.RstDistributionProductItems.Where(x => x.Id == opd.DistributionProductItemId && x.Deleted == YesNo.NO)
                         from pi in _epicSchemaDbContext.RstProductItems.Where(x => x.Id == dpi.ProductItemId && x.Deleted == YesNo.NO)
                         from p in _epicSchemaDbContext.RstProjects.Where(x => x.Id == pi.ProjectId && x.Deleted == YesNo.NO)
                         from productType in _epicSchemaDbContext.RstProjectTypes.Where(x => x.ProjectId == p.Id && x.ProjectType == RstProjectDetailTypes.ProductTypes)
                         from trading in _epicSchemaDbContext.TradingProviders.Where(x => x.TradingProviderId == op.TradingProviderId && x.Deleted == YesNo.NO).DefaultIfEmpty()
                         from bus in _epicSchemaDbContext.BusinessCustomers.Where(x => x.BusinessCustomerId == trading.BusinessCustomerId && x.Deleted == YesNo.NO).DefaultIfEmpty()
                         from province in _epicSchemaDbContext.Provinces.Where(x => x.Code == p.ProvinceCode).DefaultIfEmpty()
                         where op.Status == RstDistributionStatus.DANG_BAN
                         select new { p, productType, provinceName = province.Name });

            var listProvince = query.Select(x => new AppGetParamsProvince { ProvinceCode = x.p.ProvinceCode, ProvinceName = x.provinceName }).Distinct();
            var listProjectType = query.Select(x => x.p.ProjectType).Distinct();
            var listProductTypes = query.Select(x => x.productType.Type).Distinct();
            var minSellPrice = query.Min(x => x.p.MinSellingPrice);
            var maxSellPrice = query.Max(x => x.p.MaxSellingPrice);


            return new AppGetParamsFindProjectDto
            {
                ListProjectTypes = listProjectType.ToList(),
                ListProvince = listProvince.ToList(),
                ListProductTypes = listProductTypes.ToList(),
                MaxSellingPrice = maxSellPrice,
                MinSellingPrice = minSellPrice,
            };
        }

        /// <summary>
        /// App lấy thông tin tab tổng quan của dự án
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public AppOverviewProjectDto AppGetOverviewProject(int id, int tradingProviderId)
        {
            var tradingProviderQuery = (from tra in _epicSchemaDbContext.TradingProviders.AsNoTracking()
                                        from bus in _epicSchemaDbContext.BusinessCustomers.AsNoTracking().Where(x => x.BusinessCustomerId == tra.BusinessCustomerId && x.Deleted == YesNo.NO)
                                        where tra.TradingProviderId == tradingProviderId && tra.Deleted == YesNo.NO
                                        select bus).FirstOrDefault();

            var query = from p in _epicSchemaDbContext.RstProjects.AsNoTracking().Where(x => x.Id == id && x.Deleted == YesNo.NO)
                        from owner in _epicSchemaDbContext.RstOwners.AsNoTracking().Where(x => x.Id == p.OwnerId && x.Deleted == YesNo.NO).DefaultIfEmpty()
                        from ownerBus in _epicSchemaDbContext.BusinessCustomers.AsNoTracking().Where(x => x.BusinessCustomerId == owner.BusinessCustomerId && x.Deleted == YesNo.NO).DefaultIfEmpty()
                        from guaranteeBank in _epicSchemaDbContext.RstProjectGuaranteeBanks.AsNoTracking().Where(x => x.ProjectId == p.Id).DefaultIfEmpty()
                        from bank in _epicSchemaDbContext.CoreBanks.AsNoTracking().Where(x => x.BankId == guaranteeBank.BankId).DefaultIfEmpty()
                        select new AppOverviewProjectDto
                        {
                            Id = id,
                            OwnerId = owner.Id,
                            OwnerName = ownerBus.Name,
                            TradingProviderName = tradingProviderQuery.Name,
                            TradingReferralCode = tradingProviderQuery.ReferralCodeSelf,
                            ContractorName = p.ContractorName,
                            ContractorDescription = p.ContractorDescription,
                            ContractorLink = p.ContractorLink,
                            OperatingUnit = p.OperatingUnit,
                            OperatingUnitDescription = p.OperatingUnitDescription,
                            OperatingUnitLink = p.OperatingUnitLink,
                            GuaranteeBankName = bank.BankName,
                            TotalInvestment = p.TotalInvestment,
                            LandArea = p.LandArea,
                            BuildingDensity = p.BuildingDensity,
                            StartDate = p.StartDate,
                            ExpectedHandoverTime = p.ExpectedHandoverTime,
                            ExpectedSellingPrice = p.ExpectedSellingPrice,
                            MaxSellingPrice = p.MaxSellingPrice,
                            MinSellingPrice = p.MinSellingPrice,
                            Longitude = p.Longitude,
                            Latitude = p.Latitude,
                            Address = p.Address,
                            OverviewContent = p.OverviewContent,
                            ContentType = p.ContentType,
                            Fanpage = tradingProviderQuery.Fanpage,
                            WebsiteTradingProvider = tradingProviderQuery.Website,
                            Website = p.Website,
                            FacebookLink = p.FacebookLink,
                            ProjectHotline = p.Phone
                        };
            return query.FirstOrDefault();
        }

        /// <summary>
        /// App lấy các dự án theo Chủ đầu tư và Đại lý (thông tin ngắn gọn)
        /// </summary>
        /// <param name="ownerId"></param>
        /// <param name="tradingProviderId"></param>
        /// <returns></returns>
        public List<AppRstProjectShortInfoDto> AppFindListProject(int ownerId, int tradingProviderId, int? projectIdCurrent = null)
        {
            var query = from p in _epicSchemaDbContext.RstProjects.Where(x => x.Deleted == YesNo.NO && x.OwnerId == ownerId && (projectIdCurrent == null || x.Id != projectIdCurrent)
                            && _epicSchemaDbContext.RstOpenSells.Any(o => o.ProjectId == x.Id && x.Deleted == YesNo.NO && o.TradingProviderId == tradingProviderId))
                        from media in _epicSchemaDbContext.RstProjectMedias.Where(x => x.ProjectId == p.Id && x.Location == RstMediaLocations.ANH_DAI_DIEN_DU_AN && x.Deleted == YesNo.NO).DefaultIfEmpty()
                        select new AppRstProjectShortInfoDto
                        {
                            Id = p.Id,
                            AvatarUrlImage = media.UrlImage,
                            AvatarUrlPath = media.UrlPath,
                            Code = p.Code,
                            Name = p.Name,
                        };
            return query.ToList();

        }
    }
}
