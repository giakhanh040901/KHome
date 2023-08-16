import { ChangeDetectorRef, Component, Injector } from '@angular/core';
import {
  BillRequestList,
  EPositionFrozenCell,
  EPositionTextCell,
  ETypeDataTable,
  ETypeFormatDate,
  ETypeStatus,
  EventOverview,
  FormNotificationConst,
  SearchConst,
} from '@shared/AppConsts';
import { FormNotificationComponent } from '@shared/components/form-notification/form-notification.component';
import { FormSetDisplayColumnComponent } from '@shared/components/form-set-display-column/form-set-display-column.component';
import { CrudComponentBase } from '@shared/crud-component-base';
import { formatCalendarItem, formatDate } from '@shared/function-common';
import { IActionTable, IHeaderColumn, INotiDataModal } from '@shared/interface/InterfaceConst.interface';
import { BillRequestListModel } from '@shared/interface/sale-ticket-management/bill-request-list/BillRequestList.model';
import { Page } from '@shared/model/page';
import { BillRequestListService } from '@shared/services/bill-request-list.service';
import { RouterService } from '@shared/services/router.service';
import { MessageService } from 'primeng/api';
import { DialogService } from 'primeng/dynamicdialog';
import { debounceTime } from 'rxjs/operators';
import { BreadcrumbService } from 'src/app/layout/breadcrumb/breadcrumb.service';

@Component({
  selector: 'bill-request-list',
  templateUrl: './bill-request-list.component.html',
  styleUrls: ['./bill-request-list.component.scss'],
})
export class BillRequestListComponent extends CrudComponentBase {
  constructor(
    injector: Injector,
    messageService: MessageService,
    private dialogService: DialogService,
    private breadcrumbService: BreadcrumbService,
    private routerService: RouterService,
    private changeDetectorRef: ChangeDetectorRef,
    private billRequestListService: BillRequestListService
  ) {
    super(injector, messageService);
    this.breadcrumbService.setItems([
      { label: 'Trang chủ', routerLink: ['/home'] },
      { label: 'Quản lý bán vé', routerLink: ['/home'] },
      { label: 'Yêu cầu hóa đơn' },
    ]);
  }

  public dataSource: BillRequestListModel[] = [];
  public isLoading: boolean;
  public page: Page;
  public listAction: IActionTable[][] = [];
  public selectedColumns: IHeaderColumn[] = [];
  public headerColumns: IHeaderColumn[] = [];
  public filter: {
    keyword: string;
    eventType: number[] | undefined;
    status: number[] | undefined;
    date: any | undefined;
  } = {
    keyword: '',
    eventType: undefined,
    status: undefined,
    date: undefined,
  };

  public get listEventType() {
    return EventOverview.listTypeEvent;
  }
  public get listStatus() {
    return BillRequestList.listStatus;
  }

  public getStatusSeverity(code: any) {
    return BillRequestList.getStatus(code, ETypeStatus.SEVERITY);
  }

  public getStatusName(code: any) {
    return BillRequestList.getStatus(code, ETypeStatus.LABEL);
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
        field: 'customerPhone',
        header: 'SĐT khách hàng',
        width: '15rem',
        isPin: true,
        type: ETypeDataTable.TEXT,
      },
      {
        field: 'eventName',
        header: 'Sự kiện',
        minWidth: '30rem',
        maxWidth: '90vw',
        isPin: true,
        type: ETypeDataTable.TEXT,
      },
      {
        field: 'ticketQuantity',
        header: 'Số lượng vé',
        width: '12rem',
        isPin: true,
        type: ETypeDataTable.TEXT,
      },
      {
        field: 'requestDate',
        header: 'Ngày yêu cầu',
        width: '12rem',
        isPin: true,
        type: ETypeDataTable.TEXT,
      },
      {
        field: 'actionDate',
        header: 'Ngày xử lý',
        width: '12rem',
        isPin: true,
        type: ETypeDataTable.TEXT,
      },
      {
        field: 'receiveDate',
        header: 'Ngày khách nhận',
        width: '12rem',
        isPin: true,
        type: ETypeDataTable.TEXT,
      },
      {
        field: 'actionUser',
        header: 'Người xử lý',
        width: '12rem',
        type: ETypeDataTable.TEXT,
      },
      {
        field: 'deliveryUser',
        header: 'Người giao',
        width: '12rem',
        type: ETypeDataTable.TEXT,
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
    this.selectedColumns = this.getLocalStorage(BillRequestList.keyStorage) ?? this.headerColumns;
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
          this.setLocalStorage(this.selectedColumns, BillRequestList.keyStorage);
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
    this.listAction = this.dataSource.map((data: BillRequestListModel, index: number) => {
      const actions: IActionTable[] = [];

      if(this.isGranted(this.PermissionEventConst.YeuCauHoaDon_ChiTiet)) {
        actions.push({
          data: data,
          label: 'Xem chi tiết lệnh',
          icon: 'pi pi-info-circle',
          command: ($event) => {
            this.detail($event.item.data);
          },
        });
      }

      if (data.status === BillRequestList.CHO_XU_LY && this.PermissionEventConst.YeuCauHoaDon_DoiTrangThai_DangGiao) {
        actions.push({
          data: data,
          label: 'Đang giao',
          icon: 'pi pi-pencil',
          command: ($event) => {
            this.delivery($event.item.data);
          },
        });
      }

      if (data.status === BillRequestList.DANG_GIAO  && this.PermissionEventConst.YeuCauHoaDon_DoiTrangThai_HoanThanh) {
        actions.push({
          data: data,
          label: 'Hoàn thành',
          icon: 'pi pi-pencil',
          command: ($event) => {
            this.done($event.item.data);
          },
        });
      }

      return actions;
    });
  }

