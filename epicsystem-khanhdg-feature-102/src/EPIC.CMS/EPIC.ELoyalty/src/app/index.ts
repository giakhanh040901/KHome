import { Route } from '@angular/router';
import { AppRouteGuard } from '@shared/auth/auth-route-guard';
import { IndividualCustomerDetailComponent } from './modules/customer-management/individual-customer/individual-customer-detail/individual-customer-detail.component';
import { IndividualCustomerComponent } from './modules/customer-management/individual-customer/individual-customer.component';
import { IndividualCustomerModule } from './modules/customer-management/individual-customer/individual-customer.module';
import { BroadcastNewsComponent } from './modules/media-management/broadcast-news/broadcast-news.component';
import { KnowledgeBaseComponent } from './modules/media-management/knowledge-base/knowledge-base.component';
import { MediaManagementModule } from './modules/media-management/media-management.module';
import { MediaComponent } from './modules/media-management/media/media.component';
import { NotificationManagementModule } from './modules/notification-management/notification-management.module';
import { NotificationTemplateComponent } from './modules/notification-management/notification-template/notification-template.component';
import { NotificationDetailComponent } from './modules/notification-management/notification/notification-detail/notification-detail.component';
import { NotificationComponent } from './modules/notification-management/notification/notification.component';
import { SystemTemplateComponent } from './modules/notification-management/system-template/system-template.component';
import { AccumulatePointManagementComponent } from './modules/point-management/accumulate-point-management/accumulate-point-management.component';
import { AccumulatePointManagementModule } from './modules/point-management/accumulate-point-management/accumulate-point-management.module';
import { MembershipLevelManagementComponent } from './modules/point-management/membership-level-management/membership-level-management.component';
import { MembershipLevelManagementModule } from './modules/point-management/membership-level-management/membership-level-management.module';
import { ChangeVoucherRequestComponent } from './modules/request-management/change-voucher-request/change-voucher-request.component';
import { ChangeVoucherRequestModule } from './modules/request-management/change-voucher-request/change-voucher-request.module';
import { ApplyVoucherHistoryComponent } from './modules/voucher-management/apply-voucher-history/apply-voucher-history.component';
import { ApplyVoucherHistoryModule } from './modules/voucher-management/apply-voucher-history/apply-voucher-history.module';
import { VoucherManagementComponent } from './modules/voucher-management/voucher-management/voucher-management.component';
import { VoucherManagementModule } from './modules/voucher-management/voucher-management/voucher-management.module';
import { ManagementReportComponent } from './modules/export-report-management/management-report/management-report.component';
import { PermissionLoyaltyConst } from '@shared/AppConsts';
import { PrizeDrawManagementComponent } from './modules/prize-draw-management/prize-draw-management.component';
import { ProgramInfomationComponent } from './modules/prize-draw-management/create-prize-draw/program-infomation/program-infomation.component';
import { CreatePrizeDrawComponent } from './modules/prize-draw-management/create-prize-draw/create-prize-draw.component';
import { ProgramConfigurationComponent } from './modules/prize-draw-management/create-prize-draw/program-configuration/program-configuration.component';
import { PrizeDrawManagementModule } from './modules/prize-draw-management/prize-draw-management.module';
import { DetailPrizeDrawComponent } from './modules/prize-draw-management/detail-prize-draw/detail-prize-draw.component';
import { JoinSettingPrizeDrawComponent } from './modules/prize-draw-management/join-setting-prize-draw/join-setting-prize-draw.component';

export const modules: any[] = [
  IndividualCustomerModule,
  VoucherManagementModule,
  AccumulatePointManagementModule,
  MembershipLevelManagementModule,
  ChangeVoucherRequestModule,
  MediaManagementModule,
  NotificationManagementModule,
  ApplyVoucherHistoryModule,
  PrizeDrawManagementModule,
];

