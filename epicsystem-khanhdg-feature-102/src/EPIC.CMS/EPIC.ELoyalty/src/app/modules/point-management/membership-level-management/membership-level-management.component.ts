import { ChangeDetectorRef, Component, Injector } from '@angular/core';
import { Router } from '@angular/router';
import {
  EConfigDataModal,
  EPositionFrozenCell,
  EPositionTextCell,
  ETypeDataTable,
  ETypeFormatDate,
  FormNotificationConst,
  MembershipLevelManagement,
  PermissionLoyaltyConst,
  SearchConst,
} from '@shared/AppConsts';
import { CrudComponentBase } from '@shared/crud-component-base';
import {
  IActionTable,
  IConfigDataModal,
  IHeaderColumn,
  INotiDataModal,
  IValueFormatter,
} from '@shared/interface/InterfaceConst.interface';
import { MembershipLevelManagementModel } from '@shared/interface/point-management/membership-level-management/MembershipLevelManagement.model';
import { Page } from '@shared/model/page';
import { MembershipLevelManagementService } from '@shared/services/membership-level-management-service';
import { MessageService } from 'primeng/api';
import { DialogService } from 'primeng/dynamicdialog';
import { debounceTime } from 'rxjs/operators';
import { FormSetDisplayColumnComponent } from 'src/app/form-general/form-set-display-column/form-set-display-column/form-set-display-column.component';
import { BreadcrumbService } from 'src/app/layout/breadcrumb/breadcrumb.service';
import { CrudMembershipLevelManagementComponent } from './crud-membership-level-management/crud-membership-level-management.component';
import { formatDate } from '@shared/function-common';
import { HelpersService } from '@shared/services/helpers.service';
import { IconConfirm } from '@shared/consts/base.const';

@Component({
  selector: 'membership-level-management',
  templateUrl: './membership-level-management.component.html',
  styleUrls: ['./membership-level-management.component.scss'],
})
export class MembershipLevelManagementComponent extends CrudComponentBase {
  constructor(
    injector: Injector,
    messageService: MessageService,
    private dialogService: DialogService,
    private breadcrumbService: BreadcrumbService,
    private router: Router,
    private changeDetectorRef: ChangeDetectorRef,
    private membershipLevelManagementService: MembershipLevelManagementService,
    private _helpersService: HelpersService,
  ) {
    super(injector, messageService);
    this.breadcrumbService.setItems([
      { label: 'Trang chủ', routerLink: ['/home'] },
      { label: 'Quản lý điểm', routerLink: ['/home'] },
      { label: 'Quản lý hạng thành viên' },
    ]);
  }

