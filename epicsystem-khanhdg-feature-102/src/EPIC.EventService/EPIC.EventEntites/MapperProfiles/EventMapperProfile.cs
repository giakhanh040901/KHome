using AutoMapper;
using EPIC.EventEntites.Dto.EvtEvent;
using EPIC.EventEntites.Dto.EvtEventDetail;
using EPIC.EventEntites.Dto.EvtTicket;
using EPIC.EventEntites.Dto.EvtConfigContractCode;
using EPIC.EventEntites.Dto.EvtConfigContractCodeDetail;
using EPIC.EventEntites.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EPIC.EventEntites.Dto.EvtOrder;
using EPIC.EventEntites.Dto.EvtOrderDetail;
using EPIC.Entities.Dto.AssetManager;
using EPIC.EventEntites.Dto.EvtEventMediaDetail;
using EPIC.EventEntites.Dto.EvtEventMedia;
using EPIC.EventEntites.Dto.EvtOrderPayment;
using EPIC.EventEntites.Dto.EvtEventDescriptionMedia;
using EPIC.EventEntites.Dto.EvtTicketTemplate;
using EPIC.EventEntites.Dto.EvtDeliveryTicketTemplate;

namespace EPIC.EventEntites.MapperProfiles
{
    public class EventMapperProfile : Profile
    {
        public EventMapperProfile()
        {
            CreateMap<EvtEvent, EvtEventDto>().ReverseMap();
            CreateMap<CreateEvtEventDto, EvtEvent>();

            CreateMap<EvtEventDetail, EvtEventDetailDto>().ReverseMap();
            CreateMap<CreateEvtEventDetailDto, EvtEventDetail>();

            CreateMap<EvtTicket, EvtTicketDto>().ReverseMap();
            CreateMap<EvtEventMediaDetail, EvtEventMediaDetailDto>().ReverseMap();
            CreateMap<EvtEventMedia, ViewEvtEventMediaDto>().ReverseMap();
            CreateMap<EvtOrderPayment, EvtOrderPaymentDto>().ReverseMap();
            CreateMap<CreateEvtTicketDto, EvtTicket>();
            #region Config Contract Code
            CreateMap<CreateEvtConfigContractCodeDto, EvtConfigContractCode>().ReverseMap();
            CreateMap<EvtConfigContractCodeDto, EvtConfigContractCode>().ReverseMap();
            CreateMap<CreateEvtConfigContractCodeDetailDto, EvtConfigContractCodeDetail>().ReverseMap();
            CreateMap<EvtConfigContractCodeDetailDto, EvtConfigContractCodeDetail>().ReverseMap();
            #endregion
            #region Order
            CreateMap<CreateEvtOrderDto, EvtOrder>().ReverseMap();
            CreateMap<OrderDetailDto, EvtOrderDetail>();
            CreateMap<CreateEvtOrderDto, EvtOrderDto>()
                .ForMember(dest => dest.OrderDetails, opt => opt.MapFrom(src => src.OrderDetails));
            CreateMap<EvtOrderDetail, EvtOrderDetailDto>();
            CreateMap<EvtOrder, EvtOrderDto>()
                .ForMember(dest => dest.OrderDetails, opt => opt.MapFrom(src => src.OrderDetails));

            #endregion

            CreateMap<EvtEventDescriptionMedia, EvtEventDescriptionMediaDto>().ReverseMap();
            CreateMap<EvtEventDescriptionMedia, AppEvtEventDescriptionMediaDto>().ReverseMap();
            CreateMap<EvtTicketTemplate, ResponseEvtTicketTemplateDto>().ReverseMap();
            CreateMap<EvtTicketTemplate, CreateEvtTicketTemplateDto>().ReverseMap();
            CreateMap<EvtTicketTemplate, UpdateEvtTicketTemplateDto>().ReverseMap();

            CreateMap<EvtDeliveryTicketTemplate, EvtDeliveryTicketTemplateDto>().ReverseMap();
        }
    }
}
