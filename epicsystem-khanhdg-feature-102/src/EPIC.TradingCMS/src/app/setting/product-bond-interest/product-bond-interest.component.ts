import { Component, Injector, OnInit, ViewChild } from '@angular/core';
import { AppComponentBase } from '@shared/app-component-base';
import { ProductBondInterestConst } from '@shared/AppConsts';
import { Page } from '@shared/model/page';
import { ProductBondInterestServiceProxy } from '@shared/service-proxies/setting-service';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { CreateOrUpdateProductBondInterestComponent } from './create-or-update-product-bond-interest/create-or-update-product-bond-interest.component';

@Component({
  selector: 'app-product-bond-interest',
  templateUrl: './product-bond-interest.component.html',
  styleUrls: ['./product-bond-interest.component.scss']
})
export class ProductBondInterestComponent extends AppComponentBase {

  constructor(
    private injector: Injector,
    private _modalService: BsModalService,
    private _productBondDetailService: ProductBondInterestServiceProxy,
  ) {
    super(injector);
  }

  productBondDetails:any = [];

  titlePage: string = 'Thông tin chi tiết lãi của trái phiếu';
  rows: any[] = [];
  columns = "";
  productBondInterest: any = [];

  keyword: string = '';
  page = new Page();
  offset = 0;

  ProductBondInterestConst = ProductBondInterestConst;

  @ViewChild('productBondInterestTable') table: any;

  ngOnInit(): void {
    this.setPage({ offset: this.offset });
  }

  changeKeyword() {
    // Xóa hết keyword ô Search sẽ tự reset danh sách item không cần Enter
    if (this.keyword == '') {
      this.setPage({ offset: this.offset })
    }
  }

  setPage(pageInfo) {
    this.page.pageNumber = pageInfo.offset ?? this.offset;
    this.page.keyword = this.keyword;
    this.isLoading = true;

    this._productBondDetailService.getAll(this.page).subscribe((res) => {
      if (this.handleResponseInterceptor(res, '')) {
        this.page.totalItems = res.data.totalItems;
        this.rows = res.data.items;
        this.fixZIndexOption();
        console.log({ rows: res.data.items });
      }
      this.isLoading = false;
    }, () => {
      this.notify.error('Không lấy được dữ liệu');
      this.isLoading = false;
    });
  }

  create() {
    this.showCreateDialog();
  }

  private showCreateDialog(): void {
    let dialog: BsModalRef;
    dialog = this._modalService.show(
      CreateOrUpdateProductBondInterestComponent,
      {
        class: 'modal-sm',
        initialState: {
          title: 'Thêm thông tin chi tiết lãi của trái phiếu'
        },
      }
    );
    dialog.content.onSave.subscribe(() => {
      this.setPage({ offset: this.offset })
    });
  }

  edit(bondInterestId: number) {
    this.showEditDialog(bondInterestId);
  }

  private showEditDialog(bondInterestId: number): void {
    let dialog: BsModalRef;
    dialog = this._modalService.show(
      CreateOrUpdateProductBondInterestComponent,
      {
        class: 'modal-sm',
        initialState: {
          productBondInterest: {
            bondInterestId: bondInterestId,
          },
          title: 'Cập nhật thông tin chi tiết lãi của trái phiếu  ',
        },
      }
    );
    dialog.content.onSave.subscribe(() => {
      this.setPage({ offset: this.page.pageNumber })
    });
  }

  protected delete(row): void {
    abp.message.confirm(
      `Chắc chắn xóa "${row.bondDetailId}" ?`,
      undefined,
      (result: boolean) => {
        if (result) {
          this._productBondDetailService.delete(row.bondInterestId).subscribe((response) => {
            if (this.handleResponseInterceptor(response, 'Xóa thành công')) {
              this.setPage({ offset: this.page.pageNumber });
            }
          }, () => {
            this.notify.error('Không xóa được' + row.name);
          });
        }
      }
    );
  }

  onDetailToggle(event) {
    console.log('Detail Toggled', event);
  }
  toggleExpandRow(row, expanded) {
    console.log({ expanded: expanded });
    if (!expanded) this.table.rowDetail.collapseAllRows(row);
    this.table.rowDetail.toggleExpandRow(row);
  }
}
