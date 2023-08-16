import { MessageService } from 'primeng/api';
import { UserServiceProxy } from '@shared/service-proxies/service-proxies';
import { AppSessionService } from '@shared/session/app-session.service';
import {Component, Inject, Injector} from '@angular/core';
import { AppAuthService } from '@shared/auth/app-auth.service';
import { AppMainComponent } from '../main/app.main.component';
import { CrudComponentBase } from '@shared/crud-component-base';
import { AppConsts, PermissionRealStateConst, TRADING_CONTRACT_ROUTER, UserTypes } from '@shared/AppConsts';
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
		private _appSessionService: AppSessionService,
		private userService: UserServiceProxy,
		private _tradingProviderService: TradingProviderServiceProxy,
		private _tradingProviderSelectedService: TradingProviderSelectedService,
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
    UserTypes = UserTypes;

	isPartner: boolean;
	tradingProviders: any;
	tradingProviderIds:[];
	avatar: string;
	ngOnInit() {
        this.baseUrl = AppConsts.remoteServiceBaseUrl ?? this.baseUrl;
		this.isPartner = this.getIsPartner();
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
			this.tradingProviderIds = this.getLocalStorage('tradingProviderIdsRst') ?? []; 
			AppConsts.tradingProviderIds = this.tradingProviderIds
			this._tradingProviderSelectedService.setSelectedOptions(this.tradingProviderIds);
		}
	}

	changeTradingProviderIds(tradingProviderIds: []) {
		this.tradingProviderIds = tradingProviderIds;
		AppConsts.tradingProviderIds = tradingProviderIds
		this.setLocalStorage(this.tradingProviderIds, 'tradingProviderIdsRst')
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
			this.messageSuccess("Cập nhật thành công!");
			this.userService.postRefreshToken().subscribe();
		});
	  }

	logout() {
		this.authService.logout();
	}

	public get isShowTradingProviders() {
		return this.isPartner && this.isGranted([PermissionRealStateConst.RealStateMenuQLGiaoDichCoc]) && this.router.url.includes(TRADING_CONTRACT_ROUTER);
	}
}
