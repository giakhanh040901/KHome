using EPIC.CoreEntities.DataEntities;
using EPIC.CoreEntities.Dto.Sale;
using EPIC.CoreEntities.EntityFramework;
using EPIC.DataAccess.Base.Entities;
using EPIC.Entities.DataEntities;
using EPIC.Entities.Dto.Sale;
using EPIC.GarnerEntities.DataEntities;
using EPIC.GarnerEntities.EntiyFramework;
using EPIC.IdentityEntities.DataEntities;
using EPIC.IdentityEntities.EntityFramework;
using EPIC.InvestEntities.DataEntities;
using EPIC.InvestEntities.EntityFramework;
using EPIC.PaymentEntities.DataEntities;
using EPIC.PaymentEntities.EntityFramework;
using EPIC.RealEstateEntities.EntityFramework;
using EPIC.RealEstateEntities.DataEntities;
using EPIC.Utils.ConstantVariables.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Oracle.EntityFrameworkCore.Infrastructure.Internal;
using Oracle.EntityFrameworkCore.Migrations;
using EPIC.LoyaltyEntities.EntityFramework;
using EPIC.LoyaltyEntities.DataEntities;
using EPIC.BondEntities.DataEntities;
using Microsoft.AspNetCore.Http;
using EPIC.EventEntites.EntityFramework;
using EPIC.EventEntites.Entites;
using System.Linq;
using EPIC.Entities;
using System;
using EPIC.Utils;
using DocumentFormat.OpenXml.ExtendedProperties;

