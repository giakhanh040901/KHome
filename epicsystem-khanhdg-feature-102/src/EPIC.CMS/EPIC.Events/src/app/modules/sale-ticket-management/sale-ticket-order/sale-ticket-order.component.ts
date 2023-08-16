import { ChangeDetectorRef, Component, Injector } from '@angular/core';
import { Router } from '@angular/router';
import {
  EPositionFrozenCell,
  EPositionTextCell,
  ETypeDataTable,
  ETypeFormatDate,
  ETypeStatus,
  FormNotificationConst,
  SaleTicketOrder,
  SearchConst,
} from '@shared/AppConsts';
import { FormNotificationComponent } from '@shared/components/form-notification/form-notification.component';
import { FormSetDisplayColumnComponent } from '@shared/components/form-set-display-column/form-set-display-column.component';
import { HoldOrLockTicketCustomerComponent } from '@shared/components/hold-or-lock-ticket-customer/hold-or-lock-ticket-customer.component';
import { CrudComponentBase } from '@shared/crud-component-base';
import { convertTimeToAPI, formatCurrency, formatDate } from '@shared/function-common';
import {
  IActionTable,
  IConfigDataModal,
  IDropdown,
  IHeaderColumn,
  INotiDataModal,
  IValueFormatter,
} from '@shared/interface/InterfaceConst.interface';
import { HoldOrLockTicketCustomerModel } from '@shared/interface/sale-ticket-management/sale-ticket-order/HoldOrLockTicketCustomer.model';
import { SaleTicketOrderModel } from '@shared/interface/sale-ticket-management/sale-ticket-order/SaleTicketOrder.model';
import { Page } from '@shared/model/page';
import { RouterService } from '@shared/services/router.service';
import { SaleTicketOrderService } from '@shared/services/sale-ticket-order.service';
import { MessageService } from 'primeng/api';
import { DialogService } from 'primeng/dynamicdialog';
import { debounceTime } from 'rxjs/operators';
import { BreadcrumbService } from 'src/app/layout/breadcrumb/breadcrumb.service';

@Component({
  selector: 'sale-ticket-order',
  templateUrl: './sale-ticket-order.component.html',
  styleUrls: ['./sale-ticket-order.component.scss'],
})
export class SaleTicketOrderComponent extends CrudComponentBase {
  constructor(
    injector: Injector,
    messageService: MessageService,
    private dialogService: DialogService,
    private breadcrumbService: BreadcrumbService,
    private router: Router,
    private changeDetectorRef: ChangeDetectorRef,
    public saleTicketOrderService: SaleTicketOrderService,
    private routerService: RouterService
  ) {
    super(injector, messageService);
    this.breadcrumbService.setItems([
      { label: 'Trang chủ', routerLink: ['/home'] },
      { label: 'Quản lý bán vé', routerLink: ['/home'] },
      { label: 'Sổ lệnh' },
    ]);
  }

  public dataSource: SaleTicketOrderModel[] = [];
  public isLoading: boolean;
  public page: Page;
  public listAction: IActionTable[][] = [];
  public selectedColumns: IHeaderColumn[] = [];
  public headerColumns: IHeaderColumn[] = [];
  public listEvent: IDropdown[] = [];
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

  public get listStatus() {
    return SaleTicketOrder.listStatus;
  }

  public get listSource() {
    return SaleTicketOrder.listSource;
  }

  public get listOnineOffline() {
    return SaleTicketOrder.listOnineOffline;
  }

  public getStatusSeverity(code: any) {
    return SaleTicketOrder.getStatus(code, ETypeStatus.SEVERITY);
  }

