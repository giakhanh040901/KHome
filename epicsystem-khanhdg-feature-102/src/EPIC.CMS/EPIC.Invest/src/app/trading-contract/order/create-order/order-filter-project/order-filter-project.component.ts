import { Component, Injector, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { OrderConst, PolicyDetailTemplateConst, PolicyTemplateConst, ProductPolicyConst } from '@shared/AppConsts';
import { CrudComponentBase } from '@shared/crud-component-base';
import { Page } from '@shared/model/page';
import { OrderShareService } from '@shared/service-proxies/shared-data-service';
import { DistributionService } from '@shared/services/distribution.service';
import { MessageService } from 'primeng/api';

@Component({
    selector: 'app-order-filter-project',
    templateUrl: './order-filter-project.component.html',
    styleUrls: ['./order-filter-project.component.scss']
})
export class OrderFilterProjectComponent extends CrudComponentBase {

    page = new Page();

    orderInformation:any = {};
    customerInformation: any = {};
    //
    policies: any[] = [];
    policyDisplays: any[] = [];
    policyDetails: any[] = [];
    policyInfo:any = {};
    //
    rows: any[] = [];
    //
    projects: any[] = [];
    projectInformation:any = {};
    //
    distributions: any [] = [];
    distributionId:number;
    distributionInfo:any = {};

    profitPolicy: any;

    policyTypes = [];
    policyType: number;

    ProductPolicyConst = ProductPolicyConst;
    PolicyTemplateConst = PolicyTemplateConst;
    OrderConst = OrderConst;

    constructor(
        injector: Injector,
        messageService: MessageService,
        public orderService: OrderShareService,
        private _distributionService:  DistributionService,
        private router: Router
    ) {
        super(injector, messageService);
    }


    ngOnInit() {

        this.orderService.filterProject = true;

        if(!this.orderService.filterCustomer && !this.orderService.filterProject && !this.orderService.filterView) {
            this.orderService.resetValue();
        }
        //
        this.policyInfo = this.orderService.getPolicyInfo();
        this.policies = this.orderService.getPolicies();
        this.distributions = this.orderService.getDistributions();
        this.policyDetails = this.orderService.getPolicyDetails();
        this.distributionInfo = this.orderService.getDistributionInfo();
        this.projectInformation = this.orderService.getOrderInformation().projectInformation;
        this.orderInformation = this.orderService.getOrderInformation();
        this.customerInformation = this.orderService.getOrderInformation().customerInformation;
        this.profitPolicy = this.policyDetails.find(item => item.id == this.orderInformation.policyDetailId)?.profit;

        this.orderInformation.buyDate = this.orderInformation.buyDate ?? new Date();
        //
        if (!this.customerInformation?.customerInfo?.cifCode) {
            this.router.navigate(['trading-contract/order/create/filter-customer']);
        }

        this.isLoading = true;
        this._distributionService.getDistributionsOrder().subscribe((res) => {
            if (this.handleResponseInterceptor(res, '')) {
                this.isLoading = false;
                this.distributions = res.data?.map(item => {
                    item.invName = item.project?.invCode + ' - ' + item.project?.invName;
                    return item;
                });
                if(this.distributionInfo?.id) {
                    this.changeProject(this.distributionInfo.id);
                    this.policyType = this.orderInformation.policyType;
                    this.changePolicyType(this.policyType);
                    this.policyInfo = {...this.orderService.getPolicyInfo()};
                    this.changePolicy(this.orderInformation.policyId);
                };
            } else {
                setTimeout(() => {
                    this.router.navigate(['trading-contract/order/create/filter-customer']);
                }, 2000);
            }
        }, (err) => {
            this.isLoading = false;
            console.log('Error-------', err);
        });
    }

    ngOnDestroy(): void {
        this.orderService.filterProject = false;
    }

    changeProject(distributionId) {
        // RESET DATA
        this.policyType = null;
        this.policyDisplays = [];
        this.policyInfo = {};
        //
        this.distributionInfo = {...this.distributions.find( item => item.id == distributionId)};
        //        
        this.orderInformation = this.orderService.getOrderInformation();
        this.projectInformation = this.orderService.getOrderInformation().projectInformation;
        //
        this.policies = this.distributionInfo?.policies;
        if(this.policies?.length) {
            let policyTypeValues = this.policies.map(p => p.type);
            this.policyTypes = PolicyTemplateConst.types.filter(type => policyTypeValues.includes(type.code));
        }

        this.orderInformation.projectId = this.distributionInfo?.projectId;  
        this.projectInformation = this.distributionInfo?.project;
        //
        this.policyDetails = null;
        this.profitPolicy = null;
    }
    
    changePolicyType(type) {
        this.policyInfo = {};
        this.policyDisplays = this.policies.filter(p => p.type == type);
        if(this.orderService.getPolicyInfo()) this.policyInfo = {...this.orderService.getPolicyInfo()};
    }

    changePolicy(policy) {
        this.policyInfo = policy;
        
        this.orderInformation.policyId = policy?.id;
        this.policyDetails = policy.policyDetail;
        this.policyDetails = this.policyDetails.map(policyDetails => {
            policyDetails.periodQuantityPeriodType = policyDetails.periodQuantity + ' ' + PolicyDetailTemplateConst.getNameInterestPeriodType(policyDetails.periodType);
            return policyDetails;
        });
        console.log("policyDetails",this.policyDetails);
        this.profitPolicy = null;
    }

    changePolicyDetail(policyDetail) {
        console.log(policyDetail);
        this.profitPolicy = this.policyDetails.find(item => item.id == policyDetail)?.profit;
    }

    nextPage() {
        if (this.orderInformation?.totalValue < this.policyInfo?.minMoney){
            return this.messageError("Số tiền đầu tư phải lớn hơn số tiền đầu tư tối thiểu", "");
        }
        if (this.orderInformation?.totalValue > this.distributionInfo?.hanMucToiDa){
            return this.messageError(`Hạn mức đầu tư tối đa là ${this.utils.transformMoney(this.distributionInfo?.hanMucToiDa)} Số tiền đầu tư vượt hạn mức tối đa`, "");
        }
        if (this.orderInformation?.projectId && this.orderInformation?.policyId
            && this.orderInformation?.policyDetailId && this.orderInformation?.totalValue
            && this.orderInformation?.buyDate && this.orderInformation?.isInterest
        ) {
            this.orderService.policyInfo = this.policyInfo;
            this.orderService.distributions  = this.distributions;
            this.orderService.distributionInfo = this.distributionInfo;
            this.orderService.policies = this.policies;
            this.orderService.policyDetails = this.policyDetails;
            this.orderService.orderInformation.distributionId = this.distributionInfo.id;
            this.orderService.orderInformation.policyId = this.policyInfo.id;
            this.orderService.orderInformation.policyType = this.policyType;
            this.orderService.getOrderInformation().projectInformation = this.projectInformation;
            console.log("projectInformation...",this.projectInformation);
            
            this.router.navigate(['/trading-contract/order/create/view']);
        } else {
            this.messageService.add({
                severity: 'error',
                detail: 'Vui lòng nhập đủ thông tin!',
                life: 1500,
            });
        }

    }

    prevPage() {
        this.orderService.filterCustomer = true;
        this.router.navigate(['/trading-contract/order/create/filter-customer']);
    }

}
