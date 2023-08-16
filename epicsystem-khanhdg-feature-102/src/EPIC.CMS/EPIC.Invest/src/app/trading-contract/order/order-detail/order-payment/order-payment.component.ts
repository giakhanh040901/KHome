import { Component, ElementRef, Injector, Input, ViewChild} from "@angular/core";
import {
    AtributionConfirmConst,
  FormNotificationConst,
  OrderConst,
  OrderPaymentConst,
  TableConst,
  YesNoConst,
} from "@shared/AppConsts";
import { CrudComponentBase } from "@shared/crud-component-base";
import { Page } from "@shared/model/page";
import { ConfirmationService, MessageService } from "primeng/api";
import { ActivatedRoute } from "@angular/router";
import { DialogService } from "primeng/dynamicdialog";
import { DynamicDialogRef } from "primeng/dynamicdialog";
import { FormNotificationComponent } from "src/app/form-general/form-notification/form-notification.component";
import { forkJoin } from "rxjs";
import { DistributionService } from "@shared/services/distribution.service";
import { FunctionForDevComponent } from "./function-for-dev/function-for-dev.component";
import { environment } from "src/environments/environment";
import { OrderService } from "@shared/services/order.service";
import { OrderPaymentService } from "@shared/services/order-payment.service";
import { IColumn } from "@shared/interface/p-table.model";

@Component({
    selector: "app-order-payment",
    templateUrl: "./order-payment.component.html",
    styleUrls: ["./order-payment.component.scss"],
    providers: [ConfirmationService, MessageService],
})
export class OrderPaymentComponent extends CrudComponentBase {
    
    constructor(
        injector: Injector,
        messageService: MessageService,
        private confirmationService: ConfirmationService,
        private routeActive: ActivatedRoute,
        private _orderPaymentService: OrderPaymentService,
        private _dialogService: DialogService,
        private _orderService: OrderService,
        private _distributionService: DistributionService
    ) {
        super(injector, messageService);
        this.orderId = this.cryptDecode(
            this.routeActive.snapshot.paramMap.get("id")
        );
    }

    orderId: string;
    @Input() orderDetail: any = {};
    @Input() isPartner: boolean;
    @Input() contentHeight: number;
    idHeader:string = "page-header-payment";

    ref: DynamicDialogRef;

    modalDialog: boolean;
    deleteItemDialog: boolean = false;

    confirmRequestDialog: boolean = false;

    rows: any[] = [];

    OrderConst = OrderConst;
    OrderPaymentConst = OrderPaymentConst;
    YesNoConst = YesNoConst;
    //
    environment = environment;

    orderPayment: any = {
    id: 0, // id
    codeId: 0,
    tranDate: null, //tranDate
    tranType: 1, // Kiểu giao dịch
    tranClassify: 1, // Loại giao dịch
    paymentType: null, // Loại thanh toán
    orderId: 0, // id orderId
    paymentAmnount: null, // Số tiền paymentAmnount
    description: null, // Mô tả
    status: null,
    // "tradingBankAcc" : 0,
    };

    itemTradingProviderInfo = {};
    fieldErrors = {};
    fieldDates = ["tranDate"];

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
    paymentAccountTradingProvider: string = "";

    isDetail = false;
    actionsDisplay: any[] = [];
    actions: any[] = [];
    listAction: any[] = [];
    listBanks: any[] = [];
    isLoadingModal: boolean = false;

    disableButtonPayment = false;

    columns: IColumn[] = [];

    ngOnInit() {
        
        this.columns = [
			{ field: 'id', header: 'Mã giao dịch', width: 8.5, displaySettingColumn: false, isPin: true, isFrozen: true, alignFrozen: TableConst.alignFrozenColumn.LEFT, class: 'b-border-frozen-left' },
			{ field: 'tranType', header: 'Thu/Chi', width: 5, isPin: true, type: TableConst.columnTypes.CONVERT_DISPLAY},
			{ field: 'paymentType', header: 'Loại', width: 10, isPin: true, type: TableConst.columnTypes.CONVERT_DISPLAY },
			{ field: 'tranDate', header: 'Ngày giao dịch', width: 12, type: TableConst.columnTypes.DATETIME },
			{ field: 'paymentAmnount', header: 'Số tiền', width: 12, type: TableConst.columnTypes.CURRENCY },
			{ field: 'description', header: 'Mô tả', width: 16, isResize: true, },
			{ field: 'status', header: 'Trạng thái', width: 8.5, isFrozen: true, alignFrozen: TableConst.alignFrozenColumn.RIGHT, type: TableConst.columnTypes.STATUS, class: 'justify-content-left b-border-frozen-right' },
			{ field: '', header: '', width: 3, displaySettingColumn: false, isFrozen: true, alignFrozen: TableConst.alignFrozenColumn.RIGHT, type: TableConst.columnTypes.ACTION_DROPDOWN },
		];
        //
        this.setPage();
    }
    
