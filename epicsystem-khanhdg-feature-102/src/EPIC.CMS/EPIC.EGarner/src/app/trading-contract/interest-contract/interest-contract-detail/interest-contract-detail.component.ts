import { Component, Injector, Input, ViewChild } from '@angular/core';
import { OrderConst, PolicyDetailTemplateConst } from '@shared/AppConsts';
import { CrudComponentBase } from '@shared/crud-component-base';
import { MessageService } from 'primeng/api';
import { BreadcrumbService } from 'src/app/layout/breadcrumb/breadcrumb.service';
import { ActivatedRoute } from '@angular/router';
import { OrderServiceProxy } from '@shared/service-proxies/trading-contract-service';
import { DistributionService } from '@shared/services/distribution.service';
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
    private dialogService: DialogService,
    private _saleService: SaleService,
    private _orderService: OrderServiceProxy,
    private breadcrumbService: BreadcrumbService,
    private _distributionService: DistributionService,
  ) {
    super(injector, messageService);

    this.breadcrumbService.setItems([
      { label: 'Trang chủ', routerLink: ['/home'] },
      { label: 'Sổ lệnh', routerLink: ['/trading-contract/order'] },
      { label: 'Chi tiết sổ lệnh', },
    ]);

    this.orderId = +this.cryptDecode(this.routeActive.snapshot.paramMap.get('id'));

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
  fieldDates = ['buyDate'];
  sale: any = {};

  OrderConst = OrderConst;

  isEdit = false;
  fieldErrors: any = {};

  orderId: number;
  orderDetail: any = {};
  labelButtonEdit = "Chỉnh sửa";

  isEditAll = false;
  distributions: any[] = [];
  //
  listBank: any[] = [];
  listAddress: any[] = [];
  //
  distributionInfo: any = {};
  projectInformation: any = {};

  tabViewActive = {
    'thongTinChung': true,
    'loiNhuan': false,
  };

  @ViewChild(TabView) tabView: TabView;

  ngOnInit() {
    this.init();
    this.isLoading = true;

    this._distributionService.getDistributionsOrder().subscribe((res) => {
      if (this.handleResponseInterceptor(res, '')) {
        this.isLoadingPage = false;
        this.isLoading = false;
        this.distributions = res?.data;
      }
    }, () => {
      this.isLoading = false;
      this.isLoadingPage = false;
    });
  }

  changeProject(distribution) {
    console.log("distributionInfo...", distribution);

    // this.distributionInfo = this.distributions.find( item => item.id == distribution.id );
    this.policies = distribution?.policies;
    this.orderDetail.projectId = distribution?.projectId;
    this.orderDetail.distributionId = distribution?.id;

    this.projectInformation = distribution?.project;
    console.log("policies...", this.policies);
  }

  changeTab(e) {
    let tabHeader = this.tabView.tabs[e.index].header;
    this.tabViewActive[tabHeader] = true;
  }

  init() {
    this.isLoadingPage = true;
    this._orderService.get(this.orderId).subscribe((resOrder) => {
      if (this.handleResponseInterceptor(resOrder, '')) {

        // this.initSale(resOrder?.data?.saleReferralCode);
        this.orderDetail = {
          ...resOrder.data,
          buyDate: resOrder.data?.buyDate ? new Date(resOrder.data?.buyDate) : null,
          paymentFullDate: resOrder.data?.paymentFullDate ?? resOrder.data?.buyDate,
        };
        this.sale = this.orderDetail?.sale;
        console.log("orderDetail,");

        console.log("resOrder...", this.orderDetail);
        this.initDistributionInfo(resOrder?.data.distributionId);
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
              bank.labelName = bank.bankAccNo + ' - ' + bank.bankAccName + ' - ' + bank.bankName;
              return bank;
            });
          }
        } else
          if (this.orderDetail?.investor) {
            listBank = this.orderDetail.investor?.listBank;
            if (listBank?.length) {
              this.listBank = listBank.map(bank => {
                bank.investorBankAccId = bank.id;
                bank.labelName = bank.coreBankName + (bank.bankAccount ? ' - ' + bank.bankAccount : '') + (bank.ownerAccount ? (' - ' + bank.ownerAccount) : '');
                return bank;
              });
            }
          }
        this.isLoadingPage = false;

      } else {
        this.isLoadingPage = false;
      }
    });
  }

  initDistributionInfo(distributionId) {
    this._distributionService.getById(distributionId).subscribe((res) => {
      if (this.handleResponseInterceptor(res, '')) {

        this.distributionInfo = res?.data;
        this.projectInformation = res?.data.project;
        this.policies = res?.data.policies;
        this.policyInfo = this.policies.find(item => item.id == this.orderDetail.policyId);

        this.policyDetails = this.policyInfo.policyDetail
        this.policyDetails = this.policyDetails.map(policyDetails => {
          policyDetails.periodQuantityPeriodType = policyDetails.periodQuantity + ' ' + PolicyDetailTemplateConst.getNameInterestPeriodType(policyDetails.periodType);
          return policyDetails;
        });
        console.log("policyDetailsaaaaaa", this.policyDetails);

        this.policyDetail = this.policyDetails.find(item => item.id == this.orderDetail.policyDetailId);
        this.profit = this.policyDetail.profit;
      }
    }, () => {
      this.isLoading = false;
      this.isLoadingPage = false;
    });
  }

  changePolicy(policy) {
    this.orderDetail.policyId = policy.id;

    this.policyInfo = this.policies.find(item => item.id == policy.id);
    this.policyDetails = policy.policyDetail;
    this.profit = null;
  }

  changePolicyDetail(policyDetailId) {
    this.orderDetail.policyDetailId = policyDetailId;
    this.policyDetail = this.policyDetails.find(item => item.id == policyDetailId);
    this.profit = this.policyDetail.profit;
  }
  
}
