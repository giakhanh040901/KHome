using AutoMapper;
using EPIC.LoyaltyEntities.DataEntities;
using EPIC.LoyaltyEntities.Dto.ConversionPoint;
using EPIC.LoyaltyEntities.Dto.LoyHisAccumulatePoint;
using EPIC.LoyaltyEntities.Dto.LoyHistoryUpdate;
using EPIC.LoyaltyEntities.Dto.LoyLuckyProgram;
using EPIC.LoyaltyEntities.Dto.LoyLuckyRotationInterface;
using EPIC.LoyaltyEntities.Dto.LoyLuckyScenario;
using EPIC.LoyaltyEntities.Dto.LoyRank;
using EPIC.LoyaltyEntities.Dto.LoyVoucher;
using EPIC.LoyaltyEntities.Dto.LoyVoucherInvestor;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.LoyaltyEntities.MapperProfiles
{
    public class LoyaltyMapperProfiles : Profile
    {
        public LoyaltyMapperProfiles()
        {
            CreateMap<LoyVoucher, AddVoucherDto>().ReverseMap();
            CreateMap<LoyVoucher, ViewListVoucherDto>().ForMember(d => d.VoucherId, opt => opt.MapFrom(src => src.Id))
                .ReverseMap()
                .ForPath(s => s.Id, opt => opt.MapFrom(d => d.VoucherId)); 
            CreateMap<AppViewVoucherExpiredByInvestorDto, AppViewVoucherByInvestorDto>().ReverseMap();
            CreateMap<LoyVoucher, ViewVoucherDto>()
                .ForMember(d => d.VoucherId, opt => opt.MapFrom(src => src.Id))
                .ReverseMap()
                .ForPath(s => s.Id, opt => opt.MapFrom(d => d.VoucherId));
            CreateMap<LoyVoucher, UpdateVoucherDto>().ReverseMap()
                .ForMember(d => d.Id, opt => opt.MapFrom(src => src.VoucherId))
                .ReverseMap()
                .ForPath(s => s.VoucherId, opt => opt.MapFrom(d => d.Id));

            CreateMap<LoyVoucherInvestor, ViewVoucherShortInvestorDto>()
                .ForMember(d => d.VoucherInvestorId, opt => opt.MapFrom(src => src.Id))
                .ReverseMap()
                .ForPath(s => s.Id, opt => opt.MapFrom(d => d.VoucherInvestorId));

            CreateMap<LoyVoucherInvestor, ViewVoucherShortBusinessCustomerDto>()
                .ForMember(d => d.VoucherInvestorId, opt => opt.MapFrom(src => src.Id))
                .ReverseMap()
                .ForPath(s => s.Id, opt => opt.MapFrom(d => d.VoucherInvestorId));
            CreateMap<LoyHisAccumulatePoint, AddAccumulatePointDto>().ReverseMap();
            CreateMap<LoyHisAccumulatePoint, ViewHisAccumulatePointDto>().ReverseMap();
            CreateMap<LoyHisAccumulatePoint, UpdateAccumulatePointDto>().ReverseMap();
            CreateMap<LoyHisAccumulatePoint, AppViewAccumulatePointHistoryDto>().ReverseMap();
            CreateMap<LoyAccumulatePointStatusLog, ViewHisAccumulatePointStatusLogDto>().ReverseMap();
            CreateMap<AddLoyConversionPointDto, LoyConversionPoint>().ReverseMap();
            CreateMap<LoyConversionPoint, LoyConversionPointDto>().ReverseMap();
            CreateMap<LoyConversionPointStatusLog, LoyConversionPointStatusLogDto>().ReverseMap();
            CreateMap<LoyRank, AddRankDto>().ReverseMap();
            CreateMap<LoyRank, ViewFindRankDto>().ReverseMap();
            CreateMap<LoyVoucher, AppLoyConversionPointByInvestorInfoDto>().ReverseMap();
            CreateMap<LoyLuckyProgram, LoyLuckyProgramDto>().ReverseMap();
            CreateMap<LoyLuckyScenario, LoyLuckyScenarioDto>().ReverseMap();
            CreateMap<LoyLuckyRotationInterface, LoyLuckyRotationInterfaceDto>().ReverseMap();
            CreateMap<LoyHistoryUpdate, LoyHistoryUpdateDto>().ReverseMap();
            CreateMap<LoyLuckyScenario, AppLoyLuckyScenarioDto>().ReverseMap();
        }
    }
}