namespace EPIC.DataAccess.Base
{
    public partial class EpicSchemaDbContext : DbContext, IGarnerModel, IInvestModel, IRealEstateModel, ILoyaltyModel, IEventModel
    {
        #region Epic schema
        public DbSet<DefError> DefErrors { get; set; }
        public DbSet<SysVar> SysVars { get; set; }
        public DbSet<Province> Provinces { get; set; }
        public DbSet<District> Districts { get; set; }
        public DbSet<Ward> Wards { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<RoleDefaultRelationship> RoleDefaultRelationships { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }
        public DbSet<Users> Users { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<PartnerPermission> PartnerPermissions { get; set; }
        public DbSet<TradingProviderPermission> TradingProviderPermissions { get; set; }
        public DbSet<UserData> UserData { get; set; }

        public DbSet<UsersDevices> UsersDevices { get; set; }
        public DbSet<UsersPartner> UsersPartners { get; set; }
        public DbSet<UsersTradingProvider> UsersTradingProviders { get; set; }
        public DbSet<UsersChatRoom> UsersChatRooms { get; set; }
        public DbSet<UsersFcmToken> UsersFcmTokens { get; set; }

        public DbSet<WhiteListIp> WhiteListIps { get; set; }
        public DbSet<WhiteListIpDetail> WhiteListIpDetails { get; set; }

        public DbSet<Investor> Investors { get; set; }
        public DbSet<InvestorTemp> InvestorTemps { get; set; }
        public DbSet<InvestorSale> InvestorSales { get; set; }
        public DbSet<InvestorContactAddress> InvestorContactAddresses { get; set; }
        public DbSet<InvestorContactAddressTemp> InvestorContactAddressTemps { get; set; }
        public DbSet<InvestorIdentification> InvestorIdentifications { get; set; }
        public DbSet<InvestorIdTemp> InvestorIdTemps { get; set; }
        public DbSet<InvestorStock> InvestorStocks { get; set; }
        public DbSet<InvestorBankAccount> InvestorBankAccounts { get; set; }
        public DbSet<InvestorBankAccountTemp> InvestorBankAccountTemps { get; set; }
        public DbSet<InvestorTradingProvider> InvestorTradingProviders { get; set; }
        public DbSet<AuthOtp> AuthOtps { get; set; }
        public DbSet<CifCodes> CifCodes { get; set; }
        public DbSet<TradingProvider> TradingProviders { get; set; }
        public DbSet<Partner> Partners { get; set; }
        public DbSet<TradingProviderPartner> TradingProviderPartners { get; set; }
        #endregion

        #region connect msb
        public DbSet<TradingMsbPrefixAccount> TradingMSBPrefixAccounts { get; set; }
        public DbSet<MsbNotification> MsbNotifications { get; set; }
        public DbSet<MsbNotificationPayment> MsbNotificationPayments { get; set; }
        #endregion

        #region Bond
        public DbSet<BondOrder> BondOrders { get; set; }
        #endregion

        #region Invest
        public DbSet<InvOrder> InvOrders { get; set; }
        public DbSet<Calendar> InvestCalendars { get; set; }
        public DbSet<InvestEntities.DataEntities.OrderPayment> InvestOrderPayments { get; set; }
        public DbSet<InvestConfigContractCode> InvestConfigContractCodes { get; set; }
        public DbSet<InvestConfigContractCodeDetail> InvestConfigContractCodeDetails { get; set; }
        public DbSet<InvestContractTemplate> InvestContractTemplates { get; set; }
        public DbSet<InvestContractTemplateTemp> InvestContractTemplateTemps { get; set; }
        public DbSet<Withdrawal> InvestWithdrawals { get; set; }
        public DbSet<Policy> InvestPolicies { get; set; }
        public DbSet<PolicyDetail> InvestPolicyDetails { get; set; }
        public DbSet<Distribution> InvestDistributions { get; set; }
        public DbSet<InvestEntities.DataEntities.OrderPayment> OrderPayments { get; set; }
        public DbSet<InvestInterestPayment> InvestInterestPayments { get; set; }
        public DbSet<InvestInterestPaymentDate> InvestInterestPaymentDates { get; set; }
        public DbSet<InvRenewalsRequest> InvestRenewalsRequests { get; set; }
        public DbSet<MsbRequestPayment> MsbRequestPayment { get; set; }
        public DbSet<MsbRequestPaymentDetail> MsbRequestPaymentDetail { get; set; }
        public DbSet<InvestEntities.DataEntities.ReceiveContractTemplate> ReceiveContractTemplates { get; set; }
        public DbSet<OrderContractFile> InvestOrderContractFile { get; set; }
        public DbSet<InvestRating> InvestRatings { get; set; }
        public DbSet<Project> InvestProjects { get; set; }
        public DbSet<ProjectTradingProvider> InvestProjectTradingProviders { get; set; }
        public DbSet<InvestApprove> InvestApproves { get; set; }
        public DbSet<InvestHistoryUpdate> InvestHistoryUpdates { get; set; }
        public DbSet<InvestProjectInformationShareDetail> InvestProjectInformationShareDetails { get; set; }
        public DbSet<InvestProjectInformationShare> InvestProjectInformationShares { get; set; }
        public DbSet<PolicyTemp> InvestPolicyTemps { get; set; }
        public DbSet<PolicyDetailTemp> InvestPolicyDetailTemps { get; set; }
        public DbSet<BlockadeLiberation> InvestBlockadeLiberations { get; set; }
        #endregion

        #region Dbset not map table
        public DbSet<AppListTradingProviderDto> AppListTradingProviderDto { get; set; }
        public DbSet<SaleInBusinessCustomerSaleSubDto> SaleInBusinessCustomerSaleSubDto { get; set; }
        public DbSet<AppSaleByReferralCodeDto> AppSaleByReferralCodeDto { get; set; }
        #endregion

        #region Epic Core
        public virtual DbSet<BusinessCustomer> BusinessCustomers { get; set; }
        public virtual DbSet<BusinessCustomerTemp> BusinessCustomerTemps { get; set; }
        public virtual DbSet<BusinessCustomerBank> BusinessCustomerBanks { get; set; }
        public virtual DbSet<CoreBank> CoreBanks { get; set; }
        public virtual DbSet<Sale> Sales { get; set; }
        public virtual DbSet<SaleTradingProvider> SaleTradingProviders { get; set; }
        public virtual DbSet<DepartmentSale> DepartmentSales { get; set; }
        public virtual DbSet<CoreApprove> CoreApproves { get; set; }
        public virtual DbSet<Department> Departments { get; set; }
        public virtual DbSet<InvestorToDo> InvestorTodos { get; set; }
        public virtual DbSet<TradingProviderConfig> TradingProviderConfigs { get; set; }
        public virtual DbSet<SaleTemp> SaleTemps { get; set; }
        public virtual DbSet<CollabContract> SaleCollabContracts { get; set; }
        public virtual DbSet<BusinessCustomerTrading> BusinessCustomerTradings { get; set; }
        public virtual DbSet<BusinessCustomerPartner> BusinessCustomerPartners { get; set; }
        public virtual DbSet<TradingFirstMessage> TradingFirstMessages { get; set; }
        public virtual DbSet<PartnerBankAccount> PartnerBankAccounts { get; set; }
        public virtual DbSet<InvestorSearchHistory> InvestorSearchHistories { get; set; }
        public virtual DbSet<CallCenterConfig> CallCenterConfigs { get; set; }
        public virtual DbSet<CollabContractTemp> CollabContractTemps { get; set; }
        public virtual DbSet<SaleRegister> SaleRegisters { get; set; }

        public DbSet<XptToken> XptTokens { get; set; }
        public DbSet<XptTokenDataType> XptTokenDataTypes { get; set; }
        #endregion

        #region Company shares
        #endregion

        #region Real estate
        public virtual DbSet<RstProject> RstProjects { get; set; }
        public virtual DbSet<RstApprove> RstApproves { get; set; }
        public virtual DbSet<RstProjectType> RstProjectTypes { get; set; }
        public virtual DbSet<RstProjectPolicy> RstProjectPolicys { get; set; }
        public virtual DbSet<RstDistributionPolicyTemp> RstDistributionPolicyTemps { get; set; }
        public virtual DbSet<RstDistributionPolicy> RstDistributionPolicys { get; set; }
        public virtual DbSet<RstProjectGuaranteeBank> RstProjectGuaranteeBanks { get; set; }
        public virtual DbSet<RstOwner> RstOwners { get; set; }
        public virtual DbSet<RstSellingPolicyTemp> RstSellingPolicyTemps { get; set; }
        public virtual DbSet<RstConfigContractCode> RstConfigContractCodes { get; set; }
        public virtual DbSet<RstConfigContractCodeDetail> RstConfigContractCodeDetails { get; set; }
        public virtual DbSet<RstProjectUtility> RstProjectUtilities { get; set; }
        public virtual DbSet<RstProjectUtilityMedia> RstProjectUtilityMedias { get; set; }
        public virtual DbSet<RstProjectUtilityExtend> RstProjectUtilityExtends { get; set; }
        public virtual DbSet<RstProductItemUtility> RstProductItemProjectUtilities { get; set; }
        public virtual DbSet<RstProductItemProjectPolicy> RstProductItemProjectPolicys { get; set; }
        public virtual DbSet<RstProductItem> RstProductItems { get; set; }
        public virtual DbSet<RstDistribution> RstDistributions { get; set; }
        public virtual DbSet<RstDistributionBank> RstDistributionBanks { get; set; }
        public virtual DbSet<RstDistributionProductItem> RstDistributionProductItems { get; set; }
        public virtual DbSet<RstOpenSell> RstOpenSells { get; set; }
        public virtual DbSet<RstOpenSellDetail> RstOpenSellDetails { get; set; }
        public virtual DbSet<RstOpenSellContractTemplate> RstOpenSellContractTemplates { get; set; }
        public virtual DbSet<RstOpenSellFile> RstOpenSellFiles { get; set; }
        public virtual DbSet<RstProjectMedia> RstProjectMedias { get; set; }
        public virtual DbSet<RstProductItemMedia> RstProductItemMedias { get; set; }
        public virtual DbSet<RstContractTemplateTemp> RstContractTemplateTemps { get; set; }
        public virtual DbSet<RstOrder> RstOrders { get; set; }
        public virtual DbSet<RstOrderContractFile> RstOrderContractFiles { get; set; }
        public virtual DbSet<RstOrderPayment> RstOrderPayments { get; set; }
        public virtual DbSet<RstOrderSellingPolicy> RstOrderSellingPolicies { get; set; }
        public virtual DbSet<RstSellingPolicy> RstSellingPolicies { get; set; }
        public virtual DbSet<RstOpenSellBank> RstOpenSellBanks { get; set; }
        public virtual DbSet<RstProjectStructure> RstProjectStructures { get; set; }
        public virtual DbSet<RstRating> RstRatings { get; set; }
        public virtual DbSet<RstProjectFavourite> RstProjectFavourites { get; set; }
        public virtual DbSet<RstOpenSellInterest> RstOpenSellInterests { get; set; }
        public virtual DbSet<RstProjectInformationShareDetail> RstProjectInformationShareDetails { get; set; }
        public virtual DbSet<RstProjectInformationShare> RstProjectInformationShares { get; set; }

        public virtual DbSet<RstProductItemMaterialFile> RstProductItemMaterialFiles { get; set; }
        public virtual DbSet<RstProductItemDesignDiagramFile> RstProductItemDesignDiagramFiles { get; set; }

        #endregion

        #region Garner
        public DbSet<GarnerProduct> GarnerProducts { get; set; }
        public DbSet<GarnerContractTemplate> GarnerContractTemplates { get; set; }
        public DbSet<GarnerContractTemplateTemp> GarnerContractTemplateTemps { get; set; }
        public DbSet<GarnerProductType> GarnerProductTypes { get; set; }
        public DbSet<GarnerProductFile> GarnerProductFiles { get; set; }
        public DbSet<GarnerProductOverviewFile> GarnerProductOverviewFiles { get; set; }
        public DbSet<GarnerProductOverviewOrg> GarnerProductOverviewOrgs { get; set; }
        public DbSet<GarnerProductPrice> GarnerProductPrices { get; set; }
        public DbSet<GarnerProductTradingProvider> GarnerProductTradingProviders { get; set; }
        public DbSet<GarnerDistributionTradingBankAccount> GarnerDistributionTradingBankAccounts { get; set; }
        public DbSet<GarnerApprove> GarnerApproves { get; set; }
        public DbSet<GarnerDistribution> GarnerDistributions { get; set; }
        public DbSet<GarnerHistoryUpdate> GarnerHistoryUpdates { get; set; }
        public DbSet<GarnerPolicy> GarnerPolicies { get; set; }
        public DbSet<GarnerPolicyDetail> GarnerPolicyDetails { get; set; }
        public DbSet<GarnerOrder> GarnerOrders { get; set; }
        public DbSet<GarnerOrderContractFile> GarnerOrderContractFiles { get; set; }
        public DbSet<GarnerCalendar> GarnerCalendars { get; set; }
        public DbSet<GarnerPartnerCalendar> GarnerPartnerCalendars { get; set; }
        public DbSet<GarnerOrderPayment> GarnerOrderPayments { get; set; }
        public DbSet<GarnerWithdrawal> GarnerWithdrawals { get; set; }
        public DbSet<GarnerWithdrawalDetail> GarnerWithdrawalDetails { get; set; }
        public DbSet<GarnerConfigContractCode> GarnerConfigContractCodes { get; set; }
        public DbSet<GarnerConfigContractCodeDetail> GarnerConfigContractCodeDetails { get; set; }
        public DbSet<GarnerBlockadeLiberation> GarnerBlockadeLiberations { get; set; }
        public DbSet<GarnerReceiveContractTemp> GarnerReceiveContractTemps { get; set; }
        public DbSet<GarnerInterestPayment> GarnerInterestPayments { get; set; }
        public DbSet<GarnerInterestPaymentDetail> GarnerInterestPaymentDetails { get; set; }
        public DbSet<GarnerRating> GarnerRatings { get; set; }
        #endregion

        #region Loyalty
        public DbSet<LoyVoucher> LoyVouchers { get; set; }
        public DbSet<LoyVoucherInvestor> LoyVoucherInvestors { get; set; }
        public DbSet<LoyPointInvestor> LoyPointInvestors { get; set; }
        public DbSet<LoyHisAccumulatePoint> LoyHisAccumulatePoints { get; set; }
        public DbSet<LoyAccumulatePointStatusLog> LoyAccumulatePointStatusLogs { get; set; }
        public DbSet<LoyRank> LoyRanks { get; set; }
        public DbSet<LoyConversionPoint> LoyConversionPoints { get; set; }
        public DbSet<LoyConversionPointDetail> LoyConversionPointDetails { get; set; }
        public DbSet<LoyConversionPointStatusLog> LoyConversionPointStatusLogs { get; set; }
        public DbSet<LoyLuckyProgram> LoyLuckyPrograms { get; set; }
        public DbSet<LoyLuckyProgramInvestor> LoyLuckyProgramInvestors { get; set; }
        public DbSet<LoyLuckyProgramInvestorDetail> LoyLuckyProgramInvestorDetails { get; set; }
        public DbSet<LoyLuckyScenario> LoyLuckyScenarios { get; set; }
        public DbSet<LoyLuckyScenarioDetail> LoyLuckyScenarioDetails { get; set; }
        public DbSet<LoyLuckyRotationInterface> LoyLuckyRotationInterfaces { get; set; }
        public DbSet<LoyHistoryUpdate> LoyHistoryUpdates { get; set; }
        #endregion

        #region Event
        public DbSet<EvtEvent> EvtEvents { get; set; }
        public DbSet<EvtEventDetail> EvtEventDetails { get; set; }
        public DbSet<EvtTicket> EvtTickets { get; set; }

        public DbSet<EvtConfigContractCode> EvtConfigContractCodes { get; set; }
        public DbSet<EvtConfigContractCodeDetail> EvtConfigContractCodeDetails { get; set; }
        public DbSet<EvtEventMedia> EvtEventMedias { get; set; }
        public DbSet<EvtEventMediaDetail> EvtEventMediaDetails { get; set; }
        public DbSet<EvtOrder> EvtOrders { get; set; }
        public DbSet<EvtOrderDetail> EvtOrderDetails { get; set; }
        public DbSet<EvtOrderTicketDetail> EvtOrderTicketDetails { get; set; }
        public DbSet<EvtTicketMedia> EvtTicketMedias { get; set; }
        public DbSet<EvtOrderPayment> EvtOrderPayments { get; set; }
        public DbSet<EvtEventType> EvtEventTypes { get; set; }
        public DbSet<EvtInterestedPerson> EvtInterestedPeople { get; set; }
        public DbSet<EvtHistoryUpdate> EvtHistoryUpdates { get; set; }
        public DbSet<EvtSearchHistory> EvtSearchHistorys { get; set; }
        public DbSet<EvtEventBankAccount> EvtEventBankAccounts { get; set; }
        public DbSet<EvtEventDescriptionMedia> EvtEventDescriptionMedias { get; set; }
        public DbSet<EvtTicketTemplate> EvtTicketTemplates { get; set; }
        public DbSet<EvtAdminEvent> EvtAdminEvents { get; set; }
        public DbSet<EvtDeliveryTicketTemplate> EvtDeliveryTicketTemplates { get; set; }

        #endregion

        protected readonly string Username = null;

        public EpicSchemaDbContext(DbContextOptions<EpicSchemaDbContext> options, IHttpContextAccessor httpContext)
            : base(options)
        {
            Username = CommonUtils.GetCurrentUsername(httpContext);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema(DbSchemas.EPIC);

            IdentityModelCreating.OnModelCreating(modelBuilder);
            InvestModelCreating.OnModelCreating(modelBuilder);
            GarnerModelCreating.OnModelCreating(modelBuilder);
            CoreModelCreating.OnModelCreating(modelBuilder);
            PaymentModelCreating.OnModelCreating(modelBuilder);
            RealEstateModelCreating.OnModelCreating(modelBuilder);
            LoyaltyModelCreating.OnModelCreating(modelBuilder);
            EventModelCreating.OnModelCreating(modelBuilder);
        }

        private void CheckAudit()
        {
            ChangeTracker.DetectChanges();
            var added = ChangeTracker.Entries()
                .Where(t => t.State == EntityState.Added)
                .Select(t => t.Entity)
                .AsParallel();

            added.ForAll(entity =>
            {
                if (entity is ICreatedBy createdEntity && createdEntity.CreatedBy == null)
                {
                    createdEntity.CreatedDate = DateTime.Now;
                    createdEntity.CreatedBy = Username;
                }
            });

            var modified = ChangeTracker.Entries()
                        .Where(t => t.State == EntityState.Modified)
                        .Select(t => t.Entity)
                        .AsParallel();
            modified.ForAll(entity =>
            {
                if (entity is IModifiedBy modifiedEntity && modifiedEntity.ModifiedBy == null)
                {
                    modifiedEntity.ModifiedDate = DateTime.Now;
                    modifiedEntity.ModifiedBy = Username;
                }
                if (entity is ISoftDelted softDeltedEntity && entity is IModifiedBy softDeleteModifiedEntity  && softDeltedEntity.Deleted == YesNo.YES)
                {
                    softDeleteModifiedEntity.ModifiedBy = Username;
                    softDeleteModifiedEntity.ModifiedDate = DateTime.Now;
                }
            });
        }
        public override int SaveChanges()
        {
            CheckAudit();
            return base.SaveChanges();
        }
    }

    /// <summary>
    /// Class transient cho dbcontext, phục vụ cho việc chạy multiple thread
    /// </summary>
    public class EpicSchemaDbContextTransient : EpicSchemaDbContext
    {
        public EpicSchemaDbContextTransient(DbContextOptions<EpicSchemaDbContext> options, IHttpContextAccessor httpContext) : base(options, httpContext)
        {
        }
    }

    public class MyMigrationsSqlGenerator : OracleMigrationsSqlGenerator
    {
        public MyMigrationsSqlGenerator(
            MigrationsSqlGeneratorDependencies dependencies,
#pragma warning disable EF1001 // Internal EF Core API usage.
            IOracleOptions options)
#pragma warning restore EF1001 // Internal EF Core API usage.
            : base(dependencies, options)
        {
        }

        protected override void Generate(
            MigrationOperation operation,
            IModel model,
            MigrationCommandListBuilder builder)
        {
            base.Generate(operation, model, builder);
        }

        private void Generate(MigrationCommandListBuilder builder)
        {
            var sqlHelper = Dependencies.SqlGenerationHelper;
            var stringMapping = Dependencies.TypeMappingSource.FindMapping(typeof(string));
        }
    }


}
