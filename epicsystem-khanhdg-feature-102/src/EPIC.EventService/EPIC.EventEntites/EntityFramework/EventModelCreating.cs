using EPIC.EventEntites.Entites;
using EPIC.Utils.ConstantVariables.Shared;
using EPIC.Utils;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EPIC.Utils.ConstantVariables.Event;
using EPIC.Utils.ConstantVariables.Contract;
using EPIC.Utils.ConstantVariables.Garner;

namespace EPIC.EventEntites.EntityFramework
{
    public interface IEventModel
    {
        DbSet<EvtEvent> EvtEvents { get; set; }
        DbSet<EvtEventDetail> EvtEventDetails { get; set; }
        DbSet<EvtOrder> EvtOrders { get; set; }
        DbSet<EvtOrderDetail> EvtOrderDetails { get; set; }
        DbSet<EvtOrderTicketDetail> EvtOrderTicketDetails { get; set; }
        DbSet<EvtTicket> EvtTickets { get; set; }
        //DbSet<EvtEventMedia> EvtEventMedias { get; set; }
        //DbSet<EvtEventMediaDetail> EvtEventMediaDetails { get; set; }
        DbSet<EvtConfigContractCode> EvtConfigContractCodes { get; set; }
        DbSet<EvtConfigContractCodeDetail> EvtConfigContractCodeDetails { get; set; }
        DbSet<EvtTicketMedia> EvtTicketMedias { get; set; }
        DbSet<EvtEventType> EvtEventTypes { get; set; }
        DbSet<EvtInterestedPerson> EvtInterestedPeople { get; set; }
        DbSet<EvtHistoryUpdate> EvtHistoryUpdates { get; set; }
        DbSet<EvtSearchHistory> EvtSearchHistorys { get; set; }
        DbSet<EvtTicketTemplate> EvtTicketTemplates { get; set; }
        DbSet<EvtAdminEvent> EvtAdminEvents { get; set; }
        DbSet<EvtDeliveryTicketTemplate> EvtDeliveryTicketTemplates { get; set; }
    }

    public static class EventModelCreating
    {
        public static void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EvtEvent>(entity =>
            {
                entity.Property(e => e.CreatedDate).HasDefaultValueSql("SYSDATE");
                entity.Property(e => e.Status).HasDefaultValue(EvtOrderStatus.KHOI_TAO);
                entity.Property(e => e.IsShowApp).HasDefaultValue(YesNo.NO);
                entity.Property(e => e.Deleted).HasDefaultValue(YesNo.NO);
                entity.Property(e => e.IsCheck).HasDefaultValue(YesNo.NO);
                entity.Property(e => e.IsHighlight).HasDefaultValue(false);
                entity.Property(e => e.CanExportTicket).HasDefaultValue(false);
                entity.Property(e => e.CanExportRequestRecipt).HasDefaultValue(false);
            });
            modelBuilder.HasSequence(EvtEvent.SEQ, DbSchemas.EPIC_EVENT);
            modelBuilder.Entity<EvtEvent>()
                .HasOne(e => e.Province).WithMany().HasPrincipalKey(e => e.Code).HasForeignKey(e => e.ProvinceCode);
            modelBuilder.Entity<EvtEventDetail>(entity =>
            {
                entity.Property(e => e.CreatedDate).HasDefaultValueSql("SYSDATE");
                entity.Property(e => e.Status).HasDefaultValue(EventDetailStatus.NHAP);
                entity.Property(e => e.IsShowRemaingTicketApp).HasDefaultValue(false);
                entity.Property(e => e.Deleted).HasDefaultValue(YesNo.NO);
            });
            modelBuilder.HasSequence(EvtEventDetail.SEQ, DbSchemas.EPIC_EVENT);
            modelBuilder.Entity<EvtEventDetail>().HasOne(e => e.Event).WithMany(e => e.EventDetails).HasForeignKey(e => e.EventId).OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<EvtTicket>(entity =>
            {
                entity.Property(e => e.CreatedDate).HasDefaultValueSql("SYSDATE");
                entity.Property(e => e.IsShowApp).HasDefaultValue(YesNo.NO);
                entity.Property(e => e.Status).HasDefaultValue(EvtTicketStatus.KICH_HOAT);
                entity.Property(e => e.IsFree).HasDefaultValue(false);
                entity.Property(e => e.Deleted).HasDefaultValue(YesNo.NO);
            });
            modelBuilder.HasSequence(EvtTicket.SEQ, DbSchemas.EPIC_EVENT);
            modelBuilder.Entity<EvtTicket>().HasOne(e => e.EventDetail).WithMany(ed => ed.Tickets).HasForeignKey(e => e.EventDetailId).OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<EvtConfigContractCode>(entity =>
            {
                entity.Property(e => e.CreatedDate).HasDefaultValueSql("SYSDATE");
                entity.Property(e => e.Status).HasDefaultValue(Status.ACTIVE);
                entity.Property(e => e.Deleted).HasDefaultValue(YesNo.NO);
            });
            modelBuilder.HasSequence(EvtConfigContractCode.SEQ, DbSchemas.EPIC_EVENT);
            modelBuilder.Entity<EvtConfigContractCode>()
                .HasOne(e => e.TradingProvider)
                .WithMany()
                .HasForeignKey(e => e.TradingProviderId);

