import { Component, ElementRef, EventEmitter, Injector, Input, Output, ViewChild } from '@angular/core';
import { ProductBondPrimaryConst, ProductBondInfoConst, ProductPolicyConst, OrderConst, PolicyDetailTemplateConst, PolicyTemplateConst, InvestorConst } from '@shared/AppConsts';
import { CrudComponentBase } from '@shared/crud-component-base';
import { MessageService } from 'primeng/api';
import { BreadcrumbService } from 'src/app/layout/breadcrumb/breadcrumb.service';
import { ActivatedRoute } from '@angular/router';
import { DistributionService } from '@shared/services/distribution.service';
import { TabView } from 'primeng/tabview';
import { OrderService } from '@shared/services/order.service';
import { TabViewOrderDetail } from '@shared/interface/tabview.model';

@Component({
  selector: 'app-order-detail-view',
  templateUrl: './order-detail-view.component.html',
  styleUrls: ['./order-detail-view.component.scss']
})
export class OrderDetailViewComponent extends CrudComponentBase {

    constructor(
        injector: Injector,
        messageService: MessageService,
        private routeActive: ActivatedRoute,
        private _orderService: OrderService,
        private breadcrumbService: BreadcrumbService,
        private _distributionService: DistributionService,
  
    ) {
        super(injector, messageService);
        this.orderId = +this.cryptDecode(this.routeActive.snapshot.paramMap.get('id'));
        this.breadcrumbService.setItems([
            { label: 'Trang chủ', routerLink: ['/home'] },
            { label: 'Sổ lệnh', routerLink: ['/trading-contract/order'] },
            { label: 'Thông tin đầu tư' },
        ]);
    }
  
    orderInformation: any;
    customerInformation: any = {};
  
    bondSecondaryInformation: any = {};
    bondSecondary: any = {};
    bondInfos: any[] = [];

    policies: any[] = [];
    policyInfo: any = {};
    policyDetails: any[] = [];
    policyDetail: any = {};

    profit: number = null;

    orderData: any = {};

    sale: any = {};

    policyTypes = [];
    policyDisplays = [];
    policyType: number;

    ProductBondPrimaryConst = ProductBondPrimaryConst;
    ProductBondInfoConst = ProductBondInfoConst;
    ProductPolicyConst = ProductPolicyConst;
    InvestorConst = InvestorConst
    OrderConst = OrderConst;

    listIdNo : any;
  
    isEdit = false;
    fieldErrors: any = {};
  
    orderId: number;
    orderDetail: any = {};
  
    distributions: any[] = [];
    //
    listBank: any[] = [];
    listAddress: any[] = [];
    //
    distributionInfo: any = {};
    projectInformation: any = {};
    //
    tabViewActive: TabViewOrderDetail = new TabViewOrderDetail();

    @Input() contentHeights: number[] = [];
  
    @ViewChild(TabView) tabView: TabView;

    ngOnInit() {
        this.isPartner = this.getIsPartner();

        this.isLoading = true;
        this._distributionService.getDistributionsOrder().subscribe((res) => {
            if (this.handleResponseInterceptor(res, "")) {
                this.distributions = res?.data;
            }
        }, (err) => {
            this.isLoading = false;
            console.log("Error-------", err);
            }
        );
        this.init();
    }
    
    changeTab(e) {
        let tabHeader = this.tabView.tabs[e.index].header;
        this.tabViewActive[tabHeader] = true;
    }
  
    init() {
        this._orderService.get(this.orderId).subscribe((resOrder) => {
            if (this.handleResponseInterceptor(resOrder, "")) {
                this.isLoading = false;
                this.orderDetail = {
                    ...resOrder.data,
                    buyDate: resOrder.data?.buyDate ? new Date(resOrder.data?.buyDate) : null,
                    paymentFullDate: resOrder.data?.paymentFullDate ?? resOrder.data?.buyDate,
                    showContractCodeSystem: !!(resOrder.data.contractCodeOriginal && resOrder.data.contractCodeOriginal == resOrder.data.contractCode)
                };
                //
                if(this.orderDetail?.investor?.listInvestorIdentification) {
                    this.orderDetail.investor.listInvestorIdentification = this.orderDetail.investor.listInvestorIdentification.map(item => {
                        return {
                            ...item,
                            idNoInfo: item.idNo + `(${item.idType})`,
                        }
                    });
                }
                
                this.sale = this.orderDetail?.sale;
                this.initDistributionInfo(resOrder?.data.distributionId);
                //
                let listBank: any[] = [];
                // listBank khach
                if (this.orderDetail?.businessCustomer) {
                    listBank = this.orderDetail.businessCustomer?.businessCustomerBanks;
                    if (listBank?.length) {
                        this.listBank = listBank.map((bank) => {
                        bank.investorBankAccId = bank.businessCustomerBankAccId;
                        bank.labelName = bank.bankAccNo + " - " + bank.bankAccName + " - " + bank.bankName;
                        return bank;
                        });
                    }
                } else if (this.orderDetail?.investor) {
                    listBank = this.orderDetail.investor?.listBank;
                    if (listBank?.length) {
                        this.listBank = listBank.map((bank) => {
                        bank.investorBankAccId = bank.id;
                        bank.labelName = bank.coreBankName 
                                        + (bank.bankAccount ? " - " + bank.bankAccount : "") 
                                        + (bank.ownerAccount ? " - " + bank.ownerAccount : "");
                        return bank;
                        });
                    }
                }
            } else {
                this.isLoading = false;
            }
            },(err) => {
                this.isLoading = false;
            }
        );
    }
  
    initDistributionInfo(distributionId) {
        this.isLoading = true;
        this._distributionService.getById(distributionId).subscribe((res) => {
            this.isLoading = false;
            if (this.handleResponseInterceptor(res, "")) {
                this.distributionInfo = res?.data;
                this.projectInformation = res?.data.project;
                this.policies = res?.data.policies;

                if(this.policies?.length) {
                    let policyTypeValues = this.policies.map(p => p.type);
                    this.policyTypes = PolicyTemplateConst.types.filter(type => policyTypeValues.includes(type.code));
                    this.policyInfo = this.policies.find((item) => item.id == this.orderDetail.policyId);
                    //
                    this.policyType = this.policyInfo?.type;
                    //
                    this.policyDisplays = this.getPolicyDisplays(this.policyType);
                    //
                    this.policyDetails = this.policyInfo.policyDetail;
                    this.policyDetails = this.policyDetails.map((policyDetails) => {
                        policyDetails.periodQuantityPeriodType = policyDetails.periodQuantity + " " + PolicyDetailTemplateConst.getNameInterestPeriodType(policyDetails.periodType);
                        return policyDetails;
                    });
                    //
                    this.policyDetail = this.policyDetails.find((item) => item.id == this.orderDetail.policyDetailId);
                    this.profit = this.policyDetail.profit;
                }
            }
            },(err) => {
                this.isLoading = false;
                console.log("Error-------", err);
            }
        );
    }

    getPolicyDisplays(policyType) {
        let policyDisplays = this.policies.filter(p => p.type == policyType);
        policyDisplays = policyDisplays.map(policy => {
            policy.labelName = policy.code + ' - ' + policy.name;
            return policy;
        });
        return [...policyDisplays];
    }
}
