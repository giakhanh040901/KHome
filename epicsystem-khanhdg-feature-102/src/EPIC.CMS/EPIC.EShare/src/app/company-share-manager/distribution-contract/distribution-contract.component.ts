import { Component, Injector, Input, OnInit } from '@angular/core';
import { DistributionContractConst, SearchConst } from '@shared/AppConsts';
import { CrudComponentBase } from '@shared/crud-component-base';
import { Page } from '@shared/model/page';
import { DistributionContractServiceProxy, CompanySharePrimaryServiceProxy } from '@shared/service-proxies/company-share-manager-service';
import { ConfirmationService, MessageService } from 'primeng/api';
import { forkJoin } from 'rxjs';
import { BreadcrumbService } from 'src/app/layout/breadcrumb/breadcrumb.service';
import { Router } from "@angular/router"
import { ActivatedRoute } from '@angular/router';
import { DialogService } from 'primeng/dynamicdialog';
import { DynamicDialogRef } from 'primeng/dynamicdialog';
import { FilterTradingProviderComponent } from './filter-trading-provider/filter-trading-provider.component';
import { TradingProviderServiceProxy } from '@shared/service-proxies/setting-service';
import { FormSetDisplayColumnComponent } from 'src/app/form-set-display-column/form-set-display-column/form-set-display-column.component';
import { debounceTime } from 'rxjs/operators';

@Component({
    selector: 'app-distribution-contract',
    templateUrl: './distribution-contract.component.html',
    styleUrls: ['./distribution-contract.component.scss'],
    providers: [DialogService, ConfirmationService, MessageService]
})
export class DistributionContractComponent extends CrudComponentBase {

    constructor(
        injector: Injector,
        private dialogService: DialogService,
        private confirmationService: ConfirmationService,
        private router: Router,
        private routeActive: ActivatedRoute,
        private _distributionContractService: DistributionContractServiceProxy,
        private _companySharePrimary: CompanySharePrimaryServiceProxy,
        private _tradingProvider: TradingProviderServiceProxy,
        messageService: MessageService,
        private breadcrumbService: BreadcrumbService) {
        super(injector, messageService);
        this.breadcrumbService.setItems([
            { label: 'Trang chủ', routerLink: ['/home'] },
            { label: 'Hợp đồng phân phối' },
        ]);
    }

    statusSearch: any[] = [
        {
            name: 'Tất cả',
            code: ''
        },
        ...DistributionContractConst.status,
    ];

    ref: DynamicDialogRef;

    modalDialog: boolean;
    deleteItemDialog: boolean = false;

    confirmRequestDialog: boolean = false;

    rows: any[] = [];
    row: any;
    col: any;

    cols: any[];
    _selectedColumns: any[];

    DistributionContractConst = DistributionContractConst;

    distributionContract: any = {
        "distributionContractId": 0,  // id
        "tradingProviderId": null, // Id Địa lý sơ cấp
        "companySharePrimaryId": null,  // Id Phát hành sơ cấp
        "quantity": null, // Số lượng
        "totalValue": null, // Tổng giá trị
        "dateBuy": null,  // Ngày mua
        "status": null,
        "orderPrice": null,
    }

    itemTradingProviderInfo = {};
    fieldErrors = {};
    fieldDates = ['dateBuy'];
    submitted: boolean;
    //
    page = new Page();
    offset = 0;
    //
    listAction: any[] = [];
    companyShareInfos: any = [];
    tradingProviders: any = [];
    companyShareTypes: any = [];
    companySharePrimarys: any[] = [];
    tradingProviderInfo: any = {};
    companySharePrimaryInfo: any = {};

    actions: any[] = [];  // list button actions
    actionsDisplay: any[] = [];

