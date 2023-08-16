import { Component, Injector, OnInit, ViewChild } from '@angular/core';
import { AppComponentBase } from '@shared/app-component-base';
import { Page } from '@shared/model/page';
import { ContractTypeServiceProxy } from '@shared/service-proxies/bond-manager-service';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { CreateOrUpdateContractTypeComponent } from './create-or-update-contract-type/create-or-update-contract-type.component';

@Component({
  selector: 'app-contract-type',
  templateUrl: './contract-type.component.html',
  styleUrls: ['./contract-type.component.scss']
})
export class ContractTypeComponent extends AppComponentBase {

  constructor(
    private injector: Injector,
    private _modalService: BsModalService,
    private _contractTypeService: ContractTypeServiceProxy,
  ) {
    super(injector);
  }

  titlePage: string = 'Danh mục loại hợp đồng';
  rows: any[] = [];
  columns = "";
  contractType: any = [];

  keyword: string = '';
  page = new Page();
  offset = 0;

  @ViewChild('contractTypeTable') table: any;

  ngOnInit(): void {
    this.setPage({ offset: this.offset });
  }

  setPage(pageInfo) {
    this.page.pageNumber = pageInfo.offset ?? this.offset;
    this.page.keyword = this.keyword;
    this.isLoading = true;

    this._contractTypeService.getAll(this.page).subscribe((res) => {
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

  changeKeyword() {
    // Xóa hết keyword ô Search sẽ tự reset danh sách item không cần Enter
    if (this.keyword == '') {
      this.setPage({ offset: this.offset })
    }
  }

  create() {
    this.showCreateDialog();
  }

  private showCreateDialog(): void {
    let dialog: BsModalRef;
    dialog = this._modalService.show(
      CreateOrUpdateContractTypeComponent,
      {
        class: 'modal-sm',
        initialState: {
          title: 'Thêm loại hợp đồng'
        },
      }
    );
    dialog.content.onSave.subscribe(() => {
      this.setPage({ offset: this.offset })
    });
  }

  edit(id: number) {
    this.showEditDialog(id);
  }

  private showEditDialog(id: number): void {
    console.log(id)
    let dialog: BsModalRef;
    dialog = this._modalService.show(
      CreateOrUpdateContractTypeComponent,
      {
        class: 'modal-sm',
        initialState: {
          contractType: {
            id: id,
          },
          title: 'Cập nhật loại hợp đồng',
        },
      }
    );
    dialog.content.onSave.subscribe(() => {
      this.setPage({ offset: this.page.pageNumber })
    });
  }

  protected delete(row): void {
    abp.message.confirm(
      `Chắc chắn xóa danh mục loại hợp đồng "${row.name}" ?`,
      undefined,
      (result: boolean) => {
        if (result) {
          this._contractTypeService.delete(row.id).subscribe((response) => {
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
