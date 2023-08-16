using EPIC.Utils.ConstantVariables.Shared;
using EPIC.Utils;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EPIC.PaymentEntities.DataEntities;

namespace EPIC.PaymentEntities.EntityFramework
{
    public static class PaymentModelCreating
    {
        public static void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasSequence(MsbNotification.SEQ, DbSchemas.EPIC);
            modelBuilder.Entity<MsbNotification>(entity =>
            {
                entity.Property(e => e.CreatedDate)
                    .HasDefaultValueSql("SYSDATE");
            });
            modelBuilder.Entity<MsbNotification>()
                .HasIndex(entity => new { entity.TranSeq });

            modelBuilder.HasSequence(MsbNotificationPayment.SEQ, DbSchemas.EPIC);
            modelBuilder.Entity<MsbNotificationPayment>(entity =>
            {
                entity.Property(e => e.CreatedDate)
                    .HasDefaultValueSql("SYSDATE");
            });

            modelBuilder.HasSequence(MsbRequestPayment.SEQ, DbSchemas.EPIC);
            modelBuilder.Entity<MsbRequestPayment>(entity =>
            {
                entity.Property(e => e.CreatedDate)
                    .HasDefaultValueSql("SYSDATE");
            });
            modelBuilder.Entity<MsbRequestPayment>()
               .HasIndex(entity => new { entity.TradingProdiverId, entity.ProductType, entity.RequestType })
               .HasDatabaseName("IX_EP_MSB_REQUEST_PAYMENT");

            modelBuilder.HasSequence(MsbRequestPaymentDetail.SEQ, DbSchemas.EPIC);
            modelBuilder.Entity<MsbRequestPaymentDetail>()
               .HasIndex(entity => new { entity.ReferId, entity.RequestId })
               .HasDatabaseName("IX_EP_MSB_REQUEST_PAYMENT_DETAIL");
        }
    }
}
