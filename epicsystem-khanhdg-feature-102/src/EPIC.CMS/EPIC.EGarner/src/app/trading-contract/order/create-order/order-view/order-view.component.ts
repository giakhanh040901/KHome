import { CrudComponentBase } from '@shared/crud-component-base';
import { Component, Injector, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { OrderStepService } from '@shared/service-proxies/order-step-service';
import { MessageService } from 'primeng/api';
import { OrderConst, PolicyDetailTemplateConst, PolicyTempConst, ProductConst, ProductPolicyConst, TabView } from '@shared/AppConsts';
import { OrderServiceProxy } from '@shared/service-proxies/trading-contract-service';
import * as moment from 'moment';
import { DistributionService } from '@shared/services/distribution.service';
import { FilterSaleComponent } from '../order-filter-customer/filter-sale/filter-sale.component';
import { DialogService } from 'primeng/dynamicdialog';
import { OBJECT_ORDER } from '@shared/base-object';

@Component({
    selector: 'app-order-view',
    templateUrl: './order-view.component.html',
    styleUrls: ['./order-view.component.scss']
})
export class OrderViewComponent extends CrudComponentBase {

    constructor(
        injector: Injector,
        messageService: MessageService,
        public orderStepService: OrderStepService,
        private dialogService: DialogService,
        public _orderService: OrderServiceProxy,
        private _distributionService: DistributionService,
        private router: Router
    ) {
        super(injector, messageService);
    }

    //
    order = {...OBJECT_ORDER.CREATE};

    orderInfo: any = {};

    distributions = [];
    policies = [];
    policyDetails = [];
    //
    distributionInfo: any = {};
    policyInfo: any = {};
    policyDetailInfo: any = {}
    //
    profitPolicyDetail: number;

    //
    projectInformation: any = {};
    totalInvestment = 0;

    buyDate = new Date();

    // CONST
    ProductPolicyConst = ProductPolicyConst;
    PolicyTempConst = PolicyTempConst;
    OrderConst = OrderConst;
    ProductConst = ProductConst;
    TabView = TabView;

    ngOnInit() {
        if (!this.orderStepService?.orderInfo?.cifCode) {
            this.router.navigate(['trading-contract/order/create/filter-customer']);
        }
        //
        this.orderInfo = {...this.orderStepService.orderInfo};
        console.log('orderInfo', this.orderInfo);
        
        //
        this.isLoadingPage = true;
        this._orderService.getProduct().subscribe((res) => {
            this.isLoadingPage = false;
            if (this.handleResponseInterceptor(res, '')) {
                this.distributions = res.data.map(distribution => {
                    distribution.labelName = ProductConst.getTypeName(distribution?.garnerProduct?.productType) + ' - ' + distribution?.garnerProduct?.name;
                    return distribution;
                });
                this.setData();
            } else {
              this.messageError('Không lấy được danh sách sản phẩm. Vui lòng thử lại sau!');
            }
        }, (err) => {
            console.log('Error-------', err);
            this.isLoadingPage = false;
        });

    }

    setData() {
        if(this.orderInfo.distributionId) {
          this.changeDistribution(this.orderInfo?.distributionId, false);
          this.changePolicy(this.orderInfo?.policyId, false);
        //   this.changePolicyDetail(this.orderInfo?.policyDetailId);
        }
    }

    changeDistribution(distributionId, onChange = true) {
        this.distributionInfo = {...this.distributions.find( item => item.id == distributionId)};
        this.orderInfo.productId = this.distributionInfo?.garnerProduct?.id;
        //Reset data
        if(onChange) {
            this.policyInfo = {};
            this.policyDetailInfo = {};
            this.orderInfo.policyDetailId = null;
            this.orderInfo.policyId = null;
            this.profitPolicyDetail = null;
        }
        //
        if(this.distributionInfo) {
          this.policies = this.distributionInfo?.policies;
        }
    }
  
    getInterestment() {
        let product = this.distributionInfo?.garnerProduct;
        let totalInterestment = 0;
        let remainAmount = 0;
        if(product?.productType == ProductConst.INVEST) {
            totalInterestment = product?.invTotalInvestment;
            remainAmount = totalInterestment - +this.distributionInfo?.isInvested;
        } 
        //
        else if(product?.productType == ProductConst.SHARE) {
            totalInterestment = product?.cpsParValue * product?.cpsQuantity;
            remainAmount = totalInterestment - +this.distributionInfo?.isInvested;
        }
        //
        this.distributionInfo.remainAmount = remainAmount;
        this.totalInvestment = totalInterestment;
        return this.formatCurrency(totalInterestment);
    }
    //
    changePolicy(policyId, onChange = true) {
        this.policyInfo = {...this.policies.find(p => p.id == policyId)};
        //Reset data (Comment tạm)
        // if(onChange) { 
        //     this.policyDetailInfo = {};
        //     this.orderInfo.policyDetailId = null;
        //     this.profitPolicyDetail = null;
        // }
        // //        
        // this.policyDetails = [];
        // this.policyDetails = [...this.policyInfo?.policyDetails];
        // // CUSTOM HIỂN THỊ
        // this.policyDetails = this.policyDetails.map(policyDetails => {
        //     policyDetails.periodQuantityPeriodType = policyDetails.periodQuantity + ' ' + PolicyDetailTemplateConst.getNameInterestPeriodType(policyDetails.periodType);
        //     return policyDetails;
        // });
        // console.log("policyDetails",this.policyDetails);
        // console.log("policy",this.policyInfo);
    }
  
    changePolicyDetail(policyDetailId) {
        this.profitPolicyDetail = this.policyDetails.find(item => item.id == policyDetailId)?.profit;
    }

    searchSale() {
        const ref = this.dialogService.open(FilterSaleComponent,
            {
                header: 'Tìm kiếm sale',
                width: '1000px',
                styleClass: 'p-dialog-custom filter-business-customer customModal',
                contentStyle: { "max-height": "600px", "overflow": "auto" },
                style: { 'top': '-15%', 'overflow': 'hidden' },
                data: {},
            });

        ref.onClose.subscribe((sale) => {
            if (sale) {
                this.orderInfo.saleInfo = {...sale};
                this.orderInfo.saleReferralCode = this.orderInfo?.saleInfo?.referralCode;
            }
        });
    }
    
    complete() {
        for (const [key, value] of Object.entries(this.order)) {
            this.order[key] = this.orderInfo[key];
        }
        //
        if(this.orderInfo.isInvestor) this.order.investorBankAccId = this.orderInfo.bankAccId;
        if(!this.orderInfo.isInvestor) this.order.businessCustomerBankAccId = this.orderInfo.bankAccId;
        let body = {...this.order};
        console.log('orderBody___', body);
        this._orderService.create(body).subscribe((res) => {
            if (this.handleResponseInterceptor(res, 'Thêm thành công')) {
                this.detail(res.data.id);
                console.log("order ...", res);
            } 
        }, () => {
            this.router.navigate(['trading-contract/order/create/filter-customer']);
        });
        this.orderStepService.complete();
    }

    detail(orderId) {
        this.router.navigate(['/trading-contract/order/detail/' + this.cryptEncode(orderId)]);
    }

    prevPage() {
        this.router.navigate(['trading-contract/order/create/filter-product']);
    }

}
