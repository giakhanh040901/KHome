import { Component, Injector, OnInit } from '@angular/core';
import { CrudComponentBase } from '@shared/crud-component-base';
import { Page } from '@shared/model/page';
import { CompanyShareInfoServiceProxy, CompanyShareSecondaryServiceProxy } from '@shared/service-proxies/company-share-manager-service';
import { ConfirmationService, MessageService } from 'primeng/api';
import { BreadcrumbService } from 'src/app/layout/breadcrumb/breadcrumb.service';
import { Router } from '@angular/router';
import { InterestPaymentServiceProxy, OrderServiceProxy } from '@shared/service-proxies/trading-contract-service';
import { BlockageLiberationConst, InterestPaymentConst, OrderConst, PermissionCompanyShareConst, SearchConst } from '@shared/AppConsts';
import { forkJoin, Subject } from 'rxjs';
import { DialogService } from 'primeng/dynamicdialog';
import { FormSetDisplayColumnComponent } from 'src/app/form-set-display-column/form-set-display-column/form-set-display-column.component';
import { debounceTime } from 'rxjs/operators';
import { BlockageLiberationService } from '@shared/services/blockade-liberation.service';

@Component({
  selector: 'app-interest-contract',
  templateUrl: './interest-contract.component.html',
  styleUrls: ['./interest-contract.component.scss']
})
export class InterestContractComponent extends CrudComponentBase {

  constructor(
    injector: Injector,
    messageService: MessageService,
    private _orderService: OrderServiceProxy,
    private _interestPaymentService: InterestPaymentServiceProxy,
    private breadcrumbService: BreadcrumbService,
    private confirmationService: ConfirmationService,
    private dialogService: DialogService,
    private _blockageLiberationService: BlockageLiberationService,
    private _companyShareSecondaryService: CompanyShareSecondaryServiceProxy,
    private router: Router,
  ) {
    super(injector, messageService);
    this.breadcrumbService.setItems([
      { label: 'Trang chủ', routerLink: ['/home'] },
      { label: 'Hợp đồng phân phối' },
      { label: 'Hợp đồng' },
    ]);
  }
  liberationDialog: boolean;
  modalDialog: boolean;
  blockageDialog: boolean;

  OrderConst = OrderConst;
  InterestPaymentConst = InterestPaymentConst;

  rows: any[] = [];
  row: any;
  col: any;

