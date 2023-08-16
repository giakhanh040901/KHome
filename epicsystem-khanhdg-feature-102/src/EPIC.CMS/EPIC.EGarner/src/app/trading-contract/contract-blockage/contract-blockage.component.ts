import { Component, Injector, OnInit } from '@angular/core';
import { CrudComponentBase } from '@shared/crud-component-base';
import { Page } from '@shared/model/page';

import { ConfirmationService, MessageService } from 'primeng/api';
import { BreadcrumbService } from 'src/app/layout/breadcrumb/breadcrumb.service';
import { Router } from '@angular/router';
import { OrderServiceProxy } from '@shared/service-proxies/trading-contract-service';
import { BlockageLiberationConst, OrderConst, SearchConst } from '@shared/AppConsts';
import { DialogService } from 'primeng/dynamicdialog';
import { debounceTime } from 'rxjs/operators';
import { BlockageLiberationService } from '@shared/services/blockade-liberation.service';
import { ContractBlockageDetailComponent } from './contract-blockage-detail/contract-blockage-detail.component';
import { FormSetDisplayColumnComponent } from 'src/app/form-general/form-set-display-column/form-set-display-column/form-set-display-column.component';
import { TradingProviderSelectedService } from '@shared/services/trading-provider.service';
// import { ContractBlockageDetailComponent } from './contract-blockage-detail/contract-blockage-detail.component';

@Component({
  selector: 'app-contract-blockage',
  templateUrl: './contract-blockage.component.html',
  styleUrls: ['./contract-blockage.component.scss']
})
export class ContractBlockageComponent extends CrudComponentBase {

  constructor(
    injector: Injector,
    messageService: MessageService,
    private _orderService: OrderServiceProxy,
    private breadcrumbService: BreadcrumbService,
    private __blockageLiberationService: BlockageLiberationService,
		private _tradingProviderSelectedService: TradingProviderSelectedService,
    private confirmationService: ConfirmationService,
    private dialogService: DialogService,
    private router: Router,
  ) {
    super(injector, messageService);
    this.breadcrumbService.setItems([
      { label: 'Trang chủ', routerLink: ['/home'] },
      { label: 'Hợp đồng phân phối' },
      { label: 'Phong tỏa, giải tỏa' },
    ]);
  }
  liberationDialog: boolean;
  BlockageLiberationConst = BlockageLiberationConst
  rows: any[] = [];
  row: any;
  col: any;

  cols: any[];
  _selectedColumns: any[];
  statusSearch:any[] = [];
  submitted: boolean;
  listAction: any[] = [];

  blockageLiberation: any = {
    "id": 0,
    "type": null,
    "blockadeDescription": "",
    "blockadeDate": null,
    "orderId": 0,
    "blockader": null,
    "blockadeTime": null,
    "liberationDescription": null,
    "liberationDate": null,
    "liberator": null,
    "liberationTime": null,
    "status": null,
    "contractCode": null,
    "totalValue": null
  };
  type: number;
  status: any;

  page = new Page();
  offset = 0;

  order: any = {};
  // Menu otions thao tác

  //
  tradingProviderIds:[];
  
  ngOnInit() {
    this._tradingProviderSelectedService.TradingProviderObservable.subscribe((change) => {
			this.tradingProviderIds = change;
			this.setPage();
		})


    this.statusSearch = [
    {
      name: "Tất cả",
      code: ''
    } ,
      ...BlockageLiberationConst.status]
    this.setPage({ page: this.offset });
    this.subject.keyword.pipe(debounceTime(SearchConst.DEBOUNCE_TIME)).subscribe(() => {
      if (this.keyword === "") {
        this.setPage({ page: this.offset });
      } else {
        this.setPage();
      }
    });
    this.subject.isSetPage.subscribe(() => {
			this.setPage(this.getPageCurrentInfo());
		});
    this.cols = [
      { field: 'contractCode', isSort: true, header: 'Số hợp đồng', width: '12rem', isPin: true },
      { field: 'blockadeDate', isSort: true, header: 'Ngày PT', width: '10rem', isPin: true },
      { field: 'blockader', isSort: true, header: 'Người PT', width: '12rem', cutText: 'b-cut-text-12' },
      { field: 'blockadeDescription', isSort: true, header: 'Nội dung phong tỏa', width: '15rem' },
      { field: 'liberationDate', isSort: true, header: 'Ngày GT', width: '10rem' },
      { field: 'liberator', isSort: true, header: 'Người GT', width: '12rem', cutText: 'b-cut-text-12' },
      { field: 'liberationDescription', isSort: true, header: 'Nội dung giải tỏa', width: '15rem' },
    ];
    //
    this.cols = this.cols.map((item, index) => {
      item.position = index + 1;
      return item;
    });
    //
    this._selectedColumns = this.getLocalStorage('contractBlockageGan') ?? this.cols;
  }

