import { ChangeDetectorRef, Component, Injector } from '@angular/core';
import {
  EConfigDataModal,
  EPositionFrozenCell,
  EPositionTextCell,
  ETypeDataTable,
  ETypeFormatDate,
  ETypeStatus,
  FormNotificationConst,
  SaleTicketOrder,
} from '@shared/AppConsts';
import { FormNotificationComponent } from '@shared/components/form-notification/form-notification.component';
import { FormSetDisplayColumnComponent } from '@shared/components/form-set-display-column/form-set-display-column.component';
import { CrudComponentBase } from '@shared/crud-component-base';
import { formatCurrency, formatDate } from '@shared/function-common';
import {
  IActionTable,
  IConfigDataModal,
  IDropdown,
  IHeaderColumn,
  IImage,
  INotiDataModal,
  IValueFormatter,
} from '@shared/interface/InterfaceConst.interface';
import {
  CrudSaleTicketTransaction,
  SaleTicketOrderDetailTransactionModel,
} from '@shared/interface/sale-ticket-management/sale-ticket-order/SaleTicketOrderDetailTransaction.model';
import { RouterService } from '@shared/services/router.service';
import { SaleTicketOrderService } from '@shared/services/sale-ticket-order.service';
import { SignalrService } from '@shared/services/signalr.service';
import { MessageService } from 'primeng/api';
import { DialogService } from 'primeng/dynamicdialog';
import { Subscription } from 'rxjs';
import { BreadcrumbService } from 'src/app/layout/breadcrumb/breadcrumb.service';
import { environment } from 'src/environments/environment';
import { CrudSaleTicketTransactionComponent } from './crud-sale-ticket-transaction/crud-sale-ticket-transaction.component';
import { EmulatorMsbDialogComponent } from './emulator-msb-dialog/emulator-msb-dialog.component';

@Component({
  selector: 'sale-ticket-order-detail-transaction',
  templateUrl: './sale-ticket-order-detail-transaction.component.html',
  styleUrls: ['./sale-ticket-order-detail-transaction.component.scss'],
})
export class SaleTicketOrderDetailTransactionComponent extends CrudComponentBase {
  public listCardInfor: {
    bgIcon: string;
    icon: IImage;
    colorLabel: string;
    label: string;
    colorValue: string;
    value: string;
  }[] = [];
  public headerColumns: IHeaderColumn[] = [];
  public dataSource: SaleTicketOrderDetailTransactionModel[] = [];
  public listAction: IActionTable[][] = [];
  public selectedColumns: IHeaderColumn[] = [];
  public total: number = 0;
  public receiveMSBNotificationSubscription: Subscription = new Subscription();

  constructor(
    injector: Injector,
    messageService: MessageService,
    private dialogService: DialogService,
    private breadcrumbService: BreadcrumbService,
    private routerService: RouterService,
    private changeDetectorRef: ChangeDetectorRef,
    private saleTicketOrderService: SaleTicketOrderService,
    private signalrService: SignalrService
  ) {
    super(injector, messageService);
  }

  public get listTypeTransaction() {
    return SaleTicketOrder.listTransactionType;
  }

  public get listRevenueExpenditure() {
    return SaleTicketOrder.listRevenueExpenditure;
  }

  public get listStatus() {
    return SaleTicketOrder.listStatusTransaction;
  }

  public getStatusSeverity(code: any) {
    return SaleTicketOrder.getStatusTransaction(code, ETypeStatus.SEVERITY);
  }

  public getStatusName(code: any) {
    return SaleTicketOrder.getStatusTransaction(code, ETypeStatus.LABEL);
  }

