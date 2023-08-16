import { Component, Injector, OnInit } from '@angular/core';
import { ProductBondDetailConst, ProductBondInfoConst } from '@shared/AppConsts';
import { CrudComponentBase } from '@shared/crud-component-base';
import { Page } from '@shared/model/page';
import { ProductBondDetailServiceProxy } from '@shared/service-proxies/bond-manager-service';
import { MessageService } from 'primeng/api';
import { BreadcrumbService } from 'src/app/layout/breadcrumb/breadcrumb.service';

@Component({
	selector: 'app-product-bond-detail',
	templateUrl: './product-bond-detail.component.html',
	styleUrls: ['./product-bond-detail.component.scss']
})
export class ProductBondDetailComponent extends CrudComponentBase {

	constructor(
		injector: Injector,
		private _productBondDetailService: ProductBondDetailServiceProxy,
		messageService: MessageService,
		private breadcrumbService: BreadcrumbService) {
		super(injector, messageService);
		([
			{ label: 'Tổ chức phát hành' },
		]);
	}

	modalDialog: boolean;

	deleteItemDialog: boolean = false;

	deleteItemsDialog: boolean = false;

	rows: any[] = [];
	ProductBondDetailConst = ProductBondDetailConst;
	// productBondDetailConst = productBondDetailConst;
	ProductBondInfoConst = ProductBondInfoConst;

	productBondDetail: any = {
		"bondDetailId": 0,
		"productBondId": 0,
		"tradingProviderId": 0,
		"code": "",
		"name": "",
		"period": null,
		"periodUnit": "",
		"interestRate": 0,
		"interestPeriod": 0,
		"interestPeriodUnit": "",
		"issueDate": null,
		"dueDate": null,
		"lastOrderDate": null,
		"parValue": 0,
		"totalValue": 0,
		"allowOnlineTrading": null,
		"market": null,
		"policyIds": [],
	}

	submitted: boolean;

	cols: any[];

	page = new Page();
	offset = 0;

	ngOnInit() {
		this.setPage({ page: this.offset });
	}

	create() {
		// this.productBondDetail = {};
		this.submitted = false;
		this.modalDialog = true;
	}

	deleteSelectedItems() {
		this.deleteItemsDialog = true;
	}

	edit(productBondDetail) {
		this.productBondDetail = { ...productBondDetail };
		this.modalDialog = true;
	}

	delete(productBondDetail) {
		this.deleteItemDialog = true;
		this.productBondDetail = { ...productBondDetail };
	}

	changeCurrent() {
		console.log(this.productBondDetail.capital);
	}

	confirmDelete() {
		this.deleteItemDialog = false;
		this._productBondDetailService.delete(this.productBondDetail.userId).subscribe(
			(response) => {
				if (this.handleResponseInterceptor(response, 'Xóa thành công')) {
					this.setPage({ page: this.page.pageNumber });
					this.productBondDetail = {};
				}
			}, () => {
				this.messageService.add({
					severity: 'error',
					summary: '',
					detail: `Không xóa được tài khoản ${this.productBondDetail.name}`,
					life: 3000,
				});
			}
		);
	}

	changeKeyword() {
		if (this.keyword === '') {
			this.setPage({ page: this.offset });
		}
	}

	setPage(pageInfo?: any) {
		this.page.pageNumber = pageInfo?.page ?? this.offset;
		this.page.keyword = this.keyword;
		this.isLoading = true;

		this._productBondDetailService.getAll(this.page).subscribe((res) => {
			this.isLoading = false;
			if (this.handleResponseInterceptor(res, '')) {
				this.page.totalItems = res.data.totalItems;
				this.rows = res.data.items;
				// this.rows = res.data.items;
				console.log({ rows: res.data.items, totalItems: res.data.totalItems });
			}
		}, (err) => {
			this.isLoading = false;
			console.log('Error-------', err);
			
		});
		// fix show dropdown options bị ẩn dướ
	}

	hideDialog() {
		this.modalDialog = false;
		this.submitted = false;
	}

	save() {
		this.submitted = true;
		//
		if (this.productBondDetail.productBondDetailId) {
			this._productBondDetailService.update(this.productBondDetail).subscribe(
				(response) => {
					if (this.handleResponseInterceptor(response, 'Cập nhật thành công')) {
						this.submitted = false;
						this.setPage({ page: this.page.pageNumber });
						this.hideDialog();
					} else {
						this.submitted = false;
					}
				}, () => {
					this.submitted = false;
				}
			);
		} else {

			console.log({ productBondDetail: this.productBondDetail })
			this._productBondDetailService.create(this.productBondDetail).subscribe(
				(response) => {
					if (this.handleResponseInterceptor(response, 'Thêm thành công')) {
						this.submitted = false;
						this.setPage();
						this.hideDialog();
					} else {
						this.submitted = false;
					}
				}, () => {
					this.submitted = false;
				}
			);
		}
	}

	validForm(): boolean {

		const validRequired = this.productBondDetail?.code?.trim() && this.productBondDetail?.name?.trim();
		return validRequired;
	}
}
