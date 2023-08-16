using EPIC.CoreEntities.DataEntities;
using EPIC.Entities.DataEntities;
using EPIC.Entities.Dto.Sale;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Core;
using EPIC.Utils.ConstantVariables.Shared;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.CoreEntities.EntityFramework
{
    public class CoreModelCreating
    {
        public static void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<InvestorIdentification>()
                .HasOne<Investor>(iden => iden.Investor).WithMany(e => e.InvestorIdentifications).HasForeignKey(e => e.InvestorId);
            modelBuilder.Entity<InvestorIdTemp>()
                .HasOne<Investor>().WithMany(e => e.InvestorIdTemps).HasForeignKey(e => e.InvestorId);
            modelBuilder.Entity<InvestorBankAccount>()
                .HasOne<Investor>().WithMany(e => e.InvestorBankAccounts).HasForeignKey(e => e.InvestorId);
            modelBuilder.Entity<InvestorBankAccountTemp>()
                .HasOne<Investor>().WithMany(e => e.InvestorBankAccountTemps).HasForeignKey(e => e.InvestorId);
            modelBuilder.Entity<InvestorContactAddress>()
                .HasOne<Investor>().WithMany(e => e.InvestorContactAddresses).HasForeignKey(e => e.InvestorId);
            modelBuilder.Entity<InvestorContactAddressTemp>()
                .HasOne<Investor>().WithMany(e => e.InvestorContactAddressTemps).HasForeignKey(e => e.InvestorId);

            modelBuilder.Entity<CifCodes>()
                .HasOne(cifcode => cifcode.Investor).WithMany().HasForeignKey(e => e.InvestorId);
            modelBuilder.Entity<CifCodes>()
                .HasOne(cifcode => cifcode.BusinessCustomer).WithMany().HasForeignKey(e => e.BusinessCustomerId);

            modelBuilder.Entity<BusinessCustomerBank>()
                .HasOne(bcb => bcb.BusinessCustomer).WithMany(e => e.BusinessCustomerBanks).HasForeignKey(e => e.BusinessCustomerId);
            modelBuilder.Entity<BusinessCustomerBank>()
                .HasOne(b => b.CoreBank).WithMany(b => b.BusinessCustomerBanks).HasForeignKey(e => e.BankId);

            modelBuilder.Entity<Sale>()
                .HasOne(e => e.Investor).WithMany().HasForeignKey(e => e.InvestorId)
                .OnDelete(DeleteBehavior.SetNull);
            modelBuilder.Entity<Sale>()
                .HasOne(e => e.BusinessCustomer).WithMany().HasForeignKey(e => e.BusinessCustomerId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<AppListTradingProviderDto>().HasNoKey();

            modelBuilder.Entity<TradingMsbPrefixAccount>(entity =>
            {
                entity.Property(e => e.CreatedDate).HasDefaultValueSql("SYSDATE");
                entity.Property(e => e.Deleted).HasDefaultValue(YesNo.NO);
            });
            modelBuilder.HasSequence(TradingMsbPrefixAccount.SEQ, DbSchemas.EPIC);
            modelBuilder.Entity<TradingMsbPrefixAccount>()
                .HasIndex(entity => new { entity.Id, entity.TradingProviderId, entity.TradingBankAccountId, entity.MId, entity.TId, entity.AccessCode, entity.Deleted})
                .HasDatabaseName("IX_TRADING_MSB_PREFIX_ACCOUNT");

            modelBuilder.Entity<InvestorToDo>(entity =>
            {
                entity.Property(e => e.Status).HasDefaultValue(InvestorTodoStatus.INIT);
            });
            modelBuilder.HasSequence(InvestorToDo.SEQ, DbSchemas.EPIC);
            modelBuilder.Entity<InvestorToDo>()
                .HasIndex(entity => new { entity.InvestorId })
                .HasDatabaseName("IX_INVESTOR_TO_DO");

            modelBuilder.Entity<TradingProviderConfig>(entity =>
            {
                entity.Property(e => e.CreatedDate).HasDefaultValueSql("SYSDATE");
                entity.Property(e => e.Deleted).HasDefaultValue(YesNo.NO);
            });
            modelBuilder.HasSequence(TradingProviderConfig.SEQ, DbSchemas.EPIC);

            modelBuilder.Entity<PartnerMsbPrefixAccount>(entity =>
            {
                entity.Property(e => e.CreatedDate).HasDefaultValueSql("SYSDATE");
                entity.Property(e => e.Deleted).HasDefaultValue(YesNo.NO);
            });
            modelBuilder.HasSequence(PartnerMsbPrefixAccount.SEQ, DbSchemas.EPIC);

            modelBuilder.Entity<InvestorRegisterLog>(entity =>
            {
                entity.Property(e => e.CreatedDate).HasDefaultValueSql("SYSDATE");
                entity.Property(e => e.Deleted).HasDefaultValue(YesNo.NO);
            });
            modelBuilder.HasSequence(InvestorRegisterLog.SEQ, DbSchemas.EPIC);

            modelBuilder.Entity<PartnerBankAccount>(entity =>
            {
                entity.Property(e => e.CreatedDate).HasDefaultValueSql("SYSDATE");
                entity.Property(e => e.Deleted).HasDefaultValue(YesNo.NO);
                entity.Property(e => e.IsDefault).HasDefaultValue(YesNo.NO);
                entity.Property(e => e.Status).HasDefaultValue(TrueOrFalseNum.TRUE);
            });
            modelBuilder.HasSequence(PartnerBankAccount.SEQ, DbSchemas.EPIC);

            modelBuilder.Entity<TradingProvider>()
                .HasOne(e => e.BusinessCustomer)
                .WithMany()
                .HasForeignKey(e => e.BusinessCustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<InvestorSearchHistory>(entity =>
            {
                entity.Property(e => e.Deleted).HasDefaultValue(YesNo.NO);
            });
            modelBuilder.HasSequence(InvestorSearchHistory.SEQ, DbSchemas.EPIC);
            modelBuilder.Entity<InvestorSearchHistory>()
                .HasIndex(entity => new { entity.InvestorId })
                .HasDatabaseName("IX_INVESTOR_TO_DO");

            modelBuilder.Entity<CallCenterConfig>();
            modelBuilder.Entity<CallCenterConfig>()
                .HasOne(e => e.TradingProvider)
                .WithMany()
                .HasForeignKey(e => e.TradingProviderId)
                .OnDelete(DeleteBehavior.SetNull);
            modelBuilder.HasSequence(CallCenterConfig.SEQ, DbSchemas.EPIC);

            modelBuilder.Entity<CollabContractTemp>(entity =>
            {
                entity.Property(e => e.CreatedDate).HasDefaultValueSql("SYSDATE");
                entity.Property(e => e.Deleted).HasDefaultValue(YesNo.NO);
                entity.Property(e => e.Status).HasDefaultValue(Status.ACTIVE);
                entity.Property(e => e.Type).HasDefaultValue(CustomerTypes.INVESTOR);
            });
            modelBuilder.HasSequence(CollabContractTemp.SEQ, DbSchemas.EPIC);


            modelBuilder.Entity<XptToken>(entity =>
            {
                entity.Property(e => e.CreatedDate).HasDefaultValueSql("SYSDATE");
            });
            modelBuilder.HasSequence(XptToken.SEQ, DbSchemas.EPIC);
            modelBuilder.HasSequence(XptTokenDataType.SEQ, DbSchemas.EPIC);
        }
    }
}
