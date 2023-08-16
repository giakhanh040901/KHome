import { Component, Inject, Injector, OnInit, ViewChild } from '@angular/core';
import { AppComponentBase } from '@shared/app-component-base';
import { AppConsts, ContractTemplateConst } from '@shared/AppConsts';
import { Page } from '@shared/model/page';
import { ContractTemplateServiceProxy, ContractTypeServiceProxy } from '@shared/service-proxies/bond-manager-service';
import { API_BASE_URL } from '@shared/service-proxies/service-proxies-base';
import { AppUtilsService } from '@shared/services/utils.service';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { CreateOrUpdateContractTemplateComponent } from './create-or-update-contract-template/create-or-update-contract-template.component';

@Component({
  selector: 'app-contract-template',
  templateUrl: './contract-template.component.html',
  styleUrls: ['./contract-template.component.scss']
})
export class ContractTemplateComponent extends AppComponentBase {

  constructor(
    private injector: Injector,
    private _modalService: BsModalService,
    private _contractTemplateService : ContractTemplateServiceProxy,
    private _contractTypeService : ContractTypeServiceProxy,
    private _utilsService : AppUtilsService,
    @Inject(API_BASE_URL) baseUrl?: string,
  ) {
    super(injector);
    this.baseUrl = baseUrl !== undefined && baseUrl !== null ? baseUrl : "";
    this.baseUrl = AppConsts.remoteServiceBaseUrlLocal ?? this.baseUrl;
  }
  private baseUrl: string;

  titlePage: string = 'Danh mục mẫu hợp đồng';
  rows: any[] = [];
  contractTypes: any[] = [];
  isLoading = false;

  keyword: string = '';
  contractTypeId: number;

  page = new Page();
  offset = 0;

  contractTempTypeConst = ContractTemplateConst.contractTempType;
  ContractTemplateConst = ContractTemplateConst;
  @ViewChild('contractTemplateTable') table: any;

  ngOnInit(): void {
    // this.setPage({ offset: this.offset });
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

    this._contractTypeService.getAll(this.page).subscribe((resContractType) => {
      this.contractTypes = resContractType.data?.items;
      console.log({ 'resContractType': resContractType });
    });

    this._contractTemplateService.getAll(this.page, this.contractTypeId).subscribe((res) => {
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

  changeContractType() {
    this.setPage({ offset: this.offset });
  }

  dowloadFile(path) {
    this.baseUrl = AppConsts.remoteServiceBaseUrlLocal ?? this.baseUrl;
    const url = this.baseUrl + '/' + path;
    this._utilsService.makeDownload('', url);
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
      CreateOrUpdateContractTemplateComponent,
        {
            class: 'modal-sm',
            initialState: {
                title: 'Thêm mẫu hợp đồng'
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
      CreateOrUpdateContractTemplateComponent,
        {
            class: 'modal-sm',
            initialState: {
                contractTemplate: {
                  id: id,
                },
                title: 'Cập nhật mẫu hợp đồng',
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
          this._contractTemplateService.delete(item.id).subscribe((response) => {
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
