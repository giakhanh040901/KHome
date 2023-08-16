import { Component, Injector, Input, OnInit } from '@angular/core';
import { CrudComponentBase } from '@shared/crud-component-base';
import { ConfirmationService, MessageService } from 'primeng/api';
import { BreadcrumbService } from 'src/app/layout/breadcrumb/breadcrumb.service';
import { forkJoin } from 'rxjs';
import { debounceTime } from 'rxjs/operators';
import { PageBondPolicyTemplate } from '@shared/model/pageBondPolicyTemplate';
import {  SearchConst, FormNotificationConst, ActiveDeactiveConst, ContractFormConst } from '@shared/AppConsts';
import { DialogService } from 'primeng/dynamicdialog';
import { FormSetDisplayColumnComponent } from 'src/app/form-general/form-set-display-column/form-set-display-column/form-set-display-column.component';
import { FormNotificationComponent } from 'src/app/form-general/form-notification/form-notification.component';
import { ContractFormService } from '@shared/services/contract-form.service';
import { CreateContractCodeComponent } from './create-contract-code/create-contract-code.component';
import { Page } from '@shared/model/page';
import { IContractCodeStructure } from '@shared/interfaces/contractCodeStructure.interface';
import { IBaseListAction, IColumn } from '@shared/interfaces/base.interface';

@Component({
	selector: 'app-contract-code-structure',
	templateUrl: './contract-code-structure.component.html',
	styleUrls: ['./contract-code-structure.component.scss'],
    providers: [DialogService, ConfirmationService, MessageService]
})
export class ContractCodeStructureComponent extends CrudComponentBase {

    constructor(
        injector: Injector,
        messageService: MessageService,
        private dialogService: DialogService,
        private breadcrumbService: BreadcrumbService,
        public confirmationService: ConfirmationService,
        private _dialogService: DialogService,
		private _contractFormService: ContractFormService,
    ) {
        super(injector, messageService);
        this.breadcrumbService.setItems([
            { label: 'Trang chủ', routerLink: ['/home'] },
            { label: 'Cấu trúc mã hợp đồng' },
        ]);
    }

    rows: IContractCodeStructure[] = [];
    ActiveDeactiveConst = ActiveDeactiveConst;
    ContractFormConst = ContractFormConst;
    _selectedColumns: IColumn[];
    cols: IColumn[];
    listAction: IBaseListAction[] = [];
    page = new Page();
    offset = 0;
    fieldFilters = {
        status: null,
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
            { field: 'name', header: 'Tên cấu hình', width: '10rem', isPin: true },
            { field: 'contractCode', header: 'Cấu trúc mã hợp đồng', width: '30rem', cutText:'b-cut-text-30' },
            { field: 'description', header: 'Ghi chú', width: '12rem' },
            { field: 'createdBy', header: 'Người tạo', width: '10rem' },
            { field: 'createdDateDisplay', header: 'Ngày tạo', width: '20rem' },
            { field: 'columnResize', header: '', type:'hidden' },
        ];
        
