import { NationalityConst } from '@shared/nationality-list';
import { CrudComponentBase } from '@shared/crud-component-base';
import { Component, Injector, Input, OnInit } from '@angular/core';
import { Page } from '@shared/model/page';
import { IssuerServiceProxy } from '@shared/service-proxies/setting-service';
import { BreadcrumbService } from 'src/app/layout/breadcrumb/breadcrumb.service';
import { IssuerConst, BusinessCustomerConst, SearchConst, FormNotificationConst } from '@shared/AppConsts';
import { debounceTime } from 'rxjs/operators';

import { Router } from "@angular/router"
import { ActivatedRoute } from '@angular/router';
import { DialogService } from 'primeng/dynamicdialog';
import { DynamicDialogRef } from 'primeng/dynamicdialog';
import { ConfirmationService, MessageService } from 'primeng/api';
import { FilterBusinessCustomerComponent } from './filter-business-customer/filter-business-customer.component';
import { FormSetDisplayColumnComponent } from 'src/app/form-set-display-column/form-set-display-column/form-set-display-column.component';
import { FormNotificationComponent } from 'src/app/form-notification/form-notification.component';


@Component({
	selector: 'app-issuer',
	templateUrl: './issuer.component.html',
	styleUrls: ['./issuer.component.scss'],
	providers: [DialogService, ConfirmationService, MessageService]
})
export class IssuerComponent extends CrudComponentBase {

	constructor(
		injector: Injector,
		messageService: MessageService,
		private dialogService: DialogService,
		private confirmationService: ConfirmationService,
		private router: Router,
		private routeActive: ActivatedRoute,
		private issuerService: IssuerServiceProxy,
		private breadcrumbService: BreadcrumbService
	) {
		super(injector, messageService);
		this.breadcrumbService.setItems([
			{ label: 'Trang chủ', routerLink: ['/home'] },
			{ label: 'Tổ chức phát hành' },
		]);
	}

	status = '';
	// statusSearch: any[] = [...ProductBondInfoConst.statusConst]
	// ProductBondInfoConst = ProductBondInfoConst;
	expandedRows = {};

	ref: DynamicDialogRef;

	modalDialog: boolean;

	deleteItemDialog: boolean = false;

	confirmRequestDialog: boolean = false;

	rows: any[] = [];
	listAction: any[] = [];
	actions: any[] = [];  // list button actions
	actionsDisplay: any[] = [];

	IssuerConst = IssuerConst;
	BusinessCustomerConst = BusinessCustomerConst;
	NationalityConst = NationalityConst;

	businessCustomer: any;

	issuer: any = {
		businessCustomerId: null,
		businessTurnover: null,	// Doanh thu
		businessProfit: null,	// Lợi nhuận
		roa: null,
		roe: null,
	};

	businessCustomerId: number;

	submitted: boolean;

	page = new Page();
	offset = 0;

	statuses: any[];
	row: any;
	col:any;

	cols: any[];
	_selectedColumns: any[];
	// frozenCols: any[];

	ngOnInit() {
		this.setPage({ page: this.offset });
		this.subject.keyword.pipe(debounceTime(SearchConst.DEBOUNCE_TIME)).subscribe(() => {
			if (this.keyword === "") {
				this.setPage({ page: this.offset });
			} else {
				this.setPage();
			}
		});

		this.cols = [
            { field: 'code', header: 'Mã doanh nghiệp', width : '12rem', position: 1, cutText: 'b-cut-text-12', isPin: true},
            { field: 'name', header: 'Tên doanh nghiệp', width : '20rem', position: 1, isPin: true},
            { field: 'shortName', header: 'Tên viết tắt', width : '12rem', position: 2, cutText: 'b-cut-text-12'},
            { field: 'email', header: 'Email', width : '20rem', position: 3, cutText: 'b-cut-text-20'},
            { field: 'phone', header: 'SĐT', width : '12rem', position: 4, cutText: 'b-cut-text-12'},
            { field: 'address', header: 'Địa chỉ', width : '40rem', position: 5, cutText: 'b-cut-text-40' },
            { field: 'repName', header: 'Người đại diện', width : '12rem', position: 6, cutText: 'b-cut-text-12' },
            { field: 'repPosition', header: 'Chức vụ', width : '12rem', position: 7, cutText: 'b-cut-text-12' },
        ];

		// this._selectedColumns = this.cols;
		this._selectedColumns = this.getLocalStorage('issuer') ?? this.cols;

	}

	getLocalStorage(key) {
		return JSON.parse(localStorage.getItem(key))
	}
	setLocalStorage(data) {
		return localStorage.setItem('issuer', JSON.stringify(data));
	}

	setColumn(col,_selectedColumns) {
		console.log('cols:', col);
		
		console.log('_selectedColumns', _selectedColumns);
		
		const ref = this.dialogService.open(
			FormSetDisplayColumnComponent,
			this.getConfigDialogServiceDisplayTableColumn("Sửa cột hiển thị", col, _selectedColumns)
		);
		//
		ref.onClose.subscribe((dataCallBack) => {
			console.log('dataCallBack', dataCallBack);
			if (dataCallBack?.accept) {
				this._selectedColumns = dataCallBack.data.sort(function(a,b){
					return a.position - b.position;
				});
				this.setLocalStorage(this._selectedColumns)
				console.log('anh Nghia', this._selectedColumns);
				
				console.log('Luu o local' ,this.getLocalStorage('issuer'));
			}
		});
	}

