import { CrudComponentBase } from '@shared/crud-component-base';
import { Component, Injector, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { OrderService } from '@shared/service-proxies/shared-data-service';
import { MessageService } from 'primeng/api';
import { ProductBondSecondaryServiceProxy } from '@shared/service-proxies/bond-manager-service';
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
        private _productBondSecondary: ProductBondSecondaryServiceProxy,
        private router: Router
    ) {
        super(injector, messageService);
    }

    orderInformation: any;
    customerInformation: any;

    listBank: any[] = [];
    sale: any = {}
    investorSale: any = {}
    bondSecondaryInformation: any = {};
    bondSecondarys: any[] = [];
    policies: any[] = [];
    policyDetails: any[] = [];
    //
    order = {
        "cifCode": "string",
        "investorBankAccId": null,
        "bondSecondaryId": 0,
        "bondPolicyId": 0,
        "bondPolicyDetailId": 0,
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
        if (!this.orderService.filterCustomer && !this.orderService.filterBond ) {
            
        }

        this.isLoadingPage = true;
        this._productBondSecondary.getAllSecondary().subscribe((res) => {
            this.isLoadingPage = false;
            if (this.handleResponseInterceptor(res, '')) {
                this.bondSecondarys = res?.data;
                this.bondSecondaryInformation = this.orderService.getOrderInformation().bondSecondaryInformation;
                this.customerInformation = this.orderService.getOrderInformation().customerInformation;
                //
                console.log({ bondSecondaryInformation: this.bondSecondaryInformation });

                if (this.bondSecondaryInformation.bondSecondaryId) {
                    this.policies = this.bondSecondaryInformation.policies;
                    let policy = this.policies.find(item => item.bondPolicyId == this.bondSecondaryInformation.bondPolicyId);
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

    changePolicy(bondPolicyId) {
        let policy = this.policies.find(item => item.bondPolicyId == bondPolicyId);
        this.bondSecondaryInformation.dueDate = null;
        this.bondSecondaryInformation.policyInfo = policy;
        this.policyDetails = policy.details;
    }

    countQuantity(value = 0) {

        let toTalPrice = value || this.bondSecondaryInformation.totalValue;

        console.log('Vao day roi', toTalPrice, value, this.bondSecondaryInformation.totalValue, this.bondSecondaryInformation.priceDate);

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
        // console.log({ policyDetails: this.policyDetails, policyDetailId: bondPolicyDetailId, policyDetail: policyDetail, investDate: this.bondSecondaryInformation.investDate });
        if (policyDetail && investDate) {
            if (policyDetail?.interestDays) {
                if (!policyDetail.periodType) policyDetail.periodType = 'D';
                this.bondSecondaryInformation.dueDate = new Date(moment(investDate).add((policyDetail.interestDays || policyDetail.periodQuantity), convertUnitMoment[policyDetail.periodType]).format("YYYY-MM-DD"));
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
        this.order.bondSecondaryId = this.bondSecondaryInformation.bondSecondaryId;    // Id trái phiếu
        this.order.bondPolicyId = this.bondSecondaryInformation.bondPolicyId;  // Chính sách
        this.order.bondPolicyDetailId = this.bondSecondaryInformation.bondPolicyDetailId;  // Kỳ hạn
        this.order.totalValue = this.bondSecondaryInformation.totalValue;   // Tổng tiền
        this.order.buyDate = this.bondSecondaryInformation.buyDate;     // Ngày mua
        this.order.isInterest = this.bondSecondaryInformation.isInterest;   // Chi trả lãi
        
        let bodyOrder = this.formatCalendar(this.fieldDates, {...this.order});
        this._orderService.create(bodyOrder).subscribe((res) => {
            if (this.handleResponseInterceptor(res, 'Thêm thành công')) {
                this.isLoadingPage = true;
                setTimeout(() => {
                    this.router.navigate(['/trading-contract/order/detail', this.cryptEncode(res.data.orderId)]);
                }, 200);
            } 
            this.orderService.filterView = false;
        }, () => {
            this.router.navigate(['trading-contract/order/create/filter-customer']);
        });

        this.orderService.complete();
    }

    prevPage() {
        this.router.navigate(['trading-contract/order/create/filter-product-bond']);
    }

}
