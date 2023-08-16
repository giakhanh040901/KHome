import { Component, Injector, Input, OnInit, Output } from '@angular/core';
import { FormNotificationConst, OrderPaymentConst } from '@shared/AppConsts';
import { CrudComponentBase } from '@shared/crud-component-base';
import { Page } from '@shared/model/page';
import { ConfirmationService, MessageService } from 'primeng/api';
import { BreadcrumbService } from 'src/app/layout/breadcrumb/breadcrumb.service';
import { Router } from "@angular/router"
import { ActivatedRoute } from '@angular/router';
import { DialogService } from 'primeng/dynamicdialog';
import { DynamicDialogRef } from 'primeng/dynamicdialog';
import { OrderPaymentServiceProxy, OrderServiceProxy } from '@shared/service-proxies/trading-contract-service';
import { FormNotificationComponent } from 'src/app/form-notification/form-notification.component';
import { forkJoin } from 'rxjs';


@Component({
  selector: 'app-order-payment',
  templateUrl: './order-payment.component.html',
  styleUrls: ['./order-payment.component.scss'],
  providers: [ConfirmationService, MessageService]
})

export class OrderPaymentComponent extends CrudComponentBase {

  constructor(
    injector: Injector,
    messageService: MessageService,
    private confirmationService: ConfirmationService,
    private router: Router,
    private _orderService: OrderServiceProxy,
    private routeActive: ActivatedRoute,
    private _orderPaymentService: OrderPaymentServiceProxy,
    private _dialogService: DialogService,
    ) {
    super(injector, messageService);
    //
    
    this.orderId = this.cryptDecode(this.routeActive.snapshot.paramMap.get('id'));
  }

  orderId: string;
  @Input() orderDetail: any = {};

  ref: DynamicDialogRef;

  modalDialog: boolean;
  deleteItemDialog: boolean = false;

  confirmRequestDialog: boolean = false;

  rows: any[] = [];

  OrderPaymentConst = OrderPaymentConst;

  orderPayment: any = {
    "orderPaymentId": 0,  // orderPaymentId
    "codeId": 0,
    "tranDate": null, //tranDate
    "orderId": 0,  // id orderId
    "transactionType": null, // Loại giao dịch
    "tranClassify": 1,  // tranClassify
    "paymentType": null,  // Loại thanh toán
    "paymentAmnount": null,  // Số tiền paymentAmnount
    "description": null, // Mô tả
    "status" : null,
  }

  itemTradingProviderInfo = {};
  fieldErrors = {};
  fieldDates = ['tranDate'];
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

  paymentAccountTradingProvider: string ='';

  isDetail = false;
  actionsDisplay: any[] = [];
  actions: any[] = [];
  listAction: any[] = [];

  disableButtonPayment = false;

  ngOnInit() {
    
    this.paymentAccountTradingProvider = this.orderDetail?.tradingProviderBank?.bankName 
                                        + ' - ' + this.orderDetail?.tradingProviderBank?.bankAccNo 
                                        + ' - ' + this.orderDetail?.tradingProviderBank?.bankAccName;

    this.paymentCurrencyInfo.total = this.orderDetail.totalValue;
    this.getPaymentInfo();
    this.setPage();

    console.log({ orderDetailPayment: this.orderDetail, paymentCurrencyInfo: this.paymentCurrencyInfo });
  }

