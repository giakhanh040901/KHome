using AutoMapper;
using EPIC.PaymentEntities.DataEntities;
using EPIC.PaymentSharedEntities.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.PaymentSharedEntities.MapperProfiles
{
    public class PaymentSharedMapperProfile : Profile
    {
        public PaymentSharedMapperProfile()
        {
            CreateMap<MsbNotification, ViewMsbCollectionPaymentDto>().ReverseMap();
        }
    }
}
