import { ChangeDetectorRef, Component, Injector } from '@angular/core';
import { Router } from '@angular/router';
import { EPositionFrozenCell, EPositionTextCell, ETypeDataTable, IndividualCustomer } from '@shared/AppConsts';
import { CrudComponentBase } from '@shared/crud-component-base';
import { IndividualCustomerModel } from '@shared/interface/customer-management/individual-customer/IndividualCustomer.model';
import {
  IActionTable,
  IConstant,
  IDropdown,
  IHeaderColumn,
  ISortTable,
} from '@shared/interface/InterfaceConst.interface';
import { Page } from '@shared/model/page';
import { IndividualCustomerService } from '@shared/services/individual-customer-service';
import { MessageService } from 'primeng/api';
import { DialogService } from 'primeng/dynamicdialog';
import { FormSetDisplayColumnComponent } from 'src/app/form-general/form-set-display-column/form-set-display-column/form-set-display-column.component';
import { BreadcrumbService } from 'src/app/layout/breadcrumb/breadcrumb.service';

@Component({
  selector: 'individual-customer',
  templateUrl: './individual-customer.component.html',
  styleUrls: ['./individual-customer.component.scss'],
})
export class IndividualCustomerComponent extends CrudComponentBase {
  constructor(
    injector: Injector,
    messageService: MessageService,
    private dialogService: DialogService,
    private breadcrumbService: BreadcrumbService,
    private router: Router,
    private changeDetectorRef: ChangeDetectorRef,
    private individualCustomerService: IndividualCustomerService
  ) {
    super(injector, messageService);
    this.breadcrumbService.setItems([
      { label: 'Trang chủ', routerLink: ['/home'] },
      { label: 'Quản lý khách hàng', routerLink: ['/home'] },
      { label: 'Khách hàng cá nhân' },
    ]);
  }
  public dataSource: IndividualCustomerModel[] = [];
  public isLoading: boolean;
  public page: Page;
  public listAction: IActionTable[][] = [];
  public selectedColumns: IHeaderColumn[] = [];
  public headerColumns: IHeaderColumn[] = [];
  public filter: {
    keyword: string;
    gender: number | undefined;
    voucherLevel: number | undefined;
    account: number | undefined;
    class: number | undefined;
  } = {
    keyword: '',
    gender: undefined,
    voucherLevel: undefined,
    account: undefined,
    class: undefined,
  };
  public sort: string;
  public listClass: IDropdown[] = [];

  public get listGender() {
    return IndividualCustomer.listGender;
  }

  public get listVoucherLevel() {
    return IndividualCustomer.listVoucherLevel;
  }

  public get listAccountType() {
    return IndividualCustomer.listAccountType;
  }

  public getStatusSeverity(code: any) {
    return IndividualCustomer.getStatus(code, 'severity');
  }

