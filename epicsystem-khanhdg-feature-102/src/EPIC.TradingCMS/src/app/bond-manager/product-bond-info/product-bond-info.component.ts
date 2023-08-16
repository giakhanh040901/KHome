import { Component, Injector, OnInit, ViewChild } from '@angular/core';
import { Page } from '@shared/model/page';
import { ProductBondInfoServiceProxy } from '@shared/service-proxies/bond-manager-service';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { CreateOrUpdateProductBondInfoComponent } from './create-or-update-product-bond-info/create-or-update-product-bond-info.component';
import { AppComponentBase } from '@shared/app-component-base';
import { IssuerConst, ProductBondInfoConst } from '@shared/AppConsts';
import { forkJoin } from 'rxjs';

@Component({
  selector: 'app-product-bond-info',
  templateUrl: './product-bond-info.component.html',
  styleUrls: ['./product-bond-info.component.scss']
})
export class ProductBondInfoComponent extends AppComponentBase {

  constructor(
    private injector: Injector,
    private _modalService: BsModalService,
    private _productBondInfoService: ProductBondInfoServiceProxy,
  ) {
    super(injector);
  }

  issuers:any = [];
  tradingProviders:any = [];
  depositProviders:any = [];
  bondTypes:any = [];

   titlePage: string = 'Lô trái phiếu';
   rows: any[] = [];

   keyword: string = '';

   page = new Page();
   offset = 0;

   ProductBondInfoConst = ProductBondInfoConst;
   IssuerConst = IssuerConst;

   @ViewChild('productBondInfoTable') table: any;
   // Xóa hết keyword ô Search sẽ tự reset danh sách item không cần Enter
  ngOnInit(): void {
    forkJoin([this._productBondInfoService.getAllIssuer(this.page),
      this._productBondInfoService.getAllTradingProvider(this.page),
      this._productBondInfoService.getAllDepositProvider(this.page),
      this._productBondInfoService.getAllBondType()]).subscribe(
        ([resIssuer, resTradingProvider, resDepositProvider, resBondType]) => {
          this.issuers = resIssuer.data?.items;
          this.tradingProviders = resTradingProvider.data?.items;
          this.depositProviders = resDepositProvider.data?.items;
          this.bondTypes = resBondType.data?.items;
          console.log({'resIssuer': resIssuer, 'resTradingProvider': resTradingProvider, 'resDepositProvider': resDepositProvider, 'resBondType': resBondType});
        });

    this.setPage({ offset : this.offset});
  }

  getNameIssuer(issuerId) {
    if(this.issuers) {
      const issuer = this.issuers.find(item => item.issuerId == issuerId);
      if(issuer) {
        return issuer.name;
      }
    }
    return '';
  }
  getNameTrandingProvider(tradingProviderId) {
    if(this.tradingProviders) {
      const tradingProvider = this.tradingProviders.find(item => item.tradingProviderId == tradingProviderId);
      if(tradingProvider) {
        return tradingProvider.name;
      }
    }
    return '';
  }
  getNameDepositProvider(depositProviderId) {
    if(this.depositProviders) {
      const depositProvider = this.depositProviders.find(item => item.depositProviderId == depositProviderId);
      if(depositProvider) {
        return depositProvider.name;
      }
    }
    return '';
  }
  getNameBondType(bondTypeId) {
    if(this.bondTypes) {
      const bondType = this.bondTypes.find(item => item.id == bondTypeId);
      if(bondType) {
        return bondType.name;
      }
    }
    return '';
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

    this._productBondInfoService.getAll(this.page).subscribe((res) => {
        if(this.handleResponseInterceptor(res, '')) {
          this.page.totalItems = res.data.totalItems;
          this.rows = res.data?.items;
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

  edit(productBondId: number) {
    this.showEditDialog(productBondId);
  }

  private showCreateDialog(): void {
    let dialog: BsModalRef;
    dialog = this._modalService.show(
      CreateOrUpdateProductBondInfoComponent,
        {
            class: 'modal-lg',
            initialState: {
                title: 'Tạo mới lô trái phiếu'
            },
        }
    );

    dialog.content.onSave.subscribe(() => {
        this.setPage({ offset: this.offset })
    });
  }

  private showEditDialog(productBondId: number): void {
    let dialog: BsModalRef;
    dialog = this._modalService.show(
      CreateOrUpdateProductBondInfoComponent,
        {
            class: 'modal-lg',
            initialState: {
                productBondInfo: {
                  productBondId: productBondId,
                },
                title: 'Cập nhật lô trái phiếu',
            },
        }
    );
    dialog.content.onSave.subscribe(() => {
        this.setPage({ offset: this.page.pageNumber })
    });
  }

  protected delete(row): void {
    console.log('row', row);
    abp.message.confirm(
      `Chắc chắn xoá lô trái phiếu "${row.bondName}" ?`,
      undefined,
      (result: boolean) => {
        if (result) {
          this._productBondInfoService.delete(row.productBondId).subscribe((response) => {
            if(this.handleResponseInterceptor(response, 'Xóa thành công')) {
              this.setPage({ offset: this.page.pageNumber });
            }
          }, () => {
            this.notify.error("Không thể xoá" + row.bondName);
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


