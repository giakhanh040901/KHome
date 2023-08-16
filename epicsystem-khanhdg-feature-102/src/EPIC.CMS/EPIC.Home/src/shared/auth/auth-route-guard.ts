import { API_BASE_URL } from '@shared/service-proxies/service-proxies-base';
import { Injectable } from "@angular/core";
import { ActivatedRoute } from "@angular/router";
import { AppSessionService } from "../session/app-session.service";

import { CanActivate, Router, ActivatedRouteSnapshot, RouterStateSnapshot, CanActivateChild } from "@angular/router";
import { TokenService } from "@shared/services/token.service";
import { TokenAuthServiceProxy } from "@shared/service-proxies/service-proxies";
import { AppConsts } from '@shared/AppConsts';

@Injectable()
export class AppRouteGuard implements CanActivate, CanActivateChild {
	constructor(
		private _route: ActivatedRoute,
		private _router: Router,
		private _sessionService: AppSessionService,
		private _tokenService: TokenService,
		private _tokenAuthService: TokenAuthServiceProxy,
		private _appSessionService: AppSessionService,

	) {}

	canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Promise<boolean> {
		if (!this._sessionService.user || !this._tokenService.getToken()) {
			this._tokenAuthService.postRefreshToken().subscribe((res) => {
					this._appSessionService.init();
					return Promise.resolve(true);
				},(err) => {
					setTimeout(() => {
						this._router.navigate(['/account/login']);
					}, 1000);
					return Promise.resolve(false);
				}
			);
		}

		if (!route.data || !route.data["permission"]) {
			return Promise.resolve(true);
		}

		this._router.navigate([this.selectBestRoute()]);
		return Promise.resolve(false);
	}

	canActivateChild(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Promise<boolean> {
		return this.canActivate(route, state);
	}

	selectBestRoute(): string {
		if (!this._sessionService.user) {
			window.location.href = AppConsts.appBaseUrl + '/account/login';
			// return "/account/login";
		}
		return "/home";
	}
}
