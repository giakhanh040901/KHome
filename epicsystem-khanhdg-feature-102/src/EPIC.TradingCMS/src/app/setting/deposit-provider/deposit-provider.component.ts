import { Component, Injector, OnInit, ViewChild } from '@angular/core';
import { AppComponentBase } from '@shared/app-component-base';
import { IssuerConst } from '@shared/AppConsts';
import { Page } from '@shared/model/page';
import { DepositProviderServiceProxy } from '@shared/service-proxies/setting-service';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { CreateOrUpdateDepositProviderComponent } from './create-or-update-deposit-provider/create-or-update-deposit-provider.component';


@Component({
  selector: 'app-deposit-provider',
  templateUrl: './deposit-provider.component.html',
  styleUrls: ['./deposit-provider.component.scss']
})
export class DepositProviderComponent extends AppComponentBase {

  constructor(
    private injector: Injector,
    private _modalService: BsModalService,
    private _depositProvider: DepositProviderServiceProxy,
  ) {
    super(injector);
  }

  titlePage: string = 'Đại lý lưu ký';
  rows: any[] = [];
  isLoading = false;

  keyword: string = '';

  page = new Page();
  offset = 0;

  depositProviderConst = IssuerConst;

  @ViewChild('depositProviderTable') table: any;

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

    this._depositProvider.getAll(this.page).subscribe((res) => {
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

  edit(depositProviderId: number) {
    this.showEditDialog(depositProviderId);
  }

  
  onDetailToggle(event) {
    console.log('Detail Toggled', event);
  }
  toggleExpandRow(row, expanded) {
    console.log({ expanded: expanded});
    if(!expanded) this.table.rowDetail.collapseAllRows(row);
    this.table.rowDetail.toggleExpandRow(row);
  }
  
  private showCreateDialog(): void {
    let dialog: BsModalRef;
    dialog = this._modalService.show(
      CreateOrUpdateDepositProviderComponent,
        {
            class: 'modal-lg',
            initialState: {
                title: 'Tạo mới đại lý lưu ký'
            },
        }
    );

    dialog.content.onSave.subscribe(() => {
        this.setPage({ offset: this.offset })
    });
  }

  private showEditDialog(depositProviderId: number): void {
    let dialog: BsModalRef;
    dialog = this._modalService.show(
      CreateOrUpdateDepositProviderComponent,
        {
            class: 'modal-lg',
            initialState: {
                depositProvider: {
                  depositProviderId: depositProviderId,
                },
                title: 'Cập nhật đại lý lưu ký',
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
          this._depositProvider.delete(item.depositProviderId).subscribe((response) => {
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