            modelBuilder.Entity<EvtConfigContractCodeDetail>(entity =>
            {
            });
            modelBuilder.HasSequence(EvtConfigContractCodeDetail.SEQ, DbSchemas.EPIC_EVENT);
            modelBuilder.Entity<EvtConfigContractCodeDetail>()
                .HasOne(configContractCodeDetail => configContractCodeDetail.ConfigContractCode)
                .WithMany(configContractCode => configContractCode.ConfigContractCodeDetails)
                .HasForeignKey(configContractCodeDetail => configContractCodeDetail.ConfigContractCodeId);

            #region Hình ảnh sự kiện
            modelBuilder.Entity<EvtEventMedia>(entity =>
            {
                entity.Property(e => e.Status).HasDefaultValue(Status.ACTIVE);
                entity.Property(e => e.CreatedDate).HasDefaultValueSql("SYSDATE");
                entity.Property(e => e.Deleted).HasDefaultValue(YesNo.NO);
            });
            modelBuilder.HasSequence(EvtEventMedia.SEQ, DbSchemas.EPIC_EVENT);
            modelBuilder.Entity<EvtEventMedia>()
                .HasOne(eventMedia => eventMedia.Event).WithMany(e => e.EventMedias).HasForeignKey(eventMedia => eventMedia.EventId);

            modelBuilder.Entity<EvtEventMediaDetail>(entity =>
            {
                entity.Property(e => e.Status).HasDefaultValue(Status.ACTIVE);
                entity.Property(e => e.CreatedDate).HasDefaultValueSql("SYSDATE");
                entity.Property(e => e.Deleted).HasDefaultValue(YesNo.NO);
            });
            modelBuilder.HasSequence(EvtEventMediaDetail.SEQ, DbSchemas.EPIC_EVENT);
            modelBuilder.Entity<EvtEventMediaDetail>()
                .HasOne(eventMediaDetail => eventMediaDetail.EventMedia)
                .WithMany(eventMedia => eventMedia.EventMediaDetails)
                .HasForeignKey(eventMediaDetail => eventMediaDetail.EventMediaId);
            #endregion
            modelBuilder.Entity<EvtOrder>(entity =>
            {
                entity.Property(e => e.CreatedDate).HasDefaultValueSql("SYSDATE");
                entity.Property(e => e.Status).HasDefaultValue(EvtOrderStatus.KHOI_TAO);
                entity.Property(e => e.Deleted).HasDefaultValue(YesNo.NO);
                entity.Property(e => e.Source).HasDefaultValue(SourceOrder.ONLINE);
                entity.Property(e => e.IsLock).HasDefaultValue(false);
                entity.Property(e => e.IsReceiveHardTicket).HasDefaultValue(false);
                entity.Property(e => e.IsRequestReceiveRecipt).HasDefaultValue(false);
            });

