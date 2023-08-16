import { Component, Injector, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { OrderConst, PolicyDetailTemplateConst, PolicyTempConst, ProductConst, ProductPolicyConst } from '@shared/AppConsts';
import { CrudComponentBase } from '@shared/crud-component-base';
import { Page } from '@shared/model/page';
import { OrderStepService } from '@shared/service-proxies/order-step-service';
import { OrderServiceProxy } from '@shared/service-proxies/trading-contract-service';
import { DistributionService } from '@shared/services/distribution.service';
import { MessageService } from 'primeng/api';

@Component({
  selector: 'app-order-filter-product',
  templateUrl: './order-filter-product.component.html',
  styleUrls: ['./order-filter-product.component.scss']
})
export class OrderFilterProductComponent extends CrudComponentBase {

    page = new Page();

    orderInfo:any = {
      distributionId: null,
      policyId: null,
      policyDetailId: null,
      totalValue: null,
    };

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

    ProductPolicyConst = ProductPolicyConst;
    OrderConst = OrderConst;
    PolicyTempConst = PolicyTempConst;

    constructor(
        injector: Injector,
        messageService: MessageService,
        public OrderStepService: OrderStepService,
        private _distributionService:  DistributionService,
        private _orderService:  OrderServiceProxy,
        private router: Router
    ) {
        super(injector, messageService);
    }

    ngOnInit() {
        if (!this.OrderStepService?.orderInfo?.cifCode) {
          this.router.navigate(['trading-contract/order/create/filter-customer']);
        }
        console.log("orderInfo...", this.OrderStepService.getOrderInformation());
        //
        this.isLoadingPage = true;
        this._orderService.getProduct().subscribe((res) => {
            this.isLoadingPage = false;
            if (this.handleResponseInterceptor(res, '') && res?.data?.length) {
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
      if(this.OrderStepService.orderInfo?.distributionId) {
        for (const [key, value] of Object.entries(this.orderInfo)) {
          this.orderInfo[key] = this.OrderStepService.orderInfo[key];
        }
        //
        console.log('orderInfo______', this.orderInfo);
        
        this.changeDistribution(this.orderInfo.distributionId, false);
        this.changePolicy(this.orderInfo.policyId, false);
        // this.changePolicyDetail(this.orderInfo.policyDetailId);
      }
    }

    changeDistribution(distributionId, onChange = true) {
      this.distributionInfo = {...this.distributions.find( item => item.id == distributionId)};
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
      console.log('policies', this.policies);
      console.log('distributionInfo', this.distributionInfo);
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
        //
        return this.formatCurrency(totalInterestment);
    }

    //
    changePolicy(policyId, onChange = true) {
        this.policyInfo = {...this.policies.find(p => p.id == policyId)};
        //Reset data (Comment tạm)
        // if(onChange) {
        //   this.policyDetailInfo = {};
        //   this.orderInfo.policyDetailId = null;
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
        // this.profitPolicyDetail = null;
    }

    changePolicyDetail(policyDetailId){
        console.log(policyDetailId);
        this.profitPolicyDetail = this.policyDetails.find(item => item.id == policyDetailId)?.profit;
    }

    nextPage() {
        if (this.orderInfo?.totalValue < this.policyInfo?.minMoney){
            return this.messageError("Số tiền đầu tư phải lớn hơn số tiền đầu tư tối thiểu");
        }
        //
        if (this.orderInfo?.totalValue > this.totalInvestment){
            return this.messageError(`Hạn mức đầu tư tối đa là ${this.utils.transformMoney(this.totalInvestment)} Số tiền đầu tư vượt hạn mức tối đa`);
        }
        //
        if (this.orderInfo?.distributionId && this.orderInfo?.policyId && this.orderInfo?.totalValue) {
          for (const [key, value] of Object.entries(this.orderInfo)) {
            this.OrderStepService.orderInfo[key] = value;
          }
          console.log('this.OrderStepService.orderInfo', this.OrderStepService.orderInfo);
          
          this.router.navigate(['/trading-contract/order/create/view']);
        } else {
          this.messageError('Vui lòng nhập đủ thông tin!');
        }
    }

    prevPage() {
        this.router.navigate(['/trading-contract/order/create/filter-customer']);
    }

}
