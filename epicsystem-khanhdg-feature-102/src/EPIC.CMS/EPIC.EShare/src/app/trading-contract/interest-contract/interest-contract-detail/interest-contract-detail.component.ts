import { Component, Injector, Input, ViewChild } from '@angular/core';
import { CompanySharePrimaryConst, CompanyShareInfoConst, ProductPolicyConst, OrderConst } from '@shared/AppConsts';
import { CrudComponentBase } from '@shared/crud-component-base';
import { CompanyShareSecondaryServiceProxy } from '@shared/service-proxies/company-share-manager-service';
import { MessageService } from 'primeng/api';
import { BreadcrumbService } from 'src/app/layout/breadcrumb/breadcrumb.service';
import { ActivatedRoute } from '@angular/router';
import { OrderServiceProxy } from '@shared/service-proxies/trading-contract-service';
import * as moment from 'moment';
import { DialogService } from 'primeng/dynamicdialog';
import { SaleService } from '@shared/services/sale.service';
import { TabView } from 'primeng/tabview';


@Component({
  selector: 'app-interest-contract-detail',
  templateUrl: './interest-contract-detail.component.html',
  styleUrls: ['./interest-contract-detail.component.scss']
})
export class InterestContractDetailComponent extends CrudComponentBase {

  constructor(
    injector: Injector,
    messageService: MessageService,
    private routeActive: ActivatedRoute,
    private _orderService: OrderServiceProxy,
    private breadcrumbService: BreadcrumbService,
  ) {
    super(injector, messageService);

    this.orderId = +this.cryptDecode(this.routeActive.snapshot.paramMap.get('id'));
  }

  orderInformation: any;
  customerInformation: any = {};

  companyShareSecondaryInformation: any = {};
  companyShareSecondary: any = {};
  companyShareInfos: any[] = [];
  policies: any[] = [];
  policyInfo: any = {};
  policyDetails: any[] = [];
  policyDetail: any = {}
  profit: number = null;
  orderData: any = {};
  fieldDates = ['buyDate'];
  sale: any = {};

  investorSale: any = {}
  OrderStatusTitle: string;

  ProductPolicyConst = ProductPolicyConst;
  OrderConst = OrderConst;

  isEdit = false;
  fieldErrors: any = {};

  orderId: number;
  orderDetail: any = {};
  labelButtonEdit = "Chỉnh sửa";

  isEditAll = false;

  listBank: any[] = [];
  listAddress: any[] = [];

  listContactAddress: any[] = [];

  tabViewActive = {
    'thongTinChung': true,
    'loiTuc': false,
    'traiTuc': false,
  };

  @ViewChild(TabView) tabView: TabView;

  CompanyShareInfoConst = CompanyShareInfoConst;
  CompanySharePrimaryConst = CompanySharePrimaryConst;

  ngOnInit() {
    this.init();
  }

  changeTab(e) {
    let tabHeader = this.tabView.tabs[e.index].header;
    this.tabViewActive[tabHeader] = true;
  }

