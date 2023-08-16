import { Component, Injector, OnInit, ViewChild } from "@angular/core";
import { AppComponentBase } from "@shared/app-component-base";
import { IssuerConst } from "@shared/AppConsts";
import { Page } from "@shared/model/page";
import { TradingProviderServiceProxy } from "@shared/service-proxies/setting-service";
import { BsModalRef, BsModalService } from "ngx-bootstrap/modal";
import { CreateOrUpdateTradingProviderComponent } from "./create-or-update-trading-provider/create-or-update-trading-provider.component";

@Component({
  selector: "app-trading-provider",
  templateUrl: "./trading-provider.component.html",
  styleUrls: ["./trading-provider.component.scss"],
})
export class TradingProviderComponent extends AppComponentBase {
  constructor(
    private injector: Injector,
    private _modalService: BsModalService,
    private _tradingProviderService: TradingProviderServiceProxy
  ) {
    super(injector);
  }

  titlePage: string = "Đại lý sơ cấp";
  rows: any[] = [];

  keyword: string = "";
  page = new Page();
  offset = 0;

  tradingProviderConst = IssuerConst;

  @ViewChild("tradingProviderTable") table: any;

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

    this._tradingProviderService.getAll(this.page).subscribe((res) => {
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

  edit(tradingProviderId: number) {
    this.showEditDialog(tradingProviderId);
  }

  private showCreateDialog(): void {
    let dialog: BsModalRef;
    dialog = this._modalService.show(CreateOrUpdateTradingProviderComponent, {
      class: "modal-lg",
      initialState: {
        title: "Thêm đại lý sơ cấp",
      },
    });
    dialog.content.onSave.subscribe(() => {
      this.setPage({ offset: this.offset });
    });
  }

  private showEditDialog(tradingProviderId: number): void {
    let dialog: BsModalRef;
    dialog = this._modalService.show(CreateOrUpdateTradingProviderComponent, {
      class: "modal-lg",
      initialState: {
        tradingProvider: {
          tradingProviderId: tradingProviderId,
        },
        title: "Cập nhật đại lý sơ cấp",
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
          this._tradingProviderService.delete(row.tradingProviderId).subscribe(
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
