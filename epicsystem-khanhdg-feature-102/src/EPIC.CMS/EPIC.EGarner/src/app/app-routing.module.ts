import { ApproveReinstatementComponent } from './approve-manager/approve-reinstatement/approve-reinstatement.component';
import { InterestContractComponent } from './trading-contract/interest-contract/interest-contract.component';
import { LoginUrlComponent } from './login-url/login-url.component';
import { RouterModule } from "@angular/router";
import { NgModule } from "@angular/core";
import { AppMainComponent } from "./layout/main/app.main.component";
import { HomeComponent } from "./home/home.component";
import { UserComponent } from "./user/user.component";
import { AppRouteGuard } from "@shared/auth/auth-route-guard";
import { OrderComponent } from "./trading-contract/order/order.component";
import { CreateOrderComponent } from "./trading-contract/order/create-order/create-order.component";
import { OrderFilterCustomerComponent } from "./trading-contract/order/create-order/order-filter-customer/order-filter-customer.component";
import { OrderViewComponent } from "./trading-contract/order/create-order/order-view/order-view.component";
import { OrderDetailComponent } from "./trading-contract/order/order-detail/order-detail.component";

import { ApproveComponent } from "./approve-manager/approve/approve.component";
import { CalendarComponent } from "./setting/calendar/calendar.component";
import { ContractProcessingComponent } from "./trading-contract/contract-processing/contract-processing.component";
import { ContractActiveComponent } from "./trading-contract/contract-active/contract-active.component";
import { ContractBlockageComponent } from "./trading-contract/contract-blockage/contract-blockage.component";

import { PolicyTemplateComponent } from './setting/policy-template/policy-template.component';
import { TradingProviderComponent } from './setting/trading-provider/trading-provider.component';
import { TradingProviderDetailComponent } from './setting/trading-provider/trading-provider-detail/trading-provider-detail.component';
import { MediaComponent } from './setting/media/media.component';
import { SystemTemplateComponent } from './setting/system-template/system-template.component';
import { DeliveryContractComponent } from './trading-contract/delivery-contract/delivery-contract.component';
import { PermissionGarnerConst } from '@shared/AppConsts';
import { DeliveryContractDetailComponent } from './trading-contract/delivery-contract/delivery-contract-detail/delivery-contract-detail.component';
import { OperationalReportComponent } from './export-report/operational-report/operational-report.component';
import { BusinessReportComponent } from './export-report/business-report/business-report.component';
import { ManagementReportComponent } from './export-report/management-report/management-report.component';
import { InterestContractDetailComponent } from './trading-contract/interest-contract/interest-contract-detail/interest-contract-detail.component';
import { ApproveWithdrawalComponent } from './approve-manager/approve-withdrawal/approve-withdrawal.component';
import { DistributionComponent } from './product-manager/distribution/distribution.component';
import { DistributionDetailComponent } from './product-manager/distribution/distribution-detail/distribution-detail.component';
import { ProductComponent } from './product-manager/product/product.component';
import { ProductDetailComponent } from './product-manager/product/product-detail/product-detail.component';
import { OrderFilterProductComponent } from './trading-contract/order/create-order/order-filter-product/order-filter-product.component';
import { CalendarPartnerComponent } from './setting/calendar-partner/calendar-partner.component';
import { ContractActiveGroupComponent } from './trading-contract/contract-active-group/contract-active-group.component';
import { CollectMoneyBankComponent } from './query-manager/collect-money-bank/collect-money-bank.component';
import { ManagerWithdrawComponent } from './trading-contract/manager-withdraw/manager-withdraw.component';
import { ContractCodeStructureComponent } from './setting/contract-code-structure/contract-code-structure.component';
import { SampleContractComponent } from './setting/sample-contract/sample-contract.component';
import { PayMoneyBankComponent } from './query-manager/pay-money-bank/pay-money-bank.component';
import { GeneralDescriptionComponent } from './setting/general-description/general-description.component';
import { GeneralDescriptionDetailComponent } from './setting/general-description/general-description-detail/general-description-detail.component';
import { GarnerHistoryComponent } from './trading-contract/garner-history/garner-history.component';
import { OrderJustviewComponent } from './trading-contract/order/order-justview/order-justview.component';

