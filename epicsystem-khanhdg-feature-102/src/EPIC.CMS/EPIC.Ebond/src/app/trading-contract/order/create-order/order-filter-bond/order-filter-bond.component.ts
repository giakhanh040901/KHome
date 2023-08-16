import { CrudComponentBase } from '@shared/crud-component-base';
import { Component, Injector, OnInit } from '@angular/core';
import { OrderService } from '@shared/service-proxies/shared-data-service';
import { Router } from '@angular/router';
import { MessageService } from 'primeng/api';
import { Page } from '@shared/model/page';
import { OrderConst, ProductPolicyConst } from '@shared/AppConsts';
import { ProductBondSecondaryServiceProxy } from '@shared/service-proxies/bond-manager-service';
import * as moment from 'moment';
import { OrderServiceProxy } from '@shared/service-proxies/trading-contract-service';

@Component({
    selector: 'app-order-filter-bond',
    templateUrl: './order-filter-bond.component.html',
    styleUrls: ['./order-filter-bond.component.scss']
})
export class OrderFilterBondComponent extends CrudComponentBase {

    page = new Page();

    customerInformation: any;
    bondSecondaryInformation: any = {};

    bondSecondarys: any[] = [];
    policies: any[] = [];
    policyDetails: any[] = [];

    ProductPolicyConst = ProductPolicyConst;
    OrderConst = OrderConst;

    constructor(
        injector: Injector,
        messageService: MessageService,
        public orderService: OrderService,
        private _productBondSecondary: ProductBondSecondaryServiceProxy,
        private _orderService: OrderServiceProxy,
        private router: Router
    ) {
        super(injector, messageService);
    }

    ngOnInit() {
        this.orderService.filterBond = true;
        this.customerInformation = this.orderService.getOrderInformation().customerInformation;
        if (!this.customerInformation?.customerInfo?.cifCode) {
            this.backLink();
        }

        this.isLoadingPage = true;
        this._productBondSecondary.getAllSecondary().subscribe((res) => {
            this.isLoadingPage = false;
            if (this.handleResponseInterceptor(res, '')) {
                if (res?.data) {
                    this.bondSecondarys = res?.data;
                    console.log('data',this.bondSecondarys);
                    
                    console.log("hello", this.bondSecondarys[0].bondSecondaryId);
                    this.bondSecondaryInformation = this.orderService.getOrderInformation().bondSecondaryInformation;
                    if (this.bondSecondaryInformation.bondSecondaryId) {
                        this.policies = this.bondSecondaryInformation?.policies ?? [];
                        let policy: any = {};
                        if (this.policies?.length) {
                            policy = this.policies.find(item => item?.bondPolicyId == this.bondSecondaryInformation?.bondPolicyId);
                            this.policyDetails = policy?.details ?? [];
                        }
                    }
                }
            } else {
                setTimeout(() => {
                    this.backLink();
                }, 2000);
            }
        }, (err) => {
            this.isLoadingPage = false;
            this.isLoading = false;
            console.log('Error-------', err);
            
        });
    }
    ngOnDestroy(): void {
        //Called once, before the instance is destroyed.
        //Add 'implements OnDestroy' to the class.
        this.orderService.filterBond = false;
    }

    backLink() {
        this.router.navigate(['trading-contract/order/create/filter-customer']);
    }

    resetDataSelected() {
        this.bondSecondaryInformation.bondPolicyId = null;
        this.bondSecondaryInformation.bondPolicyDetailId = null;
        this.bondSecondaryInformation.policyInfo = {};
        this.policyDetails = [];
    }

