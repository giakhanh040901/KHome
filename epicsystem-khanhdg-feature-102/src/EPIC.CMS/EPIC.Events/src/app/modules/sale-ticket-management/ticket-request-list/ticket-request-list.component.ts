import { ChangeDetectorRef, Component, Injector } from '@angular/core';
import {
  EPositionFrozenCell,
  EPositionTextCell,
  ETypeDataTable,
  ETypeFormatDate,
  ETypeStatus,
  EventOverview,
  FormNotificationConst,
  SearchConst,
  StatusResponseConst,
  TicketRequestList,
} from '@shared/AppConsts';
import { FormNotificationComponent } from '@shared/components/form-notification/form-notification.component';
import { FormSetDisplayColumnComponent } from '@shared/components/form-set-display-column/form-set-display-column.component';
import { CrudComponentBase } from '@shared/crud-component-base';
import { formatCalendarItem, formatDate } from '@shared/function-common';
import { IActionTable, IHeaderColumn, INotiDataModal } from '@shared/interface/InterfaceConst.interface';
import { TicketRequestListModel } from '@shared/interface/sale-ticket-management/ticket-request-list/TicketRequestList.model';
import { Page } from '@shared/model/page';
import { RouterService } from '@shared/services/router.service';
import { TicketRequestListService } from '@shared/services/ticket-request-list.service';
import { MessageService } from 'primeng/api';
import { DialogService } from 'primeng/dynamicdialog';
import { debounceTime } from 'rxjs/operators';
import { BreadcrumbService } from 'src/app/layout/breadcrumb/breadcrumb.service';

@Component({
  selector: 'ticket-request-list',
  templateUrl: './ticket-request-list.component.html',
  styleUrls: ['./ticket-request-list.component.scss'],
})
export class TicketRequestListComponent extends CrudComponentBase {
  constructor(
    injector: Injector,
    messageService: MessageService,
    private dialogService: DialogService,
    private breadcrumbService: BreadcrumbService,
    private routerService: RouterService,
    private changeDetectorRef: ChangeDetectorRef,
    private ticketRequestListService: TicketRequestListService
  ) {
    super(injector, messageService);
    this.breadcrumbService.setItems([
      { label: 'Trang chủ', routerLink: ['/home'] },
      { label: 'Quản lý bán vé', routerLink: ['/home'] },
      { label: 'Yêu cầu vé cứng' },
    ]);
  }

  public dataSource: TicketRequestListModel[] = [];
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
    return TicketRequestList.listStatus;
  }

  public getStatusSeverity(code: any) {
    return TicketRequestList.getStatus(code, ETypeStatus.SEVERITY);
  }

  public getStatusName(code: any) {
    return TicketRequestList.getStatus(code, ETypeStatus.LABEL);
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
    this.selectedColumns = this.getLocalStorage(TicketRequestList.keyStorage) ?? this.headerColumns;
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
          this.setLocalStorage(this.selectedColumns, TicketRequestList.keyStorage);
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
    this.listAction = this.dataSource.map((data: TicketRequestListModel, index: number) => {
      const actions: IActionTable[] = [];

      if(this.isGranted(this.PermissionEventConst.YeuCauVeCung_ChiTiet)) {
        actions.push({
          data: data,
          label: 'Xem chi tiết lệnh',
          icon: 'pi pi-info-circle',
          command: ($event) => {
            this.detail($event.item.data);
          },
        });
      }
      
      if(this.isGranted(this.PermissionEventConst.YeuCauVeCung_XuatMauGiaoVe)) {
        actions.push({
          data: data,
          label: 'Xuất mẫu giao vé',
          icon: 'pi pi-download',
          command: ($event) => {
            this.exportDeliTicket($event.item.data);
          },
        });
      }

      if(this.isGranted(this.PermissionEventConst.YeuCauVeCung_XuatVeCung)) {
        actions.push({
          data: data,
          label: 'Xuất vé cứng',
          icon: 'pi pi-download',
          command: ($event) => {
            this.exportTicket($event.item.data);
          },
        });
      }
      
      if (data.status === TicketRequestList.CHO_XU_LY && this.isGranted(this.PermissionEventConst.YeuCauVeCung_DoiTrangThai_DangGiao)) {
        actions.push({
          data: data,
          label: 'Đang giao',
          icon: 'pi pi-pencil',
          command: ($event) => {
            this.delivery($event.item.data);
          },
        });
      }

      if (data.status === TicketRequestList.DANG_GIAO && this.isGranted(this.PermissionEventConst.YeuCauVeCung_DoiTrangThai_HoanThanh)) {
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

  public detail(data: TicketRequestListModel) {
    if (data) {
      this.routerService.routerNavigate([
        '/sale-ticket-management/sale-ticket-order/detail/' + this.cryptEncode(data.id),
      ]);
    }
  }

  public delivery(data: TicketRequestListModel) {
    if (data) {
      const ref = this.dialogService.open(FormNotificationComponent, {
        header: 'Giao vé',
        width: '600px',
        contentStyle: {
          'max-height': '600px',
          overflow: 'auto',
          'padding-bottom': '50px',
        },
        styleClass: 'p-dialog-custom',
        baseZIndex: 10000,
        data: {
          title: `Xác nhận giao vé`,
          icon: FormNotificationConst.IMAGE_APPROVE,
        } as INotiDataModal,
      });
      ref.onClose.subscribe((dataCallBack) => {
        if (dataCallBack?.accept) {
          this.ticketRequestListService.changeStatus(data.id, TicketRequestList.DANG_GIAO).subscribe(
            (response) => {
              if (this.handleResponseInterceptor(response, 'Vé đang được giao')) {
                this.setPage();
              }
            },
            (err) => {
              this.messageError(`Giao vé thất bại`);
            }
          );
        }
      });
    }
  }

  public done(data: TicketRequestListModel) {
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
          this.ticketRequestListService.changeStatus(data.id, TicketRequestList.HOAN_THANH).subscribe(
            (response) => {
              if (this.handleResponseInterceptor(response, 'Giao vé thành công')) {
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

  public exportDeliTicket(data: TicketRequestListModel) {
    if (data) {
      this.ticketRequestListService.exportDeliTicket(data.id).subscribe(
        (response) => {
          if (response?.status === StatusResponseConst.RESPONSE_TRUE) {
            this.messageSuccess('Xuất mẫu giao vé thành công');
          } else {
            this.messageError(response?.message);
          }
        },
        (err) => {
          this.messageError(`Xuất mẫu giao vé thất bại`);
        }
      );
    }
  }

  public exportTicket(data: TicketRequestListModel) {
    if (data) {
      if (data.ticketFiledUrls.length) {
        data.ticketFiledUrls.forEach((url: string) => {
          this.ticketRequestListService.exportFile(url).subscribe();
        });
      } else {
        this.messageError('Không tìm thấy mẫu vé cứng');
      }
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

    this.ticketRequestListService.findAll(this.page, filter).subscribe(
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
                  requestDate: item.pendingDate ? formatDate(item.pendingDate, ETypeFormatDate.DATE) : '',
                  actionDate: item.deliveryDate ? formatDate(item.deliveryDate, ETypeFormatDate.DATE) : '',
                  receiveDate: item.finishedDate ? formatDate(item.finishedDate, ETypeFormatDate.DATE) : '',
                  ticketFiledUrls: item.ticketFilledUrls || [],
                  status: item.deliveryStatus,
                } as TicketRequestListModel)
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
