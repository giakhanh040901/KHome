import { Component, EventEmitter, Injector, OnInit, Output } from '@angular/core';
import { TableConst } from '@shared/AppConsts';
import { CrudComponentBase } from '@shared/crud-component-base';
import { IColumn } from '@shared/interface/p-table.model';
import { Page } from '@shared/model/page';
import { OwnerServiceProxy } from '@shared/services/owner-service';
import { MessageService } from 'primeng/api';
import { DynamicDialogRef } from 'primeng/dynamicdialog';

@Component({
  selector: 'app-filter-owner',
  templateUrl: './filter-owner.component.html',
  styleUrls: ['./filter-owner.component.scss']
})
export class FilterOwnerComponent extends CrudComponentBase {

    constructor(
        injector: Injector,
        messageService : MessageService,
        private _ownerService : OwnerServiceProxy,
        private readonly dialogRef: DynamicDialogRef,
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
            { 
                field: 'name', header: 'Tên chủ đầu tư', width: 25, isResize: true, isFrozen: true, alignFrozen: TableConst.alignFrozenColumn.LEFT, 
                class: 'b-border-frozen-left' 
            },
            { field: 'code', header: 'Mã chủ đầu tư', width: 12 },
            { field: 'shortName', header: 'Tên viết tắt', width: 12 },
            { field: 'hotline', header: 'Hotline', width: 12 },
            { field: 'website', header: 'Website', width: 12 },
            { field: 'fanpage', header: 'Fanpage', width: 12 },
            { field: 'businessTurnover', header: 'Doanh thu', width: 12 },
            { field: 'businessProfit', header: 'Lợi nhuận sau thuế', width: 12 },
            { field: 'roa', header: 'ROA', width: 12 },
            { field: 'roe', header: 'ROE', width: 12 },
            {
                field: 'isChoose', header: 'Thao tác', width: 6.5, isFrozen: true, alignFrozen: TableConst.alignFrozenColumn.RIGHT, 
                type: TableConst.columnTypes.ACTION_BUTTON, icon: 'pi pi-check', class: 'b-border-frozen-right' 
            },
        ];
    }

    isChoose(row) {
        this.dialogRef.close(row.id);
    }

    setPage(event?: Page) {
        this.isLoading = true;
        this._ownerService.getAllOwner(this.page).subscribe((res) => {
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
            row.name = row.businessCustomer.name;
            row.code = row.businessCustomer.code;
            row.shortName = row.businessCustomer.shortName;
            row.isChoose = (row) => this.isChoose(row);
            return row;
        })
    }
}
