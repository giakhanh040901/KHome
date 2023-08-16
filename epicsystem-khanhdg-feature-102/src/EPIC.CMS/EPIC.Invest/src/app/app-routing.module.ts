import { ApproveReinstatementComponent } from './approve-manager/approve-reinstatement/approve-reinstatement.component';
import { InterestContractComponent } from './trading-contract/interest-contract/interest-contract.component';
import { LoginUrlComponent } from './login-url/login-url.component';
import { RouterModule } from "@angular/router";
import { NgModule } from "@angular/core";
import { AppMainComponent } from "./layout/main/app.main.component";
import { HomeComponent } from "./home/home.component";
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
import { OwnerComponent } from './setting/owner/owner.component';
import { GeneralContractorComponent } from './setting/general-contractor/general-contractor.component';
import { GeneralContractorDetailComponent } from './setting/general-contractor/general-contractor-detail/general-contractor-detail.component';
import { OwnerDetailComponent } from './setting/owner/owner-detail/owner-detail.component';
import { OrderFilterProjectComponent } from './trading-contract/order/create-order/order-filter-project/order-filter-project.component';
import { TradingProviderComponent } from './setting/trading-provider/trading-provider.component';
import { TradingProviderDetailComponent } from './setting/trading-provider/trading-provider-detail/trading-provider-detail.component';
import { MediaComponent } from './setting/media/media.component';
import { SystemTemplateComponent } from './setting/system-template/system-template.component';
import { DeliveryContractComponent } from './trading-contract/delivery-contract/delivery-contract.component';
import { PermissionInvestConst } from '@shared/AppConsts';
import { DeliveryContractDetailComponent } from './trading-contract/delivery-contract/delivery-contract-detail/delivery-contract-detail.component';
import { OperationalReportComponent } from './export-report/operational-report/operational-report.component';
import { BusinessReportComponent } from './export-report/business-report/business-report.component';
import { ManagementReportComponent } from './export-report/management-report/management-report.component';
import { ApproveWithdrawalComponent } from './approve-manager/approve-withdrawal/approve-withdrawal.component';
import { ContractCodeStructureComponent } from './setting/contract-code-structure/contract-code-structure.component';
import { SampleContractComponent } from './setting/sample-contract/sample-contract.component';
import { ExpireContractComponent } from './trading-contract/expire-contract/expire-contract.component';
import { ManagerWithdrawComponent } from './trading-contract/manager-withdraw/manager-withdraw.component';
import { ContractRenewalComponent } from './trading-contract/contract-renewal/contract-renewal.component';
import { InvestmentHistoryComponent } from './trading-contract/investment-history/investment-history.component';
import { OrderDetailViewComponent } from './trading-contract/order/order-detail-view/order-detail-view.component';
import { CollectMoneyBankComponent } from './query-manager/collect-money-bank/collect-money-bank.component';
import { PayMoneyBankComponent } from './query-manager/pay-money-bank/pay-money-bank.component';
import { ProjectComponent } from './investment-manager/project/project.component';
import { ProjectDetailComponent } from './investment-manager/project/project-detail/project-detail.component';
import { DistributionComponent } from './investment-manager/distribution/distribution.component';
import { DistributionDetailComponent } from './investment-manager/distribution/distribution-detail/distribution-detail.component';

