import { Component, Injector } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { BusinessCustomerApproveConst, DistributionContractConst, SearchConst, KeyFilter, ApproveConst, ErrorBankConst } from '@shared/AppConsts';
import { CrudComponentBase } from '@shared/crud-component-base';
import { Page } from '@shared/model/page';
import { BusinessCustomerApproveServiceProxy } from '@shared/service-proxies/business-customer-service';
import { ConfirmationService, MessageService } from 'primeng/api';
import { DialogService, DynamicDialogRef } from 'primeng/dynamicdialog';
import { BreadcrumbService } from 'src/app/layout/breadcrumb/breadcrumb.service';
import { debounceTime } from 'rxjs/operators';
import { BankServiceProxy } from '@shared/service-proxies/bank-service';
import { NationalityConst } from '@shared/nationality-list';
import { FormSetDisplayColumnComponent } from 'src/app/form-set-display-column/form-set-display-column.component';
import { InvestorServiceProxy } from '@shared/service-proxies/investor-service';
import { CreateOrUpdateBusinessCustomerApproveComponent } from './create-or-update-business-customer-approve/create-or-update-business-customer-approve.component';
@Component({
    selector: 'app-business-customer-approve',
    templateUrl: './business-customer-approve.component.html',
    styleUrls: ['./business-customer-approve.component.scss'],
    providers: [DialogService, ConfirmationService, MessageService],
})
export class BusinessCustomerApproveComponent extends CrudComponentBase {

    constructor(
        injector: Injector,
        messageService: MessageService,
        private dialogService: DialogService,
        private router: Router,
        private _businessCustomerApproveService: BusinessCustomerApproveServiceProxy,
        private breadcrumbService: BreadcrumbService,
        private _bankService: BankServiceProxy,
    ) {
        super(injector, messageService);
        this.breadcrumbService.setItems([
            { label: 'Trang chủ', routerLink: ['/home'] },
            { label: 'Thêm khách hàng doanh nghiệp', routerLink: ['/customer/business-customer/business-customer-approve'] },
        ]);
    }

    ref: DynamicDialogRef;

    modalDialog: boolean;
    deleteItemDialog: boolean = false;
    deleteItemsDialog: boolean = false;
    rows: any[] = [];
    row: any;
    col: any;

    cols: any[];
    _selectedColumns: any[];
    banks: any[] = [];
    statusSearch = [
        {
            name: "Tất cả",
            code: ''
        },
        ...BusinessCustomerApproveConst.statusList
    ];

    BusinessCustomerApproveConst = BusinessCustomerApproveConst;

    businessCustomer: any = {
        "bankAccName": null,
    };

    dataFilter = {
        fieldFilter: null,
        status: '',  
    }

    fieldErrors = {};
    fieldDates = ['licenseDate', 'decisionDate', 'dateModified'];
    submitted: boolean;
    expandedRows = {};
    statuses: any[];
    KeyFilter = KeyFilter;

    listAction: any[] = [];
    //
    page = new Page();
    offset = 0;

    sortData: any[] = []
    actions: any[] = [];  
    actionsDisplay: any[] = [];
    isLoadingBank: boolean;

    ngOnInit(): void {

        this.getAllBank();
        this.setPage({ page: this.offset });
        this.subject.keyword.pipe(debounceTime(SearchConst.DEBOUNCE_TIME)).subscribe(() => {
            if (this.keyword === "") {
                this.setPage({ page: this.offset });
            } else {
                this.setPage();
            }
        });
        this.subject.isSetPage.subscribe(() => {
			this.setPage();
		  })

        this.cols = [
            { field: 'taxCode', header: 'Mã số thuế', width: '12rem' , isPin: true, isSort: true },
            { field: 'shortName', header: 'Tên viết tắt', width: '20rem', isPin: true, isResize: true, isSort: true },
            { field: 'name', header: 'Tên doanh nghiệp', width: '35rem', isResize: true, isSort: true },
            { field: 'email', header: 'Thư điện tử', width: '18rem', isResize: true, isSort: true },
        ];

        this.cols = this.cols.map((item, index) => {
            item.position = index + 1;
            return item;
        });

        this._selectedColumns = this.getLocalStorage('busiCusAppCore') ?? this.cols;
    }

    getLocalStorage(key) {
        return JSON.parse(localStorage.getItem(key))
    }
    setLocalStorage(data) {
        return localStorage.setItem('busiCusAppCore', JSON.stringify(data));
    }

    setColumn(col, _selectedColumns) {
        const ref = this.dialogService.open(
            FormSetDisplayColumnComponent,
            this.getConfigDialogServiceDisplayTableColumn("Sửa cột hiển thị", col, _selectedColumns)
        );

        ref.onClose.subscribe((dataCallBack) => {
            console.log('dataCallBack', dataCallBack);
            if (dataCallBack?.accept) {
                this._selectedColumns = dataCallBack.data.sort(function (a, b) {
                    return a.position - b.position;
                });
                this.setLocalStorage(this._selectedColumns)
            }
        });
    }

    genListAction(data = []) {
        this.listAction = data.map(businessCustomerItem => {
            const actions = [];

            if (this.isGranted([this.PermissionCoreConst.CoreDuyetKHDN_ThongTinKhachHang])) {
                actions.push({
                    data: businessCustomerItem,
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

    getAllBank() {
        this.page.keyword = this.keyword;
        this.isLoading = true;
        this._bankService.getAllBank(this.page).subscribe(
            (res) => {
                this.isLoading = false;
                if (this.handleResponseInterceptor(res, "")) {
                    this.page.totalItems = res.data.totalItems;
                    this.banks = res.data.items;
                    console.log({ banks: res.data.items, totalItems: res.data.totalItems });
                }
            },
            (err) => {
                this.isLoading = false;
                console.log('Error-------', err);
                
            }
        );
    }

    detail(businessCustomer) {
        this.router.navigate(['/customer/business-customer/business-customer-approve/detail/' + this.cryptEncode(businessCustomer?.businessCustomerTempId)]);
    }

    create() {
        const ref = this.dialogService.open(CreateOrUpdateBusinessCustomerApproveComponent, {
            header: "Thêm mới khách hàng doanh nghiệp",
            width: '1000px',
            contentStyle: { "max-height": "600px", overflow: "auto", "margin-bottom": "60px", },
            baseZIndex: 10000,
        });
        ref.onClose.subscribe((res) => {
            this.setPage();
        });
    }

    changeStatus() {
        this.setPage({ page: this.offset });
    }

    changeFieldSearch() {
        if(this.keyword) {
            this.setPage();
        }
    }

    setPage(pageInfo?: any) {
        this.page.pageNumber = pageInfo?.page ?? this.offset;
		if(pageInfo?.rows) this.page.pageSize = pageInfo?.rows;
        this.page.keyword = this.keyword;
        //
        this.isLoading = true;
        this._businessCustomerApproveService.getAll(this.page, this.dataFilter,this.sortData).subscribe((res) => {
            this.isLoading = false;
            if (this.handleResponseInterceptor(res, '')) {
                this.page.totalItems = res.data.totalItems;
                this.rows = res.data.items;
                if (res.data?.items?.length) {
                    this.genListAction(this.rows);
                }
                console.log({ rows: res.data.items, totalItems: res.data.totalItems });
            }
        }, (err) => {
            this.isLoading = false;
            console.log('Error-------', err);
            
        });
    }

    resetValid(field) {
        this.fieldErrors[field] = false;
    }
}