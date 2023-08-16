using EPIC.EventEntites.Entites;
using EPIC.Utils.ConstantVariables.Event;
using EPIC.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EPIC.EventEntites.Dto.EvtOrder;
using EPIC.EventEntites.Dto.EvtOrderDetail;
using EPIC.Utils.ConstantVariables.Shared;
using AutoMapper;

namespace EPIC.EventRepositories
{
    public static class EvtEventRepositoryExtensions
    {
        /// <summary>
        /// Tính số lượng vé của sự kiện
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public static int CalculationEventTicketQuantity(this EvtEvent e)
        {
            return e.EventDetails.Sum(ed => ed.Tickets.Sum(t => t.Quantity));
        }

        /// <summary>
        /// Tính số lượng vé đăng ký của sự kiện
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public static int CalculationEventRegisterTicketQuantity(this EvtEvent e)
        {
            return e.EventDetails.Sum(ed => ed.Orders.Sum(o => o.OrderDetails.Sum(od => od.Quantity)));
        }

        /// <summary>
        /// Tính số lượng vé hợp lệ của sự kiện
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public static int CalculationEventValidTicketQuantity(this EvtEvent e)
        {
            return e.EventDetails.Sum(ed => ed.Orders.Where(o => o.Status == EvtOrderStatus.HOP_LE).Sum(o => o.OrderDetails.Sum(od => od.Quantity)));
        }

        /// <summary>
        /// Tính số lượng vé còn lại của sự kiện
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public static int CalculationEventRemainTicketQuantity(this EvtEvent e)
        {
            return e.EventDetails.Sum(ed => ed.Tickets.Sum(t => t.Quantity))
                    - (e.EventDetails.Sum(e => e.Orders.Where(o => (o.Status == EvtOrderStatus.HOP_LE || o.Status == EvtOrderStatus.CHO_THANH_TOAN) && o.Deleted == YesNo.NO)
                                                       .Sum(o => o.OrderDetails.Sum(od => od.Quantity))));
        }

        /// <summary>
        /// Tính số lượng vé tham gia của sự kiện
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public static int CalculationEventParticipateTicketQuantity(this EvtEvent e)
        {
            return e.EventDetails.Sum(ed => ed.Orders.Sum(o => o.OrderDetails.Sum(od => od.OrderTicketDetails.Where(otd => otd.Status == EvtOrderTicketStatus.DA_THAM_GIA).Count())));
        }

        public static IQueryable<EvtOrderDto> SelectEvtOrderDto(this IQueryable<EvtOrder> query, IMapper mapper)
        {
            return query.Select(t => new EvtOrderDto
            {
                Id = t.Id,
                HavePayment = t.OrderPayments.Any(ep => ep.OrderId == t.Id
                    && (ep.Status == OrderPaymentStatus.DA_THANH_TOAN || ep.Status == OrderPaymentStatus.NHAP)
                    && ep.Deleted == YesNo.NO),
                ContractAddressId = t.ContractAddressId,
                AmountPaid = t.OrderPayments.Where(op => op.Status == OrderPaymentStatus.DA_THANH_TOAN).Sum(op => op.PaymentAmount),
                ContractAddressName = t.ContractAddress.ContactAddress,
                EventDetailId = t.EventDetailId,
                Fullname = t.InvestorIdentification.Fullname,
                Quantity = t.OrderDetails.Sum(od => od.Quantity),
                ReferralSaleId = t.ReferralSaleId ?? null,
                TotalMoney = t.OrderDetails.Sum(od => (od.Quantity * od.Price)),
                InvestorId = t.InvestorId,
                InvestorIdenId = t.InvestorIdenId,
                ExpiredTime = t.ExpiredTime,
                IsReceiveHardTicket = t.IsReceiveHardTicket,
                IsRequestReceiveRecipt = t.IsRequestReceiveRecipt,
                Status = t.Status,
                IsLock = t.IsLock,
                Source = t.Source,
                OrderSource = t.Source == SourceOrder.ONLINE
                    ? SourceOrderFE.KHACH_HANG
                    : (t.Source == SourceOrder.OFFLINE && t.ReferralSaleId == null)
                        ? SourceOrderFE.QUAN_TRI_VIEN
                        : SourceOrderFE.SALE,
                CreatedDate = t.CreatedDate,
                Phone = t.Investor.Phone,
                ContractCode = t.ContractCodeGen ?? t.ContractCode,
                ContractCodeGen = t.ContractCodeGen,
                EventName = t.EventDetail.Event.Name,
                OrderDetails = mapper.Map<IEnumerable<EvtOrderDetailDto>>(t.OrderDetails)
            });
        }
    }
}
