import { MessageService } from 'primeng/api';
import { UserServiceProxy } from '@shared/service-proxies/service-proxies';
import { AppSessionService } from '@shared/session/app-session.service';
import { Component, Inject, Injector } from '@angular/core';
import { AppAuthService } from '@shared/auth/app-auth.service';
import { AppMainComponent } from '../main/app.main.component';
import { CrudComponentBase } from '@shared/crud-component-base';
import { API_BASE_URL } from '@shared/service-proxies/service-proxies-base';
import { AppConsts } from '@shared/AppConsts';

@Component({
	selector: 'app-topbar',
	templateUrl: './app.topbar.component.html',
	styleUrls: ['./app.topbar.component.scss'],
})
export class AppTopBarComponent extends CrudComponentBase {

	userDialog: boolean;
	activeItem: number;
	userInfo: any = {};
	user = {};
	isLoadingPage = false;

	constructor(
		injector: Injector,
		messageService: MessageService,
		public appMain: AppMainComponent,
		private authService: AppAuthService,
		private _appSessionService: AppSessionService,
		private userService: UserServiceProxy,
        @Inject(API_BASE_URL) baseUrl?: string,
	) { 
		super(injector, messageService);
        this.baseUrl = baseUrl !== undefined && baseUrl !== null ? baseUrl : "";
	}

    private baseUrl: string;
	avatar: string;

	mobileMegaMenuItemClick(index) {
		this.appMain.megaMenuMobileClick = true;
		this.activeItem = this.activeItem === index ? null : index;
	}

	ngOnInit() {
        this.baseUrl = AppConsts.remoteServiceBaseUrl ?? this.baseUrl;
		this.userInfo = this._appSessionService.user;
		this.userService.getPrivacyInfo().subscribe(res => {
			if (res?.data.avatarImageUrl) {
				this.avatar =  `${this.baseUrl}/${ res?.data?.avatarImageUrl}`;
			} else {
				this.avatar = AppConsts.defaultAvatar;
			}
		})
	}

	editUser() {
		this.user = { ...this.userInfo };
		this.userDialog = true;
	}

	hideDialog() {
		this.userDialog = false;
	}

	saveUser() {
		this.userService.update(this.user).subscribe((res) => {
			this.userInfo = { ...this.user };
			this.userDialog = false;
			this.messageService.add({ severity: 'success', summary: '', detail: 'Cập nhật thành công!', life: 1500 });
			this.userService.postRefreshToken().subscribe();
		});
	}

	logout() {
		this.isLoadingPage = true;
		this.userService.logoutSSO().subscribe(res => {
			this.callLogout();
		}, (err) => {
			console.log('errLogoutSSO___', err);
			this.callLogout();
		});
	}

	callLogout() {
		setTimeout(() => {
			this.authService.logout();
		}, 300);
	}

}
