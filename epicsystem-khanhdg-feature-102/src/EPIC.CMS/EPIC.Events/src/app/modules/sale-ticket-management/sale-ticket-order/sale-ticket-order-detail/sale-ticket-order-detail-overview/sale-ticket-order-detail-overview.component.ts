import { ChangeDetectorRef, Component, Injector } from '@angular/core';
import { EConfigDataModal, FormNotificationConst, SaleTicketOrder, YES_NO } from '@shared/AppConsts';
import { FormNotificationComponent } from '@shared/components/form-notification/form-notification.component';
import { CrudComponentBase } from '@shared/crud-component-base';
import { IConfigDataModal, INotiDataModal } from '@shared/interface/InterfaceConst.interface';
import { IntroduceSearchModel } from '@shared/interface/sale-ticket-management/sale-ticket-order/CreateSaleTicketOrder.model';
import { SaleTicketOrderDetailOverviewModel } from '@shared/interface/sale-ticket-management/sale-ticket-order/SaleTicketOrderDetailOverview.model';
import { RouterService } from '@shared/services/router.service';
import { SaleTicketOrderService } from '@shared/services/sale-ticket-order.service';
import { SpinnerService } from '@shared/services/spinner.service';
import { MessageService } from 'primeng/api';
import { DialogService } from 'primeng/dynamicdialog';
import { SaleTicketOrderSearchSaleComponent } from '../../components/sale-ticket-order-search-sale/sale-ticket-order-search-sale.component';

@Component({
  selector: 'sale-ticket-order-detail-overview',
  templateUrl: './sale-ticket-order-detail-overview.component.html',
  styleUrls: ['./sale-ticket-order-detail-overview.component.scss'],
})
export class SaleTicketOrderDetailOverviewComponent extends CrudComponentBase {
  constructor(
    injector: Injector,
    messageService: MessageService,
    private changeDetectorRef: ChangeDetectorRef,
    private saleTicketOrderService: SaleTicketOrderService,
    private dialogService: DialogService,
    private spinnerService: SpinnerService,
    private routerService: RouterService
  ) {
    super(injector, messageService);
  }
  public dto: SaleTicketOrderDetailOverviewModel = new SaleTicketOrderDetailOverviewModel();
  public isEdit: boolean = false;

  public get isDisable() {
    return !this.isEdit;
  }

  public get YES_NO() {
    return YES_NO;
  }

  ngOnInit() {
    this.isEdit = this.saleTicketOrderService.isEdit;
    this.saleTicketOrderService.getSaleTicketOrderDetail();
  }

  ngAfterViewInit() {
    this.saleTicketOrderService._saleTicketOrderDetail$.subscribe((res: any) => {
      if (res && res.data) {
        this.dto.mapDTO(res.data);
        this.saleTicketOrderService.selectedOrderStatus = this.dto.status;
        this.saleTicketOrderService.totalValue = res.data.totalMoney;
        this.saleTicketOrderService.requestCode = res.data.contractCodeGen || res.data.contractCode || '';
      }
    });
  }

  public edit(event: any) {
    if (event) {
      this.isEdit = true;
    }
  }

  public save(event: any) {
    if (event) {
      if (this.dto.isValidData()) {
        this.saleTicketOrderService.updateSaleTicketOrder(this.dto.toObjectSendToAPI()).subscribe(
          (response) => {
            if (this.handleResponseInterceptor(response, 'Lưu thành công')) {
              this.isEdit = false;
              this.saleTicketOrderService.getSaleTicketOrderDetail();
            }
          },
          (err) => {}
        );
      } else {
        this.messageDataValidator(this.dto.dataValidator);
      }
    }
  }

  public onChangeOrderQuantity(event: any, index: number) {
    if (event) {
      this.dto.ticketInfor[index].total = Number(this.dto.ticketInfor[index].value) * event.value;
    }
  }

