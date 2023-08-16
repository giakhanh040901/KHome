import { ChangeDetectorRef, Component, Injector } from '@angular/core';
import {
  COMPARE_TYPE,
  EPositionFrozenCell,
  EPositionTextCell,
  ETypeDataTable,
  ETypeFormatDate,
  ETypeStatus,
  EventOverview,
  FormNotificationConst,
  SearchConst,
  YES_NO,
} from '@shared/AppConsts';
import { FormNotificationComponent } from '@shared/components/form-notification/form-notification.component';
import { FormSetDisplayColumnComponent } from '@shared/components/form-set-display-column/form-set-display-column.component';
import { CrudComponentBase } from '@shared/crud-component-base';
import { compareDate, formatCalendarItem, formatDate } from '@shared/function-common';
import {
  IActionTable,
  IConfigDataModal,
  IDropdown,
  IHeaderColumn,
  INotiDataModal,
  IValueFormatter,
} from '@shared/interface/InterfaceConst.interface';
import { EventOverviewModel } from '@shared/interface/event-management/event-overview/EventOverview.model';
import { Page } from '@shared/model/page';
import { EventOverviewService } from '@shared/services/event-overview.service';
import { RouterService } from '@shared/services/router.service';
import { MessageService } from 'primeng/api';
import { DialogService } from 'primeng/dynamicdialog';
import { debounceTime } from 'rxjs/operators';
import { BreadcrumbService } from 'src/app/layout/breadcrumb/breadcrumb.service';
import { CreateEventOverviewComponent } from './create-event-overview/create-event-overview.component';
import { StopOrCancelEventComponent } from './stop-or-cancel-event/stop-or-cancel-event.component';
import { SignalrService } from '@shared/services/signalr.service';

@Component({
  selector: 'event-overview',
  templateUrl: './event-overview.component.html',
  styleUrls: ['./event-overview.component.scss'],
})
export class EventOverviewComponent extends CrudComponentBase {
  constructor(
    injector: Injector,
    messageService: MessageService,
    private dialogService: DialogService,
    private breadcrumbService: BreadcrumbService,
    private changeDetectorRef: ChangeDetectorRef,
    private eventOverviewService: EventOverviewService,
    private routerService: RouterService,
    private _signalrService: SignalrService
  ) {
    super(injector, messageService);
    this.breadcrumbService.setItems([
      { label: 'Trang chủ', routerLink: ['/home'] },
      { label: 'Quản lý sự kiện', routerLink: ['/home'] },
      { label: 'Tổng quan sự kiện' },
    ]);
  }

  public dataSource: EventOverviewModel[] = [];
  public isLoading: boolean;
  public page: Page;
  public listAction: IActionTable[][] = [];
  public selectedColumns: IHeaderColumn[] = [];
  public headerColumns: IHeaderColumn[] = [];
  public filter: {
    keyword: string;
    type: number[] | undefined;
    status: number[] | undefined;
    startDate: any | undefined;
  } = {
    keyword: '',
    type: undefined,
    status: undefined,
    startDate: '',
  };

  public get listTypeEvent() {
    return EventOverview.listTypeEvent;
  }

  public get listStatus() {
    return EventOverview.listStatus;
  }

  public getStatusSeverity(code: any) {
    return EventOverview.getStatus(code, ETypeStatus.SEVERITY);
  }

