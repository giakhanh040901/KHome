import { Component, Injector, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { CrudComponentBase } from '@shared/crud-component-base';
import { Page } from '@shared/model/page';
import { OrderServiceProxy } from '@shared/service-proxies/trading-contract-service';
import { ConfirmationService, MessageService } from 'primeng/api';
import { DialogService } from 'primeng/dynamicdialog';
import { BreadcrumbService } from 'src/app/layout/breadcrumb/breadcrumb.service';
import { debounceTime } from 'rxjs/operators';
import { DeliveryContractConst, OrderConst, SearchConst } from '@shared/AppConsts';
import { FormSetDisplayColumnComponent } from 'src/app/form-set-display-column/form-set-display-column/form-set-display-column.component';
import { DeliveryContractService } from '@shared/services/delivery-contract.service';
import { Subject } from 'rxjs';
import * as moment from 'moment';

@Component({
  selector: 'app-delivery-contract',
  templateUrl: './delivery-contract.component.html',
  styleUrls: ['./delivery-contract.component.scss']
})
export class DeliveryContractComponent extends CrudComponentBase {
  
  constructor(
    injector: Injector,
    messageservice: MessageService,
    private _orderService: OrderServiceProxy,
    private _deliveryContractService: DeliveryContractService,
    private breadcrumbService: BreadcrumbService,
    private confirmationService: ConfirmationService,
    private dialogService: DialogService,
    private router: Router,
  ) {
    super(injector, messageservice);
    this.breadcrumbService.setItems([
      { label: 'Trang chủ', routerLink: ['/home'] },
      { label: 'Hợp đồng phân phối' },
      { label: 'Giao nhận hợp đồng' },
    ]);
  }
  liberationDialog: boolean;
  rows: any[] = [];
  row: any;
  col: any;
  statusSearch: any[] = [];
  cols: any[];
  _selectedColumns: any[];
  deliveryStatus: any;

  submitted: boolean;
  listAction: any[] = [];
  dateFilter: any;
  dataFilter = {
    deliveryStatus: null,
		source: null,
    dateFilter: null,
    fieldFilter: null,
  };

  fieldFilters = [
    {
      name: 'Tất cả',
      field: '',
    },
    ...DeliveryContractConst.fieldFilters
  ];

  subject = {
		keyword: new Subject(),
	};

  source: any;

  type: number;
  status: any;

  page = new Page();
  offset = 0;

  order: any = {};
  // Menu otions thao tác
  DeliveryContractConst = DeliveryContractConst;

  sources: any[] = [
		{
			name: 'Tất cả',
			code: '',
		},
		...OrderConst.sources,
	];

  ngOnInit() {
    this.setPage({ page: this.offset });
    console.log("");

    this.subject.keyword.pipe(debounceTime(SearchConst.DEBOUNCE_TIME)).subscribe(() => {
      if (this.keyword === "") {
        this.setPage({ page: this.offset });
      } else {
        this.setPage();
      }
    });

    this.statusSearch = [
      {
        name: "Tất cả",
        code: ''
      },
      ...DeliveryContractConst.deliveryStatus]

    this.cols = [
      { field: 'contractCode', header: 'Số hợp đồng', width: '12rem', position: 1, isPin: true },
      { field: 'paymentFullDate', header: 'Ngày ĐT', width: '10rem', position: 1, cutText: 'b-cut-text-10', isPin: true },
      { field: 'customerType', header: 'Loại khách hàng', width: '10rem', position: 2, cutText: 'b-cut-text-10' },
      { field: 'name', header: 'Khách hàng', width: '25rem', position: 3, cutText: 'b-cut-text-25' },
      { field: 'kyHan', header: 'Kỳ hạn', width: '10rem', position: 4, cutText: 'b-cut-text-10', },
      { field: 'pendingDate', header: 'Ngày gửi yêu cầu', width: '12rem', position: 4, cutText: 'b-cut-text-12' },
      { field: 'deliveryDate', header: 'Ngày giao', width: '10rem', position: 5, cutText: 'b-cut-text-10' },
      { field: 'receivedDate', header: 'Ngày nhận', width: '10rem', position: 5, cutText: 'b-cut-text-10' },
      { field: 'finishedDate', header: 'Ngày hoàn thành', width: '12rem', position: 5, cutText: 'b-cut-text-10' },
      { field: 'online', header: 'Online', width: '5rem', position: 6, cutText: 'b-cut-text-10' },
      { field: 'offline', header: 'Offline', width: '5rem', position: 7, cutText: 'b-cut-text-10' },
    ];
    this._selectedColumns = this.getLocalStorage('deliveryContract') ?? this.cols;
  }

  getLocalStorage(key) {
    return JSON.parse(localStorage.getItem(key))
  }
  setLocalStorage(data) {
    return localStorage.setItem('deliveryContract', JSON.stringify(data));
  }

  setColumn(col, _selectedColumns) {

    const ref = this.dialogService.open(
      FormSetDisplayColumnComponent,
      this.getConfigDialogServiceDisplayTableColumn("Sửa cột hiển t ị", col, _selectedColumns)
    );

    ref.onClose.subscribe((dataCallBack) => {
      console.log('dataCallBack', dataCallBack);
      if (dataCallBack?.accept) {
        this._selectedColumns = dataCallBack.data.sort(function (a, b) {
          return a.position - b.position;
        });
        this.setLocalStorage(this._selectedColumns)
        console.log('Luu o local', this._selectedColumns);
      }
    });
  }

  handleData(rows) {

    for (let row of rows) {
      row.kyHan = row?.bondPolicyDetail?.shortName;
      row.name = row?.investor?.investorIdentification?.fullname ?? row?.businessCustomer?.name;
      row.pendingDate = this.formatDate(row.pendingDate);
      row.receivedDate = this.formatDate(row.receivedDate);
      row.finishedDate = this.formatDate(row.finishedDate);
      row.deliveryDate = this.formatDate(row.deliveryDate);
      if (row.source == 1) {
        row.online = true;
      }
      if (row.source == 2) {
        row.offline = true;
      }
      row.paymentFullDate = this.formatDate(row.paymentFullDate);
    };
  }

  changeStatus() {
    this.setPage({ Page: this.offset })
  }

  genListAction(data = []) {
    this.listAction = data.map(blockadeLiberation => {
      const actions = [];

      if (this.isGranted([this.PermissionBondConst.BondHDPP_GiaoNhanHopDong_ThongTinChiTiet])){
        actions.push({
          data: blockadeLiberation,
          label: 'Thông tin chi tiết',
          icon: 'pi pi-info-circle',
          command: ($event) => {
            this.detail($event.item.data);
          }
        })
      }
      console.log("blockadeLiberation",blockadeLiberation);

      if (this.isGranted([this.PermissionBondConst.BondHDPP_GiaoNhanHopDong_XuatHopDong])){
        actions.push({
          data: blockadeLiberation,
          label: 'Xuất hợp đồng',
          icon: 'pi pi-download',
          command: ($event) => {
            this.exportContractReceive($event.item.data);
          }
        })
      }
      
      if (blockadeLiberation.deliveryStatus == this.DeliveryContractConst.CHO_XU_LY && this.isGranted([this.PermissionBondConst.BondHDPP_GiaoNhanHopDong_DoiTrangThai])) {
        actions.push({
          data: blockadeLiberation,
          label: 'Đang giao',
          icon: 'pi pi-send',
          command: ($event) => {
            this.delivered($event.item.data);
          }
        })
      }

      if (blockadeLiberation.deliveryStatus == this.DeliveryContractConst.DANG_GIAO) {
        actions.push({
          data: blockadeLiberation,
          label: 'Đã nhận',
          icon: 'pi pi-send',
          command: ($event) => {
            this.deliveryStatusReceived($event.item.data);
          }
        })
      }

      if (blockadeLiberation.deliveryStatus == this.DeliveryContractConst.DA_NHAN) {
        actions.push({
          data: blockadeLiberation,
          label: 'Hoàn thành',
          icon: 'pi pi-send',
          command: ($event) => {
            this.deliveryStatusDone($event.item.data);
          }
        })
      }
      return actions;
    });
  }

  exportContractReceive(data) {
    this.confirmationService.confirm({
      message: 'Bạn muốn xuất hợp đồng ?',
      header: 'Xuất hợp đồng',
      icon: 'pi pi-question-circle',
      acceptLabel: 'Đồng ý',
      rejectLabel: 'Huỷ',
      accept: () => {
        this.isLoading = true;
        this._deliveryContractService.exportContractReceive(data.orderId, data.bondSecondaryId, data.tradingProviderId, data.source).subscribe((response) => {
          this.isLoading = false;
          if (this.handleResponseInterceptor(response, "Xuất thành công")) {
            // this.setPage({ page: this.offset });
          }
        }, (err) => {
          this.isLoading = false;
          console.log('Error-------', err);
          this.messageError('Có lỗi xảy ra. Vui lòng thử lại sau!', '');
        });
      },
      reject: () => {
      }
    });
  } 

  delivered(data) {
    this.confirmationService.confirm({
      message: 'Chuyển trạng thái sang đang giao?',
      header: 'Chuyển trạng thái sang đang giao',
      icon: 'pi pi-question-circle',
      acceptLabel: 'Đồng ý',
      rejectLabel: 'Huỷ',
      accept: () => {
        this._deliveryContractService.deliveryStatusDelivered(data.orderId).subscribe((response) => {
          if (this.handleResponseInterceptor(response, "Chuyển giao thành công")) {
            this.setPage({ page: this.offset });
          }
        });
      },
      reject: () => {
      }
    });
  }

  deliveryStatusReceived(data) {
    this.confirmationService.confirm({
      message: 'Chuyển trạng thái sang đã nhận?',
      header: 'Chuyển trạng thái sang đã nhận',
      icon: 'pi pi-question-circle',
      acceptLabel: 'Đồng ý',
      rejectLabel: 'Huỷ',
      accept: () => {
        this._deliveryContractService.deliveryStatusReceived(data.orderId).subscribe((response) => {
          if (this.handleResponseInterceptor(response, "Chuyển đã nhận thành công")) {
            this.setPage({ page: this.offset });
          }
        });
      },
      reject: () => {
      }
    });
  }

  deliveryStatusDone(data) {
    this.confirmationService.confirm({
      message: 'Chuyển trạng thái sang hoàn thành?',
      header: 'Chuyển trạng thái sang hoàn thành',
      icon: 'pi pi-question-circle',
      acceptLabel: 'Đồng ý',
      rejectLabel: 'Huỷ',
      accept: () => {
        this._deliveryContractService.deliveryStatusDone(data.orderId).subscribe((response) => {
          if (this.handleResponseInterceptor(response, "Chuyển hoàn thành thành công")) {
            this.setPage({ page: this.offset });
          }
        });
      },
      reject: () => {
      }
    });
  }

  detail(order) {
    this.router.navigate(['/trading-contract/delivery-contract/detail/' + this.cryptEncode(order?.orderId)]);
  }

  hideDialog() {
    this.liberationDialog = false;
  }

  changeDateFilter() {
    if (this.dateFilter) {
      this.setPage();
    }
	}

  setPage(pageInfo?: any) {
    this.page.pageNumber = pageInfo?.page ?? this.offset;
    if (pageInfo?.rows) this.page.pageSize = pageInfo?.rows;
    this.page.keyword = this.keyword;
    if (this.dateFilter) this.dataFilter.dateFilter = moment(this.dateFilter).format('YYYY-MM-DD');
    this.isLoading = true;
    this._orderService.getAllDeliveryContract(this.page, this.dataFilter).subscribe((res) => {
      this.isLoading = false;
      if (this.handleResponseInterceptor(res, '')) {
        this.page.totalItems = res.data.totalItems;
        this.rows = res.data?.items;
        if (this.rows?.length) {
          this.genListAction(this.rows);
          this.handleData(this.rows);
        }
        console.log({ rowsOrder: res.data.items, totalItems: res.data.totalItems });
      }
    }, (err) => {
      this.isLoading = false;
      console.log('Error-------', err);
      
    });
  }
}
