import { Component, EventEmitter, Injector, OnInit, Output, ViewChild } from '@angular/core';
import { SearchConst } from '@shared/AppConsts';
import { CrudComponentBase } from '@shared/crud-component-base';
import { Page } from '@shared/model/page';
import { TradingProviderServiceProxy } from '@shared/service-proxies/setting-service';
import { MessageService } from 'primeng/api';
import { debounceTime } from 'rxjs/operators';
import { DynamicDialogRef } from 'primeng/dynamicdialog';
import { DynamicDialogConfig } from 'primeng/dynamicdialog';
import { ActivatedRoute } from '@angular/router';
import { BreadcrumbService } from 'src/app/layout/breadcrumb/breadcrumb.service';
import { TabView } from 'primeng/tabview';
import { FieldErrors } from '@shared/model/field-errors.model';
import { TradingProvider } from '@shared/model/trading-provider.model';
import { TabViewActive } from '@shared/model/tab-view-active.model';


@Component({
	selector: 'app-trading-provider-detail',
	templateUrl: './trading-provider-detail.component.html',
	styleUrls: ['./trading-provider-detail.component.scss']
})
export class TradingProviderDetailComponent extends CrudComponentBase {

	constructor(
		injector: Injector,

		messageService: MessageService,
		private routeActive: ActivatedRoute,
		private tradingProviderService: TradingProviderServiceProxy,
		private breadcrumbService: BreadcrumbService
	) {
		super(injector, messageService);

		this.breadcrumbService.setItems([
			{ label: 'Trang chủ', routerLink: ['/home'] },
			{ label: 'Đại lý', routerLink: ['/setting/trading-provider'] },
			{ label: 'Chi tiết đại lý', },
		]);

		this.tradingProviderId = +this.cryptDecode(this.routeActive.snapshot.paramMap.get('id'));

	}

	fieldErrors : FieldErrors;
	fieldDates = ['licenseDate', 'decisionDate', 'dateModified'];


	tradingProviderId: number;
	tradingProviderDetail: TradingProvider;
	isEdit = false;
	labelButtonEdit = "Chỉnh sửa";

	nations = [
		{
			name: "Việt Nam",
			code: "VN",
		},
		{
			name: "Thái Lan",
			code: "TL",
		},
		{
			name: "Trung Quốc",
			code: "TQ",
		}
	];

	tabViewActive: TabViewActive= {
		'thongTinChung': true,
		'taiKhoanDangNhap': false,
	};
	
	@ViewChild(TabView) tabView: TabView;

	ngOnInit(): void {
		this.isLoading = true;
		this.tradingProviderService.getTradingProvider(this.tradingProviderId).subscribe(
			(res) => {
				this.isLoading = false;
				if (this.handleResponseInterceptor(res, '')) {
					if (res.data) {
					  this.tradingProviderDetail = {
						...res.data,
						licenseDate: res.data.licenseDate ? new Date(res.data.licenseDate) : null,
						decisionDate: res.data.decisionDate ? new Date(res.data.decisionDate) : null,
						dateModified: res.data.dateModified ? new Date(res.data.dateModified) : null,
					  };
					}
				  }
			}, (err) => {
				this.isLoading = false;
				console.log('Error-------', err);
				
			});
	}

	changeTabview(e) {
		let tabHeader = this.tabView.tabs[e.index].header;
		this.tabViewActive[tabHeader] = true;
	}

	setFieldError() {
		for (const [key, value] of Object.entries(this.tradingProviderDetail)) {
			this.fieldErrors[key] = false;
		}
		console.log({ filedError: this.fieldErrors });
	}

	resetValid(field) {
		this.fieldErrors[field] = false;
	}

	changeEdit() {
		console.log(this.tradingProviderDetail);
		if (this.isEdit) {
			let body = this.formatCalendar(this.fieldDates, {...this.tradingProviderDetail});
			this.tradingProviderService.update(body).subscribe((response) => {
				if (this.handleResponseInterceptor(response, 'Cập nhật thành công')) {
					this.isEdit = !this.isEdit;
					this.labelButtonEdit = this.isEdit ? "Lưu lại" : "Chỉnh sửa";
				} 
			});
		} else {
			this.isEdit = !this.isEdit;
			this.labelButtonEdit = this.isEdit ? "Lưu lại" : "Chỉnh sửa";
		}
	}
}
