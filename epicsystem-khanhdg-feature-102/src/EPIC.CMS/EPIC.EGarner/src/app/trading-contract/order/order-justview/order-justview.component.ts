import { Component, Injector, OnInit, ViewChild } from "@angular/core";
import { ActivatedRoute, Router } from "@angular/router";
import { OrderConst, PolicyTempConst, ProductConst } from "@shared/AppConsts";
import { OBJECT_ORDER } from "@shared/base-object";
import { CrudComponentBase } from "@shared/crud-component-base";
import { OrderServiceProxy } from "@shared/service-proxies/trading-contract-service";
import { DistributionService } from "@shared/services/distribution.service";
import { MessageService } from "primeng/api";
import { DialogService } from "primeng/dynamicdialog";
import { TabView } from "primeng/tabview";
import { forkJoin } from "rxjs";
import { BreadcrumbService } from "src/app/layout/breadcrumb/breadcrumb.service";

@Component({
  selector: "app-order-justview",
  templateUrl: "./order-justview.component.html",
  styleUrls: ["./order-justview.component.scss"],
})
export class OrderJustviewComponent extends CrudComponentBase {
  constructor(
    injector: Injector,
    messageService: MessageService,
    private routeActive: ActivatedRoute,
    private dialogService: DialogService,
    private _orderService: OrderServiceProxy,
    private breadcrumbService: BreadcrumbService,
    private router: Router,
    private _distributionService: DistributionService
  ) {
		super(injector, messageService);
		this.breadcrumbService.setItems([
			{ label: 'Trang chủ', routerLink: ['/home'] },
			{ label: 'Sổ lệnh', routerLink: ['/trading-contract/order'] },
			{ label: 'Chi tiết sổ lệnh', },
		]);
		this.orderId = +this.cryptDecode(this.routeActive.snapshot.paramMap.get('id'));
        this.typeView = this.routeActive.snapshot.paramMap.get("typeView");        

  	}
  
    /* Check isJustView */
    typeView: string;

    orderUpdate = {...OBJECT_ORDER.UPDATE};
    //
    orderDetail: any = {};
    orderId: number;
    //
    distributions = [];
    policies = [];
    policyDetails = [];
    listBank = [];
    //
    distributionInfo: any = {};
    policyInfo: any = {};
    policyDetailInfo: any = {}
    //
    profitPolicyDetail: number;
    totalInvestment = 0;

    isEdit = false;
    isEditAll = false;

	tabViewActive = {
		'thongTinChung': true,
		'thongTinThanhToan': false,
		'HSKHDangKy': false,
		'loiNhuan': false,
        'lichSu': false,
	};

    @ViewChild(TabView) tabView: TabView;

    PolicyTempConst = PolicyTempConst;
    ProductConst = ProductConst;
    OrderConst = OrderConst;
    TabView = TabView;
  	
	ngOnInit(): void {
		this.isLoading = true;
        forkJoin([
            this._orderService.get(this.orderId), 
            this._distributionService.getDistributionsOrder()]).subscribe(([resOrder, resDistribution]) => {
            this.isLoading = false;
            //===
            if (this.handleResponseInterceptor(resDistribution, '') && resDistribution?.data?.length) {
                this.distributions = resDistribution.data.map(distribution => {
                    distribution.labelName = ProductConst.getTypeName(distribution?.garnerProduct?.productType) + ' - ' + distribution?.garnerProduct?.name;
                    return distribution;
                });
            }
            //===
            if (this.handleResponseInterceptor(resOrder, '') && resOrder?.data) {
                this.handleDataOrderDetail(resOrder?.data);
                if(this.orderDetail?.investor?.listInvestorIdentification) {
                    this.orderDetail.investor.listInvestorIdentification = this.orderDetail.investor.listInvestorIdentification.map(item => {
                      item.idNoInfo = item.idNo + `(${item.idType})`;
                      return item
                    });
                  }
            }
        }, (err) => {
            this.isLoading = false;
            console.log('Error-------', err);
        });
	}

	handleDataOrderDetail(orderDetail) {
        this.orderDetail = {
            ...orderDetail,
            buyDate: orderDetail?.buyDate ? new Date(orderDetail?.buyDate) : null,
            paymentFullDate: orderDetail?.paymentFullDate ?? orderDetail?.buyDate,
            bankAccId: orderDetail?.investorBankAccId || orderDetail?.businessCustomerBankAccId,
        };
        console.log("resOrder...", this.orderDetail);
        //  SET GIÁ TRỊ CHO SẢN PHẨM, CHÍNH SÁCH, KỲ HẠN
        this.changeDistribution(this.orderDetail?.distributionId, true);
        this.changePolicy(this.orderDetail?.policyId, true);
        // this.changePolicyDetail(this.orderDetail?.policyDetailId);

        let listBank: any[] = [];
        // listBank khach
        if (this.orderDetail?.businessCustomer) {
            listBank = this.orderDetail.businessCustomer?.businessCustomerBanks;
            if (listBank?.length) {
                this.listBank = listBank.map(bank => {
                    bank.labelName = bank.bankAccNo + ' - ' + bank.bankAccName + ' - ' + bank.bankName;
                    return bank;
                });
            }
        } 
        else if (this.orderDetail?.investor) {
            listBank = this.orderDetail.investor?.listBank;
            if (listBank?.length) {
                this.listBank = listBank.map(bank => {
                    bank.labelName = bank.coreBankName + (bank.bankAccount ? ' - ' + bank.bankAccount : '') + (bank.ownerAccount ? (' - ' + bank.ownerAccount) : '');
                    return bank;
                });
            }
        }
    }

    changeTab(e) {
        let tabHeader = this.tabView.tabs[e.index].header;
		this.tabViewActive[tabHeader] = true;
    }

	changeDistribution(distributionId, setData = false) {
        this.distributionInfo = {...this.distributions.find( item => item.id == distributionId)};
        this.orderDetail.productId = this.distributionInfo?.garnerProduct?.id;
        //Reset data
        if(!setData) {
            this.policyInfo = {};
            this.policyDetailInfo = {};
            this.orderDetail.policyDetailId = null;
            this.orderDetail.policyId = null;
            this.profitPolicyDetail = null;
        }
        //
        if(this.distributionInfo) {
          this.policies = this.distributionInfo?.policies;
        }
    }

    changePolicy(policyId, setData = false) {
        if(this.policies?.length) this.policyInfo = {...this.policies.find(p => p.id == policyId)};
    }

	getInterestment() {
        let product = this.distributionInfo?.garnerProduct;
        let totalInterestment = 0;
        let remainAmount = 0;
        //
        if(product?.productType == ProductConst.INVEST) {
            totalInterestment = product?.invTotalInvestment;
            remainAmount = totalInterestment - +this.distributionInfo?.isInvested;
        } 
        //
        else if(product?.productType == ProductConst.SHARE) {
            totalInterestment = product?.cpsParValue * product?.cpsQuantity;
            remainAmount = totalInterestment - +this.distributionInfo?.isInvested;
        }
        //
        this.distributionInfo.remainAmount = remainAmount;
        this.totalInvestment = totalInterestment;
        //
        return this.formatCurrency(totalInterestment);
    }
}
