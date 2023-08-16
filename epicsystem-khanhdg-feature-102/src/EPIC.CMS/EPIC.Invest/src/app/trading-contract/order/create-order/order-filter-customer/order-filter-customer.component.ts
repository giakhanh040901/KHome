import { CrudComponentBase } from '@shared/crud-component-base';
import { Page } from '@shared/model/page';
import { Component, Injector, OnInit } from '@angular/core';
import { OrderShareService } from '@shared/service-proxies/shared-data-service';
import { Router } from '@angular/router';
import { MessageService } from 'primeng/api';
import { DialogService } from 'primeng/dynamicdialog';
import { FilterSaleComponent } from './filter-sale/filter-sale.component';
import { OrderService } from '@shared/services/order.service';
import { IColumn } from '@shared/interface/p-table.model';
import { TableConst } from '@shared/AppConsts';

@Component({
    selector: 'app-order-filter-customer',
    templateUrl: './order-filter-customer.component.html',
    styleUrls: ['./order-filter-customer.component.scss']
})

export class OrderFilterCustomerComponent extends CrudComponentBase {

    customerInformation: any = {};

    activeIndex = 0;

    page = new Page;
    investorBankId: number;
    //
    customerInvestors: any[] = [];
    businiessCustomers: any[] = [];
    //
    sales: any[] = [];
    orderInformation: any = {};
    //
    listBank: any[] = [];
    listAddress: any[] = [];
    //
    sale:any = {}
    investorSale:any = {}
    constructor(
        injector: Injector,
        messageService: MessageService,
        private dialogService :DialogService,
        public orderService: OrderShareService,
        private router: Router,
        private _orderService: OrderService,
    ) {
        super(injector, messageService);
    }

    columnInvestorCustomers: IColumn[] = [];
    columnBusinessCustomers: IColumn[] = [];

    ngOnInit() {
        if (!this.orderService.filterCustomer && !this.orderService.filterProject && !this.orderService.filterView) {
            this.resetData();
        }
        else {
            this.sale = this.orderService.saleInfomation;
            this.listBank = this.orderService.orderInformation.customerInformation.customerInfo.listBank;
            this.investorSale = this.orderService.investorSale;
            this.orderInformation = this.orderService.getOrderInformation();
            this.customerInformation = this.orderService.getOrderInformation().customerInformation;
        }
        //
        this.activeIndex = this.customerInformation.activeIndex;
        this.page.keyword = this.customerInformation.customerInfo.taxCode;
        //
        this.columnInvestorCustomers = [
            { field: 'name', header: 'Họ và tên', width: 16 },
            { field: 'phone', header: 'Số điện thoại', width: 10 },
            { field: 'idNo', header: 'Số CMND/CCCD', width: 14 },
            { field: 'email', header: 'Email', width: 14 },
            { field: 'address', header: 'Địa chỉ thường trú', width: 25, isResize: true },
            { field: 'isChoose', header: 'Chọn', width: 5, type: TableConst.columnTypes.ACTION_BUTTON, icon: 'pi pi-check', isFrozen: true, alignFrozen: TableConst.alignFrozenColumn.RIGHT },
        ];
        //
        this.columnBusinessCustomers = [
            { field: 'name', header: 'Tên doanh nghiệp', width: 25, isResize: true },
            { field: 'shortName', header: 'Tên viết tắt', width: 12 },
            { field: 'taxCode', header: 'Mã số thuế', width: 10 },
            { field: 'email', header: 'Thư điện tử', width: 15 },
            { field: 'phone', header: 'Số điện thoại', width: 10, isResize: true },
            { field: 'isChoose', header: 'Chọn', width: 5, type: TableConst.columnTypes.ACTION_BUTTON, icon: 'pi pi-check', isFrozen: true, alignFrozen: TableConst.alignFrozenColumn.RIGHT },
        ];
    }

    changeKeyword() {
        if (this.page.keyword.trim()) {
            this.getInfoCustomer();
        } else {
            this.customerInvestors = [];
            this.businiessCustomers = [];
        }
    }

    removeReferralCode() {
        this.sale = null
        this.investorSale = null
    }

    ngOnDestroy(): void {
        this.orderService.filterCustomer = false;
        this.orderService.filterProject = false;
        this.orderService.filterView = false;
    }

    resetData() {
        this.orderService.resetValue();
        this.page.keyword = '';
        this.businiessCustomers = [];
        this.customerInvestors = [];
        this.orderInformation = this.orderService.getOrderInformation();
        this.customerInformation = this.orderService.getOrderInformation().customerInformation;
    }

    changeTabview(indexTab) {
        this.resetData();
        console.log({ indexTab: indexTab });
    }

