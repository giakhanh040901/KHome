import { FormRequestComponent } from './../../form-request-approve-cancel/form-request/form-request.component';
import { FormApproveComponent } from './../../form-request-approve-cancel/form-approve/form-approve.component';
import { CompanySharePrimaryDetailComponent } from './company-share-primary-detail/company-share-primary-detail.component';
import { Component, Injector, Input, OnInit } from '@angular/core';
import { CompanySharePrimaryConst, CompanyShareInfoConst, CompanyShareDetailConst, SearchConst, KeyFilter, YesNoConst, FormNotificationConst } from '@shared/AppConsts';
import { CrudComponentBase } from '@shared/crud-component-base';
import { Page } from '@shared/model/page';
import { CompanySharePrimaryServiceProxy } from '@shared/service-proxies/company-share-manager-service';
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
  selector: 'app-company-share-primary',
  templateUrl: './company-share-primary.component.html',
  styleUrls: ['./company-share-primary.component.scss'],
  providers: [DialogService, ConfirmationService, MessageService]
})
export class CompanySharePrimaryComponent extends CrudComponentBase {

  constructor(
    injector: Injector,
    private dialogService: DialogService,
    private confirmationService: ConfirmationService,
    private router: Router,
    private routeActive: ActivatedRoute,
    private _companySharePrimaryService: CompanySharePrimaryServiceProxy,
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
    ...CompanySharePrimaryConst.status,
  ];

  ref: DynamicDialogRef;

  KeyFilter = KeyFilter;
  modalDialog: boolean;
  modalDialogDetail: boolean;
  deleteItemDialog: boolean = false;

  confirmRequestDialog: boolean = false;

  deleteItemsDialog: boolean = false;
  rows: any[] = [];


  companySharePrimary: any = {
    "companySharePrimaryId": 0,
    "companyShareId": null,
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
    // "company-shareTypeId": null,
    // "paymentType": null,
    // "policyIds":[],
  }
  listAction: any[] = [];

  itemCompanyShareInfo: any = {
    "codeName": null,  // Mã - Tên CP
    "maxInvestor": null, // Số NĐT max
    "parValue": null,  // Mệnh giá
    "quantity": null,  // Số lượng
    "totalValue": null,  // Tổng giá trị phát hành
    "company-sharePeriod": null,  // Kỳ hạn 
    "interestPeriod": null,  // Kỳ hạn trả lãi
    "issueDate": null, // Ngày phát hành
    "dueDate": null, // Ngày đáo hạn
  };



  fieldErrors = {};
  fieldDates = ['holdDate', 'openCellDate', 'closeCellDate'];
  submitted: boolean;

  CompanyShareDetailConst = CompanyShareDetailConst;
  CompanyShareInfoConst = CompanyShareInfoConst;
  CompanySharePrimaryConst = CompanySharePrimaryConst;
  YesNoConst = YesNoConst;

  page = new Page();
  offset = 0;

  companyShareInfos: any = [];
  tradingProviders: any = [];
  companyShareInfoBanks: any[] = [];

  itemTradingProviderInfo = {};

