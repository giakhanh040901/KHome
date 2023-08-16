import { Component, Injector, OnInit, ViewChild } from '@angular/core';
import { AppComponentBase } from '@shared/app-component-base';
import { IssuerConst } from '@shared/AppConsts';
import { Page } from '@shared/model/page';
import { ProductBondTypeServiceProxy } from '@shared/service-proxies/setting-service';
import { id } from '@swimlane/ngx-datatable';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { CreateOrUpdateProductBondTypeComponent } from './create-or-update-product-bond-type/create-or-update-product-bond-type.component';

@Component({
  selector: 'app-product-bond-type',
  templateUrl: './product-bond-type.component.html',
  styleUrls: ['./product-bond-type.component.scss']
})
export class ProductBondTypeComponent extends AppComponentBase {

  constructor(
    private injector: Injector,
    private _modalService: BsModalService,
    private _productBondTypeService:  ProductBondTypeServiceProxy,
  ) {
    super(injector);
  }

  titlePage: string = 'Loại sản phẩm trái phiếu';
  rows: any[] = [];
  isLoading = false;

  keyword: string = '';

  page = new Page();
  offset = 0;
  statusConst= IssuerConst.status;

  productBondTypeConst = IssuerConst;

  @ViewChild('ProductBondTypeTable') table: any;

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

    this._productBondTypeService.getAll(this.page).subscribe((res) => {
      if(this.handleResponseInterceptor(res,'')){
        this.page.totalItems = res.data.totalItems;
        this.rows = res.data.items;
        this.fixZIndexOption();
        console.log({ rows: res.data.items, totalItems: res.data.totalItems });
      }
      this.isLoading = false;
    }, () => {
      this.isLoading = false;
    });
    // fix show dropdown options bị ẩn dưới button click
    // this.fixZIndexOption();
    console.log({ rows: this.rows });
  }

  create() {
    this.showCreateDialog();
  }

  edit(id: number) {
    
    
    this.showEditDialog(id);
  }

  private showCreateDialog(): void {
    let dialog: BsModalRef;
    dialog = this._modalService.show(
      CreateOrUpdateProductBondTypeComponent,
        {
            class: 'modal-sm',
            initialState: {
                title: 'Thêm mới loại sản phẩm trái phiếu'
            },
        }
    );

    dialog.content.onSave.subscribe(() => {
        this.setPage({ offset: this.offset })
    });
  }

  private showEditDialog(id: number): void {
    let dialog: BsModalRef;
    dialog = this._modalService.show(
      CreateOrUpdateProductBondTypeComponent,
        {
            class: 'modal-sm',
            initialState: {
                productBondType: {
                  id: id,
                },
                title: 'Cập nhật loại sản phẩm trái phiếu',
            },
        }
    );

    dialog.content.onSave.subscribe(() => {
        this.setPage({ offset: this.page.pageNumber })
    });
  }
  
  protected delete(item): void {
    console.log('item',item );

    abp.message.confirm(
      `Chắc chắn xoá "${item.code}" ?`,
      undefined,
      (result: boolean) => {
        if (result) {
          this._productBondTypeService.delete(item.id).subscribe((response) => {
            if(this.handleResponseInterceptor(response, 'Xóa thành công')) {
              this.setPage({ offset: this.page.pageNumber });
            }
          }, () => {
              this.notify.error('Không xóa được' + item.name);
          });
        }
      }
    );
  }

}