  public dataSource: MembershipLevelManagementModel[] = [];
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
    return MembershipLevelManagement.listStatus;
  }

  public getStatusSeverity(code: any) {
    return MembershipLevelManagement.getStatus(code, 'severity');
  }

  public getStatusName(code: any) {
    return MembershipLevelManagement.getStatus(code, 'label');
  }

  ngOnInit() {
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
        header: 'Tên hạng',
        width: '10rem',
        isPin: true,
        type: ETypeDataTable.TEXT,
      },
      {
        field: 'description',
        header: 'Mô tả',
        width: '',
        isPin: true,
        type: ETypeDataTable.TEXT,
      },
      {
        field: 'startPoint',
        header: 'Số điểm bắt đầu',
        width: '10rem',
        isPin: true,
        type: ETypeDataTable.TEXT,
      },
      {
        field: 'endPoint',
        header: 'Số điểm kết thúc',
        width: '10rem',
        isPin: true,
        type: ETypeDataTable.TEXT,
      },
      {
        field: 'applyDate',
        header: 'Ngày áp dụng',
        width: '10rem',
        type: ETypeDataTable.TEXT,
        valueFormatter: (param: IValueFormatter) => (param.data ? formatDate(param.data, ETypeFormatDate.DATE) : ''),
      },
      {
        field: 'createDate',
        header: 'Ngày tạo',
        width: '10rem',
        type: ETypeDataTable.TEXT,
        valueFormatter: (param: IValueFormatter) => (param.data ? formatDate(param.data, ETypeFormatDate.DATE) : ''),
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
    this.selectedColumns = this.getLocalStorage(MembershipLevelManagement.keyStorage) ?? this.headerColumns;
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
          this.setLocalStorage(this.selectedColumns, MembershipLevelManagement.keyStorage);
        }
      });
    }
  }

  private genListAction() {
    this.listAction = this.dataSource.map((data: MembershipLevelManagementModel, index: number) => {
      const actions: IActionTable[] = [];

      actions.push({
        data: data,
        label: 'Thông tin chi tiết',
        icon: 'pi pi-info-circle',
        command: ($event) => {
          this.detail($event.item.data);
        },
      });

      actions.push({
        data: data,
        label: 'Chỉnh sửa',
        icon: 'pi pi-pencil',
        command: ($event) => {
          this.edit($event.item.data);
        },
      });

      data.status === MembershipLevelManagement.KICH_HOAT &&
        actions.push({
          data: data,
          label: 'Hủy kích hoạt',
          icon: 'pi pi-pencil',
          command: ($event) => {
            this.deactive($event.item.data);
          },
        });

      (data.status === MembershipLevelManagement.KHOI_TAO || data.status === MembershipLevelManagement.HUY_KICH_HOAT) &&
        actions.push({
          data: data,
          label: 'Kích hoạt ngay',
          icon: 'pi pi-pencil',
          command: ($event) => {
            this.active($event.item.data);
          },
        });

      return actions;
    });
  }

  public detail(data: MembershipLevelManagementModel) {
    if (data.id || data.id === 0) {
      this.membershipLevelManagementService.getDetail(data.id).subscribe((res: any) => {
        if (res) {
          this.dialogService.open(CrudMembershipLevelManagementComponent, {
            header: 'Thông tin tích điểm',
            width: '600px',
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

  public edit(data: MembershipLevelManagementModel) {
    if (data.id || data.id === 0) {
      this.membershipLevelManagementService.getDetail(data.id).subscribe((res: any) => {
        if (res) {
          const ref = this.dialogService.open(CrudMembershipLevelManagementComponent, {
            header: 'Chỉnh sửa',
            width: '600px',
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

  public deactive(data: MembershipLevelManagementModel) {
    if (data) {
      this._helpersService.dialogConfirmRef(
        "Xác nhận hủy kích hoạt?",
        IconConfirm.QUESTION
      ).onClose.subscribe((accept: boolean) => {
        if(accept) {
          this.membershipLevelManagementService
          .changeStatus(MembershipLevelManagement.HUY_KICH_HOAT, data.id)
          .subscribe(
            (response) => {
              if (this.handleResponseInterceptor(response, 'Hủy kích hoạt thành công')) {
                this.setPage({ page: this.offset });
              }
            },
            (err) => {
              this.messageError(`Hủy kích hoạt không thành công`);
            }
          );
        }
      })
    }
  }

  public active(data: MembershipLevelManagementModel) {
    if (data) {
      this._helpersService.dialogConfirmRef(
        "Xác nhận kích hoạt?",
        IconConfirm.APPROVE
      ).onClose.subscribe((accept: boolean) => {
        if(accept) {
          this.membershipLevelManagementService.changeStatus(MembershipLevelManagement.KICH_HOAT, data.id).subscribe(
            (response) => {
              if (this.handleResponseInterceptor(response, 'Kích hoạt thành công')) {
                this.setPage({ page: this.offset });
              }
            },
            (err) => {
              this.messageError(`Kích hoạt không thành công`);
            }
          );
        }
      })
    }
  }

  public create(event: any) {
    if (event) {
      const ref = this.dialogService.open(CrudMembershipLevelManagementComponent, {
        header: 'Thêm mới hạng thành viên',
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

    this.membershipLevelManagementService.getAll(this.page, this.filter).subscribe(
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
                  description: item.description,
                  startPoint: item.pointStart,
                  endPoint: item.pointEnd,
                  applyDate: item.activeDate,
                  createDate: item.createdDate,
                  status: item.status,
                } as MembershipLevelManagementModel)
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
