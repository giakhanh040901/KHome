import { Component, Injector, Input, OnInit } from '@angular/core';
import { IssuerConst, BusinessCustomerConst, SearchConst } from '@shared/AppConsts';
import { CrudComponentBase } from '@shared/crud-component-base';
import { Page } from '@shared/model/page';
import { TradingProviderServiceProxy } from '@shared/service-proxies/setting-service';
import { debounceTime } from 'rxjs/operators';
import { BreadcrumbService } from 'src/app/layout/breadcrumb/breadcrumb.service';
import { Router } from "@angular/router"
import { ActivatedRoute } from '@angular/router';
import { DialogService } from 'primeng/dynamicdialog';
import { DynamicDialogRef } from 'primeng/dynamicdialog';
import { ConfirmationService, MessageService } from 'primeng/api';
import { FilterBusinessCustomerComponent } from 'src/app/setting/issuer/filter-business-customer/filter-business-customer.component';
import { FormSetDisplayColumnComponent } from 'src/app/form-set-display-column/form-set-display-column/form-set-display-column.component';
import { NationalityConst } from '@shared/nationality-list';

@Component({
	selector: 'app-trading-provider',
	templateUrl: './trading-provider.component.html',
	styleUrls: ['./trading-provider.component.scss'],
	providers: [DialogService, ConfirmationService, MessageService]
})
export class TradingProviderComponent extends CrudComponentBase {

	constructor(
		injector: Injector,
		messageService: MessageService,
		private confirmationService: ConfirmationService,
		private router: Router,
		private dialogService: DialogService,
		private tradingProviderService: TradingProviderServiceProxy,
		private breadcrumbService: BreadcrumbService) {
		super(injector, messageService);
		this.breadcrumbService.setItems([
			{ label: 'Trang chủ', routerLink: ['/home'] },
			{ label: 'Đại lý sơ cấp' },
		]);
	}

	ref: DynamicDialogRef;
	modalDialog: boolean;

	deleteItemDialog: boolean = false;

	confirmRequestDialog: boolean = false;
	rows: any[] = [];

	BusinessCustomerConst = BusinessCustomerConst;
	NationalityConst = NationalityConst;

	businessCustomer: any;

	tradingProvider: any = {
		businessCustomerId: null,
		aliasName: null
	  };

	businessCustomerId: number;

	listAction: any[] = [];
	submitted: boolean;

	statuses: any[];

	page = new Page();
	offset = 0;
	//
	actions: any[] = [];  // list button actions
	actionsDisplay: any[] = [];
	row: any;
	col: any;

	cols: any[];
	_selectedColumns: any[];

	ngOnInit(): void {
		this.setPage({ page: this.offset });
		this.subject.keyword.pipe(debounceTime(SearchConst.DEBOUNCE_TIME)).subscribe(() => {
			if (this.keyword === "") {
				this.setPage({ page: this.offset });
			} else {
				this.setPage();
			}
		});

		this.cols = [
			{ field: 'code', header: 'Mã doanh nghiệp', width: '10rem', position: 1, cutText: 'b-cut-text-20', isPin: true },
			{ field: 'name', header: 'Tên doanh nghiệp', width: '20rem', position: 1, cutText: 'b-cut-text-20', isPin: true },
			{ field: 'shortName', header: 'Tên viết tắt', width: '12rem', position: 2, cutText: 'b-cut-text-12' },
			{ field: 'email', header: 'Email', width: '20rem', position: 3, cutText: 'b-cut-text-20' },
			{ field: 'phone', header: 'SĐT', width: '12rem', position: 4, cutText: 'b-cut-text-12' },
			{ field: 'address', header: 'Địa chỉ', width: '40rem', position: 5, cutText: 'b-cut-text-40' },
			{ field: 'repName', header: 'Người đại diện', width: '12rem', position: 6, cutText: 'b-cut-text-12' },
			{ field: 'repPosition', header: 'Chức vụ', width: '12rem', position: 7, cutText: 'b-cut-text-12' },
		];

		this._selectedColumns = this.cols;
		// this._selectedColumns = this.getLocalStorage('tradingProvider') ?? this.cols;
	}

	getLocalStorage(key) {
		return JSON.parse(localStorage.getItem(key))
	}
	setLocalStorage(data) {
		return localStorage.setItem('tradingProvider', JSON.stringify(data));
	}

	setColumn(col, _selectedColumns) {
		console.log('cols:', col);

		console.log('123', _selectedColumns);

		const ref = this.dialogService.open(
			FormSetDisplayColumnComponent,
			this.getConfigDialogServiceDisplayTableColumn("Sửa cột hiển thị", col, _selectedColumns)
		);
		//
		ref.onClose.subscribe((dataCallBack) => {
			console.log('dataCallBack', dataCallBack);
			if (dataCallBack?.accept) {
				this._selectedColumns = dataCallBack.data.sort(function (a, b) {
					return a.position - b.position;
				});
				this.setLocalStorage(this._selectedColumns)
				console.log('Luu o local', this.getLocalStorage('tradingProvider'));
			}
		});
	}