export const menus: any[] = [
  {
    label: 'Quản lý khách hàng',
    routerLink: ['/customer-management'],
    isShow: true,
    key: PermissionLoyaltyConst.LoyaltyMenu_QLKhachHang,
    icon: 'pi pi-users',
    items: [
      {
        label: 'Khách hàng cá nhân',
        icon: '',
        routerLink: ['/customer-management/individual-customer'],
        isShow: true,
        key: PermissionLoyaltyConst.LoyaltyMenu_KhachHangCaNhan,
      },
    ],
  },
  {
    label: 'Quản lý ưu đãi',
    routerLink: ['/voucher-management'],
    isShow: true,
    key: PermissionLoyaltyConst.LoyaltyMenu_QLUuDai,
    icon: 'pi pi-ticket',
    items: [
      {
        label: 'Danh sách ưu đãi',
        icon: '',
        routerLink: ['/voucher-management/list-voucher'],
        isShow: true,
        key: PermissionLoyaltyConst.LoyaltyMenu_DanhSachUuDai,
      },
      {
        label: 'Lịch sử cấp phát',
        icon: '',
        routerLink: ['/voucher-management/apply-history'],
        isShow: true,
        key: PermissionLoyaltyConst.LoyaltyMenu_LichSuCapPhat,
      },
    ],
  },
  {
    label: 'Quản lý điểm',
    routerLink: ['/point-management'],
    isShow: true,
    key: PermissionLoyaltyConst.LoyaltyMenu_QLDiem,
    icon: 'pi pi-wallet',
    items: [
      {
        label: 'Quản lý tích điểm',
        icon: '',
        routerLink: ['/point-management/accumulate-point-management'],
        isShow: true,
        key: PermissionLoyaltyConst.LoyaltyMenu_TichDiem,
      },
      {
        label: 'Quản lý hạng thành viên',
        icon: '',
        routerLink: ['/point-management/membership-level-management'],
        isShow: true,
        key: PermissionLoyaltyConst.LoyaltyMenu_HangThanhVien,
      },
    ],
  },
  {
    label: 'Quản lý yêu cầu',
    routerLink: ['/request-management'],
    isShow: true,
    key: PermissionLoyaltyConst.LoyaltyMenu_QLYeuCau,
    icon: 'pi pi-list',
    items: [
      {
        label: 'Yêu cầu đổi voucher',
        icon: '',
        routerLink: ['/request-management/change-voucher-request'],
        isShow: true,
        key: PermissionLoyaltyConst.LoyaltyMenu_YeuCauDoiVoucher,
      },
    ],
  },
  {
    label: 'Quản lý truyền thông',
    routerLink: ['/media-management'],
    isShow: true,
    key: PermissionLoyaltyConst.LoyaltyMenu_TruyenThong,
    icon: 'pi pi-send',
    items: [
      { 
        label: 'Hình ảnh', 
        icon: '', 
        routerLink: ['/media-management/media'], 
        isShow: true,
        key: PermissionLoyaltyConst.LoyaltyMenu_HinhAnh,
      },
      {
        label: 'Tin tức',
        icon: '',
        routerLink: ['/media-management/broadcast-news'],
        isShow: true,
        key: PermissionLoyaltyConst.LoyaltyMenu_TinTuc,
      },
    ],
  },
  {
    label: 'Quản lý thông báo',
    routerLink: ['/notification-management'],
    isShow: true,
    key: PermissionLoyaltyConst.LoyaltyMenu_ThongBao,
    icon: 'pi pi-comment',
    items: [
      {
        label: 'Thông báo hệ thống',
        icon: '',
        routerLink: ['/notification-management/system-notification-template'],
        isShow: true,
        key: PermissionLoyaltyConst.LoyaltyMenu_ThongBaoHeThong,
      },
      {
        label: 'Mẫu thông báo',
        icon: '',
        routerLink: ['/notification-management/notification-template'],
        isShow: true,
        key: PermissionLoyaltyConst.LoyaltyMenu_MauThongBao,
      },
      {
        label: 'Thông báo',
        icon: '',
        routerLink: ['/notification-management/notification-message'],
        isShow: true,
        key: PermissionLoyaltyConst.LoyaltyMenu_QLTB,
      },
    ],
  },
  {
    label: 'Báo cáo thống kê',
    routerLink: ['/export-report-management'],
    isShow: true,
    icon: 'pi pi-file',
    key: PermissionLoyaltyConst.LoyaltyMenu_BaoCao,
    items: [
      {
        label: 'Báo cáo quản trị',
        icon: '',
        routerLink: ['/export-report-management/management-report'],
        isShow: true,
        key: PermissionLoyaltyConst.Loyalty_BaoCao_QuanTri,
      },
    ]
  },
  {
    label: 'Chương trình trúng thưởng',
    icon: 'pi pi-star',
    routerLink: ['/prize-draw-management'],
    isShow: true,
    key: PermissionLoyaltyConst.LoyaltyMenu_CT_TrungThuong
  }
];

