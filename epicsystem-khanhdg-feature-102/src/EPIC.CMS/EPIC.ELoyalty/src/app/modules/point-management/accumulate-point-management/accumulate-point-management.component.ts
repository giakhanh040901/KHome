import { ChangeDetectorRef, Component, Injector } from '@angular/core';
import { Router } from '@angular/router';
import {
  AccumulatePointManegement,
  EConfigDataModal,
  EPositionFrozenCell,
  EPositionTextCell,
  ETypeDataTable,
  ETypeFormatDate,
  SearchConst,
  TYPE_UPLOAD,
} from '@shared/AppConsts';
import { CrudComponentBase } from '@shared/crud-component-base';
import {
  IActionTable,
  IConfigDataModal,
  IDropdown,
  IHeaderColumn,
  INotiDataModal,
  IValueFormatter,
} from '@shared/interface/InterfaceConst.interface';
import { AccumulatePointManegementModel } from '@shared/interface/point-management/accumulate-point-management/AccumulatePointManegement.model';
import { Page } from '@shared/model/page';
import { AccumulatePointManagementService } from '@shared/services/accumulate-point-management-service';
import { MessageService } from 'primeng/api';
import { DialogService } from 'primeng/dynamicdialog';
import { debounceTime } from 'rxjs/operators';
import { FormSetDisplayColumnComponent } from 'src/app/form-general/form-set-display-column/form-set-display-column/form-set-display-column.component';
import { BreadcrumbService } from 'src/app/layout/breadcrumb/breadcrumb.service';
import { CrudAccumulatePointManagementComponent } from './crud-accumulate-point-management/crud-accumulate-point-management.component';
import { formatDate } from '@shared/function-common';
import { HelpersService } from '@shared/services/helpers.service';
import { IconConfirm } from '@shared/consts/base.const';

@Component({
  selector: 'accumulate-point-management',
  templateUrl: './accumulate-point-management.component.html',
  styleUrls: ['./accumulate-point-management.component.scss'],
})
export class AccumulatePointManagementComponent extends CrudComponentBase {
  constructor(
    injector: Injector,
    messageService: MessageService,
    private dialogService: DialogService,
    private breadcrumbService: BreadcrumbService,
    private router: Router,
    private changeDetectorRef: ChangeDetectorRef,
    private accumulatePointManagementService: AccumulatePointManagementService,
    private _helpersService: HelpersService,
  ) {
    super(injector, messageService);
    this.breadcrumbService.setItems([
      { label: 'Trang chủ', routerLink: ['/home'] },
      { label: 'Quản lý điểm', routerLink: ['/home'] },
      { label: 'Quản lý tích điểm' },
    ]);
  }

  public dataSource: AccumulatePointManegementModel[] = [];
  public isLoading: boolean;
  public page: Page;
  public listAction: IActionTable[][] = [];
  public selectedColumns: IHeaderColumn[] = [];
  public headerColumns: IHeaderColumn[] = [];
  public filter: {
    keyword: string;
    type: number | undefined;
  } = {
    keyword: '',
    type: undefined,
  };
  public baseUrl: string = '';

  public get listType() {
    return AccumulatePointManegement.listType;
  }

  public get TYPE_UPLOAD() {
    return TYPE_UPLOAD;
  }

  public get listStatus() {
    return AccumulatePointManegement.listStatus;
  }

  public getStatusSeverity(code: any) {
    return AccumulatePointManegement.getStatus(code, 'severity');
  }

