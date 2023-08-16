using AutoMapper;
using EPIC.CoreSharedEntities.Dto.TradingProvider;
using EPIC.Entities.DataEntities;
using EPIC.Entities.Dto.BusinessCustomer;
using EPIC.Entities.Dto.Investor;
using EPIC.Entities.Dto.TradingProvider;
using EPIC.Entities.Dto.User;
using EPIC.GarnerEntities.DataEntities;
using EPIC.GarnerEntities.Dto.GarnerApprove;
using EPIC.GarnerEntities.Dto.GarnerBlockadeLiberation;
using EPIC.GarnerEntities.Dto.GarnerContractTemplate;
using EPIC.GarnerEntities.Dto.GarnerContractTemplateApp;
using EPIC.GarnerEntities.Dto.GarnerContractTemplateTemp;
using EPIC.GarnerEntities.Dto.GarnerDashboard;
using EPIC.GarnerEntities.Dto.GarnerDistribution;
using EPIC.GarnerEntities.Dto.GarnerDistributionConfigContractCode;
using EPIC.GarnerEntities.Dto.GarnerDistributionConfigContractCodeDetail;
using EPIC.GarnerEntities.Dto.GarnerHistory;
using EPIC.GarnerEntities.Dto.GarnerInterestPayment;
using EPIC.GarnerEntities.Dto.GarnerOrder;
using EPIC.GarnerEntities.Dto.GarnerOrderPayment;
using EPIC.GarnerEntities.Dto.GarnerPolicy;
using EPIC.GarnerEntities.Dto.GarnerPolicyDetail;
using EPIC.GarnerEntities.Dto.GarnerPolicyDetailTemp;
using EPIC.GarnerEntities.Dto.GarnerPolicyTemp;
using EPIC.GarnerEntities.Dto.GarnerProduct;
using EPIC.GarnerEntities.Dto.GarnerProductOverview;
using EPIC.GarnerEntities.Dto.GarnerProductTradingProvider;
using EPIC.GarnerEntities.Dto.GarnerReceiveContractTemp;
using EPIC.GarnerEntities.Dto.GarnerWithdrawal;
using EPIC.GarnerSharedEntities.Dto;
using EPIC.IdentityEntities.DataEntities;
using System.Collections.Generic;

