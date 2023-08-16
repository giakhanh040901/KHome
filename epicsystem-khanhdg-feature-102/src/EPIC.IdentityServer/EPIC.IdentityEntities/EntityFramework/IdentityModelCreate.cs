using EPIC.Entities.DataEntities;
using EPIC.IdentityEntities.DataEntities;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Shared;
using Microsoft.EntityFrameworkCore;

namespace EPIC.IdentityEntities.EntityFramework
{
    public static class IdentityModelCreating
    {
        public static void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CifCodes>()
                .HasOne(e => e.Investor)
                .WithOne()
                .HasForeignKey<CifCodes>(e => e.InvestorId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<CifCodes>()
                .HasOne(e => e.BusinessCustomer)
                .WithOne()
                .HasForeignKey<CifCodes>(e => e.BusinessCustomerId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<WhiteListIp>(entity =>
            {
                entity.Property(e => e.CreatedDate).HasDefaultValueSql("SYSDATE");
                entity.Property(e => e.Deleted).HasDefaultValue(YesNo.NO);
            });
            modelBuilder.Entity<WhiteListIpDetail>(entity =>
            {
                entity.Property(e => e.CreatedDate).HasDefaultValueSql("SYSDATE");
                entity.Property(e => e.Deleted).HasDefaultValue(YesNo.NO);
            });

            modelBuilder.Entity<UserData>()
                .HasOne(e => e.User)
                .WithMany(e => e.UserDatas)
                .HasForeignKey(e => e.UserId);
            modelBuilder.Entity<UserData>()
                .HasOne(e => e.Partner)
                .WithOne()
                .HasForeignKey<UserData>(e => e.PartnerId)
                .OnDelete(DeleteBehavior.SetNull);
            modelBuilder.Entity<UserData>()
                .HasOne(e => e.TradingProvider)
                .WithOne()
                .HasForeignKey<UserData>(e => e.TradingProviderId)
                .OnDelete(DeleteBehavior.SetNull);
            modelBuilder.HasSequence(UserData.SEQ, DbSchemas.EPIC);
        }
    }
}
