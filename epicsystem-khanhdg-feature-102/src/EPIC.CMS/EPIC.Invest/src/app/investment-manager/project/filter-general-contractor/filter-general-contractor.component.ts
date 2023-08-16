import { Component, EventEmitter, Injector, OnInit, Output } from '@angular/core';
import { TableConst } from '@shared/AppConsts';
import { CrudComponentBase } from '@shared/crud-component-base';
import { IColumn } from '@shared/interface/p-table.model';
import { Page } from '@shared/model/page';
import { GeneralContractorService } from '@shared/services/general-contractor.service';
import { MessageService } from 'primeng/api';
import { DynamicDialogRef } from 'primeng/dynamicdialog';

@Component({
  selector: 'app-filter-general-contractor',
  templateUrl: './filter-general-contractor.component.html',
  styleUrls: ['./filter-general-contractor.component.scss']
})
export class FilterGeneralContractorComponent extends CrudComponentBase {

    constructor(
        injector: Injector,
        messageService : MessageService,
        private _generalContractorService : GeneralContractorService,
        private dialogRef: DynamicDialogRef,
    ) { 
        super(injector, messageService)
    }

    @Output() onCloseDialog = new EventEmitter<any>();

    rows: any[] = [];
    page = new Page();
    columns: IColumn[] = [];

    ngOnInit(): void {
        this.setPage();
        this.columns = [
            {field: 'name', header: 'Tên tổng thầu', width: 25, isResize: true, isFrozen: true, alignFrozen: TableConst.alignFrozenColumn.LEFT, class: 'b-border-frozen-left' },
            {field: 'code', header: 'Mã tổng thầu', width: 12 },
            {field: 'taxCode', header: 'Mã số thuế', width: 12 },
            {field: 'repName', header: 'Tên người đại diện', width: 14 },
            {field: 'shortName', header: 'Tên viết tắt', width: 12 },
            {field: 'address', header: 'Địa chỉ giao dịch', width: 25 },
            {field: 'isChoose', header: 'Thao tác', width: 6.5, isFrozen: true, alignFrozen: TableConst.alignFrozenColumn.RIGHT, type: TableConst.columnTypes.ACTION_BUTTON, icon: 'pi pi-check', class: 'b-border-frozen-right' },
        ];
    }

    setPage(event?: Page) {
        this.isLoading = true;
        this._generalContractorService.getAllContractor(this.page).subscribe((res) => {
            this.isLoading = false;
            if (this.handleResponseInterceptor(res, '')) {
                this.page.totalItems = res.data.totalItems;
                this.rows = res.data.items;
                this.setData(this.rows);
            }
        }, () => {
            this.isLoading = false;
        });
    }

    setData(rows) {
        this.rows = rows.map(row => {
            row.address = row?.businessCustomer?.tradingAddress;
            row.isChoose = (row) => this.isChoose(row);
            return row;
        })
    }

    isChoose(row) {
        this.dialogRef.close(row.id);
    }
}