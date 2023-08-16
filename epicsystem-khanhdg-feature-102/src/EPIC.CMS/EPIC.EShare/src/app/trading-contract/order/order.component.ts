import { Component, Injector, OnInit } from '@angular/core';
import { CrudComponentBase } from '@shared/crud-component-base';
import { Page } from '@shared/model/page';
import { CompanyShareInfoServiceProxy, CompanyShareSecondaryServiceProxy } from '@shared/service-proxies/company-share-manager-service';
import { ConfirmationService, MessageService } from 'primeng/api';
import { BreadcrumbService } from 'src/app/layout/breadcrumb/breadcrumb.service';
import { Router } from '@angular/router';
import { OrderServiceProxy } from '@shared/service-proxies/trading-contract-service';
import { FormNotificationConst, OrderConst, SearchConst } from '@shared/AppConsts';
import { DialogService } from 'primeng/dynamicdialog';
import { FormSetDisplayColumnComponent } from 'src/app/form-set-display-column/form-set-display-column/form-set-display-column.component';
import { forkJoin, Subject } from 'rxjs';
import { debounceTime } from 'rxjs/operators';
import { FormNotificationComponent } from 'src/app/form-notification/form-notification.component';

@Component({
    selector: 'app-order',
    templateUrl: './order.component.html',
    styleUrls: ['./order.component.scss'],
})
export class OrderComponent extends CrudComponentBase {

    constructor(
        injector: Injector,
        messageService: MessageService,
        private _orderService: OrderServiceProxy,
        private breadcrumbService: BreadcrumbService,
        private confirmationService: ConfirmationService,
        private _companyShareSecondaryService: CompanyShareSecondaryServiceProxy,
        private dialogService: DialogService,
        private router: Router,
        private _dialogService: DialogService,
    ) {
        super(injector, messageService);
        this.breadcrumbService.setItems([
            { label: 'Trang chủ', routerLink: ['/home'] },
            { label: 'Hợp đồng phân phối' },
            { label: 'Sổ lệnh' },
        ]);
    }

    modalDialog: boolean;

    OrderConst = OrderConst;
    rows: any[] = [];
    row: any;
    col: any;

    cols: any[];
    _selectedColumns: any[];

    sources: any[] = [
		{
			name: 'Tất cả',
			code: '',
		},
		...OrderConst.sources,
	];

    orderSources: any[] = [
		{
			name: 'Tất cả',
			code: '',
		},
		...OrderConst.orderSources,
	];

    // data Filter
    companyShareSecondarys: any[] = [];
    companySharePolicies: any[] = [];
    companySharePolicyDetails: any[] = [];

    dataFilter = {
        companyShareSecondaryId: null,
        companySharePolicyId: null,
        companySharePolicyDetailId: null,
        fieldFilter: null,
		source: null,
        orderSource: null
    }

    tradingDate: null;

    submitted: boolean;
    listAction: any[] = [];

    statusSearch: any[] = [
        {
            name: 'Tất cả',
            code: '',
        },
        ...OrderConst.statusOrder,
    ];

    subject = {
        keyword: new Subject(),
    };

    status: any;

    page = new Page();
    offset = 0;

    order: any = {};
    // Menu otions thao tác

    ngOnInit() {
        this.init();
        this.setPage({ page: this.offset });
        this.subject.keyword.pipe(debounceTime(SearchConst.DEBOUNCE_TIME)).subscribe(() => {
            if (this.keyword === "") {
                this.setPage({ page: this.offset });
            } else {
                this.setPage();
            }
        });

        this.cols = [
            { field: 'companyShareCode', header: 'Mã TP', width: '12rem', isPin:true },
            { field: 'customerName', header: 'Khách hàng', width: '20rem', isPin: true, isReSize:true },
            { field: 'buyDate', header: 'Ngày đặt lệnh', width: '12rem'},
            { field: 'companySharePolicyName', header: 'Sản phẩm', width: '12rem' },
            { field: 'companySharePolicyDetailName', header: 'Kỳ hạn', width: '10rem'},
            { field: 'profit', header: 'Lợi tức', width: '12rem' },
            { field: 'quantityDisplay', header: 'Số lượng', width: '12rem' }
        ];

        this.cols = this.cols.map((item, index) => {
            item.position = index + 1;
            return item;
        })

        this._selectedColumns = this.cols;
        // this._selectedColumns = this.getLocalStorage('order') ?? this.cols;
    }

    getLocalStorage(key) {
        return JSON.parse(localStorage.getItem(key))
    }
    setLocalStorage(data) {
        return localStorage.setItem('order', JSON.stringify(data));
    }

