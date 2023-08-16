using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace EPIC.UpdateData.Models
{
    public partial class ModelContext : DbContext
    {
        public ModelContext()
        {
        }

        public ModelContext(DbContextOptions<ModelContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Defcode> Defcodes { get; set; }
        public virtual DbSet<Deferror> Deferrors { get; set; }
        public virtual DbSet<District> Districts { get; set; }
        public virtual DbSet<EpAuthOtp> EpAuthOtps { get; set; }
        public virtual DbSet<EpBondBlockadeLiberation> EpBondBlockadeLiberations { get; set; }
        public virtual DbSet<EpBondHistoryUpdate> EpBondHistoryUpdates { get; set; }
        public virtual DbSet<EpBondInfoOverviewFile> EpBondInfoOverviewFiles { get; set; }
        public virtual DbSet<EpBondInfoOverviewOrg> EpBondInfoOverviewOrgs { get; set; }
        public virtual DbSet<EpBondInterestPayment> EpBondInterestPayments { get; set; }
        public virtual DbSet<EpBondInterestPaymentDate> EpBondInterestPaymentDates { get; set; }
        public virtual DbSet<EpBondOrder> EpBondOrders { get; set; }
        public virtual DbSet<EpBondOrderContractFile> EpBondOrderContractFiles { get; set; }
        public virtual DbSet<EpBondOrderPayment> EpBondOrderPayments { get; set; }
        public virtual DbSet<EpBondPartnerCalendar> EpBondPartnerCalendars { get; set; }
        public virtual DbSet<EpBondReceiveContrTemplate> EpBondReceiveContrTemplates { get; set; }
        public virtual DbSet<EpBondRenewalsRequest> EpBondRenewalsRequests { get; set; }
        public virtual DbSet<EpBondSecondaryContract> EpBondSecondaryContracts { get; set; }
        public virtual DbSet<EpCalendar> EpCalendars { get; set; }
        public virtual DbSet<EpCifCode> EpCifCodes { get; set; }
        public virtual DbSet<EpConfiguration> EpConfigurations { get; set; }
        public virtual DbSet<EpContractTemplate> EpContractTemplates { get; set; }
        public virtual DbSet<EpCoreApprove> EpCoreApproves { get; set; }
        public virtual DbSet<EpCoreBank> EpCoreBanks { get; set; }
        public virtual DbSet<EpCoreBusinessCusBankTemp> EpCoreBusinessCusBankTemps { get; set; }
        public virtual DbSet<EpCoreBusinessCusPartner> EpCoreBusinessCusPartners { get; set; }
        public virtual DbSet<EpCoreBusinessCusTrading> EpCoreBusinessCusTradings { get; set; }
        public virtual DbSet<EpCoreBusinessCustomer> EpCoreBusinessCustomers { get; set; }
        public virtual DbSet<EpCoreBusinessCustomerBank> EpCoreBusinessCustomerBanks { get; set; }
        public virtual DbSet<EpCoreBusinessCustomerTemp> EpCoreBusinessCustomerTemps { get; set; }
        public virtual DbSet<EpCoreBusinessLicenseFile> EpCoreBusinessLicenseFiles { get; set; }
        public virtual DbSet<EpCoreCollabContractTemp> EpCoreCollabContractTemps { get; set; }
        public virtual DbSet<EpCoreDepartment> EpCoreDepartments { get; set; }
        public virtual DbSet<EpCoreDepartmentSale> EpCoreDepartmentSales { get; set; }
        public virtual DbSet<EpCoreHistoryUpdate> EpCoreHistoryUpdates { get; set; }
        public virtual DbSet<EpCorePartner> EpCorePartners { get; set; }
        public virtual DbSet<EpCoreProductNews> EpCoreProductNews { get; set; }
        public virtual DbSet<EpCoreSale> EpCoreSales { get; set; }
        public virtual DbSet<EpCoreSaleCollabContract> EpCoreSaleCollabContracts { get; set; }
        public virtual DbSet<EpCoreSaleRegister> EpCoreSaleRegisters { get; set; }
        public virtual DbSet<EpCoreSaleTemp> EpCoreSaleTemps { get; set; }
        public virtual DbSet<EpCoreSaleTradingProvider> EpCoreSaleTradingProviders { get; set; }
        public virtual DbSet<EpCoreUserPartner> EpCoreUserPartners { get; set; }
        public virtual DbSet<EpDepositProvider> EpDepositProviders { get; set; }
        public virtual DbSet<EpDistributionConPayment> EpDistributionConPayments { get; set; }
        public virtual DbSet<EpDistributionContract> EpDistributionContracts { get; set; }
        public virtual DbSet<EpDistributionContractFile> EpDistributionContractFiles { get; set; }
        public virtual DbSet<EpEmailSmsTemplate> EpEmailSmsTemplates { get; set; }
        public virtual DbSet<EpGuaranteeAsset> EpGuaranteeAssets { get; set; }
        public virtual DbSet<EpGuaranteeFile> EpGuaranteeFiles { get; set; }
        public virtual DbSet<EpInvApprove> EpInvApproves { get; set; }
        public virtual DbSet<EpInvBlockadeLiberation> EpInvBlockadeLiberations { get; set; }
        public virtual DbSet<EpInvCalendar> EpInvCalendars { get; set; }
        public virtual DbSet<EpInvConfigContractCode> EpInvConfigContractCodes { get; set; }
        public virtual DbSet<EpInvConfigContractCodeDetail> EpInvConfigContractCodeDetails { get; set; }
        public virtual DbSet<EpInvContractTemplate> EpInvContractTemplates { get; set; }
        public virtual DbSet<EpInvContractTemplate1> EpInvContractTemplate1s { get; set; }
        public virtual DbSet<EpInvContractTemplateTemp> EpInvContractTemplateTemps { get; set; }
        public virtual DbSet<EpInvDisTradingBankAcc> EpInvDisTradingBankAccs { get; set; }
        public virtual DbSet<EpInvDistriPolicyFile> EpInvDistriPolicyFiles { get; set; }
        public virtual DbSet<EpInvDistribution> EpInvDistributions { get; set; }
        public virtual DbSet<EpInvDistributionFile> EpInvDistributionFiles { get; set; }
        public virtual DbSet<EpInvDistributionNews> EpInvDistributionNews { get; set; }
        public virtual DbSet<EpInvDistributionVideo> EpInvDistributionVideos { get; set; }
        public virtual DbSet<EpInvGeneralContractor> EpInvGeneralContractors { get; set; }
        public virtual DbSet<EpInvHistoryUpdate> EpInvHistoryUpdates { get; set; }
        public virtual DbSet<EpInvInterestPayment> EpInvInterestPayments { get; set; }
        public virtual DbSet<EpInvInterestPaymentDate> EpInvInterestPaymentDates { get; set; }
        public virtual DbSet<EpInvOrder> EpInvOrders { get; set; }
        public virtual DbSet<EpInvOrderContractFile> EpInvOrderContractFiles { get; set; }
        public virtual DbSet<EpInvOrderContractFile1> EpInvOrderContractFile1s { get; set; }
        public virtual DbSet<EpInvOrderPayment> EpInvOrderPayments { get; set; }
        public virtual DbSet<EpInvOwner> EpInvOwners { get; set; }
        public virtual DbSet<EpInvPolicy> EpInvPolicies { get; set; }
        public virtual DbSet<EpInvPolicyDetail> EpInvPolicyDetails { get; set; }
        public virtual DbSet<EpInvPolicyDetailTemp> EpInvPolicyDetailTemps { get; set; }
        public virtual DbSet<EpInvPolicyTemp> EpInvPolicyTemps { get; set; }
        public virtual DbSet<EpInvProject> EpInvProjects { get; set; }
        public virtual DbSet<EpInvProjectImage> EpInvProjectImages { get; set; }
        public virtual DbSet<EpInvProjectJuridicalFile> EpInvProjectJuridicalFiles { get; set; }
        public virtual DbSet<EpInvProjectOverviewFile> EpInvProjectOverviewFiles { get; set; }
        public virtual DbSet<EpInvProjectOverviewOrg> EpInvProjectOverviewOrgs { get; set; }
        public virtual DbSet<EpInvProjectTradingProvider> EpInvProjectTradingProviders { get; set; }
        public virtual DbSet<EpInvProjectType> EpInvProjectTypes { get; set; }
        public virtual DbSet<EpInvReceiveContractTemp> EpInvReceiveContractTemps { get; set; }
        public virtual DbSet<EpInvRenewalsRequest> EpInvRenewalsRequests { get; set; }
        public virtual DbSet<EpInvWithdrawal> EpInvWithdrawals { get; set; }
        public virtual DbSet<EpInvestor> EpInvestors { get; set; }
        public virtual DbSet<EpInvestorAsset> EpInvestorAssets { get; set; }
        public virtual DbSet<EpInvestorBankAccount> EpInvestorBankAccounts { get; set; }
        public virtual DbSet<EpInvestorBankAccountTemp> EpInvestorBankAccountTemps { get; set; }
        public virtual DbSet<EpInvestorContactAddTemp> EpInvestorContactAddTemps { get; set; }
        public virtual DbSet<EpInvestorContactAddress> EpInvestorContactAddresses { get; set; }
        public virtual DbSet<EpInvestorIdTemp> EpInvestorIdTemps { get; set; }
        public virtual DbSet<EpInvestorIdentification> EpInvestorIdentifications { get; set; }
        public virtual DbSet<EpInvestorProfFile> EpInvestorProfFiles { get; set; }
        public virtual DbSet<EpInvestorSale> EpInvestorSales { get; set; }
        public virtual DbSet<EpInvestorStock> EpInvestorStocks { get; set; }
        public virtual DbSet<EpInvestorStockTemp> EpInvestorStockTemps { get; set; }
        public virtual DbSet<EpInvestorTemp> EpInvestorTemps { get; set; }
        public virtual DbSet<EpInvestorToDo> EpInvestorToDos { get; set; }
        public virtual DbSet<EpInvestorTradingProvider> EpInvestorTradingProviders { get; set; }
        public virtual DbSet<EpIssuer> EpIssuers { get; set; }
        public virtual DbSet<EpJuridicalFile> EpJuridicalFiles { get; set; }
        public virtual DbSet<EpMsbNotification> EpMsbNotifications { get; set; }
        public virtual DbSet<EpMsbNotificationPayment> EpMsbNotificationPayments { get; set; }
        public virtual DbSet<EpMsbRequestPayment> EpMsbRequestPayments { get; set; }
        public virtual DbSet<EpMsbRequestPaymentDetail> EpMsbRequestPaymentDetails { get; set; }
        public virtual DbSet<EpPolicyFile> EpPolicyFiles { get; set; }
        public virtual DbSet<EpProductBondInfo> EpProductBondInfos { get; set; }
        public virtual DbSet<EpProductBondPolicy> EpProductBondPolicies { get; set; }
        public virtual DbSet<EpProductBondPolicyDeTemp> EpProductBondPolicyDeTemps { get; set; }
        public virtual DbSet<EpProductBondPolicyDetail> EpProductBondPolicyDetails { get; set; }
        public virtual DbSet<EpProductBondPolicyTemp> EpProductBondPolicyTemps { get; set; }
        public virtual DbSet<EpProductBondPrimary> EpProductBondPrimaries { get; set; }
        public virtual DbSet<EpProductBondSecondPrice> EpProductBondSecondPrices { get; set; }
        public virtual DbSet<EpProductBondSecondary> EpProductBondSecondaries { get; set; }
        public virtual DbSet<EpProductBondSecondaryNews> EpProductBondSecondaryNews { get; set; }
        public virtual DbSet<EpPvcbPaymentCallback> EpPvcbPaymentCallbacks { get; set; }
        public virtual DbSet<EpReceiveContractTemplate> EpReceiveContractTemplates { get; set; }
        public virtual DbSet<EpReportTradingProvider> EpReportTradingProviders { get; set; }
        public virtual DbSet<EpTradingMsbPrefixAccount> EpTradingMsbPrefixAccounts { get; set; }
        public virtual DbSet<EpTradingProvider> EpTradingProviders { get; set; }
        public virtual DbSet<EpTradingProviderPartner> EpTradingProviderPartners { get; set; }
        public virtual DbSet<EpWhiteListIp> EpWhiteListIps { get; set; }
        public virtual DbSet<EpWhiteListIpDetail> EpWhiteListIpDetails { get; set; }
        public virtual DbSet<Group> Groups { get; set; }
        public virtual DbSet<LogError> LogErrors { get; set; }
        public virtual DbSet<LogIsDefault> LogIsDefaults { get; set; }
        public virtual DbSet<PPartnerPermission> PPartnerPermissions { get; set; }
        public virtual DbSet<PRole> PRoles { get; set; }
        public virtual DbSet<PRolePermission> PRolePermissions { get; set; }
        public virtual DbSet<PTradingProviderPermission> PTradingProviderPermissions { get; set; }
        public virtual DbSet<PUserRole> PUserRoles { get; set; }
        public virtual DbSet<Province> Provinces { get; set; }
        public virtual DbSet<Sysvar> Sysvars { get; set; }
        public virtual DbSet<TempTienVaoTheoNgay> TempTienVaoTheoNgays { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserGroup> UserGroups { get; set; }
        public virtual DbSet<UsersChatRoom> UsersChatRooms { get; set; }
        public virtual DbSet<UsersDevice> UsersDevices { get; set; }
        public virtual DbSet<UsersFcmToken> UsersFcmTokens { get; set; }
        public virtual DbSet<UsersPartner> UsersPartners { get; set; }
        public virtual DbSet<UsersTradingProvider> UsersTradingProviders { get; set; }
        public virtual DbSet<Ward> Wards { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseOracle("Name=ConnectionStrings:EPIC");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("EPIC")
                .HasAnnotation("Relational:Collation", "USING_NLS_COMP");

            modelBuilder.Entity<Defcode>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("DEFCODE");

                entity.Property(e => e.Cddesc)
                    .HasMaxLength(256)
                    .IsUnicode(false)
                    .HasColumnName("CDDESC");

                entity.Property(e => e.Cdname)
                    .HasMaxLength(32)
                    .IsUnicode(false)
                    .HasColumnName("CDNAME");

                entity.Property(e => e.Cdposition)
                    .HasColumnType("NUMBER(38)")
                    .HasColumnName("CDPOSITION");

                entity.Property(e => e.Cdtype)
                    .HasMaxLength(32)
                    .IsUnicode(false)
                    .HasColumnName("CDTYPE");

                entity.Property(e => e.Cdvalue)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("CDVALUE");

                entity.Property(e => e.Cdvaluename)
                    .HasMaxLength(32)
                    .IsUnicode(false)
                    .HasColumnName("CDVALUENAME");
            });

            modelBuilder.Entity<Deferror>(entity =>
            {
                entity.HasKey(e => e.ErrCode);

                entity.ToTable("DEFERROR");

                entity.Property(e => e.ErrCode)
                    .HasColumnType("NUMBER")
                    .HasColumnName("ERR_CODE");

                entity.Property(e => e.ErrMessage)
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("ERR_MESSAGE");

                entity.Property(e => e.ErrName)
                    .IsRequired()
                    .HasMaxLength(64)
                    .IsUnicode(false)
                    .HasColumnName("ERR_NAME");
            });

            modelBuilder.Entity<District>(entity =>
            {
                entity.HasKey(e => e.Code)
                    .HasName("DISTRICTS_PKEY");

                entity.ToTable("DISTRICTS");

                entity.Property(e => e.Code)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("CODE");

                entity.Property(e => e.AdministrativeUnitId)
                    .HasColumnType("NUMBER(38)")
                    .HasColumnName("ADMINISTRATIVE_UNIT_ID");

                entity.Property(e => e.CodeName)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("CODE_NAME");

                entity.Property(e => e.FullName)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("FULL_NAME");

                entity.Property(e => e.FullNameEn)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("FULL_NAME_EN");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("NAME");

                entity.Property(e => e.NameEn)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("NAME_EN");

                entity.Property(e => e.ProvinceCode)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("PROVINCE_CODE");
            });

            modelBuilder.Entity<EpAuthOtp>(entity =>
            {
                entity.ToTable("EP_AUTH_OTP");

                entity.Property(e => e.Id)
                    .HasColumnType("NUMBER")
                    .HasColumnName("ID");

                entity.Property(e => e.CreatedTime)
                    .HasColumnType("DATE")
                    .HasColumnName("CREATED_TIME");

                entity.Property(e => e.ExpiredTime)
                    .HasColumnType("DATE")
                    .HasColumnName("EXPIRED_TIME");

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("IS_ACTIVE")
                    .HasDefaultValueSql("('Y')\n   ")
                    .IsFixedLength(true);

                entity.Property(e => e.OtpCode)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("OTP_CODE");

                entity.Property(e => e.Phone)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("PHONE");

                entity.Property(e => e.UserId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("USER_ID")
                    .HasDefaultValueSql("0 ");
            });

            modelBuilder.Entity<EpBondBlockadeLiberation>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("EP_BOND_BLOCKADE_LIBERATION");

                entity.Property(e => e.BlockadeDate)
                    .HasColumnType("DATE")
                    .HasColumnName("BLOCKADE_DATE");

                entity.Property(e => e.BlockadeDescription)
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("BLOCKADE_DESCRIPTION");

                entity.Property(e => e.BlockadeTime)
                    .HasColumnType("DATE")
                    .HasColumnName("BLOCKADE_TIME");

                entity.Property(e => e.Blockader)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("BLOCKADER");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("CREATED_BY");

                entity.Property(e => e.Id)
                    .HasColumnType("NUMBER")
                    .HasColumnName("ID");

                entity.Property(e => e.LiberationDate)
                    .HasColumnType("DATE")
                    .HasColumnName("LIBERATION_DATE");

                entity.Property(e => e.LiberationDescription)
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("LIBERATION_DESCRIPTION");

                entity.Property(e => e.LiberationTime)
                    .HasColumnType("DATE")
                    .HasColumnName("LIBERATION_TIME");

                entity.Property(e => e.Liberator)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("LIBERATOR");

                entity.Property(e => e.OrderId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("ORDER_ID");

                entity.Property(e => e.Status)
                    .HasColumnType("NUMBER")
                    .HasColumnName("STATUS");

                entity.Property(e => e.TradingProviderId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("TRADING_PROVIDER_ID");

                entity.Property(e => e.Type)
                    .HasColumnType("NUMBER")
                    .HasColumnName("TYPE");
            });

            modelBuilder.Entity<EpBondHistoryUpdate>(entity =>
            {
                entity.ToTable("EP_BOND_HISTORY_UPDATE");

                entity.Property(e => e.Id)
                    .HasColumnType("NUMBER")
                    .HasColumnName("ID");

                entity.Property(e => e.Action)
                    .HasColumnType("NUMBER")
                    .HasColumnName("ACTION");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("CREATED_BY");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("DATE")
                    .HasColumnName("CREATED_DATE");

                entity.Property(e => e.FieldName)
                    .HasMaxLength(256)
                    .IsUnicode(false)
                    .HasColumnName("FIELD_NAME");

                entity.Property(e => e.NewValue)
                    .HasColumnType("CLOB")
                    .HasColumnName("NEW_VALUE");

                entity.Property(e => e.OldValue)
                    .HasColumnType("CLOB")
                    .HasColumnName("OLD_VALUE");

                entity.Property(e => e.RealTableId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("REAL_TABLE_ID");

                entity.Property(e => e.UpdateTable)
                    .HasColumnType("NUMBER")
                    .HasColumnName("UPDATE_TABLE");
            });

            modelBuilder.Entity<EpBondInfoOverviewFile>(entity =>
            {
                entity.ToTable("EP_BOND_INFO_OVERVIEW_FILE");

                entity.Property(e => e.Id)
                    .HasColumnType("NUMBER")
                    .HasColumnName("ID");

                entity.Property(e => e.BondSecondaryId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("BOND_SECONDARY_ID");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(256)
                    .IsUnicode(false)
                    .HasColumnName("CREATED_BY");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("DATE")
                    .HasColumnName("CREATED_DATE")
                    .HasDefaultValueSql("SYSDATE");

                entity.Property(e => e.Deleted)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("DELETED")
                    .HasDefaultValueSql("('N')")
                    .IsFixedLength(true);

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("MODIFIED_BY");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("DATE")
                    .HasColumnName("MODIFIED_DATE");

                entity.Property(e => e.Status)
                    .HasPrecision(1)
                    .HasColumnName("STATUS")
                    .HasDefaultValueSql("1");

                entity.Property(e => e.Title)
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("TITLE");

                entity.Property(e => e.TradingProviderId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("TRADING_PROVIDER_ID");

                entity.Property(e => e.Url)
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("URL");
            });

            modelBuilder.Entity<EpBondInfoOverviewOrg>(entity =>
            {
                entity.ToTable("EP_BOND_INFO_OVERVIEW_ORG");

                entity.Property(e => e.Id)
                    .HasColumnType("NUMBER")
                    .HasColumnName("ID");

                entity.Property(e => e.BondSecondaryId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("BOND_SECONDARY_ID");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(256)
                    .IsUnicode(false)
                    .HasColumnName("CREATED_BY");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("DATE")
                    .HasColumnName("CREATED_DATE")
                    .HasDefaultValueSql("SYSDATE");

                entity.Property(e => e.Deleted)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("DELETED")
                    .HasDefaultValueSql("('N')")
                    .IsFixedLength(true);

                entity.Property(e => e.Icon)
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("ICON");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("MODIFIED_BY");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("DATE")
                    .HasColumnName("MODIFIED_DATE");

                entity.Property(e => e.Name)
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("NAME");

                entity.Property(e => e.OrgCode)
                    .HasMaxLength(128)
                    .IsUnicode(false)
                    .HasColumnName("ORG_CODE");

                entity.Property(e => e.Status)
                    .HasPrecision(1)
                    .HasColumnName("STATUS")
                    .HasDefaultValueSql("1");

                entity.Property(e => e.TradingProviderId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("TRADING_PROVIDER_ID");

                entity.Property(e => e.Url)
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("URL");
            });

            modelBuilder.Entity<EpBondInterestPayment>(entity =>
            {
                entity.ToTable("EP_BOND_INTEREST_PAYMENT");

                entity.HasIndex(e => new { e.Id, e.OrderId, e.PayDate, e.TradingProviderId, e.Status }, "IX_BOND_INTEREST_PAYMENT");

                entity.Property(e => e.Id)
                    .HasColumnType("NUMBER")
                    .HasColumnName("ID");

                entity.Property(e => e.AmountMoney)
                    .HasColumnType("NUMBER")
                    .HasColumnName("AMOUNT_MONEY");

                entity.Property(e => e.ApproveBy)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("APPROVE_BY");

                entity.Property(e => e.ApproveDate)
                    .HasColumnType("DATE")
                    .HasColumnName("APPROVE_DATE");

                entity.Property(e => e.ApproveIp)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("APPROVE_IP");

                entity.Property(e => e.CifCode)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("CIF_CODE");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("CREATED_BY");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("DATE")
                    .HasColumnName("CREATED_DATE")
                    .HasDefaultValueSql("SYSDATE");

                entity.Property(e => e.Deleted)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("DELETED")
                    .HasDefaultValueSql("'N' ")
                    .IsFixedLength(true);

                entity.Property(e => e.IsLastPeriod)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("IS_LAST_PERIOD")
                    .HasDefaultValueSql("'N' ")
                    .IsFixedLength(true);

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("MODIFIED_BY");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("DATE")
                    .HasColumnName("MODIFIED_DATE");

                entity.Property(e => e.OrderId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("ORDER_ID");

                entity.Property(e => e.PayDate)
                    .HasColumnType("DATE")
                    .HasColumnName("PAY_DATE");

                entity.Property(e => e.PeriodIndex)
                    .HasColumnType("NUMBER")
                    .HasColumnName("PERIOD_INDEX");

                entity.Property(e => e.PolicyDetailId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("POLICY_DETAIL_ID");

                entity.Property(e => e.Status)
                    .HasColumnType("NUMBER")
                    .HasColumnName("STATUS");

                entity.Property(e => e.TradingProviderId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("TRADING_PROVIDER_ID");
            });

            modelBuilder.Entity<EpBondInterestPaymentDate>(entity =>
            {
                entity.ToTable("EP_BOND_INTEREST_PAYMENT_DATE");

                entity.Property(e => e.Id)
                    .HasColumnType("NUMBER")
                    .HasColumnName("ID");

                entity.Property(e => e.ClosePerDate)
                    .HasColumnType("DATE")
                    .HasColumnName("CLOSE_PER_DATE");

                entity.Property(e => e.OrderId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("ORDER_ID");

                entity.Property(e => e.PayDate)
                    .HasColumnType("DATE")
                    .HasColumnName("PAY_DATE");

                entity.Property(e => e.PeriodIndex)
                    .HasColumnType("NUMBER")
                    .HasColumnName("PERIOD_INDEX");

                entity.Property(e => e.TypeDate)
                    .HasColumnType("NUMBER")
                    .HasColumnName("TYPE_DATE");
            });

            modelBuilder.Entity<EpBondOrder>(entity =>
            {
                entity.HasKey(e => e.OrderId)
                    .HasName("SYS_C007647");

                entity.ToTable("EP_BOND_ORDER");

                entity.HasIndex(e => new { e.TradingProviderId, e.Deleted, e.BondPolicyDetailId, e.BondPolicyId, e.BondSecondaryId, e.BusinessCustomerBankAccId, e.ContractAddressId, e.DepartmentId, e.InvestorBankAccId, e.InvestorIdenId, e.DepartmentIdSub, e.ProductBondId, e.SaleReferralCode, e.SaleReferralCodeSub, e.SaleOrderId, e.Status, e.OrderId }, "IX_BOND_ORDER");

                entity.Property(e => e.OrderId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("ORDER_ID");

                entity.Property(e => e.ActiveDate)
                    .HasColumnType("DATE")
                    .HasColumnName("ACTIVE_DATE");

                entity.Property(e => e.ApproveBy)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("APPROVE_BY");

                entity.Property(e => e.ApproveDate)
                    .HasColumnType("DATE")
                    .HasColumnName("APPROVE_DATE");

                entity.Property(e => e.BondPolicyDetailId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("BOND_POLICY_DETAIL_ID");

                entity.Property(e => e.BondPolicyId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("BOND_POLICY_ID");

                entity.Property(e => e.BondSecondaryId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("BOND_SECONDARY_ID");

                entity.Property(e => e.BusinessCustomerBankAccId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("BUSINESS_CUSTOMER_BANK_ACC_ID");

                entity.Property(e => e.BuyDate)
                    .HasColumnType("DATE")
                    .HasColumnName("BUY_DATE");

                entity.Property(e => e.CifCode)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("CIF_CODE");

                entity.Property(e => e.ContractAddressId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("CONTRACT_ADDRESS_ID");

                entity.Property(e => e.ContractCode)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("CONTRACT_CODE");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("CREATED_BY");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("DATE")
                    .HasColumnName("CREATED_DATE")
                    .HasDefaultValueSql("SYSDATE");

                entity.Property(e => e.Deleted)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("DELETED")
                    .HasDefaultValueSql("( 'N' )")
                    .IsFixedLength(true);

                entity.Property(e => e.DeliveryCode)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("DELIVERY_CODE");

                entity.Property(e => e.DeliveryDate)
                    .HasColumnType("DATE")
                    .HasColumnName("DELIVERY_DATE");

                entity.Property(e => e.DeliveryDateModifiedBy)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("DELIVERY_DATE_MODIFIED_BY");

                entity.Property(e => e.DeliveryStatus)
                    .HasColumnType("NUMBER")
                    .HasColumnName("DELIVERY_STATUS");

                entity.Property(e => e.DepartmentId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("DEPARTMENT_ID");

                entity.Property(e => e.DepartmentIdSub)
                    .HasColumnType("NUMBER")
                    .HasColumnName("DEPARTMENT_ID_SUB");

                entity.Property(e => e.FinishedDate)
                    .HasColumnType("DATE")
                    .HasColumnName("FINISHED_DATE");

                entity.Property(e => e.FinishedDateModifiedBy)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("FINISHED_DATE_MODIFIED_BY");

                entity.Property(e => e.InvestDate)
                    .HasColumnType("DATE")
                    .HasColumnName("INVEST_DATE");

                entity.Property(e => e.InvestorBankAccId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("INVESTOR_BANK_ACC_ID");

                entity.Property(e => e.InvestorIdenId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("INVESTOR_IDEN_ID");

                entity.Property(e => e.IpAddressCreated)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("IP_ADDRESS_CREATED");

                entity.Property(e => e.IsInterest)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("IS_INTEREST")
                    .IsFixedLength(true);

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("MODIFIED_BY");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("DATE")
                    .HasColumnName("MODIFIED_DATE");

                entity.Property(e => e.PaymentFullDate)
                    .HasColumnType("DATE")
                    .HasColumnName("PAYMENT_FULL_DATE");

                entity.Property(e => e.PendingDate)
                    .HasColumnType("DATE")
                    .HasColumnName("PENDING_DATE");

                entity.Property(e => e.PendingDateModifiedBy)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("PENDING_DATE_MODIFIED_BY");

                entity.Property(e => e.Price)
                    .HasColumnType("NUMBER")
                    .HasColumnName("PRICE");

                entity.Property(e => e.ProductBondId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("PRODUCT_BOND_ID");

                entity.Property(e => e.Quantity)
                    .HasColumnType("NUMBER")
                    .HasColumnName("QUANTITY");

                entity.Property(e => e.ReceivedDate)
                    .HasColumnType("DATE")
                    .HasColumnName("RECEIVED_DATE");

                entity.Property(e => e.ReceivedDateModifiedBy)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("RECEIVED_DATE_MODIFIED_BY");

                entity.Property(e => e.RenewalsPolicyDetailId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("RENEWALS_POLICY_DETAIL_ID");

                entity.Property(e => e.RequestContractDate)
                    .HasColumnType("DATE")
                    .HasColumnName("REQUEST_CONTRACT_DATE");

                entity.Property(e => e.SaleOrderId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("SALE_ORDER_ID");

                entity.Property(e => e.SaleReferralCode)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("SALE_REFERRAL_CODE");

                entity.Property(e => e.SaleReferralCodeSub)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("SALE_REFERRAL_CODE_SUB");

                entity.Property(e => e.SettlementDate)
                    .HasColumnType("DATE")
                    .HasColumnName("SETTLEMENT_DATE");

                entity.Property(e => e.SettlementMethod)
                    .HasColumnType("NUMBER")
                    .HasColumnName("SETTLEMENT_METHOD")
                    .HasDefaultValueSql("1");

                entity.Property(e => e.Source)
                    .HasColumnType("NUMBER")
                    .HasColumnName("SOURCE");

                entity.Property(e => e.Status)
                    .HasColumnType("NUMBER")
                    .HasColumnName("STATUS");

                entity.Property(e => e.TotalValue)
                    .HasColumnType("NUMBER")
                    .HasColumnName("TOTAL_VALUE");

                entity.Property(e => e.TradingProviderId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("TRADING_PROVIDER_ID");
            });

            modelBuilder.Entity<EpBondOrderContractFile>(entity =>
            {
                entity.HasKey(e => e.OrderContractFileId)
                    .HasName("PK_EP_ORDER_CON_FILE_ID");

                entity.ToTable("EP_BOND_ORDER_CONTRACT_FILE");

                entity.Property(e => e.OrderContractFileId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("ORDER_CONTRACT_FILE_ID");

                entity.Property(e => e.ContractTempId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("CONTRACT_TEMP_ID");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("CREATED_BY");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("DATE")
                    .HasColumnName("CREATED_DATE")
                    .HasDefaultValueSql("SYSDATE");

                entity.Property(e => e.Deleted)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("DELETED")
                    .HasDefaultValueSql("'N'\n   ")
                    .IsFixedLength(true);

                entity.Property(e => e.FileUrl)
                    .HasMaxLength(256)
                    .IsUnicode(false)
                    .HasColumnName("FILE_URL");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("MODIFIED_BY");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("DATE")
                    .HasColumnName("MODIFIED_DATE");

                entity.Property(e => e.OrderId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("ORDER_ID");

                entity.Property(e => e.TradingProviderId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("TRADING_PROVIDER_ID");
            });

            modelBuilder.Entity<EpBondOrderPayment>(entity =>
            {
                entity.HasKey(e => e.OrderPaymentId)
                    .HasName("SYS_C007666");

                entity.ToTable("EP_BOND_ORDER_PAYMENT");

                entity.Property(e => e.OrderPaymentId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("ORDER_PAYMENT_ID");

                entity.Property(e => e.ApproveBy)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("APPROVE_BY");

                entity.Property(e => e.ApproveDate)
                    .HasColumnType("DATE")
                    .HasColumnName("APPROVE_DATE");

                entity.Property(e => e.CancelBy)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("CANCEL_BY");

                entity.Property(e => e.CancelDate)
                    .HasColumnType("DATE")
                    .HasColumnName("CANCEL_DATE");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("CREATED_BY");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("DATE")
                    .HasColumnName("CREATED_DATE")
                    .HasDefaultValueSql("SYSDATE");

                entity.Property(e => e.Deleted)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("DELETED")
                    .HasDefaultValueSql("( 'N' )")
                    .IsFixedLength(true);

                entity.Property(e => e.Description)
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("DESCRIPTION");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("MODIFIED_BY");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("DATE")
                    .HasColumnName("MODIFIED_DATE");

                entity.Property(e => e.OrderId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("ORDER_ID");

                entity.Property(e => e.OrderNo)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("ORDER_NO");

                entity.Property(e => e.PaymentAmnount)
                    .HasColumnType("NUMBER")
                    .HasColumnName("PAYMENT_AMNOUNT");

                entity.Property(e => e.PaymentType)
                    .HasColumnType("NUMBER")
                    .HasColumnName("PAYMENT_TYPE");

                entity.Property(e => e.Status)
                    .HasColumnType("NUMBER")
                    .HasColumnName("STATUS");

                entity.Property(e => e.TradingProviderId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("TRADING_PROVIDER_ID");

                entity.Property(e => e.TranClassify)
                    .HasColumnType("NUMBER")
                    .HasColumnName("TRAN_CLASSIFY");

                entity.Property(e => e.TranDate)
                    .HasColumnType("DATE")
                    .HasColumnName("TRAN_DATE");

                entity.Property(e => e.TranType)
                    .HasColumnType("NUMBER")
                    .HasColumnName("TRAN_TYPE");
            });

            modelBuilder.Entity<EpBondPartnerCalendar>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("EP_BOND_PARTNER_CALENDAR");

                entity.HasIndex(e => new { e.IsDayoff, e.PartnerId, e.WorkingDate }, "IX_BOND_PARTNER_CALENDAR");

                entity.Property(e => e.BusDate)
                    .HasColumnType("DATE")
                    .HasColumnName("BUS_DATE");

                entity.Property(e => e.IsDayoff)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("IS_DAYOFF")
                    .HasDefaultValueSql("('N')")
                    .IsFixedLength(true);

                entity.Property(e => e.PartnerId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("PARTNER_ID");

                entity.Property(e => e.WorkingDate)
                    .HasColumnType("DATE")
                    .HasColumnName("WORKING_DATE");

                entity.Property(e => e.WorkingYear)
                    .HasColumnType("NUMBER")
                    .HasColumnName("WORKING_YEAR");
            });

            modelBuilder.Entity<EpBondReceiveContrTemplate>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("EP_BOND_RECEIVE_CONTR_TEMPLATE");

                entity.Property(e => e.BondSecondaryId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("BOND_SECONDARY_ID");

                entity.Property(e => e.Code)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("CODE");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(256)
                    .IsUnicode(false)
                    .HasColumnName("CREATED_BY");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("DATE")
                    .HasColumnName("CREATED_DATE");

                entity.Property(e => e.Deleted)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("DELETED")
                    .HasDefaultValueSql("('N')")
                    .IsFixedLength(true);

                entity.Property(e => e.FileUrl)
                    .HasMaxLength(1024)
                    .IsUnicode(false)
                    .HasColumnName("FILE_URL");

                entity.Property(e => e.Id)
                    .HasColumnType("NUMBER")
                    .HasColumnName("ID");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(256)
                    .IsUnicode(false)
                    .HasColumnName("MODIFIED_BY");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("DATE")
                    .HasColumnName("MODIFIED_DATE");

                entity.Property(e => e.Name)
                    .HasMaxLength(256)
                    .IsUnicode(false)
                    .HasColumnName("NAME");

                entity.Property(e => e.Status)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("STATUS")
                    .HasDefaultValueSql("('A')")
                    .IsFixedLength(true);

                entity.Property(e => e.TradingProviderId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("TRADING_PROVIDER_ID");
            });

            modelBuilder.Entity<EpBondRenewalsRequest>(entity =>
            {
                entity.ToTable("EP_BOND_RENEWALS_REQUEST");

                entity.Property(e => e.Id)
                    .HasColumnType("NUMBER")
                    .HasColumnName("ID");

                entity.Property(e => e.Deleted)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("DELETED")
                    .HasDefaultValueSql("'N' ")
                    .IsFixedLength(true);

                entity.Property(e => e.OrderId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("ORDER_ID");

                entity.Property(e => e.RenewalsPolicyDetailId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("RENEWALS_POLICY_DETAIL_ID");

                entity.Property(e => e.SettlementMethod)
                    .HasColumnType("NUMBER")
                    .HasColumnName("SETTLEMENT_METHOD");
            });

            modelBuilder.Entity<EpBondSecondaryContract>(entity =>
            {
                entity.HasKey(e => e.SecondaryContractFileId)
                    .HasName("EP_SECONDARY_CONTRACT_FILE_PK");

                entity.ToTable("EP_BOND_SECONDARY_CONTRACT");

                entity.Property(e => e.SecondaryContractFileId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("SECONDARY_CONTRACT_FILE_ID");

                entity.Property(e => e.ContractTempId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("CONTRACT_TEMP_ID");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("CREATED_BY");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("DATE")
                    .HasColumnName("CREATED_DATE")
                    .HasDefaultValueSql("SYSDATE");

                entity.Property(e => e.Deleted)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("DELETED")
                    .HasDefaultValueSql("'N'\n   ")
                    .IsFixedLength(true);

                entity.Property(e => e.FileScanUrl)
                    .HasMaxLength(256)
                    .IsUnicode(false)
                    .HasColumnName("FILE_SCAN_URL");

                entity.Property(e => e.FileSignatureUrl)
                    .HasMaxLength(256)
                    .IsUnicode(false)
                    .HasColumnName("FILE_SIGNATURE_URL");

                entity.Property(e => e.FileTempUrl)
                    .HasMaxLength(256)
                    .IsUnicode(false)
                    .HasColumnName("FILE_TEMP_URL");

                entity.Property(e => e.IsSign)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("IS_SIGN")
                    .HasDefaultValueSql("'N'")
                    .IsFixedLength(true);

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("MODIFIED_BY");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("DATE")
                    .HasColumnName("MODIFIED_DATE");

                entity.Property(e => e.OrderId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("ORDER_ID");

                entity.Property(e => e.PageSign)
                    .HasColumnType("NUMBER")
                    .HasColumnName("PAGE_SIGN")
                    .HasDefaultValueSql("'1'");

                entity.Property(e => e.TradingProviderId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("TRADING_PROVIDER_ID");
            });

            modelBuilder.Entity<EpCalendar>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("EP_CALENDAR");

                entity.HasIndex(e => new { e.TradingProviderId, e.WorkingDate, e.IsDayoff }, "IX_CALENDAR");

                entity.Property(e => e.BusDate)
                    .HasColumnType("DATE")
                    .HasColumnName("BUS_DATE");

                entity.Property(e => e.IsDayoff)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("IS_DAYOFF")
                    .HasDefaultValueSql("('N')")
                    .IsFixedLength(true);

                entity.Property(e => e.TradingProviderId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("TRADING_PROVIDER_ID");

                entity.Property(e => e.WorkingDate)
                    .HasColumnType("DATE")
                    .HasColumnName("WORKING_DATE");

                entity.Property(e => e.WorkingYear)
                    .HasColumnType("NUMBER")
                    .HasColumnName("WORKING_YEAR");
            });

            modelBuilder.Entity<EpCifCode>(entity =>
            {
                entity.HasKey(e => e.CifId)
                    .HasName("EP_CIF_CODE_PK");

                entity.ToTable("EP_CIF_CODE");

                entity.HasIndex(e => new { e.Deleted, e.CifCode, e.InvestorId, e.BusinessCustomerId }, "IX_CIF_CODE");

                entity.Property(e => e.CifId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("CIF_ID");

                entity.Property(e => e.BusinessCustomerId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("BUSINESS_CUSTOMER_ID");

                entity.Property(e => e.CifCode)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("CIF_CODE");

                entity.Property(e => e.Deleted)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("DELETED")
                    .HasDefaultValueSql("'N'\n   ")
                    .IsFixedLength(true);

                entity.Property(e => e.InvestorId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("INVESTOR_ID");
            });

            modelBuilder.Entity<EpConfiguration>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("EP_CONFIGURATION");

                entity.Property(e => e.Grkey)
                    .HasMaxLength(256)
                    .IsUnicode(false)
                    .HasColumnName("GRKEY");

                entity.Property(e => e.Key)
                    .HasMaxLength(256)
                    .IsUnicode(false)
                    .HasColumnName("KEY");

                entity.Property(e => e.Value)
                    .HasColumnType("CLOB")
                    .HasColumnName("VALUE");
            });

            modelBuilder.Entity<EpContractTemplate>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("EP_CONTRACT_TEMPLATE");

                entity.Property(e => e.BondSecondaryId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("BOND_SECONDARY_ID");

                entity.Property(e => e.Classify)
                    .HasColumnType("NUMBER")
                    .HasColumnName("CLASSIFY");

                entity.Property(e => e.Code)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("CODE");

                entity.Property(e => e.ContractTempUrl)
                    .HasMaxLength(1024)
                    .IsUnicode(false)
                    .HasColumnName("CONTRACT_TEMP_URL");

                entity.Property(e => e.ContractType)
                    .HasColumnType("NUMBER")
                    .HasColumnName("CONTRACT_TYPE");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(256)
                    .IsUnicode(false)
                    .HasColumnName("CREATED_BY");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("DATE")
                    .HasColumnName("CREATED_DATE");

                entity.Property(e => e.Deleted)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("DELETED")
                    .HasDefaultValueSql("('N')")
                    .IsFixedLength(true);

                entity.Property(e => e.Id)
                    .HasColumnType("NUMBER")
                    .HasColumnName("ID");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(256)
                    .IsUnicode(false)
                    .HasColumnName("MODIFIED_BY");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("DATE")
                    .HasColumnName("MODIFIED_DATE");

                entity.Property(e => e.Name)
                    .HasMaxLength(256)
                    .IsUnicode(false)
                    .HasColumnName("NAME");

                entity.Property(e => e.Status)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("STATUS")
                    .HasDefaultValueSql("('A')")
                    .IsFixedLength(true);

                entity.Property(e => e.TradingProviderId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("TRADING_PROVIDER_ID");

                entity.Property(e => e.Type)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("TYPE")
                    .HasDefaultValueSql("('I')")
                    .IsFixedLength(true);
            });

            modelBuilder.Entity<EpCoreApprove>(entity =>
            {
                entity.HasKey(e => e.ApproveId)
                    .HasName("SYS_C007617");

                entity.ToTable("EP_CORE_APPROVE");

                entity.Property(e => e.ApproveId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("APPROVE_ID");

                entity.Property(e => e.ActionType)
                    .HasColumnType("NUMBER")
                    .HasColumnName("ACTION_TYPE");

                entity.Property(e => e.ApproveDate)
                    .HasColumnType("DATE")
                    .HasColumnName("APPROVE_DATE");

                entity.Property(e => e.ApproveNote)
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("APPROVE_NOTE");

                entity.Property(e => e.CancelDate)
                    .HasColumnType("DATE")
                    .HasColumnName("CANCEL_DATE");

                entity.Property(e => e.CancelNote)
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("CANCEL_NOTE");

                entity.Property(e => e.CloseDate)
                    .HasColumnType("DATE")
                    .HasColumnName("CLOSE_DATE");

                entity.Property(e => e.CloseNote)
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("CLOSE_NOTE");

                entity.Property(e => e.DataStatus)
                    .HasColumnType("NUMBER")
                    .HasColumnName("DATA_STATUS");

                entity.Property(e => e.DataStatusStr)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("DATA_STATUS_STR")
                    .IsFixedLength(true);

                entity.Property(e => e.DataType)
                    .HasColumnType("NUMBER")
                    .HasColumnName("DATA_TYPE");

                entity.Property(e => e.IsCheck)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("IS_CHECK")
                    .IsFixedLength(true);

                entity.Property(e => e.OpenDate)
                    .HasColumnType("DATE")
                    .HasColumnName("OPEN_DATE");

                entity.Property(e => e.OpenNote)
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("OPEN_NOTE");

                entity.Property(e => e.PartnerId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("PARTNER_ID");

                entity.Property(e => e.ReferId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("REFER_ID");

                entity.Property(e => e.ReferIdTemp)
                    .HasColumnType("NUMBER")
                    .HasColumnName("REFER_ID_TEMP");

                entity.Property(e => e.RequestDate)
                    .HasColumnType("DATE")
                    .HasColumnName("REQUEST_DATE");

                entity.Property(e => e.RequestNote)
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("REQUEST_NOTE");

                entity.Property(e => e.Status)
                    .HasColumnType("NUMBER")
                    .HasColumnName("STATUS");

                entity.Property(e => e.Summary)
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("SUMMARY");

                entity.Property(e => e.TradingProviderId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("TRADING_PROVIDER_ID");

                entity.Property(e => e.UserApproveId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("USER_APPROVE_ID");

                entity.Property(e => e.UserCheckId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("USER_CHECK_ID");

                entity.Property(e => e.UserRequestId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("USER_REQUEST_ID");
            });

            modelBuilder.Entity<EpCoreBank>(entity =>
            {
                entity.HasKey(e => e.BankId);

                entity.ToTable("EP_CORE_BANK");

                entity.Property(e => e.BankId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("BANK_ID");

                entity.Property(e => e.BankCode)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("BANK_CODE");

                entity.Property(e => e.BankName)
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("BANK_NAME");

                entity.Property(e => e.Bin)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("BIN");

                entity.Property(e => e.FullBankName)
                    .HasMaxLength(1000)
                    .HasColumnName("FULL_BANK_NAME");

                entity.Property(e => e.Logo)
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("LOGO");

                entity.Property(e => e.PvcbBankId)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("PVCB_BANK_ID");
            });

            modelBuilder.Entity<EpCoreBusinessCusBankTemp>(entity =>
            {
                entity.ToTable("EP_CORE_BUSINESS_CUS_BANK_TEMP");

                entity.Property(e => e.Id)
                    .HasColumnType("NUMBER")
                    .HasColumnName("ID");

                entity.Property(e => e.BankAccName)
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("BANK_ACC_NAME");

                entity.Property(e => e.BankAccNo)
                    .HasMaxLength(128)
                    .IsUnicode(false)
                    .HasColumnName("BANK_ACC_NO");

                entity.Property(e => e.BankBranchName)
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("BANK_BRANCH_NAME");

                entity.Property(e => e.BankId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("BANK_ID");

                entity.Property(e => e.BusinessCustomerBankAccId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("BUSINESS_CUSTOMER_BANK_ACC_ID");

                entity.Property(e => e.BusinessCustomerId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("BUSINESS_CUSTOMER_ID");

                entity.Property(e => e.BusinessCustomerTempId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("BUSINESS_CUSTOMER_TEMP_ID");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(128)
                    .IsUnicode(false)
                    .HasColumnName("CREATED_BY");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("DATE")
                    .HasColumnName("CREATED_DATE");

                entity.Property(e => e.Deleted)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("DELETED");

                entity.Property(e => e.IsDefault)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("IS_DEFAULT");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(128)
                    .IsUnicode(false)
                    .HasColumnName("MODIFIED_BY");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("DATE")
                    .HasColumnName("MODIFIED_DATE");

                entity.Property(e => e.Status)
                    .HasColumnType("NUMBER")
                    .HasColumnName("STATUS");
            });

            modelBuilder.Entity<EpCoreBusinessCusPartner>(entity =>
            {
                entity.ToTable("EP_CORE_BUSINESS_CUS_PARTNER");

                entity.HasIndex(e => new { e.Deleted, e.PartnerId, e.BusinessCustomerId }, "IX_CORE_BUSINESS_CUS_PARTNER");

                entity.Property(e => e.Id)
                    .HasColumnType("NUMBER")
                    .HasColumnName("ID");

                entity.Property(e => e.BusinessCustomerId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("BUSINESS_CUSTOMER_ID");

                entity.Property(e => e.CreateBy)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("CREATE_BY");

                entity.Property(e => e.CreateDate)
                    .HasColumnType("DATE")
                    .HasColumnName("CREATE_DATE")
                    .HasDefaultValueSql("SYSDATE");

                entity.Property(e => e.Deleted)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("DELETED")
                    .HasDefaultValueSql("('N')\n   ")
                    .IsFixedLength(true);

                entity.Property(e => e.PartnerId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("PARTNER_ID");
            });

            modelBuilder.Entity<EpCoreBusinessCusTrading>(entity =>
            {
                entity.ToTable("EP_CORE_BUSINESS_CUS_TRADING");

                entity.HasIndex(e => new { e.Deleted, e.BusinessCustomerId, e.TradingProviderId }, "IX_CORE_BUSINESS_CUS_TRADING");

                entity.Property(e => e.Id)
                    .HasColumnType("NUMBER")
                    .HasColumnName("ID");

                entity.Property(e => e.BusinessCustomerId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("BUSINESS_CUSTOMER_ID");

                entity.Property(e => e.CreateBy)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("CREATE_BY");

                entity.Property(e => e.CreateDate)
                    .HasColumnType("DATE")
                    .HasColumnName("CREATE_DATE")
                    .HasDefaultValueSql("SYSDATE");

                entity.Property(e => e.Deleted)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("DELETED")
                    .HasDefaultValueSql("('N')\n   ")
                    .IsFixedLength(true);

                entity.Property(e => e.TradingProviderId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("TRADING_PROVIDER_ID");
            });

            modelBuilder.Entity<EpCoreBusinessCustomer>(entity =>
            {
                entity.HasKey(e => e.BusinessCustomerId)
                    .HasName("EP_CORE_BUSINESS_CUSTOMERS_PK");

                entity.ToTable("EP_CORE_BUSINESS_CUSTOMERS");

                entity.HasIndex(e => new { e.Deleted, e.TaxCode, e.BusinessCustomerId, e.ReferralCodeSelf, e.Code, e.Name, e.ShortName, e.Address }, "IX_CORE_BUSINESS_CUSTOMERS");

                entity.Property(e => e.BusinessCustomerId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("BUSINESS_CUSTOMER_ID");

                entity.Property(e => e.Address)
                    .HasMaxLength(1024)
                    .IsUnicode(false)
                    .HasColumnName("ADDRESS");

                entity.Property(e => e.AllowDuplicate)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("ALLOW_DUPLICATE")
                    .HasDefaultValueSql("'N' ")
                    .IsFixedLength(true);

                entity.Property(e => e.AvatarImageUrl)
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("AVATAR_IMAGE_URL");

                entity.Property(e => e.BankId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("BANK_ID");

                entity.Property(e => e.BusinessRegistrationImg)
                    .HasMaxLength(256)
                    .IsUnicode(false)
                    .HasColumnName("BUSINESS_REGISTRATION_IMG");

                entity.Property(e => e.Capital)
                    .HasColumnType("NUMBER(20)")
                    .HasColumnName("CAPITAL");

                entity.Property(e => e.Code)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("CODE");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("CREATED_BY");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("DATE")
                    .HasColumnName("CREATED_DATE");

                entity.Property(e => e.DateModified)
                    .HasColumnType("DATE")
                    .HasColumnName("DATE_MODIFIED");

                entity.Property(e => e.DecisionDate)
                    .HasColumnType("DATE")
                    .HasColumnName("DECISION_DATE");

                entity.Property(e => e.DecisionNo)
                    .HasMaxLength(128)
                    .IsUnicode(false)
                    .HasColumnName("DECISION_NO");

                entity.Property(e => e.Deleted)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("DELETED")
                    .HasDefaultValueSql("'N'");

                entity.Property(e => e.Email)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("EMAIL");

                entity.Property(e => e.Fanpage)
                    .HasMaxLength(256)
                    .IsUnicode(false)
                    .HasColumnName("FANPAGE");

                entity.Property(e => e.IsCheck)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("IS_CHECK")
                    .IsFixedLength(true);

                entity.Property(e => e.Key)
                    .HasMaxLength(256)
                    .IsUnicode(false)
                    .HasColumnName("KEY");

                entity.Property(e => e.LicenseDate)
                    .HasColumnType("DATE")
                    .HasColumnName("LICENSE_DATE");

                entity.Property(e => e.LicenseIssuer)
                    .HasMaxLength(1024)
                    .IsUnicode(false)
                    .HasColumnName("LICENSE_ISSUER");

                entity.Property(e => e.Mobile)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("MOBILE");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(32)
                    .IsUnicode(false)
                    .HasColumnName("MODIFIED_BY");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("DATE")
                    .HasColumnName("MODIFIED_DATE");

                entity.Property(e => e.Name)
                    .HasMaxLength(256)
                    .IsUnicode(false)
                    .HasColumnName("NAME");

                entity.Property(e => e.Nation)
                    .HasMaxLength(256)
                    .IsUnicode(false)
                    .HasColumnName("NATION");

                entity.Property(e => e.NumberModified)
                    .HasColumnType("NUMBER")
                    .HasColumnName("NUMBER_MODIFIED");

                entity.Property(e => e.Phone)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("PHONE");

                entity.Property(e => e.ReferralCodeSelf)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("REFERRAL_CODE_SELF");

                entity.Property(e => e.RepAddress)
                    .HasMaxLength(1024)
                    .IsUnicode(false)
                    .HasColumnName("REP_ADDRESS");

                entity.Property(e => e.RepBirthDate)
                    .HasColumnType("DATE")
                    .HasColumnName("REP_BIRTH_DATE");

                entity.Property(e => e.RepIdDate)
                    .HasColumnType("DATE")
                    .HasColumnName("REP_ID_DATE");

                entity.Property(e => e.RepIdIssuer)
                    .HasMaxLength(256)
                    .IsUnicode(false)
                    .HasColumnName("REP_ID_ISSUER");

                entity.Property(e => e.RepIdNo)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("REP_ID_NO");

                entity.Property(e => e.RepName)
                    .HasMaxLength(256)
                    .IsUnicode(false)
                    .HasColumnName("REP_NAME");

                entity.Property(e => e.RepPosition)
                    .HasMaxLength(256)
                    .IsUnicode(false)
                    .HasColumnName("REP_POSITION");

                entity.Property(e => e.RepSex)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("REP_SEX");

                entity.Property(e => e.Secret)
                    .HasMaxLength(256)
                    .IsUnicode(false)
                    .HasColumnName("SECRET");

                entity.Property(e => e.Server)
                    .HasMaxLength(256)
                    .IsUnicode(false)
                    .HasColumnName("SERVER");

                entity.Property(e => e.ShortName)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("SHORT_NAME");

                entity.Property(e => e.StampImageUrl)
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("STAMP_IMAGE_URL");

                entity.Property(e => e.Status)
                    .HasColumnType("NUMBER")
                    .HasColumnName("STATUS");

                entity.Property(e => e.TaxCode)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("TAX_CODE");

                entity.Property(e => e.TradingAddress)
                    .HasMaxLength(1024)
                    .IsUnicode(false)
                    .HasColumnName("TRADING_ADDRESS");

                entity.Property(e => e.Website)
                    .HasMaxLength(256)
                    .IsUnicode(false)
                    .HasColumnName("WEBSITE");
            });

            modelBuilder.Entity<EpCoreBusinessCustomerBank>(entity =>
            {
                entity.HasKey(e => e.BusinessCustomerBankAccId)
                    .HasName("EP_CORE_BUSINESS_CUSTOMER_BANK_PK");

                entity.ToTable("EP_CORE_BUSINESS_CUSTOMER_BANK");

                entity.Property(e => e.BusinessCustomerBankAccId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("BUSINESS_CUSTOMER_BANK_ACC_ID");

                entity.Property(e => e.BankAccName)
                    .HasMaxLength(128)
                    .IsUnicode(false)
                    .HasColumnName("BANK_ACC_NAME");

                entity.Property(e => e.BankAccNo)
                    .HasMaxLength(128)
                    .IsUnicode(false)
                    .HasColumnName("BANK_ACC_NO");

                entity.Property(e => e.BankBranchName)
                    .HasMaxLength(128)
                    .IsUnicode(false)
                    .HasColumnName("BANK_BRANCH_NAME");

                entity.Property(e => e.BankId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("BANK_ID");

                entity.Property(e => e.BankName)
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("BANK_NAME");

                entity.Property(e => e.BusinessCustomerId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("BUSINESS_CUSTOMER_ID");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(128)
                    .IsUnicode(false)
                    .HasColumnName("CREATED_BY");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("DATE")
                    .HasColumnName("CREATED_DATE");

                entity.Property(e => e.Deleted)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("DELETED")
                    .HasDefaultValueSql("'N'")
                    .IsFixedLength(true);

                entity.Property(e => e.IsDefault)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("IS_DEFAULT")
                    .HasDefaultValueSql("'N'")
                    .IsFixedLength(true);

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(128)
                    .IsUnicode(false)
                    .HasColumnName("MODIFIED_BY");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("DATE")
                    .HasColumnName("MODIFIED_DATE");

                entity.Property(e => e.Status)
                    .HasColumnType("NUMBER")
                    .HasColumnName("STATUS");
            });

            modelBuilder.Entity<EpCoreBusinessCustomerTemp>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("EP_CORE_BUSINESS_CUSTOMER_TEMP");

                entity.Property(e => e.Address)
                    .HasMaxLength(1024)
                    .IsUnicode(false)
                    .HasColumnName("ADDRESS");

                entity.Property(e => e.AvatarImageUrl)
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("AVATAR_IMAGE_URL");

                entity.Property(e => e.BankAccName)
                    .HasMaxLength(128)
                    .IsUnicode(false)
                    .HasColumnName("BANK_ACC_NAME");

                entity.Property(e => e.BankAccNo)
                    .HasMaxLength(128)
                    .IsUnicode(false)
                    .HasColumnName("BANK_ACC_NO");

                entity.Property(e => e.BankBranchName)
                    .HasMaxLength(128)
                    .IsUnicode(false)
                    .HasColumnName("BANK_BRANCH_NAME");

                entity.Property(e => e.BankId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("BANK_ID");

                entity.Property(e => e.BankName)
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("BANK_NAME");

                entity.Property(e => e.BusinessCustomerBankAccId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("BUSINESS_CUSTOMER_BANK_ACC_ID");

                entity.Property(e => e.BusinessCustomerId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("BUSINESS_CUSTOMER_ID");

                entity.Property(e => e.BusinessCustomerTempId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("BUSINESS_CUSTOMER_TEMP_ID");

                entity.Property(e => e.BusinessRegistrationImg)
                    .HasMaxLength(256)
                    .IsUnicode(false)
                    .HasColumnName("BUSINESS_REGISTRATION_IMG");

                entity.Property(e => e.CancelBy)
                    .HasMaxLength(32)
                    .IsUnicode(false)
                    .HasColumnName("CANCEL_BY");

                entity.Property(e => e.CancelDate)
                    .HasColumnType("DATE")
                    .HasColumnName("CANCEL_DATE");

                entity.Property(e => e.Capital)
                    .HasColumnType("NUMBER")
                    .HasColumnName("CAPITAL");

                entity.Property(e => e.Code)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("CODE");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(32)
                    .IsUnicode(false)
                    .HasColumnName("CREATED_BY");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("DATE")
                    .HasColumnName("CREATED_DATE")
                    .HasDefaultValueSql("SYSDATE");

                entity.Property(e => e.DateModified)
                    .HasColumnType("DATE")
                    .HasColumnName("DATE_MODIFIED");

                entity.Property(e => e.DecisionDate)
                    .HasColumnType("DATE")
                    .HasColumnName("DECISION_DATE");

                entity.Property(e => e.DecisionNo)
                    .HasMaxLength(256)
                    .IsUnicode(false)
                    .HasColumnName("DECISION_NO");

                entity.Property(e => e.Deleted)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("DELETED")
                    .HasDefaultValueSql("'N'");

                entity.Property(e => e.Email)
                    .HasMaxLength(128)
                    .IsUnicode(false)
                    .HasColumnName("EMAIL");

                entity.Property(e => e.Fanpage)
                    .HasMaxLength(256)
                    .IsUnicode(false)
                    .HasColumnName("FANPAGE");

                entity.Property(e => e.GroupBusinessCusId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("GROUP_BUSINESS_CUS_ID");

                entity.Property(e => e.IsCheck)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("IS_CHECK")
                    .IsFixedLength(true);

                entity.Property(e => e.Key)
                    .HasMaxLength(256)
                    .IsUnicode(false)
                    .HasColumnName("KEY");

                entity.Property(e => e.LicenseDate)
                    .HasColumnType("DATE")
                    .HasColumnName("LICENSE_DATE");

                entity.Property(e => e.LicenseIssuer)
                    .HasMaxLength(1024)
                    .IsUnicode(false)
                    .HasColumnName("LICENSE_ISSUER");

                entity.Property(e => e.Mobile)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("MOBILE");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(32)
                    .IsUnicode(false)
                    .HasColumnName("MODIFIED_BY");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("DATE")
                    .HasColumnName("MODIFIED_DATE");

                entity.Property(e => e.Name)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("NAME");

                entity.Property(e => e.Nation)
                    .HasMaxLength(256)
                    .IsUnicode(false)
                    .HasColumnName("NATION");

                entity.Property(e => e.NumberModified)
                    .HasColumnType("NUMBER")
                    .HasColumnName("NUMBER_MODIFIED");

                entity.Property(e => e.PartnerId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("PARTNER_ID");

                entity.Property(e => e.Phone)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("PHONE");

                entity.Property(e => e.RepAddress)
                    .HasMaxLength(1024)
                    .IsUnicode(false)
                    .HasColumnName("REP_ADDRESS");

                entity.Property(e => e.RepBirthDate)
                    .HasColumnType("DATE")
                    .HasColumnName("REP_BIRTH_DATE");

                entity.Property(e => e.RepIdDate)
                    .HasColumnType("DATE")
                    .HasColumnName("REP_ID_DATE");

                entity.Property(e => e.RepIdIssuer)
                    .HasMaxLength(256)
                    .IsUnicode(false)
                    .HasColumnName("REP_ID_ISSUER");

                entity.Property(e => e.RepIdNo)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("REP_ID_NO");

                entity.Property(e => e.RepName)
                    .HasMaxLength(256)
                    .IsUnicode(false)
                    .HasColumnName("REP_NAME");

                entity.Property(e => e.RepPosition)
                    .HasMaxLength(256)
                    .IsUnicode(false)
                    .HasColumnName("REP_POSITION");

                entity.Property(e => e.RepSex)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("REP_SEX");

                entity.Property(e => e.Secret)
                    .HasMaxLength(256)
                    .IsUnicode(false)
                    .HasColumnName("SECRET");

                entity.Property(e => e.Server)
                    .HasMaxLength(256)
                    .IsUnicode(false)
                    .HasColumnName("SERVER");

                entity.Property(e => e.ShortName)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("SHORT_NAME");

                entity.Property(e => e.StampImageUrl)
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("STAMP_IMAGE_URL");

                entity.Property(e => e.Status)
                    .HasColumnType("NUMBER")
                    .HasColumnName("STATUS")
                    .HasDefaultValueSql("1");

                entity.Property(e => e.TaxCode)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("TAX_CODE");

                entity.Property(e => e.TradingAddress)
                    .HasMaxLength(1024)
                    .IsUnicode(false)
                    .HasColumnName("TRADING_ADDRESS");

                entity.Property(e => e.TradingProviderId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("TRADING_PROVIDER_ID");

                entity.Property(e => e.Website)
                    .HasMaxLength(256)
                    .IsUnicode(false)
                    .HasColumnName("WEBSITE");
            });

            modelBuilder.Entity<EpCoreBusinessLicenseFile>(entity =>
            {
                entity.ToTable("EP_CORE_BUSINESS_LICENSE_FILE");

                entity.Property(e => e.Id)
                    .HasColumnType("NUMBER")
                    .HasColumnName("ID");

                entity.Property(e => e.BusinessCustomerId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("BUSINESS_CUSTOMER_ID");

                entity.Property(e => e.BusinessCustomerTempId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("BUSINESS_CUSTOMER_TEMP_ID");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(256)
                    .IsUnicode(false)
                    .HasColumnName("CREATED_BY");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("DATE")
                    .HasColumnName("CREATED_DATE")
                    .HasDefaultValueSql("SYSDATE");

                entity.Property(e => e.Deleted)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("DELETED")
                    .HasDefaultValueSql("('N')")
                    .IsFixedLength(true);

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("MODIFIED_BY");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("DATE")
                    .HasColumnName("MODIFIED_DATE");

                entity.Property(e => e.Status)
                    .HasPrecision(1)
                    .HasColumnName("STATUS")
                    .HasDefaultValueSql("1");

                entity.Property(e => e.Title)
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("TITLE");

                entity.Property(e => e.Url)
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("URL");
            });

            modelBuilder.Entity<EpCoreCollabContractTemp>(entity =>
            {
                entity.ToTable("EP_CORE_COLLAB_CONTRACT_TEMP");

                entity.Property(e => e.Id)
                    .HasColumnType("NUMBER")
                    .HasColumnName("ID");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("CREATED_BY");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("DATE")
                    .HasColumnName("CREATED_DATE")
                    .HasDefaultValueSql("SYSDATE");

                entity.Property(e => e.Deleted)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("DELETED")
                    .HasDefaultValueSql("( 'N' )\n   ")
                    .IsFixedLength(true);

                entity.Property(e => e.FileUrl)
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("FILE_URL");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("MODIFIED_BY");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("DATE")
                    .HasColumnName("MODIFIED_DATE");

                entity.Property(e => e.Status)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("STATUS")
                    .HasDefaultValueSql("('A')")
                    .IsFixedLength(true);

                entity.Property(e => e.Title)
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("TITLE");

                entity.Property(e => e.TradingProviderId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("TRADING_PROVIDER_ID");

                entity.Property(e => e.Type)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("TYPE")
                    .HasDefaultValueSql("('I')")
                    .IsFixedLength(true);
            });

            modelBuilder.Entity<EpCoreDepartment>(entity =>
            {
                entity.HasKey(e => e.DepartmentId)
                    .HasName("SYS_C007652");

                entity.ToTable("EP_CORE_DEPARTMENT");

                entity.HasIndex(e => new { e.Deleted, e.TradingProviderId, e.ManagerId, e.ParentId, e.DepartmentId }, "IX_CORE_DEPARTMENT");

                entity.Property(e => e.DepartmentId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("DEPARTMENT_ID");

                entity.Property(e => e.Area)
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("AREA");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(256)
                    .IsUnicode(false)
                    .HasColumnName("CREATED_BY");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("DATE")
                    .HasColumnName("CREATED_DATE")
                    .HasDefaultValueSql("SYSDATE");

                entity.Property(e => e.Deleted)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("DELETED")
                    .HasDefaultValueSql("('N')")
                    .IsFixedLength(true);

                entity.Property(e => e.DepartmentAddress)
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("DEPARTMENT_ADDRESS");

                entity.Property(e => e.DepartmentLevel)
                    .HasColumnType("NUMBER")
                    .HasColumnName("DEPARTMENT_LEVEL")
                    .HasDefaultValueSql("1");

                entity.Property(e => e.DepartmentName)
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("DEPARTMENT_NAME");

                entity.Property(e => e.ManagerId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("MANAGER_ID");

                entity.Property(e => e.ManagerId2)
                    .HasColumnType("NUMBER")
                    .HasColumnName("MANAGER_ID2");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(256)
                    .IsUnicode(false)
                    .HasColumnName("MODIFIED_BY");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("DATE")
                    .HasColumnName("MODIFIED_DATE");

                entity.Property(e => e.ParentId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("PARENT_ID");

                entity.Property(e => e.TradingProviderId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("TRADING_PROVIDER_ID");
            });

            modelBuilder.Entity<EpCoreDepartmentSale>(entity =>
            {
                entity.ToTable("EP_CORE_DEPARTMENT_SALE");

                entity.HasIndex(e => new { e.Deleted, e.TradingProviderId, e.DepartmentId, e.SaleId }, "IX_CORE_DEPARTMENT_SALE");

                entity.Property(e => e.Id)
                    .HasColumnType("NUMBER")
                    .HasColumnName("ID");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(256)
                    .IsUnicode(false)
                    .HasColumnName("CREATED_BY");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("DATE")
                    .HasColumnName("CREATED_DATE")
                    .HasDefaultValueSql("SYSDATE");

                entity.Property(e => e.Deleted)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("DELETED")
                    .HasDefaultValueSql("('N')\n   ")
                    .IsFixedLength(true);

                entity.Property(e => e.DepartmentId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("DEPARTMENT_ID");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("MODIFIED_BY");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("DATE")
                    .HasColumnName("MODIFIED_DATE");

                entity.Property(e => e.SaleId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("SALE_ID");

                entity.Property(e => e.TradingProviderId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("TRADING_PROVIDER_ID");
            });

            modelBuilder.Entity<EpCoreHistoryUpdate>(entity =>
            {
                entity.ToTable("EP_CORE_HISTORY_UPDATE");

                entity.Property(e => e.Id)
                    .HasColumnType("NUMBER")
                    .HasColumnName("ID");

                entity.Property(e => e.ApproveId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("APPROVE_ID");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("CREATED_BY");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("DATE")
                    .HasColumnName("CREATED_DATE")
                    .HasDefaultValueSql("SYSDATE");

                entity.Property(e => e.Deleted)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("DELETED")
                    .HasDefaultValueSql("'N'");

                entity.Property(e => e.DeletedDate)
                    .HasColumnType("DATE")
                    .HasColumnName("DELETED_DATE");

                entity.Property(e => e.FieldName)
                    .HasMaxLength(256)
                    .IsUnicode(false)
                    .HasColumnName("FIELD_NAME");

                entity.Property(e => e.NewValue)
                    .HasColumnType("CLOB")
                    .HasColumnName("NEW_VALUE");

                entity.Property(e => e.OldValue)
                    .HasColumnType("CLOB")
                    .HasColumnName("OLD_VALUE");

                entity.Property(e => e.RealTableId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("REAL_TABLE_ID");

                entity.Property(e => e.UpdateTable)
                    .HasColumnType("NUMBER")
                    .HasColumnName("UPDATE_TABLE");
            });

            modelBuilder.Entity<EpCorePartner>(entity =>
            {
                entity.HasKey(e => e.PartnerId);

                entity.ToTable("EP_CORE_PARTNER");

                entity.Property(e => e.PartnerId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("PARTNER_ID");

                entity.Property(e => e.Address)
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("ADDRESS");

                entity.Property(e => e.Capital)
                    .HasColumnType("NUMBER(20)")
                    .HasColumnName("CAPITAL");

                entity.Property(e => e.Code)
                    .HasMaxLength(128)
                    .IsUnicode(false)
                    .HasColumnName("CODE");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(32)
                    .IsUnicode(false)
                    .HasColumnName("CREATED_BY");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("DATE")
                    .HasColumnName("CREATED_DATE");

                entity.Property(e => e.DateModified)
                    .HasColumnType("DATE")
                    .HasColumnName("DATE_MODIFIED");

                entity.Property(e => e.DecisionDate)
                    .HasColumnType("DATE")
                    .HasColumnName("DECISION_DATE");

                entity.Property(e => e.DecisionNo)
                    .HasMaxLength(128)
                    .IsUnicode(false)
                    .HasColumnName("DECISION_NO");

                entity.Property(e => e.Deleted)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("DELETED")
                    .HasDefaultValueSql("'N'");

                entity.Property(e => e.Email)
                    .HasMaxLength(128)
                    .IsUnicode(false)
                    .HasColumnName("EMAIL");

                entity.Property(e => e.LicenseDate)
                    .HasColumnType("DATE")
                    .HasColumnName("LICENSE_DATE");

                entity.Property(e => e.LicenseIssuer)
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("LICENSE_ISSUER");

                entity.Property(e => e.Mobile)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("MOBILE");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(32)
                    .IsUnicode(false)
                    .HasColumnName("MODIFIED_BY");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("DATE")
                    .HasColumnName("MODIFIED_DATE");

                entity.Property(e => e.Name)
                    .HasMaxLength(256)
                    .IsUnicode(false)
                    .HasColumnName("NAME");

                entity.Property(e => e.Nation)
                    .HasMaxLength(256)
                    .IsUnicode(false)
                    .HasColumnName("NATION");

                entity.Property(e => e.NumberModified)
                    .HasColumnType("NUMBER")
                    .HasColumnName("NUMBER_MODIFIED");

                entity.Property(e => e.Phone)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("PHONE");

                entity.Property(e => e.RepName)
                    .HasMaxLength(128)
                    .IsUnicode(false)
                    .HasColumnName("REP_NAME");

                entity.Property(e => e.RepPosition)
                    .HasMaxLength(128)
                    .IsUnicode(false)
                    .HasColumnName("REP_POSITION");

                entity.Property(e => e.ShortName)
                    .HasMaxLength(128)
                    .IsUnicode(false)
                    .HasColumnName("SHORT_NAME");

                entity.Property(e => e.Status)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("STATUS")
                    .HasDefaultValueSql("'A'");

                entity.Property(e => e.TaxCode)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("TAX_CODE");

                entity.Property(e => e.TradingAddress)
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("TRADING_ADDRESS");
            });

            modelBuilder.Entity<EpCoreProductNews>(entity =>
            {
                entity.ToTable("EP_CORE_PRODUCT_NEWS");

                entity.Property(e => e.Id)
                    .HasColumnType("NUMBER")
                    .HasColumnName("ID");

                entity.Property(e => e.Content)
                    .HasColumnType("CLOB")
                    .HasColumnName("CONTENT");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(256)
                    .IsUnicode(false)
                    .HasColumnName("CREATED_BY");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("DATE")
                    .HasColumnName("CREATED_DATE");

                entity.Property(e => e.Deleted)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("DELETED")
                    .HasDefaultValueSql("'N'\n   ")
                    .IsFixedLength(true);

                entity.Property(e => e.Feature)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("FEATURE")
                    .IsFixedLength(true);

                entity.Property(e => e.ImgUrl)
                    .HasMaxLength(256)
                    .IsUnicode(false)
                    .HasColumnName("IMG_URL");

                entity.Property(e => e.Location)
                    .HasColumnType("NUMBER")
                    .HasColumnName("LOCATION");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(256)
                    .IsUnicode(false)
                    .HasColumnName("MODIFIED_BY");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("DATE")
                    .HasColumnName("MODIFIED_DATE");

                entity.Property(e => e.Status)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("STATUS")
                    .HasDefaultValueSql("'A'")
                    .IsFixedLength(true);

                entity.Property(e => e.Title)
                    .HasMaxLength(256)
                    .IsUnicode(false)
                    .HasColumnName("TITLE");

                entity.Property(e => e.TradingProviderId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("TRADING_PROVIDER_ID");
            });

            modelBuilder.Entity<EpCoreSale>(entity =>
            {
                entity.HasKey(e => e.SaleId)
                    .HasName("SYS_C007570");

                entity.ToTable("EP_CORE_SALE");

                entity.HasIndex(e => new { e.SaleId, e.InvestorId, e.BusinessCustomerId, e.Deleted, e.Status }, "IX_CORE_SALE");

                entity.Property(e => e.SaleId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("SALE_ID");

                entity.Property(e => e.AutoDirectional)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("AUTO_DIRECTIONAL")
                    .HasDefaultValueSql("'Y' ")
                    .IsFixedLength(true);

                entity.Property(e => e.BusinessCustomerId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("BUSINESS_CUSTOMER_ID");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(256)
                    .IsUnicode(false)
                    .HasColumnName("CREATED_BY");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("DATE")
                    .HasColumnName("CREATED_DATE")
                    .HasDefaultValueSql("SYSDATE");

                entity.Property(e => e.Deleted)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("DELETED")
                    .HasDefaultValueSql("('N')")
                    .IsFixedLength(true);

                entity.Property(e => e.InvestorId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("INVESTOR_ID");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(256)
                    .IsUnicode(false)
                    .HasColumnName("MODIFIED_BY");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("DATE")
                    .HasColumnName("MODIFIED_DATE");

                entity.Property(e => e.Status)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("STATUS")
                    .HasDefaultValueSql("'A'\n   ")
                    .IsFixedLength(true);
            });

            modelBuilder.Entity<EpCoreSaleCollabContract>(entity =>
            {
                entity.ToTable("EP_CORE_SALE_COLLAB_CONTRACT");

                entity.Property(e => e.Id)
                    .HasColumnType("NUMBER")
                    .HasColumnName("ID");

                entity.Property(e => e.CollabContractTempId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("COLLAB_CONTRACT_TEMP_ID");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("CREATED_BY");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("DATE")
                    .HasColumnName("CREATED_DATE")
                    .HasDefaultValueSql("SYSDATE");

                entity.Property(e => e.Deleted)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("DELETED")
                    .HasDefaultValueSql("( 'N' )")
                    .IsFixedLength(true);

                entity.Property(e => e.FileScanUrl)
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("FILE_SCAN_URL");

                entity.Property(e => e.FileSignatureUrl)
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("FILE_SIGNATURE_URL");

                entity.Property(e => e.FileTempUrl)
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("FILE_TEMP_URL");

                entity.Property(e => e.IsSign)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("IS_SIGN")
                    .HasDefaultValueSql("'N'")
                    .IsFixedLength(true);

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("MODIFIED_BY");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("DATE")
                    .HasColumnName("MODIFIED_DATE");

                entity.Property(e => e.PageSign)
                    .HasColumnType("NUMBER")
                    .HasColumnName("PAGE_SIGN")
                    .HasDefaultValueSql("1");

                entity.Property(e => e.SaleId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("SALE_ID");

                entity.Property(e => e.TradingProviderId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("TRADING_PROVIDER_ID");
            });

            modelBuilder.Entity<EpCoreSaleRegister>(entity =>
            {
                entity.ToTable("EP_CORE_SALE_REGISTER");

                entity.Property(e => e.Id)
                    .HasColumnType("NUMBER")
                    .HasColumnName("ID");

                entity.Property(e => e.CancelDate)
                    .HasColumnType("DATE")
                    .HasColumnName("CANCEL_DATE");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("CREATED_BY");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("DATE")
                    .HasColumnName("CREATED_DATE")
                    .HasDefaultValueSql("SYSDATE");

                entity.Property(e => e.Deleted)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("DELETED")
                    .HasDefaultValueSql("('N')\n   ")
                    .IsFixedLength(true);

                entity.Property(e => e.DirectionDate)
                    .HasColumnType("DATE")
                    .HasColumnName("DIRECTION_DATE");

                entity.Property(e => e.InvestorBankAccId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("INVESTOR_BANK_ACC_ID");

                entity.Property(e => e.InvestorId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("INVESTOR_ID");

                entity.Property(e => e.IpAddress)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("IP_ADDRESS");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("MODIFIED_BY");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("DATE")
                    .HasColumnName("MODIFIED_DATE");

                entity.Property(e => e.SaleManagerId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("SALE_MANAGER_ID");

                entity.Property(e => e.Status)
                    .HasColumnType("NUMBER")
                    .HasColumnName("STATUS")
                    .HasDefaultValueSql("(1)");
            });

            modelBuilder.Entity<EpCoreSaleTemp>(entity =>
            {
                entity.ToTable("EP_CORE_SALE_TEMP");

                entity.Property(e => e.Id)
                    .HasColumnType("NUMBER")
                    .HasColumnName("ID");

                entity.Property(e => e.BusinessCustomerBankAccId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("BUSINESS_CUSTOMER_BANK_ACC_ID");

                entity.Property(e => e.BusinessCustomerId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("BUSINESS_CUSTOMER_ID");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(256)
                    .IsUnicode(false)
                    .HasColumnName("CREATED_BY");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("DATE")
                    .HasColumnName("CREATED_DATE")
                    .HasDefaultValueSql("SYSDATE");

                entity.Property(e => e.Deleted)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("DELETED")
                    .HasDefaultValueSql("('N')")
                    .IsFixedLength(true);

                entity.Property(e => e.DepartmentId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("DEPARTMENT_ID");

                entity.Property(e => e.EmployeeCode)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("EMPLOYEE_CODE");

                entity.Property(e => e.InvestorBankAccId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("INVESTOR_BANK_ACC_ID");

                entity.Property(e => e.InvestorId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("INVESTOR_ID");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(256)
                    .IsUnicode(false)
                    .HasColumnName("MODIFIED_BY");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("DATE")
                    .HasColumnName("MODIFIED_DATE");

                entity.Property(e => e.SaleParentId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("SALE_PARENT_ID");

                entity.Property(e => e.SaleRegisterId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("SALE_REGISTER_ID");

                entity.Property(e => e.SaleType)
                    .HasColumnType("NUMBER")
                    .HasColumnName("SALE_TYPE");

                entity.Property(e => e.Source)
                    .HasColumnType("NUMBER")
                    .HasColumnName("SOURCE")
                    .HasDefaultValueSql("1");

                entity.Property(e => e.Status)
                    .HasColumnType("NUMBER")
                    .HasColumnName("STATUS")
                    .HasDefaultValueSql("1");

                entity.Property(e => e.TradingProviderId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("TRADING_PROVIDER_ID");
            });

            modelBuilder.Entity<EpCoreSaleTradingProvider>(entity =>
            {
                entity.ToTable("EP_CORE_SALE_TRADING_PROVIDER");

                entity.HasIndex(e => new { e.Id, e.Deleted, e.TradingProviderId, e.Status, e.SaleId, e.InvestorBankAccId, e.EmployeeCode, e.BusinessCustomerBankAccId, e.SaleParentId, e.SaleType }, "IX_CORE_SALE_TRADING_PROVIDER");

                entity.Property(e => e.Id)
                    .HasColumnType("NUMBER")
                    .HasColumnName("ID");

                entity.Property(e => e.BusinessCustomerBankAccId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("BUSINESS_CUSTOMER_BANK_ACC_ID");

                entity.Property(e => e.ContractCode)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("CONTRACT_CODE");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(256)
                    .IsUnicode(false)
                    .HasColumnName("CREATED_BY");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("DATE")
                    .HasColumnName("CREATED_DATE")
                    .HasDefaultValueSql("SYSDATE");

                entity.Property(e => e.DeactiveDate)
                    .HasColumnType("DATE")
                    .HasColumnName("DEACTIVE_DATE");

                entity.Property(e => e.Deleted)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("DELETED")
                    .HasDefaultValueSql("('N')")
                    .IsFixedLength(true);

                entity.Property(e => e.EmployeeCode)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("EMPLOYEE_CODE");

                entity.Property(e => e.InvestorBankAccId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("INVESTOR_BANK_ACC_ID");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(256)
                    .IsUnicode(false)
                    .HasColumnName("MODIFIED_BY");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("DATE")
                    .HasColumnName("MODIFIED_DATE");

                entity.Property(e => e.SaleId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("SALE_ID");

                entity.Property(e => e.SaleParentId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("SALE_PARENT_ID");

                entity.Property(e => e.SaleType)
                    .HasColumnType("NUMBER")
                    .HasColumnName("SALE_TYPE");

                entity.Property(e => e.Status)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("STATUS")
                    .HasDefaultValueSql("('A')")
                    .IsFixedLength(true);

                entity.Property(e => e.TradingProviderId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("TRADING_PROVIDER_ID");
            });

            modelBuilder.Entity<EpCoreUserPartner>(entity =>
            {
                entity.HasKey(e => e.UserPartnerId)
                    .HasName("EP_CORE_USER_PARTNER_PK");

                entity.ToTable("EP_CORE_USER_PARTNER");

                entity.Property(e => e.UserPartnerId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("USER_PARTNER_ID");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("CREATED_BY");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("DATE")
                    .HasColumnName("CREATED_DATE")
                    .HasDefaultValueSql("SYSDATE");

                entity.Property(e => e.Deleted)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("DELETED")
                    .HasDefaultValueSql("('N')\n   ")
                    .IsFixedLength(true);

                entity.Property(e => e.PartnerId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("PARTNER_ID");

                entity.Property(e => e.UserId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("USER_ID");
            });

            modelBuilder.Entity<EpDepositProvider>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("EP_DEPOSIT_PROVIDER");

                entity.Property(e => e.BusinessCustomerId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("BUSINESS_CUSTOMER_ID");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("CREATED_BY");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("DATE")
                    .HasColumnName("CREATED_DATE");

                entity.Property(e => e.Deleted)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("DELETED");

                entity.Property(e => e.DepositProviderId)
                    .HasPrecision(10)
                    .HasColumnName("DEPOSIT_PROVIDER_ID");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("MODIFIED_BY");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("DATE")
                    .HasColumnName("MODIFIED_DATE");

                entity.Property(e => e.PartnerId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("PARTNER_ID");

                entity.Property(e => e.Status)
                    .HasColumnType("NUMBER")
                    .HasColumnName("STATUS");
            });

            modelBuilder.Entity<EpDistributionConPayment>(entity =>
            {
                entity.HasKey(e => e.PaymentId)
                    .HasName("SYS_C007609");

                entity.ToTable("EP_DISTRIBUTION_CON_PAYMENT");

                entity.Property(e => e.PaymentId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("PAYMENT_ID");

                entity.Property(e => e.ApproveBy)
                    .HasMaxLength(256)
                    .IsUnicode(false)
                    .HasColumnName("APPROVE_BY");

                entity.Property(e => e.ApproveDate)
                    .HasColumnType("DATE")
                    .HasColumnName("APPROVE_DATE");

                entity.Property(e => e.CancelBy)
                    .HasMaxLength(256)
                    .IsUnicode(false)
                    .HasColumnName("CANCEL_BY");

                entity.Property(e => e.CancelDate)
                    .HasColumnType("DATE")
                    .HasColumnName("CANCEL_DATE");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(256)
                    .IsUnicode(false)
                    .HasColumnName("CREATED_BY");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("DATE")
                    .HasColumnName("CREATED_DATE")
                    .HasDefaultValueSql("SYSDATE");

                entity.Property(e => e.Deleted)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("DELETED")
                    .HasDefaultValueSql("( 'N' )")
                    .IsFixedLength(true);

                entity.Property(e => e.Description)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("DESCRIPTION");

                entity.Property(e => e.DistributionContractId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("DISTRIBUTION_CONTRACT_ID");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(256)
                    .IsUnicode(false)
                    .HasColumnName("MODIFIED_BY");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("DATE")
                    .HasColumnName("MODIFIED_DATE");

                entity.Property(e => e.PaymentType)
                    .HasPrecision(1)
                    .HasColumnName("PAYMENT_TYPE");

                entity.Property(e => e.Status)
                    .HasPrecision(1)
                    .HasColumnName("STATUS");

                entity.Property(e => e.TotalValue)
                    .HasColumnType("NUMBER")
                    .HasColumnName("TOTAL_VALUE");

                entity.Property(e => e.TradingDate)
                    .HasColumnType("DATE")
                    .HasColumnName("TRADING_DATE");

                entity.Property(e => e.TransactionType)
                    .HasPrecision(1)
                    .HasColumnName("TRANSACTION_TYPE");
            });

            modelBuilder.Entity<EpDistributionContract>(entity =>
            {
                entity.HasKey(e => e.DistributionContractId)
                    .HasName("SYS_C007667");

                entity.ToTable("EP_DISTRIBUTION_CONTRACT");

                entity.Property(e => e.DistributionContractId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("DISTRIBUTION_CONTRACT_ID");

                entity.Property(e => e.BondPrimaryId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("BOND_PRIMARY_ID");

                entity.Property(e => e.ContractCode)
                    .HasMaxLength(256)
                    .IsUnicode(false)
                    .HasColumnName("CONTRACT_CODE");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(256)
                    .IsUnicode(false)
                    .HasColumnName("CREATED_BY");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("DATE")
                    .HasColumnName("CREATED_DATE")
                    .HasDefaultValueSql("SYSDATE");

                entity.Property(e => e.DateBuy)
                    .HasColumnType("DATE")
                    .HasColumnName("DATE_BUY");

                entity.Property(e => e.DateContract)
                    .HasColumnType("DATE")
                    .HasColumnName("DATE_CONTRACT");

                entity.Property(e => e.Deleted)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("DELETED")
                    .HasDefaultValueSql("( 'N' )")
                    .IsFixedLength(true);

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(256)
                    .IsUnicode(false)
                    .HasColumnName("MODIFIED_BY");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("DATE")
                    .HasColumnName("MODIFIED_DATE");

                entity.Property(e => e.PartnerId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("PARTNER_ID");

                entity.Property(e => e.Quantity)
                    .HasColumnType("NUMBER")
                    .HasColumnName("QUANTITY");

                entity.Property(e => e.Status)
                    .HasColumnType("NUMBER")
                    .HasColumnName("STATUS");

                entity.Property(e => e.TotalValue)
                    .HasColumnType("NUMBER")
                    .HasColumnName("TOTAL_VALUE");

                entity.Property(e => e.TradingProviderId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("TRADING_PROVIDER_ID");
            });

            modelBuilder.Entity<EpDistributionContractFile>(entity =>
            {
                entity.HasKey(e => e.FileId)
                    .HasName("SYS_C007646");

                entity.ToTable("EP_DISTRIBUTION_CONTRACT_FILE");

                entity.Property(e => e.FileId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("FILE_ID");

                entity.Property(e => e.ApproveBy)
                    .HasMaxLength(256)
                    .IsUnicode(false)
                    .HasColumnName("APPROVE_BY");

                entity.Property(e => e.ApproveDate)
                    .HasColumnType("DATE")
                    .HasColumnName("APPROVE_DATE");

                entity.Property(e => e.CancelBy)
                    .HasMaxLength(256)
                    .IsUnicode(false)
                    .HasColumnName("CANCEL_BY");

                entity.Property(e => e.CancelDate)
                    .HasColumnType("DATE")
                    .HasColumnName("CANCEL_DATE");

                entity.Property(e => e.CreateDate)
                    .HasColumnType("DATE")
                    .HasColumnName("CREATE_DATE")
                    .HasDefaultValueSql("SYSDATE");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(256)
                    .IsUnicode(false)
                    .HasColumnName("CREATED_BY");

                entity.Property(e => e.Deleted)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("DELETED")
                    .HasDefaultValueSql("( 'N' )")
                    .IsFixedLength(true);

                entity.Property(e => e.DistributionContractId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("DISTRIBUTION_CONTRACT_ID");

                entity.Property(e => e.FileUrl)
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("FILE_URL");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("MODIFIED_BY");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("DATE")
                    .HasColumnName("MODIFIED_DATE");

                entity.Property(e => e.Status)
                    .HasColumnType("NUMBER")
                    .HasColumnName("STATUS")
                    .HasDefaultValueSql("1");

                entity.Property(e => e.Title)
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("TITLE");
            });

            modelBuilder.Entity<EpEmailSmsTemplate>(entity =>
            {
                entity.ToTable("EP_EMAIL_SMS_TEMPLATE");

                entity.Property(e => e.Id)
                    .HasColumnType("NUMBER")
                    .HasColumnName("ID");

                entity.Property(e => e.Code)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("CODE");

                entity.Property(e => e.Content)
                    .HasColumnType("CLOB")
                    .HasColumnName("CONTENT");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(256)
                    .IsUnicode(false)
                    .HasColumnName("CREATED_BY");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("DATE")
                    .HasColumnName("CREATED_DATE");

                entity.Property(e => e.Deleted)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("DELETED")
                    .HasDefaultValueSql("('N')")
                    .IsFixedLength(true);

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(256)
                    .IsUnicode(false)
                    .HasColumnName("MODIFIED_BY");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("DATE")
                    .HasColumnName("MODIFIED_DATE");

                entity.Property(e => e.Status)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("STATUS")
                    .HasDefaultValueSql("('A')")
                    .IsFixedLength(true);

                entity.Property(e => e.TemplateDefine)
                    .HasColumnType("CLOB")
                    .HasColumnName("TEMPLATE_DEFINE");

                entity.Property(e => e.Title)
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("TITLE");

                entity.Property(e => e.Type)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("TYPE")
                    .HasDefaultValueSql("'E'\n   ")
                    .IsFixedLength(true);
            });

            modelBuilder.Entity<EpGuaranteeAsset>(entity =>
            {
                entity.HasKey(e => e.GuaranteeAssetId)
                    .HasName("EP_GUARANTEE_ASSET_PK");

                entity.ToTable("EP_GUARANTEE_ASSET");

                entity.Property(e => e.GuaranteeAssetId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("GUARANTEE_ASSET_ID");

                entity.Property(e => e.AssetValue)
                    .HasColumnType("NUMBER")
                    .HasColumnName("ASSET_VALUE");

                entity.Property(e => e.Code)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("CODE");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(256)
                    .IsUnicode(false)
                    .HasColumnName("CREATED_BY");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("DATE")
                    .HasColumnName("CREATED_DATE")
                    .HasDefaultValueSql("SYSDATE");

                entity.Property(e => e.Deleted)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("DELETED")
                    .HasDefaultValueSql("('N')")
                    .IsFixedLength(true);

                entity.Property(e => e.DescriptionAsset)
                    .HasColumnType("CLOB")
                    .HasColumnName("DESCRIPTION_ASSET");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("MODIFIED_BY");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("DATE")
                    .HasColumnName("MODIFIED_DATE");

                entity.Property(e => e.ProductBondId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("PRODUCT_BOND_ID");

                entity.Property(e => e.Status)
                    .HasColumnType("NUMBER")
                    .HasColumnName("STATUS");
            });

            modelBuilder.Entity<EpGuaranteeFile>(entity =>
            {
                entity.HasKey(e => e.GuaranteeFileId)
                    .HasName("PK_EP_GUARANTEE_FILE_GUARANTEE");

                entity.ToTable("EP_GUARANTEE_FILE");

                entity.Property(e => e.GuaranteeFileId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("GUARANTEE_FILE_ID");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("CREATED_BY");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("DATE")
                    .HasColumnName("CREATED_DATE")
                    .HasDefaultValueSql("SYSDATE");

                entity.Property(e => e.Deleted)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("DELETED")
                    .HasDefaultValueSql("'N'\n   ")
                    .IsFixedLength(true);

                entity.Property(e => e.FileUrl)
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("FILE_URL");

                entity.Property(e => e.GuaranteeAssetId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("GUARANTEE_ASSET_ID");

                entity.Property(e => e.Title)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("TITLE");
            });

            modelBuilder.Entity<EpInvApprove>(entity =>
            {
                entity.ToTable("EP_INV_APPROVE");

                entity.Property(e => e.Id)
                    .HasColumnType("NUMBER")
                    .HasColumnName("ID");

                entity.Property(e => e.ActionType)
                    .HasColumnType("NUMBER")
                    .HasColumnName("ACTION_TYPE");

                entity.Property(e => e.ApproveDate)
                    .HasColumnType("DATE")
                    .HasColumnName("APPROVE_DATE");

                entity.Property(e => e.ApproveNote)
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("APPROVE_NOTE");

                entity.Property(e => e.CancelDate)
                    .HasColumnType("DATE")
                    .HasColumnName("CANCEL_DATE");

                entity.Property(e => e.CancelNote)
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("CANCEL_NOTE");

                entity.Property(e => e.CloseDate)
                    .HasColumnType("DATE")
                    .HasColumnName("CLOSE_DATE");

                entity.Property(e => e.CloseNote)
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("CLOSE_NOTE");

                entity.Property(e => e.DataStatus)
                    .HasColumnType("NUMBER")
                    .HasColumnName("DATA_STATUS");

                entity.Property(e => e.DataStatusStr)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("DATA_STATUS_STR")
                    .IsFixedLength(true);

                entity.Property(e => e.DataType)
                    .HasColumnType("NUMBER")
                    .HasColumnName("DATA_TYPE");

                entity.Property(e => e.IsCheck)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("IS_CHECK")
                    .IsFixedLength(true);

                entity.Property(e => e.OpenDate)
                    .HasColumnType("DATE")
                    .HasColumnName("OPEN_DATE");

                entity.Property(e => e.OpenNote)
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("OPEN_NOTE");

                entity.Property(e => e.PartnerId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("PARTNER_ID");

                entity.Property(e => e.ReferId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("REFER_ID");

                entity.Property(e => e.ReferIdTemp)
                    .HasColumnType("NUMBER")
                    .HasColumnName("REFER_ID_TEMP");

                entity.Property(e => e.RequestDate)
                    .HasColumnType("DATE")
                    .HasColumnName("REQUEST_DATE");

                entity.Property(e => e.RequestNote)
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("REQUEST_NOTE");

                entity.Property(e => e.Status)
                    .HasColumnType("NUMBER")
                    .HasColumnName("STATUS");

                entity.Property(e => e.Summary)
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("SUMMARY");

                entity.Property(e => e.TradingProviderId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("TRADING_PROVIDER_ID");

                entity.Property(e => e.UserApproveId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("USER_APPROVE_ID");

                entity.Property(e => e.UserCheckId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("USER_CHECK_ID");

                entity.Property(e => e.UserRequestId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("USER_REQUEST_ID");
            });

            modelBuilder.Entity<EpInvBlockadeLiberation>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("EP_INV_BLOCKADE_LIBERATION");

                entity.Property(e => e.BlockadeDate)
                    .HasColumnType("DATE")
                    .HasColumnName("BLOCKADE_DATE");

                entity.Property(e => e.BlockadeDescription)
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("BLOCKADE_DESCRIPTION");

                entity.Property(e => e.BlockadeTime)
                    .HasColumnType("DATE")
                    .HasColumnName("BLOCKADE_TIME");

                entity.Property(e => e.Blockader)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("BLOCKADER");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("CREATED_BY");

                entity.Property(e => e.Id)
                    .HasColumnType("NUMBER")
                    .HasColumnName("ID");

                entity.Property(e => e.LiberationDate)
                    .HasColumnType("DATE")
                    .HasColumnName("LIBERATION_DATE");

                entity.Property(e => e.LiberationDescription)
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("LIBERATION_DESCRIPTION");

                entity.Property(e => e.LiberationTime)
                    .HasColumnType("DATE")
                    .HasColumnName("LIBERATION_TIME");

                entity.Property(e => e.Liberator)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("LIBERATOR");

                entity.Property(e => e.OrderId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("ORDER_ID");

                entity.Property(e => e.Status)
                    .HasColumnType("NUMBER")
                    .HasColumnName("STATUS");

                entity.Property(e => e.TradingProviderId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("TRADING_PROVIDER_ID");

                entity.Property(e => e.Type)
                    .HasColumnType("NUMBER")
                    .HasColumnName("TYPE");
            });

            modelBuilder.Entity<EpInvCalendar>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("EP_INV_CALENDAR");

                entity.Property(e => e.BusDate)
                    .HasColumnType("DATE")
                    .HasColumnName("BUS_DATE");

                entity.Property(e => e.IsDayoff)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("IS_DAYOFF")
                    .HasDefaultValueSql("('N')")
                    .IsFixedLength(true);

                entity.Property(e => e.TradingProviderId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("TRADING_PROVIDER_ID");

                entity.Property(e => e.WorkingDate)
                    .HasColumnType("DATE")
                    .HasColumnName("WORKING_DATE");

                entity.Property(e => e.WorkingYear)
                    .HasColumnType("NUMBER")
                    .HasColumnName("WORKING_YEAR");
            });

            modelBuilder.Entity<EpInvConfigContractCode>(entity =>
            {
                entity.ToTable("EP_INV_CONFIG_CONTRACT_CODE");

                entity.HasIndex(e => new { e.TradingProviderId, e.Deleted }, "IX_INV_CONFIG_CONTRACT_CODE");

                entity.Property(e => e.Id)
                    .HasPrecision(10)
                    .ValueGeneratedNever()
                    .HasColumnName("ID");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(50)
                    .HasColumnName("CREATED_BY");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("DATE")
                    .HasColumnName("CREATED_DATE")
                    .HasDefaultValueSql("(SYSDATE)");

                entity.Property(e => e.Deleted)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("DELETED")
                    .HasDefaultValueSql("'N' ");

                entity.Property(e => e.Description)
                    .HasMaxLength(512)
                    .HasColumnName("DESCRIPTION");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(50)
                    .HasColumnName("MODIFIED_BY");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("DATE")
                    .HasColumnName("MODIFIED_DATE");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(128)
                    .HasColumnName("NAME");

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("STATUS");

                entity.Property(e => e.TradingProviderId)
                    .HasPrecision(10)
                    .HasColumnName("TRADING_PROVIDER_ID");
            });

            modelBuilder.Entity<EpInvConfigContractCodeDetail>(entity =>
            {
                entity.ToTable("EP_INV_CONFIG_CONTRACT_CODE_DETAIL");

                entity.HasIndex(e => new { e.ConfigContractCodeId, e.Key }, "IX_INV_CONFIG_CONTRACT_CODE_DETAIL");

                entity.Property(e => e.Id)
                    .HasPrecision(10)
                    .ValueGeneratedNever()
                    .HasColumnName("ID");

                entity.Property(e => e.ConfigContractCodeId)
                    .HasPrecision(10)
                    .HasColumnName("CONFIG_CONTRACT_CODE_ID");

                entity.Property(e => e.Key)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("KEY");

                entity.Property(e => e.SortOrder)
                    .HasPrecision(10)
                    .HasColumnName("SORT_ORDER");

                entity.Property(e => e.Value)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("VALUE");
            });

            modelBuilder.Entity<EpInvContractTemplate>(entity =>
            {
                entity.ToTable("EP_INV_CONTRACT_TEMPLATE");

                entity.Property(e => e.Id)
                    .HasColumnType("NUMBER")
                    .HasColumnName("ID");

                entity.Property(e => e.Classify)
                    .HasColumnType("NUMBER")
                    .HasColumnName("CLASSIFY");

                entity.Property(e => e.Code)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("CODE");

                entity.Property(e => e.ContractTempUrl)
                    .HasMaxLength(1024)
                    .IsUnicode(false)
                    .HasColumnName("CONTRACT_TEMP_URL");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(256)
                    .IsUnicode(false)
                    .HasColumnName("CREATED_BY");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("DATE")
                    .HasColumnName("CREATED_DATE");

                entity.Property(e => e.Deleted)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("DELETED")
                    .HasDefaultValueSql("('N')")
                    .IsFixedLength(true);

                entity.Property(e => e.DisplayType)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("DISPLAY_TYPE")
                    .HasDefaultValueSql("('B')")
                    .IsFixedLength(true);

                entity.Property(e => e.DistributionId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("DISTRIBUTION_ID");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(256)
                    .IsUnicode(false)
                    .HasColumnName("MODIFIED_BY");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("DATE")
                    .HasColumnName("MODIFIED_DATE");

                entity.Property(e => e.Name)
                    .HasMaxLength(256)
                    .IsUnicode(false)
                    .HasColumnName("NAME");

                entity.Property(e => e.Status)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("STATUS")
                    .HasDefaultValueSql("('A')")
                    .IsFixedLength(true);

                entity.Property(e => e.TradingProviderId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("TRADING_PROVIDER_ID");

                entity.Property(e => e.Type)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("TYPE")
                    .HasDefaultValueSql("('I')\n   ")
                    .IsFixedLength(true);
            });

            modelBuilder.Entity<EpInvContractTemplate1>(entity =>
            {
                entity.ToTable("EP_INV_CONTRACT_TEMPLATE_1");

                entity.Property(e => e.ConfigContractId)
                    .HasPrecision(10)
                    .HasColumnName("CONFIG_CONTRACT_ID");

                entity.Property(e => e.ContractSource)
                    .HasPrecision(10)
                    .HasColumnName("CONTRACT_SOURCE")
                    .HasDefaultValueSql("1 ");

                entity.Property(e => e.ContractTemplateTempId)
                    .HasPrecision(10)
                    .HasColumnName("CONTRACT_TEMPLATE_TEMP_ID");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(50)
                    .HasColumnName("CREATED_BY");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("DATE")
                    .HasColumnName("CREATED_DATE")
                    .HasDefaultValueSql("(SYSDATE)");

                entity.Property(e => e.Deleted)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("DELETED")
                    .HasDefaultValueSql("'N' ");

                entity.Property(e => e.DisplayType)
                    .HasMaxLength(1)
                    .HasColumnName("DISPLAY_TYPE")
                    .HasDefaultValueSql("'B'");

                entity.Property(e => e.Id)
                    .HasPrecision(10)
                    .HasColumnName("ID");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(50)
                    .HasColumnName("MODIFIED_BY");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("DATE")
                    .HasColumnName("MODIFIED_DATE");

                entity.Property(e => e.PolicyId)
                    .HasPrecision(10)
                    .HasColumnName("POLICY_ID");

                entity.Property(e => e.StartDate)
                    .HasColumnType("DATE")
                    .HasColumnName("START_DATE");

                entity.Property(e => e.Status)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("STATUS")
                    .HasDefaultValueSql("'A'");

                entity.Property(e => e.TradingProviderId)
                    .HasPrecision(10)
                    .HasColumnName("TRADING_PROVIDER_ID");
            });

            modelBuilder.Entity<EpInvContractTemplateTemp>(entity =>
            {
                entity.ToTable("EP_INV_CONTRACT_TEMPLATE_TEMP");

                entity.HasIndex(e => new { e.TradingProviderId, e.Deleted }, "IX_INV_CONTRACT_TEMPLATE_TEMP");

                entity.Property(e => e.Id)
                    .HasPrecision(10)
                    .ValueGeneratedNever()
                    .HasColumnName("ID");

                entity.Property(e => e.ContractSource)
                    .HasPrecision(10)
                    .HasColumnName("CONTRACT_SOURCE")
                    .HasDefaultValueSql("1  ");

                entity.Property(e => e.ContractType)
                    .HasPrecision(10)
                    .HasColumnName("CONTRACT_TYPE")
                    .HasDefaultValueSql("1  ");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(50)
                    .HasColumnName("CREATED_BY");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("DATE")
                    .HasColumnName("CREATED_DATE")
                    .HasDefaultValueSql("(SYSDATE) ");

                entity.Property(e => e.Deleted)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("DELETED")
                    .HasDefaultValueSql("'N' ");

                entity.Property(e => e.FileBusinessCustomer)
                    .IsRequired()
                    .HasMaxLength(1024)
                    .IsUnicode(false)
                    .HasColumnName("FILE_BUSINESS_CUSTOMER");

                entity.Property(e => e.FileInvestor)
                    .IsRequired()
                    .HasMaxLength(1024)
                    .IsUnicode(false)
                    .HasColumnName("FILE_INVESTOR");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(50)
                    .HasColumnName("MODIFIED_BY");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("DATE")
                    .HasColumnName("MODIFIED_DATE");

                entity.Property(e => e.Name)
                    .HasMaxLength(256)
                    .HasColumnName("NAME");

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("STATUS")
                    .HasDefaultValueSql("'A' ");

                entity.Property(e => e.TradingProviderId)
                    .HasPrecision(10)
                    .HasColumnName("TRADING_PROVIDER_ID");
            });

            modelBuilder.Entity<EpInvDisTradingBankAcc>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("EP_INV_DIS_TRADING_BANK_ACC");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("CREATED_BY");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("DATE")
                    .HasColumnName("CREATED_DATE")
                    .HasDefaultValueSql("SYSDATE");

                entity.Property(e => e.Deleted)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("DELETED")
                    .HasDefaultValueSql("'N' ");

                entity.Property(e => e.DistributionId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("DISTRIBUTION_ID");

                entity.Property(e => e.Id)
                    .HasColumnType("NUMBER")
                    .HasColumnName("ID");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("MODIFIED_BY");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("DATE")
                    .HasColumnName("MODIFIED_DATE");

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("STATUS")
                    .HasDefaultValueSql("'N' ");

                entity.Property(e => e.TradingBankAccId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("TRADING_BANK_ACC_ID");
            });

            modelBuilder.Entity<EpInvDistriPolicyFile>(entity =>
            {
                entity.ToTable("EP_INV_DISTRI_POLICY_FILE");

                entity.Property(e => e.Id)
                    .HasColumnType("NUMBER")
                    .HasColumnName("ID");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(256)
                    .IsUnicode(false)
                    .HasColumnName("CREATED_BY");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("DATE")
                    .HasColumnName("CREATED_DATE")
                    .HasDefaultValueSql("SYSDATE");

                entity.Property(e => e.Deleted)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("DELETED")
                    .HasDefaultValueSql("'N'\n   ")
                    .IsFixedLength(true);

                entity.Property(e => e.DistributionId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("DISTRIBUTION_ID");

                entity.Property(e => e.EffectiveDate)
                    .HasColumnType("DATE")
                    .HasColumnName("EFFECTIVE_DATE");

                entity.Property(e => e.ExpirationDate)
                    .HasColumnType("DATE")
                    .HasColumnName("EXPIRATION_DATE");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("MODIFIED_BY");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("DATE")
                    .HasColumnName("MODIFIED_DATE");

                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("NAME");

                entity.Property(e => e.Status)
                    .HasColumnType("NUMBER")
                    .HasColumnName("STATUS");

                entity.Property(e => e.TradingProviderId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("TRADING_PROVIDER_ID");

                entity.Property(e => e.Url)
                    .HasMaxLength(256)
                    .IsUnicode(false)
                    .HasColumnName("URL");
            });

            modelBuilder.Entity<EpInvDistribution>(entity =>
            {
                entity.ToTable("EP_INV_DISTRIBUTION");

                entity.Property(e => e.Id)
                    .HasColumnType("NUMBER")
                    .HasColumnName("ID");

                entity.Property(e => e.BusinessCustomerBankAccId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("BUSINESS_CUSTOMER_BANK_ACC_ID");

                entity.Property(e => e.CloseCellDate)
                    .HasColumnType("DATE")
                    .HasColumnName("CLOSE_CELL_DATE");

                entity.Property(e => e.ContentType)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("CONTENT_TYPE");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("CREATED_BY");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("DATE")
                    .HasColumnName("CREATED_DATE")
                    .HasDefaultValueSql("SYSDATE");

                entity.Property(e => e.Deleted)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("DELETED")
                    .HasDefaultValueSql("'N'\n   ")
                    .IsFixedLength(true);

                entity.Property(e => e.IsCheck)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("IS_CHECK")
                    .HasDefaultValueSql("'N'")
                    .IsFixedLength(true);

                entity.Property(e => e.IsClose)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("IS_CLOSE")
                    .HasDefaultValueSql("'N'")
                    .IsFixedLength(true);

                entity.Property(e => e.IsShowApp)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("IS_SHOW_APP")
                    .HasDefaultValueSql("'Y'")
                    .IsFixedLength(true);

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("MODIFIED_BY");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("DATE")
                    .HasColumnName("MODIFIED_DATE");

                entity.Property(e => e.OpenCellDate)
                    .HasColumnType("DATE")
                    .HasColumnName("OPEN_CELL_DATE");

                entity.Property(e => e.OverviewContent)
                    .HasColumnType("CLOB")
                    .HasColumnName("OVERVIEW_CONTENT");

                entity.Property(e => e.OverviewImageUrl)
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("OVERVIEW_IMAGE_URL");

                entity.Property(e => e.ProjectId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("PROJECT_ID");

                entity.Property(e => e.Status)
                    .HasColumnType("NUMBER")
                    .HasColumnName("STATUS");

                entity.Property(e => e.TradingProviderId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("TRADING_PROVIDER_ID");
            });

            modelBuilder.Entity<EpInvDistributionFile>(entity =>
            {
                entity.ToTable("EP_INV_DISTRIBUTION_FILE");

                entity.Property(e => e.Id)
                    .HasColumnType("NUMBER")
                    .HasColumnName("ID");

                entity.Property(e => e.CreateDate)
                    .HasColumnType("DATE")
                    .HasColumnName("CREATE_DATE")
                    .HasDefaultValueSql("SYSDATE");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(256)
                    .IsUnicode(false)
                    .HasColumnName("CREATED_BY");

                entity.Property(e => e.Deleted)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("DELETED")
                    .HasDefaultValueSql("( 'N' )\n   ")
                    .IsFixedLength(true);

                entity.Property(e => e.DistributionId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("DISTRIBUTION_ID");

                entity.Property(e => e.FileUrl)
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("FILE_URL");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("MODIFIED_BY");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("DATE")
                    .HasColumnName("MODIFIED_DATE");

                entity.Property(e => e.Title)
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("TITLE");
            });

            modelBuilder.Entity<EpInvDistributionNews>(entity =>
            {
                entity.ToTable("EP_INV_DISTRIBUTION_NEWS");

                entity.Property(e => e.Id)
                    .HasColumnType("NUMBER")
                    .HasColumnName("ID");

                entity.Property(e => e.Content)
                    .HasColumnType("CLOB")
                    .HasColumnName("CONTENT");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(256)
                    .IsUnicode(false)
                    .HasColumnName("CREATED_BY");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("DATE")
                    .HasColumnName("CREATED_DATE");

                entity.Property(e => e.Deleted)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("DELETED")
                    .HasDefaultValueSql("'N'")
                    .IsFixedLength(true);

                entity.Property(e => e.DistributionId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("DISTRIBUTION_ID");

                entity.Property(e => e.ImgUrl)
                    .HasMaxLength(256)
                    .IsUnicode(false)
                    .HasColumnName("IMG_URL");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(256)
                    .IsUnicode(false)
                    .HasColumnName("MODIFIED_BY");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("DATE")
                    .HasColumnName("MODIFIED_DATE");

                entity.Property(e => e.Status)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("STATUS")
                    .HasDefaultValueSql("'A'")
                    .IsFixedLength(true);

                entity.Property(e => e.Title)
                    .HasMaxLength(256)
                    .IsUnicode(false)
                    .HasColumnName("TITLE");

                entity.Property(e => e.TradingProviderId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("TRADING_PROVIDER_ID");
            });

            modelBuilder.Entity<EpInvDistributionVideo>(entity =>
            {
                entity.ToTable("EP_INV_DISTRIBUTION_VIDEO");

                entity.Property(e => e.Id)
                    .HasColumnType("NUMBER")
                    .HasColumnName("ID");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("CREATED_BY");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("DATE")
                    .HasColumnName("CREATED_DATE");

                entity.Property(e => e.Deleted)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("DELETED")
                    .HasDefaultValueSql("'N'")
                    .IsFixedLength(true);

                entity.Property(e => e.DistributionId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("DISTRIBUTION_ID");

                entity.Property(e => e.Feature)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("FEATURE")
                    .HasDefaultValueSql("'N'\n   ")
                    .IsFixedLength(true);

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("MODIFIED_BY");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("DATE")
                    .HasColumnName("MODIFIED_DATE");

                entity.Property(e => e.Status)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("STATUS")
                    .HasDefaultValueSql("'A'")
                    .IsFixedLength(true);

                entity.Property(e => e.Title)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("TITLE");

                entity.Property(e => e.TradingProviderId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("TRADING_PROVIDER_ID");

                entity.Property(e => e.UrlVideo)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("URL_VIDEO");
            });

            modelBuilder.Entity<EpInvGeneralContractor>(entity =>
            {
                entity.ToTable("EP_INV_GENERAL_CONTRACTOR");

                entity.Property(e => e.Id)
                    .HasColumnType("NUMBER")
                    .HasColumnName("ID");

                entity.Property(e => e.BusinessCustomerId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("BUSINESS_CUSTOMER_ID");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("CREATED_BY");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("DATE")
                    .HasColumnName("CREATED_DATE");

                entity.Property(e => e.Deleted)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("DELETED")
                    .HasDefaultValueSql("('N')");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("MODIFIED_BY");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("DATE")
                    .HasColumnName("MODIFIED_DATE");

                entity.Property(e => e.PartnerId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("PARTNER_ID");

                entity.Property(e => e.Status)
                    .HasColumnType("NUMBER")
                    .HasColumnName("STATUS")
                    .HasDefaultValueSql("1");
            });

            modelBuilder.Entity<EpInvHistoryUpdate>(entity =>
            {
                entity.ToTable("EP_INV_HISTORY_UPDATE");

                entity.Property(e => e.Id)
                    .HasColumnType("NUMBER")
                    .HasColumnName("ID");

                entity.Property(e => e.Action)
                    .HasColumnType("NUMBER")
                    .HasColumnName("ACTION");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("CREATED_BY");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("DATE")
                    .HasColumnName("CREATED_DATE");

                entity.Property(e => e.FieldName)
                    .HasMaxLength(256)
                    .IsUnicode(false)
                    .HasColumnName("FIELD_NAME");

                entity.Property(e => e.NewValue)
                    .HasColumnType("CLOB")
                    .HasColumnName("NEW_VALUE");

                entity.Property(e => e.OldValue)
                    .HasColumnType("CLOB")
                    .HasColumnName("OLD_VALUE");

                entity.Property(e => e.RealTableId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("REAL_TABLE_ID");

                entity.Property(e => e.Summary)
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("SUMMARY");

                entity.Property(e => e.UpdateTable)
                    .HasColumnType("NUMBER")
                    .HasColumnName("UPDATE_TABLE");
            });

            modelBuilder.Entity<EpInvInterestPayment>(entity =>
            {
                entity.ToTable("EP_INV_INTEREST_PAYMENT");

                entity.HasIndex(e => new { e.OrderId, e.PayDate, e.TradingProviderId, e.Status }, "IX_INV_INTEREST_PAYMENT");

                entity.Property(e => e.Id)
                    .HasColumnType("NUMBER")
                    .HasColumnName("ID");

                entity.Property(e => e.AmountMoney)
                    .HasColumnType("NUMBER")
                    .HasColumnName("AMOUNT_MONEY");

                entity.Property(e => e.ApproveBy)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("APPROVE_BY");

                entity.Property(e => e.ApproveDate)
                    .HasColumnType("DATE")
                    .HasColumnName("APPROVE_DATE");

                entity.Property(e => e.ApproveIp)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("APPROVE_IP");

                entity.Property(e => e.CifCode)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("CIF_CODE");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("CREATED_BY");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("DATE")
                    .HasColumnName("CREATED_DATE")
                    .HasDefaultValueSql("SYSDATE");

                entity.Property(e => e.Deleted)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("DELETED")
                    .HasDefaultValueSql("'N' ")
                    .IsFixedLength(true);

                entity.Property(e => e.IsLastPeriod)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("IS_LAST_PERIOD")
                    .HasDefaultValueSql("'N' ")
                    .IsFixedLength(true);

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("MODIFIED_BY");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("DATE")
                    .HasColumnName("MODIFIED_DATE");

                entity.Property(e => e.OrderId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("ORDER_ID");

                entity.Property(e => e.PayDate)
                    .HasColumnType("DATE")
                    .HasColumnName("PAY_DATE");

                entity.Property(e => e.PeriodIndex)
                    .HasColumnType("NUMBER")
                    .HasColumnName("PERIOD_INDEX");

                entity.Property(e => e.PolicyDetailId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("POLICY_DETAIL_ID");

                entity.Property(e => e.Status)
                    .HasColumnType("NUMBER")
                    .HasColumnName("STATUS");

                entity.Property(e => e.Tax)
                    .HasColumnType("NUMBER")
                    .HasColumnName("TAX");

                entity.Property(e => e.TotalValueInvestment)
                    .HasColumnType("NUMBER")
                    .HasColumnName("TOTAL_VALUE_INVESTMENT");

                entity.Property(e => e.TradingProviderId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("TRADING_PROVIDER_ID");
            });

            modelBuilder.Entity<EpInvInterestPaymentDate>(entity =>
            {
                entity.ToTable("EP_INV_INTEREST_PAYMENT_DATE");

                entity.Property(e => e.Id)
                    .HasColumnType("NUMBER")
                    .HasColumnName("ID");

                entity.Property(e => e.OrderId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("ORDER_ID");

                entity.Property(e => e.PayDate)
                    .HasColumnType("DATE")
                    .HasColumnName("PAY_DATE");

                entity.Property(e => e.PeriodIndex)
                    .HasColumnType("NUMBER")
                    .HasColumnName("PERIOD_INDEX");
            });

            modelBuilder.Entity<EpInvOrder>(entity =>
            {
                entity.ToTable("EP_INV_ORDER");

                entity.HasIndex(e => new { e.DepartmentId, e.DepartmentIdSub, e.DistributionId, e.Id, e.InvestorBankAccId, e.InvestorIdenId, e.PolicyDetailId, e.PolicyId, e.ProjectId, e.SaleOrderId, e.SaleReferralCode, e.SaleReferralCodeSub, e.TradingProviderId, e.Deleted, e.Status }, "IX_INV_ORDER");

                entity.Property(e => e.Id)
                    .HasColumnType("NUMBER")
                    .HasColumnName("ID");

                entity.Property(e => e.ActiveDate)
                    .HasColumnType("DATE")
                    .HasColumnName("ACTIVE_DATE");

                entity.Property(e => e.ApproveBy)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("APPROVE_BY");

                entity.Property(e => e.ApproveDate)
                    .HasColumnType("DATE")
                    .HasColumnName("APPROVE_DATE");

                entity.Property(e => e.BusinessCustomerBankAccId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("BUSINESS_CUSTOMER_BANK_ACC_ID");

                entity.Property(e => e.BuyDate)
                    .HasColumnType("DATE")
                    .HasColumnName("BUY_DATE");

                entity.Property(e => e.CifCode)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("CIF_CODE");

                entity.Property(e => e.ContractAddressId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("CONTRACT_ADDRESS_ID");

                entity.Property(e => e.ContractCode)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("CONTRACT_CODE");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("CREATED_BY");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("DATE")
                    .HasColumnName("CREATED_DATE")
                    .HasDefaultValueSql("SYSDATE");

                entity.Property(e => e.Deleted)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("DELETED")
                    .HasDefaultValueSql("( 'N' )")
                    .IsFixedLength(true);

                entity.Property(e => e.DeliveryCode)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("DELIVERY_CODE");

                entity.Property(e => e.DeliveryDate)
                    .HasColumnType("DATE")
                    .HasColumnName("DELIVERY_DATE");

                entity.Property(e => e.DeliveryDateModifiedBy)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("DELIVERY_DATE_MODIFIED_BY");

                entity.Property(e => e.DeliveryStatus)
                    .HasColumnType("NUMBER")
                    .HasColumnName("DELIVERY_STATUS");

                entity.Property(e => e.DepartmentId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("DEPARTMENT_ID");

                entity.Property(e => e.DepartmentIdSub)
                    .HasColumnType("NUMBER")
                    .HasColumnName("DEPARTMENT_ID_SUB");

                entity.Property(e => e.DistributionId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("DISTRIBUTION_ID");

                entity.Property(e => e.FinishedDate)
                    .HasColumnType("DATE")
                    .HasColumnName("FINISHED_DATE");

                entity.Property(e => e.FinishedDateModifiedBy)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("FINISHED_DATE_MODIFIED_BY");

                entity.Property(e => e.InitTotalValue)
                    .HasColumnType("NUMBER")
                    .HasColumnName("INIT_TOTAL_VALUE");

                entity.Property(e => e.InvestDate)
                    .HasColumnType("DATE")
                    .HasColumnName("INVEST_DATE");

                entity.Property(e => e.InvestorBankAccId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("INVESTOR_BANK_ACC_ID");

                entity.Property(e => e.InvestorIdenId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("INVESTOR_IDEN_ID");

                entity.Property(e => e.IpAddressCreated)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("IP_ADDRESS_CREATED");

                entity.Property(e => e.IsInterest)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("IS_INTEREST")
                    .IsFixedLength(true);

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("MODIFIED_BY");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("DATE")
                    .HasColumnName("MODIFIED_DATE");

                entity.Property(e => e.PaymentFullDate)
                    .HasColumnType("DATE")
                    .HasColumnName("PAYMENT_FULL_DATE");

                entity.Property(e => e.PendingDate)
                    .HasColumnType("DATE")
                    .HasColumnName("PENDING_DATE");

                entity.Property(e => e.PendingDateModifiedBy)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("PENDING_DATE_MODIFIED_BY");

                entity.Property(e => e.PolicyDetailId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("POLICY_DETAIL_ID");

                entity.Property(e => e.PolicyId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("POLICY_ID");

                entity.Property(e => e.ProjectId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("PROJECT_ID");

                entity.Property(e => e.ReceivedDate)
                    .HasColumnType("DATE")
                    .HasColumnName("RECEIVED_DATE");

                entity.Property(e => e.ReceivedDateModifiedBy)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("RECEIVED_DATE_MODIFIED_BY");

                entity.Property(e => e.RenewalsPolicyDetailId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("RENEWALS_POLICY_DETAIL_ID");

                entity.Property(e => e.SaleOrderId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("SALE_ORDER_ID");

                entity.Property(e => e.SaleReferralCode)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("SALE_REFERRAL_CODE");

                entity.Property(e => e.SaleReferralCodeSub)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("SALE_REFERRAL_CODE_SUB");

                entity.Property(e => e.SettlementDate)
                    .HasColumnType("DATE")
                    .HasColumnName("SETTLEMENT_DATE");

                entity.Property(e => e.SettlementMethod)
                    .HasColumnType("NUMBER")
                    .HasColumnName("SETTLEMENT_METHOD")
                    .HasDefaultValueSql("1");

                entity.Property(e => e.Source)
                    .HasColumnType("NUMBER")
                    .HasColumnName("SOURCE");

                entity.Property(e => e.Status)
                    .HasColumnType("NUMBER")
                    .HasColumnName("STATUS");

                entity.Property(e => e.TotalValue)
                    .HasColumnType("NUMBER")
                    .HasColumnName("TOTAL_VALUE");

                entity.Property(e => e.TradingProviderId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("TRADING_PROVIDER_ID");
            });

            modelBuilder.Entity<EpInvOrderContractFile>(entity =>
            {
                entity.ToTable("EP_INV_ORDER_CONTRACT_FILE");

                entity.Property(e => e.Id)
                    .HasColumnType("NUMBER")
                    .HasColumnName("ID");

                entity.Property(e => e.ContractTempId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("CONTRACT_TEMP_ID");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("CREATED_BY");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("DATE")
                    .HasColumnName("CREATED_DATE")
                    .HasDefaultValueSql("SYSDATE");

                entity.Property(e => e.Deleted)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("DELETED")
                    .HasDefaultValueSql("'N'")
                    .IsFixedLength(true);

                entity.Property(e => e.FileScanUrl)
                    .HasMaxLength(256)
                    .IsUnicode(false)
                    .HasColumnName("FILE_SCAN_URL");

                entity.Property(e => e.FileSignatureUrl)
                    .HasMaxLength(256)
                    .IsUnicode(false)
                    .HasColumnName("FILE_SIGNATURE_URL");

                entity.Property(e => e.FileTempPdfUrl)
                    .HasMaxLength(256)
                    .IsUnicode(false)
                    .HasColumnName("FILE_TEMP_PDF_URL");

                entity.Property(e => e.FileTempUrl)
                    .HasMaxLength(256)
                    .IsUnicode(false)
                    .HasColumnName("FILE_TEMP_URL");

                entity.Property(e => e.IsSign)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("IS_SIGN")
                    .HasDefaultValueSql("'N'")
                    .IsFixedLength(true);

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("MODIFIED_BY");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("DATE")
                    .HasColumnName("MODIFIED_DATE");

                entity.Property(e => e.OrderId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("ORDER_ID");

                entity.Property(e => e.PageSign)
                    .HasColumnType("NUMBER")
                    .HasColumnName("PAGE_SIGN")
                    .HasDefaultValueSql("1");

                entity.Property(e => e.TradingProviderId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("TRADING_PROVIDER_ID");
            });

            modelBuilder.Entity<EpInvOrderContractFile1>(entity =>
            {
                entity.ToTable("EP_INV_ORDER_CONTRACT_FILE_1");

                entity.Property(e => e.ContractTempId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("CONTRACT_TEMP_ID");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("CREATED_BY");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("DATE")
                    .HasColumnName("CREATED_DATE")
                    .HasDefaultValueSql("SYSDATE");

                entity.Property(e => e.Deleted)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("DELETED")
                    .HasDefaultValueSql("'N'")
                    .IsFixedLength(true);

                entity.Property(e => e.FileScanUrl)
                    .HasMaxLength(256)
                    .IsUnicode(false)
                    .HasColumnName("FILE_SCAN_URL");

                entity.Property(e => e.FileSignatureUrl)
                    .HasMaxLength(256)
                    .IsUnicode(false)
                    .HasColumnName("FILE_SIGNATURE_URL");

                entity.Property(e => e.FileTempPdfUrl)
                    .HasMaxLength(256)
                    .IsUnicode(false)
                    .HasColumnName("FILE_TEMP_PDF_URL");

                entity.Property(e => e.FileTempUrl)
                    .HasMaxLength(256)
                    .IsUnicode(false)
                    .HasColumnName("FILE_TEMP_URL");

                entity.Property(e => e.Id)
                    .HasColumnType("NUMBER")
                    .HasColumnName("ID");

                entity.Property(e => e.IsSign)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("IS_SIGN")
                    .HasDefaultValueSql("'N'")
                    .IsFixedLength(true);

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("MODIFIED_BY");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("DATE")
                    .HasColumnName("MODIFIED_DATE");

                entity.Property(e => e.OrderId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("ORDER_ID");

                entity.Property(e => e.PageSign)
                    .HasColumnType("NUMBER")
                    .HasColumnName("PAGE_SIGN")
                    .HasDefaultValueSql("1");

                entity.Property(e => e.TradingProviderId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("TRADING_PROVIDER_ID");
            });

            modelBuilder.Entity<EpInvOrderPayment>(entity =>
            {
                entity.ToTable("EP_INV_ORDER_PAYMENT");

                entity.Property(e => e.Id)
                    .HasColumnType("NUMBER")
                    .HasColumnName("ID");

                entity.Property(e => e.ApproveBy)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("APPROVE_BY");

                entity.Property(e => e.ApproveDate)
                    .HasColumnType("DATE")
                    .HasColumnName("APPROVE_DATE");

                entity.Property(e => e.CancelBy)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("CANCEL_BY");

                entity.Property(e => e.CancelDate)
                    .HasColumnType("DATE")
                    .HasColumnName("CANCEL_DATE");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("CREATED_BY");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("DATE")
                    .HasColumnName("CREATED_DATE")
                    .HasDefaultValueSql("SYSDATE");

                entity.Property(e => e.Deleted)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("DELETED")
                    .HasDefaultValueSql("( 'N' )\n   ")
                    .IsFixedLength(true);

                entity.Property(e => e.Description)
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("DESCRIPTION");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("MODIFIED_BY");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("DATE")
                    .HasColumnName("MODIFIED_DATE");

                entity.Property(e => e.OrderId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("ORDER_ID");

                entity.Property(e => e.PaymentAmnount)
                    .HasColumnType("NUMBER")
                    .HasColumnName("PAYMENT_AMNOUNT");

                entity.Property(e => e.PaymentNo)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("PAYMENT_NO");

                entity.Property(e => e.PaymentType)
                    .HasColumnType("NUMBER")
                    .HasColumnName("PAYMENT_TYPE");

                entity.Property(e => e.Status)
                    .HasColumnType("NUMBER")
                    .HasColumnName("STATUS");

                entity.Property(e => e.TradingBankAccId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("TRADING_BANK_ACC_ID");

                entity.Property(e => e.TradingProviderId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("TRADING_PROVIDER_ID");

                entity.Property(e => e.TranClassify)
                    .HasColumnType("NUMBER")
                    .HasColumnName("TRAN_CLASSIFY");

                entity.Property(e => e.TranDate)
                    .HasColumnType("DATE")
                    .HasColumnName("TRAN_DATE");

                entity.Property(e => e.TranType)
                    .HasColumnType("NUMBER")
                    .HasColumnName("TRAN_TYPE");
            });

            modelBuilder.Entity<EpInvOwner>(entity =>
            {
                entity.ToTable("EP_INV_OWNER");

                entity.Property(e => e.Id)
                    .HasColumnType("NUMBER")
                    .HasColumnName("ID");

                entity.Property(e => e.BusinessCustomerId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("BUSINESS_CUSTOMER_ID");

                entity.Property(e => e.BusinessProfit)
                    .HasColumnType("NUMBER")
                    .HasColumnName("BUSINESS_PROFIT");

                entity.Property(e => e.BusinessTurnover)
                    .HasColumnType("NUMBER")
                    .HasColumnName("BUSINESS_TURNOVER");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("CREATED_BY");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("DATE")
                    .HasColumnName("CREATED_DATE");

                entity.Property(e => e.Deleted)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("DELETED")
                    .HasDefaultValueSql("'N'");

                entity.Property(e => e.Fanpage)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("FANPAGE");

                entity.Property(e => e.Hotline)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("HOTLINE");

                entity.Property(e => e.Image)
                    .HasMaxLength(256)
                    .IsUnicode(false)
                    .HasColumnName("IMAGE");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("MODIFIED_BY");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("DATE")
                    .HasColumnName("MODIFIED_DATE");

                entity.Property(e => e.PartnerId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("PARTNER_ID");

                entity.Property(e => e.Roa)
                    .HasColumnType("NUMBER")
                    .HasColumnName("ROA");

                entity.Property(e => e.Roe)
                    .HasColumnType("NUMBER")
                    .HasColumnName("ROE");

                entity.Property(e => e.Status)
                    .HasPrecision(1)
                    .HasColumnName("STATUS")
                    .HasDefaultValueSql("1");

                entity.Property(e => e.Website)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("WEBSITE");
            });

            modelBuilder.Entity<EpInvPolicy>(entity =>
            {
                entity.ToTable("EP_INV_POLICY");

                entity.Property(e => e.Id)
                    .HasColumnType("NUMBER")
                    .HasColumnName("ID");

                entity.Property(e => e.CalculateType)
                    .HasColumnType("NUMBER")
                    .HasColumnName("CALCULATE_TYPE")
                    .HasDefaultValueSql("2 ");

                entity.Property(e => e.Classify)
                    .HasColumnType("NUMBER")
                    .HasColumnName("CLASSIFY");

                entity.Property(e => e.Code)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("CODE");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(256)
                    .IsUnicode(false)
                    .HasColumnName("CREATED_BY");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("DATE")
                    .HasColumnName("CREATED_DATE")
                    .HasDefaultValueSql("SYSDATE");

                entity.Property(e => e.Deleted)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("DELETED")
                    .HasDefaultValueSql("('N')")
                    .IsFixedLength(true);

                entity.Property(e => e.Description)
                    .HasMaxLength(2048)
                    .IsUnicode(false)
                    .HasColumnName("DESCRIPTION");

                entity.Property(e => e.DistributionId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("DISTRIBUTION_ID");

                entity.Property(e => e.EndDate)
                    .HasColumnType("DATE")
                    .HasColumnName("END_DATE");

                entity.Property(e => e.ExitFee)
                    .HasColumnType("NUMBER")
                    .HasColumnName("EXIT_FEE");

                entity.Property(e => e.ExitFeeType)
                    .HasColumnType("NUMBER")
                    .HasColumnName("EXIT_FEE_TYPE");

                entity.Property(e => e.IncomeTax)
                    .HasColumnType("NUMBER")
                    .HasColumnName("INCOME_TAX");

                entity.Property(e => e.IsShowApp)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("IS_SHOW_APP")
                    .IsFixedLength(true);

                entity.Property(e => e.IsTransfer)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("IS_TRANSFER")
                    .HasDefaultValueSql("('N')")
                    .IsFixedLength(true);

                entity.Property(e => e.MinMoney)
                    .HasColumnType("NUMBER")
                    .HasColumnName("MIN_MONEY");

                entity.Property(e => e.MinWithdraw)
                    .HasColumnType("NUMBER")
                    .HasColumnName("MIN_WITHDRAW");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("MODIFIED_BY");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("DATE")
                    .HasColumnName("MODIFIED_DATE");

                entity.Property(e => e.Name)
                    .HasMaxLength(256)
                    .IsUnicode(false)
                    .HasColumnName("NAME");

                entity.Property(e => e.PolicyDisplayOrder)
                    .HasColumnType("NUMBER")
                    .HasColumnName("POLICY_DISPLAY_ORDER");

                entity.Property(e => e.StartDate)
                    .HasColumnType("DATE")
                    .HasColumnName("START_DATE");

                entity.Property(e => e.Status)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("STATUS")
                    .HasDefaultValueSql("('A')")
                    .IsFixedLength(true);

                entity.Property(e => e.TradingProviderId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("TRADING_PROVIDER_ID");

                entity.Property(e => e.TransferTax)
                    .HasColumnType("NUMBER")
                    .HasColumnName("TRANSFER_TAX");

                entity.Property(e => e.Type)
                    .HasColumnType("NUMBER")
                    .HasColumnName("TYPE");
            });

            modelBuilder.Entity<EpInvPolicyDetail>(entity =>
            {
                entity.ToTable("EP_INV_POLICY_DETAIL");

                entity.Property(e => e.Id)
                    .HasColumnType("NUMBER")
                    .HasColumnName("ID");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(256)
                    .IsUnicode(false)
                    .HasColumnName("CREATED_BY");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("DATE")
                    .HasColumnName("CREATED_DATE")
                    .HasDefaultValueSql("SYSDATE");

                entity.Property(e => e.Deleted)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("DELETED")
                    .HasDefaultValueSql("('N')")
                    .IsFixedLength(true);

                entity.Property(e => e.DistributionId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("DISTRIBUTION_ID");

                entity.Property(e => e.FixedPaymentDate)
                    .HasColumnType("NUMBER")
                    .HasColumnName("FIXED_PAYMENT_DATE");

                entity.Property(e => e.InterestDays)
                    .HasColumnType("NUMBER")
                    .HasColumnName("INTEREST_DAYS");

                entity.Property(e => e.InterestPeriodQuantity)
                    .HasColumnType("NUMBER")
                    .HasColumnName("INTEREST_PERIOD_QUANTITY");

                entity.Property(e => e.InterestPeriodType)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("INTEREST_PERIOD_TYPE")
                    .IsFixedLength(true);

                entity.Property(e => e.InterestType)
                    .HasColumnType("NUMBER")
                    .HasColumnName("INTEREST_TYPE");

                entity.Property(e => e.IsShowApp)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("IS_SHOW_APP")
                    .IsFixedLength(true);

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("MODIFIED_BY");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("DATE")
                    .HasColumnName("MODIFIED_DATE");

                entity.Property(e => e.Name)
                    .HasMaxLength(256)
                    .IsUnicode(false)
                    .HasColumnName("NAME");

                entity.Property(e => e.PeriodQuantity)
                    .HasColumnType("NUMBER")
                    .HasColumnName("PERIOD_QUANTITY");

                entity.Property(e => e.PeriodType)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("PERIOD_TYPE")
                    .IsFixedLength(true);

                entity.Property(e => e.PolicyId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("POLICY_ID");

                entity.Property(e => e.Profit)
                    .HasColumnType("NUMBER")
                    .HasColumnName("PROFIT");

                entity.Property(e => e.ShortName)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("SHORT_NAME");

                entity.Property(e => e.Status)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("STATUS")
                    .HasDefaultValueSql("('A')")
                    .IsFixedLength(true);

                entity.Property(e => e.Stt)
                    .HasColumnType("NUMBER")
                    .HasColumnName("STT");

                entity.Property(e => e.TradingProviderId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("TRADING_PROVIDER_ID");
            });

            modelBuilder.Entity<EpInvPolicyDetailTemp>(entity =>
            {
                entity.ToTable("EP_INV_POLICY_DETAIL_TEMP");

                entity.Property(e => e.Id)
                    .HasColumnType("NUMBER")
                    .HasColumnName("ID");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(256)
                    .IsUnicode(false)
                    .HasColumnName("CREATED_BY");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("DATE")
                    .HasColumnName("CREATED_DATE")
                    .HasDefaultValueSql("SYSDATE");

                entity.Property(e => e.Deleted)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("DELETED")
                    .HasDefaultValueSql("('N')")
                    .IsFixedLength(true);

                entity.Property(e => e.FixedPaymentDate)
                    .HasColumnType("NUMBER")
                    .HasColumnName("FIXED_PAYMENT_DATE");

                entity.Property(e => e.InterestDays)
                    .HasColumnType("NUMBER")
                    .HasColumnName("INTEREST_DAYS");

                entity.Property(e => e.InterestPeriodQuantity)
                    .HasColumnType("NUMBER")
                    .HasColumnName("INTEREST_PERIOD_QUANTITY");

                entity.Property(e => e.InterestPeriodType)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("INTEREST_PERIOD_TYPE")
                    .IsFixedLength(true);

                entity.Property(e => e.InterestType)
                    .HasColumnType("NUMBER")
                    .HasColumnName("INTEREST_TYPE");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("MODIFIED_BY");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("DATE")
                    .HasColumnName("MODIFIED_DATE");

                entity.Property(e => e.Name)
                    .HasMaxLength(256)
                    .IsUnicode(false)
                    .HasColumnName("NAME");

                entity.Property(e => e.PeriodQuantity)
                    .HasColumnType("NUMBER")
                    .HasColumnName("PERIOD_QUANTITY");

                entity.Property(e => e.PeriodType)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("PERIOD_TYPE")
                    .IsFixedLength(true);

                entity.Property(e => e.PolicyTempId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("POLICY_TEMP_ID");

                entity.Property(e => e.Profit)
                    .HasColumnType("NUMBER")
                    .HasColumnName("PROFIT");

                entity.Property(e => e.ShortName)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("SHORT_NAME");

                entity.Property(e => e.Status)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("STATUS")
                    .HasDefaultValueSql("('A')")
                    .IsFixedLength(true);

                entity.Property(e => e.Stt)
                    .HasColumnType("NUMBER")
                    .HasColumnName("STT");
            });

            modelBuilder.Entity<EpInvPolicyTemp>(entity =>
            {
                entity.ToTable("EP_INV_POLICY_TEMP");

                entity.Property(e => e.Id)
                    .HasColumnType("NUMBER")
                    .HasColumnName("ID");

                entity.Property(e => e.CalculateType)
                    .HasColumnType("NUMBER")
                    .HasColumnName("CALCULATE_TYPE")
                    .HasDefaultValueSql("2 ");

                entity.Property(e => e.Classify)
                    .HasColumnType("NUMBER")
                    .HasColumnName("CLASSIFY");

                entity.Property(e => e.Code)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("CODE");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(256)
                    .IsUnicode(false)
                    .HasColumnName("CREATED_BY");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("DATE")
                    .HasColumnName("CREATED_DATE")
                    .HasDefaultValueSql("SYSDATE");

                entity.Property(e => e.Deleted)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("DELETED")
                    .HasDefaultValueSql("('N')\n   ")
                    .IsFixedLength(true);

                entity.Property(e => e.Description)
                    .HasMaxLength(256)
                    .IsUnicode(false)
                    .HasColumnName("DESCRIPTION");

                entity.Property(e => e.ExitFee)
                    .HasColumnType("NUMBER")
                    .HasColumnName("EXIT_FEE");

                entity.Property(e => e.ExitFeeType)
                    .HasColumnType("NUMBER")
                    .HasColumnName("EXIT_FEE_TYPE");

                entity.Property(e => e.IncomeTax)
                    .HasColumnType("NUMBER")
                    .HasColumnName("INCOME_TAX");

                entity.Property(e => e.IsTransfer)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("IS_TRANSFER")
                    .HasDefaultValueSql("('N')")
                    .IsFixedLength(true);

                entity.Property(e => e.MinMoney)
                    .HasColumnType("NUMBER")
                    .HasColumnName("MIN_MONEY");

                entity.Property(e => e.MinWithdraw)
                    .HasColumnType("NUMBER")
                    .HasColumnName("MIN_WITHDRAW");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("MODIFIED_BY");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("DATE")
                    .HasColumnName("MODIFIED_DATE");

                entity.Property(e => e.Name)
                    .HasMaxLength(256)
                    .IsUnicode(false)
                    .HasColumnName("NAME");

                entity.Property(e => e.PolicyDisplayOrder)
                    .HasColumnType("NUMBER")
                    .HasColumnName("POLICY_DISPLAY_ORDER");

                entity.Property(e => e.Status)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("STATUS")
                    .HasDefaultValueSql("('A')")
                    .IsFixedLength(true);

                entity.Property(e => e.TradingProviderId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("TRADING_PROVIDER_ID");

                entity.Property(e => e.TransferTax)
                    .HasColumnType("NUMBER")
                    .HasColumnName("TRANSFER_TAX");

                entity.Property(e => e.Type)
                    .HasColumnType("NUMBER")
                    .HasColumnName("TYPE");
            });

            modelBuilder.Entity<EpInvProject>(entity =>
            {
                entity.ToTable("EP_INV_PROJECT");

                entity.Property(e => e.Id)
                    .HasColumnType("NUMBER")
                    .HasColumnName("ID");

                entity.Property(e => e.Area)
                    .HasMaxLength(128)
                    .IsUnicode(false)
                    .HasColumnName("AREA");

                entity.Property(e => e.Content)
                    .HasColumnType("CLOB")
                    .HasColumnName("CONTENT");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasColumnName("CREATED_BY");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("DATE")
                    .HasColumnName("CREATED_DATE");

                entity.Property(e => e.Deleted)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("DELETED")
                    .HasDefaultValueSql("('N')")
                    .IsFixedLength(true);

                entity.Property(e => e.EndDate)
                    .HasColumnType("DATE")
                    .HasColumnName("END_DATE");

                entity.Property(e => e.GeneralContractorId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("GENERAL_CONTRACTOR_ID");

                entity.Property(e => e.GuaranteeOrganization)
                    .HasMaxLength(256)
                    .IsUnicode(false)
                    .HasColumnName("GUARANTEE_ORGANIZATION");

                entity.Property(e => e.HasTotalInvestmentSub)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("HAS_TOTAL_INVESTMENT_SUB")
                    .HasDefaultValueSql("'N'")
                    .IsFixedLength(true);

                entity.Property(e => e.Image)
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("IMAGE");

                entity.Property(e => e.InvCode)
                    .HasMaxLength(256)
                    .IsUnicode(false)
                    .HasColumnName("INV_CODE");

                entity.Property(e => e.InvName)
                    .HasMaxLength(256)
                    .IsUnicode(false)
                    .HasColumnName("INV_NAME");

                entity.Property(e => e.IsCheck)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("IS_CHECK")
                    .IsFixedLength(true);

                entity.Property(e => e.IsPaymentGuarantee)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("IS_PAYMENT_GUARANTEE")
                    .IsFixedLength(true);

                entity.Property(e => e.Latitude)
                    .HasMaxLength(128)
                    .IsUnicode(false)
                    .HasColumnName("LATITUDE");

                entity.Property(e => e.LocationDescription)
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("LOCATION_DESCRIPTION");

                entity.Property(e => e.Longitude)
                    .HasMaxLength(128)
                    .IsUnicode(false)
                    .HasColumnName("LONGITUDE");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasColumnName("MODIFIED_BY");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("DATE")
                    .HasColumnName("MODIFIED_DATE");

                entity.Property(e => e.OwnerId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("OWNER_ID");

                entity.Property(e => e.PartnerId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("PARTNER_ID");

                entity.Property(e => e.ProjectProgress)
                    .HasMaxLength(256)
                    .IsUnicode(false)
                    .HasColumnName("PROJECT_PROGRESS");

                entity.Property(e => e.ProjectType)
                    .HasColumnType("NUMBER")
                    .HasColumnName("PROJECT_TYPE");

                entity.Property(e => e.StartDate)
                    .HasColumnType("DATE")
                    .HasColumnName("START_DATE");

                entity.Property(e => e.Status)
                    .HasPrecision(1)
                    .HasColumnName("STATUS")
                    .HasDefaultValueSql("1");

                entity.Property(e => e.TotalInvestment)
                    .HasColumnType("NUMBER")
                    .HasColumnName("TOTAL_INVESTMENT");

                entity.Property(e => e.TotalInvestmentDisplay)
                    .HasColumnType("NUMBER")
                    .HasColumnName("TOTAL_INVESTMENT_DISPLAY");

                entity.Property(e => e.TradingProviderId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("TRADING_PROVIDER_ID");
            });

            modelBuilder.Entity<EpInvProjectImage>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("EP_INV_PROJECT_IMAGE");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("DATE")
                    .HasColumnName("CREATED_DATE")
                    .HasDefaultValueSql("SYSDATE\n   ");

                entity.Property(e => e.Deleted)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("DELETED")
                    .HasDefaultValueSql("('N')")
                    .IsFixedLength(true);

                entity.Property(e => e.Id)
                    .HasColumnType("NUMBER")
                    .HasColumnName("ID");

                entity.Property(e => e.ProjectId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("PROJECT_ID");

                entity.Property(e => e.Title)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("TITLE");

                entity.Property(e => e.Url)
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("URL");
            });

            modelBuilder.Entity<EpInvProjectJuridicalFile>(entity =>
            {
                entity.ToTable("EP_INV_PROJECT_JURIDICAL_FILE");

                entity.Property(e => e.Id)
                    .HasColumnType("NUMBER")
                    .HasColumnName("ID");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(256)
                    .IsUnicode(false)
                    .HasColumnName("CREATED_BY");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("DATE")
                    .HasColumnName("CREATED_DATE")
                    .HasDefaultValueSql("SYSDATE");

                entity.Property(e => e.Deleted)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("DELETED")
                    .HasDefaultValueSql("('N')\n   ")
                    .IsFixedLength(true);

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("MODIFIED_BY");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("DATE")
                    .HasColumnName("MODIFIED_DATE");

                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("NAME");

                entity.Property(e => e.ProjectId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("PROJECT_ID");

                entity.Property(e => e.Status)
                    .HasPrecision(1)
                    .HasColumnName("STATUS")
                    .HasDefaultValueSql("1");

                entity.Property(e => e.Url)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("URL");
            });

            modelBuilder.Entity<EpInvProjectOverviewFile>(entity =>
            {
                entity.ToTable("EP_INV_PROJECT_OVERVIEW_FILE");

                entity.Property(e => e.Id)
                    .HasColumnType("NUMBER")
                    .HasColumnName("ID");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(256)
                    .IsUnicode(false)
                    .HasColumnName("CREATED_BY");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("DATE")
                    .HasColumnName("CREATED_DATE")
                    .HasDefaultValueSql("SYSDATE");

                entity.Property(e => e.Deleted)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("DELETED")
                    .HasDefaultValueSql("('N')")
                    .IsFixedLength(true);

                entity.Property(e => e.DistributionId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("DISTRIBUTION_ID");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("MODIFIED_BY");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("DATE")
                    .HasColumnName("MODIFIED_DATE");

                entity.Property(e => e.Status)
                    .HasPrecision(1)
                    .HasColumnName("STATUS")
                    .HasDefaultValueSql("1");

                entity.Property(e => e.Title)
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("TITLE");

                entity.Property(e => e.TradingProviderId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("TRADING_PROVIDER_ID");

                entity.Property(e => e.Url)
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("URL");
            });

            modelBuilder.Entity<EpInvProjectOverviewOrg>(entity =>
            {
                entity.ToTable("EP_INV_PROJECT_OVERVIEW_ORG");

                entity.Property(e => e.Id)
                    .HasColumnType("NUMBER")
                    .HasColumnName("ID");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(256)
                    .IsUnicode(false)
                    .HasColumnName("CREATED_BY");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("DATE")
                    .HasColumnName("CREATED_DATE")
                    .HasDefaultValueSql("SYSDATE");

                entity.Property(e => e.Deleted)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("DELETED")
                    .HasDefaultValueSql("('N')")
                    .IsFixedLength(true);

                entity.Property(e => e.DistributionId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("DISTRIBUTION_ID");

                entity.Property(e => e.Icon)
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("ICON");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("MODIFIED_BY");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("DATE")
                    .HasColumnName("MODIFIED_DATE");

                entity.Property(e => e.Name)
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("NAME");

                entity.Property(e => e.OrgCode)
                    .HasMaxLength(128)
                    .IsUnicode(false)
                    .HasColumnName("ORG_CODE");

                entity.Property(e => e.Status)
                    .HasPrecision(1)
                    .HasColumnName("STATUS")
                    .HasDefaultValueSql("1");

                entity.Property(e => e.TradingProviderId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("TRADING_PROVIDER_ID");

                entity.Property(e => e.Url)
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("URL");
            });

            modelBuilder.Entity<EpInvProjectTradingProvider>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("EP_INV_PROJECT_TRADING_PROVIDER");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("CREATED_BY");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("DATE")
                    .HasColumnName("CREATED_DATE");

                entity.Property(e => e.Deleted)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("DELETED")
                    .HasDefaultValueSql("'N'\n   ")
                    .IsFixedLength(true);

                entity.Property(e => e.Id)
                    .HasColumnType("NUMBER")
                    .HasColumnName("ID");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("MODIFIED_BY");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("DATE")
                    .HasColumnName("MODIFIED_DATE");

                entity.Property(e => e.PartnerId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("PARTNER_ID");

                entity.Property(e => e.ProjectId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("PROJECT_ID");

                entity.Property(e => e.TotalInvestmentSub)
                    .HasColumnType("NUMBER")
                    .HasColumnName("TOTAL_INVESTMENT_SUB");

                entity.Property(e => e.TradingProviderId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("TRADING_PROVIDER_ID");
            });

            modelBuilder.Entity<EpInvProjectType>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("EP_INV_PROJECT_TYPE");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("DATE")
                    .HasColumnName("CREATED_DATE");

                entity.Property(e => e.Deleted)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("DELETED")
                    .HasDefaultValueSql("('N')\n   ")
                    .IsFixedLength(true);

                entity.Property(e => e.Id)
                    .HasColumnType("NUMBER")
                    .HasColumnName("ID");

                entity.Property(e => e.ProjectId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("PROJECT_ID");

                entity.Property(e => e.Type)
                    .HasColumnType("NUMBER")
                    .HasColumnName("TYPE");
            });

            modelBuilder.Entity<EpInvReceiveContractTemp>(entity =>
            {
                entity.ToTable("EP_INV_RECEIVE_CONTRACT_TEMP");

                entity.Property(e => e.Id)
                    .HasColumnType("NUMBER")
                    .HasColumnName("ID");

                entity.Property(e => e.Code)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("CODE");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(256)
                    .IsUnicode(false)
                    .HasColumnName("CREATED_BY");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("DATE")
                    .HasColumnName("CREATED_DATE");

                entity.Property(e => e.Deleted)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("DELETED")
                    .HasDefaultValueSql("'N'")
                    .IsFixedLength(true);

                entity.Property(e => e.DistributionId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("DISTRIBUTION_ID");

                entity.Property(e => e.FileUrl)
                    .HasMaxLength(1024)
                    .IsUnicode(false)
                    .HasColumnName("FILE_URL");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(256)
                    .IsUnicode(false)
                    .HasColumnName("MODIFIED_BY");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("DATE")
                    .HasColumnName("MODIFIED_DATE");

                entity.Property(e => e.Name)
                    .HasMaxLength(256)
                    .IsUnicode(false)
                    .HasColumnName("NAME");

                entity.Property(e => e.Status)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("STATUS")
                    .HasDefaultValueSql("'A'")
                    .IsFixedLength(true);

                entity.Property(e => e.TradingProviderId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("TRADING_PROVIDER_ID");
            });

            modelBuilder.Entity<EpInvRenewalsRequest>(entity =>
            {
                entity.ToTable("EP_INV_RENEWALS_REQUEST");

                entity.Property(e => e.Id)
                    .HasColumnType("NUMBER")
                    .HasColumnName("ID");

                entity.Property(e => e.Deleted)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("DELETED")
                    .HasDefaultValueSql("'N' ")
                    .IsFixedLength(true);

                entity.Property(e => e.OrderId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("ORDER_ID");

                entity.Property(e => e.RenewalsPolicyDetailId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("RENEWALS_POLICY_DETAIL_ID");

                entity.Property(e => e.SettlementMethod)
                    .HasColumnType("NUMBER")
                    .HasColumnName("SETTLEMENT_METHOD");
            });

            modelBuilder.Entity<EpInvWithdrawal>(entity =>
            {
                entity.ToTable("EP_INV_WITHDRAWAL");

                entity.Property(e => e.Id)
                    .HasColumnType("NUMBER")
                    .HasColumnName("ID");

                entity.Property(e => e.ActuallyAmount)
                    .HasColumnType("NUMBER")
                    .HasColumnName("ACTUALLY_AMOUNT");

                entity.Property(e => e.AmountMoney)
                    .HasColumnType("NUMBER")
                    .HasColumnName("AMOUNT_MONEY");

                entity.Property(e => e.ApproveBy)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("APPROVE_BY");

                entity.Property(e => e.ApproveDate)
                    .HasColumnType("DATE")
                    .HasColumnName("APPROVE_DATE");

                entity.Property(e => e.ApproveIp)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("APPROVE_IP");

                entity.Property(e => e.CifCode)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("CIF_CODE");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("CREATED_BY");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("DATE")
                    .HasColumnName("CREATED_DATE")
                    .HasDefaultValueSql("SYSDATE");

                entity.Property(e => e.Deleted)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("DELETED")
                    .HasDefaultValueSql("'N' ")
                    .IsFixedLength(true);

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("MODIFIED_BY");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("DATE")
                    .HasColumnName("MODIFIED_DATE");

                entity.Property(e => e.OrderId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("ORDER_ID");

                entity.Property(e => e.PolicyDetailId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("POLICY_DETAIL_ID");

                entity.Property(e => e.RequestIp)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("REQUEST_IP");

                entity.Property(e => e.Source)
                    .HasColumnType("NUMBER")
                    .HasColumnName("SOURCE");

                entity.Property(e => e.Status)
                    .HasColumnType("NUMBER")
                    .HasColumnName("STATUS");

                entity.Property(e => e.TradingProviderId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("TRADING_PROVIDER_ID");

                entity.Property(e => e.Type)
                    .HasColumnType("NUMBER")
                    .HasColumnName("TYPE");

                entity.Property(e => e.WithdrawalDate)
                    .HasColumnType("DATE")
                    .HasColumnName("WITHDRAWAL_DATE");
            });

            modelBuilder.Entity<EpInvestor>(entity =>
            {
                entity.HasKey(e => e.InvestorId)
                    .HasName("EP_INVESTOR_PK");

                entity.ToTable("EP_INVESTOR");

                entity.HasIndex(e => new { e.Deleted, e.Phone, e.Email, e.ReferralCodeSelf, e.ReferralCode, e.InvestorGroupId, e.TaxCode, e.InvestorId }, "IX_INVESTOR");

                entity.Property(e => e.InvestorId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("INVESTOR_ID");

                entity.Property(e => e.AccountNo)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("ACCOUNT_NO");

                entity.Property(e => e.AccountStatus)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("ACCOUNT_STATUS")
                    .HasDefaultValueSql("'N'")
                    .IsFixedLength(true);

                entity.Property(e => e.AccountType)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("ACCOUNT_TYPE");

                entity.Property(e => e.Address)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("ADDRESS");

                entity.Property(e => e.AvatarImageUrl)
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("AVATAR_IMAGE_URL");

                entity.Property(e => e.BirthDate)
                    .HasColumnType("DATE")
                    .HasColumnName("BIRTH_DATE");

                entity.Property(e => e.Bori)
                    .HasMaxLength(1)
                    .HasColumnName("BORI");

                entity.Property(e => e.ContactAddress)
                    .HasMaxLength(2000)
                    .IsUnicode(false)
                    .HasColumnName("CONTACT_ADDRESS");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("CREATED_BY");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("DATE")
                    .HasColumnName("CREATED_DATE")
                    .HasDefaultValueSql("SYSDATE");

                entity.Property(e => e.Deleted)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("DELETED")
                    .HasDefaultValueSql("'N'");

                entity.Property(e => e.Description)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("DESCRIPTION");

                entity.Property(e => e.Dorf)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("DORF");

                entity.Property(e => e.EContractUrl)
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("E_CONTRACT_URL");

                entity.Property(e => e.EcSignDate)
                    .HasColumnType("DATE")
                    .HasColumnName("EC_SIGN_DATE");

                entity.Property(e => e.EkycOcrCount)
                    .HasPrecision(2)
                    .HasColumnName("EKYC_OCR_COUNT");

                entity.Property(e => e.EkycStatus)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("EKYC_STATUS")
                    .HasDefaultValueSql("'N'");

                entity.Property(e => e.Email)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("EMAIL");

                entity.Property(e => e.FaceImageUrl)
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("FACE_IMAGE_URL");

                entity.Property(e => e.Fax)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("FAX");

                entity.Property(e => e.FinalStepDate)
                    .HasColumnType("DATE")
                    .HasColumnName("FINAL_STEP_DATE");

                entity.Property(e => e.FoundationDate)
                    .HasColumnType("DATE")
                    .HasColumnName("FOUNDATION_DATE");

                entity.Property(e => e.FoundationIssuer)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("FOUNDATION_ISSUER");

                entity.Property(e => e.FoundationNo)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("FOUNDATION_NO");

                entity.Property(e => e.IdExpiredDate)
                    .HasColumnType("DATE")
                    .HasColumnName("ID_EXPIRED_DATE");

                entity.Property(e => e.IdIssuer)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("ID_ISSUER");

                entity.Property(e => e.IdPlaceOfResidence)
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("ID_PLACE_OF_RESIDENCE");

                entity.Property(e => e.InvestorGroupId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("INVESTOR_GROUP_ID");

                entity.Property(e => e.IsCheck)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("IS_CHECK")
                    .IsFixedLength(true);

                entity.Property(e => e.IsEcontractSign)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("IS_ECONTRACT_SIGN")
                    .IsFixedLength(true);

                entity.Property(e => e.IsOpnaccSigned)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("IS_OPNACC_SIGNED")
                    .HasDefaultValueSql("('N')");

                entity.Property(e => e.IsProf)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("IS_PROF")
                    .HasDefaultValueSql("'N'");

                entity.Property(e => e.Isonline)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("ISONLINE")
                    .HasDefaultValueSql("'Y'")
                    .IsFixedLength(true);

                entity.Property(e => e.LicenseDate)
                    .HasColumnType("DATE")
                    .HasColumnName("LICENSE_DATE");

                entity.Property(e => e.LicenseIssuer)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("LICENSE_ISSUER");

                entity.Property(e => e.LicenseNo)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("LICENSE_NO");

                entity.Property(e => e.LivenessVideoUrl)
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("LIVENESS_VIDEO_URL");

                entity.Property(e => e.Mobile)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("MOBILE");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("MODIFIED_BY");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("DATE")
                    .HasColumnName("MODIFIED_DATE");

                entity.Property(e => e.Name)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("NAME");

                entity.Property(e => e.NameEn)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("NAME_EN");

                entity.Property(e => e.Nationality)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("NATIONALITY");

                entity.Property(e => e.Occupation)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("OCCUPATION");

                entity.Property(e => e.Phone)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("PHONE");

                entity.Property(e => e.PinCode)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("PIN_CODE");

                entity.Property(e => e.Priority)
                    .HasColumnType("NUMBER")
                    .HasColumnName("PRIORITY");

                entity.Property(e => e.ProfDueDate)
                    .HasColumnType("DATE")
                    .HasColumnName("PROF_DUE_DATE");

                entity.Property(e => e.ProfFileUrl)
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("PROF_FILE_URL");

                entity.Property(e => e.ProfStartDate)
                    .HasColumnType("DATE")
                    .HasColumnName("PROF_START_DATE");

                entity.Property(e => e.ReferralCode)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("REFERRAL_CODE");

                entity.Property(e => e.ReferralCodeSelf)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("REFERRAL_CODE_SELF");

                entity.Property(e => e.ReferralDate)
                    .HasColumnType("DATE")
                    .HasColumnName("REFERRAL_DATE");

                entity.Property(e => e.RegisterSource)
                    .HasMaxLength(256)
                    .IsUnicode(false)
                    .HasColumnName("REGISTER_SOURCE");

                entity.Property(e => e.RegisterType)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("REGISTER_TYPE");

                entity.Property(e => e.RepIdDate)
                    .HasColumnType("DATE")
                    .HasColumnName("REP_ID_DATE");

                entity.Property(e => e.RepIdIssuer)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("REP_ID_ISSUER");

                entity.Property(e => e.RepIdNo)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("REP_ID_NO");

                entity.Property(e => e.RepName)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("REP_NAME");

                entity.Property(e => e.RepPosition)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("REP_POSITION");

                entity.Property(e => e.RepresentativeEmail)
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasColumnName("REPRESENTATIVE_EMAIL");

                entity.Property(e => e.RepresentativePhone)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("REPRESENTATIVE_PHONE");

                entity.Property(e => e.SaleId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("SALE_ID");

                entity.Property(e => e.SecurityCompany)
                    .HasColumnType("NUMBER")
                    .HasColumnName("SECURITY_COMPANY");

                entity.Property(e => e.Sex)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("SEX");

                entity.Property(e => e.ShortName)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("SHORT_NAME");

                entity.Property(e => e.SignatureImageUrl)
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("SIGNATURE_IMAGE_URL");

                entity.Property(e => e.Source)
                    .HasColumnType("NUMBER")
                    .HasColumnName("SOURCE");

                entity.Property(e => e.Status)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("STATUS");

                entity.Property(e => e.Step)
                    .HasColumnType("NUMBER")
                    .HasColumnName("STEP")
                    .HasDefaultValueSql("NULL");

                entity.Property(e => e.StockTradingAccount)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("STOCK_TRADING_ACCOUNT");

                entity.Property(e => e.TaxCode)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("TAX_CODE");

                entity.Property(e => e.TradingProviderId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("TRADING_PROVIDER_ID");

                entity.Property(e => e.VerifyEmailCode)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("VERIFY_EMAIL_CODE");

                entity.Property(e => e.VerifyEmailCodeCreatedDate)
                    .HasColumnType("DATE")
                    .HasColumnName("VERIFY_EMAIL_CODE_CREATED_DATE");
            });

            modelBuilder.Entity<EpInvestorAsset>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("EP_INVESTOR_ASSET");

                entity.Property(e => e.AccountNo)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("ACCOUNT_NO");

                entity.Property(e => e.AllowSell)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("ALLOW_SELL")
                    .HasDefaultValueSql("'N'");

                entity.Property(e => e.AssetId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("ASSET_ID");

                entity.Property(e => e.BondId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("BOND_ID");

                entity.Property(e => e.BondSecondaryId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("BOND_SECONDARY_ID");

                entity.Property(e => e.CertificateNo)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("CERTIFICATE_NO");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("CREATED_BY");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("DATE")
                    .HasColumnName("CREATED_DATE");

                entity.Property(e => e.Deleted)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("DELETED")
                    .HasDefaultValueSql("('N')\n   ")
                    .IsFixedLength(true);

                entity.Property(e => e.HoldQtty)
                    .HasColumnType("NUMBER")
                    .HasColumnName("HOLD_QTTY");

                entity.Property(e => e.HoldValue)
                    .HasColumnType("NUMBER")
                    .HasColumnName("HOLD_VALUE");

                entity.Property(e => e.InvestorId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("INVESTOR_ID");

                entity.Property(e => e.IsActive)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("IS_ACTIVE");

                entity.Property(e => e.OgAssetId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("OG_ASSET_ID");

                entity.Property(e => e.OgCertificateNo)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("OG_CERTIFICATE_NO");

                entity.Property(e => e.OrderId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("ORDER_ID");

                entity.Property(e => e.PendingQtty)
                    .HasColumnType("NUMBER")
                    .HasColumnName("PENDING_QTTY");

                entity.Property(e => e.PendingValue)
                    .HasColumnType("NUMBER")
                    .HasColumnName("PENDING_VALUE");

                entity.Property(e => e.TradeQtty)
                    .HasColumnType("NUMBER")
                    .HasColumnName("TRADE_QTTY");

                entity.Property(e => e.TradeValue)
                    .HasColumnType("NUMBER")
                    .HasColumnName("TRADE_VALUE");
            });

            modelBuilder.Entity<EpInvestorBankAccount>(entity =>
            {
                entity.ToTable("EP_INVESTOR_BANK_ACCOUNT");

                entity.Property(e => e.Id)
                    .HasColumnType("NUMBER")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID");

                entity.Property(e => e.BankAccount)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("BANK_ACCOUNT");

                entity.Property(e => e.BankBranch)
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasColumnName("BANK_BRANCH");

                entity.Property(e => e.BankCode)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("BANK_CODE");

                entity.Property(e => e.BankId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("BANK_ID");

                entity.Property(e => e.BankName)
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasColumnName("BANK_NAME");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("CREATED_BY");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("DATE")
                    .HasColumnName("CREATED_DATE");

                entity.Property(e => e.Deleted)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("DELETED")
                    .HasDefaultValueSql("'N'");

                entity.Property(e => e.InvestorGroupId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("INVESTOR_GROUP_ID");

                entity.Property(e => e.InvestorId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("INVESTOR_ID");

                entity.Property(e => e.IsDefault)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("IS_DEFAULT")
                    .HasDefaultValueSql("'N'");

                entity.Property(e => e.IsDefaultSale)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("IS_DEFAULT_SALE")
                    .HasDefaultValueSql("'N'\n   ");

                entity.Property(e => e.OwnerAccount)
                    .HasMaxLength(100)
                    .HasColumnName("OWNER_ACCOUNT");

                entity.Property(e => e.ReferId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("REFER_ID");
            });

            modelBuilder.Entity<EpInvestorBankAccountTemp>(entity =>
            {
                entity.ToTable("EP_INVESTOR_BANK_ACCOUNT_TEMP");

                entity.Property(e => e.Id)
                    .HasColumnType("NUMBER")
                    .HasColumnName("ID");

                entity.Property(e => e.BankAccount)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("BANK_ACCOUNT");

                entity.Property(e => e.BankBranch)
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasColumnName("BANK_BRANCH");

                entity.Property(e => e.BankCode)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("BANK_CODE");

                entity.Property(e => e.BankId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("BANK_ID");

                entity.Property(e => e.BankName)
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasColumnName("BANK_NAME");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("CREATED_BY");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("DATE")
                    .HasColumnName("CREATED_DATE");

                entity.Property(e => e.Deleted)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("DELETED")
                    .HasDefaultValueSql("'N'");

                entity.Property(e => e.InvestorGroupId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("INVESTOR_GROUP_ID");

                entity.Property(e => e.InvestorId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("INVESTOR_ID");

                entity.Property(e => e.IsDefault)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("IS_DEFAULT")
                    .HasDefaultValueSql("'N'");

                entity.Property(e => e.IsDefaultSale)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("IS_DEFAULT_SALE")
                    .HasDefaultValueSql("'N'\n   ");

                entity.Property(e => e.OwnerAccount)
                    .HasMaxLength(100)
                    .HasColumnName("OWNER_ACCOUNT");

                entity.Property(e => e.ReferId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("REFER_ID");
            });

            modelBuilder.Entity<EpInvestorContactAddTemp>(entity =>
            {
                entity.HasKey(e => e.ContactAddressId)
                    .HasName("EP_INVESTOR_CONTACT_ADD_TE_PK");

                entity.ToTable("EP_INVESTOR_CONTACT_ADD_TEMP");

                entity.Property(e => e.ContactAddressId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("CONTACT_ADDRESS_ID");

                entity.Property(e => e.ContactAddress)
                    .HasMaxLength(500)
                    .HasColumnName("CONTACT_ADDRESS");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("CREATED_BY");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("DATE")
                    .HasColumnName("CREATED_DATE");

                entity.Property(e => e.Deleted)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("DELETED")
                    .HasDefaultValueSql("'N'");

                entity.Property(e => e.DetailAddress)
                    .HasMaxLength(500)
                    .HasColumnName("DETAIL_ADDRESS");

                entity.Property(e => e.DistrictCode)
                    .HasMaxLength(6)
                    .IsUnicode(false)
                    .HasColumnName("DISTRICT_CODE");

                entity.Property(e => e.InvestorGroupId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("INVESTOR_GROUP_ID");

                entity.Property(e => e.InvestorId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("INVESTOR_ID");

                entity.Property(e => e.IsDefault)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("IS_DEFAULT")
                    .HasDefaultValueSql("'N'");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("MODIFIED_BY");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("DATE")
                    .HasColumnName("MODIFIED_DATE");

                entity.Property(e => e.ProvinceCode)
                    .HasMaxLength(6)
                    .IsUnicode(false)
                    .HasColumnName("PROVINCE_CODE");

                entity.Property(e => e.ReferId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("REFER_ID");

                entity.Property(e => e.WardCode)
                    .HasMaxLength(6)
                    .IsUnicode(false)
                    .HasColumnName("WARD_CODE");
            });

            modelBuilder.Entity<EpInvestorContactAddress>(entity =>
            {
                entity.HasKey(e => e.ContactAddressId)
                    .HasName("EP_INVESTOR_CONTACT_ADDRES_PK");

                entity.ToTable("EP_INVESTOR_CONTACT_ADDRESS");

                entity.Property(e => e.ContactAddressId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("CONTACT_ADDRESS_ID");

                entity.Property(e => e.ContactAddress)
                    .HasMaxLength(500)
                    .HasColumnName("CONTACT_ADDRESS");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("CREATED_BY");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("DATE")
                    .HasColumnName("CREATED_DATE")
                    .HasDefaultValueSql("SYSDATE");

                entity.Property(e => e.Deleted)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("DELETED")
                    .HasDefaultValueSql("'N'");

                entity.Property(e => e.DetailAddress)
                    .HasMaxLength(500)
                    .HasColumnName("DETAIL_ADDRESS");

                entity.Property(e => e.DistrictCode)
                    .HasMaxLength(6)
                    .IsUnicode(false)
                    .HasColumnName("DISTRICT_CODE");

                entity.Property(e => e.InvestorGroupId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("INVESTOR_GROUP_ID");

                entity.Property(e => e.InvestorId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("INVESTOR_ID");

                entity.Property(e => e.IsDefault)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("IS_DEFAULT")
                    .HasDefaultValueSql("'N'");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("MODIFIED_BY");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("DATE")
                    .HasColumnName("MODIFIED_DATE");

                entity.Property(e => e.ProvinceCode)
                    .HasMaxLength(6)
                    .IsUnicode(false)
                    .HasColumnName("PROVINCE_CODE");

                entity.Property(e => e.ReferId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("REFER_ID");

                entity.Property(e => e.WardCode)
                    .HasMaxLength(6)
                    .IsUnicode(false)
                    .HasColumnName("WARD_CODE");
            });

            modelBuilder.Entity<EpInvestorIdTemp>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("EP_INVESTOR_ID_TEMP");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(256)
                    .IsUnicode(false)
                    .HasColumnName("CREATED_BY");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("DATE")
                    .HasColumnName("CREATED_DATE")
                    .HasDefaultValueSql("SYSDATE");

                entity.Property(e => e.DateOfBirth)
                    .HasColumnType("DATE")
                    .HasColumnName("DATE_OF_BIRTH");

                entity.Property(e => e.Deleted)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("DELETED");

                entity.Property(e => e.EkycIncorrectFields)
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("EKYC_INCORRECT_FIELDS");

                entity.Property(e => e.EkycInfoIsConfirmed)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("EKYC_INFO_IS_CONFIRMED")
                    .HasDefaultValueSql("'N'\n   ")
                    .IsFixedLength(true);

                entity.Property(e => e.FaceImageUrl)
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("FACE_IMAGE_URL");

                entity.Property(e => e.FaceVideoUrl)
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("FACE_VIDEO_URL");

                entity.Property(e => e.Fullname)
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("FULLNAME");

                entity.Property(e => e.Id)
                    .HasColumnType("NUMBER")
                    .HasColumnName("ID");

                entity.Property(e => e.IdBackImageUrl)
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("ID_BACK_IMAGE_URL");

                entity.Property(e => e.IdDate)
                    .HasColumnType("DATE")
                    .HasColumnName("ID_DATE");

                entity.Property(e => e.IdExpiredDate)
                    .HasColumnType("DATE")
                    .HasColumnName("ID_EXPIRED_DATE");

                entity.Property(e => e.IdExtraImageUrl)
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("ID_EXTRA_IMAGE_URL");

                entity.Property(e => e.IdFrontImageUrl)
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("ID_FRONT_IMAGE_URL");

                entity.Property(e => e.IdIssuer)
                    .HasMaxLength(256)
                    .IsUnicode(false)
                    .HasColumnName("ID_ISSUER");

                entity.Property(e => e.IdNo)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("ID_NO");

                entity.Property(e => e.IdType)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("ID_TYPE");

                entity.Property(e => e.InvestorGroupId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("INVESTOR_GROUP_ID");

                entity.Property(e => e.InvestorId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("INVESTOR_ID");

                entity.Property(e => e.IsDefault)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("IS_DEFAULT")
                    .HasDefaultValueSql("('N')")
                    .IsFixedLength(true);

                entity.Property(e => e.IsDefaultSale)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("IS_DEFAULT_SALE")
                    .HasDefaultValueSql("'N'")
                    .IsFixedLength(true);

                entity.Property(e => e.IsVerifiedFace)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("IS_VERIFIED_FACE")
                    .HasDefaultValueSql("('N')");

                entity.Property(e => e.IsVerifiedIdentification)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("IS_VERIFIED_IDENTIFICATION")
                    .HasDefaultValueSql("('N')");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(256)
                    .IsUnicode(false)
                    .HasColumnName("MODIFIED_BY");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("DATE")
                    .HasColumnName("MODIFIED_DATE");

                entity.Property(e => e.Nationality)
                    .HasMaxLength(256)
                    .IsUnicode(false)
                    .HasColumnName("NATIONALITY");

                entity.Property(e => e.PersonalIdentification)
                    .HasMaxLength(256)
                    .IsUnicode(false)
                    .HasColumnName("PERSONAL_IDENTIFICATION");

                entity.Property(e => e.PlaceOfOrigin)
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("PLACE_OF_ORIGIN");

                entity.Property(e => e.PlaceOfResidence)
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("PLACE_OF_RESIDENCE");

                entity.Property(e => e.ReferId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("REFER_ID");

                entity.Property(e => e.Sex)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("SEX");

                entity.Property(e => e.Status)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("STATUS")
                    .HasDefaultValueSql("('A')")
                    .IsFixedLength(true);

                entity.Property(e => e.StatusApproved)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("STATUS_APPROVED")
                    .HasDefaultValueSql("0")
                    .IsFixedLength(true);
            });

            modelBuilder.Entity<EpInvestorIdentification>(entity =>
            {
                entity.ToTable("EP_INVESTOR_IDENTIFICATION");

                entity.HasIndex(e => new { e.Deleted, e.InvestorId, e.IdNo, e.Status, e.IsDefault, e.InvestorGroupId, e.IsVerifiedIdentification, e.IsVerifiedFace, e.ReferId }, "IX_INVESTOR_IDENTIFICATION");

                entity.Property(e => e.Id)
                    .HasColumnType("NUMBER")
                    .HasColumnName("ID");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(256)
                    .IsUnicode(false)
                    .HasColumnName("CREATED_BY");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("DATE")
                    .HasColumnName("CREATED_DATE")
                    .HasDefaultValueSql("SYSDATE");

                entity.Property(e => e.DateOfBirth)
                    .HasColumnType("DATE")
                    .HasColumnName("DATE_OF_BIRTH");

                entity.Property(e => e.Deleted)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("DELETED");

                entity.Property(e => e.EkycIncorrectFields)
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("EKYC_INCORRECT_FIELDS");

                entity.Property(e => e.EkycInfoIsConfirmed)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("EKYC_INFO_IS_CONFIRMED")
                    .HasDefaultValueSql("'N'\n   ")
                    .IsFixedLength(true);

                entity.Property(e => e.FaceImageUrl)
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("FACE_IMAGE_URL");

                entity.Property(e => e.FaceVideoUrl)
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("FACE_VIDEO_URL");

                entity.Property(e => e.Fullname)
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("FULLNAME");

                entity.Property(e => e.IdBackImageUrl)
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("ID_BACK_IMAGE_URL");

                entity.Property(e => e.IdDate)
                    .HasColumnType("DATE")
                    .HasColumnName("ID_DATE");

                entity.Property(e => e.IdExpiredDate)
                    .HasColumnType("DATE")
                    .HasColumnName("ID_EXPIRED_DATE");

                entity.Property(e => e.IdExtraImageUrl)
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("ID_EXTRA_IMAGE_URL");

                entity.Property(e => e.IdFrontImageUrl)
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("ID_FRONT_IMAGE_URL");

                entity.Property(e => e.IdIssuer)
                    .HasMaxLength(256)
                    .IsUnicode(false)
                    .HasColumnName("ID_ISSUER");

                entity.Property(e => e.IdNo)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("ID_NO");

                entity.Property(e => e.IdType)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("ID_TYPE");

                entity.Property(e => e.InvestorGroupId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("INVESTOR_GROUP_ID");

                entity.Property(e => e.InvestorId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("INVESTOR_ID");

                entity.Property(e => e.IsDefault)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("IS_DEFAULT")
                    .HasDefaultValueSql("('N')")
                    .IsFixedLength(true);

                entity.Property(e => e.IsDefaultSale)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("IS_DEFAULT_SALE")
                    .HasDefaultValueSql("'N'")
                    .IsFixedLength(true);

                entity.Property(e => e.IsVerifiedFace)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("IS_VERIFIED_FACE")
                    .HasDefaultValueSql("('N')");

                entity.Property(e => e.IsVerifiedIdentification)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("IS_VERIFIED_IDENTIFICATION")
                    .HasDefaultValueSql("('N')");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(256)
                    .IsUnicode(false)
                    .HasColumnName("MODIFIED_BY");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("DATE")
                    .HasColumnName("MODIFIED_DATE");

                entity.Property(e => e.Nationality)
                    .HasMaxLength(256)
                    .IsUnicode(false)
                    .HasColumnName("NATIONALITY");

                entity.Property(e => e.PersonalIdentification)
                    .HasMaxLength(256)
                    .IsUnicode(false)
                    .HasColumnName("PERSONAL_IDENTIFICATION");

                entity.Property(e => e.PlaceOfOrigin)
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("PLACE_OF_ORIGIN");

                entity.Property(e => e.PlaceOfResidence)
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("PLACE_OF_RESIDENCE");

                entity.Property(e => e.ReferId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("REFER_ID");

                entity.Property(e => e.Sex)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("SEX");

                entity.Property(e => e.Status)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("STATUS")
                    .HasDefaultValueSql("('A')")
                    .IsFixedLength(true);

                entity.Property(e => e.StatusApproved)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("STATUS_APPROVED")
                    .HasDefaultValueSql("0")
                    .IsFixedLength(true);
            });

            modelBuilder.Entity<EpInvestorProfFile>(entity =>
            {
                entity.ToTable("EP_INVESTOR_PROF_FILE");

                entity.Property(e => e.Id)
                    .HasColumnType("NUMBER")
                    .HasColumnName("ID");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("CREATED_BY");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("DATE")
                    .HasColumnName("CREATED_DATE")
                    .HasDefaultValueSql("SYSDATE");

                entity.Property(e => e.Deleted)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("DELETED")
                    .HasDefaultValueSql("'N'");

                entity.Property(e => e.InvestorGroupId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("INVESTOR_GROUP_ID");

                entity.Property(e => e.InvestorId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("INVESTOR_ID");

                entity.Property(e => e.InvestorTempId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("INVESTOR_TEMP_ID");

                entity.Property(e => e.ProfFileName)
                    .IsUnicode(false)
                    .HasColumnName("PROF_FILE_NAME");

                entity.Property(e => e.ProfFileType)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("PROF_FILE_TYPE");

                entity.Property(e => e.ProfFileUrl)
                    .IsUnicode(false)
                    .HasColumnName("PROF_FILE_URL");
            });

            modelBuilder.Entity<EpInvestorSale>(entity =>
            {
                entity.ToTable("EP_INVESTOR_SALE");

                entity.HasIndex(e => new { e.Deleted, e.InvestorId, e.IsDefault, e.SaleId, e.ReferralCode }, "IX_INVESTOR_SALE");

                entity.Property(e => e.Id)
                    .HasColumnType("NUMBER")
                    .HasColumnName("ID");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("CREATED_BY");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("DATE")
                    .HasColumnName("CREATED_DATE")
                    .HasDefaultValueSql("SYSDATE");

                entity.Property(e => e.Deleted)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("DELETED")
                    .HasDefaultValueSql("'N'")
                    .IsFixedLength(true);

                entity.Property(e => e.InvestorId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("INVESTOR_ID");

                entity.Property(e => e.IsDefault)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("IS_DEFAULT")
                    .HasDefaultValueSql("'N'\n   ")
                    .IsFixedLength(true);

                entity.Property(e => e.ReferralCode)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("REFERRAL_CODE");

                entity.Property(e => e.SaleId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("SALE_ID");
            });

            modelBuilder.Entity<EpInvestorStock>(entity =>
            {
                entity.ToTable("EP_INVESTOR_STOCK");

                entity.Property(e => e.Id)
                    .HasColumnType("NUMBER")
                    .HasColumnName("ID");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("CREATED_BY");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("DATE")
                    .HasColumnName("CREATED_DATE")
                    .HasDefaultValueSql("SYSDATE");

                entity.Property(e => e.Deleted)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("DELETED")
                    .HasDefaultValueSql("'N'")
                    .IsFixedLength(true);

                entity.Property(e => e.InvestorGroupId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("INVESTOR_GROUP_ID");

                entity.Property(e => e.InvestorId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("INVESTOR_ID");

                entity.Property(e => e.IsDefault)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("IS_DEFAULT")
                    .HasDefaultValueSql("'N'")
                    .IsFixedLength(true);

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("MODIFIED_BY");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("DATE")
                    .HasColumnName("MODIFIED_DATE");

                entity.Property(e => e.ReferId)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("REFER_ID");

                entity.Property(e => e.SecurityCompany)
                    .HasColumnType("NUMBER")
                    .HasColumnName("SECURITY_COMPANY");

                entity.Property(e => e.StockTradingAccount)
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("STOCK_TRADING_ACCOUNT");
            });

            modelBuilder.Entity<EpInvestorStockTemp>(entity =>
            {
                entity.ToTable("EP_INVESTOR_STOCK_TEMP");

                entity.Property(e => e.Id)
                    .HasColumnType("NUMBER")
                    .HasColumnName("ID");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("CREATED_BY");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("DATE")
                    .HasColumnName("CREATED_DATE")
                    .HasDefaultValueSql("SYSDATE");

                entity.Property(e => e.Deleted)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("DELETED")
                    .HasDefaultValueSql("'N'")
                    .IsFixedLength(true);

                entity.Property(e => e.InvestorGroupId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("INVESTOR_GROUP_ID");

                entity.Property(e => e.InvestorId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("INVESTOR_ID");

                entity.Property(e => e.IsDefault)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("IS_DEFAULT")
                    .HasDefaultValueSql("'N'")
                    .IsFixedLength(true);

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("MODIFIED_BY");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("DATE")
                    .HasColumnName("MODIFIED_DATE");

                entity.Property(e => e.ReferId)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("REFER_ID");

                entity.Property(e => e.SecurityCompany)
                    .HasColumnType("NUMBER")
                    .HasColumnName("SECURITY_COMPANY");

                entity.Property(e => e.StockTradingAccount)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("STOCK_TRADING_ACCOUNT");
            });

            modelBuilder.Entity<EpInvestorTemp>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("EP_INVESTOR_TEMP");

                entity.Property(e => e.AccountNo)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("ACCOUNT_NO");

                entity.Property(e => e.AccountStatus)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("ACCOUNT_STATUS")
                    .HasDefaultValueSql("'N'")
                    .IsFixedLength(true);

                entity.Property(e => e.AccountType)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("ACCOUNT_TYPE");

                entity.Property(e => e.Address)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("ADDRESS");

                entity.Property(e => e.ApprovedBy)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("APPROVED_BY");

                entity.Property(e => e.ApprovedDate)
                    .HasColumnType("DATE")
                    .HasColumnName("APPROVED_DATE")
                    .HasDefaultValueSql("SYSDATE");

                entity.Property(e => e.AvatarImageUrl)
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("AVATAR_IMAGE_URL");

                entity.Property(e => e.BirthDate)
                    .HasColumnType("DATE")
                    .HasColumnName("BIRTH_DATE");

                entity.Property(e => e.Bori)
                    .HasMaxLength(1)
                    .HasColumnName("BORI");

                entity.Property(e => e.ContactAddress)
                    .HasMaxLength(2000)
                    .IsUnicode(false)
                    .HasColumnName("CONTACT_ADDRESS");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("CREATED_BY");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("DATE")
                    .HasColumnName("CREATED_DATE")
                    .HasDefaultValueSql("SYSDATE");

                entity.Property(e => e.Deleted)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("DELETED")
                    .HasDefaultValueSql("'N'");

                entity.Property(e => e.Description)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("DESCRIPTION");

                entity.Property(e => e.Dorf)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("DORF");

                entity.Property(e => e.EContractUrl)
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("E_CONTRACT_URL");

                entity.Property(e => e.EcSignDate)
                    .HasColumnType("DATE")
                    .HasColumnName("EC_SIGN_DATE");

                entity.Property(e => e.EduLevel)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("EDU_LEVEL");

                entity.Property(e => e.EkycIncorrectFields)
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("EKYC_INCORRECT_FIELDS");

                entity.Property(e => e.EkycInfoIsConfirmed)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("EKYC_INFO_IS_CONFIRMED")
                    .IsFixedLength(true);

                entity.Property(e => e.EkycOcrCount)
                    .HasPrecision(2)
                    .HasColumnName("EKYC_OCR_COUNT");

                entity.Property(e => e.EkycStatus)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("EKYC_STATUS")
                    .HasDefaultValueSql("'N'")
                    .IsFixedLength(true);

                entity.Property(e => e.Email)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("EMAIL");

                entity.Property(e => e.FaceImageUrl)
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("FACE_IMAGE_URL");

                entity.Property(e => e.Fax)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("FAX");

                entity.Property(e => e.FoundationDate)
                    .HasColumnType("DATE")
                    .HasColumnName("FOUNDATION_DATE");

                entity.Property(e => e.FoundationIssuer)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("FOUNDATION_ISSUER");

                entity.Property(e => e.FoundationNo)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("FOUNDATION_NO");

                entity.Property(e => e.IdBackImageUrl)
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("ID_BACK_IMAGE_URL");

                entity.Property(e => e.IdDate)
                    .HasColumnType("DATE")
                    .HasColumnName("ID_DATE");

                entity.Property(e => e.IdExpiredDate)
                    .HasColumnType("DATE")
                    .HasColumnName("ID_EXPIRED_DATE");

                entity.Property(e => e.IdFrontImageUrl)
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("ID_FRONT_IMAGE_URL");

                entity.Property(e => e.IdIssuer)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("ID_ISSUER");

                entity.Property(e => e.IdNo)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("ID_NO");

                entity.Property(e => e.IdPlaceOfResidence)
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("ID_PLACE_OF_RESIDENCE");

                entity.Property(e => e.IdType)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("ID_TYPE");

                entity.Property(e => e.InvestorGroupId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("INVESTOR_GROUP_ID");

                entity.Property(e => e.InvestorId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("INVESTOR_ID");

                entity.Property(e => e.IsEcontractSign)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("IS_ECONTRACT_SIGN")
                    .IsFixedLength(true);

                entity.Property(e => e.IsOpnaccSigned)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("IS_OPNACC_SIGNED")
                    .HasDefaultValueSql("('N')");

                entity.Property(e => e.IsProf)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("IS_PROF")
                    .HasDefaultValueSql("'N'");

                entity.Property(e => e.Isonline)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("ISONLINE")
                    .HasDefaultValueSql("'Y'")
                    .IsFixedLength(true);

                entity.Property(e => e.LicenseDate)
                    .HasColumnType("DATE")
                    .HasColumnName("LICENSE_DATE");

                entity.Property(e => e.LicenseIssuer)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("LICENSE_ISSUER");

                entity.Property(e => e.LicenseNo)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("LICENSE_NO");

                entity.Property(e => e.LivenessVideoUrl)
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("LIVENESS_VIDEO_URL");

                entity.Property(e => e.Mobile)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("MOBILE");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("MODIFIED_BY");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("DATE")
                    .HasColumnName("MODIFIED_DATE");

                entity.Property(e => e.Name)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("NAME");

                entity.Property(e => e.NameEn)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("NAME_EN");

                entity.Property(e => e.Nationality)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("NATIONALITY");

                entity.Property(e => e.Notice)
                    .HasMaxLength(1000)
                    .IsUnicode(false)
                    .HasColumnName("NOTICE");

                entity.Property(e => e.Occupation)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("OCCUPATION");

                entity.Property(e => e.Phone)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("PHONE");

                entity.Property(e => e.PinCode)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("PIN_CODE");

                entity.Property(e => e.Priority)
                    .HasColumnType("NUMBER")
                    .HasColumnName("PRIORITY");

                entity.Property(e => e.ProfDueDate)
                    .HasColumnType("DATE")
                    .HasColumnName("PROF_DUE_DATE");

                entity.Property(e => e.ProfFileUrl)
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("PROF_FILE_URL");

                entity.Property(e => e.ProfStartDate)
                    .HasColumnType("DATE")
                    .HasColumnName("PROF_START_DATE");

                entity.Property(e => e.ReferralCodeSelf)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("REFERRAL_CODE_SELF");

                entity.Property(e => e.RegisterSource)
                    .HasMaxLength(256)
                    .IsUnicode(false)
                    .HasColumnName("REGISTER_SOURCE");

                entity.Property(e => e.RegisterType)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("REGISTER_TYPE");

                entity.Property(e => e.RepIdDate)
                    .HasColumnType("DATE")
                    .HasColumnName("REP_ID_DATE");

                entity.Property(e => e.RepIdIssuer)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("REP_ID_ISSUER");

                entity.Property(e => e.RepIdNo)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("REP_ID_NO");

                entity.Property(e => e.RepName)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("REP_NAME");

                entity.Property(e => e.RepPosition)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("REP_POSITION");

                entity.Property(e => e.RepresentativeEmail)
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasColumnName("REPRESENTATIVE_EMAIL");

                entity.Property(e => e.RepresentativePhone)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("REPRESENTATIVE_PHONE");

                entity.Property(e => e.SaleId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("SALE_ID");

                entity.Property(e => e.SecurityCompany)
                    .HasColumnType("NUMBER")
                    .HasColumnName("SECURITY_COMPANY");

                entity.Property(e => e.Sex)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("SEX");

                entity.Property(e => e.ShortName)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("SHORT_NAME");

                entity.Property(e => e.SignatureImageUrl)
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("SIGNATURE_IMAGE_URL");

                entity.Property(e => e.Source)
                    .HasColumnType("NUMBER")
                    .HasColumnName("SOURCE");

                entity.Property(e => e.Status)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("STATUS")
                    .IsFixedLength(true);

                entity.Property(e => e.StockTradingAccount)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("STOCK_TRADING_ACCOUNT");

                entity.Property(e => e.TaxCode)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("TAX_CODE");

                entity.Property(e => e.TradingProviderId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("TRADING_PROVIDER_ID");
            });

            modelBuilder.Entity<EpInvestorToDo>(entity =>
            {
                entity.ToTable("EP_INVESTOR_TO_DO");

                entity.HasIndex(e => e.InvestorId, "IX_INVESTOR_TO_DO");

                entity.Property(e => e.Id)
                    .HasPrecision(10)
                    .ValueGeneratedNever()
                    .HasColumnName("ID");

                entity.Property(e => e.Detail)
                    .HasMaxLength(512)
                    .HasColumnName("DETAIL");

                entity.Property(e => e.InvestorId)
                    .HasPrecision(10)
                    .HasColumnName("INVESTOR_ID");

                entity.Property(e => e.Status)
                    .HasPrecision(10)
                    .HasColumnName("STATUS")
                    .HasDefaultValueSql("1 ");

                entity.Property(e => e.Type)
                    .HasPrecision(10)
                    .HasColumnName("TYPE");
            });

            modelBuilder.Entity<EpInvestorTradingProvider>(entity =>
            {
                entity.ToTable("EP_INVESTOR_TRADING_PROVIDER");

                entity.Property(e => e.Id)
                    .HasColumnType("NUMBER")
                    .HasColumnName("ID");

                entity.Property(e => e.CreateBy)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("CREATE_BY");

                entity.Property(e => e.CreateDate)
                    .HasColumnType("DATE")
                    .HasColumnName("CREATE_DATE")
                    .HasDefaultValueSql("SYSDATE");

                entity.Property(e => e.Deleted)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("DELETED")
                    .HasDefaultValueSql("('N')\n   ")
                    .IsFixedLength(true);

                entity.Property(e => e.InvestorId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("INVESTOR_ID");

                entity.Property(e => e.TradingProviderId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("TRADING_PROVIDER_ID");
            });

            modelBuilder.Entity<EpIssuer>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("EP_ISSUER");

                entity.Property(e => e.BusinessCustomerId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("BUSINESS_CUSTOMER_ID");

                entity.Property(e => e.BusinessProfit)
                    .HasColumnType("NUMBER")
                    .HasColumnName("BUSINESS_PROFIT");

                entity.Property(e => e.BusinessTurnover)
                    .HasColumnType("NUMBER")
                    .HasColumnName("BUSINESS_TURNOVER");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("CREATED_BY");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("DATE")
                    .HasColumnName("CREATED_DATE");

                entity.Property(e => e.Deleted)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("DELETED")
                    .HasDefaultValueSql("'N'");

                entity.Property(e => e.Image)
                    .HasMaxLength(256)
                    .IsUnicode(false)
                    .HasColumnName("IMAGE");

                entity.Property(e => e.IssuerId)
                    .HasPrecision(10)
                    .HasColumnName("ISSUER_ID");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("MODIFIED_BY");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("DATE")
                    .HasColumnName("MODIFIED_DATE");

                entity.Property(e => e.PartnerId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("PARTNER_ID");

                entity.Property(e => e.Roa)
                    .HasColumnType("NUMBER")
                    .HasColumnName("ROA");

                entity.Property(e => e.Roe)
                    .HasColumnType("NUMBER")
                    .HasColumnName("ROE");

                entity.Property(e => e.Status)
                    .HasPrecision(1)
                    .HasColumnName("STATUS")
                    .HasDefaultValueSql("1");
            });

            modelBuilder.Entity<EpJuridicalFile>(entity =>
            {
                entity.HasKey(e => e.JuridicalFileId)
                    .HasName("EP_GUARANTEE_FILE_PK");

                entity.ToTable("EP_JURIDICAL_FILE");

                entity.Property(e => e.JuridicalFileId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("JURIDICAL_FILE_ID");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(256)
                    .IsUnicode(false)
                    .HasColumnName("CREATED_BY");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("DATE")
                    .HasColumnName("CREATED_DATE")
                    .HasDefaultValueSql("SYSDATE");

                entity.Property(e => e.Deleted)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("DELETED")
                    .HasDefaultValueSql("('N')")
                    .IsFixedLength(true);

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("MODIFIED_BY");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("DATE")
                    .HasColumnName("MODIFIED_DATE");

                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("NAME");

                entity.Property(e => e.ProductBondId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("PRODUCT_BOND_ID");

                entity.Property(e => e.Stastus)
                    .HasPrecision(1)
                    .HasColumnName("STASTUS")
                    .HasDefaultValueSql("1\n   ");

                entity.Property(e => e.Url)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("URL");
            });

            modelBuilder.Entity<EpMsbNotification>(entity =>
            {
                entity.ToTable("EP_MSB_NOTIFICATION");

                entity.HasIndex(e => e.TranSeq, "IX_EP_MSB_NOTIFICATION_TRAN_SEQ");

                entity.Property(e => e.Id)
                    .HasPrecision(19)
                    .ValueGeneratedNever()
                    .HasColumnName("ID");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("DATE")
                    .HasColumnName("CREATED_DATE")
                    .HasDefaultValueSql("(SYSDATE) ");

                entity.Property(e => e.Exception).HasColumnName("EXCEPTION");

                entity.Property(e => e.FromAccountName).HasColumnName("FROM_ACCOUNT_NAME");

                entity.Property(e => e.FromAccountNumber).HasColumnName("FROM_ACCOUNT_NUMBER");

                entity.Property(e => e.Ip)
                    .HasMaxLength(2000)
                    .IsUnicode(false)
                    .HasColumnName("IP");

                entity.Property(e => e.Signature).HasColumnName("SIGNATURE");

                entity.Property(e => e.Status)
                    .HasPrecision(10)
                    .HasColumnName("STATUS");

                entity.Property(e => e.ToAccountName).HasColumnName("TO_ACCOUNT_NAME");

                entity.Property(e => e.ToAccountNumber).HasColumnName("TO_ACCOUNT_NUMBER");

                entity.Property(e => e.TranAmount).HasColumnName("TRAN_AMOUNT");

                entity.Property(e => e.TranDate).HasColumnName("TRAN_DATE");

                entity.Property(e => e.TranRemark).HasColumnName("TRAN_REMARK");

                entity.Property(e => e.TranSeq)
                    .HasMaxLength(2000)
                    .HasColumnName("TRAN_SEQ");

                entity.Property(e => e.VaCode).HasColumnName("VA_CODE");

                entity.Property(e => e.VaNumber).HasColumnName("VA_NUMBER");
            });

            modelBuilder.Entity<EpMsbNotificationPayment>(entity =>
            {
                entity.ToTable("EP_MSB_NOTIFICATION_PAYMENT");

                entity.Property(e => e.Id)
                    .HasPrecision(19)
                    .ValueGeneratedNever()
                    .HasColumnName("ID");

                entity.Property(e => e.Amount).HasColumnName("AMOUNT");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("DATE")
                    .HasColumnName("CREATED_DATE")
                    .HasDefaultValueSql("(SYSDATE) ");

                entity.Property(e => e.Exception)
                    .HasMaxLength(2000)
                    .IsUnicode(false)
                    .HasColumnName("EXCEPTION");

                entity.Property(e => e.Fee).HasColumnName("FEE");

                entity.Property(e => e.HandleStatus)
                    .HasPrecision(10)
                    .HasColumnName("HANDLE_STATUS");

                entity.Property(e => e.Ip)
                    .HasMaxLength(2000)
                    .IsUnicode(false)
                    .HasColumnName("IP");

                entity.Property(e => e.MId).HasColumnName("M_ID");

                entity.Property(e => e.NapasTransId).HasColumnName("NAPAS_TRANS_ID");

                entity.Property(e => e.Note).HasColumnName("NOTE");

                entity.Property(e => e.ReceiveAccount).HasColumnName("RECEIVE_ACCOUNT");

                entity.Property(e => e.ReceiveBank).HasColumnName("RECEIVE_BANK");

                entity.Property(e => e.ReceiveName).HasColumnName("RECEIVE_NAME");

                entity.Property(e => e.Rrn).HasColumnName("RRN");

                entity.Property(e => e.SecureHash).HasColumnName("SECURE_HASH");

                entity.Property(e => e.SenderAccount).HasColumnName("SENDER_ACCOUNT");

                entity.Property(e => e.SenderName).HasColumnName("SENDER_NAME");

                entity.Property(e => e.Status).HasColumnName("STATUS");

                entity.Property(e => e.TId).HasColumnName("T_ID");

                entity.Property(e => e.TransDate).HasColumnName("TRANS_DATE");

                entity.Property(e => e.TransId).HasColumnName("TRANS_ID");
            });

            modelBuilder.Entity<EpMsbRequestPayment>(entity =>
            {
                entity.ToTable("EP_MSB_REQUEST_PAYMENT");

                entity.HasIndex(e => new { e.TradingProdiverId, e.ProductType, e.RequestType }, "IX_EP_MSB_REQUEST_PAYMENT");

                entity.Property(e => e.Id)
                    .HasPrecision(19)
                    .ValueGeneratedNever()
                    .HasColumnName("ID");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("CREATED_BY");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("DATE")
                    .HasColumnName("CREATED_DATE")
                    .HasDefaultValueSql("(SYSDATE)");

                entity.Property(e => e.ProductType)
                    .HasPrecision(10)
                    .HasColumnName("PRODUCT_TYPE");

                entity.Property(e => e.RequestType)
                    .HasPrecision(10)
                    .HasColumnName("REQUEST_TYPE");

                entity.Property(e => e.TradingProdiverId)
                    .HasPrecision(10)
                    .HasColumnName("TRADING_PRODIVER_ID");
            });

            modelBuilder.Entity<EpMsbRequestPaymentDetail>(entity =>
            {
                entity.ToTable("EP_MSB_REQUEST_PAYMENT_DETAIL");

                entity.HasIndex(e => new { e.ReferId, e.RequestId }, "IX_EP_MSB_REQUEST_PAYMENT_DETAIL");

                entity.Property(e => e.Id)
                    .HasPrecision(19)
                    .ValueGeneratedNever()
                    .HasColumnName("ID");

                entity.Property(e => e.AmountMoney)
                    .HasColumnType("NUMBER(18,2)")
                    .HasColumnName("AMOUNT_MONEY")
                    .HasDefaultValueSql("0 ");

                entity.Property(e => e.BankId)
                    .HasPrecision(10)
                    .HasColumnName("BANK_ID");

                entity.Property(e => e.Bin).HasColumnName("BIN");

                entity.Property(e => e.DataType)
                    .HasPrecision(10)
                    .HasColumnName("DATA_TYPE");

                entity.Property(e => e.Exception).HasColumnName("EXCEPTION");

                entity.Property(e => e.Note).HasColumnName("NOTE");

                entity.Property(e => e.OwnerAccount)
                    .IsRequired()
                    .HasColumnName("OWNER_ACCOUNT");

                entity.Property(e => e.ReferId)
                    .HasPrecision(10)
                    .HasColumnName("REFER_ID");

                entity.Property(e => e.RequestId)
                    .HasPrecision(19)
                    .HasColumnName("REQUEST_ID");

                entity.Property(e => e.Status)
                    .HasPrecision(10)
                    .HasColumnName("STATUS");

                entity.Property(e => e.TradingBankAccId)
                    .HasPrecision(10)
                    .HasColumnName("TRADING_BANK_ACC_ID")
                    .HasDefaultValueSql("0 ");
            });

            modelBuilder.Entity<EpPolicyFile>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("EP_POLICY_FILE");

                entity.Property(e => e.BondSecondaryId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("BOND_SECONDARY_ID");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(256)
                    .IsUnicode(false)
                    .HasColumnName("CREATED_BY");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("DATE")
                    .HasColumnName("CREATED_DATE")
                    .HasDefaultValueSql("SYSDATE");

                entity.Property(e => e.Deleted)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("DELETED")
                    .HasDefaultValueSql("'N'")
                    .IsFixedLength(true);

                entity.Property(e => e.EffectiveDate)
                    .HasColumnType("DATE")
                    .HasColumnName("EFFECTIVE_DATE");

                entity.Property(e => e.ExpirationDate)
                    .HasColumnType("DATE")
                    .HasColumnName("EXPIRATION_DATE");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("MODIFIED_BY");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("DATE")
                    .HasColumnName("MODIFIED_DATE");

                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("NAME");

                entity.Property(e => e.PolicyFileId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("POLICY_FILE_ID");

                entity.Property(e => e.Status)
                    .HasColumnType("NUMBER")
                    .HasColumnName("STATUS");

                entity.Property(e => e.TradingProviderId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("TRADING_PROVIDER_ID");

                entity.Property(e => e.Url)
                    .HasMaxLength(256)
                    .IsUnicode(false)
                    .HasColumnName("URL");
            });

            modelBuilder.Entity<EpProductBondInfo>(entity =>
            {
                entity.HasKey(e => e.ProductBondId)
                    .HasName("SYS_C007661");

                entity.ToTable("EP_PRODUCT_BOND_INFO");

                entity.Property(e => e.ProductBondId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("PRODUCT_BOND_ID");

                entity.Property(e => e.AllowSbd)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("ALLOW_SBD")
                    .IsFixedLength(true);

                entity.Property(e => e.AllowSbdMonth)
                    .HasColumnType("NUMBER")
                    .HasColumnName("ALLOW_SBD_MONTH");

                entity.Property(e => e.BondCode)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("BOND_CODE");

                entity.Property(e => e.BondName)
                    .HasMaxLength(256)
                    .IsUnicode(false)
                    .HasColumnName("BOND_NAME");

                entity.Property(e => e.BondPeriod)
                    .HasColumnType("NUMBER")
                    .HasColumnName("BOND_PERIOD");

                entity.Property(e => e.BondPeriodUnit)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("BOND_PERIOD_UNIT")
                    .IsFixedLength(true);

                entity.Property(e => e.Content)
                    .HasColumnType("CLOB")
                    .HasColumnName("CONTENT");

                entity.Property(e => e.CountType)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("COUNT_TYPE")
                    .IsFixedLength(true);

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasColumnName("CREATED_BY");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("DATE")
                    .HasColumnName("CREATED_DATE");

                entity.Property(e => e.Deleted)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("DELETED")
                    .HasDefaultValueSql("('N')")
                    .IsFixedLength(true);

                entity.Property(e => e.DepositProviderId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("DEPOSIT_PROVIDER_ID");

                entity.Property(e => e.Description)
                    .HasColumnType("CLOB")
                    .HasColumnName("DESCRIPTION");

                entity.Property(e => e.DueDate)
                    .HasColumnType("DATE")
                    .HasColumnName("DUE_DATE");

                entity.Property(e => e.FeeRate)
                    .HasColumnType("NUMBER")
                    .HasColumnName("FEE_RATE");

                entity.Property(e => e.GuaranteeOrganization)
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("GUARANTEE_ORGANIZATION");

                entity.Property(e => e.Icon)
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("ICON");

                entity.Property(e => e.InterestCouponType)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("INTEREST_COUPON_TYPE")
                    .IsFixedLength(true);

                entity.Property(e => e.InterestPeriod)
                    .HasColumnType("NUMBER")
                    .HasColumnName("INTEREST_PERIOD");

                entity.Property(e => e.InterestPeriodUnit)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("INTEREST_PERIOD_UNIT")
                    .HasDefaultValueSql("('M')")
                    .IsFixedLength(true);

                entity.Property(e => e.InterestRate)
                    .HasColumnType("NUMBER")
                    .HasColumnName("INTEREST_RATE");

                entity.Property(e => e.InterestRateType)
                    .HasColumnType("NUMBER")
                    .HasColumnName("INTEREST_RATE_TYPE");

                entity.Property(e => e.InterestType)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("INTEREST_TYPE")
                    .IsFixedLength(true);

                entity.Property(e => e.IsCheck)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("IS_CHECK")
                    .IsFixedLength(true);

                entity.Property(e => e.IsCollateral)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("IS_COLLATERAL")
                    .HasDefaultValueSql("'N'")
                    .IsFixedLength(true);

                entity.Property(e => e.IsCreated)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("IS_CREATED");

                entity.Property(e => e.IsPaymentGuarantee)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("IS_PAYMENT_GUARANTEE")
                    .IsFixedLength(true);

                entity.Property(e => e.IssueDate)
                    .HasColumnType("DATE")
                    .HasColumnName("ISSUE_DATE");

                entity.Property(e => e.IssuerId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("ISSUER_ID");

                entity.Property(e => e.MaxInvestor)
                    .HasColumnType("NUMBER")
                    .HasColumnName("MAX_INVESTOR")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasColumnName("MODIFIED_BY");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("DATE")
                    .HasColumnName("MODIFIED_DATE");

                entity.Property(e => e.NiemYet)
                    .HasMaxLength(50)
                    .HasColumnName("NIEM_YET");

                entity.Property(e => e.NumberClosePer)
                    .HasColumnType("NUMBER")
                    .HasColumnName("NUMBER_CLOSE_PER");

                entity.Property(e => e.ParValue)
                    .HasColumnType("NUMBER")
                    .HasColumnName("PAR_VALUE");

                entity.Property(e => e.PartnerId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("PARTNER_ID");

                entity.Property(e => e.PolicyPaymentContent)
                    .HasColumnType("CLOB")
                    .HasColumnName("POLICY_PAYMENT_CONTENT");

                entity.Property(e => e.Quantity)
                    .HasColumnType("NUMBER")
                    .HasColumnName("QUANTITY");

                entity.Property(e => e.Status)
                    .HasPrecision(1)
                    .HasColumnName("STATUS")
                    .HasDefaultValueSql("1");
            });

            modelBuilder.Entity<EpProductBondPolicy>(entity =>
            {
                entity.HasKey(e => e.BondPolicyId)
                    .HasName("SYS_C007551");

                entity.ToTable("EP_PRODUCT_BOND_POLICY");

                entity.Property(e => e.BondPolicyId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("BOND_POLICY_ID");

                entity.Property(e => e.BondSecondaryId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("BOND_SECONDARY_ID");

                entity.Property(e => e.Classify)
                    .HasColumnType("NUMBER")
                    .HasColumnName("CLASSIFY");

                entity.Property(e => e.Code)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("CODE");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(256)
                    .IsUnicode(false)
                    .HasColumnName("CREATED_BY");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("DATE")
                    .HasColumnName("CREATED_DATE")
                    .HasDefaultValueSql("SYSDATE");

                entity.Property(e => e.Deleted)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("DELETED")
                    .HasDefaultValueSql("('N')")
                    .IsFixedLength(true);

                entity.Property(e => e.Description)
                    .HasMaxLength(2048)
                    .IsUnicode(false)
                    .HasColumnName("DESCRIPTION");

                entity.Property(e => e.EndDate)
                    .HasColumnType("DATE")
                    .HasColumnName("END_DATE");

                entity.Property(e => e.IncomeTax)
                    .HasColumnType("NUMBER")
                    .HasColumnName("INCOME_TAX");

                entity.Property(e => e.InvestorType)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("INVESTOR_TYPE")
                    .IsFixedLength(true);

                entity.Property(e => e.IsShowApp)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("IS_SHOW_APP")
                    .HasDefaultValueSql("'Y'")
                    .IsFixedLength(true);

                entity.Property(e => e.IsTransfer)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("IS_TRANSFER")
                    .HasDefaultValueSql("('N')")
                    .IsFixedLength(true);

                entity.Property(e => e.MinMoney)
                    .HasColumnType("NUMBER")
                    .HasColumnName("MIN_MONEY");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("MODIFIED_BY");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("DATE")
                    .HasColumnName("MODIFIED_DATE");

                entity.Property(e => e.Name)
                    .HasMaxLength(256)
                    .IsUnicode(false)
                    .HasColumnName("NAME");

                entity.Property(e => e.StartDate)
                    .HasColumnType("DATE")
                    .HasColumnName("START_DATE");

                entity.Property(e => e.Status)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("STATUS")
                    .HasDefaultValueSql("('A')")
                    .IsFixedLength(true);

                entity.Property(e => e.TradingProviderId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("TRADING_PROVIDER_ID");

                entity.Property(e => e.TransferTax)
                    .HasColumnType("NUMBER")
                    .HasColumnName("TRANSFER_TAX");

                entity.Property(e => e.Type)
                    .HasColumnType("NUMBER")
                    .HasColumnName("TYPE");
            });

            modelBuilder.Entity<EpProductBondPolicyDeTemp>(entity =>
            {
                entity.HasKey(e => e.BondPolicyDetailTempId)
                    .HasName("EP_BOND_POLICY_DETAIL_TEMP_PK");

                entity.ToTable("EP_PRODUCT_BOND_POLICY_DE_TEMP");

                entity.Property(e => e.BondPolicyDetailTempId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("BOND_POLICY_DETAIL_TEMP_ID");

                entity.Property(e => e.BondPolicyTempId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("BOND_POLICY_TEMP_ID");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(256)
                    .IsUnicode(false)
                    .HasColumnName("CREATED_BY");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("DATE")
                    .HasColumnName("CREATED_DATE")
                    .HasDefaultValueSql("SYSDATE");

                entity.Property(e => e.Deleted)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("DELETED")
                    .HasDefaultValueSql("('N')\n   ")
                    .IsFixedLength(true);

                entity.Property(e => e.InterestDays)
                    .HasColumnType("NUMBER")
                    .HasColumnName("INTEREST_DAYS");

                entity.Property(e => e.InterestPeriodQuantity)
                    .HasColumnType("NUMBER")
                    .HasColumnName("INTEREST_PERIOD_QUANTITY");

                entity.Property(e => e.InterestPeriodType)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("INTEREST_PERIOD_TYPE")
                    .IsFixedLength(true);

                entity.Property(e => e.InterestType)
                    .HasColumnType("NUMBER")
                    .HasColumnName("INTEREST_TYPE");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("MODIFIED_BY");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("DATE")
                    .HasColumnName("MODIFIED_DATE");

                entity.Property(e => e.Name)
                    .HasMaxLength(256)
                    .IsUnicode(false)
                    .HasColumnName("NAME");

                entity.Property(e => e.PeriodQuantity)
                    .HasColumnType("NUMBER")
                    .HasColumnName("PERIOD_QUANTITY");

                entity.Property(e => e.PeriodType)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("PERIOD_TYPE")
                    .IsFixedLength(true);

                entity.Property(e => e.Profit)
                    .HasColumnType("NUMBER")
                    .HasColumnName("PROFIT");

                entity.Property(e => e.ShortName)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("SHORT_NAME");

                entity.Property(e => e.Status)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("STATUS")
                    .HasDefaultValueSql("('A')")
                    .IsFixedLength(true);

                entity.Property(e => e.Stt)
                    .HasColumnType("NUMBER")
                    .HasColumnName("STT");
            });

            modelBuilder.Entity<EpProductBondPolicyDetail>(entity =>
            {
                entity.HasKey(e => e.BondPolicyDetailId)
                    .HasName("EP_BOND_POLICY_DETAIL_PK");

                entity.ToTable("EP_PRODUCT_BOND_POLICY_DETAIL");

                entity.Property(e => e.BondPolicyDetailId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("BOND_POLICY_DETAIL_ID");

                entity.Property(e => e.BondPolicyId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("BOND_POLICY_ID");

                entity.Property(e => e.BondSecondaryId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("BOND_SECONDARY_ID");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(256)
                    .IsUnicode(false)
                    .HasColumnName("CREATED_BY");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("DATE")
                    .HasColumnName("CREATED_DATE")
                    .HasDefaultValueSql("SYSDATE");

                entity.Property(e => e.Deleted)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("DELETED")
                    .HasDefaultValueSql("('N')\n   ")
                    .IsFixedLength(true);

                entity.Property(e => e.InterestDays)
                    .HasColumnType("NUMBER")
                    .HasColumnName("INTEREST_DAYS");

                entity.Property(e => e.InterestPeriodQuantity)
                    .HasColumnType("NUMBER")
                    .HasColumnName("INTEREST_PERIOD_QUANTITY");

                entity.Property(e => e.InterestPeriodType)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("INTEREST_PERIOD_TYPE")
                    .IsFixedLength(true);

                entity.Property(e => e.InterestType)
                    .HasColumnType("NUMBER")
                    .HasColumnName("INTEREST_TYPE");

                entity.Property(e => e.IsShowApp)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("IS_SHOW_APP")
                    .HasDefaultValueSql("'Y'")
                    .IsFixedLength(true);

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("MODIFIED_BY");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("DATE")
                    .HasColumnName("MODIFIED_DATE");

                entity.Property(e => e.Name)
                    .HasMaxLength(256)
                    .IsUnicode(false)
                    .HasColumnName("NAME");

                entity.Property(e => e.PeriodQuantity)
                    .HasColumnType("NUMBER")
                    .HasColumnName("PERIOD_QUANTITY");

                entity.Property(e => e.PeriodType)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("PERIOD_TYPE")
                    .IsFixedLength(true);

                entity.Property(e => e.Profit)
                    .HasColumnType("NUMBER")
                    .HasColumnName("PROFIT");

                entity.Property(e => e.ShortName)
                    .HasMaxLength(256)
                    .IsUnicode(false)
                    .HasColumnName("SHORT_NAME");

                entity.Property(e => e.Status)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("STATUS")
                    .HasDefaultValueSql("('A')")
                    .IsFixedLength(true);

                entity.Property(e => e.Stt)
                    .HasColumnType("NUMBER")
                    .HasColumnName("STT");

                entity.Property(e => e.TradingProviderId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("TRADING_PROVIDER_ID");
            });

            modelBuilder.Entity<EpProductBondPolicyTemp>(entity =>
            {
                entity.HasKey(e => e.BondPolicyTempId)
                    .HasName("SYS_C007540");

                entity.ToTable("EP_PRODUCT_BOND_POLICY_TEMP");

                entity.Property(e => e.BondPolicyTempId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("BOND_POLICY_TEMP_ID");

                entity.Property(e => e.Classify)
                    .HasColumnType("NUMBER")
                    .HasColumnName("CLASSIFY");

                entity.Property(e => e.Code)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("CODE");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(256)
                    .IsUnicode(false)
                    .HasColumnName("CREATED_BY");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("DATE")
                    .HasColumnName("CREATED_DATE")
                    .HasDefaultValueSql("SYSDATE");

                entity.Property(e => e.Deleted)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("DELETED")
                    .HasDefaultValueSql("('N')\n   ")
                    .IsFixedLength(true);

                entity.Property(e => e.IncomeTax)
                    .HasColumnType("NUMBER")
                    .HasColumnName("INCOME_TAX");

                entity.Property(e => e.InvestorType)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("INVESTOR_TYPE")
                    .IsFixedLength(true);

                entity.Property(e => e.IsTransfer)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("IS_TRANSFER")
                    .HasDefaultValueSql("('N')")
                    .IsFixedLength(true);

                entity.Property(e => e.MinMoney)
                    .HasColumnType("NUMBER")
                    .HasColumnName("MIN_MONEY");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("MODIFIED_BY");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("DATE")
                    .HasColumnName("MODIFIED_DATE");

                entity.Property(e => e.Name)
                    .HasMaxLength(256)
                    .IsUnicode(false)
                    .HasColumnName("NAME");

                entity.Property(e => e.Status)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("STATUS")
                    .HasDefaultValueSql("('A')")
                    .IsFixedLength(true);

                entity.Property(e => e.TradingProviderId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("TRADING_PROVIDER_ID");

                entity.Property(e => e.TransferTax)
                    .HasColumnType("NUMBER")
                    .HasColumnName("TRANSFER_TAX");

                entity.Property(e => e.Type)
                    .HasColumnType("NUMBER")
                    .HasColumnName("TYPE");
            });

            modelBuilder.Entity<EpProductBondPrimary>(entity =>
            {
                entity.HasKey(e => e.BondPrimaryId)
                    .HasName("PK_EP_PRODUCT_BOND_PRIMARY_BON");

                entity.ToTable("EP_PRODUCT_BOND_PRIMARY");

                entity.Property(e => e.BondPrimaryId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("BOND_PRIMARY_ID");

                entity.Property(e => e.BondTypeId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("BOND_TYPE_ID");

                entity.Property(e => e.BusinessCustomerBankAccId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("BUSINESS_CUSTOMER_BANK_ACC_ID");

                entity.Property(e => e.CloseCellDate)
                    .HasColumnType("DATE")
                    .HasColumnName("CLOSE_CELL_DATE");

                entity.Property(e => e.Code)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("CODE");

                entity.Property(e => e.ContractCode)
                    .HasMaxLength(256)
                    .IsUnicode(false)
                    .HasColumnName("CONTRACT_CODE");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("CREATED_BY");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("DATE")
                    .HasColumnName("CREATED_DATE")
                    .HasDefaultValueSql("SYSDATE");

                entity.Property(e => e.Deleted)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("DELETED")
                    .HasDefaultValueSql("'N'")
                    .IsFixedLength(true);

                entity.Property(e => e.IsCheck)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("IS_CHECK")
                    .IsFixedLength(true);

                entity.Property(e => e.MaxInvestor)
                    .HasColumnType("NUMBER")
                    .HasColumnName("MAX_INVESTOR");

                entity.Property(e => e.MinMoney)
                    .HasColumnType("NUMBER")
                    .HasColumnName("MIN_MONEY");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("MODIFIED_BY");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("DATE")
                    .HasColumnName("MODIFIED_DATE");

                entity.Property(e => e.Name)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("NAME");

                entity.Property(e => e.OpenCellDate)
                    .HasColumnType("DATE")
                    .HasColumnName("OPEN_CELL_DATE");

                entity.Property(e => e.PartnerId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("PARTNER_ID");

                entity.Property(e => e.PaymentType)
                    .HasPrecision(1)
                    .HasColumnName("PAYMENT_TYPE");

                entity.Property(e => e.PriceType)
                    .HasPrecision(1)
                    .HasColumnName("PRICE_TYPE");

                entity.Property(e => e.ProductBondId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("PRODUCT_BOND_ID");

                entity.Property(e => e.Quantity)
                    .HasColumnType("NUMBER")
                    .HasColumnName("QUANTITY");

                entity.Property(e => e.Status)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("STATUS")
                    .IsFixedLength(true);

                entity.Property(e => e.TradingProviderId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("TRADING_PROVIDER_ID");
            });

            modelBuilder.Entity<EpProductBondSecondPrice>(entity =>
            {
                entity.HasKey(e => e.PriceId)
                    .HasName("PK_EP_BOND_SECOND_PRICE");

                entity.ToTable("EP_PRODUCT_BOND_SECOND_PRICE");

                entity.Property(e => e.PriceId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("PRICE_ID");

                entity.Property(e => e.BondSecondaryId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("BOND_SECONDARY_ID");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("CREATED_BY");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("DATE")
                    .HasColumnName("CREATED_DATE")
                    .HasDefaultValueSql("SYSDATE");

                entity.Property(e => e.Deleted)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("DELETED")
                    .HasDefaultValueSql("'N'")
                    .IsFixedLength(true);

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("MODIFIED_BY");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("DATE")
                    .HasColumnName("MODIFIED_DATE");

                entity.Property(e => e.Price)
                    .HasColumnType("NUMBER")
                    .HasColumnName("PRICE");

                entity.Property(e => e.PriceDate)
                    .HasColumnType("DATE")
                    .HasColumnName("PRICE_DATE");

                entity.Property(e => e.TradingProviderId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("TRADING_PROVIDER_ID");
            });

            modelBuilder.Entity<EpProductBondSecondary>(entity =>
            {
                entity.HasKey(e => e.BondSecondaryId)
                    .HasName("PK_EP_PRODUCT_BOND_SECONDARY_B");

                entity.ToTable("EP_PRODUCT_BOND_SECONDARY");

                entity.Property(e => e.BondSecondaryId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("BOND_SECONDARY_ID");

                entity.Property(e => e.BondPrimaryId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("BOND_PRIMARY_ID");

                entity.Property(e => e.BusinessCustomerBankAccId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("BUSINESS_CUSTOMER_BANK_ACC_ID");

                entity.Property(e => e.CloseCellDate)
                    .HasColumnType("DATE")
                    .HasColumnName("CLOSE_CELL_DATE");

                entity.Property(e => e.ContentType)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("CONTENT_TYPE");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("CREATED_BY");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("DATE")
                    .HasColumnName("CREATED_DATE")
                    .HasDefaultValueSql("SYSDATE");

                entity.Property(e => e.Deleted)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("DELETED")
                    .HasDefaultValueSql("'N'")
                    .IsFixedLength(true);

                entity.Property(e => e.IsCheck)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("IS_CHECK")
                    .HasDefaultValueSql("'N'\n   ")
                    .IsFixedLength(true);

                entity.Property(e => e.IsClose)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("IS_CLOSE")
                    .HasDefaultValueSql("'N'")
                    .IsFixedLength(true);

                entity.Property(e => e.IsShowApp)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("IS_SHOW_APP")
                    .HasDefaultValueSql("'Y'")
                    .IsFixedLength(true);

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("MODIFIED_BY");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("DATE")
                    .HasColumnName("MODIFIED_DATE");

                entity.Property(e => e.OpenCellDate)
                    .HasColumnType("DATE")
                    .HasColumnName("OPEN_CELL_DATE");

                entity.Property(e => e.OverviewContent)
                    .HasColumnType("CLOB")
                    .HasColumnName("OVERVIEW_CONTENT");

                entity.Property(e => e.OverviewImageUrl)
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("OVERVIEW_IMAGE_URL");

                entity.Property(e => e.ProductBondId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("PRODUCT_BOND_ID");

                entity.Property(e => e.Quantity)
                    .HasColumnType("NUMBER")
                    .HasColumnName("QUANTITY");

                entity.Property(e => e.Status)
                    .HasColumnType("NUMBER")
                    .HasColumnName("STATUS");

                entity.Property(e => e.TradingProviderId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("TRADING_PROVIDER_ID");
            });

            modelBuilder.Entity<EpProductBondSecondaryNews>(entity =>
            {
                entity.ToTable("EP_PRODUCT_BOND_SECONDARY_NEWS");

                entity.Property(e => e.Id)
                    .HasColumnType("NUMBER")
                    .HasColumnName("ID");

                entity.Property(e => e.BondSecondaryId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("BOND_SECONDARY_ID");

                entity.Property(e => e.Content)
                    .HasColumnType("CLOB")
                    .HasColumnName("CONTENT");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(256)
                    .IsUnicode(false)
                    .HasColumnName("CREATED_BY");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("DATE")
                    .HasColumnName("CREATED_DATE");

                entity.Property(e => e.Deleted)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("DELETED")
                    .HasDefaultValueSql("'N'")
                    .IsFixedLength(true);

                entity.Property(e => e.ImgUrl)
                    .HasMaxLength(256)
                    .IsUnicode(false)
                    .HasColumnName("IMG_URL");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(256)
                    .IsUnicode(false)
                    .HasColumnName("MODIFIED_BY");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("DATE")
                    .HasColumnName("MODIFIED_DATE");

                entity.Property(e => e.Status)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("STATUS")
                    .HasDefaultValueSql("'A'")
                    .IsFixedLength(true);

                entity.Property(e => e.Title)
                    .HasMaxLength(256)
                    .IsUnicode(false)
                    .HasColumnName("TITLE");

                entity.Property(e => e.TradingProviderId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("TRADING_PROVIDER_ID");
            });

            modelBuilder.Entity<EpPvcbPaymentCallback>(entity =>
            {
                entity.ToTable("EP_PVCB_PAYMENT_CALLBACK");

                entity.Property(e => e.Id)
                    .HasColumnType("NUMBER")
                    .HasColumnName("ID");

                entity.Property(e => e.Account)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("ACCOUNT");

                entity.Property(e => e.Amount)
                    .HasColumnType("NUMBER")
                    .HasColumnName("AMOUNT");

                entity.Property(e => e.Balance)
                    .HasColumnType("NUMBER")
                    .HasColumnName("BALANCE");

                entity.Property(e => e.ConAmount)
                    .HasColumnType("NUMBER")
                    .HasColumnName("CON_AMOUNT");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("DATE")
                    .HasColumnName("CREATED_DATE")
                    .HasDefaultValueSql("SYSDATE ");

                entity.Property(e => e.Currency)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("CURRENCY");

                entity.Property(e => e.Description)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("DESCRIPTION");

                entity.Property(e => e.FtType)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("FT_TYPE");

                entity.Property(e => e.NumberOfBeneficiary)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("NUMBER_OF_BENEFICIARY");

                entity.Property(e => e.RequestIp)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("REQUEST_IP");

                entity.Property(e => e.SenderBankId)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("SENDER_BANK_ID");

                entity.Property(e => e.Status)
                    .HasColumnType("NUMBER")
                    .HasColumnName("STATUS")
                    .HasDefaultValueSql("1 ");

                entity.Property(e => e.Token)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("TOKEN");

                entity.Property(e => e.TranDate)
                    .HasColumnType("DATE")
                    .HasColumnName("TRAN_DATE");

                entity.Property(e => e.TranId)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("TRAN_ID");

                entity.Property(e => e.TranStatus)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("TRAN_STATUS");
            });

            modelBuilder.Entity<EpReceiveContractTemplate>(entity =>
            {
                entity.ToTable("EP_RECEIVE_CONTRACT_TEMPLATE");

                entity.Property(e => e.Id)
                    .HasColumnType("NUMBER")
                    .HasColumnName("ID");

                entity.Property(e => e.BondSecondaryId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("BOND_SECONDARY_ID");

                entity.Property(e => e.Code)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("CODE");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(256)
                    .IsUnicode(false)
                    .HasColumnName("CREATED_BY");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("DATE")
                    .HasColumnName("CREATED_DATE");

                entity.Property(e => e.Deleted)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("DELETED")
                    .HasDefaultValueSql("('N')")
                    .IsFixedLength(true);

                entity.Property(e => e.FileUrl)
                    .HasMaxLength(1024)
                    .IsUnicode(false)
                    .HasColumnName("FILE_URL");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(256)
                    .IsUnicode(false)
                    .HasColumnName("MODIFIED_BY");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("DATE")
                    .HasColumnName("MODIFIED_DATE");

                entity.Property(e => e.Name)
                    .HasMaxLength(256)
                    .IsUnicode(false)
                    .HasColumnName("NAME");

                entity.Property(e => e.Status)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("STATUS")
                    .HasDefaultValueSql("('A')")
                    .IsFixedLength(true);

                entity.Property(e => e.TradingProviderId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("TRADING_PROVIDER_ID");
            });

            modelBuilder.Entity<EpReportTradingProvider>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("EP_REPORT_TRADING_PROVIDER");

                entity.Property(e => e.BusinessCustomerId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("BUSINESS_CUSTOMER_ID");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("CREATED_BY");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("DATE")
                    .HasColumnName("CREATED_DATE");

                entity.Property(e => e.Deleted)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("DELETED");

                entity.Property(e => e.Id)
                    .HasPrecision(10)
                    .HasColumnName("ID");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("MODIFIED_BY");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("DATE")
                    .HasColumnName("MODIFIED_DATE");

                entity.Property(e => e.Status)
                    .HasColumnType("NUMBER")
                    .HasColumnName("STATUS");
            });

            modelBuilder.Entity<EpTradingMsbPrefixAccount>(entity =>
            {
                entity.ToTable("EP_TRADING_MSB_PREFIX_ACCOUNT");

                entity.HasIndex(e => new { e.Id, e.TradingProviderId, e.TradingBankAccountId, e.Deleted }, "IX_TRADING_MSB_PREFIX_ACCOUNT");

                entity.Property(e => e.Id)
                    .HasPrecision(10)
                    .ValueGeneratedNever()
                    .HasColumnName("ID");

                entity.Property(e => e.AccessCode)
                    .HasMaxLength(50)
                    .HasColumnName("ACCESS_CODE");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(50)
                    .HasColumnName("CREATED_BY");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("DATE")
                    .HasColumnName("CREATED_DATE")
                    .HasDefaultValueSql("(SYSDATE)");

                entity.Property(e => e.Deleted)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("DELETED")
                    .HasDefaultValueSql("u'N' ");

                entity.Property(e => e.MId)
                    .HasMaxLength(50)
                    .HasColumnName("M_ID");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(50)
                    .HasColumnName("MODIFIED_BY");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("DATE")
                    .HasColumnName("MODIFIED_DATE");

                entity.Property(e => e.PrefixMsb)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("PREFIX_MSB");

                entity.Property(e => e.TId)
                    .HasMaxLength(50)
                    .HasColumnName("T_ID");

                entity.Property(e => e.TradingBankAccountId)
                    .HasPrecision(10)
                    .HasColumnName("TRADING_BANK_ACCOUNT_ID");

                entity.Property(e => e.TradingProviderId)
                    .HasPrecision(10)
                    .HasColumnName("TRADING_PROVIDER_ID");
            });

            modelBuilder.Entity<EpTradingProvider>(entity =>
            {
                entity.HasKey(e => e.TradingProviderId)
                    .HasName("PK_TRADING_PROVIDER");

                entity.ToTable("EP_TRADING_PROVIDER");

                entity.HasIndex(e => new { e.Deleted, e.BusinessCustomerId, e.Status }, "IX_TRADING_PROVIDER");

                entity.Property(e => e.TradingProviderId)
                    .HasPrecision(10)
                    .ValueGeneratedNever()
                    .HasColumnName("TRADING_PROVIDER_ID");

                entity.Property(e => e.AliasName)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("ALIAS_NAME");

                entity.Property(e => e.BusinessCustomerId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("BUSINESS_CUSTOMER_ID");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("CREATED_BY");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("DATE")
                    .HasColumnName("CREATED_DATE");

                entity.Property(e => e.Deleted)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("DELETED");

                entity.Property(e => e.IsDefaultBond)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("IS_DEFAULT_BOND")
                    .HasDefaultValueSql("u'N' ");

                entity.Property(e => e.IsDefaultCps)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("IS_DEFAULT_CPS")
                    .HasDefaultValueSql("u'N' ");

                entity.Property(e => e.IsDefaultGarner)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("IS_DEFAULT_GARNER")
                    .HasDefaultValueSql("u'N' ");

                entity.Property(e => e.IsDefaultInvest)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("IS_DEFAULT_INVEST")
                    .HasDefaultValueSql("u'N' ");

                entity.Property(e => e.IsIpPayment)
                    .IsRequired()
                    .HasMaxLength(1)
                    .HasColumnName("IS_IP_PAYMENT")
                    .HasDefaultValueSql("'N' ");

                entity.Property(e => e.Key)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("KEY");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("MODIFIED_BY");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("DATE")
                    .HasColumnName("MODIFIED_DATE");

                entity.Property(e => e.PartnerId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("PARTNER_ID");

                entity.Property(e => e.Secret)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("SECRET");

                entity.Property(e => e.Server)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("SERVER");

                entity.Property(e => e.StampImageUrl)
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("STAMP_IMAGE_URL");

                entity.Property(e => e.Status)
                    .HasColumnType("NUMBER")
                    .HasColumnName("STATUS");
            });

            modelBuilder.Entity<EpTradingProviderPartner>(entity =>
            {
                entity.ToTable("EP_TRADING_PROVIDER_PARTNER");

                entity.HasIndex(e => new { e.Deleted, e.PartnerId, e.TradingProviderId }, "IX_TRADING_PROVIDER_PARTNER");

                entity.Property(e => e.Id)
                    .HasColumnType("NUMBER")
                    .HasColumnName("ID");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(32)
                    .IsUnicode(false)
                    .HasColumnName("CREATED_BY");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("DATE")
                    .HasColumnName("CREATED_DATE")
                    .HasDefaultValueSql("SYSDATE");

                entity.Property(e => e.Deleted)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("DELETED")
                    .HasDefaultValueSql("'N' ");

                entity.Property(e => e.PartnerId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("PARTNER_ID");

                entity.Property(e => e.TradingProviderId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("TRADING_PROVIDER_ID");
            });

            modelBuilder.Entity<EpWhiteListIp>(entity =>
            {
                entity.ToTable("EP_WHITE_LIST_IP");

                entity.Property(e => e.Id)
                    .HasColumnType("NUMBER")
                    .HasColumnName("ID");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("CREATED_BY");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("DATE")
                    .HasColumnName("CREATED_DATE")
                    .HasDefaultValueSql("SYSDATE");

                entity.Property(e => e.Deleted)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("DELETED")
                    .HasDefaultValueSql("'N' ")
                    .IsFixedLength(true);

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("MODIFIED_BY");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("DATE")
                    .HasColumnName("MODIFIED_DATE");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("NAME");

                entity.Property(e => e.TradingProviderId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("TRADING_PROVIDER_ID");

                entity.Property(e => e.Type)
                    .HasColumnType("NUMBER")
                    .HasColumnName("TYPE");
            });

            modelBuilder.Entity<EpWhiteListIpDetail>(entity =>
            {
                entity.ToTable("EP_WHITE_LIST_IP_DETAIL");

                entity.Property(e => e.Id)
                    .HasColumnType("NUMBER")
                    .HasColumnName("ID");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("DATE")
                    .HasColumnName("CREATED_DATE")
                    .HasDefaultValueSql("SYSDATE");

                entity.Property(e => e.Deleted)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("DELETED")
                    .HasDefaultValueSql("'N' ")
                    .IsFixedLength(true);

                entity.Property(e => e.IpAddressEnd)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("IP_ADDRESS_END");

                entity.Property(e => e.IpAddressStart)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("IP_ADDRESS_START");

                entity.Property(e => e.TradingProviderId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("TRADING_PROVIDER_ID");

                entity.Property(e => e.WhiteListIpId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("WHITE_LIST_IP_ID");
            });

            modelBuilder.Entity<Group>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("GROUPS");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(32)
                    .IsUnicode(false)
                    .HasColumnName("CREATED_BY");

                entity.Property(e => e.Createdate)
                    .HasColumnType("DATE")
                    .HasColumnName("CREATEDATE");

                entity.Property(e => e.Description)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("DESCRIPTION");

                entity.Property(e => e.GroupCode)
                    .HasMaxLength(64)
                    .IsUnicode(false)
                    .HasColumnName("GROUP_CODE");

                entity.Property(e => e.GroupId)
                    .HasColumnType("NUMBER(38)")
                    .HasColumnName("GROUP_ID");

                entity.Property(e => e.Groupname)
                    .HasMaxLength(64)
                    .HasColumnName("GROUPNAME");

                entity.Property(e => e.Grptype)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("GRPTYPE");

                entity.Property(e => e.IsDeleted)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("IS_DELETED");

                entity.Property(e => e.Lastmodify)
                    .HasColumnType("DATE")
                    .HasColumnName("LASTMODIFY");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(32)
                    .IsUnicode(false)
                    .HasColumnName("MODIFIED_BY");

                entity.Property(e => e.Status)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("STATUS");

                entity.Property(e => e.Usertype)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("USERTYPE");
            });

            modelBuilder.Entity<LogError>(entity =>
            {
                entity.HasKey(e => e.Autoid)
                    .HasName("SYS_C007586");

                entity.ToTable("LOG_ERROR");

                entity.Property(e => e.Autoid)
                    .HasColumnType("NUMBER")
                    .HasColumnName("AUTOID");

                entity.Property(e => e.Deleted)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("DELETED");

                entity.Property(e => e.Errno)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("ERRNO");

                entity.Property(e => e.LogDate)
                    .HasColumnType("DATE")
                    .HasColumnName("LOG_DATE");

                entity.Property(e => e.LogTime)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("LOG_TIME");

                entity.Property(e => e.Lstupddtetme)
                    .HasColumnType("DATE")
                    .HasColumnName("LSTUPDDTETME");

                entity.Property(e => e.Lstupdusrcde)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("LSTUPDUSRCDE");

                entity.Property(e => e.Message)
                    .IsUnicode(false)
                    .HasColumnName("MESSAGE");

                entity.Property(e => e.Msgid).HasColumnName("MSGID");

                entity.Property(e => e.Target)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("TARGET");
            });

            modelBuilder.Entity<LogIsDefault>(entity =>
            {
                entity.ToTable("LOG_IS_DEFAULT");

                entity.Property(e => e.Id)
                    .HasColumnType("NUMBER")
                    .HasColumnName("ID");

                entity.Property(e => e.IdRecord)
                    .HasColumnType("NUMBER")
                    .HasColumnName("ID_RECORD");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("DATE")
                    .HasColumnName("MODIFIED_DATE")
                    .HasDefaultValueSql("SYSDATE");

                entity.Property(e => e.NewValue)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("NEW_VALUE");

                entity.Property(e => e.Notice)
                    .HasMaxLength(500)
                    .HasColumnName("NOTICE");

                entity.Property(e => e.OldValue)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("OLD_VALUE");

                entity.Property(e => e.TableName)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("TABLE_NAME");
            });

            modelBuilder.Entity<PPartnerPermission>(entity =>
            {
                entity.ToTable("P_PARTNER_PERMISSION");

                entity.Property(e => e.Id)
                    .HasColumnType("NUMBER")
                    .HasColumnName("ID");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(256)
                    .IsUnicode(false)
                    .HasColumnName("CREATED_BY");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("DATE")
                    .HasColumnName("CREATED_DATE")
                    .HasDefaultValueSql("SYSDATE");

                entity.Property(e => e.Deleted)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("DELETED")
                    .HasDefaultValueSql("'N'")
                    .IsFixedLength(true);

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("MODIFIED_BY");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("DATE")
                    .HasColumnName("MODIFIED_DATE");

                entity.Property(e => e.PartnerId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("PARTNER_ID");

                entity.Property(e => e.PermissionInWeb)
                    .HasColumnType("NUMBER")
                    .HasColumnName("PERMISSION_IN_WEB");

                entity.Property(e => e.PermissionKey)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("PERMISSION_KEY");

                entity.Property(e => e.PermissionType)
                    .HasColumnType("NUMBER")
                    .HasColumnName("PERMISSION_TYPE");
            });

            modelBuilder.Entity<PRole>(entity =>
            {
                entity.ToTable("P_ROLE");

                entity.Property(e => e.Id)
                    .HasColumnType("NUMBER")
                    .HasColumnName("ID");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(256)
                    .IsUnicode(false)
                    .HasColumnName("CREATED_BY");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("DATE")
                    .HasColumnName("CREATED_DATE")
                    .HasDefaultValueSql("SYSDATE");

                entity.Property(e => e.Deleted)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("DELETED")
                    .HasDefaultValueSql("'N'")
                    .IsFixedLength(true);

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("MODIFIED_BY");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("DATE")
                    .HasColumnName("MODIFIED_DATE");

                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("NAME");

                entity.Property(e => e.PartnerId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("PARTNER_ID");

                entity.Property(e => e.PermissionInWeb)
                    .HasColumnType("NUMBER")
                    .HasColumnName("PERMISSION_IN_WEB");

                entity.Property(e => e.RoleType)
                    .HasColumnType("NUMBER")
                    .HasColumnName("ROLE_TYPE");

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("STATUS")
                    .HasDefaultValueSql("'A' ")
                    .IsFixedLength(true);

                entity.Property(e => e.TradingProviderId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("TRADING_PROVIDER_ID");
            });

            modelBuilder.Entity<PRolePermission>(entity =>
            {
                entity.ToTable("P_ROLE_PERMISSION");

                entity.Property(e => e.Id)
                    .HasColumnType("NUMBER")
                    .HasColumnName("ID");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(256)
                    .IsUnicode(false)
                    .HasColumnName("CREATED_BY");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("DATE")
                    .HasColumnName("CREATED_DATE")
                    .HasDefaultValueSql("SYSDATE");

                entity.Property(e => e.Deleted)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("DELETED")
                    .HasDefaultValueSql("'N'")
                    .IsFixedLength(true);

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("MODIFIED_BY");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("DATE")
                    .HasColumnName("MODIFIED_DATE");

                entity.Property(e => e.PermissionKey)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("PERMISSION_KEY");

                entity.Property(e => e.PermissionType)
                    .HasColumnType("NUMBER")
                    .HasColumnName("PERMISSION_TYPE");

                entity.Property(e => e.RoleId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("ROLE_ID");
            });

            modelBuilder.Entity<PTradingProviderPermission>(entity =>
            {
                entity.ToTable("P_TRADING_PROVIDER_PERMISSION");

                entity.Property(e => e.Id)
                    .HasColumnType("NUMBER")
                    .HasColumnName("ID");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(256)
                    .IsUnicode(false)
                    .HasColumnName("CREATED_BY");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("DATE")
                    .HasColumnName("CREATED_DATE")
                    .HasDefaultValueSql("SYSDATE");

                entity.Property(e => e.PermissionInWeb)
                    .HasColumnType("NUMBER")
                    .HasColumnName("PERMISSION_IN_WEB");

                entity.Property(e => e.PermissionKey)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("PERMISSION_KEY");

                entity.Property(e => e.PermissionType)
                    .HasColumnType("NUMBER")
                    .HasColumnName("PERMISSION_TYPE");

                entity.Property(e => e.TradingProviderId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("TRADING_PROVIDER_ID");
            });

            modelBuilder.Entity<PUserRole>(entity =>
            {
                entity.ToTable("P_USER_ROLE");

                entity.Property(e => e.Id)
                    .HasColumnType("NUMBER")
                    .HasColumnName("ID");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(256)
                    .IsUnicode(false)
                    .HasColumnName("CREATED_BY");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("DATE")
                    .HasColumnName("CREATED_DATE")
                    .HasDefaultValueSql("SYSDATE");

                entity.Property(e => e.Deleted)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("DELETED")
                    .HasDefaultValueSql("'N'")
                    .IsFixedLength(true);

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("MODIFIED_BY");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("DATE")
                    .HasColumnName("MODIFIED_DATE");

                entity.Property(e => e.RoleId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("ROLE_ID");

                entity.Property(e => e.UserId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("USER_ID");
            });

            modelBuilder.Entity<Province>(entity =>
            {
                entity.HasKey(e => e.Code)
                    .HasName("PROVINCES_PKEY");

                entity.ToTable("PROVINCES");

                entity.Property(e => e.Code)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("CODE");

                entity.Property(e => e.AdministrativeRegionId)
                    .HasColumnType("NUMBER(38)")
                    .HasColumnName("ADMINISTRATIVE_REGION_ID");

                entity.Property(e => e.AdministrativeUnitId)
                    .HasColumnType("NUMBER(38)")
                    .HasColumnName("ADMINISTRATIVE_UNIT_ID");

                entity.Property(e => e.CodeName)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("CODE_NAME");

                entity.Property(e => e.FullName)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("FULL_NAME");

                entity.Property(e => e.FullNameEn)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("FULL_NAME_EN");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("NAME");

                entity.Property(e => e.NameEn)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("NAME_EN");
            });

            modelBuilder.Entity<Sysvar>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("SYSVAR");

                entity.Property(e => e.Grname)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("GRNAME");

                entity.Property(e => e.Vardesc)
                    .HasMaxLength(200)
                    .HasColumnName("VARDESC");

                entity.Property(e => e.Varname)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("VARNAME");

                entity.Property(e => e.Varvalue).HasColumnName("VARVALUE");
            });

            modelBuilder.Entity<TempTienVaoTheoNgay>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("TEMP_TIEN_VAO_THEO_NGAY");

                entity.Property(e => e.Ngay)
                    .HasColumnType("DATE")
                    .HasColumnName("NGAY");

                entity.Property(e => e.TienRa)
                    .HasColumnType("NUMBER")
                    .HasColumnName("TIEN_RA");

                entity.Property(e => e.TienVao)
                    .HasColumnType("NUMBER")
                    .HasColumnName("TIEN_VAO");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("USERS");

                entity.Property(e => e.UserId)
                    .HasColumnType("NUMBER(20)")
                    .HasColumnName("USER_ID");

                entity.Property(e => e.CaUsername)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("CA_USERNAME");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(32)
                    .IsUnicode(false)
                    .HasColumnName("CREATED_BY");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("DATE")
                    .HasColumnName("CREATED_DATE");

                entity.Property(e => e.DataSearchRole)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("DATA_SEARCH_ROLE");

                entity.Property(e => e.DeletedBy)
                    .HasMaxLength(32)
                    .IsUnicode(false)
                    .HasColumnName("DELETED_BY");

                entity.Property(e => e.DeletedDate)
                    .HasColumnType("DATE")
                    .HasColumnName("DELETED_DATE");

                entity.Property(e => e.Description)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("DESCRIPTION");

                entity.Property(e => e.DisplayName)
                    .HasMaxLength(200)
                    .HasColumnName("DISPLAY_NAME");

                entity.Property(e => e.Email)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("EMAIL");

                entity.Property(e => e.FailAttemp)
                    .HasColumnType("NUMBER")
                    .HasColumnName("FAIL_ATTEMP");

                entity.Property(e => e.InvestorId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("INVESTOR_ID");

                entity.Property(e => e.IpAddress)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("IP_ADDRESS");

                entity.Property(e => e.IsDeleted)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("IS_DELETED")
                    .HasDefaultValueSql("'N'");

                entity.Property(e => e.IsFirstTime)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("IS_FIRST_TIME")
                    .HasDefaultValueSql("'N'");

                entity.Property(e => e.IsTempPassword)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("IS_TEMP_PASSWORD")
                    .HasDefaultValueSql("'N'");

                entity.Property(e => e.IsTempPin)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("IS_TEMP_PIN")
                    .HasDefaultValueSql("'N'")
                    .IsFixedLength(true);

                entity.Property(e => e.IsVerifiedEmail)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("IS_VERIFIED_EMAIL")
                    .HasDefaultValueSql("'N'\n   ")
                    .IsFixedLength(true);

                entity.Property(e => e.LastFailedLogin)
                    .HasColumnType("DATE")
                    .HasColumnName("LAST_FAILED_LOGIN");

                entity.Property(e => e.LastLogin)
                    .HasColumnType("DATE")
                    .HasColumnName("LAST_LOGIN");

                entity.Property(e => e.LockedDate)
                    .HasColumnType("DATE")
                    .HasColumnName("LOCKED_DATE");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(32)
                    .IsUnicode(false)
                    .HasColumnName("MODIFIED_BY");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("DATE")
                    .HasColumnName("MODIFIED_DATE");

                entity.Property(e => e.PartnerId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("PARTNER_ID");

                entity.Property(e => e.Password)
                    .HasMaxLength(200)
                    .HasColumnName("PASSWORD");

                entity.Property(e => e.PinCode)
                    .HasMaxLength(256)
                    .IsUnicode(false)
                    .HasColumnName("PIN_CODE");

                entity.Property(e => e.ResetPasswordToken)
                    .HasMaxLength(256)
                    .IsUnicode(false)
                    .HasColumnName("RESET_PASSWORD_TOKEN");

                entity.Property(e => e.ResetPasswordTokenExp)
                    .HasColumnType("DATE")
                    .HasColumnName("RESET_PASSWORD_TOKEN_EXP");

                entity.Property(e => e.ResetPwRequire)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("RESET_PW_REQUIRE");

                entity.Property(e => e.SaleId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("SALE_ID");

                entity.Property(e => e.Status)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("STATUS");

                entity.Property(e => e.SubstituteType)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("SUBSTITUTE_TYPE");

                entity.Property(e => e.SupperUser)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("SUPPER_USER")
                    .HasDefaultValueSql("'N'");

                entity.Property(e => e.TradingProviderId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("TRADING_PROVIDER_ID");

                entity.Property(e => e.Username)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("USERNAME");

                entity.Property(e => e.Usertype)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("USERTYPE");

                entity.Property(e => e.VerifyEmailCode)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("VERIFY_EMAIL_CODE");
            });

            modelBuilder.Entity<UserGroup>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("USER_GROUPS");

                entity.Property(e => e.AutoId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("AUTO_ID");

                entity.Property(e => e.DeletedDate)
                    .HasColumnType("DATE")
                    .HasColumnName("DELETED_DATE");

                entity.Property(e => e.GroupId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("GROUP_ID");

                entity.Property(e => e.IsDeleted)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("IS_DELETED");

                entity.Property(e => e.Status)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("STATUS");

                entity.Property(e => e.UserId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("USER_ID");
            });

            modelBuilder.Entity<UsersChatRoom>(entity =>
            {
                entity.ToTable("USERS_CHAT_ROOM");

                entity.Property(e => e.Id)
                    .HasColumnType("NUMBER")
                    .HasColumnName("ID");

                entity.Property(e => e.AgentId)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("AGENT_ID");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("DATE")
                    .HasColumnName("CREATED_DATE")
                    .HasDefaultValueSql("SYSDATE");

                entity.Property(e => e.Deleted)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("DELETED")
                    .HasDefaultValueSql("( 'N' )")
                    .IsFixedLength(true);

                entity.Property(e => e.RoomEndDate)
                    .HasColumnType("DATE")
                    .HasColumnName("ROOM_END_DATE");

                entity.Property(e => e.RoomId)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("ROOM_ID");

                entity.Property(e => e.RoomStartDate)
                    .HasColumnType("DATE")
                    .HasColumnName("ROOM_START_DATE");

                entity.Property(e => e.RoomToken)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("ROOM_TOKEN");

                entity.Property(e => e.UserId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("USER_ID");

                entity.Property(e => e.VisitorId)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("VISITOR_ID");

                entity.Property(e => e.VisitorToken)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("VISITOR_TOKEN");
            });

            modelBuilder.Entity<UsersDevice>(entity =>
            {
                entity.ToTable("USERS_DEVICES");

                entity.Property(e => e.Id)
                    .HasColumnType("NUMBER")
                    .HasColumnName("ID");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("CREATED_BY");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("DATE")
                    .HasColumnName("CREATED_DATE")
                    .HasDefaultValueSql("SYSDATE\n   ");

                entity.Property(e => e.Deleted)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("DELETED")
                    .HasDefaultValueSql("'N'");

                entity.Property(e => e.DeviceId)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("DEVICE_ID");

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasPrecision(1)
                    .HasColumnName("STATUS")
                    .HasDefaultValueSql("1");

                entity.Property(e => e.UserId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("USER_ID");
            });

            modelBuilder.Entity<UsersFcmToken>(entity =>
            {
                entity.ToTable("USERS_FCM_TOKEN");

                entity.Property(e => e.Id)
                    .HasColumnType("NUMBER")
                    .HasColumnName("ID");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("DATE")
                    .HasColumnName("CREATED_DATE")
                    .HasDefaultValueSql("SYSDATE");

                entity.Property(e => e.Deleted)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("DELETED")
                    .HasDefaultValueSql("'N'\n   ")
                    .IsFixedLength(true);

                entity.Property(e => e.FcmToken)
                    .IsRequired()
                    .IsUnicode(false)
                    .HasColumnName("FCM_TOKEN");

                entity.Property(e => e.UserId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("USER_ID");
            });

            modelBuilder.Entity<UsersPartner>(entity =>
            {
                entity.ToTable("USERS_PARTNER");

                entity.Property(e => e.Id)
                    .HasColumnType("NUMBER")
                    .HasColumnName("ID");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(256)
                    .IsUnicode(false)
                    .HasColumnName("CREATED_BY");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("DATE")
                    .HasColumnName("CREATED_DATE")
                    .HasDefaultValueSql("SYSDATE");

                entity.Property(e => e.Deleted)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("DELETED")
                    .HasDefaultValueSql("'N'")
                    .IsFixedLength(true);

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("MODIFIED_BY");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("DATE")
                    .HasColumnName("MODIFIED_DATE");

                entity.Property(e => e.PartnerId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("PARTNER_ID");

                entity.Property(e => e.UserId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("USER_ID");
            });

            modelBuilder.Entity<UsersTradingProvider>(entity =>
            {
                entity.ToTable("USERS_TRADING_PROVIDER");

                entity.Property(e => e.Id)
                    .HasColumnType("NUMBER")
                    .HasColumnName("ID");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(256)
                    .IsUnicode(false)
                    .HasColumnName("CREATED_BY");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("DATE")
                    .HasColumnName("CREATED_DATE")
                    .HasDefaultValueSql("SYSDATE");

                entity.Property(e => e.Deleted)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("DELETED")
                    .HasDefaultValueSql("'N'")
                    .IsFixedLength(true);

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("MODIFIED_BY");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("DATE")
                    .HasColumnName("MODIFIED_DATE");

                entity.Property(e => e.TradingProviderId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("TRADING_PROVIDER_ID");

                entity.Property(e => e.UserId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("USER_ID");
            });

            modelBuilder.Entity<Ward>(entity =>
            {
                entity.HasKey(e => e.Code)
                    .HasName("WARDS_PKEY");

                entity.ToTable("WARDS");

                entity.Property(e => e.Code)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("CODE");

                entity.Property(e => e.AdministrativeUnitId)
                    .HasColumnType("NUMBER(38)")
                    .HasColumnName("ADMINISTRATIVE_UNIT_ID");

                entity.Property(e => e.CodeName)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("CODE_NAME");

                entity.Property(e => e.DistrictCode)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("DISTRICT_CODE");

                entity.Property(e => e.FullName)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("FULL_NAME");

                entity.Property(e => e.FullNameEn)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("FULL_NAME_EN");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("NAME");

                entity.Property(e => e.NameEn)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("NAME_EN");
            });

            modelBuilder.HasSequence("BOND_POLICY_DETAIL_TEMP");

            modelBuilder.HasSequence("DBOBJECTID_SEQUENCE").IncrementsBy(50);

            modelBuilder.HasSequence("SEQ_AUTH_OTP");

            modelBuilder.HasSequence("SEQ_BOND_BLOCKADE_LIBERATION");

            modelBuilder.HasSequence("SEQ_BOND_HISTORY_UPDATE");

            modelBuilder.HasSequence("SEQ_BOND_INFO_OVERVIEW_FILE");

            modelBuilder.HasSequence("SEQ_BOND_INFO_OVERVIEW_ORG");

            modelBuilder.HasSequence("SEQ_BOND_INTEREST_PAYMENT");

            modelBuilder.HasSequence("SEQ_BOND_ORDER");

            modelBuilder.HasSequence("SEQ_BOND_ORDER_CONTRACT_FILE");

            modelBuilder.HasSequence("SEQ_BOND_ORDER_PAYMENT");

            modelBuilder.HasSequence("SEQ_BOND_POLICY_DE_TEMP");

            modelBuilder.HasSequence("SEQ_BOND_POLICY_TEMP");

            modelBuilder.HasSequence("SEQ_BOND_RENEWALS_REQUEST");

            modelBuilder.HasSequence("SEQ_BOND_SECONDARY_CONTRACT");

            modelBuilder.HasSequence("SEQ_BOND_SECONDARY_NEWS");

            modelBuilder.HasSequence("SEQ_BUSI_CUS_TRADING_PROVIDER");

            modelBuilder.HasSequence("SEQ_BUSINESS_BANK_TEMP");

            modelBuilder.HasSequence("SEQ_BUSINESS_CUS_BANK");

            modelBuilder.HasSequence("SEQ_BUSINESS_CUSTOMER_PARTNER");

            modelBuilder.HasSequence("SEQ_BUSINESS_CUSTOMERS");

            modelBuilder.HasSequence("SEQ_BUSINESS_PARTNER");

            modelBuilder.HasSequence("SEQ_CIF_CODE");

            modelBuilder.HasSequence("SEQ_CONTRACT_FILE");

            modelBuilder.HasSequence("SEQ_CONTRACT_PAYMENT");

            modelBuilder.HasSequence("SEQ_CONTRACT_TEMPLATE");

            modelBuilder.HasSequence("SEQ_CONTRACT_TYPE");

            modelBuilder.HasSequence("SEQ_CORE_APPROVE");

            modelBuilder.HasSequence("SEQ_CORE_BANK");

            modelBuilder.HasSequence("SEQ_CORE_BUS_CUS_APPROVE");

            modelBuilder.HasSequence("SEQ_CORE_BUSINESS_LICENSE");

            modelBuilder.HasSequence("SEQ_CORE_COLLAB_CONTRACT_TEMP");

            modelBuilder.HasSequence("SEQ_CORE_DEPARTMENT");

            modelBuilder.HasSequence("SEQ_CORE_DEPARTMENT_SALE");

            modelBuilder.HasSequence("SEQ_CORE_HISTORY_UPDATE");

            modelBuilder.HasSequence("SEQ_CORE_PRODUCT_NEWS");

            modelBuilder.HasSequence("SEQ_CORE_SALE");

            modelBuilder.HasSequence("SEQ_CORE_SALE_COLLAB_CONTR");

            modelBuilder.HasSequence("SEQ_CORE_SALE_COLLAB_CONTRACT");

            modelBuilder.HasSequence("SEQ_CORE_SALE_REGISTER");

            modelBuilder.HasSequence("SEQ_CORE_SALE_TEMP");

            modelBuilder.HasSequence("SEQ_CORE_SALE_TRADING_PROVIDER");

            modelBuilder.HasSequence("SEQ_DEPOSIT_PROVIDERS");

            modelBuilder.HasSequence("SEQ_DISTRIBUTION_CONTRACT");

            modelBuilder.HasSequence("SEQ_EP_INV_ORDER");

            modelBuilder.HasSequence("SEQ_GROUP_BUSINESS_CUSTOMER");

            modelBuilder.HasSequence("SEQ_GUARANTEE_ASSET");

            modelBuilder.HasSequence("SEQ_GUARANTEE_FILE");

            modelBuilder.HasSequence("SEQ_INV_APPROVE");

            modelBuilder.HasSequence("SEQ_INV_BLOCKADE_LIBERATION");

            modelBuilder.HasSequence("SEQ_INV_CONTRACT_TEMPLATE");

            modelBuilder.HasSequence("SEQ_INV_DIS_TRADING_BANK_ACC");

            modelBuilder.HasSequence("SEQ_INV_DISTRI_POLICY_FILE");

            modelBuilder.HasSequence("SEQ_INV_DISTRIBUTION");

            modelBuilder.HasSequence("SEQ_INV_DISTRIBUTION_FILE");

            modelBuilder.HasSequence("SEQ_INV_DISTRIBUTION_NEWS");

            modelBuilder.HasSequence("SEQ_INV_DISTRIBUTION_VIDEO");

            modelBuilder.HasSequence("SEQ_INV_HISTORY_UPDATE");

            modelBuilder.HasSequence("SEQ_INV_INTEREST_PAYMENT");

            modelBuilder.HasSequence("SEQ_INV_INTEREST_PAYMENT_DATE");

            modelBuilder.HasSequence("SEQ_INV_JURIDICAL_FILE");

            modelBuilder.HasSequence("SEQ_INV_ORDER");

            modelBuilder.HasSequence("SEQ_INV_ORDER_CONTRACT_FILE");

            modelBuilder.HasSequence("SEQ_INV_ORDER_PAYMENT");

            modelBuilder.HasSequence("SEQ_INV_OWNER");

            modelBuilder.HasSequence("SEQ_INV_POLICY");

            modelBuilder.HasSequence("SEQ_INV_POLICY_DETAIL");

            modelBuilder.HasSequence("SEQ_INV_POLICY_DETAIL_TEMP");

            modelBuilder.HasSequence("SEQ_INV_POLICY_TEMP");

            modelBuilder.HasSequence("SEQ_INV_PROJECT");

            modelBuilder.HasSequence("SEQ_INV_PROJECT_IMAGE");

            modelBuilder.HasSequence("SEQ_INV_PROJECT_OVERVIEW_FILE");

            modelBuilder.HasSequence("SEQ_INV_PROJECT_OVERVIEW_ORG");

            modelBuilder.HasSequence("SEQ_INV_PROJECT_TYPE");

            modelBuilder.HasSequence("SEQ_INV_RECEIVE_CONTRACT_TEMP");

            modelBuilder.HasSequence("SEQ_INV_RENEWALS_REQUEST");

            modelBuilder.HasSequence("SEQ_INV_WITHDRAWAL");

            modelBuilder.HasSequence("SEQ_INVEST_CONTRACT_TEMPLATE");

            modelBuilder.HasSequence("SEQ_INVEST_CONTRACT_TEMPLATE_TEMP");

            modelBuilder.HasSequence("SEQ_INVESTOR");

            modelBuilder.HasSequence("SEQ_INVESTOR_APPROVE");

            modelBuilder.HasSequence("SEQ_INVESTOR_BANK_ACCOUNT");

            modelBuilder.HasSequence("SEQ_INVESTOR_BANK_ACCOUNT_TEMP");

            modelBuilder.HasSequence("SEQ_INVESTOR_CONTACT_ADD_TEMP");

            modelBuilder.HasSequence("SEQ_INVESTOR_CONTACT_ADDRESS");

            modelBuilder.HasSequence("SEQ_INVESTOR_CONTRACT");

            modelBuilder.HasSequence("SEQ_INVESTOR_GROUP");

            modelBuilder.HasSequence("SEQ_INVESTOR_ID_TEMP");

            modelBuilder.HasSequence("SEQ_INVESTOR_IDENTIFICATION");

            modelBuilder.HasSequence("SEQ_INVESTOR_INTEREST");

            modelBuilder.HasSequence("SEQ_INVESTOR_PROF_FILE");

            modelBuilder.HasSequence("SEQ_INVESTOR_SALE");

            modelBuilder.HasSequence("SEQ_INVESTOR_STOCK");

            modelBuilder.HasSequence("SEQ_INVESTOR_STOCK_TEMP");

            modelBuilder.HasSequence("SEQ_INVESTOR_TEMP");

            modelBuilder.HasSequence("SEQ_INVESTOR_TO_DO");

            modelBuilder.HasSequence("SEQ_INVESTOR_TRADING_PROVIDER");

            modelBuilder.HasSequence("SEQ_ISSUERS");

            modelBuilder.HasSequence("SEQ_JURIDICAL_FILE");

            modelBuilder.HasSequence("SEQ_LOG_ERROR");

            modelBuilder.HasSequence("SEQ_LOG_IS_DEFAULT");

            modelBuilder.HasSequence("SEQ_MSB_NOTIFICATION");

            modelBuilder.HasSequence("SEQ_MSB_NOTIFICATION_PAYMENT");

            modelBuilder.HasSequence("SEQ_MSB_REQUEST_PAYMENT");

            modelBuilder.HasSequence("SEQ_MSB_REQUEST_PAYMENT_DETAIL");

            modelBuilder.HasSequence("SEQ_P_PARTNER_PERMISSION");

            modelBuilder.HasSequence("SEQ_P_ROLE");

            modelBuilder.HasSequence("SEQ_P_ROLE_PERMISSION");

            modelBuilder.HasSequence("SEQ_P_TRADING_PERMISSION");

            modelBuilder.HasSequence("SEQ_P_USER_ROLE");

            modelBuilder.HasSequence("SEQ_PARTNER");

            modelBuilder.HasSequence("SEQ_POLICY_FILE");

            modelBuilder.HasSequence("SEQ_PRICE_ID");

            modelBuilder.HasSequence("SEQ_PRODUCT_BOND_DETAIL");

            modelBuilder.HasSequence("SEQ_PRODUCT_BOND_INFO");

            modelBuilder.HasSequence("SEQ_PRODUCT_BOND_INTEREST");

            modelBuilder.HasSequence("SEQ_PRODUCT_BOND_POLICY");

            modelBuilder.HasSequence("SEQ_PRODUCT_BOND_POLICY_DETAIL");

            modelBuilder.HasSequence("SEQ_PRODUCT_BOND_POLICY_TEMP");

            modelBuilder.HasSequence("SEQ_PRODUCT_BOND_PRIMARY");

            modelBuilder.HasSequence("SEQ_PRODUCT_BOND_SECONDARY");

            modelBuilder.HasSequence("SEQ_PRODUCT_BOND_TYPE");

            modelBuilder.HasSequence("SEQ_PRODUCT_CATEGORIES");

            modelBuilder.HasSequence("SEQ_PRODUCT_PERIOD");

            modelBuilder.HasSequence("SEQ_PRODUCT_POLICY");

            modelBuilder.HasSequence("SEQ_PROJECT_TRADING_PROVIDER");

            modelBuilder.HasSequence("SEQ_PVCB_PAYMENT_CALLBACK");

            modelBuilder.HasSequence("SEQ_RECEIVE_CONTRACT_TEMPLATE");

            modelBuilder.HasSequence("SEQ_TRADING_MSB_PREFIX_ACCOUNT");

            modelBuilder.HasSequence("SEQ_TRADING_PROVIDER_PARTNER");

            modelBuilder.HasSequence("SEQ_TRADING_PROVIDER_SALE");

            modelBuilder.HasSequence("SEQ_TRADING_PROVIDERS");

            modelBuilder.HasSequence("SEQ_USERS");

            modelBuilder.HasSequence("SEQ_USERS_CHAT_ROOM");

            modelBuilder.HasSequence("SEQ_USERS_DEVICES");

            modelBuilder.HasSequence("SEQ_USERS_FCM_TOKEN");

            modelBuilder.HasSequence("SEQ_USERS_PARTNER");

            modelBuilder.HasSequence("SEQ_USERS_TRADING");

            modelBuilder.HasSequence("SEQ_USERS_TRADING_PROVIDER");

            modelBuilder.HasSequence("SEQ_WHITE_LIST_IP");

            modelBuilder.HasSequence("SEQ_WHITE_LIST_IP_DETAIL");

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