    changeBondSecondary(bondSecondaryId) {
        this.isLoading = true;
        this._orderService.getPriceDate(bondSecondaryId).subscribe((res) => {
            this.isLoading = false;
            if (this.handleResponseInterceptor(res, '')) {
                this.bondSecondaryInformation.priceDate = res?.data?.price ?? 0;
                console.log({ priceDate: this.bondSecondaryInformation.priceDate });
                let item = { ...this.bondSecondarys.find(item => item.bondSecondaryId == bondSecondaryId) };
                this.policies = item.policies;
                this.bondSecondaryInformation = {
                    ...item,
                    totalValue: this.bondSecondaryInformation.totalValue,
                    buyDate: this.bondSecondaryInformation.buyDate,
                    investDate: this.bondSecondaryInformation.investDate,
                    signContractDate: this.bondSecondaryInformation.signContractDate,
                    priceDate: this.bondSecondaryInformation.priceDate,
                    isInterest: this.bondSecondaryInformation.isInterest,
                }
                this.countQuantity();
                this.resetDataSelected();
                console.log({ bondSecondaryInformation: this.bondSecondaryInformation });
            }
        }, (err) => {
            this.isLoading = false;
            console.log('Error-------', err);
            
        });
    }

    countQuantity(value = 0) {
        let toTalPrice = value || this.bondSecondaryInformation.totalValue;
        console.log("priceDate", this.bondSecondaryInformation.priceDate);
        
        if (this.bondSecondaryInformation.priceDate > 0 && toTalPrice > this.bondSecondaryInformation.priceDate) {
            this.bondSecondaryInformation.orderQuantity = Math.floor(toTalPrice / this.bondSecondaryInformation.priceDate);
            this.bondSecondaryInformation.orderPrice = toTalPrice / this.bondSecondaryInformation.orderQuantity;
            console.log(this.bondSecondaryInformation.orderQuantity, this.bondSecondaryInformation.orderPrice, toTalPrice, this.bondSecondaryInformation.priceDate);
        }
    }

    setDueDate(bondPolicyDetailId) {
        let policyDetail = this.policyDetails.find(item => item.bondPolicyDetailId == bondPolicyDetailId);
        let investDate = this.bondSecondaryInformation.investDate;
        let convertUnitMoment = {
            D: 'd',
            M: 'M',
            Y: 'y',
        }
        if (policyDetail && investDate) {
            if (policyDetail?.interestDays) {
                policyDetail.periodType = 'D';
                this.bondSecondaryInformation.dueDate = new Date(moment(investDate).add((policyDetail.interestDays), convertUnitMoment[policyDetail.periodType]).format("YYYY-MM-DD"));
                console.log('Số ngày', policyDetail?.interestDays, 'Kỳ hạn', policyDetail.periodType);
            }
            if (!policyDetail?.interestDays && policyDetail?.periodQuantity) {
                this.bondSecondaryInformation.dueDate = new Date(moment(investDate).add((policyDetail.periodQuantity), convertUnitMoment[policyDetail.periodType]).format("YYYY-MM-DD"));
            }
            console.log({ policyDetail: policyDetail, interestDays: policyDetail.interestDays });
            console.log('Ngày đáo hạn', this.bondSecondaryInformation.dueDate);
        }
    }

    changePolicy(bondPolicyId) {
        let policy = this.policies.find(item => item.bondPolicyId == bondPolicyId);
        this.bondSecondaryInformation.dueDate = null;
        this.bondSecondaryInformation.policyInfo = policy;
        this.policyDetails = policy.details;
        console.log({ policyDetails: this.policyDetails });
    }

    resetValid(field: string) {

    }

    nextPage() {

        this.orderService.filterBond = false;
        this.orderService.filterView = true;

        if (this.bondSecondaryInformation?.bondSecondaryId && this.bondSecondaryInformation?.bondPolicyId
            && this.bondSecondaryInformation?.bondPolicyDetailId && this.bondSecondaryInformation?.totalValue
            && this.bondSecondaryInformation?.buyDate && this.bondSecondaryInformation?.isInterest
        ) {
            this.orderService.orderInformation.bondSecondaryInformation = this.bondSecondaryInformation;
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
        this.orderService.filterBond = false;
        this.orderService.filterCustomer = true;
        this.orderService.getOrderInformation().orderEditting = true;   // Trạng thái chỉnh sửa đặt lệnh được kích hoạt khi back
        this.router.navigate(['/trading-contract/order/create/filter-customer']);
    }

}
