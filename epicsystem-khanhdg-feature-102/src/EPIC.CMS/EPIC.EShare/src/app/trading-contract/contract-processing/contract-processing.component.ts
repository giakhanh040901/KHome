import { Component, Injector, OnInit } from '@angular/core';
import { CrudComponentBase } from '@shared/crud-component-base';
import { Page } from '@shared/model/page';
import { CompanyShareInfoServiceProxy, CompanyShareSecondaryServiceProxy } from '@shared/service-proxies/company-share-manager-service';
import { ConfirmationService, MessageService } from 'primeng/api';
import { BreadcrumbService } from 'src/app/layout/breadcrumb/breadcrumb.service';
import { Router } from '@angular/router';
import { OrderServiceProxy } from '@shared/service-proxies/trading-contract-service';
import { OrderConst, SearchConst } from '@shared/AppConsts';
import { forkJoin, Subject } from 'rxjs';
import { DialogService } from 'primeng/dynamicdialog';
import { FormSetDisplayColumnComponent } from 'src/app/form-set-display-column/form-set-display-column/form-set-display-column.component';
import { debounceTime } from 'rxjs/operators';

@Component({
  selector: 'app-contract-processing',
  templateUrl: './contract-processing.component.html',
  styleUrls: ['./contract-processing.component.scss']
})
export class ContractProcessingComponent extends CrudComponentBase {

  constructor(
    injector: Injector,
    messageService: MessageService,
    private _orderService: OrderServiceProxy,
    private dialogService: DialogService,
    private breadcrumbService: BreadcrumbService,
    private confirmationService: ConfirmationService,
    private _companyShareSecondaryService: CompanyShareSecondaryServiceProxy,
    private router: Router,
  ) {
    super(injector, messageService);
    this.breadcrumbService.setItems([
      { label: 'Trang chủ', routerLink: ['/home'] },
      { label: 'Hợp đồng phân phối' },
      { label: 'Xử lý hợp đồng' },
    ]);
  }

  OrderConst = OrderConst;

  rows: any[] = [];
  row: any;
  col: any;

  cols: any[];
  _selectedColumns: any[];

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

  page = new Page();
  offset = 0;

  order: any = {};
  // Menu otions thao tác
  statusSearch: any[] = [
    {
      name: 'Tất cả',
      code: '',
    },
    ...OrderConst.statusProcessing
  ];

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

  subject = {
		keyword: new Subject(),
    
	};

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
      { field: 'companyShareCode', header: 'Mã TP', width: '12rem',isPin: true },
      { field: 'customerName', header: 'Khách hàng', width: '20rem', isPin: true },
      { field: 'buyDate', header: 'Ngày đặt lệnh', width: '12rem'},
      { field: 'companySharePolicyName', header: 'Sản phẩm', width: '12rem', cutText: 'b-cut-text-12' },
      { field: 'companySharePolicyDetailName', header: 'Kỳ hạn', width: '10rem', cutText: 'b-cut-text-10' },
      { field: 'profitDisplay', header: 'Lợi tức', width: '12rem', cutText: 'b-cut-text-12' },
      { field: 'quantityDisplay', header: 'Số lượng', width: '12rem', cutText: 'b-cut-text-12' }
    ];

    this.cols = this.cols.map((item, index) => {
			item.position = index + 1;
			return item;
		})

    // this._selectedColumns = this.cols;
    this._selectedColumns = this.getLocalStorage('contractProcessing') ?? this.cols;
  }

  getLocalStorage(key) {
    return JSON.parse(localStorage.getItem(key))
  }
  setLocalStorage(data) {
    return localStorage.setItem('contractProcessing', JSON.stringify(data));
  }

  setColumn(col, _selectedColumns) {
    console.log('cols:', col);

    console.log('_selectedColumns', _selectedColumns);

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
        console.log('Luu o local', this._selectedColumns);
      }
    });
  }

  showData(rows) {
    for (let row of rows) {
        row.companyShareCode = row?.companyShareInfo?.companyShareCode;
        row.customerName = row?.businessCustomer?.name || (row?.investor?.name ?? row?.investor?.investorIdentification?.fullname);
        row.buyDate = this.formatDateTime(row?.buyDate),
        row.companySharePolicyName = row?.companySharePolicy?.name;
        row.companySharePolicyDetailName = row?.companySharePolicyDetail?.name;
        row.profitDisplay = this.utils.transformPercent(row?.companySharePolicyDetail?.profit) + "%/năm";
        if (row?.totalValue != null && row?.buyPrice != null){
          row.quantityDisplay = this.utils.transformMoney(Math.floor(row?.totalValue / row?.buyPrice));
      } else {
          row.quantityDisplay = '';
      }
    };
    console.log('showData', rows);
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
      return actions;
    });
  }

  detail(order) {
    this.router.navigate(['/trading-contract/order/detail/' + this.cryptEncode(order?.orderId)]);
  }

  init() {
    this.isLoading = true;
    forkJoin([this._orderService.getAll(this.page, 'orderContractProcessing', this.status ,this.dataFilter), this._companyShareSecondaryService.getAllNoPaging()]).subscribe(([resOrder, resSecondary]) => {
      this.isLoading = false;
      if (this.handleResponseInterceptor(resOrder, '')) {
        this.page.totalItems = resOrder?.data?.totalItems;
        this.rows = resOrder?.data?.items;
        this.companyShareSecondarys = resSecondary?.data?.items;
        this.companyShareSecondarys = [...[{ companyShareName: 'Tất cả', companyShareSecondaryId: '' }], ...this.companyShareSecondarys];

        if(this.rows?.length) {
          this.genListAction(this.rows);
          this.showData(this.rows)
         }
        console.log({ rowsOrder: resOrder.data.items, totalItems: resOrder.data.totalItems, resSecondary: resSecondary.data.items });
      }
    }, (err) => {
      this.isLoading = false;
      console.log('Error-------', err);
      
    });
  }

  changeStatus() {
    this.setPage({ Page: this.offset })
  }

  setPage(pageInfo?: any) {
    this.page.pageNumber = pageInfo?.page ?? this.offset;
    if(pageInfo?.rows) this.page.pageSize = pageInfo?.rows;

    this.page.keyword = this.keyword;
    //
    this.isLoading = true;
    this._orderService.getAll(this.page, 'orderContractProcessing', this.status, this.dataFilter).subscribe((resOrder) => {
      this.isLoading = false;
      if (this.handleResponseInterceptor(resOrder, '')) {
        this.page.totalItems = resOrder?.data?.totalItems;
        this.rows = resOrder?.data?.items;
        // 
        if(this.rows?.length) {
          this.genListAction(this.rows);
          this.showData(this.rows)
         }
        
        console.log({ rowsOrder: resOrder.data.items, totalItems: resOrder.data.totalItems });
      }
    });
  }

  changeFieldFilter() {
    if (this.keyword) {
      this.setPage();
    }
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
