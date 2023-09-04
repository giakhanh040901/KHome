import { LoginUrlComponent } from './login-url/login-url.component';
import { RouterModule } from "@angular/router";
import { NgModule } from "@angular/core";
import { AppMainComponent } from "./layout/main/app.main.component";
import { HomeComponent } from "./home/home.component";
import { UserComponent } from "./user/user.component";
import { AppRouteGuard } from "@shared/auth/auth-route-guard";
import { PermissionCoreConst, PermissionRealStateConst } from '@shared/AppConsts';
import { OperationalReportComponent } from './export-report/operational-report/operational-report.component';
import { BusinessReportComponent } from './export-report/business-report/business-report.component';
import { ManagementReportComponent } from './export-report/management-report/management-report.component';
import { ProductItemComponent } from './modules/product-item/product-item.component';
import { ProjectComponent } from './modules/project/project.component';
import { ProductItemFindComponent } from './modules/product-item-find/product-item-find.component';
import { ProjectFindComponent } from './modules/project-find/project-find.component';

@NgModule({
	imports: [
		RouterModule.forChild([
			{
				path: "",
				component: AppMainComponent,
				children: [
					{ path: "login/url/:accessToken/:refreshToken", component: LoginUrlComponent },
					{ path: "home", component: HomeComponent, canActivate: [AppRouteGuard] },
					{ path: "user", component: UserComponent, canActivate: [AppRouteGuard] },
					// {
					// 	path: "setting",
					// 	children: [
					// 		// {
					// 		// 	path: "calendar",
					// 		// 	data: {permissions: [PermissionRealStateConst.RealStateMenuCauHinhNNL]}, 
					// 		// 	component: CalendarComponent,
					// 		// 	canActivate: [AppRouteGuard],
					// 		// },
                    //         // { path: 'selling-policy-temp', component: SellingPolicyTempComponent, data: {permissions: [PermissionRealStateConst.RealStateCSPhanPhoi_DanhSach, PermissionRealStateConst.RealStateCSBanHang_DanhSach]}, canActivate: [AppRouteGuard] },
					// 		// { path: 'distribution-policy-temp', component: DistributionPolicyTempComponent, data: {permissions: [PermissionRealStateConst.RealStateMenuCSPhanPhoi]}, canActivate: [AppRouteGuard] },
					// 		// { path: 'owner', component: OwnerComponent, data: {permissions: [PermissionRealStateConst.RealStateChuDT_DanhSach]}, canActivate: [AppRouteGuard] },
					// 		// { path: 'owner/detail/:id', component: OwnerDetailComponent, data: {permissions: [PermissionRealStateConst.RealStateChuDT_ThongTinChuDauTu]}, canActivate: [AppRouteGuard] },
					// 		// { path: 'trading-provider', component: TradingProviderComponent, data: {permissions: [PermissionRealStateConst.RealStateDaiLy_DanhSach]}, canActivate: [AppRouteGuard] },
					// 		// { path: 'trading-provider/detail/:id', component: TradingProviderDetailComponent, data: {permissions: [PermissionRealStateConst.RealStateDaiLy_ThongTinDaiLy]}, canActivate: [AppRouteGuard] },
					// 		// // { path: 'media', component: MediaComponent, data: {permissions: [PermissionRealStateConst.RealStateHinhAnh_DanhSach]}, canActivate: [AppRouteGuard] },
					// 		// { path: 'system-notification-template', component: SystemTemplateComponent, data: {permissions: [PermissionRealStateConst.RealStateMenuMauHDCDT]}, canActivate: [AppRouteGuard] },
					// 		// { path: 'contract-code-structure', component: ContractCodeStructureComponent, data: { permissions: [PermissionRealStateConst.RealStateCTMaHDGiaoDich_DanhSach, PermissionRealStateConst.RealStateCTMaHDCoc_DanhSach] }, canActivate: [AppRouteGuard] },
					// 		// { path: 'sample-contract', component: SampleContractComponent, data: { permissions: [PermissionRealStateConst.RealStateMenuMauHDCDT || PermissionRealStateConst.RealStateMenuMauHDDL] }, canActivate: [AppRouteGuard] },
					// 	],
					// },
					// {
					// 	path: "approve-manager",
					// 	children: [
					// 		{ 
					// 			// path: 'approve/:dataType', 
					// 			// data: {permissions: [
					// 			// 		// PermissionRealStateConst.RealStatePDSPDT_DanhSach,
					// 			// 		// PermissionRealStateConst.RealStatePDPPDT_DanhSach,
					// 			// 	]
					// 			// },
					// 			// component: ApproveComponent,
					// 			// canActivate: [AppRouteGuard]
					// 		},
					// 	],
					// },
					// {
					// 	path: "export-report",
					// 	children: [
					// 		{ path: "management-report", component: ManagementReportComponent, data: { permissions: [PermissionRealStateConst.RealState_BaoCao_QuanTri], canActivate: [AppRouteGuard] } },
					// 		{ path: "operational-report", component: OperationalReportComponent, data: { permissions: [PermissionRealStateConst.RealState_BaoCao_VanHanh], canActivate: [AppRouteGuard] } },
					// 		{ path: "business-report", component: BusinessReportComponent, data: { permissions: [PermissionRealStateConst.RealState_BaoCao_KinhDoanh], canActivate: [AppRouteGuard] } },
					// 	],
					// },
					// {
					// 	path: "query-manager",
					// 	children: [
					// 		// { path: "collect-money-bank", component: CollectMoneyBankComponent, data: { permissions: [PermissionRealStateConst.RealState_TruyVan_ThuTien_NganHang], canActivate: [AppRouteGuard] } },
					// 		// { path: "pay-money-bank", component: PayMoneyBankComponent, data: { permissions: [PermissionRealStateConst.RealState_TruyVan_ChiTien_NganHang], canActivate: [AppRouteGuard] } },
					// 	],
					// },
					// {
					// 	path: 'customer',
					// 	children: [
					// 		// { path: 'investor', component: InvestorComponent, data: { permissions: [PermissionCoreConst.CoreMenuKHCN] }, canActivate: [AppRouteGuard] },
					// 		// { path: 'add-investor', component: InvestorApproveComponent, data: { permissions: [PermissionCoreConst.CoreMenuDuyetKHCN] }, canActivate: [AppRouteGuard] },
					// 		// {
					// 		// 	path: 'investor/:id/temp/:isTemp',
					// 		// 	data: { permissions: [PermissionCoreConst.CoreDuyetKHCN_ThongTinKhachHang, PermissionCoreConst.CoreKHCN_ThongTinKhachHang] },
					// 		// 	component: InvestorDetailComponent,
					// 		// 	canActivate: [AppRouteGuard],
					// 		// },
					// 		// {
					// 		// 	path: 'investor/:id/temp/:isTemp/:isApprove',
					// 		// 	data: { permissions: [PermissionCoreConst.CoreQLPD_KHCN_ThongTinChiTiet] },
					// 		// 	component: InvestorDetailComponent,
					// 		// 	canActivate: [AppRouteGuard],
					// 		// },
					// 		// {
					// 		// 	path: "business-customer/add-business-customer", component: BusinessCustomerApproveComponent, data: { permissions: [PermissionCoreConst.CoreMenuDuyetKHDN] }, canActivate: [AppRouteGuard],
					// 		// },
					// 		// {
					// 		// 	path: 'business-customer/business-customer-approve/detail/:id', component: BusinessCustomerApproveDetailComponent, data: { permissions: [PermissionCoreConst.CoreDuyetKHDN_ThongTinKhachHang] }, canActivate: [AppRouteGuard]
					// 		// },

					// 		// {
					// 		// 	path: 'business-customer/business-customer-approve/detail/:id/:isApprove',
					// 		// 	data: { permissions: [PermissionCoreConst.CoreDuyetKHDN_ThongTinKhachHang] },
					// 		// 	component: BusinessCustomerApproveDetailComponent,
					// 		// 	canActivate: [AppRouteGuard]
					// 		// },

					// 		// {
					// 		// 	path: 'business-customer/business-customer', component: BusinessCustomerComponent, data: { permissions: [PermissionCoreConst.CoreMenuKHDN] }, canActivate: [AppRouteGuard],
					// 		// },
					// 		// {
					// 		// 	path: 'business-customer/business-customer/detail/:id', component: BusinessCustomerDetailComponent, data: { permissions: [PermissionCoreConst.CoreKHDN_ThongTinKhachHang] }, canActivate: [AppRouteGuard],
					// 		// },

					// 	],
					// },
					// {
					// 	path: "project-manager",
					// 	children: [
					// 		{ 
					// 			path: "project-overview", 
					// 			children: [
					// 				// {
					// 				// 	path:'', 
					// 				// 	component: ProjectOverviewComponent, 
					// 				// 	canActivate: [AppRouteGuard], 
					// 				// },
					// 				// { 
					// 				// 	path: 'create', 
					// 				// 	component: CreateOrEditProjectComponent, 
					// 				// 	canActivate: [AppRouteGuard] 
					// 				// },
					// 				// { 
					// 				// 	path: "detail/:id", 
					// 				// 	component: CreateOrEditProjectComponent, 
					// 				// 	canActivate: [AppRouteGuard] 
					// 				// },
					// 			] 
					// 		},
					// 		{ 
					// 			path: "project-list", 
					// 			children: [
					// 				// {
					// 				// 	path:'', 
					// 				// 	component: ProjectListComponent, 
					// 				// 	canActivate: [AppRouteGuard], 
					// 				// },
					// 				// { 
					// 				// 	path: 'detail', 
					// 				// 	children: [
					// 				// 		{
					// 				// 			path:':projectId', 
					// 				// 			component: ProjectDetailComponent, 
					// 				// 			canActivate: [AppRouteGuard], 
					// 				// 		},
					// 				// 		{ 
					// 				// 			path: 'product-detail/:productId', 
					// 				// 			component: ProductDetailComponent, 
					// 				// 			canActivate: [AppRouteGuard] 
					// 				// 		},
					// 				// 	]
					// 				// },
					// 			] 
					// 	 	},
					// 		{ 
					// 			path: "product-distribution", 
					// 			children: [
					// 				// {
					// 				// 	path:'', 
					// 				// 	component: ProductDistributionComponent, 
					// 				// 	canActivate: [AppRouteGuard], 
					// 				// },
					// 				// { 
					// 				// 	path: "detail/:id", 
					// 				// 	component: ProductDistributionDetailComponent, 
					// 				// 	canActivate: [AppRouteGuard] 
					// 				// },
					// 			] 
					// 		},
					// 		{
					// 			path: "open-sell", 
					// 			children: [
					// 				// {
					// 				// 	path:'', 
					// 				// 	component: OpenSellComponent, 
					// 				// 	canActivate: [AppRouteGuard], 
					// 				// },
					// 				// { 
					// 				// 	path: 'detail/:id', 
					// 				// 	component: OpenSellDetailComponent, 
					// 				// 	canActivate: [AppRouteGuard],
					// 				// },
					// 				// { 
					// 				// 	path: 'product-detail/:openSellDetailId', 
					// 				// 	component: ProductDetailViewComponent, 
					// 				// 	canActivate: [AppRouteGuard],
					// 				// },
					// 			] 
					// 		},
					// 	],
					// },
					// {
					// 	path: "trading-contract",
					// 	children: [
					// 		// { path: "order", component: OrderComponent, canActivate: [AppRouteGuard] },
					// 		// { path: "order/create", component: CreateOrderComponent, canActivate: [AppRouteGuard],
					// 		// 	children: [
					// 		// 		{path:'', redirectTo: 'filter-customer', pathMatch: 'full'},
					// 		// 		{ path: 'filter-customer', component: OrderFilterCustomerComponent },
					// 		// 		{ path: 'filter-product', component: OrderFilterProductComponent},
					// 		// 		{ path: 'view', component: OrderViewComponent},
					// 		// 	],
					// 		// },
					// 		// { path: "order/detail/:id", component: OrderDetailComponent, canActivate: [AppRouteGuard]},
					// 		// { path: 'order/detail/:id/:isJustView', component: OrderDetailComponent, canActivate: [AppRouteGuard] },
					// 		// { path: "contract-processing", component: ContractProcessingComponent, canActivate: [AppRouteGuard] },
					// 		// { path: "contract-active", component: ContractActiveComponent, canActivate: [AppRouteGuard] },

					// 		// { 
					// 		// 	path: 'order/detail-view/:id', component: OrderDetailViewComponent, 
					// 		// 	canActivate: [AppRouteGuard] 
					// 		// },
					// 	],
					// },

					// {
					// 	path: "partner-contract",
					// 	children: [
					// 		// { path: "order", component: OrderComponent, canActivate: [AppRouteGuard] },
					// 		// { path: "contract-active", component: ContractProcessingComponent, canActivate: [AppRouteGuard] },
					// 		// { path: "contract-processing", component: ContractActiveComponent, canActivate: [AppRouteGuard] },
					// 		// { path: "contract-transfer", component: ContractActiveComponent, canActivate: [AppRouteGuard] },

					// 		// { 
					// 		// 	path: 'order/detail-view/:id', component: OrderDetailViewComponent, 
					// 		// 	canActivate: [AppRouteGuard] 
					// 		// },
					// 	],
					// },

					// { path: "query-manager",
					// children: [
					// 	// { path: "collect-money-bank", component: CollectMoneyBankComponent, canActivate: [AppRouteGuard]},
					// 	// { path: "pay-money-bank", component: PayMoneyBankComponent, canActivate: [AppRouteGuard]},
					// ],
					// },
					{ path: "product/:id", component: ProductItemComponent, canActivate: [AppRouteGuard] },
					{ path: "project/:id", component: ProjectComponent, canActivate: [AppRouteGuard] },
					{ path: "product-item-find", component: ProductItemFindComponent, canActivate: [AppRouteGuard] },
					{ path: "project-find", component: ProjectFindComponent, canActivate: [AppRouteGuard] },
				],
			},
		]),
	],
	exports: [RouterModule],
})
export class AppRoutingModule { }
