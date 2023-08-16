using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Spreadsheet;
using EPIC.CoreEntities.Dto.Sale;
using EPIC.CoreEntities.Dto.SaleInvestor;
using EPIC.DataAccess;
using EPIC.DataAccess.Base;
using EPIC.DataAccess.Models;
using EPIC.Entities.DataEntities;
using EPIC.Entities.Dto.Department;
using EPIC.Entities.Dto.ExportReport;
using EPIC.Entities.Dto.ManagerInvestor;
using EPIC.Entities.Dto.Sale;
using EPIC.Entities.Dto.SaleAppStatistical;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Core;
using EPIC.Utils.ConstantVariables.Shared;
using EPIC.Utils.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MySqlX.XDevAPI.Common;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.CoreRepositories
{
    public class SaleEFRepository : BaseEFRepository<Sale>
    {
        private const string PROC_GET_SALE_IN_BUSINESS_SALE_SUB_EF = DbSchemas.EPIC + ".PKG_CORE_SALE.PROC_GET_SALE_IN_BUSINESS_SALE_SUB_EF";
        private const string PROC_SALE_BY_REFERALL_CODE = DbSchemas.EPIC + ".PKG_CORE_SALE.PROC_SALE_BY_REFERALL_CODE";
        public SaleEFRepository(EpicSchemaDbContext dbContext, ILogger logger) : base(dbContext, logger, Sale.SEQ)
        {
        }

        /// <summary>
        /// Kiểm tra xem nhà đầu tư đã là sale hay chưa
        /// </summary>
        /// <param name="investorId"></param>
        /// <returns></returns>
        public Sale FindSaleByInvestor(int investorId)
        {
            return _dbSet.FirstOrDefault(s => s.InvestorId == investorId && s.Deleted == YesNo.NO);
        }

        /// <summary>
        /// LấY danh sách Đại lý mà Sale đang tham gia
        /// </summary>
        /// <param name="saleId"></param>
        /// <returns></returns>
        public List<AppListTradingProviderDto> ListTradingProviderBySale(int saleId)
        {
            var result = from sale in _dbSet
                         join saleTrading in _epicSchemaDbContext.SaleTradingProviders on sale.SaleId equals saleTrading.SaleId
                         join tradingProvider in _epicSchemaDbContext.TradingProviders on saleTrading.TradingProviderId equals tradingProvider.TradingProviderId
                         join businessCustomer in _epicSchemaDbContext.BusinessCustomers on tradingProvider.BusinessCustomerId equals businessCustomer.BusinessCustomerId
                         where sale.SaleId == saleId && sale.Deleted == YesNo.NO && saleTrading.Deleted == YesNo.NO && tradingProvider.Deleted == YesNo.NO && businessCustomer.Deleted == YesNo.NO
                         && saleTrading.Status == Utils.Status.ACTIVE
                         select new AppListTradingProviderDto
                         {
                             TradingProviderId = tradingProvider.TradingProviderId,
                             TradingProviderName = businessCustomer.Name,
                             TradingProviderAliasName = tradingProvider.AliasName,
                             ShortName = businessCustomer.ShortName,
                             AvatarImageUrl = businessCustomer.AvatarImageUrl,
                             Status = saleTrading.Status,
                             SignDate = saleTrading.CreatedDate,
                             DeactiveDate = saleTrading.DeactiveDate
                         };
            return result.ToList();
        }

        /// <summary>
        /// Lấy sale theo mã giới thiệu của (investor/business-customer)
        /// </summary>
        /// <param name="referralCodeSelf"></param>
        /// <returns></returns>
        public Sale GetSaleByReferralCodeSelf(string referralCodeSelf)
        {
            var query = from cs in _epicSchemaDbContext.Sales
                        from i in _epicSchemaDbContext.Investors
                                    .Where(x => x.InvestorId == (cs.InvestorId ?? 0) && x.Deleted == YesNo.NO).DefaultIfEmpty()
                        from bc in _epicSchemaDbContext.BusinessCustomers
                                    .Where(x => x.BusinessCustomerId == cs.BusinessCustomerId && x.Deleted == YesNo.NO).DefaultIfEmpty()
                        where cs.Deleted == YesNo.NO && referralCodeSelf != null && (i.ReferralCodeSelf == referralCodeSelf || bc.ReferralCodeSelf == referralCodeSelf)
                        select cs;
            return query.FirstOrDefault();
        }

        /// <summary>
        /// Lấy tên của sale
        /// </summary>
        /// <param name="referralCodeSelf"></param>
        /// <returns></returns>
        public SaleInfoNameDto FindSaleName(string referralCodeSelf)
        {
            var result = new SaleInfoNameDto();
            var findSale = GetSaleByReferralCodeSelf(referralCodeSelf);
            if (findSale != null && findSale.InvestorId != null)
            {
                var investor = _epicSchemaDbContext.InvestorIdentifications.Where(i => i.InvestorId == findSale.InvestorId && i.Deleted == YesNo.NO && i.Status == Status.ACTIVE)
                                    .OrderByDescending(x => x.IsDefault).ThenByDescending(x => x.Id).FirstOrDefault();
                result.SaleId = findSale.SaleId;
                result.Name = investor?.Fullname;
            }
            else if (findSale != null && findSale.BusinessCustomerId != null)
            {
                var businessCustomer = _epicSchemaDbContext.BusinessCustomers.Where(b => b.BusinessCustomerId == findSale.BusinessCustomerId).FirstOrDefault();
                if (businessCustomer != null)
                {
                    result.SaleId = findSale.SaleId;
                    result.Name = businessCustomer.Name;
                }
            }
            return result;
        }

        /// <summary>
        /// Tìm sale của sale bán hộ là đại lý
        /// </summary>
        /// <param name="referralCode"></param>
        /// <param name="tradingProviderId"></param>
        /// <returns></returns>
        public SaleInBusinessCustomerSaleSubDto FindSaleInBusinessCustomerSaleSub(string referralCode, int tradingProviderId)
        {
            _logger.LogInformation($"{System.Reflection.MethodBase.GetCurrentMethod().Name}: referralCode = {referralCode}, tradingProviderId = {tradingProviderId};");

            var conveted = ObjectToParamAndQueryList(PROC_GET_SALE_IN_BUSINESS_SALE_SUB_EF, new
            {
                pv_REFERRAL_CODE = referralCode,
                pv_TRADING_PROVIDER_ID = tradingProviderId,
            });
            try
            {
                var result = _dbContext.Set<SaleInBusinessCustomerSaleSubDto>().FromSqlRaw(conveted.SqlQuery, conveted.Parameters).ToList();
                return result.FirstOrDefault();
            }
            catch (OracleException ex)
            {
                throw ThrowOracleUserException(ex);
            }
        }

        public AppSaleByReferralCodeDto FindSaleByReferralCode(string referralCode, int? tradingProviderId, string phone = null)
        {
            _logger.LogInformation($"{nameof(FindSaleByReferralCode)}: referralCode = {referralCode}, tradingProviderId = {tradingProviderId};");

            var conveted = ObjectToParamAndQueryList(PROC_SALE_BY_REFERALL_CODE, new
            {
                pv_REFERRAL_CODE = referralCode,
                pv_TRADING_PROVIDER_ID = tradingProviderId,
                pv_PHONE = phone
            });

            try
            {
                var result = _dbContext.Set<AppSaleByReferralCodeDto>().FromSqlRaw(conveted.SqlQuery, conveted.Parameters).ToList();
                return result.FirstOrDefault().ThrowIfNull(ErrorCode.CoreSaleNotFound, $"Không tìm thấy sale có mã giới thiệu \"{referralCode}\"");
            }
            catch (OracleException ex)
            {
                throw ThrowOracleUserException(ex);
            }
        }

        /// <summary>
        /// Tìm sale theo mã giới thiệu
        /// Tìm xem có là sale hay không, chưa tìm theo đại lý
        /// </summary>
        /// <param name="referralCode"></param>
        /// <returns></returns>
        public Sale FindSaleByReferralCode(string referralCode)
        {
            _logger.LogInformation($"{nameof(FindSaleByReferralCode)}: referralCode = {referralCode};");

            var findSale = (from sale in _dbSet
                            join investor in _epicSchemaDbContext.Investors on sale.InvestorId equals investor.InvestorId into listInvestor
                            from investor in listInvestor.DefaultIfEmpty()
                            join businessCustomer in _epicSchemaDbContext.BusinessCustomers on sale.BusinessCustomerId equals businessCustomer.BusinessCustomerId into listBusinessCustomer
                            from businessCustomer in listBusinessCustomer.DefaultIfEmpty()
                            where ((investor.ReferralCodeSelf == referralCode && investor.Deleted == YesNo.NO) || (referralCode == businessCustomer.ReferralCodeSelf && businessCustomer.Deleted == YesNo.NO))
                            && sale.Deleted == YesNo.NO
                            select sale).FirstOrDefault();
            return findSale;
        }

        public ViewSaleDto FindSaleTradingProviderByReferralCode(string referralCode, int tradingProviderId)
        {
            _logger.LogInformation($"{nameof(FindSaleTradingProviderByReferralCode)}: referralCode = {referralCode}, tradingProviderId = {tradingProviderId};");
            var findSaleByReferralCode = FindSaleByReferralCode(referralCode);
            var saleInTrading = FindSaleTradingProvider(findSaleByReferralCode?.SaleId ?? 0, tradingProviderId);
            return saleInTrading;
        }

        /// <summary>
        /// Tìm thông tin sale theo đại lý
        /// </summary>
        /// <param name="saleId"></param>
        /// <param name="tradingProviderId"></param>
        /// <returns></returns>
        public ViewSaleDto FindSaleTradingProvider(int saleId, int tradingProviderId)
        {
            _logger.LogInformation($"{nameof(FindSaleTradingProvider)}: saleId = {saleId}, tradingProviderId = {tradingProviderId};");

            var findSaleInTrading = (from saleTrading in _epicSchemaDbContext.SaleTradingProviders
                                     join sale in _dbSet on saleTrading.SaleId equals sale.SaleId
                                     join saleDepartment in _epicSchemaDbContext.DepartmentSales on new { saleTrading.SaleId, saleTrading.TradingProviderId } equals new { saleDepartment.SaleId, saleDepartment.TradingProviderId }
                                     join department in _epicSchemaDbContext.Departments on saleDepartment.DepartmentId equals department.DepartmentId
                                     where saleTrading.SaleId == saleId && saleTrading.TradingProviderId == tradingProviderId
                                     && saleTrading.Deleted == YesNo.NO && sale.Deleted == YesNo.NO && saleDepartment.Deleted == YesNo.NO && department.Deleted == YesNo.NO
                                     select new ViewSaleDto
                                     {
                                         SaleId = saleId,
                                         InvestorId = sale.InvestorId,
                                         BusinessCustomerId = sale.BusinessCustomerId,
                                         SaleType = saleTrading.SaleType,
                                         Status = saleTrading.Status,
                                         EmployeeCode = saleTrading.EmployeeCode,
                                         DepartmentName = department.DepartmentName,
                                         DepartmentId = department.DepartmentId,
                                         SaleParentId = saleTrading.SaleParentId,
                                         ContractCode = saleTrading.ContractCode,
                                         InvestorBankAccId = saleTrading.InvestorBankAccId,
                                         BusinessCustomerBankAccId = saleTrading.BusinessCustomerBankAccId,
                                         DeactiveDate = saleTrading.DeactiveDate,
                                         ManagerDepartmentId = department.ManagerId,
                                         AutoDirectional = sale.AutoDirectional,
                                         SaleTradingStatus = saleTrading.Status,
                                     }).FirstOrDefault();
            return findSaleInTrading;
        }

        /// <summary>
        /// Tìm thông tin sale, tìm cả sale bán hộ
        /// Nếu mã tư vấn không thuộc đại lý, hoặc tìm thấy nhưng sale đang Deactive, 
        /// Tìm xuống kênh bán hộ
        /// </summary>
        /// <param name="referralCode"></param>
        /// <param name="tradingProviderId"></param>
        /// <returns></returns>
        public SaleInBusinessCustomerSaleSubDto AppFindSaleOrderByReferralCode(string referralCode, int tradingProviderId)
        {
            var result = new SaleInBusinessCustomerSaleSubDto();
            // Tìm kiếm thông tin sale theo mã giới thiệu
            var findSaleByReferralCode = FindSaleByReferralCode(referralCode);
            if (findSaleByReferralCode == null)
            {
                ThrowException(ErrorCode.CoreSaleNotFound);
            }
            result.SaleId = findSaleByReferralCode.SaleId;
            // Tìm kiếm sale trong đại lý
            var findSaleInTradingProvider = FindSaleTradingProvider(findSaleByReferralCode.SaleId, tradingProviderId);
            if (findSaleInTradingProvider == null)
            {
                // Tìm đến kênh bán hộ
                var findSaleSub = FindSaleInBusinessCustomerSaleSub(referralCode, tradingProviderId);
                if (findSaleSub == null)
                {
                    ThrowException(ErrorCode.CoreSaleNotFound);
                }
                result.ReferralCode = findSaleSub.ReferralCode;
                result.ReferralCodeSub = referralCode;
                result.DepartmentId = findSaleSub.DepartmentId;
                result.DepartmentIdSub = findSaleSub.DepartmentIdSub;
            }
            else
            {
                result.DepartmentId = findSaleInTradingProvider.DepartmentId;
                result.ReferralCode = referralCode;

                // Nếu sale đang bị khó thì tìm đến kênh bán hộ
                if (findSaleInTradingProvider.Status == Status.INACTIVE)
                {
                    var findSaleSub = FindSaleInBusinessCustomerSaleSub(referralCode, tradingProviderId);
                    // Nếu tìm thấy thì gán giá trị
                    if (findSaleSub != null)
                    {
                        result.ReferralCode = findSaleSub.ReferralCode;
                        result.ReferralCodeSub = referralCode;
                        result.DepartmentId = findSaleSub.DepartmentId;
                        result.DepartmentIdSub = findSaleSub.DepartmentIdSub;
                    }
                    else
                    {
                        // Mã giới thiệu hiện không hợp lệ
                        ThrowException(ErrorCode.CoreSaleStatusIllegal);
                    }
                }
            }
            return result;
        }
        public Entities.DataEntities.Department FindDepartmentById(int departmentId, int tradingProviderId)
        {
            return _epicSchemaDbContext.Departments.FirstOrDefault(d => d.DepartmentId == departmentId && d.TradingProviderId == tradingProviderId && d.Deleted == YesNo.NO);
        }

        /// <summary>
        /// Tìm sale cá nhân bằng mã giới thiệu
        /// </summary>
        /// <param name="saleReferralCode"></param>
        /// <param name="isInvestor"></param>
        /// <returns></returns>
        public SaleInfoExcelDto FindSaleByRefferCode(string saleReferralCode, bool isInvestor, int? tradingProviderId)
        {
            if (isInvestor)
            {
                return (from investors in _epicSchemaDbContext.Investors
                        join sales in _epicSchemaDbContext.Sales on investors.InvestorId equals sales.InvestorId
                        join departmentSales in _epicSchemaDbContext.DepartmentSales on sales.SaleId equals departmentSales.SaleId
                        join departments in _epicSchemaDbContext.Departments on departmentSales.DepartmentId equals departments.DepartmentId
                        join investorIdentifications in _epicSchemaDbContext.InvestorIdentifications on sales.InvestorId equals investorIdentifications.InvestorId
                        where sales.Deleted == YesNo.NO && investors.Deleted == YesNo.NO && sales.Deleted == YesNo.NO
                               && departmentSales.Deleted == YesNo.NO && departments.Deleted == YesNo.NO
                               && investorIdentifications.Deleted == YesNo.NO
                               && (tradingProviderId == null || departmentSales.TradingProviderId == tradingProviderId)
                               && investors.ReferralCodeSelf == saleReferralCode
                        select new SaleInfoExcelDto
                        {
                            DepartmentName = departments.DepartmentName,
                            Fullname = investorIdentifications.Fullname,
                            DepartmentId = departments.DepartmentId,
                            InvestorId = investors.InvestorId,
                        }).FirstOrDefault();
            }
            else
            {
                return (from businessCustomers in _epicSchemaDbContext.BusinessCustomers
                        join sales in _epicSchemaDbContext.Sales on businessCustomers.BusinessCustomerId equals sales.BusinessCustomerId
                        join saleTradings in _epicSchemaDbContext.SaleTradingProviders on sales.SaleId equals saleTradings.SaleId
                        join departmentSales in _epicSchemaDbContext.DepartmentSales on saleTradings.SaleId equals departmentSales.SaleId
                        join departments in _epicSchemaDbContext.Departments on departmentSales.DepartmentId equals departments.DepartmentId
                        where businessCustomers.Deleted == YesNo.NO && sales.Deleted == YesNo.NO
                               && departmentSales.Deleted == YesNo.NO && departments.Deleted == YesNo.NO
                               && businessCustomers.Deleted == YesNo.NO
                               && (tradingProviderId == null || saleTradings.TradingProviderId == tradingProviderId)
                               && departmentSales.TradingProviderId == tradingProviderId
                               && businessCustomers.ReferralCodeSelf == saleReferralCode
                        select new SaleInfoExcelDto
                        {
                            DepartmentName = departments.DepartmentName,
                            Name = businessCustomers.Name,
                            DepartmentId = departments.DepartmentId,
                            BusinessCustomerId = businessCustomers.BusinessCustomerId,
                        }).FirstOrDefault();
            }
        }

        public IQueryable<int> Check(int tradingProviderId)
        {
            var result = from tradingProviderSub in _epicSchemaDbContext.TradingProviders
                         join businessCustomerSub in _epicSchemaDbContext.BusinessCustomers on tradingProviderSub.BusinessCustomerId equals businessCustomerSub.BusinessCustomerId
                         join saleSub in _epicSchemaDbContext.Sales on businessCustomerSub.BusinessCustomerId equals saleSub.BusinessCustomerId
                         join saleTradings in _epicSchemaDbContext.SaleTradingProviders on saleSub.SaleId equals saleTradings.SaleId
                         join tradingProvider in _epicSchemaDbContext.TradingProviders on saleTradings.TradingProviderId equals tradingProvider.TradingProviderId
                         where businessCustomerSub.Deleted == YesNo.NO && saleSub.Deleted == YesNo.NO && tradingProviderSub.Deleted == YesNo.NO
                         && saleTradings.Deleted == YesNo.NO && saleTradings.Status == Status.ACTIVE && tradingProvider.Deleted == YesNo.NO
                         && tradingProviderSub.TradingProviderId == tradingProviderId
                         select tradingProvider.TradingProviderId;
            return result;
        }

        /// <summary>
        /// Tìm những đại lý mà đại lý hiện tại đang bán hộ
        /// </summary>
        /// <param name="tradingProviderId"></param>
        /// <returns></returns>
        public List<int> FindTradingProviderBanHo(int tradingProviderId)
        {
            var query = from tradingProvider in _epicSchemaDbContext.TradingProviders
                        join businessCustomer in _epicSchemaDbContext.BusinessCustomers on tradingProvider.BusinessCustomerId equals businessCustomer.BusinessCustomerId
                        join saleBusiness in _epicSchemaDbContext.Sales on businessCustomer.BusinessCustomerId equals saleBusiness.BusinessCustomerId
                        join saleTrading in _epicSchemaDbContext.SaleTradingProviders on saleBusiness.SaleId equals saleTrading.SaleId
                        where
                           tradingProvider.TradingProviderId == tradingProviderId
                           && tradingProvider.Deleted == YesNo.NO
                           && businessCustomer.Deleted == YesNo.NO
                           && saleBusiness.Deleted == YesNo.NO
                           && saleTrading.Deleted == YesNo.NO
                           && saleTrading.Status == Status.ACTIVE
                           && saleTrading.TradingProviderId != tradingProviderId
                        select saleTrading.TradingProviderId;
            return query.ToList();
        }

        public SaleTemp AppCheckInvetorSaleSource(int investorId, int tradingProdviderId)
        {
            var query = _epicSchemaDbContext.SaleTemps.FirstOrDefault(s => s.InvestorId == investorId && s.TradingProviderId == tradingProdviderId && s.Deleted == YesNo.NO && s.InvestorId != null && s.Status == SaleTempStatus.DA_DUYET);
            return query;
        }

        // Lấy thông tin Sale có trọng phòng ban
        public SaleInfoInDepartmentDto SaleInfoInDepartment(int saleId, int tradingProviderId)
        {
            var result = from sale in _dbSet.AsNoTracking()
                         join saleTradingProvider in _epicSchemaDbContext.SaleTradingProviders.AsNoTracking() on sale.SaleId equals saleTradingProvider.SaleId
                         join departmentSale in _epicSchemaDbContext.DepartmentSales.AsNoTracking() on saleTradingProvider.SaleId equals departmentSale.SaleId
                         join department in _epicSchemaDbContext.Departments.AsNoTracking() on departmentSale.DepartmentId equals department.DepartmentId
                         join investor in _epicSchemaDbContext.Investors.AsNoTracking().Where(i => i.Deleted == YesNo.NO) on sale.InvestorId equals investor.InvestorId into investors
                         from investor in investors.DefaultIfEmpty()
                         join businessCustomer in _epicSchemaDbContext.BusinessCustomers.AsNoTracking().Where(i => i.Deleted == YesNo.NO) on sale.BusinessCustomerId equals businessCustomer.BusinessCustomerId into businessCustomers
                         from businessCustomer in businessCustomers.DefaultIfEmpty()
                         where saleTradingProvider.TradingProviderId == tradingProviderId && sale.SaleId == saleId
                         && sale.Deleted == YesNo.NO && saleTradingProvider.Status == Status.ACTIVE && saleTradingProvider.Deleted == YesNo.NO
                         && department.Deleted == YesNo.NO && departmentSale.Deleted == YesNo.NO && departmentSale.TradingProviderId == tradingProviderId
                         select new SaleInfoInDepartmentDto()
                         {
                             SaleId = sale.SaleId,
                             Status = saleTradingProvider.Status,
                             SaleType = saleTradingProvider.SaleType,
                             ReferralCode = (businessCustomer != null) ? businessCustomer.ReferralCodeSelf : investor.ReferralCodeSelf,
                             SignDate = saleTradingProvider.CreatedDate,
                             BusinessCustomerId = sale.BusinessCustomerId,
                             InvestorId = sale.InvestorId,
                             EmployeeCode = saleTradingProvider.EmployeeCode,
                             DepartmentId = department.DepartmentId,
                             DepartmentName = department.DepartmentName,
                             SaleParentId = saleTradingProvider.SaleParentId,
                             StartInDepartmentDate = departmentSale.CreatedDate ?? saleTradingProvider.CreatedDate,
                             ManagerStartDate = (saleId == department.ManagerId) ? department.ManagerStartDate :
                                                (saleId == department.ManagerId2) ? department.Manager2StartDate :
                                                null,
                         };
            return result.FirstOrDefault();
        }

        /// <summary>
        /// Xem danh sách Sale con trực tiếp qua SaleParentId và Phòng ban DepartmentId
        /// </summary>
        public IQueryable<SaleInfoAppDto> GetAllSaleChild(int saleId, int departmentId, int tradingProviderId, int? saleType, DateTime? startDate = null, DateTime? endDate = null)
        {
            var result = from sale in _dbSet.AsNoTracking()
                         join saleTradingProvider in _epicSchemaDbContext.SaleTradingProviders.AsNoTracking() on sale.SaleId equals saleTradingProvider.SaleId
                         join departmentSale in _epicSchemaDbContext.DepartmentSales.AsNoTracking() on sale.SaleId equals departmentSale.SaleId
                         join investor in _epicSchemaDbContext.Investors.AsNoTracking().Where(i => i.Deleted == YesNo.NO) on sale.InvestorId equals investor.InvestorId into investors
                         from investor in investors.DefaultIfEmpty()
                         join businessCustomer in _epicSchemaDbContext.BusinessCustomers.AsNoTracking().Where(i => i.Deleted == YesNo.NO) on sale.BusinessCustomerId equals businessCustomer.BusinessCustomerId into businessCustomers
                         from businessCustomer in businessCustomers.DefaultIfEmpty()
                         where saleTradingProvider.TradingProviderId == tradingProviderId && saleTradingProvider.SaleParentId == saleId
                         && investor.Deleted == YesNo.NO && sale.Deleted == YesNo.NO && saleTradingProvider.Status == Status.ACTIVE && saleTradingProvider.Deleted == YesNo.NO
                         && (saleType == null || saleTradingProvider.SaleType == saleType) && (startDate == null || (saleTradingProvider.CreatedDate != null && saleTradingProvider.CreatedDate.Value.Date >= startDate.Value.Date))
                         && (endDate == null || (saleTradingProvider.CreatedDate != null && saleTradingProvider.CreatedDate.Value.Date <= endDate.Value.Date)) && departmentSale.DepartmentId == departmentId && departmentSale.Deleted == YesNo.NO
                         && departmentSale.TradingProviderId == tradingProviderId
                         select new SaleInfoAppDto()
                         {
                             SaleId = sale.SaleId,
                             Status = saleTradingProvider.Status,
                             SaleType = saleTradingProvider.SaleType,
                             AvatarImageUrl = (businessCustomer != null) ? businessCustomer.AvatarImageUrl : investor.AvatarImageUrl,
                             ReferralCode = (businessCustomer != null) ? businessCustomer.ReferralCodeSelf : investor.ReferralCodeSelf,
                             StartDate = saleTradingProvider.CreatedDate,
                             EmployeeCode = saleTradingProvider.EmployeeCode,
                             SaleName = (businessCustomer != null) ? businessCustomer.Name :
                                        (_epicSchemaDbContext.InvestorIdentifications
                                        .Where(ii => ii.InvestorId == investor.InvestorId && ii.Deleted == YesNo.NO)
                                        .OrderByDescending(ii => ii.IsDefault)
                                        .ThenByDescending(ii => ii.Id)
                                        .Select(ii => ii.Fullname)
                                        .FirstOrDefault())
                         };
            return result;
        }

        public IQueryable<SaleInfoAppDto> GetAllSaleInfoInDepartment(int departmentId, int tradingProviderId, int? saleType, DateTime? startDate = null, DateTime? endDate = null)
        {
            var result = from sale in _dbSet.AsNoTracking()
                         join saleTradingProvider in _epicSchemaDbContext.SaleTradingProviders.AsNoTracking() on sale.SaleId equals saleTradingProvider.SaleId
                         join departmentTrading in _epicSchemaDbContext.DepartmentSales.AsNoTracking() on saleTradingProvider.SaleId equals departmentTrading.SaleId
                         join investor in _epicSchemaDbContext.Investors.AsNoTracking().Where(i => i.Deleted == YesNo.NO) on sale.InvestorId equals investor.InvestorId into investors
                         from investor in investors.DefaultIfEmpty()
                         join businessCustomer in _epicSchemaDbContext.BusinessCustomers.AsNoTracking().Where(i => i.Deleted == YesNo.NO) on sale.BusinessCustomerId equals businessCustomer.BusinessCustomerId into businessCustomers
                         from businessCustomer in businessCustomers.DefaultIfEmpty()
                         where saleTradingProvider.TradingProviderId == tradingProviderId && departmentTrading.TradingProviderId == tradingProviderId
                         && investor.Deleted == YesNo.NO && sale.Deleted == YesNo.NO && saleTradingProvider.Status == Status.ACTIVE && saleTradingProvider.Deleted == YesNo.NO
                         && (saleType == null || saleTradingProvider.SaleType == saleType) && departmentTrading.DepartmentId == departmentId && departmentTrading.Deleted == YesNo.NO
                         && (startDate == null || saleTradingProvider.CreatedDate >= startDate) && (endDate == null || (saleTradingProvider.CreatedDate != null && saleTradingProvider.CreatedDate.Value.Date <= endDate))
                         select new SaleInfoAppDto()
                         {
                             SaleId = sale.SaleId,
                             InvestorId = sale.InvestorId,
                             Status = saleTradingProvider.Status,
                             SaleType = saleTradingProvider.SaleType,
                             AvatarImageUrl = (businessCustomer != null) ? businessCustomer.AvatarImageUrl : investor.AvatarImageUrl,
                             ReferralCode = (businessCustomer != null) ? businessCustomer.ReferralCodeSelf : investor.ReferralCodeSelf,
                             StartDate = saleTradingProvider.CreatedDate,
                             SaleName = (businessCustomer != null) ? businessCustomer.Name :
                                        (_epicSchemaDbContext.InvestorIdentifications
                                        .Where(ii => ii.InvestorId == investor.InvestorId && ii.Deleted == YesNo.NO)
                                        .OrderByDescending(ii => ii.IsDefault)
                                        .ThenByDescending(ii => ii.Id)
                                        .Select(ii => ii.Fullname)
                                        .FirstOrDefault())
                         };
            return result;
        }

        public IQueryable<RecursionSaleInDepartmentDto> GetAllSaleIdDepartment(int departmentId, int tradingProviderId, DateTime? startDate = null, DateTime? endDate = null)
        {
            var result = from sale in _dbSet.AsNoTracking()
                         join saleTradingProvider in _epicSchemaDbContext.SaleTradingProviders.AsNoTracking() on sale.SaleId equals saleTradingProvider.SaleId
                         join departmentTrading in _epicSchemaDbContext.DepartmentSales.AsNoTracking() on saleTradingProvider.SaleId equals departmentTrading.SaleId
                         where saleTradingProvider.TradingProviderId == tradingProviderId && departmentTrading.TradingProviderId == tradingProviderId
                         && sale.Deleted == YesNo.NO && saleTradingProvider.Status == Status.ACTIVE && saleTradingProvider.Deleted == YesNo.NO
                         && departmentTrading.DepartmentId == departmentId && departmentTrading.Deleted == YesNo.NO
                         && (startDate == null || saleTradingProvider.CreatedDate >= startDate) && (endDate == null || saleTradingProvider.CreatedDate <= endDate)
                         select new RecursionSaleInDepartmentDto()
                         {
                             SaleId = sale.SaleId,
                             InvestorId = sale.InvestorId,
                         };
            return result;
        }


        /// <summary>
        /// Lần ngược lên trên danh sách những đại lý là đại lý của đại lý đang xét (khi đại lý đang xét là saler doanh nghiệp)
        /// </summary>
        /// <returns></returns>
        public IQueryable<int> FindAllTradingUpFromSaleTrading(int tradingProviderId)
        {
            var result = from tradingProvider in _epicSchemaDbContext.TradingProviders
                         join businessCustomer in _epicSchemaDbContext.BusinessCustomers on tradingProvider.BusinessCustomerId equals businessCustomer.BusinessCustomerId
                         join sale in _epicSchemaDbContext.Sales on businessCustomer.BusinessCustomerId equals sale.BusinessCustomerId
                         join saleTradingProvider in _epicSchemaDbContext.SaleTradingProviders on sale.SaleId equals saleTradingProvider.SaleId
                         join tradingProviderParent in _epicSchemaDbContext.TradingProviders on saleTradingProvider.TradingProviderId equals tradingProviderParent.TradingProviderId
                         where tradingProvider.Deleted == YesNo.NO && businessCustomer.Deleted == YesNo.NO && sale.Deleted == YesNo.NO
                         && saleTradingProvider.Deleted == YesNo.NO && saleTradingProvider.Status == Status.ACTIVE && tradingProviderParent.Deleted == YesNo.NO
                         && tradingProvider.TradingProviderId == tradingProviderId
                         select tradingProviderParent.TradingProviderId;
            return result.Distinct();
        }

        /// <summary>
        /// Lần ngược lên trên danh sách những đại lý là đại lý của đại lý đang xét (khi đại lý đang xét là saler doanh nghiệp)
        /// Lần 4 cấp
        /// </summary>
        /// <returns></returns>
        public List<int> FindAllTradingUpFromSaleTrading4Cap(int tradingProviderId)
        {
            var result = new List<int>();

            var tradingUp1s = FindAllTradingUpFromSaleTrading(tradingProviderId);
            foreach (var itemUp1 in tradingUp1s)
            {
                result.Add(itemUp1);
                var tradingUp2s = FindAllTradingUpFromSaleTrading(itemUp1);
                foreach (var itemUp2 in tradingUp2s)
                {
                    result.Add(itemUp2);
                    var tradingUp3s = FindAllTradingUpFromSaleTrading(itemUp2);
                    foreach (var itemUp3 in tradingUp3s)
                    {
                        result.Add(itemUp3);
                    }
                }
            }
            return result.Distinct().ToList();
        }

        /// <summary>
        /// Lấy danh sách investorId theo quan hệ của Sale
        /// </summary>
        /// <param name="saleId"></param>
        /// <returns></returns>
        public IQueryable<int> ListInvestorOfSale(List<int> saleIds)
        {
            var investorSaleQuery = (from investorSale in _epicSchemaDbContext.InvestorSales.AsNoTracking()
                                     join investor in _epicSchemaDbContext.Investors.AsNoTracking() on investorSale.InvestorId equals investor.InvestorId
                                     where investorSale.Deleted == YesNo.NO && investor.Deleted == YesNo.NO
                                     && investor.Step > InvestorAppStep.DA_DANG_KY //&& investor.FaceImageUrl != null
                                     && saleIds.Contains(investorSale.SaleId ?? 0) && investor.Status != InvestorStatus.TEMP
                                     select investorSale.InvestorId).Distinct();
            return investorSaleQuery;
        }

        public PagingResult<SaleTempDto> FindAllSaleTemp(FilterSaleTempDto input)
        {
            var query = from saleTemp in _epicSchemaDbContext.SaleTemps
                        join department in _epicSchemaDbContext.Departments on saleTemp.DepartmentId equals department.DepartmentId into departments
                        from department in departments.DefaultIfEmpty()
                        join investor in _epicSchemaDbContext.Investors on saleTemp.InvestorId equals investor.InvestorId into investors
                        from investor in investors.DefaultIfEmpty()
                        join businessCustomer in _epicSchemaDbContext.BusinessCustomers on saleTemp.BusinessCustomerId equals businessCustomer.BusinessCustomerId into businessCustomers
                        from businessCustomer in businessCustomers.DefaultIfEmpty()
                        let identification = _epicSchemaDbContext.InvestorIdentifications.Where(e => e.InvestorId == investor.InvestorId && e.Deleted == YesNo.NO).OrderByDescending(o => o.IsDefault).FirstOrDefault()
                        where saleTemp.Deleted == YesNo.NO
                        && (input.Keyword == null || investor.Phone == input.Keyword)
                        && (saleTemp.TradingProviderId == input.TradingProviderId)
                        && (input.SaleType == null || saleTemp.SaleType == input.SaleType)
                        && (input.EmployeeCode == null || saleTemp.EmployeeCode.ToLower().Contains(input.EmployeeCode.ToLower()))
                        && (input.Phone == null || investor.Phone.Contains(input.Phone))
                        && (input.Email == null || investor.Email.ToLower().Contains(input.Email.ToLower()))
                        && (input.IdNo == null || identification.IdNo.ToLower().Contains(input.IdNo.ToLower()))
                        && ((input.Status == null && saleTemp.Status != SaleTempStatus.DA_DUYET) || saleTemp.Status == input.Status)
                        && (input.Source == null || saleTemp.Source == input.Source)
                        && (input.TaxCode == null || investor.TaxCode.ToLower().Contains(input.TaxCode.ToLower()) || businessCustomer.TaxCode.ToLower().Contains(input.TaxCode.ToLower()))
                        && (input.IsInvestor == null || (input.IsInvestor == 1 && saleTemp.InvestorId != null) || (input.IsInvestor == 2 && saleTemp.BusinessCustomerId != null))
                        select new SaleTempDto
                        {
                            Id = saleTemp.Id,
                            InvestorId = investor.InvestorId,
                            BusinessCustomerId = businessCustomer.BusinessCustomerId,
                            TradingProviderId = saleTemp.TradingProviderId,
                            DepartmentId = saleTemp.DepartmentId,
                            Source = saleTemp.Source,
                            Status = saleTemp.Status,
                            EmployeeCode = saleTemp.EmployeeCode,
                            SaleType = saleTemp.SaleType,
                            SaleParentId = saleTemp.SaleParentId,
                            Department = department == null ? null : new DepartmentDto
                            {
                                DepartmentId = department.DepartmentId,
                                TradingProviderId = department.TradingProviderId,
                                DepartmentName = department.DepartmentName,
                                DepartmentAddress = department.DepartmentAddress,
                                ParentId = department.ParentId,
                                DepartmentLevel = department.DepartmentLevel,
                                ManagerId = department.ManagerId,
                                ManagerId2 = department.ManagerId2,
                            },

                            Investor = investor == null ? null : new Entities.Dto.Investor.InvestorDto
                            {
                                InvestorId = investor.InvestorId,
                                AccountStatus = investor.AccountStatus,
                                Name = investor.Name,
                                CifCode = investor.CifCode,
                                CreatedDate = investor.CreatedDate.Value,
                                Email = investor.Email,
                                FaceImageUrl = investor.FaceImageUrl,
                                InvestorIdentification = identification == null ? null : new Entities.Dto.Investor.InvestorIdentificationDto
                                {
                                    CreatedBy = identification.CreatedBy,
                                    CreatedDate = identification.CreatedDate,
                                    DateOfBirth = identification.DateOfBirth,
                                    Deleted = identification.Deleted,
                                    EkycInfoIsConfirmed = identification.EkycInfoIsConfirmed,
                                    EkycIncorrectFields = identification.EkycIncorrectFields,
                                    FaceImageUrl = identification.FaceImageUrl,
                                    FaceVideoUrl = identification.FaceVideoUrl,
                                    Fullname = identification.Fullname,
                                    Id = identification.Id,
                                    IdBackImageUrl = identification.IdBackImageUrl,
                                    IdDate = identification.IdDate,
                                    IdExpiredDate = identification.IdExpiredDate,
                                    IdExtraImageUrl = identification.IdExtraImageUrl,
                                    IdFrontImageUrl = identification.IdFrontImageUrl,
                                    IdIssuer = identification.IdIssuer,
                                    IdNo = identification.IdNo,
                                    IdType = identification.IdType,
                                    InvestorId = identification.InvestorId,
                                    IsDefault = identification.IsDefault,
                                    IsVerifiedFace = identification.IsVerifiedFace,
                                    IsVerifiedIdentification = identification.IsVerifiedIdentification,
                                    Nationality = identification.Nationality,
                                    PlaceOfOrigin = identification.PlaceOfOrigin,
                                    PersonalIdentification = identification.PersonalIdentification,
                                    PlaceOfResidence = identification.PlaceOfResidence,
                                    Sex = identification.Sex,
                                    Status = identification.Status,
                                    StatusApproved = identification.StatusApproved,
                                }
                            },
                            BusinessCustomer = businessCustomer == null ? null : new Entities.Dto.BusinessCustomer.BusinessCustomerDto
                            {
                                Address = businessCustomer.Address,
                                AllowDuplicate = businessCustomer.AllowDuplicate,
                                AvatarImageUrl= businessCustomer.AvatarImageUrl,
                                Capital = businessCustomer.Capital,
                                Code = businessCustomer.Code,
                                Email = businessCustomer.Email,
                                Fanpage = businessCustomer.Fanpage,
                                IsCheck = businessCustomer.IsCheck,
                                LicenseDate = businessCustomer.LicenseDate,
                                LicenseIssuer = businessCustomer.LicenseIssuer,
                                Name = businessCustomer.Name,
                                Nation = businessCustomer.Nation,
                                Phone = businessCustomer.Phone,
                                ReferralCodeSelf = businessCustomer.ReferralCodeSelf,
                                RepAddress = businessCustomer.RepAddress,
                                RepBirthDate = businessCustomer.RepBirthDate,
                                RepIdDate = businessCustomer.RepIdDate,
                                RepIdIssuer = businessCustomer.RepIdIssuer,
                                RepIdNo = businessCustomer.RepIdNo,
                                RepName = businessCustomer.RepName,
                                RepPosition = businessCustomer.RepPosition,
                                RepSex = businessCustomer.RepSex,
                                BusinessRegistrationImg = businessCustomer.BusinessRegistrationImg,
                                Secret = businessCustomer.Secret,
                                ShortName = businessCustomer.ShortName,
                                TradingAddress = businessCustomer.TradingAddress,
                                Website = businessCustomer.Website,
                                TaxCode = businessCustomer.TaxCode,
                                BusinessCustomerId = businessCustomer.BusinessCustomerId,
                            },
                            Fullname = investor == null ? businessCustomer.Name : (identification == null ? null : identification.Fullname)
                        };
            var result = new PagingResult<SaleTempDto>();
            result.TotalItems = query.Count();
            query = query.OrderDynamic(input.Sort);
            if (input.PageSize == -1)
            {
                query = query.Skip(input.Skip).Take(input.PageSize);
            }
            result.Items = query;
            return result;
        }

        public PagingResult<ViewSaleDto> FindAllSale(EPIC.CoreEntities.Dto.Sale.FilterSaleSto input)
        {
            var query = from departmentSale in _epicSchemaDbContext.DepartmentSales
                        join sale in _epicSchemaDbContext.Sales on departmentSale.SaleId equals sale.SaleId
                        join saleTrading in _epicSchemaDbContext.SaleTradingProviders on departmentSale.SaleId equals saleTrading.SaleId
                        join investor in _epicSchemaDbContext.Investors.Where(e => e.Deleted == YesNo.NO) on sale.InvestorId equals investor.InvestorId into investors
                        from investor in investors.DefaultIfEmpty()
                        join businessCustomer in _epicSchemaDbContext.BusinessCustomers.Where(e => e.Deleted == YesNo.NO) on sale.BusinessCustomerId equals businessCustomer.BusinessCustomerId into businessCustomers
                        from businessCustomer in businessCustomers.DefaultIfEmpty()
                        let identification = _epicSchemaDbContext.InvestorIdentifications.Where(e => e.InvestorId == investor.InvestorId && e.Deleted == YesNo.NO).OrderByDescending(o => o.IsDefault).FirstOrDefault()
                        let department = _epicSchemaDbContext.Departments.FirstOrDefault(d => d.DepartmentId == departmentSale.DepartmentId && (input.TradingProviderId == null || d.TradingProviderId == input.TradingProviderId))
                        where sale.Deleted == YesNo.NO && departmentSale.Deleted == YesNo.NO
                        && sale.Deleted == YesNo.NO && saleTrading.Deleted == YesNo.NO
                        && departmentSale.TradingProviderId == saleTrading.TradingProviderId
                        && (input.DepartmentId == null || departmentSale.DepartmentId == input.DepartmentId)
                        && (input.TradingProviderId == null || departmentSale.TradingProviderId == input.TradingProviderId)
                        && (input.SaleType == null || saleTrading.SaleType == input.SaleType)
                        && (input.EmployeeCode == null || saleTrading.EmployeeCode.ToLower().Contains(input.EmployeeCode.ToLower()))
                        && (input.Phone == null || investor.Phone.Contains(input.Phone))
                        && (input.Email == null || investor.Email.ToLower().Contains(input.Email.ToLower()))
                        && (input.IdNo == null || identification.IdNo.ToLower().Contains(input.IdNo.ToLower()))
                        && (input.Status == null || saleTrading.Status == input.Status)
                        && (input.TaxCode == null || investor.TaxCode.ToLower().Contains(input.TaxCode.ToLower()) || businessCustomer.TaxCode.ToLower().Contains(input.TaxCode.ToLower()))
                        && (input.InvestorName == null || (identification.Fullname.ToLower().Contains(input.InvestorName.ToLower())) || (businessCustomer.Name.ToLower().Contains(input.InvestorName.ToLower())))
                        && (input.ReferralCode == null || (investor.ReferralCodeSelf.ToLower().Contains(input.ReferralCode.ToLower())) || (businessCustomer.ReferralCodeSelf.ToLower().Contains(input.ReferralCode.ToLower())))
                        && (input.IsInvestor == null || (input.IsInvestor == CustomerTypeForSearch.INVESTOR && investor != null && businessCustomer == null) || (input.IsInvestor == CustomerTypeForSearch.BUSINESS_CUSTOMER && investor == null && businessCustomer != null))
                        select new ViewSaleDto
                        {
                            SaleId = sale.SaleId,
                            InvestorId = investor.InvestorId,
                            BusinessCustomerId = businessCustomer.BusinessCustomerId,
                            DepartmentId = departmentSale.DepartmentId,
                            Status = saleTrading.Status,
                            ReferralCode = investor == null ? businessCustomer.ReferralCodeSelf : investor.ReferralCodeSelf,
                            ContractCode = saleTrading.ContractCode,
                            EmployeeCode = saleTrading.EmployeeCode,
                            AutoDirectional = sale.AutoDirectional,
                            SaleTradingCreatedDate = saleTrading.CreatedDate,
                            SaleType = saleTrading.SaleType,
                            Department = department == null ? null : new DepartmentDto
                            {
                                DepartmentId = department.DepartmentId,
                                TradingProviderId = department.TradingProviderId,
                                DepartmentName = department.DepartmentName,
                                DepartmentAddress = department.DepartmentAddress,
                                ParentId = department.ParentId,
                                DepartmentLevel = department.DepartmentLevel,
                                ManagerId = department.ManagerId,
                                ManagerId2 = department.ManagerId2,
                            },

                            Investor = investor == null ? null : new Entities.Dto.Investor.InvestorDto
                            {
                                InvestorId = investor.InvestorId,
                                AccountStatus = investor.AccountStatus,
                                Name = investor.Name,
                                CifCode = investor.CifCode,
                                CreatedDate = investor.CreatedDate.Value,
                                Email = investor.Email,
                                FaceImageUrl = investor.FaceImageUrl,
                                InvestorIdentification = identification == null ? null : new Entities.Dto.Investor.InvestorIdentificationDto
                                {
                                    CreatedBy = identification.CreatedBy,
                                    CreatedDate = identification.CreatedDate,
                                    DateOfBirth = identification.DateOfBirth,
                                    Deleted = identification.Deleted,
                                    EkycInfoIsConfirmed = identification.EkycInfoIsConfirmed,
                                    EkycIncorrectFields = identification.EkycIncorrectFields,
                                    FaceImageUrl = identification.FaceImageUrl,
                                    FaceVideoUrl = identification.FaceVideoUrl,
                                    Fullname = identification.Fullname,
                                    Id = identification.Id,
                                    IdBackImageUrl = identification.IdBackImageUrl,
                                    IdDate = identification.IdDate,
                                    IdExpiredDate = identification.IdExpiredDate,
                                    IdExtraImageUrl = identification.IdExtraImageUrl,
                                    IdFrontImageUrl = identification.IdFrontImageUrl,
                                    IdIssuer = identification.IdIssuer,
                                    IdNo = identification.IdNo,
                                    IdType = identification.IdType,
                                    InvestorId = identification.InvestorId,
                                    IsDefault = identification.IsDefault,
                                    IsVerifiedFace = identification.IsVerifiedFace,
                                    IsVerifiedIdentification = identification.IsVerifiedIdentification,
                                    Nationality = identification.Nationality,
                                    PlaceOfOrigin = identification.PlaceOfOrigin,
                                    PersonalIdentification = identification.PersonalIdentification,
                                    PlaceOfResidence = identification.PlaceOfResidence,
                                    Sex = identification.Sex,
                                    Status = identification.Status,
                                    StatusApproved = identification.StatusApproved,
                                }
                            },
                            BusinessCustomer = businessCustomer == null ? null : new Entities.Dto.BusinessCustomer.BusinessCustomerDto
                            {
                                Address = businessCustomer.Address,
                                AllowDuplicate = businessCustomer.AllowDuplicate,
                                AvatarImageUrl = businessCustomer.AvatarImageUrl,
                                Capital = businessCustomer.Capital,
                                Code = businessCustomer.Code,
                                Email = businessCustomer.Email,
                                Fanpage = businessCustomer.Fanpage,
                                IsCheck = businessCustomer.IsCheck,
                                LicenseDate = businessCustomer.LicenseDate,
                                LicenseIssuer = businessCustomer.LicenseIssuer,
                                Name = businessCustomer.Name,
                                Nation = businessCustomer.Nation,
                                Phone = businessCustomer.Phone,
                                ReferralCodeSelf = businessCustomer.ReferralCodeSelf,
                                RepAddress = businessCustomer.RepAddress,
                                RepBirthDate = businessCustomer.RepBirthDate,
                                RepIdDate = businessCustomer.RepIdDate,
                                RepIdIssuer = businessCustomer.RepIdIssuer,
                                RepIdNo = businessCustomer.RepIdNo,
                                RepName = businessCustomer.RepName,
                                RepPosition = businessCustomer.RepPosition,
                                RepSex = businessCustomer.RepSex,
                                BusinessRegistrationImg = businessCustomer.BusinessRegistrationImg,
                                Secret = businessCustomer.Secret,
                                ShortName = businessCustomer.ShortName,
                                TradingAddress = businessCustomer.TradingAddress,
                                Website = businessCustomer.Website,
                                TaxCode = businessCustomer.TaxCode,
                                BusinessCustomerId = businessCustomer.BusinessCustomerId,
                            },
                            Fullname = investor == null ? businessCustomer.Name : (identification == null ? null : identification.Fullname)
                        };
            var result = new PagingResult<ViewSaleDto>();
            result.TotalItems = query.Count();
            query = query.OrderByDescending(s => s.SaleId);
            query = query.OrderDynamic(input.Sort);
            if (input.PageSize != -1)
            {
                query = query.Skip(input.Skip).Take(input.PageSize);
            }
            result.Items = query;
            return result;
        }
        public PagingResult<ViewSaleRegisterDto> FindAllSaleRegister(FilterSaleRegisterDto input)
        {
            var result = new PagingResult<ViewSaleRegisterDto>();
            var query = from saleRegister in _epicSchemaDbContext.SaleRegisters
                        join investor in _epicSchemaDbContext.Investors on saleRegister.InvestorId equals investor.InvestorId
                        from identification in _epicSchemaDbContext.InvestorIdentifications.Where(i => i.InvestorId == investor.InvestorId && i.Deleted == YesNo.NO)
                        .OrderByDescending(c => c.IsDefault).ThenByDescending(c => c.Id).Take(1).DefaultIfEmpty()
                        where saleRegister.Deleted == YesNo.NO && investor.Deleted == YesNo.NO
                        && (input.Keyword == null || investor.Phone.Contains(input.Keyword))
                        && (input.Phone == null || investor.Phone.ToLower().Contains(input.Phone.ToLower()))
                        && (input.IdNo == null || identification.IdNo.ToLower().Contains(input.IdNo.ToLower()))
                        && (input.InvestorName == null || identification.Fullname.ToLower().Contains(input.InvestorName.ToLower()))
                        && (input.Status == null || saleRegister.Status == input.Status)
                        select new ViewSaleRegisterDto
                        {
                            Id = saleRegister.Id,
                            InvestorBankAccId = saleRegister.InvestorBankAccId,
                            InvestorId = saleRegister.InvestorId ?? 0,
                            SaleManagerId = saleRegister.SaleManagerId ?? 0,
                            Status = saleRegister.Status ?? 0,
                            Investor = new EPIC.Entities.Dto.Investor.InvestorDto
                            {
                                InvestorId = investor.InvestorId,
                                AccountStatus = investor.AccountStatus,
                                Address = investor.Address,
                                AvatarImageUrl = investor.AvatarImageUrl,
                                Email = investor.Email,
                                CreatedBy = investor.CreatedBy,
                                CreatedDate = (DateTime)investor.CreatedDate,
                                FaceImageUrl = investor.FaceImageUrl,
                                IsProf = investor.IsProf,
                                Isonline = investor.Isonline,
                                Name = investor.Name,
                                ReferralCodeSelf = investor.ReferralCodeSelf,
                                Phone = investor.Phone,
                                Status = investor.Status,
                                Nationality = investor.Nationality,
                                InvestorIdentification = identification == null ? null : new Entities.Dto.Investor.InvestorIdentificationDto
                                {
                                    InvestorId = identification.InvestorId,
                                    Nationality = identification.Nationality,
                                    DateOfBirth = identification.DateOfBirth,
                                    Deleted = identification.Deleted,
                                    EkycInfoIsConfirmed = identification.EkycInfoIsConfirmed,
                                    EkycIncorrectFields = identification.EkycIncorrectFields,
                                    FaceImageUrl = identification.FaceImageUrl,
                                    FaceVideoUrl = identification.FaceVideoUrl,
                                    IsVerifiedFace = identification.IsVerifiedFace,
                                    CreatedBy = identification.CreatedBy,
                                    CreatedDate = identification.CreatedDate,
                                    Id = identification.Id,
                                    IdBackImageUrl = identification.IdBackImageUrl,
                                    IdDate = identification.IdDate,
                                    IdExpiredDate = identification.IdExpiredDate,
                                    IdExtraImageUrl = identification.IdExtraImageUrl,
                                    IdFrontImageUrl = identification.IdFrontImageUrl,
                                    IdIssuer = identification.IdIssuer,
                                    IdNo = identification.IdNo,
                                    IdType = identification.IdType,
                                    InvestorGroupId = identification.InvestorGroupId.ToString(),
                                    IsDefault = identification.IsDefault,
                                    IsVerifiedIdentification = identification.IsVerifiedIdentification,
                                    PlaceOfOrigin = identification.PlaceOfOrigin,
                                    PlaceOfResidence = identification.PlaceOfResidence,
                                    Sex = identification.Sex,
                                    Status = identification.Status,
                                    Fullname = identification.Fullname
                                },
                            },
                        };
            result.TotalItems = query.Count();
            query = query.OrderDynamic(input.Sort);
            if(input.PageSize != -1)
            {
                query = query.Skip(input.Skip).Take(input.PageSize);
            }
            result.Items = query;
            return result;
        }
    }
}
