import { Component, Inject, Injector, Input, OnInit } from '@angular/core';
import { CrudComponentBase } from '@shared/crud-component-base';
import { ConfirmationService, MessageService } from 'primeng/api';
import { BreadcrumbService } from 'src/app/layout/breadcrumb/breadcrumb.service';
import { forkJoin } from 'rxjs';
import { debounceTime } from 'rxjs/operators';
import { PageBondPolicyTemplate } from '@shared/model/pageBondPolicyTemplate';
import {  SearchConst, FormNotificationConst, ActiveDeactiveConst, ContractFormConst, SampleContractConst } from '@shared/AppConsts';
import { DialogService } from 'primeng/dynamicdialog';
import { FormSetDisplayColumnComponent } from 'src/app/form-general/form-set-display-column/form-set-display-column/form-set-display-column.component';
import { FormNotificationComponent } from 'src/app/form-general/form-notification/form-notification.component';
import { SampleContractService } from '@shared/services/sample-contract.service';
import { CreateSampleContractComponent } from './create-sample-contract/create-sample-contract.component';
import { AppUtilsService } from '@shared/services/utils.service';
import { API_BASE_URL } from '@shared/service-proxies/service-proxies-base';
import { Page } from '@shared/model/page';
import { IBaseListAction, IColumn } from '@shared/interfaces/base.interface';
import { ISampleContract } from '@shared/interfaces/sample-contract.interface';

@Component({
  selector: 'app-sample-contract',
  templateUrl: './sample-contract.component.html',
  styleUrls: ['./sample-contract.component.scss'],
    providers: [DialogService, ConfirmationService, MessageService]
})
export class SampleContractComponent extends CrudComponentBase {

    constructor(
        injector: Injector,
        messageService: MessageService,
        private dialogService: DialogService,
        private breadcrumbService: BreadcrumbService,
        public confirmationService: ConfirmationService,
        private _dialogService: DialogService,
		private _sampleContractService: SampleContractService,
        private _utilsService: AppUtilsService,
        @Inject(API_BASE_URL) baseUrl?: string,
    ) {
        super(injector, messageService);
        this.baseUrl = baseUrl !== undefined && baseUrl !== null ? baseUrl : "";
        this.breadcrumbService.setItems([
            { label: 'Trang chủ', routerLink: ['/home'] },
            { label: 'Mẫu hợp đồng' },
        ]);
    }
    private baseUrl: string;
    rows: ISampleContract[] = [];
    ActiveDeactiveConst = ActiveDeactiveConst;
    SampleContractConst = SampleContractConst;
    ContractFormConst = ContractFormConst;
    _selectedColumns: IColumn[];
    cols: IColumn[];
    listAction: IBaseListAction[] = [];
    page = new Page();
    offset = 0;
    fieldFilters = {
        status: null,
        contractSource: null,
        contractType: null,
    }

    ngOnInit(): void {
        this.setPage({ page: this.offset });
        this.subject.keyword.pipe(debounceTime(SearchConst.DEBOUNCE_TIME)).subscribe(() => {
            if (this.keyword === "") {
                this.setPage({ page: this.offset });
            } else {
                this.setPage();
            }
        });
        // Xử lý ẩn hiện cột trong bảng
        this.cols = [
       
            { field: 'name', header: 'Tên hợp đồng', width: '16rem', isPin: true },
            { field: 'contractTypeGetAll', header: 'Loại hợp đồng ', width: '10rem' },
            { field: 'contractSourceGetAll', header: 'Kiểu hợp đồng', width: '10rem' },
            { field: 'createdBy', header: 'Người tạo', width: '15rem' },
            { field: 'createdDateDisplay', header: 'Ngày tạo', width: '12rem' },
            { field: 'columnResize', header: '', type:'hidden' },
        ];

        this.cols = this.cols.map((item, index) => {
            item.position = index + 1;
            return item;
          });
        // this._selectedColumns = this.cols;
        this._selectedColumns = this.getLocalStorage('sampleContractGan') ?? this.cols;
    }

