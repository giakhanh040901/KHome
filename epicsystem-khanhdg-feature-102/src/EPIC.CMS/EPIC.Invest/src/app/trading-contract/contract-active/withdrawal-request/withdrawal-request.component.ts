
import { Component, Injector } from '@angular/core';
import { CrudComponentBase } from '@shared/crud-component-base';
import { MessageService } from 'primeng/api';
import { DynamicDialogRef } from 'primeng/dynamicdialog';
import { DynamicDialogConfig } from 'primeng/dynamicdialog';
import { OrderConst } from '@shared/AppConsts';
import { OrderService } from '@shared/services/order.service';

@Component({
  selector: 'app-withdrawal-request',
  templateUrl: './withdrawal-request.component.html',
  styleUrls: ['./withdrawal-request.component.scss']
})
export class WithdrawalRequestComponent extends CrudComponentBase {

    constructor(
        injector: Injector,
        messageService: MessageService,
        private _orderService: OrderService,
        private dialogConfig: DynamicDialogConfig,
        private ref : DynamicDialogRef,
    ) {
        super(injector, messageService);
    }

    OrderConst = OrderConst;

    orderId: number;
    orderDetail: any = {};

    drawdownRequest = {
        orderId: 0,
        withdrawalDate: new Date(),
        amountMoney: null,
        withdrawalValue: 1,
    }

    fieldDates = ['withdrawalDate'];

    ngOnInit() {
        this.orderId = this.dialogConfig.data?.orderId;
        this.drawdownRequest.orderId = this.orderId;
        this.init();
    }

    init() {
        this.isLoading = true;
        this._orderService.get(this.orderId).subscribe((resOrder) => {
            this.isLoading = false;
            if (this.handleResponseInterceptor(resOrder, '')) {
                this.orderDetail = { ...resOrder.data };
            } 
        });
    }

    changeValue(value) {
        if(value > this.orderDetail.totalValue) {
            this.drawdownRequest.amountMoney = this.orderDetail.totalValue;
        }
        //
        if(value < this.orderDetail.totalValue) {
            this.drawdownRequest.withdrawalValue = OrderConst.WITHDRAWAL_PARTIAL
        } else {
            this.drawdownRequest.withdrawalValue = OrderConst.WITHDRAWAL_ALL;
        }
    }

    changeWithdrawalValue(typeValue) {
        if(typeValue == OrderConst.WITHDRAWAL_PARTIAL) { 
            this.drawdownRequest.amountMoney = null;
        } else {
            this.drawdownRequest.amountMoney = this.orderDetail.totalValue;
        }
    }

    cancel() {
        this.ref.close();
    }

    save() {
        let body = this.formatCalendar(this.fieldDates, {...this.drawdownRequest});
        this._orderService.withdrawalRequest(body).subscribe(res => {
            if(this.handleResponseInterceptor(res)) {
                this.ref.close(res);
            }
        }, (err) => {
            console.log('err___', err);
            this.messageError('Có lỗi xảy ra vui lòng thử lại sau', '');
        });
    }
}
