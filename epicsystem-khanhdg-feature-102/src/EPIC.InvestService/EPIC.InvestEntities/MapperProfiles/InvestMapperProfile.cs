using AutoMapper;
using EPIC.Entities.DataEntities;
using EPIC.Entities.Dto.BusinessCustomer;
using EPIC.Entities.Dto.Department;
using EPIC.Entities.Dto.Investor;
using EPIC.Entities.Dto.ManagerInvestor;
using EPIC.Entities.Dto.Sale;
using EPIC.Entities.Dto.User;
using EPIC.GarnerEntities.DataEntities;
using EPIC.GarnerEntities.Dto.GarnerContractTemplate;
using EPIC.GarnerEntities.Dto.GarnerExportExcel;
using EPIC.IdentityEntities.DataEntities;
using EPIC.InvestEntities.DataEntities;
using EPIC.InvestEntities.Dto.BlockadeLiberation;
using EPIC.InvestEntities.Dto.ContractTemplate;
using EPIC.InvestEntities.Dto.ContractTemplateTemp;
using EPIC.InvestEntities.Dto.Distribution;
using EPIC.InvestEntities.Dto.DistributionFile;
using EPIC.InvestEntities.Dto.DistributionNews;
using EPIC.InvestEntities.Dto.DistributionVideo;
using EPIC.InvestEntities.Dto.GeneralContractor;
using EPIC.InvestEntities.Dto.InterestPayment;
using EPIC.InvestEntities.Dto.InvConfigContractCode;
using EPIC.InvestEntities.Dto.InvConfigContractCodeDetail;
using EPIC.InvestEntities.Dto.InvestApprove;
using EPIC.InvestEntities.Dto.InvestProject;
using EPIC.InvestEntities.Dto.InvestShared;
using EPIC.InvestEntities.Dto.Order;
using EPIC.InvestEntities.Dto.OrderPayment;
using EPIC.InvestEntities.Dto.Owner;
using EPIC.InvestEntities.Dto.Policy;
using EPIC.InvestEntities.Dto.PolicyTemp;
using EPIC.InvestEntities.Dto.Project;
using EPIC.InvestEntities.Dto.ProjectImage;
using EPIC.InvestEntities.Dto.ProjectInformationShare;
using EPIC.InvestEntities.Dto.ProjectJuridicalFile;
using EPIC.InvestEntities.Dto.ProjectOverViewFile;
using EPIC.InvestEntities.Dto.ProjectOverviewOrg;
using EPIC.InvestEntities.Dto.ProjectTradingProvider;
using EPIC.InvestEntities.Dto.ProjectType;
using EPIC.InvestEntities.Dto.Withdrawal;
using EPIC.InvestSharedEntites.Dto.InvestShared;
using EPIC.InvestSharedEntites.Dto.Order;
using System.Collections.Generic;
using BlockadeLiberation = EPIC.InvestEntities.DataEntities.BlockadeLiberation;
using InvOrder = EPIC.InvestEntities.DataEntities.InvOrder;

