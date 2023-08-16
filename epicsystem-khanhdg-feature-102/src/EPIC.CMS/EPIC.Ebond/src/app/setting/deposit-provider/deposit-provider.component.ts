import { DepositProviderServiceProxy } from '@shared/service-proxies/setting-service';
import { BreadcrumbService } from 'src/app/layout/breadcrumb/breadcrumb.service';
import { CrudComponentBase } from '@shared/crud-component-base';
import { Component, Injector, Input, OnInit } from '@angular/core';
import { Page } from '@shared/model/page';
import { DepositProviderConst, BusinessCustomerConst, SearchConst, FormNotificationConst } from '@shared/AppConsts';
import { debounceTime } from 'rxjs/operators';

import { Router } from "@angular/router"
import { ActivatedRoute } from '@angular/router';
import { DialogService } from 'primeng/dynamicdialog';
import { DynamicDialogRef } from 'primeng/dynamicdialog';
import { ConfirmationService, MessageService } from 'primeng/api';
import { FilterBusinessCustomerComponent } from 'src/app/setting/issuer/filter-business-customer/filter-business-customer.component';
import { FormSetDisplayColumnComponent } from 'src/app/form-set-display-column/form-set-display-column/form-set-display-column.component';
import { NationalityConst } from '@shared/nationality-list';
import { FormNotificationComponent } from 'src/app/form-notification/form-notification.component';

@Component({
    selector: 'app-deposit-provider',
    templateUrl: './deposit-provider.component.html',
    styleUrls: ['./deposit-provider.component.scss'],
    providers: [DialogService, ConfirmationService, MessageService]
})
export class DepositProviderComponent extends CrudComponentBase {

    constructor(
        injector: Injector,
        private dialogService: DialogService,
        private confirmationService: ConfirmationService,
        private depositProviderService: DepositProviderServiceProxy,
        messageService: MessageService,
        private router: Router,
        private breadcrumbService: BreadcrumbService
    ) {
        super(injector, messageService);
        this.breadcrumbService.setItems([
            { label: 'Trang chủ', routerLink: ['/home'] },
            { label: 'Đại lý lưu ký' },
        ]);
    }
    // statusSearch:any[]= [ ...DepositProviderConst.statusConst]

    ref: DynamicDialogRef;
    modalDialog: boolean;

    deleteItemDialog: boolean = false;

    confirmRequestDialog: boolean = false;
    rows: any[] = [];

    actions: any[] = [];  // list button actions
    actionsDisplay: any[] = [];
    listAction: any[] = [];

    DepositProviderConst = DepositProviderConst
    BusinessCustomerConst = BusinessCustomerConst;
    NationalityConst = NationalityConst;

    businessCustomer: any;
    depositProvider: any;

    submitted: boolean;
    businessCustomerId: number;

    statuses: any[];
    page = new Page();
    offset = 0;

    row: any;
    col: any;

    cols: any[];
    _selectedColumns: any[];

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
            { field: 'code', header: 'Mã doanh nghiệp', width: '12rem', position: 1, cutText: 'b-cut-text-12', isPin: true },
            { field: 'name', header: 'Tên doanh nghiệp', width: '20rem', position: 1, cutText: 'b-cut-text-20', isPin: true },
            { field: 'shortName', header: 'Tên viết tắt', width: '12rem', position: 2, cutText: 'b-cut-text-12' },
            { field: 'email', header: 'Email', width: '20rem', position: 3, cutText: 'b-cut-text-20' },
            { field: 'phone', header: 'SĐT', width: '12rem', position: 4, cutText: 'b-cut-text-12' },
            { field: 'address', header: 'Địa chỉ', width: '40rem', position: 5, cutText: 'b-cut-text-40' },
            { field: 'repName', header: 'Người đại diện', width: '12rem', position: 6, cutText: 'b-cut-text-12' },
            { field: 'repPosition', header: 'Chức vụ', width: '12rem', position: 7, cutText: 'b-cut-text-12' },
        ];

        this._selectedColumns = this.cols;
        // this._selectedColumns = this.getLocalStorage('depositProvider') ?? this.cols;

    }

    getLocalStorage(key) {
        return JSON.parse(localStorage.getItem(key))
    }
    setLocalStorage(data) {
        return localStorage.setItem('depositProvider', JSON.stringify(data));
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
                console.log('Luu o local', JSON.parse(localStorage.getItem('depositProvider')));
            }
        });
    }

    genListAction(data = []) {
        this.listAction = data.map(depositProvider => {
            const actions = [];
            
            if (this.isGranted([this.PermissionBondConst.Bond_DLLK_ChiTiet])){
				actions.push({
                    data: depositProvider,
                    label: 'Thông tin chi tiết',
                    icon: 'pi pi-info-circle',
                    command: ($event) => {
                        this.detail($event.item.data);
                    }
				})
			}
            if (this.isGranted([this.PermissionBondConst.Bond_DLLK_Xoa])){
				actions.push({
                    data: depositProvider,
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
        this.depositProvider = { ...row };
        this.actionsDisplay = this.actions.filter(action => action.statusActive.includes(row.status) && action.permission);
        console.log({ distributionContract: row });
    }

    create() {
        this.depositProvider = {};
        this.submitted = false;
        this.modalDialog = true;
    }

    delete(depositProvider) {
        const ref = this.dialogService.open(
			FormNotificationComponent,
			{
				header: "Cảnh báo",
				width: '600px',
				contentStyle: { "max-height": "600px", "overflow": "auto", "padding-bottom": "50px" },
				styleClass: 'p-dialog-custom',
				baseZIndex: 10000,
				data: {
					title : `Bạn có chắc chắn xóa đại lý lưu ký ${depositProvider.name} này?`,
					icon: FormNotificationConst.IMAGE_CLOSE,
				},
			}
		);
		ref.onClose.subscribe((dataCallBack) => {
			console.log({ dataCallBack });
			if (dataCallBack?.accept) {
				
                this.depositProviderService.delete(depositProvider.depositProviderId).subscribe((response) => {
			if (
				this.handleResponseInterceptor(
				response,
				"Xóa thành công"
				)
			) {
				this.setPage();
				this.depositProvider = {};
			}
			});
			}
		});
    }

    detail(depositProvider) {
        this.router.navigate(['/setting/deposit-provider/detail', this.cryptEncode(depositProvider?.depositProviderId)]);
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
        if (businessCustomerId != null) {
            this.depositProviderService.get(businessCustomerId).subscribe((item) => {
                this.isLoading = false;
                if (this.handleResponseInterceptor(item, '')) {
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

        this.depositProviderService.getAll(this.page).subscribe(
            (res) => {
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
        // fix show dropdown options bị ẩn dướ
    }

    hideDialog() {
        this.modalDialog = false;
        this.submitted = false;
    }

    save() {
        this.submitted = true;
        this.depositProviderService.create(this.businessCustomer).subscribe(
            (response) => {
                if (this.handleResponseInterceptor(response, 'Thêm thành công')) {
                    this.hideDialog();
                    this.isLoadingPage = true;
                    setTimeout(() => {
                        this.isLoadingPage = false;
                        this.router.navigate(['/setting/deposit-provider/detail', this.cryptEncode(response.data.depositProviderId)]);
                    }, 1000);
                }
                this.submitted = false;
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
