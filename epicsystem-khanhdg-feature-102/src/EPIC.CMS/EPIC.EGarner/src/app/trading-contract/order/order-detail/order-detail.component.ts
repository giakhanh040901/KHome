import { Component, Injector, Input, ViewChild,ElementRef } from '@angular/core';
import { OrderConst, PolicyDetailTemplateConst, ProductConst, PolicyTempConst, FormNotificationConst } from '@shared/AppConsts';
import { CrudComponentBase } from '@shared/crud-component-base';
import { MessageService } from 'primeng/api';
import { BreadcrumbService } from 'src/app/layout/breadcrumb/breadcrumb.service';
import { ActivatedRoute, Router } from '@angular/router';
import { OrderServiceProxy } from '@shared/service-proxies/trading-contract-service';
import { forkJoin } from 'rxjs';
import { DistributionService } from '@shared/services/distribution.service';
import { FilterSaleComponent } from '../create-order/order-filter-customer/filter-sale/filter-sale.component';
import { DialogService } from 'primeng/dynamicdialog';
import { SaleService } from '@shared/services/sale.service';
import { TabView } from 'primeng/tabview';
import { OBJECT_ORDER } from '@shared/base-object';
import { FormNotificationComponent } from 'src/app/form-general/form-notification/form-notification.component';
import { WithdrawalRequestComponent } from '../../contract-active/withdrawal-request/withdrawal-request.component';
import { SpinnerService } from '@shared/services/spinner.service';

@Component({
    selector: 'app-order-detail',
    templateUrl: './order-detail.component.html',
    styleUrls: ['./order-detail.component.scss']
})
export class OrderDetailComponent extends CrudComponentBase {

    constructor(
        injector: Injector,
        messageService: MessageService,
        private routeActive: ActivatedRoute,
        private dialogService: DialogService,
        private _saleService: SaleService,
        private _orderService: OrderServiceProxy,
        private breadcrumbService: BreadcrumbService,
		private router: Router,
        private _distributionService: DistributionService,
        public spinnerService: SpinnerService,
    ) {
        super(injector, messageService);
        this.breadcrumbService.setItems([
            { label: 'Trang chủ', routerLink: ['/home'] },
            { label: 'Sổ lệnh', routerLink: ['/trading-contract/order'] },
            { label: 'Chi tiết sổ lệnh', },
        ]);
        this.orderId = +this.cryptDecode(this.routeActive.snapshot.paramMap.get('id'));
        this.isJustView = this.routeActive.snapshot.paramMap.get("isJustView");        
    }

    /* Check isJustView */
    isJustView: string = null;

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
    //
    labelButtonEdit = "Chỉnh sửa";
    //
    fieldUpdates = {
        totalValue: {
            'isEdit': false,
            'apiPath': '/api/garner/order/update/total-value',
            'name': 'totalValue',
            'idHTML': 'totalValue'
        },

        policyDetailId: {
            'isEdit': false,
            'apiPath': '/api/garner/order/update/update-policy-detail',
            'name': 'policyDetailId',
            'messageRequired': 'Vui lòng chọn kỳ hạn!',
            'idHTML': 'policyDetailId'

        },

        policyId: {
            'isEdit': false,
            'apiPath': '/api/garner/order/update/update-policy',
            'name': 'policyId',
            'messageRequired': 'Vui lòng chọn chính sách!',
            'idHTML': 'policyId'
        },

        saleReferralCode: {
            'isEdit': false,
            'apiPath': '/api/garner/order/update/referral-code',
            'name': 'saleReferralCode',
            'messageRequired': 'Vui lòng nhập mã giới thiệu!',
            'idHTML': 'saleReferralCode'

        },

        infoCustomer: {
            isEdit: false,
            apiPath: "/api/garner/order/update/info-customer?",
            name: "infoCustomer",
            // messageRequired: "Vui lòng chọn tài khoản ngân hàng!",
            idHTML: "infoCustomer",
        },

        bankAccId: {
            'isEdit': false,
            'apiPath': '/api/garner/order/update/bank-account',
            'name': 'bankAccId',
            'messageRequired': 'Vui lòng chọn tài khoản ngân hàng!',
            'idHTML': 'bankAccId'        
        },
    };

    tabViewActive = {
		'thongTinChung': true,
		'thongTinThanhToan': false,
		'HSKHDangKy': false,
		'loiNhuan': false,
        'lichSu': false,
        'dongTien': false,
	};

