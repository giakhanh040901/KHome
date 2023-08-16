import { Component, Injector, OnInit } from '@angular/core';
import { ActivatedRoute, RouterLinkActive } from '@angular/router';
import { InvestorAccountConst } from '@shared/AppConsts';
import { CrudComponentBase } from '@shared/crud-component-base';
import { Page } from '@shared/model/page';
import { UserServiceProxy } from '@shared/service-proxies/service-proxies';
import { TradingAccountService } from '@shared/service-proxies/trading-account-service';
import { ConfirmationService, MessageService } from 'primeng/api';
import { DialogService } from 'primeng/dynamicdialog';
import { CreateTradingProviderAccountComponent } from './create-trading-provider-account/create-trading-provider-account.component';

@Component({
  selector: 'app-trading-provider-account',
  templateUrl: './trading-provider-account.component.html',
  styleUrls: ['./trading-provider-account.component.scss']
})
export class TradingProviderAccountComponent extends CrudComponentBase {

  constructor(
    injector: Injector,
    messageService: MessageService,
    private userService: UserServiceProxy,
    private dialogService: DialogService,
    private routeActive: ActivatedRoute,
    private _tradingProviderService: TradingAccountService,
  ) {
    super(injector, messageService);
    this.tradingProviderId = +this.cryptDecode(this.routeActive.snapshot.paramMap.get('id'));
  }

  tradingProviderId: number;

  rows: any[] = [];

  submitted: boolean;

  listAction: any[] = [];

  InvestorAccountConst= InvestorAccountConst;

  page = new Page();
  offset = 0;

  ngOnInit() {
    this.setPage({ page: this.offset });
  }

  genListAction(data = []) {
    this.listAction = data.map(businessCustomerItem => {
      const actions = [
        // {
        //   data: businessCustomerItem,
        //   label: 'Sửa',
        //   icon: 'pi pi-pencil',
        //   command: ($event) => {
        //     this.edit($event.item.data);
        //   }
        // },
        // {
        //   data: businessCustomerItem,
        //   label: 'Xoá',
        //   icon: 'pi pi-trash',
        //   command: ($event) => {
        //     this.delete($event.item.data);
        //   }
        // }
      ];

      return actions;
    });
  }

  create() {
    const ref = this.dialogService.open(
      CreateTradingProviderAccountComponent, 
      {
      header: 'Thêm tài khoản',
      contentStyle: { "max-height": "600px", "overflow": "auto", "margin-bottom": "60px" },
      width: '500px',
      baseZIndex: 10000,
      data: {
        user: {
          userId: -1,
          tradingProviderId: this.tradingProviderId,
          displayName: null,
          email: null,
          userName: null,
          password: null,
          confirmPassword: null,
        }
      }
    }
    );

    ref.onClose.subscribe(res => { 
      console.log('res', res);
      this.setPage();
    });
  }

  changeKeyword() {
    if (this.keyword === '') {
      this.setPage({ page: this.offset });
    }
  }

  setPage(pageInfo?: any) {
    this.page.pageNumber = pageInfo?.page ?? this.offset;
    this.page.keyword = this.keyword;
    this.isLoading = true;

    this._tradingProviderService.getAll(this.page, this.tradingProviderId).subscribe((res) => {
      this.isLoading = false;
      if (this.handleResponseInterceptor(res, '')) {
        this.page.totalItems = res.data.totalItems;
        this.rows = res.data.items;
        this.genListAction(this.rows);
        console.log({ rows: res.data.items, totalItems: res.data.totalItems });
      }
    }, () => {
      this.isLoading = false;
    });
    // fix show dropdown options bị ẩn dướ
  } 

}