    setPage(event?: Page) {
        if(!event) this.page.pageNumber = 0;
        this.isLoading = true;
        forkJoin([
            this._orderPaymentService.getAll(this.page, this.orderId),
            this._orderService.get(+this.orderId),
        ]).subscribe(([resOrderPayment, resOrder]) => {
            this.isLoading = false;
            if (this.handleResponseInterceptor(resOrderPayment, "")) {
                this.page.totalItems = resOrderPayment.data.totalItems;
                this.rows = resOrderPayment?.data?.items?.filter((row) => row.tranType == OrderPaymentConst.THU);
                if (this.rows.length) {
                    this.genListAction(this.rows);
                    this.setData(this.rows);
                }
            }
            //
            if (this.handleResponseInterceptor(resOrder)) {
                this.orderDetail.status = resOrder?.data?.status;
                this.orderDetail.distributionId = resOrder?.data?.distributionId;
                this.orderDetail.tradingProviderBank = resOrder?.data?.tradingProviderBank;
                this.paymentAccountTradingProvider = this.orderDetail?.tradingProviderBank?.bankName 
                                                    + " - " + this.orderDetail?.tradingProviderBank?.bankAccNo 
                                                    + " - " + this.orderDetail?.tradingProviderBank?.bankAccName;
            }
        }, (err) => {
            this.isLoading = false;
        });
    }

    setData(rows) {
        this.rows = rows.map(row => {
            row.tranTypeDisplay = OrderPaymentConst.getNameTransactionType(row.tranType);
            row.paymentTypeDisplay = OrderPaymentConst.getNamePaymentType(row.paymentType);
            //
            row.statusElement = OrderPaymentConst.getPaymentStatusInfo(row.status);
            return row;
        });
        console.log('rows', this.rows);
        
    }

    genListAction(data = []) {
        this.listAction = data.map((orderItem) => {
            const actions = [];
            if ((orderItem.status == this.OrderPaymentConst.PAYMENT_CLOSE || orderItem.status == this.OrderPaymentConst.PAYMENT_SUCCESS) 
                && this.isGranted([this.PermissionInvestConst.InvestHDPP_SoLenh_TTCT_TTThanhToan_ChiTietThanhToan])
            ) {
                actions.push({
                    data: orderItem,
                    label: "Chi tiết thanh toán",
                    icon: "pi pi-info-circle",
                    command: ($event) => {
                    this.detail($event.item.data);
                    },
                });
            }

            if (!this.isPartner && orderItem.status == this.OrderPaymentConst.PAYMENT_TEMP &&
                this.isGranted([this.PermissionInvestConst.InvestHDPP_SoLenh_TTCT_TTThanhToan_CapNhat])
            ) {
                actions.push({
                    data: orderItem,
                    label: "Sửa",
                    icon: "pi pi-pencil",
                    command: ($event) => {
                    this.edit($event.item.data);
                    },
                });
            }

            if (!this.isPartner && orderItem.status == this.OrderPaymentConst.PAYMENT_TEMP &&
                this.isGranted([this.PermissionInvestConst.InvestHDPP_SoLenh_TTCT_TTThanhToan_PheDuyet])
            ) {
                actions.push({
                    data: orderItem,
                    label: "Phê duyệt",
                    icon: "pi pi-check",
                    statusActive: [this.OrderPaymentConst.PAYMENT_TEMP],
                    command: ($event) => {
                    this.approve($event.item.data);
                    },
                });
            }

            if (!this.isPartner && orderItem.status == this.OrderPaymentConst.PAYMENT_SUCCESS 
                && this.isGranted([this.PermissionInvestConst.InvestHDPP_SoLenh_TTCT_TTThanhToan_HuyPheDuyet]) 
                && !OrderPaymentConst.BLOCK_ACTION_THANH_TOAN.includes(orderItem.tranClassify)
            ) {
                actions.push({
                    data: orderItem,
                    label: "Hủy phê duyệt",
                    icon: "pi pi-times",
                    command: ($event) => {
                    this.cancel($event.item.data);
                    },
                });
            }

            if (!this.isPartner && orderItem.status == this.OrderPaymentConst.PAYMENT_SUCCESS 
                && this.isGranted([this.PermissionInvestConst.InvestHDPP_SoLenh_TTCT_TTThanhToan_GuiThongBao])
            ) {
                actions.push({
                    data: orderItem,
                    label: "Gửi thông báo",
                    icon: "pi pi-send",
                    command: ($event) => {
                    this.resentNotify($event.item.data);
                    },
                });
            }

            if (!this.isPartner &&  orderItem.status == this.OrderPaymentConst.PAYMENT_TEMP && this.isGranted([this.PermissionInvestConst.InvestHDPP_SoLenh_TTCT_TTThanhToan_Xoa])) {
                actions.push({
                    data: orderItem,
                    label: "Xóa",
                    icon: "pi pi-trash",
                    command: ($event) => {
                    this.delete($event.item.data);
                    },
                });
            }
            return actions;
        });
    }

