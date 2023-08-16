import { AppConsts } from '@shared/AppConsts';
import { Injectable } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { AppSessionService } from '../session/app-session.service';

import { CanActivate, Router, ActivatedRouteSnapshot, RouterStateSnapshot, CanActivateChild } from '@angular/router';
import { TokenService } from '@shared/services/token.service';
import { TokenAuthServiceProxy, UserServiceProxy } from '@shared/service-proxies/service-proxies';
import { MessageService } from 'primeng/api';
import { PermissionsService } from '@shared/services/permissions.service';
import { HelpersService } from '@shared/services/helpers.service';

@Injectable()
export class AppRouteGuard implements CanActivate, CanActivateChild {
  constructor(
    private _tokenService: TokenService,
    private _tokenAuthService: TokenAuthServiceProxy,
    private _userService: UserServiceProxy,
    private _permissionService: PermissionsService,
	private _helpersService: HelpersService,

  ) {}

  	permissions: string[] = [];
	canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Promise<boolean> {
		if (!this._tokenService.getToken()) {
			this._tokenAuthService.postRefreshToken().subscribe((res) => {
					return Promise.resolve(true);
				},(err) => {
					setTimeout(() => {
						window.location.href = AppConsts.baseUrlHome;
					}, 1500);
					return Promise.resolve(false);
				}
			);
		}
		
		return new Promise<boolean>((resolve) => {
			this._userService.getAllPermission().subscribe(response => {
				this.permissions = response.data;
				this._permissionService.setPermissions(this.permissions);
				if(!route?.data?.permissions) return resolve(true);
				if(this.permissions?.length) {
					for(let permission of route.data.permissions) {
						if(this.permissions.includes(permission)) {
							return resolve(true);
						}
					}
					//
					this._helpersService.messageError('Bạn không có quyền truy cập đường dẫn này!');
					return resolve(false);
0				}
				return resolve(false);
			});
		});
	}

	canActivateChild(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Promise<boolean> {
		return this.canActivate(route, state);
	}

	selectBestRoute(): string {
		return "/home";
	}
}
