import { Component, Injector, Input, OnInit, Output } from '@angular/core';
import { FormNotificationConst, OrderConst, OrderPaymentConst, YesNoConst } from '@shared/AppConsts';
import { CrudComponentBase } from '@shared/crud-component-base';
import { Page } from '@shared/model/page';
import { ConfirmationService, MessageService } from 'primeng/api';
import { BreadcrumbService } from 'src/app/layout/breadcrumb/breadcrumb.service';
import { Router } from "@angular/router"
import { ActivatedRoute } from '@angular/router';
import { DialogService } from 'primeng/dynamicdialog';
import { DynamicDialogRef } from 'primeng/dynamicdialog';
import { OrderPaymentServiceProxy, OrderServiceProxy } from '@shared/service-proxies/trading-contract-service';
import { forkJoin } from 'rxjs';
import { FormNotificationComponent } from 'src/app/form-general/form-notification/form-notification.component';
import { DistributionService } from '@shared/services/distribution.service';
import { environment } from 'src/environments/environment';
import { FunctionForDevComponent } from './function-for-dev/function-for-dev.component';

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
    private routeActive: ActivatedRoute,
    private _orderPaymentService: OrderPaymentServiceProxy,
    private _dialogService: DialogService,
    private _orderService : OrderServiceProxy,
    private _distributionService: DistributionService,
    ) {
    super(injector, messageService);
    //
    
    this.orderId = this.cryptDecode(this.routeActive.snapshot.paramMap.get('id'));
  }

  orderId: string;
  @Input() orderDetail: any = {};
  @Input() isJustView: any;
  @Input() isPartner: boolean;

  ref: DynamicDialogRef;

  modalDialog: boolean;
  deleteItemDialog: boolean = false;
  environment = environment;
  confirmRequestDialog: boolean = false;

  rows: any[] = [];

  OrderPaymentConst = OrderPaymentConst;
  YesNoConst = YesNoConst;
  OrderConst = OrderConst;

  orderPayment: any = {
    "id": 0,  // id
    "codeId": 0,
    "tranDate": null, //tranDate
    "tranType": 1, // Kiểu giao dịch
    "tranClassify": 1,  // Loại giao dịch
    "paymentType": null,  // Loại thanh toán
    "orderId": 0,  // id orderId
    "paymentAmount": null,  // Số tiền paymentAmount
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
  isLoadingModal: boolean = false;

  paymentAccountTradingProvider: string ='';

  isDetail = false;
  actionsDisplay: any[] = [];
  actions: any[] = [];
  listAction: any[] = [];
  listBanks: any[] = [];
  disableButtonPayment = false;

  ngOnInit() {
    this.paymentAccountTradingProvider = this.getBankInfo(this.orderDetail?.firstPaymentBankDto);
    this.setPage();
  }

  funcForDev() {
    const ref = this._dialogService.open(FunctionForDevComponent, {
      header: "Giả lập MSB",
      width: "800px",
      contentStyle: { "max-height": "600px", overflow: "auto","margin-bottom": "60px" },
      baseZIndex: 10000,
      data: {
        orderDetail: this.orderDetail,
      },
    });
    //
    ref.onClose.subscribe((res) => {
      this.setPage();
    });
  }

  genListAction(data = []) {
    this.listAction = data.map(orderItem => {
      const actions = [];
      if ((orderItem.status == this.OrderPaymentConst.PAYMENT_CLOSE || orderItem.status == this.OrderPaymentConst.PAYMENT_SUCCESS) && this.isGranted([this.PermissionGarnerConst.GarnerHDPP_SoLenh_TTCT_TTThanhToan_ChiTietThanhToan])) {
        actions.push({
            data: orderItem,
            label: 'Chi tiết thanh toán',
            icon: 'pi pi-info-circle',
            command: ($event) => {
              this.detail($event.item.data);
            }
        });
      }

      if (!this.isPartner && orderItem.status == this.OrderPaymentConst.PAYMENT_TEMP && this.isGranted([this.PermissionGarnerConst.GarnerHDPP_SoLenh_TTCT_TTThanhToan_CapNhat])) {
        actions.push({
          data: orderItem,
          label: 'Sửa',
          icon: 'pi pi-pencil',
          command: ($event) => {
            this.edit($event.item.data);
          }
        })
      }

      if (!this.isPartner && orderItem.status == this.OrderPaymentConst.PAYMENT_TEMP && this.isGranted([this.PermissionGarnerConst.GarnerHDPP_SoLenh_TTCT_TTThanhToan_PheDuyet])) {
        actions.push({
          data: orderItem,
          label: 'Phê duyệt',
          icon: 'pi pi-check',
          statusActive: [this.OrderPaymentConst.PAYMENT_TEMP],
          command: ($event) => {
            this.approve($event.item.data);
          }
        })
      }

      if (!this.isPartner && orderItem.status == this.OrderPaymentConst.PAYMENT_SUCCESS && this.isGranted([this.PermissionGarnerConst.GarnerHDPP_SoLenh_TTCT_TTThanhToan_HuyPheDuyet])) {
        actions.push({
          data: orderItem,
          label: 'Hủy phê duyệt',
          icon: 'pi pi-times',
          command: ($event) => {
            this.cancel($event.item.data);
          }
        })
      }

      if (!this.isPartner && orderItem.status == this.OrderPaymentConst.PAYMENT_SUCCESS && this.isGranted([this.PermissionGarnerConst.GarnerHDPP_SoLenh_TTCT_TTThanhToan_GuiThongBao])) {
        actions.push({
          data: orderItem,
          label: 'Gửi thông báo',
          icon: 'pi pi-send',
          command: ($event) => {
            this.resentNotify($event.item.data);
          }
        });
      }

      if (!this.isPartner && orderItem.status == this.OrderPaymentConst.PAYMENT_TEMP && this.isGranted([this.PermissionGarnerConst.GarnerHDPP_SoLenh_TTCT_TTThanhToan_Xoa])) {
        actions.push({
          data: orderItem,
          label: 'Xóa',
          icon: 'pi pi-trash',
          command: ($event) => {
            this.delete($event.item.data);
          }
        });
      }
      
      return actions;
    });
  }

  setPage(pageInfo?: any) {
    this.page.pageNumber = pageInfo?.page ?? this.offset;
    this.page.keyword = this.keyword;
    this.isLoading = true;
    forkJoin([this._orderService.get(+this.orderId), this._orderPaymentService.getAll(this.page, this.orderId)]).subscribe(([resOrder, resOrderPayment]) => {
      this.isLoading = false;
      if(this.handleResponseInterceptor(resOrder)) {
        this.orderDetail.status = resOrder?.data?.status;
        this.orderDetail.distributionId = resOrder?.data?.distributionId;
        this.orderDetail.firstPaymentBankDto = resOrder?.data?.firstPaymentBankDto;
        this.paymentAccountTradingProvider = this.getBankInfo(this.orderDetail?.firstPaymentBankDto);
      }
      //
      if (this.handleResponseInterceptor(resOrderPayment, '')) {
        this.page.totalItems = resOrderPayment.data.totalItems;
        this.rows = resOrderPayment.data.items.filter(i => i.tranType == OrderPaymentConst.THU);
        //
        if(this.rows.length) {
          this.sumValuePaymentSuccess();
          this.genListAction(this.rows);
        }
        console.log({ rowsPayment: resOrderPayment.data.items, totalItems: resOrderPayment.data.totalItems });
      }
    }, (err) => {
      this.isLoading = false;
    });
  }

  getBanks() {
    this.isLoadingModal = true;
    forkJoin([this._distributionService.getBankList({ distributionId: this.orderDetail.distributionId, type: OrderPaymentConst.THU })]).subscribe(([resBank]) => {
      this.isLoadingModal = false;
      if(this.handleResponseInterceptor(resBank)) {
        this.listBanks = resBank.data.map(bank => {
          bank.labelName = this.getBankInfo(bank);
          return bank;
        });
      }
      console.log("this.listBanks",this.listBanks);
    }, (err) => {
      this.isLoadingModal = false;
    });
  }

  getBankInfo(bank): string {
    if(bank) {
      return [bank?.bankAccNo, bank.bankAccName, bank.bankName].join('-');
    }
    return '';
  }

  differenceMoney: number = 0;
  sumValuePaymentSuccess(): number {
    let paid = this.rows.reduce((total, row) => total + (row.status == this.OrderPaymentConst.PAYMENT_SUCCESS && row?.tranType == OrderPaymentConst.THU ? row.paymentAmount : 0), 0)
    this.differenceMoney = this.orderDetail.initTotalValue - paid;
    return paid;
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
    this.getBanks();
    this.isDetail = false;
    this.orderPayment = {};
    this.orderPayment.codeId = new Date().getTime();
    this.orderPayment.orderId = +this.orderId;
    this.orderPayment.description = 'TT '+ this.orderDetail?.contractCode;
    this.orderPayment.paymentAmount = this.differenceMoney;
    this.orderPayment.tranDate = new Date();
    
    if(this.orderDetail.firstPaymentBankDto) {
      this.orderPayment.tradingBankAccId = this.orderDetail?.firstPaymentBankDto?.businessCustomerBankId;
    }

    this.submitted = false;
    this.modalDialog = true;
  }


  edit(orderPayment) {
    console.log("this.orderPayment edit",orderPayment);
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
    this._orderPaymentService.delete(orderPayment.id).subscribe(
      (response) => {
        if (this.handleResponseInterceptor(response, 'Xóa thành công')) {
          this.setPage({ page: this.page.pageNumber });
          orderPayment = {};
        }
      }, () => {
        this.messageError('Không xóa được hợp đồng phân phối!');
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
			this._orderPaymentService.delete(orderPayment.id).subscribe((response) => {
			  if (this.handleResponseInterceptor(response,"Xóa thanh toán thành công")) {
				  this.setPage();
			  }
			});
			} else { 
        this.messageSuccess('Xóa thành công!');
			}
		});
	}

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
        this._orderPaymentService.approve(orderPayment.id).subscribe((response) => {
          if (this.handleResponseInterceptor(response,"Phê duyệt thanh toán thành công")) {
            this.setPage();
          }
        });
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
        this._orderPaymentService.cancel(orderPayment.id).subscribe((response) => {
          if (this.handleResponseInterceptor(response,"Hủy phê duyệt thanh toán thành công")) {
            this.setPage();
          }
        });
			}
		});
	}

  resentNotify(orderPayment) {
    this.confirmationService.confirm({
        message: 'Bạn có chắc chắn gửi thông báo?',
        header: 'Thông báo',
        acceptLabel: "Đồng ý",
        rejectLabel: "Hủy",
        icon: 'pi pi-question-circle',
        accept: () => {
            this._orderPaymentService.resentNotify(orderPayment.id).subscribe((res) => {
                this.handleResponseInterceptor(res, 'Gửi thành công');
            }, (err) =>  {
                this.messageError('Gửi thất bại!');
            });
        },
        reject: () => {

        },
      }
    );
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
    if (this.orderPayment.id) {
      this._orderPaymentService.update(body).subscribe((response) => {
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
    const validRequired = this.orderPayment?.paymentType && this.orderPayment?.paymentAmount && this.orderPayment?.tranDate;
    return validRequired;
  }
}