            modelBuilder.HasSequence(EvtOrder.SEQ, DbSchemas.EPIC_EVENT);
            modelBuilder.Entity<EvtOrder>().HasMany(o => o.OrderDetails)
                  .WithOne(od => od.Order)
                  .HasForeignKey(od => od.OrderId)
                  .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<EvtOrder>().HasOne(e => e.EventDetail).WithMany(ed => ed.Orders).HasForeignKey(e => e.EventDetailId).OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<EvtOrder>()
                .HasOne(e => e.Investor).WithMany().HasForeignKey(e => e.InvestorId);
            modelBuilder.Entity<EvtOrder>()
                .HasOne(e => e.TradingProvider).WithMany().HasForeignKey(e => e.TradingProviderId);
            modelBuilder.Entity<EvtOrder>()
                .HasOne(e => e.InvestorIdentification).WithMany().HasForeignKey(e => e.InvestorIdenId);
            modelBuilder.Entity<EvtOrder>()
                .HasOne(e => e.Sale).WithMany().HasForeignKey(e => e.ReferralSaleId);
            modelBuilder.Entity<EvtOrder>()
                .HasOne(e => e.Department).WithMany().HasForeignKey(e => e.DepartmentId);
            modelBuilder.Entity<EvtOrder>()
               .HasOne(e => e.ContractAddress).WithMany().HasForeignKey(e => e.ContractAddressId);

            modelBuilder.HasSequence(EvtOrderDetail.SEQ, DbSchemas.EPIC_EVENT);
            modelBuilder.Entity<EvtOrderDetail>().HasOne(e => e.Order).WithMany(ed => ed.OrderDetails).HasForeignKey(e => e.OrderId).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<EvtOrderDetail>().HasOne(e => e.Ticket).WithMany(ed => ed.OrderDetails).HasForeignKey(e => e.TicketId).OnDelete(DeleteBehavior.Cascade);

            modelBuilder.HasSequence(EvtOrderTicketDetail.SEQ, DbSchemas.EPIC_EVENT);
            modelBuilder.Entity<EvtOrderTicketDetail>().HasOne(e => e.OrderDetail).WithMany(ed => ed.OrderTicketDetails).HasForeignKey(e => e.OrderDetailId).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<EvtOrderTicketDetail>().HasOne(e => e.Ticket).WithMany(ed => ed.OrderTicketDetails).HasForeignKey(e => e.TicketId).OnDelete(DeleteBehavior.Cascade);

            modelBuilder.HasSequence(EvtTicketMedia.SEQ, DbSchemas.EPIC_EVENT);
            modelBuilder.Entity<EvtTicketMedia>()
                .HasOne(e => e.Ticket).WithMany(t => t.TicketMedias).HasForeignKey(e => e.TicketId);
            modelBuilder.Entity<EvtOrderPayment>(entity =>
            {
                entity.Property(e => e.Status).HasDefaultValue(OrderPaymentStatus.NHAP);
                entity.Property(e => e.CreatedDate).HasDefaultValueSql("SYSDATE");
                entity.Property(e => e.Deleted).HasDefaultValue(YesNo.NO);
            });
            modelBuilder.HasSequence(EvtOrderPayment.SEQ, DbSchemas.EPIC_EVENT);
            modelBuilder.Entity<EvtOrderPayment>()
                .HasOne(o => o.Order).WithMany(o => o.OrderPayments).HasForeignKey(p => p.OrderId);

            modelBuilder.HasSequence(EvtEventType.SEQ, DbSchemas.EPIC_EVENT);
            modelBuilder.Entity<EvtEventType>()
                .HasOne(o => o.Event).WithMany(e => e.EventTypes).HasForeignKey(p => p.EventId);

