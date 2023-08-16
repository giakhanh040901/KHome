import { ChangeDetectorRef, Component, Injector } from '@angular/core';
import {
  EPositionFrozenCell,
  EPositionTextCell,
  ETypeDataTable,
  ETypeFormatDate,
  ETypeStatus,
  FormNotificationConst,
  SaleTicketOrder,
  SearchConst,
  TransactionProcessing,
  ValidSaleTicket,
} from '@shared/AppConsts';
import { FormNotificationComponent } from '@shared/components/form-notification/form-notification.component';
import { FormSetDisplayColumnComponent } from '@shared/components/form-set-display-column/form-set-display-column.component';
import { HoldOrLockTicketCustomerComponent } from '@shared/components/hold-or-lock-ticket-customer/hold-or-lock-ticket-customer.component';
import { CrudComponentBase } from '@shared/crud-component-base';
import { formatCurrency, formatDate } from '@shared/function-common';
import {
  IActionTable,
  IConfigDataModal,
  IDropdown,
  IHeaderColumn,
  INotiDataModal,
  IValueFormatter,
} from '@shared/interface/InterfaceConst.interface';
import { HoldOrLockTicketCustomerModel } from '@shared/interface/sale-ticket-management/sale-ticket-order/HoldOrLockTicketCustomer.model';
import { ValidSaleTicketModel } from '@shared/interface/sale-ticket-management/valid-sale-ticket/ValidSaleTicket.model';
import { Page } from '@shared/model/page';
import { RouterService } from '@shared/services/router.service';
import { SaleTicketOrderService } from '@shared/services/sale-ticket-order.service';
import { MessageService } from 'primeng/api';
import { DialogService } from 'primeng/dynamicdialog';
import { debounceTime } from 'rxjs/operators';
import { BreadcrumbService } from 'src/app/layout/breadcrumb/breadcrumb.service';

@Component({
  selector: 'valid-sale-ticket',
  templateUrl: './valid-sale-ticket.component.html',
  styleUrls: ['./valid-sale-ticket.component.scss'],
})
export class ValidSaleTicketComponent extends CrudComponentBase {
  constructor(
    injector: Injector,
    messageService: MessageService,
    private dialogService: DialogService,
    private breadcrumbService: BreadcrumbService,
    private routerService: RouterService,
    private changeDetectorRef: ChangeDetectorRef,
    private saleTicketOrderService: SaleTicketOrderService
  ) {
    super(injector, messageService);
    this.breadcrumbService.setItems([
      { label: 'Trang chủ', routerLink: ['/home'] },
      { label: 'Quản lý bán vé', routerLink: ['/home'] },
      { label: 'Vé bán hợp lệ' },
    ]);
  }

  public dataSource: ValidSaleTicketModel[] = [];
  public isLoading: boolean;
  public page: Page;
  public listAction: IActionTable[][] = [];
  public selectedColumns: IHeaderColumn[] = [];
  public headerColumns: IHeaderColumn[] = [];
  public filter: {
    keyword: string;
    event: number[] | undefined;
    status: number[] | undefined;
    source: number[] | undefined;
  } = {
    keyword: '',
    event: undefined,
    status: undefined,
    source: undefined,
  };
  public listEvent: IDropdown[] = [];

  public get listStatus() {
    return ValidSaleTicket.listStatus;
  }
  public get listSource() {
    return ValidSaleTicket.listSource;
  }

  public getStatusSeverity(code: any) {
    return ValidSaleTicket.getStatus(code, ETypeStatus.SEVERITY);
  }

  public getStatusName(code: any) {
    return ValidSaleTicket.getStatus(code, ETypeStatus.LABEL);
  }

  public get listOnineOffline() {
    return TransactionProcessing.listOnineOffline;
  }

  public get listReason() {
    return SaleTicketOrder.listReason;
  }

