import { Component, Injector, Input, ViewChild } from '@angular/core';
import { ProductPolicyConst, OrderConst } from '@shared/AppConsts';
import { CrudComponentBase } from '@shared/crud-component-base';
import { MessageService } from 'primeng/api';
import { BreadcrumbService } from 'src/app/layout/breadcrumb/breadcrumb.service';
import { ActivatedRoute } from '@angular/router';
import { OrderServiceProxy } from '@shared/service-proxies/trading-contract-service';
import { DistributionService } from '@shared/services/distribution.service';
import { DialogService, DynamicDialogRef } from 'primeng/dynamicdialog';
import { SaleService } from '@shared/services/sale.service';
import {DynamicDialogConfig} from 'primeng/dynamicdialog';

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
    private _distributionService: DistributionService,
    private dialogConfig: DynamicDialogConfig,
    private ref : DynamicDialogRef,
  ) {
      super(injector, messageService);
  }


  bondSecondary: any = {};
  policies: any[] = [];
  policyInfo: any = {};
  policyDetails: any[] = [];
  policyDetail: any = {};
  profit:number = null;

  ProductPolicyConst = ProductPolicyConst;
  OrderConst = OrderConst;

  orderId: number;
  orderDetail: any = {};

  body = {
    settlementMethod: 1,
    renewalsPolicyDetailId: 0,
    orderId: 0,
    requestNote: '',
    summary: '',
  }

  ngOnInit() {
    this.orderId = this.dialogConfig.data?.orderId;
    this.body.orderId = this.orderId;
    this.init();
  }

  init() {
    this.isLoadingPage = true;
    this._orderService.get(this.orderId).subscribe((resOrder) => {
        if (this.handleResponseInterceptor(resOrder, '')) {
            this.orderDetail = { ...resOrder.data };
            console.log("orderDetail", this.orderDetail);
            this.initDistributionInfo(this.orderDetail?.distributionId);
        } else {
            this.isLoadingPage = false;
        }
    });
  }

  initDistributionInfo(distributionId) {
    this._distributionService.getById(distributionId).subscribe((res) => {
        this.isLoadingPage = false;
        if (this.handleResponseInterceptor(res, '')) {
            this.policies = res?.data.policies;
            this.policyInfo = this.policies.find(item => item.id == this.orderDetail.policyId);
            this.policyDetails = this.policyInfo.policyDetail
            console.log("policyDetails",this.policyDetails);
            
            this.policyDetail = this.policyDetails.find(item => item.id == this.orderDetail.policyDetailId);
            this.profit = this.policyDetail.profit;
        }
    }, (err) => {
        this.isLoadingPage = false;
        console.log('Error-------', err);
    });
  }

  changePolicy(policy) {
    this.orderDetail.policyId = policy.id;
    this.policyInfo = this.policies.find(item => item.id == policy.id);

    console.log('policy___', this.policyInfo);
    
    this.policyDetails = policy.policyDetail;
    this.profit = null;
  }

  changePolicyDetail(policyDetailId) {
    this.orderDetail.policyDetailId = policyDetailId;
    this.policyDetail = this.policyDetails.find(item => item.id == policyDetailId);
    this.profit = this.policyDetail.profit;
  }
  
  cancel() {
    this.ref.close();
  }

  save() {
    this.body.summary = this.orderDetail.contractCode 
                        + ' - ' + (this.orderDetail.investor?.investorIdentification?.fullname || this.orderDetail?.businessCustomer?.name)
                        + '- ' + (this.orderDetail?.investor?.phone || this.orderDetail?.businessCustomer?.name);
    this._orderService.reinstatementRequest(this.body).subscribe(res => {
      if(this.handleResponseInterceptor(res, '')) {
        this.ref.close(res);
      }
    }, (err) => {
      this.messageError('Yêu cầu thất bại vui lòng thử lại sau!');
    });
  }

}