@NgModule({
	imports: [
		RouterModule.forChild([
			{
				path: "",
				component: AppMainComponent,
				children: [
					{ path: "login/url/:accessToken/:refreshToken", component: LoginUrlComponent },
					// { path: "home", component: HomeComponent, canActivate: [AppRouteGuard] },
					{ path: "home", component: HomeComponent },
					{ path: "user", component: UserComponent, canActivate: [AppRouteGuard] },
					{
						path: "setting",
						children: [
							{
								path: "calendar",
								data: { permissions: [PermissionGarnerConst.GarnerMenuCauHinhNNL] },
								component: CalendarComponent,
								canActivate: [AppRouteGuard],
							},
							{
								path: "calendar-partner",
								data: { permissions: [PermissionGarnerConst.GarnerMenuCauHinhNNL] },
								component: CalendarPartnerComponent,
								canActivate: [AppRouteGuard],
							},
							{ path: 'policy-template', component: PolicyTemplateComponent, data: { permissions: [PermissionGarnerConst.GarnerCSM_DanhSach] }, canActivate: [AppRouteGuard] },
							{ path: 'trading-provider', component: TradingProviderComponent, data: { permissions: [PermissionGarnerConst.GarnerDaiLy_DanhSach] }, canActivate: [AppRouteGuard] },
							{ path: 'trading-provider/detail/:id', component: TradingProviderDetailComponent, data: { permissions: [PermissionGarnerConst.GarnerDaiLy_ThongTinDaiLy] }, canActivate: [AppRouteGuard] },
							{ path: 'media', component: MediaComponent, data: { permissions: [PermissionGarnerConst.GarnerHinhAnh_DanhSach] }, canActivate: [AppRouteGuard] },
							{ path: 'system-notification-template', component: SystemTemplateComponent, data: { permissions: [PermissionGarnerConst.GarnerMenuThongBaoHeThong] }, canActivate: [AppRouteGuard] },
							{ path: 'general-description', component: GeneralDescriptionComponent, data: { permissions: [PermissionGarnerConst.GarnerMTC_DanhSach] }, canActivate: [AppRouteGuard] },
							{ path: 'general-description/detail', component: GeneralDescriptionDetailComponent, data: { permissions: [PermissionGarnerConst.GarnerMTC_ThongTinMTC] }, canActivate: [AppRouteGuard] },
							{ path: 'contract-code-structure', component: ContractCodeStructureComponent, data: { permissions: [PermissionGarnerConst.GarnerCauHinhMaHD_DanhSach] }, canActivate: [AppRouteGuard] },
							{ path: 'sample-contract', component: SampleContractComponent, data: { permissions: [PermissionGarnerConst.GarnerMauHD_DanhSach] }, canActivate: [AppRouteGuard] },
						],
					},
					{
						path: "product-manager",
						children: [
							{ path: "list", component: ProductComponent, data: {permissions: [PermissionGarnerConst.GarnerPPSP_ThongTinPPSP]}, canActivate: [AppRouteGuard] },
							{ path: "detail/:id", component: ProductDetailComponent, data: {permissions: [PermissionGarnerConst.GarnerPPSP_ThongTinPPSP]}, canActivate: [AppRouteGuard] },
							{ path: "distribution/list", component: DistributionComponent, data: {permissions: [PermissionGarnerConst.GarnerPPSP_ThongTinPPSP]}, canActivate: [AppRouteGuard] },
							{ path: "distribution/detail/:id", component: DistributionDetailComponent, data: {permissions: [PermissionGarnerConst.GarnerPPSP_ThongTinPPSP]}, canActivate: [AppRouteGuard] },
						],
					},
					{
						path: "approve-manager",
						children: [
							{ 
								path: 'approve/:dataType', 
								data: {permissions: [
										PermissionGarnerConst.GarnerPDSPTL_DanhSach,
										PermissionGarnerConst.GarnerPDPPSP_DanhSach,
									]
								},
								component: ApproveComponent,
								canActivate: [AppRouteGuard]
							},
							{
								path: 'request-reinstatement',
								data: { permissions: [PermissionGarnerConst.GarnerPDYCTT_DanhSach] },
								component: ApproveReinstatementComponent, canActivate: [AppRouteGuard]
							},

						],
					},
					{
						path: "trading-contract",
						children: [
							{ path: "order", component: OrderComponent, data: { permissions: [PermissionGarnerConst.GarnerHDPP_SoLenh_DanhSach] }, canActivate: [AppRouteGuard] },
							//
							{
								path: "order/create", 
								component: CreateOrderComponent, 
								children: [
									{ path: '', redirectTo: 'filter-customer', pathMatch: 'full' },
									{ path: 'filter-customer', component: OrderFilterCustomerComponent },
									{ path: 'filter-product', component: OrderFilterProductComponent },
									{ path: 'view', component: OrderViewComponent },
								],
							},
							//
							{
								path: "order/detail/:id",
								component: OrderDetailComponent,
								data: { permissions: [PermissionGarnerConst.GarnerHDPP_SoLenh_TTCT] }, 
								canActivate: [AppRouteGuard]
							},
							{
								path: 'order/detail/:id/:isJustView',
								data: {permissions: [PermissionGarnerConst.GarnerHDPP_SoLenh_TTCT]},
								component: OrderDetailComponent,
								canActivate: [AppRouteGuard], 
							},
							{
								path: 'order/justview/:id/:typeView',
								data: {permissions: [PermissionGarnerConst.GarnerHDPP_SoLenh_TTCT]},
								component: OrderJustviewComponent,
								canActivate: [AppRouteGuard], 
							},
							// 
							{ 
								path: "contract-processing", 
								component: ContractProcessingComponent, 
								data: { permissions: [PermissionGarnerConst.GarnerHDPP_XLHD_DanhSach] }, 
								canActivate: [AppRouteGuard] 
							},
							// 
							{ 
								path: "contract-active", 
								component: ContractActiveComponent, 
								data: { permissions: [PermissionGarnerConst.GarnerHDPP_HopDong_DanhSach] }, 
								canActivate: [AppRouteGuard] 
							},
							// 
							{ 
								path: "contract-active-group", 
								component: ContractActiveGroupComponent, 
								data: { permissions: [PermissionGarnerConst.GarnerHDPP_HopDong_DanhSach] }, 
								canActivate: [AppRouteGuard] 
							},
							// 
							{ 
								path: "contract-blockage", 
								component: ContractBlockageComponent, 
								data: { permissions: [PermissionGarnerConst.GarnerHopDong_PhongToaGiaiToa_DanhSach] }, 
								canActivate: [AppRouteGuard] 
							},
							// 
							{ 
								path: "delivery-contract", 
								component: DeliveryContractComponent, 
								data: { permissions: [PermissionGarnerConst.GarnerHDPP_GiaoNhanHopDong_DanhSach] }, 
								canActivate: [AppRouteGuard] 
							},
							//
							{ 
								path: "delivery-contract/detail/:id", 
								component: DeliveryContractDetailComponent, 
								data: { permissions: [PermissionGarnerConst.GarnerHDPP_GiaoNhanHopDong_DanhSach] }, 
								canActivate: [AppRouteGuard] 
							},
							// 
							{ 
								path: "interest-contract", 
								component: InterestContractComponent, 
								data: { permissions: [PermissionGarnerConst.GarnerHDPP_CTLC] }, 
								canActivate: [AppRouteGuard] 
							},
							//
							{ 
								path: "interest-contract/detail/:id", 
								component: InterestContractDetailComponent, 
								data: { permissions: [PermissionGarnerConst.GarnerHDPP_CTLC] }, 
								canActivate: [AppRouteGuard] 
							},
							// 
							{
								path: 'manager-withdraw',
								component: ManagerWithdrawComponent, canActivate: [AppRouteGuard],
								data: { permissions: [PermissionGarnerConst.GarnerHDPP_XLRutTien] }
							},
							{
								path: 'garner-history',
								component: GarnerHistoryComponent, canActivate: [AppRouteGuard],
								data: { permissions: [PermissionGarnerConst.GarnerHDPP_XLRutTien] }
							},
						],
					},
					{
						path: "export-report",
						children: [
							{ 
								path: "management-report", 
								component: ManagementReportComponent, 
								data: { permissions: [PermissionGarnerConst.Garner_BaoCao_QuanTri] },
								canActivate: [AppRouteGuard] 
							},
							// 
							{ 
								path: "operational-report", 
								component: OperationalReportComponent, 
								data: { permissions: [PermissionGarnerConst.Garner_BaoCao_VanHanh] },
								canActivate: [AppRouteGuard] 
							},
							// 
							{ 
								path: "business-report", 
								component: BusinessReportComponent, 
								data: { permissions: [PermissionGarnerConst.Garner_BaoCao_KinhDoanh] },
								canActivate: [AppRouteGuard]
							},
						],
					},
					{
						path: "query",
						children: [
							{ path: "collect-money-bank", component: CollectMoneyBankComponent, data: {permissions: [PermissionGarnerConst.Garner_TruyVan_ThuTien_NganHang]}, canActivate: [AppRouteGuard] },
							{ path: "pay-money-bank", component: PayMoneyBankComponent, data: {permissions: [PermissionGarnerConst.Garner_TruyVan_ChiTien_NganHang]}, canActivate: [AppRouteGuard] },
						],
					},
				],
			},
		]),
	],
	exports: [RouterModule],
})
export class AppRoutingModule { }
