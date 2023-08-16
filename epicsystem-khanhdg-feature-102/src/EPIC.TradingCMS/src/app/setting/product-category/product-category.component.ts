import { Component, Injector, OnInit, ViewChild } from '@angular/core';
import { AppComponentBase } from '@shared/app-component-base';
import { Page } from '@shared/model/page';
import { ProductCategoryServiceProxy } from '@shared/service-proxies/setting-service';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { CreateOrUpdateProductCategoryComponent } from './create-or-update-product-category/create-or-update-product-category.component';

@Component({
  selector: 'app-product-category',
  templateUrl: './product-category.component.html',
  styleUrls: ['./product-category.component.scss']
})
export class ProductCategoryComponent extends AppComponentBase {

  constructor(
    private injector: Injector,
    private _modalService: BsModalService,
    private _productCategoryService: ProductCategoryServiceProxy,
  ) {
    super(injector);
  }

  titlePage: string = 'Danh mục trái phiếu';
  rows: any[] = [];

  keyword: string = '';
  page = new Page();
  offset = 0;

  @ViewChild('productCategoryTable') table: any;

  ngOnInit(): void {
    this.setPage({ offset: this.offset });
  }

  changeKeyword() {
    // Xóa hết keyword ô Search sẽ tự reset danh sách item không cần Enter
    if (this.keyword == "") {
      this.setPage({ offset: this.offset });
    }
  }

  setPage(pageInfo) {
    this.page.pageNumber = pageInfo.offset ?? this.offset;
    this.page.keyword = this.keyword;
    this.isLoading = true;

    this._productCategoryService.getAll(this.page).subscribe((res) => {
        if(this.handleResponseInterceptor(res, '')) {
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

  edit(productCategoryId: number) {
    console.log({'productCategory': productCategoryId})
    this.showEditDialog(productCategoryId);
  }

  private showCreateDialog(): void {
    let dialog: BsModalRef;
    dialog = this._modalService.show(CreateOrUpdateProductCategoryComponent, {
      class: "modal-sm",
      initialState: {
        title: "Thêm danh mục trái phiếu",
      },
    });
    dialog.content.onSave.subscribe(() => {
      this.setPage({ offset: this.offset });
    });
  }

  private showEditDialog(productCategoryId: number): void {
    let dialog: BsModalRef;
    dialog = this._modalService.show(CreateOrUpdateProductCategoryComponent, {
      class: "modal-sm",
      initialState: {
        productCategory: {
          productCategoryId: productCategoryId,
        },
        title: "Cập nhật danh mục trái phiếu",
      },
    });
    dialog.content.onSave.subscribe(() => {
      this.setPage({ offset: this.page.pageNumber });
    });
  }

  protected delete(row): void {
    abp.message.confirm(
      `Chắc chắn xoá "${row.name}" ?`,
      undefined,
      (result: boolean) => {
        if (result) {
          this._productCategoryService.delete(row.productCategoryId).subscribe(
            (response) => {
              if (this.handleResponseInterceptor(response, "Xoá thành công")) {
                this.setPage({ offset: this.page.pageNumber });
              }
            },
            () => {
              this.notify.error("Không xoá được" + row.name);
            }
          );
        }
      }
    );
  }

  onDetailToggle(event) {
    console.log('Detail Toggled', event);
  }

  toggleExpandRow(row, expanded) {
    console.log({ expanded: expanded});
    if(!expanded) this.table.rowDetail.collapseAllRows(row);
    this.table.rowDetail.toggleExpandRow(row);
  }
}