  ngOnInit() {
    this.listCardInfor = [
      {
        bgIcon: '#f3f1ff ',
        icon: {
          src: 'assets/layout/images/tong-tien-thanh-toan.svg',
          width: this.DEFAULT_SIZE,
          height: this.DEFAULT_SIZE,
        } as IImage,
        colorLabel: '#76809B',
        label: 'Tổng cần tiền thanh toán',
        colorValue: '#212633',
        value: '0 VND',
      },
      {
        bgIcon: '#fdf3ed',
        icon: {
          src: 'assets/layout/images/so-tien-da-thanh-toan.svg',
          width: this.DEFAULT_SIZE,
          height: this.DEFAULT_SIZE,
        } as IImage,
        colorLabel: '#76809B',
        label: 'Tổng tiền đã thanh toán',
        colorValue: '#212633',
        value: '0 VND',
      },
      {
        bgIcon: '#fdf8ed',
        icon: {
          src: 'assets/layout/images/so-tien-chenh-lech.svg',
          width: this.DEFAULT_SIZE,
          height: this.DEFAULT_SIZE,
        } as IImage,
        colorLabel: '#76809B',
        label: 'Tổng tiền chênh lệch',
        colorValue: '#212633',
        value: '0 VND',
      },
    ];

    this.headerColumns = [
      {
        field: 'code',
        header: 'Mã GD',
        type: ETypeDataTable.TEXT,
        width: '8rem',
      },
      {
        field: 'revenueExpenditure',
        header: 'Thu/ Chi',
        type: ETypeDataTable.TEXT,
        width: '8rem',
        valueFormatter: (param: IValueFormatter) =>
          this.listRevenueExpenditure.find((e: IDropdown) => e.value === param.data)?.label || '',
      },
      {
        field: 'type',
        header: 'Loại',
        type: ETypeDataTable.TEXT,
        width: '12rem',
        valueFormatter: (param: IValueFormatter) =>
          this.listTypeTransaction.find((e: IDropdown) => e.value === param.data)?.label || '',
      },
      {
        field: 'tradingDate',
        header: 'Ngày GD',
        type: ETypeDataTable.TEXT,
        width: '12rem',
      },
      {
        field: 'value',
        header: 'Số tiền',
        type: ETypeDataTable.TEXT,
        width: '12rem',
        valueFormatter: (param: IValueFormatter) => (param.data ? formatCurrency(param.data) : ''),
      },
      {
        field: 'description',
        header: 'Mô tả',
        type: ETypeDataTable.TEXT,
        width: 'auto',
      },
      {
        field: 'status',
        header: 'Trạng thái',
        width: '10rem',
        type: ETypeDataTable.STATUS,
        funcStyleClassStatus: this.funcStyleClassStatus,
        funcLabelStatus: this.funcLabelStatus,
        posTextCell: EPositionTextCell.LEFT,
        isFrozen: true,
        posFrozen: EPositionFrozenCell.RIGHT,
        class: 'b-border-frozen-right',
      },
      {
        field: '',
        header: '',
        width: '3rem',
        type: ETypeDataTable.ACTION,
        posTextCell: EPositionTextCell.CENTER,
        isFrozen: true,
        posFrozen: EPositionFrozenCell.RIGHT,
        hideBtnSetColumn: true,
      },
    ] as IHeaderColumn[];
    this.selectedColumns = this.getLocalStorage(SaleTicketOrder.keyStorageTransaction) ?? this.headerColumns;
    this.setPage();

    this.saleTicketOrderService.getListBankAccount();
  }

  ngAfterViewInit() {
    this.signalrService
      .startConnectionMSBNoti()
      .then(() => {
        // Đổi trạng thái giao dịch
        this.signalrService.listenReceiveMSBNotification();
        this.receiveMSBNotificationSubscription = this.signalrService._receiveMSBNotification$.subscribe((res: any) => {
          if (res && res.data) {
            this.dataSource.forEach((data: SaleTicketOrderDetailTransactionModel) => {
              if (data.id === res.data.id) {
                data.status = res.data.status;
              }
            });
          }
        });
      })
      .catch((err) => {});
  }

  public setColumn(event: any) {
    if (event) {
      const ref = this.dialogService.open(
        FormSetDisplayColumnComponent,
        this.getConfigDialogServiceDisplayTableColumn(this.headerColumns, this.selectedColumns)
      );
      ref.onClose.subscribe((dataCallBack) => {
        if (dataCallBack?.accept) {
          this.selectedColumns = dataCallBack.data.sort(function (a, b) {
            return a.position - b.position;
          });
          this.setLocalStorage(this.selectedColumns, SaleTicketOrder.keyStorageTransaction);
        }
      });
    }
  }

  public funcStyleClassStatus = (status: any) => {
    return this.getStatusSeverity(status);
  };

  public funcLabelStatus = (status: any) => {
    return this.getStatusName(status);
  };

