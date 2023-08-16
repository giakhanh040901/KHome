import { Component, Injector, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ActiveDeactiveConst, SearchConst } from '@shared/AppConsts';
import { CrudComponentBase } from '@shared/crud-component-base';
import { Page } from '@shared/model/page';
import { GeneralDescriptionService } from '@shared/services/general-description.service';
import { ConfirmationService, MessageService } from 'primeng/api';
import { DialogService } from 'primeng/dynamicdialog';
import { debounceTime } from 'rxjs/operators';
import { FormSetDisplayColumnComponent } from 'src/app/form-general/form-set-display-column/form-set-display-column/form-set-display-column.component';
import { BreadcrumbService } from 'src/app/layout/breadcrumb/breadcrumb.service';

@Component({
  selector: 'app-general-description',
  templateUrl: './general-description.component.html',
  styleUrls: ['./general-description.component.scss']
})
export class GeneralDescriptionComponent extends CrudComponentBase {

  constructor(
    injector: Injector,
    messageService: MessageService,
    private router: Router,
    private breadcrumbService: BreadcrumbService,
    private dialogService: DialogService,
    private confirmationService: ConfirmationService,
    private _generalDescriptionService: GeneralDescriptionService
  ) { 
    super(injector, messageService);
    this.breadcrumbService.setItems([
      { label: "Trang chủ", routerLink: ["/home"] },
      { label: "Mô tả chung" },
    ]);
  }

  rows: any[] = [];
  row: any;
  col: any;
  _selectedColumns: any[];

  // ACTION BUTTON
  listAction: any[] = [];

  ActiveDeactiveConst = ActiveDeactiveConst;

  cols: any[];
  page = new Page();
  offset = 0;

  ngOnInit(): void {
    this.setPage({ page: this.offset });
    //
    this.subject.keyword.pipe(debounceTime(SearchConst.DEBOUNCE_TIME)).subscribe(() => {
      if (this.keyword === "") {
        this.setPage({ page: this.offset });
      } else {
        this.setPage();
      }
    });
  
    this.cols = [
      { field: 'vaCode', header: 'Đại lý', width: '15rem', isPin: false, isResize: false, class: '' },
      { field: 'vaNumber', header: 'Ngày cài đặt', width: '25rem', isPin: false, isResize: true, class: '' },
      { field: 'tranSeq', header: 'Người cài đặt', width: '15rem', isPin: false, isResize: false, class: ''},
      { field: 'tranRemark', header: 'Ngày hủy', width: '20rem', isPin: false, isResize: false, class: ''},
  
    ];
    //
    this.cols = this.cols.map((item,index) => {
      item.position = index + 1;
      return item;
    });
    // this._selectedColumns = this.cols;
    this._selectedColumns = [...(this.getLocalStorage('generalDescriptionGan') ?? this.cols)];
    console.log('_selectedColumns', this._selectedColumns);
  }

  
  setColumn(col, _selectedColumns) {
    const ref = this.dialogService.open(
      FormSetDisplayColumnComponent,
      this.getConfigDialogServiceDisplayTableColumn(col, _selectedColumns)
    );
    //
    ref.onClose.subscribe((dataCallBack) => {
      if (dataCallBack?.accept) {
        this._selectedColumns = dataCallBack.data.sort(function (a, b) {
          return a.position - b.position;
        });
        this.setLocalStorage(this._selectedColumns, 'generalDescriptionGan');
      }
    });
  }
  
  showData(rows) {
    for (let row of rows) {
  
    }
    console.log('showData', rows);
  }

  create(){
    this.router.navigate(["/setting/general-description/detail"]);
  }
  
  setPage(pageInfo?: any) {
    this.page.pageNumber = pageInfo?.page ?? this.offset;
    if(pageInfo?.rows) this.page.pageSize = pageInfo?.rows;
    this.page.keyword = this.keyword;
    this.isLoading = true;
    //
    // this._generalDescriptionService.getAllGeneralDescription(this.page, this.status).subscribe((res) => {
    //   console.log('!!! res: ', res);
      
    //   this.isLoading = false;
    //   if(this.handleResponseInterceptor(res) && res?.data?.items?.length) {
    //     this.page.totalItems = res?.data?.totalItems;
    //     this.rows = res.data.items;
    //     if(this.rows?.length) { 
    //       // this.genListAction(this.rows);
    //       this.showData(this.rows);
    //     }
    //   }
    //   console.log({ resDistribution: res });
    //   }, (err) => {
    //     this.isLoading = false;
    //     console.log('Error-------', err);
    //   }
    // );
  }
  
  }
  