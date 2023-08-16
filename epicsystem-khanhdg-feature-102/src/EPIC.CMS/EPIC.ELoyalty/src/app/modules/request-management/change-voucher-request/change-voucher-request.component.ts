import { ChangeDetectorRef, Component, Injector } from '@angular/core';
import { Router } from '@angular/router';
import {
  ChangeVoucherRequest,
  EConfigDataModal,
  EPositionFrozenCell,
  EPositionTextCell,
  ETypeDataTable,
  ETypeFormatDate,
  FormNotificationConst,
  SearchConst,
} from '@shared/AppConsts';
import { FormNotificationDescriptionComponent } from '@shared/components/form-notification-description/form-notification-description.component';
import { CrudComponentBase } from '@shared/crud-component-base';
import { formatCalendarItem, formatDate } from '@shared/function-common';
import {
  IActionTable,
  IConfigDataModal,
  IHeaderColumn,
  INotiDataModal,
} from '@shared/interface/InterfaceConst.interface';
import { ChangeVoucherRequestModel } from '@shared/interface/request-management/change-voucher-request/ChangeVoucherRequest.model';
import { Page } from '@shared/model/page';
import { ChangeVoucherRequestService } from '@shared/services/change-voucher-request-service';
import { MessageService } from 'primeng/api';
import { DialogService } from 'primeng/dynamicdialog';
import { debounceTime } from 'rxjs/operators';
import { FormSetDisplayColumnComponent } from 'src/app/form-general/form-set-display-column/form-set-display-column/form-set-display-column.component';
import { BreadcrumbService } from 'src/app/layout/breadcrumb/breadcrumb.service';
import { CrudChangeVoucherRequestComponent } from './crud-change-voucher-request/crud-change-voucher-request.component';
import { HelpersService } from '@shared/services/helpers.service';
import { IconConfirm } from '@shared/consts/base.const';

@Component({
  selector: 'change-voucher-request',
  templateUrl: './change-voucher-request.component.html',
  styleUrls: ['./change-voucher-request.component.scss'],
})
export class ChangeVoucherRequestComponent extends CrudComponentBase {
  constructor(
    injector: Injector,
    messageService: MessageService,
    private dialogService: DialogService,
    private breadcrumbService: BreadcrumbService,
    private router: Router,
    private changeDetectorRef: ChangeDetectorRef,
    private changeVoucherRequestService: ChangeVoucherRequestService,
    private _helpersService: HelpersService,
  ) {
    super(injector, messageService);
    this.breadcrumbService.setItems([
      { label: 'Trang chủ', routerLink: ['/home'] },
      { label: 'Quản lý yêu cầu', routerLink: ['/home'] },
      { label: 'Yêu cầu đổi voucher' },
    ]);
  }

  public dataSource: ChangeVoucherRequestModel[] = [];
  public isLoading: boolean;
  public page: Page;
  public listAction: IActionTable[][] = [];
  public selectedColumns: IHeaderColumn[] = [];
  public headerColumns: IHeaderColumn[] = [];
  public filter: {
    keyword: string;
    status: number | undefined;
    date: any | undefined;
  } = {
    keyword: '',
    status: undefined,
    date: '',
  };

  public get listStatus() {
    return ChangeVoucherRequest.listStatus;
  }

  public getStatusSeverity(code: any) {
    return ChangeVoucherRequest.getStatus(code, 'severity');
  }

  public getStatusName(code: any) {
    return ChangeVoucherRequest.getStatus(code, 'label');
  }

  ngOnInit() {
    this.changeVoucherRequestService.getListVoucherRequest();
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
        field: 'customer',
        header: 'Khách hàng',
        width: '20rem',
        isPin: true,
        type: ETypeDataTable.TEXT,
      },
      {
        field: 'numberPhone',
        header: 'Số điện thoại',
        width: '10rem',
        isPin: true,
        type: ETypeDataTable.TEXT,
      },
      {
        field: 'currentPoint',
        header: 'Điểm hiện tại',
        width: '10rem',
        isPin: true,
        type: ETypeDataTable.TEXT,
      },
      {
        field: 'changePoint',
        header: 'Điểm đổi',
        width: '10rem',
        isPin: true,
        type: ETypeDataTable.TEXT,
      },
      {
        field: 'requestDate',
        header: 'Yêu cầu',
        width: '10rem',
        type: ETypeDataTable.TEXT,
      },
      {
        field: 'receiveDate',
        header: 'Tiếp nhận',
        width: '10rem',
        type: ETypeDataTable.TEXT,
      },
      {
        field: 'handOverDate',
        header: 'Bàn giao',
        width: '10rem',
        type: ETypeDataTable.TEXT,
      },
      {
        field: 'finishDate',
        header: 'Hoàn thành',
        width: '10rem',
        type: ETypeDataTable.TEXT,
      },
      {
        field: 'status',
        header: 'Trạng thái',
        width: '8rem',
        type: ETypeDataTable.STATUS,
        funcStyleClassStatus: this.funcStyleClassStatus,
        funcLabelStatus: this.funcLabelStatus,
        posTextCell: EPositionTextCell.CENTER,
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
    this.selectedColumns = this.getLocalStorage(ChangeVoucherRequest.keyStorage) ?? this.headerColumns;
    this.setPage({ page: this.offset });
  }

