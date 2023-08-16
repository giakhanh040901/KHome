import { LoginUrlComponent } from './login-url/login-url.component';
import { RouterModule } from "@angular/router";
import { NgModule } from "@angular/core";
import { AppMainComponent } from "./layout/main/app.main.component";
import { HomeComponent } from "./home/home.component";
import { UserComponent } from "./user/user.component";
import { AppRouteGuard } from "@shared/auth/auth-route-guard";
import { IssuerComponent } from "./setting/issuer/issuer.component";
import { DepositProviderComponent } from "./setting/deposit-provider/deposit-provider.component";
import { ProductCategoryComponent } from "./setting/product-category/product-category.component";
import { CompanyShareTypeComponent } from "./setting/company-share-type/company-share-type.component";
import { CompanyShareDetailComponent } from "./company-share-manager/company-share-detail/company-share-detail.component";
import { CompanyShareInfoComponent } from "./company-share-manager/company-share-info/company-share-info.component";
import { ContractTemplateComponent } from "./company-share-manager/contract-template/contract-template.component";
import { CompanySharePrimaryComponent } from "./company-share-manager/company-share-primary/company-share-primary.component";
import { CompanySharePolicyTemplateComponent } from './setting/company-share-policy-template/company-share-policy-template.component';
import { CompanyShareSecondaryComponent } from "./company-share-manager/company-share-secondary/company-share-secondary.component";
import { DistributionContractComponent } from "./company-share-manager/distribution-contract/distribution-contract.component";
import { DistributionContractDetailComponent } from "./company-share-manager/distribution-contract/distribution-contract-detail/distribution-contract-detail.component";
import { CompanyShareSecondaryUpdateComponent } from "./company-share-manager/company-share-secondary/company-share-secondary-update/company-share-secondary-update.component";
import { CompanySharePrimaryDetailComponent } from "./company-share-manager/company-share-primary/company-share-primary-detail/company-share-primary-detail.component";

import { CompanyShareInfoDetailComponent } from "./company-share-manager/company-share-info/company-share-info-detail/company-share-info-detail.component";
import { OrderComponent } from "./trading-contract/order/order.component";
import { CreateOrderComponent } from "./trading-contract/order/create-order/create-order.component";
import { IssuerDetailComponent } from "./setting/issuer/issuer-detail/issuer-detail.component";
import { DepositProviderDetailComponent } from "./setting/deposit-provider/deposit-provider-detail/deposit-provider-detail.component";
import { OrderFilterCustomerComponent } from "./trading-contract/order/create-order/order-filter-customer/order-filter-customer.component";
import { OrderFilterCompanyShareComponent } from "./trading-contract/order/create-order/order-filter-company-share/order-filter-company-share.component";
import { OrderViewComponent } from "./trading-contract/order/create-order/order-view/order-view.component";
import { OrderDetailComponent } from "./trading-contract/order/order-detail/order-detail.component";

import { CompanyShareSecondPriceComponent } from "./company-share-manager/company-share-secondary/company-share-second-price/company-share-second-price.component";
import { ApproveComponent } from "./approve-manager/approve/approve.component";
import { CalendarComponent } from "./setting/calendar/calendar.component";
import { ContractProcessingComponent } from "./trading-contract/contract-processing/contract-processing.component";
import { ContractActiveComponent } from "./trading-contract/contract-active/contract-active.component";
import { ContractBlockageComponent } from "./trading-contract/contract-blockage/contract-blockage.component";
import { TradingProviderComponent } from './setting/trading-provider/trading-provider.component';
import { TradingProviderDetailComponent } from './setting/trading-provider/trading-provider-detail/trading-provider-detail.component';
import { SystemTemplateComponent } from './notification/system-template/system-template.component';
import { MediaComponent } from './setting/media/media.component';
import { CalendarPartnerComponent } from './setting/calendar-partner/calendar-partner.component';
import { DeliveryContractComponent } from './trading-contract/delivery-contract/delivery-contract.component';
import { ExportReportComponent } from './export-report/export-report.component';
import { PermissionCompanyShareConst } from '@shared/AppConsts';
import { DeliveryContractDetailComponent } from './trading-contract/delivery-contract/delivery-contract-detail/delivery-contract-detail.component';
import { ManagementReportComponent } from './export-report/management-report/management-report.component';
import { OperationalReportComponent } from './export-report/operational-report/operational-report.component';
import { BusinessReportComponent } from './export-report/business-report/business-report.component';
import { InterestContractComponent } from './trading-contract/interest-contract/interest-contract.component';
import { InterestContractDetailComponent } from './trading-contract/interest-contract/interest-contract-detail/interest-contract-detail.component';
import { ApproveReinstatementComponent } from './approve-manager/approve-reinstatement/approve-reinstatement.component';

