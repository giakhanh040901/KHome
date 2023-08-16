import { Component, Injector, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AppConsts, FormNotificationConst } from '@shared/AppConsts';
import { CrudComponentBase } from '@shared/crud-component-base';
import { Page } from '@shared/model/page';
import { MessageService } from 'primeng/api';
import { DialogService } from 'primeng/dynamicdialog';
import { BreadcrumbService } from 'src/app/layout/breadcrumb/breadcrumb.service';
import { decode } from 'html-entities'
import { NotificationService } from '@shared/services/notification.service';
import { HelpersService } from '@shared/services/helpers.service';
import { IconConfirm } from '@shared/consts/base.const';

@Component({
  selector: 'app-notification',
  templateUrl: './notification.component.html',
  styleUrls: ['./notification.component.scss']
})
export class NotificationComponent extends CrudComponentBase implements OnInit {

  page = new Page()
  rows: any[] = [];
  actions: any[];
  currentActionData: any;
  baseUrl: string;
  notificationTemplates: [] = [];
  listAction: any[] = [];
  constructor(
    injector: Injector,
    messageService: MessageService,
    public dialogService: DialogService,
    private breadcrumbService: BreadcrumbService,
    private router: Router,
    private notificationService: NotificationService,
    private _helpersService: HelpersService,
  ) {
    super(injector, messageService);
    this.isLoading = false;
    this.rows = [];
    this.page = new Page();
  }
  status: string;

  statusSearch: any[] = [
    {
      name: "Đã gửi",
      code: 'SENT'
    },
    {
      name: "Khởi tạo",
      code: 'DRAFT'
    },
  ]

  ngOnInit(): void {
    this.baseUrl = AppConsts.remoteServiceBaseUrl ?? this.baseUrl;
    this.setPage({ page: this.offset });
    this.breadcrumbService.setItems([
      { label: 'Trang chủ', routerLink: ['/home'] },
      { label: 'Thông báo' }
    ]);

  }

  changeStatus() {
    this.setPage({ page: this.offset })
  }

  genListAction(data = []) {
    this.listAction = data.map((item) => {
      const status = item?.approve?.status;

      const actions = [];

      if (this.isGranted([this.PermissionLoyaltyConst.LoyaltyQLTB_PageChiTiet])) {
        actions.push({
          data: item,
          label: "Chi tiết thông báo",
          icon: "pi pi-info-circle",
          command: ($event) => {
            this.notificationDetail();
          },
        });
      }

      if (this.isGranted([this.PermissionLoyaltyConst.LoyaltyQLTB_Xoa])) {
        actions.push({
          data: item,
          label: "Xoá",
          icon: "pi pi-trash",
          command: ($event) => {
            this.deleteNotification();
          },
        });
      }

      return actions;
    });
  }

  notificationDetail() {
    let id = this.currentActionData.id;
    console.log(this.currentActionData)
    this.router.navigate(['notification-management/notification-message/detail'], { queryParams: { id } });
  }

  deleteNotification() {
    this._helpersService.dialogConfirmRef(
      "Xác nhận xóa thông báo?",
      IconConfirm.DELETE
    ).onClose.subscribe((accept: boolean) => {
      if(accept) {
        this.notificationService.deleteNotification(this.currentActionData.id).subscribe(result => {
          this.messageSuccess('Xóa thành công!');
          this.setPage({ page: this.offset });
        })
      }
    })
  }

  initNotificationTemplate(pageInfo?: any) {
    this.page.pageNumber = pageInfo?.page ?? this.offset;
    this.page.keyword = this.keyword;
    this.isLoading = true;
    // this.page.pageSize = 20;
    this.notificationService.getAllNotificationTemplate(this.page).subscribe((res) => {
      this.page.totalItems = res.totalResults;
      this.notificationTemplates = res.results;
      this.isLoading = false;
    }, (err) => {
      this.isLoading = false;
      console.log('Error-------', err);
      
    });
  }

  clickDropdown(row) {
    this.currentActionData = row;
  }

  setPage(pageInfo?: any) {
    this.page.pageNumber = pageInfo?.page ?? this.offset;
    this.page.keyword = this.keyword;
    this.isLoading = true;
    // this.page.pageSize = 20;
    this.notificationService.getAll(this.page,this.status).subscribe((res) => {
      this.isLoading = false;
      this.page.totalItems = res.totalResults;
      this.rows = res.results.map((row) => {
        row.content = decode(row.content);
        row.emailContent = decode(row.emailContent);
        row.smsContent = decode(row.smsContent);
        row.notificationContent = decode(row.notificationContent);
        return { ...row }
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
    this.router.navigate(['notification-management/notification-message/detail']);
  }

  getStatusSeverity(status) {
    let statusesWithLabel = {
      'SENT': "success",
      'DRAFT': "danger"
    }
    return statusesWithLabel[status];
  }

  getStatusName(status) {
    let statusesWithLabel = {
      'SENT': "Đã gửi",
      'DRAFT': "Khởi tạo"
    }
    return statusesWithLabel[status];
  }

  approve(row) {
    this._helpersService.dialogConfirmRef(
      "Xác nhận đổi trạng thái thông báo?",
      IconConfirm.DELETE
    ).onClose.subscribe((accept: boolean) => {
      if(accept) {
        row.status = row.status == 'ACTIVE' ? "INACTIVE" : "ACTIVE";
        this.notificationService.saveNotificationTemplate(row).subscribe(result => {
          this.messageSuccess("Đổi trạng thái thành công!");
        })
      }
    })
  }

}
