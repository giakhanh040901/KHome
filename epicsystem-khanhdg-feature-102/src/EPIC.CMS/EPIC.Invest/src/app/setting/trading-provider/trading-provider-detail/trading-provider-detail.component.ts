import { Component, Injector, Input, ViewChild } from '@angular/core';
import { CrudComponentBase } from '@shared/crud-component-base';
import { MessageService } from 'primeng/api';
import { ActivatedRoute } from '@angular/router';
import { BreadcrumbService } from 'src/app/layout/breadcrumb/breadcrumb.service';
import { TabView } from 'primeng/tabview';
import { TradingProviderService } from '@shared/services/trading-provider.service';


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
		private tradingProviderService: TradingProviderService,
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

	fieldDates = ['licenseDate', 'decisionDate', 'dateModified'];

	tradingProviderId: number;
	tradingProviderDetail: any;
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

	tabViewActive = {
		'thongTinChung': true,
		'taiKhoanDangNhap': false,
	};
	
	@Input() contentHeights: number[] = [];

	@ViewChild(TabView) tabView: TabView;

	ngOnInit(): void {
		this.isLoading = true;
		this.tradingProviderService.getTradingProvider(this.tradingProviderId).subscribe(
			(res) => {
				this.isLoading = false;
				if (this.handleResponseInterceptor(res, '')) {
					this.tradingProviderDetail = res.data;
					this.tradingProviderDetail = {
						...this.tradingProviderDetail,
						licenseDate: this.tradingProviderDetail?.licenseDate ? new Date(this.tradingProviderDetail?.licenseDate) : null,
						decisionDate: this.tradingProviderDetail?.decisionDate ? new Date(this.tradingProviderDetail?.decisionDate) : null,
						dateModified: this.tradingProviderDetail?.dateModified ? new Date(this.tradingProviderDetail?.dateModified) : null,
					};
					console.log({ tradingProviderDetail: this.tradingProviderDetail });
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
