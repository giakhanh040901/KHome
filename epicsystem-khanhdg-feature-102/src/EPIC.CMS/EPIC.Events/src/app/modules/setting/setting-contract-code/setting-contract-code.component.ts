import { ChangeDetectorRef, Component, Injector } from '@angular/core';
import { Router } from '@angular/router';
import {
  EConfigDataModal,
  EPositionFrozenCell,
  EPositionTextCell,
  ETypeDataTable,
  ETypeFormatDate,
  ETypeStatus,
  FormNotificationConst,
  SearchConst,
  SettingContractCode,
} from '@shared/AppConsts';
import { FormNotificationComponent } from '@shared/components/form-notification/form-notification.component';
import { FormSetDisplayColumnComponent } from '@shared/components/form-set-display-column/form-set-display-column.component';
import { CrudComponentBase } from '@shared/crud-component-base';
import { formatDate } from '@shared/function-common';
import {
  IActionTable,
  IConfigDataModal,
  IHeaderColumn,
  INotiDataModal,
} from '@shared/interface/InterfaceConst.interface';
import { ContractCodeModel } from '@shared/interface/setting/setting-contract-code/ContractCode.model';
import { Page } from '@shared/model/page';
import { SettingContractCodeService } from '@shared/services/setting-contract-code-service';
import { MessageService } from 'primeng/api';
import { DialogService } from 'primeng/dynamicdialog';
import { debounceTime } from 'rxjs/operators';
import { BreadcrumbService } from 'src/app/layout/breadcrumb/breadcrumb.service';
import { CrudContractCodeComponent } from './crud-contract-code/crud-contract-code.component';

@Component({
  selector: 'setting-contract-code',
  templateUrl: './setting-contract-code.component.html',
  styleUrls: ['./setting-contract-code.component.scss'],
})
export class SettingContractCodeComponent extends CrudComponentBase {
  constructor(
    injector: Injector,
    messageService: MessageService,
    private dialogService: DialogService,
    private breadcrumbService: BreadcrumbService,
    private router: Router,
    private changeDetectorRef: ChangeDetectorRef,
    private settingContractCodeService: SettingContractCodeService
  ) {
    super(injector, messageService);
    this.breadcrumbService.setItems([
      { label: 'Trang chủ', routerLink: ['/home'] },
      { label: 'Cài đặt', routerLink: ['/home'] },
      { label: 'Cấu trúc mã hợp đồng' },
    ]);
  }

  public dataSource: ContractCodeModel[] = [];
  public isLoading: boolean;
  public page: Page;
  public listAction: IActionTable[][] = [];
  public selectedColumns: IHeaderColumn[] = [];
  public headerColumns: IHeaderColumn[] = [];
  public filter: {
    keyword: string;
    status: number | undefined;
  } = {
    keyword: '',
    status: undefined,
  };

  public get listStatus() {
    return SettingContractCode.listStatus;
  }

  public getStatusSeverity(code: any) {
    return SettingContractCode.getStatus(code, ETypeStatus.SEVERITY);
  }

  public getStatusName(code: any) {
    return SettingContractCode.getStatus(code, ETypeStatus.LABEL);
  }

  ngOnInit(): void {
    this.headerColumns = (
      [
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
          header: 'Tên cấu trúc',
          minWidth: '15rem',
          maxWidth: '30vw',
          isPin: true,
          type: ETypeDataTable.TEXT,
        },
        {
          field: 'code',
          header: 'Cấu trúc mã hợp đồng',
          minWidth: '20rem',
          maxWidth: '30vw',
          isPin: true,
          type: ETypeDataTable.TEXT,
        },
        {
          field: 'description',
          header: 'Ghi chú',
          minWidth: '15rem',
          maxWidth: '30vw',
          isPin: true,
          type: ETypeDataTable.TEXT,
        },
        {
          field: 'settingUser',
          header: 'Người cài đặt',
          width: '10rem',
          isPin: true,
          type: ETypeDataTable.TEXT,
        },

        {
          field: 'settingDate',
          header: 'Ngày cài đặt',
          width: '12rem',
          isPin: true,
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
      ] as IHeaderColumn[]
    ).map((item: IHeaderColumn, index: number) => {
      item.position = index + 1;
      return item;
    });
    this.selectedColumns = this.getLocalStorage(SettingContractCode.keyStorage) ?? this.headerColumns;
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
          this.setLocalStorage(this.selectedColumns, SettingContractCode.keyStorage);
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
    this.listAction = this.dataSource.map((data: ContractCodeModel, index: number) => {
      const actions: IActionTable[] = [];

      if (this.isGranted([this.PermissionEventConst.CauTrucMaHD_ChiTiet])) {
        actions.push({
          data: data,
          label: 'Thông tin chi tiết',
          icon: 'pi pi-info-circle',
          command: ($event) => {
            this.detail($event.item.data);
          },
        });
      }

      if (this.isGranted([this.PermissionEventConst.CauTrucMaHD_CapNhat])) {
        actions.push({
          data: data,
          label: 'Chỉnh sửa',
          icon: 'pi pi-pencil',
          command: ($event) => {
            this.edit($event.item.data);
          },
        });
      }

      if (
        this.isGranted([this.PermissionEventConst.CauTrucMaHD_DoiTrangThai]) &&
        data.status === SettingContractCode.DANG_SU_DUNG
      ) {
        actions.push({
          data: data,
          label: 'Hủy kích hoạt',
          icon: 'pi pi-times',
          command: ($event) => {
            this.deactivate($event.item.data);
          },
        });
      }

      if (
        this.isGranted([this.PermissionEventConst.CauTrucMaHD_DoiTrangThai]) &&
        data.status === SettingContractCode.HUY_KICH_HOAT
      ) {
        actions.push({
          data: data,
          label: 'Kích hoạt',
          icon: 'pi pi-check',
          command: ($event) => {
            this.activate($event.item.data);
          },
        });
      }
      
      actions.push({
        data: data,
        label: 'Xóa cấu trúc',
        icon: 'pi pi-trash',
        command: ($event) => {
          this.delete($event.item.data);
        },
      });
      return actions;
    });
  }

