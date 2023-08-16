import { FormCancelComponent } from './../../../form-request-approve-cancel/form-cancel/form-cancel.component';
import { FormApproveComponent } from './../../../form-request-approve-cancel/form-approve/form-approve.component';
import { Component, Injector, Input, OnInit, Output } from '@angular/core';
import { DistributionContractConst, FormNotificationConst } from '@shared/AppConsts';
import { CrudComponentBase } from '@shared/crud-component-base';
import { Page } from '@shared/model/page';
import { DistributionContractPaymentServiceProxy, DistributionContractServiceProxy } from '@shared/service-proxies/bond-manager-service';
import { ConfirmationService, MessageService } from 'primeng/api';
import { BreadcrumbService } from 'src/app/layout/breadcrumb/breadcrumb.service';
import { Router } from "@angular/router"
import { ActivatedRoute } from '@angular/router';
import { DialogService } from 'primeng/dynamicdialog';
import { DynamicDialogRef } from 'primeng/dynamicdialog';
import { TradingProviderServiceProxy } from '@shared/service-proxies/setting-service';
import { FormNotificationComponent } from 'src/app/form-notification/form-notification.component';

@Component({
  selector: 'app-distribution-contract-payment',
  templateUrl: './distribution-contract-payment.component.html',
  styleUrls: ['./distribution-contract-payment.component.scss'],
  providers: [ConfirmationService, MessageService]
})
export class DistributionContractPaymentComponent extends CrudComponentBase {

  constructor(
    injector: Injector,
    messageService: MessageService,
    private confirmationService: ConfirmationService,
    private _dialogService: DialogService,
    private router: Router,
    private routeActive: ActivatedRoute,
    private _distributionContractPaymentService: DistributionContractPaymentServiceProxy,

    ) {
    super(injector, messageService);
    //
    
    this.distributionContractId = this.cryptDecode(this.routeActive.snapshot.paramMap.get('id'));
    
  }

  distributionContractId: string;
  @Input() distributionContractDetail: any = {};

  ref: DynamicDialogRef;

  modalDialog: boolean;
  deleteItemDialog: boolean = false;

  confirmRequestDialog: boolean = false;

  rows: any[] = [];

  DistributionContractConst = DistributionContractConst;

  distributionContractPayment: any = {
    "paymentId": 0,  // id
    "codeId": 0,
    "tradingDate": null,
    "distributionContractId": 0,  // id
    "transactionType": null, // Loại giao dịch
    "tradingType": null,
    "paymentType": null,  // Loại thanh toán
    "totalValue": null,  // Số tiền
    "description": null, // Mô tả
  }

  itemTradingProviderInfo = {};
  fieldErrors = {};
  fieldDates = ['tradingDate'];
  submitted: boolean;
  //
  page = new Page();
  offset = 0;
  //
  bondInfos: any = [];
  tradingProviders: any = [];
  bondTypes: any = [];
  tradingProviderInfo: any = {};
  productBondPrimaryInfo: any = {};
  paymentCurrencyInfo = {
    'total': 0, // Tổng giá trị
    'paid': 0,  // Đã thanh toán
    'remain': 0, // Còn lại
  };

  isDetail = false;
  actionsDisplay: any[] = [];
  actions: any[] = [];
  listAction: any[] = [];

  disableButtonPayment = false;

  ngOnInit() {
    this.actions = [];

    this.paymentCurrencyInfo.total = this.distributionContractDetail.totalValue;
    this.getPaymentInfo();
    this.setPage();

    console.log({ distributionContractDetailPayment: this.distributionContractDetail, paymentCurrencyInfo: this.paymentCurrencyInfo });
  }