    ngOnInit() {
        this.setPage({ page: this.offset });
        this.subject.keyword.pipe(debounceTime(SearchConst.DEBOUNCE_TIME)).subscribe(() => {
            if (this.keyword === "") {
                this.setPage({ page: this.offset });
            } else {
                this.setPage();
            }
        });

        this.cols = [
            { field: 'companyShareCode', header: 'Mã CP', width: '10rem', cutText: 'b-cut-text-10', isPin: true },
            { field: 'tradingProviderName', header: 'Đại lý sơ cấp', width: '16rem', cutText: 'b-cut-text-16', isPin: true },
            { field: 'contractCode', header: 'Mã hợp đồng', width: '15rem', cutText: 'b-cut-text-=15' },
            { field: 'quantity', header: 'Số lượng CP', width: '10rem', class: 'text-right justify-content-end', cutText: 'b-cut-text-10', },
            { field: 'parValue', header: 'Đơn giá', width: '10rem', class: 'text-right justify-content-end', cutText: 'b-cut-text-10' },
            { field: 'totalValue', header: 'Tổng tiền', width: '10rem', class: 'text-right justify-content-end', cutText: 'b-cut-text-10' },
            { field: 'dateBuy', header: 'Ngày mua', width: '12rem', cutText: 'b-cut-text-12' },
            { field: 'dueDate', header: 'Ngày đáo hạn', width: '12rem', cutText: 'b-cut-text-12' }
        ];

        this.cols = this.cols.map((item, index) => {
            item.position = index + 1;
            return item;
        })

        // this._selectedColumns = this.cols;
        this._selectedColumns = this.getLocalStorage('distributionContract') ?? this.cols;

    }

    getLocalStorage(key) {
        return JSON.parse(localStorage.getItem(key))
    }
    setLocalStorage(data) {
        return localStorage.setItem('distributionContract', JSON.stringify(data));
    }

    setColumn(col, _selectedColumns) {
        console.log('cols:', col);

        console.log('_selectedColumns', _selectedColumns);

        const ref = this.dialogService.open(
            FormSetDisplayColumnComponent,
            this.getConfigDialogServiceDisplayTableColumn("Sửa cột hiển thị", col, _selectedColumns)
        );
        //
        ref.onClose.subscribe((dataCallBack) => {
            console.log('dataCallBack', dataCallBack);
            if (dataCallBack?.accept) {
                this._selectedColumns = dataCallBack.data.sort(function (a, b) {
                    return a.position - b.position;
                });
                this.setLocalStorage(this._selectedColumns)
                console.log('anh Nghia', this._selectedColumns);

                console.log('Luu o local', this.getLocalStorage('distributionContract'));
            }
        });
    }

    showData(rows) {
        for (let row of rows) {
            row.companyShareCode = row?.companyShareInfo?.companyShareCode,
                row.tradingProviderName = row?.tradingProvider?.businessCustomer?.shortName,
                row.contractCode = row?.contractCode,
                row.quantity = this.utils.transformMoney(row?.quantity),
                row.parValue = this.utils.transformMoney(row?.companyShareInfo?.parValue),
                row.totalValue = this.utils.transformMoney(row?.totalValue),
                row.dateBuy = this.formatDate(row?.dateBuy);
            row.dueDate = this.formatDate(row?.companyShareInfo?.dueDate);
        };
        console.log('row', rows);
    }

    genListAction(data = []) {
        this.listAction = data.map(distributionContractItem => {
            const actions = [];

            if (this.isGranted([this.PermissionCompanyShareConst.CompanyShareMenuQLTP_HDPP_TTCT])){
				actions.push({
                    data: distributionContractItem,
                    label: 'Thông tin chi tiết',
                    icon: 'pi pi-info-circle',
                    command: ($event) => {
                        this.detail($event.item.data);
                    }
				})
			}

            if (distributionContractItem.status == this.DistributionContractConst.DAT_DAU_TU && this.isGranted([this.PermissionCompanyShareConst.CompanyShareMenuQLTP_HDPP_Xoa])) {
                actions.push({
                    data: distributionContractItem,
                    label: 'Xóa',
                    icon: 'pi pi-trash',
                    command: ($event) => {
                        this.delete($event.item.data);
                    }
                });
            }

            return actions;
        });
    }

