import { Component, Injector } from '@angular/core';
import { CrudComponentBase } from '@shared/crud-component-base';
import { ConfirmationService, MessageService } from 'primeng/api';
import { BreadcrumbService } from 'src/app/layout/breadcrumb/breadcrumb.service';
import { FormNotificationConst, ContractFormConst, TableConst } from '@shared/AppConsts';
import { DialogService } from 'primeng/dynamicdialog';
import { ContractFormService } from '@shared/services/contract-form.service';
import { CreateContractCodeComponent } from './create-contract-code/create-contract-code.component';
import { FormNotificationComponent } from 'src/app/form-general/form-notification/form-notification.component';
import { Page } from '@shared/model/page';
import { DataTableEmit, IColumn, Sort } from '@shared/interface/p-table.model';
import { DataFilter } from '@shared/interface/contract-code-structure.model';
import { BasicFilter } from '@shared/interface/filter.model';

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

    ContractFormConst = ContractFormConst;

    listAction: any[] = [];
    page = new Page();

    dataFilter: BasicFilter = new BasicFilter();

    rows: any[] = [];
	columns: IColumn[] = [];
	dataTableEmit: DataTableEmit = new DataTableEmit();

    ngOnInit(): void {
        // Xử lý ẩn hiện cột trong bảng
        this.columns = [
			{ field: 'id', header: '#ID', width: 5, isPin: true, isFrozen: true, alignFrozen: TableConst.alignFrozenColumn.LEFT, class: 'b-border-frozen-left', isSort: true },
            { field: 'name', header: 'Tên cấu hình', width: 20, isPin: true, isSort: true},
            { field: 'contractCode', header: 'Cấu trúc mã hợp đồng', width: 30},
            { field: 'description', header: 'Ghi chú', width: 20},
            { field: 'createdBy', header: 'Người tạo', width: 10},
            { field: 'createdDate', header: 'Ngày tạo', width: 12, isResize: true, isSort: true},
			{ field: 'status', header: 'Trạng thái', width: 8.5, isFrozen: true, alignFrozen: TableConst.alignFrozenColumn.RIGHT, type: TableConst.columnTypes.STATUS, class: 'justify-content-left b-border-frozen-right' },
			{ field: '', header: '', width: 4, displaySettingColumn: false, isFrozen: true, alignFrozen: TableConst.alignFrozenColumn.RIGHT, type: TableConst.columnTypes.ACTION_DROPDOWN, class: 'justify-content-end b-table-actions' },
        ];
        //
        this.setPage();
    }

    setPage(event?: Page) {
        if(!event) this.page.pageNumber = 0;
        this.isLoading = true;
        this._contractFormService.getAll(this.page, this.dataFilter).subscribe((res) => {
            this.isLoading = false;
            if (this.handleResponseInterceptor(res, '')) {
                this.page.totalItems = res.data.totalItems;
                this.rows = res?.data?.items;
                //
                if (this.rows?.length) {
                    this.genListAction(this.rows);
                    this.setData(this.rows);
                }
            }
        }, (err) => {
            this.isLoading = false;
            console.log('Error-------', err);
            
        });
    }

    setData(rows) {
        for (let row of rows) {
			row.customerTypeGetAll = ContractFormConst.getCustomerInfo(row.customerType, 'name');
            row.contractCode = this.genContractCodeStructure(row?.configContractCodeDetails);
            row.createdDate = this.formatDateTime(row?.createdDate);
            //
			row.statusElement = ContractFormConst.getStatusInfo(row?.status);
        }
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

    onSort(event) {
		this.dataFilter.sortFields = event;
		this.setPage();
	}

    genListAction(data = []) {
        this.listAction = data.map((contractCodeStructure, index) => {
            const actions = [];
            if (this.isGranted([this.PermissionInvestConst.InvestCauHinhMaHD_CapNhat])) {
                actions.push({
                    data: contractCodeStructure,
                    index: index,
                    label: 'Sửa',
                    icon: 'pi pi-pencil',
                    command: ($event) => {
                        this.edit($event.item.data);
                    }
                })
            }

            if (this.isGranted([this.PermissionInvestConst.InvestCauHinhMaHD_Xoa])) {
                actions.push({
                    data: contractCodeStructure,
                    label: 'Xoá',
                    icon: 'pi pi-trash',
                    command: ($event) => {
                        this.delete($event.item.data);
                    }
                })
            }
            return actions;
        });
    }

    create() {
		this.dialogService.open(CreateContractCodeComponent, {
            header: "Thêm mới cấu trúc hợp đồng",
            width: '800px',
        }).onClose.subscribe((res) => {
            if(res) {
                this.setPage();
            }
        });
	}

    edit(contractCodeStructure) {
      this.dialogService.open(CreateContractCodeComponent, {
        header: "Sửa cấu trúc hợp đồng",
        width: '800px',
        data: {
            configContractCodeId : contractCodeStructure?.id
        },
      }).onClose.subscribe((res) => {
        this.setPage();
      });
    }

    delete(contractCodeStructure) {
        this._dialogService.open(
            FormNotificationComponent,
            {
                header: "Thông báo",
                width: '600px',
                data: {
                    title: "Bạn có chắc chắn muốn xóa cấu trúc này?",
                    icon: FormNotificationConst.IMAGE_CLOSE,
                },
            }
        ).onClose.subscribe((dataCallBack) => {
            if (dataCallBack?.accept) {
                this._contractFormService.delete(contractCodeStructure.id).subscribe((response) => {
                    if (this.handleResponseInterceptor(response,"Xóa thành công")) {
                        this.setPage(this.page);
                    }
                }, (err) => {
                    console.log('err____', err);
                    this.messageError(`Không xóa được ${contractCodeStructure.name}`);
                });
            } else {
            }
        });
    }

}