  cols: any[];
  _selectedColumns: any[];
  releaseDialog: boolean;
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
    orderSource: null,
    ngayChiTra: new Date(),
    typeInterest: InterestPaymentConst.STATUS_DUEDATE,
  }

  tradingDate: null;

  submitted: boolean;
  listAction: any[] = [];
  page = new Page();
  offset = 0;

  statusSearch: any[] = [
    {
      name: 'Tất cả',
      code: '',
    },
    ...OrderConst.statusActive
  ];

  typeInterests : any[] = InterestPaymentConst.statusInterest;

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

  blockageLiberation: any = {
    "id": 0,
    "type": null,
    "blockadeDescription": "",
    "blockadeDate": null,
    "orderId": 0,
    "blockader": null,
    "blockadeTime": null,
    "liberationDescription": null,
    "liberationDate": null,
    "liberator": null,
    "liberationTime": null,
    "status": null,
    "contractCode": null,
    "totalValue": null
  };

  selectedContracts = [];

  subject = {
    keyword: new Subject(),
  };

  blockage: any = {};
  status: any;

  loadingStep: number = 0;
  createdList: boolean;

  linkForStepPayment = {
		[InterestPaymentConst.STATUS_DUEDATE]: {
			'api' : '/api/companyShare/order/lap-danh-sach-chi-tra?',
			'loadingStep': 0,
		},
		[InterestPaymentConst.STATUS_CREATED_LIST]: {
			'api': '/api/companyShare/interest-payment/find?',
			'loadingStep': 50
		},
		[InterestPaymentConst.STATUS_DONE]: {
			'api': '/api/companyShare/interest-payment/find?',
			'loadingStep': 100,
		},
	}

  interestPaymentItem = {
		"orderId": null,
		"periodIndex": null,
		"cifCode": null,
		"amountMoney": null,
		"policyDetailId": null,
		"payDate": null,
    "isLastPeriod": null,
	}

  BlockageLiberationConst = BlockageLiberationConst;

  // blockageTypes  =  BlockageReleaseConst.blockageTypes;
  order: any = {};
  // Menu otions thao tác
  ngOnInit() {
    // this.init();

    this.blockageLiberation.type = 2;
    this.setPage({ page: this.offset });
    this.subject.keyword.pipe(debounceTime(SearchConst.DEBOUNCE_TIME)).subscribe(() => {
      if (this.keyword.trim()) {
        this.setPage({ page: this.offset });
      } 
		});

    this.cols = [
      { field: 'contractCode', header: 'Số hợp đồng', width: '12rem', isPin: true },
      { field: 'name', header: 'Khách hàng', width: '16rem', isPin: true },
      { field: 'policyDetailName', header: 'Kỳ hạn', width: '10rem', cutText: 'b-cut-text-11' },
      { field: 'totalValue', header: 'Tiền đầu tư', width: '12rem', cutText: 'b-cut-text-12' },
      { field: 'profit', header: 'Lợi nhuận', width: '10rem', cutText: 'b-cut-text-8' },
      { field: 'actuallyProfit', header: 'Tiền tất toán', width: '10rem', cutText: 'b-cut-text-8' },
    ];

    this.cols = this.cols.map((item, index) => {
      item.position = index + 1;
      return item;
    })

    // this._selectedColumns = this.cols;
    this._selectedColumns = this.getLocalStorage('contractActive') ?? this.cols;
  }

  getLocalStorage(key) {
    return JSON.parse(localStorage.getItem(key))
  }

  setLocalStorage(data) {
    return localStorage.setItem('contractActive', JSON.stringify(data));
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

  genListAction(data = []) {
    this.listAction = data.map(orderItem => {
      console.log("orderItem",orderItem);
      
      const actions = [];

      if (this.isGranted([this.PermissionCompanyShareConst.CompanyShareHDPP_HDDH_ThongTinDauTu])) {
        actions.push({
          data: orderItem,
          label: 'Thông tin chi tiết',
          icon: 'pi pi-info-circle',
          command: ($event) => {
            this.detail($event.item.data);
          }
        })
      }
      
      //
      // if (this.isGranted([this.PermissionCompanyShareConst.CompanyShareHDPP_HDDH_DuyetTaiTuc])) {
      //   actions.push({
      //     data: orderItem,
      //     label: 'Duyệt tái tục',
      //     icon: 'pi pi-check-circle',
      //     command: ($event) => {
      //       // function
      //     }
      //   })
      // }

      return actions;
    });
  }

  showData(rows) {
    for (let row of rows) {
        row.policyDetailName = row?.policyDetailName ?? row?.policyDetail?.name,
        row.profit = this.utils.transformPercent(row?.profit),
        row.actuallyProfit = this.utils.transformPercent(row?.amountMoney || row?.actuallyProfit),
        row.totalValue = this.utils.transformPercent(row?.companyShareOrder.totalValue);
    };
  }

  init() {
    this.isLoading = true;
    forkJoin([this._orderService.getAll(this.page, 'orderContract', this.status, this.dataFilter), this._companyShareSecondaryService.getAllNoPaging()]).subscribe(([resOrder, resSecondary]) => {
      this.isLoading = false;
      if (this.handleResponseInterceptor(resOrder, '')) {
        this.page.totalItems = resOrder?.data?.totalItems;
        this.rows = resOrder?.data?.items;
        this.companyShareSecondarys = resSecondary?.data?.items;
        this.companyShareSecondarys = [...[{ companyShareName: 'Tất cả', companyShareSecondaryId: '' }], ...this.companyShareSecondarys];
        if (this.rows?.length) {
          this.genListAction(this.rows);
          this.showData(this.rows)
        }
        console.log({ rowsOrder: resOrder.data.items, totalItems: resOrder.data.totalItems, resSecondary: resSecondary.data.items });
      }
    });
  }

  setPage(pageInfo?: any) {
    this.selectedContracts = [];
    this.page.pageNumber = pageInfo?.page ?? this.offset;
    if (pageInfo?.rows) this.page.pageSize = pageInfo?.rows;

    this.page.keyword = this.keyword;
    //
    this.isLoading = true;
    this._interestPaymentService.getAllContractInterest(this.page, this.dataFilter, this.linkForStepPayment[this.dataFilter.typeInterest].api).subscribe((res) => {
      this.isLoading = false;
      console.log('res______', res);
      //
      if (this.handleResponseInterceptor(res, '')) {
        this.page.totalItems = res.data.result.totalItems;
        this.rows = res.data.result.items;

        if (this.rows?.length) {
          this.genListAction(this.rows);
          this.showData(this.rows)
        }
        console.log({ rowsOrder: res.data.items, totalItems: res.data.totalItems });
      }
    });
  }

  changeTypeInterest(typeInterest) {
		this.loadingStep = this.linkForStepPayment[typeInterest].loadingStep;
		this.setPage();
	}

  payInterest() {
		const body = { ids: [] };
		for(let contract of this.selectedContracts) {
			body.ids.push(contract.id);
		}

		console.log('body____', body);
		this.isLoading = true;
		this._interestPaymentService.payInterest(body).subscribe((res) => {
			this.isLoading = false;
			if (this.handleResponseInterceptor(res, '')) {
			  this.loadingStep = 100;
				this.dataFilter.typeInterest = InterestPaymentConst.STATUS_DONE;
				this.setPage();
			}
		});
	}

  createList() {
    const body = { interestPayments: [] };
    for(let order of this.selectedContracts) {
			const item = {};
			for (const [key, value] of Object.entries(this.interestPaymentItem)) {
				item[key] = order[key];
			}
			body.interestPayments.push(item);
		}

		console.log('body____', body);
		this.isLoading = true;
		this._interestPaymentService.addListInterest(body).subscribe((res) => {
			this.isLoading = false;
			if (this.handleResponseInterceptor(res, '')) {
			  this.loadingStep = 50;
				this.dataFilter.typeInterest = InterestPaymentConst.STATUS_CREATED_LIST;
				this.setPage();
			}
		});
  }

  pushPay() {
    this.loadingStep = 100;
    this.setPage();
  }

  changeFieldFilter() {
    if (this.keyword) {
      this.setPage();
    }
  }

  changeStatus() {
    this.setPage({ Page: this.offset })
  }

  changeKeywordSearch() {
    this.setPage({ Page: this.offset })
  }

  detail(order) {
    this.router.navigate(['/trading-contract/interest-contract/detail/' + this.cryptEncode(order?.orderId)]);
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