  public getStatusName(code: any) {
    return SaleTicketOrder.getStatus(code, ETypeStatus.LABEL);
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
        field: 'orderDate',
        header: 'Ngày đặt lệnh',
        width: '12rem',
        type: ETypeDataTable.TEXT,
      },
      {
        field: 'type',
        header: 'Loại',
        width: '12rem',
        type: ETypeDataTable.TEXT,
        valueFormatter: (param: IValueFormatter) =>
          this.listOnineOffline.find((e: IDropdown) => e.value === param.data)?.label || '',
      },
      {
        field: 'source',
        header: 'Nguồn',
        width: '12rem',
        type: ETypeDataTable.TEXT,
        valueFormatter: (param: IValueFormatter) =>
          this.listSource.find((e: IDropdown) => e.value === param.data)?.label || '',
      },
      {
        field: 'restTime',
        header: 'Thời gian còn lại',
        width: '12rem',
        type: ETypeDataTable.COUNT_DOWN_TIME,
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
      },
    ].map((item: IHeaderColumn, index: number) => {
      item.position = index + 1;
      return item;
    });
    this.selectedColumns = this.getLocalStorage(SaleTicketOrder.keyStorage) ?? this.headerColumns;
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
          this.setLocalStorage(this.selectedColumns, SaleTicketOrder.keyStorage);
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
    this.listAction = this.dataSource.map((data: SaleTicketOrderModel, index: number) => {
      const actions: IActionTable[] = [];

      if(this.isGranted(this.PermissionEventConst.SoLenh_ChiTiet)) {
        actions.push({
          data: data,
          label: 'Thông tin chi tiết',
          icon: 'pi pi-info-circle',
          command: ($event) => {
            this.detail($event.item.data);
          },
        });
      }
      
      if(this.isGranted(this.PermissionEventConst.SoLenh_ChiTiet_ThongTinChung_CapNhat)) {
        actions.push({
          data: data,
          label: 'Chỉnh sửa',
          icon: 'pi pi-pencil',
          command: ($event) => {
            this.edit($event.item.data);
          },
        });
      }
      
      if(this.isGranted(this.PermissionEventConst.SoLenh_GiaHanGiulenh)) {
        actions.push({
          data: data,
          label: 'Gia hạn giữ lệnh',
          icon: 'pi pi-pencil',
          command: ($event) => {
            this.hold($event.item.data);
          },
        });
      }
      
      if(this.isGranted(this.PermissionEventConst.SoLenh_Xoa)) {
        actions.push({
          data: data,
          label: 'Xóa',
          icon: 'pi pi-trash',
          command: ($event) => {
            this.delete($event.item.data);
          },
        });
      }
      return actions;
    });
  }

  public detail(data: SaleTicketOrderModel) {
    if (data) {
      this.routerService.routerNavigate([
        '/sale-ticket-management/sale-ticket-order/detail/' + this.cryptEncode(data.id),
      ]);
    }
  }

  public edit(data: SaleTicketOrderModel) {
    if (data) {
      this.routerService.routerNavigate([
        '/sale-ticket-management/sale-ticket-order/edit/' + this.cryptEncode(data.id),
      ]);
    }
  }

  public hold(data: SaleTicketOrderModel) {
    if (data) {
      let dataSource = new HoldOrLockTicketCustomerModel();
      dataSource.id = data.id;
      dataSource.labelReason = 'Chọn lý do gia hạn';
      dataSource.listReason = this.listReason;
      dataSource.isShowTime = true;
      dataSource.createUser = this.saleTicketOrderService.currentUser;
      dataSource.service = this.saleTicketOrderService.holdSaleTicketOrder.bind(this.saleTicketOrderService);
      dataSource.mapToObject = (dto: HoldOrLockTicketCustomerModel) => {
        return {
          orderId: dto.id,
          reason: dto.reason,
          keepTime: dto.time ? convertTimeToAPI(dto.time) : undefined,
          summary: dto.content,
        };
      };

      const ref = this.dialogService.open(HoldOrLockTicketCustomerComponent, {
        header: 'Xác nhận gia hạn yêu cầu mua vé',
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
            detail: 'Gia hạn yêu cầu mua vé thành công',
            life: 1500,
          });
          this.setPage();
        }
      });
    }
  }

  public delete(data: SaleTicketOrderModel) {
    if (data.id) {
      if (data.isHavePayment) {
        this.messageError(`Lệnh đã phát sinh giao dịch, không thể xóa!`);
      } else {
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
            title: 'Bạn có chắc chắn muốn xóa sổ lệnh?',
            icon: FormNotificationConst.IMAGE_CLOSE,
          } as INotiDataModal,
        });
        ref.onClose.subscribe((dataCallBack) => {
          if (dataCallBack?.accept) {
            this.saleTicketOrderService.deleteSaleTicketOrder(data.id).subscribe(
              (response) => {
                if (this.handleResponseInterceptor(response, 'Xóa thành công')) {
                  this.setPage();
                }
              },
              (err) => {
                this.messageError(`Không xóa được sổ lệnh`);
              }
            );
          }
        });
      }
    }
  }

  public create(event: any) {
    if (event) {
      this.router.navigate(['/sale-ticket-management/sale-ticket-order/create/']);
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

    this.saleTicketOrderService.findAllSaleTicketOrder(this.page, this.filter).subscribe(
      (res) => {
        this.isLoading = false;
        if (this.handleResponseInterceptor(res, '')) {
          this.page.totalItems = res.data.totalItems;
          if (res.data?.items) {
            this.dataSource = res.data.items.map((item: any) => {
              const restTimes = item.expiredTime
                ? new Date(item.expiredTime).getTime() - new Date().getTime()
                : undefined;
              const status = restTimes !== undefined && restTimes <= 0 ? SaleTicketOrder.HET_THOI_GIAN : item.status;
              return {
                id: item.id,
                requestCode: item.contractCode,
                customerName: item.fullname,
                customerPhone: item.phone,
                event: item.eventName,
                ticketQuantity: item.quantity,
                value: item.totalMoney ? formatCurrency(item.totalMoney) : '',
                orderDate: item.createdDate ? formatDate(item.createdDate, ETypeFormatDate.DATE_TIME) : '',
                type: item.source,
                source: item.orderSource,
                restTime: restTimes !== undefined ? (restTimes > 0 ? restTimes : 0) : undefined,
                isHavePayment: !!item.havePayment,
                status: status,
              } as SaleTicketOrderModel;
            });
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
