import { Component, EventEmitter, Injector, OnInit, Output } from '@angular/core';
import { IssuerConst, SearchConst } from '@shared/AppConsts';
import { CrudComponentBase } from '@shared/crud-component-base';
import { Page } from '@shared/model/page';
import { IssuerServiceProxy, TradingProviderServiceProxy } from '@shared/service-proxies/setting-service';
import { MessageService } from 'primeng/api';
import { debounceTime } from 'rxjs/operators';
import { DynamicDialogRef } from 'primeng/dynamicdialog';
import { DynamicDialogConfig } from 'primeng/dynamicdialog';

@Component({
  selector: 'app-filter-business-customer',
  templateUrl: './filter-business-customer.component.html',
  styleUrls: ['./filter-business-customer.component.scss'], 
})
export class FilterBusinessCustomerComponent extends CrudComponentBase {

  constructor(
    injector: Injector,
    private dialogConfig: DynamicDialogConfig,
    private issuerService: IssuerServiceProxy,
    private tradingprovider: TradingProviderServiceProxy,
    private readonly dialogRef: DynamicDialogRef,
    messageService: MessageService,
    ) {
    super(injector, messageService);
  }

  @Output() onCloseDialog = new EventEmitter<any>();

  rows: any[] = [];

  IssuerConst = IssuerConst

  expandedRows = {}

  page = new Page();
  offset = 0;

  ngOnInit(): void {
    // this.setPage({ page: this.offset });
    this.subject.keyword.pipe(debounceTime(SearchConst.DEBOUNCE_TIME)).subscribe(() => {
      if (this.keyword === "") {
        this.rows = [];
        // this.setPage({ page: this.offset });
      } else {
        this.setPage();
      }
    });
  }

  setPage(pageInfo?: any) {
    this.isLoading = true;

    this.issuerService.getAll(this.keyword?.trim()).subscribe((res) => {
      this.isLoading = false;
      if (this.handleResponseInterceptor(res, '') && res?.data) {
        this.rows = [res.data];
        console.log({ rows: res.data.items, totalItems: res.data.totalItems });
      } else {
        this.messageService.add({ severity: 'error', detail: 'Không tìm thấy dữ liệu', life: 2000 });
      }
    }, () => {
      this.isLoading = false;
    });

    // this.tradingprovider.getAll(this.page).subscribe((res) => {
    //   if (this.handleResponseInterceptor(res, '')) {
    //     this.page.totalItems = res.data.totalItems;
    //     this.rows = res.data.items;
    //     if(this.keyword == "") this.rows = [];
    //     console.log({ rows: res.data.items, totalItems: res.data.totalItems });
    //   }
    //   this.isLoading = false;
    // }, () => {
    //   this.isLoading = false;
    // });
  }

  isChoose(businessCustomerId) {
    this.dialogRef.close(businessCustomerId);
  }

}
