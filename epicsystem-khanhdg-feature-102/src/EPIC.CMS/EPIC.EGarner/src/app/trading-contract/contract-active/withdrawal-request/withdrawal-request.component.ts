
import { Component, Injector } from '@angular/core';
import { CrudComponentBase } from '@shared/crud-component-base';
import { MessageService } from 'primeng/api';
import { OrderServiceProxy } from '@shared/service-proxies/trading-contract-service';
import { DynamicDialogRef } from 'primeng/dynamicdialog';
import { DynamicDialogConfig } from 'primeng/dynamicdialog';
import { OrderConst } from '@shared/AppConsts';
import { WithdrawalService } from '@shared/services/withdrawal-service';

@Component({
  selector: 'app-withdrawal-request',
  templateUrl: './withdrawal-request.component.html',
  styleUrls: ['./withdrawal-request.component.scss']
})
export class WithdrawalRequestComponent extends CrudComponentBase {

  constructor(
    injector: Injector,
    messageService: MessageService,
    private _orderService: OrderServiceProxy,
    private dialogConfig: DynamicDialogConfig,
    private ref : DynamicDialogRef,
    private _withDrawal: WithdrawalService,
  ) {
      super(injector, messageService);
      this.userLogin = this.getUser();
  }

  userLogin: any;

  OrderConst = OrderConst;

  orderId: number;
  orderDetail: any = {};

  drawdownRequest = {
    policyId: 0,
    withdrawDate: new Date(),
    amount: null,
    cifCode: '',
    bankAccountId: null
  }

  policies = [];
  banks = []

  today = new Date();

  fieldDate = ['withdrawDate'];

  ngOnInit() {
    this.init();    
    this.orderDetail = this.dialogConfig.data?.orderDetail;
    this.drawdownRequest.cifCode = this.orderDetail.cifCode;
    this.drawdownRequest.policyId = this.orderDetail.policyId;
  }

  init() {
		this.isLoading = true;
		this._orderService.getAllBankByCifcode(this.dialogConfig.data?.orderDetail.cifCode).subscribe((res) => {
		  this.isLoading = false;
		  if (this.handleResponseInterceptor(res, '')) {
        if(res?.data?.length){
          this.banks = res.data.map( (bank) => {
            bank.id = bank?.id;
            bank.label = bank.bankName + ' - ' + bank.bankAccount + ' - ' + bank.ownerAccount;

            return bank;
          });
        }

		  }
		}, (err) => {
			this.isLoading = false;
			console.log('Error-------', err);
			
		});
	}

  cancel() {
    this.ref.close();
  }

  save() {
    if(this.checkValid()) {
      let body = this.formatCalendar(this.fieldDate, {...this.drawdownRequest});
      this._withDrawal.request(body).subscribe(res => {
        if(this.handleResponseInterceptor(res)) {
          this.ref.close(res);
        }
      }, (err) => {
        console.log('err___', err);
        this.messageError('Có lỗi xảy ra vui lòng thử lại sau');
      });
    } else {
      this.messageError('Vui lòng nhập đủ thông tin cho các trường có dấu (*)');
    }
  }

  checkValid() {
    return this.drawdownRequest.withdrawDate && this.drawdownRequest.amount && this.drawdownRequest.policyId && this.drawdownRequest.cifCode && this.drawdownRequest.bankAccountId;
  }

}