namespace EPIC.GarnerEntities.MapperProfiles
{
    public class GarnerMapperProfile : Profile
    {
        public GarnerMapperProfile()
        {
            #region GarnerProduct
            CreateMap<GarnerProduct, GarnerProductDto>();
            CreateMap<CreateGarnerProductDto, GarnerProduct>();
            CreateMap<UpdateGarnerProductDto, GarnerProduct>();
            CreateMap<CreateGarnerProductTradingProviderDto, GarnerProductTradingProvider>();
            CreateMap<UpdateGarnerProductTradingProviderDto, GarnerProductTradingProvider>();
            CreateMap<GarnerProductTradingProvider, GarnerProductTradingProviderDto>();
            CreateMap<GarnerProduct, GarnerProductByTradingProviderDto>();
            CreateMap<GarnerProduct, GarnerDashboardProductPickListDto>().ReverseMap();
            #endregion

            #region Distribution
            CreateMap<CreateGarnerDistributionDto, GarnerDistribution>();
            CreateMap<UpdateGarnerDistributionDto, GarnerDistribution>();
            CreateMap<GarnerDistribution, GarnerDistributionDto>();
            CreateMap<UpdateGarnerProductOverviewDto, GarnerDistribution>();
            CreateMap<GarnerProductOverviewFile, GarnerProductOverviewFileDto>();
            CreateMap<GarnerProductOverviewOrg, GarnerProductOverviewOrgDto>();
            CreateMap<GarnerPolicyDetail, AppGarnerPolicyDetailDto>();
            CreateMap<GarnerPolicy, AppGarnerDistributionDto>();
            CreateMap<GarnerDistribution, GarnerProductOverviewDto>();
            CreateMap<GarnerProductOverviewFile, AppProductOverviewFileDto>();
            CreateMap<GarnerProductOverviewOrg, AppProductOverviewOrgDto>();

            CreateMap<GarnerContractTemplateTempDto, GarnerContractTemplateTemp>().ReverseMap();
            CreateMap<GarnerContractTemplateTemp, GarnerContractTemplate>();
            CreateMap<GarnerPolicyDetailTempDto, GarnerPolicyDetailTemp>();
            CreateMap<GarnerPolicyDetailTemp, GarnerPolicyDetail>();
            CreateMap<GarnerProductFileDto, GarnerProductFile>().ReverseMap();

            #endregion

            CreateMap<GarnerApprove, ViewGarnerApproveDto>();
            CreateMap<GarnerHistoryUpdate, GarnerHistoryUpdateDto>();

            #region Diffenrce
            CreateMap<Users, UserDto>();
            CreateMap<TradingProvider, TradingProviderDto>();
            CreateMap<BusinessCustomer, BusinessCustomerDto>();
            #endregion

            #region policy temp
            CreateMap<CreateGarnerPolicyTempDto, GarnerPolicyTemp>();
            CreateMap<UpdateGarnerPolicyTempDto, GarnerPolicyTemp>();
            CreateMap<GarnerPolicyTemp, GarnerPolicyTempDto>();
            CreateMap<GarnerPolicyTempDto, CreateGarnerPolicyTempDto>();
            #endregion

            #region policy detail temp
            CreateMap<CreateGarnerPolicyDetailTempDto, GarnerPolicyDetailTemp>();
            CreateMap<UpdateGarnerPolicyDetailTempDto, GarnerPolicyDetailTemp>();
            CreateMap<GarnerPolicyDetailTemp, GarnerPolicyDetailTempDto>();
            #endregion

            #region ContractTemplate
            CreateMap<GarnerContractTemplate, GarnerContractTemplateDto>();
            CreateMap<CreateGarnerContractTemplateDto, GarnerContractTemplate>();
            CreateMap<UpdateGarnerContractTemplateDto, GarnerContractTemplate>();
            CreateMap<GarnerContractTemplate, GarnerContractTemplateAppDto>();
            CreateMap<GarnerContractTemplateForOrderDto, GarnerContractTemplateAppDto>();
            #endregion

            #region contract template temp
            CreateMap<CreateGarnerContractTemplateTempDto, GarnerContractTemplateTemp>();
            CreateMap<UpdateGarnerContractTemplateTempDto, GarnerContractTemplateTemp>();
            CreateMap<GarnerContractTemplateTemp, GarnerContractTemplateTempDto>().ReverseMap();
            #endregion

            #region Chính sách bán theo kỳ hạn
            CreateMap<GarnerPolicy, GarnerPolicyMoreInfoDto>();
            CreateMap<GarnerPolicy, CreatePolicyDto>();
            CreateMap<GarnerPolicy, AppGarnerDistributionDto>().ReverseMap();
            CreateMap<GarnerPolicy, GarnerPolicyDto>().ReverseMap();
            CreateMap<CreatePolicyDto, GarnerPolicy>();
            CreateMap<GarnerPolicy, UpdatePolicyDto>();
            CreateMap<AppTradingBankAccountDto, BusinessCustomerBankDto>().ReverseMap();

            CreateMap<GarnerPolicyDetail, GarnerPolicyDetailDto>();
            CreateMap<GarnerPolicyDetail, GarnerPolicyDetailDto>();
            CreateMap<CreatePolicyDetailDto, GarnerPolicyDetail>();
            CreateMap<GarnerPolicyDetail, UpdatePolicyDetailDto>();

            #endregion

            #region Order
            CreateMap<AppCreateGarnerOrderDto, GarnerOrder>();
            CreateMap<CreateGarnerOrderDto, GarnerOrder>();
            CreateMap<GarnerOrder, GarnerOrderMoreInfoDto>();
            CreateMap<GarnerOrder, GarnerOrderDto>();
            CreateMap<GarnerOrder, UpdateGarnerOrderDto>();
            CreateMap<UpdateGarnerOrderDto, GarnerOrder>();

            CreateMap<GarnerOrder, AppGarnerOrderListDto>();
            CreateMap<AppGarnerPolicyGroupOrderDto, AppGarnerOrderByPolicyDto>();
            CreateMap<GarnerOrder, AppGarnerOrderDetailDto>();
            CreateMap<GarnerOrder, AppGarnerOrderDto>();
            CreateMap<Investor, InvestorDto>();
            CreateMap<InvestorDto, Investor>();
            CreateMap<InvestorIdentification, InvestorIdentificationDto>();
            CreateMap<BusinessCustomerBank, FirstPaymentBankDto>();
            CreateMap<GarnerOrder, GarnerOrderCifCodeDto>();
            CreateMap<GarnerOrderMoreInfoDto, GarnerOrderMoreInfoDto>();
            #endregion

            #region OrderPayment
            CreateMap<CreateGarnerOrderPaymentDto, GarnerOrderPayment>();
            CreateMap<UpdateGarnerOrderPaymentDto, GarnerOrderPayment>();
            CreateMap<GarnerOrderPayment, GarnerOrderPaymentDto>();
            CreateMap<GarnerOrderPaymentDto, GarnerOrderPayment>();
            #endregion

            #region withdrawal
            CreateMap<GarnerWithdrawal, GarnerWithdrawalDto>().ReverseMap();
            CreateMap<GarnerWithdrawalOrderDetailDto, GarnerWithdrawalDetail>().ReverseMap();
            CreateMap<CalculateGarnerWithdrawalDto, ViewCalculateGarnerWithdrawalDto>().ReverseMap();
            #endregion

            #region InterestPayment
            CreateMap<GarnerInterestPaymentDetail, GarnerInterestPaymentDetailDto>().ReverseMap();
            #endregion
            #region Config Contract Code
            CreateMap<CreateConfigContractCodeDto, GarnerConfigContractCode>().ReverseMap();
            CreateMap<GarnerConfigContractCodeDto, GarnerConfigContractCode>().ReverseMap();
            CreateMap<CreateConfigContractCodeDetailDto, GarnerConfigContractCodeDetail>().ReverseMap();
            CreateMap<GarnerConfigContractCodeDetailDto, GarnerConfigContractCodeDetail>().ReverseMap();
            #endregion

            #region Phong toả, giải toả
            CreateMap<CreateGarnerBlockadeLiberationDto, GarnerBlockadeLiberation>().ReverseMap();
            CreateMap<UpdateGarnerBlockadeLiberationDto, GarnerBlockadeLiberation>().ReverseMap();
            CreateMap<GarnerBlockadeLiberationDto, GarnerBlockadeLiberation>().ReverseMap();
            #endregion

            #region Mẫu hợp đồng giao nhận
            CreateMap<GarnerReceiveContractTemplateDto, GarnerReceiveContractTemp>().ReverseMap();
            #endregion
        }
    }
}
