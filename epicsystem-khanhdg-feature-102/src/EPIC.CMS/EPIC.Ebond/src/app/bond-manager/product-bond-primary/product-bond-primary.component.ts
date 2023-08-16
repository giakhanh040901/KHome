import { FormRequestComponent } from './../../form-request-approve-cancel/form-request/form-request.component';
import { FormApproveComponent } from './../../form-request-approve-cancel/form-approve/form-approve.component';
import { ProductBondPrimaryDetailComponent } from './product-bond-primary-detail/product-bond-primary-detail.component';
import { Component, Injector, Input, OnInit } from '@angular/core';
import { ProductBondPrimaryConst, ProductBondInfoConst, ProductBondDetailConst, SearchConst, KeyFilter, YesNoConst, FormNotificationConst } from '@shared/AppConsts';
import { CrudComponentBase } from '@shared/crud-component-base';
import { Page } from '@shared/model/page';
import { ProductBondPrimaryServiceProxy } from '@shared/service-proxies/bond-manager-service';
import { ConfirmationService, MessageService } from 'primeng/api';
import { forkJoin } from 'rxjs';
import { BreadcrumbService } from 'src/app/layout/breadcrumb/breadcrumb.service';
import { Router } from "@angular/router"
import { ActivatedRoute } from '@angular/router';
import { DialogService } from 'primeng/dynamicdialog';
import { DynamicDialogRef } from 'primeng/dynamicdialog';
import { debounceTime } from 'rxjs/operators';
import { FilterTradingProviderComponent } from '../distribution-contract/filter-trading-provider/filter-trading-provider.component';
import { FormCancelComponent } from 'src/app/form-request-approve-cancel/form-cancel/form-cancel.component';
import { FormSetDisplayColumnComponent } from 'src/app/form-set-display-column/form-set-display-column/form-set-display-column.component';
import { FormNotificationComponent } from 'src/app/form-notification/form-notification.component';

@Component({
  selector: 'app-product-bond-primary',
  templateUrl: './product-bond-primary.component.html',
  styleUrls: ['./product-bond-primary.component.scss'],
  providers: [DialogService, ConfirmationService, MessageService]
})
export class ProductBondPrimaryComponent extends CrudComponentBase {

  constructor(
    injector: Injector,
    private dialogService: DialogService,
    private confirmationService: ConfirmationService,
    private router: Router,
    private routeActive: ActivatedRoute,
    private _productBondPrimaryService: ProductBondPrimaryServiceProxy,
    messageService: MessageService,
    private breadcrumbService: BreadcrumbService,
    private _dialogService: DialogService,
  ) {
    super(injector, messageService);
    this.breadcrumbService.setItems([
      { label: 'Trang chủ', routerLink: ['/home'] },
      { label: 'Phát hành sơ cấp' },
    ]);
  }

  statusSearch: any[] = [
    {
      name: 'Tất cả',
      code: ''
    },
    ...ProductBondPrimaryConst.status,
  ];

  ref: DynamicDialogRef;

  KeyFilter = KeyFilter;
  modalDialog: boolean;
  modalDialogDetail: boolean;
  deleteItemDialog: boolean = false;

  confirmRequestDialog: boolean = false;

  deleteItemsDialog: boolean = false;
  rows: any[] = [];


  productBondPrimary: any = {
    "bondPrimaryId": 0,
    "productBondId": null,
    "tradingProviderId": null,
    "contractCode": null,
    "quantity": null,
    "numberInvestor": null,
    "priceType": null,
    "minMoney": null,
    "holdDate": null,
    "openCellDate": null,
    "closeCellDate": null,
    "status": null,
    "businessCustomerBankAccId": null,
    "soLuongTraiPhieuConLai": null
    // "bondTypeId": null,
    // "paymentType": null,
    // "policyIds":[],
  }
  listAction: any[] = [];

  itemBondInfo: any = {
    "codeName": null,  // Mã - Tên TP
    "maxInvestor": null, // Số NĐT max
    "parValue": null,  // Mệnh giá
    "quantity": null,  // Số lượng
    "totalValue": null,  // Tổng giá trị phát hành
    "bondPeriod": null,  // Kỳ hạn 
    "interestPeriod": null,  // Kỳ hạn trả lãi
    "issueDate": null, // Ngày phát hành
    "dueDate": null, // Ngày đáo hạn
  };