  public onClickChooseSearchSale(event: any) {
    if (event) {
      const ref = this.dialogService.open(SaleTicketOrderSearchSaleComponent, {
        header: 'Tìm kiếm Tư vấn viên',
        width: '1200px',
        baseZIndex: 10000,
        data: {
          dataSource: {
            id: undefined,
            code: this.dto.saleCode,
            name: this.dto.saleName,
            phone: this.dto.salePhone,
            email: this.dto.saleEmail,
            transaction: this.dto.transactionRoom,
            contractTransaction: this.dto.transactionRoomCommand,
          } as IntroduceSearchModel,
          type: EConfigDataModal.CREATE,
        } as IConfigDataModal,
      });
      ref.onClose.subscribe((response) => {
        if (response) {
          this.dto.saleCode = response.code;
          this.dto.saleName = response.name;
          this.dto.salePhone = response.phone;
          this.dto.saleEmail = response.email;
          this.dto.transactionRoom = response.transaction;
          this.dto.transactionRoomCommand = response.contractTransaction;
        }
      });
    }
  }

  public get showListButtonRequest() {
    return this.dto.status === SaleTicketOrder.HOP_LE;
  }

  public requestBill(event: any) {
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
          title: 'Tạo yêu cầu nhận hóa đơn?',
          icon: FormNotificationConst.IMAGE_APPROVE,
        } as INotiDataModal,
      });
      ref.onClose.subscribe((dataCallBack) => {
        if (dataCallBack?.accept) {
          this.saleTicketOrderService.requestBill(this.saleTicketOrderService.selectedOrderId).subscribe(
            (response) => {
              if (this.handleResponseInterceptor(response, 'Tạo yêu cầu thành công')) {
                this.saleTicketOrderService.getSaleTicketOrderDetail();
              }
            },
            (err) => {
              this.messageError(`Không tạo được yêu cầu`);
            }
          );
        }
      });
    }
  }

  public requestTicket(event: any) {
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
          title: 'Tạo yêu cầu vé cứng?',
          icon: FormNotificationConst.IMAGE_APPROVE,
        } as INotiDataModal,
      });
      ref.onClose.subscribe((dataCallBack) => {
        if (dataCallBack?.accept) {
          this.saleTicketOrderService.requestTicket(this.saleTicketOrderService.selectedOrderId).subscribe(
            (response) => {
              if (this.handleResponseInterceptor(response, 'Tạo yêu cầu thành công')) {
                this.saleTicketOrderService.getSaleTicketOrderDetail();
              }
            },
            (err) => {
              this.messageError(`Không tạo được yêu cầu`);
            }
          );
        }
      });
    }
  }

  public changeReferralCode(event: any) {
    if (event) {
      const ref = this.dialogService.open(SaleTicketOrderSearchSaleComponent, {
        header: 'Tìm kiếm Tư vấn viên',
        width: '1200px',
        baseZIndex: 10000,
        data: {
          dataSource: {
            id: undefined,
            code: this.dto.saleCode,
            name: this.dto.saleName,
            phone: this.dto.salePhone,
            email: this.dto.saleEmail,
            transaction: this.dto.transactionRoom,
            contractTransaction: this.dto.transactionRoomCommand,
          } as IntroduceSearchModel,
          type: EConfigDataModal.CREATE,
        } as IConfigDataModal,
      });
      ref.onClose.subscribe((response) => {
        if (response) {
          this.saleTicketOrderService
            .changeReferralCode({
              orderId: this.saleTicketOrderService.selectedOrderId,
              saleReferralCode: response.code,
            })
            .subscribe(
              (response) => {
                if (this.handleResponseInterceptor(response, 'Thay đổi mã tư vấn viên thành công')) {
                  this.saleTicketOrderService.getSaleTicketOrderDetail();
                }
              },
              (err) => {
                this.messageError(`Thay đổi mã tư vấn viên thất bại`);
              }
            );
        }
      });
    }
  }

  public get showButtonRemove() {
    return this.dto.status !== SaleTicketOrder.CHO_XU_LY && this.dto.status !== SaleTicketOrder.HOP_LE;
  }

  public delete(event: any) {
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
          title: 'Bạn có chắc chắn muốn xóa lệnh?',
          icon: FormNotificationConst.IMAGE_CLOSE,
        } as INotiDataModal,
      });
      ref.onClose.subscribe((dataCallBack) => {
        if (dataCallBack?.accept) {
          this.saleTicketOrderService.deleteSaleTicketOrder(this.saleTicketOrderService.selectedOrderId).subscribe(
            (response) => {
              if (this.handleResponseInterceptor(response, 'Xóa thành công')) {
                this.routerService.routerNavigate([this.routerService.previousUrl]);
              }
            },
            (err) => {
              this.messageError(`Không xóa được lệnh`);
            }
          );
        }
      });
    }
  }
}