	genListAction(data = []) {
		this.listAction = data.map(tradingProviderItem => {
			const actions = [];
			if (this.isGranted([this.PermissionBondConst.Bond_DLSC_ThongTinChiTiet])){
				actions.push({
					data: tradingProviderItem,
					label: 'Thông tin chi tiết',
					icon: 'pi pi-info-circle',
					command: ($event) => {
						this.detail($event.item.data);
					}
				})
			}
			return actions;
		});
	}

	create() {
		this.tradingProvider = {};
		this.businessCustomer = {};
		this.submitted = false;
		this.modalDialog = true;
	}

	detail(tradingProvider) {
		this.router.navigate(['/setting/trading-provider/detail', this.cryptEncode(tradingProvider?.tradingProviderId)]);
	}

	confirmDelete() {
		this.deleteItemDialog = false;
		this.tradingProviderService.delete(this.tradingProvider.tradingProviderId).subscribe(
			(response) => {
				if (this.handleResponseInterceptor(response, 'Xóa thành công')) {
					this.setPage({ page: this.page.pageNumber });
					this.tradingProvider = {};
				}
			}, () => {
				this.messageService.add({
					severity: 'error',
					summary: '',
					detail: `Không xóa được tài khoản ${this.tradingProvider.name}`,
					life: 3000,
				});
			}
		);
	}

	showBusinessCustomer() {
		const ref = this.dialogService.open(FilterBusinessCustomerComponent,
			{
				header: 'Tìm kiếm khách hàng doanh nghiệp',
				width: '1000px',
				styleClass: 'p-dialog-custom filter-business-customer customModal',
				style: { 'min-height': '300px', 'height': 'auto', 'top': '-15%' },
			});

		ref.onClose.subscribe((businessCustomerId) => {
			this.changeBusinessCustomer(businessCustomerId);
		});
	}

	changeBusinessCustomer(businessCustomerId) {
		this.isLoading = true;
		console.log("businessCustomerId", businessCustomerId);
		if (businessCustomerId != null) {
			this.tradingProviderService.get(businessCustomerId).subscribe((item) => {
				this.isLoading = false;
				if (this.handleResponseInterceptor(item, '')) {
					this.tradingProvider.businessCustomerId = item.data.businessCustomerId;
					this.businessCustomer = item.data;
					console.log({ itembusinessCustomerId: item.data });
				}
			}, (err) => {
				this.isLoading = false;
				console.log('Error-------', err);
				
			});

		}
		this.isLoading = false;
	}

	clickDropdown(row) {
		this.row = { ...row };
		this.actionsDisplay = this.actions.filter(action => action.statusActive.includes(row.status) && action.permission);
	}

	setPage(pageInfo?: any) {
		this.page.pageNumber = pageInfo?.page ?? this.offset;
		if(pageInfo?.rows) this.page.pageSize = pageInfo?.rows;
		this.page.keyword = this.keyword;
		this.isLoading = true;

		this.tradingProviderService.getAllTradingProvider(this.page).subscribe((res) => {
			this.isLoading = false;
			if (this.handleResponseInterceptor(res, '')) {
				this.page.totalItems = res?.data?.totalItems;
				this.rows = res?.data?.items;
				this.genListAction(this.rows);
				console.log({ rows: res.data.items, totalItems: res.data.totalItems });
			}
		}, (err) => {
			this.isLoading = false;
			console.log('Error-------', err);
			
		});
		// fix show dropdown options bị ẩn dướ
	}

	delete(tradingProvider) {
		this.confirmationService.confirm({
			message: `Bạn có chắc chắn xóa ${tradingProvider?.name} này?`,
			header: 'Cảnh báo',
			icon: 'pi pi-exclamation-triangle',
			acceptLabel: 'Đồng ý',
			rejectLabel: 'Hủy',
			accept: () => {
				this.tradingProviderService.delete(tradingProvider?.tradingProviderId).subscribe(
					(response) => {
						if (this.handleResponseInterceptor(response, 'Xóa thành công')) {
							this.setPage({ page: this.page.pageNumber });
							this.tradingProvider = {};
						}
					}, () => {
						this.messageService.add({ severity: 'error', summary: '', detail: `Không xóa được ${tradingProvider?.name}`, life: 3000 });
					}
				);
			},
			reject: () => {

			},
		});
	}

	hideDialog() {
		this.modalDialog = false;
		this.submitted = false;
	}

	save() {
		this.submitted = true;
		this.tradingProviderService.create(this.tradingProvider).subscribe(
			(response) => {
				if (this.handleResponseInterceptor(response, 'Thêm thành công')) {
					this.submitted = false;
					this.hideDialog();
					this.isLoadingPage = true;
					setTimeout(() => {
						this.isLoadingPage = false;
						this.router.navigate(['/setting/trading-provider/detail', this.cryptEncode(response.data.tradingProviderId)]);
					}, 1000);
				}
				this.submitted = false;
			}, () => {
				this.submitted = false;
				this.messageService.add({ severity: 'error', summary: '', detail: 'Không lấy được dữ liệu', life: 2000 });
			}
		);
	}

	validForm(): boolean {
		if(this.businessCustomer?.name?.trim()){
			return true;
		} 
		return false;
	}

}
