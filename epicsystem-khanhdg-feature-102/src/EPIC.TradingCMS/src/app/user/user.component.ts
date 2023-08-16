import { Component, Injector, OnInit, ViewChild } from '@angular/core';
import { AppComponentBase } from '@shared/app-component-base';
import { IssuerConst } from '@shared/AppConsts';
import { Page } from '@shared/model/page';
import { UserServiceProxy } from '@shared/service-proxies/service-proxies';
import { IssuerServiceProxy } from '@shared/service-proxies/setting-service';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { CreateOrUpdateUserComponent } from './create-or-update-user/create-or-update-user.component';

@Component({
  selector: 'app-user',
  templateUrl: './user.component.html',
  styleUrls: ['./user.component.scss']
})
export class UserComponent extends AppComponentBase {

  constructor(
    private injector: Injector,
    private _modalService: BsModalService,
    private _issuerService: IssuerServiceProxy,
    private _userService: UserServiceProxy,
  ) {
    super(injector);
  }

  titlePage: string = 'Quản lý tài khoản';
  rows: any[] = [];
  columns = "";

  keyword: string = '';

  page = new Page();
  offset = 0;
  IssuerConst = IssuerConst;

  @ViewChild('userTable') table: any;

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

    this._userService.getAll(this.page).subscribe((res) => {
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
      CreateOrUpdateUserComponent,
        {
            class: 'modal-sm',
            initialState: {
                title: 'Thêm tài khoản'
            },
        }
    );
    dialog.content.onSave.subscribe(() => {
        this.setPage({ offset: this.offset })
    });
  }

  edit(userId: number) {
    this.showEditDialog(userId);
  }

  private showEditDialog(userId: number): void {
    let dialog: BsModalRef;
    dialog = this._modalService.show(
      CreateOrUpdateUserComponent,
        {
            class: 'modal-sm',
            initialState: {
                user: {
                  userId: userId,
                },
                title: 'Cập nhật tài khoản',
            },
        }
    );
    dialog.content.onSave.subscribe(() => {
        this.setPage({ offset: this.page.pageNumber })
    });
  }

  protected delete(row): void {
    abp.message.confirm(
      `Chắc chắn xóa tài khoản "${row.displayName}" ?`,
      undefined,
      (result: boolean) => {
        if (result) {
          this._userService.delete(row.userId).subscribe((response) => {
            if(this.handleResponseInterceptor(response, 'Xóa thành công')) {
              this.setPage({ offset: this.page.pageNumber });
            }
          }, () => {
              this.notify.error('Không xóa được tài khoản' + row.displayName);
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

