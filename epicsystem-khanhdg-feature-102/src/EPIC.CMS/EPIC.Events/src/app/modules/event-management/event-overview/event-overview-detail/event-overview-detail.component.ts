import { Component, Injector } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { FormNotificationConst } from '@shared/AppConsts';
import { FormNotificationComponent } from '@shared/components/form-notification/form-notification.component';
import { CrudComponentBase } from '@shared/crud-component-base';
import { INotiDataModal, ITabView } from '@shared/interface/InterfaceConst.interface';
import { EventOverviewService } from '@shared/services/event-overview.service';
import { RouterService } from '@shared/services/router.service';
import { MessageService } from 'primeng/api';
import { DialogService } from 'primeng/dynamicdialog';
import { BreadcrumbService } from 'src/app/layout/breadcrumb/breadcrumb.service';
import { EventOverviewDetailDeliTicketComponent } from './event-overview-detail-deli-ticket/event-overview-detail-deli-ticket.component';
import { EventOverviewDetailDescriptionComponent } from './event-overview-detail-description/event-overview-detail-description.component';
import { EventOverviewDetailGeneralComponent } from './event-overview-detail-general/event-overview-detail-general.component';
import { EventOverviewDetailInforComponent } from './event-overview-detail-infor/event-overview-detail-infor.component';
import { EventOverviewDetailMediaComponent } from './event-overview-detail-media/event-overview-detail-media.component';
import { EventOverviewDetailTicketComponent } from './event-overview-detail-ticket/event-overview-detail-ticket.component';
import { EventOverviewDetailAdminComponent } from './event-overview-detail-admin/event-overview-detail-admin.component';

@Component({
  selector: 'event-overview-detail',
  templateUrl: './event-overview-detail.component.html',
  styleUrls: ['./event-overview-detail.component.scss'],
})
export class EventOverviewDetailComponent extends CrudComponentBase {
  
  public listTabPanel: ITabView[] = [];
  
  constructor(
    injector: Injector,
    messageService: MessageService,
    private breadcrumbService: BreadcrumbService,
    private routerService: RouterService,
    private eventOverviewService: EventOverviewService,
    private routeActive: ActivatedRoute,
    private dialogService: DialogService
  ) {
    super(injector, messageService);
    this.breadcrumbService.setItems([
      { label: 'Trang chủ', routerLink: ['/home'] },
      { label: 'Quản lý sự kiện', routerLink: ['/home'] },
      { label: 'Tổng quan sự kiện', routerLink: ['/event-management/event-overview'] },
      { label: 'Chi tiết sự kiện' },
    ]);
    this.eventOverviewService.eventId = +this.cryptDecode(this.routeActive.snapshot.paramMap.get('id'));
  }

  ngOnInit() {
    this.eventOverviewService.getListProvice();
    this.eventOverviewService.getListAccountMoney();
    this.eventOverviewService.getListContractCode();
    this.eventOverviewService.showBtnOpenSell = false;
    this.eventOverviewService.isEdit = this.routerService.getRouterInclude('edit');
    this.getListTabPanel();
  }

  public back(event: any) {
    if (event) {
      this.routerService.routerNavigate(['/event-management/event-overview/']);
    }
  }

  public openSell(event: any) {
    if (event) {
      const ref = this.dialogService.open(FormNotificationComponent, {
        header: 'Mở bán vé tham gia sự kiện',
        width: '600px',
        contentStyle: {
          'max-height': '600px',
          overflow: 'auto',
          'padding-bottom': '50px',
        },
        styleClass: 'p-dialog-custom',
        baseZIndex: 10000,
        data: {
          title: `Xác nhận mở bán vé tham gia sự kiện`,
          icon: FormNotificationConst.IMAGE_APPROVE,
        } as INotiDataModal,
      });
      ref.onClose.subscribe((dataCallBack) => {
        if (dataCallBack?.accept) {
          this.eventOverviewService.openSellEvent(this.eventOverviewService.eventId).subscribe(
            (response) => {
              if (this.handleResponseInterceptor(response, 'Mở bán vé tham gia sự kiện thành công')) {
              }
            },
            (err) => {
              this.messageError(`Mở bán vé tham gia sự kiện thất bại`);
            }
          );
        }
      });
    }
  }

  getListTabPanel() {
    this.listTabPanel = [];
    if(this.isGranted(this.PermissionEventConst.TongQuanSuKien_ChiTiet_ThongTinChung)) {
      this.listTabPanel.push(
        {
          key: 'overview',
          title: 'Thông tin chung',
          component: EventOverviewDetailGeneralComponent,
          isDisabled: false,
        }
      )
    }
    //
    if(this.isGranted(this.PermissionEventConst.TongQuanSuKien_ChiTiet_ThongTinMoTa)) {
      this.listTabPanel.push(
        {
          key: 'description',
          title: 'Thông tin mô tả',
          component: EventOverviewDetailDescriptionComponent,
          isDisabled: false,
        },
      )
    }
    //
    if(this.isGranted(this.PermissionEventConst.TongQuanSuKien_ChiTiet_ThoiGianVaLoaiVe)) {
      this.listTabPanel.push(
        {
          key: 'infor',
          title: 'Thời gian & loại vé',
          component: EventOverviewDetailInforComponent,
          isDisabled: false,
        },
      )
    }
    //
    if(this.isGranted(this.PermissionEventConst.TongQuanSuKien_ChiTiet_HinhAnhSuKien)) {
      this.listTabPanel.push(
        {
          key: 'media',
          title: 'Hình ảnh sự kiện',
          component: EventOverviewDetailMediaComponent,
          isDisabled: false,
        },
      )
    }
    //
    if(this.isGranted(this.PermissionEventConst.TongQuanSuKien_ChiTiet_MauGiaoNhanVe)) { 
      this.listTabPanel.push(
        {
          key: 'deliTicket',
          title: 'Mẫu giao nhận vé',
          component: EventOverviewDetailDeliTicketComponent,
          isDisabled: false,
        }
      )
    }
    //
    if(this.isGranted(this.PermissionEventConst.TongQuanSuKien_ChiTiet_MauVe)) {
      this.listTabPanel.push(
        {
          key: 'ticket',
          title: 'Mẫu vé',
          component: EventOverviewDetailTicketComponent,
          isDisabled: false,
        },
      )
    }
  }

  public get showBtnOpenSell() {
    return this.eventOverviewService.showBtnOpenSell;
  }
 
}
