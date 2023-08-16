import { ChangeDetectorRef, Component, Injector } from '@angular/core';
import { Router } from '@angular/router';
import {
  EConfigDataModal,
  EPositionFrozenCell,
  EPositionTextCell,
  ETypeDataTable,
  FormNotificationConst,
  PermissionLoyaltyConst,
  SearchConst,
  VoucherManagement,
  YES_NO,
} from '@shared/AppConsts';
import { CrudComponentBase } from '@shared/crud-component-base';
import { formatCalendarItem, formatCurrency } from '@shared/function-common';
import {
  IActionTable,
  IConfigDataModal,
  IDropdown,
  IHeaderColumn,
  INotiDataModal,
} from '@shared/interface/InterfaceConst.interface';
import { VoucherManagementModel } from '@shared/interface/voucher-management/voucher-management/VoucherManagement.model';
import { Page } from '@shared/model/page';
import { VoucherManagementService } from '@shared/services/voucher-management-service';
import { MessageService } from 'primeng/api';
import { DialogService } from 'primeng/dynamicdialog';
import { debounceTime } from 'rxjs/operators';
import { FormSetDisplayColumnComponent } from 'src/app/form-general/form-set-display-column/form-set-display-column/form-set-display-column.component';
import { BreadcrumbService } from 'src/app/layout/breadcrumb/breadcrumb.service';
import { CreateOrEditVoucherDialogComponent } from './create-or-edit-voucher-dialog/create-or-edit-voucher-dialog.component';
import { HelpersService } from '@shared/services/helpers.service';
import { IconConfirm } from '@shared/consts/base.const';

@Component({
  selector: 'voucher-management',
  templateUrl: './voucher-management.component.html',
  styleUrls: ['./voucher-management.component.scss'],
})
export class VoucherManagementComponent extends CrudComponentBase {
  constructor(
    injector: Injector,
    messageService: MessageService,
    private dialogService: DialogService,
    private breadcrumbService: BreadcrumbService,
    private router: Router,
    private changeDetectorRef: ChangeDetectorRef,
    private voucherManagementService: VoucherManagementService,
    private _helpersService: HelpersService,
  ) {
    super(injector, messageService);
    this.breadcrumbService.setItems([
      { label: 'Trang chủ', routerLink: ['/home'] },
      { label: 'Quản lý ưu đãi', routerLink: ['/home'] },
      { label: 'Danh sách ưu đãi' },
    ]);
  }

  public dataSource: VoucherManagementModel[] = [];
  public isLoading: boolean;
  public page: Page;
  public listAction: IActionTable[][] = [];
  public selectedColumns: IHeaderColumn[] = [];
  public headerColumns: IHeaderColumn[] = [];
  public filter: {
    keyword: string;
    status: number | undefined;
    kind: number | undefined;
    type: string | undefined;
    isShowApp: string | undefined;
    expireDate: any | undefined;
  } = {
    keyword: '',
    status: undefined,
    kind: undefined,
    type: undefined,
    isShowApp: undefined,
    expireDate: '',
  };
  public baseUrl: string = '';

  public get listStatus() {
    return VoucherManagement.listStatus;
  }

  public get listKindOfVoucher() {
    return VoucherManagement.listKindOfVoucher;
  }

  public get listTypeOfVoucher() {
    return VoucherManagement.listTypeOfVoucher;
  }

  public get listShowApp() {
    return VoucherManagement.listShowApp;
  }

  public getStatusSeverity(code: any) {
    return VoucherManagement.getStatus(code, 'severity');
  }

  public getStatusName(code: any) {
    return VoucherManagement.getStatus(code, 'label');
  }
  