export const routes: Route[] = [
  {
    path: 'customer-management',
    children: [
      {
        path: 'individual-customer',
        children: [
          {
            path: '',
            component: IndividualCustomerComponent,
            data: {permissions: [PermissionLoyaltyConst.LoyaltyMenu_KhachHangCaNhan]}, 
            canActivate: [AppRouteGuard],
          },
          {
            path: 'detail/:id',
            component: IndividualCustomerDetailComponent,
            data: {permissions: [PermissionLoyaltyConst.Loyalty_KhachHangCaNhan_ChiTiet]}, 
            canActivate: [AppRouteGuard],
          },
          {
            path: 'edit/:id',
            component: IndividualCustomerDetailComponent,
            data: {permissions: [PermissionLoyaltyConst.Loyalty_KhachHangCaNhan_CapNhat]}, 
            canActivate: [AppRouteGuard],
          },
        ],
      },
    ],
  },
  {
    path: 'voucher-management',
    children: [
      {
        path: 'list-voucher',
        children: [
          {
            path: '',
            component: VoucherManagementComponent,
            data: {permissions: [PermissionLoyaltyConst.LoyaltyMenu_DanhSachUuDai]}, 
            canActivate: [AppRouteGuard],
          },
        ],
      },
      {
        path: 'apply-history',
        children: [
          {
            path: '',
            component: ApplyVoucherHistoryComponent,
            data: {permissions: [PermissionLoyaltyConst.LoyaltyMenu_LichSuCapPhat]}, 
            canActivate: [AppRouteGuard],
          },
        ],
      },
    ],
  },
  {
    path: 'media-management',
    children: [
      {
        path: 'media',
        component: MediaComponent,
        data: {permissions: [PermissionLoyaltyConst.LoyaltyMenu_HinhAnh]}, 
        canActivate: [AppRouteGuard],
      },
      {
        path: 'knowledge-base',
        component: KnowledgeBaseComponent,
        canActivate: [AppRouteGuard],
      },
      {
        path: 'broadcast-news',
        data: {permissions: [PermissionLoyaltyConst.LoyaltyMenu_TinTuc]}, 
        component: BroadcastNewsComponent,
        canActivate: [AppRouteGuard],
      },
    ],
  },
  {
    path: 'notification-management',
    children: [
      {
        path: 'notification-template',
        component: NotificationTemplateComponent,
        canActivate: [AppRouteGuard],
        data: {permissions: [PermissionLoyaltyConst.LoyaltyMenu_MauThongBao]}, 
      },
      {
        path: 'notification-message',
        component: NotificationComponent,
        canActivate: [AppRouteGuard],
        data: {permissions: [PermissionLoyaltyConst.LoyaltyMenu_QLTB]}, 
      },
      {
        path: 'notification-message/detail',
        component: NotificationDetailComponent,
        canActivate: [AppRouteGuard],
        data: {permissions: [PermissionLoyaltyConst.LoyaltyQLTB_PageChiTiet]}, 
      },
      {
        path: 'notification-message/detail/:id',
        component: NotificationDetailComponent,
        canActivate: [AppRouteGuard],
        data: {permissions: [PermissionLoyaltyConst.LoyaltyQLTB_PageChiTiet]}, 
      },
      {
        path: 'system-notification-template',
        component: SystemTemplateComponent,
        canActivate: [AppRouteGuard],
        data: {permissions: [PermissionLoyaltyConst.LoyaltyMenu_ThongBaoHeThong]}, 
      },
    ],
  },
  {
    path: 'point-management',
    children: [
      {
        path: 'accumulate-point-management',
        children: [
          {
            path: '',
            component: AccumulatePointManagementComponent,
            canActivate: [AppRouteGuard],
            data: {permissions: [PermissionLoyaltyConst.LoyaltyMenu_TichDiem]}, 
          },
        ],
      },
      {
        path: 'membership-level-management',
        children: [
          {
            path: '',
            component: MembershipLevelManagementComponent,
            canActivate: [AppRouteGuard],
            data: {permissions: [PermissionLoyaltyConst.LoyaltyMenu_HangThanhVien]}, 
          },
        ],
      },
    ],
  },
  {
    path: 'request-management',
    children: [
      {
        path: 'change-voucher-request',
        children: [
          {
            path: '',
            component: ChangeVoucherRequestComponent,
            data: {permissions: [PermissionLoyaltyConst.LoyaltyMenu_YeuCauDoiVoucher]}, 
            canActivate: [AppRouteGuard],
          },
        ],
      },
    ],
  },
  {
    path: 'export-report-management',
    children: [
      {
        path: 'management-report',
        children: [
          {
            path: '',
            component: ManagementReportComponent,
            data: {permissions: [PermissionLoyaltyConst.Loyalty_BaoCao_QuanTri]}, 
            canActivate: [AppRouteGuard]
          }

        ],
      }
    ]
  },
  {
    path: 'prize-draw-management',
    children: [
      { path: "", component: PrizeDrawManagementComponent, data: {permissions: [PermissionLoyaltyConst.LoyaltyCT_TrungThuong_DanhSach]}, canActivate: [AppRouteGuard]},
      { path: "prize-draw/create", component: CreatePrizeDrawComponent, canActivate: [AppRouteGuard],
        children: [
          { path:'', redirectTo: 'program-infomation', pathMatch: 'full'},
          { path: 'program-infomation', component: ProgramInfomationComponent},
          { path: 'program-configuration', component: ProgramConfigurationComponent},
        ]
      },
      { path: 'detail/:id', component: DetailPrizeDrawComponent, data: {permissions: [PermissionLoyaltyConst.LoyaltyCT_TrungThuong_PageChiTiet]}, canActivate: [AppRouteGuard]},
      { path: 'join-setting/:id', component: JoinSettingPrizeDrawComponent, data: {permissions: [PermissionLoyaltyConst.LoyaltyCT_TrungThuong_CaiDatThamGia]}, canActivate: [AppRouteGuard]}
      // { path: 'detail', component: DetailPrizeDrawComponent}
    ]
  }
];