    setColumn(col, _selectedColumns) {
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
                this._selectedColumns = dataCallBack.data.sort(function (a, b) {
                    return a.position - b.position;
                });
                this.setLocalStorage(this._selectedColumns)

                console.log('Luu o local', this._selectedColumns);
            }
        });
    }

    showData(rows) {
        for (let row of rows) {
            row.companyShareCode = row?.companyShareInfo?.companyShareCode,
            row.customerName = row?.businessCustomer?.name || (row?.investor?.name ?? row?.investor?.investorIdentification?.fullname),
            row.buyDate = this.formatDateTime(row?.buyDate),
            row.companySharePolicyName = row?.companySharePolicy?.name,
            row.companySharePolicyDetailName = row?.companySharePolicyDetail?.name,
            row.profit = row?.companySharePolicyDetail?.profit + "%/năm";
            if (row?.totalValue != null && row?.buyPrice != null){
                row.quantityDisplay = this.utils.transformMoney(Math.floor(row?.totalValue / row?.buyPrice));
            } else {
                row.quantityDisplay = '';
            }

        };
        console.log('row', rows);
    }

    genListAction(data = []) {
        this.listAction = data.map(orderItem => {
            const actions = [];

            if (this.isGranted([this.PermissionCompanyShareConst.CompanyShareHDPP_SoLenh_TTCT])){
                actions.push({
                    data: orderItem,
                    label: 'Thông tin chi tiết',
                    icon: 'pi pi-info-circle',
                    command: ($event) => {
                        this.detail($event.item.data);
                    } 
                })
            }

            if ((orderItem.status == this.OrderConst.KHOI_TAO || orderItem.status == this.OrderConst.CHO_THANH_TOAN ) && this.isGranted([this.PermissionCompanyShareConst.CompanyShareHDPP_SoLenh_Xoa])) {
                actions.push({
                    data: orderItem,
                    label: 'Xoá',
                    icon: 'pi pi-trash',
                    command: ($event) => {
                        this.delete($event.item.data);
                    }
                });
            }

            return actions;
        });
    }

    // delete(order) {
    //     this.confirmationService.confirm({
    //         message: 'Bạn có chắc chắn xoá số lệnh này?',
    //         header: 'Cảnh báo!',
    //         icon: 'pi pi-exclamation-triangle',
    //         acceptLabel: 'Có',
    //         rejectLabel: 'Không',
    //         accept: () => {
    //             console.log("hello : ", order);
    //             this._orderService.delete(order?.orderId).subscribe((response) => {
    //                 if (this.handleResponseInterceptor(response, "")) {
    //                     this.setPage();
    //                 }
    //             });
    //         },
    //         reject: () => {

    //         },
    //     });
    // }

    delete(order) {
		const ref = this._dialogService.open(
				FormNotificationComponent,
				{
					header: "Thông báo",
					width: '600px',
					contentStyle: { "max-height": "600px", "overflow": "auto", "padding-bottom": "50px" },
					styleClass: 'p-dialog-custom',
					baseZIndex: 10000,
					data: {
						title : "Bạn có chắc chắn xoá số lệnh này?",
						icon: FormNotificationConst.IMAGE_CLOSE,
					},
				}
			);
		ref.onClose.subscribe((dataCallBack) => {
      console.log({ dataCallBack });
			if (dataCallBack?.accept) {
			this._orderService.delete(order?.orderId).subscribe((response) => {
			  if (
				this.handleResponseInterceptor(
				  response,
				  "Xóa số lệnh thành công"
				)
			  ) {
				this.setPage();
			  }
			});
			}
		});
	  }

    detail(order) {
        this.router.navigate(['/trading-contract/order/detail/' + this.cryptEncode(order?.orderId)]);
    }

    create() {
        this.router.navigate(['/trading-contract/order/create/filter-customer']);
    }

    changeStatus() {
        this.setPage({ Page: this.offset })
    }

    init() {
        this.isLoading = true;
        forkJoin([this._orderService.getAll(this.page, 'order', this.status, this.dataFilter), this._companyShareSecondaryService.getAllSecondary()]).subscribe(([res, resSecondary]) => {
            this.isLoading = false;
            if (this.handleResponseInterceptor(res, '')) {
                this.page.totalItems = res?.data?.totalItems;
                this.rows = res?.data?.items;
                this.companyShareSecondarys = resSecondary?.data;
                this.companyShareSecondarys = [...[{ companyShareName: 'Tất cả', companyShareSecondaryId: '' }], ...this.companyShareSecondarys];
                if (this.rows?.length) {
                    this.genListAction(this.rows);
                    this.showData(this.rows)
                }

                console.log({ rowsOrder: res.data.items, totalItems: res.data.totalItems, resSecondary: resSecondary.data.items });
            }
        });
    }

    changeFieldFilter() {
        if (this.keyword) {
          this.setPage();
        }
    }

    setPage(pageInfo?: any) {
        this.page.pageNumber = pageInfo?.page ?? this.offset;
        if (pageInfo?.rows) this.page.pageSize = pageInfo?.rows;

        this.page.keyword = this.keyword;
        //
        this.isLoading = true;
        this._orderService.getAll(this.page, 'order', this.status, this.dataFilter).subscribe((res) => {
            this.isLoading = false;
            if (this.handleResponseInterceptor(res, '')) {
                this.page.totalItems = res.data.totalItems;
                this.rows = res.data.items;
                if (this.rows?.length) {
                    this.genListAction(this.rows);
                    this.showData(this.rows);
                }
                console.log({ rowsOrder: res.data.items, totalItems: res.data.totalItems });
            }
        }, (err) => {
            this.isLoading = false;
            
            console.log('Error-------', err);
        });
    }

    changeKeywordSearch() {
        this.setPage({ Page: this.offset })
      }

    changeCompanyShareSecondary(companyShareSecondaryId) {
        this.dataFilter.companySharePolicyId = [];
        this.dataFilter.companySharePolicyDetailId = '';
        this.companySharePolicies = [];
        const companyShareSecondary = this.companyShareSecondarys.find(item => item.companyShareSecondaryId == companyShareSecondaryId);
        this.companySharePolicies = companyShareSecondary?.policies ?? [];
        if (this.companySharePolicies?.length) {
            this.companySharePolicies = [...this.companySharePolicies];
        }
        this.setPage();
    }

    changeCompanySharePolicy(companySharePolicyId) {
        this.dataFilter.companySharePolicyDetailId = '';
        this.companySharePolicyDetails = [];
        const companySharePolicy = this.companySharePolicies.find(item => item.companySharePolicyId == companySharePolicyId);

        this.companySharePolicyDetails = companySharePolicy?.details ?? [];
        if (this.companySharePolicyDetails?.length) {
            this.companySharePolicyDetails = [...this.companySharePolicyDetails];
        }
        this.setPage();
    }
}