  fieldErrors = {};
  fieldDates = ['holdDate', 'openCellDate', 'closeCellDate'];
  submitted: boolean;

  ProductBondDetailConst = ProductBondDetailConst;
  ProductBondInfoConst = ProductBondInfoConst;
  ProductBondPrimaryConst = ProductBondPrimaryConst;
  YesNoConst = YesNoConst;

  page = new Page();
  offset = 0;

  bondInfos: any = [];
  tradingProviders: any = [];
  bondInfoBanks: any[] = [];

  itemTradingProviderInfo = {};

  productBondPrimaryId: number;

  actions: any[] = [];
  actionsDisplay: any[] = [];

  minDate = null;
  maxDate = null;
  maxInvestor = 999999999999999;

  row: any;
  col: any;

  cols: any[];
  _selectedColumns: any[];

  ngOnInit() {
    this.status = '';

    this.subject.keyword.pipe(debounceTime(SearchConst.DEBOUNCE_TIME)).subscribe(() => {
      if (this.keyword === "") {
        this.setPage({ page: this.offset });
      } else {
        this.setPage();
      }
    });

    this.isLoading = true;
    this._productBondPrimaryService.getAllBondInfo(this.page).subscribe((resBondInfo) => {
      if (this.handleResponseInterceptor(resBondInfo, '')) {
        this.bondInfos = resBondInfo?.data?.items ?? [];
        // Lọc TCPH có số lượng còn lại > 0 (Update Api)
        if (this.bondInfos?.length) {
          this.bondInfos = this.bondInfos.filter(item => item.soLuongConLai > 0);
          for (let bondInfo of this.bondInfos) {
            bondInfo.labelName = bondInfo.bondCode + ' - ' + bondInfo.bondName;
          }
        }
        console.log({ 'resBondInfo': resBondInfo });
      }
      this.setPage();
    }, (err) => {
      this.setPage();
      console.log('Error-------', err);
    });
    //
    const T = 'Khởi tạo';
    const P = 'Chờ duyệt';
    const A = 'Hoạt động';
    const C = 'Đóng';

    this.cols = [
      { field: 'bondCode', header: 'Mã trái phiếu', width: '10rem', cutText: 'b-cut-text-10', isPin: true },
      { field: 'tradingProviderName', header: 'Đại lý sơ cấp', width: '16rem', cutText: 'b-cut-text-16', isPin: true },
      { field: 'contractCode', header: 'Mã hợp đồng', width: '15rem', cutText: 'b-cut-text-15' },
      { field: 'formatQuantity', header: 'Số lượng TP', width: '10rem', class: 'justify-content-end', cutText: 'b-cut-text-10' },
      { field: 'soLuongTraiPhieuNamGiu', header: 'SL TP nắm giữ', width: '10rem', class: 'justify-content-end', cutText: 'b-cut-text-10' },
      { field: 'soLuongTraiPhieuConLai', header: 'SL còn lại', width: '10rem', class: 'justify-content-end', cutText: 'b-cut-text-12' },
      { field: 'totalValue', header: 'Tổng giá trị', width: '12rem', class: 'justify-content-end', cutText: 'b-cut-text-12' },
      { field: 'namePriceType', header: 'Kiểu tính giá', width: '15rem', cutText: 'b-cut-text-15' },
      { field: 'minMoney', header: 'Số tiền đầu tư tối thiểu', width: '14rem', class: 'justify-content-center', cutText: 'b-cut-text-10' },
      { field: 'openCellDate', header: 'Ngày mở bán', width: '10rem', class: 'justify-content-center', cutText: 'b-cut-text-10' },
      { field: 'closeCellDate', header: 'Ngày đóng', width: '10rem', class: 'justify-content-center', cutText: 'b-cut-text-10' },
    ];

		this.cols = this.cols.map((item, index) => {
			item.position = index + 1;
			return item;
		})

    // this._selectedColumns = this.cols;
    this._selectedColumns = this.getLocalStorage('bondPrimary') ?? this.cols;
  }