  init() {
    this.isLoadingPage = true;
    this._orderService.get(this.orderId).subscribe((resOrder) => {
      if (this.handleResponseInterceptor(resOrder, '')) {
        this.listContactAddress = resOrder?.data.listContactAddress;
        this._orderService.getPriceDate(resOrder.data.companyShareSecondaryId, resOrder.data.paymentFullDate ?? new Date()).subscribe((resPrice) => {
          if (this.handleResponseInterceptor(resPrice, '')) {
            this.orderDetail = {
              ...resOrder.data,
              buyDate: resOrder.data?.buyDate ? new Date(resOrder.data?.buyDate) : null,
              paymentFullDate: resOrder.data?.paymentFullDate ?? resOrder.data?.buyDate,
              referralCode: resOrder.data?.saleReferralCode,
            };
            this.sale = this.orderDetail?.sale;
            //set field edit for permission
            if (this.orderDetail.status < this.OrderConst.CHO_KY_HOP_DONG) {
              this.isEditAll = true;
            }
            //
            let listBank: any[] = [];
            // listBank khach
            if (this.orderDetail?.businessCustomer) {
              listBank = this.orderDetail.businessCustomer?.businessCustomerBanks;
              if (listBank?.length) {
                this.listBank = listBank.map(bank => {
                  bank.investorBankAccId = bank.businessCustomerBankId;
                  bank.labelName = bank.bankName + ' - ' + bank.bankAccNo + ' - ' + bank.bankAccName;
                  return bank;
                });
              }
            } else
              if (this.orderDetail?.investor) {
                listBank = this.orderDetail.investor?.listBank;
                if (listBank?.length) {
                  this.listBank = listBank.map(bank => {
                    bank.investorBankAccId = bank.id;
                    bank.labelName = (bank.coreBankName ? bank.coreBankName + ' - ' : '') + (bank.bankAccount ? + bank.bankAccount + ' - ' : '') + (bank.ownerAccount ? bank.ownerAccount : '');
                    return bank;
                  });
                }
              }

            //
            this.companyShareSecondary = resOrder?.data?.companyShareSecondary;
            this.companyShareInfos.push({
              companyShareName: resOrder?.data?.companyShareInfo.companyShareName,
              companyShareCode: resOrder?.data?.companyShareInfo.companyShareCode,
              companyShareSecondaryId: resOrder?.data?.companyShareSecondaryId,
            });
            //
            this.policies = this.companyShareSecondary?.policies ?? [];
            this.policyInfo = this.policies.find(item => item.companySharePolicyId == this.orderDetail.companySharePolicyId);

            this.policyDetails = this.policyInfo?.details ?? [];
            this.policyDetail = this.policyDetails.find(item => item.companySharePolicyDetailId == this.orderDetail?.companySharePolicyDetailId);
            this.profit = this.policyDetail.profit;
            if (this.policyDetail) {
              let convertUnitMoment = {
                D: 'd',
                M: 'M',
                Y: 'y',
              }
              if (this.orderDetail?.paymentFullDate) {
                this.orderDetail.dueDate = new Date(moment(this.orderDetail.paymentFullDate).add((this.policyDetail.interestDays || this.policyDetail.periodQuantity), (this.policyDetail.interestDays ? 'd' : convertUnitMoment[this.policyDetail.periodType])).format("YYYY-MM-DD"));
              }
            }

            if (resPrice.data?.price) {
              this.orderDetail.buyPrice = this.orderDetail.buyPrice ?? resPrice.data?.price;
              this.orderDetail.orderQuantity = Math.floor(this.orderDetail.totalValue / resPrice.data?.price);
              this.orderDetail.orderPrice = this.orderDetail.totalValue / this.orderDetail.orderQuantity;
            }
            // this.countQuantity();
            // this.resetDataSelected();
          }
          //
          this.isLoadingPage = false;
        });
      } else {
        this.isLoadingPage = false;
      }
    });
  }

  changePolicy(companySharePolicyId) {
    let policy = this.policies.find(item => item.companySharePolicyId == companySharePolicyId);
    this.orderDetail.dueDate = null;
    this.policyInfo = policy;
    this.policyDetails = policy.details;
    this.profit = null;
  }

  setDueDate(companySharePolicyDetailId) {
    this.policyDetail = this.policyDetails.find(item => item.companySharePolicyDetailId == companySharePolicyDetailId);
    this.profit = this.policyDetail.profit;
    console.log('policyDetail', this.policyDetail);

    let convertUnitMoment = {
      D: 'd',
      M: 'M',
      Y: 'y',
    }
    if (this.orderDetail?.paymentFullDate) {
      this.orderDetail.dueDate = new Date(moment(this.orderDetail.paymentFullDate).add((this.policyDetail.interestDays || this.policyDetail.periodQuantity), (this.policyDetail.interestDays ? 'd' : convertUnitMoment[this.policyDetail.periodType])).format("YYYY-MM-DD"));
    }
  }

  changeContractAddress(contractAddressId) {
    this.orderDetail.contractAddressId = contractAddressId;
  }
}