  private genListAction() {
    this.listAction = this.dataSource.map((data: SaleTicketOrderDetailTransactionModel, index: number) => {
      const actions: IActionTable[] = [];

      if(this.isGranted(this.PermissionEventConst.SoLenh_ChiTiet_GiaoDich_ChiTiet)) {
        actions.push({
          data: data,
          label: 'Thông tin chi tiết',
          icon: 'pi pi-info-circle',
          command: ($event) => {
            this.detail($event.item.data);
          },
        });
      }
      
      if (data.status === SaleTicketOrder.TRINH_DUYET) {
        if (!this.hideButtonEdit && this.isGranted(this.PermissionEventConst.SoLenh_ChiTiet_GiaoDich_CapNhat)) {
          actions.push({
            data: data,
            label: 'Chỉnh sửa',
            icon: 'pi pi-pencil',
            command: ($event) => {
              this.edit($event.item.data);
            },
          });
        }

        if (data.status === SaleTicketOrder.DA_THANH_TOAN && this.isGranted(this.PermissionEventConst.SoLenh_ChiTiet_GiaoDich_GuiThongBao)) {
          actions.push({
            data: data,
            label: 'Gửi thông báo',
            icon: 'pi pi-send',
            command: ($event) => {
              this.sendNoti($event.item.data);
            },
          });
        }

        if (data.status === SaleTicketOrder.TRINH_DUYET && this.isGranted(this.PermissionEventConst.SoLenh_ChiTiet_GiaoDich_PheDuyet)) {
          actions.push({
            data: data,
            label: 'Phê duyệt',
            icon: 'pi pi-check',
            command: ($event) => {
              this.approve($event.item.data);
            },
          });
        }

        if(this.isGranted(this.PermissionEventConst.SoLenh_ChiTiet_GiaoDich_HuyThanhToan)) {
          actions.push({
            data: data,
            label: 'Hủy thanh toán',
            icon: 'pi pi-trash',
            command: ($event) => {
              this.cancel($event.item.data);
            },
          });
        }
        
      }

      return actions;
    });
  }

  public detail(data: SaleTicketOrderDetailTransactionModel) {
    if (data) {
      this.saleTicketOrderService.saleTicketOrderTransactionDetail(data.id).subscribe((res: any) => {
        if (res) {
          this.dialogService.open(CrudSaleTicketTransactionComponent, {
            header: 'Thông tin chi tiết',
            width: '1200px',
            baseZIndex: 10000,
            data: {
              dataSource: res.data,
              type: EConfigDataModal.VIEW,
            } as IConfigDataModal,
          });
        }
      });
    }
  }

  public edit(data: SaleTicketOrderDetailTransactionModel) {
    if (data) {
      this.saleTicketOrderService.saleTicketOrderTransactionDetail(data.id).subscribe((res: any) => {
        if (res) {
          const ref = this.dialogService.open(CrudSaleTicketTransactionComponent, {
            header: 'Xem chi tiết',
            width: '1200px',
            baseZIndex: 10000,
            data: {
              dataSource: res.data,
              type: EConfigDataModal.EDIT,
            } as IConfigDataModal,
          });
          ref.onClose.subscribe((response) => {
            if (this.handleResponseInterceptor(response, '')) {
              this.messageService.add({
                severity: 'success',
                summary: '',
                detail: 'Chỉnh sửa thành công',
                life: 1500,
              });
              this.setPage();
            }
          });
        }
      });
    }
  }

