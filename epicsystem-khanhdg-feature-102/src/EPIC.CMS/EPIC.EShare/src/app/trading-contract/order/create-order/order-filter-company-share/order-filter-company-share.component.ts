import { CrudComponentBase } from '@shared/crud-component-base';
import { Component, Injector, OnInit } from '@angular/core';
import { OrderService } from '@shared/service-proxies/shared-data-service';
import { Router } from '@angular/router';
import { MessageService } from 'primeng/api';
import { Page } from '@shared/model/page';
import { OrderConst, ProductPolicyConst } from '@shared/AppConsts';
import { CompanyShareSecondaryServiceProxy } from '@shared/service-proxies/company-share-manager-service';
import * as moment from 'moment';
import { OrderServiceProxy } from '@shared/service-proxies/trading-contract-service';

@Component({
    selector: 'app-order-filter-company-share',
    templateUrl: './order-filter-company-share.component.html',
    styleUrls: ['./order-filter-company-share.component.scss']
})
export class OrderFilterCompanyShareComponent extends CrudComponentBase {

    page = new Page();

    customerInformation: any;
    companyShareSecondaryInformation: any = {};

    companyShareSecondary: any[] = [];
    policies: any[] = [];
    policyDetails: any[] = [];

    ProductPolicyConst = ProductPolicyConst;
    OrderConst = OrderConst;

    constructor(
        injector: Injector,
        messageService: MessageService,
        public orderService: OrderService,
        private _companyShareSecondary: CompanyShareSecondaryServiceProxy,
        private _orderService: OrderServiceProxy,
        private router: Router
    ) {
        super(injector, messageService);
    }

    ngOnInit() {
        this.orderService.filterCompanyShare = true;
        this.customerInformation = this.orderService.getOrderInformation().customerInformation;
        if (!this.customerInformation?.customerInfo?.cifCode) {
            this.backLink();
        }

        this.isLoadingPage = true;
        this._companyShareSecondary.getAllSecondary().subscribe((res) => {
            this.isLoadingPage = false;
            if (this.handleResponseInterceptor(res, '')) {
                if (res?.data) {
                    this.companyShareSecondary = res?.data;
                    console.log('data',this.companyShareSecondary);
                    
                    console.log("hello", this.companyShareSecondary[0].secondaryId);
                    this.companyShareSecondaryInformation = this.orderService.getOrderInformation().companyShareSecondaryInformation;
                    if (this.companyShareSecondaryInformation.secondaryId) {
                        this.policies = this.companyShareSecondaryInformation?.policies ?? [];
                        let policy: any = {};
                        if (this.policies?.length) {
                            policy = this.policies.find(item => item?.policyId == this.companyShareSecondaryInformation?.policyId);
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
        this.orderService.filterCompanyShare = false;
    }

    backLink() {
        this.router.navigate(['trading-contract/order/create/filter-customer']);
    }

    resetDataSelected() {
        this.companyShareSecondaryInformation.policyId = null;
        this.companyShareSecondaryInformation.policyDetailId = null;
        this.companyShareSecondaryInformation.policyInfo = {};
        this.policyDetails = [];
    }

    changeCompanyShareSecondary(secondaryId) {
        this.isLoading = true;
        this._orderService.getPriceDate(secondaryId).subscribe((res) => {
            this.isLoading = false;
            if (this.handleResponseInterceptor(res, '')) {
                this.companyShareSecondaryInformation.priceDate = res?.data?.price ?? 0;
                console.log({ priceDate: this.companyShareSecondaryInformation.priceDate });
                let item = { ...this.companyShareSecondary.find(item => item.secondaryId == secondaryId) };
                this.policies = item.policies;
                this.companyShareSecondaryInformation = {
                    ...item,
                    totalValue: this.companyShareSecondaryInformation.totalValue,
                    buyDate: this.companyShareSecondaryInformation.buyDate,
                    investDate: this.companyShareSecondaryInformation.investDate,
                    signContractDate: this.companyShareSecondaryInformation.signContractDate,
                    priceDate: this.companyShareSecondaryInformation.priceDate,
                    isInterest: this.companyShareSecondaryInformation.isInterest,
                }
                this.countQuantity();
                this.resetDataSelected();
                console.log({ companyShareSecondaryInformation: this.companyShareSecondaryInformation });
            }
        }, (err) => {
            this.isLoading = false;
            console.log('Error-------', err);
            
        });
    }

    countQuantity(value = 0) {
        let toTalPrice = value || this.companyShareSecondaryInformation.totalValue;
        console.log("priceDate", this.companyShareSecondaryInformation.priceDate);
        
        if (this.companyShareSecondaryInformation.priceDate > 0 && toTalPrice > this.companyShareSecondaryInformation.priceDate) {
            this.companyShareSecondaryInformation.orderQuantity = Math.floor(toTalPrice / this.companyShareSecondaryInformation.priceDate);
            this.companyShareSecondaryInformation.orderPrice = toTalPrice / this.companyShareSecondaryInformation.orderQuantity;
            console.log(this.companyShareSecondaryInformation.orderQuantity, this.companyShareSecondaryInformation.orderPrice, toTalPrice, this.companyShareSecondaryInformation.priceDate);
        }
    }

    setDueDate(policyDetailId) {
        let policyDetail = this.policyDetails.find(item => item.policyDetailId == policyDetailId);
        let investDate = this.companyShareSecondaryInformation.investDate;
        let convertUnitMoment = {
            D: 'd',
            M: 'M',
            Y: 'y',
        }
        if (policyDetail && investDate) {
            if (policyDetail?.interestDays) {
                policyDetail.periodType = 'D';
                this.companyShareSecondaryInformation.dueDate = new Date(moment(investDate).add((policyDetail.interestDays), convertUnitMoment[policyDetail.periodType]).format("YYYY-MM-DD"));
                console.log('Số ngày', policyDetail?.interestDays, 'Kỳ hạn', policyDetail.periodType);
            }
            if (!policyDetail?.interestDays && policyDetail?.periodQuantity) {
                this.companyShareSecondaryInformation.dueDate = new Date(moment(investDate).add((policyDetail.periodQuantity), convertUnitMoment[policyDetail.periodType]).format("YYYY-MM-DD"));
            }
            console.log({ policyDetail: policyDetail, interestDays: policyDetail.interestDays });
            console.log('Ngày đáo hạn', this.companyShareSecondaryInformation.dueDate);
        }
    }

    changePolicy(policyId) {
        let policy = this.policies.find(item => item.policyId == policyId);
        this.companyShareSecondaryInformation.dueDate = null;
        this.companyShareSecondaryInformation.policyInfo = policy;
        this.policyDetails = policy.details;
        console.log({ policyDetails: this.policyDetails });
    }

    resetValid(field: string) {

    }

    nextPage() {

        this.orderService.filterCompanyShare = false;
        this.orderService.filterView = true;

        if (this.companyShareSecondaryInformation?.secondaryId && this.companyShareSecondaryInformation?.policyId
            && this.companyShareSecondaryInformation?.policyDetailId && this.companyShareSecondaryInformation?.totalValue
            && this.companyShareSecondaryInformation?.buyDate && this.companyShareSecondaryInformation?.isInterest
        ) {
            this.orderService.orderInformation.companyShareSecondaryInformation = this.companyShareSecondaryInformation;
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
        this.orderService.filterCompanyShare = false;
        this.orderService.filterCustomer = true;
        this.orderService.getOrderInformation().orderEditting = true;   // Trạng thái chỉnh sửa đặt lệnh được kích hoạt khi back
        this.router.navigate(['/trading-contract/order/create/filter-customer']);
    }

}
