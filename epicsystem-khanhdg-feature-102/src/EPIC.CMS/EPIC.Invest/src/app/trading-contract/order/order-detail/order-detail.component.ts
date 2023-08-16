import {Component, Injector, Input, ViewChild, ElementRef } from "@angular/core";
import { ProductBondPrimaryConst, ProductBondInfoConst, ProductPolicyConst, OrderConst, PolicyDetailTemplateConst, FormNotificationConst, PolicyTemplateConst, InvestorConst } from "@shared/AppConsts";
import { CrudComponentBase } from "@shared/crud-component-base";
import { MessageService } from "primeng/api";
import { BreadcrumbService } from "src/app/layout/breadcrumb/breadcrumb.service";
import { ActivatedRoute, Router } from "@angular/router";
import { DistributionService } from "@shared/services/distribution.service";
import { FilterSaleComponent } from "../create-order/order-filter-customer/filter-sale/filter-sale.component";
import { DialogService } from "primeng/dynamicdialog";
import { TabView } from "primeng/tabview";
import { FormNotificationComponent } from "src/app/form-general/form-notification/form-notification.component";
import { OrderService } from "@shared/services/order.service";
import { TabViewOrderDetail } from "@shared/interface/tabview.model";
  
  @Component({
    selector: "app-order-detail",
    templateUrl: "./order-detail.component.html",
    styleUrls: ["./order-detail.component.scss"],
  })
  
  export class OrderDetailComponent extends CrudComponentBase {
    constructor(
        injector: Injector,
        messageService: MessageService,
        private router: Router,
        private routeActive: ActivatedRoute,
        private dialogService: DialogService,
        private _orderService: OrderService,
        private breadcrumbService: BreadcrumbService,
        private _distributionService: DistributionService,
        private _dialogService: DialogService,
  
    ) {
        super(injector, messageService);
        this.orderId = +this.cryptDecode(this.routeActive.snapshot.paramMap.get('id'));
        this.breadcrumbService.setItems([
            { label: 'Trang chủ', routerLink: ['/home'] },
            { label: 'Sổ lệnh', routerLink: ['/trading-contract/order'] },
            { label: 'Chi tiết sổ lệnh' },
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

    fieldDates = ["buyDate"];
    sale: any = {};

    policyTypes = [];
    policyDisplays = [];
    policyType: number;

    ProductBondPrimaryConst = ProductBondPrimaryConst;
    ProductBondInfoConst = ProductBondInfoConst;
    ProductPolicyConst = ProductPolicyConst;
    InvestorConst =InvestorConst
    OrderConst = OrderConst;

    listIdNo : any;
  
    isEdit = false;
    fieldErrors: any = {};
  
    orderId: number;
    orderDetail: any = {};
    labelButtonEdit: string = "Chỉnh sửa";
  
    distributions: any[] = [];
    //
    listBank: any[] = [];
    listAddress: any[] = [];
    //
    distributionInfo: any = {};
    projectInformation: any = {};
    //
    fieldUpdates = {
      totalValue: {
        isEdit: false,
        apiPath: "/api/invest/order/update/total-value?",
        name: "totalValue",
        messageRequired: "",
        idHTML: "totalValue",
      },

      policyDetailId: {
        isEdit: false,
        apiPath: "/api/invest/order/update/update-policy-detail?",
        name: "policyDetailId",
        messageRequired: "Vui lòng chọn kỳ hạn!",
        idHTML: "policyDetailId",
      },

      referralCode: {
        isEdit: false,
        apiPath: "/api/invest/order/update/referral-code?",
        name: "saleReferralCode",
        messageRequired: "",
        idHTML: "referralCode",
      },

      infoCustomer: {
        isEdit: false,
        apiPath: "/api/invest/order/update/info-customer?",
        name: "infoCustomer",
        messageRequired: "",
        idHTML: "infoCustomer",
      },

      investorInfo: {
        isEdit: false,
        apiPath: "/api/invest/order/update/info-customer?",
        name: "investorIdenId",
        messageRequired: "Vui lòng chọn tài khoản ngân hàng!",
        idHTML: "investorInfo",
      },
    };

    //
    orderUpdate = {
      "id": 0,
      "policyId": null,
      "distributionId": null,
      "policyDetailId": null,
      "investorBankAccId": null,
      "contractAddressId": null,
      "cifCode": null,
      "totalValue": null,
      "isInterest": null,
      "saleReferralCode": null,
    }
  
    tabViewActive: TabViewOrderDetail = new TabViewOrderDetail();

    @Input() contentHeights: number[] = [];
  
    @ViewChild(TabView) tabView: TabView;

    ngOnInit() {
        this.isPartner = this.getIsPartner();
        this.init();

        this.isLoading = true;
        this._distributionService.getDistributionsOrder().subscribe((res) => {
            if (this.handleResponseInterceptor(res, "")) {
                this.isLoading = false;
                this.distributions = res?.data;
            }
        }, (err) => {
            this.isLoading = false;
            console.log("Error-------", err);
            }
        );
    }

    changeProject(distribution) {
        // this.distributionInfo = this.distributions.find( item => item.id == distribution.id );
        this.policies = distribution?.policies;
        this.orderDetail.projectId = distribution?.projectId;
        this.orderDetail.distributionId = distribution?.id;
        this.projectInformation = distribution?.project;
    }
  
    openThongTinChung: boolean = true;
    changeTab(e) {
        let tabHeader = this.tabView.tabs[e.index].header;
		this.openThongTinChung = (tabHeader == 'thongTinChung');
		if(!this.tabViewActive[tabHeader]) {
			this.tabViewActive[tabHeader] = true;
		}
    }
  
    init(isLoading: boolean = true) {
        this.isLoading = isLoading;
        this._orderService.get(this.orderId).subscribe((resOrder) => {
            this.isLoading = false;
            if (this.handleResponseInterceptor(resOrder, "")) {
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
            } 
        },(err) => {
            this.isLoading = false;
        });
    }
  
    searchSale() {
        this.dialogService.open(FilterSaleComponent, {
            header: "Tìm kiếm sale",
            width: "1000px",
            styleClass: "p-dialog-custom filter-business-customer customModal",
            contentStyle: { "max-height": "600px", overflow: "auto" },
            style: { top: "-15%", overflow: "hidden" },
        }).onClose.subscribe((sale) => {
            if (sale) {
                this.sale = sale;
                this.orderDetail.saleReferralCode = this.sale?.referralCode;
                this.orderDetail.referralCode = this.sale?.referralCode;
            }
        });
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
        });
    }
  
    resetStatusFieldUpdates() {
        for (const [key, value] of Object.entries(this.fieldUpdates)) {
            this.fieldUpdates[key].isEdit = false;
        }
    }
  
    setStatusEdit() {
        this.isEdit = !this.isEdit;
        this.labelButtonEdit = this.isEdit ? "Lưu lại" : "Chỉnh sửa";
        this.editorConfig.editable = this.isEdit;
    }

    changePolicyType(value) {
      // RESET DATA
      this.orderDetail.policyId = null;
      this.orderDetail.policyDetailId = null;
      this.profit = null;
      this.policyDisplays = this.getPolicyDisplays(value);
    }

    getPolicyDisplays(policyType) {
        let policyDisplays = this.policies.filter(p => p.type == policyType);
        policyDisplays = policyDisplays.map(policy => {
            policy.labelName = policy.code + ' - ' + policy.name;
            return policy;
        });
        return [...policyDisplays];
    }
  
    changePolicy(policy) {
      this.orderDetail.policyDetailId = null;
      this.orderDetail.policyId = policy.id;
      this.policyInfo = this.policies.find((item) => item.id == policy.id);
      this.policyDetails = this.policyInfo.policyDetail;
      this.profit = null;
    }
  
    changePolicyDetail(policyDetailId) {
        this.orderDetail.policyDetailId = policyDetailId;
        this.policyDetail = this.policyDetails.find((item) => item.id == policyDetailId);
        this.profit = this.policyDetail.profit;
    }

    removeReferralCode() {
        this.sale = null
        this.orderDetail.saleReferralCode = null;
        this.orderDetail.departmentName = null; 
        this.orderDetail.managerDepartmentName = null;
    }
  
    changeEdit() {
        if (this.isEdit) {
            let body = this.formatCalendar(this.fieldDates, {...this.filterField(this.orderDetail, this.orderUpdate)});
            this._orderService.update(body).subscribe((response) => {
                if (this.handleResponseInterceptor(response, "Cập nhật thành công")) {
                    this.init(true);
                    this.setStatusEdit();
                }
            });
        } else {
            this.setStatusEdit();
        }
    }
  
    deleteOrder() {
        this._dialogService.open(FormNotificationComponent, {
            header: "Xóa sổ lệnh",
            width: "600px",
            data: {
                title: "Bạn có chắc chắn xóa sổ lệnh này?",
                icon: FormNotificationConst.IMAGE_CLOSE,
            },
        }).onClose.subscribe((dataCallBack) => {
            if (dataCallBack?.accept) {
                this._orderService.delete([this.orderDetail?.id]).subscribe((response) => {
                    if (this.handleResponseInterceptor(response,"Xóa sổ lệnh thành công")) {
                        this.router.navigate([OrderConst.getBackLink(this.orderDetail?.status)]);
                    }
                });
            }
        });
    }
  
    updateField(field) {
        if (this.fieldUpdates[field].isEdit) {
            if (this.orderDetail[this.fieldUpdates[field].name] || !this.fieldUpdates[field].messageRequired) {
                this._orderService.updateField(this.orderDetail, this.fieldUpdates[field]).subscribe((response) => {
                    if (this.handleResponseInterceptor(response, "Cập nhật thành công")) {
                        this.init(true);
                        this.resetStatusFieldUpdates();
                        // Cập nhật lại nếu có thay đổi về trạng thái order
                        if (this.fieldUpdates[field] == "totalValue") {
                        this.tabViewActive.thongTinThanhToan = false;
                        }
                        this.tabViewActive.lichSu = false;
                    }
                });
            } else {
                this.messageError(this.fieldUpdates[field].messageRequired);
            }
        } else {
            const focus = document.getElementById(this.fieldUpdates[field].idHTML);
            focus.scrollIntoView({behavior: "smooth"});
            //
            this.resetStatusFieldUpdates();
            this.fieldUpdates[field].isEdit = true;
        }
    }

    updateInfoContactCustomer() {
        if (this.fieldUpdates.infoCustomer.isEdit) {
            let body = {
                orderId: this.orderDetail.id,
                investorBankAccId: this.orderDetail.investorBankAccId,  
                contractAddressId : this.orderDetail.contractAddressId,  
                investorIdenId: this.orderDetail.investorIdenId,  
            };
            //
            this._orderService.updateInfoContactCustomer(body).subscribe((response) => {
                if (this.handleResponseInterceptor(response, "Cập nhật thành công")) {
                    this.resetStatusFieldUpdates();
                    this.tabViewActive.lichSu = false;
                }
            });
        } else {
            this.fieldUpdates.infoCustomer.isEdit = true;
        }
    }
  }
  