  companySharePrimaryId: number;

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
    this._companySharePrimaryService.getAllCompanyShareInfo(this.page).subscribe((resCompanyShareInfo) => {
      if (this.handleResponseInterceptor(resCompanyShareInfo, '')) {
        this.companyShareInfos = resCompanyShareInfo?.data?.items ?? [];
        // Lọc TCPH có số lượng còn lại > 0 (Update Api)
        if (this.companyShareInfos?.length) {
          this.companyShareInfos = this.companyShareInfos.filter(item => item.soLuongConLai > 0);
          for (let companyShareInfo of this.companyShareInfos) {
            companyShareInfo.labelName = companyShareInfo.companyShareCode + ' - ' + companyShareInfo.companyShareName;
          }
        }
        console.log({ 'resCompanyShareInfo': resCompanyShareInfo });
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
      { field: 'companyShareCode', header: 'Mã CP', width: '10rem', cutText: 'b-cut-text-10', isPin: true },
      { field: 'tradingProviderName', header: 'Đại lý sơ cấp', width: '16rem', cutText: 'b-cut-text-16', isPin: true },
      { field: 'contractCode', header: 'Mã hợp đồng', width: '15rem', cutText: 'b-cut-text-15' },
      { field: 'formatQuantity', header: 'Số lượng CP', width: '10rem', class: 'justify-content-end', cutText: 'b-cut-text-10' },
      { field: 'soLuongTraiPhieuNamGiu', header: 'SL CP nắm giữ', width: '10rem', class: 'justify-content-end', cutText: 'b-cut-text-10' },
      { field: 'soLuongTraiPhieuConLai', header: 'SL CP còn lại', width: '10rem', class: 'justify-content-end', cutText: 'b-cut-text-12' },
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
    this._selectedColumns = this.getLocalStorage('company-sharePrimary') ?? this.cols;
  }

  getLocalStorage(key) {
    return JSON.parse(localStorage.getItem(key))
  }
  setLocalStorage(data) {
    return localStorage.setItem('company-sharePrimary', JSON.stringify(data));
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
      row.tradingProviderName = row?.tradingProvider?.businessCustomer?.shortName,
      row.contractCode = row?.contractCode,
      row.formatQuantity = this.utils.transformMoney(row?.quantity),
      row.soLuongTraiPhieuNamGiu = this.utils.transformMoney(row?.soLuongTraiPhieuNamGiu),
      row.soLuongTraiPhieuConLai = this.utils.transformMoney(row?.soLuongTraiPhieuConLai),
      row.namePriceType = this.CompanySharePrimaryConst.getNamePriceType(row?.priceType),
      row.totalValue = this.utils.transformMoney(row?.quantity * row?.companyShareInfo?.parValue),
      row.minMoney = this.utils.transformMoney(row?.minMoney),
      row.openCellDate = this.formatDate(row?.openCellDate),
      row.closeCellDate = this.formatDate(row?.closeCellDate)
      console.log('So luong', row.quantity, 'DOn gia', row?.companyShareInfo?.parValue,'123', row.totalValue);
    };
    console.log('row', rows);
  }

  genListAction(data = []) {
    this.listAction = data.map(companySharePrimaryItem => {
      const actions = [];

      if (this.isGranted([this.PermissionCompanyShareConst.CompanyShareMenuQLTP_PHSC_TTCT])){
				actions.push({
          data: companySharePrimaryItem,
          label: 'Thông tin chi tiết',
          icon: 'pi pi-info-circle',
          command: ($event) => {
            this.detail($event.item.data);
          }
				})
			}

      if (companySharePrimaryItem.status == this.CompanySharePrimaryConst.KHOI_TAO && this.isGranted([this.PermissionCompanyShareConst.CompanyShareMenuQLTP_PHSC_Xoa])) {
        actions.push({
          data: companySharePrimaryItem,
          label: 'Xoá',
          icon: 'pi pi-trash',
          command: ($event) => {
            this.delete($event.item.data);
          }
        });
      }

      // if (company-sharePrimaryItem.status == this.CompanySharePrimaryConst.KHOI_TAO && this.isGranted(this.PermissionCompanyShareConst.CompanyShareMenuQLTP_PHSC_TrinhDuyet)) {
      //   actions.push({
      //     data: company-sharePrimaryItem,
      //     label: 'Trình duyệt',
      //     icon: 'pi pi-arrow-up',
      //     command: ($event) => {
      //       this.request($event.item.data);
      //     }
      //   });
      // }

      // if (company-sharePrimaryItem.status == this.CompanySharePrimaryConst.TRINH_DUYET && this.isGranted(this.PermissionCompanyShareConst.CompanyShareMenuQLTP_PHSC_PheDuyetOrHuy)) {
      //   actions.push({
      //     data: company-sharePrimaryItem,
      //     label: 'Phê duyệt',
      //     icon: 'pi pi-check',
      //     command: ($event) => {
      //       this.approve($event.item.data);
      //     }
      //   });
      // }

      // if (company-sharePrimaryItem.status == this.CompanySharePrimaryConst.TRINH_DUYET && this.isGranted(this.PermissionCompanyShareConst.CompanyShareMenuQLTP_PHSC_PheDuyetOrHuy)) {
      //   actions.push({
      //     data: company-sharePrimaryItem,
      //     label: 'Hủy duyệt',
      //     icon: 'pi pi-times',
      //     command: ($event) => {
      //       this.cancel($event.item.data);
      //     }
      //   });
      // }

      // if (company-sharePrimaryItem.status == this.CompanySharePrimaryConst.HOAT_DONG && company-sharePrimaryItem.isCheck == false && this.isGranted()) {
      //   actions.push({
      //     data: company-sharePrimaryItem,
      //     label: 'Phê duyệt (Epic)',
      //     icon: 'pi pi-check',
      //     command: ($event) => {
      //       this.check($event.item.data);
      //     }
      //   });
      // }

      // if (company-sharePrimaryItem.status == this.CompanySharePrimaryConst.HOAT_DONG && this.isGranted()) {
      //   actions.push({
      //     data: company-sharePrimaryItem,
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
    this._companySharePrimaryService.getAll(this.page, this.status).subscribe((res) => {
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

  changeCompanyShareInfo(companyShareId) {
    let itemCompanyShareInfo = this.companyShareInfos.find(item => item.companyShareId == companyShareId);
    if (itemCompanyShareInfo) {
      this.companyShareInfoBanks = itemCompanyShareInfo?.issuer?.businiessCustomer?.businessCustomerBanks;
      console.log({ banks: this.companyShareInfoBanks });
    }
  }

  clickDropdown(row) {
    this.companySharePrimary = { ...row };
    this.actionsDisplay = this.actions.filter(action => action.statusActive.includes(row.status) && action.permission);
  }

  detail(companySharePrimary) {

    this.router.navigate(['/company-share-manager/company-share-primary/detail/' + this.cryptEncode(companySharePrimary?.companySharePrimaryId)]);

  }

  setFieldError() {
    for (const [key, value] of Object.entries(this.companySharePrimary)) {
      this.fieldErrors[key] = false;
    }
    console.log({ filedError: this.fieldErrors });
  }

  header(): string {
    return this.companySharePrimary?.companySharePrimaryId > 0 ? 'Sửa phát hành sơ cấp' : 'Thêm phát hành sơ cấp';
  }

  resetData() {
    this.companySharePrimary = {};
    this.itemCompanyShareInfo = {};
    this.itemTradingProviderInfo = {};
  }

  create() {
    this.resetData();
    this.submitted = false;
    this.modalDialog = true;
  }

  changeCellDate(value) {
    if (!this.companySharePrimary?.contractCode?.trim()) {
      this.companySharePrimary.contractCode = this.formatDate(value).split("/").join("") + '/MAHĐ/' + new Date().getTime();
    }
    // Check ngày mở bán không được lớn hơn ngày đóng
    setTimeout(() => {
      let openCellDate = this.companySharePrimary.openCellDate ? +new Date(this.companySharePrimary.openCellDate) : null;
      let closeCellDate = this.companySharePrimary.closeCellDate ? +new Date(this.companySharePrimary.closeCellDate) : null;
      console.log(openCellDate, closeCellDate);

      if (openCellDate && closeCellDate) {
        if (openCellDate >= closeCellDate) {
          this.companySharePrimary.closeCellDate = null;
        }
      }
    }, 100);
  }

  changeCompanyShare(companyShareId) {
    let companyShareInfo = this.companyShareInfos.find(item => item.companyShareId == companyShareId);
    this.itemCompanyShareInfo = [];
    this.itemCompanyShareInfo = { ...companyShareInfo };
    this.companyShareInfoBanks = [];
    if (companyShareInfo) {
      this.companyShareInfoBanks = companyShareInfo?.issuer?.businessCustomer?.businessCustomerBanks;
      for (let bank of this.companyShareInfoBanks) {
        bank.labelName = bank.bankAccNo + ' - ' + bank.bankName;
      }
      console.log({ banks: this.companyShareInfoBanks, companyShareInfo: companyShareInfo });
    }
    // Show value custom data
    this.itemCompanyShareInfo.codeName = companyShareInfo?.companyShareCode + ' - ' + companyShareInfo?.companyShareName;
    this.itemCompanyShareInfo.companySharePeriod = companyShareInfo?.companySharePeriod + ' ' + CompanyShareInfoConst.getPeriodUnits(companyShareInfo?.companySharePeriodUnit);
    this.itemCompanyShareInfo.interestPeriod = companyShareInfo?.interestPeriod + ' ' + CompanyShareInfoConst.getPeriodUnits(companyShareInfo?.interestPeriodUnit);
    // Set điều kiện thông số của PHSC không được vượt quá Lô CP
    this.minDate = new Date(companyShareInfo?.issueDate ?? new Date());
    this.maxDate = new Date(companyShareInfo?.dueDate ?? "2100-01-01");

    if (companyShareInfo?.maxInvestor) {
      if (this.companySharePrimary.maxInvestor > companyShareInfo.maxInvestor) this.companySharePrimary.maxInvestor = null;
      this.maxInvestor = companyShareInfo.maxInvestor;
    }

    console.log(companyShareId, this.itemCompanyShareInfo, companyShareInfo);
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
        this.companySharePrimary.tradingProviderId = tradingProviderInfo.tradingProviderId;
      }
    });
  }


  edit() {
    this.companySharePrimary = {
      ...this.companySharePrimary,
      openCellDate: this.companySharePrimary?.openCellDate ? new Date(this.companySharePrimary?.openCellDate) : null,
      closeCellDate: this.companySharePrimary?.closeCellDate ? new Date(this.companySharePrimary?.closeCellDate) : null,
    };

    if (this.companySharePrimary.companyShareId) this.changeCompanyShare({ value: this.companySharePrimary.companyShareId });
    // if (this.companySharePrimary.tradingProviderId) this.changeTradingProvider({ value: this.companySharePrimary.tradingProviderId });

    console.log({ companySharePrimary: this.companySharePrimary });
    this.modalDialog = true;
  }

  // delete(companySharePrimary) {
  //   this.confirmationService.confirm({
  //     message: `Bạn có chắc chắn xóa phát hành sơ cấp : ${companySharePrimary?.company-shareName} này?`,
  //     header: 'Thông báo',
  //     icon: 'pi pi-times-circle',
  //     acceptLabel: 'Đồng ý',
  //     rejectLabel: 'Hủy',
  //     accept: () => {
  //       this._companySharePrimaryService.delete(companySharePrimary?.company-sharePrimaryId).subscribe(
  //         (response) => {
  //           if (this.handleResponseInterceptor(response, '')) {
  //             this.setPage({ page: this.page.pageNumber });
  //             this.companySharePrimary = {};
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

  delete(companySharePrimary) {
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
			this._companySharePrimaryService.delete(companySharePrimary?.companySharePrimaryId).subscribe((response) => {
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

  request(companySharePrimary) {
    const summary = 'Phát hành sơ cấp: ' + companySharePrimary.companyShareInfo.companyShareCode + ' - ' + companySharePrimary.companyShareInfo.companyShareName + '( ID: ' + companySharePrimary.companySharePrimaryId + ' )';
    const ref = this.dialogService.open(
      FormRequestComponent,
      this.getConfigDialogServiceRAC("Trình duyệt", companySharePrimary.companySharePrimaryId, summary)
    );
    //
    ref.onClose.subscribe((dataCallBack) => {
      console.log('dataCallBack', dataCallBack);

      if (dataCallBack?.accept) {
        this._companySharePrimaryService.request(dataCallBack.data).subscribe((response) => {
          if (this.handleResponseInterceptor(response, "Trình duyệt thành công")) {
            this.setPage();
          }
        });
      }
    });
  }

  cancel(companySharePrimary) {
    const ref = this.dialogService.open(
      FormCancelComponent,
      this.getConfigDialogServiceRAC("Hủy phê duyệt", companySharePrimary?.companySharePrimaryId)
    );
    //
    ref.onClose.subscribe((dataCallBack) => {
      if (dataCallBack?.accept) {
        this._companySharePrimaryService.cancel(dataCallBack.data).subscribe((response) => {
          if (this.handleResponseInterceptor(response, "Hủy phê duyệt thành công")) {
            this.setPage();
          }
        });
      }
    });
  }

  approve(companySharePrimary) {
    const ref = this.dialogService.open(
      FormApproveComponent,
      this.getConfigDialogServiceRAC("Phê duyệt", companySharePrimary.companySharePrimaryId)
    );
    //
    ref.onClose.subscribe((dataCallBack) => {
      if (dataCallBack?.accept) {
        this._companySharePrimaryService.approve(dataCallBack.data).subscribe((response) => {
          if (this.handleResponseInterceptor(response, "Phê duyệt thành công")) {
            this.setPage();
          }
        });
      }
    });
  }

  // Api Epic kiểm tra
  check(companySharePrimary) {
    this.confirmationService.confirm({
      message: 'Bạn có chắc chắn phê duyệt phát hành sơ cấp này không?',
      header: 'Thông báo',
      acceptLabel: "Đồng ý",
			rejectLabel: "Hủy",
      icon: 'pi pi-check-circle',
      accept: () => {
        this._companySharePrimaryService.check({ id: companySharePrimary?.companySharePrimaryId }).subscribe((response) => {
          if (this.handleResponseInterceptor(response, "Phê duyệt thành công")) {
            this.setPage();
          }
        });
      },
      reject: () => {
      },
    });
  }

  // close(companySharePrimary) {
  //   this.confirmationService.confirm({
  //     message: 'Bạn có chắc chắn đóng phát hành sơ cấp này này?',
  //     header: 'Đóng phát hành sơ cấp!',
  //     icon: 'pi pi-exclamation-triangle',
  //     accept: () => {
  //       this._companySharePrimaryService.close(companySharePrimary?.company-sharePrimaryId).subscribe((response) => {
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
    this.setTimeZoneList(this.fieldDates, this.companySharePrimary);
    console.log({ companySharePrimary: this.companySharePrimary });

    if (this.companySharePrimary.companySharePrimaryId) {
      this._companySharePrimaryService.update(this.companySharePrimary).subscribe((response) => {
        this.callTriggerFiledError(response, this.fieldErrors);
        if (this.handleResponseInterceptor(response, 'Cập nhật thành công')) {
          this.submitted = false;
          this.setPage({ page: this.page.pageNumber });
          this.hideDialog();
        } else {
          this.callTriggerFiledError(response, this.fieldErrors);
          this.resetTimeZoneList(this.fieldDates, this.companySharePrimary);
          this.submitted = false;
        }
      }, () => {
        this.submitted = false;
      }
      );
    } else {
      this._companySharePrimaryService.create(this.companySharePrimary).subscribe(
        (response) => {
          if (this.handleResponseInterceptor(response, 'Thêm thành công')) {
            this.submitted = false;
            this.hideDialog();
            this.isLoadingPage = true;
            setTimeout(() => {
              this.router.navigate(['/company-share-manager/company-share-primary/detail/', this.cryptEncode(response.data.companySharePrimaryId)]);
            }, 1000);
          } else {
            this.callTriggerFiledError(response, this.fieldErrors);
            this.resetTimeZoneList(this.fieldDates, this.companySharePrimary);
            this.submitted = false;
          }
        }, () => {
          this.submitted = false;
        }
      );
    }
  }

  validForm(): boolean {

    const validRequired = this.companySharePrimary?.companyShareId
      && this.companySharePrimary?.tradingProviderId
      && this.companySharePrimary?.quantity
      && this.companySharePrimary?.priceType
      && this.companySharePrimary?.minMoney
      && this.companySharePrimary?.contractCode
      && this.companySharePrimary?.openCellDate;
    return validRequired;
  }

}