            modelBuilder.HasSequence(EvtInterestedPerson.SEQ, DbSchemas.EPIC_EVENT);

            modelBuilder.Entity<EvtInterestedPerson>()
               .HasOne(eip => eip.Event)
               .WithMany(e => e.InterestedPeople)
               .HasForeignKey(eip => eip.EventId);
            modelBuilder.Entity<EvtInterestedPerson>()
                   .HasOne(eip => eip.Investor)
                   .WithMany()
                   .HasForeignKey(eip => eip.InvestorId);

            modelBuilder.HasSequence(EvtHistoryUpdate.SEQ, DbSchemas.EPIC_EVENT);

            modelBuilder.Entity<EvtSearchHistory>()
              .HasOne(sh => sh.Event)
              .WithMany(e => e.SearchHistorys)
              .HasForeignKey(eip => eip.EventId);
            modelBuilder.Entity<EvtInterestedPerson>()
                   .HasOne(eip => eip.Investor)
                   .WithMany()
                   .HasForeignKey(eip => eip.InvestorId);
            modelBuilder.HasSequence(EvtSearchHistory.SEQ, DbSchemas.EPIC_EVENT);
            modelBuilder.Entity<EvtEventBankAccount>(entity =>
            {
                entity.Property(e => e.Status).HasDefaultValue(Status.ACTIVE);
                entity.Property(e => e.CreatedDate).HasDefaultValueSql("SYSDATE");
                entity.Property(e => e.Deleted).HasDefaultValue(YesNo.NO);
            });
            modelBuilder.HasSequence(EvtEventBankAccount.SEQ, DbSchemas.EPIC_EVENT);
            modelBuilder.Entity<EvtEventBankAccount>()
               .HasOne(b => b.Event)
               .WithMany(e => e.EvtEventBankAccounts)
               .HasForeignKey(e => e.EventId);

            modelBuilder.HasSequence(EvtEventDescriptionMedia.SEQ, DbSchemas.EPIC_EVENT);
            modelBuilder.Entity<EvtEventDescriptionMedia>()
                .HasOne(e => e.Event).WithMany(t => t.EventDescriptionMedias).HasForeignKey(e => e.EventId);

            modelBuilder.HasSequence(EvtTicketTemplate.SEQ, DbSchemas.EPIC_EVENT);
            modelBuilder.Entity<EvtTicketTemplate>(entity =>
            {
                entity.Property(e => e.CreatedDate).HasDefaultValueSql("SYSDATE");
                entity.Property(e => e.Status).HasDefaultValue(StatusCommon.DEACTIVE);
                entity.Property(e => e.Deleted).HasDefaultValue(YesNo.NO);
            });
            modelBuilder.Entity<EvtTicketTemplate>().HasOne(e => e.Event).WithMany(e => e.EvtTicketTemplates).HasForeignKey(e => e.EventId).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.HasSequence(EvtAdminEvent.SEQ, DbSchemas.EPIC_EVENT);
            modelBuilder.Entity<EvtAdminEvent>()
                .HasOne(e => e.Event).WithMany(t => t.AdminEvents).HasForeignKey(e => e.EventId).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<EvtAdminEvent>()
                .HasOne(e => e.Investor).WithMany().HasForeignKey(e => e.InvestorId);

            modelBuilder.Entity<EvtDeliveryTicketTemplate>(entity =>
            {
                entity.Property(e => e.CreatedDate).HasDefaultValueSql("SYSDATE");
                entity.Property(e => e.Status).HasDefaultValue(StatusCommon.ACTIVE);
                entity.Property(e => e.Deleted).HasDefaultValue(YesNo.NO);
            });
            modelBuilder.HasSequence(EvtDeliveryTicketTemplate.SEQ, DbSchemas.EPIC_EVENT);
            modelBuilder.Entity<EvtDeliveryTicketTemplate>().HasOne(e => e.Event).WithMany(e => e.DeliveryTicketTemplates).HasForeignKey(e => e.EventId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
