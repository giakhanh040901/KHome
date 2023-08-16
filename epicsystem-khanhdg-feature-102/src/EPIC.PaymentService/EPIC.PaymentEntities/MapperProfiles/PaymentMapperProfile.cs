using AutoMapper;
using EPIC.MSB.Dto.Notification;
using EPIC.PaymentEntities.DataEntities;
using EPIC.PaymentEntities.Dto.Msb;
using EPIC.PaymentEntities.Dto.MsbRequestPayment;
using EPIC.PaymentEntities.Dto.MsbRequestPaymentDetail;
using EPIC.PaymentEntities.Dto.Pvcb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.PaymentEntities.MapperProfiles
{
    public class PaymentMapperProfile : Profile
    {
        public PaymentMapperProfile()
        {
            CreateMap<PvcbCallback, PvcbCallbackDto>();
            CreateMap<ReceiveNotificationDto, MsbNotification>();
            CreateMap<MsbNotification, MsbNotificationDto>().ReverseMap();
            CreateMap<ReceiveNotificationPaymentDto, MsbNotificationPayment>();
            CreateMap<MsbRequestPayment, CreateMsbRequestPaymentDto>().ReverseMap();
            CreateMap<MsbRequestPaymentDetail, CreateMsbRequestDetailDto>().ReverseMap();
            CreateMap<MsbRequestPaymentDetail, MsbRequestPaymentWithErrorDto>().ReverseMap();
            CreateMap<MsbRequestPaymentDetail, ViewMsbRequestPaymentDto>().ReverseMap();
        }
    }
}
