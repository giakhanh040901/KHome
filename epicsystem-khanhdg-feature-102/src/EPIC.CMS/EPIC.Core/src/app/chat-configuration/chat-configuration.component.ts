import { Component, Injector, OnInit } from '@angular/core';
import {SearchConst, FormNotificationConst, MSBPrefixAccountConst, YesNoConst, StatusDeleteConst, TypeFormatDateConst, ConfigureSystemConst, UserTypes } from '@shared/AppConsts';
import { CrudComponentBase } from '@shared/crud-component-base';
import { Page } from '@shared/model/page';
import { NationalityConst } from '@shared/nationality-list';
import { MSBPrefixAccountServiceProxy } from '@shared/service-proxies/whitelist-ip-service';
import { ChatConfigurationService } from '@shared/services/chat-configuration.service';
import { ConfigureSystemService } from '@shared/services/configure-system.service';

import { ConfirmationService, MessageService } from 'primeng/api';
import { DialogService, DynamicDialogRef } from 'primeng/dynamicdialog';
import { forkJoin } from 'rxjs';
import { debounceTime } from 'rxjs/operators';
import { FormNotificationComponent } from 'src/app/form-notification/form-notification.component';
import { FormSetDisplayColumnComponent } from 'src/app/form-set-display-column/form-set-display-column.component';
import { BreadcrumbService } from 'src/app/layout/breadcrumb/breadcrumb.service';
import { AddChatConfigurationComponent } from './add-chat-configuration/add-chat-configuration.component';
// import { CreateConfigureSystemComponent } from './create-configure-system/create-configure-system.component';

@Component({
  selector: 'app-chat-configuration',
  templateUrl: './chat-configuration.component.html',
  styleUrls: ['./chat-configuration.component.scss'],
  providers: [ConfirmationService, MessageService]
})
export class ChatConfigurationComponent extends CrudComponentBase {

  constructor(
    injector: Injector,
    messageService: MessageService,
    private dialogService: DialogService,
    private _dialogService: DialogService,
    private _chatConfigurationService: ChatConfigurationService,
    private breadcrumbService: BreadcrumbService
    ) {
    super(injector, messageService);
    this.breadcrumbService.setItems([
      { label: 'Trang chủ', routerLink: ['/home'] },
      { label: 'Cấu hình chat', routerLink: ['/chat-configuration'] },
    ]);
    this.userLogin = this.getUser();
    console.log('userLogin____', this.userLogin); 
  }
  userLogin: any = {};
  ref: DynamicDialogRef;

  rows: any[] = [];
  tradingProviders: any[] = [];
	cols: any[];
	_selectedColumns: any[];
  
  ConfigureSystemConst = ConfigureSystemConst;
  UserTypes = UserTypes;
  submitted: boolean;
	listAction: any[] = [];
  checkKey: any[] = [];

  page = new Page();
  offset = 0;
  //
  listBanks: any;

  ngOnInit(): void {
    this.setPage();
    this.subject.keyword.pipe(debounceTime(SearchConst.DEBOUNCE_TIME)).subscribe(() => {
      if (this.keyword === "") {
        this.setPage();
      } else {
        this.setPage();
      }
    });
    if(UserTypes.TYPE_TRADING_PROVIDERS.includes(this.userLogin.user_type)) {
      this.cols = [
        { field: 'message', header: 'Cấu hình chat', width: '15rem', isPin: true },
      ];
    } else {
      this.cols = [
        { field: 'tradingProviderDisplay', header: 'Đại lý', width: '15rem', isPin: true },
        { field: 'message', header: 'Cấu hình chat', width: '15rem', isPin: true },
      ];
    }
   
  }

	showData(rows) {
		for (let row of rows) {
      row.tradingProviderDisplay = this.tradingProviders.find(item =>
        item.tradingProviderId == row.tradingProviderId)?.name;
		};
	}

  genListAction(data = []) {
		this.listAction = data.map(item => {
			const actions = [];
      
      if (true) {
        actions.push({
          data: item,
          label: 'Thông tin chi tiết',
          icon: 'pi pi-info-circle',
          command: ($event) => {
            this.detail($event.item.data);
          }
        })
      }

      if (true) {
        actions.push({
          data: item,
          label: 'Sửa',
          icon: 'pi pi-pencil',
          command: ($event) => {
            this.edit($event.item.data);
          }
        })
      }

			return actions;
		});
	}

  create() {
    const ref = this.dialogService.open(
      AddChatConfigurationComponent,
      {
        header: 'Cấu hình chat',
        width: '500px',
        contentStyle: { "max-height": "600px", "overflow": "auto", "margin-bottom": "60px" },
        data: {
          create: true,
          checkKey: this.checkKey,
          tradingProviders: this.tradingProviders,
        }
      });
  
    ref.onClose.subscribe((statusResponse) => {
      if (statusResponse) {
        this.setPage();
      }
    });
  }

  detail(item) {
    const ref = this.dialogService.open(
      AddChatConfigurationComponent,
        {
          header: 'Thông tin cấu hình',
          width: '500px',
          contentStyle: { "max-height": "600px", "overflow": "auto", "margin-bottom": "60px" },
          data: {
            configureSystem: item,
            view: true,
            tradingProviders: this.tradingProviders,
          }
        }
    );
  }
  //
  edit(item) {
    const ref = this.dialogService.open(
      AddChatConfigurationComponent,
        {
          header: 'Sửa cấu hình',
          width: '500px',
          contentStyle: { "max-height": "600px", "overflow": "auto", "margin-bottom": "60px" },
          data: {
            configureSystem: item,
            tradingProviders: this.tradingProviders,
          }
        }
    );
    
    ref.onClose.subscribe((statusResponse) => {
        if (statusResponse) {
            this.setPage();
        }
    });
  }

  setPage() {
    this.isLoading = true;

    forkJoin([this._chatConfigurationService.getAll(),this._chatConfigurationService.getAllTradingProvider()]).subscribe(([resChat, resTrading]) => {
      this.isLoading = false;
      if (this.handleResponseInterceptor(resChat, '')) {
        this.page.totalItems = 1;
        this.rows = resChat.data;
        this.genListAction(this.rows);
       
      }
      if (this.handleResponseInterceptor(resTrading, '')) {
        this.tradingProviders = resTrading.data.items;
        this.checkKey = this.tradingProviders.filter(x => !this.rows.map(item => item.tradingProviderId).includes(x.tradingProviderId)); 
        this.showData(this.rows);
      }
      
    }, (err) => {
      this.isLoading = false;
      console.log('Error-------', err);
    });
  }

}




