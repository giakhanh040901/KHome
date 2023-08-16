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
  VoucherManagement,
} from '@shared/AppConsts';
import { FormNotificationDescriptionComponent } from '@shared/components/form-notification-description/form-notification-description.component';
import { IconConfirm } from '@shared/consts/base.const';
import { CrudComponentBase } from '@shared/crud-component-base';
import { formatDate } from '@shared/function-common';
import {
  IConstant,
  IDropdown,
  IHeaderColumn,
  INotiDataModal,
  IValueFormatter,
} from '@shared/interface/InterfaceConst.interface';
import {
  ChangeVoucherRequestItem,
  CrudChangeVoucherRequest,
} from '@shared/interface/request-management/change-voucher-request/ChangeVoucherRequest.model';
import { ChangeVoucherRequestService } from '@shared/services/change-voucher-request-service';
import { HelpersService } from '@shared/services/helpers.service';
import { MessageService } from 'primeng/api';
import { DialogService, DynamicDialogConfig, DynamicDialogRef } from 'primeng/dynamicdialog';

@Component({
  selector: 'crud-change-voucher-request',
  templateUrl: './crud-change-voucher-request.component.html',
  styleUrls: ['./crud-change-voucher-request.component.scss'],
})
export class CrudChangeVoucherRequestComponent extends CrudComponentBase {
  public type: string = EConfigDataModal.CREATE;
  public crudDTO: CrudChangeVoucherRequest = new CrudChangeVoucherRequest();
  public headerColumns: IHeaderColumn[] = [];
  public listVoucher: IDropdown[] = [];
  public initCusomter: any = undefined;
  public headerCustomer: IHeaderColumn[] = [];
  public minusPoint: number | undefined = undefined;
  public currentPoint: number | undefined = undefined;

  constructor(
    injector: Injector,
    messageService: MessageService,
    private router: Router,
    private ref: DynamicDialogRef,
    private changeDetectorRef: ChangeDetectorRef,
    private config: DynamicDialogConfig,
    private dialogService: DialogService,
    private changeVoucherRequestService: ChangeVoucherRequestService,
    private _helpersService: HelpersService,
  ) {
    super(injector, messageService);
  }

  ngOnInit() {
    const action: IHeaderColumn = {
      field: 'action',
      header: '',
      type: ETypeDataTable.ACTION_BUTTON,
      width: '10rem',
      posTextCell: EPositionTextCell.CENTER,
      isFrozen: true,
      posFrozen: EPositionFrozenCell.RIGHT,
    };

    this.headerCustomer = [
      {
        field: 'name',
        header: 'Tên',
        type: ETypeDataTable.TEXT,
        width: '20rem',
      },
      {
        field: 'numberPhone',
        header: 'Số điện thoại',
        type: ETypeDataTable.TEXT,
        width: '10rem',
      },
      {
        field: 'email',
        header: 'Email',
        type: ETypeDataTable.TEXT,
        width: '20rem',
      },
      {
        field: 'address',
        header: 'Địa chỉ',
        type: ETypeDataTable.TEXT,
        width: '20rem',
      },
      {
        field: 'membershipClass',
        header: 'Hạng thành viên',
        type: ETypeDataTable.TEXT,
        width: '15rem',
      },
      {
        field: 'totalPoint',
        header: 'Điểm tích lũy',
        type: ETypeDataTable.TEXT,
        width: '10rem',
        posTextCell: EPositionTextCell.CENTER,
        isFrozen: true,
        posFrozen: EPositionFrozenCell.RIGHT,
      },
      {
        field: 'currentPointDisplay',
        header: 'Điểm hiện tại',
        type: ETypeDataTable.TEXT,
        width: '10rem',
        posTextCell: EPositionTextCell.CENTER,
        isFrozen: true,
        posFrozen: EPositionFrozenCell.RIGHT,
      },
    ];
    this.headerColumns = [
      {
        field: 'id',
        header: '#ID',
        width: '3rem',
        type: ETypeDataTable.INDEX,
      },
      {
        field: 'date',
        header: 'Ngày',
        width: '12rem',
        type: ETypeDataTable.TEXT,
        valueFormatter: (param: IValueFormatter) =>
          param.data ? formatDate(param.data, ETypeFormatDate.DATE_TIME) : '',
      },
      {
        field: 'status',
        header: 'Trạng thái',
        width: '10rem',
        type: ETypeDataTable.TEXT,
        valueFormatter: (param: IValueFormatter) =>
          this.listStatus.find((e: IDropdown) => e.value === param.data)?.label || '',
      },
      {
        field: 'actionUser',
        header: 'Người thực hiện',
        width: '10rem',
        type: ETypeDataTable.TEXT,
      },
      {
        field: 'source',
        header: 'Nguồn',
        width: '8rem',
        type: ETypeDataTable.TEXT,
        valueFormatter: (param: IValueFormatter) =>
          this.listSource.find((e: IConstant) => e.id === param.data)?.value || '',
      },
      {
        field: 'note',
        header: 'Ghi chú',
        width: '',
        type: ETypeDataTable.TEXT,
      },
    ];
    if (this.config.data) {
      this.type = this.config.data.type;
    }
    this.type !== EConfigDataModal.VIEW && this.headerCustomer.push(action);
    if (this.type === EConfigDataModal.CREATE) {
      this.crudDTO.requestType = Number(ChangeVoucherRequest.DOI_VOUCHER);
      this.crudDTO.requestDate = new Date();
      this.crudDTO.isMinusPoint = true;
      this.crudDTO.changeVoucherRequestItem = [new ChangeVoucherRequestItem()];
    } else {
      this.crudDTO.mapData(this.config.data.dataSource);
      this.initCusomter = {
        type: VoucherManagement.CUSTOMER.INDIVIDUAL,
        customerId: this.crudDTO.individualId,
      };
    }
  }

