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
import { ProductBondTypeComponent } from "./setting/product-bond-type/product-bond-type.component";
import { ProductBondDetailComponent } from "./bond-manager/product-bond-detail/product-bond-detail.component";
import { ProductBondInfoComponent } from "./bond-manager/product-bond-info/product-bond-info.component";
import { ContractTemplateComponent } from "./bond-manager/contract-template/contract-template.component";
import { ProductBondPrimaryComponent } from "./bond-manager/product-bond-primary/product-bond-primary.component";
import { ProductBondPolicyTemplateComponent } from './setting/product-bond-policy-template/product-bond-policy-template.component';
import { ProductBondSecondaryComponent } from "./bond-manager/product-bond-secondary/product-bond-secondary.component";
import { DistributionContractComponent } from "./bond-manager/distribution-contract/distribution-contract.component";
import { DistributionContractDetailComponent } from "./bond-manager/distribution-contract/distribution-contract-detail/distribution-contract-detail.component";
import { ProductBondSecondaryUpdateComponent } from "./bond-manager/product-bond-secondary/product-bond-secondary-update/product-bond-secondary-update.component";
import { ProductBondPrimaryDetailComponent } from "./bond-manager/product-bond-primary/product-bond-primary-detail/product-bond-primary-detail.component";

import { ProductBondInfoDetailComponent } from "./bond-manager/product-bond-info/product-bond-info-detail/product-bond-info-detail.component";
import { OrderComponent } from "./trading-contract/order/order.component";
import { CreateOrderComponent } from "./trading-contract/order/create-order/create-order.component";
import { IssuerDetailComponent } from "./setting/issuer/issuer-detail/issuer-detail.component";
import { DepositProviderDetailComponent } from "./setting/deposit-provider/deposit-provider-detail/deposit-provider-detail.component";
import { OrderFilterCustomerComponent } from "./trading-contract/order/create-order/order-filter-customer/order-filter-customer.component";
import { OrderFilterBondComponent } from "./trading-contract/order/create-order/order-filter-bond/order-filter-bond.component";
import { OrderViewComponent } from "./trading-contract/order/create-order/order-view/order-view.component";
import { OrderDetailComponent } from "./trading-contract/order/order-detail/order-detail.component";

