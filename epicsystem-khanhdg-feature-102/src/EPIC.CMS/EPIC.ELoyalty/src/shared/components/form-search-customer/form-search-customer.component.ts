import { ChangeDetectorRef, Component, EventEmitter, Injector, Input, Output, SimpleChanges } from '@angular/core';
import {
  EConfigDataModal,
  ETypeDataTable,
  FormNotificationConst,
  SearchConst,
  VoucherManagement,
} from '@shared/AppConsts';
import { IconConfirm } from '@shared/consts/base.const';
import { CrudComponentBase } from '@shared/crud-component-base';
import {
  IActionButtonTable,
  IHeaderColumn,
  INotiDataModal,
  ITabView,
} from '@shared/interface/InterfaceConst.interface';
import { CustomerSearchModel } from '@shared/interface/voucher-management/voucher-management/VoucherManagement.model';
import { GetCustomerService } from '@shared/services/get-customer-service';
import { HelpersService } from '@shared/services/helpers.service';
import { MessageService } from 'primeng/api';
import { DialogService } from 'primeng/dynamicdialog';
import { debounceTime } from 'rxjs/operators';

@Component({
  selector: 'form-search-customer',
  templateUrl: './form-search-customer.component.html',
  styleUrls: ['./form-search-customer.component.scss'],
})
export class FormSearchCustomerComponent extends CrudComponentBase {
  @Input()
  public classContainer: string = '';
  @Input()
  public showTitle: boolean = true;
  @Input()
  public classTitle: string = 'font-semibold text-xl';
  @Input()
  public title: string = '';
  @Input()
  public isDisabled: boolean = false;
  @Input()
  public initCusomter:
    | {
        type: number;
        customerId: number;
      }
    | undefined = undefined;
  @Input()
  public typeModal: EConfigDataModal = EConfigDataModal.CREATE;
  @Input()
  public showHeader: boolean = true;
  @Input()
  public headerIndividual: IHeaderColumn[];
  @Input()
  public minusPoint: number | undefined = undefined;
  @Output()
  public _onChange: EventEmitter<any> = new EventEmitter<any>();
  @Output()
  public _onChangeCurrentPoint: EventEmitter<number | undefined> = new EventEmitter<number | undefined>();

  public listTabPanel: ITabView[] = [
    {
      key: 'customer',
      title: 'Khách hàng cá nhân',
      component: null,
      isDisabled: this.isDisabled,
    },
    {
      key: 'bussiness',
      title: 'Khách hàng doanh nghiệp',
      component: null,
      isDisabled: true,
    },
  ];
  public isLoading: boolean;
  public dataSource: CustomerSearchModel[] = [];
  public selectedColumns: IHeaderColumn[] = [];
  public headerColumns: any[] = [];
  public activeIndex: number = this.CUSTOMER.INDIVIDUAL;
  public keyword: string = '';
  public selectedUser: CustomerSearchModel = new CustomerSearchModel();
  public listButtonTable: IActionButtonTable[] = [];

  constructor(
    injector: Injector,
    messageService: MessageService,
    private dialogService: DialogService,
    private changeDetectorRef: ChangeDetectorRef,
    private getCustomerService: GetCustomerService,
    private _helpersService: HelpersService,
  ) {
    super(injector, messageService);
    this.headerColumns = [
      {
        key: this.CUSTOMER.INDIVIDUAL,
        value: [
          { field: 'name', header: 'Tên', type: ETypeDataTable.TEXT, width: 'auto' },
          { field: 'numberPhone', header: 'Số điện thoại', type: ETypeDataTable.TEXT, width: '10rem' },
          { field: 'idNo', header: 'Số giấy tờ', type: ETypeDataTable.TEXT, width: 'auto' },
          { field: 'email', header: 'Email', type: ETypeDataTable.TEXT, width: 'auto' },
          { field: 'action', header: '', type: ETypeDataTable.ACTION_BUTTON, width: '10rem' },
        ] as IHeaderColumn[],
      },
      {
        key: this.CUSTOMER.BUSINESS,
        value: [
          { field: 'name', header: 'Tên', type: ETypeDataTable.TEXT, width: 'auto' },
          { field: 'abbreviation', header: 'Tên viết tắt', type: ETypeDataTable.TEXT, width: '10rem' },
          { field: 'taxCode', header: 'Mã số thuế', type: ETypeDataTable.TEXT, width: 'auto' },
          { field: 'email', header: 'Email', type: ETypeDataTable.TEXT, width: 'auto' },
          { field: 'action', header: '', type: ETypeDataTable.ACTION_BUTTON, width: '10rem' },
        ] as IHeaderColumn[],
      },
    ];
  }

