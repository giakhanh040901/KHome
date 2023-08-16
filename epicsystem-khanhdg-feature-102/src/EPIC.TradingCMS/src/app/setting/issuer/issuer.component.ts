import { Component, Injector, OnInit, ViewChild } from '@angular/core';
import { AppComponentBase } from '@shared/app-component-base';
import { IssuerConst } from '@shared/AppConsts';
import { Page } from '@shared/model/page';
import { IssuerServiceProxy } from '@shared/service-proxies/setting-service';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { CreateOrUpdateIssuerComponent } from './create-or-update-issuer/create-or-update-issuer.component';

@Component({
  selector: 'app-issuer',
  templateUrl: './issuer.component.html',
  styleUrls: ['./issuer.component.scss']
})
export class IssuerComponent extends AppComponentBase {

  constructor(
    private injector: Injector,
    private _modalService: BsModalService,
    private _issuerService: IssuerServiceProxy,
  ) {
    super(injector);
  }

  titlePage: string = 'Tổ chức phát hành';
  rows: any[] = [];
  columns = "";

  keyword: string = '';

  page = new Page();
  offset = 0;
  IssuerConst = IssuerConst;

  @ViewChild('issuerTable') table: any;

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

    this._issuerService.getAll(this.page).subscribe((res) => {
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

  private showCreateDialog(): void {
    let dialog: BsModalRef;
    dialog = this._modalService.show(
      CreateOrUpdateIssuerComponent,
        {
            class: 'modal-lg',
            initialState: {
                title: 'Thêm tổ chức phát hành'
            },
        }
    );
    dialog.content.onSave.subscribe(() => {
        this.setPage({ offset: this.offset })
    });
  }

  edit(issuerId: number) {
    this.showEditDialog(issuerId);
  }

  private showEditDialog(issuerId: number): void {
    let dialog: BsModalRef;
    dialog = this._modalService.show(
      CreateOrUpdateIssuerComponent,
        {
            class: 'modal-lg',
            initialState: {
                issuer: {
                  issuerId: issuerId,
                },
                title: 'Cập nhật tổ chức phát hành',
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
          this._issuerService.delete(row.issuerId).subscribe((response) => {
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