import { ProductBondSecondPriceComponent } from "./bond-manager/product-bond-secondary/product-bond-second-price/product-bond-second-price.component";
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
import { PermissionBondConst } from '@shared/AppConsts';
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
								data: {permissions: [PermissionBondConst.BondMenuCaiDat_CHNNL]}, 
								component: CalendarComponent,
								canActivate: [AppRouteGuard],
							},
							{
								path: "calendar-partner",
								data: {permissions: [PermissionBondConst.BondMenuCaiDat_CHNNL]}, 
								component: CalendarPartnerComponent,
								canActivate: [AppRouteGuard],
							},
							{ path: 'issuer', component: IssuerComponent, data: {permissions: [PermissionBondConst.BondMenuCaiDat_TCPH]}, canActivate: [AppRouteGuard] },
							{ path: 'issuer/detail/:id', component: IssuerDetailComponent, data: {permissions: [PermissionBondConst.Bond_TCPH_ThongTinChiTiet]}, canActivate: [AppRouteGuard] },
                            { path: 'trading-provider', component: TradingProviderComponent, data: {permissions: [PermissionBondConst.BondMenuCaiDat_DLSC]}, canActivate: [AppRouteGuard] },
							{ path: 'trading-provider/detail/:id', component: TradingProviderDetailComponent, data: {permissions: [PermissionBondConst.Bond_DLSC_ThongTinChiTiet]}, canActivate: [AppRouteGuard] },
							{ path: 'deposit-provider', component: DepositProviderComponent, data: {permissions: [PermissionBondConst.BondMenuCaiDat_DLLK]}, canActivate: [AppRouteGuard] },
							{ path: 'deposit-provider/detail/:id', component: DepositProviderDetailComponent, data: {permissions: [PermissionBondConst.Bond_DLLK_ThongTinChiTiet]}, canActivate: [AppRouteGuard] },
                            { path: 'product-category', component: ProductCategoryComponent, canActivate: [AppRouteGuard] },
                            { path: 'product-bond-type', component: ProductBondTypeComponent, canActivate: [AppRouteGuard] },
                            { path: 'product-bond-policy-template', component: ProductBondPolicyTemplateComponent, data: {permissions: [PermissionBondConst.BondMenuCaiDat_CSM]}, canActivate: [AppRouteGuard] },
                            { path: 'system-notification', component: SystemTemplateComponent, data: {permissions: [PermissionBondConst.BondMenuCaiDat_MTB]}, canActivate: [AppRouteGuard] },
							{ path: 'media', component: MediaComponent, data: {permissions: [PermissionBondConst.BondMenuCaiDat_HinhAnh]}, canActivate: [AppRouteGuard] },
						],
					},
					{
						path: "bond-manager",
						children: [
							{ path: 'distribution-contract', component: DistributionContractComponent, data: {permissions: [PermissionBondConst.BondMenuQLTP_HDPP]}, canActivate: [AppRouteGuard] },
                            { path: 'distribution-contract/detail/:id', component: DistributionContractDetailComponent, data: {permissions: [PermissionBondConst.BondMenuQLTP_HDPP_TTCT]}, canActivate: [AppRouteGuard] },
							{ path: "product-bond-primary", component: ProductBondPrimaryComponent, data: {permissions: [PermissionBondConst.BondMenuQLTP_PHSC]}, canActivate: [AppRouteGuard] },
							{ path: "product-bond-primary/detail/:id", component: ProductBondPrimaryDetailComponent, data: {permissions: [PermissionBondConst.BondMenuQLTP_PHSC_TTCT]}, canActivate: [AppRouteGuard] },
							{
								path: "product-bond-detail",
								component: ProductBondDetailComponent,
								canActivate: [AppRouteGuard],
							},
							{ path: "product-bond-info", component: ProductBondInfoComponent, data: {permissions: [PermissionBondConst.BondMenuQLTP_LTP]}, canActivate: [AppRouteGuard] },
							{ path: "product-bond-info/detail/:id", component: ProductBondInfoDetailComponent, data: {permissions: [PermissionBondConst.BondMenuQLTP_LTP_TTCT]}, canActivate: [AppRouteGuard] },

							{
								path: "contract-template",
								data: {permissions: [PermissionBondConst.Bond_BTKH_TTCT_MauHopDong]}, 
								component: ContractTemplateComponent,
								canActivate: [AppRouteGuard],
							},
							{
								path: "product-bond-secondary",
								data: {permissions: [PermissionBondConst.BondMenuQLTP_BTKH]}, 
								component: ProductBondSecondaryComponent,
								canActivate: [AppRouteGuard],
							},
							{
								path: "product-bond-secondary/update/:id",
								data: {permissions: [PermissionBondConst.Bond_BTKH_TTCT_ThongTinChung_CapNhat]}, 
								component: ProductBondSecondaryUpdateComponent,
								canActivate: [AppRouteGuard],
							},
							{ 
								path: 'product-bond-secondary/product-bond-second-price', 
								data: {permissions: [PermissionBondConst.Bond_BTKH_TTCT_BangGia]}, 
								component: ProductBondSecondPriceComponent, 
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
										PermissionBondConst.BondQLPD_PDLTP_DanhSach,
										PermissionBondConst.BondQLPD_PDBTKH_DanhSach,
									]
								},
								component: ApproveComponent, 
								canActivate: [AppRouteGuard] 
							},
							{ 
								path: 'request-reinstatement', 
								data: { permissions: [PermissionBondConst.BondQLPD_PDYCTT_DanhSach]}, 
								component: ApproveReinstatementComponent, canActivate: [AppRouteGuard] 
							},
						],
					},
					{
						path: "trading-contract",
						children: [
							{ path: "order", component: OrderComponent, data: {permissions: [PermissionBondConst.BondHDPP_SoLenh]}, canActivate: [AppRouteGuard] },
							{ path: "order/create", component: CreateOrderComponent, data: {permissions: [PermissionBondConst.BondHDPP_SoLenh_ThemMoi]}, canActivate: [AppRouteGuard], 
								children: [
									{ path:'', redirectTo: 'filter-customer', pathMatch: 'full'},
									{ path: 'filter-customer', component: OrderFilterCustomerComponent },
									{ path: 'filter-product-bond', component: OrderFilterBondComponent },
									{ path: 'view', component: OrderViewComponent},
								],
							},
							{ path: "order/detail/:id", component: OrderDetailComponent, data: {permissions: [PermissionBondConst.BondHDPP_SoLenh_TTCT]}, canActivate: [AppRouteGuard] },
							{ path: "contract-processing", component: ContractProcessingComponent, data: {permissions: [PermissionBondConst.BondHDPP_XLHD]}, canActivate: [AppRouteGuard] },
							{ path: "contract-active", component: ContractActiveComponent, data: {permissions: [PermissionBondConst.BondHDPP_HopDong]}, canActivate: [AppRouteGuard] },
							{ path: "contract-blockage", component: ContractBlockageComponent, data: {permissions: [PermissionBondConst.BondHDPP_PTGT]}, canActivate: [AppRouteGuard] },
							{ path: "delivery-contract", component: DeliveryContractComponent, data: {permissions: [PermissionBondConst.BondHDPP_GiaoNhanHopDong]}, canActivate: [AppRouteGuard] },
							{ path: "delivery-contract/detail/:id", component: DeliveryContractDetailComponent, data: {permissions: [PermissionBondConst.BondHDPP_GiaoNhanHopDong]}, canActivate: [AppRouteGuard] },
							{ path: "interest-contract", component: InterestContractComponent, data: {permissions: [PermissionBondConst.BondHDPP_HDDH]}, canActivate: [AppRouteGuard] },
							{ path: "interest-contract/detail/:id", component: InterestContractDetailComponent, data: {permissions: [PermissionBondConst.BondHDPP_HDDH_ThongTinDauTu]}, canActivate: [AppRouteGuard] },

						],
					},
					{ 
						path: "export-report", 
						children: [
							{path: "management-report", component: ManagementReportComponent, data: {permissions: [PermissionBondConst.Bond_BaoCao_QuanTri], canActivate: [AppRouteGuard]}},
							{path: "operational-report", component: OperationalReportComponent, data: {permissions: [PermissionBondConst.Bond_BaoCao_VanHanh], canActivate: [AppRouteGuard]}},
							{path: "business-report", component: BusinessReportComponent, data: {permissions: [PermissionBondConst.Bond_BaoCao_KinhDoanh], canActivate: [AppRouteGuard]}},
						],
					},
					

				],
			},
		]),
	],
	exports: [RouterModule],
})
export class AppRoutingModule {}
