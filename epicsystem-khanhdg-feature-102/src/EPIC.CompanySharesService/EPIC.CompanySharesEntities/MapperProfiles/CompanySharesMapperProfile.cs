using AutoMapper;
using EPIC.CompanySharesEntities.DataEntities;
using EPIC.CompanySharesEntities.Dto.Issuer;
using EPIC.Entities.DataEntities;
using EPIC.Entities.Dto.BusinessCustomer;
using EPIC.CompanySharesEntities.Dto.CompanySharesInfoTradingProvider;
using EPIC.CompanySharesEntities.Dto.CpsInfo;
using EPIC.CompanySharesEntities.Dto.CpsSecondary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.CompanySharesEntities.MapperProfiles
{
    public class CompanySharesMapperProfile : Profile
    {
        public CompanySharesMapperProfile()
        {
            CreateMap<BusinessCustomer, BusinessCustomerDto>();
            CreateMap<BusinessCustomerBank, BusinessCustomerBankDto>();
            CreateMap<CpsIssuer, ViewCpsIssuerDto>();
            CreateMap<CpsInfo, CpsInfoDto>();
            CreateMap<CpsInfoTradingProvider, CpsInfoTradingProviderDto>();
            CreateMap<CpsSecondary, ViewCpsSecondaryDto>();

        }
    }
}
