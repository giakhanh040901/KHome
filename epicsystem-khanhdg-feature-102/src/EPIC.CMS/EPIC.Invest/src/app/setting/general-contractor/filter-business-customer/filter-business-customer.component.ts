import { Component, EventEmitter, Injector, OnInit, Output } from '@angular/core';
import { IssuerConst, TableConst } from '@shared/AppConsts';
import { CrudComponentBase } from '@shared/crud-component-base';
import { Page } from '@shared/model/page';
import { MessageService } from 'primeng/api';
import { DynamicDialogRef } from 'primeng/dynamicdialog';
import { GeneralContractorService } from '@shared/services/general-contractor.service';
import { IColumn } from '@shared/interface/p-table.model';

@Component({
  selector: 'app-filter-business-customer',
  templateUrl: './filter-business-customer.component.html',
  styleUrls: ['./filter-business-customer.component.scss'], 
})
export class FilterBusinessCustomerComponent extends CrudComponentBase{

    constructor(
        injector: Injector,
        messageService: MessageService,
        private generalContractorService: GeneralContractorService,
        private readonly dialogRef: DynamicDialogRef,
        ) {
        super(injector, messageService);
    }

    @Output() onCloseDialog = new EventEmitter<any>();

    rows: any[] = [];

    IssuerConst = IssuerConst

    page = new Page();
    columns: IColumn[] = [];

    ngOnInit(): void {
        this.columns = [
            { field: 'taxCode', header: 'Mã số thuế', width: 10 },
            { field: 'name', header: 'Tên doanh nghiệp', width: 20 },
            { field: 'shortName', header: 'Tên viết tắt', width: 12 },
            { field: 'email', header: 'Thư điện tử', width: 16 },
            { field: 'phone', header: 'Số điện thoại', width: 10 },
            { field: 'address', header: 'Địa chỉ', width: 25 },
            { field: 'repName', header: 'Người đại diện', width: 12 },
            { field: 'repPosition', header: 'Chức vụ', width: 10 },
            { field: 'isChoose', header: 'Chọn', width: 8, type: TableConst.columnTypes.ACTION_BUTTON, icon: 'pi pi-check', isFrozen: true, alignFrozen: TableConst.alignFrozenColumn.RIGHT, class: 'b-border-frozen-right' },
        ];
    }
    //
    setPage() {
        this.isLoading = true;
        this.generalContractorService.getByTaxCode(this.page.keyword).subscribe((res) => {
            this.isLoading = false;
            if (this.handleResponseInterceptor(res, '') && res?.data) {
                let row = {
                    ...res.data,
                    isChoose: (row) => this.isChoose(row),
                };
                this.page.totalItems = res.data.totalItems;
                this.rows = [row];
            } else {
                this.messageError('Không tìm thấy dữ liệu');
            }
        }, () => {
            this.isLoading = false;
        });
    }

    isChoose(businessCustomer) {
        this.dialogRef.close(businessCustomer);
    }
}
