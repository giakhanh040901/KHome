using EPIC.Entities.Dto.Sale;
using EPIC.GarnerEntities.DataEntities;
using EPIC.GarnerEntities.Dto.Calendar;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Contract;
using EPIC.Utils.ConstantVariables.Garner;
using EPIC.Utils.ConstantVariables.Shared;
using EPIC.Utils.DataUtils;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.GarnerEntities.EntiyFramework
{
    public interface IGarnerModel
    {
        DbSet<GarnerProduct> GarnerProducts { get; set; }
        DbSet<GarnerProductType> GarnerProductTypes { get; set; }
        DbSet<GarnerProductOverviewFile> GarnerProductOverviewFiles { get; set; }
        DbSet<GarnerProductFile> GarnerProductFiles { get; set; }
        DbSet<GarnerProductOverviewOrg> GarnerProductOverviewOrgs { get; set; }
        DbSet<GarnerProductTradingProvider> GarnerProductTradingProviders { get; set; }
        DbSet<GarnerDistributionTradingBankAccount> GarnerDistributionTradingBankAccounts { get; set; }
        DbSet<GarnerApprove> GarnerApproves { get; set; }
        DbSet<GarnerDistribution> GarnerDistributions { get; set; }
        DbSet<GarnerHistoryUpdate> GarnerHistoryUpdates { get; set; }
        DbSet<GarnerPolicy> GarnerPolicies { get; set; }
        DbSet<GarnerPolicyDetail> GarnerPolicyDetails { get; set; }
        DbSet<GarnerOrder> GarnerOrders { get; set; }
        DbSet<GarnerCalendar> GarnerCalendars { get; set; }
        DbSet<GarnerPartnerCalendar> GarnerPartnerCalendars { get; set; }
        DbSet<GarnerOrderPayment> GarnerOrderPayments { get; set; }
        DbSet<GarnerInterestPayment> GarnerInterestPayments { get; set; }
        DbSet<GarnerInterestPaymentDetail> GarnerInterestPaymentDetails { get; set; }
        DbSet<GarnerWithdrawal> GarnerWithdrawals { get; set; }
        DbSet<GarnerWithdrawalDetail> GarnerWithdrawalDetails { get; set; }
        DbSet<GarnerBlockadeLiberation> GarnerBlockadeLiberations { get; set; }
        DbSet<GarnerReceiveContractTemp> GarnerReceiveContractTemps { get; set; }
        DbSet<GarnerRating> GarnerRatings { get; set; }
    }

    public static class GarnerModelCreating
    {
        public static void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region thông tin chung
            modelBuilder.Entity<GarnerProduct>(entity =>
            {
                entity.Property(e => e.CreatedDate).HasDefaultValueSql("SYSDATE");
                entity.Property(e => e.IsCheck).HasDefaultValue(YesNo.NO);
                entity.Property(e => e.Deleted).HasDefaultValue(YesNo.NO);
            });
            modelBuilder.HasSequence(GarnerProduct.SEQ, DbSchemas.EPIC_GARNER);
            modelBuilder.Entity<GarnerProduct>()
                .HasIndex(entity => new { entity.Code, entity.PartnerId, entity.Deleted })
                .HasDatabaseName("IX_GAN_GARNER_PRODUCT");

            modelBuilder.Entity<GarnerProductPrice>(entity =>
            {
                entity.Property(e => e.CreatedDate).HasDefaultValueSql("SYSDATE");
            });
            modelBuilder.HasSequence(GarnerProductPrice.SEQ, DbSchemas.EPIC_GARNER);
            modelBuilder.Entity<GarnerProductPrice>()
                .HasIndex(entity => new { entity.Id, entity.TradingProviderId, entity.DistributionId })
                .HasDatabaseName("IX_GAN_GARNER_PRODUCT_PRICE");

            modelBuilder.Entity<GarnerProductType>(entity =>
            {
                entity.Property(e => e.CreatedDate).HasDefaultValueSql("SYSDATE");
            });
            modelBuilder.HasSequence(GarnerProductType.SEQ, DbSchemas.EPIC_GARNER);
            modelBuilder.Entity<GarnerProductType>()
                .HasIndex(entity => new { entity.ProductId })
                .HasDatabaseName("IX_GAN_GARNER_PRODUCT_TYPE");

            modelBuilder.Entity<GarnerProductTradingProvider>(entity =>
            {
                entity.Property(e => e.HasTotalInvestmentSub).HasDefaultValue(YesNo.YES);
                entity.Property(e => e.IsProfitFromPartner).HasDefaultValue(YesNo.NO);
                entity.Property(e => e.DistributionDate).HasDefaultValueSql("SYSDATE");
                entity.Property(e => e.CreatedDate).HasDefaultValueSql("SYSDATE");
                entity.Property(e => e.Deleted).HasDefaultValue(YesNo.NO);
            });
            modelBuilder.HasSequence(GarnerProductTradingProvider.SEQ, DbSchemas.EPIC_GARNER);
            modelBuilder.Entity<GarnerProductTradingProvider>()
                .HasIndex(entity => new { entity.PartnerId, entity.ProductId, entity.TradingProviderId, entity.Deleted })
                .HasDatabaseName("IX_GAN_PRODUCT_TRADING");

            modelBuilder.Entity<GarnerProductFile>(entity =>
            {
                entity.Property(e => e.Status).HasDefaultValue(Utils.Status.ACTIVE);
                entity.Property(e => e.CreatedDate).HasDefaultValueSql("SYSDATE");
                entity.Property(e => e.Deleted).HasDefaultValue(YesNo.NO);
            });
            modelBuilder.HasSequence(GarnerProductFile.SEQ, DbSchemas.EPIC_GARNER);
            modelBuilder.Entity<GarnerProductFile>()
                .HasIndex(entity => new { entity.PartnerId, entity.ProductId, entity.Deleted })
                .HasDatabaseName("IX_GAN_PRODUCT_FILE");

            modelBuilder.Entity<GarnerApprove>(entity =>
            {
                entity.Property(e => e.CreatedDate).HasDefaultValueSql("SYSDATE");
                entity.Property(e => e.Deleted).HasDefaultValue(YesNo.NO);
            });
            modelBuilder.HasSequence(GarnerApprove.SEQ, DbSchemas.EPIC_GARNER);
            modelBuilder.Entity<GarnerApprove>()
                .HasIndex(entity => new { entity.PartnerId, entity.TradingProviderId, entity.ReferId, entity.ReferIdTemp, entity.Deleted })
                .HasDatabaseName("IX_GAN_APPROVE");

            modelBuilder.Entity<GarnerHistoryUpdate>(entity =>
            {
                entity.Property(e => e.CreatedDate).HasDefaultValueSql("SYSDATE");
            });
            modelBuilder.HasSequence(GarnerHistoryUpdate.SEQ, DbSchemas.EPIC_GARNER);
            modelBuilder.Entity<GarnerHistoryUpdate>()
                .HasIndex(entity => new { entity.RealTableId, entity.UpdateTable })
                .HasDatabaseName("IX_GAN_HISTORY_UPDATE");

            modelBuilder.Entity<GarnerHistoryUpdateDetail>();
            modelBuilder.HasSequence(GarnerHistoryUpdateDetail.SEQ, DbSchemas.EPIC_GARNER);
            #endregion

            #region bán phân phối
            modelBuilder.Entity<GarnerDistribution>(entity =>
            {
                entity.Property(e => e.IsShowApp).HasDefaultValue(YesNo.YES);
                entity.Property(e => e.IsClose).HasDefaultValue(YesNo.NO);
                entity.Property(e => e.IsCheck).HasDefaultValue(YesNo.NO);
                entity.Property(e => e.CreatedDate).HasDefaultValueSql("SYSDATE");
                entity.Property(e => e.Deleted).HasDefaultValue(YesNo.NO);
            });
            modelBuilder.HasSequence(GarnerDistribution.SEQ, DbSchemas.EPIC_GARNER);
            modelBuilder.Entity<GarnerDistribution>()
                .HasIndex(entity => new { entity.TradingProviderId, entity.ProductId, entity.Deleted })
                .HasDatabaseName("IX_GAN_DISTRIBUTION");

            modelBuilder.Entity<GarnerPolicy>(entity =>
            {
                entity.Property(e => e.GarnerType).HasDefaultValue(GarnerPolicyTypes.KHONG_CHON_KY_HAN);
                entity.Property(e => e.CalculateType).HasDefaultValue(CalculateTypes.GROSS);
                entity.Property(e => e.IsTransferAssets).HasDefaultValue(YesNo.NO);
                entity.Property(e => e.IsShowApp).HasDefaultValue(YesNo.YES);
                entity.Property(e => e.IsDefault).HasDefaultValue(YesNo.NO);
                entity.Property(e => e.IsDefaultEpic).HasDefaultValue(YesNo.NO);
                entity.Property(e => e.Status).HasDefaultValue(Utils.Status.ACTIVE);
                entity.Property(e => e.SortOrder).HasDefaultValue(0);
                entity.Property(e => e.CreatedDate).HasDefaultValueSql("SYSDATE");
                entity.Property(e => e.Deleted).HasDefaultValue(YesNo.NO);
            });
            modelBuilder.HasSequence(GarnerPolicy.SEQ, DbSchemas.EPIC_GARNER);
            modelBuilder.Entity<GarnerPolicy>()
                .HasIndex(entity => new { entity.TradingProviderId, entity.DistributionId, entity.Deleted })
                .HasDatabaseName("IX_GAN_POLICY");

            modelBuilder.Entity<GarnerPolicyDetail>(entity =>
            {
                entity.Property(e => e.Status).HasDefaultValue(Utils.Status.ACTIVE);
                entity.Property(e => e.IsShowApp).HasDefaultValue(YesNo.YES);
                entity.Property(e => e.CreatedDate).HasDefaultValueSql("SYSDATE");
                entity.Property(e => e.Deleted).HasDefaultValue(YesNo.NO);
            });
            modelBuilder.HasSequence(GarnerPolicyDetail.SEQ, DbSchemas.EPIC_GARNER);
            modelBuilder.Entity<GarnerPolicyDetail>()
                .HasIndex(entity => new { entity.TradingProviderId, entity.DistributionId, entity.PolicyId, entity.Deleted })
                .HasDatabaseName("IX_GAN_POLICY_DETAIL");

            modelBuilder.Entity<GarnerProductOverviewFile>(entity =>
            {
                entity.Property(e => e.Status).HasDefaultValue(Utils.Status.ACTIVE);
                entity.Property(e => e.CreatedDate).HasDefaultValueSql("SYSDATE");
                entity.Property(e => e.Deleted).HasDefaultValue(YesNo.NO);
            });
            modelBuilder.HasSequence(GarnerProductOverviewFile.SEQ, DbSchemas.EPIC_GARNER);
            modelBuilder.Entity<GarnerProductOverviewFile>()
                .HasIndex(entity => new { entity.TradingProviderId, entity.DistributionId, entity.Deleted })
                .HasDatabaseName("IX_GAN_PRODUCT_OVERVIEW_FILE");

            modelBuilder.Entity<GarnerProductOverviewOrg>(entity =>
            {
                entity.Property(e => e.Status).HasDefaultValue(Utils.Status.ACTIVE);
                entity.Property(e => e.CreatedDate).HasDefaultValueSql("SYSDATE");
                entity.Property(e => e.Deleted).HasDefaultValue(YesNo.NO);
            });
            modelBuilder.HasSequence(GarnerProductOverviewOrg.SEQ, DbSchemas.EPIC_GARNER);
            modelBuilder.Entity<GarnerProductOverviewOrg>()
                .HasIndex(entity => new { entity.TradingProviderId, entity.DistributionId, entity.Deleted })
                .HasDatabaseName("IX_GAN_PRODUCT_OVERVIEW_ORG");

            modelBuilder.Entity<GarnerDistributionTradingBankAccount>(entity =>
            {
                entity.Property(e => e.Status).HasDefaultValue(Utils.Status.ACTIVE);
                entity.Property(e => e.Type).HasDefaultValue(DistributionTradingBankAccountTypes.THU);
                entity.Property(e => e.CreatedDate).HasDefaultValueSql("SYSDATE");
                entity.Property(e => e.Deleted).HasDefaultValue(YesNo.NO);
                entity.Property(e => e.IsAuto).HasDefaultValue(YesNo.NO);
            });
            modelBuilder.HasSequence(GarnerDistributionTradingBankAccount.SEQ, DbSchemas.EPIC_GARNER);
            modelBuilder.Entity<GarnerDistributionTradingBankAccount>()
                .HasIndex(entity => new { entity.DistributionId, entity.BusinessCustomerBankAccId, entity.Deleted })
                .HasDatabaseName("IX_GAN_DISTRIBUTION_TRADING_BANK_ACC");

            modelBuilder.Entity<GarnerConfigContractCode>(entity =>
            {
                entity.Property(e => e.Status).HasDefaultValue(Utils.Status.ACTIVE);
                entity.Property(e => e.CreatedDate).HasDefaultValueSql("SYSDATE");
                entity.Property(e => e.Deleted).HasDefaultValue(YesNo.NO);
            });
            modelBuilder.HasSequence(GarnerConfigContractCode.SEQ, DbSchemas.EPIC_GARNER);
            modelBuilder.Entity<GarnerConfigContractCode>()
                .HasIndex(entity => new { entity.TradingProviderId, entity.Deleted })
                .HasDatabaseName("IX_GAN_CONFIG_CONTRACT_CODE");

            modelBuilder.Entity<GarnerConfigContractCodeDetail>();
            modelBuilder.HasSequence(GarnerConfigContractCodeDetail.SEQ, DbSchemas.EPIC_GARNER);
            modelBuilder.Entity<GarnerConfigContractCodeDetail>()
                .HasIndex(entity => new { entity.ConfigContractCodeId })
                .HasDatabaseName("IX_GAN_DISTRIBUTION_CONFIG_CONTRACT_CODE_DETAIL");
            #endregion

            #region sổ lệnh
            modelBuilder.Entity<GarnerOrder>(entity =>
            {
                entity.Property(e => e.BuyDate).HasDefaultValueSql("SYSDATE");
                entity.Property(e => e.Status).HasDefaultValue(OrderStatus.KHOI_TAO);
                entity.Property(e => e.CreatedDate).HasDefaultValueSql("SYSDATE");
                entity.Property(e => e.Deleted).HasDefaultValue(YesNo.NO);
            });
            modelBuilder.HasSequence(GarnerOrder.SEQ, DbSchemas.EPIC_GARNER);
            modelBuilder.Entity<GarnerOrder>()
                .HasIndex(entity => new { entity.TradingProviderId, entity.Status, entity.Deleted, entity.CifCode, entity.ProductId, entity.DistributionId, entity.PolicyId, entity.PolicyDetailId, entity.SaleReferralCode })
                .HasDatabaseName("IX_GAN_ORDER");

            modelBuilder.Entity<GarnerOrderPayment>(entity =>
            {
                entity.Property(e => e.Status).HasDefaultValue(OrderPaymentStatus.NHAP);
                entity.Property(e => e.CreatedDate).HasDefaultValueSql("SYSDATE");
                entity.Property(e => e.Deleted).HasDefaultValue(YesNo.NO);
            });
            modelBuilder.HasSequence(GarnerOrderPayment.SEQ, DbSchemas.EPIC_GARNER);
            modelBuilder.Entity<GarnerOrderPayment>()
                .HasIndex(entity => new { entity.TradingProviderId, entity.OrderId, entity.TradingBankAccId, entity.Deleted })
                .HasDatabaseName("IX_GAN_ORDER_PAYMENT");

            modelBuilder.Entity<GarnerOrder>()
               .HasOne(e => e.CifCodes).WithMany().HasPrincipalKey(e => e.CifCode).HasForeignKey(e => e.CifCode);

            modelBuilder.Entity<GarnerOrder>()
               .HasOne(order => order.Product).WithMany().HasForeignKey(order => order.ProductId);

            modelBuilder.Entity<GarnerOrder>()
               .HasOne(order => order.Distribution).WithMany().HasForeignKey(order => order.DistributionId);

            modelBuilder.Entity<GarnerOrder>()
               .HasOne(order => order.Policy).WithMany().HasForeignKey(order => order.PolicyId);

            modelBuilder.Entity<GarnerOrder>()
               .HasOne(order => order.PolicyDetail).WithMany().HasForeignKey(order => order.PolicyDetailId); 
            modelBuilder.Entity<GarnerOrder>()
               .HasOne(order => order.InvestorIdentification).WithMany().HasForeignKey(order => order.InvestorIdenId);
            #endregion

            #region calendar
            modelBuilder.Entity<Test>(x =>
            {
                x.HasNoKey();
            });

            modelBuilder.Entity<GarnerCalendar>(entity =>
            {
                entity.HasNoKey();
            });
            modelBuilder.Entity<GarnerCalendar>()
                .HasIndex(entity => new { entity.TradingProviderId, entity.WorkingDate, entity.IsDayOff })
                .HasDatabaseName("IX_GAN_CALENDAR");

            modelBuilder.Entity<GarnerPartnerCalendar>()
                .HasIndex(entity => new { entity.WorkingDate, entity.WorkingYear, entity.PartnerId })
                .HasDatabaseName("IX_GAN_CALENDAR_PARTNER");
            modelBuilder.Entity<GarnerPartnerCalendar>().HasNoKey();
            modelBuilder.Entity<GarnerPartnerCalendar>().HasNoKey();
            #endregion

            #region chính sách mẫu, kỳ hạn mẫu
            modelBuilder.Entity<GarnerPolicyTemp>(entity =>
            {
                entity.Property(e => e.GarnerType).HasDefaultValue(GarnerPolicyTypes.KHONG_CHON_KY_HAN);
                entity.Property(e => e.CalculateType).HasDefaultValue(CalculateTypes.GROSS);
                entity.Property(e => e.IsTransferAssets).HasDefaultValue(YesNo.NO);
                entity.Property(e => e.Status).HasDefaultValue(Utils.Status.ACTIVE);
                entity.Property(e => e.CreatedDate).HasDefaultValueSql("SYSDATE");
                entity.Property(e => e.SortOrder).HasDefaultValue(0);
                entity.Property(e => e.Deleted).HasDefaultValue(YesNo.NO);
            });
            modelBuilder.HasSequence(GarnerPolicyTemp.SEQ, DbSchemas.EPIC_GARNER);
            modelBuilder.Entity<GarnerPolicyTemp>()
                .HasIndex(entity => new { entity.TradingProviderId, entity.Code, entity.Deleted })
                .HasDatabaseName("IX_GAN_POLICY_TEMP");

            modelBuilder.Entity<GarnerPolicyDetailTemp>(entity =>
            {
                entity.Property(e => e.Status).HasDefaultValue(Utils.Status.ACTIVE);
                entity.Property(e => e.InterestPeriodType).HasDefaultValue(InterestPeriodTypes.DAY);
                entity.Property(e => e.CreatedDate).HasDefaultValueSql("SYSDATE");
                entity.Property(e => e.Deleted).HasDefaultValue(YesNo.NO);
            });
            modelBuilder.HasSequence(GarnerPolicyDetailTemp.SEQ, DbSchemas.EPIC_GARNER);
            modelBuilder.Entity<GarnerPolicyDetailTemp>()
                .HasIndex(entity => new { entity.TradingProviderId, entity.PolicyTempId, entity.Deleted })
                .HasDatabaseName("IX_GAN_POLICY_DETAIL_TEMP");


            #endregion

            #region chi trả định kỳ, rút tiền
            modelBuilder.Entity<GarnerInterestPayment>(entity =>
            {
                entity.Property(e => e.Status).HasDefaultValue(InterestPaymentStatus.DA_LAP_CHUA_CHI_TRA);
                entity.Property(e => e.CreatedDate).HasDefaultValueSql("SYSDATE");
                entity.Property(e => e.Deleted).HasDefaultValue(YesNo.NO);
            });
            modelBuilder.HasSequence(GarnerInterestPayment.SEQ, DbSchemas.EPIC_GARNER);
            modelBuilder.Entity<GarnerInterestPayment>()
                .HasIndex(entity => new { entity.TradingProviderId, entity.PolicyId, entity.CifCode, entity.Status, entity.Deleted })
                .HasDatabaseName("IX_GAN_INTEREST_PAYMENT");

            modelBuilder.Entity<GarnerInterestPaymentDetail>();
            modelBuilder.HasSequence(GarnerInterestPaymentDetail.SEQ, DbSchemas.EPIC_GARNER);
            modelBuilder.Entity<GarnerInterestPaymentDetail>()
                .HasIndex(entity => new { entity.InterestPaymentId, entity.OrderId })
                .HasDatabaseName("IX_GAN_INTEREST_PAYMENT_DETAIL");

            modelBuilder.Entity<GarnerWithdrawal>(entity =>
            {
                entity.Property(e => e.CreatedDate).HasDefaultValueSql("SYSDATE");
                entity.Property(e => e.Deleted).HasDefaultValue(YesNo.NO);
            });
            modelBuilder.HasSequence(GarnerWithdrawal.SEQ, DbSchemas.EPIC_GARNER);
            modelBuilder.Entity<GarnerWithdrawal>()
                .HasIndex(entity => new { entity.TradingProviderId, entity.DistributionId, entity.PolicyId, entity.CifCode, entity.Deleted })
                .HasDatabaseName("IX_GAN_WITHDRAWAL");

            modelBuilder.Entity<GarnerWithdrawalDetail>();
            modelBuilder.HasSequence(GarnerWithdrawalDetail.SEQ, DbSchemas.EPIC_GARNER);
            modelBuilder.Entity<GarnerWithdrawalDetail>()
                .HasIndex(entity => new { entity.WithdrawalId, entity.OrderId })
                .HasDatabaseName("IX_GAN_WITHDRAWAL_DETAIL");
            #endregion

            #region Mẫu hợp đồng, mẫu hợp đồng mẫu
            modelBuilder.Entity<GarnerContractTemplate>(entity =>
            {
                entity.Property(e => e.DisplayType).HasDefaultValue(DisplayType.TRUOC_KHI_DUYET);
                entity.Property(e => e.Status).HasDefaultValue(Status.ACTIVE);
                entity.Property(e => e.CreatedDate).HasDefaultValueSql("SYSDATE");
                entity.Property(e => e.Deleted).HasDefaultValue(YesNo.NO);
                entity.Property(e => e.ContractSource).HasDefaultValue(ContractSources.ONLINE);
            });
            modelBuilder.HasSequence(GarnerContractTemplate.SEQ, DbSchemas.EPIC_GARNER);
            modelBuilder.Entity<GarnerContractTemplate>()
                .HasIndex(entity => new { entity.TradingProviderId, entity.Deleted })
                .HasDatabaseName("IX_GAN_CONTRACT_TEMPLATE");

            modelBuilder.Entity<GarnerContractTemplateTemp>(entity =>
            {
                entity.Property(e => e.ContractType).HasDefaultValue(ContractTypes.DAT_LENH);
                entity.Property(e => e.Status).HasDefaultValue(Status.ACTIVE);
                entity.Property(e => e.CreatedDate).HasDefaultValueSql("SYSDATE");
                entity.Property(e => e.Deleted).HasDefaultValue(YesNo.NO);
                entity.Property(e => e.ContractSource).HasDefaultValue(ContractSources.ONLINE);
            });
            modelBuilder.HasSequence(GarnerContractTemplateTemp.SEQ, DbSchemas.EPIC_GARNER);
            modelBuilder.Entity<GarnerContractTemplateTemp>()
                .HasIndex(entity => new { entity.TradingProviderId, entity.Deleted })
                .HasDatabaseName("IX_GAN_CONTRACT_TEMPLATE_TEMP");
            #endregion

            #region Hợp đồng khách hàng (Order Contract File)
            modelBuilder.Entity<GarnerOrderContractFile>(entity =>
            {
                entity.Property(e => e.IsSign).HasDefaultValue(YesNo.NO);
                entity.Property(e => e.CreatedDate).HasDefaultValueSql("SYSDATE");
                entity.Property(e => e.Deleted).HasDefaultValue(YesNo.NO);
            });
            modelBuilder.HasSequence(GarnerOrderContractFile.SEQ, DbSchemas.EPIC_GARNER);
            modelBuilder.Entity<GarnerOrderContractFile>()
                .HasIndex(entity => new { entity.TradingProviderId, entity.OrderId, entity.ContractTempId, entity.Deleted })
                .HasDatabaseName("IX_GAN_ORDER_CONTRACT_FILE");

            modelBuilder.Entity<GarnerOrderContractFile>()
                .HasOne(orderContractFile => orderContractFile.Order).WithMany(order => order.OrderContracFiles).HasForeignKey(orderContractFile => orderContractFile.OrderId);
            #endregion

            #region Phong toả giải toả
            modelBuilder.Entity<GarnerBlockadeLiberation>(entity =>
            {
                entity.Property(e => e.Status).HasDefaultValue(BlockadeLiberationStatus.PHONG_TOA);
                entity.Property(e => e.BlockadeTime).HasDefaultValueSql("SYSDATE");
            });
            modelBuilder.HasSequence(GarnerBlockadeLiberation.SEQ, DbSchemas.EPIC_GARNER);
            modelBuilder.Entity<GarnerBlockadeLiberation>()
                .HasIndex(entity => new { entity.TradingProviderId, entity.OrderId, entity.Type})
                .HasDatabaseName("IX_GAN_ORDER_BLOCKADE_LIBERATION");
            #endregion

            #region Giao nhận hợp đồng
            modelBuilder.Entity<GarnerReceiveContractTemp>(entity =>
            {
                entity.Property(e => e.Status).HasDefaultValue(InvestorStatus.DEACTIVE);
                entity.Property(e => e.Deleted).HasDefaultValue(YesNo.NO);
            });
            modelBuilder.HasSequence(GarnerReceiveContractTemp.SEQ, DbSchemas.EPIC_GARNER);
            modelBuilder.Entity<GarnerReceiveContractTemp>()
                .HasIndex(entity => new { entity.TradingProviderId, entity.DistributionId })
                .HasDatabaseName("IX_GAN_RECEIVE_CONTRACT_TEMP");
            #endregion

            #region Đánh giá
            modelBuilder.Entity<GarnerRating>(entity =>
            {
            });
            modelBuilder.HasSequence(GarnerRating.SEQ, DbSchemas.EPIC_GARNER);
            modelBuilder.Entity<GarnerRating>()
                .HasIndex(entity => new { entity.InvestorId, entity.OrderId })
                .HasDatabaseName("IX_GAN_RATING");
            #endregion
        }
    }
}