  genListAction(data = []) {
    this.listAction = data.map(orderItem => {
        const actions = [];

        if (orderItem.status == this.DistributionContractConst.PAYMENT_TEMP){
          if (this.isGranted([this.PermissionBondConst.BondMenuQLTP_HDPP_TTCT])){
            actions.push({
              data: orderItem,
              label: 'Sửa',
              icon: 'pi pi-pencil',
              command: ($event) => {
                this.edit($event.item.data);
              }
            })
          }
          if (this.isGranted([this.PermissionBondConst.Bond_HDPP_TTTT_PheDuyet])){
            actions.push({
              data: orderItem,
              label: 'Phê duyệt',
              icon: 'pi pi-check',
              command: ($event) => {
                this.approve($event.item.data);
              }
            })
          }
          if (this.isGranted([this.PermissionBondConst.Bond_HDPP_TTTT_Xoa])){
            actions.push({
              data: orderItem,
              label: 'Xóa',
              icon: 'pi pi-trash',
              command: ($event) => {
                this.delete($event.item.data);
              }
            })
          }
        }     
        
        if ( (orderItem.status == this.DistributionContractConst.PAYMENT_SUCCESS) || (orderItem.status == this.DistributionContractConst.PAYMENT_CLOSE) 
            && this.isGranted([this.PermissionBondConst.Bond_HDPP_TTTT_ChiTietThanhToan])){
          actions.push({
            data: orderItem,
            label: 'Chi tiết thanh toán',
            icon: 'pi pi-info-circle',
            command: ($event) => {
              this.detail($event.item.data);
            }
          })
        }

        if (orderItem.status == this.DistributionContractConst.PAYMENT_SUCCESS && this.isGranted([this.PermissionBondConst.Bond_HDPP_TTTT_HuyPheDuyet])) {
          actions.push(
          {
            data: orderItem,
            label: 'Hủy phê duyệt',
            icon: 'pi pi-times',
            command: ($event) => {
              this.cancel($event.item.data);
            }
            });
      }
      console.log("orderItem.status",this.actions);
      
        return actions;
    });
}

  clickTab(e) {
  }

  setPage(pageInfo?: any) {
    this.page.pageNumber = pageInfo?.page ?? this.offset;
    this.page.keyword = this.keyword;
    this.isLoading = true;

    this._distributionContractPaymentService.getAll(this.page, this.distributionContractId).subscribe((res) => {
      this.isLoading = false;
      if (this.handleResponseInterceptor(res, '')) {
        this.page.totalItems = res.data.totalItems;
        this.rows = res.data.items;
        if(this.rows.length) {
          this.getPaymentInfo();
          this.genListAction(this.rows);
        }
        console.log({ rowsPayment: res.data.items, totalItems: res.data.totalItems });
      }
    }, () => {
      this.isLoading = false;
    });
  }

  clickDropdown(row) {
    this.distributionContractPayment = { ...row };
    console.log({ distributionContractPayment: row });
    this.actionsDisplay = this.actions.filter(action => action.statusActive.includes(+row.status) && action.permission);
  }

  getPaymentInfo() {
    let paid = 0;
    for(let row of this.rows) {
      if(row.status == this.DistributionContractConst.PAYMENT_SUCCESS) {
        paid = paid + row.totalValue;
      }
    }
    this.paymentCurrencyInfo.paid = paid;
    this.paymentCurrencyInfo.remain = this.paymentCurrencyInfo.total - this.paymentCurrencyInfo.paid;
    if(this.paymentCurrencyInfo.remain <= 0) {
      this.disableButtonPayment = true;
    } else {
      this.disableButtonPayment = false;
    }
    console.log({ paymentCurrencyInfo: this.paymentCurrencyInfo })
  }


  setFieldError() {
    for (const [key, value] of Object.entries(this.distributionContractPayment)) {
      this.fieldErrors[key] = false;
    }
    console.log({ filedError: this.fieldErrors });
  }

  header(): string {
    return !this.distributionContractPayment?.status ? 'Thêm thanh toán' : (this.distributionContractPayment?.status == DistributionContractConst.PAYMENT_TEMP ? 'Sửa thanh toán' : 'Chi tiết thanh toán');
  }

