using AutoMapper;
using EPIC.BondDomain.Interfaces;
using EPIC.BondRepositories;
using EPIC.CoreDomain.Interfaces;
using EPIC.CoreRepositories;
using EPIC.DataAccess.Base;
using EPIC.Entities;
using EPIC.Entities.Dto.ManagerInvestor;
using EPIC.GarnerDomain.Interfaces;
using EPIC.GarnerRepositories;
using EPIC.InvestDomain.Interfaces;
using EPIC.InvestRepositories;
using EPIC.RealEstateRepositories;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.CoreDomain.Implements
{
    public class AssetManagerServices : IAssetManagerServices
    {
        private readonly EpicSchemaDbContext _dbContext;
        private readonly ILogger<AssetManagerServices> _logger;
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;
        private readonly AssetManagerRepository _assetManagerRepository;
        private readonly BondOrderRepository _bondOrderRepository;
        private readonly BondSecondaryRepository _productBondSecondaryRepository;
        private readonly InvestOrderRepository _investOrderRepository;
        private readonly GarnerOrderEFRepository _garnerOrderEFRepository;
        private readonly InvestPolicyRepository _policyRepository;
        private readonly RstOrderEFRepository _rstOrderEFRepository;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IBondSharedService _bondSharedService;
        private readonly IInvestSharedServices _investSharedServices;
        private readonly IGarnerFormulaServices _garnerFormulaServices;
        private readonly IMapper _mapper;

        public AssetManagerServices(
            EpicSchemaDbContext dbContext,
            ILogger<AssetManagerServices> logger,
            IConfiguration configuration,
            DatabaseOptions databaseOptions,
            IHttpContextAccessor httpContext,
            IBondSharedService bondSharedService,
            IInvestSharedServices investSharedServices,
            IGarnerFormulaServices garnerFormulaServices,
            IMapper mapper)
        {
            _dbContext = dbContext;
            _logger = logger;
            _configuration = configuration;
            _connectionString = databaseOptions.ConnectionString;
            _assetManagerRepository = new AssetManagerRepository(_connectionString, _logger);
            _bondOrderRepository = new BondOrderRepository(_connectionString, _logger);
            _productBondSecondaryRepository = new BondSecondaryRepository(_connectionString, _logger);
            _investOrderRepository = new InvestOrderRepository(_connectionString, _logger);
            _garnerOrderEFRepository = new GarnerOrderEFRepository(_dbContext, _logger);
            _policyRepository = new InvestPolicyRepository(_connectionString, _logger);
            _rstOrderEFRepository = new RstOrderEFRepository(dbContext, logger);
            _httpContext = httpContext;
            _bondSharedService = bondSharedService;
            _investSharedServices = investSharedServices;
            _garnerFormulaServices = garnerFormulaServices;
            _mapper = mapper;
        }

        /// <summary>
        /// Màn quản lý tổng quan của nhà đầu tư cá nhân
        /// </summary>
        /// <returns></returns>
        public AssetManagerDto AssetManagerInvestor()
        {
            var investorId = CommonUtils.GetCurrentInvestorId(_httpContext);
            var result = new AssetManagerDto();
            //var assetBond = _assetManagerRepository.AppBondOrderInvestorQuantity(investorId);
            var assetInvest = _assetManagerRepository.AppInvestOrderInvestorQuantity(investorId);
            result.AssetBond = assetInvest;
            result.AssetGarner =  InvestManager().AssetManagerGarner.TotalValue;
            result.TotalValue = result.AssetBond + result.AssetGarner;
            result.AssetRealEstate = _rstOrderEFRepository.AssetRealEstateOrder(investorId);
            //Dòng tiền giao dịch

            result.TradingRecently = new();
            // Lấy dòng tiền từ Invest và Bond
            result.TradingRecently.AddRange( _assetManagerRepository.TradingRecently(investorId));

            // Lấy dòng tiền từ Garner
            result.TradingRecently.AddRange(_garnerFormulaServices.GarnerTradingRecently(investorId));
            result.TradingRecently = result.TradingRecently.OrderByDescending(t => t.TranDate).Take(5).ToList();
            result.AllProfit = InvestManager().AssetManagerBond.ProfitNow + InvestManager().AssetManagerInvest.ProfitNow + InvestManager().AssetManagerGarner.ProfitNow;
            return result;
        }

        /// <summary>
        /// Màn xem quản lý đầu tư của nhà đầu tư cá nhân
        /// </summary>
        /// <returns></returns>
        public InvestManagerDto InvestManager()
        {
            var investorId = CommonUtils.GetCurrentInvestorId(_httpContext);
            var result = new InvestManagerDto();
            /*var bondOrder = _bondOrderRepository.AppGetAll(investorId, null).Where(o => o.Status == OrderStatus.DANG_DAU_TU);
            if (bondOrder != null)
            {
                decimal bondMoney = 0;
                decimal profitNow = 0;
                foreach (var item in bondOrder)
                {
                    var bondOrderFind = _bondOrderRepository.FindById(item.OrderId, null);
                    if (bondOrderFind != null)
                    {
                        var bondPolicyDetail = _productBondSecondaryRepository.FindPolicyDetailById(bondOrderFind.PolicyDetailId, bondOrderFind.TradingProviderId);
                        if (bondPolicyDetail != null )
                        {
                            DateTime ngayDauTu = DateTime.Now.Date;
                            if (bondOrderFind.PaymentFullDate != null)
                            {
                                ngayDauTu = bondOrderFind.PaymentFullDate.Value;
                            }
                            DateTime ngayDaoHan = _bondSharedService.CalculateDueDate(bondPolicyDetail, ngayDauTu);
                            var profitNowItem = _bondSharedService.ProfitNow(ngayDauTu, ngayDaoHan, bondPolicyDetail.Profit, bondOrderFind.TotalValue);
                            bondMoney += item.TotalValue ?? 0;
                            profitNow += profitNowItem;
                        }    
                    }    
                }
                result.AssetManagerBond = new();
                result.AssetManagerBond.TotalValue = bondMoney;
                result.AssetManagerBond.ProfitNow = profitNow;
            }*/

            result.AssetManagerBond = new();
            var investOrder = _investOrderRepository.AppGetAll(investorId, null);
            if (investOrder != null)
            {
                decimal investMoney = 0;
                decimal profitNow = 0;
                foreach (var item in investOrder)
                {
                    var investOrderFind = _investOrderRepository.FindById(item.Id, null);
                    if(investOrderFind != null)
                    {
                        var investPolicyDetail = _policyRepository.FindPolicyDetailById(investOrderFind.PolicyDetailId, investOrderFind.TradingProviderId);
                        if (investPolicyDetail != null)
                        {
                            DateTime ngayDauTu = DateTime.Now.Date;
                            if (investOrderFind.PaymentFullDate != null)
                            {
                                ngayDauTu = investOrderFind.PaymentFullDate.Value;
                            }
                            var distribution = _dbContext.InvestDistributions.FirstOrDefault(r => r.Id == investOrderFind.DistributionId && r.Deleted == YesNo.NO);
                            DateTime ngayDaoHan = investOrderFind.DueDate ?? _investSharedServices.CalculateDueDate(investPolicyDetail, ngayDauTu.Date, distribution?.CloseCellDate);
                            var profitNowItem = _investSharedServices.ProfitNow(ngayDauTu, ngayDaoHan, investPolicyDetail.Profit ?? 0, investOrderFind.TotalValue);

                            investMoney += item.TotalValue ?? 0;
                            profitNow += profitNowItem;
                        }    
                    }    
                }
                result.AssetManagerInvest = new();
                result.AssetManagerInvest.TotalValue = investMoney;
                result.AssetManagerInvest.ProfitNow = profitNow;
            }
            result.AssetManagerStock = new();
            result.AssetManagerDeposit = new();
            result.AssetManagerGarner = new();

            var garnerOrder = _garnerOrderEFRepository.AppInvestorGetAllOrder(investorId, OrderStatus.DANG_DAU_TU);
            if (garnerOrder != null)
            {
                decimal investMoney = 0;
                decimal profitNow = 0;
                foreach (var item in garnerOrder)
                {
                    var calculateProfit = _garnerFormulaServices.CaculateProfitNow(item.Id);
                    if (calculateProfit != null)
                    {
                        investMoney += item.TotalValue;
                        profitNow += calculateProfit.ActualProfit;
                    }    
                }
                result.AssetManagerGarner.TotalValue = investMoney;
                result.AssetManagerGarner.ProfitNow = profitNow;
            }
            result.AllProfit = result.AssetManagerBond.ProfitNow + result.AssetManagerInvest.ProfitNow + result.AssetManagerGarner.ProfitNow;
            result.TotalValue = result.AssetManagerBond.TotalValue + result.AssetManagerInvest.TotalValue + result.AssetManagerGarner.TotalValue;
            return result;
        }

    }
}
