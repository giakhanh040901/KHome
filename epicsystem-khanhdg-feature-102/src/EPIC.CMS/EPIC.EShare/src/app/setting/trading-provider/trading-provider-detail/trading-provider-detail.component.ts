import { Component, EventEmitter, Injector, OnInit, Output, ViewChild } from '@angular/core';
import { SearchConst } from '@shared/AppConsts';
import { CrudComponentBase } from '@shared/crud-component-base';
import { Page } from '@shared/model/page';
import { TradingProviderServiceProxy } from '@shared/service-proxies/setting-service';
import { MessageService } from 'primeng/api';
import { debounceTime } from 'rxjs/operators';
import { DynamicDialogRef } from 'primeng/dynamicdialog';
import { DynamicDialogConfig } from 'primeng/dynamicdialog';
import { ActivatedRoute } from '@angular/router';
import { BreadcrumbService } from 'src/app/layout/breadcrumb/breadcrumb.service';
import { NationalityConst } from '@shared/nationality-list';
import { TabView } from 'primeng/tabview';


@Component({
  selector: 'app-trading-provider-detail',
  templateUrl: './trading-provider-detail.component.html',
  styleUrls: ['./trading-provider-detail.component.scss']
})
export class TradingProviderDetailComponent extends CrudComponentBase {

  constructor(
    injector: Injector,
    
    messageService: MessageService,
    private routeActive: ActivatedRoute,
    private tradingProviderService: TradingProviderServiceProxy,
    private breadcrumbService: BreadcrumbService
  ) {
    super(injector, messageService);

    this.breadcrumbService.setItems([
      { label: 'Trang chủ', routerLink: ['/home'] },
      { label: 'Đại lý sơ cấp', routerLink: ['/setting/trading-provider']  },
      { label: 'Chi tiết đại lý sơ cấp',},
    ]);

    this.tradingProviderId = +this.cryptDecode(this.routeActive.snapshot.paramMap.get('id'));
 
   }

   tradingProviderId: number;
   tradingProviderDetail: any;

   tabViewActive = {
    'thongTinChung': true,
    'taiKhoanDangNhap': false,
  };

  @ViewChild(TabView) tabView: TabView;

   NationalityConst = NationalityConst;
   
   ngOnInit(): void {
     this.isLoading = true;
     this.tradingProviderService.getTradingProvider(this.tradingProviderId).subscribe(
       (res) => {
        this.isLoading = false;
        if (this.handleResponseInterceptor(res, '')) {
            this.tradingProviderDetail = res.data;
            this.tradingProviderDetail = {
            ...this.tradingProviderDetail,
            licenseDate: this.tradingProviderDetail?.licenseDate ? new Date(this.tradingProviderDetail?.licenseDate): null,
            decisionDate: this.tradingProviderDetail?.decisionDate ? new Date(this.tradingProviderDetail?.decisionDate): null,
            dateModified: this.tradingProviderDetail?.dateModified ? new Date(this.tradingProviderDetail?.dateModified): null,
            };
            console.log({ tradingProviderDetail: this.tradingProviderDetail });
        }
     }, (err) => {
        this.isLoading = false;
        console.log('Error-------', err);
        
    });
   }

   changeTabview(e) {
    let tabHeader = this.tabView.tabs[e.index].header;
    this.tabViewActive[tabHeader] = true;
  }
}