  ngOnInit(): void {
    this.baseUrl = this.AppConsts.remoteServiceBaseUrl ?? this.baseUrl;
    this.headerColumns = [
      {
        field: 'voucherId',
        header: '#ID',
        width: '5rem',
        isPin: true,
        type: ETypeDataTable.INDEX,
        posTextCell: EPositionTextCell.CENTER,
        isFrozen: true,
        posFrozen: EPositionFrozenCell.LEFT,
      },
      {
        field: 'code',
        header: 'Mã lô',
        width: '10rem',
        isPin: true,
        type: ETypeDataTable.TEXT,
      },
      {
        field: 'name',
        header: 'Tên voucher',
        width: '20rem',
        isPin: true,
        type: ETypeDataTable.TEXT,
      },
      {
        field: 'kind',
        header: 'Loại hình',
        width: '8rem',
        isPin: true,
        type: ETypeDataTable.TEXT,
      },
      {
        field: 'type',
        header: 'Loại voucher',
        width: '8rem',
        isPin: true,
        type: ETypeDataTable.TEXT,
      },
      {
        field: 'value',
        header: 'Giá trị',
        width: '10rem',
        isPin: true,
        type: ETypeDataTable.TEXT,
      },
      {
        field: 'point',
        header: 'Điểm quy đổi',
        width: '9rem',
        type: ETypeDataTable.TEXT,
      },
      {
        field: 'quantity',
        header: 'Số lượng',
        width: '8rem',
        type: ETypeDataTable.TEXT,
      },
      {
        field: 'apply',
        header: 'Cấp phát',
        width: '8rem',
        type: ETypeDataTable.TEXT,
      },
      {
        field: 'isShowApp',
        header: 'Show app',
        width: '7rem',
        type: ETypeDataTable.CHECK_BOX,
        posTextCell: EPositionTextCell.CENTER,
      },
      {
        field: 'isHighlight',
        header: 'Nổi bật',
        width: '6rem',
        type: ETypeDataTable.CHECK_BOX,
        posTextCell: EPositionTextCell.CENTER,
      },
      {
        field: 'createUser',
        header: 'Người cài đặt',
        width: '15rem',
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
    this.selectedColumns = this.getLocalStorage(VoucherManagement.keyStorage) ?? this.headerColumns;
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
          this.setLocalStorage(this.selectedColumns, VoucherManagement.keyStorage);
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
    this.listAction = this.dataSource.map((data: VoucherManagementModel, index: number) => {
      const actions: IActionTable[] = [];
      if (this.isGranted([this.PermissionLoyaltyConst.Loyalty_DanhSachUuDai_ChiTiet])){
        actions.push({
          data: data,
          label: 'Thông tin chi tiết',
          icon: 'pi pi-info-circle',
          command: ($event) => {
            this.detail($event.item.data);
          },
        });
      }

      // voucher trạng thái !== XÓA
      if (data.status !== VoucherManagement.DA_XOA) {
        if (this.isGranted([this.PermissionLoyaltyConst.Loyalty_DanhSachUuDai_ChinhSua])){
          actions.push({
            data: data,
            label: 'Chỉnh sửa',
            icon: 'pi pi-pencil',
            command: ($event) => {
              this.edit($event.item.data);
            },
          });
        }

        if (data.status === VoucherManagement.KICH_HOAT) {
          if(this.isGranted([this.PermissionLoyaltyConst.Loyalty_DanhSachUuDai_DanhDauOrTatNoiBat])){
            actions.push({
              data: data,
              label: !!data.isHighlight ? 'Tắt nổi bật' : 'Đánh dấu nổi bật',
              icon: 'pi pi-pencil',
              command: ($event) => {
                this.highlight($event.item.data, !!data.isHighlight);
              },
            });
          }

          if(this.isGranted([this.PermissionLoyaltyConst.Loyalty_DanhSachUuDai_BatOrTatShowApp])){
            actions.push({
              data: data,
              label: !!data.isShowApp ? 'Tắt ShowApp' : 'Bật ShowApp',
              icon: 'pi pi-pencil',
              command: ($event) => {
                this.showApp($event.item.data, !!data.isShowApp);
              },
            });
          }

          if(this.isGranted([this.PermissionLoyaltyConst.Loyalty_DanhSachUuDai_KichHoatOrHuy])){
            actions.push({
              data: data,
              label: 'Hủy kích hoạt',
              icon: 'pi pi-times',
              command: ($event) => {
                this.deactivate($event.item.data);
              },
            });
          }
        }

        // voucher trạng thái HỦY KÍCH HOẠT => có thể KÍCH HOẠT
        if(this.isGranted([this.PermissionLoyaltyConst.Loyalty_DanhSachUuDai_KichHoatOrHuy]) && data.status === VoucherManagement.HUY_KICH_HOAT){
          actions.push({
            data: data,
            label: 'Kích hoạt',
            icon: 'pi pi-check',
            command: ($event) => {
              this.activate($event.item.data);
            },
          });
        }

        // voucher trạng thái !== HẾT HẠN
        if(this.isGranted([this.PermissionLoyaltyConst.Loyalty_DanhSachUuDai_KichHoatOrHuy]) && data.status !== VoucherManagement.HET_HAN){
          actions.push({
            data: data,
            label: 'Xóa',
            icon: 'pi pi-trash',
            command: ($event) => {
              this.delete($event.item.data);
            },
          });
        }

      }
      return actions;
    });
  }

  public detail(data: VoucherManagementModel) {
    if (data.voucherId) {
      this.voucherManagementService.getVoucherById(data.voucherId).subscribe((res: any) => {
        if (res) {
          this.dialogService.open(CreateOrEditVoucherDialogComponent, {
            header: 'Xem chi tiết voucher',
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

  public edit(data: VoucherManagementModel) {
    if (data.voucherId) {
      this.voucherManagementService.getVoucherById(data.voucherId).subscribe((res: any) => {
        if (res) {
          const ref = this.dialogService.open(CreateOrEditVoucherDialogComponent, {
            header: 'Chỉnh sửa voucher',
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

  public highlight(data: VoucherManagementModel, isHighlight: boolean) {
    if (data) {
      this._helpersService.dialogConfirmRef(
        `Xác nhận ${isHighlight ? 'Tắt nổi bật' : 'Đánh dấu nổi bật'}?`,
        IconConfirm.QUESTION
      ).onClose.subscribe((accept: boolean) => {
        if(accept) {
          this.voucherManagementService
          .highlightVoucher(data.voucherId, isHighlight ? YES_NO.NO : YES_NO.YES)
          .subscribe(
            (response) => {
              if (this.handleResponseInterceptor(response, 'Thành công')) {
                this.setPage({ page: this.offset });
              }
            }, (err) => {
              this.messageError(`Thất bại`);
            }
          );
        }
      })
    }
  }

  public showApp(data: VoucherManagementModel, isShowApp: boolean) {
    if (data) {
      this._helpersService.dialogConfirmRef(
        `Xác nhận ${isShowApp ? 'Tắt ShowApp' : 'Bật ShowApp'}?`,
        IconConfirm.DELETE
      ).onClose.subscribe((accept: boolean) => {
        if(accept) {
          this.voucherManagementService.showAppVoucher(data.voucherId, isShowApp ? YES_NO.NO : YES_NO.YES).subscribe(
            (response) => {
              if (this.handleResponseInterceptor(response, 'Thành công')) {
                this.setPage({ page: this.offset });
              }
            }, (err) => {
              this.messageError(`Thất bại`);
            }
          );
        }
      })
    }
  }

  public deactivate(data: VoucherManagementModel) {
    if (data) {
      this._helpersService.dialogConfirmRef(
        "Xác nhận hủy kích hoạt?",
        IconConfirm.QUESTION
      ).onClose.subscribe((accept: boolean) => {
        if(accept) {
          this.voucherManagementService.changeStatusVoucher(VoucherManagement.HUY_KICH_HOAT, data.voucherId).subscribe(
            (response) => {
              if (this.handleResponseInterceptor(response, 'Hủy kích hoạt thành công')) {
                this.setPage({ page: this.offset });
              }
            },
            (err) => {
              this.messageError(`Không hủy kích hoạt được voucher`);
            }
          );
        }
      })
    }
  }

  public activate(data: VoucherManagementModel) {
    if (data) {
      this._helpersService.dialogConfirmRef(
        "Xác nhận kích hoạt?",
        IconConfirm.APPROVE
      ).onClose.subscribe((accept: boolean) => {
        if(accept) {
          this.voucherManagementService.changeStatusVoucher(VoucherManagement.KICH_HOAT, data.voucherId).subscribe(
            (response) => {
              if (this.handleResponseInterceptor(response, 'Kích hoạt thành công')) {
                this.setPage({ page: this.offset });
              }
            }, (err) => {
              this.messageError(`Hủy kích hoạt không thành công`);
            });
          }
        }
      )
    }
  }

  public delete(data: VoucherManagementModel) {
    if (data) {
      this._helpersService.dialogConfirmRef(
        "Xác nhận xóa voucher?",
        IconConfirm.DELETE
      ).onClose.subscribe((accept: boolean) => {
        if(accept) {
          this.voucherManagementService.deleteVoucher(data.voucherId).subscribe(
            (response) => {
              if (this.handleResponseInterceptor(response, 'Xóa thành công')) {
                this.setPage({ page: this.offset });
              }
            },
            (err) => {
              this.messageError(`Không xóa được voucher`);
            }
          );
        }
      })
    }
  }

  public create(event: any) {
    if (event) {
      const ref = this.dialogService.open(CreateOrEditVoucherDialogComponent, {
        header: 'Thêm mới voucher',
        width: '1200px',
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
    filter.expireDate && (filter.expireDate = formatCalendarItem(filter.expireDate));

    this.voucherManagementService.getAllVoucher(this.page, filter).subscribe(
      (res) => {
        this.isLoading = false;
        if (this.handleResponseInterceptor(res, '')) {
          this.page.totalItems = res.data.totalItems;
          if (res.data?.items) {
            this.dataSource = res.data.items.map(
              (item: any) =>
                ({
                  voucherId: item.voucherId,
                  code: item.code,
                  name: item.name,
                  kind: this.listKindOfVoucher.find((e: IDropdown) => e.value === item.voucherType)?.label || '',
                  type: this.listTypeOfVoucher.find((e: IDropdown) => e.value === item.useType)?.label || '',
                  value: item.value
                    ? ((value: number, valueUnit: string) => {
                        return `${formatCurrency(value)} ${
                          VoucherManagement.listValueUnit.find((e: IDropdown) => e.value === valueUnit)?.label || ''
                        }`;
                      })(item.value, item.unit)
                    : 0,
                  point: item.point,
                  quantity: item.publishNum,
                  apply: item.conversionQuantiy,
                  isShowApp: item.isShowApp === YES_NO.YES,
                  isHighlight: item.isHot === YES_NO.YES,
                  createUser: item.createdBy,
                  status: (item.isExpiredDate && item.status === VoucherManagement.KICH_HOAT) ? VoucherManagement.HET_HAN : item.status,
                } as VoucherManagementModel)
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
