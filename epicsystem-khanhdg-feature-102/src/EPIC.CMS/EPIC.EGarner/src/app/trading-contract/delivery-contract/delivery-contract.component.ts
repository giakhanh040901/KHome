import { Component, Injector, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { DeliveryContractConst, OrderConst, SearchConst } from '@shared/AppConsts';
import { CrudComponentBase } from '@shared/crud-component-base';
import { Page } from '@shared/model/page';
import { OrderServiceProxy } from '@shared/service-proxies/trading-contract-service';
import { DeliveryContractService } from '@shared/services/delivery-contract.service';
import { ConfirmationService, MessageService } from 'primeng/api';
import { DialogService } from 'primeng/dynamicdialog';
import { BreadcrumbService } from 'src/app/layout/breadcrumb/breadcrumb.service';
import { debounceTime } from 'rxjs/operators';
import { FormSetDisplayColumnComponent } from 'src/app/form-general/form-set-display-column/form-set-display-column/form-set-display-column.component';
import { TradingProviderSelectedService } from '@shared/services/trading-provider.service';
import { Subscription } from 'rxjs';

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
		private _tradingProviderSelectedService: TradingProviderSelectedService,
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
  statusSearch:any [] = [];
  cols: any[];
  _selectedColumns: any[];
  deliveryStatus:any;
  source: any;

  submitted: boolean;
  listAction: any[] = [];
  dataFilter: any[] = [];

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

  //
  tradingProviderIds:[];
	tradingProviderSub: Subscription;

  ngOnInit() {
    this.isPartner = this.getIsPartner();
    this._tradingProviderSelectedService.TradingProviderObservable.subscribe((change) => {
			this.tradingProviderIds = change;
			this.setPage();
		})

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
    this.subject.isSetPage.subscribe(() => {
      this.setPage(this.getPageCurrentInfo());
    });
    this.cols = [
      { field: 'contractCode', isSort: true, header: 'Mã HĐ', width: '9rem', isPin: true },
      { field: 'paymentFullDate', isSort: true, header: 'Ngày ĐT', width: '10rem', isPin: true },
      { field: 'nameDisplay', header: 'Khách hàng', width: '20rem' },
      { field: 'pendingDate', isSort: true, header: 'Ngày gửi y/c', width: '10rem' },
      { field: 'deliveryDate', isSort: true, header: 'Ngày giao', width: '9rem' },
      { field: 'receivedDate', isSort: true, header: 'Ngày nhận', width: '9rem' },
      { field: 'finishedDate', isSort: true, header: 'Ngày hoàn thành', width: '12rem' },
      { field: 'online', header: 'Online', width: '5rem' },
      { field: 'offline', header: 'Offline', width: '5rem' },
      { field: 'columnResize', header: '', type:'hidden' },
    ];
    //
    this.cols = this.cols.map((item, index) => {
      item.position = index + 1;
      return item;
    });
    //
    this._selectedColumns = this.getLocalStorage('deliveryContractGan') ?? this.cols;
  }

  ngOnDestroy(): void {
		if(this.tradingProviderSub) (<Subscription>this.tradingProviderSub).unsubscribe();
	}

  setColumn(col, selectedColumns) {
    const ref = this.dialogService.open(FormSetDisplayColumnComponent,this.getConfigDialogServiceDisplayTableColumn(col, selectedColumns));
    ref.onClose.subscribe((dataCallBack) => {
      if (dataCallBack?.accept) {
        this._selectedColumns = dataCallBack.data.sort(function (a, b) {
          return a.position - b.position;
        });
        this.setLocalStorage(this._selectedColumns, 'deliveryContractGan')
      }
    });
  }

  handleData(rows) {

    for (let row of rows) {
      row.kyHan = row.policyDetail?.name;
      row.nameDisplay = (row?.investor ? row.investor?.investorIdentification?.fullname : null) || (row?.businessCustomer ? row?.businessCustomer?.name : null); 
      row.pendingDate = this.formatDate(row.pendingDate);
      row.receivedDate = this.formatDate(row.receivedDate);
      row.finishedDate = this.formatDate(row.finishedDate);
      row.deliveryDate = this.formatDate(row.deliveryDate);

      row.paymentFullDate = this.formatDate(row.paymentFullDate);
      row.buyDate = this.formatDate(row?.buyDate);
      row.investDate = this.formatDate(row?.investDate);
      row.endDate = this.formatDate(row?.endDate);
      
      if (row.source == 1) {
        row.online = true;
      }
      if (row.source == 2) {
        row.offline = true;
      }
    };
  }

  changeStatus() {
    this.setPage({ Page: this.offset })
  }

  genListAction(data = []) {
    this.listAction = data.map(blockadeLiberation => {
      const actions = [];

      if (this.isGranted([this.PermissionGarnerConst.GarnerHDPP_GiaoNhanHopDong_ThongTinChiTiet])) {
        actions.push({
          data: blockadeLiberation,
          label: 'Thông tin chi tiết',
          icon: 'pi pi-info-circle',
          command: ($event) => {
            this.detail($event.item.data);
          }
        })
      }

      if (!this.isPartner && this.isGranted([this.PermissionGarnerConst.GarnerHDPP_GiaoNhanHopDong_XuatHopDong])) {
        actions.push({
          data: blockadeLiberation,
          label: 'Xuất hợp đồng',
          icon: 'pi pi-download',
          command: ($event) => {
            this.exportContractReceive($event.item.data);
          }
        })
      }

      if (!this.isPartner && blockadeLiberation.deliveryStatus == this.DeliveryContractConst.CHO_XU_LY) {
        actions.push({
          data: blockadeLiberation,
          label: 'Đang giao',
          icon: 'pi pi-send',
          command: ($event) => {
            this.delivered($event.item.data);
          }
        })
      }
      if (!this.isPartner && blockadeLiberation.deliveryStatus == this.DeliveryContractConst.DANG_GIAO) {
        actions.push({
          data: blockadeLiberation,
          label: 'Đã nhận',
          icon: 'pi pi-send',
          command: ($event) => {
            this.deliveryStatusReceived($event.item.data);
          }
        })
      }

      if (!this.isPartner && blockadeLiberation.deliveryStatus == this.DeliveryContractConst.DA_NHAN) {
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
        this._deliveryContractService.exportContractReceive(data.id).subscribe((response) => {
          this.isLoading = false;
          this.handleResponseInterceptor(response)
        }, (err) => {
          this.isLoading = false;
          this.messageError('Có lỗi xảy ra. Vui lòng thử lại sau!');
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
        this._deliveryContractService.deliveryStatusDelivered(data.id).subscribe((response) => {
          if (this.handleResponseInterceptor(response, "Chuyển giao thành công")) {
            this.setPage(this.getPageCurrentInfo());
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
        this._deliveryContractService.deliveryStatusReceived(data.id).subscribe((response) => {
          if (this.handleResponseInterceptor(response, "Chuyển đã nhận thành công")) {
            this.setPage(this.getPageCurrentInfo());
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
        this._deliveryContractService.deliveryStatusDone(data.id).subscribe((response) => {
          if (this.handleResponseInterceptor(response, "Chuyển hoàn thành thành công")) {
            this.setPage(this.getPageCurrentInfo());
          }
        });
      },
      reject: () => {
      }
    });
  }

  detail(order) {
    let cryptEncodeId = encodeURIComponent(this.cryptEncode(order?.id));  
		window.open('/trading-contract/order/justview/' + (cryptEncodeId) + '/'+ (OrderConst.VIEW_GIAO_NHAN_HOP_DONG), "_blank");
  }

  

  hideDialog() {
    this.liberationDialog = false;
  }

  getPageCurrentInfo() {
		return {page: this.page.pageNumber, rows: this.page.pageSize};
	}

  setPage(pageInfo?: any) {
    this.page.pageNumber = pageInfo?.page ?? this.offset;
    if (pageInfo?.rows) this.page.pageSize = pageInfo?.rows;
    this.page.keyword = this.keyword;
    this.isLoading = true;
    this._orderService.getAllDeliveryContract(this.page, this.deliveryStatus, this.source, this.tradingProviderIds, this.sortData).subscribe((res) => {
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
    });
  }

}