  public cancel(data: SaleTicketOrderDetailTransactionModel) {
    if (data) {
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
          title: 'Bạn có chắc chắn muốn hủy thanh toán?',
          icon: FormNotificationConst.IMAGE_CLOSE,
        } as INotiDataModal,
      });
      ref.onClose.subscribe((dataCallBack) => {
        if (dataCallBack?.accept) {
          this.saleTicketOrderService.cancelSaleTicketOrderTransaction(data.id).subscribe(
            (response) => {
              if (this.handleResponseInterceptor(response, 'Hủy thanh toán thành công')) {
                this.setPage();
              }
            },
            (err) => {
              this.messageError(`Không hủy thanh toán được voucher`);
            }
          );
        }
      });
    }
  }

  public approve(data: SaleTicketOrderDetailTransactionModel) {
    if (data) {
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
          title: 'Bạn có chắc chắn muốn phê duyệt?',
          icon: FormNotificationConst.IMAGE_APPROVE,
        } as INotiDataModal,
      });
      ref.onClose.subscribe((dataCallBack) => {
        if (dataCallBack?.accept) {
          this.saleTicketOrderService.approveSaleTicketOrderTransaction(data.id).subscribe(
            (response) => {
              if (this.handleResponseInterceptor(response, 'Phê duyệt thành công')) {
                this.setPage();
              }
            },
            (err) => {
              this.messageError(`Phê duyệt thất bại`);
            }
          );
        }
      });
    }
  }

  public sendNoti(data: SaleTicketOrderDetailTransactionModel) {
    if (data) {
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
          title: 'Bạn có chắc chắn muốn gửi thông báo?',
          icon: FormNotificationConst.IMAGE_APPROVE,
        } as INotiDataModal,
      });
      ref.onClose.subscribe((dataCallBack) => {
        if (dataCallBack?.accept) {
          this.saleTicketOrderService.notiSaleTicketOrderTransaction(data.id).subscribe(
            (response) => {
              if (this.handleResponseInterceptor(response, 'Gửi thông báo thành công')) {
                this.setPage();
              }
            },
            (err) => {
              this.messageError(`Gửi thông báo thất bại`);
            }
          );
        }
      });
    }
  }

  public create(event: any) {
    if (event) {
      if (!this.total) {
        this.messageWarn('Số tiền chênh lệch bằng 0. Không thể thêm mới thanh toán!');
      } else {
        let dataSource: CrudSaleTicketTransaction = new CrudSaleTicketTransaction();
        dataSource.description = `TT ${this.saleTicketOrderService.requestCode || ''}`;
        dataSource.value = this.getRestValue();
        const ref = this.dialogService.open(CrudSaleTicketTransactionComponent, {
          header: 'Thêm mới',
          width: '1200px',
          baseZIndex: 10000,
          data: {
            type: EConfigDataModal.CREATE,
            dataSource: dataSource,
          } as IConfigDataModal,
        });
        ref.onClose.subscribe((response) => {
          if (this.handleResponseInterceptor(response, '')) {
            this.messageService.add({
              severity: 'success',
              summary: '',
              detail: 'Thêm mới thành công',
              life: 1500,
            });
            this.setPage();
          }
        });
      }
    }
  }

  private getRestValue() {
    return (
      Number(this.saleTicketOrderService.totalValue) -
      this.dataSource.reduce((res: number, item: SaleTicketOrderDetailTransactionModel) => {
        return (res += item.status === SaleTicketOrder.DA_THANH_TOAN ? Number(item.value) : 0);
      }, 0)
    );
  }

  public get DEFAULT_SIZE() {
    return 48;
  }

  public changePage(event: any) {
    if (event) {
      this.setPage();
    }
  }

  public setPage() {
    this.isLoading = true;

    this.saleTicketOrderService.findSaleTicketOrderTransaction(this.page).subscribe(
      (res) => {
        this.isLoading = false;
        if (this.handleResponseInterceptor(res, '')) {
          this.page.totalItems = res.data.totalItems;
          if (res.data?.items) {
            this.dataSource = res.data.items.map(
              (item: any) =>
                ({
                  id: item.id,
                  code: item.id,
                  revenueExpenditure: SaleTicketOrder.THU,
                  type: item.paymentType,
                  tradingDate: item.tranDate ? formatDate(item.tranDate, ETypeFormatDate.DATE) : '',
                  value: item.paymentAmount,
                  description: item.description,
                  status: item.status,
                } as SaleTicketOrderDetailTransactionModel)
            );
            this.genListAction();
          }
          this.getDataCardInfor();
        }
      },
      (err) => {
        this.isLoading = false;
      }
    );
  }

  private getDataCardInfor() {
    const valueCard1 = this.saleTicketOrderService.totalValue;
    const valueCard2 =
      this.dataSource && this.dataSource.length
        ? this.dataSource.reduce((res: number, value: SaleTicketOrderDetailTransactionModel) => {
            return (res += value.status === SaleTicketOrder.DA_THANH_TOAN ? Number(value.value) : 0);
          }, 0)
        : 0;
    this.total = valueCard1 - valueCard2;
    this.listCardInfor[0].value = formatCurrency(valueCard1) + ' VND';
    this.listCardInfor[1].value = formatCurrency(valueCard2) + ' VND';
    this.listCardInfor[2].value = formatCurrency(valueCard1 - valueCard2) + ' VND';
  }

  public get hideButtonEdit() {
    return this.saleTicketOrderService.selectedOrderStatus === SaleTicketOrder.HOP_LE;
  }

  public get environment() {
    return environment;
  }

  public emulatorMSB(event: any) {
    if (event) {
      const ref = this.dialogService.open(EmulatorMsbDialogComponent, {
        header: 'Giả lập MSB',
        width: '600px',
        contentStyle: { 'max-height': '600px', overflow: 'auto', 'margin-bottom': '60px' },
        baseZIndex: 10000,
        data: {
          type: EConfigDataModal.CREATE,
          dataSource: {
            tranAmount: this.getRestValue(),
          },
        } as IConfigDataModal,
      });
      ref.onClose.subscribe((res) => {
        this.setPage();
      });
    }
  }
}
