import { CrudComponentBase } from '@shared/crud-component-base';
import { Page } from '@shared/model/page';
import { Component, Injector, OnInit } from '@angular/core';
import { OrderService } from '@shared/service-proxies/shared-data-service';
import { Router } from '@angular/router';
import { MessageService } from 'primeng/api';
import { OrderServiceProxy } from '@shared/service-proxies/trading-contract-service';
import * as moment from 'moment';

import { SaleService } from '@shared/service-proxies/sale-service';
import { DialogService, DynamicDialogRef } from 'primeng/dynamicdialog';
import { debounceTime } from 'rxjs/operators';
import { SearchConst, TabView } from '@shared/AppConsts';


@Component({
  selector: 'app-investor-filter',
  templateUrl: './investor-filter.component.html',
  styleUrls: ['./investor-filter.component.scss']
})
export class InvestorFilterComponent extends CrudComponentBase {

  customerInformation: any = {};

    activeIndex = TabView.FIRST;

    keyword: string;
    investorSale: any = {}
    page = new Page;
    sale: any = {}
    customers: any[] = [];
    businiessCustomers: any[] = [];
    sales: any[] = [];

    listBank: any[] = [];
    listAddress: any [] = [];
    TabView = TabView;
    constructor(
        injector: Injector,
        messageService: MessageService,
        public orderService: OrderService,
        private router: Router,
        private _saleService: SaleService,
        private dialogService: DialogService,
        public ref: DynamicDialogRef,
    ) {
        super(injector, messageService);
    }

    ngOnInit() {
        this.subject.keyword.pipe(debounceTime(SearchConst.DEBOUNCE_TIME)).subscribe(() => {
            if (this.keyword.trim()) {
                this.keyword = this.keyword.trim();
                this.getInfoCustomer();
            } else {
                this.customers = [];
                this.businiessCustomers = [];
            }
        });
    }
    
    changeContractAddress(contractAddressId) {
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
        if (this.activeIndex == TabView.FIRST) {
            this._saleService.getInfoInvestorCustomer(this.keyword).subscribe((res) => {
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
        if (this.activeIndex == TabView.SECOND && this.keyword.length > 0) {
            this._saleService.getInfoBusinessCustomer(this.keyword).subscribe((res) => {
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
        } else if(this.activeIndex == TabView.SECOND && this.keyword.length == 0) {
            this.messageService.add({ severity: 'error', summary: '', detail: 'Không tìm thấy dữ liệu', life: 1200 });
            this.isLoading = false;
        }

    }

    resetData() {
        this.keyword = '';
        this.businiessCustomers = [];
        this.customers = [];
        this.investorSale = {};
        this.sale = {};
    }
    
    isChooseBusinessCustomer(row) {
        this.resetData();
        this.customerInformation.customerInfo = row;
        this.ref.close(row);
    }

    isChooseInvestorCustomer(row) {
        this.resetData();
        this.customerInformation.customerInfo = row;
        this.ref.close(row);
    }

    clearKeyword() {
        if (this.keyword === '') {
            this.getInfoCustomer();
        }
    }

}