  public get CUSTOMER() {
    return VoucherManagement.CUSTOMER;
  }

  ngOnInit() {
    this.listButtonTable = [
      {
        label: 'Chọn',
        icon: 'pi pi-check',
        showFunction: this.showBtnSelect,
        callBack: this.select,
        styleClassButton: 'p-button',
      },
      {
        label: '',
        icon: 'pi pi-times',
        showFunction: this.showBtnRemove,
        callBack: this.remove,
        styleClassButton: 'p-button p-button-danger',
      },
    ];
  }

  ngAfterViewInit() {
    this.subject.keyword.pipe(debounceTime(SearchConst.DEBOUNCE_TIME)).subscribe(() => {
      if (this.keyword.trim()) {
        this.getCustomer();
      } else {
        this.dataSource = [];
      }
    });
  }

  ngOnChanges(changes: SimpleChanges) {
    if (this.typeModal) {
      this.typeModal === EConfigDataModal.CREATE && (this.activeIndex = this.CUSTOMER.INDIVIDUAL);
    }
    if (this.headerIndividual && this.headerIndividual.length) {
      this.headerColumns[0].value = this.headerIndividual;
    }
    if (this.initCusomter && this.initCusomter.customerId) {
      if (this.initCusomter.type === this.CUSTOMER.INDIVIDUAL) {
        if (!this.isChangeVoucherRequest) {
          this.getCustomerService.getCustomerDetail(this.initCusomter.customerId).subscribe((res) => {
            if (res.data) {
              this.activeIndex = this.CUSTOMER.INDIVIDUAL;
              this.dataSource = [
                {
                  id: res.data?.defaultIdentification?.investorId,
                  name: res.data?.defaultIdentification?.fullname || '',
                  numberPhone: res.data.phone,
                  idNo: res.data?.defaultIdentification?.idNo,
                  email: res.data.email,
                  isSelected: true,
                } as CustomerSearchModel,
              ];
              this.selectedUser = {
                id: res.data?.defaultIdentification?.investorId,
                name: res.data?.defaultIdentification?.fullname || '',
                numberPhone: res.data.phone,
                idNo: res.data?.defaultIdentification?.idNo,
                email: res.data.email,
                isSelected: true,
              } as CustomerSearchModel;
              this.keyword = res.data.phone;
              this.getCustomer();
              this.handleChange();
            }
          });
        } else {
          this.getCustomerService.getCustomerForVoucher(this.initCusomter.customerId).subscribe((res) => {
            if (res.data && res.data.length) {
              this.activeIndex = this.CUSTOMER.INDIVIDUAL;
              const data = res.data[0];
              this.dataSource = [
                {
                  id: data.investorId,
                  name: data.fullname,
                  numberPhone: data.phone,
                  email: data.email,
                  address: data.address,
                  membershipClass: data.rankName,
                  totalPoint: data.loyTotalPoint,
                  currentPoint: data.loyCurrentPoint,
                  currentPointDisplay: data.loyCurrentPoint,
                  isSelected: true,
                } as CustomerSearchModel,
              ];
              this.selectedUser = {
                id: data.investorId,
                name: data.fullname,
                numberPhone: data.phone,
                email: data.email,
                address: data.address,
                membershipClass: data.rankName,
                totalPoint: data.loyTotalPoint,
                currentPoint: data.loyCurrentPoint,
                currentPointDisplay: data.loyCurrentPoint,
                isSelected: true,
              } as CustomerSearchModel;
              this.keyword = data.phone;
              this.getCustomer();
              this.handleChange();
            }
          });
        }
      }
    }
    if (this.minusPoint || this.minusPoint === 0) {
      if (this.dataSource.length) {
        this.dataSource[0].currentPointDisplay = this.dataSource[0].currentPoint - this.minusPoint;
        this.changeCurrentPoint();
      }
    }
    this.getCustomer();
    this.changeDetectorRef.detectChanges();
    this.changeDetectorRef.markForCheck();
  }

  public showBtnSelect = (data: any, action: IActionButtonTable, index: number) => {
    return !this.isDisabled && !data.isSelected;
  };

  public showBtnRemove = (data: any, action: IActionButtonTable, index: number) => {
    return !this.isDisabled && data.isSelected;
  };

  public select = (row: any, index: number) => {
    if (row) {
      this.selectedUser = { ...row, isSelected: true };
      this.dataSource = this.dataSource.map((e: CustomerSearchModel) => {
        if (e.id === row.id) e.isSelected = true;
        return e;
      });
      this.handleChange();
    }
  };