@NgModule({
	imports: [
		RouterModule.forChild([
			{
				path: "",
				component: AppMainComponent,
				children: [
					{ path: "login/url/:accessToken/:refreshToken", component: LoginUrlComponent},
					{ path: "home", component: HomeComponent, canActivate: [AppRouteGuard] },
					{ path: "user", component: UserComponent, canActivate: [AppRouteGuard] },
					{
						path: "setting",
						children: [
							{
								path: "calendar",
								data: {permissions: [PermissionCompanyShareConst.CompanyShareMenuCaiDat_CHNNL]}, 
								component: CalendarComponent,
								canActivate: [AppRouteGuard],
							},
							{
								path: "calendar-partner",
								data: {permissions: [PermissionCompanyShareConst.CompanyShareMenuCaiDat_CHNNL]}, 
								component: CalendarPartnerComponent,
								canActivate: [AppRouteGuard],
							},
							{ path: 'issuer', component: IssuerComponent, data: {permissions: [PermissionCompanyShareConst.CompanyShareMenuCaiDat_TCPH]}, canActivate: [AppRouteGuard] },
							{ path: 'issuer/detail/:id', component: IssuerDetailComponent, data: {permissions: [PermissionCompanyShareConst.CompanyShare_TCPH_ThongTinChiTiet]}, canActivate: [AppRouteGuard] },
                            { path: 'trading-provider', component: TradingProviderComponent, data: {permissions: [PermissionCompanyShareConst.CompanyShareMenuCaiDat_DLSC]}, canActivate: [AppRouteGuard] },
							{ path: 'trading-provider/detail/:id', component: TradingProviderDetailComponent, data: {permissions: [PermissionCompanyShareConst.CompanyShare_DLSC_ThongTinChiTiet]}, canActivate: [AppRouteGuard] },
							// { path: 'deposit-provider', component: DepositProviderComponent, data: {permissions: [PermissionCompanyShareConst.CompanyShareMenuCaiDat_DLLK]}, canActivate: [AppRouteGuard] },
							// { path: 'deposit-provider/detail/:id', component: DepositProviderDetailComponent, data: {permissions: [PermissionCompanyShareConst.CompanyShare_DLLK_ThongTinChiTiet]}, canActivate: [AppRouteGuard] },
                            { path: 'product-category', component: ProductCategoryComponent, canActivate: [AppRouteGuard] },
                            { path: 'company-share-type', component: CompanyShareTypeComponent, canActivate: [AppRouteGuard] },
                            { path: 'company-share-policy-template', component: CompanySharePolicyTemplateComponent, data: {permissions: [PermissionCompanyShareConst.CompanyShareMenuCaiDat_CSM]}, canActivate: [AppRouteGuard] },
                            { path: 'system-notification', component: SystemTemplateComponent, data: {permissions: [PermissionCompanyShareConst.CompanyShareMenuCaiDat_MTB]}, canActivate: [AppRouteGuard] },
							{ path: 'media', component: MediaComponent, data: {permissions: [PermissionCompanyShareConst.CompanyShareMenuCaiDat_HinhAnh]}, canActivate: [AppRouteGuard] },
						],
					},
					{
						path: "company-share-manager",
						children: [
							{ path: 'distribution-contract', component: DistributionContractComponent, data: {permissions: [PermissionCompanyShareConst.CompanyShareMenuQLTP_HDPP]}, canActivate: [AppRouteGuard] },
                            { path: 'distribution-contract/detail/:id', component: DistributionContractDetailComponent, data: {permissions: [PermissionCompanyShareConst.CompanyShareMenuQLTP_HDPP_TTCT]}, canActivate: [AppRouteGuard] },
							{ path: "company-share-primary", component: CompanySharePrimaryComponent, data: {permissions: [PermissionCompanyShareConst.CompanyShareMenuQLTP_PHSC]}, canActivate: [AppRouteGuard] },
							{ path: "company-share-primary/detail/:id", component: CompanySharePrimaryDetailComponent, data: {permissions: [PermissionCompanyShareConst.CompanyShareMenuQLTP_PHSC_TTCT]}, canActivate: [AppRouteGuard] },
							{
								path: "company-share-detail",
								component: CompanyShareDetailComponent,
								canActivate: [AppRouteGuard],
							},
							{ path: "company-share-info", component: CompanyShareInfoComponent, data: {permissions: [PermissionCompanyShareConst.CompanyShareMenuQLTP_LTP]}, canActivate: [AppRouteGuard] },
							{ path: "company-share-info/detail/:id", component: CompanyShareInfoDetailComponent, data: {permissions: [PermissionCompanyShareConst.CompanyShareMenuQLTP_LTP_TTCT]}, canActivate: [AppRouteGuard] },

							{
								path: "contract-template",
								data: {permissions: [PermissionCompanyShareConst.CompanyShare_BTKH_TTCT_MauHopDong]}, 
								component: ContractTemplateComponent,
								canActivate: [AppRouteGuard],
							},
							{
								path: "company-share-secondary",
								data: {permissions: [PermissionCompanyShareConst.CompanyShareMenuQLTP_BTKH]}, 
								component: CompanyShareSecondaryComponent,
								canActivate: [AppRouteGuard],
							},
							{
								path: "company-share-secondary/update/:id",
								data: {permissions: [PermissionCompanyShareConst.CompanyShare_BTKH_TTCT_ThongTinChung_CapNhat]}, 
								component: CompanyShareSecondaryUpdateComponent,
								canActivate: [AppRouteGuard],
							},
							{ 
								path: 'company-share-secondary/company-share-second-price', 
								data: {permissions: [PermissionCompanyShareConst.CompanyShare_BTKH_TTCT_BangGia]}, 
								component: CompanyShareSecondPriceComponent, 
								canActivate: [AppRouteGuard] 
							},

						],
					},
					{
						path: "approve-manager",
						children: [
							{ 
								path: 'approve/:dataType', 
								data: {permissions: [
										PermissionCompanyShareConst.CompanyShareQLPD_PDLTP_DanhSach,
										PermissionCompanyShareConst.CompanyShareQLPD_PDBTKH_DanhSach,
									]
								},
								component: ApproveComponent, 
								canActivate: [AppRouteGuard] 
							},
							{ 
								path: 'request-reinstatement', 
								data: { permissions: [PermissionCompanyShareConst.CompanyShareQLPD_PDYCTT_DanhSach]}, 
								component: ApproveReinstatementComponent, canActivate: [AppRouteGuard] 
							},
						],
					},
					{
						path: "trading-contract",
						children: [
							{ path: "order", component: OrderComponent, data: {permissions: [PermissionCompanyShareConst.CompanyShareHDPP_SoLenh]}, canActivate: [AppRouteGuard] },
							{ path: "order/create", component: CreateOrderComponent, data: {permissions: [PermissionCompanyShareConst.CompanyShareHDPP_SoLenh_ThemMoi]}, canActivate: [AppRouteGuard], 
								children: [
									{ path:'', redirectTo: 'filter-customer', pathMatch: 'full'},
									{ path: 'filter-customer', component: OrderFilterCustomerComponent },
									{ path: 'filter-company-share', component: OrderFilterCompanyShareComponent },
									{ path: 'view', component: OrderViewComponent},
								],
							},
							{ path: "order/detail/:id", component: OrderDetailComponent, data: {permissions: [PermissionCompanyShareConst.CompanyShareHDPP_SoLenh_TTCT]}, canActivate: [AppRouteGuard] },
							{ path: "contract-processing", component: ContractProcessingComponent, data: {permissions: [PermissionCompanyShareConst.CompanyShareHDPP_XLHD]}, canActivate: [AppRouteGuard] },
							{ path: "contract-active", component: ContractActiveComponent, data: {permissions: [PermissionCompanyShareConst.CompanyShareHDPP_HopDong]}, canActivate: [AppRouteGuard] },
							{ path: "contract-blockage", component: ContractBlockageComponent, data: {permissions: [PermissionCompanyShareConst.CompanyShareHDPP_PTGT]}, canActivate: [AppRouteGuard] },
							{ path: "delivery-contract", component: DeliveryContractComponent, data: {permissions: [PermissionCompanyShareConst.CompanyShareHDPP_GiaoNhanHopDong]}, canActivate: [AppRouteGuard] },
							{ path: "delivery-contract/detail/:id", component: DeliveryContractDetailComponent, data: {permissions: [PermissionCompanyShareConst.CompanyShareHDPP_GiaoNhanHopDong]}, canActivate: [AppRouteGuard] },
							{ path: "interest-contract", component: InterestContractComponent, data: {permissions: [PermissionCompanyShareConst.CompanyShareHDPP_HDDH]}, canActivate: [AppRouteGuard] },
							{ path: "interest-contract/detail/:id", component: InterestContractDetailComponent, data: {permissions: [PermissionCompanyShareConst.CompanyShareHDPP_HDDH_ThongTinDauTu]}, canActivate: [AppRouteGuard] },

						],
					},
					{ 
						path: "export-report", 
						children: [
							{path: "management-report", component: ManagementReportComponent, data: {permissions: [PermissionCompanyShareConst.CompanyShare_BaoCao_QuanTri], canActivate: [AppRouteGuard]}},
							{path: "operational-report", component: OperationalReportComponent, data: {permissions: [PermissionCompanyShareConst.CompanyShare_BaoCao_VanHanh], canActivate: [AppRouteGuard]}},
							{path: "business-report", component: BusinessReportComponent, data: {permissions: [PermissionCompanyShareConst.CompanyShare_BaoCao_KinhDoanh], canActivate: [AppRouteGuard]}},
						],
					},
					

				],
			},
		]),
	],
	exports: [RouterModule],
})
export class AppRoutingModule {}
