import { Component, Injector, Input, OnInit } from "@angular/core";
import { ActivatedRoute } from "@angular/router";
import { OrderConst, OrderPaymentConst, YesNoConst } from "@shared/AppConsts";
import { CrudComponentBase } from "@shared/crud-component-base";
import { Page } from "@shared/model/page";
import { OrderPaymentServiceProxy, OrderServiceProxy } from "@shared/service-proxies/trading-contract-service";
import { DistributionService } from "@shared/services/distribution.service";
import { ConfirmationService, MessageService } from "primeng/api";
import { DialogService, DynamicDialogRef } from "primeng/dynamicdialog";
import { forkJoin } from "rxjs";

@Component({
  selector: "app-order-justview-payment",
  templateUrl: "./order-justview-payment.component.html",
  styleUrls: ["./order-justview-payment.component.scss"],
})
export class OrderJustviewPaymentComponent extends CrudComponentBase {
  constructor(
	injector: Injector,
    messageService: MessageService,
    private routeActive: ActivatedRoute,
    private _orderPaymentService: OrderPaymentServiceProxy,
    private _orderService: OrderServiceProxy,
	) {
    	super(injector, messageService);
		//
		this.orderId = this.cryptDecode(
		this.routeActive.snapshot.paramMap.get("id")
		);
  	}

	orderId: string;
	@Input() orderDetail: any = {};

	ref: DynamicDialogRef;

	modalDialog: boolean;
	deleteItemDialog: boolean = false;
  
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

  	ngOnInit(): void {
		this.paymentAccountTradingProvider = this.getBankInfo(this.orderDetail?.firstPaymentBankDto);
		this.setPage();
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

	getBankInfo(bank): string {
		if(bank) {
		  return [bank?.bankName, bank?.bankAccNo, bank?.bankAccName].join('-');
		}
		return '';
	}

	differenceMoney: number = 0;
	sumValuePaymentSuccess(): number {
		let paid = this.rows.reduce((total, row) => total + (row.status == this.OrderPaymentConst.PAYMENT_SUCCESS && row?.tranType == OrderPaymentConst.THU ? row.paymentAmount : 0), 0)
		this.differenceMoney = this.orderDetail.initTotalValue - paid;
		return paid;
	}

	hideDialog() {
		this.modalDialog = false;
		this.submitted = false;
	}

	header(): string {
		return !this.orderPayment?.status ? 'Thêm thanh toán' : this.orderPayment?.status == OrderPaymentConst.PAYMENT_TEMP ? 'Sửa thanh toán' : 'Chi tiết thanh toán';
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
			this.rows = resOrderPayment.data.items;
	
			if(this.rows.length) {
			  this.sumValuePaymentSuccess();
			}
			console.log({ rowsPayment: resOrderPayment.data.items, totalItems: resOrderPayment.data.totalItems });
		  }
		}, (err) => {
		  this.isLoading = false;
		});
	  }
}
