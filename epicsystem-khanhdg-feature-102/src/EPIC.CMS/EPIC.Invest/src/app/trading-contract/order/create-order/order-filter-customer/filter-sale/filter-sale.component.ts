import { Component, EventEmitter, Injector, OnInit, Output } from '@angular/core';
import { SaleConst, TableConst } from '@shared/AppConsts';
import { CrudComponentBase } from '@shared/crud-component-base';
import { IColumn } from '@shared/interface/p-table.model';
import { Page } from '@shared/model/page';
import { SaleService } from '@shared/services/sale.service';
import { MessageService } from 'primeng/api';
import { DynamicDialogConfig, DynamicDialogRef } from 'primeng/dynamicdialog';

@Component({
  selector: 'app-filter-sale',
  templateUrl: './filter-sale.component.html',
  styleUrls: ['./filter-sale.component.scss']
})
export class FilterSaleComponent extends CrudComponentBase {
    constructor(
        injector: Injector,
        messageService : MessageService,
        private _saleService: SaleService,
        private readonly dialogRef: DynamicDialogRef,
        public configDialog: DynamicDialogConfig,
    ) { 
        super(injector, messageService)
    }

    @Output() onCloseDialog = new EventEmitter<any>();

    rows: any[] = [];

    SaleConst = SaleConst;
    fieldSearch: string;

    columns: IColumn[] = [];

    ngOnInit(): void {
        this.columns = [
            { field: 'referralCode', header: 'Mã giới thiệu', width: 10 },
            { field: 'customerName', header: 'Người giới thiệu', width: 18 },
            { field: 'customerPhone', header: 'Số điện thoại', width: 10 },
            { field: 'customerEmail', header: 'Thư điện tử', width: 18 },
            {   
                field: 'isChoose', header: 'Chọn', width: 5, 
                type: TableConst.columnTypes.ACTION_BUTTON, icon: 'pi pi-check', isFrozen: true, alignFrozen: TableConst.alignFrozenColumn.RIGHT, 
                class: 'b-border-frozen-right' 
            },
        ]
    }

    setPage() {
        this.isLoading = true;
        this._saleService.getByTradingProvider(this.page.keyword, this.fieldSearch).subscribe((res) => {
            this.isLoading = false;
            if (this.page.keyword && this.handleResponseInterceptor(res, '') && res?.data) {
                let row = res?.data;
                row = {
                    ...row,
                    customerName: row?.investor?.investorIdentification?.fullname || row?.businessCustomer?.name,
                    customerPhone: row?.investor?.phone || row?.businessCustomer?.phone,
                    customerEmail: row?.investor?.email || row?.businessCustomer?.email,
                    isChoose: (row) => this.isChoose(row), 
                }
                this.rows = [row];
            } else {
                this.rows = [];
            }
        }, () => {
            this.isLoading = false;
        });
    }

    isChoose(row) {
        this.dialogRef.close(row);
    }
}
