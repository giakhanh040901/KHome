import { Component, Injector, OnInit } from '@angular/core';
import { CrudComponentBase } from '@shared/crud-component-base';
import { Page } from '@shared/model/page';
import { CompanyShareInfoServiceProxy, CompanyShareSecondaryServiceProxy } from '@shared/service-proxies/company-share-manager-service';
import { ConfirmationService, MessageService } from 'primeng/api';
import { BreadcrumbService } from 'src/app/layout/breadcrumb/breadcrumb.service';
import { Router } from '@angular/router';
import { OrderServiceProxy } from '@shared/service-proxies/trading-contract-service';
import { BlockageLiberationConst, OrderConst, SearchConst } from '@shared/AppConsts';
import { forkJoin, Subject } from 'rxjs';
import { DialogService } from 'primeng/dynamicdialog';
import { FormSetDisplayColumnComponent } from 'src/app/form-set-display-column/form-set-display-column/form-set-display-column.component';
import { debounceTime } from 'rxjs/operators';
import { BlockageLiberationService } from '@shared/services/blockade-liberation.service';
import { ReinstatementRequestComponent } from './reinstatement-request/reinstatement-request.component';


@Component({
  selector: 'app-contract-active',
  templateUrl: './contract-active.component.html',
  styleUrls: ['./contract-active.component.scss']
})
export class ContractActiveComponent extends CrudComponentBase {

