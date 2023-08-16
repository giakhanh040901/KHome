import { MessageService } from 'primeng/api';
import { UserServiceProxy } from '@shared/service-proxies/service-proxies';
import { AppSessionService } from '@shared/session/app-session.service';
import {Component, Inject, Injector} from '@angular/core';
import { AppAuthService } from '@shared/auth/app-auth.service';
import { AppMainComponent } from '../main/app.main.component';
import { CrudComponentBase } from '@shared/crud-component-base';
import { AppConsts, TRADING_CONTRACT_ROUTER, UserTypes } from '@shared/AppConsts';
import { TradingProviderServiceProxy } from '@shared/service-proxies/setting-service';
import { TradingProviderSelectedService } from '@shared/services/trading-provider.service';
import { Router } from '@angular/router';
import { API_BASE_URL } from '@shared/service-proxies/service-proxies-base';
import { forkJoin } from 'rxjs';

@Component({
    selector: 'app-topbar',
	templateUrl: './app.topbar.component.html',
})
export class AppTopBarComponent extends CrudComponentBase {
    constructor(
		injector: Injector,
		messageService: MessageService,
		public appMain: AppMainComponent,
		private authService: AppAuthService,
		private _tradingProviderService: TradingProviderServiceProxy,
		private _tradingProviderSelectedService: TradingProviderSelectedService,
		private _appSessionService: AppSessionService,
		private userService: UserServiceProxy,
		private router: Router,
        @Inject(API_BASE_URL) baseUrl?: string,

	) {
		super(injector, messageService);
        this.userLogin = this.getUser();
		this.tradingProviderIds = [];
        this.baseUrl = baseUrl !== undefined && baseUrl !== null ? baseUrl : "";
	}

    private baseUrl: string;
	userDialog: boolean;
    activeItem: number;
	userInfo: any = {};
	user = {};

    userLogin: any = {};
    UserTypes = UserTypes;

	tradingProviders: any;
	tradingProviderIds:[];
	isPartner: boolean;
	avatar: string;

	ngOnInit() {
        this.baseUrl = AppConsts.remoteServiceBaseUrl ?? this.baseUrl;

		if (this.UserTypes.TYPE_PARTNERS.includes(this.userLogin.user_type)){
			this.isPartner = true;
		}
		forkJoin([this._tradingProviderService.getAllTradingProviderNoPermision(), this.userService.getPrivacyInfo()]).subscribe(([resTrading, resUser]) => {
			if (this.handleResponseInterceptor(resTrading, '')) {
				this.tradingProviders = resTrading?.data?.items; 
			}
			if (this.handleResponseInterceptor(resUser, '')) {
				if (resUser?.data.avatarImageUrl) {
					this.avatar =  `${this.baseUrl}/${resUser?.data?.avatarImageUrl}`;
				} else {
					this.avatar = AppConsts.defaultAvatar;
				}
			}
		});
		this.userInfo = this._appSessionService.user;
		if (UserTypes.TYPE_PARTNERS.includes(this.userLogin.user_type)){
			this.tradingProviderIds = this.getLocalStorage('tradingProviderIdsGar') ?? []; 
			this._tradingProviderSelectedService.setSelectedOptions(this.tradingProviderIds);
		}
	}

	changeTradingProviderIds(tradingProviderIds: any) {
		this.tradingProviderIds = tradingProviderIds;
		console.log('topbar ', this.tradingProviderIds);
		this.setLocalStorage(this.tradingProviderIds, 'tradingProviderIdsGar')
	    this._tradingProviderSelectedService.setSelectedOptions(this.tradingProviderIds);
	}

	mobileMegaMenuItemClick(index) {
        this.appMain.megaMenuMobileClick = true;
        this.activeItem = this.activeItem === index ? null : index;
    }

	editUser() {
        this.user = {...this.userInfo};
        this.userDialog = true;
    }

	hideDialog() {
        this.userDialog = false;
    }

	saveUser() {
		this.userService.update(this.user).subscribe((res) => {
			this.userInfo = {...this.user};
			this.userDialog = false;
			this.messageService.add({severity: 'success', summary: '', detail: 'Cập nhật thành công!', life: 1500});
			this.userService.postRefreshToken().subscribe();
		});
	  }

	logout() {
		this.authService.logout();
	}

	public get isShowTradingProviders() {
		return this.isPartner && this.isGranted([this.PermissionGarnerConst.GarnerMenuHDPP]) && this.router.url.includes(TRADING_CONTRACT_ROUTER);
	}
}
