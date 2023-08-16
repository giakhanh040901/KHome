import { Component, Injector, Input, ViewChild } from '@angular/core';
import { ProductBondPrimaryConst, ProductBondInfoConst, ProductPolicyConst, OrderConst } from '@shared/AppConsts';
import { CrudComponentBase } from '@shared/crud-component-base';
import { ProductBondInfoServiceProxy, ProductBondPrimaryServiceProxy, ProductBondSecondaryServiceProxy } from '@shared/service-proxies/bond-manager-service';
import { MessageService } from 'primeng/api';
import { BreadcrumbService } from 'src/app/layout/breadcrumb/breadcrumb.service';
import { ActivatedRoute } from '@angular/router';
import { AngularEditorConfig } from '@kolkov/angular-editor';
import { OrderServiceProxy } from '@shared/service-proxies/trading-contract-service';
import { forkJoin } from 'rxjs';
import * as moment from 'moment';
import { DialogService } from 'primeng/dynamicdialog';
import { FilterSaleComponent } from '../create-order/order-filter-customer/filter-sale/filter-sale.component';
import { SaleService } from '@shared/services/sale.service';
import { TabView } from 'primeng/tabview';

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
        private _productBondSecondary: ProductBondSecondaryServiceProxy,
        private breadcrumbService: BreadcrumbService,
    ) {
        super(injector, messageService);

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
    policyDetail: any = {}
    profit:number = null;
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

    fieldUpdates = {
        totalValue: {
            'isEdit': false,
            'apiPath': '/api/bond/order/update/total-value?',
            'name': 'totalValue',
        },
        policy: {
            'isEdit': false,
            'apiPath': '/api/bond/order/update/update-policy-detail?',
            'name': 'bondPolicyDetailId',
            'messageRequired': 'Vui lòng chọn kỳ hạn!',
        },
        referralCode: {
            'isEdit': false,
            'apiPath': '/api/bond/order/update/referral-code?',
            'name': 'saleReferralCode',
            'messageRequired': 'Vui lòng chọn mã giới thiệu!',
        },
        bankAccount: {
            'isEdit': false,
            'apiPath': '/api/bond/order/update/bank-account?',
            'name': 'investorBankAccId',
            'messageRequired': 'Vui lòng chọn tài khoản ngân hàng!',
        },
    };
    //

    tabViewActive = {
		'thongTinChung': true,
		'thongTinThanhToan': false,
		'HSKHDangKy': false,
		'loiTuc': false,
		'traiTuc': false,
	};
	
	@ViewChild(TabView) tabView: TabView;

    ProductBondInfoConst = ProductBondInfoConst;
    ProductBondPrimaryConst = ProductBondPrimaryConst;

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
                const OrderStatus = resOrder.data.status;
                switch (OrderStatus) {
                    case 1:
                    case 2:
                    case 3:
                        this.breadcrumbService.setItems([
                            { label: 'Trang chủ', routerLink: ['/home'] },
                            { label: 'Sổ lệnh', routerLink: ['/trading-contract/order'] },
                            { label: 'Chi tiết sổ lệnh', },
                        ]);
                        this.OrderStatusTitle = "Chi tiết sổ lệnh";
                        break;
                    case 4:
                        this.breadcrumbService.setItems([
                            { label: 'Trang chủ', routerLink: ['/home'] },
                            { label: 'Xử lý hợp đồng', routerLink: ['/trading-contract/contract-processing'] },
                            { label: 'Chi tiết xử lý hợp đồng', },
                        ]);
                        this.OrderStatusTitle = "Chi tiết xử lý hợp đồng";
                        break;
                    case 5:
                        this.breadcrumbService.setItems([
                            { label: 'Trang chủ', routerLink: ['/home'] },
                            { label: 'Hợp đồng', routerLink: ['/trading-contract/contract-active'] },
                            { label: 'Chi tiết hợp đồng', },
                        ]);
                        this.OrderStatusTitle = "Chi tiết hợp đồng";
                        break;
                }
                //
                this._orderService.getPriceDate(resOrder.data.bondSecondaryId, resOrder.data.paymentFullDate ?? new Date()).subscribe((resPrice) => {
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
                                    bank.labelName = bank.bankName + ' - ' + bank.bankAccNo + ' - ' + bank.bankAccName ;
                                    return bank;
                                });
                            }
                        } else
                            if (this.orderDetail?.investor) {
                                listBank = this.orderDetail.investor?.listBank;
                                if (listBank?.length) {
                                    this.listBank = listBank.map(bank => {
                                        bank.investorBankAccId = bank.id;
                                        bank.labelName = (bank.coreBankName ? bank.coreBankName + ' - ': '' ) +  (bank.bankAccount ? + bank.bankAccount + ' - ' : '') + (bank.ownerAccount ? bank.ownerAccount : '');
                                        return bank;
                                    });
                                }
                            }

                        //
                        this.bondSecondary = resOrder?.data?.bondSecondary;
                        this.bondInfos.push({
                            bondName: resOrder?.data?.bondInfo.bondName,
                            bondCode: resOrder?.data?.bondInfo.bondCode,
                            bondSecondaryId: resOrder?.data?.bondSecondaryId,
                        });
                        //
                        this.policies = this.bondSecondary?.policies ?? [];
                        this.policyInfo = this.policies.find(item => item.bondPolicyId == this.orderDetail.bondPolicyId);

                        this.policyDetails = this.policyInfo?.details ?? [];
                        this.policyDetail = this.policyDetails.find(item => item.bondPolicyDetailId == this.orderDetail?.bondPolicyDetailId);
                        this.profit = this.policyDetail?.profit;
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
                this.orderDetail.referralCode = this.sale?.referralCode;
                this.orderDetail.saleReferralCode = this.sale?.referralCode;
            }
        });
    }

    resetStatusFieldUpdates() {
        for (const [key, value] of Object.entries(this.fieldUpdates)) {
            this.fieldUpdates[key].isEdit = false;
        }
    }

    setFieldError() {
        for (const [key, value] of Object.entries(this.orderDetail)) {
            this.fieldErrors[key] = false;
        }
    }

    resetValid(field) {
        this.fieldErrors[field] = false;
    }

    setStatusEdit() {
        this.isEdit = !this.isEdit;
        this.labelButtonEdit = this.isEdit ? "Lưu lại" : "Chỉnh sửa";
        this.editorConfig.editable = this.isEdit;
    }

    changePolicy(bondPolicyId) {
        let policy = this.policies.find(item => item.bondPolicyId == bondPolicyId);
        this.orderDetail.dueDate = null;
        this.policyInfo = policy;
        this.policyDetails = policy.details;
        this.profit = null;
    }

    setDueDate(bondPolicyDetailId) {
        this.policyDetail = this.policyDetails.find(item => item.bondPolicyDetailId == bondPolicyDetailId);
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

    changeEdit() {
        console.log(this.orderDetail);
        if (this.isEdit) {
            let body = this.formatCalendar(this.fieldDates, {...this.orderDetail});
            this._orderService.update(body).subscribe((response) => {
                if (this.handleResponseInterceptor(response, 'Cập nhật thành công')) {
                    this.setStatusEdit();
                } 
            });
        } else {
            this.setStatusEdit();
        }
    }

    updateField(field) {
        if (this.isEdit) {
            if (this.orderDetail[this.fieldUpdates[field].name]) {
                this._orderService.updateField(this.orderDetail, this.fieldUpdates[field]).subscribe((response) => {
                    if (this.handleResponseInterceptor(response, 'Cập nhật thành công')) {
                        this.setStatusEdit();
                        this.resetStatusFieldUpdates();
                    }
                });
            } else {
                this.messageService.add({ severity: 'error', summary: '', detail: this.fieldUpdates[field].messageRequired, life: 1500 });
            }
        } else {
            this.setStatusEdit();
            this.fieldUpdates[field].isEdit = true;
        }
    }

}
