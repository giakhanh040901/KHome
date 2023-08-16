import { ChangeDetectorRef, Component, Injector } from '@angular/core';
import {
  EPositionFrozenCell,
  EPositionTextCell,
  ETypeDataTable,
  ETypeFormatDate,
  ETypeStatus,
  FormNotificationConst,
  ParticipationManagement,
  SaleTicketOrder,
  SearchConst,
} from '@shared/AppConsts';
import { FormNotificationComponent } from '@shared/components/form-notification/form-notification.component';
import { FormSetDisplayColumnComponent } from '@shared/components/form-set-display-column/form-set-display-column.component';
import { CrudComponentBase } from '@shared/crud-component-base';
import { formatDate } from '@shared/function-common';
import {
  IActionTable,
  IConfigDataModal,
  IDropdown,
  IHeaderColumn,
  INotiDataModal,
  IValueFormatter,
} from '@shared/interface/InterfaceConst.interface';
import { ParticipationManagementModel } from '@shared/interface/sale-ticket-management/participation-management/ParticipationManagement.model';
import { HoldOrLockTicketCustomerModel } from '@shared/interface/sale-ticket-management/sale-ticket-order/HoldOrLockTicketCustomer.model';
import { Page } from '@shared/model/page';
import { RouterService } from '@shared/services/router.service';
import { SaleTicketOrderService } from '@shared/services/sale-ticket-order.service';
import { MessageService } from 'primeng/api';
import { DialogService } from 'primeng/dynamicdialog';
import { debounceTime } from 'rxjs/operators';
import { BreadcrumbService } from 'src/app/layout/breadcrumb/breadcrumb.service';
import { HoldOrLockTicketCustomerComponent } from '../../../../shared/components/hold-or-lock-ticket-customer/hold-or-lock-ticket-customer.component';
import { SignalrService } from '@shared/services/signalr.service';

@Component({
  selector: 'participation-management',
  templateUrl: './participation-management.component.html',
  styleUrls: ['./participation-management.component.scss'],
})
export class ParticipationManagementComponent extends CrudComponentBase {
  constructor(
    injector: Injector,
    messageService: MessageService,
    private dialogService: DialogService,
    private breadcrumbService: BreadcrumbService,
    private routerService: RouterService,
    private changeDetectorRef: ChangeDetectorRef,
    private saleTicketOrderService: SaleTicketOrderService,
    private _signalrService: SignalrService
  ) {
    super(injector, messageService);
    this.breadcrumbService.setItems([
      { label: 'Trang chủ', routerLink: ['/home'] },
      { label: 'Quản lý bán vé', routerLink: ['/home'] },
      { label: 'Quản lý tham gia' },
    ]);
  }

  public dataSource: ParticipationManagementModel[] = [];
  public isLoading: boolean;
  public page: Page;
  public listAction: IActionTable[][] = [];
  public selectedColumns: IHeaderColumn[] = [];
  public headerColumns: IHeaderColumn[] = [];
  public filter: {
    keyword: string;
    event: number[] | undefined;
    status: number[] | undefined;
    checkinType: number | undefined;
  } = {
    keyword: '',
    event: undefined,
    status: undefined,
    checkinType: undefined,
  };
  public listEvent: IDropdown[] = [];

  public get listStatus() {
    return ParticipationManagement.listStatus;
  }
  public get listCheckinType() {
    return ParticipationManagement.listCheckinType;
  }

  public getStatusSeverity(code: any) {
    return ParticipationManagement.getStatus(code, ETypeStatus.SEVERITY);
  }

  public getStatusName(code: any) {
    return ParticipationManagement.getStatus(code, ETypeStatus.LABEL);
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
        field: 'customerPhone',
        header: 'SĐT khách hàng',
        width: '15rem',
        isPin: true,
        type: ETypeDataTable.TEXT,
      },
      {
        field: 'ticketCode',
        header: 'Mã vé',
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
        field: 'ticketCheckin',
        header: 'Checkin trên vé',
        width: '12rem',
        isPin: true,
        type: ETypeDataTable.TEXT,
      },
      {
        field: 'realCheckin',
        header: 'Checkin thực tế',
        width: '12rem',
        isPin: true,
        type: ETypeDataTable.TEXT,
      },
      {
        field: 'checkinType',
        header: 'Hình thức checkin',
        width: '12rem',
        isPin: true,
        type: ETypeDataTable.TEXT,
        valueFormatter: (param: IValueFormatter) =>
          param.data ? this.listCheckinType.find((e: IDropdown) => e.value === param.data)?.label : '',
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
    this.selectedColumns = this.getLocalStorage(ParticipationManagement.keyStorage) ?? this.headerColumns;
    this.setPage();
    this.startSignalR();
  }