    showData(rows) {
        for (let row of rows) {
            row.name = row.name;
			row.contractTypeGetAll = SampleContractConst.getContractType(row.contractType, 'name');
            row.contractSourceGetAll = SampleContractConst.getContractSource(row.contractSource, 'name');
            row.createdDateDisplay = this.formatDateTime(row?.createdDate);
        }
        console.log('row', rows);
    }

    changeStatusFilter() {
        this.setPage({ page: this.page.pageNumber });
    }

    setColumn(col, _selectedColumns) {
        console.log({ 'cols:': col, '_selectedColumns': _selectedColumns });

        const ref = this.dialogService.open(
            FormSetDisplayColumnComponent,
            this.getConfigDialogServiceDisplayTableColumn(col, _selectedColumns)
        );
        //
        ref.onClose.subscribe((dataCallBack) => {
            console.log('dataCallBack', dataCallBack);
            if (dataCallBack?.accept) {
                this._selectedColumns = dataCallBack.data.sort(function (a, b) {
                    return a.position - b.position;
                });
                this.setLocalStorage(this._selectedColumns, 'sampleContractGan')
                console.log('Luu o local', this.getLocalStorage('sampleContract'));
            }
        });
    }

    genListAction(data = []) {
        this.listAction = data.map((sampleContract, index) => {

            const actions = [];

            if (this.isGranted([this.PermissionGarnerConst.GarnerMauHD_TaiFileCaNhan])) {
                actions.push({
                    data: sampleContract,
                    index: index,
                    label: 'Tải file cá nhân',
                    icon: 'pi pi-download',
                    command: ($event) => {
                        this.downloadFileInvestor($event.item.data);
                    }
                })
            }

            if (this.isGranted([this.PermissionGarnerConst.GarnerMauHD_TaiFileDoanhNghiep])) {
                actions.push({
                    data: sampleContract,
                    index: index,
                    label: 'Tải file d/nghiệp',
                    icon: 'pi pi-download',
                    command: ($event) => {
                        this.downloadFileBusiCus($event.item.data);
                    }
                })
            }

            if (this.isGranted([this.PermissionGarnerConst.GarnerMauHD_CapNhat])) {
                actions.push({
                    data: sampleContract,
                    index: index,
                    label: 'Sửa',
                    icon: 'pi pi-pencil',
                    command: ($event) => {
                        this.edit($event.item.data);
                    }
                })
            }

            if (sampleContract.status == ContractFormConst.StatusActive && this.isGranted([this.PermissionGarnerConst.GarnerMauHD_CapNhat])) {
                actions.push({
                    data: sampleContract,
                    index: index,
                    label: 'Đóng',
                    icon: 'pi pi-times-circle',
                    command: ($event) => {
                        this.changeStatus($event.item.data);
                    }
                })
            }

            if (sampleContract.status == ContractFormConst.StatusDeactive && this.isGranted([this.PermissionGarnerConst.GarnerCSM_CapNhat])) {
                actions.push({
                    data: sampleContract,
                    index: index,
                    label: 'Kích hoạt',
                    icon: 'pi pi pi-check-circle',
                    command: ($event) => {
                        this.changeStatus($event.item.data);
                    }
                })
            }

            if (this.isGranted([this.PermissionGarnerConst.GarnerMauHD_Xoa])) {
                actions.push({
                    data: sampleContract,
                    label: 'Xoá',
                    icon: 'pi pi-trash',
                    command: ($event) => {
                        this.delete($event.item.data);
                    }
                })
            }

            return actions;
        });
        console.log('listActions', this.listAction);

    }

