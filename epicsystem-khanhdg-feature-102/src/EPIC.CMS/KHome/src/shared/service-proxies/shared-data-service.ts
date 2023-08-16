import { OrderConst } from './../AppConsts';
import { mergeMap as _observableMergeMap, catchError as _observableCatch } from "rxjs/operators";
import { Observable, throwError as _observableThrow, of as _observableOf } from "rxjs";
import { Injectable } from "@angular/core";


import { Subject } from 'rxjs';

@Injectable()
export class OrderService {
    
    orderInformation:any = {
        id: 0,
        tradingProviderId: 0,
        cifCode: null,
        departmentId: 0,
        projectId: 0,
        distributionId: 0,
        policyType: 0,
        policyId: 0,
        policyDetailId: 0,
        totalValue: null,
        buyDate: null,
        isInterest: 'Y',
        saleReferralCode: null,
        source: 2,
        contractCode: null,
        investorBankAccId: 0,
        paymentFullDate: null,
        status: 1,
        customerInformation: {
            activeIndex: 0,
            customerInfo: {
                cifCode: null,
                taxCode: null,
                name: null,
                phone: null,
                email: null,
                adress: null,
                investorBankAccId: 0,
                contractAddressId: null,
                listAddress: [],
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
        projectInformation:{
            id: 0,
            ownerId: 0,
            generalContractorId: 0,
            invCode: null,
            invName: null,
            content: null,
            startDate: null,
            endDate: null,
            image: null,
            isPaymentGuarantee: null,
            area: 0,
            longitude: 0,
            latitude: 0,
            locationDescription: null,
            totalInvestment: 0,
            projectType: 0,
            projectProgress: null,
            guaranteeOrganization: null,
            isCheck: null,
            status: null,
            juridicalFiles: null,
            owner:{},
            generalContractor:{},
            partnerId: null,
        },
        policy:{},
        policyDetail:{}

    };
    
   
    distributionInfo:any = {}
    policyInfo:any = {}
    policies:any[] = [];
    policyDetails:any[] = [];
    distributions:any[] = [];
    private orderComplete = new Subject<any>();
    
    orderComplete$ = this.orderComplete.asObservable();

    filterCustomer:boolean;
    filterProject:boolean;
    filterView:boolean;
    investorSale:any = {}
    saleInfomation:any = {}
    getDistributions(){
        return this.distributions;
    }
    getPolicyDetails(){
        return this.policyDetails;
    }

    getPolicies(){
        
        return this.policies;
    }

    resetValue() {
        this.saleInfomation = {}
        this.investorSale = {}
        this.distributionInfo = {}
        this.policyInfo = {}
        this.policies = [];
        this.policyDetails = [];
        this.distributions = [];
        this.orderInformation = {
            id: 0,
            tradingProviderId: 0,
            cifCode: null,
            departmentId: 0,
            projectId: 0,
            distributionId: 0,
            policyId: 0,
            policyDetailId: 0,
            totalValue: null,
            buyDate: null,
            isInterest: 'Y',
            saleReferralCode: null,
            source: 2,
            contractCode: null,
            investorBankAccId: 0,
            paymentFullDate: null,
            status: 1,
            customerInformation: {
                activeIndex: 0,
                customerInfo: {
                    cifCode: null,
                    taxCode: null,
                    name: null,
                    phone: null,
                    email: null,
                    adress: null,
                    investorBankAccId: 0,
                    contractAddressId: null,
                    listAddress: [],
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
            projectInformation:{
                id: 0,
                ownerId: 0,
                generalContractorId: 0,
                invCode: null,
                invName: null,
                content: null,
                startDate: null,
                endDate: null,
                image: null,
                isPaymentGuarantee: null,
                area: 0,
                longitude: 0,
                latitude: 0,
                locationDescription: null,
                totalInvestment: 0,
                projectType: 0,
                projectProgress: null,
                guaranteeOrganization: null,
                isCheck: null,
                status: null,
                juridicalFiles: null,
                owner:{},
                generalContractor:{},
                partnerId: null,
            },
            policy:{},
            policyDetail:{}
        };
    }

    // getCustomerClear(){
    //     return this.orderClear.customerInformation;
    // }
    
    // getOrderClear(){
    //     return this.orderClear;
    // }
   
    getOrderInformation() {
        return this.orderInformation ;
    }
    getPolicyInfo(){
        return this.policyInfo;
    }
    getDistributionInfo(){
        return this.distributionInfo;
    }

    setOrderInformation(orderInformation) {
        this.orderInformation = orderInformation;
    }

    complete() {
        this.orderComplete.next(this.orderInformation.customerInformation);
    }
}