  getLocalStorage(key) {
    return JSON.parse(localStorage.getItem(key))
  }
  setLocalStorage(data) {
    return localStorage.setItem('bondPrimary', JSON.stringify(data));
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
      row.bondCode = row?.productBondInfo?.bondCode,
      row.tradingProviderName = row?.tradingProvider?.businessCustomer?.shortName,
      row.contractCode = row?.contractCode,
      row.formatQuantity = this.utils.transformMoney(row?.quantity),
      row.soLuongTraiPhieuNamGiu = this.utils.transformMoney(row?.soLuongTraiPhieuNamGiu),
      row.soLuongTraiPhieuConLai = this.utils.transformMoney(row?.soLuongTraiPhieuConLai),
      row.namePriceType = this.ProductBondPrimaryConst.getNamePriceType(row?.priceType),
      row.totalValue = this.utils.transformMoney(row?.quantity * row?.productBondInfo?.parValue),
      row.minMoney = this.utils.transformMoney(row?.minMoney),
      row.openCellDate = this.formatDate(row?.openCellDate),
      row.closeCellDate = this.formatDate(row?.closeCellDate)
      console.log('So luong', row.quantity, 'DOn gia', row?.productBondInfo?.parValue,'123', row.totalValue);
    };
    console.log('row', rows);
  }

  genListAction(data = []) {
    this.listAction = data.map(bondPrimaryItem => {
      const actions = [];

      if (this.isGranted([this.PermissionBondConst.BondMenuQLTP_PHSC_TTCT])){
				actions.push({
          data: bondPrimaryItem,
          label: 'Thông tin chi tiết',
          icon: 'pi pi-info-circle',
          command: ($event) => {
            this.detail($event.item.data);
          }
				})
			}

      if (bondPrimaryItem.status == this.ProductBondPrimaryConst.KHOI_TAO && this.isGranted([this.PermissionBondConst.BondMenuQLTP_PHSC_Xoa])) {
        actions.push({
          data: bondPrimaryItem,
          label: 'Xoá',
          icon: 'pi pi-trash',
          command: ($event) => {
            this.delete($event.item.data);
          }
        });
      }

      // if (bondPrimaryItem.status == this.ProductBondPrimaryConst.KHOI_TAO && this.isGranted(this.PermissionBondConst.BondMenuQLTP_PHSC_TrinhDuyet)) {
      //   actions.push({
      //     data: bondPrimaryItem,
      //     label: 'Trình duyệt',
      //     icon: 'pi pi-arrow-up',
      //     command: ($event) => {
      //       this.request($event.item.data);
      //     }
      //   });
      // }

      // if (bondPrimaryItem.status == this.ProductBondPrimaryConst.TRINH_DUYET && this.isGranted(this.PermissionBondConst.BondMenuQLTP_PHSC_PheDuyetOrHuy)) {
      //   actions.push({
      //     data: bondPrimaryItem,
      //     label: 'Phê duyệt',
      //     icon: 'pi pi-check',
      //     command: ($event) => {
      //       this.approve($event.item.data);
      //     }
      //   });
      // }

      // if (bondPrimaryItem.status == this.ProductBondPrimaryConst.TRINH_DUYET && this.isGranted(this.PermissionBondConst.BondMenuQLTP_PHSC_PheDuyetOrHuy)) {
      //   actions.push({
      //     data: bondPrimaryItem,
      //     label: 'Hủy duyệt',
      //     icon: 'pi pi-times',
      //     command: ($event) => {
      //       this.cancel($event.item.data);
      //     }
      //   });
      // }

      // if (bondPrimaryItem.status == this.ProductBondPrimaryConst.HOAT_DONG && bondPrimaryItem.isCheck == false && this.isGranted()) {
      //   actions.push({
      //     data: bondPrimaryItem,
      //     label: 'Phê duyệt (Epic)',
      //     icon: 'pi pi-check',
      //     command: ($event) => {
      //       this.check($event.item.data);
      //     }
      //   });
      // }

      // if (bondPrimaryItem.status == this.ProductBondPrimaryConst.HOAT_DONG && this.isGranted()) {
      //   actions.push({
      //     data: bondPrimaryItem,
      //     label: 'Đóng',
      //     icon: 'pi pi-lock',
      //     command: ($event) => {
      //       this.close($event.item.data);
      //     }
      //   });
      // }
      return actions;
    });
  }

  changeStatus() {
    this.setPage({ Page: this.offset })
  }

  setPage(pageInfo?: any) {
    this.page.pageNumber = pageInfo?.page ?? this.offset;
    if(pageInfo?.rows) this.page.pageSize = pageInfo?.rows;
    this.page.keyword = this.keyword;
    this.isLoading = true;
    //
    this._productBondPrimaryService.getAll(this.page, this.status).subscribe((res) => {
      this.isLoading = false;
      if (this.handleResponseInterceptor(res, '')) {
        this.page.totalItems = res.data.totalItems;
        this.rows = res.data.items;
        if (res.data?.items) {
          this.rows = res.data.items.map(row => {
            row.isCheck = (row.isCheck == this.YesNoConst.STATUS_YES);
            return row;
          });
        }
        //
        if(this.rows?.length) { 
          this.genListAction(this.rows);
          this.showData(this.rows)
        }
        console.log({ rows: res.data.items, totalItems: res.data.totalItems });
      }
    }, (err) => {
      this.isLoading = false;
      console.log('Error-------', err);
      
    });
  }

  changeBondInfo(productBondId) {
    let itemBondInfo = this.bondInfos.find(item => item.productBondId == productBondId);
    if (itemBondInfo) {
      this.bondInfoBanks = itemBondInfo?.issuer?.businiessCustomer?.businessCustomerBanks;
      console.log({ banks: this.bondInfoBanks });
    }
  }

  clickDropdown(row) {
    this.productBondPrimary = { ...row };
    this.actionsDisplay = this.actions.filter(action => action.statusActive.includes(row.status) && action.permission);
  }

  detail(productBondPrimary) {

    this.router.navigate(['/bond-manager/product-bond-primary/detail/' + this.cryptEncode(productBondPrimary?.bondPrimaryId)]);

  }

  setFieldError() {
    for (const [key, value] of Object.entries(this.productBondPrimary)) {
      this.fieldErrors[key] = false;
    }
    console.log({ filedError: this.fieldErrors });
  }

  header(): string {
    return this.productBondPrimary?.bondPrimaryId > 0 ? 'Sửa phát hành sơ cấp' : 'Thêm phát hành sơ cấp';
  }

  resetData() {
    this.productBondPrimary = {};
    this.itemBondInfo = {};
    this.itemTradingProviderInfo = {};
  }

  create() {
    this.resetData();
    this.submitted = false;
    this.modalDialog = true;
  }

  changeCellDate(value) {
    if (!this.productBondPrimary?.contractCode?.trim()) {
      this.productBondPrimary.contractCode = this.formatDate(value).split("/").join("") + '/MAHĐ/' + new Date().getTime();
    }
    // Check ngày mở bán không được lớn hơn ngày đóng
    setTimeout(() => {
      let openCellDate = this.productBondPrimary.openCellDate ? +new Date(this.productBondPrimary.openCellDate) : null;
      let closeCellDate = this.productBondPrimary.closeCellDate ? +new Date(this.productBondPrimary.closeCellDate) : null;
      console.log(openCellDate, closeCellDate);

      if (openCellDate && closeCellDate) {
        if (openCellDate >= closeCellDate) {
          this.productBondPrimary.closeCellDate = null;
        }
      }
    }, 100);
  }

  changeProductBond(productBondId) {
    let productBondInfo = this.bondInfos.find(item => item.productBondId == productBondId);
    this.itemBondInfo = [];
    this.itemBondInfo = { ...productBondInfo };
    this.bondInfoBanks = [];
    if (productBondInfo) {
      this.bondInfoBanks = productBondInfo?.issuer?.businessCustomer?.businessCustomerBanks;
      for (let bank of this.bondInfoBanks) {
        bank.labelName = bank.bankAccNo + ' - ' + bank.bankName;
      }
      console.log({ banks: this.bondInfoBanks, productBondInfo: productBondInfo });
    }
    // Show value custom data
    this.itemBondInfo.codeName = productBondInfo?.bondCode + ' - ' + productBondInfo?.bondName;
    this.itemBondInfo.bondPeriod = productBondInfo?.bondPeriod + ' ' + ProductBondInfoConst.getPeriodUnits(productBondInfo?.bondPeriodUnit);
    this.itemBondInfo.interestPeriod = productBondInfo?.interestPeriod + ' ' + ProductBondInfoConst.getPeriodUnits(productBondInfo?.interestPeriodUnit);
    // Set điều kiện thông số của PHSC không được vượt quá Lô TP
    this.minDate = new Date(productBondInfo?.issueDate ?? new Date());
    this.maxDate = new Date(productBondInfo?.dueDate ?? "2100-01-01");

    if (productBondInfo?.maxInvestor) {
      if (this.productBondPrimary.maxInvestor > productBondInfo.maxInvestor) this.productBondPrimary.maxInvestor = null;
      this.maxInvestor = productBondInfo.maxInvestor;
    }

    console.log(productBondId, this.itemBondInfo, productBondInfo);
  }

  // changeTradingProvider(event) {
  //   let tradingProviderId = event?.value;
  //   this.itemTradingProviderInfo = this.tradingProviders.find(item => item.tradingProviderId == tradingProviderId);
  //   console.log(event.value, this.itemTradingProviderInfo);
  // }

  showTradingProvider() {
    const ref = this.dialogService.open(FilterTradingProviderComponent,
      {
        header: 'Tìm kiếm đại lý sơ cấp',
        width: '1000px',
        styleClass: 'p-dialog-custom filter-trading-provider customModal',
        style: {'min-height': '300px','height':'auto', 'top': '-25%'}
        // height: '100%',
      });

    ref.onClose.subscribe((tradingProviderInfo) => {
      if (tradingProviderInfo) {
        this.itemTradingProviderInfo = tradingProviderInfo;
        this.productBondPrimary.tradingProviderId = tradingProviderInfo.tradingProviderId;
      }
    });
  }


  edit() {
    this.productBondPrimary = {
      ...this.productBondPrimary,
      openCellDate: this.productBondPrimary?.openCellDate ? new Date(this.productBondPrimary?.openCellDate) : null,
      closeCellDate: this.productBondPrimary?.closeCellDate ? new Date(this.productBondPrimary?.closeCellDate) : null,
    };

    if (this.productBondPrimary.productBondId) this.changeProductBond({ value: this.productBondPrimary.productBondId });
    // if (this.productBondPrimary.tradingProviderId) this.changeTradingProvider({ value: this.productBondPrimary.tradingProviderId });

    console.log({ productBondPrimary: this.productBondPrimary });
    this.modalDialog = true;
  }

  // delete(productBondPrimary) {
  //   this.confirmationService.confirm({
  //     message: `Bạn có chắc chắn xóa phát hành sơ cấp : ${productBondPrimary?.bondName} này?`,
  //     header: 'Thông báo',
  //     icon: 'pi pi-times-circle',
  //     acceptLabel: 'Đồng ý',
  //     rejectLabel: 'Hủy',
  //     accept: () => {
  //       this._productBondPrimaryService.delete(productBondPrimary?.bondPrimaryId).subscribe(
  //         (response) => {
  //           if (this.handleResponseInterceptor(response, '')) {
  //             this.setPage({ page: this.page.pageNumber });
  //             this.productBondPrimary = {};
  //           }
  //         }, () => {
  //           this.messageService.add({ severity: 'success', summary: '', detail: 'Xóa thành công!', life: 1500 });
  //         }
  //       );
  //     },
  //     reject: () => {

  //     },
  //   });
  // }

  delete(productBondPrimary) {
		const ref = this._dialogService.open(
				FormNotificationComponent,
				{
					header: "Thông báo",
					width: '600px',
					contentStyle: { "max-height": "600px", "overflow": "auto", "padding-bottom": "50px" },
					styleClass: 'p-dialog-custom',
					baseZIndex: 10000,
					data: {
						title : "Bạn có chắc chắn xóa phát hành sơ cấp này?",
						icon: FormNotificationConst.IMAGE_CLOSE,
					},
				}
			);
		ref.onClose.subscribe((dataCallBack) => {
			console.log({ dataCallBack });
			if (dataCallBack?.accept) {
			this._productBondPrimaryService.delete(productBondPrimary?.bondPrimaryId).subscribe((response) => {
			  if (
				this.handleResponseInterceptor(
				  response,
				  "Xóa phát hành sơ cấp thành công"
				)
			  ) {
				this.setPage();
			  }
			});
			}
		});
	  }

  request(productBondPrimary) {
    const summary = 'Phát hành sơ cấp: ' + productBondPrimary.productBondInfo.bondCode + ' - ' + productBondPrimary.productBondInfo.bondName + '( ID: ' + productBondPrimary.bondPrimaryId + ' )';
    const ref = this.dialogService.open(
      FormRequestComponent,
      this.getConfigDialogServiceRAC("Trình duyệt", productBondPrimary.bondPrimaryId, summary)
    );
    //
    ref.onClose.subscribe((dataCallBack) => {
      console.log('dataCallBack', dataCallBack);

      if (dataCallBack?.accept) {
        this._productBondPrimaryService.request(dataCallBack.data).subscribe((response) => {
          if (this.handleResponseInterceptor(response, "Trình duyệt thành công")) {
            this.setPage();
          }
        });
      }
    });
  }

  cancel(productBondPrimary) {
    const ref = this.dialogService.open(
      FormCancelComponent,
      this.getConfigDialogServiceRAC("Hủy phê duyệt", productBondPrimary?.bondPrimaryId)
    );
    //
    ref.onClose.subscribe((dataCallBack) => {
      if (dataCallBack?.accept) {
        this._productBondPrimaryService.cancel(dataCallBack.data).subscribe((response) => {
          if (this.handleResponseInterceptor(response, "Hủy phê duyệt thành công")) {
            this.setPage();
          }
        });
      }
    });
  }

  approve(productBondPrimary) {
    const ref = this.dialogService.open(
      FormApproveComponent,
      this.getConfigDialogServiceRAC("Phê duyệt", productBondPrimary.bondPrimaryId)
    );
    //
    ref.onClose.subscribe((dataCallBack) => {
      if (dataCallBack?.accept) {
        this._productBondPrimaryService.approve(dataCallBack.data).subscribe((response) => {
          if (this.handleResponseInterceptor(response, "Phê duyệt thành công")) {
            this.setPage();
          }
        });
      }
    });
  }

  // Api Epic kiểm tra
  check(productBondPrimary) {
    this.confirmationService.confirm({
      message: 'Bạn có chắc chắn phê duyệt phát hành sơ cấp này không?',
      header: 'Thông báo',
      acceptLabel: "Đồng ý",
			rejectLabel: "Hủy",
      icon: 'pi pi-check-circle',
      accept: () => {
        this._productBondPrimaryService.check({ id: productBondPrimary?.bondPrimaryId }).subscribe((response) => {
          if (this.handleResponseInterceptor(response, "Phê duyệt thành công")) {
            this.setPage();
          }
        });
      },
      reject: () => {
      },
    });
  }

  // close(productBondPrimary) {
  //   this.confirmationService.confirm({
  //     message: 'Bạn có chắc chắn đóng phát hành sơ cấp này này?',
  //     header: 'Đóng phát hành sơ cấp!',
  //     icon: 'pi pi-exclamation-triangle',
  //     accept: () => {
  //       this._productBondPrimaryService.close(productBondPrimary?.bondPrimaryId).subscribe((response) => {
  //         if (this.handleResponseInterceptor(response, "Đóng thành công")) {
  //           this.setPage();
  //         }
  //       });
  //     },
  //     reject: () => {

  //     },
  //   });
  // }

  hideDialog() {
    this.modalDialog = false;
    this.submitted = false;
  }

  resetValid(field) {
    this.fieldErrors[field] = false;
  }

  save() {
    this.submitted = true;
    //
    let body = this.formatCalendar(this.fieldDates, {...this.productBondPrimary});
    console.log({ productBondPrimary: body });

    if (body.bondPrimaryId) {
      this._productBondPrimaryService.update(body).subscribe((response) => {
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
      this._productBondPrimaryService.create(body).subscribe(
        (response) => {
          if (this.handleResponseInterceptor(response, 'Thêm thành công')) {
            this.submitted = false;
            this.hideDialog();
            this.isLoadingPage = true;
            setTimeout(() => {
              this.router.navigate(['/bond-manager/product-bond-primary/detail/', this.cryptEncode(response.data.bondPrimaryId)]);
            }, 1000);
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

    const validRequired = this.productBondPrimary?.productBondId
      && this.productBondPrimary?.tradingProviderId
      && this.productBondPrimary?.quantity
      && this.productBondPrimary?.priceType
      && this.productBondPrimary?.minMoney
      && this.productBondPrimary?.contractCode
      && this.productBondPrimary?.openCellDate;
    return validRequired;
  }

}