  public detail(data: ContractCodeModel) {
    if (data.id) {
      this.getById(data.id).subscribe((res: any) => {
        if (res) {
          this.dialogService.open(CrudContractCodeComponent, {
            header: 'Xem chi tiết',
            width: '800px',
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

  public edit(data: ContractCodeModel) {
    if (data.id) {
      this.getById(data.id).subscribe((res: any) => {
        if (res) {
          const ref = this.dialogService.open(CrudContractCodeComponent, {
            header: 'Chỉnh sửa',
            width: '800px',
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
              this.setPage();
            }
          });
        }
      });
    }
  }

  public deactivate(data: ContractCodeModel) {
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
          title: 'Bạn có chắc chắn muốn hủy kích hoạt?',
          icon: FormNotificationConst.IMAGE_CLOSE,
        } as INotiDataModal,
      });
      ref.onClose.subscribe((dataCallBack) => {
        if (dataCallBack?.accept) {
          this.settingContractCodeService.changeStatus(data.id).subscribe(
            (response) => {
              if (this.handleResponseInterceptor(response, 'Hủy kích hoạt thành công')) {
                this.setPage();
              }
            },
            (err) => {
              this.messageError(`Không hủy kích hoạt được`);
            }
          );
        }
      });
    }
  }

  public activate(data: ContractCodeModel) {
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
          title: 'Bạn có chắc chắn muốn kích hoạt?',
          icon: FormNotificationConst.IMAGE_CLOSE,
        } as INotiDataModal,
      });
      ref.onClose.subscribe((dataCallBack) => {
        if (dataCallBack?.accept) {
          this.settingContractCodeService.changeStatus(data.id).subscribe(
            (response) => {
              if (this.handleResponseInterceptor(response, 'Kích hoạt thành công')) {
                this.setPage();
              }
            },
            (err) => {
              this.messageError(`Không kích hoạt được`);
            }
          );
        }
      });
    }
  }

  public delete(data: ContractCodeModel) {
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
          title: 'Bạn có chắc chắn muốn xóa cấu trúc mã hợp đồng?',
          icon: FormNotificationConst.IMAGE_CLOSE,
        } as INotiDataModal,
      });
      ref.onClose.subscribe((dataCallBack) => {
        if (dataCallBack?.accept) {
          this.settingContractCodeService.delete(data.id).subscribe(
            (response) => {
              if (this.handleResponseInterceptor(response, 'Xóa cấu trúc mã hợp đồng thành công')) {
                this.setPage();
              }
            },
            (err) => {
              this.messageError(`Xóa cấu trúc mã hợp đồng không thành công`);
            }
          );
        }
      });
    }
  }

  public create(event: any) {
    if (event) {
      const ref = this.dialogService.open(CrudContractCodeComponent, {
        header: 'Thêm mới mã hợp đồng',
        width: '800px',
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
          this.setPage();
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

    this.settingContractCodeService.findAll(this.page, this.filter).subscribe(
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
                  code:
                    item.configContractCodeDetails && item.configContractCodeDetails.length
                      ? item.configContractCodeDetails.reduce((value, item) => {
                          return (value += `<${item.key}>`);
                        }, '')
                      : '',
                  description: item.description,
                  settingUser: item.createdBy,
                  settingDate: item.createdDate ? formatDate(item.createdDate, ETypeFormatDate.DATE_TIME) : '',
                  status: item.status,
                } as ContractCodeModel)
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

  private getById(id: number) {
    return this.settingContractCodeService.getById(id);
  }
}