  public getStatusName(code: any) {
    return IndividualCustomer.getStatus(code, 'label');
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
          isSort: true,
          fieldSort: 'investorId',
        },
        {
          field: 'code',
          header: 'Mã khách hàng',
          width: '12rem',
          isPin: true,
          type: ETypeDataTable.TEXT,
          isSort: true,
          fieldSort: 'cifCode',
        },
        {
          field: 'name',
          header: 'Tên khách hàng',
          width: '20rem',
          isPin: true,
          type: ETypeDataTable.TEXT,
          isSort: true,
          fieldSort: 'fullname',
        },
        {
          field: 'phoneNumber',
          header: 'Số điện thoại',
          width: '10rem',
          isPin: true,
          type: ETypeDataTable.TEXT,
          isSort: true,
          fieldSort: 'phone',
        },
        {
          field: 'email',
          header: 'Email',
          width: '20rem',
          isPin: true,
          class: 'text-ellipsis',
          type: ETypeDataTable.TEXT,
          isSort: true,
          fieldSort: 'email',
        },
        {
          field: 'gender',
          header: 'Giới tính',
          width: '10rem',
          isPin: true,
          type: ETypeDataTable.TEXT,
          isSort: true,
          fieldSort: 'sex',
        },
        {
          field: 'totalPoint',
          header: 'Điểm tích lũy',
          width: '10rem',
          type: ETypeDataTable.TEXT,
          isSort: true,
          fieldSort: 'loyTotalPoint',
        },
        {
          field: 'currentPoint',
          header: 'Điểm hiện tại',
          width: '10rem',
          type: ETypeDataTable.TEXT,
          isSort: true,
          fieldSort: 'loyCurrentPoint',
        },
        {
          field: 'class',
          header: 'Hạng',
          width: '10rem',
          isPin: true,
          type: ETypeDataTable.TEXT,
          isSort: true,
          fieldSort: 'rankId',
        },
        {
          field: 'voucherNumber',
          header: 'Số voucher',
          width: '10rem',
          type: ETypeDataTable.TEXT,
          isSort: true,
          fieldSort: 'voucherNum',
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
      ] as IHeaderColumn[]
    ).map((item: IHeaderColumn, index: number) => {
      item.position = index + 1;
      return item;
    });
    this.selectedColumns = this.getLocalStorage(IndividualCustomer.keyStorage) ?? this.headerColumns;
    this.setPage({ page: this.offset });
  }

  ngAfterViewInit() {
    this.individualCustomerService.getListClass().subscribe((res: any) => {
      if (this.handleResponseInterceptor(res, '')) {
        if (res.data.items) {
          this.listClass = res.data.items.map(
            (item: any) =>
              ({
                value: item.id,
                label: item.name,
              } as IDropdown)
          );
        }
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
          this.setLocalStorage(this.selectedColumns, IndividualCustomer.keyStorage);
        }
      });
    }
  }

  private genListAction() {
    this.listAction = this.dataSource.map((data: IndividualCustomerModel, index: number) => {
      const actions: IActionTable[] = [];
      if (this.isGranted([this.PermissionLoyaltyConst.Loyalty_KhachHangCaNhan_ChiTiet])){
        actions.push({
          data: data,
          label: 'Thông tin chi tiết',
          icon: 'pi pi-info-circle',
          command: ($event) => {
            this.detail($event.item.data);
          },
        });
      }

      if (this.isGranted([this.PermissionLoyaltyConst.Loyalty_KhachHangCaNhan_CapNhat])){
        actions.push({
          data: data,
          label: 'Chỉnh sửa',
          icon: 'pi pi-pencil',
          command: ($event) => {
            this.edit($event.item.data);
          },
        });
      }

      return actions;
    });
  }

  public funcStyleClassStatus = (status: any) => {
    return this.getStatusSeverity(status);
  };

  public funcLabelStatus = (status: any) => {
    return this.getStatusName(status);
  };

  public detail(data: IndividualCustomerModel) {
    this.router.navigate(['/customer-management/individual-customer/detail/' + this.cryptEncode(data.id)]);
  }

  public edit(data: IndividualCustomerModel) {
    this.router.navigate(['/customer-management/individual-customer/edit/' + this.cryptEncode(data.id)]);
  }

  public changeFilter(event: any) {
    if (event) {
      this.setPage({ page: this.offset });
    }
  }

  public setPage(pageInfo?: any) {
    this.isLoading = true;
    this.page.pageNumber = pageInfo?.page ?? this.offset;
    if (pageInfo?.rows) this.page.pageSize = pageInfo?.rows;
    this.page.keyword = this.filter.keyword;

    this.individualCustomerService.getAllIndividualCustomer(this.page, this.filter, this.sort).subscribe(
      (res) => {
        this.isLoading = false;
        if (this.handleResponseInterceptor(res, '')) {
          this.page.totalItems = res.data.totalItems;
          if (res.data?.items) {
            this.dataSource = res.data.items.map(
              (item: any) =>
                ({
                  id: item.investorId,
                  code: item.cifCode,
                  name: item.fullname,
                  phoneNumber: item.phone,
                  email: item.email,
                  gender: IndividualCustomer.listSex.find((e: IConstant) => e.id === item.sex)?.value || '',
                  totalPoint: item.loyTotalPoint || 0,
                  currentPoint: item.loyCurrentPoint || 0,
                  class: item.rankName,
                  voucherNumber: item.voucherNum || 0,
                  status: item.isVerified,
                } as IndividualCustomerModel)
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

  public onSort(event: ISortTable) {
    if (event) {
      this.sort = event.sort;
      this.setPage();
    }
  }

  public exportExcel(event: any) {
    if (event) {
      this.isLoading = true;
      this.page.keyword = this.filter.keyword;
      this.individualCustomerService.exportExcel(this.page, this.filter).subscribe(
        (res) => {
          this.isLoading = false;
        },
        () => {
          this.isLoading = false;
          this.messageError('Có lỗi xảy ra!');
        }
      );
    }
  }
}
