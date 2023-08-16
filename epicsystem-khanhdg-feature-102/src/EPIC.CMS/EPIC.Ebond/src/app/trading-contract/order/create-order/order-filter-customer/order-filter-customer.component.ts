import { debounceTime } from 'rxjs/operators';
import { CrudComponentBase } from '@shared/crud-component-base';
import { Page } from '@shared/model/page';
import { Component, Injector, OnInit } from '@angular/core';
import { OrderService } from '@shared/service-proxies/shared-data-service';
import { Router } from '@angular/router';
import { MessageService } from 'primeng/api';
import { OrderServiceProxy } from '@shared/service-proxies/trading-contract-service';
import * as moment from 'moment';
import { SearchConst } from '@shared/AppConsts';
import { FilterSaleComponent } from './filter-sale/filter-sale.component';
import { DialogService } from 'primeng/dynamicdialog';
import { SaleService } from '@shared/services/sale.service';

@Component({
    selector: 'app-order-filter-customer',
    templateUrl: './order-filter-customer.component.html',
    styleUrls: ['./order-filter-customer.component.scss']
})

export class OrderFilterCustomerComponent extends CrudComponentBase {

    customerInformation: any = {};

    activeIndex = 0;

    keyword: string;
    investorSale: any = {}
    page = new Page;
    sale: any = {}
    customers: any[] = [];
    businiessCustomers: any[] = [];
    sales: any[] = [];

    listBank: any[] = [];
    listAddress: any [] = [];

    constructor(
        injector: Injector,
        messageService: MessageService,
        public orderService: OrderService,
        private router: Router,
        private _saleService: SaleService,
        private dialogService: DialogService,
        private _orderService: OrderServiceProxy,
    ) {
        super(injector, messageService);
    }

    ngOnInit() {
        this.subject.keyword.pipe(debounceTime(SearchConst.DEBOUNCE_TIME)).subscribe(() => {
            if (this.keyword.trim()) {
                this.getInfoCustomer();
            } else {
                this.customers = [];
                this.businiessCustomers = [];
            }
        });
        
        this.customerInformation = this.orderService.orderInformation.customerInformation;
        if (!this.orderService.filterCustomer && !this.orderService.filterBond && !this.orderService.filterView) {
            this.sale = {};
            this.orderService.resetValue();
            this.investorSale = {};
            this.orderService.saleInfomation = {}
            this.orderService.orderInformation.customerInformation.customerInfo.listBank = [];
            this.orderService.investorSale = {};
        }
        else {
            this.sale = this.orderService.saleInfomation;
            this.listBank = this.orderService.orderInformation.customerInformation.customerInfo.listBank;
            this.investorSale = this.orderService.investorSale;
        }
        this.orderService.filterCustomer = true;

        if (this.orderService.getOrderInformation().orderEditting) {
            this.orderService.getOrderInformation().orderEditting = false;
            this.customerInformation = this.orderService.getOrderInformation().customerInformation;
        } else {
            this.customerInformation = this.orderService.getOrderInformationReset().customerInformation;
        }
    }

    ngOnDestroy(): void {
        this.orderService.filterCustomer = false;
        this.orderService.filterBond = false;
        this.orderService.filterView = false;
    }

    changeContractAddress(contractAddressId){
        this.customerInformation.customerInfo.contractAddressId = contractAddressId;
    }

    changeTabview(indexTab) {
        console.log({ indexTab: indexTab });
        this.keyword = '';
        this.businiessCustomers = [];
        this.customers = [];
    }

    getInfoCustomer() {
        this.isLoading = true;
        if (this.activeIndex == 0) {
            this._orderService.getInfoInvestorCustomer(this.keyword).subscribe((res) => {
                this.isLoadingPage = false;
                this.isLoading = false;
                if (this.handleResponseInterceptor(res, '')) {
                    this.customers = res?.data?.items;
                    if (!this.customers.length) this.messageService.add({ severity: 'error', summary: '', detail: 'Không tìm thấy dữ liệu', life: 1200 });
                }
            }, (err) => {
                this.isLoading = false;
                this.isLoadingPage = false;
                console.log('Error-------', err);
                
            });
        }
        if (this.activeIndex == 1) {
            this._orderService.getInfoBusinessCustomer(this.page, this.keyword).subscribe((res) => {
                this.isLoading = false;
                this.isLoadingPage = false;
                if (this.handleResponseInterceptor(res, '')) {
                    this.businiessCustomers = res?.data?.items;
                    if (!this.businiessCustomers.length) {
                        this.messageService.add({ severity: 'error', summary: '', detail: 'Không tìm thấy dữ liệu', life: 1200 });
                    }
                }
            }, (err) => {
                this.isLoading = false;
                this.isLoadingPage = false;
                console.log('Error-------', err);
                
            });
        }

    }

    resetData() {
        this.keyword = '';
        this.businiessCustomers = [];
        this.customers = [];
        // this.investorSale = {};
        // this.sale = {};
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

    isChooseBusinessCustomer(row) {
        this.resetData();
        this.customerInformation.customerInfo = row;
        const listBank = row?.businessCustomerBanks;
        if (listBank?.length) {
            this.listBank = listBank.map(bank => {
                bank.investorBankAccId = bank.businessCustomerBankId;
                bank.labelName = bank.bankName + ' - ' + bank.bankAccNo + ' - ' + bank.bankAccName;
                return bank;
            });
        }

        this.customerInformation.customerInfo.listBank = this.listBank;
        this.orderService.orderInformation.customerInformation.customerInfo.listBank = this.listBank;
        this.messageService.add({ severity: 'success', summary: '', detail: 'Thêm dữ liệu khách hàng thành công!', life: 3000 });
    }

    isChooseInvestorCustomer(row) {
        this.resetData();
        this.customerInformation.customerInfo = row;
        this.customerInformation.customerInfo.listAddress = row?.listContactAddress ?? [];
        const listBank = row?.listBank;
        if (listBank?.length) {
            this.listBank = listBank.map(bank => {
                bank.investorBankAccId = bank.id;
                bank.labelName = (bank.coreBankName ? bank.coreBankName + ' - ': '' ) +  (bank.bankAccount ? + bank.bankAccount + ' - ' : '') + (bank.ownerAccount ? bank.ownerAccount : '');
                return bank;
            });
        }
        this.customerInformation.customerInfo.listBank = this.listBank;

        this.messageService.add({ severity: 'success', summary: '', detail: 'Thêm dữ liệu khách hàng thành công!', life: 3000 });
    }

    clearKeyword() {
        if (this.keyword === '') {
            this.getInfoCustomer();
        }
    }

    nextPage() {
        this.orderService.filterCustomer = false;
        this.orderService.filterBond = true;
        if (this.customerInformation?.customerInfo?.cifCode && this.customerInformation?.customerInfo?.investorBankAccId) {
            this.orderService.orderInformation.customerInformation = this.customerInformation;

            this.router.navigate(['trading-contract/order/create/filter-product-bond']);
        } else {
            if (!this.customerInformation?.customerInfo?.cifCode) this.messageService.add({ severity: 'error', detail: 'Vui lòng chọn khách hàng!', life: 1500 });
            if (this.customerInformation?.customerInfo?.cifCode && !this.customerInformation?.customerInfo?.investorBankAccId) this.messageService.add({ severity: 'error', detail: 'Vui lòng chọn tài khoản ngân hàng!', life: 1500 });
        }
    }
}
