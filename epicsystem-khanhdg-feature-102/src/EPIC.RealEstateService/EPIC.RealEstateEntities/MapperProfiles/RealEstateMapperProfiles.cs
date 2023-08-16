using AutoMapper;
using EPIC.RealEstateEntities.DataEntities;
using EPIC.RealEstateEntities.Dto.RstProject;
using EPIC.RealEstateEntities.Dto.RstOwner;
using EPIC.RealEstateEntities.Dto.RstProjectPolicy;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EPIC.RealEstateEntities.Dto.RstApprove;
using EPIC.RealEstateEntities.Dto.RstProjectFile;
using EPIC.RealEstateEntities.Dto.RstDistributionPolicyTemp;
using EPIC.RealEstateEntities.Dto.RstContractTemplateTemp;
using EPIC.RealEstateEntities.Dto.RstConfigContractCode;
using EPIC.RealEstateEntities.Dto.RstConfigContractCodeDetail;
using EPIC.RealEstateEntities.Dto.RstSellingPolicy;
using EPIC.RealEstateEntities.Dto.RstProjectStructure;
using EPIC.RealEstateEntities.Dto.RstProjectUtility;
using EPIC.RealEstateEntities.Dto.RstProjectUtilityMedia;
using EPIC.RealEstateEntities.Dto.RstProjectUtilityExtend;
using EPIC.RealEstateEntities.Dto.RstProductItem;
using EPIC.RealEstateEntities.Dto.RstProjectMedia;
using EPIC.RealEstateEntities.Dto.RstProjectMediaDetail;
using EPIC.RealEstateEntities.Dto.RstProductItemProjectUtility;
using EPIC.RealEstateEntities.Dto.RstDistribution;
using EPIC.RealEstateEntities.Dto.RstDistributionPolicy;
using EPIC.RealEstateEntities.Dto.RstProductItemMedia;
using EPIC.RealEstateEntities.Dto.RstProductItemMediaDetail;
using EPIC.RealEstateEntities.Dto.RstDistributionContractTemplate;
using EPIC.RealEstateEntities.Dto.RstOpenSell;
using EPIC.RealEstateEntities.Dto.RstOpenSellFile;
using EPIC.RealEstateEntities.Dto.RstOpenSellContractTemplate;
using EPIC.RealEstateEntities.Dto.RstOpenSellDetail;
using EPIC.RealEstateEntities.Dto.RstCart;
using EPIC.RealEstateEntities.Dto.RstOrder;
using EPIC.RealEstateEntities.Dto.RstOrderSellingPolicy;
using EPIC.RealEstateEntities.Dto.RstOrderPayment;
using EPIC.RealEstateEntities.Dto.RstOrderCoOwner;
using EPIC.RealEstateEntities.Dto.RstProjectExtend;
using EPIC.RealEstateEntities.Dto.RstProductItemExtend;
using EPIC.RealEstateEntities.Dto.RstOrderContractFile;
using EPIC.RealEstateEntities.Dto.RstProjectFavourite;
using EPIC.RealEstateEntities.Dto.RstHistoryUpdate;
using EPIC.RealEstateEntities.Dto.RstProjectInformationShare;

