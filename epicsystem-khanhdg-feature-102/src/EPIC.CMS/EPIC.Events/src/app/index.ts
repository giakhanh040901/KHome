import { Route } from '@angular/router';
import { AppRouteGuard } from '@shared/auth/auth-route-guard';
import { EventOverviewDetailComponent } from './modules/event-management/event-overview/event-overview-detail/event-overview-detail.component';
import { EventOverviewComponent } from './modules/event-management/event-overview/event-overview.component';
import { EventOverviewModule } from './modules/event-management/event-overview/event-overview.module';
import { BillRequestListComponent } from './modules/sale-ticket-management/bill-request-list/bill-request-list.component';
import { BillRequestListModule } from './modules/sale-ticket-management/bill-request-list/bill-request-list.module';
import { ParticipationManagementComponent } from './modules/sale-ticket-management/participation-management/participation-management.component';
import { ParticipationManagementModule } from './modules/sale-ticket-management/participation-management/participation-management.module';
import { CreateSaleTicketOrderConfirmComponent } from './modules/sale-ticket-management/sale-ticket-order/create-sale-ticket-order/create-sale-ticket-order-confirm/create-sale-ticket-order-confirm.component';
import { CreateSaleTicketOrderCustomerComponent } from './modules/sale-ticket-management/sale-ticket-order/create-sale-ticket-order/create-sale-ticket-order-customer/create-sale-ticket-order-customer.component';
import { CreateSaleTicketOrderEventComponent } from './modules/sale-ticket-management/sale-ticket-order/create-sale-ticket-order/create-sale-ticket-order-event/create-sale-ticket-order-event.component';
import { CreateSaleTicketOrderComponent } from './modules/sale-ticket-management/sale-ticket-order/create-sale-ticket-order/create-sale-ticket-order.component';
import { SaleTicketOrderDetailComponent } from './modules/sale-ticket-management/sale-ticket-order/sale-ticket-order-detail/sale-ticket-order-detail.component';
import { SaleTicketOrderComponent } from './modules/sale-ticket-management/sale-ticket-order/sale-ticket-order.component';
import { SaleTicketOrderModule } from './modules/sale-ticket-management/sale-ticket-order/sale-ticket-order.module';
import { TicketRequestListComponent } from './modules/sale-ticket-management/ticket-request-list/ticket-request-list.component';
import { TicketRequestListModule } from './modules/sale-ticket-management/ticket-request-list/ticket-request-list.module';
import { TransactionProcessingComponent } from './modules/sale-ticket-management/transaction-processing/transaction-processing.component';
import { TransactionProcessingModule } from './modules/sale-ticket-management/transaction-processing/transaction-processing.module';
import { ValidSaleTicketComponent } from './modules/sale-ticket-management/valid-sale-ticket/valid-sale-ticket.component';
import { ValidSaleTicketModule } from './modules/sale-ticket-management/valid-sale-ticket/valid-sale-ticket.module';
import { SettingContractCodeComponent } from './modules/setting/setting-contract-code/setting-contract-code.component';
import { SettingContractCodeModule } from './modules/setting/setting-contract-code/setting-contract-code.module';
import { SystemNotificationTemplateComponent } from './modules/setting/system-notification-template/system-notification-template.component';
import { PermissionEventConst } from '@shared/AppConsts';

export const modules: any[] = [
  SettingContractCodeModule,
  EventOverviewModule,
  SaleTicketOrderModule,
  ValidSaleTicketModule,
  ParticipationManagementModule,
  TransactionProcessingModule,
  TicketRequestListModule,
  BillRequestListModule,
];

export const menus: any[] = [
  {
    label: 'Cài đặt',
    routerLink: ['/setting'],
    isShow: true,
    icon: 'pi pi-fw pi-cog',
    permission: PermissionEventConst.Menu_CaiDat,
    items: [
      {
        label: 'Cấu trúc mã hợp đồng',
        icon: '',
        routerLink: ['/setting/setting-contract-code'],
        permission: PermissionEventConst.Menu_CauTrucMaHD,
        isShow: true,
      },
      {
        label: 'Thông báo hệ thống',
        icon: '',
        routerLink: ['/setting/system-notification-template'],
        permission: PermissionEventConst.Menu_ThongBaoHeThong,
        isShow: true,
      },
    ],
  },
  {
    label: 'Quản lý sự kiện',
    routerLink: ['/event-management'],
    permission: PermissionEventConst.Menu_QuanLySuKien,
    isShow: true,
    icon: 'pi pi-sitemap',
    items: [
      {
        label: 'Tổng quan sự kiện',
        icon: '',
        routerLink: ['/event-management/event-overview'],
        permission: PermissionEventConst.Menu_TongQuanSuKien,
        isShow: true,
      },
    ],
  },
  {
    label: 'Quản lý bán vé',
    routerLink: ['/sale-ticket-management'],
    permission: PermissionEventConst.Menu_QuanLyBanVe,
    isShow: true,
    icon: 'pi pi-list',
    items: [
      {
        label: 'Sổ lệnh',
        icon: '',
        routerLink: ['/sale-ticket-management/sale-ticket-order'],
        permission: PermissionEventConst.Menu_SoLenh,
        isShow: true,
      },
      {
        label: 'Xử lý giao dịch',
        icon: '',
        routerLink: ['/sale-ticket-management/transaction-processing'],
        permission: PermissionEventConst.Menu_XuLyGiaoDich,
        isShow: true,
      },
      {
        label: 'Vé bán hợp lệ',
        icon: '',
        routerLink: ['/sale-ticket-management/valid-sale-ticket'],
        permission: PermissionEventConst.Menu_VeBanHopLe,
        isShow: true,
      },
      {
        label: 'Quản lý tham gia',
        icon: '',
        routerLink: ['/sale-ticket-management/participation-management'],
        permission: PermissionEventConst.Menu_QuanLyThamGia,
        isShow: true,
      },
      {
        label: 'Yêu cầu vé cứng',
        icon: '',
        routerLink: ['/sale-ticket-management/ticket-request-list'],
        permission: PermissionEventConst.Menu_YeuCauVeCung,
        isShow: true,
      },
      {
        label: 'Yêu cầu hóa đơn',
        icon: '',
        routerLink: ['/sale-ticket-management/bill-request-list'],
        permission: PermissionEventConst.Menu_YeuCauHoaDon,
        isShow: true,
      },
    ],
  },
];

