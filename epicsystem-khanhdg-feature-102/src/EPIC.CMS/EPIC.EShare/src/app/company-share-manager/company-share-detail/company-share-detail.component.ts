import { Component, Injector, OnInit } from '@angular/core';
import { CompanyShareDetailConst, CompanyShareInfoConst } from '@shared/AppConsts';
import { CrudComponentBase } from '@shared/crud-component-base';
import { Page } from '@shared/model/page';
import { CompanyShareDetailServiceProxy } from '@shared/service-proxies/company-share-manager-service';
import { MessageService } from 'primeng/api';
import { BreadcrumbService } from 'src/app/layout/breadcrumb/breadcrumb.service';

@Component({
	selector: 'app-company-share-detail',
	templateUrl: './company-share-detail.component.html',
	styleUrls: ['./company-share-detail.component.scss']
})
export class CompanyShareDetailComponent extends CrudComponentBase {

	constructor(
		injector: Injector,
		private _companyShareDetailService: CompanyShareDetailServiceProxy,
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
	CompanyShareDetailConst = CompanyShareDetailConst;
	// companyShareDetailConst = companyShareDetailConst;
	CompanyShareInfoConst = CompanyShareInfoConst;

	companyShareDetail: any = {
		"companyShareDetailId": 0,
		"companyShareId": 0,
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
		// this.companyShareDetail = {};
		this.submitted = false;
		this.modalDialog = true;
	}

	deleteSelectedItems() {
		this.deleteItemsDialog = true;
	}

	edit(companyShareDetail) {
		this.companyShareDetail = { ...companyShareDetail };
		this.modalDialog = true;
	}

	delete(companyShareDetail) {
		this.deleteItemDialog = true;
		this.companyShareDetail = { ...companyShareDetail };
	}

	changeCurrent() {
		console.log(this.companyShareDetail.capital);
	}

	confirmDelete() {
		this.deleteItemDialog = false;
		this._companyShareDetailService.delete(this.companyShareDetail.userId).subscribe(
			(response) => {
				if (this.handleResponseInterceptor(response, 'Xóa thành công')) {
					this.setPage({ page: this.page.pageNumber });
					this.companyShareDetail = {};
				}
			}, () => {
				this.messageService.add({
					severity: 'error',
					summary: '',
					detail: `Không xóa được tài khoản ${this.companyShareDetail.name}`,
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

		this._companyShareDetailService.getAll(this.page).subscribe((res) => {
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
		if (this.companyShareDetail.companyShareDetailId) {
			this._companyShareDetailService.update(this.companyShareDetail).subscribe(
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

			console.log({ companyShareDetail: this.companyShareDetail })
			this._companyShareDetailService.create(this.companyShareDetail).subscribe(
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

		const validRequired = this.companyShareDetail?.code?.trim() && this.companyShareDetail?.name?.trim();
		return validRequired;
	}
}
