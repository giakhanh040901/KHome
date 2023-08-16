import { Component, Injector, OnInit } from '@angular/core';
import { AppConsts, ETypeFormatDate, FormNotificationConst, MediaNewsConst, NotificationTemplateConst, TypeFormatDateConst } from '@shared/AppConsts';
import { CrudComponentBase } from '@shared/crud-component-base';
import { Page } from '@shared/model/page';
import { MessageService } from 'primeng/api';
import { DialogService } from 'primeng/dynamicdialog';
import { BreadcrumbService } from 'src/app/layout/breadcrumb/breadcrumb.service';
import { decode } from 'html-entities'
import { NotificationService } from '@shared/services/notification.service';
import { AddNotificationTemplateComponent } from './add-notification-template/add-notification-template.component';
import { formatDate } from '@shared/function-common';
import { HelpersService } from '@shared/services/helpers.service';
import { IconConfirm } from '@shared/consts/base.const';
@Component({
  selector: 'app-notification-template',
  templateUrl: './notification-template.component.html',
  styleUrls: ['./notification-template.component.scss']
})
export class NotificationTemplateComponent extends CrudComponentBase {

  page = new Page()
  rows: any[] = [];
  baseUrl: string;
  listAction: any[] = []
  constructor(
    injector: Injector,
    messageService: MessageService,
    public dialogService: DialogService,
    private breadcrumbService: BreadcrumbService,
    private notificationService: NotificationService,
    private _helpersService: HelpersService,
    ) {
    super(injector, messageService);
    this.isLoading = false;
    this.rows = [];
    this.page = new Page();
  }

  statusSearch: any[] = [
    {
      name: "Tất cả",
      code: ''
    },

    ...NotificationTemplateConst.status
  ]

  ngOnInit(): void {
    this.baseUrl = AppConsts.remoteServiceBaseUrl ?? this.baseUrl;
    this.setPage({ page: this.offset });
    this.breadcrumbService.setItems([
      { label: 'Trang chủ', routerLink: ['/home'] },
      { label: 'Thông báo', routerLink: ['/notification/notification-template']  },
      { label: 'Mẫu thông báo' },
    ]);
  }

  genListAction(data = []) {
    this.listAction = data.map((item) => {
      const status = item?.approve?.status;

      const actions = [];

      if (this.isGranted([this.PermissionLoyaltyConst.LoyaltyMauThongBao_CapNhat]) && item?.status == NotificationTemplateConst.KICH_HOAT) {
        actions.push({
          data: item,
          label: "Chỉnh sửa",
          icon: "pi pi-user-edit",
          command: ($event) => {
            this.edit($event.item.data);
          },
        });
      }

      if (this.isGranted([this.PermissionLoyaltyConst.LoyaltyMauThongBao_Xoa]) && item?.status == NotificationTemplateConst.KICH_HOAT ) {
        actions.push({
          data: item,
          label: "Xoá",
          icon: "pi pi-trash",
          command: ($event) => {
            this.remove($event.item.data);
          },
        });
      }

      if (this.isGranted([this.PermissionLoyaltyConst.LoyaltyMauThongBao_KichHoatOrHuy]) && item?.status == NotificationTemplateConst.KICH_HOAT) {
        actions.push({
          data: item,
          label: "Huỷ kích hoạt",
          icon: "pi pi-check-circle",
          command: ($event) => {
            this.approve($event.item.data);
          },
        });
      }

      if (this.isGranted([this.PermissionLoyaltyConst.LoyaltyMauThongBao_KichHoatOrHuy]) && item?.status == NotificationTemplateConst.HUY_KICH_HOAT) {
        actions.push({
          data: item,
          label: "Kích hoạt",
          icon: "pi pi-check-circle",
          command: ($event) => {
            this.approve($event.item.data);
          },
        });
      }

      return actions;
    });
  }

  changeStatus() {
    this.setPage({ page: this.offset })
  }

  setPage(pageInfo?: any) {
    this.page.pageNumber = pageInfo?.page ?? this.offset;
    this.page.keyword = this.keyword;
    this.isLoading = true;
    // this.page.pageSize = 20;
    this.notificationService.getAllNotificationTemplate(this.page,this.status).subscribe((res) => {
      this.isLoading = false;
      this.page.totalItems = res.totalResults;
      this.rows = res.results.map((row) => {
        row.emailContent = decode(row.emailContent);
        row.smsContent = decode(row.smsContent);
        row.notificationContent = decode(row.notificationContent);
        row.createdAtDisplay= formatDate(row?.createdAt, ETypeFormatDate.DATE_TIME);
        return row
      });
      this.genListAction(this.rows);
    }, (err) => {
      this.isLoading = false;
      console.log('Error-------', err);
      
    });
  }

  setLengthStringInScreen(ratio) {
    return (this.screenWidth/ratio).toFixed();
  }

  create() {
    const ref = this.dialogService.open(AddNotificationTemplateComponent, {
      data: {
        inputData: null
      },
      header: 'Thêm mới mẫu thông báo',
      width: '100%',
      height: '100%',
      style: {'max-height': '100%', 'border-radius': '0px'},
      contentStyle: { "overflow": "auto", "margin-bottom": "60px" },
      baseZIndex: 10000,
      footer: ""
    }).onClose.subscribe(result => {
      this.offset = 0;
      this.setPage({ page: this.offset });
    })
  }

  edit(row) {
    let modal = this.dialogService.open(AddNotificationTemplateComponent, {
      data: {
        inputData: row
      },
      header: 'Chỉnh sửa mẫu thông báo',
      width: '100%',
      height: '100%',
      style: {'max-height': '100%',  'border-radius': '0px'},
      contentStyle: { "overflow": "auto", "margin-bottom": "60px" },
      baseZIndex: 10000,
      footer: ""
    })
    modal.onClose.subscribe(result => {
      this.offset = 0
      this.setPage({ page: this.offset })
    });
  }

  getStatusSeverity(status) {
    let statusesWithLabel = {
      'ACTIVE': "success",
      'INACTIVE': "danger"
    }
    return statusesWithLabel[status];
  }
  getStatusName(status) {
    let statusesWithLabel = {
      'ACTIVE': "Kích hoạt",
      'INACTIVE': "Hủy kích hoạt"
    }
    return statusesWithLabel[status];
  }

  remove(row) {
    this._helpersService.dialogConfirmRef(
      "Xác nhận xóa mẫu thông báo?",
      IconConfirm.DELETE
    ).onClose.subscribe((accept: boolean) => {
      if(accept) {
        this.notificationService.deleteNotificationTemplate(row.id).subscribe(result => {
          this.messageSuccess("Xóa thành công!");
          this.setPage();
        })
      }
    })
  }


  approve(row) {
    this._helpersService.dialogConfirmRef(
      "Xác nhận đổi trạng thái thông báo?",
      IconConfirm.APPROVE
    ).onClose.subscribe((accept: boolean) => {
      if(accept) {
        row.status = row.status == 'ACTIVE' ? "INACTIVE" : "ACTIVE";
        this.notificationService.saveNotificationTemplate(row).subscribe(result => {
          this.messageSuccess("Đổi trạng thái thành công!");
          this.setPage();
        })
      }
    })
  }
}