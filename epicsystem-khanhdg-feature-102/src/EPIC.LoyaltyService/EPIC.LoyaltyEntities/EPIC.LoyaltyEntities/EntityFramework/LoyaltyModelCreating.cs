using EPIC.LoyaltyEntities.DataEntities;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Loyalty;
using EPIC.Utils.ConstantVariables.Shared;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.LoyaltyEntities.EntityFramework
{
    public interface ILoyaltyModel
    {
        DbSet<LoyVoucher> LoyVouchers { get; set; }
        DbSet<LoyVoucherInvestor> LoyVoucherInvestors { get; set; }
        DbSet<LoyHisAccumulatePoint> LoyHisAccumulatePoints { get; set; }
        DbSet<LoyAccumulatePointStatusLog> LoyAccumulatePointStatusLogs { get; set; }
        DbSet<LoyRank> LoyRanks { get; set; }
        DbSet<LoyPointInvestor> LoyPointInvestors { get; set; }
        DbSet<LoyConversionPoint> LoyConversionPoints { get; set; }
        DbSet<LoyConversionPointDetail> LoyConversionPointDetails { get; set; }
        DbSet<LoyLuckyProgram> LoyLuckyPrograms { get; set; }
        DbSet<LoyLuckyScenario> LoyLuckyScenarios { get; set; }
        DbSet<LoyLuckyScenarioDetail> LoyLuckyScenarioDetails { get; set; }
        DbSet<LoyLuckyRotationInterface> LoyLuckyRotationInterfaces { get; set; }
        DbSet<LoyHistoryUpdate> LoyHistoryUpdates { get; set; }
    }

    public class LoyaltyModelCreating
    {
        public static void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region thông tin voucher
            modelBuilder.Entity<LoyVoucher>(entity =>
            {
                entity.Property(e => e.CreatedDate).HasDefaultValueSql("SYSDATE");
                entity.Property(e => e.VoucherType).HasDefaultValue(LoyVoucherTypes.CUNG);
                entity.Property(e => e.Deleted).HasDefaultValue(YesNo.NO);
            });
            modelBuilder.HasSequence(LoyVoucher.SEQ, DbSchemas.EPIC_LOYALTY);

            modelBuilder.Entity<LoyVoucherInvestor>(entity =>
            {
                entity.Property(e => e.CreatedDate).HasDefaultValueSql("SYSDATE");
                entity.Property(e => e.Status).HasDefaultValue(LoyVoucherInvestorStatus.KICH_HOAT);
                entity.Property(e => e.Deleted).HasDefaultValue(YesNo.NO);
            });
            modelBuilder.HasSequence(LoyVoucherInvestor.SEQ, DbSchemas.EPIC_LOYALTY);

            modelBuilder.Entity<LoyHisAccumulatePoint>(entity =>
            {
                entity.Property(e => e.CreatedDate).HasDefaultValueSql("SYSDATE");
                entity.Property(e => e.PointType).HasDefaultValue(LoyPointTypes.TICH_DIEM);
                entity.Property(e => e.Deleted).HasDefaultValue(YesNo.NO);
            });
            modelBuilder.HasSequence(LoyHisAccumulatePoint.SEQ, DbSchemas.EPIC_LOYALTY);
            #endregion

            modelBuilder.Entity<LoyConversionPoint>(entity =>
            {
                entity.Property(e => e.CreatedDate).HasDefaultValueSql("SYSDATE");
                entity.Property(e => e.Status).HasDefaultValue(LoyConversionPointStatus.CREATED);
                entity.Property(e => e.Deleted).HasDefaultValue(YesNo.NO);
            });
            modelBuilder.HasSequence(LoyConversionPoint.SEQ, DbSchemas.EPIC_LOYALTY);

            modelBuilder.Entity<LoyConversionPointDetail>(entity =>
            {
                entity.Property(e => e.CreatedDate).HasDefaultValueSql("SYSDATE");
                entity.Property(e => e.Deleted).HasDefaultValue(YesNo.NO);
            });
            modelBuilder.HasSequence(LoyConversionPointDetail.SEQ, DbSchemas.EPIC_LOYALTY);

            modelBuilder.Entity<LoyConversionPointStatusLog>(entity =>
            {
                entity.Property(e => e.CreatedDate).HasDefaultValueSql("SYSDATE");
                entity.Property(e => e.Deleted).HasDefaultValue(YesNo.NO);
            });
            modelBuilder.HasSequence(LoyConversionPointStatusLog.SEQ, DbSchemas.EPIC_LOYALTY);


            modelBuilder.Entity<LoyLuckyProgram>(entity =>
            {
                entity.Property(e => e.Status).HasDefaultValue(LoyLuckyProgramStatus.KICH_HOAT);
                entity.Property(e => e.CreatedDate).HasDefaultValueSql("SYSDATE");
                entity.Property(e => e.Deleted).HasDefaultValue(YesNo.NO);
            });
            modelBuilder.HasSequence(LoyLuckyProgram.SEQ, DbSchemas.EPIC_LOYALTY);


            modelBuilder.Entity<LoyLuckyScenario>(entity =>
            {
                entity.Property(e => e.Status).HasDefaultValue(Status.ACTIVE);
                entity.Property(e => e.CreatedDate).HasDefaultValueSql("SYSDATE");
                entity.Property(e => e.Deleted).HasDefaultValue(YesNo.NO);
            });
            modelBuilder.HasSequence(LoyLuckyScenario.SEQ, DbSchemas.EPIC_LOYALTY);
            modelBuilder.Entity<LoyLuckyScenario>().HasOne(e => e.LoyLuckyProgram).WithMany(e => e.LoyLuckyScenarios).HasForeignKey(e => e.LuckyProgramId).OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<LoyLuckyScenarioDetail>(entity =>
            {
                entity.Property(e => e.CreatedDate).HasDefaultValueSql("SYSDATE");
                entity.Property(e => e.Deleted).HasDefaultValue(YesNo.NO);
            });
            modelBuilder.HasSequence(LoyLuckyScenarioDetail.SEQ, DbSchemas.EPIC_LOYALTY);
            modelBuilder.Entity<LoyLuckyScenarioDetail>().HasOne(e => e.LoyLuckyScenario).WithMany(e => e.LoyLuckyScenarioDetails).HasForeignKey(e => e.LuckyScenarioId).OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<LoyLuckyRotationInterface>(entity =>
            {
                entity.Property(e => e.CreatedDate).HasDefaultValueSql("SYSDATE");
                entity.Property(e => e.Deleted).HasDefaultValue(YesNo.NO);
            });
            modelBuilder.HasSequence(LoyLuckyRotationInterface.SEQ, DbSchemas.EPIC_LOYALTY);

            modelBuilder.Entity<LoyHistoryUpdate>(entity =>
            {
                entity.Property(e => e.CreatedDate).HasDefaultValueSql("SYSDATE");
            });
            modelBuilder.HasSequence(LoyHistoryUpdate.SEQ, DbSchemas.EPIC_LOYALTY);


            modelBuilder.Entity<LoyLuckyProgramInvestor>(entity =>
            {
                entity.Property(e => e.CreatedDate).HasDefaultValueSql("SYSDATE");
                entity.Property(e => e.Deleted).HasDefaultValue(YesNo.NO);
            });
            modelBuilder.HasSequence(LoyLuckyProgramInvestor.SEQ, DbSchemas.EPIC_LOYALTY);

            modelBuilder.Entity<LoyLuckyProgramInvestorDetail>(entity =>
            {
                entity.Property(e => e.CreatedDate).HasDefaultValueSql("SYSDATE");
                entity.Property(e => e.Deleted).HasDefaultValue(YesNo.NO);
            });
            modelBuilder.HasSequence(LoyLuckyProgramInvestorDetail.SEQ, DbSchemas.EPIC_LOYALTY);
        }
    }
}
