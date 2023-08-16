import { Component, Injector, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { CrudComponentBase } from '@shared/crud-component-base';
import { NationalityConst } from '@shared/nationnalality-list';
import { TradingProviderServiceProxy } from '@shared/service-proxies/trading-provider-service';
import { MessageService } from 'primeng/api';
import { BreadcrumbService } from 'src/app/layout/breadcrumb/breadcrumb.service';

@Component({
    selector: 'app-trading-provider-detail',
    templateUrl: './trading-provider-detail.component.html',
    styleUrls: ['./trading-provider-detail.component.scss']
})
export class TradingProviderDetailComponent extends CrudComponentBase {

    constructor(
        injector: Injector,
        messageService: MessageService,
        private activatedRoute: ActivatedRoute,
        private routeActive: ActivatedRoute,
        private tradingProviderService: TradingProviderServiceProxy,
        private breadcrumbService: BreadcrumbService
    ) {
        super(injector, messageService);

        this.breadcrumbService.setItems([
            { label: 'Trang chủ', routerLink: ['/home'] },
            { label: 'Danh sách tài khoản', routerLink: ['/user/list'] },
            { label: 'Thông tin đại lý', },
        ]);
        this.tradingProviderId = this.routeActive.snapshot.paramMap.get('id');
    }
    

    tradingProviderId: string;
    tradingProviderDetail: any;

    NationalityConst = NationalityConst;

    ngOnInit(): void {
        this.isLoading = true;
        this.tradingProviderService.getTradingProvider(+this.tradingProviderId).subscribe(
            (res) => {
                this.isLoading = false;
                if (this.handleResponseInterceptor(res, '')) {
                    this.tradingProviderDetail = res.data;
                    this.tradingProviderDetail = {
                        ...this.tradingProviderDetail,
                        licenseDate: this.tradingProviderDetail?.licenseDate ? new Date(this.tradingProviderDetail?.licenseDate) : null,
                        decisionDate: this.tradingProviderDetail?.decisionDate ? new Date(this.tradingProviderDetail?.decisionDate) : null,
                        dateModified: this.tradingProviderDetail?.dateModified ? new Date(this.tradingProviderDetail?.dateModified) : null,
                    };
                }
            }, (err) => {
                this.isLoading = false;
            });
    }
}