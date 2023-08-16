import { Component, Injector } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { FormNotificationConst, SaleTicketOrder } from '@shared/AppConsts';
import { FormNotificationComponent } from '@shared/components/form-notification/form-notification.component';
import { CrudComponentBase } from '@shared/crud-component-base';
import { INotiDataModal, ITabView } from '@shared/interface/InterfaceConst.interface';
import {
  CreateSaleTicketOrderEvent,
  CustomerSearchModel,
  IntroduceSearchModel,
} from '@shared/interface/sale-ticket-management/sale-ticket-order/CreateSaleTicketOrder.model';
import { RouterService } from '@shared/services/router.service';
import { SaleTicketOrderService } from '@shared/services/sale-ticket-order.service';
import { MessageService } from 'primeng/api';
import { DialogService } from 'primeng/dynamicdialog';
import { BreadcrumbService } from 'src/app/layout/breadcrumb/breadcrumb.service';
import { SaleTicketOrderDetailHistoryComponent } from './sale-ticket-order-detail-history/sale-ticket-order-detail-history.component';
import { SaleTicketOrderDetailListComponent } from './sale-ticket-order-detail-list/sale-ticket-order-detail-list.component';
import { SaleTicketOrderDetailOverviewComponent } from './sale-ticket-order-detail-overview/sale-ticket-order-detail-overview.component';
import { SaleTicketOrderDetailTransactionComponent } from './sale-ticket-order-detail-transaction/sale-ticket-order-detail-transaction.component';

@Component({
  selector: 'sale-ticket-order-detail',
  templateUrl: './sale-ticket-order-detail.component.html',
  styleUrls: ['./sale-ticket-order-detail.component.scss'],
})
export class SaleTicketOrderDetailComponent extends CrudComponentBase {
  public listTabPanel: ITabView[] = [];

  constructor(
    injector: Injector,
    messageService: MessageService,
    private breadcrumbService: BreadcrumbService,
    private routerService: RouterService,
    private routeActive: ActivatedRoute,
    private saleTicketOrderService: SaleTicketOrderService,
    private dialogService: DialogService
  ) {
    super(injector, messageService);
    this.breadcrumbService.setItems([
      { label: 'Trang chủ', routerLink: ['/home'] },
      { label: 'Quản lý bán vé', routerLink: ['/home'] },
      { label: 'Sổ lệnh', routerLink: ['/sale-ticket-management/sale-ticket-order'] },
      { label: 'Chi tiết sổ lệnh' },
    ]);

    this.saleTicketOrderService.selectedOrderId = +this.cryptDecode(this.routeActive.snapshot.paramMap.get('id'));
  }

  ngOnInit() {
    if(this.isGranted(
      [
        this.PermissionEventConst.SoLenh_ChiTiet_ThongTinChung, 
        this.PermissionEventConst.XuLyGiaoDich_ChiTiet_ThongTinChung, 
        this.PermissionEventConst.VeBanHopLe_ChiTiet_ThongTinChung,
        this.PermissionEventConst.YeuCauVeCung_ChiTiet_ThongTinChung,
        this.PermissionEventConst.YeuCauHoaDon_ChiTiet_ThongTinChung,
      ]
    )) {
      this.listTabPanel.push(
        {
          key: 'overview',
          title: 'Thông tin chung',
          component: SaleTicketOrderDetailOverviewComponent,
          isDisabled: false,
        }
      )
    }  
    //
    if(this.isGranted(
      [
        this.PermissionEventConst.SoLenh_ChiTiet_GiaoDich,
        this.PermissionEventConst.XuLyGiaoDich_ChiTiet_GiaoDich, 
        this.PermissionEventConst.VeBanHopLe_ChiTiet_GiaoDich,
        this.PermissionEventConst.YeuCauVeCung_ChiTiet_GiaoDich,
        this.PermissionEventConst.YeuCauHoaDon_ChiTiet_GiaoDich,
      ]
    )) {
      this.listTabPanel.push(
        {
          key: 'transaction',
          title: 'Giao dịch',
          component: SaleTicketOrderDetailTransactionComponent,
          isDisabled: false,
        }
      )
    }  
    //
    if(this.isGranted(
      [
        this.PermissionEventConst.SoLenh_ChiTiet_DanhSachVe,
        this.PermissionEventConst.XuLyGiaoDich_ChiTiet_DanhSachVe, 
        this.PermissionEventConst.VeBanHopLe_ChiTiet_DanhSachVe,
        this.PermissionEventConst.YeuCauVeCung_ChiTiet_DanhSachVe,
        this.PermissionEventConst.YeuCauHoaDon_ChiTiet_DanhSachVe,
      ]
    )) {
      this.listTabPanel.push(
        {
          key: 'list',
          title: 'Danh sách vé',
          component: SaleTicketOrderDetailListComponent,
          isDisabled: false,
        }
      )
    }  
    //
    if(this.isGranted(
      [
        this.PermissionEventConst.SoLenh_ChiTiet_LichSu,
        this.PermissionEventConst.XuLyGiaoDich_ChiTiet_LichSu, 
        this.PermissionEventConst.VeBanHopLe_ChiTiet_LichSu,
        this.PermissionEventConst.YeuCauVeCung_ChiTiet_LichSu,
        this.PermissionEventConst.YeuCauHoaDon_ChiTiet_LichSu,
      ]
    )) {
      this.listTabPanel.push(
        {
          key: 'history',
          title: 'Lịch sử',
          component: SaleTicketOrderDetailHistoryComponent,
          isDisabled: false,
        },
      )
    }  
    //
    this.saleTicketOrderService.isEdit = this.routerService.getRouterInclude('edit');
    this.refreshSelectedData();
  }

  private refreshSelectedData() {
    this.saleTicketOrderService.selectedOrder = {
      customer: new CustomerSearchModel(),
      introduce: new IntroduceSearchModel(),
      event: new CreateSaleTicketOrderEvent(),
    };
    this.saleTicketOrderService.dtoCustomer = new CustomerSearchModel();
    this.saleTicketOrderService.dtoIntroduce = new IntroduceSearchModel();
    this.saleTicketOrderService.dtoEvent = new CreateSaleTicketOrderEvent();
  }

  public back(event: any) {
    if (event) {
      this.routerService.routerNavigate([this.routerService.previousUrl]);
    }
  }

  public get showBtnApprove() {
    return this.saleTicketOrderService.selectedOrderStatus === SaleTicketOrder.CHO_XU_LY;
  }

  public approve(event: any) {
    if (event) {
      const ref = this.dialogService.open(FormNotificationComponent, {
        header: 'Thông báo',
        width: '600px',
        contentStyle: {
          'max-height': '600px',
          overflow: 'auto',
          'padding-bottom': '50px',
        },
        styleClass: 'p-dialog-custom',
        baseZIndex: 10000,
        data: {
          title: 'Phê duyệt mua vé?',
          icon: FormNotificationConst.IMAGE_APPROVE,
        } as INotiDataModal,
      });
      ref.onClose.subscribe((dataCallBack) => {
        if (dataCallBack?.accept) {
          this.saleTicketOrderService.approveSaleTicketOrder(this.saleTicketOrderService.selectedOrderId).subscribe(
            (response) => {
              if (this.handleResponseInterceptor(response, 'Phê duyệt mua vé thành công')) {
                this.saleTicketOrderService.getSaleTicketOrderDetail();
              }
            },
            (err) => {
              this.messageError(`Phê duyệt mua vé không thành công`);
            }
          );
        }
      });
    }
  }
}
