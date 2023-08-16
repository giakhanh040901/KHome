import { Component, ElementRef, Injector, Input, OnInit, Output, ViewChild } from '@angular/core';
import { OrderConst, OrderPaymentConst, TableConst, YesNoConst } from '@shared/AppConsts';
import { CrudComponentBase } from '@shared/crud-component-base';
import { Page } from '@shared/model/page';
import { MessageService } from 'primeng/api';
import { ActivatedRoute } from '@angular/router';
import { DynamicDialogRef } from 'primeng/dynamicdialog';
import { forkJoin } from 'rxjs';
import { DistributionService } from '@shared/services/distribution.service';
import { OrderService } from '@shared/services/order.service';
import { OrderPaymentService } from '@shared/services/order-payment.service';
import { IColumn } from '@shared/interface/p-table.model';

@Component({
  selector: 'app-order-payment-view',
  templateUrl: './order-payment-view.component.html',
  styleUrls: ['./order-payment-view.component.scss']
})
export class OrderPaymentViewComponent extends CrudComponentBase {

    constructor(
        injector: Injector,
        messageService: MessageService,
        private routeActive: ActivatedRoute,
        private _orderPaymentService: OrderPaymentService,
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
    @Input() contentHeight: number = 0;
    idHeader: string ="page-header-payment-view";

    modalDialog: boolean;
    deleteItemDialog: boolean = false;

    confirmRequestDialog: boolean = false;

    rows: any[] = [];

    OrderConst = OrderConst;
    OrderPaymentConst = OrderPaymentConst;
    YesNoConst = YesNoConst;
    //

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
    };

    page = new Page();

    paymentAccountTradingProvider: string = "";

    listAction: any[] = [];
    listBanks: any[] = [];
    isLoadingModal: boolean = false;

    columns: IColumn[] = [];

    ngOnInit() {
        this.columns = [
            { field: 'id', header: 'Mã giao dịch', width: 8.5, displaySettingColumn: false, isPin: true, isFrozen: true, alignFrozen: TableConst.alignFrozenColumn.LEFT, class: 'b-border-frozen-left' },
            { field: 'tranTypeDisplay', header: 'Thu/Chi', width: 5, isPin: true},
            { field: 'paymentTypeDisplay', header: 'Loại', width: 10, isPin: true },
            { field: 'tranDateDisplay', header: 'Ngày giao dịch', width: 12, class: 'justify-content-end text-right'},
            { field: 'paymentAmnountDisplay', header: 'Số tiền', width: 12, class: 'justify-content-end text-right' },
            { field: 'description', header: 'Mô tả', width: 16, isResize: true, },
            { field: 'status', header: 'Trạng thái', width: 8, isFrozen: true, alignFrozen: TableConst.alignFrozenColumn.RIGHT, type: TableConst.columnTypes.STATUS, class: 'justify-content-left b-border-frozen-right' },
            { field: '', header: '', width: 3, displaySettingColumn: false, isFrozen: true, alignFrozen: TableConst.alignFrozenColumn.RIGHT, type: TableConst.columnTypes.ACTION_DROPDOWN },
        ];
        this.setPage();
    }

    genListAction(data = []) {
        this.listAction = data.map((orderItem) => {
            const actions = [];
            actions.push({
                data: orderItem,
                label: "Chi tiết thanh toán",
                icon: "pi pi-info-circle",
                command: ($event) => {
                this.detail($event.item.data);
                },
            });
            return actions;
        });
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
                    this.sumValuePaymentSuccess();
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
            row.tranDateDisplay = this.formatDateTime(row.tranDate);
            row.paymentAmnountDisplay = this.utils.transformMoney(row.paymentAmnount);
            row.statusElement = OrderPaymentConst.getPaymentStatusInfo(row.status);
            return row;
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
        let paid = this.rows.reduce((total, row) => total + (row?.status == this.OrderPaymentConst.PAYMENT_SUCCESS && row?.tranType == OrderPaymentConst.THU ? row.paymentAmnount : 0), 0);
        this.differenceMoney = this.orderDetail.initTotalValue - paid;
        return paid;
    }
    
    detail(orderPayment) {
        this.orderPayment = {};
        this.orderPayment = {
            ...orderPayment,
            tranDate: orderPayment.tranDate ? new Date(orderPayment.tranDate) : null,
        };
        this.modalDialog = true;
    }
}