    changeStatus(sampleContract) {
        const ref = this._dialogService.open(
            FormNotificationComponent,
            {
                header: "Thông báo",
                width: '350px',
                contentStyle: { "max-height": "600px", "overflow": "auto", "padding-bottom": "50px" },
                styleClass: 'p-dialog-custom',
                baseZIndex: 10000,
                data: {	
                    title: sampleContract.status == ContractFormConst.StatusActive ? 'Khóa mẫu hợp đồng?' : 'Kích hoạt mẫu hợp đồng?',
                    icon: sampleContract.status == ContractFormConst.StatusActive ? FormNotificationConst.IMAGE_CLOSE : FormNotificationConst.IMAGE_APPROVE,
                },
            }
        );
        
        ref.onClose.subscribe((dataCallBack) => {
            console.log({ dataCallBack });
            if (dataCallBack?.accept) {
                this._sampleContractService.updateStatus(sampleContract).subscribe((response) => {
                    if (this.handleResponseInterceptor(response,"Cập nhật thành công")) {
                        this.setPage({ page: this.page.pageNumber });
                    }
                }, (err) => {
                    console.log('err____', err);
                    
                });
            } else {
            }
        });
    }

    downloadFileInvestor(file) {
        
        let fileUrl = file.fileInvestor;
        const url = this.baseUrl + "/" + fileUrl;
        this._utilsService.makeDownload("", url);
    }

    downloadFileBusiCus(file) {
        
        let fileUrl = file.fileBusinessCustomer;
        const url = this.baseUrl + "/" + fileUrl;
        this._utilsService.makeDownload("", url);
    }

    create() {
		const ref = this.dialogService.open(CreateSampleContractComponent, {
            header: "Thêm mới hợp đồng mẫu",
            width: '800px',
            contentStyle: { "max-height": "600px", overflow: "auto", "margin-bottom": "60px", },
            baseZIndex: 10000,
            data: {
            
            },
        });
        //
        ref.onClose.subscribe((res) => {
            if(res) {
                this.setPage();
            }
        });
	}

    edit(sampleContract) {
		const ref = this.dialogService.open(CreateSampleContractComponent, {
			header: "Sửa hợp đồng mẫu",
			width: '800px',
			contentStyle: { "max-height": "600px", overflow: "auto", "margin-bottom": "60px", },
			baseZIndex: 10000,
			data: {
				sampleContract : sampleContract
			},
		});
		//
		ref.onClose.subscribe((res) => {
            if(res) {
                this.setPage();
            }
		});
    }

    delete(sampleContract) {
        const ref = this._dialogService.open(
            FormNotificationComponent,
            {
                header: "Thông báo",
                width: '600px',
                contentStyle: { "max-height": "600px", "overflow": "auto", "padding-bottom": "50px" },
                styleClass: 'p-dialog-custom',
                baseZIndex: 10000,
                data: {
                    title: "Bạn có chắc chắn muốn xóa mẫu hợp đồng này?",
                    icon: FormNotificationConst.IMAGE_CLOSE,
                },
            }
        );
        ref.onClose.subscribe((dataCallBack) => {
            if (dataCallBack?.accept) {
                this._sampleContractService.delete(sampleContract.id).subscribe((response) => {
                    if (this.handleResponseInterceptor(response,"Xóa mẫu hợp đồng thành công")) {
                        this.setPage({ page: this.page.pageNumber });
                    }
                }, (err) => {
                    console.log('err____', err);
                    this.messageError(`Không xóa được mẫu hợp đồng ${sampleContract.name}`);
                });
            } else {
            }
        });
    }

    setPage(pageInfo?: any) {
        this.page.pageNumber = pageInfo?.page ?? this.offset;
        if (pageInfo?.rows) this.page.pageSize = pageInfo?.rows;

        this.page.keyword = this.keyword;
        this.isLoading = true;
        this._sampleContractService.getAll(this.page, this.fieldFilters).subscribe((res) => {
            this.isLoading = false;
            if (this.handleResponseInterceptor(res, '')) {
                
                this.page.totalItems = res.data.totalItems;
                this.rows = res?.data?.items;
                //
                if (this.rows?.length) {
                    this.genListAction(this.rows);
                    this.showData(this.rows);
                }
            }
        }, (err) => {
            this.isLoading = false;
            console.log('Error-------', err);
            
        });
    }

    formatCurrency(value) {
        return value.toLocaleString('de-DE', { style: 'currency', currency: 'USD' });
    }

}