namespace EPIC.InvestEntities.MapperProfiles
{
    public class InvestMapperProfile : Profile
    {
        public InvestMapperProfile()
        {
            CreateMap<Project, ProjectDto>();

            CreateMap<DistributionFile, DistributionFileDto>();
            CreateMap<InvestApprove, ViewInvestApproveDto>();
            CreateMap<ProjectJuridicalFile, ProjectJuridicalFileDto>();
            CreateMap<Owner, OwnerDto>();
            CreateMap<Owner, ViewOwnerDto>();
            CreateMap<BusinessCustomer, BusinessCustomerDto>();
            CreateMap<BusinessCustomerBank, BusinessCustomerBankDto>();
            CreateMap<ViewPolicyDto, Policy>();
            CreateMap<Policy, ViewPolicyDto>();
            CreateMap<PolicyDto, Policy>().ReverseMap();
            CreateMap<GeneralContractor, ViewGeneralContractorDto>();
            CreateMap<PolicyDetail, ViewPolicyDetailDto>();
            CreateMap<ViewPolicyDetailDto, PolicyDetail>();
            CreateMap<ViewDistributionDto, Distribution>();
            CreateMap<Distribution, ViewDistributionDto>();
            CreateMap<Distribution, OverViewDistributionDto>();
            CreateMap<PolicyDetailTemp, PolicyDetailTempDto>();
            CreateMap<PolicyDetailTempDto, PolicyDetailTemp>();
            CreateMap<Distribution, DistributionDto>();
            CreateMap<BusinessCustomerBankDto, BusinessCustomerBank>();
            CreateMap<DataEntities.OrderPayment, OrderPaymentDto>();
            CreateMap<InvOrder, ViewOrderDto>().ReverseMap();
            CreateMap<InvOrder, SCInvestOrderDto>();
            CreateMap<InvestorIdentification, InvestorIdentificationDto>();
            CreateMap<Investor, InvestorDto>();
            CreateMap<ProjectDistributionDto, ProjectDistributionDto>();
            CreateMap<ProjectDto, AppProjectDto>();
            CreateMap<Project, AppProjectDto>();
            CreateMap<Project, SCInvestProjectDto>();
            CreateMap<ProjectImage, ProjectImageDto>();
            CreateMap<Distribution, AppProjectNewsDto>();
            CreateMap<Distribution, SCInvestDistributionDto>();
            CreateMap<DistributionVideo, ViewDistributionVideoDto>();
            CreateMap<DistriPolicyFile, AppPolicyFileDto>();
            CreateMap<ProjectJuridicalFile, AppJuridicalFileDto>();
            CreateMap<ViewDistributionVideoDto, AppDistributionVideoDto>();
            CreateMap<ViewDistributionNewsDto, AppDistributionNewsDto>();
            CreateMap<ViewOrderDto, AppInvestOrderInvestorDetailDto>();
            CreateMap<BusinessCustomerBank, Entities.Dto.Order.AppPaymentInfoDto>();
            CreateMap<CashFlowDto, AppCashFlowDto>();
            CreateMap<AppCashFlowDto, CashFlowDto>();
            CreateMap<CashFlowDto, ProfitDto>();
            CreateMap<ProfitDto, CashFlowDto>();
            CreateMap<AppSaleByReferralCodeDto, ViewSaleDto>();
            CreateMap<InvestContractTemplate, ContractTemplateAppDto>();
            CreateMap<DistributionFileDto, AppDistributionFileDto>();
            CreateMap<ProjectDistributionFindDto, ProjectDistributionDto>();
            CreateMap<ProjectType, ProjectTypeDto>();
            CreateMap<ProjectImage, ProjectImageDto>();
            CreateMap<BlockadeLiberation, BlockadeLiberationDto>();
            CreateMap<InvestInterestPayment, DanhSachChiTraDto>();
            CreateMap<Withdrawal, WithdrawalDto>();
            CreateMap<EPIC.Entities.Dto.Investor.InvestorIdentificationDto, InvestorIdentification>().ReverseMap();

            CreateMap<Policy, SCnvestPolicyDto>();
            CreateMap<PolicyDetail, SCInvestPolicyDetailDto>();
            CreateMap<Department, ViewDepartmentDto>();
            CreateMap<Department, SCDepartmentDto>();
            CreateMap<Users, UserDto>();
            CreateMap<AppSaleInvestorCreateOrderDto, AppCreateOrderDto>();
            CreateMap<ViewOrderDto, AppInvestOrderInvestorDetailDto>();
            CreateMap<ProjectOverviewFile, ProjectOverviewFileDto>();
            CreateMap<ProjectOverviewFile, ViewProjectOverViewFileDto>();
            CreateMap<ProjectOverviewOrg, ProjectOverviewOrgDto>();
            CreateMap<ProjectOverviewOrg, ViewProjectOverviewOrgDto>();
            CreateMap<ProjectTradingProvider, ProjectTradingProviderDto>();
            CreateMap<InvestorBankAccount, SCInvestorBankAccountDto>();
            CreateMap<TradingRecentlyDto, AppInvTransactionListDto>();
            CreateMap<InvestInterestPayment, InvestInterestPaymentDto>();
            CreateMap<PolicyDetail, PolicyDetailDto>();
            CreateMap<InvRenewalsRequest, OrderRenewalsRequestDto>();
            CreateMap<RutVonDto, InvestViewWithdrawalDataDto>();
            CreateMap<GarnerListInvestment, GarnerListInvestmentDto>();
            CreateMap<Sale, ViewSaleDto>();
            CreateMap<ViewPolicyTempDto, PolicyTemp>().ReverseMap();
            CreateMap<ViewPolicyDetailTempDto, PolicyDetailTemp>().ReverseMap();

            #region Config Contract Code
            CreateMap<CreateConfigContractCodeDto, InvestConfigContractCode>().ReverseMap();
            CreateMap<InvConfigContractCodeDto, InvestConfigContractCode>().ReverseMap();
            CreateMap<CreateConfigContractCodeDetailDto, InvestConfigContractCodeDetail>().ReverseMap();
            CreateMap<InvConfigContractCodeDetailDto, InvestConfigContractCodeDetail>().ReverseMap();
            #endregion

            #region Contract Template
            CreateMap<CreateContractTemplateDto, InvestContractTemplate>().ReverseMap();
            CreateMap<UpdateContractTemplateDto, InvestContractTemplate>().ReverseMap();
            CreateMap<ContractTemplateDto, InvestContractTemplate>().ReverseMap();
            CreateMap<CreateInvestContractTemplateTempDto, InvestContractTemplateTemp>().ReverseMap();
            CreateMap<UpdateInvestContractTemplateTempDto, InvestContractTemplateTemp>().ReverseMap();
            CreateMap<InvestContractTemplateTempDto, InvestContractTemplateTemp>().ReverseMap();
            CreateMap<ContractTemplateForUpdateContractDto, ContractTemplateAppDto>().ReverseMap();
            #endregion

            #region ProjectInformationShare
            CreateMap<InvProjectInformationShareDto, InvestProjectInformationShare>().ReverseMap();
            CreateMap<AppInvProjectInformationShareDto, InvestProjectInformationShare>().ReverseMap();
            CreateMap<InvestProjectInformationShareDetail, InvProjectInformationShareDetailDto>().ReverseMap();
            #endregion
        }
    }
}
