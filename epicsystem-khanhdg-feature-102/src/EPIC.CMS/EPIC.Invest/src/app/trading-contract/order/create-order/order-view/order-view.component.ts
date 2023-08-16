import { CrudComponentBase } from '@shared/crud-component-base';
import { Component, Injector, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { OrderShareService } from '@shared/service-proxies/shared-data-service';
import { MessageService } from 'primeng/api';
import { OrderConst, PolicyDetailTemplateConst, PolicyTemplateConst, ProductPolicyConst } from '@shared/AppConsts';
import { DistributionService } from '@shared/services/distribution.service';
import { FilterSaleComponent } from '../order-filter-customer/filter-sale/filter-sale.component';
import { DialogService } from 'primeng/dynamicdialog';
import { OrderService } from '@shared/services/order.service';

@Component({
    selector: 'app-order-view',
    templateUrl: './order-view.component.html',
    styleUrls: ['./order-view.component.scss']
})
export class OrderViewComponent extends CrudComponentBase {

    constructor(
        injector: Injector,
        messageService: MessageService,
        public orderService: OrderShareService,
        private dialogService: DialogService,
        public _orderService: OrderService,
        private _distributionService: DistributionService,
        private router: Router
    ) {
        super(injector, messageService);
    }

    orderInformation: any;
    customerInformation: any;
    projectInformation: any = {};
    listBank: any[] = [];
    projects: any[] = [];
    bondSecondaryInformation: any = {};

    policies: any[] = [];
    policyDisplays: any[] = [];
    policyDetails: any[] = [];
    policyInfo: any = {}
    //
    policyTypes = [];
    //
    distributions: any[] = [];
    distributionInfo: any = {}
    investorSale: any = {}
    sale: any = {}
    profitPolicy: any;
    //
    order = {
        "cifCode": "",
        "distributionId": 0,
        "policyId": 0,
        "policyDetailId": 0,
        "totalValue": 0,
        "investorBankAccId": 0,
        "isInterest": "",
        "saleReferralCode": "",
        "projectId": 0,
        "departmentId": 0,
        "contractAddressId": null,
    }
    //
    fieldDates = ['buyDate'];

    ProductPolicyConst = ProductPolicyConst;
    OrderConst = OrderConst;

    ngOnInit() {
        this.orderInformation = this.orderService.getOrderInformation();
        this.sale = this.orderService.saleInfomation;
        this.investorSale = this.orderService.investorSale;
        this.orderService.filterView = true;
        this.policyInfo = this.orderService.getPolicyInfo();
        this.policies = this.orderService.getPolicies();
        if(this.policies?.length) {
            let policyTypeValues = this.policies.map(p => p.type);
            this.policyTypes = PolicyTemplateConst.types.filter(type => policyTypeValues.includes(type.code));
            this.changePolicyType(this.orderInformation.policyType);
        }
        this.distributions = this.orderService.getDistributions();
        this.policyDetails = this.orderService.getPolicyDetails();
        this.distributionInfo = this.orderService.getDistributionInfo();
        this.projectInformation = this.orderService.getOrderInformation().projectInformation;
        this.customerInformation = this.orderService.getOrderInformation().customerInformation;
        console.log("orderInformation...", this.orderInformation);
        this.listBank = this.customerInformation?.customerInfo?.listBank ?? [];
        this.profitPolicy = this.policyDetails.find(item => item.id == this.orderInformation.policyDetailId)?.profit;
        
        if (!this.customerInformation?.customerInfo?.cifCode) {
            this.router.navigate(['trading-contract/order/create/filter-customer']);
        }
        this._distributionService.getDistributionsOrder().subscribe((res) => {
            if (this.handleResponseInterceptor(res, '')) {
                this.distributions = res.data?.map(item => {
                    item.invName = item.project?.invCode + ' - ' + item.project?.invName;
                    return item;
                });
            }
        });

    }

    removeReferralCode() {
        this.sale = null
        this.investorSale = null
        this.orderService.saleInfomation.referralCode = null
    }

    ngOnDestroy(): void {
        this.orderService.filterView = false;

    }

    resetDataSelected() {
        this.bondSecondaryInformation.bondPolicyId = null;
        this.bondSecondaryInformation.bondPolicyDetailId = null;
        this.bondSecondaryInformation.policyInfo = {};
        this.policyDetails = [];
    }

    changeProject(distributionId) {
        this.distributionInfo = {...this.distributions.find(item => item.id == distributionId)};
        this.orderInformation = this.orderService.getOrderInformation();
        this.projectInformation = this.orderService.getOrderInformation().projectInformation;
        this.policies = this.distributionInfo?.policies;
        if(this.policies?.length) {
            let policyTypeValues = this.policies.map(p => p.type);
            this.policyTypes = PolicyTemplateConst.types.filter(type => policyTypeValues.includes(type.code));
        }
        this.orderInformation.projectId = this.distributionInfo?.projectId;
        this.projectInformation = this.distributionInfo?.project;
        console.log("project...", this.projectInformation);
        this.policyDetails = null;
        this.profitPolicy = null;
    }

    changePolicyType(value) {
        this.policyDisplays = this.policies.filter(p => p.type == value);
    }

    changePolicy(policy) {
        this.policyInfo = policy;
        this.orderInformation.policyId = policy?.id;
        this.policyDetails = policy.policyDetail;
        this.policyDetails = this.policyDetails.map(policyDetails => {
            policyDetails.periodQuantityPeriodType = policyDetails.periodQuantity + ' ' + PolicyDetailTemplateConst.getNameInterestPeriodType(policyDetails.periodType);
            return policyDetails;
        });
        this.profitPolicy = null;
    }

    changePolicyDetail(policyDetail) {
        console.log(policyDetail);
        this.profitPolicy = this.policyDetails.find(item => item.id == policyDetail)?.profit;
    }

    showSale() {
        const ref = this.dialogService.open(FilterSaleComponent,
            {
                header: 'Tìm kiếm sale',
                width: '1000px',
                styleClass: 'p-dialog-custom filter-business-customer customModal',
                contentStyle: { "max-height": "600px", "overflow": "auto" },
                style: { 'top': '-15%', 'overflow': 'hidden' }
            });

        ref.onClose.subscribe((sale) => {
            if (sale) {
                if(sale?.referralCode) {
                    this.orderService.saleInfomation.referralCode = sale?.referralCode
                }
                
                this.sale = sale;
                this.investorSale = sale?.investor;
            }
        });
    }

    complete() {
        this.order.cifCode = this.orderInformation.cifCode;
        this.order.distributionId = this.orderInformation.distributionId;
        this.order.policyId = this.orderInformation.policyId;
        this.order.policyDetailId = this.orderInformation.policyDetailId;
        this.order.totalValue = this.orderInformation.totalValue;
        this.order.investorBankAccId = this.orderInformation.investorBankAccId;
        this.order.isInterest = this.orderInformation.isInterest;
        this.order.projectId = this.orderInformation.projectId;
        this.order.departmentId = this.orderInformation.departmentId;
        this.order.saleReferralCode = this.orderService.saleInfomation.referralCode;
        this.order.contractAddressId = this.customerInformation.customerInfo.contractAddressId;
        //
        let body = this.formatCalendar(this.fieldDates, {...this.order});
        console.log('orderBody___', body);
        this._orderService.create(body).subscribe((res) => {
            if (this.handleResponseInterceptor(res, 'Thêm thành công')) {
                this.detail(res.data.id);
                console.log("order ...", res);
            } 
        }, () => {
            this.router.navigate(['trading-contract/order/create/filter-customer']);
        });

        this.orderService.complete();
    }


    detail(orderId) {
        this.router.navigate(['/trading-contract/order/detail/' + this.cryptEncode(orderId)]);

    }

    prevPage() {
        this.router.navigate(['trading-contract/order/create/filter-project']);
    }

}
