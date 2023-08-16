import { CrudComponentBase } from '@shared/crud-component-base';
import { Component, Injector, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { OrderService } from '@shared/service-proxies/shared-data-service';
import { MessageService } from 'primeng/api';
import { CompanyShareSecondaryServiceProxy } from '@shared/service-proxies/company-share-manager-service';
import { OrderConst, ProductPolicyConst } from '@shared/AppConsts';
import { OrderServiceProxy } from '@shared/service-proxies/trading-contract-service';
import * as moment from 'moment';
import { DialogService } from 'primeng/dynamicdialog';
import { FilterSaleComponent } from '../order-filter-customer/filter-sale/filter-sale.component';

@Component({
    selector: 'app-order-view',
    templateUrl: './order-view.component.html',
    styleUrls: ['./order-view.component.scss']
})
export class OrderViewComponent extends CrudComponentBase {

    constructor(
        injector: Injector,
        messageService: MessageService,
        public orderService: OrderService,
        private dialogService : DialogService,
        public _orderService: OrderServiceProxy,
        private _companyShareSecondary: CompanyShareSecondaryServiceProxy,
        private router: Router
    ) {
        super(injector, messageService);
    }

    orderInformation: any;
    customerInformation: any;

    listBank: any[] = [];
    sale: any = {}
    investorSale: any = {}
    companyShareSecondaryInformation: any = {};
    companyShareSecondarys: any[] = [];
    policies: any[] = [];
    policyDetails: any[] = [];
    //
    order = {
        "cifCode": "string",
        "investorBankAccId": null,
        "companyShareSecondaryId": 0,
        "companySharePolicyId": 0,
        "companySharePolicyDetailId": 0,
        "totalValue": 0,
        "buyDate": "2022-06-14T08:13:55.618Z",
        "isInterest": "string",
        "saleReferralCode": "string"
    }
    //
    fieldDates = ['buyDate'];

    ProductPolicyConst = ProductPolicyConst;
    OrderConst = OrderConst;

    //
    ngOnInit() {
        this.orderService.filterView = true;
        this.sale = this.orderService.saleInfomation;
        this.investorSale = this.orderService.investorSale;
        this.customerInformation = this.orderService.getOrderInformation().customerInformation;
        this.listBank = this.customerInformation?.customerInfo?.listBank ?? [];
        if (!this.customerInformation?.customerInfo?.cifCode) {
            this.router.navigate(['trading-contract/order/create/filter-customer']);
        }
        if (!this.orderService.filterCustomer && !this.orderService.filterCompanyShare ) {
            
        }

        this.isLoadingPage = true;
        this._companyShareSecondary.getAllSecondary().subscribe((res) => {
            this.isLoadingPage = false;
            if (this.handleResponseInterceptor(res, '')) {
                this.companyShareSecondarys = res?.data;
                this.companyShareSecondaryInformation = this.orderService.getOrderInformation().companyShareSecondaryInformation;
                this.customerInformation = this.orderService.getOrderInformation().customerInformation;
                //
                console.log({ companyShareSecondaryInformation: this.companyShareSecondaryInformation });

                if (this.companyShareSecondaryInformation.companyShareSecondaryId) {
                    this.policies = this.companyShareSecondaryInformation.policies;
                    let policy = this.policies.find(item => item.companySharePolicyId == this.companyShareSecondaryInformation.companySharePolicyId);
                    this.policyDetails = policy.details;
                }
            }
        }, (err) => {
            this.isLoadingPage = false;
            console.log('Error-------', err);
            
        });
    }
    ngOnDestroy(): void {
        this.orderService.filterView = false;
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
                this.sale = sale;
                this.investorSale = sale?.investor;
                this.orderService.saleInfomation = sale;
                this.orderService.investorSale = sale?.investor;
            }
        });
    }
    resetDataSelected() {
        this.companyShareSecondaryInformation.companySharePolicyId = null;
        this.companyShareSecondaryInformation.companySharePolicyDetailId = null;
        this.companyShareSecondaryInformation.policyInfo = {};
        this.policyDetails = [];
    }

    changeCompanyShareSecondary(companyShareSecondaryId) {
        this.isLoading = true;
        this._orderService.getPriceDate(companyShareSecondaryId).subscribe((res) => {
            this.isLoading = false;
            if (this.handleResponseInterceptor(res, '')) {
                this.companyShareSecondaryInformation.priceDate = res?.data?.price ?? 0;
                console.log({ priceDate: this.companyShareSecondaryInformation.priceDate });
                let item = { ...this.companyShareSecondarys.find(item => item.companyShareSecondaryId == companyShareSecondaryId) };
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

    changePolicy(companySharePolicyId) {
        let policy = this.policies.find(item => item.companySharePolicyId == companySharePolicyId);
        this.companyShareSecondaryInformation.dueDate = null;
        this.companyShareSecondaryInformation.policyInfo = policy;
        this.policyDetails = policy.details;
    }

    countQuantity(value = 0) {

        let toTalPrice = value || this.companyShareSecondaryInformation.totalValue;

        console.log('Vao day roi', toTalPrice, value, this.companyShareSecondaryInformation.totalValue, this.companyShareSecondaryInformation.priceDate);

        if (this.companyShareSecondaryInformation.priceDate > 0 && toTalPrice > this.companyShareSecondaryInformation.priceDate) {
            this.companyShareSecondaryInformation.orderQuantity = Math.floor(toTalPrice / this.companyShareSecondaryInformation.priceDate);
            this.companyShareSecondaryInformation.orderPrice = toTalPrice / this.companyShareSecondaryInformation.orderQuantity;
            console.log(this.companyShareSecondaryInformation.orderQuantity, this.companyShareSecondaryInformation.orderPrice, toTalPrice, this.companyShareSecondaryInformation.priceDate);
        }
    }

    setDueDate(companySharePolicyDetailId) {
        let policyDetail = this.policyDetails.find(item => item.companySharePolicyDetailId == companySharePolicyDetailId);
        let investDate = this.companyShareSecondaryInformation.investDate;
        let convertUnitMoment = {
            D: 'd',
            M: 'M',
            Y: 'y',
        }
        // console.log({ policyDetails: this.policyDetails, policyDetailId: companySharePolicyDetailId, policyDetail: policyDetail, investDate: this.companyShareSecondaryInformation.investDate });
        if (policyDetail && investDate) {
            if (policyDetail?.interestDays) {
                if (!policyDetail.periodType) policyDetail.periodType = 'D';
                this.companyShareSecondaryInformation.dueDate = new Date(moment(investDate).add((policyDetail.interestDays || policyDetail.periodQuantity), convertUnitMoment[policyDetail.periodType]).format("YYYY-MM-DD"));
                console.log({ policyDetail: policyDetail, interestDays: policyDetail.interestDays });
                console.log({ dueDate: moment(investDate).add(policyDetail.interestDays, convertUnitMoment[policyDetail.periodType]).format("DD-MM-YYYY") });
            }
        }
    }

    complete() {
        // orderInfo
        this.order.cifCode = this.customerInformation.customerInfo.cifCode;    // Định danh khách hàng
        this.order.investorBankAccId = this.customerInformation.customerInfo.investorBankAccId;    // Tài khoản ngân hàng của khách hàng
        this.order.saleReferralCode = this.sale.referralCode;   // Định danh người giới thiệu
        this.order.companyShareSecondaryId = this.companyShareSecondaryInformation.companyShareSecondaryId;    //ID cổ phần
        this.order.companySharePolicyId = this.companyShareSecondaryInformation.companySharePolicyId;  // Chính sách
        this.order.companySharePolicyDetailId = this.companyShareSecondaryInformation.companySharePolicyDetailId;  // Kỳ hạn
        this.order.totalValue = this.companyShareSecondaryInformation.totalValue;   // Tổng tiền
        this.order.buyDate = this.companyShareSecondaryInformation.buyDate;     // Ngày mua
        this.order.isInterest = this.companyShareSecondaryInformation.isInterest;   // Chi trả lãi
        this.setTimeZoneList(this.fieldDates, this.order);
        console.log("hello this order: ", this.order);

        this._orderService.create(this.order).subscribe((res) => {
            console.log("hello this order: ", this.order);

            if (this.handleResponseInterceptor(res, 'Thêm thành công')) {
                this.isLoadingPage = true;
                setTimeout(() => {
                    this.router.navigate(['/trading-contract/order/detail', this.cryptEncode(res.data.orderId)]);
                }, 200);
            } else {
                // this.callTriggerFiledError(response, this.fieldErrors);
                this.resetTimeZoneList(this.fieldDates, this.order);
            }
            this.orderService.filterView = false;
        }, () => {
            this.router.navigate(['trading-contract/order/create/filter-customer']);
        });

        this.orderService.complete();
    }

    prevPage() {
        this.router.navigate(['trading-contract/order/create/filter-company-share']);
    }

}
