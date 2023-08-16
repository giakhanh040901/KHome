import { YesNoConst } from '@shared/AppConsts';
import { ProductPolicyServiceProxy } from './../../../shared/service-proxies/setting-service';
import { ProductPolicyConst } from './../../../shared/AppConsts';
import { Component, Injector, OnInit, ViewChild } from '@angular/core';
import { AppComponentBase } from '@shared/app-component-base';
import { Page } from '@shared/model/page';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { CreateProductPolicyComponent } from './create-product-policy/create-product-policy.component';

@Component({
  selector: 'app-product-policy',
  templateUrl: './product-policy.component.html',
  styleUrls: ['./product-policy.component.scss']
})
export class ProductPolicyComponent extends AppComponentBase {

  constructor(
    private injector: Injector,
    private _modalService: BsModalService,
    private _productPolicyService: ProductPolicyServiceProxy,
  ) {
    super(injector);
  }

  titlePage: string = 'Quản lý chính sách';
  rows: any[] = [];

  keyword: string = '';
  page = new Page();
  offset = 0;

  market: string;

  ProductPolicyConst = ProductPolicyConst;
  YesNoConst = YesNoConst;

  @ViewChild('productPolicyTable') table: any;

  ngOnInit(): void {
    // this.setPage({ offset: this.offset });
  }

  changeKeyword() {
    // Xóa hết keyword ô Search sẽ tự reset danh sách item không cần Enter
    if (this.keyword == '') {
      this.setPage({ offset: this.offset });
    }
  }

  changeMarket() {
    this.setPage({ offset: this.offset })
  }

  setPage(pageInfo) {
    this.page.pageNumber = pageInfo.offset ?? 0;
    this.page.keyword = this.keyword;
    this.isLoading = true;

    this._productPolicyService.getAll(this.page, this.market).subscribe((res) => {
      this.page.totalItems = res.data.totalItems;
      this.rows = res.data.items;
      console.log({ rows: this.rows });
      this.isLoading = false;
    }, () => {
      this.isLoading = false;
      this.notify.error('Không lấy được dữ liệu!');
    });
  }

  create() {
    this.showCreateDialog();
  }

  edit(bondPolicyId: number) {
    this.showEditDialog(bondPolicyId);
  }

  private showCreateDialog(): void {
    let dialog: BsModalRef;
    dialog = this._modalService.show(
      CreateProductPolicyComponent,
        {
            class: 'modal-md',
            initialState: {
                title: 'Thêm chính sách'
            },
        }
    );
    dialog.content.onSave.subscribe(() => {
        this.setPage({ offset: this.offset })
    });
  }

  private showEditDialog(bondPolicyId: number): void {
    let dialog: BsModalRef;
    dialog = this._modalService.show(
      CreateProductPolicyComponent,
        {
            class: 'modal-md',
            initialState: {
                productPolicy: {
                  bondPolicyId: bondPolicyId,
                },
                title: 'Cập nhật chính sách',
            },
        }
    );
    dialog.content.onSave.subscribe(() => {
        this.setPage({ offset: this.page.pageNumber })
    });
  }

  protected delete(row): void {
    abp.message.confirm(
      `Chắc chắn xóa "${row.name}" ?`,
      undefined,
      (result: boolean) => {
        if (result) {
          this._productPolicyService.delete(row.bondPolicyId).subscribe((response) => {
            if(this.handleResponseInterceptor(response, 'Xóa thành công')) {
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
    console.log({ expanded: expanded});
    if(!expanded) this.table.rowDetail.collapseAllRows(row);
    this.table.rowDetail.toggleExpandRow(row);
  }

}