  create() {
    this.isDetail = false;
    this.distributionContractPayment = {};
    this.distributionContractPayment.codeId = new Date().getTime();
    this.distributionContractPayment.distributionContractId = +this.distributionContractId;
    this.distributionContractPayment.description = 'TT/HĐ/'+ this.distributionContractDetail.contractCode;
    this.distributionContractPayment.totalValue = this.paymentCurrencyInfo.remain > 0 ? this.paymentCurrencyInfo.remain : null;
    this.distributionContractPayment.paymentType = this.DistributionContractConst.PAYMENT_TYPE_TRANSFER;
    this.distributionContractPayment.tradingDate = new Date();
    
    this.submitted = false;
    this.modalDialog = true;
  }


  edit(distributionContractPayment) {
    this.isDetail = false;
    this.distributionContractPayment = {
      ...distributionContractPayment,
      tradingDate : distributionContractPayment.tradingDate ? new Date(distributionContractPayment.tradingDate) : null,
    };

    console.log({ distributionContractPayment: this.distributionContractPayment });
    this.modalDialog = true;
  }


  detail(distributionContractPayment) {
    this.isDetail = true;
    this.distributionContractPayment = {
      ...distributionContractPayment,
    };

    console.log({ distributionContractPayment: distributionContractPayment });
    this.modalDialog = true;
  }




  confirmDelete(distributionContractPayment) {
    this.deleteItemDialog = false;
    this._distributionContractPaymentService.delete(distributionContractPayment.distributionContractPaymentId).subscribe(
      (response) => {
        if (this.handleResponseInterceptor(response, 'Xóa thành công')) {
          this.setPage({ page: this.page.pageNumber });
          distributionContractPayment = {};
        }
      }, () => {
        this.messageService.add({
          severity: 'error',
          summary: '',
          detail: `Không xóa được hợp đồng phân phối!`,
          life: 3000,
        });
      }
    );
  }

  // delete(distributionContractPayment) {
  //   this.confirmationService.confirm({
  //     message: 'Bạn có chắc chắn xóa thanh toán?',
  //     header: 'Xóa thanh toán',
  //     acceptLabel: "Đồng ý",
	// 		rejectLabel: "Hủy",
  //     icon: 'pi pi-exclamation-triangle',
  //     accept: () => {
  //       this._distributionContractPaymentService.delete(distributionContractPayment.paymentId).subscribe((response) => {
  //         if (this.handleResponseInterceptor(response, "Xóa thành công")) {
  //           this.messageService.add({ severity: 'success', summary: '', detail: 'Thành công!', life: 1500 });
  //           this.setPage();
  //         }
  //       });
  //     },
  //     reject: () => {

  //     },
  //   });
  // }

  delete(distributionContractPayment) {
		const ref = this._dialogService.open(
				FormNotificationComponent,
				{
					header: "Thông báo",
					width: '600px',
					contentStyle: { "max-height": "600px", "overflow": "auto", "padding-bottom": "50px" },
					styleClass: 'p-dialog-custom',
					baseZIndex: 10000,
					data: {
						title : "Bạn có chắc chắn xóa thanh toán?",
						icon: FormNotificationConst.IMAGE_CLOSE,
					},
				}
			);
		ref.onClose.subscribe((dataCallBack) => {
      console.log({ dataCallBack });
			if (dataCallBack?.accept) {
			this._distributionContractPaymentService.delete(distributionContractPayment.paymentId).subscribe((response) => {
			  if (
				this.handleResponseInterceptor(
				  response,
				  "Xóa thanh toán thành công"
				)
			  ) {
				this.setPage();
			  }
			});
			}
		});
	  }


  // approve(distributionContractPayment) {
  //   this.confirmationService.confirm({
  //       message: 'Bạn có muốn phê duyệt?',
  //       header: 'Phê duyệt',
  //       icon: 'pi pi-question-circle',
  //       acceptLabel: 'Đồng ý',
  //       rejectLabel: 'Huỷ',
  //       accept: () => {
  //           let dataApprove = {
  //               id: distributionContractPayment.paymentId,
  //               userApproveId: 1,
  //               approveNote: null,
  //           }
  //           this._distributionContractPaymentService.approve(dataApprove.id).subscribe((response) => {
  //               if (this.handleResponseInterceptor(response, "Phê duyệt thành công")) {
  //                 this.setPage();
  //               }
  //           });
  //       },
  //       reject: () => {
  //       }
  //   });
  // }

