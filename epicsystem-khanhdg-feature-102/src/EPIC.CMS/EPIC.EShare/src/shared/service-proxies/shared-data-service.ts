import { OrderConst } from './../AppConsts';
import { mergeMap as _observableMergeMap, catchError as _observableCatch } from "rxjs/operators";
import { Observable, throwError as _observableThrow, of as _observableOf } from "rxjs";
import { Injectable } from "@angular/core";


import { Subject } from 'rxjs';

@Injectable()
export class OrderService {

    orderInformation = {
        customerInformation: {
            activeIndex: 0,
            customerInfo: {
                cifCode: null,
                taxCode: null,
                name: null,
                phone: null,
                email: null,
                adress: null,
                investorBankAccId: null,
                listBank: [],
            },
            presenterInfo: {
                referalCode: null,
            },
            aboutCode: null,
            presenterName: null,
            presenterPhone: null,
            presenterEmail: null,
        },
        companyShareSecondaryInformation: {
            companyShareSecondaryId: null,
            companyShareInfo: {
                companyShareCode: null,
                companyShareName: null,
                issueDate: null,
            },
            openCellDate: null,
            quantity: null,
            soLuongConLai: null,
            tongGiaTri: null,
            companySharePolicyId: 0,
            policyInfo: {
                code: null,
                name: null,
                type: null,
                soNgayDuocBanLai: null,
                kyHan: null,
            },
            companySharePolicyDetailId: 0,
            totalValue: null,
            buyDate: new Date(),
            investDate: new Date(),
            signContractDate: null,
            priceDate: 0,
            orderQuantity: null,
            orderPrice: null,
            dueDate: null,
            isInterest: OrderConst.INTEREST_TYPE_YES,
        },
        orderEditting: false,
    };
    saleInfomation: any = {}
    investorSale: any = {}
    private orderComplete = new Subject<any>();

    filterCustomer:boolean;
    filterCompanyShare:boolean;
    filterView:boolean;

    orderComplete$ = this.orderComplete.asObservable();

    getOrderInformation() {
        return this.orderInformation;
    }

    getOrderInformationReset() {
        return this.orderInformation = {
            customerInformation: {
                activeIndex: 0,
                customerInfo: {
                    cifCode: null,
                    taxCode: null,
                    name: null,
                    phone: null,
                    email: null,
                    adress: null,
                    investorBankAccId: null,
                    listBank: [],
                },
                presenterInfo: {
                    referalCode: null,
                },
                aboutCode: null,
                presenterName: null,
                presenterPhone: null,
                presenterEmail: null,
            },
            companyShareSecondaryInformation: {
                companyShareSecondaryId: null,
                companyShareInfo: {
                    companyShareCode: null,
                    companyShareName: null,
                    issueDate: null,
                },
                openCellDate: null,
                quantity: null,
                soLuongConLai: null,
                tongGiaTri: null,
                companySharePolicyId: 0,
                policyInfo: {
                    code: null,
                    name: null,
                    type: null,
                    soNgayDuocBanLai: null,
                    kyHan: null,
                },
                companySharePolicyDetailId: 0,
                totalValue: null,
                buyDate: new Date(),
                investDate: new Date(),
                signContractDate: null,
                priceDate: 0,
                orderQuantity: null,
                orderPrice: null,
                dueDate: null,
                isInterest: OrderConst.INTEREST_TYPE_YES,
            },
            orderEditting: false,
        }
    }

    resetValue(){
        this.orderInformation = {
            customerInformation: {
                activeIndex: 0,
                customerInfo: {
                    cifCode: null,
                    taxCode: null,
                    name: null,
                    phone: null,
                    email: null,
                    adress: null,
                    investorBankAccId: null,
                    listBank: [],
                },
                presenterInfo: {
                    referalCode: null,
                },
                aboutCode: null,
                presenterName: null,
                presenterPhone: null,
                presenterEmail: null,
            },
            companyShareSecondaryInformation: {
                companyShareSecondaryId: null,
                companyShareInfo: {
                    companyShareCode: null,
                    companyShareName: null,
                    issueDate: null,
                },
                openCellDate: null,
                quantity: null,
                soLuongConLai: null,
                tongGiaTri: null,
                companySharePolicyId: 0,
                policyInfo: {
                    code: null,
                    name: null,
                    type: null,
                    soNgayDuocBanLai: null,
                    kyHan: null,
                },
                companySharePolicyDetailId: 0,
                totalValue: null,
                buyDate: new Date(),
                investDate: new Date(),
                signContractDate: null,
                priceDate: 0,
                orderQuantity: null,
                orderPrice: null,
                dueDate: null,
                isInterest: OrderConst.INTEREST_TYPE_YES,
            },
            orderEditting: false,
        };
        this.saleInfomation = {}
        this.investorSale = {}
    }

    complete() {
        this.orderComplete.next(this.orderInformation.customerInformation);
    }
}