@NgModule({
	imports: [
		RouterModule.forChild([
			{
				path: "",
				component: AppMainComponent,
				children: [
					{ path: "login/url/:accessToken/:refreshToken", component: LoginUrlComponent},
					{ path: "home", component: HomeComponent, canActivate: [AppRouteGuard] },
					{
						path: "setting",
						children: [
							{
								path: "calendar",
								data: {permissions: [PermissionInvestConst.InvestMenuCauHinhNNL]}, 
								component: CalendarComponent,
								canActivate: [AppRouteGuard],
							},
                            { path: 'policy-template', component: PolicyTemplateComponent, data: {permissions: [PermissionInvestConst.InvestCSM_DanhSach]}, canActivate: [AppRouteGuard] },
							{ path: 'owner', component: OwnerComponent, data: {permissions: [PermissionInvestConst.InvestChuDT_DanhSach]}, canActivate: [AppRouteGuard] },
							{ path: 'owner/detail/:id', component: OwnerDetailComponent, data: {permissions: [PermissionInvestConst.InvestChuDT_ThongTinChuDauTu]}, canActivate: [AppRouteGuard] },
							{ path: 'general-contractor', component: GeneralContractorComponent, data: {permissions: [PermissionInvestConst.InvestTongThau_DanhSach]}, canActivate: [AppRouteGuard] },
							{ path: 'general-contractor/detail/:id', component: GeneralContractorDetailComponent, data: {permissions: [PermissionInvestConst.InvestTongThau_ThongTinTongThau]}, canActivate: [AppRouteGuard] },
							{ path: 'trading-provider', component: TradingProviderComponent, data: {permissions: [PermissionInvestConst.InvestDaiLy_DanhSach]}, canActivate: [AppRouteGuard] },
							{ path: 'trading-provider/detail/:id', component: TradingProviderDetailComponent, data: {permissions: [PermissionInvestConst.InvestDaiLy_ThongTinDaiLy]}, canActivate: [AppRouteGuard] },
							{ path: 'media', component: MediaComponent, data: {permissions: [PermissionInvestConst.InvestHinhAnh_DanhSach]}, canActivate: [AppRouteGuard] },
							{ path: 'system-notification-template', component: SystemTemplateComponent, data: {permissions: [PermissionInvestConst.InvestMenuThongBaoHeThong]}, canActivate: [AppRouteGuard] },
							{ path: 'contract-code-structure', component: ContractCodeStructureComponent, data: { permissions: [PermissionInvestConst.InvestMenuCauHinhMaHD] }, canActivate: [AppRouteGuard] },
							{ path: 'sample-contract', component: SampleContractComponent, data: { permissions: [PermissionInvestConst.InvestMenuMauHD] }, canActivate: [AppRouteGuard] },
						],
					},
					{
						path: "approve-manager",
						children: [
							{ 
								path: 'approve/:dataType', 
								data: {permissions: [
										PermissionInvestConst.InvestPDSPDT_DanhSach,
										PermissionInvestConst.InvestPDPPDT_DanhSach,
									]
								}, 
								component: ApproveComponent, 
								canActivate: [AppRouteGuard] 
							},
							{ 
								path: 'request-reinstatement', 
								data: { permissions: [PermissionInvestConst.InvestPDYCTT_DanhSach]}, 
								component: ApproveReinstatementComponent, canActivate: [AppRouteGuard] 
							},
							{ 
								path: 'request-withdrawal', 
								data: { permissions: [PermissionInvestConst.InvestPDYCRV_DanhSach]}, 
								component: ApproveWithdrawalComponent, canActivate: [AppRouteGuard] 
							},
						],
					},
					{
						path: "trading-contract",
						children: [
							{ path: "order", component: OrderComponent, data: {permissions: [PermissionInvestConst.InvestHDPP_SoLenh_DanhSach]}, canActivate: [AppRouteGuard] },
							{ path: "order/create", component: CreateOrderComponent, data: {permissions: [PermissionInvestConst.InvestHDPP_SoLenh_DanhSach]}, canActivate: [AppRouteGuard],
								children: [
									{path:'', redirectTo: 'filter-customer', pathMatch: 'full'},
									{ path: 'filter-customer', component: OrderFilterCustomerComponent },
									{ path: 'filter-project', component:OrderFilterProjectComponent},
									{ path: 'view', component: OrderViewComponent},
								],
							},
							{ path: "order/detail/:id", component: OrderDetailComponent, data: {permissions: [PermissionInvestConst.InvestHDPP_SoLenh_TTCT, PermissionInvestConst.InvestPDYCRV_ChiTietHD]}, canActivate: [AppRouteGuard] },
							{ path: 'order/detail/:id/:isJustView', component: OrderDetailComponent, data: {permissions: [PermissionInvestConst.InvestHDPP_SoLenh_TTCT]}, canActivate: [AppRouteGuard] },
							{ 
								path: 'order/detail-view/:id', component: OrderDetailViewComponent, 
								data: {permissions: [PermissionInvestConst.InvestHDPP_LSDT_ThongTinDauTu, PermissionInvestConst.InvestHDPP_HDDH_ThongTinDauTu]}, 
								canActivate: [AppRouteGuard] 
							},
							
							{ path: "contract-processing", component: ContractProcessingComponent, data: {permissions: [PermissionInvestConst.InvestHDPP_XLHD_DanhSach]}, canActivate: [AppRouteGuard] },
							{ path: "contract-active", component: ContractActiveComponent, data: {permissions: [PermissionInvestConst.InvestHDPP_HopDong_DanhSach]}, canActivate: [AppRouteGuard] },
							{ path: "contract-blockage", component: ContractBlockageComponent, data: {permissions: [PermissionInvestConst.InvestHopDong_PhongToaGiaiToa_DanhSach]}, canActivate: [AppRouteGuard] },
							{ path: "delivery-contract", component: DeliveryContractComponent, data: {permissions: [PermissionInvestConst.InvestHDPP_GiaoNhanHopDong_DanhSach]}, canActivate: [AppRouteGuard] },
							{ path: "delivery-contract/detail/:id", component: DeliveryContractDetailComponent, data: {permissions: [PermissionInvestConst.InvestHDPP_GiaoNhanHopDong_DanhSach]}, canActivate: [AppRouteGuard] },
							
							{ path: "interest-contract", component: InterestContractComponent, data: {permissions: [PermissionInvestConst.InvestHDPP_CTLT]}, canActivate: [AppRouteGuard] },
							
							{ path: "manager-withdraw", component: ManagerWithdrawComponent, data: {permissions: [PermissionInvestConst.InvestHDPP_XLRT]}, canActivate: [AppRouteGuard] },
							
							{ path: "expire-contract", component: ExpireContractComponent, data: {permissions: [PermissionInvestConst.InvestHDPP_HDDH]}, canActivate: [AppRouteGuard] },	
							
							{ path: "renewal-contract", component: ContractRenewalComponent, data: {permissions: [PermissionInvestConst.InvestHDPP_HDTT]}, canActivate: [AppRouteGuard] },
							{ path: "investment-history", component: InvestmentHistoryComponent, data: {permissions: [PermissionInvestConst.InvestHDPP_SoLenh_TTCT]}, canActivate: [AppRouteGuard] },
						],
					},
					{
						path: "invest-manager",
						children: [
							{ path: "project", component: ProjectComponent, data: {permissions: [PermissionInvestConst.InvestSPDT_DanhSach]}, canActivate: [AppRouteGuard] },
							{ path: "project/detail/:id", component: ProjectDetailComponent, data: {permissions: [PermissionInvestConst.InvestSPDT_ThongTinSPDT]}, canActivate: [AppRouteGuard] },
							{ path: "distribution", component: DistributionComponent, data: {permissions: [PermissionInvestConst.InvestPPDT_DanhSach]}, canActivate: [AppRouteGuard] },
							{ path: "distribution/detail/:id", component: DistributionDetailComponent, data: {permissions: [PermissionInvestConst.InvestPPDT_ThongTinPPDT]}, canActivate: [AppRouteGuard] },
							
						],
					},
					{ path: "export-report",
						children: [
							{path: "management-report", component: ManagementReportComponent, data: {permissions: [PermissionInvestConst.Invest_BaoCao_QuanTri], canActivate: [AppRouteGuard]}},
							{path: "operational-report", component: OperationalReportComponent, data: {permissions: [PermissionInvestConst.Invest_BaoCao_VanHanh], canActivate: [AppRouteGuard]}},
							{path: "business-report", component: BusinessReportComponent, data: {permissions: [PermissionInvestConst.Invest_BaoCao_KinhDoanh], canActivate: [AppRouteGuard]}},
						],
					},
					{ path: "query-manager",
					children: [
						{path: "collect-money-bank", component: CollectMoneyBankComponent, data: {permissions: [PermissionInvestConst.Invest_TruyVan_ThuTien_NganHang], canActivate: [AppRouteGuard]}},
						{path: "pay-money-bank", component: PayMoneyBankComponent, data: {permissions: [PermissionInvestConst.Invest_TruyVan_ChiTien_NganHang], canActivate: [AppRouteGuard]}},
					],
				},
				],
			},
		]),
	],
	exports: [RouterModule],
})
export class AppRoutingModule {}