  public getStatusName(code: any) {
    return EventOverview.getStatus(code, ETypeStatus.LABEL);
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
        field: 'name',
        header: 'Tên sự kiện',
        minWidth: '30rem',
        maxWidth: '50vw',
        isPin: true,
        type: ETypeDataTable.TEXT,
      },
      {
        field: 'organize',
        header: 'Ban tổ chức',
        minWidth: '20rem',
        maxWidth: '50vw',
        isPin: true,
        type: ETypeDataTable.TEXT,
      },
      {
        field: 'type',
        header: 'Loại hình sự kiện',
        width: '25rem',
        isPin: true,
        type: ETypeDataTable.TEXT,
        valueFormatter: (param: IValueFormatter) => {
          if (param.data && param.data.length) {
            return param.data
              .map((d: number) => this.listTypeEvent.find((e: IDropdown) => e.value === d)?.label || '')
              .join(', ');
          }
          return '';
        },
      },
      {
        field: 'startDate',
        header: 'Ngày bắt đầu',
        width: '12rem',
        isPin: true,
        type: ETypeDataTable.TEXT,
      },
      {
        field: 'endDate',
        header: 'Ngày kết thúc',
        width: '12rem',
        isPin: true,
        type: ETypeDataTable.TEXT,
      },
      {
        field: 'ticket',
        header: 'Số vé',
        width: '8rem',
        isPin: true,
        type: ETypeDataTable.TEXT,
      },
      {
        field: 'register',
        header: 'Đăng ký',
        width: '8rem',
        isPin: true,
        type: ETypeDataTable.TEXT,
      },
      {
        field: 'valid',
        header: 'Hợp lệ',
        width: '8rem',
        isPin: true,
        type: ETypeDataTable.TEXT,
      },
      {
        field: 'rest',
        header: 'Còn lại',
        width: '8rem',
        isPin: true,
        type: ETypeDataTable.TEXT,
      },
      {
        field: 'join',
        header: 'Tham gia',
        width: '8rem',
        isPin: true,
        type: ETypeDataTable.TEXT,
      },
      {
        field: 'settingDate',
        header: 'Ngày cài đặt',
        width: '12rem',
        type: ETypeDataTable.TEXT,
      },
      {
        field: 'settingUser',
        header: 'Người cài đặt',
        width: '10rem',
        type: ETypeDataTable.TEXT,
      },
      {
        field: 'status',
        header: 'Trạng thái',
        width: '8rem',
        type: ETypeDataTable.STATUS,
        funcStyleClassStatus: (status: any) => {
          return this.getStatusSeverity.bind(this)(status);
        },
        funcLabelStatus: (status: any) => {
          return this.getStatusName.bind(this)(status);
        },
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
    this.selectedColumns = this.getLocalStorage(EventOverview.keyStorage) ?? this.headerColumns;
    this.setPage();
    this.startSignalR();
  }