  public getStatusName(code: any) {
    return AccumulatePointManegement.getStatus(code, 'label');
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
        field: 'customerCode',
        header: 'Mã khách hàng',
        width: '15rem',
        isPin: true,
        type: ETypeDataTable.TEXT,
      },
      {
        field: 'customerName',
        header: 'Khách hàng',
        width: '25rem',
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
        field: 'point',
        header: 'Số điểm',
        width: '10rem',
        isPin: true,
        type: ETypeDataTable.TEXT,
      },
      {
        field: 'type',
        header: 'Loại hình',
        width: '10rem',
        isPin: true,
        type: ETypeDataTable.TEXT,
        valueFormatter: (param: IValueFormatter) =>
          this.listType.find((e: IDropdown) => e.value === param.data)?.label || '',
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
        width: '15rem',
        type: ETypeDataTable.TEXT,
        valueFormatter: (param: IValueFormatter) => (param.data ? formatDate(param.data, ETypeFormatDate.DATE) : ''),
      },
      {
        field: 'createUser',
        header: 'Người tạo',
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
    this.selectedColumns = this.getLocalStorage(AccumulatePointManegement.keyStorage) ?? this.headerColumns;
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
          this.setLocalStorage(this.selectedColumns, AccumulatePointManegement.keyStorage);
        }
      });
    }
  }

  private genListAction() {
    this.listAction = this.dataSource.map((data: AccumulatePointManegementModel, index: number) => {
      const actions: IActionTable[] = [];

      if (this.isGranted([this.PermissionLoyaltyConst.LoyaltyMenu_TichDiem_ChiTiet])){
        actions.push({
          data: data,
          label: 'Thông tin chi tiết',
          icon: 'pi pi-info-circle',
          command: ($event) => {
            this.detail($event.item.data);
          },
        });
      }

      if (this.isGranted([this.PermissionLoyaltyConst.LoyaltyMenu_TichDiem_ChinhSua])){
        actions.push({
          data: data,
          label: 'Chỉnh sửa',
          icon: 'pi pi-pencil',
          command: ($event) => {
            this.edit($event.item.data);
          },
        });
  
      }

      if (this.isGranted([this.PermissionLoyaltyConst.LoyaltyMenu_TichDiem_Huy]) && data.status !== AccumulatePointManegement.HUY_YEU_CAU) {
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

  public detail(data: AccumulatePointManegementModel) {
    if (data.id) {
      this.accumulatePointManagementService.getDetail(data.id).subscribe((res: any) => {
        if (res) {
          this.dialogService.open(CrudAccumulatePointManagementComponent, {
            header: 'Thông tin tích điểm',
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

  public edit(data: AccumulatePointManegementModel) {
    if (data.id) {
      this.accumulatePointManagementService.getDetail(data.id).subscribe((res: any) => {
        if (res) {
          const ref = this.dialogService.open(CrudAccumulatePointManagementComponent, {
            header: 'Chỉnh sửa tích điểm',
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

  public create(event: any) {
    if (event) {
      const ref = this.dialogService.open(CrudAccumulatePointManagementComponent, {
        header: 'Thông tin điểm',
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

  public cancel(data: AccumulatePointManegementModel) {
    if (data) {
      this._helpersService.dialogConfirmRef(
        "Xác nhận hủy yêu cầu Tích điểm/Yêu điểm?",
        IconConfirm.QUESTION
      ).onClose.subscribe((accept: boolean) => {
        if(accept) {
          this.accumulatePointManagementService.cancelRequest(data.id).subscribe(
            (response) => {
              if (this.handleResponseInterceptor(response, 'Hủy yêu cầu thành công')) {
                this.setPage({ page: this.offset });
              }
            },
            (err) => {
              this.messageError(`Hủy yêu cầu không thành công`);
            }
          );
        }
      })
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

    this.accumulatePointManagementService.getAll(this.page, this.filter).subscribe(
      (res) => {
        this.isLoading = false;
        if (this.handleResponseInterceptor(res, '')) {
          this.page.totalItems = res.data.totalItems;
          if (res.data?.items) {
            this.dataSource = res.data.items.map(
              (item: any) =>
                ({
                  id: item.id,
                  customerCode: item.cifCode,
                  customerName: item.fullname,
                  numberPhone: item.phone,
                  point: item.point,
                  type: item.pointType,
                  applyDate: item.applyDate,
                  createDate: item.createdDate,
                  createUser: item.createdBy,
                  status: item.status,
                } as AccumulatePointManegementModel)
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

  public import(event: any) {
    if (event) {
      this.accumulatePointManagementService.importPoint(event?.files[0]).subscribe((data) => {
        if (this.handleResponseInterceptor(data, '')) {
          this.messageService.add({
            severity: 'success',
            summary: '',
            detail: 'Tải lên thành công',
            life: 1500,
          });
          this.setPage({ page: this.offset });
        }
      });
    }
  }
}