  approve(distributionContractPayment) {
		const ref = this._dialogService.open(
				FormNotificationComponent,
				{
					header: "Thông báo",
					width: '600px',
					contentStyle: { "max-height": "600px", "overflow": "auto", "padding-bottom": "50px" },
					styleClass: 'p-dialog-custom',
					baseZIndex: 10000,
					data: {
						title : "Bạn có muốn phê duyệt?",
						icon: FormNotificationConst.IMAGE_APPROVE,
					},
				}
			);
		ref.onClose.subscribe((dataCallBack) => {
      console.log({ dataCallBack });
      let dataApprove = {
        id: distributionContractPayment.paymentId,
        userApproveId: 1,
        approveNote: null,
      }
			if (dataCallBack?.accept) {
			this._distributionContractPaymentService.approve(dataApprove.id).subscribe((response) => {
			  if (
				this.handleResponseInterceptor(
				  response,
				  "Phê duyệt thành công"
				)
			  ) {
				this.setPage();
			  }
			});
			}
		});
	  }

  // cancel(distributionContractPayment) {
  //       this.confirmationService.confirm({
  //           message: 'Bạn có muốn huỷ phê duyệt?',
  //           header: 'Huỷ phê duyệt',
  //           icon: 'pi pi-question-circle',
  //           acceptLabel: 'Đồng ý',
  //           rejectLabel: 'Huỷ',
  //           accept: () => {
  //               let dataCancel = {
  //                   id: distributionContractPayment.paymentId,
  //                   userCancel: 1,
  //                   cancelNote: null,
  //                 }
  //               this._distributionContractPaymentService.cancel(dataCancel.id).subscribe((response) => {
  //                   if (this.handleResponseInterceptor(response, "Đóng thành công")) {
  //                     this.setPage();
  //                   }
  //               });
  //           },
  //           reject: () => {
  //           }
  //       });
  // }

  cancel(distributionContractPayment) {
		const ref = this._dialogService.open(
				FormNotificationComponent,
				{
					header: "Thông báo",
					width: '600px',
					contentStyle: { "max-height": "600px", "overflow": "auto", "padding-bottom": "50px" },
					styleClass: 'p-dialog-custom',
					baseZIndex: 10000,
					data: {
						title : "Bạn có muốn huỷ phê duyệt?",
						icon: FormNotificationConst.IMAGE_CLOSE,
					},
				}
			);
		ref.onClose.subscribe((dataCallBack) => {
      console.log({ dataCallBack });
      let dataCancel = {
          id: distributionContractPayment.paymentId,
          userCancel: 1,
          cancelNote: null,
        }
			if (dataCallBack?.accept) {
			this._distributionContractPaymentService.cancel(dataCancel.id).subscribe((response) => {
			  if (
				this.handleResponseInterceptor(
				  response,
				  "Huỷ phê duyệt thành công"
				)
			  ) {
				this.setPage();
			  }
			});
			}
		});
	  }

  changeKeyword() {
    if (this.keyword === '') {
      this.setPage({ page: this.offset });
    }
  }

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
    let body = this.formatCalendar(this.fieldDates, {...this.distributionContractPayment});
    console.log({ distributionContractPayment: body });
    //
    if (this.distributionContractPayment.paymentId) {
      this._distributionContractPaymentService.update(body).subscribe((response) => {
        //
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
      this._distributionContractPaymentService.create(body).subscribe(
        (response) => {
          if (this.handleResponseInterceptor(response, 'Thêm thành công')) {
            this.submitted = false;
            this.setPage();
            this.hideDialog();
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
    console.log({ distributionContract: this.distributionContractPayment });
    const validRequired = this.distributionContractPayment?.paymentType
      && this.distributionContractPayment?.totalValue;
    return validRequired;
  }
}
