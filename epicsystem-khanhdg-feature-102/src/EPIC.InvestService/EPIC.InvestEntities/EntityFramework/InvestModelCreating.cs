using EPIC.Utils.ConstantVariables.Shared;
using EPIC.Utils;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EPIC.GarnerEntities.DataEntities;
using EPIC.InvestEntities.DataEntities;
using EPIC.Utils.ConstantVariables.Contract;

namespace EPIC.InvestEntities.EntityFramework
{
    public interface IInvestModel
    {
        DbSet<InvestConfigContractCode> InvestConfigContractCodes { get; set; }
        DbSet<InvestConfigContractCodeDetail> InvestConfigContractCodeDetails { get; set; }
        DbSet<ReceiveContractTemplate> ReceiveContractTemplates { get; set; }
        DbSet<OrderContractFile> InvestOrderContractFile { get; set; }
        DbSet<InvestProjectInformationShareDetail> InvestProjectInformationShareDetails { get; set; }
        DbSet<InvestProjectInformationShare> InvestProjectInformationShares { get; set; }
    }

    public class InvestModelCreating
    {
        public static void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<InvOrder>()
                .HasOne(e => e.CifCodes).WithMany().HasPrincipalKey(e => e.CifCode).HasForeignKey(e => e.CifCode);
            modelBuilder.Entity<InvOrder>()
                .HasOne(e => e.TradingProvider).WithMany().HasForeignKey(e => e.TradingProviderId);
            modelBuilder.Entity<InvOrder>()
                .HasOne(e => e.Department).WithMany().HasForeignKey(e => e.DepartmentId)
                .OnDelete(DeleteBehavior.SetNull);
            modelBuilder.Entity<InvOrder>()
                .HasOne(e => e.Project).WithMany().HasForeignKey(e => e.ProjectId);
            modelBuilder.Entity<InvOrder>()
                .HasOne(e => e.Distribution).WithMany(distribution => distribution.Orders).HasForeignKey(e => e.DistributionId);
            modelBuilder.Entity<InvOrder>()
                .HasOne(e => e.Policy).WithMany().HasForeignKey(e => e.PolicyId);
            modelBuilder.Entity<InvOrder>()
                .HasOne(e => e.PolicyDetail).WithMany().HasForeignKey(e => e.PolicyDetailId);
            modelBuilder.Entity<InvOrder>()
                .HasOne(e => e.BusinessCustomerBankAcc).WithMany().HasForeignKey(e => e.BusinessCustomerBankAccId)
                .OnDelete(DeleteBehavior.SetNull);
            modelBuilder.Entity<InvOrder>()
                .HasOne(e => e.InvestorIdentification).WithMany().HasForeignKey(e => e.InvestorIdenId)
                .OnDelete(DeleteBehavior.SetNull);
            modelBuilder.Entity<InvOrder>()
                .HasOne(e => e.InvestorContactAddress).WithMany().HasForeignKey(e => e.ContractAddressId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<InvestConfigContractCode>(entity =>
            {
                entity.Property(e => e.CreatedDate).HasDefaultValueSql("SYSDATE");
                entity.Property(e => e.Deleted).HasDefaultValue(YesNo.NO);
            });
            modelBuilder.HasSequence(InvestConfigContractCode.SEQ, DbSchemas.EPIC);
            modelBuilder.Entity<InvestConfigContractCode>()
                .HasIndex(entity => new { entity.TradingProviderId, entity.Deleted })
                .HasDatabaseName("IX_INV_CONFIG_CONTRACT_CODE");

            modelBuilder.Entity<InvestConfigContractCodeDetail>();
            modelBuilder.HasSequence(InvestConfigContractCodeDetail.SEQ, DbSchemas.EPIC);
            modelBuilder.Entity<InvestConfigContractCodeDetail>()
                .HasIndex(entity => new { entity.ConfigContractCodeId, entity.Key })
                .HasDatabaseName("IX_INV_CONFIG_CONTRACT_CODE_DETAIL");

            #region ContractTemplate
            modelBuilder.Entity<InvestContractTemplate>(entity =>
            {
                entity.Property(e => e.DisplayType).HasDefaultValue(DisplayType.TRUOC_KHI_DUYET);
                entity.Property(e => e.Status).HasDefaultValue(Status.ACTIVE);
                entity.Property(e => e.CreatedDate).HasDefaultValueSql("SYSDATE");
                entity.Property(e => e.Deleted).HasDefaultValue(YesNo.NO);
                entity.Property(e => e.ContractSource).HasDefaultValue(ContractSources.ONLINE);
            });
            modelBuilder.HasSequence(InvestContractTemplate.SEQ, DbSchemas.EPIC);
            modelBuilder.Entity<InvestContractTemplate>()
                .HasIndex(entity => new { entity.TradingProviderId, entity.Deleted })
                .HasDatabaseName("IX_INV_CONTRACT_TEMPLATE");

            modelBuilder.Entity<InvestContractTemplateTemp>(entity =>
            {
                entity.Property(e => e.ContractType).HasDefaultValue(ContractTypes.DAT_LENH);
                entity.Property(e => e.Status).HasDefaultValue(Status.ACTIVE);
                entity.Property(e => e.CreatedDate).HasDefaultValueSql("SYSDATE");
                entity.Property(e => e.Deleted).HasDefaultValue(YesNo.NO);
                entity.Property(e => e.ContractSource).HasDefaultValue(ContractSources.ONLINE);
            });
            modelBuilder.HasSequence(InvestContractTemplateTemp.SEQ, DbSchemas.EPIC);
            modelBuilder.Entity<InvestContractTemplateTemp>()
                .HasIndex(entity => new { entity.TradingProviderId, entity.Deleted })
                .HasDatabaseName("IX_INV_CONTRACT_TEMPLATE_TEMP");
            #endregion

            #region Giao nhận hợp đồng
            modelBuilder.Entity<ReceiveContractTemplate>(entity =>
            {
                entity.Property(e => e.Status).HasDefaultValue(ContractTemplateStatus.DEACTIVE);
                entity.Property(e => e.Deleted).HasDefaultValue(YesNo.NO);
            });
            modelBuilder.Entity<ReceiveContractTemplate>()
                .HasIndex(entity => new { entity.TradingProviderId, entity.Deleted })
                .HasDatabaseName("IX_INV_RECEIVE_CONTRACT_TEMP");
            #endregion

            #region Đánh giá
            modelBuilder.Entity<InvestRating>(entity =>
            {
                entity.Property(e => e.CreatedDate).HasDefaultValueSql("SYSDATE");
                entity.Property(e => e.Deleted).HasDefaultValue(YesNo.NO);
            });

            modelBuilder.HasSequence(InvestRating.SEQ, DbSchemas.EPIC);
            modelBuilder.Entity<InvestRating>()
                .HasOne<InvOrder>().WithMany().HasForeignKey(p => p.OrderId);
            #endregion

            #region Owner
            modelBuilder.Entity<Owner>()
                .HasOne(owner => owner.BusinessCustomer)
                .WithMany()
                .HasForeignKey(owner => owner.BusinessCustomerId);
            #endregion

            #region Project
            modelBuilder.Entity<Project>()
                .HasOne(project => project.Owner)
                .WithMany(owner => owner.Projects)
                .HasForeignKey(project => project.OwnerId);

            modelBuilder.Entity<Project>()
                .HasOne(project => project.GeneralContractor)
                .WithMany(generalContractor => generalContractor.Projects)
                .HasForeignKey(project => project.GeneralContractorId);
            #endregion

            #region Distribution
            modelBuilder.Entity<Distribution>()
                .HasOne(distribution => distribution.Project)
                .WithMany(project => project.Distributions)
                .HasForeignKey(distribution => distribution.ProjectId);

            modelBuilder.Entity<Distribution>()
                .HasOne(e => e.TradingProvider)
                .WithMany()
                .HasForeignKey(e => e.TradingProviderId);
            #endregion

            #region ProjectTradingProvider
            modelBuilder.Entity<ProjectTradingProvider>()
                .HasOne(projectTradingProvider => projectTradingProvider.Project)
                .WithMany(project => project.ProjectTradingProviders)
                .HasForeignKey(projectTradingProvider => projectTradingProvider.ProjectId);
            #endregion

            #region Policy & PolicyDetail
            modelBuilder.Entity<Policy>()
                .HasOne(policy => policy.Distribution)
                .WithMany(distribution => distribution.Policies)
                .HasForeignKey(policy => policy.DistributionId);

            modelBuilder.Entity<PolicyDetail>()
               .HasOne(policyDetail => policyDetail.Distribution)
               .WithMany(distribution => distribution.PolicyDetails)
               .HasForeignKey(policy => policy.DistributionId);

            modelBuilder.Entity<PolicyDetail>()
               .HasOne(policyDetail => policyDetail.Policy)
               .WithMany(policy => policy.PolicyDetails)
               .HasForeignKey(policyDetail => policyDetail.PolicyId);
            #endregion

            #region withdrawal
            modelBuilder.Entity<Withdrawal>()
                .HasOne(withdrawal => withdrawal.Order).WithMany().HasForeignKey(e => e.OrderId)
                .OnDelete(DeleteBehavior.SetNull);
            modelBuilder.Entity<Withdrawal>()
                .HasOne(withdrawal => withdrawal.PolicyDetail).WithMany().HasForeignKey(e => e.PolicyDetailId);
            modelBuilder.Entity<Withdrawal>()
               .HasOne(e => e.CifCodes).WithMany().HasPrincipalKey(e => e.CifCode).HasForeignKey(e => e.CifCode);
            #endregion

            #region InterestPayment
            modelBuilder.Entity<InvestInterestPayment>()
                .HasOne(e => e.Order).WithMany().HasForeignKey(e => e.OrderId)
                .OnDelete(DeleteBehavior.SetNull);
            modelBuilder.Entity<InvestInterestPayment>()
                .HasOne(e => e.PolicyDetail).WithMany().HasForeignKey(e => e.PolicyDetailId);
            modelBuilder.Entity<InvestInterestPayment>()
               .HasOne(e => e.CifCodes).WithMany().HasPrincipalKey(e => e.CifCode).HasForeignKey(e => e.CifCode);

            #endregion
            #region ProjectInformationShare, ProjectInformationShareDetail
            modelBuilder.HasSequence(InvestProjectInformationShare.SEQ, DbSchemas.EPIC);
            modelBuilder.Entity<InvestProjectInformationShare>(entity =>
            {
                entity.Property(e => e.Status).HasDefaultValue(Status.TAM);
                entity.Property(e => e.CreatedDate).HasDefaultValueSql("SYSDATE");
                entity.Property(e => e.Deleted).HasDefaultValue(YesNo.NO);
            });
            modelBuilder.HasSequence(InvestProjectInformationShareDetail.SEQ, DbSchemas.EPIC);
            modelBuilder.Entity<InvestProjectInformationShareDetail>(entity =>
            {
                entity.Property(e => e.CreatedDate).HasDefaultValueSql("SYSDATE");
                entity.Property(e => e.Deleted).HasDefaultValue(YesNo.NO);
            });

            modelBuilder.Entity<InvestProjectInformationShare>()
                        .HasOne(project => project.Project)
                        .WithMany(projectshare => projectshare.ProjectInformationShares)
                        .HasForeignKey(e => e.ProjectId)
                        .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<InvestProjectInformationShareDetail>()
                        .HasOne(projectshareDetail => projectshareDetail.ProjectInformationShare)
                        .WithMany(projectshare => projectshare.ProjectInformationShareDetails)
                        .HasForeignKey(projectshare => projectshare.ProjectShareId)
                        .OnDelete(DeleteBehavior.SetNull);
            #endregion
            #region PolicyTemp
            modelBuilder.Entity<PolicyDetailTemp>()
                .HasOne(pt => pt.PolicyTemp).WithMany(p => p.PolicyDetailTemps).HasForeignKey(e => e.PolicyTempId);
            #endregion

            #region General Contractor
            modelBuilder.Entity<GeneralContractor>()
                .HasOne(gc => gc.BusinessCustomer).WithMany().HasForeignKey(e => e.BusinessCustomerId);
            #endregion

            #region BlockadeLibertion
            modelBuilder.Entity<BlockadeLiberation>()
                .HasOne(bl => bl.Order).WithMany().HasForeignKey(e => e.OrderId);
            #endregion

            modelBuilder.Entity<InvRenewalsRequest>()
                .HasOne(req => req.Order).WithMany().HasForeignKey(req => req.OrderId);

            modelBuilder.Entity<Calendar>(entity =>
            {
                entity.HasNoKey();
            });

            modelBuilder.Entity<OrderContractFile>(entity =>
            {
                entity.Property(e => e.CreatedDate).HasDefaultValueSql("SYSDATE");
                entity.Property(e => e.Deleted).HasDefaultValue(YesNo.NO);
                entity.Property(e => e.IsSign).HasDefaultValue(YesNo.NO);
                entity.Property(e => e.PageSign).HasDefaultValue(1);
            });
        }
    }
}