    funcForDev() {
        this._dialogService.open(FunctionForDevComponent, {
            header: "Giả lập MSB",
            width: "800px",
            data: {
                orderDetail: this.orderDetail,
            },
        }).onClose.subscribe((res) => {
            this.setPage();
        });
    }

    getBanks() {
        this.isLoadingModal = true;
        let params = { 
            distributionId: this.orderDetail.distributionId, 
            type: OrderPaymentConst.THU
        };
        this._distributionService.getBankList(params).subscribe((resBank) => {
            this.isLoadingModal = false;
            if (this.handleResponseInterceptor(resBank)) {
                this.listBanks = resBank.data.map((bank) => {
                    bank.labelName = [bank?.bankAccNo, bank.bankAccName, bank.bankName].join('-');
                    return bank;
                });
            }
            }, (err) => {
                this.isLoadingModal = false;
            }
        );
    }

    differenceMoney: number = 0;
    sumValuePaymentSuccess(): number {
        if(this.rows?.length) {
            let paid = this.rows.reduce((total, row) => total + (row?.status == this.OrderPaymentConst.PAYMENT_SUCCESS && row?.tranType == OrderPaymentConst.THU ? row.paymentAmnount : 0), 0);
            this.differenceMoney = this.orderDetail.initTotalValue - paid;
            return paid;
        }
        return 0;
    }

    header(): string {
        return !this.orderPayment?.status
            ? "Thêm thanh toán"
            : this.orderPayment?.status == OrderPaymentConst.PAYMENT_TEMP
            ? "Sửa thanh toán"
            : "Chi tiết thanh toán";
    }

    create() {
        this.getBanks();
        this.isDetail = false;
        this.orderPayment = {};
        this.orderPayment.codeId = new Date().getTime();
        this.orderPayment.orderId = +this.orderId;
        this.orderPayment.description = "TT " + this.orderDetail?.genContractCode ?? this.orderDetail?.contractCode;
        this.orderPayment.paymentAmnount = this.differenceMoney;
        this.orderPayment.tranDate = new Date();
        //
        if (this.orderDetail?.tradingProviderBank) {
            this.orderPayment.tradingBankAccId =
            this.orderDetail?.tradingProviderBank?.businessCustomerBankAccId;
        }

        this.submitted = false;
        this.modalDialog = true;
    }

    edit(orderPayment) {
        this.orderPayment = {};
        this.isDetail = false;
        this.orderPayment = {
            ...orderPayment,
            tranDate: orderPayment.tranDate ? new Date(orderPayment.tranDate) : null,
        };
        this.modalDialog = true;
    }

    detail(orderPayment) {
        this.orderPayment = {};
        this.isDetail = true;
        this.orderPayment = {
            ...orderPayment,
            tranDate: orderPayment.tranDate ? new Date(orderPayment.tranDate) : null,
        };
        this.modalDialog = true;
    }

