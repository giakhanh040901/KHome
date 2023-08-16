import { Component, Injector, OnInit } from "@angular/core";
import { MessageService } from "primeng/api";
import { BreadcrumbService } from "../layout/breadcrumb/breadcrumb.service";
import { BusinessCustomerServiceProxy } from "@shared/service-proxies/business-customer-service";
import { CrudComponentBase } from "@shared/crud-component-base";
import { PermissionCoreConst, UserTypes } from "@shared/AppConsts";

@Component({
	selector: "app-detail-business",
	templateUrl: "./detail-business.component.html",
	styleUrls: ["./detail-business.component.scss"],
})
export class DetailBusinessComponent extends CrudComponentBase {
	constructor(
		injector: Injector,
		messageService: MessageService,
		private breadcrumbService: BreadcrumbService,
		private _businessCustomerService: BusinessCustomerServiceProxy,
	) {
		super(injector, messageService);
		this.breadcrumbService.setItems([
			{ label: 'Trang chủ', routerLink: ['/home'] },
			{ label: 'Thông tin doanh nghiệp',},
		]);
		this.userLogin = this.getUser();
        console.log('userLogin____', this.userLogin); 
	}

	UserTypes = UserTypes;

	userLogin: any = {};
	partnerId: number;

	businessCustomerId: number;

	PermissionCoreConst = PermissionCoreConst;

	ngOnInit(): void {
		this.isLoading = true;
		if (UserTypes.TYPE_TRADING_PROVIDERS.includes(this.userLogin.user_type)){
			this._businessCustomerService.getInfoLogin().subscribe((res) => {
				if (this.handleResponseInterceptor(res, '')) {
					this.businessCustomerId = res.data.businessCustomerId;
				}
				this.isLoading = false;
			}, (err) => {
				this.isLoading = false;
				console.log('Error-------', err);
				this.messageError('Có lỗi xảy ra. Vui lòng thử lại sau!');
			})
		}

		if (UserTypes.TYPE_PARTNERS.includes(this.userLogin.user_type)){
			this.partnerId = Number(this.userLogin.partner_id);
			this.isLoading = false;	
		}

	}
}