    showSale() {
        const ref = this.dialogService.open(FilterSaleComponent,
            {
                header: 'Tìm kiếm sale',
                width: '850px',
                contentStyle: {'padding-bottom': '10px'},
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

    getInfoCustomer() {
        this.isLoading = true;
        if (this.activeIndex == 0) {
            this._orderService.getInfoInvestorCustomer(this.page.keyword).subscribe((res) => {
                this.disableLoading();
                if (this.handleResponseInterceptor(res, '')) {
                    let customerInvestors = res?.data?.items;
                    this.customerInvestors = customerInvestors.map(customer => {
                        return {
                            ...customer,
                            name: customer?.defaultIdentification?.fullname || 'None',
                            phone: customer?.phone || 'None',
                            idNo: customer?.defaultIdentification?.idNo || 'None',
                            email: customer?.email || 'None',
                            address: customer?.defaultIdentification?.placeOfResidence || 'None',
                            isChoose: (item) => this.isChooseInvestorCustomer(item)
                        }
                    })
                    if (!this.customerInvestors.length) this.messageService.add({ severity: 'error', summary: '', detail: 'Không tìm thấy dữ liệu', life: 1200 });
                }
            }, (err) => {
                this.isLoading = false;
                console.log('Error-------', err);
            });
        }
        if (this.activeIndex == 1) {
            this.page.keyword = this.page.keyword.trim();
            if(this.page.keyword != ""){
                this._orderService.getInfoBusinessCustomer(this.page, this.page.keyword).subscribe((res) => {
                    this.disableLoading();
                    if (this.handleResponseInterceptor(res, '')) {
                        let businiessCustomers = res?.data?.items;
                        this.businiessCustomers = businiessCustomers.map(customer => {
                            return {
                                ...customer,
                                isChoose: (item) => this.isChooseBusinessCustomer(item),
                            }
                        })
                        if (!this.businiessCustomers.length) {
                            this.messageService.add({ severity: 'error', summary: '', detail: 'Không tìm thấy dữ liệu', life: 1200 });
                        }
                    }
                }, (err) => {
                    this.isLoading = false;
                    console.log('Error-------', err);
                });
            } else {
                this.isLoading = false;
                this.messageService.add({ severity: 'error', summary: '', detail: 'Không tìm thấy dữ liệu', life: 1200 });
            }
        }

    }

    disableLoading(reloadTime = 300) {
        setTimeout(() => {
            this.isLoading = false;
        }, reloadTime);
    }

    isChooseBusinessCustomer(row) {
        this.resetData();
        //
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
        this.messageService.add({ severity: 'success', summary: '', detail: 'Thêm dữ liệu khách hàng thành công!', life: 3000 });
    }

    isChooseInvestorCustomer(row) {
        this.resetData();
        this.customerInformation.customerInfo = row;
        const listBank = row?.listBank;
        if (listBank?.length) {
            this.listBank = listBank.map(bank => {
                bank.investorBankAccId = bank.id;
                bank.labelName = bank.coreBankName + ' - ' + bank.bankAccount + (bank.ownerAccount ? ' - ' + bank.ownerAccount : '') + (bank.bankName ? (' - ' + bank.bankName) : '');
                return bank;
            });
        }

        if(row?.listContactAddress?.length){
            this.customerInformation.customerInfo.listAddress = row?.listContactAddress.map(item => {
                item.fullAddress = item.contactAddress + ' - ' + item.detailAddress;
                return item;
            });
        }

        this.customerInformation.customerInfo.listBank = this.listBank;
        this.customerInformation.customerInfo.listAddress = row?.listContactAddress ?? [];
        this.customerInformation.customerInfo.contractAddress = this.customerInformation.customerInfo.listAddress.find(item => item.isDefault == 'Y');
        this.customerInformation.customerInfo.contractAddressId = this.customerInformation?.customerInfo?.contractAddress?.contactAddressId;

        this.messageService.add({ severity: 'success', summary: '', detail: 'Thêm dữ liệu khách hàng thành công!', life: 3000 });
    }

    nextPage() {
        this.orderService.filterProject = true;
       
        if (this.customerInformation?.customerInfo?.cifCode && this.customerInformation?.customerInfo?.investorBankAccId ) { //&& !checkCoincidentSaleCustomer
            this.orderService.orderInformation.customerInformation = this.customerInformation;
            this.orderService.saleInfomation = this.sale;
            this.orderService.getOrderInformation().investorBankAccId = this.customerInformation.customerInfo.investorBankAccId; // Check lại
            this.orderService.getOrderInformation().customerInformation.investorBankAccId = this.customerInformation.customerInfo.investorBankAccId;
            this.orderService.getOrderInformation().customerInformation.contractAddressId = this.customerInformation.customerInfo.contractAddressId;
            this.orderService.getOrderInformation().cifCode = this.customerInformation.customerInfo.cifCode;    //Check lại 
            this.orderService.getOrderInformation().customerInformation.cifCode = this.customerInformation.customerInfo.cifCode;
            this.orderService.getOrderInformation().customerInformation.activeIndex = this.activeIndex;

            this.router.navigate(['trading-contract/order/create/filter-project']);
        } else {
            if (!this.customerInformation?.customerInfo?.cifCode) this.messageService.add({ severity: 'error', detail: 'Vui lòng chọn khách hàng!', life: 1500 });
            if (this.customerInformation?.customerInfo?.cifCode && !this.customerInformation?.customerInfo?.investorBankAccId) this.messageService.add({ severity: 'error', detail: 'Vui lòng chọn tài khoản ngân hàng!', life: 1500 });
        }
    }
}