namespace EPIC.RealEstateEntities.MapperProfiles
{
    public class RealEstateMapperProfiles : Profile
    {
        public RealEstateMapperProfiles()
        {
            #region Project
            CreateMap<RstProject, RstCreateProjectDto>().ReverseMap();
            CreateMap<RstProject, RstUpdateProjectDto>().ReverseMap();
            CreateMap<RstProject, RstProjectDto>().ReverseMap();
            CreateMap<RstProject, ViewRstProjectDto>().ReverseMap();
            CreateMap<RstProject, AppViewListProjectDto>().ReverseMap();
            CreateMap<CreateRstProjectExtendDto, RstProjectExtend>().ReverseMap();
            CreateMap<RstProjectExtendDto, RstProjectExtend>().ReverseMap();
            CreateMap<AppRstProjectExtendDto, RstProjectExtend>().ReverseMap();
            #endregion

            #region DistributionPolicyTemp
            CreateMap<RstDistributionPolicyTemp, CreateRstDistributionPolicyTempDto>().ReverseMap();
            CreateMap<RstDistributionPolicyTemp, UpdateRstDistributionPolicyTempDto>().ReverseMap();
            CreateMap<RstDistributionPolicyTemp, RstDistributionPolicyTempDto>().ReverseMap();
            #endregion

            #region DistributionPolicy
            CreateMap<RstDistributionPolicy, CreateRstDistributionPolicyDto>().ReverseMap();
            CreateMap<RstDistributionPolicy, UpdateRstDistributionPolicyDto>().ReverseMap();
            CreateMap<RstDistributionPolicy, RstDistributionPolicyDto>().ReverseMap();
            #endregion

            #region chủ đầu tư
            CreateMap<CreateRstOwnerDto, RstOwner>().ReverseMap();
            CreateMap<UpdateRstOwnerDto, RstOwner>().ReverseMap();
            CreateMap<ViewRstOwnerDto, RstOwner>().ReverseMap();
            #endregion

            #region Chính sách ưu đãi chủ đầu tư
            CreateMap<CreateRstProjectPolicyDto, RstProjectPolicy>().ReverseMap();
            CreateMap<UpdateRstProjectPolicyDto, RstProjectPolicy>().ReverseMap();
            #endregion

            #region Module Duyệt
            CreateMap<RstApprove, RstDataApproveDto>().ReverseMap();
            #endregion

            #region Hồ sơ pháp lý
            CreateMap<CreateRstProjectFileDto, RstProjectFile>().ReverseMap();
            CreateMap<UpdateRstProjectFileDto, RstProjectFile>().ReverseMap();
            CreateMap<AppViewProjectFileDto, RstProjectFile>().ReverseMap();
            #endregion

            #region Mẫu hợp đồng mẫu
            CreateMap<CreateRstContractTemplateTempDto, RstContractTemplateTemp>().ReverseMap();
            CreateMap<UpdateRstContractTemplateTempDto, RstContractTemplateTemp>().ReverseMap();
            CreateMap<RstContractTemplateTempDto, RstContractTemplateTemp>().ReverseMap();
            #endregion

            #region Cấu trúc mã hợp đồng
            CreateMap<CreateRstConfigContractCodeDto, RstConfigContractCode>().ReverseMap();
            CreateMap<UpdateRstConfigContractCodeDto, RstConfigContractCode>().ReverseMap();
            CreateMap<RstConfigContractCodeDto, RstConfigContractCode>().ReverseMap();
            #endregion

            #region Chi tiết cấu trúc mã hợp đồng
            CreateMap<CreateRstConfigContractCodeDetailDto, RstConfigContractCodeDetail>().ReverseMap();
            CreateMap<RstConfigContractCodeDetailDto, RstConfigContractCodeDetail>().ReverseMap();
            #endregion

            #region Chính sách bán hàng
            CreateMap<RstSellingPolicyTemp, ViewRstSellingPolicyTempDto>().ReverseMap();
            CreateMap<RstSellingPolicy, RstSellingPolicyDto>().ReverseMap();
            #endregion

            #region Cấu trúc cấu thành dự án
            CreateMap<CreateRstProjectStructureDto, RstProjectStructure>().ReverseMap();
            CreateMap<UpdateRstProjectStructureDto, RstProjectStructure>().ReverseMap();
            CreateMap<RstProjectStructureDto, RstProjectStructure>().ReverseMap();
            CreateMap<RstProjectStructureChildDto, RstProjectStructure>().ReverseMap();
            #endregion

            #region Tiện ích dự án
            CreateMap<CreateRstProjectUtilityDto, RstProjectUtility>().ReverseMap();
            CreateMap<ViewCreateRstProjectUtilityDto, RstProjectUtility>().ReverseMap();
            CreateMap<RstProjectUtilityDto, RstProjectUtility>().ReverseMap();
            CreateMap<CreateRstProjectUtilityMediaDto, RstProjectUtilityMedia>().ReverseMap();
            CreateMap<RstProjectUtilityMediaDto, RstProjectUtilityMedia>().ReverseMap();
            CreateMap<AppRstProjectUtilityMediaDto, RstProjectUtilityMedia>().ReverseMap();
            CreateMap<ViewCreateRstProjectUtilityExtendDto, RstProjectUtilityExtend>().ReverseMap();
            CreateMap<RstProjectUtilityExtendDto, RstProjectUtilityExtend>().ReverseMap();
            CreateMap<RstProjectMedia, AppFindProjectMediaDto>().ReverseMap();
            #endregion

            #region Quản lý dự án - sản phẩm bán
            CreateMap<CreateRstProductItemDto, RstProductItem>().ReverseMap();
            CreateMap<UpdateRstProductItemDto, RstProductItem>().ReverseMap();
            CreateMap<RstSelectProductItemProjectUtilityDto, RstProjectUtility>().ReverseMap();
            CreateMap<RstProductItemDto, RstProductItem>().ReverseMap();
            CreateMap<AppGetAllProductItemDto, RstProductItem>().ReverseMap();
            CreateMap<AppRstProductItemDetailDto, RstProductItem>().ReverseMap();
            CreateMap<AppRstProductItemSimilarDetailDto, RstCartDetailDto>().ReverseMap();
            CreateMap<AppRstOrderDetailDto, RstProductItem>().ReverseMap();
            CreateMap<CreateRstProductItemExtendDto, RstProductItemExtend>().ReverseMap();
            CreateMap<RstProductItemExtendDto, RstProductItemExtend>().ReverseMap();
            CreateMap<AppRstProductItemExtendDto, RstProductItemExtend>().ReverseMap();
            CreateMap<RstProductItemByOpenSellDetailDto, RstProductItem>().ReverseMap();
            CreateMap<RstProductItemUtilityDto, RstProductItemUtility>().ReverseMap();
            #endregion

            #region Hình ảnh dự án
            CreateMap<CreateRstProjectMediaDto, RstProjectMedia>().ReverseMap();
            CreateMap<UpdateRstProjectMediaDto, RstProjectMedia>().ReverseMap();
            CreateMap<RstProjectMediaDto, RstProjectMedia>().ReverseMap();
            CreateMap<CreateRstProjectMediaDetailDto, RstProjectMediaDetail>().ReverseMap();
            CreateMap<UpdateRstProjectMediaDetailDto, RstProjectMediaDetail>().ReverseMap();
            CreateMap<RstProjectMediaDetailDto, RstProjectMediaDetail>().ReverseMap();
            CreateMap<AppRstProductItemMediaDto, RstProductItemMedia>().ReverseMap();
            CreateMap<AppViewProjectMediaDetailDto, RstProjectMediaDetail>().ReverseMap();
            CreateMap<AppViewProjectMediaDto, RstProjectMedia>().ReverseMap();
            CreateMap<AppViewProjectMediaDetailDto, RstProjectMediaDetailDto>().ReverseMap();
            #endregion

            #region Phân phối sản phẩm
            CreateMap<CreateRstDistributionDto, RstDistribution>().ReverseMap();
            CreateMap<UpdateRstDistributionDto, RstDistribution>().ReverseMap();
            CreateMap<RstDistributionDto, RstDistribution>().ReverseMap();
            #endregion

            #region Mở bán sản phẩm
            CreateMap<CreateRstOpenSellDto, RstOpenSell>().ReverseMap();
            CreateMap<UpdateRstOpenSellDto, RstOpenSell>().ReverseMap();
            CreateMap<RstOpenSellDto, RstOpenSell>().ReverseMap();
            CreateMap<RstOpenSellDetailDto, RstOpenSellDetail>().ReverseMap();
            CreateMap<RstOpenSellDetailInfoDto, RstCartDetailDto>().ReverseMap();
            #endregion

            #region RstOpenSellContractTemplate
            CreateMap<CreateRstOpenSellContractTemplateDto, RstOpenSellContractTemplate>().ReverseMap();
            CreateMap<UpdateRstOpenSellContractTemplateDto, RstOpenSellContractTemplate>().ReverseMap();
            CreateMap<RstOpenSellContractTemplateDto, RstOpenSellContractTemplate>().ReverseMap();
            #endregion

            #region Hình ảnh sản phẩm
            CreateMap<CreateRstProductItemMediaDto, RstProductItemMedia>().ReverseMap();
            CreateMap<UpdateRstProductItemMediaDto, RstProductItemMedia>().ReverseMap();
            CreateMap<RstProductItemMediaDto, RstProductItemMedia>().ReverseMap();
            CreateMap<CreateRstProductItemMediaDetailDto, RstProductItemMediaDetail>().ReverseMap();
            CreateMap<UpdateRstProductItemMediaDetailDto, RstProductItemMediaDetail>().ReverseMap();
            CreateMap<RstProductItemMediaDetailDto, RstProductItemMediaDetail>().ReverseMap();
            #endregion

            #region Biểu mẫu hợp đồng
            CreateMap<RstDistributionContractTemplateDto, RstDistributionContractTemplate>().ReverseMap();
            CreateMap<CreateRstDistributionContractTemplateDto, RstDistributionContractTemplate>().ReverseMap();
            CreateMap<UpdateRstDistributionContractTemplateDto, RstDistributionContractTemplate>().ReverseMap();
            #endregion

            #region Hồ sơ mở bán
            CreateMap<CreateRstOpenSellFileDto, RstOpenSellFile>().ReverseMap();
            CreateMap<UpdateRstOpenSellFileDto, RstOpenSellFile>().ReverseMap();
            CreateMap<AppViewOpenSellFileDto, RstOpenSellFile>().ReverseMap();
            #endregion

            #region Cart
            CreateMap<RstCartDetailDto, AppRstCartDto>().ReverseMap();
            #endregion

            #region Sổ lệnh
            CreateMap<CreateRstOrderDto, RstOrder>().ReverseMap();
            CreateMap<UpdateRstOrderDto, RstOrder>().ReverseMap();
            CreateMap<RstOrderDto, RstOrder>().ReverseMap();
            CreateMap<RstOrderMoreInfoDto, RstOrder>().ReverseMap();
            CreateMap<AppCreateRstOrderDto, RstOrder>().ReverseMap();
            CreateMap<RstOpenSellDetailInfoDto, AppRstOrderDto>().ReverseMap();
            CreateMap<RstProductItem, AppRstOrderDto>().ReverseMap();
            CreateMap<RstOrderSellingPolicyDto, AppRstSellingPolicyDto>().ReverseMap();
            #endregion

            #region Thanh toán sổ lệnh
            CreateMap<CreateRstOrderPaymentDto, RstOrderPayment>().ReverseMap();
            CreateMap<UpdateRstOrderPaymentDto, RstOrderPayment>().ReverseMap();
            CreateMap<RstOrderPaymentDto, RstOrderPayment>().ReverseMap();
            #endregion

            #region Đồng sở hữu
            CreateMap<RstOrderCoOwnerDto, RstOrderCoOwner>().ReverseMap();
            CreateMap<CreateRstOrderCoOwnerDto, RstOrderCoOwner>().ReverseMap();
            #endregion

            #region chính sách mở bán
            CreateMap<RstSellingPolicyDto, RstSellingPolicy>().ReverseMap();
            #endregion

            #region Hợp đồng 
            CreateMap<RstOrderContractFileDto, RstOrderContractFile>().ReverseMap();
            #endregion

            #region Dự án yêu thích
            CreateMap<CreateRstProjectFavouriteDto, RstProjectFavourite>().ReverseMap();
            CreateMap<RstProjectFavouriteDto, RstProjectFavourite>().ReverseMap();
            #endregion

            #region Lịch sử chỉnh sửa 
            CreateMap<RstHistoryUpdateDto, RstHistoryUpdate>().ReverseMap();
            #endregion

            #region ProjectShare
            CreateMap<RstProjectInformationShareDto, RstProjectInformationShare>().ReverseMap();
            CreateMap<AppRstProjectInformationShareDto, RstProjectInformationShare>().ReverseMap();
            CreateMap<RstProjectInformationShareDetail, RstProjectInformationShareDetailDto>().ReverseMap();
            #endregion
        }
    }
}
