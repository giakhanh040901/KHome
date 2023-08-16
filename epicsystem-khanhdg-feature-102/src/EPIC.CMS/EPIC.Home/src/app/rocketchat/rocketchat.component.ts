import { Component, Injector, OnDestroy, OnInit } from "@angular/core";
import { NavigationStart, Router } from "@angular/router";
import { AppConsts } from "@shared/AppConsts";
import { CrudComponentBase } from "@shared/crud-component-base";
import { UserServiceProxy } from "@shared/service-proxies/service-proxies";
import { MessageService } from "primeng/api";
import { BreadcrumbService } from "src/app/layout/breadcrumb/breadcrumb.service";

@Component({
	selector: "app-rocketchat",
	templateUrl: "./rocketchat.component.html",
	styleUrls: ["./rocketchat.component.scss"],
})
export class RocketchatComponent extends CrudComponentBase implements OnDestroy {
	constructor(injector: Injector, messageService: MessageService, private _userServices: UserServiceProxy, private router: Router, private breadcrumbService: BreadcrumbService) {
		super(injector, messageService);
		this.breadcrumbService.setItems([
			{ label: "Trang chủ", routerLink: ["/home"] },
			{ label: "Đối tác", routerLink: ["/partner-manager/partner"] },
		]);
	}

	public src: string = AppConsts.rocketchat.iframeSrc;
	private subscription = null;

	ngOnInit(): void {
		this._userServices.getChannelName().subscribe(
			(res: any) => {
				console.log('res: ', res);
				const formatted = JSON.parse(res.body);
				if (this.handleResponseInterceptor(formatted, '')) {
					const path = formatted.data.channelPath ? `/${formatted.data.channelPath}/` : '/';

					this.src = `${AppConsts.rocketchat.iframeSrc}/?layout=embedded`;
					
					this.loginSSORocketchat();
				} else {
					this.src = `${AppConsts.rocketchat.iframeSrc}/channel/nothing/?layout=embedded`;
				}
			},
			err => {
				console.log(err);
				this.src = `${AppConsts.rocketchat.iframeSrc}/channel/nothing/?layout=embedded`;
			}
		)
		
	}

	loginSSORocketchat() {
		this._userServices.loginRocketchat().subscribe(
			(res) => {
				// if (this.handleResponseInterceptor(res, '')) {
				// }
			},
			(err) => {
				this.messageError("Có sự có khi sso. Vui lòng đăng nhập.");
			}
		);
	}

	ngOnDestroy(): void {
		this._userServices.logoutSSO().subscribe();
	}
}
