import { Component, EventEmitter, Injector, OnInit, Output } from '@angular/core';
import { SearchConst } from '@shared/AppConsts';
import { CrudComponentBase } from '@shared/crud-component-base';
import { Page } from '@shared/model/page';
import { SaleService } from '@shared/services/sale.service';
import { MessageService } from 'primeng/api';
import { DynamicDialogRef } from 'primeng/dynamicdialog';
import { debounceTime } from "rxjs/operators";

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

  ) 
  { 
    super(injector, messageService)
  }
  @Output() onCloseDialog = new EventEmitter<any>();

  rows: any[] = [];
  page = new Page();
  offset = 0;
  ngOnInit(): void {
    this.rows = [];
    this.subject.keyword.pipe(debounceTime(SearchConst.DEBOUNCE_TIME)).subscribe(() => {
      if (this.keyword.trim()) {
        this.setPage({ page: this.offset });
      } else {
        this.rows = [];
      }
    });
  }

  isChoose(row) {
    this.dialogRef.close(row);
  }

  setPage(pageInfo?: any) {
    let keywordSearch;
    this.isLoading = true;
    if(this.keyword){
      keywordSearch = this.keyword;
    }
    this._saleService.getByTradingProvider(keywordSearch).subscribe((res) => {
      this.isLoading = false;
      if (this.handleResponseInterceptor(res, '')) {
        this.rows[0] = res?.data;
      } else {
        this.rows = [];
      }
    }, () => {
      this.isLoading = false;
    });

  }
}