  public remove = (row: any, index: number) => {
    if (row && !this.isDisabled) {
      this._helpersService.dialogConfirmRef(
        "Xác nhận bỏ chọn khách hàng?",
        IconConfirm.DELETE
      ).onClose.subscribe((accept: boolean) => {
        if(accept) {
          this.dataSource = this.dataSource.filter((e: CustomerSearchModel) => e.id !== row.id);
          this.selectedUser = new CustomerSearchModel();
          !this.dataSource.length && (this.keyword = '');
          this.handleChange();
        }
      })
    }
  };

  public onClickSearch(event: any) {
    if (event && !this.isDisabled) {
      this.getCustomer();
    }
  }

  public getCustomer() {
    this.selectedColumns = this.headerColumns.find((e: any) => e.key === this.activeIndex)?.value || [];
    this.keyword = this.keyword.trim();
    this.dataSource = [];
    if (this.keyword.length) {
      this.isLoading = true;
      if (!this.isChangeVoucherRequest) {
        this.getCustomerService.getCustomer(this.keyword, !!this.activeIndex).subscribe(
          (res) => {
            if (this.handleResponseInterceptor(res, '')) {
              if (this.activeIndex === this.CUSTOMER.INDIVIDUAL) {
                this.dataSource = res?.data?.items.map(
                  (e: any) =>
                    ({
                      id: e?.defaultIdentification?.investorId,
                      name: e?.defaultIdentification?.fullname || '',
                      numberPhone: e.phone,
                      idNo: e?.defaultIdentification?.idNo,
                      email: e.email,
                      isSelected: this.selectedUser.id === e?.defaultIdentification?.investorId,
                    } as CustomerSearchModel)
                );
              } else if (this.activeIndex === this.CUSTOMER.BUSINESS) {
                this.dataSource = res?.data?.items.map(
                  (e: any) =>
                    ({
                      id: e.id,
                      name: e.name,
                      abbreviation: e.shortName,
                      taxCode: e.taxCode,
                      numberPhone: e.phone,
                      email: e.email,
                    } as CustomerSearchModel)
                );
              }
              if (!this.dataSource.length)
                this.messageService.add({
                  severity: 'error',
                  summary: '',
                  detail: 'Không tìm thấy dữ liệu',
                  life: 1200,
                });
            }
            this.isLoading = false;
          },
          (err) => {
            this.isLoading = false;
          }
        );
      } else {
        this.getCustomerService.getCustomerForVoucher(undefined, this.keyword).subscribe(
          (res) => {
            if (this.handleResponseInterceptor(res, '')) {
              if (this.activeIndex === this.CUSTOMER.INDIVIDUAL) {
                this.dataSource = res?.data.map(
                  (e: any) =>
                    ({
                      id: e.investorId,
                      name: e.fullname || '',
                      numberPhone: e.phone,
                      email: e.email,
                      address: e.address,
                      membershipClass: e.rankName,
                      totalPoint: e.loyTotalPoint,
                      currentPoint: e.loyCurrentPoint,
                      currentPointDisplay: e.loyCurrentPoint - (this.minusPoint || 0),
                      isSelected: this.selectedUser.id === e.investorId,
                    } as CustomerSearchModel)
                );
                this.changeCurrentPoint();
              }
              if (!this.dataSource.length)
                this.messageService.add({
                  severity: 'error',
                  summary: '',
                  detail: 'Không tìm thấy dữ liệu',
                  life: 1200,
                });
            }
            this.isLoading = false;
          },
          (err) => {
            this.isLoading = false;
          }
        );
      }
    }
  }

  private handleChange() {
    const customerId = this.selectedUser?.id;
    let result: {
      individualId: number | undefined;
      businessId: number | undefined;
    } = {
      individualId: undefined,
      businessId: undefined,
    };
    if (this.activeIndex === this.CUSTOMER.INDIVIDUAL) {
      result.individualId = customerId;
      result.businessId = undefined;
    } else if (this.activeIndex === this.CUSTOMER.BUSINESS) {
      result.individualId = undefined;
      result.businessId = customerId;
    }
    this._onChange.emit(result);
  }

  public get isChangeVoucherRequest() {
    return this.headerIndividual && this.headerIndividual.length;
  }

  public changeCurrentPoint() {
    if (this.dataSource.length) {
      this._onChangeCurrentPoint.emit(this.dataSource[0].currentPoint);
    }
  }
}