  private startSignalR() {
    this._signalrService
      .startConnection()
      .then(() => {
        this._signalrService.listen('UpdateEventTicket', (data) => {
          const index = this.dataSource.findIndex((e) => e.id === data?.id);
          if (index !== -1) {
            this.dataSource[index].join = data?.participateQuantity;
            this.dataSource[index].register = data?.registerQuantity;
            this.dataSource[index].ticket = data?.ticketQuantity;
            this.dataSource[index].valid = data?.validQuantity;
            this.dataSource[index].rest = data?.remainingTickets;
          }
        });
      })
      .catch((error) => {
        console.log('Error while connecting to SignalR:', error);
      });
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
          this.setLocalStorage(this.selectedColumns, EventOverview.keyStorage);
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
    this.listAction = this.dataSource.map((data: EventOverviewModel, index: number) => {
      const actions: IActionTable[] = [];

      if (this.isGranted([this.PermissionEventConst.TongQuanSuKien_ChiTiet])) {
        actions.push({
          data: data,
          label: 'Thông tin chi tiết',
          icon: 'pi pi-info-circle',
          command: ($event) => {
            this.detail($event.item.data);
          },
        });
      }

      // trạng thái !== KET_THUC && !== HUY_SU_KIEN
      if (data.status !== EventOverview.KET_THUC && data.status !== EventOverview.HUY_SU_KIEN) {
        if (this.isGranted([this.PermissionEventConst.TongQuanSuKien_ChiTiet_ThongTinChung_CapNhat])) {
          actions.push({
            data: data,
            label: 'Chỉnh sửa',
            icon: 'pi pi-pencil',
            command: ($event) => {
              this.edit($event.item.data);
            },
          });
        }
        // trạng thái === TAM_DUNG
        if (
          data.status === EventOverview.TAM_DUNG &&
          this.isGranted([this.PermissionEventConst.TongQuanSuKien_MoBanVe])
        ) {
          actions.push({
            data: data,
            label: 'Mở bán',
            icon: 'pi pi-pencil',
            command: ($event) => {
              this.openSell($event.item.data);
            },
          });
        }
        // trạng thái: DANG_MO_BAN
        if (data.status === EventOverview.DANG_MO_BAN) {
          if (this.isGranted([this.PermissionEventConst.TongQuanSuKien_BatTatShowApp])) {
            actions.push({
              data: data,
              label: !!data.isShowApp ? 'Tắt ShowApp' : 'Bật ShowApp',
              icon: !!data.isShowApp ? 'pi pi-eye-slash' : 'pi pi-eye',
              command: ($event) => {
                this.showApp($event.item.data, !!data.isShowApp);
              },
            });
          }
          //
          if (this.isGranted([this.PermissionEventConst.TongQuanSuKien_TamDung_HuySuKien])) {
            actions.push({
              data: data,
              label: 'Tạm dừng',
              icon: 'pi pi-pencil',
              command: ($event) => {
                this.stop($event.item.data);
              },
            });
          }
        }
        // trạng thái !== HUY_SU_KIEN && !== KET_THUC
        if (this.isGranted([this.PermissionEventConst.TongQuanSuKien_TamDung_HuySuKien])) {
          if (data.status !== EventOverview.HUY_SU_KIEN && data.status !== EventOverview.KET_THUC) {
            actions.push({
              data: data,
              label: 'Hủy sự kiện',
              icon: 'pi pi-times',
              command: ($event) => {
                this.cancel($event.item.data);
              },
            });
          }
        }
      }
      //
      return actions;
    });
  }

  public detail(data: EventOverviewModel) {
    if (data) {
      this.routerService.routerNavigate(['/event-management/event-overview/detail/' + this.cryptEncode(data.id)]);
    }
  }

  public edit(data: EventOverviewModel) {
    if (data) {
      this.routerService.routerNavigate(['/event-management/event-overview/edit/' + this.cryptEncode(data.id)]);
    }
  }

  public openSell(data: EventOverviewModel) {
    if (data) {
      const ref = this.dialogService.open(FormNotificationComponent, {
        header: 'Mở bán vé tham gia sự kiện',
        width: '600px',
        contentStyle: {
          'max-height': '600px',
          overflow: 'auto',
          'padding-bottom': '50px',
        },
        styleClass: 'p-dialog-custom',
        baseZIndex: 10000,
        data: {
          title: `Xác nhận mở bán vé tham gia sự kiện`,
          icon: FormNotificationConst.IMAGE_APPROVE,
        } as INotiDataModal,
      });
      ref.onClose.subscribe((dataCallBack) => {
        if (dataCallBack?.accept) {
          this.eventOverviewService.openSellEvent(data.id).subscribe(
            (response) => {
              if (this.handleResponseInterceptor(response, 'Mở bán vé tham gia sự kiện thành công')) {
                this.setPage();
              }
            },
            (err) => {
              this.messageError(`Mở bán vé tham gia sự kiện thất bại`);
            }
          );
        }
      });
    }
  }