	genListAction(data = []) {
		this.listAction = data.map(issuerItem => {
			const actions = [];

			if (this.isGranted([this.PermissionBondConst.Bond_TCPH_ThongTinChiTiet])){
				actions.push({
					data: issuerItem,
					label: 'Thông tin chi tiết',
					icon: 'pi pi-info-circle',
					command: ($event) => {
						this.detail($event.item.data);
					}
				})
			}

			if (this.isGranted([this.PermissionBondConst.Bond_TCPH_Xoa])){
				actions.push({
					data: issuerItem,
					label: 'Xoá',
					icon: 'pi pi-trash',
					command: ($event) => {
						this.delete($event.item.data);
					}
				})
			}

			return actions;
		});
	}

	clickDropdown(row) {
		this.row = { ...row };
		this.actionsDisplay = this.actions.filter(action => action.statusActive.includes(row.status) && action.permission);
	}

	create() {
		this.issuer = {};
		this.businessCustomer = {};
		this.submitted = false;
		this.modalDialog = true;
	}

	delete(issuer) {
		const ref = this.dialogService.open(
			FormNotificationComponent,
			{
				header: "Cảnh báo",
				width: '600px',
				contentStyle: { "max-height": "600px", "overflow": "auto", "padding-bottom": "50px" },
				styleClass: 'p-dialog-custom',
				baseZIndex: 10000,
				data: {
					title : `Bạn có chắc chắn xóa TCPH ${issuer?.name} này?`,
					icon: FormNotificationConst.IMAGE_CLOSE,
				},
			}
		);
		ref.onClose.subscribe((dataCallBack) => {
			console.log({ dataCallBack });
			if (dataCallBack?.accept) {
				
			this.issuerService.delete(issuer?.issuerId).subscribe((response) => {
			if (
				this.handleResponseInterceptor(
				response,
				"Xóa thành công"
				)
			) {
				this.setPage();
				this.issuer = {};
			}
			});
			}
		});
		// this.confirmationService.confirm({
		// 	message: `Bạn có chắc chắn xóa TCPH ${issuer?.name} này?`,
		// 	header: 'Cảnh báo',
		// 	icon: 'pi pi-exclamation-triangle',
		// 	acceptLabel: 'Đồng ý',
		// 	rejectLabel: 'Hủy',
		// 	accept: () => {
		// 		this.issuerService.delete(issuer?.issuerId).subscribe(
		// 			(response) => {
		// 				if (this.handleResponseInterceptor(response, 'Xóa thành công')) {
		// 					this.setPage({ page: this.page.pageNumber });
		// 					this.issuer = {};
		// 			}
		// 		}, () => {
		// 			this.messageService.add({ severity: 'success', summary: '', detail: 'Xóa thành công!', life: 1500 });
		// 		}
		// 		);
		// 	},
		// 	reject: () => {

		// 	},
		// });
	}

	detail(issuer) {
		this.router.navigate(['/setting/issuer/detail', this.cryptEncode(issuer?.issuerId)]);
	}

	showBusinessCustomer() {
		const ref = this.dialogService.open(FilterBusinessCustomerComponent,
			{
				header: 'Tìm kiếm khách hàng doanh nghiệp',
				width: '1000px',
				styleClass: 'p-dialog-custom filter-business-customer customModal',
				style:{'min-height': '300px', 'height': 'auto', 'top': '-15%'},
			});

		ref.onClose.subscribe((businessCustomerId) => {
			this.changeBusinessCustomer(businessCustomerId);
		});
	}

	changeBusinessCustomer(businessCustomerId) {
        this.isLoading = true;
		console.log("businessCustomerId",businessCustomerId);
		if (businessCustomerId != null) {

			this.issuerService.get(businessCustomerId).subscribe((item) => {
				this.isLoading = false;
				if (this.handleResponseInterceptor(item, '')) {
					this.issuer.businessCustomerId = item.data.businessCustomerId;
					this.businessCustomerId = item.data.businessCustomerId;
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

	setPage(pageInfo?: any) {
		this.page.pageNumber = pageInfo?.page ?? this.offset;
		if(pageInfo?.rows) this.page.pageSize = pageInfo?.rows;
		this.page.keyword = this.keyword;
		this.isLoading = true;

		this.issuerService.getAllIssuer(this.page).subscribe((res) => {
			if(this.callBackData(res)) {
				this.setPage();
			}
			this.isLoading = false;
			if (this.handleResponseInterceptor(res, '')) {
				this.page.totalItems = res.data.totalItems;
				this.rows = res.data.items;
				this.genListAction(this.rows);
				console.log({ rows: res.data.items, totalItems: res.data.totalItems });
			}
		}, (err) => {
			this.isLoading = false;
			console.log('Error-------', err);
			
		});
	}

	hideDialog() {
		this.modalDialog = false;
		this.submitted = false;
	}

	save() {
		this.submitted = true;
		console.log('issuer', this.issuer);
		
		this.issuerService.create(this.issuer).subscribe(
			(response) => {
				if (this.handleResponseInterceptor(response, 'Thêm thành công')) {
					this.submitted = false;
					this.hideDialog();
					this.isLoadingPage = true;
					setTimeout(() => {
						this.isLoadingPage = false;
						this.router.navigate(['/setting/issuer/detail', this.cryptEncode(response.data.issuerId)]);
					}, 1000);
				} else {
					this.submitted = false;
				}
			}, () => {
				this.submitted = false;
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

