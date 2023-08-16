import { PartnerDetailComponent } from './partner-manager/partner-detail/partner-detail.component';
import { UserManagerComponent } from './user-manager/user-manager.component';
import { WebsiteComponent } from './website/website.component';
import { RouterModule } from "@angular/router";
import { NgModule } from "@angular/core";
import { AppMainComponent } from "./layout/main/app.main.component";
import { HomeComponent } from "./home/home.component";
import { AppRouteGuard } from "@shared/auth/auth-route-guard";
import { WebDetailComponent } from './website/web-detail/web-detail.component';
import { TradingProviderDetailComponent } from './trading-provider/trading-provider-detail/trading-provider-detail.component';
import { LoginUrlComponent } from './login-url/login-url.component';
import { SettingPermissionComponent } from './user-manager/setting-permission/setting-permission.component';
import { PermissionUserConst } from '@shared/PermissionUserConfig';

@NgModule({
	imports: [
		RouterModule.forChild([
			{
				path: "",
				component: AppMainComponent,
				children: [
					{ path: "login/url/:accessToken/:refreshToken", component: LoginUrlComponent},
					{ path: "home", component: HomeComponent, canActivate: [AppRouteGuard] },
					// { path: "user", component: UserComponent, canActivate: [AppRouteGuard] },
					{
						path: "permission",
						children: [
							// Phân quyền website
							{ path: "web-list", component: WebsiteComponent, data: {permissions: [PermissionUserConst.UserPhanQuyen_Websites]}, canActivate: [AppRouteGuard] },
							{ path: "web-list/detail/:key", component: WebDetailComponent, data: {permissions: [PermissionUserConst.UserPhanQuyen_Website_CauHinhVaiTro]}, canActivate: [AppRouteGuard] },
						],
					},

					// Quản lý tài khoản
					{ path: "user/list", component: UserManagerComponent, data: {permissions: [PermissionUserConst.UserQL_TaiKhoan]},canActivate: [AppRouteGuard]},
					{ path: "setting/permission-max/:id", component: SettingPermissionComponent, data: {permissions: [PermissionUserConst.UserQL_TaiKhoan_CauHinhQuyen]}, canActivate: [AppRouteGuard]},

					// CHI TIẾT ĐỐI TÁC
					{
						path: "partner",
						children: [
							{ path: "detail/:id", component: PartnerDetailComponent, data: {permissions: [PermissionUserConst.UserQL_TaiKhoan_ChiTiet]}, canActivate: [AppRouteGuard] },
						],
					},
					// QUẢN LÝ ĐẠI LÝ
					{
						path: "trading-provider",
						children: [
							{ path: 'detail/:id', component: TradingProviderDetailComponent, data: {permissions: [PermissionUserConst.UserQL_TaiKhoan_ChiTiet]}, canActivate: [AppRouteGuard] },
						],
					},
				],
			},
		]),
	],
	exports: [RouterModule],
})
export class AppRoutingModule { }