export const routes: Route[] = [
  {
    path: 'setting',
    children: [
      {
        path: 'setting-contract-code',
        children: [
          {
            path: '',
            component: SettingContractCodeComponent,
            data: { permissions: [PermissionEventConst.Menu_CauTrucMaHD] },
            canActivate: [AppRouteGuard],
          },
        ],
      },
      {
        path: 'system-notification-template',
        children: [
          {
            path: '',
            component: SystemNotificationTemplateComponent,
            data: { permissions: [PermissionEventConst.Menu_ThongBaoHeThong] },
            canActivate: [AppRouteGuard],
          },
        ],
      },
    ],
  },
  {
    path: 'event-management',
    children: [
      {
        path: 'event-overview',
        children: [
          {
            path: '',
            component: EventOverviewComponent,
            data: { permissions: [PermissionEventConst.Menu_TongQuanSuKien] },
            canActivate: [AppRouteGuard],
          },
          {
            path: 'detail/:id',
            component: EventOverviewDetailComponent,
            data: { permissions: [PermissionEventConst.TongQuanSuKien_ChiTiet] },
            canActivate: [AppRouteGuard],
          },
          {
            path: 'edit/:id',
            component: EventOverviewDetailComponent,
            data: { permissions: [PermissionEventConst.TongQuanSuKien_ChiTiet_MauVe_CapNhat] },
            canActivate: [AppRouteGuard],
          },
        ],
      },
    ],
  },
  {
    path: 'sale-ticket-management',
    children: [
      {
        path: 'sale-ticket-order',
        children: [
          {
            path: '',
            component: SaleTicketOrderComponent,
            data: { permissions: [PermissionEventConst.Menu_SoLenh] },
            canActivate: [AppRouteGuard],
          },
          {
            path: 'create',
            component: CreateSaleTicketOrderComponent,
            data: { permissions: [PermissionEventConst.SoLenh_ThemMoi] },
            canActivate: [AppRouteGuard],
            children: [
              {
                path: '',
                redirectTo: 'filter-customer',
                pathMatch: 'full',
              },
              {
                path: 'customer',
                component: CreateSaleTicketOrderCustomerComponent,
                canActivate: [AppRouteGuard],
              },
              {
                path: 'event',
                component: CreateSaleTicketOrderEventComponent,
                canActivate: [AppRouteGuard],
              },
              {
                path: 'confirm',
                component: CreateSaleTicketOrderConfirmComponent,
                canActivate: [AppRouteGuard],
              },
            ],
          },
          {
            path: 'detail/:id',
            component: SaleTicketOrderDetailComponent,
            data: { permissions: 
              [ 
                PermissionEventConst.SoLenh_ChiTiet, 
                PermissionEventConst.XuLyGiaoDich_ChiTiet, 
                PermissionEventConst.VeBanHopLe_ChiTiet,
                PermissionEventConst.YeuCauVeCung_ChiTiet,
                PermissionEventConst.YeuCauHoaDon_ChiTiet,
              ] 
            },
            canActivate: [AppRouteGuard],
          },
          {
            path: 'edit/:id',
            component: SaleTicketOrderDetailComponent,
            data: { permissions: 
              [
                PermissionEventConst.SoLenh_ChiTiet_ThongTinChung_CapNhat,
                PermissionEventConst.XuLyGiaoDich_ChiTiet_ThongTinChung_CapNhat, 
              ] 
            },
            canActivate: [AppRouteGuard],
          },
        ],
      },
      {
        path: 'transaction-processing',
        children: [
          {
            path: '',
            component: TransactionProcessingComponent,
            data: { permissions: [PermissionEventConst.Menu_XuLyGiaoDich] },
            canActivate: [AppRouteGuard],
          },
        ],
      },
      {
        path: 'valid-sale-ticket',
        children: [
          {
            path: '',
            component: ValidSaleTicketComponent,
            data: { permissions: [PermissionEventConst.Menu_XuLyGiaoDich] },
            canActivate: [AppRouteGuard],
          },
        ],
      },
      {
        path: 'participation-management',
        children: [
          {
            path: '',
            component: ParticipationManagementComponent,
            data: { permissions: [PermissionEventConst.Menu_VeBanHopLe] },
            canActivate: [AppRouteGuard],
          },
        ],
      },
      {
        path: 'ticket-request-list',
        children: [
          {
            path: '',
            component: TicketRequestListComponent,
            data: { permissions: [PermissionEventConst.Menu_YeuCauVeCung] },
            canActivate: [AppRouteGuard],
          },
        ],
      },
      {
        path: 'bill-request-list',
        children: [
          {
            path: '',
            component: BillRequestListComponent,
            data: { permissions: [PermissionEventConst.Menu_YeuCauHoaDon] },
            canActivate: [AppRouteGuard],
          },
        ],
      },
    ],
  },
];