  ngAfterViewInit() {
    this.changeVoucherRequestService._listVoucherRequest$.subscribe((res: IDropdown[] | undefined) => {
      if (res) {
        this.listVoucher = res;
      }
    });

    this.changeDetectorRef.detectChanges();
    this.changeDetectorRef.markForCheck();
  }

  public get isDisable() {
    return this.type === EConfigDataModal.VIEW;
  }

  public get listRequestType() {
    return ChangeVoucherRequest.listRequestType;
  }

  public get listApplyType() {
    return ChangeVoucherRequest.listApplyType;
  }

  public get listStatus() {
    return ChangeVoucherRequest.listStatus;
  }

  public get listSource() {
    return ChangeVoucherRequest.listSource;
  }

  public get showMinusPoint() {
    return this.crudDTO.applyType === ChangeVoucherRequest.TANG_KHACH_HANG;
  }

  public get showDetailProcess() {
    return this.type !== EConfigDataModal.CREATE;
  }

  public get EConfigDataModal() {
    return EConfigDataModal;
  }

  public get showBtnReceive() {
    return this.crudDTO.status === ChangeVoucherRequest.KHOI_TAO;
  }

  public get showBtnCancel() {
    return (
      this.crudDTO.status !== ChangeVoucherRequest.HOAN_THANH &&
      this.crudDTO.status !== ChangeVoucherRequest.HUY_YEU_CAU
    );
  }

  public onChangeSearchCustomer(event: any) {
    if (event) {
      this.crudDTO.individualId = event.individualId;
      this.crudDTO.businessId = event.businessId;
    }
  }

  public add(event: any) {
    if (event) {
      this.crudDTO.changeVoucherRequestItem.push(new ChangeVoucherRequestItem());
    }
  }

  public deleteItem(event: any, index: number) {
    if (event) {
      this.crudDTO.changeVoucherRequestItem.splice(index, 1);
      this.getMinusPoint();
    }
  }

  public close(event: any) {
    this.ref.close();
  }

  public save(event?: any) {
    if (event) {
      if (this.crudDTO.isValidData(this.listVoucher, this.currentPoint)) {
        this.changeVoucherRequestService
          .saveChangeRequest(this.crudDTO.toObjectSendToAPI(), this.type === EConfigDataModal.EDIT)
          .subscribe(
            (response) => {
              if (this.handleResponseInterceptor(response, '')) {
                this.ref.close(response);
              }
            },
            (err) => {}
          );
      } else {
        const messageError = this.crudDTO.dataValidator.length ? this.crudDTO.dataValidator[0].message : undefined;
        messageError && this.messageError(messageError);
      }
    }
  }

  public cancel(event: any) {
    this._helpersService.dialogConfirmRef(
      "Xác nhận hủy yêu cầu?",
      IconConfirm.QUESTION
    ).onClose.subscribe((accept: boolean) => {
      if(accept) {
        this.changeVoucherRequestService.changeStatus('cancel', this.crudDTO.id).subscribe(
          (response) => {
            if (this.handleResponseInterceptor(response, 'Xác nhận hủy yêu cầu')) {
              this.ref.close(response);
            }
          },
          (err) => {
            this.messageError(`Có lỗi hủy yêu cầu`);
          }
        );
      }
    })
  }

  public receive(event: any) {
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
        title: 'Xác nhận tiếp nhận yêu cầu',
        icon: FormNotificationConst.IMAGE_APPROVE,
      } as INotiDataModal,
    });
    ref.onClose.subscribe((dataCallBack) => {
      if (dataCallBack?.accept) {
        this.changeVoucherRequestService
          .changeStatus('pending', this.crudDTO.id, dataCallBack.data.description)
          .subscribe(
            (response) => {
              if (this.handleResponseInterceptor(response, 'Xác nhận đã tiếp nhận yêu cầu')) {
                this.ref.close(response);
              }
            },
            (err) => {
              this.messageError(`Có lỗi tiếp nhận yêu cầu`);
            }
          );
      }
    });
  }

  public dataSourceChange(event: ChangeVoucherRequestItem) {
    if (event) {
      this.getMinusPoint();
    }
  }

  public onChangeCurrentPoint(event: number | undefined) {
    if (event || event === 0) {
      this.currentPoint = event;
    }
  }

  public getMinusPoint() {
    this.minusPoint = this.crudDTO.changeVoucherRequestItem.reduce((result: number, item: ChangeVoucherRequestItem) => {
      return (result += item.totalPoint);
    }, 0);
  }
}
