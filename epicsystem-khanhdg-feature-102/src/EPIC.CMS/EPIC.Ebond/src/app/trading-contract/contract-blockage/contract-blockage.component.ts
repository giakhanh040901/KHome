import { Component, Injector, OnInit } from '@angular/core';
import { CrudComponentBase } from '@shared/crud-component-base';
import { Page } from '@shared/model/page';
import { ProductBondInfoServiceProxy } from '@shared/service-proxies/bond-manager-service';
import { ConfirmationService, MessageService } from 'primeng/api';
import { BreadcrumbService } from 'src/app/layout/breadcrumb/breadcrumb.service';
import { Router } from '@angular/router';
import { OrderServiceProxy } from '@shared/service-proxies/trading-contract-service';
import { BlockageLiberationConst, OrderConst, SearchConst } from '@shared/AppConsts';
import { DialogService } from 'primeng/dynamicdialog';
import { FormSetDisplayColumnComponent } from 'src/app/form-set-display-column/form-set-display-column/form-set-display-column.component';
import { debounceTime } from 'rxjs/operators';
import { BlockageLiberationService } from '@shared/services/blockade-liberation.service';
import { ContractBlockageDetailComponent } from './contract-blockage-detail/contract-blockage-detail.component';

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
  BlockageLiberationConst = BlockageLiberationConst;
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
  ngOnInit() {
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
    this.cols = [
      { field: 'contractCode', header: 'Số hợp đồng', width: '10rem', position: 1, cutText: 'b-cut-text-10', isPin: true },
      { field: 'blockadeDate', header: 'Ngày phong tỏa', width: '15rem', position: 1, cutText: 'b-cut-text-10', isPin: true },
      { field: 'blockader', header: 'Người phong tỏa', width: '15rem', position: 2, cutText: 'b-cut-text-10' },
      { field: 'blockadeDescription', header: 'Nội dung phong tỏa', width: '15rem', position: 3, cutText: 'b-cut-text-10' },
      { field: 'liberationDate', header: 'Ngày giải tỏa', width: '15rem', position: 4, cutText: 'b-cut-text-10' },
      { field: 'liberator', header: 'Người giải tỏa', width: '15rem', position: 5, cutText: 'b-cut-text-10' },
      { field: 'liberationDescription', header: 'Nội dung giải tỏa', width: '15rem', position: 6, cutText: 'b-cut-text-10' },
    ];
    this._selectedColumns = this.getLocalStorage('contractBlockage') ?? this.cols;
  }

  getLocalStorage(key) {
    return JSON.parse(localStorage.getItem(key))
  }
  setLocalStorage(data) {
    return localStorage.setItem('contractBlockage', JSON.stringify(data));
  }

  setColumn(col, _selectedColumns) {
    console.log('cols:', col);

    console.log('_selectedColumns', _selectedColumns);

    const ref = this.dialogService.open(
      FormSetDisplayColumnComponent,
      this.getConfigDialogServiceDisplayTableColumn("Sửa cột hiển thị", col, _selectedColumns)
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

      if (this.isGranted([this.PermissionBondConst.BondHDPP_PTGT_XemChiTiet])){
        actions.push({
          data: blockadeLiberation,
          label: 'Thông tin chi tiết',
          icon: 'pi pi-info-circle',
          command: ($event) => {
            this.detail($event.item.data);
          }
        })
      }

      if (blockadeLiberation.status == this.BlockageLiberationConst.PHONG_TOA && this.isGranted([this.PermissionBondConst.BondHDPP_PTGT_GiaiToaHopDong])) {
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
        let body = this.formatCalendar(['liberationDate'], {...this.blockageLiberation})
        this.__blockageLiberationService.liberationContractActive(body, this.blockageLiberation.id).subscribe((response) => {
          if (this.handleResponseInterceptor(response, "Phong toả thành công")) {
            this.setPage();
            this.liberationDialog = false;
          }
        });
      },
      reject: () => {
      },
    });
  }

  setPage(pageInfo?: any) {
    this.page.pageNumber = pageInfo?.page ?? this.offset;
    if (pageInfo?.rows) this.page.pageSize = pageInfo?.rows;
    this.page.keyword = this.keyword;
    this.isLoading = true;
    this.__blockageLiberationService.getAll(this.page, this.status, this.type).subscribe((res) => {
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