  ngAfterViewInit() {
    this.subject.keyword.pipe(debounceTime(SearchConst.DEBOUNCE_TIME)).subscribe(() => {
      if (this.filter.keyword === '') {
        this.setPage({ page: this.offset });
      } else {
        this.setPage();
      }
    });

    this.changeDetectorRef.detectChanges();
    this.changeDetectorRef.markForCheck();
  }

  public funcStyleClassStatus = (status: any) => {
    return this.getStatusSeverity(status);
  };

  public funcLabelStatus = (status: any) => {
    return this.getStatusName(status);
  };

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
          this.setLocalStorage(this.selectedColumns, ChangeVoucherRequest.keyStorage);
        }
      });
    }
  }

  private genListAction() {
    this.listAction = this.dataSource.map((data: ChangeVoucherRequestModel, index: number) => {
      const actions: IActionTable[] = [];
      if( this.isGranted([this.PermissionLoyaltyConst.Loyalty_YeuCauDoiVoucher_ChiTiet])){
        actions.push({
          data: data,
          label: 'Thông tin chi tiết',
          icon: 'pi pi-info-circle',
          command: ($event) => {
            this.detail($event.item.data);
          },
        });
      }


      if(this.isGranted([this.PermissionLoyaltyConst.Loyalty_YeuCauDoiVoucher_ChinhSua]) && (data.status === ChangeVoucherRequest.KHOI_TAO || data.status === ChangeVoucherRequest.TIEP_NHAN_YC)){
        actions.push({
          data: data,
          label: 'Chỉnh sửa',
          icon: 'pi pi-pencil',
          command: ($event) => {
            this.edit($event.item.data);
          },
        });
      }

      if (this.isGranted([this.PermissionLoyaltyConst.Loyalty_YeuCauDoiVoucher_BanGiao]) && data.status === ChangeVoucherRequest.TIEP_NHAN_YC) {
        actions.push({
          data: data,
          label: 'Bàn giao',
          icon: 'pi pi-check-circle',
          command: ($event) => {
            this.handOver($event.item.data);
          },
        });
      }

      if (this.isGranted([this.PermissionLoyaltyConst.Loyalty_YeuCauDoiVoucher_HoanThanh]) && data.status === ChangeVoucherRequest.DANG_GIAO) {
        actions.push({
          data: data,
          label: 'Hoàn thành',
          icon: 'pi pi-check',
          command: ($event) => {
            this.finish($event.item.data);
          },
        });
      }

      if (this.isGranted([this.PermissionLoyaltyConst.Loyalty_YeuCauDoiVoucher_HuyYeuCau]) && data.status !== ChangeVoucherRequest.HOAN_THANH && data.status !== ChangeVoucherRequest.HUY_YEU_CAU) {
        actions.push({
          data: data,
          label: 'Hủy yêu cầu',
          icon: 'pi pi-times',
          command: ($event) => {
            this.cancel($event.item.data);
          },
        });
      }
      return actions;
    });
  }

  public detail(data: ChangeVoucherRequestModel) {
    if (data.id) {
      this.changeVoucherRequestService.getDetail(data.id).subscribe((res: any) => {
        if (res) {
          const ref = this.dialogService.open(CrudChangeVoucherRequestComponent, {
            header: 'Chi tiết yêu cầu',
            width: '1200px',
            baseZIndex: 10000,
            data: {
              dataSource: res.data,
              type: EConfigDataModal.VIEW,
            } as IConfigDataModal,
          });
          ref.onClose.subscribe((response) => {
            if (this.handleResponseInterceptor(response, '')) {
              this.setPage({ page: this.offset });
            }
          });
        }
      });
    }
  }

  public edit(data: ChangeVoucherRequestModel) {
    if (data.id) {
      this.changeVoucherRequestService.getDetail(data.id).subscribe((res: any) => {
        if (res) {
          const ref = this.dialogService.open(CrudChangeVoucherRequestComponent, {
            header: 'Chi tiết yêu cầu',
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
              this.setPage({ page: this.offset });
            }
          });
        }
      });
    }
  }

  public cancel(data: ChangeVoucherRequestModel) {
    if (data) {
      this._helpersService.dialogConfirmRef(
        "Xác nhận hủy yêu cầu đổi điểm?",
        IconConfirm.QUESTION
      ).onClose.subscribe((accept: boolean) => {
        if(accept) {
          this.changeVoucherRequestService.changeStatus('cancel', data.id).subscribe(
            (response) => {
              if (this.handleResponseInterceptor(response, 'Xác nhận hủy yêu cầu đổi điểm')) {
                this.setPage({ page: this.offset });
              }
            },
            (err) => {
              this.messageError(`Có lỗi hủy yêu cầu đổi điểm`);
            }
          );
        }
      })
    }
  }

  public handOver(data: ChangeVoucherRequestModel) {
    if (data) {
      const ref = this.dialogService.open(FormNotificationDescriptionComponent, {
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
          title: 'Xác nhận giao voucher',
          icon: FormNotificationConst.IMAGE_APPROVE,
        } as INotiDataModal,
      });
      ref.onClose.subscribe((dataCallBack) => {
        if (dataCallBack?.accept) {
          this.changeVoucherRequestService.changeStatus('delivery', data.id, dataCallBack.data.description).subscribe(
            (response) => {
              if (this.handleResponseInterceptor(response, 'Xác nhận giao voucher')) {
                this.setPage({ page: this.offset });
              }
            },
            (err) => {
              this.messageError(`Có lỗi giao voucher`);
            }
          );
        }
      });
    }
  }

  public finish(data: ChangeVoucherRequestModel) {
    if (data) {
      const ref = this.dialogService.open(FormNotificationDescriptionComponent, {
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
          title: 'Xác nhận đã giao voucher tới khách',
          icon: FormNotificationConst.IMAGE_APPROVE,
        } as INotiDataModal,
      });
      ref.onClose.subscribe((dataCallBack) => {
        if (dataCallBack?.accept) {
          this.changeVoucherRequestService.changeStatus('finished', data.id, dataCallBack.data.description).subscribe(
            (response) => {
              if (this.handleResponseInterceptor(response, 'Xác nhận đã giao voucher tới khách')) {
                this.setPage({ page: this.offset });
              }
            },
            (err) => {
              this.messageError(`Có lỗi giao voucher tới khách`);
            }
          );
        }
      });
    }
  }

  public create(event: any) {
    if (event) {
      const ref = this.dialogService.open(CrudChangeVoucherRequestComponent, {
        header: 'Yêu cầu đổi voucher',
        width: '1200px',
        baseZIndex: 10000,
        data: {
          type: EConfigDataModal.CREATE,
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
          this.setPage({ page: this.offset });
        }
      });
    }
  }

  public changeFilter(event) {
    this.setPage({ page: this.offset });
  }

  public setPage(pageInfo?: any) {
    this.isLoading = true;
    this.page.pageNumber = pageInfo?.page ?? this.offset;
    if (pageInfo?.rows) this.page.pageSize = pageInfo?.rows;
    this.page.keyword = this.filter.keyword;
    let filter = { ...this.filter };
    filter.date && (filter.date = formatCalendarItem(filter.date));

    this.changeVoucherRequestService.getAll(this.page, filter).subscribe(
      (res) => {
        this.isLoading = false;
        if (this.handleResponseInterceptor(res, '')) {
          this.page.totalItems = res.data.totalItems;
          this.dataSource = res.data.items.map(
            (item: any) =>
              ({
                id: item.id,
                customer: item.fullname,
                numberPhone: item.phone,
                currentPoint: item.loyCurrentPoint,
                changePoint: item.totalConversionPoint,
                requestDate: item.requestDate ? formatDate(item.requestDate, ETypeFormatDate.DATE) : '',
                receiveDate: item.pendingDate ? formatDate(item.pendingDate, ETypeFormatDate.DATE) : '',
                handOverDate: item.deliveryDate ? formatDate(item.deliveryDate, ETypeFormatDate.DATE) : '',
                finishDate: item.finishedDate ? formatDate(item.finishedDate, ETypeFormatDate.DATE) : '',
                status: item.status,
              } as ChangeVoucherRequestModel)
          );
          this.genListAction();
        }
      },
      (err) => {
        this.isLoading = false;
      }
    );
  }
}