  setColumn(col, _selectedColumns) {
    console.log('cols:', col);

    console.log('_selectedColumns', _selectedColumns);

    const ref = this.dialogService.open(
      FormSetDisplayColumnComponent,
      this.getConfigDialogServiceDisplayTableColumn(col, _selectedColumns)
    );

    ref.onClose.subscribe((dataCallBack) => {
      console.log('dataCallBack', dataCallBack);
      if (dataCallBack?.accept) {
        this._selectedColumns = dataCallBack.data.sort(function (a, b) {
          return a.position - b.position;
        });
        this.setLocalStorage(this._selectedColumns, 'contractBlockageGan')
        console.log('Luu o local', this._selectedColumns);
      }
    });
  }

  handleDate(rows) {
    for (let row of rows) {
      row.blockadeTime = this.formatDate(row.blockadeTime);
      row.blockadeDate = this.formatDate(row.blockadeDate);
      row.blockadeDescription = row.blockadeDescription;
      row.liberationDate = this.formatDate(row.liberationDate);
      row.liberationTime = this.formatDate(row.liberationTime);
      row.liberationDescription = row.liberationDescription;
    };
  }

  changeStatus() {
    this.setPage({ Page: this.offset })
  }

  genListAction(data = []) {
    this.listAction = data.map(blockadeLiberation => {
      const actions = [];

      if (this.isGranted([this.PermissionGarnerConst.GarnerHopDong_PhongToaGiaiToa_ThongTinPhongToaGiaiToa])) {
        actions.push({
          data: blockadeLiberation,
          label: 'Thông tin chi tiết',
          icon: 'pi pi-info-circle',
          command: ($event) => {
            this.detail($event.item.data);
          }
        })
      }

      if ((blockadeLiberation.status == this.BlockageLiberationConst.PHONG_TOA) && this.isGranted([this.PermissionGarnerConst.GarnerHopDong_PhongToaGiaiToa_GiaiToaHD])) {
        actions.push({
          data: blockadeLiberation,
          label: 'Giải tỏa HĐ',
          icon: 'pi pi-key',
          command: ($event) => {
            this.liberation($event.item.data);
          }
        })
      }
      
      return actions;
    });
  }

  detail(order) { 
    console.log("row",order);
    
    let modal = this.dialogService.open(ContractBlockageDetailComponent, {
      data: {
        inputData: order
      },
      header: 'Thông tin chi tiết',
      width: '800px',
      footer: ""
    })
    modal.onClose.subscribe(result => {
    });
  }
 

  liberation(contractActive) {
    this.blockageLiberation.orderId = contractActive?.orderId;
    this.blockageLiberation.totalValue = contractActive?.totalValue;
    this.blockageLiberation.liberationDate = new Date();
    this.blockageLiberation.id = contractActive?.id;
    this.liberationDialog = true;
  }

  hideDialog() {
    this.liberationDialog = false;
  }
  
  saveLiberation() {
    this.confirmationService.confirm({
      message: 'Bạn có chắc chắn giải toả hợp đồng này?',
      header: 'Giải toả hợp đồng',
      acceptLabel: "Đồng ý",
      rejectLabel: "Hủy",
      icon: 'pi pi-exclamation-triangle',
      accept: () => {
        this.__blockageLiberationService.liberationContractActive(this.blockageLiberation, this.blockageLiberation.id).subscribe((response) => {
          if (this.handleResponseInterceptor(response, "Giải toả thành công")) {
            this.setPage(this.getPageCurrentInfo());
            this.liberationDialog = false;
          }
        });
      },
      reject: () => {
      },
    });
  }

  getPageCurrentInfo() {
		return {page: this.page.pageNumber, rows: this.page.pageSize};
	}

  setPage(pageInfo?: any) {
    this.page.pageNumber = pageInfo?.page ?? this.offset;
    if (pageInfo?.rows) this.page.pageSize = pageInfo?.rows;
    this.page.keyword = this.keyword;
    this.isLoading = true;
    this.__blockageLiberationService.getAll(this.page, this.status, this.type, this.tradingProviderIds, this.sortData).subscribe((res) => {
      this.isLoading = false;
      if (this.handleResponseInterceptor(res, '')) {
        this.page.totalItems = res.data.totalItems;
        this.rows = res.data?.items;
        if (this.rows?.length) {
          this.genListAction(this.rows);
          this.handleDate(this.rows);
        }
        console.log({ rowsOrder: res.data.items, totalItems: res.data.totalItems });
      }
    }, (err) => {
      this.isLoading = false;
      console.log('Error-------', err);
      
    });
  }

}