  public detail(data: BillRequestListModel) {
    if (data) {
      this.routerService.routerNavigate([
        '/sale-ticket-management/sale-ticket-order/detail/' + this.cryptEncode(data.id),
      ]);
    }
  }

  public delivery(data: BillRequestListModel) {
    if (data) {
      const ref = this.dialogService.open(FormNotificationComponent, {
        header: 'Giao hóa đơn',
        width: '600px',
        contentStyle: {
          'max-height': '600px',
          overflow: 'auto',
          'padding-bottom': '50px',
        },
        styleClass: 'p-dialog-custom',
        baseZIndex: 10000,
        data: {
          title: `Xác nhận giao hóa đơn`,
          icon: FormNotificationConst.IMAGE_APPROVE,
        } as INotiDataModal,
      });
      ref.onClose.subscribe((dataCallBack) => {
        if (dataCallBack?.accept) {
          this.billRequestListService.changeStatus(data.id, BillRequestList.DANG_GIAO).subscribe(
            (response) => {
              if (this.handleResponseInterceptor(response, 'Hóa đơn đang được giao')) {
                this.setPage();
              }
            },
            (err) => {
              this.messageError(`Giao hóa đơn thất bại`);
            }
          );
        }
      });
    }
  }

  public done(data: BillRequestListModel) {
    if (data) {
      const ref = this.dialogService.open(FormNotificationComponent, {
        header: 'Hoàn thành',
        width: '600px',
        contentStyle: {
          'max-height': '600px',
          overflow: 'auto',
          'padding-bottom': '50px',
        },
        styleClass: 'p-dialog-custom',
        baseZIndex: 10000,
        data: {
          title: `Xác nhận hoàn thành`,
          icon: FormNotificationConst.IMAGE_APPROVE,
        } as INotiDataModal,
      });
      ref.onClose.subscribe((dataCallBack) => {
        if (dataCallBack?.accept) {
          this.billRequestListService.changeStatus(data.id, BillRequestList.HOAN_THANH).subscribe(
            (response) => {
              if (this.handleResponseInterceptor(response, 'Giao hóa đơn thành công')) {
                this.setPage();
              }
            },
            (err) => {
              this.messageError(`Hoàn thành thất bại`);
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
    let filter = { ...this.filter };
    filter.date && (filter.date = formatCalendarItem(filter.date));

    this.billRequestListService.findAll(this.page, filter).subscribe(
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
                  customerPhone: item.phone,
                  eventName: item.eventName,
                  ticketQuantity: item.quantity,
                  requestDate: item.pendingInvoiceDate ? formatDate(item.pendingInvoiceDate, ETypeFormatDate.DATE) : '',
                  actionDate: item.deliveryInvoiceDate
                    ? formatDate(item.deliveryInvoiceDate, ETypeFormatDate.DATE)
                    : '',
                  receiveDate: item.finishedInvoiceDate
                    ? formatDate(item.finishedInvoiceDate, ETypeFormatDate.DATE)
                    : '',
                  status: item.deliveryInvoiceStatus,
                } as BillRequestListModel)
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
