import { Component, Injector, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { FormNotificationConst } from '@shared/AppConsts';
import { CrudComponentBase } from '@shared/crud-component-base';
import { Page } from '@shared/model/page';
import { DateViewPipe } from '@shared/pipes/dateview.pipe';
import { CompanyShareSecondaryServiceProxy } from '@shared/service-proxies/company-share-manager-service';
import * as moment from 'moment';
import { ConfirmationService, MessageService } from 'primeng/api';
import { DialogService, DynamicDialogRef } from 'primeng/dynamicdialog';
import { FormNotificationComponent } from 'src/app/form-notification/form-notification.component';
import { BreadcrumbService } from 'src/app/layout/breadcrumb/breadcrumb.service';
@Component({
  selector: 'app-company-share-second-price',
  templateUrl: './company-share-second-price.component.html',
  styleUrls: ['./company-share-second-price.component.scss']
})
export class CompanyShareSecondPriceComponent extends CrudComponentBase {
  titleFile: any;

  constructor(
    injector: Injector,
    private routeActive: ActivatedRoute,
    private _companyShareSecondaryServiceProxy: CompanyShareSecondaryServiceProxy,
    messageService: MessageService,
    private breadcrumbService: BreadcrumbService,
    private confirmationService: ConfirmationService,
    private _dialogService: DialogService,

  ) {
    super(injector, messageService);
    this.companyShareSecondaryId = this.cryptDecode(this.routeActive.snapshot.paramMap.get('id'));

  }
  companyShareSecondaryId: any;

  fieldErrors = {};
  //ref: DynamicDialogRef;

  modalDialog: boolean;
  modalDialogPolicyDetailTemplate: boolean;
  modalUpdateDialogPolicyTemplate: boolean;

  deleteItemDialog: boolean = false;
  updateRowDialog: boolean = false;
  deleteCompanySharePolicyTemplateDialog: boolean = false;
  deleteCompanySharePolicyDetailTemplateDialog: boolean = false;

  rows: any[] = [];

  expandedRows = {};

  isExpanded: boolean = false;

  isUpload = false;

  companyShareSecondPrice: any = {
    'priceId': null,
    'tradingProviderId': null,
    'price': null,
    'priceDate': null

  }

  submitted: boolean;

  cols: any[];

  statuses: any[];

  fieldDates = ['priceDate'];

  page = new Page();
  offset = 0;

  fileExcel: any;

  ngOnInit(): void {
    this.setPage({ page: this.offset });
  }

  create() {
    this.submitted = false;
    this.modalDialog = true;
  }
  downloadFile() {
    let link = document.createElement("a");
    link.download = "EXCEL_TEMPLATE.xlsx";
    link.href = "../../../../assets/file_template/EXCEL_TEMPLATE.xlsx";
    link.click();
  }

  importFileExcel() {
    // this.expandAll();
    this.companyShareSecondPrice = {};
    this.submitted = false;
    this.modalDialog = true;

  }

  delete() {

    // this.deleteItemDialog = true;

    const ref = this._dialogService.open(
      FormNotificationComponent,
      {
        header: "Thông báo",
        width: '600px',
        contentStyle: { "max-height": "600px", "overflow": "auto", "padding-bottom": "50px" },
        styleClass: 'p-dialog-custom',
        baseZIndex: 10000,
        data: {
          title : "Bạn có chắc chắn xóa bảng giá bán theo kỳ hạn?",
          icon: FormNotificationConst.IMAGE_CLOSE,
        },
      }
    );
  ref.onClose.subscribe((dataCallBack) => {
    console.log({ dataCallBack });
    if (dataCallBack?.accept) {
    this._companyShareSecondaryServiceProxy.deleteSecondPrice(this.companyShareSecondaryId).subscribe((response) => {
      if (
      this.handleResponseInterceptor(
        response,
        "Xóa bảng giá bán theo kỳ hạn thành công"
      )
      ) {
        this.setPage({ page: this.page.pageNumber });
        this.companyShareSecondPrice = {};
      }
    });
    } else {
        this.messageService.add({
          severity: 'error',
          summary: '',
          detail: `Không xoá được bảng giá bán theo kỳ hạn!`,
          life: 3000,
        });
    }
  });
  }

  confirmDelete() {
    this.deleteItemDialog = false;
    this._companyShareSecondaryServiceProxy.deleteSecondPrice(this.companyShareSecondaryId).subscribe(
      (response) => {
        if (this.handleResponseInterceptor(response, 'Xóa thành công')) {
          this.setPage({ page: this.page.pageNumber });
          this.companyShareSecondPrice = {};
        }
      }, () => {
        this.messageService.add({
          severity: 'error',
          summary: '',
          detail: `Không xoá được bảng giá bán theo kỳ hạn!`,
          life: 3000,
        });
      }
    );

    const ref = this._dialogService.open(
      FormNotificationComponent,
      {
        header: "Thông báo",
        width: '600px',
        contentStyle: { "max-height": "600px", "overflow": "auto", "padding-bottom": "50px" },
        styleClass: 'p-dialog-custom',
        baseZIndex: 10000,
        data: {
          title : "Bạn có chắc chắn xóa bảng giá bán theo kỳ hạn?",
          icon: FormNotificationConst.IMAGE_CLOSE,
        },
      }
    );
  ref.onClose.subscribe((dataCallBack) => {
    console.log({ dataCallBack });
    if (dataCallBack?.accept) {
    this._companyShareSecondaryServiceProxy.deleteSecondPrice(this.companyShareSecondaryId).subscribe((response) => {
      if (
      this.handleResponseInterceptor(
        response,
        "Xóa bảng giá bán theo kỳ hạn thành công"
      )
      ) {
        this.setPage({ page: this.page.pageNumber });
        this.companyShareSecondPrice = {};
      }
    });
    } else {
        this.messageService.add({
          severity: 'error',
          summary: '',
          detail: `Không xoá được bảng giá bán theo kỳ hạn!`,
          life: 3000,
        });
    }
  });
  }

  setPage(pageInfo?: any) {
    this.page.pageNumber = pageInfo?.page ?? this.offset;
    this.isLoading = true;
    this.isLoadingPage = true;
    this._companyShareSecondaryServiceProxy.getAllSecondPrice(this.page, this.companyShareSecondaryId).subscribe((res) => {
      if (this.handleResponseInterceptor(res, '')) {
        console.log('data', res?.data?.items);
        this.rows = res?.data?.items;
        this.isLoadingPage = true;
      }
      this.isLoading = false;
      this.isLoadingPage = false;
    }, (err) => {
      this.isLoading = false;
      this.isLoadingPage = false;
      console.log('Error', err);
      
    });
  }

  // edit(companyShareSecondPrice) {
  //   this.confirmationService.confirm({
  //     message: `Bạn có chắc chắn sửa giá ở ngày ${moment(companyShareSecondPrice.priceDate).format('DD/MM/YYYY')}?`,
  //     header: 'Cảnh báo',
  //     icon: 'pi pi-exclamation-triangle',
  //     acceptLabel: 'Đồng ý',
  //     rejectLabel: 'Hủy',
  //     accept: () => {
  //       this._companyShareSecondaryServiceProxy.updateSecondPrice(companyShareSecondPrice).subscribe(
  //         (response) => {
  //           if (this.handleResponseInterceptor(response, 'Cập nhập thành công')) {
  //             this.setPage({ page: this.page.pageNumber });
  //             this.companyShareSecondPrice = {};

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

  edit(companyShareSecondPrice) {
		const ref = this._dialogService.open(
				FormNotificationComponent,
				{
					header: "Thông báo",
					width: '600px',
					contentStyle: { "max-height": "600px", "overflow": "auto", "padding-bottom": "50px" },
					styleClass: 'p-dialog-custom',
					baseZIndex: 10000,
					data: {
						title : "Bạn có chắc chắn sửa giá ở ngày này?",
						icon: FormNotificationConst.IMAGE_APPROVE,
					},
				}
			);
		ref.onClose.subscribe((dataCallBack) => {
      console.log({ dataCallBack });
			if (dataCallBack?.accept) {
			this._companyShareSecondaryServiceProxy.updateSecondPrice(companyShareSecondPrice).subscribe((response) => {
			  if (
				this.handleResponseInterceptor(
				  response,
				  "Sửa giá ở ngày này thành công"
				)
			  ) {
          this.setPage({ page: this.page.pageNumber });
			  }
			});
			}
		});
	  }


  @ViewChild('fubauto') fubauto: any;

  myUploader(event) {
    this.fileExcel = event?.files[0];
    if (this.fileExcel) {
      this.isLoading = true;
      this._companyShareSecondaryServiceProxy.importPriceFromExcel({ file: this.fileExcel }, this.companyShareSecondaryId).subscribe(
        (response) => {

          this.fubauto.clear();
          if (this.handleResponseInterceptor(response, '')) {
            this.setPage({ page: this.page.pageNumber });
          }
        },
        (err) => {
          this.messageError("Có sự cố khi upload!");
        }
      ).add(() => {
        this.isLoading = false;
      });
    }
  }

  changeKeyword() {
    if (this.keyword === '') {
      this.setPage({ page: this.offset });
    }
  }

  hideModalAll() {
    this.modalDialog = false;
    this.modalDialogPolicyDetailTemplate = false;
    this.modalUpdateDialogPolicyTemplate = false;
  }

  hideDialog(modalName?: string) {
    this[modalName] = false;
    if (!modalName) {
      this.modalDialog = false;

      this.deleteItemDialog = false;
    }
  }
  validForm(): boolean {
    return true;
  }

  formatCurrency(value) {
    return value.toLocaleString('de-DE', { style: 'currency', currency: 'USD' });
  }

}
