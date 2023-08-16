import { Component, Injector, OnInit, ViewChild } from '@angular/core';
import { AppComponentBase } from '@shared/app-component-base';
import { ProductBondDetailConst, ProductPolicyConst } from '@shared/AppConsts';
import { Page } from '@shared/model/page';
import { ProductBondDetailServiceProxy } from '@shared/service-proxies/bond-manager-service';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { CreateOrUpdateProductBondDetailComponent } from './create-or-update-product-bond-detail/create-or-update-product-bond-detail.component';

@Component({
  selector: 'app-product-bond-detail',
  templateUrl: './product-bond-detail.component.html',
  styleUrls: ['./product-bond-detail.component.scss']
})
export class ProductBondDetailComponent extends AppComponentBase {

  constructor(
    private injector: Injector,
    private _modalService: BsModalService,
    private _productBondDetailService: ProductBondDetailServiceProxy,
  ) {
    super(injector);
  }

  titlePage: string = 'Quản lý phát hành trái phiếu';
  rows: any[] = [];
  productBondInfos: any = [];

  keyword: string = '';
  market: string;
  page = new Page();
  offset = 0;

  ProductBondDetailConst = ProductBondDetailConst;
  ProductPolicyConst = ProductPolicyConst;

  @ViewChild('productBondDetailTable') table: any;

  ngOnInit(): void {
    this.setPage({ offset: this.offset });
  }

  changeKeyword() {
    // Xóa hết keyword ô Search sẽ tự reset danh sách item không cần Enter
    if (this.keyword == '') {
      this.setPage({ offset: this.offset })
    }
  }

  async setPage(pageInfo) {
    this.page.pageNumber = pageInfo.offset ?? 0;
    this.page.keyword = this.keyword;
    this.isLoading = true;

    await this._productBondDetailService.getAllBondInfo(this.page).subscribe((res) => {
      this.productBondInfos = res.data.items;
      console.log({ productBondInfo: res.data.items });
    });

    await this._productBondDetailService.getAll(this.page, this.market).subscribe((res) => {
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

  changeMarket() {
    this.setPage({ offset: this.offset });
  }

  getProductBondDetail(productBondId) {
    const detail = this.productBondInfos.find(item => item.productBondId == productBondId);
    if(detail) {
      return detail.bondName;
    }
    return '-';
  }


  create(market) {
    this.showCreateDialog(market);
  }

  edit(bondDetailId: number) {
    this.showEditDialog(bondDetailId);
  }

  private showCreateDialog(market): void {
    let dialog: BsModalRef;
    dialog = this._modalService.show(
      CreateOrUpdateProductBondDetailComponent,
        {
            class: 'modal-lg',
            initialState: {
              productBondDetail: {
                market: market,
              },
              title: 'Thêm phát hành ' + ProductPolicyConst.getNameMarket(market),

            },
        }
    );
    dialog.content.onSave.subscribe(() => {
        this.setPage({ offset: this.offset })
    });
  }

  private showEditDialog(row): void {
    let dialog: BsModalRef;
    dialog = this._modalService.show(
      CreateOrUpdateProductBondDetailComponent,
        {
            class: 'modal-lg',
            initialState: {
                productBondDetail: {
                  bondDetailId: row.bondDetailId,
                },
                title: 'Cập nhật phát hành ' +  ProductPolicyConst.getNameMarket(row.market),
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
          this._productBondDetailService.delete(row.bondDetailId).subscribe((response) => {
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