  private startSignalR() {
    this._signalrService
      .startConnection()
      .then(() => {
        this._signalrService.listen('UpdateOrderTickeDetail', (data) => {
          const index = this.dataSource.findIndex((e) => e.id === data?.id);
          if (index !== -1) {
            this.dataSource[index].realCheckin = data.checkIn;
            this.dataSource[index].checkinType = data.checkInType;
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
          this.setLocalStorage(this.selectedColumns, ParticipationManagement.keyStorage);
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
    this.listAction = this.dataSource.map((data: ParticipationManagementModel, index: number) => {
      const actions: IActionTable[] = [];

      if (this.isGranted(this.PermissionEventConst.QuanLyThamGia_XemVe)) {
        actions.push({
          data: data,
          label: 'Xem vé',
          icon: 'pi pi-info-circle',
          command: ($event) => {
            this.detail($event.item.data);
          },
        });
      }

      // trạng thái !== DA_THAM_GIA
      if (data.status !== ParticipationManagement.DA_THAM_GIA) {
        if (this.isGranted(this.PermissionEventConst.QuanLyThamGia_TaiVe)) {
          actions.push({
            data: data,
            label: 'Tải về',
            icon: 'pi pi-pencil',
            command: ($event) => {
              this.download($event.item.data);
            },
          });
        }

        // trạng thái === CHUA_THAM_GIA
        if (data.status === ParticipationManagement.CHUA_THAM_GIA) {
          if (this.isGranted(this.PermissionEventConst.QuanLyThamGia_XacNhanThamGia)) {
            actions.push({
              data: data,
              label: 'Xác nhận tham gia',
              icon: 'pi pi-pencil',
              command: ($event) => {
                this.participate($event.item.data);
              },
            });
          }

          if (this.isGranted(this.PermissionEventConst.QuanLyThamGia_MoKhoaVe)) {
            actions.push({
              data: data,
              label: 'Khóa vé',
              icon: 'pi pi-pencil',
              command: ($event) => {
                this.lock($event.item.data);
              },
            });
          }
        }
        // trạng thái === TAM_KHOA
        if (
          data.status === ParticipationManagement.TAM_KHOA &&
          this.isGranted(this.PermissionEventConst.QuanLyThamGia_MoKhoaVe)
        ) {
          actions.push({
            data: data,
            label: 'Mở khóa vé',
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

  public detail(data: ParticipationManagementModel) {
    if (data) {
      if (data.ticketFiledUrl && data.ticketFiledUrl.length) {
        this.saleTicketOrderService.viewFilePDF('/' + data.ticketFiledUrl).subscribe();
      } else {
        this.messageError('Không tìm thấy vé');
      }
    }
  }

  public download(data: ParticipationManagementModel) {
    if (data) {
      if (data.ticketFiledUrl && data.ticketFiledUrl.length) {
        this.saleTicketOrderService.exportFile(data.ticketFiledUrl).subscribe();
      } else {
        this.messageError('Không tìm thấy vé');
      }
    }
  }

  public participate(data: ParticipationManagementModel) {
    if (data) {
      const ref = this.dialogService.open(FormNotificationComponent, {
        header: 'Xác nhận tham gia sự kiện',
        width: '600px',
        contentStyle: {
          'max-height': '600px',
          overflow: 'auto',
          'padding-bottom': '50px',
        },
        styleClass: 'p-dialog-custom',
        baseZIndex: 10000,
        data: {
          title: `Xác nhận khách hàng sử dụng vé tham gia sự kiện`,
          icon: FormNotificationConst.IMAGE_APPROVE,
        } as INotiDataModal,
      });
      ref.onClose.subscribe((dataCallBack) => {
        if (dataCallBack?.accept) {
          this.saleTicketOrderService.participateParticipationManagement(data.id).subscribe(
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

  public lock(data: ParticipationManagementModel) {
    if (data) {
      let dataSource = new HoldOrLockTicketCustomerModel();
      dataSource.id = data.id;
      dataSource.labelReason = 'Chọn lý do khóa';
      dataSource.listReason = this.listReason;
      dataSource.isShowTime = false;
      dataSource.createUser = this.saleTicketOrderService.currentUser;
      dataSource.service = this.saleTicketOrderService.lockTicket.bind(this.saleTicketOrderService);
      dataSource.mapToObject = (dto: HoldOrLockTicketCustomerModel) => {
        return {
          orderTickId: dto.id,
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

  public unlock(data: ParticipationManagementModel) {
    if (data) {
      const ref = this.dialogService.open(FormNotificationComponent, {
        header: 'Xác nhận mở khóa danh sách vé của khách hàng',
        width: '600px',
        contentStyle: {
          'max-height': '600px',
          overflow: 'auto',
          'padding-bottom': '50px',
        },
        styleClass: 'p-dialog-custom',
        baseZIndex: 10000,
        data: {
          title: `Xác nhận mở khóa danh sách vé của khách hàng`,
          icon: FormNotificationConst.IMAGE_APPROVE,
        } as INotiDataModal,
      });
      ref.onClose.subscribe((dataCallBack) => {
        if (dataCallBack?.accept) {
          this.saleTicketOrderService.unlockTicket(data.id).subscribe(
            (response) => {
              if (this.handleResponseInterceptor(response, 'Mở khóa vé thành công')) {
                this.setPage();
              }
            },
            (err) => {
              this.messageError(`'Mở khóa vé thất bại`);
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
    this.page.keyword = this.filter.keyword;

    this.saleTicketOrderService.findAllParticipationManagement(this.page, this.filter).subscribe(
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
                  ticketCode: item.ticketCode,
                  event: item.eventName,
                  ticketCheckin: item.checkIn ? formatDate(item.checkIn, ETypeFormatDate.DATE_TIME) : '',
                  realCheckin: item.checkInReal ? formatDate(item.checkInReal, ETypeFormatDate.DATE_TIME) : '',
                  checkinType: item.checkInType,
                  ticketFiledUrl: item.ticketFilledUrl || '',
                  status: item.status,
                } as ParticipationManagementModel)
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
