import { API_BASE_URL } from '@shared/service-proxies/service-proxies-base';
import { Injectable } from "@angular/core";
import { ActivatedRoute } from "@angular/router";
import { AppSessionService } from "../session/app-session.service";

import { CanActivate, Router, ActivatedRouteSnapshot, RouterStateSnapshot, CanActivateChild } from "@angular/router";
import { TokenService } from "@shared/services/token.service";
import { TokenAuthServiceProxy, UserServiceProxy } from "@shared/service-proxies/service-proxies";
import { AppConsts } from '@shared/AppConsts';
import { MessageService } from 'primeng/api';

@Injectable()
export class AppRouteGuard implements CanActivate, CanActivateChild {
	constructor(
		private _route: ActivatedRoute,
		private _router: Router,
		private _sessionService: AppSessionService,
		private _tokenService: TokenService,
		private _tokenAuthService: TokenAuthServiceProxy,
		private _userService: UserServiceProxy,
		private messageService: MessageService,
	) {}

	canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Promise<boolean> {
		if (!this._tokenService.getToken()) {
			this._tokenAuthService.postRefreshToken().subscribe((res) => {
					return Promise.resolve(true);
				},(err) => {
					setTimeout(() => {
						window.location.href = AppConsts.baseUrlHome;
					}, 1500);
					return Promise.resolve(true);
				}
			);
		}

		if (!route.data || !route.data["permissions"]) {
			return Promise.resolve(true);
		}

		if (route?.data["permissions"]) {
			return new Promise<boolean>((resolve) => {
				this._userService.getAllPermission().subscribe(res => {
						if(res?.data.length && route?.data["permissions"]?.length) {
							for(let permission of route?.data["permissions"]) {
								if(res.data.includes(permission)) {
									return resolve(true);
								}
							}
							//
							this.messageService.add({ severity: 'error', detail: 'Bạn không có quyền truy cập đường dẫn này!', life: 2000});
							this._router.navigate([this.selectBestRoute()]);
							return resolve(false);
						}
					}
				);
			});
		}

		this._router.navigate([this.selectBestRoute()]);
		return Promise.resolve(false);
	}

	canActivateChild(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Promise<boolean> {
		return this.canActivate(route, state);
	}

	selectBestRoute(): string {
		return "/home";
	}
}