  ngOnInit(): void {
    this.saleTicketOrderService.getEventActiveToOrder();

    this.headerColumns = [
      {
        field: 'id',
        header: '#ID',
        width: '5rem',
        isPin: true,
        type: ETypeDataTable.INDEX,
        posTextCell: EPositionTextCell.CENTER,
        isFrozen: true,
        posFrozen: EPositionFrozenCell.LEFT,
      },
      {
        field: 'requestCode',
        header: 'Mã yêu cầu',
        width: '15rem',
        isPin: true,
        type: ETypeDataTable.TEXT,
      },
      {
        field: 'customerName',
        header: 'Tên khách hàng',
        width: '20rem',
        isPin: true,
        type: ETypeDataTable.TEXT,
      },
      {
        field: 'customerPhone',
        header: 'SĐT khách hàng',
        width: '15rem',
        isPin: true,
        type: ETypeDataTable.TEXT,
      },
      {
        field: 'event',
        header: 'Sự kiện',
        minWidth: '30rem',
        maxWidth: '90vw',
        isPin: true,
        type: ETypeDataTable.TEXT,
      },
      {
        field: 'ticketQuantity',
        header: 'Số lượng vé',
        width: '10rem',
        isPin: true,
        type: ETypeDataTable.TEXT,
      },
      {
        field: 'value',
        header: 'Số tiền (VND)',
        width: '10rem',
        type: ETypeDataTable.TEXT,
      },
      {
        field: 'approveRequest',
        header: 'Duyêt yêu cầu',
        width: '12rem',
        type: ETypeDataTable.TEXT,
      },
      {
        field: 'type',
        header: 'Loại',
        width: '8rem',
        type: ETypeDataTable.TEXT,
        valueFormatter: (param: IValueFormatter) =>
          this.listOnineOffline.find((e: IDropdown) => e.value === param.data)?.label || '',
      },
      {
        field: 'source',
        header: 'Nguồn',
        width: '10rem',
        type: ETypeDataTable.TEXT,
        valueFormatter: (param: IValueFormatter) =>
          this.listSource.find((e: IDropdown) => e.value === param.data)?.label || '',
      },
      {
        field: 'status',
        header: 'Trạng thái',
        width: '8rem',
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
      },
    ].map((item: IHeaderColumn, index: number) => {
      item.position = index + 1;
      return item;
    });
    this.selectedColumns = this.getLocalStorage(ValidSaleTicket.keyStorage) ?? this.headerColumns;
    this.setPage();
  }

  ngAfterViewInit() {
    this.subject.keyword.pipe(debounceTime(SearchConst.DEBOUNCE_TIME)).subscribe(() => {
      if (this.filter.keyword === '') {
        this.setPage();
      } else {
        this.setPage();
      }
    });

    this.saleTicketOrderService._listEventActiveToOrder$.subscribe((res: IDropdown[]) => {
      if (res) {
        this.listEvent = res;
      }
    });

    this.changeDetectorRef.detectChanges();
    this.changeDetectorRef.markForCheck();
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
          this.setLocalStorage(this.selectedColumns, ValidSaleTicket.keyStorage);
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
    this.listAction = this.dataSource.map((data: ValidSaleTicketModel, index: number) => {
      const actions: IActionTable[] = [];

      if(this.isGranted(this.PermissionEventConst.VeBanHopLe_ChiTiet)) {
        actions.push({
          data: data,
          label: 'Xem chi tiết lệnh',
          icon: 'pi pi-info-circle',
          command: ($event) => {
            this.detail($event.item.data);
          },
        });
      }
      
      if(this.isGranted(this.PermissionEventConst.VeBanHopLe_YeuCauHoaDon)) {
        actions.push({
          data: data,
          label: 'Yêu cầu hóa đơn',
          icon: 'pi pi-pencil',
          command: ($event) => {
            this.requestBill($event.item.data);
          },
        });
      }
      
      if(this.isGranted(this.PermissionEventConst.VeBanHopLe_YeuCauVeCung)) {
        actions.push({
          data: data,
          label: 'Yêu cầu vé cứng',
          icon: 'pi pi-pencil',
          command: ($event) => {
            this.requestTicket($event.item.data);
          },
        });
      }

      // trạng thái === HOP_LE
      if (this.isGranted(this.PermissionEventConst.VeBanHopLe_KhoaYeuCau)) {
        if(data.status === ValidSaleTicket.HOP_LE) {
          actions.push({
            data: data,
            label: 'Khóa yêu cầu',
            icon: 'pi pi-pencil',
            command: ($event) => {
              this.lock($event.item.data);
            },
          });
        }
        //
        if (data.status === ValidSaleTicket.TAM_KHOA) {
          actions.push({
            data: data,
            label: 'Mở khóa',
            icon: 'pi pi-pencil',
            command: ($event) => {
              this.unlock($event.item.data);
            },
          });
        }
      }
      //
      return actions;
    });
  }

  public detail(data: ValidSaleTicketModel) {
    if (data) {
      if (data) {
        this.routerService.routerNavigate([
          '/sale-ticket-management/sale-ticket-order/detail/' + this.cryptEncode(data.id),
        ]);
      }
    }
  }