  genListAction(data = []) {
    this.listAction = data.map(orderItem => {
        const actions = [];

        if (this.isGranted([this.PermissionBondConst.BondHDPP_SoLenh_TTCT_TTThanhToan_ChiTietThanhToan])){
          actions.push({
            data: orderItem,
            label: 'Chi tiết thanh toán',
            icon: 'pi pi-info-circle',
            command: ($event) => {
              this.detail($event.item.data);
            }
          })
        }

        if (orderItem.status == this.OrderPaymentConst.PAYMENT_TEMP) {
          if (this.isGranted([this.PermissionBondConst.BondHDPP_SoLenh_TTCT_TTThanhToan_CapNhat])){
            actions.push({
              data: orderItem,
              label: 'Sửa',
              icon: 'pi pi-pencil',
              command: ($event) => {
                this.edit($event.item.data);
              }
            })
          }
          if (this.isGranted([this.PermissionBondConst.BondHDPP_SoLenh_TTCT_TTThanhToan_PheDuyet])){
            actions.push({
              data: orderItem,
              label: 'Phê duyệt',
              icon: 'pi pi-check',
              statusActive: [this.OrderPaymentConst.PAYMENT_TEMP],
              permission: this.isGranted(),
              command: ($event) => {
                this.approve($event.item.data);
              }
            })
          }
          if (this.isGranted([this.PermissionBondConst.BondHDPP_SoLenh_TTCT_TTThanhToan_Xoa])){
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

        if (orderItem.status == this.OrderPaymentConst.PAYMENT_SUCCESS) {
          if (this.isGranted([this.PermissionBondConst.BondHDPP_SoLenh_TTCT_TTThanhToan_HuyPheDuyet])){
            actions.push({
              data: orderItem,
              label: 'Hủy phê duyệt',
              icon: 'pi pi-times',
              command: ($event) => {
                this.cancel($event.item.data);
              }
            })
          }

          if (this.isGranted([this.PermissionBondConst.BondHDPP_SoLenh_TTCT_TTThanhToan_GuiThongBao])){
            actions.push({
              data: orderItem,
              label: 'Gửi thông báo',
              icon: 'pi pi-send',
              command: ($event) => {
                this.resentNotify($event.item.data);
              }
            })
          }
      }
      console.log("orderItem.status",this.actions);
      
        return actions;
    });
}

  clickTab(e){
  }

  setPage(pageInfo?: any) {
    this.page.pageNumber = pageInfo?.page ?? this.offset;
    this.page.keyword = this.keyword;
    this.isLoading = true;
    forkJoin([this._orderPaymentService.getAll(this.page, this.orderId), this._orderService.get(+this.orderId)]).subscribe(([resOrderPayment, resOrder]) => {
      this.isLoading = false;
      if(this.handleResponseInterceptor(resOrder)) {
        this.orderDetail.status = resOrder.data.status;
      }
      //
      if (this.handleResponseInterceptor(resOrderPayment, '')) {
        this.page.totalItems = resOrderPayment.data.totalItems;
        this.rows = resOrderPayment.data.items;
        if (this.rows.length) {
          this.genListAction(this.rows);
          this.getPaymentInfo();
        }
        console.log({ rowsPayment: resOrderPayment.data.items, totalItems: resOrderPayment.data.totalItems });
      }
    }, (err) => {
      console.log('err__', err);
      this.isLoading = false;
    });
  }

  getPaymentInfo() {
    let paid = 0;
    for(let row of this.rows) {
      if(row.status == this.OrderPaymentConst.PAYMENT_SUCCESS) {
        paid = paid + row.paymentAmnount;
      }
    }
    this.paymentCurrencyInfo.paid = paid;
    this.paymentCurrencyInfo.remain = this.paymentCurrencyInfo.total - this.paymentCurrencyInfo.paid;
    if(this.paymentCurrencyInfo.remain <= 0) {
      this.disableButtonPayment = true;
    } else {
      this.disableButtonPayment = false;
    }
  }

  setFieldError() {
    for (const [key, value] of Object.entries(this.orderPayment)) {
      this.fieldErrors[key] = false;
    }
    console.log({ filedError: this.fieldErrors });
  }

  header(): string {
    return !this.orderPayment?.status ? 'Thêm thanh toán' : this.orderPayment?.status == OrderPaymentConst.PAYMENT_TEMP ? 'Sửa thanh toán' : 'Chi tiết thanh toán';
  }

  create() {
    this.isDetail = false;
    this.orderPayment = {};
    this.orderPayment.codeId = new Date().getTime();
    this.orderPayment.orderId = +this.orderId;
    this.orderPayment.description = 'TT '+ this.orderDetail?.contractCode;

    this.orderPayment.paymentAmnount = this.paymentCurrencyInfo.remain > 0 ? this.paymentCurrencyInfo.remain : null;
    // this.orderPayment.paymentType = this.OrderPaymentConst.PAYMENT_TYPE_TRANSFER;
    this.orderPayment.tranDate = new Date();
    
    this.submitted = false;
    this.modalDialog = true;
  }

  edit(orderPayment) {
    console.log('Chinh Sua', orderPayment);
    this.orderPayment = {};
    this.isDetail = false;
    this.orderPayment = {
      ...orderPayment,
      tranDate: orderPayment.tranDate ? new Date(orderPayment.tranDate) : null,
    };

    console.log({ orderPayment: this.orderPayment });
    this.modalDialog = true;
  }


  detail(orderPayment) {
    console.log('Chi Tiet',orderPayment);
    this.orderPayment = {};
    this.isDetail = true;
    this.orderPayment = {
      ...orderPayment,
      tranDate: orderPayment.tranDate ? new Date(orderPayment.tranDate) : null,
    };

    console.log({ orderPayment: this.orderPayment });
    this.modalDialog = true;
  }

  confirmDelete(orderPayment) {
    this.deleteItemDialog = false;
    this._orderPaymentService.delete(this.orderPayment.orderPaymentId).subscribe(
      (response) => {
        if (this.handleResponseInterceptor(response, 'Xóa thành công')) {
          this.setPage({ page: this.page.pageNumber });
          this.orderPayment = {};
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

  delete(orderPayment) {
		const ref = this._dialogService.open(
				FormNotificationComponent,
				{
					header: "Thông báo",
					width: '600px',
					contentStyle: { "max-height": "600px", "overflow": "auto", "padding-bottom": "50px" },
					styleClass: 'p-dialog-custom',
					baseZIndex: 10000,
					data: {
						title : "Bạn có chắc chắn xóa thanh toán này?",
						icon: FormNotificationConst.IMAGE_CLOSE,
					},
				}
			);
		ref.onClose.subscribe((dataCallBack) => {
      console.log({ dataCallBack });
			if (dataCallBack?.accept) {
			this._orderPaymentService.delete(orderPayment.orderPaymentId).subscribe((response) => {
			  if (
				this.handleResponseInterceptor(
				  response,
				  "Xóa thanh toán thành công"
				)
			  ) {
				  this.setPage();
			  }
			});
      } else {
        reject: () => {

        }
      }
		});
	  }
  //

  approve(orderPayment) {
		const ref = this._dialogService.open(
				FormNotificationComponent,
				{
					header: "Thông báo",
					width: '600px',
					contentStyle: { "max-height": "600px", "overflow": "auto", "padding-bottom": "50px" },
					styleClass: 'p-dialog-custom',
					baseZIndex: 10000,
					data: {
						title : "Bạn có chắc chắn phê duyệt thanh toán này?",
						icon: FormNotificationConst.IMAGE_APPROVE,
					},
				}
			);
		ref.onClose.subscribe((dataCallBack) => {
      console.log({ dataCallBack });
			if (dataCallBack?.accept) {
			this._orderPaymentService.approve(orderPayment.orderPaymentId).subscribe((response) => {
			  if (this.handleResponseInterceptor(response, "Phê duyệt thanh toán thành công")) {
				  this.setPage();
			  }
			});
      } else {
        reject: () => {

        }
      }
		});
	}
  //
 
  cancel(orderPayment) {
		const ref = this._dialogService.open(
				FormNotificationComponent,
				{
					header: "Thông báo",
					width: '600px',
					contentStyle: { "max-height": "600px", "overflow": "auto", "padding-bottom": "50px" },
					styleClass: 'p-dialog-custom',
					baseZIndex: 10000,
					data: {
						title : "Bạn có chắc chắn hủy phê duyệt thanh toán này?",
						icon: FormNotificationConst.IMAGE_CLOSE,
					},
				}
			);
		ref.onClose.subscribe((dataCallBack) => {
      console.log({ dataCallBack });
			if (dataCallBack?.accept) {
			this._orderPaymentService.cancel(orderPayment.orderPaymentId).subscribe((response) => {
			  if (
				this.handleResponseInterceptor(
				  response,
				  "Hủy phê duyệt thanh toán thành công"
				)
			  ) {
				this.setPage();
			  }
			});
      } else {
        reject: () => {

        }
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
    let body = this.formatCalendar(this.fieldDates, {...this.orderPayment});
    console.log({ orderPayment: this.orderPayment });
    //
    if (body.orderPaymentId) {
      this._orderPaymentService.update(body).subscribe((response) => {
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
      this._orderPaymentService.create(body).subscribe(
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
    console.log({ order: this.orderPayment });
    const validRequired = this.orderPayment?.paymentType
      && this.orderPayment?.paymentAmnount;
    return validRequired;
  }

  resentNotify(orderPayment) {
    this.confirmationService.confirm({
        message: 'Bạn có chắc chắn gửi thông báo?',
        header: 'Thông báo',
        acceptLabel: "Đồng ý",
        rejectLabel: "Hủy",
        icon: 'pi pi-question-circle',
        accept: () => {
            this._orderPaymentService.resentNotify(orderPayment.orderPaymentId).subscribe((res) => {
                this.handleResponseInterceptor(res, 'Gửi thành công');
            }, (err) =>  {
                this.messageError('Gửi thất bại!', '');
            });
        },
        reject: () => {

        },
    });
}
}