        this.cols = this.cols.map((item, index) => {
            item.position = index + 1;
            return item;
          });
        this._selectedColumns = this.getLocalStorage('contractCodeStructureGan') ?? this.cols;
    }

    showData(rows) {
        for (let row of rows) {
            // row.id = row.id,
            row.code = row.code;
            row.name = row.name;
			row.customerTypeGetAll = ContractFormConst.getCustomerInfo(row.customerType, 'name');
            row.contractCode = this.genContractCodeStructure(row?.configContractCodeDetails);
            row.createdDateDisplay = this.formatDateTime(row?.createdDate);
        }
    }

    changeStatusFilter() {
        this.setPage(this.getPageCurrentInfo());
    }

    setColumn(col, _selectedColumns) {
        const ref = this.dialogService.open(
            FormSetDisplayColumnComponent,
            this.getConfigDialogServiceDisplayTableColumn(col, _selectedColumns)
        );
        //
        ref.onClose.subscribe((dataCallBack) => {
            if (dataCallBack?.accept) {
                this._selectedColumns = dataCallBack.data.sort(function (a, b) {
                    return a.position - b.position;
                });
                this.setLocalStorage(this._selectedColumns, 'contractCodeStructureGan')
            }
        });
    }

    genContractCodeStructure(configContractCodeDetails = []) {
        let contractCodeStructure = '';
        if(configContractCodeDetails?.length) {
            configContractCodeDetails.forEach(element => {
                contractCodeStructure += '<'+ (element?.value ? element.value : element?.key) +'>';
            });
        }
        //
        return contractCodeStructure;
    }

    genListAction(rows = []) {
        this.listAction = rows.map((row, index) => {

            const actions = [];

            if (this.isGranted([this.PermissionGarnerConst.GarnerCauHinhMaHD_CapNhat])) {
                actions.push({
                    data: row,
                    index: index,
                    label: 'Sửa',
                    icon: 'pi pi-pencil',
                    command: ($event) => {
                        this.edit($event.item.data);
                    }
                })
            }

            if ((row.status == ActiveDeactiveConst.ACTIVE || row.status == ActiveDeactiveConst.DEACTIVE) && this.isGranted([this.PermissionGarnerConst.GarnerCauHinhMaHD_CapNhat])) {
                actions.push({
                    data: row,
                    index: index,
                    label: row?.status == ActiveDeactiveConst.ACTIVE ? 'Khóa' : 'Kích hoạt',
                    icon: row?.status == ActiveDeactiveConst.ACTIVE ? 'pi pi-times-circle' : 'pi pi-check-circle',
                    command: ($event) => {
                        this.changeStatus($event.item.data);
                    }
                })
            }

            if (this.isGranted([this.PermissionGarnerConst.GarnerCauHinhMaHD_Xoa])) {
                actions.push({
                    data: row,
                    index: index,
                    label: 'Xóa',
                    icon: 'pi pi-trash',
                    command: ($event) => {
                        this.delete($event.item.data);
                    }
                })
            }

            return actions;
        });
    }

    changeStatus(item) {
        const ref = this._dialogService.open(
            FormNotificationComponent,
            {
                header: "Thông báo",
                width: '350px',
                contentStyle: { "max-height": "600px", "overflow": "auto", "padding-bottom": "50px" },
                styleClass: 'p-dialog-custom',
                baseZIndex: 10000,
                data: {	
                    title: item.status == ActiveDeactiveConst.ACTIVE ? 'Khóa mẫu cấu hình mã hợp đồng?' : 'Kích hoạt mẫu cấu hình mã hợp đồng?',
                    icon: item.status == ActiveDeactiveConst.ACTIVE ? FormNotificationConst.IMAGE_CLOSE : FormNotificationConst.IMAGE_APPROVE,
                },
            }
        );
        
        ref.onClose.subscribe((dataCallBack) => {
            if (dataCallBack?.accept) {
                this._contractFormService.updateStatus(item).subscribe((res) => {
                    this.isLoading = false;
                    if (this.handleResponseInterceptor(res, 'Cập nhật thành công')) {
                        this.setPage(this.getPageCurrentInfo());
                    }
                }, (err) => {
                    this.isLoading = false;
                    console.log('Error-------', err);
                });
            } 
        });

    }

    create() {
		const ref = this.dialogService.open(CreateContractCodeComponent, {
            header: "Thêm mới cấu trúc hợp đồng",
            width: '800px',
            contentStyle: { "max-height": "600px", overflow: "auto", "margin-bottom": "60px", },
            baseZIndex: 10000,
            data: {
            
            },
        });
    //
    ref.onClose.subscribe((res) => {
        if(res) {
            this.setPage(this.getPageCurrentInfo());
        }
    });
	}

    edit(contractCodeStructure) {
		const ref = this.dialogService.open(CreateContractCodeComponent, {
			header: "Sửa cấu trúc hợp đồng",
			width: '800px',
			contentStyle: { "max-height": "600px", overflow: "auto", "margin-bottom": "60px", },
			baseZIndex: 10000,
			data: {
				contractCodeStructure : contractCodeStructure
			},
		});
		//
		ref.onClose.subscribe((res) => {
            if(res) {
                this.setPage(this.getPageCurrentInfo());
            }
		});
    }

    delete(contractCodeStructure) {
        const ref = this._dialogService.open(
            FormNotificationComponent,
            {
                header: "Thông báo",
                width: '600px',
                contentStyle: { "max-height": "600px", "overflow": "auto", "padding-bottom": "50px" },
                styleClass: 'p-dialog-custom',
                baseZIndex: 10000,
                data: {
                    title: "Bạn có chắc chắn muốn xóa cấu trúc này?",
                    icon: FormNotificationConst.IMAGE_CLOSE,
                },
            }
        );
        ref.onClose.subscribe((dataCallBack) => {
            if (dataCallBack?.accept) {
                this._contractFormService.delete(contractCodeStructure.id).subscribe((response) => {
                    if (this.handleResponseInterceptor(response,"Xóa thành công")) {
                        this.setPage(this.getPageCurrentInfo());
                    }
                }, (err) => {
                    console.log('err____', err);
                    this.messageError(`Không xóa được ${contractCodeStructure.name}`);
                });
            } else {
            }
        });
    }

    getPageCurrentInfo() {
		return {page: this.page.pageNumber, rows: this.page.pageSize};
	}

    setPage(pageInfo?: any) {
        this.page.pageNumber = pageInfo?.page ?? this.offset;
        if (pageInfo?.rows) this.page.pageSize = pageInfo?.rows;

        this.page.keyword = this.keyword;
        this.isLoading = true;
        this._contractFormService.getAll(this.page, this.fieldFilters).subscribe((res) => {
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