    showTradingProvider() {
        const ref = this.dialogService.open(FilterTradingProviderComponent,
            {
                header: 'Tìm kiếm đại lý sơ cấp',
                width: '1000px',
                styleClass: 'p-dialog-custom filter-trading-provider customModal',
                style: { 'min-height': '300px', 'height': 'auto', 'top': '-25%' }
                // height: '100%',
            });

        ref.onClose.subscribe((tradingProviderInfo) => {
            if (tradingProviderInfo) {
                this.distributionContract.companySharePrimaryId = null;
                //
                this.tradingProviderInfo = tradingProviderInfo;
                this.distributionContract.tradingProviderId = tradingProviderInfo.tradingProviderId;
                //
                this._companySharePrimary.getFindForTradingProvider(tradingProviderInfo.tradingProviderId).subscribe((resCompanySharePrimary) => {
                    this.isLoading = false;
                    if (this.handleResponseInterceptor(resCompanySharePrimary, '')) {
                        this.companySharePrimarys = resCompanySharePrimary?.data;
                        console.log({ 'resCompanySharePrimarys': resCompanySharePrimary });
                    }
                }, (err) => {
                    this.isLoading = false;
                    console.log('Error-------', err);
                    
                });

            }
        });
    }

    setPage(pageInfo?: any) {
        this.page.pageNumber = pageInfo?.page ?? this.offset;
        if(pageInfo?.rows) this.page.pageSize = pageInfo?.rows;

        this.page.keyword = this.keyword;
        this.isLoading = true;

        this._distributionContractService.getAll(this.page, this.status).subscribe((res) => {
            this.isLoading = false;
            if (this.handleResponseInterceptor(res, '')) {
                this.page.totalItems = res.data.totalItems;
                this.rows = res.data.items;
                if (this.rows?.length) {
                    this.genListAction(this.rows);
                    this.showData(this.rows)
                }
                console.log({ rows: res.data.items, totalItems: res.data.totalItems });
            }
        }, (err) => {
            this.isLoading = false;
            console.log('Error-------', err);
            
        });
    }



    countOrderPrice(value, field) {
        if (this.distributionContract[field] && value !== 0) {
            this.distributionContract.orderPrice = (field == 'totalValue') ? (this.distributionContract[field] / value) : (value / this.distributionContract[field]);
        }
    }

    changeCompanySharePrimary(event) {
        let companySharePrimaryId = event.value;
        if (this.companySharePrimarys) {
            this.companySharePrimaryInfo = this.companySharePrimarys.find(item => item.companySharePrimaryId == companySharePrimaryId);
            this.companySharePrimaryInfo.openCellDate = new Date(this.companySharePrimaryInfo?.openCellDate);
            if (this.companySharePrimaryInfo.closeCellDate == null) {
                this.companySharePrimaryInfo.closeCellDate = new Date(this.companySharePrimaryInfo?.openCellDate);
                this.companySharePrimaryInfo.closeCellDate.setFullYear(this.companySharePrimaryInfo.closeCellDate.getFullYear() + 100);
            }
            else {
                this.companySharePrimaryInfo.closeCellDate = new Date(this.companySharePrimaryInfo.closeCellDate);
            }
        }
    }

    clickDropdown(row) {
        this.distributionContract = { ...row };
        this.actionsDisplay = this.actions.filter(action => action.statusActive.includes(row.status) && action.permission);
        console.log({ distributionContract: row });
    }

    detail(distributionContract) {
        this.router.navigate(['/company-share-manager/distribution-contract/detail/' + this.cryptEncode(distributionContract?.distributionContractId)]);

    }

    setFieldError() {
        for (const [key, value] of Object.entries(this.distributionContract)) {
            this.fieldErrors[key] = false;
        }
        console.log({ filedError: this.fieldErrors });
    }