    @ViewChild(TabView) tabView: TabView;

    PolicyTempConst = PolicyTempConst;
    ProductConst = ProductConst;
    OrderConst = OrderConst;
    TabView = TabView;
	
    ngOnInit() {
        this.isPartner = this.getIsPartner();
        this.isLoading = true;
        this.spinnerService.isShowSpinner();
        forkJoin([
            this._orderService.get(this.orderId), 
            this._distributionService.getDistributionsOrder()]).subscribe(([resOrder, resDistribution]) => {
            this.isLoading = false;
            this.spinnerService.remove();
            //===
            if (this.handleResponseInterceptor(resDistribution, '') && resDistribution?.data?.length) {
                this.distributions = resDistribution.data.map(distribution => {
                    distribution.labelName = ProductConst.getTypeName(distribution?.garnerProduct?.productType) + ' - ' + distribution?.garnerProduct?.name;
                    return distribution;
                });
            }
            //===
            if (this.handleResponseInterceptor(resOrder) && resOrder?.data) {
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

    changeTab(e) {
        let tabHeader = this.tabView.tabs[e.index].header;
		this.tabViewActive[tabHeader] = true;
    }

    getOrderDetail(isLoading = true) {
        this.isLoading = isLoading;
        this._orderService.get(this.orderId).subscribe((resOrder) => {
            if (this.handleResponseInterceptor(resOrder, '') && resOrder?.data) {
                this.handleDataOrderDetail(resOrder?.data);
            } else {
                this.isLoading = false;
            }
        }, (err) => {
            this.isLoading = false;
        });
    }

    handleDataOrderDetail(orderDetail) {
        this.orderDetail = {
            ...orderDetail,
            buyDate: orderDetail?.buyDate ? new Date(orderDetail?.buyDate) : null,
            paymentFullDate: orderDetail?.paymentFullDate ?? orderDetail?.buyDate,
            bankAccId: orderDetail?.investorBankAccId || orderDetail?.businessCustomerBankAccId,
        };
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

    getInterestment() {
        let product = this.distributionInfo?.garnerProduct;
        let totalInterestment = 0;
        let remainAmount = 0;
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
        return this.formatCurrency(totalInterestment);
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
        console.log('policies', this.policies);
        console.log('distributionInfo', this.distributionInfo);
    }

    changePolicy(policyId, setData = false) {
        if(this.policies?.length) this.policyInfo = {...this.policies.find(p => p.id == policyId)};
        //Reset data (Comment tạm)
        // if(!setData) { 
        //     this.policyDetailInfo = {};
        //     this.orderDetail.policyDetailId = null;
        //     this.profitPolicyDetail = null;
        // }
        // //        
        // this.policyDetails = [];
        // this.policyDetails = [...this.policyInfo?.policyDetails];
        // // CUSTOM HIỂN THỊ
        // this.policyDetails = this.policyDetails.map(pD => { pD.periodQuantityPeriodType = pD.periodQuantity + ' ' + PolicyDetailTemplateConst.getNameInterestPeriodType(pD.periodType);
        //     return pD;
        // });
        // console.log("policyDetails",this.policyDetails);
        // console.log("policy",this.policyInfo);
    }
  
    changePolicyDetail(policyDetailId) {
        this.profitPolicyDetail = this.policyDetails.find(item => item.id == policyDetailId)?.profit;
    }


    // Form yêu cầu rút vốn
	withdrawalRequest() {
		const ref = this.dialogService.open(
			WithdrawalRequestComponent,
			{
				header: "Tạo yêu cầu rút tích lũy",
				width: '500px',
				contentStyle: { "max-height": "600px", "overflow": "auto", "padding": 0, "padding-bottom": "50px" },
				style: { "overflow": "auto" },
				data: {
				orderDetail: this.orderDetail,
			}
		});

		ref.onClose.subscribe((res) => {
			if(res.status) {
				this.messageSuccess('Yêu cầu thành công');
				// this.setPage();
			}
		});
	}

    deleteOrder() {
        const ref = this.dialogService.open(
            FormNotificationComponent,
            {
                header: "Thông báo",
                width: '600px',
                contentStyle: { "max-height": "600px", "overflow": "auto", "padding-bottom": "50px" },
                styleClass: 'p-dialog-custom',
                baseZIndex: 10000,
                data: {
                    title: "Bạn có chắc chắn muốn xóa sổ lệnh này?",
                    icon: FormNotificationConst.IMAGE_CLOSE,
                },
            }
        );
        ref.onClose.subscribe((dataCallBack) => {
            console.log({ dataCallBack });
            if (dataCallBack?.accept) {
                this._orderService.delete([this.orderId]).subscribe((response) => {
                    if (this.handleResponseInterceptor(response,"Xóa sổ lệnh thành công")) {
                        this.router.navigate(["/trading-contract/order"]);
                    }
                }, (err) => {
                    console.log('err____', err);
                    this.messageError(`Không xóa được sổ lệnh`);
                });
            } else {
            }
        });
    }

    searchSale() {
        if(this.isEdit || this.fieldUpdates.saleReferralCode.isEdit) {
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
                    this.orderDetail.sale = {...sale}
                    this.orderDetail.saleReferralCode = sale?.referralCode;

                    console.log('saleInfo', sale);
                    
                }
            });
        }
    }
    //
    resetStatusFieldUpdates() {
        for (const [key, value] of Object.entries(this.fieldUpdates)) {
            this.fieldUpdates[key].isEdit = false;
        }
    }

    setStatusEdit() {
        this.isEdit = !this.isEdit;
        this.labelButtonEdit = this.isEdit ? "Lưu lại" : "Chỉnh sửa";
        // this.editorConfig.editable = this.isEdit;
    }

    getBody() {
        let body = {...this.filterField(this.orderDetail, this.orderUpdate)};
        console.log('body__', body, this.orderDetail);
        if(this.orderDetail.investorBankAccId) body.investorBankAccId = this.orderDetail.bankAccId;
        if(this.orderDetail.businessCustomerBankAccId) body.businessCustomerBankAccId = this.orderDetail.bankAccId;
        return body;
    }

    changeEdit() {
        if (this.isEdit) {
            let body = this.getBody();
            this._orderService.update(body).subscribe((response) => {
                if (this.handleResponseInterceptor(response, 'Cập nhật thành công')) {
                    console.log("order data ...", body);
                    this.setStatusEdit();
                } 
            });
        } else {
            this.setStatusEdit();
        }
    }

    resetStatusEditFieldUpdates() {
        for(const [key, value] of Object.entries(this.fieldUpdates)) {
            this.fieldUpdates[key].isEdit = false;
        }
    }

    updateField(field) {
        if (this.fieldUpdates[field].isEdit) {
            let body = this.getBody();
            if (this.orderDetail[this.fieldUpdates[field].name]) {
                this._orderService.updateField(body, this.fieldUpdates[field]).subscribe((response) => {
                    if (this.handleResponseInterceptor(response, 'Cập nhật thành công')) {
                        this.resetStatusFieldUpdates();
                        if(this.fieldUpdates[field].name == 'totalValue') {
                            // Cập nhật lại nếu có thay đổi về trạng thái order
                            this.tabViewActive.thongTinThanhToan = false;
                            this.getOrderDetail(true);
                        }
                        this.tabViewActive.lichSu = false;
                        this.tabViewActive.dongTien = false;
                    }
                });
            } else {
                this.messageError(this.fieldUpdates[field].messageRequired);
            }
        } else {
            if(this.fieldUpdates[field].idHTML != "bankAccId"){
                const focus = document.getElementById(this.fieldUpdates[field].idHTML);
                focus.scrollIntoView({behavior: 'smooth'});
            }
            this.resetStatusEditFieldUpdates();
            this.fieldUpdates[field].isEdit = true;
        }
    }

    approve() {
        this._orderService.approve(this.orderId).subscribe((res) => {
            if(this.handleResponseInterceptor(res, "Duyệt thành công")) {
                this.getOrderDetail(false);
            }
        });
    }

    updateInfoContactCustomer() {
        if (this.fieldUpdates.infoCustomer.isEdit) {
            
          let body = {
            orderId: this.orderDetail.id,
            customerBankAccId: this.orderDetail.bankAccId ?? null,  
            contractAddressId : this.orderDetail.contractAddressId ?? null,  
            investorIdenId: this.orderDetail.investorIdenId ?? null,  
          };
          //
          this._orderService.updateInfoContactCustomer(body).subscribe((response) => {
            if (this.handleResponseInterceptor(response, "Cập nhật thành công")) {
              this.resetStatusFieldUpdates();
              this.tabViewActive.lichSu = false;
              this.tabViewActive.dongTien = false;
            }
          });
        } else {
          this.fieldUpdates.infoCustomer.isEdit = true;
        }
      }
}