  constructor(
    injector: Injector,
    messageService: MessageService,
    private _orderService: OrderServiceProxy,
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
    orderSource: null
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

  subject = {
    keyword: new Subject(),
  };

  blockage: any = {};
  status: any;

  BlockageLiberationConst = BlockageLiberationConst;

  // blockageTypes  =  BlockageReleaseConst.blockageTypes;
  order: any = {};
  // Menu otions thao tác
  ngOnInit() {
    this.init();
    this.blockageLiberation.type = 2;
    this.setPage({ page: this.offset });
    this.subject.keyword.pipe(debounceTime(SearchConst.DEBOUNCE_TIME)).subscribe(() => {

			if (this.keyword === "") {
				this.setPage({ page: this.offset });
			} else {
				this.setPage();
			}
		});
    this.cols = [
      { field: 'companyShareCode', header: 'Mã TP', width: '12rem', isPin: true },
      { field: 'customerName', header: 'Khách hàng', width: '20rem', isPin: true },
      { field: 'investDate', header: 'Ngày đầu tư', width: '12rem'},
      { field: 'companySharePolicyName', header: 'Sản phẩm', width: '11rem'},
      { field: 'companySharePolicyDetailName', header: 'Kỳ hạn', width: '8rem'},
      { field: 'profitDisplay', header: 'Lợi tức', width: '8rem'},
      { field: 'quantityDisplay', header: 'Số lượng', width: '8rem'}
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

  showData(rows) {
    for (let row of rows) {
      row.companyShareCode = row?.companyShareInfo?.companyShareCode,
        row.customerName = row?.businessCustomer?.name || (row?.investor?.name ?? row?.investor?.investorIdentification?.fullname),
        row.companySharePolicyName = row?.companySharePolicy?.name,
        row.investDate = this.formatDateTime(row?.investDate),
        row.companySharePolicyDetailName = row?.companySharePolicyDetail?.name,
        row.profitDisplay = this.utils.transformPercent(row?.companySharePolicyDetail?.profit) + "%/năm",
        row.quantityDisplay = this.utils.transformMoney(row?.quantity);
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
    }, (err) => {
      this.isLoading = false;
      console.log('Error-------', err);
      
    });
  }

  setPage(pageInfo?: any) {
    this.page.pageNumber = pageInfo?.page ?? this.offset;
    if (pageInfo?.rows) this.page.pageSize = pageInfo?.rows;

    this.page.keyword = this.keyword;
    //
    this.isLoading = true;
    this._orderService.getAll(this.page, 'orderContract', this.status, this.dataFilter).subscribe((res) => {
      this.isLoading = false;
      if (this.handleResponseInterceptor(res, '')) {
        this.page.totalItems = res.data.totalItems;
        this.rows = res.data.items;
        if (this.rows?.length) {
          this.genListAction(this.rows);
          this.showData(this.rows)
        }
        console.log({ rowsOrder: res.data.items, totalItems: res.data.totalItems });
      }
    }, (err) => {
      this.isLoading = false;
      console.log('Error-------', err);
      
    });
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
  
  genListAction(data = []) {
    this.listAction = data.map(orderItem => {
      console.log("orderItem",orderItem);
      
      const actions = [];

      if (this.isGranted([this.PermissionCompanyShareConst.CompanyShareHDPP_SoLenh_TTCT_TTC])){
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
      if (orderItem?.status == OrderConst.DANG_DAU_TU && this.isGranted([this.PermissionCompanyShareConst.CompanyShareHDPP_SoLenh_TTCT_HSKHDangKy_GuiThongBao])) {
        actions.push({
          data: orderItem,
          label: 'Gửi thông báo',
					icon: 'pi pi-envelope',
					command: ($event) => {
						this.resentNotify($event.item.data);
					}
        });
      }

      //                                                
      if (orderItem?.status == OrderConst.DANG_DAU_TU && this.isGranted([this.PermissionCompanyShareConst.CompanyShareHDPP_HopDong_YeuCauTaiTuc]) && !orderItem?.isRenewalsRequest) {
				actions.push({
					data: orderItem,
					label: 'Yêu cầu tái tục',
          icon: 'pi pi-sort-amount-up',
					command: ($event) => {
						this.reinstatementRequest($event.item.data);
					},
				});
			}
      //
      if (orderItem?.status == OrderConst.DANG_DAU_TU && this.isGranted([this.PermissionCompanyShareConst.CompanyShareHDPP_HopDong_PhongToaHopDong])) {
				actions.push({
					data: orderItem,
					label: 'Phong tỏa HĐ',
          icon: 'pi pi-ban',
					command: ($event) => {
						this.blockade($event.item.data);
					},
				});
			}

      return actions;
    });
  }

  detail(order) {
    this.router.navigate(['/trading-contract/order/detail/' + this.cryptEncode(order?.orderId)]);
  }

  // Form gửi thông báo
  resentNotify(order) {
    this.confirmationService.confirm({
        message: 'Bạn có chắc chắn gửi thông báo?',
        header: 'Thông báo',
        acceptLabel: "Đồng ý",
  rejectLabel: "Hủy",
        icon: 'pi pi-question-circle',
        accept: () => {
            this._orderService.resentNotify(order?.orderId).subscribe((res) => {
                if (this.handleResponseInterceptor(res, 'Gửi thành công')) {
                    // this.getOrderDetail();
                };
            }, (err) =>  {
                this.messageError('Gửi thất bại!', '');
            });
        },
        reject: () => {

        },
    });
}

  // Form yêu cầu tái tục
  reinstatementRequest(order) {
    const ref = this.dialogService.open(
			ReinstatementRequestComponent,
			{
				header: "Yêu cầu tái tục",
				width: '450px',
				contentStyle: { "max-height": "600px", "overflow": "auto", "padding": 0, "padding-bottom": "50px" },
        style: { "overflow": "auto" },
				data: {
          orderId: order.orderId,
        }
			}
		);

		ref.onClose.subscribe((dataCallBack) => {
      // xử lý close form
		});
  }

  blockade(contractActive) {
    console.log("contractActive",contractActive);
    
    this.blockageLiberation.orderId = contractActive?.orderId;
    this.blockageLiberation.totalValue = contractActive?.totalValue;
    this.blockageLiberation.blockadeDate = new Date();
    this.blockageDialog = true;
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

  hideDialog() {
    this.blockageDialog = false;
  }
  //
  saveBlockade() {
    this.confirmationService.confirm({
      message: 'Bạn có chắc chắn phong toả hợp đồng này?',
      header: 'Phong toả hợp đồng',
      acceptLabel: "Đồng ý",
      rejectLabel: "Hủy",
      icon: 'pi pi-exclamation-triangle',
      accept: () => {
        this._blockageLiberationService.blockadeContractActive(this.blockageLiberation).subscribe((response) => {
            if (this.handleResponseInterceptor(response, "Phong toả thành công")) {
                this.setPage();
                this.blockageDialog = false;
            }
        });
      },
      reject: () => {

      },
    });
  }
  //
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