  public requestBill(data: ValidSaleTicketModel) {
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
          title: 'Tạo yêu cầu nhận hóa đơn?',
          icon: FormNotificationConst.IMAGE_APPROVE,
        } as INotiDataModal,
      });
      ref.onClose.subscribe((dataCallBack) => {
        if (dataCallBack?.accept) {
          this.saleTicketOrderService.requestBill(data.id).subscribe(
            (response) => {
              if (this.handleResponseInterceptor(response, 'Tạo yêu cầu thành công')) {
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

  public requestTicket(data: ValidSaleTicketModel) {
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
          title: 'Tạo yêu cầu vé cứng?',
          icon: FormNotificationConst.IMAGE_APPROVE,
        } as INotiDataModal,
      });
      ref.onClose.subscribe((dataCallBack) => {
        if (dataCallBack?.accept) {
          this.saleTicketOrderService.requestTicket(data.id).subscribe(
            (response) => {
              if (this.handleResponseInterceptor(response, 'Tạo yêu cầu thành công')) {
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

  public lock(data: ValidSaleTicketModel) {
    if (data) {
      let dataSource = new HoldOrLockTicketCustomerModel();
      dataSource.id = data.id;
      dataSource.labelReason = 'Chọn lý do khóa';
      dataSource.listReason = this.listReason;
      dataSource.isShowTime = false;
      dataSource.createUser = this.saleTicketOrderService.currentUser;
      dataSource.service = this.saleTicketOrderService.lockValidSaleTicket.bind(this.saleTicketOrderService);
      dataSource.mapToObject = (dto: HoldOrLockTicketCustomerModel) => {
        return {
          orderId: dto.id,
          reason: dto.reason,
          summary: dto.content,
        };
      };

      const ref = this.dialogService.open(HoldOrLockTicketCustomerComponent, {
        header: 'Xác nhận khóa danh sách vé của khách hàng',
        width: '600px',
        baseZIndex: 10000,
        data: {
          dataSource: dataSource,
        } as IConfigDataModal,
      });
      ref.onClose.subscribe((response) => {
        if (this.handleResponseInterceptor(response, '')) {
          this.messageService.add({
            severity: 'success',
            summary: '',
            detail: 'Khóa danh sách vé của khách hàng thành công',
            life: 1500,
          });
          this.setPage();
        }
      });
    }
  }

  public unlock(data: ValidSaleTicketModel) {
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
          title: 'Xác nhận mở khóa danh sách vé của khách hàng?',
          icon: FormNotificationConst.IMAGE_APPROVE,
        } as INotiDataModal,
      });
      ref.onClose.subscribe((dataCallBack) => {
        if (dataCallBack?.accept) {
          this.saleTicketOrderService.unlockValidSaleTicket(data.id).subscribe(
            (response) => {
              if (this.handleResponseInterceptor(response, 'Mở khóa danh sách vé của khách hàng thành công')) {
                this.setPage();
              }
            },
            (err) => {
              this.messageError(`Mở khóa danh sách vé của khách hàng không thành công`);
            }
          );
        }
      });
    }
  }

  public changeFilter(event: any) {
    this.setPage();
  }

  public changePage(event: any) {
    if (event) {
      this.setPage();
    }
  }

  public setPage() {
    this.isLoading = true;
    this.page.keyword = this.filter.keyword;

    this.saleTicketOrderService.findAllValidSaleTicket(this.page, this.filter).subscribe(
      (res) => {
        this.isLoading = false;
        if (this.handleResponseInterceptor(res, '')) {
          this.page.totalItems = res.data.totalItems;
          if (res.data?.items) {
            this.dataSource = res.data.items.map(
              (item: any) =>
                ({
                  id: item.id,
                  requestCode: item.contractCode,
                  customerName: item.fullname,
                  customerPhone: item.phone,
                  event: item.eventName,
                  ticketQuantity: item.quantity,
                  value: item.totalMoney ? formatCurrency(item.totalMoney) : '',
                  approveRequest: item.approveDate ? formatDate(item.approveDate, ETypeFormatDate.DATE_TIME) : '',
                  type: item.source,
                  source: item.orderSource,
                  status: !!item.isLock,
                } as ValidSaleTicketModel)
            );
            this.genListAction();
          }
        }
      },
      (err) => {
        this.isLoading = false;
      }
    );
  }
}
