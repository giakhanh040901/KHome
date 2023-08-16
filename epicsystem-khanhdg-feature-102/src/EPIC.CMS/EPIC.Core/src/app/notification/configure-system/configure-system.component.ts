import { Component, Injector, OnInit } from '@angular/core';
import {SearchConst, FormNotificationConst, MSBPrefixAccountConst, YesNoConst, StatusDeleteConst, TypeFormatDateConst, ConfigureSystemConst } from '@shared/AppConsts';
import { CrudComponentBase } from '@shared/crud-component-base';
import { Page } from '@shared/model/page';
import { NationalityConst } from '@shared/nationality-list';
import { MSBPrefixAccountServiceProxy } from '@shared/service-proxies/whitelist-ip-service';
import { ConfigureSystemService } from '@shared/services/configure-system.service';

import { ConfirmationService, MessageService } from 'primeng/api';
import { DialogService, DynamicDialogRef } from 'primeng/dynamicdialog';
import { forkJoin } from 'rxjs';
import { debounceTime } from 'rxjs/operators';
import { FormNotificationComponent } from 'src/app/form-notification/form-notification.component';
import { FormSetDisplayColumnComponent } from 'src/app/form-set-display-column/form-set-display-column.component';
import { BreadcrumbService } from 'src/app/layout/breadcrumb/breadcrumb.service';
import { CreateConfigureSystemComponent } from './create-configure-system/create-configure-system.component';

@Component({
  selector: 'app-configure-system',
  templateUrl: './configure-system.component.html',
  styleUrls: ['./configure-system.component.scss'],
  providers: [ConfirmationService, MessageService]
})
export class ConfigureSystemComponent extends CrudComponentBase {

  constructor(
    injector: Injector,
    messageService: MessageService,
    private dialogService: DialogService,
    private _dialogService: DialogService,
    private _configureSystemService: ConfigureSystemService,
    private breadcrumbService: BreadcrumbService
    ) {
    super(injector, messageService);
    this.breadcrumbService.setItems([
      { label: 'Trang chủ', routerLink: ['/home'] },
      { label: 'Cấu hình hệ thống', routerLink: ['/establish/configure-system'] },
    ]);
  }

  ref: DynamicDialogRef;

  rows: any[] = [];

	cols: any[];
	_selectedColumns: any[];
  
  NationalityConst = NationalityConst;
  MSBPrefixAccountConst = MSBPrefixAccountConst;
  StatusDeleteConst = StatusDeleteConst;
  ConfigureSystemConst = ConfigureSystemConst;
 
  submitted: boolean;
	listAction: any[] = [];
  checkKey: any[] = [];

  page = new Page();
  offset = 0;
  //
  listBanks: any;

  ngOnInit(): void {
    //
    this.setPage();

    this.subject.keyword.pipe(debounceTime(SearchConst.DEBOUNCE_TIME)).subscribe(() => {
      if (this.keyword === "") {
        this.setPage({ page: this.offset });
      } else {
        this.setPage();
      }
    });
    //
    this.cols = [
			{ field: 'keyDisplay', header: 'Cấu hình', width: '15rem', isPin: true },
			{ field: 'value', header: 'Giá trị', width: '15rem', isPin: true },
			{ field: 'createdDateDisplay', header: 'Ngày tạo', width: '15rem' },
			{ field: 'createdBy', header: 'Người tạo', width: '15rem' },
			{ field: 'modifiedBy', header: 'Người sửa cuối', width: '15rem' },
			{ field: 'modifiedDateDisplay', header: 'Lần sửa cuối', width: '15rem' },
		];
    //
		this._selectedColumns = this.getLocalStorage('configureSystem') ?? this.cols;  
  }
  
  getLocalStorage(key) {
		return JSON.parse(localStorage.getItem(key))
	}

	setLocalStorage(data) {
		return localStorage.setItem('configureSystem', JSON.stringify(data));
	}

	setColumn(col, _selectedColumns) {
		const ref = this.dialogService.open(
			FormSetDisplayColumnComponent,
			this.getConfigDialogServiceDisplayTableColumn("Sửa cột hiển thị", col, _selectedColumns)
		);

		ref.onClose.subscribe((dataCallBack) => {
			if (dataCallBack?.accept) {
				this._selectedColumns = dataCallBack.data.sort(function (a, b) {
					return a.position - b.position;
				});
				this.setLocalStorage(this._selectedColumns)
			}
		});
	}

	showData(rows) {
		for (let row of rows) {
      row.keyDisplay = ConfigureSystemConst.getName(row.key);
      row.createdDateDisplay = this.formatDate(row?.createdDate,TypeFormatDateConst.DMYHm);
      row.modifiedDateDisplay = this.formatDate(row?.modifiedDate,TypeFormatDateConst.DMYHm);
		};
	}

  genListAction(data = []) {
		this.listAction = data.map(item => {
			const actions = [];
      
      if (this.isGranted([this.PermissionCoreConst.CoreCauHinhHeThong_ChiTiet])) {
        actions.push({
          data: item,
          label: 'Thông tin chi tiết',
          icon: 'pi pi-info-circle',
          command: ($event) => {
            this.detail($event.item.data);
          }
        })
      }

      if (this.isGranted([this.PermissionCoreConst.CoreCauHinhHeThong_CapNhat])) {
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
      CreateConfigureSystemComponent,
      {
        header: 'Cấu hình hệ thống',
        width: '500px',
        contentStyle: { "max-height": "600px", "overflow": "auto", "margin-bottom": "60px" },
        data: {
          create: true,
          checkKey: this.checkKey,
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
      CreateConfigureSystemComponent,
        {
          header: 'Thông tin cấu hình',
          width: '500px',
          contentStyle: { "max-height": "600px", "overflow": "auto", "margin-bottom": "60px" },
          data: {
            configureSystem: item,
            view: true,
          }
        }
    );
  }
  //
  edit(item) {
    const ref = this.dialogService.open(
      CreateConfigureSystemComponent,
        {
          header: 'Sửa cấu hình',
          width: '500px',
          contentStyle: { "max-height": "600px", "overflow": "auto", "margin-bottom": "60px" },
          data: {
            configureSystem: item,
          }
        }
    );
    
    ref.onClose.subscribe((statusResponse) => {
        if (statusResponse) {
            this.setPage();
        }
    });
  }

  setPage(event?: any) {
    if(event) {
      this.page.pageNumber = event.page;
      this.page.pageSize = event.rows;
    }
    
    this.page.keyword = this.keyword;
    this.isLoading = true;

    forkJoin([this._configureSystemService.getAll(this.page)]).subscribe(([res]) => {
      this.isLoading = false;
      if (this.handleResponseInterceptor(res, '')) {                                                          
        this.rows = res.data.filter(r => !ConfigureSystemConst.keyDates.includes(r.key));
        this.page.totalItems = this.rows.length;
        this.checkKey = ConfigureSystemConst.key.filter(x => !this.rows.map(item => item.key).includes(x.code)); 
        this.genListAction(this.rows);
        this.showData(this.rows);
      }
    }, (err) => {
      this.isLoading = false;
      console.log('Error-------', err);
    });
  }

}