    confirmDelete(orderPayment) {
        this.deleteItemDialog = false;
        this._orderPaymentService.delete(orderPayment.id).subscribe((response) => {
            if (this.handleResponseInterceptor(response, "Xóa thành công")) {
                this.setPage();
                orderPayment = {};
            }
            },(error) => {
                this.messageError('Xóa thất bại. Vui lòng thử lại!')
            }
        );
    }

    delete(orderPayment) {
        this._dialogService.open(FormNotificationComponent, {
            header: "Thông báo",
            width: "600px",
            data: {
                title: "Bạn có chắc chắn xóa thanh toán này?",
                icon: FormNotificationConst.IMAGE_CLOSE,
            },
        }).onClose.subscribe((dataCallBack) => {
            if (dataCallBack?.accept) {
                this._orderPaymentService.delete(orderPayment.id).subscribe((response) => {
                    let messageNotify = "Xóa thanh toán thành công";
                    if (this.handleResponseInterceptor(response, messageNotify)) {
                        this.setPage();
                    }
                });
            } 
        });
    }

    approve(orderPayment) {
        this._dialogService.open(FormNotificationComponent, {
            header: "Thông báo",
            width: "600px",
            data: {
                title: "Bạn có chắc chắn phê duyệt thanh toán này?",
                icon: FormNotificationConst.IMAGE_APPROVE,
            },
        }).onClose.subscribe((dataCallBack) => {
            if (dataCallBack?.accept) {
                this._orderPaymentService.approve(orderPayment.id).subscribe((response) => {
                    let messageNotify = "Phê duyệt thanh toán thành công";
                    if (this.handleResponseInterceptor(response, messageNotify)) {
                        this.setPage();
                    }
                });
            }
        });
    }
    //
    cancel(orderPayment) {
        this._dialogService.open(FormNotificationComponent, {
            header: "Thông báo",
            width: "600px",
            data: {
                title: "Bạn có chắc chắn hủy phê duyệt thanh toán này?",
                icon: FormNotificationConst.IMAGE_CLOSE,
            },
        }).onClose.subscribe((dataCallBack) => {
            if (dataCallBack?.accept) {
                this._orderPaymentService.cancel(orderPayment.id).subscribe((response) => {
                    let messageNotify = "Hủy phê duyệt thanh toán thành công"
                    if (this.handleResponseInterceptor(response, messageNotify)) {
                        this.setPage();
                    }
                });
            }
        });
    }

    resentNotify(orderPayment) {
        this.confirmationService.confirm({
            message: "Bạn có chắc chắn gửi thông báo?",
            ...AtributionConfirmConst,
            accept: () => {
            this._orderPaymentService.resentNotify(orderPayment.id).subscribe((res) => {
                this.handleResponseInterceptor(res, "Gửi thành công");
            },(err) => {
                this.messageError("Gửi thất bại!", "");
            });
            },
        });
    }
    
    hideDialog() {
        this.modalDialog = false;
        this.submitted = false;
    }

    save() {
        this.submitted = true;
        let body = this.formatCalendar(this.fieldDates, { ...this.orderPayment });
        if (this.orderPayment.id) {
            this._orderPaymentService.update(body).subscribe((response) => {
                if (this.handleResponseInterceptor(response, "Cập nhật thành công")) {
                    this.submitted = false;
                    this.setPage();
                    this.hideDialog();
                } else {
                    this.submitted = false;
                }
            }, (error) => {
                this.submitted = false;
            });
        } else {
            this._orderPaymentService.create(body).subscribe((response) => {
                if (this.handleResponseInterceptor(response, "Thêm thành công")) {
                    this.submitted = false;
                    this.setPage();
                    this.hideDialog();
                } else {
                    this.submitted = false;
                }
            },() => {
                this.submitted = false;
            }
            );
        }
    }

    changeBank(bankId) {
        console.log("bankId", bankId);
    }

    validForm(): boolean {
        const validRequired = this.orderPayment?.paymentType 
                            && this.orderPayment?.paymentAmnount 
                            && this.orderPayment?.tranDate 
                            && this.orderPayment?.tradingBankAccId;
        return validRequired;
    }
}
