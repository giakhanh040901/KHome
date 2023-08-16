import { Component, Injector, Input, OnInit, ViewChild } from '@angular/core';
import { CompanySharePrimaryConst, CompanyShareInfoConst, CompanyShareDetailConst, DistributionContractConst } from '@shared/AppConsts';
import { CrudComponentBase } from '@shared/crud-component-base';
import { Page } from '@shared/model/page';
import { DistributionContractServiceProxy, CompanySharePrimaryServiceProxy } from '@shared/service-proxies/company-share-manager-service';
import { MessageService } from 'primeng/api';
import { forkJoin } from 'rxjs';
import { BreadcrumbService } from 'src/app/layout/breadcrumb/breadcrumb.service';
import { Router } from "@angular/router"
import { ActivatedRoute } from '@angular/router';
import { DialogService } from 'primeng/dynamicdialog';
import { DynamicDialogRef } from 'primeng/dynamicdialog';
import { TradingProviderServiceProxy } from '@shared/service-proxies/setting-service';
import { FilterTradingProviderComponent } from '../filter-trading-provider/filter-trading-provider.component';
import { TabView } from 'primeng/tabview';

@Component({
  selector: 'app-distribution-contract-detail',
  templateUrl: './distribution-contract-detail.component.html',
  styleUrls: ['./distribution-contract-detail.component.scss'],
  providers: [DialogService]
})
export class DistributionContractDetailComponent extends CrudComponentBase {

  constructor(
    injector: Injector,
    messageService: MessageService,
    private routeActive: ActivatedRoute,
    private _distributionContractService: DistributionContractServiceProxy,
    private breadcrumbService: BreadcrumbService
    ){
    super(injector, messageService);
    
    this.breadcrumbService.setItems([
      { label: 'Trang chủ', routerLink: ['/home'] },
      { label: 'Hợp đồng phân phối', routerLink: ['/company-share-manager/distribution-contract']  },
      { label: 'Chi tiết hợp đồng phân phối',},
    ]);

    this.distributionContractId = +this.cryptDecode(this.routeActive.snapshot.paramMap.get('id'));
  }

  ref: DynamicDialogRef;

  distributionContractId: number;
  distributionContractDetail: any = {};

  tabViewActive = {
    'thongTinChung': true,
    'thongTinThanhToan': false,
    'danhMucHoSo': false,
    'thongTinTraiTuc': false,
  };

  @ViewChild(TabView) tabView: TabView;

  DistributionContractConst = DistributionContractConst;

  activeIndex = 0;

  ngOnInit() {
    this.isLoading = true;
    this._distributionContractService.get(this.distributionContractId).subscribe(
      (res) => {
        this.isLoading = false;
        if (this.handleResponseInterceptor(res, '')) {
            this.distributionContractDetail = res.data;
            console.log("hello", this.distributionContractDetail?.tradingProvider);
            console.log({ distributionContractDetail: res.data });
        }
    },(err) => {
        this.isLoading = false;
        console.log('Error', err);
        
    });
  }

  changeTabview(e) {
    let tabHeader = this.tabView.tabs[e.index].header;
    this.tabViewActive[tabHeader] = true;
  }
}
