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
} from '@shared/AppConsts';
import { FormNotificationComponent } from '@shared/components/form-notification/form-notification.component';
import { FormSetDisplayColumnComponent } from '@shared/components/form-set-display-column/form-set-display-column.component';
import { CrudComponentBase } from '@shared/crud-component-base';
import { formatCurrency, formatDate } from '@shared/function-common';
import {
  IActionTable,
  IDropdown,
  IHeaderColumn,
  INotiDataModal,
  IValueFormatter,
} from '@shared/interface/InterfaceConst.interface';
import { TransactionProcessingModel } from '@shared/interface/sale-ticket-management/transaction-processing/TransactionProcessing.model';
import { Page } from '@shared/model/page';
import { RouterService } from '@shared/services/router.service';
import { SaleTicketOrderService } from '@shared/services/sale-ticket-order.service';
import { MessageService } from 'primeng/api';
import { DialogService } from 'primeng/dynamicdialog';
import { debounceTime } from 'rxjs/operators';
import { BreadcrumbService } from 'src/app/layout/breadcrumb/breadcrumb.service';

@Component({
  selector: 'transaction-processing',
  templateUrl: './transaction-processing.component.html',
  styleUrls: ['./transaction-processing.component.scss'],
})
export class TransactionProcessingComponent extends CrudComponentBase {
  constructor(
    injector: Injector,
    messageService: MessageService,
    private dialogService: DialogService,
    private breadcrumbService: BreadcrumbService,
    private routerService: RouterService,
    private changeDetectorRef: ChangeDetectorRef,
    public saleTicketOrderService: SaleTicketOrderService
  ) {
    super(injector, messageService);
    this.breadcrumbService.setItems([
      { label: 'Trang chủ', routerLink: ['/home'] },
      { label: 'Quản lý bán vé', routerLink: ['/home'] },
      { label: 'Xử lý giao dịch' },
    ]);
  }

  public dataSource: TransactionProcessingModel[] = [];
  public isLoading: boolean;
  public page: Page;
  public listAction: IActionTable[][] = [];
  public selectedColumns: IHeaderColumn[] = [];
  public headerColumns: IHeaderColumn[] = [];
  public filter: {
    keyword: string;
    event: number[] | undefined;
    source: number[] | undefined;
  } = {
    keyword: '',
    event: undefined,
    source: undefined,
  };
  public listEvent: IDropdown[] = [];

  public get listStatus() {
    return TransactionProcessing.listStatus;
  }

  public get listSource() {
    return TransactionProcessing.listSource;
  }

  public get listOnineOffline() {
    return TransactionProcessing.listOnineOffline;
  }

  public getStatusSeverity(code: any) {
    return TransactionProcessing.getStatus(code, ETypeStatus.SEVERITY);
  }

  public getStatusName(code: any) {
    return TransactionProcessing.getStatus(code, ETypeStatus.LABEL);
  }

  public get listReason() {
    return SaleTicketOrder.listReason;
  }

  ngOnInit(): void {
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
    this.selectedColumns = this.getLocalStorage(TransactionProcessing.keyStorage) ?? this.headerColumns;
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
          this.setLocalStorage(this.selectedColumns, TransactionProcessing.keyStorage);
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
    this.listAction = this.dataSource.map((data: TransactionProcessingModel, index: number) => {
      const actions: IActionTable[] = [];

      if(this.isGranted(this.PermissionEventConst.XuLyGiaoDich_ChiTiet)) {
        actions.push({
          data: data,
          label: 'Xem chi tiết lệnh',
          icon: 'pi pi-info-circle',
          command: ($event) => {
            this.detail($event.item.data);
          },
        });
      }
      
      if(this.isGranted(this.PermissionEventConst.XuLyGiaoDich_ChiTiet_ThongTinChung_CapNhat)) {
        actions.push({
          data: data,
          label: 'Chỉnh sửa lệnh',
          icon: 'pi pi-pencil',
          command: ($event) => {
            this.edit($event.item.data);
          },
        });
      }
     
      if(this.isGranted(this.PermissionEventConst.XuLyGiaoDich_PheDuyetMuaVe)) {
        actions.push({
          data: data,
          label: 'Phê duyệt mua vé',
          icon: 'pi pi-check',
          command: ($event) => {
            this.approve($event.item.data);
          },
        });
      }
      return actions;
    });
  }

  public changeFilter(event: any) {
    this.setPage();
  }

  public changePage(event: any) {
    if (event) {
      this.setPage();
    }
  }

  public detail(data: TransactionProcessingModel) {
    if (data) {
      this.routerService.routerNavigate([
        '/sale-ticket-management/sale-ticket-order/detail/' + this.cryptEncode(data.id),
      ]);
    }
  }

  public edit(data: TransactionProcessingModel) {
    if (data) {
      this.routerService.routerNavigate([
        '/sale-ticket-management/sale-ticket-order/edit/' + this.cryptEncode(data.id),
      ]);
    }
  }

  public approve(data: TransactionProcessingModel) {
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
          title: 'Phê duyệt mua vé?',
          icon: FormNotificationConst.IMAGE_APPROVE,
        } as INotiDataModal,
      });
      ref.onClose.subscribe((dataCallBack) => {
        if (dataCallBack?.accept) {
          this.saleTicketOrderService.approveSaleTicketOrder(data.id).subscribe(
            (response) => {
              if (this.handleResponseInterceptor(response, 'Phê duyệt mua vé thành công')) {
                this.setPage();
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

  public setPage() {
    this.isLoading = true;
    this.page.keyword = this.filter.keyword;

    this.saleTicketOrderService.findAllTransactionProcessing(this.page, this.filter).subscribe(
      (res) => {
        this.isLoading = false;
        if (this.handleResponseInterceptor(res, '')) {
          this.page.totalItems = res.data.totalItems;
          if (res.data?.items) {
            this.dataSource = res.data.items.map((item: any) => {
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
                status: item.status,
              } as TransactionProcessingModel;
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
