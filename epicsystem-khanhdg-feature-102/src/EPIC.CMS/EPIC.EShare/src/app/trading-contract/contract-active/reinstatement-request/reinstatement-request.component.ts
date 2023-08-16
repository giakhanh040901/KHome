import { Component, Injector } from '@angular/core';
import { ProductPolicyConst, OrderConst } from '@shared/AppConsts';
import { CrudComponentBase } from '@shared/crud-component-base';
import { CompanyShareSecondaryServiceProxy } from '@shared/service-proxies/company-share-manager-service';
import { MessageService } from 'primeng/api';
import { BreadcrumbService } from 'src/app/layout/breadcrumb/breadcrumb.service';
import { ActivatedRoute } from '@angular/router';
import { OrderServiceProxy } from '@shared/service-proxies/trading-contract-service';
import { DialogService, DynamicDialogRef } from 'primeng/dynamicdialog';
import { SaleService } from '@shared/services/sale.service';
import { DynamicDialogConfig } from 'primeng/dynamicdialog';

@Component({
  selector: 'app-reinstatement-request',
  templateUrl: './reinstatement-request.component.html',
  styleUrls: ['./reinstatement-request.component.scss']
})
export class ReinstatementRequestComponent extends CrudComponentBase {

  constructor(
    injector: Injector,
    messageService: MessageService,
    private _orderService: OrderServiceProxy,
    private dialogConfig: DynamicDialogConfig,
    private ref: DynamicDialogRef,
    
  ) {
    super(injector, messageService);
  }

  companyShareSecondary: any = {};
  policies: any[] = [];
  policyInfo: any = {};
  policyDetails: any[] = [];
  policyDetail: any = {}
  profit: number = null;

  OrderConst = OrderConst;

  orderId: number;
  orderDetail: any = {};

  body = {
    "settlementMethod": 1,
    "renewarsPolicyDetailId": 0,
    "orderId": 0,
    "summary" : null,
    "requestNote": null,
  }

  ngOnInit() {
    this.orderId = this.dialogConfig.data?.orderId;
    this.body.orderId = this.orderId;
    this.init();
  }

  init() {
    this.isLoadingPage = true;
    this._orderService.get(this.orderId).subscribe((resOrder) => {
        this.isLoadingPage = false;
        if (this.handleResponseInterceptor(resOrder, '')) {
            this._orderService.getPriceDate(resOrder.data.companyShareSecondaryId, resOrder.data.paymentFullDate ?? new Date()).subscribe((resPrice) => {
                if (this.handleResponseInterceptor(resPrice, '')) {
                    this.orderDetail = {...resOrder.data}
                    this.companyShareSecondary = resOrder?.data?.companyShareSecondary;
                    //
                    this.policies = this.companyShareSecondary?.policies ?? [];
                    this.policyInfo = this.policies.find(item => item.companySharePolicyId == this.orderDetail.companySharePolicyId);

                    this.policyDetails = this.policyInfo?.details ?? [];
                    this.policyDetail = this.policyDetails.find(item => item.companySharePolicyDetailId == this.orderDetail?.companySharePolicyDetailId);
                    this.profit = this.policyDetail.profit;
                }
                //
            });
        } else {
            this.isLoadingPage = false;
        }
    });
  }

  changePolicy(companySharePolicyId) {
    let policy = this.policies.find(item => item.companySharePolicyId == companySharePolicyId);
    this.orderDetail.dueDate = null;
    this.policyInfo = policy;
    this.policyDetails = policy.details;
    this.profit = null;
  }

  changePolicyDetail(companySharePolicyDetailId) {
    this.policyDetail = this.policyDetails.find(item => item.companySharePolicyDetailId == companySharePolicyDetailId);
    this.profit = this.policyDetail.profit;
    console.log('policyDetail', this.policyDetail);
  }
  
  cancel() {
    this.ref.close();
  }

  save() {
    console.log("this.body", this.body);
    
    this.body.summary = this.orderDetail.contractCode 
                        + ' - ' + (this.orderDetail.investor?.investorIdentification?.fullname || this.orderDetail?.businessCustomer?.name)
                        + '- ' + (this.orderDetail?.investor?.phone || this.orderDetail?.businessCustomer?.name);
    this._orderService.reinstatementRequest(this.body).subscribe(res => {
      if(this.handleResponseInterceptor(res, '')) {
        this.ref.close(res);
      }
    }, (err) => {
      this.messageError('Yêu cầu thất bại vui lòng thử lại sau!', '');
    });
  }

}