    resetData() {
        this.companySharePrimaryInfo = {};
        this.distributionContract = {};
        this.tradingProviderInfo = {};
    }

    create() {
        this.resetData();
        console.log({ companySharePrimaryInfo: this.companySharePrimaryInfo });
        this.submitted = false;
        this.modalDialog = true;
    }

    delete(distributionContract) {
        this.deleteItemDialog = true;
        this.confirmDelete(distributionContract);
    }

    pending() {
        this.confirmationService.confirm({
            message: 'Bạn có chắc chắn trình duyệt hợp đồng này?',
            header: 'Phê duyệt thanh toán',
            acceptLabel: "Đồng ý",
            rejectLabel: "Hủy",
            icon: 'pi pi-exclamation-triangle',
            accept: () => {
                this.messageService.add({
                    severity: 'success',
                    summary: '',
                    detail: 'Trình duyệt thành công!',
                    life: 1500,
                });
            },
            reject: () => {

            },
        });
    }

    confirmDelete(distributionContract) {
        this.deleteItemDialog = false;
        this._distributionContractService.delete(distributionContract?.distributionContractId).subscribe(
            (response) => {
                if (this.handleResponseInterceptor(response, 'Xóa thành công')) {
                    this.setPage({ page: this.page.pageNumber });
                    this.distributionContract = {};
                }
            }, () => {
                this.messageService.add({
                    severity: 'error',
                    summary: '',
                    detail: `Không xóa được hợp đồng phân phối!`,
                    life: 3000,
                });
            }
        );
    }

    confirm() {
        this.confirmRequestDialog = true;
    }

    confirmRequest() {
        this.confirmRequestDialog = false;
        this.messageService.add({
            severity: 'info',
            summary: '',
            detail: `Gửi yêu cầu phê duyệt thành công`,
            life: 1500,
        });
    }


    hideDialog() {
        this.modalDialog = false;
        this.submitted = false;
    }

    resetValid(field) {
        this.fieldErrors[field] = false;
    }

    save() {
        this.submitted = true;
        //
        this.setTimeZoneList(this.fieldDates, this.distributionContract);
        console.log({ distributionContract: this.distributionContract });
        if (this.distributionContract.distributionContractId) {
            this._distributionContractService.update(this.distributionContract).subscribe((response) => {
                this.callTriggerFiledError(response, this.fieldErrors);
                if (this.handleResponseInterceptor(response, 'Cập nhật thành công')) {
                    this.submitted = false;
                    this.setPage({ page: this.page.pageNumber });
                    this.hideDialog();
                } else {
                    this.callTriggerFiledError(response, this.fieldErrors);
                    this.resetTimeZoneList(this.fieldDates, this.distributionContract);
                    this.submitted = false;
                }
            }, () => {
                this.submitted = false;
            }
            );
        } else {
            this._distributionContractService.create(this.distributionContract).subscribe(
                (response) => {
                    if (this.handleResponseInterceptor(response, 'Thêm thành công')) {
                        this.submitted = false;
                        this.hideDialog();
                        this.isLoadingPage = true;
                        setTimeout(() => {
                            this.router.navigate(['/company-share-manager/distribution-contract/detail/', this.cryptEncode(response.data.distributionContractId)]);
                        }, 1000);
                    } else {
                        this.callTriggerFiledError(response, this.fieldErrors);
                        this.resetTimeZoneList(this.fieldDates, this.distributionContract);
                        this.submitted = false;
                    }
                }, () => {
                    this.submitted = false;
                }
            );
        }
    }

    validForm(): boolean {
        const validRequired = this.distributionContract?.companySharePrimaryId
            && this.distributionContract?.quantity
            && this.distributionContract?.totalValue
            && this.distributionContract?.dateBuy;
        return validRequired;
    }

    header(): string {
        return !this.distributionContract?.distributionContractId ? 'Thêm hợp đồng phân phối' : 'Sửa hợp đồng phân phối';
    }

    changeStatus() {
        this.setPage({ Page: this.offset })
    }

}