  public showApp(data: EventOverviewModel, isShowApp: boolean) {
    if (data) {
      const ref = this.dialogService.open(FormNotificationComponent, {
        header: 'Show App',
        width: '600px',
        contentStyle: {
          'max-height': '600px',
          overflow: 'auto',
          'padding-bottom': '50px',
        },
        styleClass: 'p-dialog-custom',
        baseZIndex: 10000,
        data: {
          title: `Xác nhận ${isShowApp ? 'Tắt ShowApp' : 'Bật ShowApp'}?`,
          icon: isShowApp ? FormNotificationConst.IMAGE_CLOSE : FormNotificationConst.IMAGE_APPROVE,
        } as INotiDataModal,
      });
      ref.onClose.subscribe((dataCallBack) => {
        if (dataCallBack?.accept) {
          this.eventOverviewService.changeShowAppEvent(data.id).subscribe(
            (response) => {
              if (this.handleResponseInterceptor(response, 'Thành công')) {
                this.setPage();
              }
            },
            (err) => {
              this.messageError(`Thất bại`);
            }
          );
        }
      });
    }
  }

  public stop(data: EventOverviewModel) {
    if (data) {
      const ref = this.dialogService.open(StopOrCancelEventComponent, {
        header: 'Tạm dừng sự kiện',
        width: '600px',
        baseZIndex: 10000,
        data: {
          dataSource: data,
          type: 'pause',
        } as IConfigDataModal,
      });
      ref.onClose.subscribe((response) => {
        if (this.handleResponseInterceptor(response, '')) {
          this.messageService.add({
            severity: 'success',
            summary: '',
            detail: 'Tạm dừng sự kiện thành công',
            life: 1500,
          });
          this.setPage();
        }
      });
    }
  }

  public cancel(data: EventOverviewModel) {
    if (data) {
      const ref = this.dialogService.open(StopOrCancelEventComponent, {
        header: 'Hủy sự kiện',
        width: '600px',
        baseZIndex: 10000,
        data: {
          dataSource: data,
          type: 'cancel',
        } as IConfigDataModal,
      });
      ref.onClose.subscribe((response) => {
        if (this.handleResponseInterceptor(response, '')) {
          this.messageService.add({
            severity: 'success',
            summary: '',
            detail: 'Hủy sự kiện thành công',
            life: 1500,
          });
          this.setPage();
        }
      });
    }
  }

  public create(event: any) {
    if (event) {
      const ref = this.dialogService.open(CreateEventOverviewComponent, {
        header: 'Thêm mới sự kiện',
        width: '600px',
        baseZIndex: 10000,
      });
      ref.onClose.subscribe((response) => {
        if (this.handleResponseInterceptor(response, '')) {
          this.messageService.add({
            severity: 'success',
            summary: '',
            detail: 'Thêm mới thành công',
            life: 1500,
          });
          if (response.data.id) {
            this.edit({ id: response.data.id } as EventOverviewModel);
          } else {
            this.setPage();
          }
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
    filter.startDate && (filter.startDate = formatCalendarItem(filter.startDate));

    this.eventOverviewService.findAll(this.page, filter).subscribe(
      (res) => {
        this.isLoading = false;
        if (this.handleResponseInterceptor(res, '')) {
          this.page.totalItems = res.data.totalItems;
          if (res.data?.items) {
            this.dataSource = res.data.items.map(
              (item: any) =>
                ({
                  id: item.id,
                  name: item.name,
                  organize: item.organizator,
                  type: item.eventTypes,
                  startDate: item.startDate ? formatDate(item.startDate, ETypeFormatDate.DATE_TIME) : '',
                  endDate: item.endDate ? formatDate(item.endDate, ETypeFormatDate.DATE_TIME) : '',
                  ticket: item.ticketQuantity,
                  register: item.registerQuantity,
                  valid: item.validQuantity,
                  rest: item.remainingTickets,
                  join: item.participateQuantity,
                  isShowApp: item.isShowApp === YES_NO.YES,
                  settingUser: item.createdBy,
                  settingDate: item.createdDate ? formatDate(item.createdDate, ETypeFormatDate.DATE) : '',
                  status: item.endDate
                    ? compareDate(item.endDate, new Date(), COMPARE_TYPE.LESS_EQUAL)
                      ? EventOverview.KET_THUC
                      : item.status
                    : item.status,
                } as EventOverviewModel)
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
