import { NationalityConst } from './../../../../shared/nationality-list';
import { Component, EventEmitter, Injector, OnInit, Output } from '@angular/core';
import { DepositProviderConst, SearchConst } from '@shared/AppConsts';
import { CrudComponentBase } from '@shared/crud-component-base';
import { Page } from '@shared/model/page';
import { DepositProviderServiceProxy } from '@shared/service-proxies/setting-service';
import { MessageService } from 'primeng/api';
import { debounceTime } from 'rxjs/operators';
import { DynamicDialogRef } from 'primeng/dynamicdialog';
import { DynamicDialogConfig } from 'primeng/dynamicdialog';
import { ActivatedRoute } from '@angular/router';
import { BreadcrumbService } from 'src/app/layout/breadcrumb/breadcrumb.service';


@Component({
  selector: 'app-deposit-provider-detail',
  templateUrl: './deposit-provider-detail.component.html',
  styleUrls: ['./deposit-provider-detail.component.scss']
})
export class DepositProviderDetailComponent  extends CrudComponentBase  {

  constructor(
    injector: Injector,
    
    messageService: MessageService,
    private routeActive: ActivatedRoute,
    private depositProviderService:  DepositProviderServiceProxy,
    private breadcrumbService: BreadcrumbService
  ) { 
    super(injector, messageService);

    this.breadcrumbService.setItems([
      { label: 'Trang chủ', routerLink: ['/home'] },
      { label: 'Đại lý lưu ký', routerLink: ['/setting/deposit-provider']  },
      { label: 'Chi tiết đại lý lưu ký',},
    ]);
    this.depositProviderId = +this.cryptDecode(this.routeActive.snapshot.paramMap.get('id'));
  }

  depositProviderId: number;
  depositProviderDetail: any = {};

  DepositProviderConst = DepositProviderConst;

  NationalityConst = NationalityConst;

  ngOnInit(): void {
    this.isLoading = true;
    this.depositProviderService.getDepositProvider(this.depositProviderId).subscribe(
      (res) => {
        this.isLoading = false;
        if (this.handleResponseInterceptor(res, '')) {
            this.depositProviderDetail = res.data;
            // this.depositProviderDetail = {
            //   ...this.depositProviderDetail,
            //   licenseDate: this.depositProviderDetail?.licenseDate ? new Date(this.depositProviderDetail?.licenseDate): null,
            //   decisionDate: this.depositProviderDetail?.decisionDate ? new Date(this.depositProviderDetail?.decisionDate): null,
            //   dateModified: this.depositProviderDetail?.dateModified ? new Date(this.depositProviderDetail?.dateModified): null,
            // };
            console.log({ depositProviderDetail: res });    
        }
    }, (err) => {
        this.isLoading = false;
        console.log('Error-------', err);
        
    });
  }
 
}
