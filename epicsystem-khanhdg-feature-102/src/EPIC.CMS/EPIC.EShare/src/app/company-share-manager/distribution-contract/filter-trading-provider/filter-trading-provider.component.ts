import { Component, EventEmitter, Injector, OnInit, Output } from '@angular/core';
import { IssuerConst, SearchConst } from '@shared/AppConsts';
import { CrudComponentBase } from '@shared/crud-component-base';
import { Page } from '@shared/model/page';
import { TradingProviderServiceProxy } from '@shared/service-proxies/setting-service';
import { MessageService } from 'primeng/api';
import { debounceTime } from 'rxjs/operators';
import { DynamicDialogRef } from 'primeng/dynamicdialog';
import { DynamicDialogConfig } from 'primeng/dynamicdialog';

@Component({
  selector: 'app-filter-trading-provider',
  templateUrl: './filter-trading-provider.component.html',
  styleUrls: ['./filter-trading-provider.component.scss'],
})
export class FilterTradingProviderComponent extends CrudComponentBase {

  constructor(
    injector: Injector,
    private dialogConfig: DynamicDialogConfig,
    private tradingProviderService: TradingProviderServiceProxy,
    private readonly dialogRef: DynamicDialogRef,
    messageService: MessageService,
    ) {
    super(injector, messageService);
  }

  @Output() onCloseDialog = new EventEmitter<any>();

  rows: any[] = [];

  TradingProviderConst = IssuerConst

  expandedRows = {}
  page = new Page();
  offset = 0;

  ngOnInit(): void {
    this.subject.keyword.pipe(debounceTime(SearchConst.DEBOUNCE_TIME)).subscribe(() => {
      if (this.keyword === "") {
        this.rows = []
      } else {
        this.setPage();
      }
    });
  }

  setPage(pageInfo?: any) {
    this.page.pageNumber = pageInfo?.page ?? this.offset;
    this.page.keyword = this.keyword;
    this.isLoading = true;
    if ( this.keyword.trim() != '') {
      this.tradingProviderService.getTradingProviderTaxCode(this.keyword).subscribe((res) => {
        this.isLoading = false;
        if (this.handleResponseInterceptor(res, '')) {
          if(res?.data) {
            this.rows = res?.data;
          }

          console.log({ rows: res?.data });
        }

      }, () => {
        this.isLoading = false;
      });
    } else {
      this.isLoading = false;
    }
  }

  isChoose(tradingProviderInfo) {
    this.dialogRef.close(tradingProviderInfo);
  }

}
